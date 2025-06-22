param(
    [switch]$Reset
)

if ($Reset) {
    ./PipelineScripts/run-pre-commit-checks.ps1 -Reset
    exit 0
}

./PipelineScripts/run-pre-commit-checks.ps1 -ModulesIsln "Modules/Intent.Modules.Web.isln" -TestsIsln "Tests/Testing/AngularTest/intent/AngularTest.isln"

exit 0
