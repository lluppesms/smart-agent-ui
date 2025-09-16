// Copyright (c) Microsoft. All rights reserved.

using static MudBlazor.CategoryTypes;

namespace ClientApp.Components;

public sealed partial class DisclaimerDialog : Microsoft.AspNetCore.Components.ComponentBase
{

    [CascadingParameter] public required IMudDialogInstance Dialog { get; set; }

    private string _disclaimerMessage;

    private void OnCloseClick() => Dialog.Close(DialogResult.Ok(true));

    protected override void OnParametersSet()
    {
        _disclaimerMessage = ClientAppConfiguration.DisclaimerMessage;
        base.OnParametersSet();
    }
}
