#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#
#COPY Client/Client.csproj .
#RUN dotnet restore Client.csproj
#
#COPY Shared/Shared.csproj .
#RUN dotnet restore Shared.csproj
#
#COPY . .
#RUN dotnet build Client/Client.csproj -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish Client/Client.csproj -c Release -o /app/publish
#
#FROM nginx:alpine AS final
#WORKDIR /usr/share/nginx/html
#COPY --from=publish /app/publish/wwwroot .
#COPY Client/nginx.conf /etc/nginx/nginx.conf

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Client/Client.csproj", "src/Client/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
RUN dotnet restore "src/Server/Client.csproj"
COPY . .
WORKDIR "/src/src/Client"
RUN dotnet build "Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["src/Client/nginx.conf", "etc/nginx/nginx.conf"]