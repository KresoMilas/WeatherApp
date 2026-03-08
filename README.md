# WeatherApp

A full-stack weather forecast web application built with **ASP.NET Core (.NET 9)**, **React 19 (Vite + TypeScript)**, **PostgreSQL**, and **Docker**.

---

## Features

- JWT-based user authentication (register & login)
- 5-day weather forecast with interactive filtering and charts
- City search history and statistics per user
- Responsive, modern UI built with React + TypeScript

---

## Tech Stack

| Layer     | Technology                          |
|-----------|-------------------------------------|
| Backend   | ASP.NET Core (.NET 9), Entity Framework Core |
| Frontend  | React 19, TypeScript, Vite          |
| Database  | PostgreSQL (via Docker)             |
| Auth      | JWT (JSON Web Tokens)               |
| Weather   | OpenWeatherMap API                  |

---

## Prerequisites

Make sure you have the following installed before starting:

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js LTS](https://nodejs.org/) + npm
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- (Optional) [nvm-windows](https://github.com/coreybutler/nvm-windows) for Node version management

---

## 1. Clone the Repository

```sh
git clone <your-repo-url>
cd WeatherApp
```

---

## 2. Environment Variables & Secrets

This project uses environment variables so that no secrets are committed to git.

**Set the following environment variables on your machine:**

```cmd
setx ConnectionStrings__DefaultConnection "Host=localhost;Port=5432;Database=weatherapp;Username=postgres;Password=yourStrongDbPassword"
setx Jwt__Key "yourSuperSecretJwtKey"
setx OpenWeather__ApiKey "yourOpenWeatherApiKey"
```

> ASP.NET Core automatically maps environment variables using `__` as a separator for nested config keys (e.g. `Jwt__Key` maps to `Jwt:Key` in appsettings).
> After running `setx`, restart your terminal and IDE for the variables to take effect.

Alternatively, create `backend/WeatherApp.API/appsettings.Development.json`** (gitignored):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=weatherapp;Username=postgres;Password=yourStrongDbPassword"
  },
  "Jwt": {
    "Key": "yourSuperSecretJwtKey",
    "Issuer": "WeatherApp",
    "Audience": "WeatherAppUsers"
  },
  "OpenWeather": {
    "ApiKey": "yourOpenWeatherApiKey",
    "BaseUrl": "https://api.openweathermap.org/data/2.5"
  }
}
```

Get a free OpenWeatherMap API key at: https://openweathermap.org/api

---

## 3. Start PostgreSQL with Docker

From the root of the project (where `docker-compose.yml` is located):

```sh
docker compose up -d
```

Verify the container is running:

```sh
docker compose ps
```

You should see the PostgreSQL container with status **Up** on port **5432**.

To stop the database:

```sh
docker compose down
```

> ⚠️ To stop and also delete all database data (full reset):
> ```sh
> docker compose down -v
> ```

---

## 4. Backend Setup (ASP.NET Core)

```sh
cd backend/WeatherApp.API
```

Restore dependencies and build:

```sh
dotnet restore
dotnet build
```

Apply database migrations (creates all tables):

```sh
dotnet ef database update
```

> If `dotnet ef` is not found, install it globally first:
> ```sh
> dotnet tool install --global dotnet-ef
> ```

Run the backend:

```sh
dotnet run
```

The API will be available at: `http://localhost:5069`  
Swagger UI (API docs): `http://localhost:5069/swagger`

---

## 5. Frontend Setup (React + Vite)

```sh
cd frontend
```

Install dependencies:

```sh
npm install
```

Start the development server:

```sh
npm run dev
```

The frontend will be available at: `http://localhost:5173`

---

## 6. Usage

1. Open `http://localhost:5173` in your browser
2. Register a new account or log in
3. Search for any city to view its 5-day weather forecast
4. Use filters and charts to explore weather data
5. View your personal search history and statistics

---

## 7. Project Structure

```
WeatherApp/
├── backend/
│   └── WeatherApp.API/         # ASP.NET Core API
│       ├── Controllers/
│       ├── Services/
│       ├── Models/
│       ├── appsettings.json            # Non-sensitive config (committed)
│       └── appsettings.Development.json  # Secrets (gitignored)
├── frontend/                   # React + Vite + TypeScript
│   ├── src/
│   └── vite.config.ts
├── docker-compose.yml          # PostgreSQL container config
├── .env                        # Secrets for Docker (gitignored)
└── README.md
```

---

## License

MIT
