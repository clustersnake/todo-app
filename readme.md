# Todo Management System

Todo application built with **ASP.NET Core (Web API)** and **React (Vite + TypeScript)**.


## Backend Setup (.NET 10)

### 1. Database Configuration

Rename `appsettings.Development.json` in the `Todo.API` project with your **PostgreSQL (Supabase)** connection string and add the ApiKey value

```json

"ConnectionStrings": {

"DefaultConnection": "Host=your_host;Database=your_db;Username=your_user;Password=your_password"

},
"ApiKeySettings": {

"ApiKey": "secret key"

}
```
### 2. Run migrations 
The project uses Entity Framework Core with a Seed system for Priority Levels. Run these commands from the **root folder** to set up the database:
```
$ dotnet ef migrations add InitialCreate --project backend/Todo.Infrastructure --startup-project backend/Todo.API
$ dotnet ef database update --project backend/Todo.Infrastructure --startup-project backend/Todo.API
```
### 3. Run the API
```
$ cd backend/Todo.API
$ dotnet run
```

## Frontend Setup

### 1. Install dependencies
```
$ cd frontend
$ yarn // or npm install
```
### 2. Run the frontend application
```
$ yarn dev
``` 
