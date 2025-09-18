// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Models;

public record ChatTurn(string User, string? Assistant = null);
