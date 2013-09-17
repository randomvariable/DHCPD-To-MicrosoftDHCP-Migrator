

function find_reservations
{
    foreach($reservation_id in $global:reservationsIPv4)
    {
        if($global:isc_config[$reservation_id].ContainsKey("fixed-address"))
        {
            $fixed_address_array = $global:isc_config[$reservation_id].Get_Item("fixed-address")
            foreach($ip_address in $fixed_address_array)
            {
                foreach($subnetmask in $global:subnetmask_hash.Keys)
                {
                    $hash = $global:subnetmask_hash.Get_Item($subnetmask)
                    $band = binary_and $ip_address.ip_address $subnetmask
                    if($hash.ContainsKey($band))
                    {
                        $scope_index = $hash.Get_Item($band)
                        $mac_array = @()
                        $reservations = @{}
                        if($global:isc_config[$scope_index].ContainsKey("mac-array")) 
                        {
                            $mac_array = $global:isc_config[$scope_index].Get_Item("mac-array")
                            $global:isc_config[$scope_index].Remove("mac-array")
                        }   
                   
                        if($mac_array -Contains($global:isc_config[$reservation_id].Get_Item("hardware-address")))
                        {
                            if($mac_array.Length -ne 0) {$global:isc_config[$scope_index].Add("mac-array",$mac_array)}
                            $log = "The folowing reservation failed due to duplicate mac address: " + $global:isc_config[$reservation_id].Get_Item("name") + ", IP Address: " + $ip_address
                            $global:unmigrated_hosts = $global:unmigrated_hosts+ $reservation_id
                            writeLog $log
                        }
                        else
                        {
                        
                            if($global:isc_config[$scope_index].ContainsKey("reservations"))
                            {
                                $reservations = $global:isc_config[$scope_index].Get_Item("reservations")
                                $global:isc_config[$scope_index].Remove("reservations")
                            }

                            $reservations.Add($reservation_id,$ip_address)
                            $mac_array += $global:isc_config[$reservation_id].Get_Item("hardware-address")

                            $global:isc_config[$scope_index].Add("reservations",$reservations)
                            $global:isc_config[$scope_index].Add("mac-array",$mac_array)
                        }
                    
                        break
                    }
                }
            }
        }
        else
        {
            $log = "The folowing reservation failed as there was no fixed address statement: " + $global:isc_config[$reservation_id].Get_Item("name")
            writeLog $log
            $global:unmigrated_hosts = $global:unmigrated_hosts+ $reservation_id
        }
    }
}



function find_reservations_old
{
    foreach($reservation_id in $global:reservationsIPv4)
    {
        $fixed_address_array = $global:isc_config[$reservation_id].Get_Item("fixed-address")
        foreach($ip_address in $fixed_address_array)
        {
            foreach($scope_index in $global:scopesIPv4)
            {
                $scopeid = $global:isc_config[$scope_index].Get_Item("subnet")
                $subnetnumber = $global:isc_config[$scope_index].Get_Item("netmask")
                if(Test-SameSubnet $scopeid $ip_address.ip_address $subnetnumber)
                {
                    
                    $mac_array = @()
                    $reservations = @{}
                    if($global:isc_config[$scope_index].ContainsKey("mac-array")) 
                    {
                        $mac_array = $global:isc_config[$scope_index].Get_Item("mac-array")
                        $global:isc_config[$scope_index].Remove("mac-array")
                    }   
                   
                    if($mac_array -Contains($global:isc_config[$reservation_id].Get_Item("hardware-address")))
                    {
                        if($mac_array.Length -ne 0) {$global:isc_config[$scope_index].Add("mac-array",$mac_array)}
                        $log = "The folowing reservation failed due to duplicate mac address: " + $global:isc_config[$reservation_id].Get_Item("name") + ", IP Address: " + $ip_address
                        writeLog $log
                    }
                    else
                    {
                        
                        if($global:isc_config[$scope_index].ContainsKey("reservations"))
                        {
                            $reservations = $global:isc_config[$scope_index].Get_Item("reservations")
                            $global:isc_config[$scope_index].Remove("reservations")
                        }

                        $reservations.Add($reservation_id,$ip_address)
                        $mac_array += $global:isc_config[$reservation_id].Get_Item("hardware-address")

                        $global:isc_config[$scope_index].Add("reservations",$reservations)
                        $global:isc_config[$scope_index].Add("mac-array",$mac_array)
                    }
                    
                    break
                }
            }
        }
    }
}