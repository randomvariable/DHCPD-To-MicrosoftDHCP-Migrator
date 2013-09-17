



$source =@"

public class option_pair
{
    public string option_space;
    public int option_id;
    public void init(string opt_space,int id)
    {
        option_space = opt_space;
        option_id = id;
    }
}
public class policy
{
    public bool has_range;
    public string name;
    public int processing_order;
    public bool enabled;
    public string logical_operator;
    public string description;
    public string[] tag;
    public string[] condition;
    public string[] equality_check;
    public dhcp_range[] range;
    public void init(string n,int order,bool check,string op,string des,string[] cond,string[] eq, string[] t)
    {
        has_range = false;
        name = n;
        processing_order = order;
        enabled = check;
        logical_operator = op;
        description = des;
        condition = new string[cond.Length];
        equality_check = new string[cond.Length];
        tag = new string[t.Length];
        for(int i=0;i<cond.Length;i++)
        {
            equality_check[i] = eq[i];
            tag[i] = t[i];
            condition[i] = cond[i];
            
        }
        
    }
    public void set_range(dhcp_range[] r)
    {
        range = new dhcp_range[r.Length];
        for(int i=0;i<r.Length;i++)
        {
            range[i] = r[i];
        }
        has_range = true;
    }

}
public class dhcp_class
{
    public string name;
    public string data;
    public string type;
    public string description;
    public string option_space;
    public void init(string n,string da,string t,string d)
    {
        name = n;
        data = da;
        type = t;
        description = d;
        

    }
    public void set_option_space(string opt)
    {
        option_space = opt;
    }

}


public class match_expression
{
    public string match_id;
    public string match_value;
  
    
    public void init(string a,string b)
    {
        match_id = a;
        match_value = b;
      
    }

}
public class option_record
{
    public string name;
    public int id;
    public bool new_option;
    public bool multivalued;
    public string type;

    public void init(string n,int i, string t,bool multi)
    {
        name = n;
        id = i;
        type = t;
        multivalued = multi;
        new_option = true;
    }

    public void init(string n,int i)
    {
        name = n;
        id = i;
        new_option = false;
    }
}



public class dhcp_range
{
    public dhcp_ip_address low_addr;
    public dhcp_ip_address high_addr;
    public bool check;
    public bool is_deny;
    public void set_addr(string low,string high)
    {
        low_addr = new dhcp_ip_address();
        low_addr.set_address(low);
        if(System.String.Compare(high,"-1") != 0)
        {
            high_addr = new dhcp_ip_address();
            high_addr.set_address(high);
            check = true;
        }
        else 
        {
            high_addr = new dhcp_ip_address();
            high_addr.set_address(low);
            check = false;
        }
    }
    public void set_addr(dhcp_ip_address low,dhcp_ip_address high)
    {
        check = true;
        low_addr = low;
        high_addr = high;
    }

    public dhcp_ip_address get_low_addr()
    {
        return low_addr;
    }
    public dhcp_ip_address get_high_addr()
    {
        return high_addr;
    }
}


public class dhcp_date
{
    public int W;
    public int year;
    public int month;
    public int day;
    public int hour;
    public int min;
    public int sec;

  
}


public class dhcp_ip_address
{
    public int[] byte_ip;
    public string ip_address;
    
    public void set_address(string ip)
    {
        ip_address = ip;
        byte_ip = new int[4];
        int i=0;
        while(i<4)
        {
            int pos = ip.IndexOf('.');
            string temp;
            if(pos != -1) temp = ip.Substring(0,pos);
            else temp = ip;
            byte_ip[i] = System.Convert.ToInt32(temp);
            i++;
            ip = ip.Substring(pos+1,ip.Length-pos-1);
        }
    }

    public dhcp_ip_address get_address(int val1,int val2,int inc)
    {
        dhcp_ip_address temp = new dhcp_ip_address();
        temp.byte_ip = new int[4];
        int i=3;
        while(i>=0)
        {
            if(byte_ip[i] == val1) i--;
            else break;
        }
        for(int j=0;j<i;j++)
        {
            temp.byte_ip[j] = byte_ip[j];
        }
        temp.byte_ip[i] = byte_ip[i] + inc;
        for(int j=i+1;j<4;j++)
        {
            temp.byte_ip[j] = val2;
        }
        temp.set_string();
        return temp;
    }

