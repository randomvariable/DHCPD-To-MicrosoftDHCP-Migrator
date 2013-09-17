
##################FUNCTION : GET option data type #######################################
function get_option_data_type([string] $type)
{
    if($type.CompareTo("boolean") -eq 0)
    {
        return "Byte"
    }
    if($type.CompareTo("ip-address") -eq 0)
    {
        return "IPv4Address"
    }
    if($type.CompareTo("string") -eq 0 -or $type.CompareTo("text") -eq 0)
    {
        return "String"
    }
    if($type.CompareTo("encapsulate") -eq 0)
    {
        return "Encapsulated"
    }
    if($type.CompareTo("signed") -eq 0 -or $type.CompareTo("unsigned") -eq 0)
    {
        $type =next_token
    }
    if($type.CompareTo("integer") -eq 0)
    {
        $type = next_token
        switch($type)
        {
            "8"  {return "Byte" }
            "16" {return "Word" }
            "32" {return "Long" }
        }
    }
}

function to_ip($text)
{
    

    
    if([System.Char]::IsDigit($text[$text.Length-1]))
    {
        return $text
    }
    try
    {
        return ([System.Net.Dns]::GetHostAddresses($text).IPAddressToString)

    }
    catch
    {
        return "-1"
    }
}

##########################################CODE BEGINS HERE#############################################

function writeLog($text)
{
    Add-Content -Value $text -Path $global:log_file_path
    
}


