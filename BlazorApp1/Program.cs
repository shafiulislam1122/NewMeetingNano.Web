using BlazorApp1;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7057/")
});

// JWT/Auth
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddAuthorizationCore();

// MeetingRoomService
builder.Services.AddHttpClient<MeetingRoomService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7057/");
});

await builder.Build().RunAsync();
