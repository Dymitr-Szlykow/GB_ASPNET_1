#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UI/WebStore.WebApp/WebStore.WebApp/WebStore.WebApp.csproj", "UI/WebStore.WebApp/WebStore.WebApp/"]
RUN dotnet restore "UI/WebStore.WebApp/WebStore.WebApp/WebStore.WebApp.csproj"
COPY . .
WORKDIR "/src/UI/WebStore.WebApp/WebStore.WebApp"
RUN dotnet build "WebStore.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebStore.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebStore.WebApp.dll"]


#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["GB.ASPNET.WebStore/GB.ASPNET.WebStore.csproj", "GB.ASPNET.WebStore/"]
#COPY ["WebStore.Domain/WebStore.Domain.csproj", "WebStore.Domain/"]
#COPY ["WebStore.DAL/WebStore.DAL.csproj", "WebStore.DAL/"]
#RUN dotnet restore "GB.ASPNET.WebStore/GB.ASPNET.WebStore.csproj"
#COPY . .
#WORKDIR "/src/GB.ASPNET.WebStore"
#RUN dotnet build "GB.ASPNET.WebStore.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "GB.ASPNET.WebStore.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GB.ASPNET.WebStore.dll"]
#