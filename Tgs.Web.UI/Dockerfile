FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["Tgs.Web.UI/Tgs.Web.UI.csproj", "Tgs.Web.UI/"]
RUN dotnet restore "Tgs.Web.UI/Tgs.Web.UI.csproj"
COPY . .
WORKDIR "/src/Tgs.Web.UI"
RUN dotnet build "Tgs.Web.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tgs.Web.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Web.UI.dll"]