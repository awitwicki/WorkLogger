FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# QuestPDF is based in SkiaSharp, which requires some extra dependencies
# libfontconfig1 and fontconfig packages are required for SkiaSharp to work
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update && apt-get install -y libfontconfig1 fontconfig fonts-liberation libc6 libc6-dev libgtk2.0-0 libnss3 libatk-bridge2.0-0 libx11-xcb1 libxcb-dri3-0 libdrm-common libgbm1 libasound2 libappindicator3-1 libxrender1 libfontconfig1 libxshmfence1
RUN chmod 777 .

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Required for making self signed cert
RUN apt-get update && apt-get install -y openssl

WORKDIR /src
COPY ["Work-logger/Work-logger.csproj", "Work-logger/"]
COPY ["WorkLogger.Domain/WorkLogger.Domain.csproj", "WorkLogger.Domain/"]
COPY ["WorkLogger.Infrastructure/WorkLogger.Infrastructure.csproj", "WorkLogger.Infrastructure/"]
COPY ["WorkLogger.Services/WorkLogger.Services.csproj", "WorkLogger.Services/"]
COPY ["WorkLogger.Common/WorkLogger.Common.csproj", "WorkLogger.Common/"]
RUN dotnet restore "Work-logger/Work-logger.csproj"
COPY . .
WORKDIR "/src/Work-logger"
RUN dotnet build "Work-logger.csproj" -c Release -o /app/build

# Make self signed cert for 10 years
RUN openssl req -newkey rsa:2048 -nodes -keyout server.key -x509 -days 3650 -out server.crt -subj "/C=EU/ST=Kodzustate/L=City/O=Kv/CN=kv.io"
RUN openssl pkcs12 -export -out Work-logger.pfx -inkey server.key -in server.crt -password pass:pa55w_0Rd


FROM build AS publish
RUN dotnet publish "Work-logger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Move self signed cert to app
COPY --from=build /src/Work-logger/Work-logger.pfx /https/

ENTRYPOINT ["dotnet", "Work-logger.dll"]
