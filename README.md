# Blazor-starter

.NET 7.0 Blazor based serverside web app for log work hours.

![License](https://img.shields.io/badge/License-Apache%20License%202.0-blue)
![Tests](https://img.shields.io/badge/dotnet%20version-7.0-blue)

## How to run

1. Create `.env` file and fill with necessary params (where `xxxxxxxxxx` is your secrets):
    ```
        DB_CONNECTION_STRING=xxxxxxxxxx
        GOOGLE_OAUTH_CLIENT_ID=xxxxxxxxxx;
        GOOGLE_OAUTH_CLIENT_SECRET=xxxxxxxxxx;
   ```

2. `docker-compose up --build -d`
