#!/bin/sh
set -e

echo "Waiting for SQL Server to be ready..."

for i in {1..30}; do
  /opt/mssql-tools/bin/sqlcmd -S db -U sa -P "$SA_PASSWORD" -Q "SELECT 1" && break
  echo "[$i] SQL Server is not ready yet. Waiting..."
  sleep 1
done

echo "Starting application..."
exec dotnet MarketPriceService.UI.dll