using Irony.Ast;
using Irony.Parsing;
using Irony.Parsing.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHCPDParser
{
    class ConfigParse : Common
    {
        public ConfigParse() 
        {
            KeyTerm ALLOW = ToTerm("allow");
            KeyTerm ADAPTIVE_LEASE_TIME_THRESHOLD = ToTerm("adaptive-lease-time-threshold");
            KeyTerm ALWAYS_BROADCAST = ToTerm("always-broadcast");
            KeyTerm ALWAYS_REPLY_RFC1048 = ToTerm("always-reply-rfc1048");
            KeyTerm AUTHORITATIVE = ToTerm("authoritative");
            KeyTerm BOOT_UNKNOWN_CLIENTS = ToTerm("boot-unknown-clients");
            KeyTerm BOOTING = ToTerm("booting");
            KeyTerm BOOTP = ToTerm("bootp");
            KeyTerm CLASS = ToTerm("vendor-class-identifier");
            KeyTerm CLIENT_HOSTNAME = ToTerm("client-hostname");
            KeyTerm CLIENT_UPDATES = ToTerm("client-updates");
            KeyTerm DB_TIME_FORMAT = ToTerm("db-time-format");
            KeyTerm DDNS_DOMAINNAME = ToTerm("ddns-domainname");
            KeyTerm DDNS_HOSTNAME = ToTerm("ddns-hostname");
            KeyTerm DDNS_REV_DOMAINNAME = ToTerm("ddns-rev-domainname");
            KeyTerm DDNS_UPDATE_STYPE = ToTerm("ddns-update-style");
            KeyTerm DDNS_UPDATES = ToTerm("ddns-updates");
            KeyTerm DEFAULT_LEASE_TIME = ToTerm("default-lease-time");
            KeyTerm DELAYED_ACK = ToTerm("delayed-ack");
            KeyTerm DENY = ToTerm("deny");
            KeyTerm DO_FORWARD_UPDATES = ToTerm("do-forward-updates");
            KeyTerm DYNAMIC_BOOTP_LEASE_CUTOFF = ToTerm("dynamic-bootp-lease-cutoff");
            KeyTerm DYNAMIC_BOOTP_LEASE_LENGTH = ToTerm("dynamic-bootp-lease-length");
            KeyTerm ENDS = ToTerm("ENDS");
            KeyTerm FAILOVER = ToTerm("failover");
            KeyTerm PEER = ToTerm("peer");
            KeyTerm FILENAME = ToTerm("filename");
            KeyTerm FIXED_ADDRESS = ToTerm("fixed-address address");
            KeyTerm FIXED_ADDRESS6 = ToTerm("fixed-address6 ip6-address");
            KeyTerm GET_LEASE_HOSTNAMES = ToTerm("get-lease-hostnames");
            KeyTerm GROUP = ToTerm("group");
            KeyTerm SHARED_NETWORK = ToTerm("shared-network");
            KeyTerm HOST_IDENTIFIER = ToTerm("host-identifier");
            KeyTerm HOST = ToTerm("host");
            KeyTerm INFINITE_IS_RESERVED = ToTerm("infinite-is-reserved");
            KeyTerm LEASE = ToTerm("lease");
            KeyTerm LEASE_FILE_NAME = ToTerm("lease-file-name");
            KeyTerm leaseDeclarations = ToTerm("lease-declarations");
            KeyTerm MAX_ACK_DELAY = ToTerm("max-ack-delay");
            KeyTerm MAX_LEASE_TIME = ToTerm("max-lease-time");
            KeyTerm NETMASK = ToTerm("netmask");
            KeyTerm NEXT_SERVER = ToTerm("next-server");
            KeyTerm NOT_AUTHORITATIVE = ToTerm("not-authoritative");
            KeyTerm ONE_LEASE_PER_CLIENT = ToTerm("one-lease-per-client");
            KeyTerm OPTION_PARAMETER = ToTerm("option");
            KeyTerm POOL = ToTerm("pool");
            KeyTerm RANGE = ToTerm("range");
            KeyTerm SERVER_IDENTIFIER = ToTerm("server-name");
            KeyTerm SERVER_NAME = ToTerm("server-name");
            KeyTerm SUBNET = ToTerm("subnet");
            KeyTerm SUBNET6 = ToTerm("subnet6");
            KeyTerm STARTS = ToTerm("starts");
            KeyTerm TEMPORARY = ToTerm("temporary");
            KeyTerm TIMESTAMP = ToTerm("timestamp");
            KeyTerm UID = ToTerm("uid");
            KeyTerm UNKNOWN_CLIENTS = ToTerm("unknown-clients");
            KeyTerm USE_HOST_DECL_NAME = ToTerm("use-host-decl-names");
            KeyTerm USE_LEASE_ADDR_FOR_DEFAULT_ROUTE = ToTerm("use-lease-addr-for-default-route");
            KeyTerm USER_CLASS = ToTerm("dhcp-user-class");
            KeyTerm VENDOR_CLASS = ToTerm("dhcp-vendor-class");

            NonTerminal addressRangeDeclaration = new NonTerminal("address-range-declaration");
            NonTerminal addressRange6Declaration = new NonTerminal("address-range6-declaration");
            NonTerminal allowDenyKeyword = new NonTerminal("allow-deny-keyword");
            NonTerminal confFile = new NonTerminal("conf-file");
            NonTerminal classDeclaration = new NonTerminal("class-declaration");
            NonTerminal declaration = new NonTerminal("declaration");
            NonTerminal failOverDeclaration = new NonTerminal("failover-declaration");
            NonTerminal fixedAddrParameter = new NonTerminal("fixed-addr-parameter");
            NonTerminal fixedPrefix6 = new NonTerminal("fixed-prefix6");
            NonTerminal declarations = new NonTerminal("declarations");
            NonTerminal groupDeclaration = new NonTerminal("group-declaration");
            NonTerminal hostDeclaration = new NonTerminal("host-declaration");
            NonTerminal ipAddrsOrHostnames = new NonTerminal("ip-addrs-or-hostnames");
            NonTerminal leaseDeclaration = new NonTerminal("lease_declaration");
            NonTerminal leaseFile = new NonTerminal("lease-file");
            NonTerminal leaseParameter = new NonTerminal("lease_parameter");
            NonTerminal leaseParameters = new NonTerminal("lease_parameters");
            NonTerminal leaseStatements = new NonTerminal("lease-statements");
            NonTerminal parameter = new NonTerminal("parameter");
            NonTerminal parameters = new NonTerminal("parameters");
            NonTerminal poolDeclaration = new NonTerminal("pool-declaration");
            NonTerminal sharedNetworkDeclaration = new NonTerminal("shared-network-declaration");
            NonTerminal statement = new NonTerminal("statement");
            NonTerminal statements = new NonTerminal("statements");
            NonTerminal subnetDeclaration = new NonTerminal("subnet-declaration");
            NonTerminal subnet6Declaration = new NonTerminal("subnet6-declaration");
            
            
            StringLiteral bits = new StringLiteral("bits", "\"");
            StringLiteral fixedAddressParameter = new StringLiteral("fixed-address-parameter", "\"");
            StringLiteral hexNumbers = new StringLiteral("hex-numbers", "\"");
            IdentifierTerminal net = new IdentifierTerminal("net");
            net.AllChars = net.AllChars + ".";
            IdentifierTerminal netmask = new IdentifierTerminal("netmask");
            netmask.AllChars = net.AllChars + ".";
          

            /* conf-file :== parameters declarations END_OF_FILE
             * conf-file :== parameters declarations END_OF_FILE
             */
            confFile.Rule = statements+ Eof;

            this.Root = confFile;

            /* 
             * parameters :== <nil> | parameter | parameters parameter
             * parameters :== <nil> | parameter | parameters parameter
             */

            parameters.Rule = nil | parameter | parameters + parameter;

            /*declarations :== <nil> | declaration | declarations declaration 
             declarations :== <nil> | declaration | declarations declaration */

            declarations.Rule = nil | declaration | declarations + declaration;

            /* lease-file :== lease-declarations END_OF_FILE
               lease-statements :== <nil>
                         | lease-declaration
                         | lease-declarations lease-declaration */

            leaseFile.Rule = leaseDeclarations + Eof;
            leaseStatements.Rule = nil | leaseDeclaration | leaseDeclarations + leaseDeclaration;

            /* statement :== parameter | declaration

               parameter :== DEFAULT_LEASE_TIME lease_time
                       | MAX_LEASE_TIME lease_time
                       | DYNAMIC_BOOTP_LEASE_CUTOFF date
                       | DYNAMIC_BOOTP_LEASE_LENGTH lease_time
                       | BOOT_UNKNOWN_CLIENTS boolean
                       | ONE_LEASE_PER_CLIENT boolean
                       | GET_LEASE_HOSTNAMES boolean
                       | USE_HOST_DECL_NAME boolean
                       | NEXT_SERVER ip-addr-or-hostname SEMI
                       | option_parameter
                       | SERVER-IDENTIFIER ip-addr-or-hostname SEMI
                       | FILENAME string-parameter
                       | SERVER_NAME string-parameter
                       | hardware-parameter
                       | fixed-address-parameter
                       | ALLOW allow-deny-keyword
                       | DENY allow-deny-keyword
                       | USE_LEASE_ADDR_FOR_DEFAULT_ROUTE boolean
                       | AUTHORITATIVE
                       | NOT AUTHORITATIVE

               declaration :== host-declaration
                     | group-declaration
                     | shared-network-declaration
                     | subnet-declaration
                     | VENDOR_CLASS class-declaration
                     | USER_CLASS class-declaration
                     | RANGE address-range-declaration */

            

            parameter.Rule = DEFAULT_LEASE_TIME + leaseTime |
                            MAX_LEASE_TIME + leaseTime |
                            DYNAMIC_BOOTP_LEASE_CUTOFF + date |
                            DYNAMIC_BOOTP_LEASE_LENGTH + leaseTime |
                            BOOT_UNKNOWN_CLIENTS  + BOOLEAN |
                            ONE_LEASE_PER_CLIENT + BOOLEAN |
                            GET_LEASE_HOSTNAMES + BOOLEAN |
                            USE_HOST_DECL_NAME + BOOLEAN |
                            NEXT_SERVER + ipAddrOrHostname + SEMI |
                            optionParameter |
                            SERVER_IDENTIFIER + ipAddrOrHostname + SEMI |
                            FILENAME + stringParameter |
                            hardwareParameter |
                            fixedAddressParameter | 
                            optionStatement |
                            LOG_FACILITY + IDENTIFIER + SEMI|
                            ALLOW + allowDenyKeyword + SEMI |
                            DENY + allowDenyKeyword + SEMI|
                            USE_LEASE_ADDR_FOR_DEFAULT_ROUTE + BOOLEAN |
                            AUTHORITATIVE |
                            PRIMARY + SEMI|
                            NOT_AUTHORITATIVE;
                
            declaration.Rule = hostDeclaration |
                               groupDeclaration |
                               sharedNetworkDeclaration |
                               subnetDeclaration |
                               VENDOR_CLASS + classDeclaration |
                               USER_CLASS + classDeclaration |
                               poolDeclaration |
                               failOverDeclaration |
                               RANGE + addressRangeDeclaration;



            statement.Rule = DEFAULT_LEASE_TIME + leaseTime |
                            MAX_LEASE_TIME + leaseTime |
                            DYNAMIC_BOOTP_LEASE_CUTOFF + date |
                            DYNAMIC_BOOTP_LEASE_LENGTH + leaseTime |
                            BOOT_UNKNOWN_CLIENTS + BOOLEAN |
                            ONE_LEASE_PER_CLIENT + BOOLEAN |
                            GET_LEASE_HOSTNAMES + BOOLEAN |
                            USE_HOST_DECL_NAME + BOOLEAN |
                            NEXT_SERVER + ipAddrOrHostname + SEMI |
                            optionParameter |
                            SERVER_IDENTIFIER + ipAddrOrHostname + SEMI |
                            FILENAME + stringParameter |
                            hardwareParameter |
                            fixedAddressParameter |
                            optionStatement |
                            ALLOW + allowDenyKeyword + SEMI |
                            DENY + allowDenyKeyword + SEMI |
                            USE_LEASE_ADDR_FOR_DEFAULT_ROUTE + BOOLEAN |
                            AUTHORITATIVE + SEMI |
                            NOT_AUTHORITATIVE + SEMI |
                            hostDeclaration |
                               groupDeclaration |
                               sharedNetworkDeclaration |
                               subnetDeclaration |
                               VENDOR_CLASS + classDeclaration |
                               USER_CLASS + classDeclaration |
                               poolDeclaration |
                               failOverDeclaration |
                               RANGE + addressRangeDeclaration;
            // statements.Rule = MakeStarRule(parameter, declaration);
            //statements.Rule = statement | statements;
            //statements.Rule = declarations | parameters | statements;

            statements.Rule = nil | statement | statements + statement;



            failOverDeclaration.Rule = FAILOVER + PEER + STRING  + SEMI |
                                        FAILOVER + PEER + STRING + LBRACE +  parameters + RBRACE ;

            /* host-declaration :== hostname RBRACE parameters declarations LBRACE */

            hostDeclaration.Rule = HOST + hostname + LBRACE + statements + RBRACE;

            /* class-declaration :== STRING LBRACE parameters declarations RBRACE
            */

            classDeclaration.Rule = CLASS + STRING + LBRACE + statements + RBRACE;

            /* shared-network-declaration :==
                        hostname LBRACE declarations parameters RBRACE */

            sharedNetworkDeclaration.Rule = SHARED_NETWORK + hostname + LBRACE + statements + RBRACE;

            /* subnet-declaration :==
                net NETMASK netmask RBRACE parameters declarations LBRACE */

            subnetDeclaration.Rule = SUBNET + ipAddress + NETMASK + ipAddress + LBRACE + statements + RBRACE ;

            /* subnet6-declaration :==
                net / bits RBRACE parameters declarations LBRACE */

            subnet6Declaration.Rule = SUBNET6 + net | bits + LBRACE + statements + RBRACE;

            /* group-declaration :== RBRACE parameters declarations LBRACE */

            groupDeclaration.Rule = GROUP + LBRACE + statements + RBRACE |
                                    GROUP + LBRACE + RBRACE;

            /* fixed-addr-parameter :== ip-addrs-or-hostnames SEMI
               ip-addrs-or-hostnames :== ip-addr-or-hostname
                           | ip-addrs-or-hostnames ip-addr-or-hostname */

            fixedAddrParameter.Rule = ipAddrsOrHostnames + SEMI;
            ipAddrsOrHostnames.Rule = ipAddrOrHostname | ipAddrsOrHostnames + ipAddrOrHostname;

            /* lease_declaration :== LEASE ip_address LBRACE lease_parameters RBRACE

               lease_parameters :== <nil>
                          | lease_parameter
                          | lease_parameters lease_parameter

               lease_parameter :== STARTS date
                         | ENDS date
                         | TIMESTAMP date
                         | HARDWARE hardware-parameter
                         | UID hex_numbers SEMI
                         | HOSTNAME hostname SEMI
                         | CLIENT_HOSTNAME hostname SEMI
                         | CLASS identifier SEMI
                         | DYNAMIC_BOOTP SEMI */

            leaseDeclaration = new NonTerminal("lease-declaration");
            leaseDeclaration.Rule = LEASE + ipAddress + LBRACE + leaseParameters + RBRACE;
            leaseParameters = new NonTerminal("lease-parameters");
            leaseParameters.Rule = leaseParameter | leaseParameters + leaseParameter;
            leaseParameter = new NonTerminal("lease-parameter");
            leaseParameter.Rule = STARTS + date |
                                  ENDS + date |
                                  TIMESTAMP + date |
                                  UID + hexNumbers + SEMI |
                                  HOST + hostname + SEMI |
                                  CLIENT_HOSTNAME + hostname + SEMI |
                                  CLASS + IDENTIFIER + SEMI |
                                  DYNAMIC_BOOTP + SEMI;



            poolDeclaration.Rule = POOL + LBRACE + statements + RBRACE ;


            /* address-range-declaration :== ip-address ip-address SEMI
                               | DYNAMIC_BOOTP ip-address ip-address SEMI */

            addressRangeDeclaration.Rule = ipAddress + ipAddress + SEMI |
                                            DYNAMIC_BOOTP + ipAddress + ipAddress + SEMI;

            /* address-range6-declaration :== ip-address6 ip-address6 SEMI
                               | ip-address6 SLASH number SEMI
                               | ip-address6 [SLASH number] TEMPORARY SEMI */

            addressRange6Declaration.Rule = ip6Address + ip6Address + SEMI |
                                            ip6Address + SLASH + number + SEMI |
                                            ip6Address + (SLASH + number) + TEMPORARY + SEMI;

            /* fixed-prefix6 :== ip6-address SLASH number SEMI */
            fixedPrefix6 = new NonTerminal("fixed-prefix6");
            fixedPrefix6.Rule = ip6Address + SLASH + number + SEMI;

            /* allow-deny-keyword :== BOOTP
                        | BOOTING
                        | DYNAMIC_BOOTP
                        | UNKNOWN_CLIENTS */

            allowDenyKeyword.Rule = BOOTP | BOOTING | DYNAMIC_BOOTP | UNKNOWN_CLIENTS | UNKNOWN_CLIENTS2 |  CLIENT_UPDATES;

            MarkTransient( declaration, declarations);
        }
    }
}
