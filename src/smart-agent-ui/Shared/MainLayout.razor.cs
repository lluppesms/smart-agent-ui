// Copyright (c) Microsoft. All rights reserved.

using SmartAgentUI.Components;
using static System.Net.WebRequestMethods;

namespace SmartAgentUI.Shared;

public sealed partial class MainLayout
{
    public readonly MudTheme _theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = AppConfiguration.ColorPaletteLightPrimary,
            AppbarBackground = AppConfiguration.ColorPaletteLightAppbarBackground,
            Secondary = AppConfiguration.ColorPaletteLightSecondary
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#1277bd",
        }
    };
    public bool _drawerOpen = false;
    public bool _settingsOpen = false;
    private SettingsPanel? _settingsPanel;

    public bool _isDarkTheme
    {
        // Server-side session storage replacement for client-side local storage
        get 
        {
            var session = HttpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var value = session.GetString(StorageKeys.PrefersDarkTheme);
                return bool.TryParse(value, out var result) && result;
            }
            return false;
        }
        set 
        {
            var session = HttpContextAccessor.HttpContext?.Session;
            session?.SetString(StorageKeys.PrefersDarkTheme, value.ToString());
        }
    }

    public bool _isReversed
    {
        get 
        {
            var session = HttpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var value = session.GetString(StorageKeys.PrefersReversedConversationSorting);
                return bool.TryParse(value, out var result) ? result : true;
            }
            return true;
        }
        set 
        {
            var session = HttpContextAccessor.HttpContext?.Session;
            session?.SetString(StorageKeys.PrefersReversedConversationSorting, value.ToString());
        }
    }

    // // this also fails... why...???
    // private bool GetLocalBool(string settingName, bool defaultValue = false)
    // {
    //     try
    //     {
    //         if (LocalStorage != null)
    //         {
    //             return LocalStorage.GetItem<bool?>(settingName) ?? defaultValue;
    //         }
    //         return defaultValue;
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.WriteLine($"Error getting setting {settingName}: {ex.Message}");
    //         return defaultValue;
    //     }
    // }

    public bool _isRightToLeft =>
        Thread.CurrentThread.CurrentUICulture is { TextInfo.IsRightToLeft: true };

    [Inject] public required NavigationManager Nav { get; set; }
    [Inject] public required IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject] public required IDialogService Dialog { get; set; }

    public bool SettingsDisabled => new Uri(Nav.Uri).Segments.LastOrDefault() switch
    {
        "ask" or "chat" => false,
        _ => true
    };

    public string LogoImagePath
    {
        get
        {
            return AppConfiguration.LogoImagePath;
        }
    }

    private int LogoImageWidth
    {
        get
        {
            return AppConfiguration.LogoImageWidth;
        }
    }

    public bool SortDisabled
    {
        get
        {
            return true;
            //return new Uri(Nav.Uri).Segments.LastOrDefault() switch
            //{
            //    "documents" => true,
            //    _ => false
            //};
        }
    }

    public void OnMenuClicked() => _drawerOpen = !_drawerOpen;

    public void OnThemeChanged() => _isDarkTheme = !_isDarkTheme;

    public void OnIsReversedChanged() => _isReversed = !_isReversed;
}
