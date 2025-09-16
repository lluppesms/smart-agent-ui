// Copyright (c) Microsoft. All rights reserved.

namespace ClientApp.Components;

public sealed partial class PdfViewerDialog : Microsoft.AspNetCore.Components.ComponentBase
{
    private bool _isLoading = true;
    private string _pdfViewerVisibilityStyle => _isLoading ? "display:none;" : "display:default;";

    [Parameter] public required string FileName { get; set; }
    [Parameter] public required string BaseUrl { get; set; }

    [CascadingParameter] public required IMudDialogInstance Dialog { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        // TODO: Replace with Blazor Server-compatible JavaScript interop
        // await JavaScriptModule.RegisterIFrameLoadedAsync(
        //     "#pdf-viewer",
        //     () =>
        //     {
        //         _isLoading = false;
        //         StateHasChanged();
        //     });
        
        // For now, just set loading to false
        _isLoading = false;
    }

    private void OnCloseClick() => Dialog.Close(DialogResult.Ok(true));
}
