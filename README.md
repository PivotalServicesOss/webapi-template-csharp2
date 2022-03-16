# Pivotal ASP.NET Web API Template - v2 (CF anf K8s ready)

This is an opinionated ASP.NET Web API template for the dotnet new command. This template can **help developers to start writing code for business features**, right after using it. This also comes with various cool features as mentioned below.

## Features
- Well tested (built using [TDD](https://en.wikipedia.org/wiki/Test-driven_development#:~:text=Test%2Ddriven%20development%20(TDD),software%20against%20all%20test%20cases.)). In addition, all available unit & integration tests can also be used as code references for engineers who are new to writing unit/integration tests.

- [Psake](https://github.com/psake/psake), a powershell based build automation tool is used, which reduces some of the dependencies of CI tools. You can learn more about the tool [here](https://www.alfusjaganathan.com/blogs/psake-build-automation-net-msbuild/).

- Unit tests are integrated to generate [Cobertura Code Coverage](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux) results.

- [MediatR](https://github.com/jbogard/MediatR), a simple mediator implementation for .NET. I love the usage of this tool which reduces tight coupling between code components.

- A simple [Modular Monolith](https://modularmonolith.net/) to start with. Eventually can easily seperate it as needed, but please refer the article [Should that be a Microservice?](https://tanzu.vmware.com/content/blog/should-that-be-a-microservice-keep-these-six-factors-in-mind).

- [Autofac](https://autofac.org/) is used in addition to .NET framework  provided [ServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollection?view=dotnet-plat-ext-6.0) as `Inversion of Control container`

- [Serilog](https://serilog.net/) is used for structured logging capability and custom injection of additional properties for better diagnostics.

- In addition to xml documentation, APIs are well documented using [Swagger Annotations](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcoreannotations), a testable way of documentation, rather than xml based documentation.

- Uses Uri based API versioning, learn more [here](https://code-maze.com/aspnetcore-api-versioning/)

- Usage of sample `Request` and `Response` headers. Optional to modify as needed, based on the need. 

- Standardized error responses, using [ProblemDetails](https://datatracker.ietf.org/doc/html/rfc7807) 

- [High-performance logging in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging)

- [Fluent validation](https://fluentvalidation.net/), instead of ASP.NET framework model validation

- [Fluent Assertions](https://fluentassertions.com/), [XUnit](https://xunit.net/), [Moq](https://github.com/moq/moq) and [Microsoft Testing SDK](https://www.nuget.org/packages/Microsoft.NET.Test.SDK) for unit and integration tests.

- Usage of a global versioniong of all nuget dependencies across the projects.

- [ASP.NET Health checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0), a sample implementation included.

- [Steeltoe Application Configuration](https://docs.steeltoe.io/api/v3/configuration/index.html) is used for auto wiring of `configurations/secrets` from [Kubernetes](https://kubernetes.io/) and [Cloud Foundry](https://www.cloudfoundry.org/)

- [Docker](https://www.docker.com/) ready.

- Very well suited for one code base one repo (ref: [Beyond 12 factors](https://tanzu.vmware.com/content/blog/beyond-the-twelve-factor-app))

## Code Structure

1. Application 
    - The top shelf layer which exposes the API. In this case REST API. 
    - Contains application configuration and controllers
    - Contains unit and integration testing

1. Module
    - Contains core business features/logic
    - Contains the data access (optionally can move to a seperate project)
    - Contains unit tests

1. Shared
    - Contains shared infrastructure components
    - Contains unit tests

_*All common scripts like build scripts, dockerfile, test run settings, etc. are in the root level (where solution file resides)*_


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

    Package Version 1.0.2 -> [.NET Core SDK Version 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)


1. Goto the folder and run either `build.bat` or `./build.sh` for the initial build.

1. If docker is available and to run the application in docker, run `docker compose up` or `docker-compose up`

> If you have any queries, please raise an issue [here](https://github.com/alfusinigoj/pivotal-webapi-template-csharp2/issues)

> If you are looking for a lighter version of web api template, install from [nuget.org](https://www.nuget.org/packages/PivotalServices.WebApiTemplate.CSharp/#:~:text=shell/command%20line.-,README,-Dependencies)