// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Models;

public record RequestSettingsOverrides
{
    public RequestOverrides Overrides { get; set; } = new();
}
