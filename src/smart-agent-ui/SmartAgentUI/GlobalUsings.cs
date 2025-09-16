// Copyright (c) Microsoft. All rights reserved.

global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Globalization;
global using System.Net;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.Routing;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;
global using Microsoft.JSInterop;
global using MudBlazor;
global using Markdig;
global using SmartAgentUI.Services;
global using ClientApp.Models;
global using ClientApp.Services;

// Backend global usings
global using System.Runtime.CompilerServices;
global using Azure.AI.OpenAI;
global using Azure.Identity;
global using Azure.Search.Documents;
global using Azure.Search.Documents.Models;
global using Azure.Storage.Blobs;
global using Azure.Storage.Blobs.Models;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.SemanticKernel;
global using Microsoft.AspNetCore.Antiforgery;
global using Microsoft.Azure.Cosmos;
global using MinimalApi.Extensions;
global using MinimalApi.Services;
global using MinimalApi.Services.ChatHistory;
global using MinimalApi.Services.Documents;
global using MinimalApi.Services.Profile;
global using MinimalApi.Services.Search;
global using MinimalApi.Services.Security;
global using Shared;
global using Shared.Models;
global using System.Reflection;
