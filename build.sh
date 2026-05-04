#!/bin/bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 10.0 -InstallDir ./dotnet

# Inject Gemini API Key from Vercel environment variable
mkdir -p SlowLivingCompass.Client/wwwroot
echo "{\"GeminiApiKey\": \"$GEMINI_API_KEY\"}" > SlowLivingCompass.Client/wwwroot/appsettings.json

# Publish the Blazor app
./dotnet/dotnet publish SlowLivingCompass.Client/SlowLivingCompass.Client.csproj -c Release -o release
