// Copyright (c) Microsoft. All rights reserved.

namespace ClientApp.Pages;

public sealed partial class Ingestion : Microsoft.AspNetCore.Components.ComponentBase
{
    [Inject]
    public required ApiClient Client { get; set; }
    private string _sourceContinerName = string.Empty;
    private string _indexName = string.Empty;


    protected void OnInitialized()
    {
    }

    private async Task SubmitAsync()
    {

    }
}
