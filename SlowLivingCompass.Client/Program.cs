using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SlowLivingCompass.Client;
using SlowLivingCompass.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
    builder.Services.AddSingleton<PlaceService>();
    builder.Services.AddScoped<JourneyService>();
    builder.Services.AddScoped<LlmService>();

await builder.Build().RunAsync();
