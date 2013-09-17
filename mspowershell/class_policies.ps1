
function handle_match_helper($tag_name,$match_value,$convert)
{
    
    if($match_value.CompareTo(";") -eq 0)
    {
        $match_object = New-Object match_expression
        $match_object.init($tag_name,"none")
        $global:isc_config[$cur_index].Set_Item("has-subclasses",1)
                
    }
    else
    {
        $match_object = New-Object match_expression
        if($convert) {$match_value = $match_value.Replace(":","-")}
        $match_object.init($tag_name,$match_value)
                
    }
    $match_to_add = $match_object
    return $match_to_add            
}


function handle_match ($match_expression)
{
    
    if($match_expression.CompareTo("if") -eq 0)
    {
      $match_expression = next_token
    }
    switch($match_expression)
    {
        "hardware"
        {
            
            $match_value = next_token
            $match_to_add = handle_match_helper "MacAddress" $match_value 1 
            $match_to_add.match_value = $match_to_add.match_value.Substring(3,$match_to_add.match_value.Length - 3)
            $global:isc_config[$cur_index].Add("match",$match_to_add)
           
        }
        "option"
        {
            $option_value = next_token
            switch($option_value)
            {
                "dhcp-client-identifier"
                {
                    $match_value = next_token
                    $match_to_add = handle_match_helper "ClientId" $match_value 0 
                    $global:isc_config[$cur_index].Add("match",$match_to_add)
                       
                }
                "vendor-class-identifier"
                {
                    
                    $match_value = next_token
                    $match_to_add = handle_match_helper "VendorClass" $match_value 0 
                    
                    $global:isc_config[$cur_index].Add("match",$match_to_add)
                }
                "dhcp-user-class"
                {                    
                    $match_value = next_token
                    $match_to_add = handle_match_helper "UserClass" $match_value 0 
                    $global:isc_config[$cur_index].Add("match",$match_to_add)
                        
                }
                default
                {
                    writeLog "match expression not supported here!"
                    return
                }
            }
        }
       
        default
        {
            writeLog "match expression not supported here"
            return
        }

    }
    
    $global:isc_config[$cur_index].Set_Item("has-match",1)
    

}


function add_vendor_class($identifier, $option_space)
{
    #Write-Host $identifier $option_space
    $class_name =  $identifier + "_" + $option_space
    $class_to_add = New-Object dhcp_Class
    $class_to_add.init($class_name,$identifier,"Vendor","This vendor class using the vendor-option-space declaration and vendor-option-identifier declaration. The option space is " + $option_space + " and the vendor class identifier is " +$identifier)
    $class_to_add.set_option_space($option_space)
    $to_add = 1
    foreach($class in $global:microsoft_classes)
    {
        
        if($class.name.CompareTo($class_name) -eq 0)
        {
            $to_add = 0
            break
        }
    }
    if($to_add) 
    {
        $global:microsoft_classes = $global:microsoft_classes + $class_to_add
        $array_to_add = @()
        if($global:option_space_map.ContainsKey($option_space))
        {
            $array_to_add = $global:option_space_map.Get_Item($option_space)
            $global:option_space_map.Remove($option_space)
        }
        $array_to_add = $array_to_add + $class_name
        $global:option_space_map.Add($option_space,$array_to_add)
    }
    
}

function add_user_class($identifier)
{
    #Write-Host $identifier
    $new_class = New-Object dhcp_class
    $user_class = $identifier
    $new_class.init($user_class,$user_class,"User","This user class is created for the user-class-identifier" + $user_class)
    $to_add = 1
    foreach($class in $global:microsoft_classes)
    {
        if($class.name.CompareTo($identifier) -eq 0)
        {
            $to_add = 0
            break
        }
    }
    if($to_add) {$global:microsoft_classes = $global:microsoft_classes + $new_class}
    
}

