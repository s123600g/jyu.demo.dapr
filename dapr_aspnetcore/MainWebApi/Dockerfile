#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MainWebApi/MainWebApi.csproj", "MainWebApi/"]
RUN dotnet restore "MainWebApi/MainWebApi.csproj"
COPY . .
WORKDIR "/src/MainWebApi"
RUN dotnet build "MainWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainWebApi.dll"]