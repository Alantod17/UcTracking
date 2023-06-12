#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AwesomeDi.Api/AwesomeDi.Api.csproj", "AwesomeDi.Api/"]
#Run dotnet add "AwesomeDi.Api/AwesomeDi.Api.csproj" package DevExpress.Document.Processor -v 21.2.4 -s https://nuget.devexpress.com/q8b56vRcd7BURlmw3RHY92hbHbf8oZJJ6nsYGrpBCB8t7pjWXu/api
RUN dotnet restore "AwesomeDi.Api/AwesomeDi.Api.csproj"
COPY . .
WORKDIR "/src/AwesomeDi.Api"
RUN dotnet build "AwesomeDi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AwesomeDi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AwesomeDi.Api.dll"]