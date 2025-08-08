using Microsoft.EntityFrameworkCore;
using DoItList.Data;
using DoItList.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Configure Cookie Authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.Cookie.Name = "DoItListAuth";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? string.Empty))
    };
});

// 3) Add services
builder.Services.AddControllers();

// Mappeo de Razor Pages: la raíz (“/”) ahora muestra Register.cshtml
builder.Services.AddRazorPages(options =>
            {
                // Iniciar aplicación en la página de login
                options.Conventions.AddPageRoute("/Login", "");
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DoItList API", Version = "v1" });
});
// 3.1) Registrar IHttpClientFactory
builder.Services.AddHttpClient();
// Registrar autenticación y el servicio
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// 4) Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// 5) Endpoints
app.MapControllers();
app.MapRazorPages();

// No necesitamos MapGet("/"), pues Razor Pages ya maneja la ruta raíz.

app.Run();
