FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dotnet.Homeworks.Data.csproj", "Dotnet.Homeworks.Data/"]
COPY ["Dotnet.Homeworks.DataAccess.csproj", "Dotnet.Homeworks.DataAccess/"]
COPY ["Dotnet.Homeworks.Features.csproj", "Dotnet.Homeworks.Features/"]
COPY ["Dotnet.Homeworks.Infrastructure.csproj", "Dotnet.Homeworks.Infrastructure/"]
COPY ["Dotnet.Homeworks.MainProject.csproj", "Dotnet.Homeworks.MainProject/"]
COPY ["Dotnet.Homeworks.Domain.csproj", "Dotnet.Homeworks.Domain/"]
COPY ["Dotnet.Homeworks.Mailing.csproj", "Dotnet.Homeworks.Mailing/"]
COPY ["Dotnet.Homeworks.Mediator.csproj", "Dotnet.Homeworks.Mediator/"]
COPY ["Dotnet.Homeworks.Shared.csproj", "Dotnet.Homeworks.Shared/"]
COPY ["Dotnet.Homeworks.Storage.API.csproj", "Dotnet.Homeworks.Storage.API/"]

RUN dotnet restore "Dotnet.Homeworks.MainProject/Dotnet.Homeworks.MainProject.csproj"

COPY . .

WORKDIR "/src/Dotnet.Homeworks.MainProject"

RUN dotnet build "Dotnet.Homeworks.MainProject.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Dotnet.Homeworks.MainProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Dotnet.Homeworks.MainProject.dll"]