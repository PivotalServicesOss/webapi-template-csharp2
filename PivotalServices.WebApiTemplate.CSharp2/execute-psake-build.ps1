[cmdletbinding()]
param(
    [Parameter(Position = 0, Mandatory = $false)]
    [string]$task_name
)
$ErrorActionPreference = "Stop"

$has_psake = Get-Module -ListAvailable | Select-String -Pattern "Psake" -Quiet
if(-Not($has_psake)) {
    #import psake
    $module = Join-Path $current_dir -Child "tools\psake.4.9.0\psake.psm1"
    Write-Host "Importing module $module"
    Import-Module $module -Scope Global
}
#execute psake build
try{
    Write-Host "Executing task $task_name"
    invoke-psake .\default.ps1 $task_name -parameters @{"solution_name"="PivotalServices.WebApiTemplate.CSharp2";}
    exit !($psake.build_success)
}
catch{
    Write-Host POWERSHELLERROR
    Write-Error -Exception $_.Exception
    exit 1
}