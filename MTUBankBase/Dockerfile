# юзаем SDK образ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# юзаем runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
# запускаем как dotnet MTUBankBase.dll
ENTRYPOINT ["dotnet", "MTUBankBase.dll"]