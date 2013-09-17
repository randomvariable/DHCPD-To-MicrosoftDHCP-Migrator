


function handle_if_else($condition)
{
    #write-host $condition $cur_index
    $continue = 1
    $tag = @()
    $tag_value = @()
    $equality_check = @()
    $incomplete = @()
    
    $equality = "EQ"
    $logical_operator = "none"
    
    if($global:isc_config[$cur_index].ContainsKey("tag"))
    {
        $tag = $global:isc_config[$cur_index].Get_Item("tag")
        $tag_value = $global:isc_config[$cur_index].Get_Item("tag-value")
        $equality_check= $global:isc_config[$cur_index].Get_Item("equality-check")
        $logical_operator = $global:isc_config[$cur_index].Get_Item("logical-operator")
        
        $global:isc_config[$cur_index].Remove("tag")
        $global:isc_config[$cur_index].Remove("tag-value")
        $global:isc_config[$cur_index].Remove("equality-check")
        $global:isc_config[$cur_index].Remove("logical-operator")
        
        if($logical_operator.CompareTo("OR") -eq 0)
        {
            
            $global:isc_config[$cur_index].Add("Throw-if",1)
            throw_out
            return
        }
        else
        {
            $logical_operator = "AND"
        }
        
    }
    while($continue)
    {
        switch($condition)
        {
            "option"
            {
                $option_name = next_token
                #write-host $option_name
                switch($option_name)
                {
                    "vendor-class-identifier"
                    {
                                            
                        $vendor_class = next_token
                        $vendor_class = next_token
                        $tag = $tag + "VendorClass"
                        $tag_value = $tag_value + $vendor_class
                        $equality_check = $equality_check +  $equality
                        $equality = "EQ"
                        #Write-Host -ForegroundColor Cyan $vendor_class                      
                    }
                    "dhcp-user-class"
                    {
                        $user_class = next_token
                        $user_class = next_token
                        $tag = $tag + "UserClass"
                        $tag_value = $tag_value + $user_class
                        $equality_check = $equality_check +  $equality
                        $equality = "EQ"
                        add_user_class $user_class
                                            
                    }
                    "dhcp-client-identifier"
                    {
                        $identifier_name = next_token
                        $identifier_name = next_token
                        $tag = $tag + "ClientId"
                        $tag_value = $tag_value + $identifier_name
                        $equality_check = $equality_check +  $equality
                        $equality = "EQ"
    
                    }
                                        
                }
            }
            "hardware"
            {
                $identifier_name = next_token
                $identifier_name = next_token
                $tag = $tag + "MacAddress"
                $identifier_name=$identifier_name.Replace(":","-")
                $tag_value = $tag_value + $identifier_name.Substring(3,$identifier_name.Length-3)
                
                $equality_check = $equality_check +  $equality
                $equality = "EQ"
    
            }
            "not"
            {
                $equality = "NE"
            }
            "and"
            {
                if($logical_operator -eq "OR")
                {
                    $global:isc_config[$cur_index].Add("Throw-if",1)
                    throw_out   
                    return
                }
                else
                {
                    $logical_operator = "AND"
                }
            }
            "or"
            {
                if($logical_operator -eq "AND")
                {
                    $global:isc_config[$cur_index].Add("Throw-if",1)
                    throw_out
                    return   
                }
                else
                {
                    $logical_operator = "OR"
                }
            }
            "{"
            {
                
                $accept_next_token = 0
                $global:isc_config[$cur_index].Add("tag",$tag)
                $global:isc_config[$cur_index].Add("tag-value",$tag_value)
                $global:isc_config[$cur_index].Add("equality-check",$equality_check)
                $global:isc_config[$cur_index].Add("logical-operator",$logical_operator)
                return
              
            }
            default
            {
                $global:isc_config[$cur_index].Add("Throw-if",1)
                throw_out
                return
            }
        }
        $condition = next_token
        
    }
}


function throw_out
{
    $number_braces = -1
    $token = next_token
    while(1)
    {
        
        if($token.CompareTo("") -eq 0)
        {
            return
        }
        if($token.CompareTo("{") -eq 0)
        {
            $number_braces++
        }
        else
        {
            if($token.CompareTo("}") -eq 0)
            {
                if($number_braces -eq 0)
                {
                    return
                }
                else
                {
                    $number_braces--
                }
            }
        }
        
        $token = next_token
        #Write-Host $token
    }    
}