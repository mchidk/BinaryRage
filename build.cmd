@echo off

cls

:: Update NuGet executable.
tools\nuget\nuget.exe u -self

:Build
cls

:: Install FAKE.
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

:: Install NUnit Runners
"tools\nuget\nuget.exe" "install" "NUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion" "-Version" "2.6.2"

cls

:: Run the FAKE build script.
"packages\FAKE\tools\Fake.exe" build.fsx %*

:Quit
exit /b %errorlevel%