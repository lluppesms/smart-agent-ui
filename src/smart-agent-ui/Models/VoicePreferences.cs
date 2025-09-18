// Copyright (c) Microsoft. All rights reserved.

namespace ClientApp.Models;

public record class VoicePreferences
{
    private const string PreferredVoiceKey = "preferred-voice";
    private const string PreferredSpeedKey = "preferred-speed";
    private const string TtsIsEnabledKey = "tts-is-enabled";

    private string? _voice;
    private double? _rate;
    private bool? _isEnabled;

    private readonly IHttpContextAccessor _contextAccessor;

    public VoicePreferences(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public string? Voice
    {
        get => _voice ??= _contextAccessor.HttpContext?.Session.GetString(PreferredVoiceKey);
        set
        {
            if (_voice != value && value is not null)
            {
                _voice = value;
                _contextAccessor.HttpContext?.Session.SetString(PreferredVoiceKey, value);
            }
        }
    }

    public double Rate
    {
        get
        {
            if (_rate.HasValue) return _rate.Value;
            
            var rateStr = _contextAccessor.HttpContext?.Session.GetString(PreferredSpeedKey);
            _rate = double.TryParse(rateStr, out var rate) && rate > 0 ? rate : 1;
            return _rate.Value;
        }
        set
        {
            if (_rate != value)
            {
                _rate = value;
                _contextAccessor.HttpContext?.Session.SetString(PreferredSpeedKey, value.ToString());
            }
        }
    }

    public bool IsEnabled
    {
        get
        {
            if (_isEnabled.HasValue) return _isEnabled.Value;
            
            var enabledStr = _contextAccessor.HttpContext?.Session.GetString(TtsIsEnabledKey);
            _isEnabled = bool.TryParse(enabledStr, out var enabled) && enabled;
            return _isEnabled.Value;
        }
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                _contextAccessor.HttpContext?.Session.SetString(TtsIsEnabledKey, value.ToString());
            }
        }
    }

    public void Deconstruct(out string? voice, out double rate, out bool isEnabled) => (voice, rate, isEnabled) = (Voice, Rate, IsEnabled);
}
