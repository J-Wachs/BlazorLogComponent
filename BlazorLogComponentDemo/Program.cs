using BlazorLogComponent.Interfaces;
using BlazorLogComponent.Logging;
using BlazorLogComponent.Services;
using BlazorLogComponentDemo.Components;

var builder = WebApplication.CreateBuilder(args);

// ******************************************************************
// Setup for the custom logging service and provider.
//
// 1. Register the singleton logging service.
builder.Services.AddSingleton<ILoggingService, LoggingService>();

// 2. Add the custom logger provider to the .NET logging pipeline.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.Services.AddSingleton<ILoggerProvider, InMemoryLoggerProvider>();
});
// ******************************************************************

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
