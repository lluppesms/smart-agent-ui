// Copyright (c) Microsoft. All rights reserved.

using SmartAgentUI.Models;
using Microsoft.AspNetCore.DataProtection;
using SmartAgentUI;
using Azure;
using Azure.Identity;
using Azure.AI.Agents.Persistent;
using Microsoft.SemanticKernel.Agents.AzureAI;
using MudBlazor.Services;
using System.Runtime.InteropServices.JavaScript;

#pragma warning disable SKEXP0110

Console.WriteLine("Starting SmartAgentUI - Unified Application... {0}", BuildInfo.Instance);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddControllersWithViews();
builder.Services.AddCrossOriginResourceSharing();
builder.Services.AddHttpContextAccessor();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add HttpClient
builder.Services.AddHttpClient();

// For Server-side Blazor, use session storage instead of local storage
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// bind configuration
builder.Services.AddOptions<AppConfiguration>()
.Bind(builder.Configuration)
.PostConfigure(options =>
{
    // set default values for options
    options.ApplicationInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
    options.AzureServicePrincipalClientID = builder.Configuration["AZURE_SP_CLIENT_ID"];
    options.AzureServicePrincipalClientSecret = builder.Configuration["AZURE_SP_CLIENT_SECRET"];
    options.AzureTenantID = builder.Configuration["AZURE_TENANT_ID"];
    options.AzureAuthorityHost = builder.Configuration["AZURE_AUTHORITY_HOST"];
    options.AzureServicePrincipalOpenAIAudience = builder.Configuration["AZURE_SP_OPENAI_AUDIENCE"];
})
.ValidateDataAnnotations()
.ValidateOnStart();

var appConfiguration = new AppConfiguration();
builder.Configuration.Bind(appConfiguration);

// Add Azure services with enhanced error handling
try
{
    Console.WriteLine("Adding Azure services...");
    if (appConfiguration.UseManagedIdentityResourceAccess)
    {
        Console.WriteLine("Using Managed Identity credentials...");
        builder.Services.AddAzureWithMICredentialsServices(appConfiguration);
    }
    else
    {
        Console.WriteLine("Using default credentials...");
        builder.Services.AddAzureServices(appConfiguration);
    }
    Console.WriteLine("Azure services added successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: Failed to configure Azure services: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");

    // Log missing configuration values that might cause issues
    var missingConfigs = new List<string>();
    if (string.IsNullOrEmpty(appConfiguration.AzureStorageAccountEndpoint))
        missingConfigs.Add("AzureStorageAccountEndpoint");
    if (string.IsNullOrEmpty(appConfiguration.AzureSearchServiceEndpoint))
        missingConfigs.Add("AzureSearchServiceEndpoint");
    if (string.IsNullOrEmpty(appConfiguration.CosmosDbEndpoint))
        missingConfigs.Add("CosmosDbEndpoint");

    if (missingConfigs.Any())
    {
        Console.WriteLine($"Missing configuration values: {string.Join(", ", missingConfigs)}");
    }

    Console.WriteLine("Application will continue without Azure services.");
    // Don't throw since we want the app to start even without Azure services
}

builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN-HEADER"; options.FormFieldName = "X-CSRF-TOKEN-FORM"; });

builder.Services.AddSingleton<AppConfiguration>(appConfiguration);
builder.Services.AddSingleton<ProfileService>();

// Configure AppSettings for frontend compatibility
builder.Services.Configure<SmartAgentUI.Options.AppSettings>(
    builder.Configuration.GetSection(nameof(SmartAgentUI.Options.AppSettings))
);

// Register ApiClient service
builder.Services.AddScoped<SmartAgentUI.Services.ApiClient>();

// Register VoicePreferences service
builder.Services.AddScoped<VoicePreferences>();

// Load client-side configuration settings
SmartAgentUIConfiguration.Load(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    // set application telemetry
    if (!string.IsNullOrEmpty(appConfiguration.ApplicationInsightsConnectionString))
    {
        builder.Services.AddApplicationInsightsTelemetry((option) =>
        {
            option.ConnectionString = appConfiguration.ApplicationInsightsConnectionString;
        });
    }

    if (appConfiguration.EnableDataProtectionBlobKeyStorage)
    {
        var containerName = appConfiguration.DataProtectionKeyContainer;
        var storageAccount = appConfiguration.AzureStorageAccountEndpoint;
        var fileName = "keys.xml";

        builder.Services.AddDataProtection().PersistKeysToAzureBlobStorage(storageAccount, containerName, fileName)
            .SetApplicationName("SmartAgentUI")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(90));
    }
}

builder.Services.AddCustomHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.UseRouting();
app.UseStaticFiles();
app.UseSession();
app.UseCors();
app.UseAntiforgery();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Use(next => context =>
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens?.RequestToken ?? string.Empty, new CookieOptions() { HttpOnly = false });
    return next(context);
});

// Map API endpoints
app.MapAgentManagementApi();
app.MapChatApi();
app.MapApi();

app.MapCustomHealthChecks();

#pragma warning disable CA1416 // Validate platform compatibility
await JSHost.ImportAsync(
    moduleName: nameof(JavaScriptModule),
    moduleUrl: $"../js/iframe.js?{Guid.NewGuid()}" /* cache bust */);
#pragma warning restore CA1416 // Validate platform compatibility

Console.WriteLine("SmartAgentUI - Migration Complete! Application is running successfully.");
app.Run();