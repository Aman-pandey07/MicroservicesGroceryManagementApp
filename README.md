# 🛍 Grocery Ordering System – Microservices Backend (.NET Core)

A scalable and modular **Grocery Ordering System** built using **Microservices Architecture** with **ASP.NET Core**, designed for seamless and secure ordering experience across distributed services.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Microservices](#microservices)
- [Key Features](#key-features)
- [Authentication & Authorization](#authentication--authorization)
- [Database Design](#database-design)
- [Inter-Service Communication](#inter-service-communication)
- [How to Run Locally](#how-to-run-locally)
- [Future Enhancements](#future-enhancements)
- [Folder Structure](#folder-structure)
- [Screenshots (Optional)](#screenshots-optional)

---

## 🧐 Overview

This backend system powers a **Grocery Ordering Platform**, enabling customers to browse products, place orders, and receive order notifications. It uses clean code principles and ensures high modularity, maintainability, and scalability by implementing **microservices with clear separation of concerns**.

---

## 🏗 Architecture

The solution is built with a **Microservices Architecture**, where each service operates independently and communicates with other services via **HTTP** and **RabbitMQ** for event-driven communication.

```
API Gateway (planned)
     |
 ┌──────────────┐
 |  UserService |
 └──────────────┘
        |
 ┌─────▼─────┐       ┌───────────────┐
 | ProductService |↔↔↔| OrderService |
 └──────────┘       └────────┘
        |                      |
 ┌─────▼─────┐        ┌────▼─────┐
 | NotificationService |<--------RabbitMQ
 └──────────────┘
```

---

## 🧰 Tech Stack

| Layer         | Technology                          |
|---------------|--------------------------------------|
| Language      | C# (.NET 8)                          |
| Framework     | ASP.NET Core Web API                 |
| Auth          | ASP.NET Core Identity + JWT          |
| DB            | SQL Server + Entity Framework Core   |
| Messaging     | RabbitMQ (Event Bus)                 |
| Architecture  | Clean Architecture, SOLID, DDD       |
| AuthZ         | Role-based Authorization (Admin/User)|
| Tools         | Swagger, Postman, Git                |

---

## 🔌 Microservices

Each service is independent with its own database and models.

1. **UserService** – User Registration, Login, JWT, Role management.
2. **ProductService** – Product CRUD, Inventory Management.
3. **OrderService** – Order placement, Quantity validation, Stock checks.
4. **NotificationService** – Sends order confirmation via events.

---

## ✨ Key Features

- ✅ **Microservices Architecture** for modular and independent development.
- 🔒 **JWT-based Authentication** with **role-based authorization**.
- ♻️ **RabbitMQ Integration** for asynchronous communication between services.
- ⚙️ **Real-time quantity validation** before order placement.
- 📦 **Entity Framework Core** with Code-First migrations.
- ↺ Clean **Repository + Service Layer** separation using Clean Architecture.
- 🧠 **DTOs** for consistent data transfer between services.
- 🛡 Secure endpoints using policies and roles (Admin, User).
- ⚙️ Easily extendable for **gRPC** or **API Gateway** integration.

---

## 🔐 Authentication & Authorization

- Auth system built using **ASP.NET Core Identity**.
- JWT Token is generated during login and used for accessing secured endpoints.
- Role-based access:
  - `Admin`: Can manage products.
  - `User`: Can place orders and view their history.

---

## 📃 Database Design

- **Code-First** approach with separate database per service.
- Entity Framework handles migrations.
- ProductService includes stock quantity tracking.
- OrderService cross-validates quantity during order placement.

---

## 🔗 Inter-Service Communication

| Type        | Tech Used     | Use Case                              |
|-------------|---------------|----------------------------------------|
| HTTP        | HttpClient    | Synchronous requests (e.g., fetch product) |
| Messaging   | RabbitMQ      | Asynchronous events (e.g., order placed)  |
| DTO Sharing | Internal Class Libraries | Ensures schema consistency |

---

## 🚀 How to Run Locally

1. Clone the repo:
   ```bash
   git clone https://github.com/your-username/grocery-ordering-system.git
   ```

2. Navigate to the solution directory and run:
   ```bash
   dotnet build
   dotnet run --project FacelessGrocery.API
   ```

3. Setup SQL Server and update connection strings in `appsettings.json`.

4. Apply Migrations (for each service):
   ```bash
   dotnet ef database update --project FacelessGrocery.ProductService
   ```

5. Start RabbitMQ locally or use Docker:
   ```bash
   docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 rabbitmq
   ```

6. Use Swagger/Postman to test the APIs.

---

## 🔮 Future Enhancements

- 🧹 API Gateway using Ocelot/YARP.
- 📊 Health Checks and Monitoring (e.g., Prometheus + Grafana).
- 📧 Email and SMS Integration in NotificationService.
- 🛒 Admin Dashboard UI using Angular/React.
- ⚡ Add **gRPC** or **Kafka** for faster communication.
- 🧰 Unit + Integration Testing.

---

## 📁 Folder Structure (Simplified)

```
/GroceryOrderingSystem
│
├── /UserService
│   ├── Controllers
│   ├── DTOs
│   ├── Services
│   └── Data (DbContext, Migrations)
│
├── /ProductService
├── /OrderService
├── /NotificationService
├── /SharedLibraries (Common DTOs/Utilities)
├── /FacelessGrocery.API (Startup + Config)
└── README.md
```

---

## 📸 Screenshots (Optional)

> You can add Postman screenshots or Swagger UI if needed here.

---

## 🤝 Contributing

This project is for learning purposes, but contributions are welcome. Fork the repo, make changes, and raise a PR.

---

## 🧑‍💻 Author

**Aman Arvind Pandey**  
🔗 [LinkedIn](https://www.linkedin.com/in/LinkedIn)  
💻 [GitHub](https://github.com/Aman-pandey07)

---

## 📄 License

This project is licensed under the MIT License.

