# INDT.Seguros

Projeto desenvolvido como parte de um **teste técnico** para a empresa **INDT**.  
Objetivo: plataforma simples para **gerenciamento e contratação de propostas de seguro** (criar propostas, consultar status e efetuar contratação). Implementação atual (entregável): dois microserviços em .NET **9** com persistência SQLite e uma UI estática simples servida pelo `PropostaService`.

**Autor:** Adriano

---

## Resumo da implementação (o que já existe)
- **Tecnologia:** .NET 9 (C#), Entity Framework Core, SQLite, Swagger.
- **Serviços:**
  - `PropostaService` — API REST para criação, listagem e alteração de status de propostas. Serve uma UI estática simples (`wwwroot/index.html`) para criar/visualizar/aprovar propostas e iniciar contratações.
  - `ContratacaoService` — API REST que valida uma proposta consultando o `PropostaService` e persiste contratações.
- **Persistência:** SQLite (arquivos `proposals.db` e `contracts.db`) gerados via EF Core Migrations.
- **Documentação:** cada serviço expõe Swagger UI.
- **Frontend:** UI HTML+JS simples (index.html) servida por `PropostaService` em `http://localhost:5001/`.

---

## Estrutura do repositório
```
INDT.Seguros/
│── src/
│   ├── PropostaService/
│   │   ├── Controllers/
│   │   ├── Domain/
│   │   ├── Infra/
│   │   ├── wwwroot/ (index.html UI)
│   │   └── Program.cs
│   └── ContratacaoService/
│       ├── Controllers/
│       ├── Domain/
│       ├── Infra/
│       └── Program.cs
│
│── INDT.Seguros.sln
│── README.md
```

---

## Pré-requisitos (o que instalar)
- .NET SDK **9** (`dotnet --version` deve retornar `9.x`)
- (Opcional) VS Code + extensão C# e Thunder Client para testar
- (uma vez) EF Core CLI:
```bash
dotnet tool install --global dotnet-ef
# ou atualizar caso já tenha:
dotnet tool update --global dotnet-ef
```

---

## Restaurar pacotes
No diretório raiz do projeto (ou em cada projeto):
```bash
dotnet restore
```

---

## Criar migrations e gerar os arquivos SQLite
Execute os comandos dentro da pasta de cada projeto (onde está o `.csproj`):

**PropostaService**
```bash
cd src/PropostaService
dotnet ef migrations add InitialCreate -o Infra/Migrations
dotnet ef database update
# gera proposals.db no diretório do projeto
cd ../..
```

**ContratacaoService**
```bash
cd src/ContratacaoService
dotnet ef migrations add InitialCreate -o Infra/Migrations
dotnet ef database update
# gera contracts.db no diretório do projeto
cd ../..
```

> Observação: se houver problema e você precisar avançar rápido, pode trocar temporariamente o `DbContext` para `UseInMemoryDatabase("teste")` — mas deixe claro no README do repositório que foi uma decisão temporária.

---

## Rodando localmente (duas janelas/terminals recomendadas)
**Terminal 1 — PropostaService (porta 5001)**
```bash
cd src/PropostaService
dotnet run --urls=http://localhost:5001
```

**Terminal 2 — ContratacaoService (porta 5002)**
```bash
cd src/ContratacaoService
dotnet run --urls=http://localhost:5002
```

Acesse:
- UI (frontend simples): `http://localhost:5001/`
- Swagger PropostaService: `http://localhost:5001/swagger`
- Swagger ContratacaoService: `http://localhost:5002/swagger`

---

## Fluxo de uso e exemplos (curl e UI)
Fluxo: criar proposta → aprovar → contratar (ContratacaoService só cria contratação se proposta estiver `Aprovada`).

**Criar proposta**
```bash
curl -X POST http://localhost:5001/api/propostas   -H "Content-Type: application/json"   -d '{"cliente":"Joao","valor":1200,"descricao":"Seguro Auto"}'
```

**Listar propostas**
```bash
curl http://localhost:5001/api/propostas
```

**Aprovar proposta** (substitua `<ID>` pelo id retornado)
```bash
curl -X PUT http://localhost:5001/api/propostas/<ID>/status   -H "Content-Type: application/json"   -d '{"status":"Aprovada"}'
```

**Contratar proposta** (substitua `<ID>`)
```bash
curl -X POST http://localhost:5002/api/contratacoes   -H "Content-Type: application/json"   -d '{"propostaId":"<ID>","contratadoPor":"Usuario UI"}'
```

Também é possível executar o fluxo via a UI em `http://localhost:5001/` (criar → listar → aprovar → contratar).

---

## CORS e comunicação entre serviços
- `ContratacaoService` usa `HttpClient` com base address `http://localhost:5001/` por padrão (ver `Program.cs`).
- A UI serve de `http://localhost:5001` e faz chamadas para `http://localhost:5002` ao contratar; por isso, `ContratacaoService` precisa permitir CORS da origem `http://localhost:5001`. As políticas CORS foram adicionadas nos `Program.cs` dos serviços para permitir esse fluxo.

---
