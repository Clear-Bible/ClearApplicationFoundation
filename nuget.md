# Building and deploying ClearApplicationFoundation Nuget package

## Prerequisite

[Get nuget.exe and put it in your path](https://www.nuget.org/downloads)

## Steps

1. Change the version number of the ClearApplicationFoundation project
2. Build the solution
3. Open a terminal window in the `ClearApplicationFoundation\bin\Debug` directory
4. Execute `nuget push .\ClearApplicationFoundation.<VERSION>.nupkg -ApiKey <YOUR KEY> -Source https://nuget.pkg.github.com/clear-bible/index.json`