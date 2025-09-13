using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InfinionInterviewProject.Infrastructure.Persistence;
using InfinionInterviewProject.Infrastructure.Services;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Infrastructure.Interfaces;
using InfinionInterviewProject.Infrastructure.Seed;
using InfinionInterviewProject.Infrastructure.Repositories;
using InfinionInterviewProject.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InfinionInterviewProject.Infrastructure.Seed.InfinionInterviewProject.Infrastructure.Seed;
using InfinionInterviewProject.Application.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger + JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "InfinionInterviewProject API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database (local SQL by default)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Server=(localdb)\\mssqllocaldb;Database=InfinionInterviewDb;Trusted_Connection=True;"
    ));

// HttpClients
builder.Services.AddHttpClient<ExternalStateClient>();
builder.Services.AddHttpClient();

// Application Services
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<JwtSettings>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IEmailSmsService, FakeEmailSmsService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IStateService, StateService>();

builder.Services.AddHttpClient<IBankService, BankService>(client =>
{
    client.BaseAddress = new Uri("https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/");
});

// JWT Setup
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwtSettings = jwtSection.Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

var app = builder.Build();

// Seed DB with States & LGAs
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var http = scope.ServiceProvider.GetRequiredService<HttpClient>();
        StateLgaSeeder.SeedAsync(db, http).GetAwaiter().GetResult();
        Console.WriteLine("Seed complete (or skipped).");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Seed error: " + ex.Message);
    }
}

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

//Enable JWT auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
