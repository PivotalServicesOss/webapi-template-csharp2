@echo Executing build with default.ps1 configuration

@echo on

powershell.exe -NoProfile -NonInteractive -ExecutionPolicy bypass -Command "& { .\execute-psake-build.ps1 %1; exit $LASTEXITCODE }"

EXIT /B %ERRORLEVEL%

