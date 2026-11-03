FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["WebApplication1/WebApplication1/WebApplication1.csproj", "WebApplication1/WebApplication1/"]
RUN dotnet restore "WebApplication1/WebApplication1/WebApplication1.csproj"

COPY . .
WORKDIR "/src/WebApplication1/WebApplication1"
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["sh", "-c", "if [ -n \"$PORT\" ]; then export ASPNETCORE_URLS=\"http://0.0.0.0:$PORT\"; fi; exec dotnet WebApplication1.dll"]
