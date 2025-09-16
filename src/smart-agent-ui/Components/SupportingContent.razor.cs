// Copyright (c) Microsoft. All rights reserved.

namespace ClientApp.Components;

public sealed partial class SupportingContent : Microsoft.AspNetCore.Components.ComponentBase
{
    [Parameter, EditorRequired] public required SupportingContentRecord[] DataPoints { get; set; }

    private ParsedSupportingContentItem[] _supportingContent = [];

    protected override void OnParametersSet()
    {
        if (DataPoints is { Length: > 0 })
        {
            _supportingContent =
                DataPoints.Select(ParseSupportingContent).ToArray();
        }

        base.OnParametersSet();
    }
}
