#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_HTTP_PORT=5001
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GmailChallenge/GmailChallenge.csproj", "GmailChallenge/"]
RUN dotnet restore "GmailChallenge/GmailChallenge.csproj"
COPY . .
WORKDIR "/src/GmailChallenge"
RUN dotnet build "GmailChallenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GmailChallenge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GmailChallenge.dll"]