function writeStat($text)
{
    Add-Content -Value $text -Path $global:stat_file_path
}

    
###################SERVER LEVEL CONFIGURATIONS  ARE STORED IN $global##################

 function ISCv4
 {
    
    
    $global = @{"parent-index" = -1; "type" = 1 }
    $global:isc_config = $global:isc_config + $global
    [option_record[]]$standard_option_array = @()
    foreach($standard_option in $global:standard_options.Keys)
    {
        [int] $id = ($global:standard_options.Get_Item($standard_option))
    
        $temp = New-Object option_record 
        $temp.init($standard_option, $id)
        $standard_option_array = $standard_option_array + $temp
    }
    $option_definition = @{"standard_options"=$standard_option_array}
    $global:isc_config[0].Add("option-definitions",$option_definition)

    write-host "Parsing begins"
    writeLog("Parsing Begins`n-------------------------------------------------")
    $command = next_token
    
    while($command.CompareTo("") -ne 0)
    {
        
        #Write-Host $command
        $category = IPv4category $command
        switch($category)
        {
            #####################                        Topology            ##########################
            "1"
            {
                
                $child_array = @()
                if($global:isc_config[$cur_index].ContainsKey("child-index"))
                {
                    $child_array = $global:isc_config[$cur_index].Get_Item("child-index")
                    $global:isc_config[$cur_index].Remove("child-index")
                }
                $max_index++
                $child_array = $child_array + $max_index
                $global:isc_config[$cur_index].Add("child-index",$child_array)
                $temp = @{"parent-index" = $cur_index; "my-index" = $max_index}
                $global:isc_config = $global:isc_config + $temp
                $cur_index = $max_index 
                $accept_next_token = 1
                switch($command)
                {
                    "group"
                    {
                        writeLog "Group Statement Encountered"
                        $global:groups = $global:groups + $cur_index
                        $global:isc_config[$cur_index].Add("type",2)
                    }
                    "shared-network"
                    {
                        $log = "Shared-Network " + $command + " is now being parsed"
                        writeLog $log
                        $global:superscopesIPv4 =$global:superscopesIPv4 + $cur_index
                        $global:isc_config[$cur_index].Add("type",3)
                        $command = next_token
                        $global:isc_config[$cur_index].Add("name",$command)
                    }
                    "subnet"
                    {
                        $log = "Subnet "
                        $global:scopesIPv4 = $global:scopesIPv4 + $cur_index
                        $global:isc_config[$cur_index].Add("type",4)
                        $command = next_token
                        
                        $temp = $command
                        $command = to_ip $command
                        
                        if($command.CompareTo("-1") -eq 0)
                        {
                            $log = "Error subnet " + $temp + " could not be resolved to ip address. Please check your internet connection or manually correct the subnet"
                            writeLog $log
                            $subnet_check = 1

                        }
                        
                        $global:isc_config[$cur_index].Add("subnet",$command)
                        $subnet = $command
                        
                        $log += $command + " netmask "
                        $command = next_token
                        $command = next_token
                        $temp = $command
                        $command = to_ip $command
                        if($command.CompareTo("-1") -eq 0)
                        {
                            $log = "Error netmask " + $temp + " could not be resolved to ip address. Please check your internet connection or manually correct the subnet"
                            writeLog $log
                            $netmask_check = 1

                        }
                        $global:isc_config[$cur_index].Add("netmask",$command)
                        $netmask = $command
                        $band = binary_and $subnet $netmask
                        
                        $subnetip=[Net.IPAddress]$subnet;
                        $netmaskip=[Net.IPAddress]$netmask;
                        $subnetbytes = $subnetip.GetAddressBytes();
                        $netmaskbytes = $netmaskip.GetAddressBytes();
                        $c = 0;
                        $destinationip = [byte[]]($subnetbytes | %{ $_ -bor ($netmaskbytes[$c++] -bxor 255); })
                        $subnetbytes[$subnetbytes.Length - 1] = $subnetbytes[$subnetbytes.Length - 1] + 1;
                        $destinationip[$destinationip.Length - 1] = $destinationip[$destinationip.Length - 1] - 1;
                        
                        $hash = @{}
                        if($global:subnetmask_hash.ContainsKey($netmask))
                        {
                            $hash = $global:subnetmask_hash.Get_Item($netmask)
                            $global:subnetmask_hash.Remove($netmask)
                        }
                        $hash.Add($band,$cur_index)
                        $global:subnetmask_hash.Add($netmask,$hash)
                        $log+= $command
                        writeLog $log
                        $max_ip = New-Object dhcp_ip_address
                        $max_ip.set_address(([Net.IPAddress]$destinationip).IPAddressToString)
                        $min_ip = New-Object dhcp_ip_address
                        $min_ip.set_address(([Net.IPAddress]$subnetbytes).IPAddressToString)
                        $global:isc_config[$cur_index].Add("is-Set-Range",0)
                        $global:isc_config[$cur_index].Add("max-ip",$max_ip)
                        $global:isc_config[$cur_index].Add("min-ip",$min_ip)
                        $global:isc_config[$cur_index].Add("reserved-ips",@{})
                        $global:isc_config[$cur_index].Add("pools",@())
                        if($subnet_check -or $netmask_check)
                        {
                            throw_out
                            $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                            $accept_next_token = 0
                        }
                        
                    }
                    "pool"
                    {
                        
                        $parent_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                        if($global:isc_config[$parent_index].Get_Item("type") -eq 4)
                        {
                            $pools = $global:isc_config[$parent_index].Get_Item("pools")
                            $pools = $pools + $cur_index
                            $global:isc_config[$parent_index].Set_Item("pools",$pools)
                            $subnet_number = $global:isc_config[$parent_index].Get_Item("subnet")
                            $netmask = $global:isc_config[$parent_index].Get_Item("netmask")
                            $log = "pool in subnet " + $subnet_number + " netmask " + $netmask + " is now being parsed"
                            writeLog $log
                        }
                        else
                        {
                            
                            $global:unmigrated_pools = $global:unmigrated_pools + $cur_index
                            $shared_network = $global:isc_config[$parent_index].Get_Item("name")
                            $log = "Pool in shared Network" + $shared_network + " cannot be migrated"
                            writeLog $log
                        }
                        $global:isc_config[$cur_index].Add("type",5)
                    }
                    "host"
                    {
                        $global:reservationsIPv4 = $global:reservationsIPv4 + $cur_index
                        $global:isc_config[$cur_index].Add("type",6)
                        $command = next_token
                        $global:isc_config[$cur_index].Add("name",$command)
                        $log = "Host " + $command + " is now being parsed"
                        writeLog $log

                    }
                    "class"
                    {
                        $global:classes = $global:classes + $cur_index
                        $name = next_token
                        $log = "Class " + $name + " is now being parsed"
                        writeLog $log
                        $global:isc_config[$cur_index].Add("type",7)
                        $global:isc_config[$cur_index].Add("name",$name)
                        $global:isc_config[$cur_index].Add("class-name",$name)
                        $global:isc_config[$cur_index].Add("has-match",0)
                        $global:isc_config[$cur_index].Add("has-subclasses",0)
                        [int[]]$temp_class_array = @()
                        if($global:isc_config[$global:isc_config[$cur_index].Get_Item("parent-index")].ContainsKey("class"))
                        {
                            $temp_class_array = $global:isc_config[$global:isc_config[$cur_index].Get_Item("parent-index")].Get_Item("class")
                            $global:isc_config[$global:isc_config[$cur_index].Get_Item("parent-index")].Remove("class")
                        }
                        $temp_class_array = $temp_class_array + $cur_index
                        $global:isc_config[$global:isc_config[$cur_index].Get_Item("parent-index")].Add("class",$temp_class_array)

                    }
                    "subclass"
                    {
                        $global:subclasses = $global:subclasses + $cur_index
                        $class_name = next_token
                        
                        $global:isc_config[$cur_index].Add("class-name",$class_name)

                        $global:isc_config[$cur_index].Add("type",8)
                        $match_value = next_token
                        $log = "Subclass whose class is " +$class_name + " and match value is " + $match_value + " is now being parsed" 
                        writeLog $log
                        #$match_value.Replace(":","-")
                        $global:isc_config[$cur_index].Add("match-value",$match_value)
                        $check = next_token
                        #write-host $cur_index " " $accept_next_token " " $check
                        if($check.CompareTo(";") -eq 0)
                        {
                            $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                            
                        }
                        $accept_next_token = 0
                        
                    }
                    "if"
                    {
                        writeLog "If statement is now being parsed"
                        $global:isc_config[$cur_index].Add("type",9)
                        $global:if_statements = $global:if_statements + $cur_index
                        $parent_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                        $parent_type = $global:isc_config[$parent_index].Get_Item("type")
                        if($parent_type -eq 9)
                        {
                            foreach($key in $global:isc_config[$parent_index].Keys)
                            {
                                $ignore = "type" , "my-index", "child-index", "parent-index"
                                if($ignore -contains $key) {continue}
                                $global:isc_config[$cur_index].Add($key,$global:isc_config[$parent_index].Get_Item($key))
                            }
                           
                            
                        }
                        else
                        {
                           
                            $global:isc_config[$cur_index].Add("create-policy-index",$parent_index)
                        }
                        $condition = next_token
                        handle_if_else $condition
                        if($global:isc_config[$cur_index].ContainsKey("Throw-If"))
                        {
                            writeLog "If statement could not be parsed as it contains an incorrect if statement. for full details for if-else blocks supported by MS check the Readme"
                            $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                            #Write-Host $cur_index
                        }
                        $accept_next_token = 0
                    }
                    "elsif"
                    {
                        writeLog "elsIf statement is now being parsed"
                        $global:isc_config[$cur_index].Add("type",9)
                        $global:if_statements = $global:if_statements + $cur_index
                        $parent_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                        $parent_type = $global:isc_config[$parent_index].Get_Item("type")
                        if($parent_type -eq 9)
                        {
                            foreach($key in $global:isc_config[$parent_index].Keys)
                            {
                                $ignore = "type" , "my-index", "child-index", "parent-index"
                                if($ignore -contains $key) {continue}
                                $global:isc_config[$cur_index].Add($key,$global:isc_config[$parent_index].Get_Item($key))
                            }
                           
                            
                        }
                        else
                        {
                            $global:isc_config[$cur_index].Add("create-policy-index",$parent_index)
                        }
                        $condition = next_token
                        handle_if_else $condition
                        if($global:isc_config[$cur_index].ContainsKey("Throw-If"))
                        {
                            writeLog "elsIf statement could not be parsed as it contains an incorrect if statement. for full details for if-else blocks supported by MS check the Readme"
                            $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                            #Write-Host $cur_index
                        }
                        $accept_next_token = 0
                    }
                    "else"
                    {
                        writeLog "else statement is now being parsed"
                        $global:isc_config[$cur_index].Add("type",10)
                        $accept_next_token = 0
                        $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                    }
                }
                if($accept_next_token)
                {
                    
                    $command = next_token
                }
            
            }
            #######################                 Boolean Parameters                 ####################
            "2"
            {
                $value = next_token
                $value = $value.ToLower()
                if($value.CompareTo("on") -eq 0 -or $value.CompareTo("true") -eq 0)
                {
                    [bool] $boolean_parameter_value = 1
                    if($global:isc_config[$cur_index].ContainsKey($command))
                    {
                        $global:isc_config[$cur_index].Remove($command)
                    }
                    $global:isc_config[$cur_index].Add($command,$boolean_parameter_value)
                }
                else
                {
                    [bool] $boolean_parameter_value = 0
                    if($global:isc_config[$cur_index].ContainsKey($command))
                    {
                        $global:isc_config[$cur_index].Remove($command)
                    }
                    $global:isc_config[$cur_index].Add($command,$boolean_parameter_value)
                }
                if($command.CompareTo("ping-check") -eq 0)
                {}
                else
                {
                    $log = "The command " +  $command + " has not been used to configure your server and has been ignored!" 
                    writeLog $log
                }
            }
            ####################                  Other Parameters                #########################
            "3"
            {
                $value = next_token            
                
                if($global:isc_config[$cur_index].ContainsKey($command))
                {
                    $global:isc_config[$cur_index].Remove($command)
                }
                $global:isc_config[$cur_index].Add($command,$value)
                if(-not $global:other_imp_parameters -contains $command)
                {
                    writeLog "the command" $command " has not been used to configure your server and has been ignored"
                }
            }
            ###################                     Constructs                     #######################
            "4"
            {
                switch($command)
                {
                    "{"
                    {
                        
                        $brace_count++
                        
                    }
                    "}"
                    {
                        
                         $is_deny = 0;
                        if($brace_count -eq 0)
                        {
                           
                            
                            if($global:isc_config[$cur_index].Get_Item("type") -eq 9 -and (-not $global:isc_config[$cur_index].ContainsKey("Throw-If")) )
                            {
                           
                                $tag = $global:isc_config[$cur_index].Get_Item("tag")
                                $tag_value=$global:isc_config[$cur_index].Get_Item("tag-value")
                                $equality_check = $global:isc_config[$cur_index].Get_Item("equality-check")
                                for($i = 0;$i -lt $tag.Length; $i++)
                                {
                                    if($tag[$i].CompareTo("VendorClass") -eq 0)
                                    {
                                        $option_space = $global:isc_config[$cur_index].Get_Item("vendor-option-space")
                                        
                                        add_vendor_class $tag_value[$i] $option_space
                                        $tag_value[$i] = $tag_value[$i] + "_" + $option_space
                                    }
                                }
                                $parent_index = $global:isc_config[$cur_index].Get_Item("create-policy-index")
                                #Write-Host "Parent index is " $parent_index
                                $policies = @()
                                $policies_options = @()
                                             
                                if($global:isc_config[$parent_index].ContainsKey("policies"))
                                {
                                    $policies = $global:isc_config[$parent_index].Get_Item("policies")
                                    $policies_options = $global:isc_config[$parent_index].Get_Item("policies-options")
                                    $global:isc_config[$parent_index].Remove("policies")
                                    $global:isc_config[$parent_index].Remove("policies-options")
                                }

                                $options = @()
                                $logical_operator = $global:isc_config[$cur_index].Get_Item("logical-operator")
                                if($logical_operator.CompareTo("none") -eq 0)
                                {
                                    $logical_operator = "OR"
                                }
                                if($global:isc_config[$cur_index].ContainsKey("options"))
                                {
                                    $options = $global:isc_config[$cur_index].Get_Item("options")
                                }
                                $policy_to_add = New-Object policy
                                $policy_to_add.init("if/elsif_" +$global:isc_config[$cur_index].Get_Item("my-index"),$policies.Length+1,1, $logical_operator, "This policy is created from an if or elsif block",$tag_value, "EQ",$tag)
                                $policy_to_add.equality_check = $equality_check
                                $policies = $policies + $policy_to_add

                                $policies_options = $policies_options + $options
                                $global:isc_config[$parent_index].Add("policies",$policies)
                                $global:isc_config[$parent_index].Add("policies-options",$policies_options)
                                writeLog "Policy created from the if-else statement"
                            }
                            $cur_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                        }
                        else
                        {
                            $brace_count--
                        }
                    }
                    ";"
                    {
                        Write-Verbose "caught and ignored"
                    }
                }
            
            }
            ############## Special Parameters ######################################
            "5"
            {
                switch($command)
                {
                    "authoritative"
                    {
                        [bool] $boolean_parameter_value = 1
                        if($global:isc_config[$cur_index].ContainsKey($command))
                        {
                            $global:isc_config[$cur_index].Remove($command)
                        }
                        $global:isc_config[$cur_index].Add("authoritative",$boolean_parameter_value)
                        writeLog "the command authoritative is not supported by the Microsoft DHCP server"
                    }
                    "not"
                    {
                        [bool] $boolean_parameter_value = 0
                        if($global:isc_config[$cur_index].ContainsKey($command))
                        {
                            $global:isc_config[$cur_index].Remove($command)
                        }
                        $global:isc_config[$cur_index].Add("authoritative",$boolean_parameter_value)
                        writeLog "the command authoritative is not supported by the Microsoft DHCP server"
                    }
                    "range"
                    {
                        $range = New-Object dhcp_range
                        $range.is_deny = $is_deny;
                        $low = next_token
                        if($low.CompareTo("1") -eq 0 -or $low.CompareTo("0") -eq 0) #dynamic bootp flag
                        {
                            if($low.CompareTo("0") -eq 0)
                            {
                                $global:isc_config[$cur_index].Add("deny-bootp","true")
                                $parent_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                                $parent_type = $global:isc_config[$parent_index].Get_Item("type")
                                if($parent_type -eq 4)
                                {
                                    $global:isc_config[$parent_index].Add("deny-bootp","true")
                                }

                            }
                            $low = next_token
                        }
                        $high = next_token
                        $low_check = to_ip $low
                        $high_check = "1"
                    
                        if($high.CompareTo(";") -ne 0)
                        {
                            $high_check = to_ip $high
                            
                            
                        }
                        if($low_check.CompareTo("-1") -eq 0 -or $high_check.CompareTo("-1") -eq 0)
                        {
                            $log = "The ip-address in this range declaration could not be resolved. Please check your internet connection: " + $low + " $high"
                        }
                        else
                        {
                            
                            
                           
                            if($high.CompareTo(";") -eq 0)
                            {
                                $low = $low_check
                                $high = $low
                           
                                $range.set_addr($low,$high)
                               
                            }
                            else
                            {
                                $high = $high_check
                                $low = $low_check
                                $range.set_addr($low,$high)
                            }

                            $parent_index = $global:isc_config[$cur_index].Get_Item("parent-index")
                            if (4 -eq $global:isc_config[$cur_index].Get_Item("type"))
                            {
                                $configvar = $global:isc_config[$cur_index];
                            }
                            elseif (4 -eq $global:isc_config[$parent_index].Get_Item("type"))
                            {
                                $configvar = $global:isc_config[$parent_index];
                            }
                            else
                            {
                                writeLog "Cannot parse range ";
                                break;
                            }


                            if($configvar.ContainsKey("range")) 
                            {
                                $range_array = $configvar.Get_Item("range")
                            }
                            else
                            {
                                $range_array = @();
                                $configvar.Add("range",$range_array)
                            }

                            $configvar.Set_Item("is-Set-Range",1);
                            $range_array += $range;
                            $configvar["range"] = $range_array;

                            $min_ip = $configvar.Get_Item("min-ip")
                            $max_ip = $configvar.Get_Item("max-ip")
                            $low = $range.get_low_addr()
                            $high = $range.get_high_addr()
                            if($low.compare($min_ip) -eq -1)
                            {
                                $configvar.Set_Item("min-ip",$low)
                            }
                            if($high.compare($max_ip) -eq 1)
                            {
                                $configvar.Set_Item("max-ip",$high)
                            }
                        }
                    }
                    "hardware"
                    {
                    
                    
                        $hardware_type = next_token
                        $hardware_address = next_token
                        $global:isc_config[$cur_index].Add("hardware-type",$hardware_type)
                        $global:isc_config[$cur_index].Add("hardware-address",$hardware_address)

                        writeLog "hardware statement parsed"
                    }
                    "fixed-address"
                    {
                    
                        $address = @()
                        $token = next_token
                        $temp_address = New-Object dhcp_ip_address
                        while($token.CompareTo(";") -ne 0)
                        {
                            $token_check = to_ip $token
                            if($token_check.CompareTo("-1") -eq 0)
                            {
                                $log = "The fixed address " + $token + " could not be resolved to the ip address. Please check your internet connection"
                            } 
                            else
                            {
                                $token = $token_check
                            }
                            $temp_address.set_address($token)
                            $address = $address + $temp_address
                            $token = next_token
                            $temp_address = New-Object dhcp_ip_address
                        }
                        $log = "fixed address parsed: " + [string]::Join(" ",$address.ip_address)
                        writeLog $log
                        #Write-Host "error" $isc_config[$cur_index].Get_Item("name") $address[0].ip_address
                        $global:isc_config[$cur_index].Add("fixed-address",$address)
                    }
                    "allow"
                    {
                        $filter = next_token
                        $to_add = allow_deny_helper $filter
                        $list =@()
      
                         if($global:isc_config[$cur_index].ContainsKey("allow"))
                        {
                            $list = $global:isc_config[$cur_index].Get_Item("allow")
                            $global:isc_config[$cur_index].Remove("allow")
                        }
                        $list = $list + $to_add
                        $global:isc_config[$cur_index].Add("allow",$list)
                        writeLog "Allow Statement parsed"
                    }
                    "deny"
                    {
                        $filter = next_token
                        $to_add = allow_deny_helper $filter
                        $list =@()
                         if($global:isc_config[$cur_index].ContainsKey("deny"))
                        {
                            $list = $global:isc_config[$cur_index].Get_Item("deny")
                            $global:isc_config[$cur_index].Remove("deny")
                        }
                        $list = $list + $to_add
                        $global:isc_config[$cur_index].Add("deny",$list)
                        $is_deny = 1;
                        writeLog "Deny Statement parsed"
                    }
                    "ignore"
                    {
                        $filter = next_token
                        $to_add = allow_deny_helper $filter
                        $list =@()
                         if($global:isc_config[$cur_index].ContainsKey("ignore"))
                        {
                            $list = $global:isc_config[$cur_index].Get_Item("ignore")
                            $global:isc_config[$cur_index].Remove("ignore")
                        }
                        $list = $list + $to_add
                        $global:isc_config[$cur_index].Add("ignore",$list)
                        writeLog "Ignore Statement parsed"
                    }
                    "option"
                    {
                        handle_options([ref] $global:isc_config )
                    }
                    "match"
                    {
                        
                        $match_expression = next_token
                        
                        handle_match $match_expression

                    }
                
                }
            }
        }
        $previous_token = $command
        $command = next_token
  
    }
    Write-Host "Parsing ends here"
    writeLog "PARSING ENDS HERE`n------------------------------------"
    writeLog "Shared Networks and Groups are now being removed from the network topology"
    Write-Host "Shared Networks and Groups are now being removed from the network topology"
    writeLog "------------------------------------"
    writeLog "shared networks and groups have been removed from the network topology"
    $global:isc_config = remove_groups_and_shared_networks $global:groups $global:isc_config
    $global:isc_config = remove_groups_and_shared_networks $global:superscopesIPv4 $global:isc_config
    Write-Host "Shared Networks and Groups have been removed successfully"
    writeLog "Properties of classes are now being migrated to subclasses"
    remove_subclasses_classes_and_classes_without_match 
    writeLog "Properties of classes have been migrated to subclasses"
    writeLog "------------------------------------------------------"
    writeLog "Reservations are now being set into the correct scopes"
    writeLog "------------------------------------------------------"
    Write-Host "Reservations are now being set into the correct scopes"
    
    find_reservations
    
    Write-Host "Reservations are now set into the correct scopes"
    writeLog "Reservations are now set into the correct scopes"
    writeLog "-------------------------------------------------"
    writeLog "Exclusions are now being found"
    writeLog "-------------------------------------------------"
    Write-Host "Exclusions are now being found"
    
    foreach($scope_id in $global:scopesIPv4)
    {
        $range = $global:isc_config[$scope_id].Get_Item("range")
        $min_ip = $global:isc_config[$scope_id].Get_Item("min-ip")
        $max_ip = $global:isc_config[$scope_id].Get_Item("max-ip") 

        $deny_list_length = ($range | ? { $_.is_deny }).Length ;
        $allow_list_length = ($range | ? { !$_.is_deny }).Length ;

        if ($deny_list_length -and $allow_list_length)
        {
            $log = "Scopes with both allow and deny ranges are not supported. Subnet is" + $global:isc_config[$scope_id].Get_Item("subnet") + "and netmask is " + $global:isc_config[$scope_id].Get_Item("netmask")
            continue;
        }

        $global:isc_config[$scope_id]["is-Set-Range"] = 1;
        $log = "the folowing exclusions are added to the scope whose subnet is" + $global:isc_config[$scope_id].Get_Item("subnet") + "and netmask is " + $global:isc_config[$scope_id].Get_Item("netmask")
        if ($allow_list_length)
        {
            $to_add = find_all_exclusions ($range | ? { !$_.is_deny }) $min_ip $max_ip
        }
        if($to_add.Length -gt 0)
        {
            $global:isc_config[$scope_id].Add("exclusions",$to_add)
        }
        foreach($key in $isc_config[0].keys)
        {
            if($global:isc_config[$scope_id].ContainsKey($key))
            {
                continue;
            }
            else
            {
                $array = "min-seconds", "default-lease-time", "max-lease-time", "min-lease-time"
                if($array -contains $key)
                {
                    $global:isc_config[$scope_id].Add($key,$global:isc_config[0].Get_Item($key))
                }
            }
        }
    }
    
    Write-Host "exclusions are now set correctly"
    writeLog "exclusions are now set correctly"
    writeLog "--------------------------------"
    Write-Host "policies are now being created"
    writeLog "policies are now being created"
    writeLog "--------------------------------"
#(Scope level = 1) => check only at scope level,     
    create_policies 0 0
   
    foreach($scope_id in $global:scopesIPv4)
    {
        create_policies $scope_id 1
    }
    Write-Host "Policies have been created!"
}


