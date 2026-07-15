# Todoweb

A full-stack Todo application with a C# / ASP.NET Core Minimal API backend (EF Core + SQL Server) and a React (Vite) frontend.

## Demo
Demo of this project is available at: https://todoweb-tau.vercel.app/

Please note that due to the hosting service the backend may take a while to boot up. 

## Tech Stack

- **Backend:** ASP.NET Core Minimal API, Entity Framework Core, SQL Server
- **Frontend:** React + Vite
- **Testing:** xUnit + Moq

## Prerequisites

Make sure you have the following installed before starting:

- [.NET SDK](https://dotnet.microsoft.com/download) (version matching this project, e.g. .NET 10)
- [Node.js](https://nodejs.org/) (LTS version) and npm
- SQL Server — either:
  - **SQL Server Express** (with SQL Server Management Studio or Azure Data Studio for viewing/managing the database), or
  - **Docker**, running the `mcr.microsoft.com/mssql/server` image
- The [`dotnet-ef` tool](https://learn.microsoft.com/en-us/ef/core/cli/dotnet), installed globally:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Project Structure

```
Todoweb/
├──Todoweb/
  ├── Backend/           # C# backend source (models, repositories, services, endpoints)
  ├── Frontend-vite/      # React frontend
  └── Todoweb.csproj
└──Todoweb.Test
```

## 1. Clone the repository

```bash
git clone https://github.com/KKondor/Todoweb.git
cd Todoweb
```

## 2. Backend setup

### 2.1 Configure your database connection (User Secrets)

This project keeps connection strings out of `appsettings.json` using .NET User Secrets, so each developer can point at their own local database.

From the backend project folder (where `Todoweb.csproj` lives):

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultDB" "Server=localhost\SQLEXPRESS;Database=TodoDb;Trusted_Connection=True;TrustServerCertificate=True"
dotnet user-secrets set "ConnectionStrings:DefaultFrontEnd" "http://localhost:5173"
```

Adjust the `DefaultDB` connection string to match your own SQL Server instance name and authentication method (Windows Auth vs. SQL login). `DefaultFrontEnd` should match whatever origin your frontend dev server runs on (Vite's default is `http://localhost:5173`) — this is used for the CORS policy.

### 2.2 Apply database migrations

This creates the database schema (tables, columns, etc.) based on the current EF Core models.

```bash
cd Todoweb
dotnet ef database update
```

> If this fails inside Visual Studio's Package Manager Console with an unhelpful "Build failed" message, run it from a regular terminal instead — it gives more useful error output.

### 2.3 Run the backend

```bash
dotnet run
```

By default this starts the API on the port shown in the console output (check `Properties/launchSettings.json` for the exact port). Swagger UI is available at `/swagger` once running, useful for testing endpoints directly.

## 3. Frontend setup

From the `Frontend-vite` folder:

```bash
cd Frontend-vite
npm install
```

### 3.1 Configure the API URL

Create a `.env` file in `Frontend-vite/`:

```
VITE_API_URL=http://localhost:5000
```

Replace the URL/port with wherever your backend is actually running (see step 2.3).

### 3.2 Run the frontend

```bash
npm run dev
```

This starts the Vite dev server (default `http://localhost:5173`) and opens the app in your browser.

## Running Tests

Backend unit tests (xUnit + Moq) live in a separate test project. From the solution root:

```bash
dotnet test
```

## Troubleshooting

- **CORS errors in the browser console:** Confirm the `DefaultFrontEnd` User Secret matches your frontend's actual running origin exactly (protocol, host, and port).
- **"Invalid column name" SQL errors:** Usually means a migration hasn't been applied yet — run `dotnet ef database update`.
- **Certificate/TLS errors connecting to SQL Server:** Add `TrustServerCertificate=True` to your connection string (safe for local development).
- **`dotnet ef` command not found:** Install the tool globally with `dotnet tool install --global dotnet-ef`.
