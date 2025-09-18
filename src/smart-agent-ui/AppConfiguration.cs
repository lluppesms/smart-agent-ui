// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json.Serialization;

namespace SmartAgentUI;

public class AppConfiguration
{
    public string DataProtectionKeyContainer { get; private set; } = "dataprotectionkeys";
    public bool EnableDataProtectionBlobKeyStorage { get; private set; }
    public string UserDocumentUploadBlobStorageContentContainer { get; private set; } = "content";
    public string UserDocumentUploadBlobStorageExtractContainer { get; private set; } = "content-extract";
    public bool UseManagedIdentityResourceAccess { get; init; }
    public string UserAssignedManagedIdentityClientId { get; init; }
    public string VisualStudioTenantId { get; init; }
    public string? CosmosDbEndpoint { get; init; }
    public string? CosmosDBConnectionString { get; init; }
    public string? AzureSearchServiceEndpoint { get; init; }
    public string? AzureSearchServiceKey { get; init; }
    public string? AzureSearchServiceIndexName { get; init; }
    public string AzureStorageAccountEndpoint { get; init; }
    public string AzureStorageAccountConnectionString { get; init; }
    public string AzureStorageUserUploadContainer { get; init; }
    public string AzureStorageContainer { get; init; }
    public string DocumentUploadStrategy { get; init; } = "AzureNative";
    public string? IngestionPipelineAPI { get; init; }
    public string IngestionPipelineAPIKey { get; init; }

    [JsonPropertyName("APPLICATIONINSIGHTS_CONNECTION_STRING")]
    public string? ApplicationInsightsConnectionString { get; set; }

    // On-Behalf-Of (OBO) Flow
    [JsonPropertyName("AZURE_SP_CLIENT_ID")]
    public string? AzureServicePrincipalClientID { get; set; }
    [JsonPropertyName("AZURE_SP_CLIENT_SECRET")]
    public string? AzureServicePrincipalClientSecret { get; set; }
    [JsonPropertyName("AZURE_TENANT_ID")]
    public string? AzureTenantID { get; set; }
    [JsonPropertyName("AZURE_AUTHORITY_HOST")]
    public string? AzureAuthorityHost { get; set; }
    [JsonPropertyName("AZURE_SP_OPENAI_AUDIENCE")]
    public string? AzureServicePrincipalOpenAIAudience { get; set; }

    public string OcpApimSubscriptionHeaderName { get; init; } = "Ocp-Apim-Subscription-Key";
    public string OcpApimSubscriptionKey { get; init; } = "Ocp-Apim-Subscription-Key";
    public string XMsTokenAadAccessToken { get; init; } = "X-MS-TOKEN-AAD-ACCESS-TOKEN";

    public string? AOAIStandardChatGptDeployment { get; init; }
    public string? AOAIStandardServiceEndpoint { get; init; }
    public string? AOAIStandardServiceKey { get; init; }

    // Profile configuration
    public string? ProfileConfigurationBlobStorageContainer { get; init; }
    public string? ProfileConfiguration { get; init; }
    public string ProfileFileName { get; init; } = "profiles";

    // Frontend configuration properties for compatibility
    public static string ColorPaletteLightPrimary { get; set; } = "#005eb8";
    public static string ColorPaletteLightSecondary { get; set; } = "#287FA4";
    public static string ColorPaletteLightAppbarBackground { get; set; } = "#84B1CB";
    public static string LogoImagePath { get; set; } = "icon-512.png";
    public static int LogoImageWidth { get; set; } = 150;
    public static string DisclaimerMessage { get; set; } = "DISCLAIMER MESSAGE";
}
