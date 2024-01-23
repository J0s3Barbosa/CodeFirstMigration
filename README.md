# Code First approach

technique which helps us to create a database, migrate and maintain the database and its tables from the code. From the code, means that you directly maintain the database and its corresponding tables from the .NET Code. It is helpful when you don’t have a database ready and you want to start working with new fresh project and want to create a database and maintain the database directly from your code.

![Project Logo](logo.png) <!-- Add your project logo if applicable -->

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Docker Deployment](#docker-deployment)
    - [Prerequisites](#prerequisites-1)
    - [Build the Docker Images](#build-the-docker-images)
    - [Deploy with Docker Compose](#deploy-with-docker-compose)
- [Accessing the Web Service](#accessing-the-web-service)
- [Cleaning Up](#cleaning-up)
- [Documentation and Code Quality](#documentation-and-code-quality)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)

## Features

- **Technology Stack:**
  - Language: C#
  - Framework: .NET Core 7
  - Frontend: Angular v15 with Angular Material
  - Database: SQL server
  - Containerization: Docker and Docker Compose
  - Message Queue: RabbitMQ (optional)
  - Cache: Redis
  - Tests: Unit tests(xunit, Moq), Integration tests

- **Local Development:**
  The service is designed to run locally without any external dependencies, making it easy to set up and test.

- **Cache Layer:**
  A cache layer is thoughtfully implemented to optimize performance by reducing data retrieval operations.

- **Architectural Design:**
  Our project boasts a meticulously designed architecture that includes the selection of components and services to ensure scalability and maintainability.

- **Code Organization and Structure:**
  The codebase adheres to industry best practices for code organization and structure, ensuring a smooth development experience.

- **Documentation and Code Quality:**
  We take pride in our extensive documentation, explaining the project's architecture, components, and development practices. The codebase follows impeccable coding standards, ensuring maintainability and reliability.

- **Commit History and Version Control:**
  We maintain a pristine and well-organized commit history, following best version control practices, including atomic commits and meaningful commit messages.

- **Best Coding Practices:**
  Our codebase adheres to industry-standard coding practices, including naming conventions, error handling, testing, and security measures.

## Getting Started

Let's get you started with the Awesome Web Service!

### Prerequisites

Before you dive in, ensure you have the following installed:

- [.NET Core 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Angular CLI](https://angular.io/guide/setup-local)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/J0s3Barbosa/CodeFirstMigration.git
   ```
2. Set up the backend:
  ```bash
cd Backend
dotnet restore
dotnet build
dotnet run
   ```

3. Set up the frontend:

  ```bash
cd Frontend
npm install
ng serve
   ```



