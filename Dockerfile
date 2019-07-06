FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS builder

WORKDIR /sources

COPY ./NotadogApi/*.csproj .
RUN dotnet restore

COPY ./NotadogApi .
RUN dotnet publish --output /app/ --configuration Release

FROM mcr.microsoft.com/dotnet/core/sdk:2.1
WORKDIR /app
COPY --from=builder /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet NotadogApi.dll