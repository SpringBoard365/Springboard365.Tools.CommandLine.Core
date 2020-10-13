NuGet.exe pack ../Springboard365.Tools.CommandLine.Core.csproj -Build -symbols

NuGet.exe push *.nupkg

pause