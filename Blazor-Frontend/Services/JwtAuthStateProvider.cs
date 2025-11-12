using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public JwtAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        var identity = string.IsNullOrWhiteSpace(savedToken)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    // -------------------- Add this method --------------------
    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }

    public async Task<string> GetUserRoleAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }

    public async Task<string> GetUserNameAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        var user = authState.User;
        return user.Identity?.Name ?? "";
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs != null && keyValuePairs.ContainsKey("role"))
            claims.Add(new Claim(ClaimTypes.Role, keyValuePairs["role"].ToString()));
        if (keyValuePairs != null && keyValuePairs.ContainsKey("unique_name"))
            claims.Add(new Claim(ClaimTypes.Name, keyValuePairs["unique_name"].ToString()));
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
