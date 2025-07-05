# MarketPriceService API

MarketPriceService � �� API ��� ������ � ��������� ��������, ������������� �� ���������� ������� ����� ����� WebSocket.

## ������ ���������
- ������ � ��������� �������� �� �������������
- ϳ��������� �� ���������� WebSocket ��� ��������� ���
- Swagger UI ��� ���������� API

## ������
- .NET 8 SDK
- Docker (�������)
- SQL Server (�������� ��� � Docker)
- ������ �� ���������� WebSocket (Fintacharts)

## ������������
1. �������� ���� `appsettings.json` �� ���������� ����� ���������� �� ���� ����� � ������ `ConnectionStrings:DefaultConnection`.
2. ������ ��������� ������ `Fintacharts` ��� ���������� �� WebSocket.

## ������ ����� Docker
1. ³���������� `.env` ���� (���� �������).
2. �������� �������:
   ```sh
   docker-compose up --build
   ```
3. API ���� ��������� �� �������: `https://localhost:5001` (��� ����, �������� � docker-compose).

## ��������� ������
1. ��������� ���������:
   ```sh
   dotnet restore
   ```
2. �������� ������� (�������, ���� ���� �������):
   ```sh
   dotnet ef database update --project MarketPriceService.DataAccess
   ```
3. �������� API:
   ```sh
   dotnet run --project MarketPriceService.UI
   ```
4. Swagger UI: `https://localhost:5001/swagger`

## ������������ API
Swagger UI ��������� �� `/swagger` ���� �������.

## ���������
- ��� ���� ����������� WebSocket ��� ���� ����� �������������� ������� ������ � `appsettings.json`.
- ��� production-������ �������������, �� �� ������� �� ����� �������� ��������.
