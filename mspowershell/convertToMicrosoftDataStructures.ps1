
#########IPv4################

$global:ConflictDetectionAttempts = 1
$global:NapEnabled = "false"
$global:NpsUnreachableAction = "Full"
$global:ActivatePolicies = "true"


function migrate_global
{
    if($global:isc_config[0].ContainsKey("ping-check"))
    {
        
        $global:isc_config[0].Add("ConflictDetectionAttempts",$global:ConflictDetectionAttempts)
    }
    else
    {
        $global:isc_config[0].Add("ConflictDetectionAttempts",0)
    }
    $global:isc_config[0].Add("NapEnabled",$global:NapEnabled)
    $global:isc_config[0].Add("NpsUnreachableAction",$global:NpsUnreachableAction)
    $global:isc_config[0].Add("ActivatePolicies",$global:ActivatePolicies)
    if($global:isc_config[0].ContainsKey("options"))
    {
        $options =$global:isc_config[0].Get_Item("options")
        $options = migrate_options $options
        $global:isc_config[0].Remove("options")
        $global:isc_config[0].Add("options",$options)
        
        
    }

    

}

function migrate_classes
{
    $temp = $global:microsoft_classes
    $global:microsoft_classes = @()
    foreach($class in $temp)
    {
        $class_to_add = @{}
        $class_to_add.Add("Name",$class.name)
        $class_to_add.Add("Type",$class.type)
        $class_to_add.Add("Data",$class.data)
        $class_to_add.Add("Description",$class.description)
        $global:microsoft_classes = $global:microsoft_classes + $class_to_add
    }
}


$global:Delay = 0
$global:Type = "Both"
$global:MaxBootpClients = 0
$global:LeaseDuration = "8.00:00:00" # days.hours:min:seconds




function migrate_options($options)
{
    $options_to_add = @{}
    foreach($pair in $options.Keys)
    {
        if($pair.option_space.CompareTo("standard_options") -eq 0)
        {
            $options_to_add.Add($pair,$options.Get_Item($pair))
        }
        else
        {
            $vendor_classes = $global:option_space_map.Get_Item($pair.option_space)
            foreach($vendor_class in $vendor_classes)
            {
                $pair_to_add = New-Object option_pair
                $pair_to_add.init($vendor_class,$pair.option_id)
                $options_to_add.Add($pair_to_add,$options.Get_Item($pair))
                    
            }
                    
        }
    }
    return $options_to_add
    
}

function migrate_reservations
{
    foreach($reservation_id in $global:reservationsIPv4)
    {
        $name = $global:isc_config[$reservation_id].Get_Item("name")
        $global:isc_config[$reservation_id].Remove("name")
        $global:isc_config[$reservation_id].Add("Name",$name)

        $type = "Both"
        if($global:isc_config[$reservation_id].Get_Item("deny") -contains "bootp" -or
            $global:isc_config[$reservation_id].Get_Item("ignore") -contains "bootp")
        {
            $type = "Dhcp"
        }
        $global:isc_config[$reservation_id].Remove("type")
        $global:isc_config[$reservation_id].Add("Type",$type)
        $ClientId = $global:isc_config[$reservation_id].Get_Item("hardware-address")
        $ClientId = $ClientId.Replace(":","-")
        $global:isc_config[$reservation_id].Add("ClientId",$ClientId)
       
        if($global:isc_config[$reservation_id].ContainsKey("options"))
        {
            $options =$global:isc_config[$reservation_id].Get_Item("options")
            $options = migrate_options $options
            $global:isc_config[$reservation_id].Remove("options")
            $global:isc_config[$reservation_id].Add("options",$options)
        }
    }
}

