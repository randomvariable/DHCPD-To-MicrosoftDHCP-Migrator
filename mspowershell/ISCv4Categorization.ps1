
################CATEGORIZATION OF ISC COMMANDS##############
$global:boolean_parameters = "always-broadcast", "always-reply-rfc1048", "boot-unknown-clients",
                             "ddns-updates", "do-forward-updates", "get-lease-hostnames",
                             "one-lease-per-client", "ping-check", "stash-agent-options", "update-optimization",
                             "update-static-leases", "use-host-decl-names", "use-lease-addr-for-default-route"

$global:boolean_imp_parameters = @("ping-check")
$global:other_parameters = "ddns-hostname","ddns-domainname","ddns-rev-domainname","ddns-update-style",
                             "default-lease-time", "dynamic-bootp-lease-length","filename" ,"lease-file-name",
                             "local-port", "local-address", "log-facility", "max-lease-time", "min-lease-time",
                             "min-secs", "next-server", "omapi-port", "pid-file-name", "ping-timeout", "server-identifier",
                             "server-name", "site-option-space", "vendor-option-space"
$global:other_imp_parameters = "min-secs", "default-lease-time", "max-lease-time", "min-lease-time", "ping-timeout", "vendor-option-space"
$global:special_parameters = "authoritative", "not", "range", "dynamic-bootp-lease-cutoff", "fixed-address",
                             "hardware" , "allow" , "deny", "ignore", "option", "match"

$global:topology_parameters = "group", "shared-network", "subnet", "pool", "host", "class", "subclass", "if", "elsif", "else"

$global:constructs = "{", "}", ";"


#################FUNCTION : To return the category of the ISC command ###################################
function IPv4category([string]$command)
{
    foreach($item in $global:topology_parameters)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 1
        }
    }
    foreach($item in $global:boolean_parameters)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 2
        }
    }
    foreach($item in $global:other_parameters)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 3
        }
    }
    foreach($item in $global:constructs)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 4
        }
    }
    foreach($item in $global:special_parameters)
    {
        if($item.CompareTo($command) -eq 0)
        {
            return 5
        }
    }
    
    
    
    return 6
}