# Stack tecnológica

## Stack proposta

- **Linguagem e plataforma:** C# com .NET 10 (`net10.0`).
- **Aplicação web:** ASP.NET Core 10, usando o SDK `Microsoft.NET.Sdk.Web`.
- **Interface:** Razor Pages com HTML e CSS. Não é necessário um framework JavaScript separado para esta aplicação; JavaScript pode ser adicionado pontualmente se surgir uma necessidade de interação no navegador.
- **Persistência:** SQLite, em arquivo local, sem necessidade de instalar ou manter um servidor de banco de dados.
- **Acesso a dados:** Entity Framework Core 10, com o provedor `Microsoft.EntityFrameworkCore.Sqlite`.
- **Hospedagem inicial:** Kestrel, servidor web incluído no ASP.NET Core, acessado localmente pelo navegador. Hospedagem externa não faz parte deste escopo.

Essa combinação mantém a interface e o backend no ecossistema .NET e é suficiente para desenvolver e executar a aplicação localmente, com poucas dependências externas.

## Pacotes e ferramentas

### Aplicação

- `Microsoft.EntityFrameworkCore.Sqlite` — integração do Entity Framework Core com SQLite.
- `Microsoft.EntityFrameworkCore.Design` — suporte de design para criação e aplicação de migrations durante o desenvolvimento.

O ASP.NET Core e o Kestrel são fornecidos pelo framework compartilhado .NET; não é necessário adicionar um pacote separado para eles. Os pacotes do Entity Framework Core devem usar versões compatíveis com o .NET 10.

### Desenvolvimento e testes

- **.NET 10 SDK** — restauração, compilação e execução em ambiente de desenvolvimento.
- **Ferramenta `dotnet-ef` 10.x** — gerenciamento de migrations, quando usado pelo projeto.
- **xUnit e `Microsoft.NET.Test.Sdk`** — opções recomendadas para testes automatizados; são dependências do projeto de testes, não necessárias para executar a aplicação.
- Um navegador web moderno para acessar a interface.

## Execução local

No ambiente de desenvolvimento, com o .NET 10 SDK instalado:

1. Restaurar os pacotes NuGet com `dotnet restore`.
2. Aplicar as migrations do Entity Framework Core para criar/atualizar o banco SQLite, quando houver migrations configuradas.
3. Executar com `dotnet run` e abrir no navegador o endereço local informado pelo ASP.NET Core.

O arquivo do banco SQLite é armazenado localmente. Não é necessário instalar um serviço de banco de dados. Para executar uma publicação dependente do framework em outra máquina, ela deve ter instalado o runtime ASP.NET Core 10; uma publicação independente pode incluir o runtime.

## Estado atual do repositório

O projeto existente ainda usa `Microsoft.NET.Sdk`, `OutputType` `Exe` e não declara ASP.NET Core ou os pacotes de persistência. Portanto, as tecnologias acima são a stack proposta para a aplicação web e precisam ser incorporadas ao projeto antes que os comandos de execução descritos sejam aplicáveis.

Decisões de arquitetura e regras de negócio são documentadas em arquivos próprios, não neste documento.
