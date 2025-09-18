// Copyright (c) Microsoft. All rights reserved.

global using System.Diagnostics;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.Json;

global using Azure.AI.OpenAI;
global using Azure.Identity;
global using Azure.Search.Documents;
global using Azure.Search.Documents.Models;
global using Azure.Storage.Blobs;
global using Azure.Storage.Blobs.Models;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.SemanticKernel;
global using Microsoft.AspNetCore.Antiforgery;
global using Microsoft.Azure.Cosmos;

global using SmartAgentUI.Extensions;
global using SmartAgentUI.Services;
global using SmartAgentUI.Services.ChatHistory;
global using SmartAgentUI.Services.Documents;
global using SmartAgentUI.Services.Profile;
global using SmartAgentUI.Services.Search;
global using SmartAgentUI.Services.Security;

global using SmartAgentUI.Models;
global using System.Net;
global using System.Reflection;
