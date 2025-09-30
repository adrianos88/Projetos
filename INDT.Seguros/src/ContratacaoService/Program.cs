using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ContratacaoService.Infra;
using ContratacaoService.Ports;
using ContratacaoService.Adapters;
using ContratacaoService.Application;

var builder = WebApplication.CreateBuilder(args);

// SQLite local (arquivo contracts.db)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=contracts.db"));

// HttpClient adapter: IPropostaClient -> HttpPropostaClient
// BaseAddress pode ser configurado via configuração "PropostasApi" (ex: appsettings) ou usa o localhost padrão
builder.Services.AddHttpClient<IPropostaClient, HttpPropostaClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PropostasApi"] ?? "http://localhost:5001/");
});

// Registrar o Application Service de Contratacao
builder.Services.AddScoped<ContratacaoAppService>();

// CORS: permitir que a UI (servida em PropostaService) acesse este serviço
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    // enviar enums como strings (ex: "Aprovada")
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// CORS
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
