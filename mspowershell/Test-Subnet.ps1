Function Test-SameSubnet {
param (
[parameter(Mandatory=$true)]
[Net.IPAddress]
$ip1,

[parameter(Mandatory=$true)]
[Net.IPAddress]
$ip2,

[parameter()]
[alias("SubnetMask")]
[Net.IPAddress]
$mask ="255.255.255.0"
)

if (($ip1.address -band $mask.address) -eq ($ip2.address -band $mask.address)) {return $true}
else {return $false}

} 

function binary_and([Net.IPAddress]$val1,[Net.IPAddress]$val2)
{
    $to_return = ($val1.address -band $val2.address)
    return $to_return
}
