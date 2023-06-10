using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pract.Database;
using Pract.Hubs;
using Pract.Services;
using Pract.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatContext>(el =>
{
    //el.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    el.UseNpgsql("Server=localhost;Port=6789;Database=postgres;Username=postgres;Password=123456");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        };
    });

builder.Services.AddScoped<ChatContext>();

builder.Services.AddScoped<AccountLoginRequestValidator>();
builder.Services.AddScoped<AccountCreateRequestValidator>();

builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ChatRoomService>();
builder.Services.AddTransient<ChatMessageService>();

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy
//                          .AllowAnyOrigin()
//                          .AllowAnyHeader()                                                  
//                          .AllowAnyMethod()
//                          .SetIsOriginAllowed(origin => true)
//                          .AllowCredentials(); 
//                      });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);
app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials()); // allow credentials

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");


app.MapControllers();

app.Run();
public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}