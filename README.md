# MarketPriceService API

MarketPriceService — це API для роботи з ринковими активами, інструментами та отриманням цінових даних через WebSocket.

## Основні можливості
- Робота з ринковими активами та інструментами
- Підключення до зовнішнього WebSocket для отримання цін
- Swagger UI для тестування API

## Вимоги
- .NET 8 SDK
- Docker (опційно)
- SQL Server (локально або в Docker)
- Доступ до зовнішнього WebSocket (Fintacharts)

## Налаштування
Перед запуском додайте файл `appsettings.Development.json` у корінь проєкту MarketPriceService.UI.  
Скопіюйте вміст із `appsettings.Template.json` і вставте реальні дані:

- Fintacharts `Username` і `Password`
- Connection string до бази даних

## Запуск через Docker
1. Додайте `.env` файл з рядком підключення та паролем до бази даних.
2. Запустіть команду:
   ```sh
   docker-compose up --build
   ```
3. API буде доступний за адресою: `https://localhost:5001` (або порт, вказаний у docker-compose).

## Локальний запуск
1. Встановіть залежності:
   ```sh
   dotnet restore
   ```
2. Запустіть міграції (опційно, якщо база порожня):
   ```sh
   dotnet ef database update --project MarketPriceService.DataAccess
   ```
3. Запустіть API:
   ```sh
   dotnet run --project MarketPriceService.UI
   ```
4. Swagger UI: `https://localhost:5001/swagger`

## Документація API
Swagger UI доступний за `/swagger` після запуску.

## Додатково
- Для зміни налаштувань WebSocket або бази даних використовуйте відповідні секції у `appsettings.json`.
- Для production-режиму переконайтесь, що всі секрети та ключі збережені безпечно.
