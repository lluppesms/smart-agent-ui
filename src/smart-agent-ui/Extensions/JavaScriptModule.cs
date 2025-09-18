// Copyright (c) Microsoft. All rights reserved.

namespace SmartAgentUI.Extensions;

using System.Runtime.InteropServices.JavaScript;

internal sealed partial class JavaScriptModule
{
    [JSImport("listenForIFrameLoaded", nameof(JavaScriptModule))]
    public static partial Task RegisterIFrameLoadedAsync(
        string selector,
        [JSMarshalAs<JSType.Function>] Action onLoaded);
}
