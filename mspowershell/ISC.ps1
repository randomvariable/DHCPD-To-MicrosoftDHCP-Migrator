$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$loop = 1

#Push-Location $MyInvocation.MyCommand.Path
#[Environment]::CurrentDirectory = $PWD
#return
while($loop)
{
    try
    {
        $allow_deny = $dir + "\allow_deny.ps1"
        $common = $dir + "\common.ps1"
        $iscv4C = $dir + "\ISCv4Categorization.ps1"
        $options = $dir + "\options.ps1"
        $find_exclusions = $dir + "\find_exclusions.ps1"
        $remove_groups_shared_networks = $dir + "\remove_groups_shared_networks.ps1"
        $iscv4 = $dir + "\ISCv4.ps1"
        $xml_generator = $dir + "\XML_generator.ps1"
        $class_policies = $dir + "\class_policies.ps1"
        $if_else = $dir + "\if_else.ps1"
        $test_subnet = $dir + "\Test-Subnet.ps1"
        $convertToMicrosoftDataStructures = $dir + "\convertToMicrosoftDataStructures.ps1"
        $find_reservations = $dir + "\find_reservations.ps1"
        $option_definitions = $dir + "\optionDefinitions.ps1"
        . $allow_deny
        . $common
        . $iscv4C
        . $options
        . $find_exclusions
        . $remove_groups_shared_networks
        . $iscv4
        . $xml_generator
        . $class_policies
        . $if_else
        . $test_subnet
        . $convertToMicrosoftDataStructures
        . $find_reservations
        . $option_definitions
        $loop = 0
    }
    catch
    {
        $dir = Read-Host "Sorry! unable to find the location of the script files. please enter it manually"
    }
}


read_isc_file

$read_destination_path = Read-Host "Please enter the destination path for the Xml file"
#del -force "C:\Users\sabansal\Desktop\Migration\ISC\output.xml"
#$read_destination_path = "C:\Users\sabansal\Desktop\Migration\ISC\output8.xml";
$loop = xml_initialize $read_destination_path 
while($loop)
{
    $read_destination_path = Read-Host "Sorry! Error, enter destination path again"
    $loop = xml_initialize $read_destination_path 
    
}
$global:log_file_path = $dir + "\LogFile.txt"
$global:stat_file_path = $dir + "\StatFile.txt"
 
Set-Content -Value "Log File for ISC DHCP Server Migration to Microsoft DHCP Server" -Path $global:log_file_path
Set-Content -Value "The final statistics are: " -Path $global:stat_file_path


 ##########
$global:isc_config = @()

[int] $cur_index = 0
[int] $max_index = 0
[int[]]$global:scopesIPv4 = @()
[int[]]$global:superscopesIPv4 = @()
[int[]]$global:reservationsIPv4 = @()
[int[]]$global:groupsIPv4 = @()
[int] $brace_count = 0
$global:if_statements = @()
$global:classes = @()
$global:subclasses = @()
$global:final_classes = @()
$global:microsoft_classes = @()
$global:option_space_map = @{}
$global:unmigrated_hosts = @()
$global:migrated_hosts = @()
$global:unmigrated_pools = @()
$global:unmigrated_scopes = @()
$global:unmigrated_ranges = @{}

$global:subnetmask_hash = @{}
$global:ctr = 0
[bool]$global:v6 = 0

ISCv4



migrate_global
migrate_scopes
migrate_reservations
migrate_option_definitions
migrate_classes











#$master_table,$scopes,$option_definitions,$microsoft_classes
DHCPserver $global:isc_config $scopesIPv4 $global:isc_config[0].Get_Item("option-definitions") $global:microsoft_classes

writeStat "The folowing scopes could not be migrated as there was no corresponding range declaration: "

foreach($scope_id in $global:unmigrated_scopes)
{
    $subnet = $global:isc_config[$scope_id].Get_Item("subnet")
    $netmask = $global:isc_config[$scope_id].Get_Item("netmask")
    $stat = "Subnet: " + $subnet + ", Netmask: " + $netmask
    writeStat $stat
}
writeStat "---------------------------------------------------------------------------------------"

writeStat "The folowing host declarations could not be mapped: "
writeStat "Possible reasons: a) Duplicate MAC addresses for 2 reservations in the same scope"
writeStat "                  b) No fixed-address statement present"
writeStat "                  c) Reservation defined out of range of the newly-configured scopes"
foreach($reservation_id in $global:unmigrated_hosts)
{
    $name = $global:isc_config[$reservation_id].Get_Item("name")
    $stat = "Host: " + $name
    writeStat $stat
}
writeStat "---------------------------------------------------------------------------------------"
writeStat "The folowing pools could not be migrated as they were defined at a shared-network level"
foreach($pool_id in $global:unnmigrated_pools)
{
    $name = $global:isc_config[$global:isc_config[$pool_id].Get_Item("parent-index")].Get_Item("name")
    $stat = "Pool in Shared Network " + $name
    writeStat $stat
}
writeStat "---------------------------------------------------------------------------------------"

$stat = "The folowing range declarations could not be updated to the MS server. This occured because microsoft dhcp server does not allow different policies (allow/deny/ignore/if/elsif) statements with overlapping range"
writeStat $stat
foreach($subnet in $global:unmigrated_ranges.Keys)
{
    $stat = "ScopeId: " + $subnet
    writeStat $stat
    foreach($range in $global:unmigrated_ranges.Get_Item($subnet))
    {
        $stat = $range.low_addr.ip_address + "-" + $range.high_addr.ip_address
        writeStat $stat
    }
}

writeStat "---------------------------------------------------------------------------------------"

writeStat "Final Statistics"

$stat = "Total Number of Subnets in ISC conf File: " + $global:scopesIPv4.Length 
writeStat $stat

$stat = "Subnets not migrated to corresponding Scopes: " + $global:unmigrated_scopes.Length
writeStat $stat

$stat = "Subnets migrated to corresponding Scopes: " + ($global:scopesIPv4.Length -$global:unmigrated_scopes.Length)
writeStat $stat


$stat = "Total Number of Host Declarations in the ISC conf File: " + $global:reservationsIPv4.Length
writeStat $stat

$stat = "Host Declarations not migrated to corresponding Scopes: " + $global:unmigrated_hosts.Length
writeStat $stat

$stat = "Host Declarations migrated to corresponding Scopes: " + ($global:reservationsIPv4.Length - $global:unmigrated_hosts.Length)
writeStat $stat

$stat = "Total Number of pools that could not be migrated to the Microsoft Server: " + $global:unmigrated_pools.Length
writeStat $stat


