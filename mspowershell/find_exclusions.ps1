
function mysort([dhcp_range[]] $range)
{
   [int] $i = 0
   while($i -lt $range.Length)
   {
        $min = New-Object dhcp_ip_address
        $min.set_address("255.255.255.255")
        $min_index = -1
        [int]$j = $i
        while($j -lt $range.Length)
        {
            [dhcp_ip_address] $low = $range[$j].get_low_addr()
            if($min.compare($low) -eq 1)
            {
                $min = $range[$j].get_low_addr()
                $min_index = $j
            }
            $j++
        }
        $temp = $range[$i]
        $range[$i] = $range[$min_index]
        $range[$min_index] = $temp
        $i++
   }
   return $range

}

function find_all_exclusions([dhcp_range[]] $range, [dhcp_ip_address] $min_ip, [dhcp_ip_address] $max_ip)
{
    
    [dhcp_range[]]$exclusions = @()
    $sorted_range = mysort $range
    [int] $i = 0
    if($min_ip.get_next_address().compare($sorted_range[$i].get_low_addr().get_previous_address()) -ne 1)
    {
        
        $to_add = New-Object dhcp_range
        $to_add.set_addr($min_ip.get_next_address(),$sorted_range[$i].get_low_addr().get_previous_address())
        $log = $min_ip.get_next_address().ip_address + "-" + $sorted_range[$i].get_low_addr().get_previous_address().ip_address + " has been added"
        writeLog $log 
        $exclusions = $exclusions + $to_add
    }
    while($i -lt $sorted_range.Length-1)
    {
        
        $val1 = New-Object dhcp_ip_address
        $val2 = New-Object dhcp_ip_address
        if($sorted_range[$i].check)
        {
            $val1 = $sorted_range[$i].get_high_addr().get_next_address()
        }
        else
        {
            $val1 = $sorted_range[$i].get_low_addr().get_next_address()
        }
        $val2 = $sorted_range[$i+1].get_low_addr().get_previous_address()
        if($val1.compare($val2) -ne 1)
        {
            $to_add = New-Object dhcp_range
            $to_add.set_addr($val1,$val2)
            $log = $val1.ip_address + "-" + $val2.ip_address + " has been added"
            writeLog $log
            $exclusions = $exclusions + $to_add
        }
        $i++
    }
    if($sorted_range[$sorted_range.Length-1].check -eq 1)
    {
        $val1 = $sorted_range[$sorted_range.Length-1].get_high_addr().get_next_address()
    }
    else
    {
        $val1 = $sorted_range[$sorted_range.Length-1].get_low_addr().get_next_address()
    }
    #Write-Host $max_ip.ip_address
    $val2 = $max_ip.get_previous_address()
    if($val1.compare($val2) -ne 1)
    {
        $to_add = New-Object dhcp_range
        $to_add.set_addr($val1,$val2)
        $log = $val1.ip_address + "-" + $val2.ip_address + " has been added"
        writeLog $log
        $exclusions = $exclusions + $to_add
    }
    return $exclusions
}
