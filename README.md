# Cadastro API

## Sobre o Projeto

Este projeto foi desenvolvido como parte de uma avaliação técnica, com o objetivo de demonstrar conhecimentos em desenvolvimento de APIs utilizando o ecossistema .NET, aplicando boas práticas de arquitetura, organização de código e qualidade de software.

A aplicação foi desenvolvida com **ASP.NET Core 8**, seguindo conceitos de **API REST**, **Clean Architecture**, **DDD (Domain-Driven Design)**, **Orientação a Objetos**, **Injeção de Dependência** e **Testes Unitários**.

---

## Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- FluentValidation
- Serilog
- Swagger / OpenAPI
- JWT Authentication
- xUnit
- FluentAssertions

---

## Conceitos Aplicados

Durante o desenvolvimento foram utilizados os seguintes conceitos:

- API REST
- Clean Architecture
- Domain-Driven Design (DDD)
- Orientação a Objetos (OOP)
- Injeção de Dependência
- Repository Pattern
- Validação de Dados
- Tratamento Global de Exceções
- Logging
- Testes Unitários

---

## Banco de Dados

A aplicação utiliza **SQLite** para armazenamento dos dados.

Ao abrir o projeto **Lar.Avaliacao.Api**, verifique se o arquivo existe:

```text
Cadastro.db
```

está presente no projeto da API.

Caso o banco não exista ou seja necessário recriá-lo, recomenda-se executar o comando:

```powershell
Update-Database
```

### Executando a Migration

Antes de executar o comando, certifique-se de:

1. Definir o projeto **Lar.Avaliacao.infra** como projeto padrão das migrations.
2. Selecionar o projeto correto no **Package Manager Console**.
3. Executar:

```powershell
Update-Database
```

Esse comando criará automaticamente as tabelas necessárias no banco de dados SQLite caso elas ainda não existam.

> Observação: Assume-se que o avaliador possua conhecimentos básicos de Visual Studio e Entity Framework Core para execução das migrations.

---

## Documentação da API

A API possui documentação interativa através do **Swagger (OpenAPI)**.

Após iniciar a aplicação, acesse:

```text
https://localhost:7287/swagger/index.html
```

Através do Swagger é possível visualizar todos os endpoints disponíveis, suas respectivas entradas e saídas, além de realizar testes diretamente pela interface.

---

## Autenticação

A API utiliza autenticação baseada em **JWT (JSON Web Token)**.

### Fluxo recomendado

1. Realizar o cadastro de um usuário.
2. Efetuar o login.
3. Copiar o token JWT retornado.
4. No Swagger, clicar no botão **Authorize**.
5. Informar o token recebido.
6. Executar os endpoints protegidos.

Exemplo:

```text
{seu_token}
```

Após a autenticação, o Swagger enviará automaticamente o token nas requisições autorizadas.

---

## Logs da Aplicação

O projeto utiliza **Serilog** para registro de logs.

Os arquivos de log são armazenados na pasta dentro do projeto **Lar.Avaliacao.Api**:

```text
Logs/
```

Todos os eventos importantes, erros e informações de execução da aplicação são registrados nessa pasta para auxiliar no monitoramento e diagnóstico da aplicação.

---

## Validações

As validações das requisições são realizadas através do **FluentValidation**, garantindo que os dados enviados estejam de acordo com as regras de negócio antes de serem processados.

---

## Persistência

A persistência dos dados é realizada utilizando:

- Entity Framework Core
- SQLite

A camada de infraestrutura é responsável pelo acesso aos dados, mantendo o domínio desacoplado das tecnologias utilizadas.

---

## Estrutura da Solução

A solução foi organizada seguindo os princípios da Clean Architecture:

```text
src/
├── API
├── Application
├── Domain
└── Infrastructure

tests/
└── Tests
```

### Responsabilidades

- **API**: Controllers, configuração da aplicação e autenticação.
- **Application**: Casos de uso, DTOs, validações e regras de aplicação.
- **Domain**: Entidades e regras de negócio.
- **Infrastructure**: Persistência, repositórios e integrações externas.
- **Tests**: Testes unitários e validações.

---

## Como Executar

### Clonar o Repositório
```bash
git clone https://github.com/Douglasdossantos/Lar.Cadastro.git
```
### Minha preferencia seria que fosse executado pelo Visual studio
```bash
dotnet restore
```
### Aplicar Migrations (caso oarquivo de banco de dados não exista)
```powershell
Update-Database
```
 definir o projeto **Lar.Avaliacao.Api** como projeto de inicialização no Visual Studio.
---
## Executando os Testes

```bash
dotnet test
```
## Observações

Este projeto foi desenvolvido exclusivamente para fins de avaliação técnica, buscando demonstrar boas práticas de desenvolvimento de software, arquitetura de aplicações e utilização das principais ferramentas do ecossistema .NET.