function remove_subclasses_classes_and_classes_without_match
{
    foreach($class_index in $global:classes)
    {
        if(-not $global:isc_config[$class_index].Get_Item("has-subclasses"))
        {
           
            if($global:isc_config[$class_index].Get_Item("has-match"))
            {
                $match = $global:isc_config[$class_index].Get_Item("match")
                if($match.match_id.CompareTo("UserClass") -eq 0)
                {
                    add_user_class $identifier
                    
                }
                if($match.match_id.CompareTo("VendorClass") -eq 0)
                {
                    $option_space = $global:isc_config[$class_index].Get_Item("vendor-option-space")
                    add_vendor_class $match.match_value $option_space             
                }
                $global:final_classes = $global:final_classes + $class_index
            }
        }
    }
    foreach($subclass_index in $global:subclasses)
    {
        $subclass_name = $global:isc_config[$subclass_index].Get_Item("class-name")
        
        foreach($class_index in $global:classes)
        {
            
            if($subclass_name.CompareTo($global:isc_config[$class_index].Get_Item("name")) -eq 0)
            {
                #Write-Host $subclass_name
                foreach($key in $global:isc_config[$class_index].keys)
                {
                    if($key.CompareTo("name") -eq 0 -or $key.CompareTo("has-subclasses") -eq 0 -or $key.CompareTo("has-match") -eq 0 -or $key.CompareTo("type") -eq 0)
                    {
                        
                        continue;
                    }
                    else
                    {
                        if($key.CompareTo("match") -eq 0)
                        {
                            $match = New-Object match_expression
                            $match.match_id = $global:isc_config[$class_index].Get_Item($key).match_id
                            $match.match_value = $global:isc_config[$class_index].Get_Item($key).match_value 
                            
                            #Write-Host $match.match_id
                            $match.match_value = $global:isc_config[$subclass_index].Get_Item("match-value")
                            if($match.match_id.CompareTo("MacAddress") -eq 0)
                            {
                                $match.match_value = $match.match_value.Replace(":","-")
                                $match.match_value = $match.match_value.Substring(3,$match.match_value.Length - 3)
                            }
                            if($match.match_id.CompareTo("UserClass") -eq 0)
                            {
                                add_user_class $match.match_value
                            }
                            if($match.match_id.CompareTo("VendorClass") -eq 0)
                            {
                                if($global:isc_config[$subclass_index].ContainsKey("vendor-option-space"))
                                {
                                    $option_space = $global:isc_config[$subclass_index].Get_Item("vendor-option-space")
                                }
                                else
                                {
                                    $option_space = $global:isc_config[$class_index].Get_Item("vendor-option-space")
                                }
                                
                                add_vendor_class $match.match_value $option_space
                                $match.match_value = $match.match_value + "_" + $option_space             
                            }
                            $global:isc_config[$subclass_index].Add("match",$match)
                        }
                        else
                        {
                            if($key.CompareTo("options") -eq 0)
                            {
                                foreach($pair in $global:isc_config[$class_index].Get_Item("options"))
                                {
                                    $options_subclass = @{}
                                    if($global:isc_config[$subclass_index].ContainsKey("options"))
                                    {
                                        $options_subclass = $global:isc_config[$subclass_index].Get_Item("options")
                                        $global:isc_config[$subclass_index].Remove("options")
                                    }
                                    if($options_subclass.ContainsKey($pair))
                                    {}
                                    else
                                    {
                                        $options_subclass.Add($pair,$global:isc_config[$class_index].Get_Item("options").Get_Item($pair))
                                    }
                                    $global:isc_config[$subclass_index].Add("options",$options_subclass)
                                }
                                
                            }
                            else
                            {
                                if($global:isc_config[$subclass_index].ContainsKey($key))
                                {}
                                else
                                {
                                    $global:isc_config[$subclass_index].Add($key,$global:isc_config[$class_index].Get_Item($key))
                                }

                            }
                        }
                    }
                }
                break    
            }
            
        }
        $global:final_classes = $global:final_classes + $subclass_index
    }
}



  #public bool has_range;
  #  public string name;
  #  public int processing_order;
  #  public bool enabled;
   # public string logical_operator;
   # public string description;
   # public string[] tag;
   # public string[] condition;
   # public string[] equality_check;
   # public dhcp_range[] range;
   # public void init(string n,int order,bool check,string op,string des,string[] cond,string[] eq, string[] t)



function deny_to_policy($list)
{
    
    $tag = @()
    $tag_value = @()
    $equality_check = @()
    $logical_operator = "AND"
    
    foreach($class in $list)
    {
       
        foreach($isc_class_index in $global:final_classes)
        {
            $isc_class_name =$global:isc_config[$isc_class_index].Get_Item("class-name")
            if($isc_class_name.CompareTo($class) -eq 0)
            {
               
                $match = $global:isc_config[$isc_class_index].Get_Item("match")
                $tag = $tag + $match.match_id
                $tag_value =  $tag_value + $match.match_value
                $equality_check = $equality_check + "NE"
            }
        }
    }
    if($tag.Length -ne 0)
    {
       
        $policy = New-Object policy
        $name = "Deny" + "_" + "Policy"
        $policy.init($name,1,1,$logical_operator,"This is a test policy", $tag_value,$equality_check, $tag)
        $deny_policy = @($policy)
        return $deny_policy
    }
    else
    {
        return @()
    }
}

