#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Coding.Blog/Server/Coding.Blog.Server.csproj", "Coding.Blog/Server/"]
COPY ["Coding.Blog/Shared/Coding.Blog.Shared.csproj", "Coding.Blog/Shared/"]
COPY ["Coding.Blog/Client/Coding.Blog.Client.csproj", "Coding.Blog/Client/"]
COPY ["Coding.Blog/Client/Coding.Blog.Client.csproj", "Coding.Blog/Engine/"]
RUN dotnet restore "Coding.Blog/Server/Coding.Blog.Server.csproj"
COPY . .
WORKDIR "/src/Coding.Blog/Server"
RUN dotnet build "Coding.Blog.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coding.Blog.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coding.Blog.Server.dll"]