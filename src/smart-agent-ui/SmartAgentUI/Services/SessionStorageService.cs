using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SmartAgentUI.Services;

public interface ISessionStorageService
{
    Task<T?> GetItemAsync<T>(string key);
    Task SetItemAsync<T>(string key, T value);
    Task RemoveItemAsync(string key);
}

public class SessionStorageService : ISessionStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionStorageService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<T?> GetItemAsync<T>(string key)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return Task.FromResult<T?>(default);

        var value = session.GetString(key);
        if (string.IsNullOrEmpty(value)) return Task.FromResult<T?>(default);

        try
        {
            return Task.FromResult(JsonSerializer.Deserialize<T>(value));
        }
        catch
        {
            return Task.FromResult<T?>(default);
        }
    }

    public Task SetItemAsync<T>(string key, T value)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return Task.CompletedTask;

        var json = JsonSerializer.Serialize(value);
        session.SetString(key, json);
        return Task.CompletedTask;
    }

    public Task RemoveItemAsync(string key)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove(key);
        return Task.CompletedTask;
    }
}