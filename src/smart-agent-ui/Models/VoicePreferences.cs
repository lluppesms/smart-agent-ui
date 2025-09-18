// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Models;

public record class VoicePreferences
{
    private const string PreferredVoiceKey = "preferred-voice";
    private const string PreferredSpeedKey = "preferred-speed";
    private const string TtsIsEnabledKey = "tts-is-enabled";

    private string? _voice;
    private double? _rate;
    private bool? _isEnabled;

    private readonly ILocalStorageService _localStorage;

    public VoicePreferences(ILocalStorageService localStorage) => _localStorage = localStorage;

    public string? Voice
    {
        get
        {
            if (_voice == null)
            {
                try
                {
                    _voice = _localStorage.GetItem<string?>(PreferredVoiceKey);
                }
                catch
                {
                    _voice = null;
                }
            }
            return _voice;
        }
        set
        {
            if (_voice != value && value is not null)
            {
                _voice = value;
                try
                {
                    _localStorage.SetItem(PreferredVoiceKey, value);
                }
                catch
                {
                    // Handle storage errors gracefully
                }
            }
        }
    }

    public double Rate
    {
        get
        {
            if (_rate.HasValue) return _rate.Value;
            
            try
            {
                var rate = _localStorage.GetItem<double?>(PreferredSpeedKey);
                _rate = rate > 0 ? rate : 1;
            }
            catch
            {
                _rate = 1;
            }
            return _rate.Value;
        }
        set
        {
            if (_rate != value)
            {
                _rate = value;
                try
                {
                    _localStorage.SetItem(PreferredSpeedKey, value);
                }
                catch
                {
                    // Handle storage errors gracefully
                }
            }
        }
    }

    public bool IsEnabled
    {
        get
        {
            if (_isEnabled.HasValue) return _isEnabled.Value;
            
            try
            {
                _isEnabled = _localStorage.GetItem<bool?>(TtsIsEnabledKey) ?? false;
            }
            catch
            {
                _isEnabled = false;
            }
            return _isEnabled.Value;
        }
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                try
                {
                    _localStorage.SetItem(TtsIsEnabledKey, value);
                }
                catch
                {
                    // Handle storage errors gracefully
                }
            }
        }
    }

    public void Deconstruct(out string? voice, out double rate, out bool isEnabled) => (voice, rate, isEnabled) = (Voice, Rate, IsEnabled);
}
