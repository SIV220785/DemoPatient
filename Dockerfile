# Используем базовый образ для среды сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Копируем файлы решения и всех проектов
COPY *.sln ./
COPY Patient.API/*.csproj ./Patient.API/
COPY Patient.BLL/*.csproj ./Patient.BLL/
COPY Patient.DAL/*.csproj ./Patient.DAL/
COPY Patient.BLL.Tests/*.csproj ./Patient.BLL.Tests/
COPY Patient.ConsoleApp/*.csproj ./Patient.ConsoleApp/

# Восстановление всех зависимостей
RUN dotnet restore

# Копирование всех остальных файлов
COPY Patient.API/ ./Patient.API/
COPY Patient.BLL/ ./Patient.BLL/
COPY Patient.DAL/ ./Patient.DAL/
COPY Patient.BLL.Tests/*.csproj ./Patient.BLL.Tests/
COPY Patient.ConsoleApp/*.csproj ./Patient.ConsoleApp/

# Собираем и публикуем проект
RUN dotnet publish Patient.API/Patient.API.csproj -c Release -o out

# Готовим финальный образ на основе runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out ./
EXPOSE 80

ENTRYPOINT ["dotnet", "Patient.API.dll"]