#Function to decide the category of the parameter while removing shared networks or groups from the topology
function remove_categories([string] $key)
{
    $category_1 = @()
    $category_1 = $category_1 + "name"

    $category_2 = "allow","deny","ignore"

    $category_4 = "options"

    $category_5 = "policies", "policies-options"

   
    if($key.CompareTo("options") -eq 0)
    {
        return 4
    }
    if($category_5 -contains $key)
    {
        return 5
    }
  
    foreach($item in $category_1)
    {
        if($key.CompareTo($item) -eq 0)
        {
            return 1
        }
    }
    foreach($item in $category_2)
    {
        if($key.CompareTo($item) -eq 0)
        {
            return 2
        }
    }
    return 3

}


############# Function to remove a group or shared network from the network topology
function remove_groups_and_shared_networks([int[]]$parent_array, $config)
{
    foreach($parent_index in $parent_array)
    {
        $children_array = $config[$parent_index].Get_Item("child-index")
        foreach($child_index in $children_array)
        {
            if($config[$parent_index].ContainsKey("name"))
            {
                $config[$child_index].Add("Superscope-name",$config[$parent_index].Get_Item("name"))
            }
            foreach($key in $config[$parent_index].keys)
            {
                
                $remove_category = remove_categories $key
                if($remove_category -eq 5)
                {
                    $to_add = $config[$parent_index].Get_Item($key)
                    if($config[$child_index].ContainsKey($key))
                    {
                        $present_value = $config[$child_index].Get_Item($key)
                        if($key.CompareTo("policies") -eq 0)
                        {
                            $ctr = $present_value.Length + 1
                            for($i = 0;$i -lt $to_add.Length;$i++)
                            {
                                $to_add[$i].processing_order = $ctr
                                $ctr++
                            }
                        }
                        $to_add = $present_value + $to_add
                        $config[$child_index].Set_Item($key,$to_add)
                    }
                    else
                    {
                        $config[$child_index].Add($key,$to_add)
                    }
                    
                }
                if($remove_category -eq 4)
                {
                    $option_parent = $config[$parent_index].Get_Item("options")
                    $option_child = @{}
                    if($config[$child_index].ContainsKey("options"))
                    {
                        $option_child = $config[$child_index].Get_Item("options")
                        $config[$child_index].Remove("options")
                    }
                    foreach($pair in $option_parent.Keys)
                    {
                        if($option_child.ContainsKey($pair))
                        {
                            
                        }
                        else
                        {
                            $option_child.Add($pair,$option_parent.Get_Item($pair))
                        }
                    }
                    $config[$child_index].Add("options",$option_child)
                    break;
                }

                if($config[$child_index].ContainsKey($key)) 
                {
                    if($remove_category -eq 2)
                    {
                        $config[$child_index].Set_Item($key,($config[$child_index].Get_Item($key)+$config[$parent_index].Get_Item($key)))
                    }
                }
                else
                {
                    
                    if($remove_category -eq 3 -or $remove_category -eq 2)
                    {
                        $config[$child_index].Add($key,$config[$parent_index].Get_Item($key))
                    }
                }
            }
        }
    }
    return $config    
}
