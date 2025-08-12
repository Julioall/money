# Documentação do Sistema Financeiro - ProfitPay

## Visão Geral

O **ProfitPay** é um sistema de gerenciamento financeiro pessoal e familiar que visa ajudar os usuários a controlar suas finanças de maneira eficaz. O sistema permite o gerenciamento de contas, cartões de crédito, receitas e despesas, proporcionando uma visão clara da situação financeira dos usuários.

## Estrutura das Classes

### 1. **Usuário (User)**
Representa o usuário do sistema. Possui as seguintes propriedades:
- **Id**: Identificador único do usuário.
- **Account**: Informações de autenticação e acesso.
- **Profile**: Dados pessoais do usuário.
- **Finance**: Informações financeiras, incluindo contas, cartões, categorias, transações e tags.

### 2. **Conta (Account)**
Classe que armazena informações sobre uma conta financeira. Propriedades:
- **Id**: Identificador único da conta.
- **AccountName**: Nome da conta.
- **Balance**: Saldo atual da conta.
- **HideBalance**: Indica se o saldo deve ser ocultado.
- **IconUrl**: URL do ícone da conta.

### 3. **Cartão de Crédito (CreditCard)**
Classe que controla as informações de um cartão de crédito. Propriedades:
- **Id**: Identificador único do cartão.
- **UserId**: Identificador do usuário proprietário.
- **CardName**: Nome do cartão.
- **Limit**: Limite de crédito do cartão.
- **ClosingDate**: Data de fechamento da fatura do cartão.
- **DueDate**: Data de vencimento da fatura do cartão.
- **PaymentAccountId**: ID da conta utilizada para pagamento.
- **AnnualFee**: Taxa anual do cartão (se aplicável).
- **IconUrl**: URL do ícone do cartão.

### 4. **Transação (Transaction)**
Classe que define as propriedades de uma transação financeira. Propriedades:
- **Id**: Identificador único da transação.
- **UserId**: Identificador do usuário que realizou a transação.
- **Amount**: Valor da transação.
- **CategoryId**: Categoria da transação.
- **Date**: Data da transação.
- **Description**: Descrição da transação.
- **Type**: Tipo de transação (despesa ou receita).
- **AccountDestinationId**: Conta associada à transação.
- **CreditCardDestinationId**: Cartão de crédito utilizado (se aplicável).
- **IsRecurring**: Indica se a transação é recorrente.
- **TotalInstallments**: Número total de parcelas (se aplicável).
- **CurrentInstallment**: Parcela atual (se aplicável).
- **InstallmentAmount**: Valor de cada parcela (se aplicável).
- **ParentTransactionId**: Referência à transação principal (se aplicável).
- **Tag**: Etiqueta opcional para categorização adicional.

## Fluxo de Dados

1. **Criação do Usuário**: Criação de um novo usuário e gerenciamento de suas contas e cartões de crédito.
2. **Gerenciamento de Contas**: Criação de contas que armazenam saldo e associam despesas e receitas.
3. **Adição de Cartões de Crédito**: Inclusão de cartões de crédito vinculados a contas específicas para rastreamento de gastos.
4. **Transações**: Registro de receitas e despesas, utilizando a classe de transação.
5. **Relatórios e Análises**: Geração de relatórios financeiros baseados nas transações registradas.

## Considerações Finais

O sistema **ProfitPay** foi projetado para ser extensível e fácil de usar, permitindo que os usuários gerenciem suas finanças de maneira eficiente.

## Recursos
https://www.flaticon.com/free-icon/wallet_9165728?term=wallet&related_id=9165728

