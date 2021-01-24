
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["Pokégen.PkHex/Pokégen.PkHex.csproj", "Pokégen.PkHex/"]
RUN dotnet restore "Pokégen.PkHex/Pokégen.PkHex.csproj"
COPY . .
WORKDIR "/src/Pokégen.PkHex"
RUN dotnet build "Pokégen.PkHex.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokégen.PkHex.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokégen.PkHex.dll"]
