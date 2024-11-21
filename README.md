# ToDoList ASP.NET Core MVC Application

This repository contains a simple ASP.NET Core MVC application backed by a PostgreSQL database, orchestrated using Docker Compose.

## Prerequisites

Before running this application, ensure you have the following installed on your system:

- Docker
- Docker Compose

## Setup Instructions

### 1. Clone the Repository

Clone this repository to your local machine:

```bash
git clone https://github.com/damiantasevski/todolist.git
cd todolist
```
### 2. Docker Compose Configuration (you can skip this step)
Key Configuration:
- The **app** service exposes two ports: **8080** and **8081**.
- The **postgres** service is configured with the following credentials:
    -  **User**: ToDoListUser
    - **Password**: yourpassword
    - **Database**: ToDoList

Persistent data storage for PostgreSQL is managed using the **postgres-data** volume

### 3. Environment Variables (you can skip this step)
The application uses the following environment variables, defined in the **docker-compose.yml** file:
- **ASPNETCORE_ENVIRONMENT**: Set to **Development** for local development.
- **ConnectionStrings__DefaultConnection**: Connection string to the PostgreSQL database.

### 4. Build and Run the Application
To build and run the application with Docker Compose, execute:
```bash
sudo docker compose up -d
```

### 5. Access the Application
Once the services are running:
- The web application will be accessible at http://localhost:8080
- The PostgreSQL database is available at localhost:5432

### 6. Stopping the Application
To stop and remove the running containers, use:
```bash
sudo docker compose down
```
