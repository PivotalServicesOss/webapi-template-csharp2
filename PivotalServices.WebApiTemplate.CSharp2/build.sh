#!/bin/bash

echo Executing build with default.ps1 configuration

echo on

pwsh -NoProfile -NonInteractive -ExecutionPolicy bypass -Command "& { .\execute-psake-build.ps1 $1; exit $COMMAND_LAST }"

exit $?
