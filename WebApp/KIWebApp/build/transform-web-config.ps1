param (
	[Parameter(Mandatory=$true)][string]$secretpath,
	[Parameter(Mandatory=$true)][string]$webconfigpath,
	[hashtable]$extraproperties
)

#$secretpath = "C:\Users\david\Documents\Secrets\ki-local-config.xml"
#$web_config_path = "C:\Users\david\Documents\GitHub\kaukasusinsurgency-api\WebApp\KIWebApp\Test.Web.config"

function decorate_config_property($prop) {
	'${' + $prop + '}'
}

[xml]$config = Get-Content $secretpath
$webconfig = Get-Content $webconfigpath -Raw
$config_nodes = $config.SelectNodes("//Config//*")

foreach ($node in $config_nodes) {
    $property_name = decorate_config_property $node.LocalName
	Write-Host "replacing " $property_name
	$webconfig = $webconfig.replace($property_name, $node.InnerXml)
}

$extraproperties.GetEnumerator() | ForEach-Object { 
	$property_name = decorate_config_property $_.Key
	Write-Host "replacing " $property_name
	$webconfig = $webconfig.replace($property_name, $_.Value)
}

Set-Content -Path $webconfigpath -Value $webconfig

