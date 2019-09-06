FROM microsoft/dotnet:sdk AS build-env

WORKDIR /app

EXPOSE 80

ENV LC_ALL C.UTF-8

COPY src/API/Core/IPLookup.API.Host src/API/Core/IPLookup.API.Host
COPY src/API/IPLookup.API.InMemoryDataBase src/API/IPLookup.API.InMemoryDataBase
COPY src/API/IPLookup.API.Services src/API/IPLookup.API.Services

COPY src/Common src/Common

RUN dotnet publish src/API/Core/IPLookup.API.Host/IPLookup.API.Host.csproj -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime

WORKDIR /app
COPY --from=build-env /app/src/API/Core/IPLookup.API.Host/out .

ENTRYPOINT ["dotnet", "IPLookup.API.Host.dll"]