# INDT.Seguros

Projeto desenvolvido como parte de um **teste técnico** para a empresa **INDT**.  
Autor: **Adriano**

## Visão geral
INDT.Seguros é composto por **dois microserviços** em .NET 9 seguindo a abordagem **Hexagonal (Ports & Adapters)** e com uma camada **Application** (serviços de aplicação). Persistência com **SQLite** via Entity Framework Core. O `PropostaService` também serve uma UI estática (HTML+JS) para demonstrar o fluxo.

- **PropostaService**: criação/listagem/alteração de status de propostas; Application layer (`PropostaAppService`); port `IPropostaRepository` e adapter `PropostaRepositoryEf`; UI estática em `wwwroot/`; banco `proposals.db`.
- **ContratacaoService**: criação de contratações após validação com PropostaService; port `IPropostaClient` e adapter `HttpPropostaClient`; Application layer `ContratacaoAppService`; banco `contracts.db`.

---

## Estrutura do repositório
```
INDT.Seguros/
├─ src/
│  ├─ PropostaService/
│  │  ├─ Controllers/
│  │  ├─ Application/        # PropostaAppService, interfaces
│  │  ├─ Domain/             # Entidades, ports (IPropostaRepository)
│  │  ├─ Infra/              # AppDbContext, PropostaRepositoryEf, Migrations
│  │  ├─ wwwroot/            # index.html UI
│  │  └─ Program.cs
│  └─ ContratacaoService/
│     ├─ Controllers/
│     ├─ Application/        # ContratacaoAppService
│     ├─ Domain/
│     ├─ Infra/              # AppDbContext, Migrations
│     ├─ Adapters/           # HttpPropostaClient
│     ├─ Ports/              # IPropostaClient
│     └─ Program.cs
├─ INDT.Seguros.sln
└─ README.md
```

---

## Pré-requisitos
- .NET SDK **9** (`dotnet --version` deve retornar 9.x)
- (Opcional) VS Code com extensão C#
- (uma vez) EF Core CLI:
```bash
dotnet tool install --global dotnet-ef
# ou atualizar
dotnet tool update --global dotnet-ef
```

---

## Restaurar dependências
No diretório raiz:
```bash
dotnet restore
```

---

## Migrations e criar DB (SQLite)
As migrations estão versionadas em `Infra/Migrations`. Para aplicar e criar os arquivos `.db`, execute dentro de cada projeto (no diretório onde está o `.csproj`):

**PropostaService**
```bash
cd src/PropostaService
dotnet ef database update
# gera proposals.db
cd ../..
```

**ContratacaoService**
```bash
cd src/ContratacaoService
dotnet ef database update
# gera contracts.db
cd ../..
```

Se alterar entidades e precisar criar novas migrations:
```bash
dotnet ef migrations add NomeDaMigration -o Infra/Migrations
dotnet ef database update
```

---

## Como executar (local)
Abra dois terminais:

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

Endpoints úteis:
- UI (PropostaService): `http://localhost:5001/`
- Swagger PropostaService: `http://localhost:5001/swagger`
- Swagger ContratacaoService: `http://localhost:5002/swagger`

---

## Endpoints principais

### PropostaService
- `POST /api/propostas` — criar proposta  
  Body:
  ```json
  { "cliente": "Joao", "valor": 1200, "descricao": "Seguro Auto" }
  ```
- `GET /api/propostas` — listar propostas
- `GET /api/propostas/{id}` — obter proposta por id
- `PUT /api/propostas/{id}/status` — alterar status  
  Body:
  ```json
  { "status": "Aprovada" }
  ```

### ContratacaoService
- `POST /api/contratacoes` — criar contratação (consulta PropostaService)  
  Body:
  ```json
  { "propostaId": "GUID_DA_PROPOSTA", "contratadoPor": "Nome" }
  ```
- `GET /api/contratacoes` — listar contratações
- `GET /api/contratacoes/{id}` — obter contratação por id

---

## Teste rápido (curl) — fluxo criar → aprovar
1. Criar proposta:
```bash
curl -s -X POST http://localhost:5001/api/propostas   -H "Content-Type: application/json"   -d '{"cliente":"Joao","valor":1200,"descricao":"Seguro Auto"}'
```
Salve o `id` retornado.

2. Verificar proposta:
```bash
curl http://localhost:5001/api/propostas
```

3. Aprovar proposta (substitua <ID>):
```bash
curl -X PUT http://localhost:5001/api/propostas/<ID>/status   -H "Content-Type: application/json"   -d '{"status":"Aprovada"}' -i
```

4. Contratar (opcional — chama ContratacaoService):
```bash
curl -X POST http://localhost:5002/api/contratacoes   -H "Content-Type: application/json"   -d '{"propostaId":"<ID>","contratadoPor":"Usuario UI"}' -i
```

---




