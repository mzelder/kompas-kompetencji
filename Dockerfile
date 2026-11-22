FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src
COPY knowledge-base/knowledge-base.json knowledge-base/
COPY src/KompasKompetencji.Web/KompasKompetencji.Web.csproj src/KompasKompetencji.Web/
RUN dotnet restore src/KompasKompetencji.Web/KompasKompetencji.Web.csproj
COPY src/KompasKompetencji.Web/ src/KompasKompetencji.Web/
RUN dotnet publish src/KompasKompetencji.Web/KompasKompetencji.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:11.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Demo \
    ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080
ENTRYPOINT ["dotnet", "KompasKompetencji.Web.dll"]