function migrate_option_definitions
{
    #$Mandatory          = @("Name", "OptionId", "Type", "MultiValued")
    #$Optional           = @("DefaultValue", "Description", "VendorClass")
    $option_definitions = $global:isc_config[0].Get_Item("option-definitions")
    $option_definitions_to_add = @()
    foreach($option_space in $option_definitions.Keys)
    {
        if($option_space.CompareTo("standard_options") -eq 0) { $vendor_classes = @("standard_options")}
        else { $vendor_classes = $global:option_space_map.Get_Item($option_space)}
        foreach($vendor_class in $vendor_classes)
        {
            foreach($option_definition in $option_definitions.Get_Item($option_space))
            {
                $option_definition_to_add = @{}
                if($option_definition.new_option)
                {
                    $option_definition_to_add.Add("Name",$option_definition.name)
                    $option_definition_to_add.Add("OptionId",$option_definition.id)
                    $option_definition_to_add.Add("Type",$option_definition.type)
                    if($option_definition.type.CompareTo("IPv4Address") -eq 0)
                    {
                        $option_definition_to_add.Add("DefaultValue","0.0.0.0")
                    }
                    if($option_definition.multivalued) { $option_definition_to_add.Add("MultiValued","true") }
                    else { $option_definition_to_add.Add("MultiValued","false") }
                    if($vendor_class.CompareTo("standard_options") -ne 0) {$option_definition_to_add.Add("VendorClass",$vendor_class)}
                    $option_definitions_to_add = $option_definitions_to_add + $option_definition_to_add
                }
                
            }
        }

    }
    #$reader = [System.Xml.XmlReader]::Create("C:\Users\t-rahils\Desktop\Final Project\StandardOptions")
    $standard_options = @()
    #[xml]$reader = Get-Content "C:\Users\t-rahils\Desktop\Final Project\StandardOptions"  
    #foreach ($optionDefinition in $reader.OptionDefinitions.OptionDefinition)
    #{
    #    $option_definition_to_add = @{}
    #    $option_definition_to_add.Add("Name",$optionDefinition.Name)
    #    $option_definition_to_add.Add("OptionId",$optionDefinition.OptionId)
    #    $option_definition_to_add.Add("Type",$optionDefinition.Type)
    #    $option_definition_to_add.Add("MultiValued",$optionDefinition.MultiValued)
    #    $vendor_class = $optionDefinition.VendorClass
    #    if($vendor_class.Length -ne 0) {$option_definition_to_add.Add("VendorClass",$optionDefinition.VendorClass)}
    #    $standard_options = $standard_options  + $option_definition_to_add
        
        
    #}
    #$option_definitions_to_add = $standard_options + $option_definitions_to_add

    
    $option_definitions = $global:isc_config[0].Set_Item("option-definitions",$option_definitions_to_add)

}

