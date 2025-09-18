// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Models;

public readonly record struct UserQuestion(
    string Question,
    DateTime AskedOn);
