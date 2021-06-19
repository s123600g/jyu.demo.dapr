#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["dapr_aspnetcore/dapr_aspnetcore.csproj", "dapr_aspnetcore/"]
RUN dotnet restore "dapr_aspnetcore/dapr_aspnetcore.csproj"
COPY . .
WORKDIR "/src/dapr_aspnetcore"
RUN dotnet build "dapr_aspnetcore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dapr_aspnetcore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dapr_aspnetcore.dll"]
