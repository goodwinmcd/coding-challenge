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

# ENV ConnectionStrings__PostgresConnection=host=referral.postgres.compose;Username=docker;Password=docker;Port=5432;Database=referrals;
ENV ConnectionStrings__PostgresConnection=host=ec2-174-129-255-72.compute-1.amazonaws.com;Username=slswqmprgyvfqj;Password=afdc24aafc218d2569d6eb7d68088ba11284fb8e3bc3aadecd22f2bea1ff49fb;Port=5432;Database=d7s455aneg1nts;


COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ReferalApi.dll"]