function migrate_scopes
{
    foreach($scope_id in $global:scopesIPv4)
    {
        if($global:isc_config[$scope_id].ContainsKey("options"))
        {
            $options =$global:isc_config[$scope_id].Get_Item("options")
            $options = migrate_options $options
            $global:isc_config[$scope_id].Remove("options")
            $global:isc_config[$scope_id].Add("options",$options)
        }
        if($global:isc_config[$scope_id].ContainsKey("policies-options"))
        {
            $policies_options = $global:isc_config[$scope_id].Get_Item("policies-options")
            $global:isc_config[$scope_id].Remove("policies-options")
            $policies_options_to_add = @()
            foreach($policy_options in $policies_options)
            {
                $policy_options_to_add = migrate_options $policy_options
                $policies_options_to_add = $policies_options_to_add + $policy_options_to_add
            }
            $global:isc_config[$scope_id].Add("policies-options",$policies_options_to_add)
        }
        
        
        $delay = $global:Delay 
        $type = $global:Type


        #Lease duration
        $lease_duration = $global:LeaseDuration
        $time = -1
        if($global:isc_config[$scope_id].ContainsKey("default-lease-time"))
        {
            $time = $global:isc_config[$scope_id].Get_Item("default-lease-time")
        }
        else 
        {
            if($global:isc_config[$scope_id].ContainsKey("max-lease-time"))
            {
                $time = $global:isc_config[$scope_id].Get_Item("max-lease-time")
            }
            else
            {
                if($global:isc_config[$scope_id].ContainsKey("min-lease-time"))
                {
                    $time = $global:isc_config[$scope_id].Get_Item("min-lease-time")
                }
            }

        }
        if($time -ne -1)
        {
            [int]$days = $time / 86400
            [int]$time = $time % 86400
            [int]$hours = $time / 3600
            [int]$hours = $time % 3600
            [int]$minutes = $time / 60
            [int]$seconds = $time % 60
            if($seconds -gt 59)
            {
                $minutes++
                $seconds = $seconds - 60
            }
            if($minutes -gt 59)
            {
                $hours++
                $minutes = $minutes - 60
            }
            if($hours -gt 23)
            {
                $days++
                $hours = $hours - 24
            }
            $days1 = $days.ToString()
            $hours1= $hours.ToString()
            $minutes1 = $minutes.ToString()
            $seconds1 = $seconds.ToString()
            [string]$lease_duration = $days1 + "." + $hours1 + ":" + $minutes1+ ":" + $seconds1
        }


        #state
        $state = "Active"
        $global:isc_config[$scope_id].Add("State",$state)

        

        #type-of-clients
        $type = $global:isc_config[$scope_id].Get_Item("type")
        $global:isc_config[$scope_id].Remove("type")
        $global:isc_config[$scope_id].Add("type-modified",$type)
        if($global:isc_config[$scope_id].ContainsKey("deny-bootp") -or
           $global:isc_config[$scope_id].Get_Item("deny") -contains "bootp" -or
           $global:isc_config[$scope_id].Get_Item("deny") -contains "dynamic-bootp-clients")
        {
            $global:isc_config[$scope_id].Add("Type","Dhcp")
            
        }
        else
        {
            $global:isc_config[$scope_id].Add("Type","Both")
        }


        
        if($global:isc_config[$scope_id].ContainsKey("min-seconds"))
        {
            $delay = $global:isc_config[$scope_id].Get_Item("min-seconds")
        }
        if($global:isc_config[$scope_id].ContainsKey("Superscope-name"))
        {
           
            $superscopename = $global:isc_config[$scope_id].Get_Item("Superscope-name")
            $global:isc_config[$scope_id].Add("SuperScopeName",$superscopename)
        }
        #Mandatory         = @("ScopeId", "Name", "SubnetMask", "StartRange", "EndRange", "LeaseDuration", "State", "Type", "MaxBootpClients", "NapEnable")
        #Optional          = @("Delay", "NapProfile", "Description", "ActivatePolicies", "SuperScopeName")
        $global:isc_config[$scope_id].Add("ScopeId",$global:isc_config[$scope_id].Get_Item("subnet"))
        $global:isc_config[$scope_id].Add("Name","Scope" + $global:isc_config[$scope_id].Get_Item("subnet") + "_" + $global:isc_config[$scope_id].Get_Item("netmask"))
        $global:isc_config[$scope_id].Add("SubnetMask", $global:isc_config[$scope_id].Get_Item("netmask"))
        $global:isc_config[$scope_id].Add("StartRange",$global:isc_config[$scope_id].Get_Item("min-ip").ip_address)
        $global:isc_config[$scope_id].Add("EndRange",$global:isc_config[$scope_id].Get_Item("max-ip").ip_address)
        $global:isc_config[$scope_id].Add("LeaseDuration",$lease_duration)
        $global:isc_config[$scope_id].Add("MaxBootpClients",4294967295)
        $global:isc_config[$scope_id].Add("NapEnable","false")
        $global:isc_config[$scope_id].Add("Delay",$delay/1000)
        $global:isc_config[$scope_id].Add("ActivatePolicies","true")
    }
    
}

