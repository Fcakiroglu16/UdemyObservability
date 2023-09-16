#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UdemyObservability.ConsoleApp/UdemyObservability.ConsoleApp.csproj", "UdemyObservability.ConsoleApp/"]
RUN dotnet restore "UdemyObservability.ConsoleApp/UdemyObservability.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/UdemyObservability.ConsoleApp"
RUN dotnet build "UdemyObservability.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "UdemyObservability.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UdemyObservability.ConsoleApp.dll"]