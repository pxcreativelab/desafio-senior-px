# Banco de dados — HubieTest

SQL Server (LocalDB já é suficiente). São dois scripts, na ordem:

1. `01_schema.sql` — cria as tabelas (`APP_USER`, `CATEGORY`, `TICKET`, `INTERACTION`, `ATTACHMENT`).
2. `02_seed.sql` — carga inicial: 2 usuários (requester/agent) e 3 categorias.

## Opção A — Visual Studio / SSMS / Azure Data Studio

1. Conecte em `(localdb)\MSSQLLocalDB`.
2. `CREATE DATABASE HubieTest;`
3. Abra e execute `01_schema.sql` e depois `02_seed.sql` (com o banco `HubieTest` selecionado).

## Opção B — linha de comando (sqlcmd)

```powershell
sqllocaldb start MSSQLLocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "IF DB_ID('HubieTest') IS NULL CREATE DATABASE HubieTest;"
sqlcmd -S "(localdb)\MSSQLLocalDB" -i 01_schema.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -i 02_seed.sql
```

## Usuários de teste

| Perfil    | Login       | Senha    |
|-----------|-------------|----------|
| Requester | `requester` | `123456` |
| Agent     | `agent`     | `123456` |

A senha é gravada como **SHA-256(senha)** em hexadecimal — a mesma função usada em
`HubieTest.Business/Security/SecurityHelper.cs`. Para criar novos usuários, gere o hash com
essa função (ou com qualquer ferramenta SHA-256).

## Sobre o EDMX

O modelo (`HubieTest.Dal/Model/DataModel.edmx`) já está mapeado para este schema. Se você
alterar as tabelas, pode regenerar via **"Update Model from Database"** no designer do EDMX
dentro do Visual Studio.

> Observação: a tabela de usuários se chama `APP_USER` porque `USER` é palavra reservada no T-SQL.
