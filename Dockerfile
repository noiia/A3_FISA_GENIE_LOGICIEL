## Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet publish EasySave.csproj -c Release -o /app 

# Execute    
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
#EXPOSE 5000
ENTRYPOINT ["dotnet", "EasySave.dll"]