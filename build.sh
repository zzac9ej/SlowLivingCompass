#!/bin/bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 10.0 -InstallDir ./dotnet

# Inject Gemini API Key from Vercel environment variable
mkdir -p SlowLivingCompass.Client/wwwroot
echo "{\"GeminiApiKey\": \"$GEMINI_API_KEY\"}" > SlowLivingCompass.Client/wwwroot/appsettings.json

# Publish the Blazor app
./dotnet/dotnet publish SlowLivingCompass.Client/SlowLivingCompass.Client.csproj -c Release -o release

# Fix fingerprinted assets
find release/wwwroot/_framework -name 'blazor.webassembly.*.js' -exec cp {} release/wwwroot/_framework/blazor.webassembly.js \;
find release/wwwroot/_framework -name 'dotnet.*.js' -exec cp {} release/wwwroot/_framework/dotnet.js \;
find release/wwwroot/_framework -name 'dotnet.native.*.js' -exec cp {} release/wwwroot/_framework/dotnet.native.js \;
find release/wwwroot/_framework -name 'dotnet.runtime.*.js' -exec cp {} release/wwwroot/_framework/dotnet.runtime.js \;
find release/wwwroot/_framework -name 'dotnet.native.*.wasm' -exec cp {} release/wwwroot/_framework/dotnet.native.wasm \;

# Brute force: Copy everything to the root directory
cp -r release/wwwroot/* .
