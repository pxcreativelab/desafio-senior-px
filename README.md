# Teste Técnico — Desenvolvedor(a) .NET Sênior · Hubie

Olá! 👋 Este é o nosso desafio técnico para a vaga de **Desenvolvedor(a) .NET Sênior**.

Ele foi montado **com a mesma stack e os mesmos padrões do Hubie** (o produto em que você
vai trabalhar). Não é um "todo-list app genérico": a ideia é você sentir como é o nosso dia
a dia e nós entendermos como você pensa arquitetura, escreve código e resolve um problema
**de ponta a ponta — do frontend ao banco**.

> Reservamos tempo para preparar isso porque a vaga é importante para o time. Capriche que
> a gente lê com atenção. 🙂
>
> Observação: o **código está todo em inglês** (classes, variáveis, etc.). Apenas esta
> documentação está em português.

---

## 🧩 O cenário

Uma **Central de Chamados** (*Support Desk*) com dois perfis:

- **Requester** (solicitante) — abre um chamado (*ticket*): uma **Complaint** (reclamação),
  uma **Question** (dúvida), etc. Acompanha o andamento, conversa com o atendente e anexa
  arquivos.
- **Agent** (atendente) — vê a fila de chamados, assume um chamado, responde, troca o status
  e também anexa arquivos.

O chamado tem uma **categoria** (Complaint / Question / Suggestion) e passa por **status**
(`OPEN → IN_PROGRESS → ANSWERED → CLOSED`). Requester e Agent trocam mensagens numa **thread
de interações** (`INTERACTION`), e ambos podem **anexar arquivos** (`ATTACHMENT`).

---

## 🛠️ Stack obrigatória (a mesma do Hubie)

| Camada    | Tecnologia                                                        |
|-----------|-------------------------------------------------------------------|
| Frontend  | **AngularJS 1.x** (ui-router, `$http`, `ngStorage`)               |
| Backend   | **ASP.NET .NET Framework 4.8** com **Generic Handlers (`.ashx`)** |
| Dados     | **SQL Server** + **Entity Framework 6 (Database-First / EDMX)**   |
| Auth      | **JWT** (Bearer token)                                            |
| JSON      | **Newtonsoft.Json**                                               |

Não troque a stack (nada de Web API/MVC, EF Core, Dapper, React/Angular 2+, minimal API
etc.). O objetivo é justamente avaliar você **dentro destes padrões**.

---

## 🏗️ Como o projeto está organizado

Três projetos, espelhando a separação de camadas do Hubie:

```
src/
├── HubieTest.Dal/        → EF6 Database-First (EDMX), entidades e DbContext
├── HubieTest.Business/   → regras de negócio (Rules/) + acesso a dados (Data/) + segurança
└── HubieTest.Web/        → handlers .ashx + frontend AngularJS
```

**Fluxo de uma requisição** (igual ao Hubie):

```
AngularJS (apiService)                     .ashx                     Business            Dal (EF6)
   POST { method, data }   ───────►   AshxBase + safety   ───►   xxxBusiness   ───►   xxxDB ───► EDMX/SQL
   Authorization: Bearer            (valida o token JWT)       (regras)            (DbContext)
   ◄─────────────────────────────────  JSON (Newtonsoft)  ◄──────────────────────────────────
```

- Um único `.ashx` atende **várias operações**, despachadas pelo campo **`method`**
  (exatamente como o `process/ticket.ashx` do Hubie real).
- O **token JWT** vai no header `Authorization: Bearer <token>`. O `AshxBase` valida e
  injeta o **usuário logado** (id/profile/name) na camada de negócio — **nunca** confie em
  um id de usuário vindo do corpo da requisição.

---

## ✅ O que JÁ vem pronto (não precisa refazer)

Deixamos o "encanamento" e **um exemplo de referência ponta a ponta** para você ter um
norte do padrão esperado:

- 🔐 **Login completo** (`auth/starter.ashx` + `userBusiness` + JWT) — autentica os dois
  perfis e devolve o token.
- 🧱 **`AshxBase` + `safety`** — leitura de `method`/`data`, CORS e validação do token.
- 🧭 **Fatia de referência: Categories** — do dropdown no AngularJS
  (`apiService.listCategories`) até `categories.ashx → categoryBusiness → categoryDB →
  EF → JSON`. **Use isto como modelo.**
- 🎨 Frontend já configurado (módulo, rotas por perfil, layout, tela de login).

---

## 🎯 O que VOCÊ deve implementar