function allow_to_policy($list)
{
    $policy_ctr = 1
    $allow_policies = @()
    $allow_options = @()
    foreach($class in $list)
    {
        foreach($isc_class_index in $global:final_classes)
        {
            $isc_class_name =$global:isc_config[$isc_class_index].Get_Item("class-name")
            if($isc_class_name.CompareTo($class) -eq 0)
            {
                $match = $global:isc_config[$isc_class_index].Get_Item("match")
                $name = "Allow" + "_" + $isc_class_name + "_" + $match.match_id + "_" + $match.match_value
                $processing_order = $policy_ctr
                $policy_ctr++
                $tag = @($match.match_id)
                
                $tag_value = @($match.match_value)
                $equality_check = @("EQ")
                $policy = New-Object policy
                $policy.init($name,$processing_order,1,"AND","This is an allow policy",$tag_value,$equality_check,$tag)
                $allow_policies = $allow_policies + $policy
                $allow_option = @{}       
                if($global:isc_config[$isc_class_index].ContainsKey("options"))
                {
                    foreach($key in $global:isc_config[$isc_class_index].Get_Item("options").Keys)
                    {
                        $allow_option.Add($key,$global:isc_config[$isc_class_index].Get_Item("options").Get_Item($key))
                    }
                    $allow_options = $allow_options + $allow_option
                }
                else
                {
                    $allow_options = $allow_options + @{}
                }
            }
                
        }
    }
    return @($allow_policies,$allow_options)
}


function merge_option_values($pool_index, $policy_options)
{
    
    if($global:isc_config[$pool_index].ContainsKey("options"))
    {
        $pool_options = $global:isc_config[$pool_index].Get_Item("options")
        foreach($key in $pool_options.Keys)
        {
            if($policy_options.ContainsKey($key))
            {}
            else
            {
                $policy_options.Add($key,$pool_options.Get_Item($key))
            }
        }
    }
    return $policy_options
    
}


function create_policies_helper($index, $allow_list, $deny_list, $is_pool, $pool_index = -1)
{
    if($index -eq 0)
    {
        writeLog "Policies are now being created at the server level using the allow/deny/ignore statements defined globally"
    }
    else
    {
        if($is_pool)
        {
            $log = "Policies are being created from the allow/deny/ignore statements present in a pool in " + $global:isc_config[$index].Get_Item("subnet")
        }
        else
        {
            $log = "Policies are being created from the allow/deny/ignore statements present in " + $global:isc_config[$index].Get_Item("subnet")
        }
        writeLog $log
    }
    $allow_length = $allow_list.Length
    $deny_length = $deny_list.Length
    
    if($allow_length -ne 0)
    {
        $allow_temp = allow_to_policy $allow_list 
        $allow_policies = $allow_temp[0]
        $allow_options = $allow_temp[1]
        
    }
    if($deny_length -ne 0)
    {
        
        $deny_policies = deny_to_policy $deny_list
        
    }
    $policies = @()
    $policies_options = @()
    if($allow_policies.Length -ne 0 -and $deny_policies.Length -ne 0)
    {
        writeLog "Due to multiple allow/deny statements within one pool/subnet/global declaration, deny policy is being merged with the multiple allow policies"
        $deny_policy = $deny_policies[0]
        $policies = @()
        $policies_options = @()
        for($i = 0;$i -lt $allow_policies.Length; $i++)
        {
            $allow_policy = $allow_policies[$i]
            $allow_policy.name = "Allow_and_Deny"
            $allow_policy.description = "Allow and deny policies are merged"
            $allow_policy.tag = $allow_policy.tag + $deny_policy.tag
            $allow_policy.condition = $allow_policy.condition + $deny_policy.condition
            $allow_policy.equality_check = $allow_policy.equality_check + $deny_policy.equality_check  
            $policies = $policies + $allow_policy
            $policy_options = $allow_options[$i]
            
            if($is_pool)
            {
                $policy_options = merge_option_values $pool_index $policy_options
                if($global:isc_config[$pool_index].ContainsKey("range"))
                {
                    $log = "The range statement in a pool " + " in subnet " + $global:isc_config[$index].Get_Item("subnet") + " has been ignored. kindly update the policy manually"
                    $subnet = $global:isc_config[$index].Get_Item("subnet")
                    $to_add = @()
                    if($global:unmigrated_ranges.ContainsKey($subnet))
                    {
                        $to_add = $global:unmigrated_ranges.Get_Item($subnet)
                        $global:unmigrated_ranges.Remove($subnet)
                    }
                    $to_add += $global:isc_config[$pool_index].Get_Item("range")
                    $global:unmigrated_ranges.Add($subnet,$to_add)
                    
                    writeLog $log
                }
            }
            $policies_options = $policies_options + $policy_options
        }
        writeLog "Due to multiple allow/deny statements within one pool/subnet/global declaration, deny policy have now been merged with the multiple allow policies"
   
        
    }
    else
    {
        if($allow_policies.Length -ne 0)
        {
            $policies = $allow_policies
            $policies_options = $allow_options
            if($is_pool)
            {
                for($i = 0;$i -lt $policies.Length; $i++)
                {
                    writeLog "pool options and options from the respective class are now being merged together for the allow policy"
                    $policies_options[$i] = merge_option_values $pool_index $policies_options[$i]
                }
                if($global:isc_config[$pool_index].ContainsKey("range"))
                {
                    $log = "The range statement in a pool " + " in subnet " + $global:isc_config[$index].Get_Item("subnet") + " has been ignored. kindly update the policy manually"
                    $subnet = $global:isc_config[$index].Get_Item("subnet")
                    $to_add = @()
                    if($global:unmigrated_ranges.ContainsKey($subnet))
                    {
                        $to_add = $global:unmigrated_ranges.Get_Item($subnet)
                        $global:unmigrated_ranges.Remove($subnet)
                    }
                    $to_add += $global:isc_config[$pool_index].Get_Item("range")
                    $global:unmigrated_ranges.Add($subnet,$to_add)
                    
                    writeLog $log
                }
            }
        }
        else
        {
            if($deny_policies.Length -ne 0)
            {
                $policies = $deny_policies
                $policies_options = @(@{})
                if($is_pool)
                {
                    if($global:isc_config[$pool_index].ContainsKey("options"))
                    {
                        $policies_options = @($global:isc_config[$pool_index].Get_Item("options"))
                    }
                    if($global:isc_config[$pool_index].ContainsKey("range"))
                    {
                        $policies[0].set_range($global:isc_config[$pool_index].Get_Item("range"))
                    }
                }
                else
                {
                    $policies_options = @(@{})
                    if($global:isc_config[$index].ContainsKey("range"))
                    {
                        $policies[0].set_range($global:isc_config[$pool_index].Get_Item("range"))
                    }
                }
                
            }
            else
            {
                $policies = @()
                $policies_options = @()
            }
            
        }
    }
    return @($policies,$policies_options)

    
}









