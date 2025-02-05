## Build-CLI
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-cli
WORKDIR /source
COPY . .
RUN dotnet publish ./EasySaveCLI/EasySaveCLI.csproj -c Release -o /app

# Execute    
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-cli /app ./
#EXPOSE 5000
ENTRYPOINT ["dotnet", "EasySaveCLI.dll"]