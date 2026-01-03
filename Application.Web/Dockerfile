FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Application.Web/Application.Web.csproj", "Application.Web/"]
COPY ["Application.Service/Application.Service.csproj", "Application.Service/"]
COPY ["Application.Infrastructure/Application.Infrastructure.csproj", "Application.Infrastructure/"]
COPY ["Application.Domain/Application.Domain.csproj", "Application.Domain/"]
RUN dotnet restore "Application.Web/Application.Web.csproj"

COPY . .
WORKDIR /src/Application.Web
RUN dotnet publish "Application.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Application.Web.dll"]
