#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiServices/News.Web.Api/News.Web.Api/News.Web.Api.csproj", "ApiServices/News.Web.Api/News.Web.Api/"]
RUN dotnet restore "ApiServices/News.Web.Api/News.Web.Api/News.Web.Api.csproj"
COPY . .
WORKDIR "/src/ApiServices/News.Web.Api/News.Web.Api"
RUN dotnet build "News.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "News.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "News.Web.Api.dll"]