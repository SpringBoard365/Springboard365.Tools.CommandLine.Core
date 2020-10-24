@echo off
SET packageVersion=2.0.1

SET configuration=Release
SET id=Springboard365.Tools.CommandLine.Core
SET author="Springboard 365 Ltd"
SET repo="https://github.com/SpringBoard365/Springboard365.Tools.CommandLine.Core"
SET description="The core helper project to allow parameters to be passed in and verified on a console application."
SET tags="CommandLine Springboard365ToolHelper"

dotnet restore ../%id%.sln

dotnet build ../%id%.sln -c  %configuration% -p:Version=%packageVersion% -f net462 --nologo

pause

NuGet.exe pack ../src/CommandLine.Core.nuspec -Build -symbols -Version %packageVersion% -Properties Configuration=%configuration%;id=%id%;author=%author%;repo=%repo%;description=%description%;tags=%tags%;

NuGet.exe push Springboard365.Tools.CommandLine.Core.%packageVersion%.nupkg -Source "https://api.nuget.org/v3/index.json"

pause