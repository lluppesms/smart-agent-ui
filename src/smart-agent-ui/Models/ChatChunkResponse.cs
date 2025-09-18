// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Models;

public record class ChatChunkResponse(string Text, ApproachResponse? FinalResult = null);
