FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY . .
WORKDIR "/src/Coding.Blog/Server"
RUN dotnet publish "Coding.Blog.Server.csproj" -c Release --runtime alpine-x64 --self-contained true -p:PublishTrimmed=true -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coding.Blog.Server.dll"]