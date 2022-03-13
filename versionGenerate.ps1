Write-Host "SourceBranch: $(Build.SourceBranch)"
Write-Host "SourceBranchName: $(Build.SourceBranchName)"
Write-Host "SourceVersionMessage: $(Build.SourceVersionMessage)"

$env:AZURE_DEVOPS_EXT_PAT = '$(system.Pat)'

$majorVersion = "$(version.Major)"
$minorVersion = "$(version.Minor)"
$patchVersion = "$(version.Patch)"
$buildCounter = "$(Build.BuildId)" 
$branch = "$(Build.SourceBranch)"
$pipelineName = "$(Build.DefinitionName)"

$cmd = "git log --pretty=format:""%s"" -1"
$commitMessage = Invoke-Expression $cmd
$commitMessage = $commitMessage.ToLower()
Write-Host "branch: $branch"
Write-Host "commitMessage: $commitMessage"
$buildNumber = "${majorVersion}.${minorVersion}.${patchVersion}.${buildCounter}"

if ($branch -eq "refs/heads/master") 
{
    if ($commitMessage -like '*hotfix*') {
        $version = [int]$patchVersion
        $version = $version + 1
        $patchVersion = [string]$version;
        Write-Host "##vso[task.setvariable variable=currentPatch;]$patchVersion"
        az pipelines variable update --org https://dev.azure.com/dwa125/ --project AwesomeDi  --pipeline-name $pipelineName --name version.Patch --value $patchVersion 
    }
    
    if ($commitMessage -like '*release*') {
        $patchVersion = "0";
        Write-Host "##vso[task.setvariable variable=currentPatch;]$patchVersion"
        az pipelines variable update --org https://dev.azure.com/dwa125/ --project AwesomeDi  --pipeline-name $pipelineName --name version.Patch --value $patchVersion 
        $version = [int]$minorVersion
        $version = $version + 1
        $minorVersion = [string]$version;
        Write-Host "##vso[task.setvariable variable=currentMinor;]$minorVersion"
        az pipelines variable update --org https://dev.azure.com/dwa125/ --project AwesomeDi  --pipeline-name $pipelineName --name version.Minor --value $minorVersion
    }
    $buildNumber = "${majorVersion}.${minorVersion}.${patchVersion}.${buildCounter}"
    $deployEnv = "Prestage"
}
elseif ($branch -eq "refs/heads/develop") 
{
    $version = [int]$minorVersion
    $version = $version + 1
    $minorVersion = [string]$version;
    $buildNumber = "${majorVersion}.${minorVersion}.0-Dev${buildCounter}"
    $deployEnv = "Development"
}
elseif ($branch -match "release/*") 
{    
    $version = [int]$minorVersion
    $version = $version + 1
    $minorVersion = [string]$version;
    $buildNumber = "${majorVersion}.${minorVersion}.0-Rel${buildCounter}"
    $deployEnv = "Test"
}
elseif ($branch -match "feature/*") 
{    
    $version = [int]$minorVersion
    $version = $version + 1
    $minorVersion = [string]$version;
    $buildNumber = "${majorVersion}.${minorVersion}.0-Fea${buildCounter}"
    $deployEnv = "Development"
}
elseif ($branch -match "hotfix/*") 
{
    $version = [int]$patchVersion
    $version = $version + 1
    $patchVersion = [string]$version;
    $buildNumber = "${majorVersion}.${minorVersion}.${patchVersion}-Fix${buildCounter}"
    $deployEnv = "Prestage"
}
else
{
    if ($branch.Contains("/")) 
    {
        $branch = $branch.substring($branch.lastIndexOf("/")).trim("/")
    }
    if ("${majorVersion}.${minorVersion}.${patchVersion}-${branch}${buildCounter}".length -gt 20)
    {
        $branch = $branch.substring(0, 20 - "${majorVersion}.${minorVersion}.${patchVersion}-${buildCounter}".length);
    }
    $buildNumber = "${majorVersion}.${minorVersion}.${patchVersion}-${branch}${buildCounter}"
    $deployEnv = "Development"
}

$buildNumber = $buildNumber -replace "_", ""
Write-Host "buildNumber: $buildNumber"
Write-Host " deployEnv:  $deployEnv "
Write-Host "##vso[task.setvariable variable=PackageId;]$buildNumber"
Write-Host "Set environment variable to ($env:PackageId)"
