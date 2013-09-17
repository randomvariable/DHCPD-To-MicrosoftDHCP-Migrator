$global:option_definitions = "subnet-mask",
                             "time-offset",
                             "routers",
                             "time-servers",
                             "ien116-name-servers", 
                             "domain-name-servers",
                             "log-servers",
                             "cookie-servers",
                             "lpr-servers",
                             "impress-servers",
                             "resource-location-servers",
                             "host-name",
                             "boot-size",
                             "merit-dump",
                             "domain-name",
                             "swap-server",
                             "root-path",
                             "extensions-path",
                             "ip-forwarding",
                             "non-local-source-routing",
                             "policy-filter",
                             "max-dgram-reassembly",
                             "default-ip-ttl",
                             "path-mtu-aging-timeout",
                             "path-mtu-plateau-table",
                             "interface-mtu",
                             "all-subnets-local",
                             "broadcast-address",
                             "perform-mask-discovery",
                             "mask-supplier",
                             "router-discovery",
                             "router-solicitation-address",
                             "static-routes",
                             "trailer-encapsulation",
                             "arp-cache-timeout",
                             "ieee802-3-encapsulation",
                             "default-tcp-ttl",
                             "tcp-keepalive-interval",
                             "tcp-keepalive-garbage",
                             "nis-domain",
                             "nis-servers",
                             "ntp-servers",
                             "vendor-encapsulated-options",
                             "netbios-name-servers",
                             "netbios-dd-server",
                             "netbios-node-type",
                             "netbios-scope",
                             "font-servers",
                             "x-display-manager",
                             "nisplus-domain",
                             "nisplus-servers",
                             "tftp-server-name",
                             "bootfile-name",
                             "mobile-ip-home-agent",
                             "smtp-server",
                             "pop-server",
                             "nntp-server",
                             "www-server",
                             "finger-server",
                             "irc-server",
                             "streettalk-server",
                             "streettalk-directory-assistance-server"
                             
$global:standard_options = @{}
[int]$option_ctr = 1
for([int]$i=0;$i -lt $global:option_definitions.Length;$i++)
{
    
    
    $global:standard_options.Add($global:option_definitions[$i],$option_ctr)
    if($option_ctr -eq 49)
    {
        if($global:option_definitions[$i].CompareTo("x-display-manager") -ne 0)
        {
            Write-Host "Error"
        }
        $option_ctr = 63
    }
    $option_ctr++

    
}

$global:standard_options.Add("dhcp-lease-time",51)
$global:standard_options.Add("dhcp-renewal-time",58)
$global:standard_options.Add("dhcp-rebinding-time",59)