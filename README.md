# Login Application (React + .NET Web API + SQL Server)

This project contains a beginner-friendly login system with JWT authentication.

## Folder Structure

```text
Login-Application/
├── dotnet-backend/
│   ├── Controllers/
│   │   └── AuthController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── DTOs/
│   │   ├── LoginDto.cs
│   │   └── RegisterDto.cs
│   ├── Models/
│   │   └── User.cs
│   ├── Repositories/
│   │   ├── IUserRepository.cs
│   │   └── UserRepository.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   └── IAuthService.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── LoginApi.csproj
└── react-frontend/
    ├── public/
    │   └── index.html
    ├── src/
    │   ├── components/
    │   │   ├── DashboardPage.js
    │   │   ├── LoginPage.js
    │   │   ├── ProtectedRoute.js
    │   │   └── RegisterPage.js
    │   ├── services/
    │   │   └── api.js
    │   ├── styles/
    │   │   └── theme.css
    │   ├── App.js
    │   └── index.js
    └── package.json
```

## JWT Configuration (.NET)

JWT is configured in `dotnet-backend/Program.cs` using `AddAuthentication().AddJwtBearer(...)`.
JWT settings are loaded from `dotnet-backend/appsettings.json`:

- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:ExpiryMinutes`

## Example SQL Table

```sql
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL
);
```

## Flow Explanation

### Register → Save to DB
1. React `RegisterPage` sends `{ username, password }` to `POST /api/auth/register`.
2. `AuthController` calls `IAuthService.RegisterAsync`.
3. `AuthService` checks existing user via `IUserRepository`.
4. Password is hashed using `PasswordHasher<User>`.
5. `UserRepository` adds user and saves to SQL Server through `AppDbContext`.

### Login → Generate JWT → Store in localStorage
1. React `LoginPage` sends `{ username, password }` to `POST /api/auth/login`.
2. `AuthService` verifies password hash.
3. If valid, `AuthService` generates JWT.
4. React stores JWT in `localStorage` under `token`.

### Access Protected Endpoint
1. React `api.js` interceptor reads `token` from `localStorage`.
2. Sends `Authorization: Bearer <token>` header automatically.
3. `.NET` endpoint `[Authorize] GET /api/auth/profile` validates JWT.
4. If token is valid, protected data is returned.

## Run Instructions

### Backend
```bash
cd dotnet-backend
dotnet restore
dotnet run
```

### Frontend
```bash
cd react-frontend
npm install
npm start
```

> Update API base URL in `react-frontend/src/services/api.js` if backend port is different.
