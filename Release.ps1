
function Edit-XmlNodes {
param (
    [xml] $doc = $(throw "doc is a required parameter"),
    [string] $xpath = $(throw "xpath is a required parameter"),
    [string] $value = $(throw "value is a required parameter"),
    [bool] $condition = $true
)    
    if ($condition -eq $true) {
        $nodes = $doc.SelectNodes($xpath)
         
        foreach ($node in $nodes) {
            if ($node -ne $null) {
                if ($node.NodeType -eq "Element") {
                    $node.InnerXml = $value
                }
                else {
                    $node.Value = $value
                }
            }
        }
    }
}

function Get-ReleaseEntriesFromFile{
param(
    [string] $file = $(throw "file is a required parameter")
)
    $lines = Get-Content $file

    Write-Host "Lines: $lines"
    Write-Output "Lines: $lines"

    $matches = select-string -path $file -pattern "^(?<hash>[0-9a-fA-F]{40})\s+(?<filename>\S+(?<version>\d+\.\d+\.\d+\.\d+)(?:\-(?<fullOrDelta>\w+)){0,1}\S+)\s+(?<size>\d+)[\r]*$" -allmatches

    if ($matches -eq $null) { 
        return @() 
    }

    return $matches.Matches | %{ 
            $releaseEntry = New-Object -TypeName PSObject 
            $releaseEntry | Add-Member -MemberType NoteProperty -Name Version -Value (New-Object version($_.Groups["version"].Value)) 
            $releaseEntry | Add-Member -MemberType NoteProperty -Name FileName -Value $_.Groups["filename"].Value
            $releaseEntry | Add-Member -MemberType NoteProperty -Name Hash -Value $_.Groups["hash"].Value
            $releaseEntry | Add-Member -MemberType NoteProperty -Name Size -Value $_.Groups["size"].Value
            $releaseEntry | Add-Member -MemberType NoteProperty -Name IsFullOrDelta -Value $_.Groups["fullOrDelta"].Value
            $releaseEntry | Add-Member -MemberType NoteProperty -Name RawLine -Value $_.Value
     
            $releaseEntry
        }
}

function Write-RoutingRules{
    param(
        [version] $version = $(throw "version is a required parameter"),
        [string] $key = $(throw "key is a required parameter"),
        [string] $bucket = $(throw "bucket is a required parameter")
    )

       $website = Get-S3BucketWebsite -BucketName $bucket
       $routingRule = ($website.RoutingRules | ?{ $_.Condition.KeyPrefixEquals -contains "$key/Setup.exe" } | select -First 1)
       $routingRule = if ($routingRule -eq $null) 
       { 
            $rule = New-Object Amazon.S3.Model.RoutingRule 
            $rule.Condition = New-Object Amazon.S3.Model.RoutingRuleCondition
            $rule.Condition.KeyPrefixEquals = "$key/Setup.exe"
            $rule.Redirect = New-Object Amazon.S3.Model.RoutingRuleRedirect
            $website.RoutingRules.Add($rule)
            $rule
       } 
       else 
       { $routingRule }

       $routingRule.Redirect.ReplaceKeyWith = "$key/Setup_$version.exe"

       $website.IndexDocumentSuffix = if ($website.IndexDocumentSuffix -eq $null -or $website.IndexDocumentSuffix -eq "") {"index.htm"} else {$website.IndexDocumentSuffix}

       Write-S3BucketWebsite -BucketName $bucket -WebsiteConfiguration_RoutingRule $website.RoutingRules -WebsiteConfiguration_IndexDocumentSuffix $website.IndexDocumentSuffix
}

#Import-Module "C:\Program Files (x86)\AWS Tools\PowerShell\AWSPowerShell\AWSPowerShell.psd1"

function Run{
    Set-AWSCredentials -AccessKey $env:s3accessKey -SecretKey $env:s3secretaccessKey

    $version = $env:APPVEYOR_BUILD_VERSION
    $name = "LingSubPlayer"
    $nugetOutputDir = $PSScriptRoot +"\NuGetPack"
    $nuspec = $PSScriptRoot+"\"+$name+".nuspec"
    $squirrelReleaseDir = "$PSScriptRoot\Releases"
    $s3Bucket = "lingsubplayer.yezutov.com"
    $s3Key = "releases/dev"

    
    Write-Output "Updating version in NuSpec"  
    $xml = [xml](Get-Content $nuspec)
    Edit-XmlNodes $xml -xpath "/package/metadata/version" -value $version
    $xml.Save($nuspec)   
    

    Write-Output "Running NuGet"
    $nuget = (Get-ChildItem -Path $PSScriptRoot -Filter Nuget.exe -Recurse).FullName
    &$nuget "pack" $nuspec "-OutputDirectory" "$nugetOutputDir" | Write-Output
    Write-Output "NuGet completed"
    
    
    $releasesFile = Read-S3Object -BucketName $s3Bucket -Key "$s3Key/RELEASES" -File "$squirrelReleaseDir\RELEASES"

    Write-Host "Release file downloaded: $releasesFile"
    
    Write-Output "Running Squirrel"
    $squirrel = (Get-ChildItem -Path $PSScriptRoot -Filter Squirrel.exe -Recurse).FullName
    &$squirrel --releasify "$nugetOutputDir\$name.$version.nupkg" "--setupIcon=$PSScriptRoot\Mockups\favicon.ico" "--releaseDir=$squirrelReleaseDir" | Write-Output
    Write-Output "Squirrel is completed"    

    #leave unique entries in file 
    [array] $releaseEntries = Get-ReleaseEntriesFromFile -file $releasesFile
    $releaseEntries = $releaseEntries | group { "$($_.Version)-$($_.IsFullOrDelta)" } | select @{Name="LastInGroup";Expression={$_.Group[$_.Count-1]}} | select -ExpandProperty LastInGroup
    [string[]] $releaseEntriesStrings = $releaseEntries | select -ExpandProperty RawLine
    if ($releaseEntries -eq $null){
        $releasesEntries = @();
    }
    if ($releaseEntriesStrings -eq $null){
        $releaseEntriesStrings = @();
    }    

    Write-Output "Uploading RELEASES"
    Write-Output ($releaseEntriesStrings | Out-String)

    #Write-S3Object -BucketName $s3Bucket -Key "$s3Key/RELEASES" -Content ($releaseEntriesStrings | Out-String)

    $squirrelNuGetPackageFileName = $releaseEntries[$releaseEntries.Length - 1].FileName

    Get-ChildItem -Path "$squirrelReleaseDir"

    Write-Output "Uploading NuGet package $squirrelNuGetPackageFileName"
    #Write-S3Object -BucketName $s3Bucket -Key "$s3Key/$squirrelNuGetPackageFileName" -File "$squirrelReleaseDir\$squirrelNuGetPackageFileName"

    Write-Output "Uploading Setup.exe"
    #Write-S3Object -BucketName $s3Bucket -Key "$s3Key/Setup_$($releaseEntries[$releaseEntries.Length - 1].Version).exe" -File "$squirrelReleaseDir\Setup.exe"
    
    Write-Output "Updating Routing rules"
    #Write-RoutingRules -Version $releaseEntries[$releaseEntries.Length - 1].Version -Key $s3Key -Bucket $s3Bucket
}

$ErrorActionPreference = "Stop"

Run