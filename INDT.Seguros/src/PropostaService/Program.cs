using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using PropostaService.Infra;
using PropostaService.Domain;
using PropostaService.Application;

var builder = WebApplication.CreateBuilder(args);

// SQLite local (arquivo proposals.db)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=proposals.db"));

// Registrar ports & adapters e application services
builder.Services.AddScoped<IPropostaRepository, PropostaRepositoryEf>();
builder.Services.AddScoped<IPropostaAppService, PropostaAppService>();

// permitir que a UI (serve neste mesmo serviço) faça requisições
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5001", policy =>
    {
        policy.WithOrigins("http://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    // manda enums como strings (Ex: "Aprovada")
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// servir a UI estática em wwwroot (index.html). Crie pasta wwwroot e index.html (depois)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowLocalhost5001");
app.MapControllers();
app.Run();
