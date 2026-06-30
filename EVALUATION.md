# Rubrica de avaliação — uso interno do time

> ⚠️ **Documento interno.** Se for publicar o repositório do desafio, **remova este
> arquivo** (ou mantenha em um repositório/branch separado). Ele entrega o "gabarito" do
> que estamos olhando.

Sugestão de pesos (total 100). Ajuste conforme a senioridade real do candidato.

## 1. Arquitetura e aderência ao padrão — 25 pts
- [ ] Handler `.ashx` fino: só despacha por `method`, sem regra de negócio dentro.
- [ ] Regras no `ticketBusiness`; acesso a dados no `ticketDB` (não misturou).
- [ ] Seguiu a fatia de referência (categories) em vez de inventar outro estilo.
- [ ] Não vazou `DbContext`/entidades EF para o frontend de forma descuidada.

## 2. Backend / Entity Framework — 25 pts
- [ ] CRUD do ticket correto (create devolve o Id, update sem sobrescrever indevido).
- [ ] Interactions e attachments persistidos e listados corretamente.
- [ ] Uso consciente do EF (proxy/lazy desligados quando faz sentido; sem N+1 grotesco).
- [ ] Queries filtram pelo usuário/perfil correto.

## 3. Frontend AngularJS — 20 pts
- [ ] Usou `apiService` no padrão (`{ method, data }` + Bearer).
- [ ] Telas dos dois perfis funcionam (open, list, detail, handle, interact, attach).
- [ ] Estado de carregamento/erro tratado; navegação por perfil coerente.
- [ ] Upload de arquivo funcionando (multipart) e listagem/visualização do anexo.

## 4. Segurança — 15 pts
- [ ] Usa o usuário **do token** (não confia em id vindo do corpo).
- [ ] Respeita permissão por **perfil** (requester não acessa ações de agent e vice-versa).
- [ ] Validação de entrada; não derruba a aplicação com payload inválido.
- [ ] (Diferencial) validação de upload: tamanho/extensão/path seguro.

## 5. Qualidade geral — 15 pts
- [ ] Código limpo e legível; nomes coerentes; sem "código morto" demais.
- [ ] Commits incrementais e com mensagem útil.
- [ ] README do candidato explicando decisões, trade-offs e o que faltou.
- [ ] Tratamento de erros consistente (mensagens úteis, status HTTP coerentes).
- [ ] (Diferencial) testes unitários nas regras de negócio.

## 🚩 Sinais de alerta
- Trocou a stack (Web API, EF Core, Dapper, SPA moderna) — não seguiu o enunciado.
- Regras de negócio dentro do `.ashx` ou direto no controller AngularJS.
- Confia em `userId` vindo do front; sem checagem de perfil.
- Concatenação de SQL / qualquer brecha de injeção.
- "Funciona na minha máquina" sem instruções de setup.

## 💎 Sinais de excelência
- Transições de status modeladas com clareza (e bloqueadas quando inválidas).
- Camada de negócio testável e testada.
- Atenção a detalhes de EF/performance e a mensagens de erro amigáveis.
- README próprio bem escrito, com decisões e próximos passos.
