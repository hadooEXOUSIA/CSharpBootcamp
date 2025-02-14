@echo off
echo Starting Message Broker API...
start cmd /k "cd /d MessageBroker\MessageBroker && dotnet run"

:: Wait 3 seconds to ensure API starts first
timeout /t 3 /nobreak >nul

echo Starting Producer App...
start cmd /k "cd /d ProducerApp\ProducerApp && dotnet run"

:: Wait 1 second before launching the Consumer
timeout /t 1 /nobreak >nul

echo Starting Consumer App...
start cmd /k "cd /d ConsumerApp\ConsumerApp && dotnet run"

echo ✅ All applications started in separate CMD windows!
