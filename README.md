# Money
ProfitPay é uma aplicação de gerenciamento financeiro pessoal e familiar projetada para simplificar o controle de despesas e maximizar lucros nos investimentos. 

Os padrões de commit são essenciais para manter a clareza e a organização em projetos de desenvolvimento.
Um bom padrão de commit ajuda a equipe a entender rapidamente o que foi alterado, facilitando a manutenção e a colaboração.
Abaixo estão os tipos de commits comumente utilizados:
```
feat: Modelagem dos Objetos Principais da Aplicação 
- Criada a classe Account para representar as contas financeiras do usuário.
- Criada a classe CreditCard para gerenciar os cartões de crédito associados às contas.
- Criada a classe FinancialTransaction como base para as classes de despesas e receitas.
- Criada a classe TransactionCategory para categorizar transações financeiras, com propriedades para nome, ícone, descrição e tipo de categoria.
issue #1
```
**Importante:** O padrão de commit deve ser seguido pois, ao incluir a tag `issue #[número da issue]`, o commit é automaticamente atribuído à issue correspondente. Isso facilita o rastreamento de mudanças e a gestão de tarefas.

## Diretrizes para Implementação

1. **Abrir uma Issue:**
   - Sempre que um problema for identificado, crie uma issue no [projeto](https://github.com/users/Julioall/projects/7).
   - O título da issue deve ser claro e descritivo, e a descrição deve detalhar o problema e, se possível, a solução proposta.

2. **Criar um Branch:**
   - Após abrir a issue, crie um branch a partir dela. O nome do branch deve ser descritivo e pode seguir um padrão como `feat/nome-da-funcionalidade` ou `fix/nome-do-bug`.

3. **Realizar as Implementações:**
   - Execute as alterações necessárias no código seguindo as melhores práticas e padrões do projeto.

4. **Pull Request (PR):**
   - Após finalizar as implementações, crie um pull request (PR) direcionado ao branch master.

Seguindo essas diretrizes, garantimos um fluxo de trabalho organizado e eficiente, permitindo que a equipe mantenha a qualidade do código e colabore de forma eficaz.
