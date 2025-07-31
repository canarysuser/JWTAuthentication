using AuthenticationService.Infrastructure;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(connStr));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(builder =>
{
    builder.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Authentication Service API",
        Version = "v1",
        Description = "API for user authentication and token management"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); 
}


app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
