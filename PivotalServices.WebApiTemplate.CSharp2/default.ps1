$script:project_config = "Debug"

properties {
  $base_dir = resolve-path .
  $solution_file = "$base_dir\$solution_name.sln"
  $app_project_file = "$base_dir\src\$solution_name.Application\$solution_name.Application.csproj"
  $date = Get-Date
  $dotnet_exe = get-dotnet

  $release_id = "linux-x64"
  $target_frameworks = "net6.0"
  $app_publish_dir = "$base_dir\publish-artifacts\app\$release_id"
  $test_results_dir = "$base_dir\test-results"
  $test_coverage_threshold = 85
}
#These are aliases for other build tasks. They typically are named after the camelcase letters (rd = Rebuild Databases)
task default -depends DevBuild
task dp -depends DevPublish
task ci -depends CiBuild
task cp -depends CiPublish
task ? -depends help

task EmitProperties {
  Write-Host "base_dir=$base_dir"
  Write-Host "solution_file=$solution_file"
  Write-Host "app_project_file=$app_project_file"
  Write-Host "app_publish_dir=$app_publish_dir"
  Write-Host "project_config=$project_config"
  Write-Host "test_coverage_threshold=$test_coverage_threshold%"
}

task help {
   Write-Help-Header
   Write-Help-Section-Header "Comprehensive Building"
   Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
   Write-Help-For-Alias "dp" "Developer build and test with publishing"
   Write-Help-For-Alias "ci" "Continuous Integration build (long and thorough)"
   Write-Help-For-Alias "cp" "Continuous Integration build (long and thorough) with publishing"
   Write-Help-Footer
   exit 0
}
#These are the actual build tasks. They should be Pascal case by convention
task DevBuild -depends SetDebugBuild, EmitProperties, Restore, Clean, Compile, UnitTests, IntegrationTests
task DevPublish -depends DevBuild, Publish
task CiBuild -depends SetReleaseBuild, EmitProperties, Restore, Clean, Compile, UnitTests, IntegrationTests
task CiPublish -depends CiBuild, Publish

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task Restore {
    Write-Host "******************* Now restoring the solution dependencies *********************"  -ForegroundColor Green
    exec {
        & $dotnet_exe msbuild /t:restore $solution_file /v:m /p:NuGetInteractive="true" /p:RuntimeIdentifier=$release_id
    }
}

task Clean {
    Write-Host "******************* Now cleaning the solution and artifacts *********************"  -ForegroundColor Green
    if (Test-Path $app_publish_dir) {
        delete_directory $app_publish_dir
    }
    exec {
        & $dotnet_exe msbuild /t:clean /v:m /p:Configuration=$project_config $solution_file
    }
}

task Compile {
    Write-Host "******************* Now compiling the solution *********************"  -ForegroundColor Green
    exec {
        & $dotnet_exe msbuild /t:build /v:m /p:Configuration=$project_config /nologo /p:Platform="Any CPU" /nologo $solution_file
    }
}

task UnitTests {
    Write-Host "******************* Now running unit tests, generating and assessing code coverage results*********************"  -ForegroundColor Green
    if (Test-Path $test_results_dir) {
        delete_directory $test_results_dir
    }
    Push-Location $base_dir
    $test_projects = @((Get-ChildItem -Recurse -Filter "*UnitTests.csproj").FullName) -join '~'

    foreach($test_project in $test_projects.Split("~"))
    {
        Write-Host "Executing tests on: $test_project"
        exec {
            $test_project_name = (Get-Item $test_project).Directory.Name.TrimEnd("UnitTests").TrimEnd(".")
            & $dotnet_exe test /p:threshold=$test_coverage_threshold /p:ThresholdType=line /p:SkipAutoProps=true /p:Include="[$test_project_name]*" /p:CollectCoverage=true /p:CoverletOutput="$test_results_dir/$test_project_name/" /p:CoverletOutputFormat="cobertura" $test_project --no-restore --configuration $project_config --settings "$base_dir\test-run-settings" -- xunit.parallelizeTestCollections=true
        }
    }
    Pop-Location
 }

 task IntegrationTests {
    Write-Host "******************* Now running integration tests *********************"  -ForegroundColor Green
    Push-Location $base_dir
    $test_projects = @((Get-ChildItem -Recurse -Filter "*IntegrationTests.csproj").FullName) -join '~'

    foreach($test_project in $test_projects.Split("~"))
    {
        Write-Host "Executing tests on: $test_project"
        exec {
            & $dotnet_exe test -c $project_config $test_project --logger "console;verbosity=detailed" -- xunit.parallelizeTestCollections=$false
        }
    }
    Pop-Location
 }

task Publish {
    Write-Host "******************* Now publishing the application to $app_publish_dir *********************"  -ForegroundColor Green
    exec {
        & $dotnet_exe msbuild /t:restore $solution_file /v:m /p:NuGetInteractive="true" /p:RuntimeIdentifier=$release_id /p:TargetFrameworks=$target_frameworks
        & $dotnet_exe msbuild /t:publish /v:m /p:Platform=$platform /p:TargetFrameworks=$target_frameworks /p:RuntimeIdentifier=$release_id /p:PublishDir=$app_publish_dir /p:Configuration=$project_config /nologo $app_project_file
    }
}

task DockerUp {
    Write-Host "******************* Now re-building docker image and running it *********************"  -ForegroundColor Green
    exec {
        & docker compose down
        & docker compose build
        & docker compose up -d
    }
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions for Help Section
# --------------------------------------------------------------------------------------------------------------
function Write-Help-Header($description) {
   Write-Host ""
   Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
   Write-Host " HELP " -foregroundcolor Green  -nonewline;
   Write-Host "********************************"  -foregroundcolor DarkGreen
   Write-Host ""
   Write-Host "This build script has the following common build " -nonewline;
   Write-Host "task " -foregroundcolor Green -nonewline;
   Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
   Write-Host ""
   Write-Host " For a complete list of build tasks, view default.ps1."
   Write-Host ""
   Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
   Write-Host ""
   Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
   Write-Host "  > " -nonewline;
   Write-Host "$alias" -foregroundcolor Green -nonewline;
   Write-Host " = " -nonewline;
   Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions
# --------------------------------------------------------------------------------------------------------------
function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null }
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:get-dotnet(){
	return (Get-Command dotnet).Path
}