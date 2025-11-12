using BlazorApp1;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// ✅ Shared HttpClient (optional but good)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7057/")
});

// ✅ JWT/Auth services
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    provider => provider.GetRequiredService<JwtAuthStateProvider>()
);

builder.Services.AddAuthorizationCore();

// ✅ ✅ Correct MeetingRoomService registration
builder.Services.AddHttpClient<MeetingRoomService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7057/");
});

await builder.Build().RunAsync();
