using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoItList.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

public string ErrorMessage { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombre")]
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var payload = new
            {
                name = Input.Name,
                email = Input.Email,
                password = Input.Password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/api/Auth/register", content);
if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Tasks/Index");
            }

            var errorJson = await response.Content.ReadAsStringAsync();
            ErrorMessage = errorJson; // Puedes parsear JSON para mostrar errores específicos
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }
    }
}
