using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ContratacaoService.Infra;

var builder = WebApplication.CreateBuilder(args);

// SQLite local
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=contracts.db"));

// Permitir requests do frontend que será servido em http://localhost:5001
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HttpClient para acessar PropostaService
builder.Services.AddHttpClient("propostas", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PropostasApi"] ?? "http://localhost:5001/");
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();
