FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update && apt-get install -y libfontconfig1 fontconfig fonts-liberation libc6 libc6-dev libgtk2.0-0 libnss3 libatk-bridge2.0-0 libx11-xcb1 libxcb-dri3-0 libdrm-common libgbm1 libasound2 libappindicator3-1 libxrender1 libfontconfig1 libxshmfence1
RUN chmod 777 .

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Work-logger/Work-logger.csproj", "Work-logger/"]
COPY ["WorkLogger.Domain/WorkLogger.Domain.csproj", "WorkLogger.Domain/"]
COPY ["WorkLogger.Infrastructure/WorkLogger.Infrastructure.csproj", "WorkLogger.Infrastructure/"]
COPY ["WorkLogger.Services/WorkLogger.Services.csproj", "WorkLogger.Services/"]
RUN dotnet restore "Work-logger/Work-logger.csproj"
COPY . .
WORKDIR "/src/Work-logger"
RUN dotnet build "Work-logger.csproj" -c Release -o /app/build

RUN dotnet dev-certs https -ep /cert/Work-logger.pfx -p pa55w_0Rd

FROM build AS publish
RUN dotnet publish "Work-logger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /cert/Work-logger.pfx /https/Work-logger.pfx
ENTRYPOINT ["dotnet", "Work-logger.dll"]
