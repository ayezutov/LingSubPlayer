. "$PSScriptRoot\deployment.settings.ps1"
#. "$PSScriptRoot\deployment.keys.ps1"
# Import-Module "C:\Program Files (x86)\AWS Tools\PowerShell\AWSPowerShell\AWSPowerShell.psd1"

Set-AWSCredentials -AccessKey $env:s3accessKey -SecretKey $env:s3secretaccessKey


function Get-ReleaseEntriesFromFile{
param(
    [string] $file = $(throw "file is a required parameter")
)
    $lines = Get-Content $file

    $matches = select-string -path $file -pattern "^(?<hash>[0-9a-fA-F]{40})\s+(?<filename>\S+(?<version>\d+\.\d+\.\d+\.\d+)(?:\-(?<fullOrDelta>\w+)){0,1}\S+)\s+(?<size>\d+)[\r]*$" -allmatches
    
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

function Get-ReleasesForChannel{
param(
    [string] $channelName = $(throw "channelName is a required parameter"),
    [uri]    $uri = $(throw "uri is a required parameter")
)
    $webclient = New-Object System.Net.WebClient
    $file = "$PSScriptRoot\$channelName\RELEASES"
    $directory = $file | Split-Path
    $directory = New-Item -Path $directory -ItemType Directory -Force
    try{
        $temp = Read-S3Object -BucketName $settings.amazon.bucketName -Key "$uri/RELEASES" -File $file
        
        if ([System.IO.File]::Exists($file))
        { 
            return Get-ReleaseEntriesFromFile -file $file
        }
        else
        {
            return $null;
        }
    }
    catch [Amazon.S3.AmazonS3Exception]
    {
        Write-Host "There was an error when trying to get file for channel $channelName : $_"
        return $null
    }
}


$devReleases = Get-ReleasesForChannel -uri $settings.channels.dev.url -channelName "dev"

$maxDevVersion = ($devReleases | Measure-Object -Maximum -Property Version).Maximum

Write-Output "Maximum version available: $maxDevVersion"

$settings.channels.GetEnumerator() | % { 
    $channelName = $_.key;
    $channel = $_.value;
    $targetChannelVersion = $channel.version;
    
    if ($channelName -ne "dev") {
       $channelReleases = Get-ReleasesForChannel -uri $channel.url -channelName $channelName

       if ($channelReleases -eq $null)
       {
            Write-Warning "Unable to get releases for channel '$channelName'"
       }

       $maxChannelVersion = if ($channelReleases -eq $null) { New-Object version("0.0.0.0") } else { ($channelReleases | Measure-Object -Maximum -Property Version).Maximum }
       Write-Output "Attempt to upgrade channel '$channelName' from '$maxChannelVersion' to '$targetChannelVersion'";

       if ($targetChannelVersion -eq $maxChannelVersion)
       {
           Write-Output "'$channelName': No need to upgrade, already up to date"
		   #Add-AppveyorMessage -Message "'$channelName': No need to upgrade, already up to date" -Category Information
           return
       }

       if ($targetChannelVersion -gt $maxDevVersion){
           throw ("Cannot upgrade channel '$channelName' to version '$targetChannelVersion' which is higher than last developed '$maxDevVersion'")
		   #Add-AppveyorMessage -Message "Cannot upgrade channel '$channelName' to version '$targetChannelVersion' which is higher than last developed '$maxDevVersion'" -Category Error
       }

       if ($targetChannelVersion -lt $maxChannelVersion){
           Write-Warning "ATTENTION: channel '$channelName' will be downgraded to version '$targetChannelVersion'"
		   #Add-AppveyorMessage -Message "ATTENTION: channel '$channelName' will be downgraded to version '$targetChannelVersion'" -Category Warning
       }

       $newFileName = "$PSScriptRoot\$channelName\RELEASES-new"
       $result = ($channelReleases | ?{ $_.version -le $targetChannelVersion } | select -ExpandProperty RawLine)
       $result += ($devReleases | ?{ $_.version -gt $maxChannelVersion -and $_.version -le $targetChannelVersion } | select -ExpandProperty RawLine)

       $result | Out-File -FilePath $newFileName

       Write-S3Object -BucketName $settings.amazon.bucketName -Key "$($channel.url)/RELEASES" -File $newFileName

       $website = Get-S3BucketWebsite -BucketName $settings.amazon.bucketName
       $routingRule = ($website.RoutingRules | ?{ $_.Condition.KeyPrefixEquals -contains "$($channel.url)/Setup.exe" } | select -First 1)
       $routingRule = if ($routingRule -eq $null) 
       { 
            $rule = New-Object Amazon.S3.Model.RoutingRule 
            $rule.Condition = New-Object Amazon.S3.Model.RoutingRuleCondition
            $rule.Condition.KeyPrefixEquals = "$($channel.url)/Setup.exe"
            $rule.Redirect = New-Object Amazon.S3.Model.RoutingRuleRedirect
            $website.RoutingRules.Add($rule)
            $rule
       } 
       else 
       { $routingRule }

       $routingRule.Redirect.ReplaceKeyWith = "$($settings.channels.dev.url)/Setup_$targetChannelVersion.exe"

       $website.IndexDocumentSuffix = if ($website.IndexDocumentSuffix -eq $null -or $website.IndexDocumentSuffix -eq "") {"index.htm"} else {$website.IndexDocumentSuffix}

       Write-S3BucketWebsite -BucketName $settings.amazon.bucketName -WebsiteConfiguration_RoutingRule $website.RoutingRules -WebsiteConfiguration_IndexDocumentSuffix $website.IndexDocumentSuffix
    }
    else {
        Write-Debug "dev channel is skipped"
    }
}
