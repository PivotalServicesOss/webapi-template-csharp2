# PivotalServices.WebApiTemplate.CSharp2

## Getting Started

1. Make sure you have the compatible .NET Core SDK versions installed

    [.NET Core SDK Version 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)


1. Goto the folder and run either `.\build.bat` or `./build.sh` for the initial build.

1. If docker is available and to run the application in docker, run `docker compose up` or `docker-compose up`


## Code Structure

1. Application 
    - The top shelf layer which exposes the API. In this case REST API. 
    - Contains application configuration and controllers
    - Contains unit and integration testing

1. Module
    - Contains core business features/logic
    - Contains the data access (optionally can be moved to a seperate project)
    - Contains unit tests

1. Shared
    - Contains shared infrastructure components
    - Contains unit tests

_*All common scripts like build scripts, dockerfile, test run settings, etc. are in the root level (where solution file resides)*_
