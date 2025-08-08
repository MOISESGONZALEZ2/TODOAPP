using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DoItList.Data;
using DoItList.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DoItList.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

public async Task<bool> RegisterAsync(DoItList.Models.LoginViewModel model, HttpContext httpContext)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
                return false;

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = ""
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignInUserAsync(user, httpContext);
            return true;
        }

public async Task<bool> LoginAsync(DoItList.Models.LoginViewModel model, HttpContext httpContext)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
                return false;

            await SignInUserAsync(user, httpContext);
            return true;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task SignInUserAsync(User user, HttpContext httpContext)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
