// Copyright (c) Microsoft. All rights reserved.

using ClientApp.Models;
using Microsoft.AspNetCore.DataProtection;
using MinimalApi;
using Azure;
using Azure.Identity;
using Azure.AI.Agents.Persistent;
using Microsoft.SemanticKernel.Agents.AzureAI;

#pragma warning disable SKEXP0110

Console.WriteLine("Starting SmartAgent UI... {0}", BuildInfo.Instance);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Backend services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddControllersWithViews();
builder.Services.AddCrossOriginResourceSharing();
builder.Services.AddHttpContextAccessor();

// MudBlazor services for UI
builder.Services.AddMudServices();

// Frontend local/session storage services - server-side implementations
builder.Services.AddScoped<ClientApp.Services.ILocalStorageService, ClientApp.Services.InMemoryStorageService>();

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

// Add Azure services to the container.
if (appConfiguration.UseManagedIdentityResourceAccess)
    builder.Services.AddAzureWithMICredentialsServices(appConfiguration);
else
    builder.Services.AddAzureServices(appConfiguration);

builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN-HEADER"; options.FormFieldName = "X-CSRF-TOKEN-FORM"; });

builder.Services.AddSingleton<AppConfiguration>();
builder.Services.AddSingleton<ProfileService>();

// Frontend services
builder.Services.AddScoped<ApiClient>();
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Environment.WebRootPath);
});

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
        var storageAccunt = appConfiguration.AzureStorageAccountEndpoint;
        var fileName = "keys.xml";

        builder.Services.AddDataProtection().PersistKeysToAzureBlobStorage(storageAccunt, containerName, fileName)
            .SetApplicationName("SmartAgent.UI")
            .SetDefaultKeyLifetime(TimeSpan.FromDays(90));
    }
}

builder.Services.AddCustomHealthChecks();

var app = builder.Build();

// Configure client app configuration
ClientApp.ClientAppConfiguration.Load(builder.Configuration);

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
app.UseCors();
app.UseAntiforgery();

// Configure Blazor Server
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Configure API endpoints
app.MapControllers();

app.Use(next => context =>
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens?.RequestToken ?? string.Empty, new CookieOptions() { HttpOnly = false });
    return next(context);
});

app.MapAgentManagementApi();
app.MapChatApi();
app.MapApi();

app.MapCustomHealthChecks();

app.Run();