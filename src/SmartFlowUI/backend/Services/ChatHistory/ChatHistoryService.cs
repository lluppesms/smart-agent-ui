// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Client;
using Shared.Models;

namespace MinimalApi.Services.ChatHistory;


public class ChatHistoryService : IChatHistoryService
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _cosmosContainer;

    public ChatHistoryService(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;

        // Create database if it doesn't exist
        try
        {
            var db = _cosmosClient.CreateDatabaseIfNotExistsAsync(DefaultSettings.CosmosDbDatabaseName).GetAwaiter().GetResult();
            // Create get container if it doesn't exist
            _cosmosContainer = db.Database.CreateContainerIfNotExistsAsync(DefaultSettings.CosmosDbCollectionName, "/userId").GetAwaiter().GetResult();
        }
        catch (CosmosException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Forbidden && ex.Message.Contains("firewall settings", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"==> Connection to Cosmos {_cosmosClient.Endpoint.Host} failed because of a misconfigured firewall setting!");
                // Message might contain this...: "code":"Forbidden","message":"Request originated from IP x.x.188.38 through public internet. This is blocked by your Cosmos DB account firewall settings. More info: https:...
                var startLoc = ex.Message.IndexOf("\"code\":\"Forbidden\",\"message\":\"", StringComparison.InvariantCultureIgnoreCase);
                var endLoc = ex.Message.IndexOf("More info:", startLoc + 30, StringComparison.InvariantCultureIgnoreCase);
                if (startLoc > 0 && endLoc > 0)
                {
                    var ipMessage = ex.Message.Substring(startLoc + 30, endLoc - startLoc - 30);
                    Console.WriteLine($"==> {ipMessage}");
                }
                Console.ResetColor();
            }
            //throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"*** Connection to Cosmos failed: {ex.Message}");
            throw;
        }
    }

    public async Task RecordChatMessageAsync(UserInformation user, ChatRequest chatRequest, ApproachResponse response)
    {
        var lastHistoryItem = chatRequest.History?.LastOrDefault();
        var prompt = lastHistoryItem?.User;
        if (prompt == null)
            throw new InvalidOperationException("The prompt cannot be null.");

        var chatMessage = new ChatMessageRecord(user.UserId, chatRequest.ChatId.ToString(), chatRequest.ChatTurnId.ToString(), prompt, response.Answer, response.Context);
        await _cosmosContainer.CreateItemAsync(chatMessage, partitionKey: new PartitionKey(chatMessage.ChatId));
    }

    public async Task RecordRatingAsync(UserInformation user, ChatRatingRequest chatRatingRequest)
    {
        var chatRatingId = chatRatingRequest.MessageId.ToString();
        var partitionKey = new PartitionKey(chatRatingRequest.ChatId.ToString());
        var response = await _cosmosContainer.ReadItemAsync<ChatMessageRecord>(chatRatingId, partitionKey);
        var existingChatRating = response.Resource;

        var rating = new ChatRating(chatRatingRequest.Feedback, chatRatingRequest.Rating);
        existingChatRating.Rating = rating;
        await _cosmosContainer.UpsertItemAsync(existingChatRating, partitionKey: partitionKey);
    }


    public async Task<List<ChatMessageRecord>> GetMostRecentRatingsItemsAsync(UserInformation user)
    {

        var query = _cosmosContainer.GetItemQueryIterator<ChatMessageRecord>(
            new QueryDefinition("SELECT TOP 100 * FROM c WHERE c.rating != null AND c.userId = @username ORDER BY c.rating.timestamp DESC")
            .WithParameter("@username", user.UserId));

        var results = new List<ChatMessageRecord>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<List<ChatMessageRecord>> GetMostRecentChatItemsAsync(UserInformation user)
    {
        var query = _cosmosContainer.GetItemQueryIterator<ChatMessageRecord>(
            new QueryDefinition("SELECT TOP 100 * FROM c WHERE c.userId = @username ORDER BY c.timestamp DESC")
            .WithParameter("@username", user.UserId));

        var results = new List<ChatMessageRecord>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<List<ChatMessageRecord>> GetChatHistoryMessagesAsync(UserInformation user, string chatId)
    {
        var query = _cosmosContainer.GetItemQueryIterator<ChatMessageRecord>(
            new QueryDefinition("SELECT * FROM c WHERE c.userId = @username AND c.chatId = @chatid ORDER BY c.timestamp DESC")
            .WithParameter("@username", user.UserId)
            .WithParameter("@chatid", chatId));

        var results = new List<ChatMessageRecord>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }
}
