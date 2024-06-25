# Distributed Transactions in Microservice Architecture

Welcome to this demo project repository! This project contains three nested projects located in the root of the repository:

- **ChoreographySaga**
- **TransactionalOutbox**
- **TwoPhaseCommit**

These projects are designed to demonstrate different patterns in distributed systems and can be run using Docker and docker-compose. Follow the instructions below to get started.

## Prerequisites

Make sure you have the following software installed on your machine:

- [Docker](https://www.docker.com/products/docker-desktop)

## Running the Application with Docker

To run any of the nested projects using docker-compose, follow these steps:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/Milan-Kovacevic/distributed-microservice-transactions.git

2. **Position yourself in the required directory:**
   
   ```bash
   cd distributed-microservice-transactions
   
   # Then choose one of the demo project
   cd ChoreographySaga
   cd TransactionalOutbox
   cd TwoPhaseCommit

3. **Run the docker compose command:**

   ```bash
   docker-compose up -d

4. **Thats it :)**
