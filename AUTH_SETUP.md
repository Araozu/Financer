# ASP.NET Identity & JWT Authentication Setup

This application uses ASP.NET Identity for user management and JWT tokens for authentication.

## Features

- **ASP.NET Identity**: Full user management with Entity Framework Core
- **JWT Authentication**: Secure token-based authentication
- **Dual Token Support**: Access tokens (15 min) and refresh tokens (7 days)
- **Flexible Authentication**: Supports both Bearer tokens and secure cookies
- **Secure Cookies**: HttpOnly, Secure, SameSite=Strict

## Configuration

Update `appsettings.json` with your JWT settings:

```json
{
  "Jwt": {
    "Secret": "your-secret-key-here-minimum-32-characters-long!!!",
    "Issuer": "Financer",
    "Audience": "FinancerClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

## Authentication Endpoint

### POST /api/Auth

Login with email and password.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "YourPassword123"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-encoded-refresh-token"
}
```

The endpoint also sets tokens as secure HTTP-only cookies:
- `access_token`: JWT access token
- `refresh_token`: Refresh token for obtaining new access tokens

## Using Authentication

### Option 1: Bearer Token (Authorization Header)

```bash
curl -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  http://localhost:5000/api/protected-endpoint
```

### Option 2: Cookie-based (automatic)

Once logged in, cookies are automatically sent with subsequent requests to the same domain.

## Protected Endpoints

Add `[Authorize]` attribute to controllers or actions that require authentication:

```csharp
[Authorize]
[HttpGet("protected")]
public IActionResult ProtectedEndpoint()
{
    return Ok("This is protected!");
}
```

## Database Migration

Run the migration to create Identity tables:

```bash
cd Infrastructure
dotnet ef database update --startup-project ../Api
```
