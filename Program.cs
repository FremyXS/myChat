using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pract.Database;
using Pract.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatContext>(el =>
{
    //el.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    el.UseNpgsql("Server=localhost;Port=6789;Database=postgres;Username=postgres;Password=123456");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        };
    });

builder.Services.AddScoped<ChatContext>();

builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ChatRoomService>();
builder.Services.AddTransient<ChatMessageService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()                                                  
                          .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}