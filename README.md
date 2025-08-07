## OSPI Engine: Open Source Payment Institution

### 📖 Sobre o Projeto

O **OSPI Engine** é um MVP experimental de microsserviço back-end desenvolvido em **.NET 8** focado, inicialmente, no processamento de pagamentos via Pix. Sua principal função é orquestrar a lógica de débito e crédito em transferências internas, garantindo a integridade transacional e a comunicação assíncrona com outros serviços através de uma arquitetura robusta e escalável.

Este projeto foi construído para demonstrar a aplicação de padrões de arquitetura modernos, boas práticas de desenvolvimento e a criação de um sistema resiliente e observável.

#### Planos Futuros

A intenção é expandir este microsserviço para suportar outros métodos de pagamento, como cartões de crédito, boletos e integrações com gateways de pagamento. Além disso, planejo implementar funcionalidades adicionais, como:
  * Suporte a múltiplas moedas e conversão cambial.
  * Integração com sistemas antifraude.
  * Implementação de um sistema de notificações para eventos de pagamento.
  * Dashboard administrativo para monitoramento e gestão de transações.
  * Suporte a reembolsos e estornos.
  * Implementação de testes automatizados (unitários, integração e end-to-end).
  * Melhoria contínua da observabilidade com métricas, logs estruturados e tracing distribuído.
  * Implementação de autenticação e autorização robusta para acesso às APIs.
  * Documentação completa da API utilizando Swagger/OpenAPI.
  * Implementação de um sistema de fila para gerenciar picos de demanda e garantir a entrega das mensagens.

Tornando no futuro uma plataforma completa e aberta para instituições financeiras e desenvolvedores que desejam construir soluções de pagamento inovadoras.

### ⚙️ Arquitetura e Padrões de Design

O projeto foi estruturado com foco em desacoplamento, testabilidade e manutenibilidade. As seguintes abordagens foram aplicadas:

  * **Arquitetura Limpa (Clean Architecture):** As responsabilidades do projeto são divididas em camadas bem definidas (`Domain`, `Application`, `Infrastructure`, `Presentation`), com um fluxo de dependência que aponta sempre para o núcleo da aplicação.
  * **Padrão Mediator:** O fluxo de requisições é gerenciado por uma camada de **Use Cases** que orquestra a lógica de negócio, seguindo o Princípio da Única Responsabilidade (SRP).
  * **Padrão Unit of Work:** Garante que todas as operações de banco de dados relacionadas a uma única transação de negócio sejam tratadas como uma unidade, mantendo a consistência e integridade dos dados.
  * **Padrão Result:** Operações que podem falhar retornam um objeto `Result<T>` em vez de lançar exceções. Isso proporciona um controle de fluxo de erro explícito, claro e previsível.
  * **Mensageria Assíncrona (MessageBroker):** A comunicação entre as etapas de débito e crédito é feita via **Kafka**, garantindo resiliência e desacoplamento do fluxo transacional.
  * **Padrão Strategy (nos Consumidores Kafka):** A lógica de consumo de mensagens do Kafka é desacoplada da lógica de negócio do evento. Isso permite que a mesma classe de consumidor genérico seja reutilizada para processar diferentes tipos de eventos, promovendo a reutilização de código e aderindo ao Princípio da Responsabilidade Única (SRP).
  * **Idempotência:** Mecanismos foram implementados para garantir que o processamento de eventos do Kafka seja seguro para ser executado múltiplas vezes sem causar efeitos colaterais indesejados.

### 🚀 Tecnologias Utilizadas

  * **.NET 8:** Framework de desenvolvimento
  * **C\# 12:** Linguagem de programação
  * **Entity Framework Core:** ORM (Object-Relational Mapper)
  * **PostgreSQL:** Banco de dados relacional
  * **Apache Kafka:** Message broker para comunicação assíncrona
  * **Docker e Docker Compose:** Para orquestração do ambiente de desenvolvimento
  * **Serilog:** Para logging estruturado

### 📦 Como Executar o Projeto

Para iniciar o ambiente de desenvolvimento, incluindo o Kafka e o Zookeeper, siga os passos abaixo:

1.  Clone o repositório: `git clone https://github.com/soumarcelo/ospi.git`
2.  Navegue até a pasta raiz do projeto: `cd engine`
3.  Execute o Docker Compose para subir os serviços:
    ```bash
    docker-compose up --build
    ```
4.  O microsserviço estará disponível em `http://localhost:5000`.

### ✅ To-Do List

O projeto está em constante evolução. Abaixo estão algumas das próximas etapas planejadas para a sua conclusão:

  * [ ] Implementar o processo completo de crédito após a transação de débito.
  * [ ] Adicionar autenticação e autorização para as rotas da API.
  * [ ] Criar testes de integração e de unidade para as camadas de `Application` e `Infrastructure`.
  * [ ] Configurar a observabilidade completa com métricas e traces (OpenTelemetry).
  * [ ] Adicionar documentação da API com Swagger/OpenAPI.
  * [ ] Finalizar a implementação do repositório de `Outbox`.
  * [ ] Implementar a lógica de idempotência em mais pontos do fluxo.
  * [ ] Melhorar mensagens de erro e logging.
  * [ ] Melhorar validações de Idempotência.

### 🙋 Contribuições

Contribuições, sugestões e feedbacks são bem-vindos\! Se você encontrar um bug ou tiver uma ideia para melhorar o projeto, sinta-se à vontade para abrir uma *issue* ou um *pull request*.