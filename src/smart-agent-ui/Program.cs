// Copyright (c) Microsoft. All rights reserved.

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SmartAgentUI;
using SmartAgentUI.Services;
using SmartAgentUI.Options;
using SmartAgentUI.Models;
using SmartAgentUI.Interop;

Console.WriteLine("Starting SmartAgentUI - Unified Blazor WebAssembly Application... {0}", BuildInfo.Instance);

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Configure AppSettings for frontend compatibility
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(nameof(AppSettings))
);

// Add HttpClient services
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), Timeout = TimeSpan.FromMinutes(5) });

// Add Blazor WebAssembly specific services
builder.Services.AddLocalStorageServices();
builder.Services.AddSessionStorageServices();
builder.Services.AddMudServices();

// Register VoicePreferences service (will use client-side storage)
builder.Services.AddScoped<VoicePreferences>();

// Load app configuration
SmartAgentUIConfiguration.Load(builder.Configuration);

// Import JavaScript modules
await JSHost.ImportAsync(
    moduleName: nameof(JavaScriptModule),
    moduleUrl: $"../js/iframe.js?{Guid.NewGuid()}" /* cache bust */);

var host = builder.Build();
await host.RunAsync();