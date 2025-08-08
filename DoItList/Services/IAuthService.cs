using System.Threading.Tasks;
using DoItList.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DoItList.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(LoginViewModel model, HttpContext httpContext);
        Task<bool> LoginAsync(LoginViewModel model, HttpContext httpContext);
        Task LogoutAsync(HttpContext httpContext);
    }
}
