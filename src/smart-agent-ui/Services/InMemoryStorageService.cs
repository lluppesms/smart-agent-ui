// Copyright (c) Microsoft. All rights reserved.

namespace ClientApp.Services;

// Temporary interface to replace the WebAssembly local storage service
public interface ILocalStorageService
{
    T? GetItem<T>(string key);
    void SetItem<T>(string key, T value);
}

// Simple in-memory implementation for server-side use
public class InMemoryStorageService : ILocalStorageService
{
    private readonly Dictionary<string, object?> _storage = new();

    public T? GetItem<T>(string key)
    {
        if (_storage.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    public void SetItem<T>(string key, T value)
    {
        _storage[key] = value;
    }
}