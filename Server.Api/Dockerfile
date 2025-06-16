# Stage 1: сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Открываем рабочую директорию сразу на уровень проекта
WORKDIR /src/Server.Api

# Копируем только csproj (он лежит в вложенной папке Server.Api/Server.Api)
COPY Server.Api/Server.Api/Server.Api.csproj ./

# Восстанавливаем зависимости
RUN dotnet restore Server.Api.csproj

# Копируем всё содержимое проекта (только файлы из Server.Api/Server.Api)
COPY Server.Api/Server.Api/ ./

# Публикуем в папку /app/publish
RUN dotnet publish Server.Api.csproj -c Release -o /app/publish

# Stage 2: рантайм
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем результат публикации
COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80

ENTRYPOINT ["dotnet", "Server.Api.dll"]
