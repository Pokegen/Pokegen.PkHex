
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["PKHeX.API/PKHeX-API.csproj", "PKHeX.API/"]
RUN dotnet restore "PKHeX.API/PKHeX-API.csproj"
COPY . .
WORKDIR "/src/PKHeX.API"
RUN dotnet build "PKHeX-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PKHeX-API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PKHeX-API.dll"]
