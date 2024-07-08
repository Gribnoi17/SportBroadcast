FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
LABEL author="Gribanov Danil"
ENV TZ="Europe/Moscow"
WORKDIR /src
COPY ./src .
RUN dotnet restore ./Sport.Broadcast.Host/Sport.Broadcast.Host.csproj && \
    dotnet publish ./Sport.Broadcast.Host/Sport.Broadcast.Host.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /src
COPY --from=build-env /src/out .
RUN echo "Europe/Moscow" > /etc/timezone
RUN dpkg-reconfigure -f noninteractive tzdata
ENTRYPOINT ["dotnet", "Sport.Broadcast.Host.dll"]