using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepoSearches.DAL;
using RepoSearches.Models;
using System.Text;
using services = RepoSearches.Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using RepoSearches.Core.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using RepoSearches.JwtHandler;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using AdApp.Core.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "token"; 
       // options.Cookie.HttpOnly = true;
        options.Cookie.HttpOnly = false;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Changed to Always      
        options.LoginPath = "/login"; // login path
        options.LogoutPath = "/logout"; // logout path

    });
// Define CORS policy


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Set-Cookie");  // Add this line
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information));

// Add services to the container.
builder.Services.AddScoped<services.Bookmarks.IBookmarksService, services.Bookmarks.BookmarksService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<TokenValidationParameters>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers()
    .AddFluentValidation(static fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());
// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Bind the GitHub configuration
builder.Services.AddOptions<GitHubConfig>()
    .Bind(builder.Configuration.GetSection("GitHub"))
    .ValidateDataAnnotations();

var app = builder.Build();
// Ensure database is created on startup (for demo purposes only)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Migrate or create database schema
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Request Path: {Path}", context.Request.Path);
    logger.LogInformation("Request Method: {Method}", context.Request.Method);

    foreach (var header in context.Request.Headers)
    {
        logger.LogInformation("Header: {Key} = {Value}", header.Key, header.Value);
    }

    foreach (var cookie in context.Request.Cookies)
    {
        logger.LogInformation("Cookie: {Key} = {Value}", cookie.Key, cookie.Value);
    }

    await next();
});
// Use the middleware
//app.UseMiddleware<JwtMiddleware>();

app.Run();

