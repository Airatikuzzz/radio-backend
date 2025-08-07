# Этап 1: Сборка фронтенда (Node.js)
FROM mirror.gcr.io/library/node:18 AS frontend-build
WORKDIR /app

# Копируем файлы фронтенда (package.json и т.д.)
COPY ClientApp/package*.json ./
RUN npm install

# Копируем весь фронтенд и собираем
COPY ClientApp/ .
RUN npm run build && ls -R /app

# Этап 2: Сборка бэкенда (.NET)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /src

# Копируем .csproj и восстанавливаем зависимости
COPY *.csproj .
RUN dotnet restore

# Копируем всё и собираем бэкенд
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Этап 3: Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем бэкенд
COPY --from=backend-build /app/publish .

# Копируем собранный фронтенд в папку wwwroot (или куда нужно)
COPY --from=frontend-build /app/build ./wwwroot

# Запускаем приложение
ENTRYPOINT ["dotnet", "radio-backend.dll"]