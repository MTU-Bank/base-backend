
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /source
EXPOSE 80
EXPOSE 8090
ARG Semver

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . ./
RUN dotnet restore "./MTUBankBase/MTUBankBase.csproj" --no-cache
WORKDIR /source/MTUBankBase
RUN dotnet build "MTUBankBase.csproj" -c Release -o /source/build

FROM base AS final
WORKDIR /source
COPY --from=build /source/build .
ENTRYPOINT ["dotnet", "MTUBankBase.dll"]