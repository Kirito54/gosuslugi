# Stage 1: сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/Client.Wasm

# Копируем только csproj из вложенной папки Client.Wasm/Client.Wasm
COPY Client.Wasm/Client.Wasm/Client.Wasm.csproj ./

# Восстанавливаем зависимости
RUN dotnet restore Client.Wasm.csproj

# Копируем весь код проекта
COPY Client.Wasm/Client.Wasm/ ./

# Публикуем в wwwroot
RUN dotnet publish Client.Wasm.csproj -c Release -o /app/publish

# Stage 2: рантайм через Nginx
FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

# Очистим дефолтный контент и скопируем собранный клиент
RUN rm -rf ./*
COPY --from=build /app/publish/wwwroot ./

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
