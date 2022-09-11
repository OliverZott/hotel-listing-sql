$postgresqlServiceName = "postgresql-x64-14"
$postgresqlService = Get-Service -Name $postgresqlServiceName

$seqServiceName = "Seq"
$seqService = Get-Service -Name $seqServiceName

$services = @($postgresqlService, $seqService)

Write-Output $services

# Set-Service $service -StartupType Manual

foreach ($service in $services) {
    if ($service.Status -ne "Running") {
        Start-Service $service
        $service
        # Start-Service -Name "MongoDB"
        # Start-Service -InputObject $service
    }
}
