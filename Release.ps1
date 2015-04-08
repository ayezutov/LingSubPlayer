
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

$version = "0.1.0.11-dev"
$name = "LingSubPlayer"
$nugetOutputDir = $PSScriptRoot +"\NuGetPack"
$nuspec = $PSScriptRoot+"\"+$name+".nuspec"


# Release during any build:
#0. Ensure, that generic current version was not yet uploaded to NuGet, fail it was
#1. download and unpack previous version from NuGet
#2. create nupkg from current version based on latest version
#3. upload current version option



# Deployment:
#0. verify, that 


$xml = [xml](Get-Content $nuspec)
Edit-XmlNodes $xml -xpath "/package/metadata/version" -value $version
$xml.Save($nuspec)

$squirrel = (Get-ChildItem -Path $PSScriptRoot -Filter Squirrel.exe -Recurse).FullName
$nuget = (Get-ChildItem -Path $PSScriptRoot -Filter Nuget.exe -Recurse).FullName

"Running NuGet"

&$nuget "pack" $nuspec "-OutputDirectory" "$nugetOutputDir"
"Running Squirrel"
&$squirrel --releasify "$nugetOutputDir\$name.$version.nupkg" "--setupIcon=$PSScriptRoot\Mockups\favicon.ico" "--releaseDir=$PSScriptRoot\Releases" "--bootstrapperExe=.\LingSubPlayer.Setup.exe"
"Squirrel is completed"

#make sure duplicated entries are removed from RELEASES
