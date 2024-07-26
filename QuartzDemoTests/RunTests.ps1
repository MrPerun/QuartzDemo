Set-Location $PSScriptRoot
$project = ./QuartzDemoTests.csproj

dotnet test $project -- NUnit.NumberOfTestWorkers=1

while (1) {
    dotnet test $project --no-build -- NUnit.NumberOfTestWorkers=1
    $exitCode = $LASTEXITCODE;
    if ($exitCode -ne 0) {
        exit $exitCode
    }    
}