A fatia central do produto, **nas duas pontas (frontend + backend)**. Os pontos de extensão
já estão marcados com `// TODO` e com dicas no código.

### Backend (`.ashx` → `ticketBusiness` → `ticketDB` → EF)
1. **Abrir chamado** (`open`, requester): título, descrição e categoria; status inicial `OPEN`.
2. **Listar meus chamados** (`listMine`, requester).
3. **Fila de atendimento** (`listQueue`, agent), com filtro opcional por status.
4. **Detalhe do chamado** (`get`): cabeçalho + interações + anexos.
5. **Assumir chamado** (`assign`, agent): status → `IN_PROGRESS`, grava o agent.
6. **Alterar status** (`changeStatus`) respeitando transições válidas (ex.: `ANSWERED`, `CLOSED`).
7. **Interagir** (`addInteraction`, thread de mensagens) — **válido para os dois perfis**.
8. **Anexar arquivo** ao chamado (upload via `attachment.ashx`) e **listar/baixar** anexos —
   **disponível para os dois perfis**.

### Frontend (AngularJS)
9. **Requester**: abrir chamado (com upload), listar, ver detalhe, responder e anexar.
10. **Agent**: fila, abrir o chamado, assumir, responder, anexar e mudar status.

> O contrato dos `method` esperados pelo backend já está documentado em
> `ashx/process/ticket.ashx.cs`. Mantenha esses nomes para o front e o back conversarem.

---

## ⭐ O que pontua (e o que pontua MAIS)

**Esperado (base):**
- Funciona de ponta a ponta, dentro do padrão da stack.
- Camadas bem separadas (handler fino, regra no Business, dados no DB).
- Segurança: usa o usuário do **token**, valida permissão por **perfil**.
- Tratamento de erro razoável (não estoura exceção crua na tela).

**Diferenciais (sênior):**
- Validações de domínio e **transições de status** consistentes.
- Cuidado com EF (sem N+1 gritante, `AsNoTracking`/proxy desligado quando fizer sentido).
- Validação de upload (tamanho/extensão) e organização dos arquivos.
- UX simples, porém limpa; feedback de carregamento/erro.
- **Testes** (unitários nas regras de negócio já contam bastante).
- Commits pequenos e legíveis; um bom `README` seu explicando decisões.

Não precisa entregar tudo perfeito — preferimos **bem feito e coerente** a **muito e
quebrado**. Se faltar tempo, priorize a fatia: *open → handle → interact → attach*.

---

## ▶️ Como rodar

**Pré-requisitos:** Visual Studio 2022 (workload *ASP.NET e desenvolvimento web*),
.NET Framework 4.8, SQL Server (LocalDB já serve — vem com o VS).

1. **Banco**: siga o [`database/README.md`](database/README.md) (cria o banco `HubieTest` e
   roda `01_schema.sql` + `02_seed.sql`).
2. Abra **`src/HubieTest.sln`** no Visual Studio (ele restaura os pacotes NuGet
   automaticamente; ou rode `nuget restore src/HubieTest.sln`).
3. Confira a connection string `HubieContext` em `HubieTest.Web/Web.config` (por padrão
   aponta para `(localdb)\MSSQLLocalDB`).
4. Defina **`HubieTest.Web`** como projeto de inicialização e rode (IIS Express).
5. Acesse **`/index.html`**. Faça login com os usuários de teste:

   | Perfil    | Login       | Senha    |
   |-----------|-------------|----------|
   | Requester | `requester` | `123456` |
   | Agent     | `agent`     | `123456` |

> Se o frontend e o backend rodarem em portas/origens diferentes, ajuste
> `$sessionStorage.webServer` (o CORS já está liberado nos handlers).

---

## 📦 Entrega

⏱️ **Prazo: 3 dias úteis** a partir do recebimento deste teste.

1. Crie um **repositório público novo** (no GitHub/GitLab/Bitbucket) com o conteúdo deste
   projeto e desenvolva a partir daqui.
2. Commits incrementais (a gente gosta de ver o caminho, não só o destino).
3. No seu **README**, conte: o que fez, o que faltou, decisões e *trade-offs*, e como rodar
   algo que não seja óbvio.
4. Ao terminar, **envie o link do repositório público** para quem está conduzindo o
   processo.

> Não dá para entregar tudo em 3 dias? Sem problema — priorize a **qualidade da fatia
> vertical** (*open → handle → interact → attach*) e deixe claro no seu README o que ficou
> de fora e por quê. Preferimos **bem feito e coerente** a **muito e quebrado**.

Boa sorte — estamos torcendo por você! 🚀
