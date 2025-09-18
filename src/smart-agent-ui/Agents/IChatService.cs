// Copyright (c) Microsoft. All rights reserved.

using SmartAgentUI.Services.Profile;

namespace MinimalApi.Agents;

public interface IChatService
{
    IAsyncEnumerable<ChatChunkResponse> ReplyAsync(UserInformation user, ProfileDefinition profile, ChatRequest request, CancellationToken cancellationToken = default);
}
