FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /bankaccountsmicroservice

FROM base as final
WORKDIR /bankaccountsmicroservice
COPY --from=build /bankaccountsmicroservice .
ENTRYPOINT ["dotnet", "Demo.Bank.Accounts.Api.dll"]