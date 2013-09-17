

function handle_options([ref]$isc_hashmap)
{
    $option_name = next_token
    $option_value = next_token 
    if($option_name.CompareTo("space") -eq 0)
    {
            
        $option_definitions = $isc_hashmap.value[0].Get_Item("option-definitions")
        [option_record[]] $addRecord = @()
        $option_definitions.Add($option_value,$addRecord)
        $isc_hashmap.value[0].Set_Item("option-definitions",$option_definitions)
        #$new_class = New-Object dhcp_class
        #$new_class.init($option_value,$option_value,"Vendor","This is the default class for the option space " + $option_value,$option_value)
        #$global:vendor_user_classes = $global:vendor_user_classes + $new_class
        $log = "new option space added: " + $option_value
        writeLog $log
    }
    else
    {
        if($option_value.CompareTo("code") -eq 0)
        {
            [bool] $add_option = 1
            [string] $vendor_space = "standard_options"
                            
                            
            [int] $index = $option_name.IndexOf(".")
            if($index -ne -1) #vendor option addition
            {
                $vendor_space = $option_name.Substring(0,$index)
                $option_name = $option_name.Substring($index+1,$option_name.Length - $index - 1)
                                
            }
            $option_id = next_token
            if($option_id.IndexOf("=") -ne -1)
            {
                $option_id = $option_id.Substring(0,$option_id.Length - 1)
            }
            else
            {
                $junk = next_token
            }
            $type = next_token
            if($type.CompareTo("array") -eq 0)
            {
                $multivalued =1
                $junk = next_token
                $type = next_token
                                
            }
            else
            {
                if($type.CompareTo("{") -eq 0)
                {
                    $type = next_token
                    while($type.CompareTo(";") -ne 0)
                    {
                        $type = next_token
                    }
                    $add_option = 0
                }
                $multivalued = 0
            }
            if($add_option)
            {
                $type = get_option_data_type $type
                                
            }
            if($type.CompareTo("Encapsulated") -eq 0)
            {
                $type = next_token
                $add_option = 0
                $log = $option_name + " " + $option_value + ": Encapsulated not supported"
                writeLog $log
            }
            if($add_option)
            {
                $option_definition = New-Object option_record
                $option_definition.init($option_name,$option_id,$type,$multivalued)
                $hash_map = $isc_hashmap.value[0].Get_Item("option-definitions")
                $option_array = $hash_map.Get_Item($vendor_space)
                $option_array = $option_array + $option_definition
                $hash_map.Set_Item($vendor_space,$option_array)
                $isc_hashmap.value[0].Set_Item("option-definitions",$hash_map)
            }
            else
            {
                writeLog "option definition not added"
            }
        }
        else
        {
                            
            $index = $option_name.IndexOf(".")
            $vendor_space = "standard_options"
            if($index -ne -1)
            {
                                
                $vendor_space = $option_name.Substring(0,$index)
                $option_name = $option_name.Substring($index+1,$option_name.Length - $index-1)
            }
            $option_array =$isc_hashmap.value[0].Get_Item("option-definitions").Get_Item($vendor_space)
            for($i = 0;$i -lt $option_array.Length; $i++)
            {
                $option = $option_array[$i]
                if($option.name.CompareTo($option_name) -eq 0)
                {
                    $option_index = $option.id
                    break
                }
            }
            $option_values = @()
                            
            while($option_value.CompareTo(";") -ne 0)
            {
                $option_values = $option_Values + $option_value
                $option_value = next_token
            }
            $optionHashMap = @{}
            if($isc_hashmap.value[$cur_index].ContainsKey("options"))
            {
                $optionHashMap = $isc_hashmap.value[$cur_index].Get_Item("options")
                $isc_hashmap.value[$cur_index].Remove("options")
            }
            $pair = New-Object option_pair
            $pair.init($vendor_space,$option_index)
            $optionHashMap.Add($pair,$option_values)
            $isc_hashmap.value[$cur_index].Add("options",$optionHashMap)
            
        }
    }
    #return $isc_hashmap.value
}
