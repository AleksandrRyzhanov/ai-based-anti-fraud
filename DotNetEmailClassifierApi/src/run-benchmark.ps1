# Use relative paths and sanitize model name for output file
param(
    [Parameter(Mandatory=$true)]
    [string]$ModelName
)

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$apiUrl = "http://localhost:50082/api/emailclassification/classify"
$issueTypesPath = Join-Path $scriptRoot ".\Resources\IssueTypes.json"
$benchmarkFolder = Join-Path $scriptRoot ".\..\benchmark"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$sanitizedModelName = $ModelName -replace '[\/:"*?<>|]', '_'
$outputFile = "$benchmarkFolder\benchmark_${sanitizedModelName}_$timestamp.txt"

if (!(Test-Path $benchmarkFolder)) {
    New-Item -ItemType Directory -Path $benchmarkFolder | Out-Null
}

# Load issue types
$issueTypesJson = Get-Content $issueTypesPath -Raw -Encoding UTF8
$issueTypes = ($issueTypesJson | ConvertFrom-Json).issue_types

# Write header
@"
Model: $ModelName
Timestamp: $timestamp

"@ | Set-Content $outputFile

foreach ($topic in $issueTypes) {
    Add-Content $outputFile "=========================="
    Add-Content $outputFile "Topic: $($topic.type) (ID: $($topic.id))"
    Add-Content $outputFile ""

    foreach ($testType in @("relevant", "partially_relevant", "irrelevant")) {
        $msg = $topic.$testType
        if ($null -ne $msg -and $msg -ne "") {
            $payload = [PSCustomObject]@{
                message = $msg
                topicid = "$($topic.id)"
                model = $ModelName
            } | ConvertTo-Json -Depth 3

            try {
                $response = Invoke-RestMethod -Uri $apiUrl -Method Post -ContentType "application/json" -Body $payload
                $aiOutput = ConvertTo-Json $response -Depth 3
            } catch {
                $aiOutput = "ERROR: $($_.Exception.Message)"
            }

            Add-Content $outputFile "[$testType]"
            Add-Content $outputFile "Message: $msg"
            Add-Content $outputFile "AI Output: $aiOutput"
            Add-Content $outputFile ""
        }
    }
    Add-Content $outputFile "--------------------------"
}

Write-Host "Benchmark complete. Results saved to $outputFile"