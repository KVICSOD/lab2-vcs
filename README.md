# Проект ЛР №1 — Система аутентификации (вариант 2)

Система с сетевыми структурами классов и слабой связностью.

## Автор

Александр, группа ИСТ

## Описание

Проект демонстрирует слабую связность через интерфейсы (DIP + DI).

- Интерфейс `IAuthenticator` с методом `Authenticate(credentials)`
- Реализации: `PasswordAuthenticator`, `FingerprintAuthenticator`, `QrAuthenticator`
- Основной модуль `AuthenticationService` пробует методы по очереди до первого успеха
- Расширение: `TokenAuthenticator` добавлен без изменения `AuthenticationService`

## Стек

- C# / .NET 8
- Serilog (логирование)
- Newtonsoft.Json
- FluentValidation
- xUnit (тесты)

## Установка

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project 3/3.csproj
```

## Требования

- .NET SDK 8.0+
- Git
