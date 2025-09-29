# INDT.Seguros

Projeto desenvolvido como parte de um **teste** para a empresa **INDT**. O objetivo é criar uma plataforma simples para **gerenciamento e contratação de propostas de seguro**, seguindo boas práticas de **Arquitetura Hexagonal**, **Clean Code**, **DDD** e **SOLID**.

---

## ✅ Estrutura do Projeto

```
INDT.Seguros/
│── src/
│   ├── PropostaService/      # Microserviço para criação e gestão de propostas
│   └── ContratacaoService/   # (Em desenvolvimento) Microserviço para contratação
│
│── INDT.Seguros.sln          # Solution .NET
│── README.md
```

---

## 🚀 Tecnologias Utilizadas

| Tecnologia | Finalidade |
|------------|------------|
| .NET 8 / C# | API Web |
| Entity Framework Core | ORM / Acesso a dados |
| SQLite | Banco de dados local |
| Swagger | Documentação e testes da API |
| xUnit | Testes unitários (a implementar) |

---

## ▶️ Como Executar

1. Acesse a pasta do microserviço PropostaService:

```bash
cd src/PropostaService
```

2. Restaure os pacotes:

```bash
dotnet restore
```

3. Execute a aplicação:

```bash
dotnet run
```

4. Acesse no navegador:

- Swagger UI: **http://localhost:5001/swagger**
- Página HTML simples (frontend): **http://localhost:5001**

---


---

## ✒ Autor

Desenvolvido por **Adriano** para o teste técnico da **INDT**.
