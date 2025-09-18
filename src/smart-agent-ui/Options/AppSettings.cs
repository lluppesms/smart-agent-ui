// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Options;

public class AppSettings
{
    [ConfigurationKeyName("BACKEND_URI")]
    public string BackendUri { get; set; } = "https://localhost:7181"!;
}
