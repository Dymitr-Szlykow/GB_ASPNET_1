#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UI/WebStore.WebApp/WebStore.WebApp.csproj", "UI/WebStore.WebApp/"]
COPY ["Services/WebStore.Services/WebStore.Services.csproj", "Services/WebStore.Services/"]
COPY ["Services/WebStore.Interfaces/WebStore.Interfaces.csproj", "Services/WebStore.Interfaces/"]
COPY ["Common/WebStore.Domain/WebStore.Domain.csproj", "Common/WebStore.Domain/"]
COPY ["Data/WebStore.DAL/WebStore.DAL.csproj", "Data/WebStore.DAL/"]
COPY ["Data/WebStore.DAL.Sqlite/WebStore.DAL.Sqlite.csproj", "Data/WebStore.DAL.Sqlite/"]
COPY ["Services/WebStore.WebAPI.Clients/WebStore.WebAPI.Clients.csproj", "Services/WebStore.WebAPI.Clients/"]
RUN dotnet restore "UI/WebStore.WebApp/WebStore.WebApp.csproj"
COPY . .
WORKDIR "/src/UI/WebStore.WebApp"
RUN dotnet build "WebStore.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebStore.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebStore.WebApp.dll"]