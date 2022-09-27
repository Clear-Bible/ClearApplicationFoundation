cd ClearApplicationFoundation\
rem msbuild -t:pack
dotnet pack

cd bin\Release
nuget push .\ClearApplicationFoundation.1.0.27.nupkg -ApiKey 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 -Source https://nuget.pkg.github.com/clear-bible/index.json

pause