    public dhcp_ip_address get_next_address()
    {
        return get_address(255,0,1);
    }
    public dhcp_ip_address get_previous_address()
    {
        return get_address(0,255,-1);
    }
    public void set_string()
    {
        ip_address =byte_ip[0].ToString();
        for(int i=1;i<4;i++)
        {
            ip_address = ip_address + "." + byte_ip[i].ToString();
        }
    }

    public int compare(dhcp_ip_address address)
    {
        int i=0;
        while(i<4)
        {
            if(byte_ip[i] < address.byte_ip[i])
            {
                return -1;
            }
            else if(byte_ip[i] > address.byte_ip[i])
            {
                return 1;
            }
            else i++;
        }
        return 0;
    }
    
 
}

"@
Add-Type  -TypeDefinition $source
$comments = "#[^`n]*`n"
$text = "`"([^`"]*)`""
$open_brace = "({)"
$close_brace = "(})"
$semicolon = "(;)"
$comma = ","
$space = "\s"

$global:splitter = "`"([^`"]*)`"| #.* | ({)|(})|(;)|,|\s"
[string[]]$global:content = ""
function read_isc_file()
{
    $isc_conf_file_path = Read-Host "Please enter the path of the ISC configuration file"
    #$isc_conf_file_path = "C:\Users\sabansal\Desktop\Migration\ISC\ISC.conf.txt"
    #$isc_conf_file_path = "C:\Users\sabansal\Desktop\Migration\ISC\dhcpd.conf"
    $loop = 1
    while($loop)
    {
        try
        {
            $ErrorActionPreference = "Stop";
            $temp_content = Get-Content -Path $isc_conf_file_path
            $loop = 0
        }
        catch
        {
            $isc_conf_file_path = Read-Host "Could not read! Try again!"
            $loop = 1
        }
    }

    
    $temp_content = [string]::Join("`n", $temp_content)
    $global:content = [regex]::Split($temp_content,$global:splitter)
    

    [int]$include_index = [System.Array]::IndexOf($global:content,"include")

    $previous_index = 0
    while($include_index -ne -1)
    {
    
        if($include_index -eq 0)
        {
            $string_upto_include = ""
        }
        else
        {
            $string_upto_include = $global:content[0 .. ($include_index-1)]
        }
        $after_index = $include_index+1
        while($global:content[$after_index].CompareTo("") -eq 0 -and $after_index -lt $global:content.Length)
        {
                $after_index++
        }
        
        if(($after_index+1) -lt ($global:content.Length))
        {
            $string_after_include = $global:content[$after_index .. (($global:content.Length) - 1)]
        }
        else
        {
            $string_after_include = ""
        }
        
        
        try
        {
            $ErrorActionPreference = "Stop";
            $string_to_include = Get-Content $global:content[($after_index)]
            $string_to_include = [regex]::Split($string_to_include,$global:splitter)
            $global:content = $string_upto_include + $string_to_include + $string_after_include
            $include_index = [System.Array]::IndexOf($global:content,"include",$previous_index)
        
            continue
        }
        catch
        {
                $include_index = [System.Array]::IndexOf($global:content,"include",($include_index+1))
                $previous_index = $include_index + 1
                continue
        } 
    }
    
}


#################FUNCTION : To return the next Token ###################################
function next_token()
{
    
  
    while($global:ctr -lt $global:content.Length -and ($global:content[$global:ctr] -eq $null -or $global:content[$global:ctr].CompareTo("") -eq 0))
    {
        $global:ctr++
    }
    $to_return = ""
    if($global:ctr -eq $global:content.Length) 
    {
        if($global:v6)
        {
            return ""
        }
        else
        {
            $temp_content = "C:\Users\t-rahils\Desktop\Final Project\testv6"
            $global:content = [regex]::Split($temp_content,$global:splitter)
            $global:ctr = 0
            $global:v6 = 1
            return ""
        }
    }
    else
    {
        
        $to_return = $global:content[$global:ctr]
        if($to_return.StartsWith("`"") -and $to_return.EndsWith("`""))
        {

            $to_return = $to_return.Substring(1,$to_return.Length-2)
        }
    }
    $global:ctr = $global:ctr + 1
    return $to_return
}



