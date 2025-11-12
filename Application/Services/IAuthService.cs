using Application.DTOs;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request); // Registration now returns string message
        Task<AuthResponse> LoginAsync(LoginRequest request);  // Login returns JWT token wrapped in AuthResponse
    }
}
