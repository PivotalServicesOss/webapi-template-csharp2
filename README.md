# Pivotal ASP.NET Web API Template - v2 (CF anf K8s ready)

This is an opinionated ASP.NET Web API template for the dotnet new command. This template can help developers to start writing code for business features, right after using it. This also comes with various cool features as mentioned below.

## Features
- Completely tested (build using [TDD](https://en.wikipedia.org/wiki/Test-driven_development#:~:text=Test%2Ddriven%20development%20(TDD),software%20against%20all%20test%20cases.)). Available unit & integration tests can be used as code references.
- Produces [Cobertura Code Coverage](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux) results
- Uses [Psake](), a powershell based build automation tool, learn more [here](https://www.alfusjaganathan.com/blogs/psake-build-automation-net-msbuild/).
- Uses mediator pattern using [MediatR](https://github.com/jbogard/MediatR), adds loose coupling between application and core modules.
- Uses [Autofac](https://autofac.org/) in addition to .NETframework  provided [ServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollection?view=dotnet-plat-ext-6.0) for `Dependency Injection`
- Structured logging using [Serilog](https://serilog.net/), custom injection of additional properties for better diagnostics.
- A [Modular Monolith](https://modularmonolith.net/) to start with..
- In addition to xml documentation, APIs are well documented using [Swagger Annotations](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcoreannotations), a testable way of documentation, rather than xml based documentation.
- Uses Uri based API versioning, learn more [here](https://code-maze.com/aspnetcore-api-versioning/)
- Opinionated usage of `Request` and `Response` headers. Optional to modify as needed. Includes usage of `Correlation` headers 
- Standardized error responses, using [ProblemDetails](https://datatracker.ietf.org/doc/html/rfc7807)
- Usage of [High-performance logging in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging) 
- Uses [Fluent validation](https://fluentvalidation.net/), instead of ASP.NET framework model validation
- Uses [Fluent Assertions](https://fluentassertions.com/), [XUnit](https://xunit.net/), [Moq](https://github.com/moq/moq) and [Microsoft Testing SDK](https://www.nuget.org/packages/Microsoft.NET.Test.SDK) for unit and integration tests
- Usage of a global file `version.props` which contains all nuget dependency versions across the application
- Auto wiring of configurations from [Kubernetes](https://kubernetes.io/) and [Cloud Foundry](https://www.cloudfoundry.org/)
- [Docker](https://www.docker.com/) and [Docker Compose](https://docs.docker.com/compose/) files are included.

            
## Getting Started

1. Install the template from [nuget.org](https://www.nuget.org/packages/PivotalServices.WebApiTemplate.CSharp2/#:~:text=shell/command%20line.-,README,-Dependencies)

    ```
    dotnet new -i PivotalServices.WebApiTemplate.CSharp2
    ```

1. This should install a template with the shortname `pvtlwebapi2`

    ```
    Templates                                         Short Name         Language          Tags
    ----------------------------------------------------------------------------------------------------------------------------
    Pivotal WebAPI Template - v2                        pvtlwebapi2         [C#]              WebAPI/Web/Modular
    ```

2. To generate a new project

    ```
    dotnet new pvtlwebapi2 -n <NAME_OF_SOLUTION>
    ```

1. Make sure you have the compatible .NET Core SDK versions installed

    Package Version 1.0.0 -> [.NET Core SDK Version 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)


1. Goto the folder and run either `build.bat` or `./build.sh` for the initial build.

1. If docker is available and to run the application in docker, run `docker compose up` or `docker-compose up`

> If you have any queries, please raise an issue [here](https://github.com/alfusinigoj/pivotal-webapi-template-csharp2/issues)

> If you are looking for a lighter version of web api template, install from [nuget.org](https://www.nuget.org/packages/PivotalServices.WebApiTemplate.CSharp/#:~:text=shell/command%20line.-,README,-Dependencies)