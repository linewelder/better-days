FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY "BetterDays.csproj" .
RUN dotnet restore "BetterDays.csproj"
COPY . .
RUN dotnet publish "BetterDays.csproj" -c $BUILD_CONFIGURATION -o /publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080/tcp
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "BetterDays.dll"]
