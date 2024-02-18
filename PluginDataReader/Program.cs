using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PluginDataReader;
using PluginDataReader.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
                .AddLocalization()
                .AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>())
                .AddScoped<KoikatuAndKoikatsuSunshineHandler>();

await builder.Build().RunAsync();
