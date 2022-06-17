FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore Demo.Bank.Accounts.Api.csproj
COPY . .
RUN dotnet publish "Demo.Bank.Accounts.Api.csproj" -c release -o /bankaccountsmicroservice

FROM base as final
WORKDIR /bankaccountsmicroservice
COPY --from=build /bankaccountsmicroservice .
ENTRYPOINT ["dotnet", "Demo.Bank.Accounts.Api.dll"]