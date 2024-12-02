# Documentação do Sistema Financeiro - ProfitPay

## Visão Geral

O **ProfitPay** é um sistema de gerenciamento financeiro pessoal e familiar que visa ajudar os usuários a controlar suas finanças de maneira eficaz. O sistema permite o gerenciamento de contas, cartões de crédito, receitas e despesas, proporcionando uma visão clara da situação financeira dos usuários.

## Estrutura das Classes

### 1. **Usuário (User)**
Gerencia as IDs das contas e cartões de crédito associados ao usuário.

### 2. **Conta (Account)**
Classe que armazena informações sobre uma conta financeira. Inclui informações como:
- **Id**: Identificador único da conta.
- **UserId**: Identificador do usuário que possui a conta.
- **AccountName**: Nome da conta.
- **Balance**: Saldo atual da conta.
- **HideBalance**: Indica se o saldo deve ser ocultado.
- **IconUrl**: URL do ícone da conta.
- **ExpenseIds**: Lista de IDs de despesas associadas à conta.
- **IncomeIds**: Lista de IDs de receitas associadas à conta.

### 3. **Cartão de Crédito (CreditCard)**
Classe que controla as informações de um cartão de crédito. Inclui informações como:
- **Id**: Identificador único do cartão.
- **Name**: Nome do cartão.
- **IconUrl**: URL do ícone do cartão.
- **Limit**: Limite de crédito do cartão.
- **ClosingDate**: Data de fechamento da fatura do cartão.
- **DueDate**: Data de vencimento da fatura do cartão.
- **PaymentAccountId**: ID da conta associada ao pagamento do cartão.

### 4. **Transação Financeira (FinancialTransaction)**
Classe que define as propriedades básicas de uma transação financeira, compartilhadas entre despesas e receitas. Inclui informações como:
- **Id**: Identificador único da transação.
- **UserId**: Identificador do usuário que realizou a transação.
- **Amount**: Valor da transação.
- **CategoryId**: Categoria da transação.
- **Date**: Data da transação.
- **Description**: Descrição da transação.
- **AccountDestinationId**: Conta associada à transação.
- **Type**: Tipo de transação (despesa ou receita).
- **CreditCardDestinationId**: Cartão de crédito utilizado (se aplicável).
- **IsRecurring**: Indica se a transação é recorrente.
- **TotalInstallments**: Número total de parcelas (se aplicável).
- **CurrentInstallment**: Parcela atual (se aplicável).
- **InstallmentAmount**: Valor de cada parcela (se aplicável).
- **ParentTransactionId**: Referência a transações maiores (se aplicável).



## Fluxo de Dados

1. **Criação do Usuário**: Criação de um novo usuário e gerenciamento de suas contas e cartões de crédito.
2. **Gerenciamento de Contas**: Criação de contas que armazenam saldo e associam despesas e receitas.
3. **Adição de Cartões de Crédito**: Inclusão de cartões de crédito vinculados a contas específicas para rastreamento de gastos.
4. **Transações Financeiras**: Registro de receitas e despesas, utilizando as classes correspondentes.
5. **Relatórios e Análises**: Geração de relatórios financeiros baseados nas transações registradas.

## Considerações Finais

O sistema **ProfitPay** foi projetado para ser extensível e fácil de usar, permitindo que os usuários gerenciem suas finanças de maneira eficiente.

## Recursos
https://www.flaticon.com/free-icon/wallet_9165728?term=wallet&related_id=9165728
