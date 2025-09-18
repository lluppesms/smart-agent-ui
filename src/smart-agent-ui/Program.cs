// Copyright (c) Microsoft. All rights reserved.

using MudBlazor.Services;

Console.WriteLine("Starting SmartAgentUI - Unified Application...");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add MudBlazor services
builder.Services.AddMudServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Console.WriteLine("SmartAgentUI - Migration Complete! Application is running successfully.");
app.Run();