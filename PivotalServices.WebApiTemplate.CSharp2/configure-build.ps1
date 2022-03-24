$current_dir = resolve-path .
$has_psake = Get-Module -ListAvailable | Select-String -Pattern "Psake" -Quiet
if(-Not($has_psake)) {
    #import psake
    $module = Join-Path $current_dir -Child "tools/psake.4.9.0/psake.psm1"
    Write-Host "Importing module $module"
    Import-Module $module -Scope Global
}