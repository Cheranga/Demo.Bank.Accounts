FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /databasemigration

FROM base as final
WORKDIR /databasemigration
COPY --from=build /databasemigration .
ENTRYPOINT ["dotnet", "Demo.Bank.Accounts.Database.Setup.dll"]
CMD ["arg0", "arg1", "arg2", "arg3"]