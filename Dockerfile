# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Копируем файлы проекта и восстанавливаем зависимости
COPY . ./
RUN dotnet restore

# Компилируем и публикуем проект в папку out
RUN dotnet publish -c Release -o out

# Этап выполнения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

# Копируем собранные файлы из предыдущего этапа
COPY --from=build /App/out .

EXPOSE 8080

# Устанавливаем точку входа
ENTRYPOINT ["dotnet", "BankAccounts.API.dll"]
