FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EvergreenResort.sln ./
COPY EvergreenResort.Api/EvergreenResort.Api.csproj EvergreenResort.Api/
RUN dotnet restore EvergreenResort.Api/EvergreenResort.Api.csproj
COPY . .
RUN dotnet publish EvergreenResort.Api/EvergreenResort.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5024
EXPOSE 5024
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","EvergreenResort.Api.dll"]