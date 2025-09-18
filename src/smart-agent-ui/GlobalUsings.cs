// Copyright (c) Microsoft. All rights reserved.

global using System.Diagnostics;
global using System.Globalization;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;

global using Azure.AI.OpenAI;
global using Azure.Identity;
global using Azure.Search.Documents;
global using Azure.Search.Documents.Models;
global using Azure.Storage.Blobs;
global using Azure.Storage.Blobs.Models;

global using Microsoft.AspNetCore.Antiforgery;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Routing;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.Azure.Cosmos;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.JSInterop;
global using Microsoft.SemanticKernel;

global using MudBlazor;
global using MudBlazor.Services;
global using Markdig;

global using ClientApp.Components;
global using ClientApp.Extensions;
global using ClientApp.Models;
global using ClientApp.Options;
global using ClientApp.Services;

global using MinimalApi.Extensions;
global using MinimalApi.Services;
global using MinimalApi.Services.ChatHistory;
global using MinimalApi.Services.Documents;
global using MinimalApi.Services.Profile;
global using MinimalApi.Services.Search;
global using MinimalApi.Services.Security;

global using Shared;
global using Shared.Json;
global using Shared.Models;
