# Blazor-starter

.NET 8.0 Blazor based serverside web app for log work hours.

![License](https://img.shields.io/badge/License-Apache%20License%202.0-blue)
![Tests](https://img.shields.io/badge/dotnet%20version-8.0-blue)

## How to run

1. Create `appsettings.Release.json` file and fill with necessary params:
    ```
      {
         "ConnectionStrings": {
            "DefaultConnection": "DB_CONNECTION_STRING"
         },
         "Cors": {
            "AllowedOrigins": [
               "http://example.com",
               "http://another-origin.com"
            ]
         },
         "GoogleClientId": "YOUR_GOOGLE_CLIENT_ID",
         "Jwt": {
            "Key": "SecretKey",
            "Issuer": "appName",
            "Audience": "appUsers"
         }
      }
   ```

2. `docker-compose up --build -d`
3. First registered user in database gets `Admin` role