# Como Subir as Aplicações usando Docker

Este guia explica como subir as aplicações SensorWeb e SensorApi utilizando Docker.

## Pré-requisitos

Certifique-se de ter o Docker instalado em sua máquina. Você pode baixar e instalar o Docker a partir do [site oficial do Docker](https://www.docker.com/get-started).

## Passo a Passo

1. Clone este repositório para sua máquina:

    ```bash
    git clone https://github.com/IOTNestAdmin/SensorWeb.git
    ```

2. Navegue até o diretório raiz do projeto:

    ```bash
    cd SensorWeb
    ```

3. Construa as imagens Docker para SensorWeb e SensorApi:

    ```bash
    docker build -t sensorweb -f Dockerfile.sensorweb .
    docker build -t sensorapi -f Dockerfile.sensorapi .
    ```

4. Execute os contêineres:

    ```bash
    docker run -d -p 8090:80 sensorweb
    docker run -d -p 8081:80 sensorapi
    ```

5. Agora, você pode acessar suas aplicações:

    - SensorWeb: [http://localhost:8090](http://localhost:8090)
    - SensorApi: [http://localhost:8081](http://localhost:8081)