function create_policies($index, $is_global)
{
    
    
    $allow_list = $global:isc_config[$index].Get_Item("allow")
    $deny_list = $global:isc_config[$index].Get_Item("deny")
    $deny_list = $deny_lists + $global:isc_config[$index].Get_Item("ignore")
    $array = create_policies_helper $index $allow_list $deny_list 0
    $policies_temp = $array[0]
    $policies_options_temp = $array[1]
    if($global:isc_config[$index].ContainsKey("pools"))
    {
        foreach($pool_index in $global:isc_config[$index].Get_Item("pools"))
        {
          
            
            $allow_list = $global:isc_config[$pool_index].Get_Item("allow")
            $deny_list = $global:isc_config[$pool_index].Get_Item("deny")
            if($global:isc_config[$pool_index].ContainsKey("ignore"))
            {
                $deny_list = $deny_list + $global:isc_config[$pool_index].Get_Item("ignore")
            }
        
            $array = create_policies_helper $index $allow_list $deny_list 1 $pool_index
            $policies_temp = $policies_temp + $array[0]
            $policies_options_temp = $policies_options_temp + $array[1]
        }
    }
    $policy_ctr = 1
    $policies = @()
    $policies_options = @()
    if($global:isc_config[$index].ContainsKey("policies"))
    {
        $policies = $global:isc_config[$index].Get_Item("policies")
        $policies_options = $global:isc_config[$index].Get_Item("policies-options")
        $global:isc_config[$index].Remove("policies")
        $global:isc_config[$index].Remove("policies-options")
    }
    $policies = $policies + $policies_temp
    $policies_options = $policies_options + $policies_options_temp
    $policy_ctr = 1
    $policy_count = 1
    for($i = 0;$i -lt $policies.Length; $i++) 
    {
        $policies[$i].name = $policies[$i].name + ",Id:" + $policy_count
        $policy_count++
        $policies[$i].processing_order = $policy_ctr
        $policy_ctr++
    }
    if($policies.Length -ne 0)
    {
        $global:isc_config[$index].Add("policies",$policies)
        $global:isc_config[$index].Add("policies-options",$policies_options)
    }
}
