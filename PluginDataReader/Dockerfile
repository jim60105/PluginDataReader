#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PluginDataReader/PluginDataReader.csproj", "PluginDataReader/"]
RUN dotnet restore "PluginDataReader/PluginDataReader.csproj"
COPY PluginDataReader PluginDataReader
WORKDIR "/src/PluginDataReader"
RUN dotnet build "PluginDataReader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PluginDataReader.csproj" -c Release -o /app/publish

FROM nginx:1.25.3-alpine3.18 AS final
EXPOSE 80
WORKDIR /usr/share/nginx/html
ENV BLAZOR_ENVIRONMENT=Production
COPY --from=publish "/app/publish/wwwroot" .
COPY "PluginDataReader/nginx.conf" /etc/nginx/nginx.conf
COPY "PluginDataReader/start.sh" ./
RUN sed -i -e "s/\r$//" start.sh
CMD /bin/sh ./start.sh