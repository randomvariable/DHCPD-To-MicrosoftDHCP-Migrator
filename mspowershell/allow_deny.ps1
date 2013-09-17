


$global:allow_deny_category1 = "unknown-clients","bootp","booting","duplicates","declines","client-updates",
                               "known-clients"
$global:allow_deny_category2 = "members", "dynamic", "authenticated", "unauthenticated", "all" 



#################FUNCTION : To return the category of the allow deny statement#####################
function allow_deny_category([string]$command)   
{
    foreach($item in $global:allow_deny_category1)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 1
        }
    }
    foreach($item in $global:allow_deny_category2)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 2
        }
    }
    return 3
}

#################FUNCTION : To return the allow/deny/ignore class to be added####################
function allow_deny_helper([string] $filter)
{
    
    $allow_deny_category = allow_deny_category $filter
    $to_add = 0;
    switch($allow_deny_category)
    {
        "1"
        {
            $to_add = $filter
        }
        "2"
        {
            if($filter.CompareTo("members") -eq 0)
            {
                $to_add = next_token
                $to_add = next_token
               
            }
            else
            {
                if($filter.CompareTo("dynamic") -eq 0)
                {
                    $junk = next_token
                    $junk = next_token
                    $to_add = "dynamic-bootp-clients"
                }
                else
                {
                    $to_add = $filter
                    $junk = next_token
                }
            }
            
        }
    }
    return $to_add
}
