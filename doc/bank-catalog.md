# Catálogo de instituições financeiras — Money

A tela `/settings/accounts` usa o projeto [logos-bancos-br](https://github.com/rzmt/logos-bancos-br) para pesquisar instituições financeiras por nome popular, nome oficial, código COMPE e ISPB.

## Integração

- **Fonte:** CDN oficial do pacote npm `logos-bancos-br`, versão **0.8.4** fixada em `Application.Client/src/app/core/services/banks/bank-catalog.service.ts`.
- **Metadados:** `data/bancos.json` (bancos com COMPE) e `data/instituicoes-pix.json` (instituições Pix sem COMPE).
- **Imagens:** caminhos `logo.svg` / `logo.png` fornecidos no dataset, servidos pelo mesmo CDN versionado. Cooperativas podem compartilhar o mesmo logo; não assuma que o nome do arquivo corresponde ao ISPB exibido.
- **Busca:** local, em memória, sem consultas remotas a cada tecla digitada. O navegador armazena em cache os arquivos do CDN.
- **Privacidade:** os metadados são baixados com `fetch(..., { credentials: 'omit' })` para não enviar o JWT do Money. O interceptor Angular atual adiciona `Authorization` a chamadas `HttpClient`; por isso **não** usar `HttpClient` para consultas de terceiros.
- **Sem dependência npm:** a integração via CDN é oficialmente documentada pelo projeto; não é preciso copiar arquivos de logos ao repositório nem modificar `package-lock.json`.

## Persistência

O modelo `Account` continua guardando `AccountName`, `BankName`, `AccountType`, `Balance` e `HideBalance`, além de `BankIspb` (opcional, 8 dígitos).

A interface deriva o logo pelo ISPB usando o catálogo. Para compatibilidade com a API atual, o campo `IconUrl` guarda a URL do CDN, quando existir, ou o marcador `bank-default`. Quando não houver logo ou ele falhar, a interface exibe um ícone neutro.

## Contas legadas

As contas que usam `img/logo-123.svg` continuam persistidas e editáveis, mas **esses arquivos antigos foram removidos**. A interface não requisita esses caminhos: exibe um ícone padrão e permite escolher a instituição correta novamente, sem inventar uma correspondência entre o antigo número do arquivo e um ISPB.

Não inferir ISPB por similaridade aproximada do nome: identificar incorretamente um banco é pior que exibir um ícone genérico.

## Atualizações

A versão do CDN é fixa para reproduzibilidade. Para atualizar o catálogo, alterar a versão fixada no serviço e testar novamente busca, logotipos e nomes das instituições. Em caso de indisponibilidade do CDN, o formulário permite cadastro manual, sem ISPB.

Os logotipos são marcas das respectivas instituições; a licença MIT do código e dos dados do pacote não transfere os direitos sobre essas marcas. Consulte o `DISCLAIMER.md` do projeto de origem.
