using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DoItList.Models;
using DoItList.Services;
using Microsoft.AspNetCore.Http;

namespace DoItList.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            bool success = Input.Action == "register"
                ? await _authService.RegisterAsync(Input, HttpContext)
                : await _authService.LoginAsync(Input, HttpContext);

            if (success)
                return RedirectToPage("/Tasks/Index");

            ModelState.AddModelError(string.Empty, "Acceso o registro inválido.");
            return Page();
        }
    }
}
