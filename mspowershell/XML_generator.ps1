


########       Scope #####################



#$table is a name value pair

function generic_xml_writer($mandatory, $optional,$table,[string] $startTagName,[bool] $writeEndTag,[string] $namespace = "-1")
{
    if($namespace.CompareTo("-1") -eq 0)
    {
        $global:writer.WriteStartElement($startTagName)
    } 
    else
    {
        $global:writer.WriteStartElement($startTagName,$namespace)
    }   
    foreach($element in $mandatory)
    {
        if($table.ContainsKey($element))
        {
            $global:writer.WriteElementString($element,$table.Get_Item($element))
        }
        else
        {
            write-host "Error"
            break
        }
    }
    foreach($element in $optional)
    {
        if($table.ContainsKey($element))
        {
            $global:writer.WriteElementString($element,$table.Get_Item($element))
        }
        else
        {
            #$global:writer.WriteStartElement($element)
            #$global:writer.WriteEndElement()
        }
    }
    if($writeEndTag)
    {
        $global:writer.WriteEndElement()
    }
    $global:writer.Flush()
}

function populatePolicies($master_table,$id)
{
    if($master_table[$id].ContainsKey("policies"))
    {
        $PolicyPropertiesMandatory          = @("Name", "ProcessingOrder", "Enabled", "Condition")
        $PolicyPropertiesOptional           = @("Description", "VendorClass", "UserClass", "MacAddress", "ClientId", "RelayAgent", "CircuitId", "RemoteId", "SubscriberId")
        $policies_options = ($master_table[$id].Get_Item("policies-options"))
        $global:writer.WriteStartElement("Policies")
        for($j = 0;$j -lt $master_table[$id].Get_Item("policies").Length;$j++)
        {
           $policy = ($master_table[$id].Get_Item("policies"))[$j]
           $global:writer.WriteStartElement("Policy")
           $global:writer.WriteElementString("Name",$policy.name)
           $global:writer.WriteElementString("ProcessingOrder",$policy.processing_order)
           $global:writer.WriteElementString("Enabled","true")
           $global:writer.WriteElementString("Condition",$policy.logical_operator)
           $global:writer.WriteElementString("Description",$policy.description)
           for($i = 0;$i -lt $policy.tag.Length;$i++)
           {
                $global:writer.WriteElementString($policy.tag[$i],$policy.equality_check[$i])
                $global:writer.WriteElementString($policy.tag[$i],$policy.condition[$i])
           }
            
           if($policy.has_range)
           {
                $global:writer.WriteStartElement("IPRanges")
                foreach($range in $policy.range)
                {
                    $table = @{"StartRange" = $range.low_addr.ip_address;
                               "EndRange" = $range.high_addr.ip_address}
                    $array = "StartRange","EndRange"
                    generic_xml_writer $array @() $table "IPRange" 1               
                }
                $global:writer.WriteEndElement()  
            
           }
           $policy_options = $policies_options[$j]
           
           if($policy_options.Count -ne 0)
           {
                populateOptions $policy_options
           }
          
           $global:writer.WriteEndElement()
        }
        $global:writer.WriteEndElement()
        $global:writer.Flush()    
        
    }
    
}

function populateClasses($table)
{
    $Mandatory           = @("Name", "Type", "Data")
    $Optional            = @("Description")
    if($table.Length -ne 0)
    {
        $global:writer.WriteStartElement("Classes")
        foreach($class in $table)
        {
            
            generic_xml_writer $Mandatory $Optional $class "Class" 1
        }
        $global:writer.WriteEndElement()
    }
}

function populateOptionDefinitions($table)
{
    
    $Mandatory          = @("Name", "OptionId", "Type", "MultiValued")
    $Optional           = @("DefaultValue","Description", "VendorClass") 
    if($table.Length -ne 0)
    {
    $global:writer.WriteStartElement("OptionDefinitions")
    
        foreach($option_definition in $table)
        {
                generic_xml_writer $Mandatory $Optional $option_definition "OptionDefinition" 1
        }
        $global:writer.WriteEndElement()
    }
}

