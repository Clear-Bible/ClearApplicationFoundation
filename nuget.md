# Building and deploying ClearApplicationFoundation Nuget package

## Prerequisite

[Get nuget.exe and put it in your path](https://www.nuget.org/downloads)

## Steps

1. Open Powershell terminal in Engine's base solution directory.
2. Change the version number of the ClearApplicationFoundation project
3. Build the solution
4. Open a terminal window in the Clearapplication\bin\Debug directory
5. Execute `nuget push .\ClearApplicationFoundation.<VERSION>.nupkg -ApiKey <YOUR KEY> -Source https://nuget.pkg.github.com/clear-bible/index.json`