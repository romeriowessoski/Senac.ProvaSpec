# Arquitetura

## Visão geral

A aplicação é organizada em três camadas, com fluxo de chamadas da API para Application e desta para Domain:

```text
Cliente Web
	↓
API
	↓
Application
	↓
Domain
```

As dependências apontam para dentro: Domain não depende de Application nem de API. Application coordena os casos de uso sem depender de detalhes de transporte ou apresentação. API é a camada externa de entrada e saída.

## Responsabilidades das camadas

### API

- Receber requisições e produzir respostas para os clientes.
- Converter contratos externos em chamadas da camada Application.
- Traduzir resultados e falhas da Application em respostas apropriadas.
- Não implementar regras de negócio nem acessar diretamente a persistência.

### Application

- Orquestrar os casos de uso da aplicação.
- Coordenar o acesso ao Domain e às abstrações necessárias, como repositórios.
- Não conter detalhes de HTTP, interface ou tecnologia de persistência.

### Domain

- Conter o modelo central da aplicação e suas regras.
- Proteger as invariantes do modelo.
- Permanecer independente das camadas externas e de infraestrutura.

## Fluxo de uma operação

1. O cliente envia uma requisição à API.
2. A API valida o contrato de transporte e encaminha a operação à Application.
3. A Application coordena o caso de uso usando o modelo do Domain e, quando necessário, abstrações de acesso a dados.
4. O resultado retorna à API, que o converte em resposta ao cliente.

## Persistência e integrações

A camada Application deve depender de abstrações para acesso a dados ou serviços externos. As implementações concretas dessas abstrações ficam fora do Domain e são conectadas à aplicação pela camada externa apropriada. A tecnologia de persistência e detalhes de infraestrutura são definidos separadamente.

## Organização lógica sugerida

```text
src/
  Senac.ProvaSpec.Api/
  Senac.ProvaSpec.Application/
  Senac.ProvaSpec.Domain/
```

Cada camada pode ser implementada como projeto separado ou como módulo claramente delimitado. Em ambos os casos, devem ser preservadas as responsabilidades e a direção de dependências descritas neste documento.
