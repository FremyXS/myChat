using Microsoft.EntityFrameworkCore;
using Pract.Database;
using Pract.Services;

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

builder.Services.AddScoped<ChatContext>();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ChatRoomService>();
builder.Services.AddTransient<ChatMessageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
