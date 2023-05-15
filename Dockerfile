FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 3000

ENV ASPNETCORE_URLS=http://+:3000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ./*.sln .
COPY ["./EvergreenView/EvergreenView.csproj", "./EvergreenView/"]
COPY ["./EvergreenAPI/EvergreenAPI.csproj", "./EvergreenAPI/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/EvergreenView"
RUN dotnet build "EvergreenView.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EvergreenView.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet EvergreenView.dll