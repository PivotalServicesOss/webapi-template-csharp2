FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build-env
WORKDIR /app

# Copy everything, restore, build and publish
COPY . ./
RUN dotnet publish ./src/PivotalServices.WebApiTemplate.CSharp2.Application/PivotalServices.WebApiTemplate.CSharp2.Application.csproj -c Release -o _publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build-env /app/_publish .
ENTRYPOINT ["dotnet", "PivotalServices.WebApiTemplate.CSharp2.Application.dll"]