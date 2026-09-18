# Build from the SmartSchool solution root:
# docker build -f src/SmartSchool.Api/Dockerfile -t smartschool-api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/SmartSchool.Api/SmartSchool.Api.csproj"
RUN dotnet publish "src/SmartSchool.Api/SmartSchool.Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
RUN mkdir -p /app/logs /app/App_Data/Documents && chown -R $APP_UID:$APP_UID /app
USER $APP_UID

ENTRYPOINT ["dotnet", "SmartSchool.Api.dll"]
