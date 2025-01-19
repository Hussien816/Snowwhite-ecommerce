using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcommerceApi.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Skip authentication for cart endpoints
    options.ForwardDefaultSelector = context =>
        context.Request.Path.StartsWithSegments("/api/Cart") ? null : JwtBearerDefaults.AuthenticationScheme;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured"))),
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.CommandTimeout(120);
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: new[] { -2, 258, -1, 4060, 4221, 18456 });
            sqlOptions.MinBatchSize(1);
            sqlOptions.MaxBatchSize(100);
        }));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });
    
    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
    });
}

// Configure the middleware pipeline in correct order
app.UseCors(); // First to allow CORS headers
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles(); // Serve files from wwwroot

// Serve files from the root directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "..")),
    RequestPath = ""
});

// Configure images directory
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

// Serve images with caching disabled
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images",
    ServeUnknownFileTypes = true,
    DefaultContentType = "image/jpeg",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        ctx.Context.Response.Headers.Append("Expires", "-1");
    }
});

// Authentication & Authorization middleware
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

// Map controllers and image routes
app.MapControllers();
app.MapGet("/images/{**path}", async (string path, HttpContext context) =>
{
    var filePath = Path.Combine(imagesPath, path);
    if (!File.Exists(filePath))
    {
        return Results.NotFound();
    }

    var contentType = Path.GetExtension(filePath).ToLower() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".gif" => "image/gif",
        _ => "application/octet-stream"
    };

    return Results.File(filePath, contentType);
});

// Log application configuration
Console.WriteLine("\nApplication Configuration:");
Console.WriteLine("1. CORS enabled - Allowing cross-origin requests");
Console.WriteLine("2. HTTPS redirection enabled - Secure communication");
Console.WriteLine("3. Static files enabled - Serving wwwroot and images");
Console.WriteLine("4. Authentication enabled - JWT Bearer scheme");
Console.WriteLine("5. Authorization enabled - Protected endpoints");
Console.WriteLine($"6. JWT Configuration:");
Console.WriteLine($"   - Issuer: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"   - Audience: {builder.Configuration["Jwt:Audience"]}");
Console.WriteLine($"   - Token expiration: {TimeSpan.FromDays(7)}");
Console.WriteLine($"   - Clock skew: {TimeSpan.Zero}\n");

app.Run();
