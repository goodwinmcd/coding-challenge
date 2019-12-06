FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c debug -o out

FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app
EXPOSE 3000

ENV ConnectionStrings__PostgresConnection=host=referral.postgres.compose;Username=docker;Password=docker;Port=5432;Database=referrals;

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ibotta_challenge.dll"]