function populateReservationsIPv4($master_table,[int] $scope_id)
{
    $Mandatory     = @("Name", "IPAddress")
    $Optional      = @("ClientId", "Type", "Description")
  
    if($master_table[$scope_id].ContainsKey("reservations"))
    {
        
        $global:writer.WriteStartElement("Reservations")
        foreach($reservation_id in $master_table[$scope_id].Get_Item("reservations").Keys)
        {
            $global:writer.WriteStartElement("Reservation")
            $global:writer.WriteElementString("Name",$master_table[$reservation_id].Get_Item("Name"))
            $global:writer.WriteElementString("IPAddress",$master_table[$scope_id].Get_Item("reservations").Get_Item($reservation_id).ip_address)
            foreach($element in $Optional)
            {
                if($master_table[$reservation_id].ContainsKey($element))
                {
                    $global:writer.WriteElementString($element,$master_table[$reservation_id].Get_Item($element))
                }
                else
                {
                    $global:writer.WriteStartElement($element)
                    $global:writer.WriteEndElement()
                }
            }
            if($master_table[$reservation_id].ContainsKey("options"))
            {
                populateOptions $master_table[$reservation_id].Get_Item("options")
            }
            $global:writer.WriteEndElement()
           
        }
        $global:writer.WriteEndElement()
        $global:writer.Flush()
    }
}
function populateOptions($table)
{
    $global:writer.WriteStartElement("OptionValues")
    foreach($pair in $table.keys)
    {
        $option_id = $pair.option_id
        $vendor_class = $pair.option_space
        $global:writer.WriteStartElement("OptionValue")
        $global:writer.WriteElementString("OptionId",$option_id)
        $option_values = $table.Get_Item($pair)

        foreach($option_value in $option_values)
        {
            $global:writer.WriteElementString("Value",$option_value)
        }
        if($vendor_class.CompareTo("standard_options") -eq 0)
        {
            $global:writer.WriteStartElement("VendorClass")
            $global:writer.WriteEndElement()
        }
        else
        {
            $global:writer.WriteElementString("VendorClass",$vendor_class)
        }
        $global:writer.WriteStartElement("UserClass")
        $global:writer.WriteEndElement()
        $global:writer.WriteEndElement()     
    }
    
    $global:writer.WriteEndElement()
    $global:writer.Flush()
}

function populateExclusionRangesIPv4($master_table, [int] $scope_id) 
{
    $Mandatory = @("StartRange","EndRange")
    $Optional = @()
    if($master_table[$scope_id].ContainsKey("exclusions"))
    {
        $global:writer.WriteStartElement("ExclusionRanges")
        foreach($exclusionrange in $master_table[$scope_id].Get_Item("exclusions"))
        {
            $table = @{"StartRange" = $exclusionrange.low_addr.ip_address;
                       "EndRange" = $exclusionrange.high_addr.ip_address}
            
            generic_xml_writer $Mandatory $Optional $table "IPRange" 1               
        }
        $global:writer.WriteEndElement()   
    }
}

function populateScopesIPv4($master_table,$scopes)
{
    $global:writer.WriteStartElement("Scopes")
    if($scopes.Length -ne 0)
    {
        foreach($scope_id in $scopes)
        {
            if($master_table[$scope_id].Get_Item("is-Set-Range"))
            {
            
            }
            else 
            {
                $log = "The scope " + $master_table[$scope_id].Get_Item("subnet") + ", netmask: " + $master_table[$scope_id].Get_Item("netmask") + " could not be added because there is no range declaration"
                writeLog $log
                $global:unmigrated_scopes = $global:unmigrated_scopes + $scope_id
                continue
            }
        
            $mandatory = @("ScopeId", "Name", "SubnetMask", "StartRange", "EndRange", "LeaseDuration", "State", "Type", "MaxBootpClients", "NapEnable")
            $optional = @("Delay", "NapProfile", "Description", "ActivatePolicies", "SuperScopeName")
            generic_xml_writer $mandatory $optional $master_table[$scope_id] "Scope" 0 
            populateExclusionRangesIPv4 $master_table $scope_id 
            populatePolicies $master_table $scope_id
        
            if($master_table[$scope_id].ContainsKey("options"))
            {
                populateOptions $master_table[$scope_id].Get_Item("options")
            
            }
            populateReservationsIPv4 $master_table $scope_id 
        
            $global:writer.WriteEndElement()
        
        }
        $global:writer.WriteEndElement()
    }
    
}



function populateIPv4($master_table,$scopes,$reservations,$option_definitions,$microsoft_classes)
{
    
    $table = @{}
    $array = "ConflictDetectionAttempts","NapEnabled","NpsUnreachableAction","ActivatePolicies"
    
    generic_xml_writer $array @() $master_table[0] "IPv4"  0 ""
    populateClasses $microsoft_classes
    populateOptionDefinitions $option_definitions
    populatePolicies $master_table 0
    if($master_table[0].ContainsKey("options"))
    {
        populateOptions $master_table[0].Get_Item("options")
    } 
    populateScopesIPv4 $master_table $scopes
    
    $global:writer.WriteEndElement()

}



function DHCPserver($master_table,$scopes,$option_definitions,$microsoft_classes)
{
    $global:writer.WriteStartElement("DHCPServer","http://schemas.microsoft.com/windows/DHCPServer")
    $global:writer.WriteElementString("MajorVersion","","6")
    $global:writer.WriteElementString("MinorVersion","","2")
    populateIPv4 $master_table $scopes $reservations $option_definitions $microsoft_classes
    $global:writer.Close()
}


function Xml_initialize($read_destination_path) 
{
    try
    {
        $settings = New-Object System.Xml.XmlWriterSettings
        $settings.Indent = 1
        $settings.IndentChars = "  ";
        $settings.NewLineChars = "`r`n";
        $settings.NewLineHandling = [System.Xml.NewLineHandling]::Replace
        $global:writer = [System.Xml.XmlWriter]::Create($read_destination_path,$settings)    
        return 0
    }
    catch
    {
        return 1
    }
}




