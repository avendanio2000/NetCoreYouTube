using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatsappAPI;
using WhatsappAPI.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//NO FUNCIONA CON ROLES
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDBContext>()
//    .AddDefaultTokenProviders()
//    .AddRoles<IdentityRole>(); // Agrega esta línea para habilitar la gestión de roles

//MODIFICACION AddIdentity<IdentityUser, IdentityRole>() x AddIdentityCore<IdentityUser>()
builder.Services.AddIdentityCore<IdentityUser>()
        .AddEntityFrameworkStores<ApplicationDBContext>()
        .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    //Titulo (Diseño)
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Whatsapp API", Version = "v1" });

    //Boton Autorize (Swagger)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JwtAuthorization",
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
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy =>
//    {
//        policy.RequireRole("administrador");
//    });
//});


builder.Services.AddLog4net();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//  );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
