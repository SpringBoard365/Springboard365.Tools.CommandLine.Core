NuGet.exe pack ../src/CommandLine.Core.csproj -Build -symbols

NuGet.exe push *.nupkg

pause