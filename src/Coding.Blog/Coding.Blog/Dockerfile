FROM mcr.microsoft.com/dotnet/aspnet:9.0.1-bookworm-slim AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0.102-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Coding.Blog/Coding.Blog/Coding.Blog.csproj", "Coding.Blog/Coding.Blog/"]
COPY ["Coding.Blog/Coding.Blog.Client/Coding.Blog.Client.csproj", "Coding.Blog/Coding.Blog.Client/"]
COPY ["Coding.Blog/Coding.Blog.Library/Coding.Blog.Library.csproj", "Coding.Blog/Coding.Blog.Library/"]
RUN dotnet restore "./Coding.Blog/Coding.Blog/./Coding.Blog.csproj"
COPY . .
WORKDIR "/src/Coding.Blog/Coding.Blog"
RUN dotnet build "./Coding.Blog.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Coding.Blog.csproj" -c $BUILD_CONFIGURATION --runtime linux-x64 --self-contained true -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coding.Blog.dll"]
