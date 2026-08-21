# Build from the SmartSchool solution root:
# docker build -f src/SmartSchool.Identity.Api/Dockerfile -t smartschool-identity-api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/SmartSchool.Identity.Api/SmartSchool.Identity.Api.csproj"
RUN dotnet publish "src/SmartSchool.Identity.Api/SmartSchool.Identity.Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartSchool.Identity.Api.dll"]
