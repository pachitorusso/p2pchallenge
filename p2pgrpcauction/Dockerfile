FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["p2pgrpcauction/p2pgrpcauction.csproj", "p2pgrpcauction/"]
RUN dotnet restore "p2pgrpcauction/p2pgrpcauction.csproj"
COPY . .
WORKDIR "/src/p2pgrpcauction"
RUN dotnet build "p2pgrpcauction.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "p2pgrpcauction.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "p2pgrpcauction.dll"]
