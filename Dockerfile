# Используем базовый образ для среды сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Копируем все файлы проекта. Предполагается, что .dockerignore настроен должным образом.
COPY . .

# Восстановление всех зависимостей и сборка проекта
RUN dotnet restore \
    && dotnet publish Patient.API/Patient.API.csproj -c Release -o out

# Готовим финальный образ на основе runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем файлы сборки из промежуточного слоя
COPY --from=build-env /app/out .

# Открываем порты 80 и 443 для HTTP и HTTPS соответственно
EXPOSE 80
EXPOSE 443

# Задаем точку входа для запуска приложения
ENTRYPOINT ["dotnet", "Patient.API.dll"]