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
            KeyTerm DEFAULT_LEASE_TIME = ToTerm("default-lease-time");
            KeyTerm MAX_LEASE_TIME = ToTerm("max-lease-time");
            KeyTerm DYNAMIC_BOOTP_LEASE_CUTOFF = ToTerm("dynamic-bootp-lease-cutoff");
            KeyTerm DYNAMIC_BOOTP_LEASE_LENGTH = ToTerm("dynamic-bootp-lease-length");
            KeyTerm BOOT_UNKNOWN_CLIENTS = ToTerm("boot-unknown-clients");
            KeyTerm ONE_LEASE_PER_CLIENT = ToTerm("one-lease-per-client");
            KeyTerm GET_LEASE_HOSTNAMES = ToTerm("get-lease-hostnames");
            KeyTerm USE_HOST_DECL_NAME = ToTerm("use-host-decl-names");
            KeyTerm NEXT_SERVER = ToTerm("next-server");
            KeyTerm SERVER_IDENTIFIER = ToTerm("server-name");
            KeyTerm SERVER_NAME = ToTerm("server-name");
            KeyTerm FILENAME = ToTerm("filename");
            KeyTerm ALLOW = ToTerm("allow");
            KeyTerm DENY = ToTerm("deny");
            KeyTerm USE_LEASE_ADDR_FOR_DEFAULT_ROUTE = ToTerm("use-lease-addr-for-default-route");
            KeyTerm AUTHORITATIVE = ToTerm("authoritative");
            KeyTerm NOT_AUTHORITATIVE = ToTerm("not-authoritative");
            KeyTerm BOOTING = ToTerm("allow-booting");
            KeyTerm BOOTP = ToTerm("allow-bootp");
            KeyTerm CLASS = ToTerm("vendor-class-identifier");
            KeyTerm VENDOR_CLASS = ToTerm("dhcp-vendor-class");
            KeyTerm USER_CLASS = ToTerm("dhcp-user-class");
            KeyTerm RANGE = ToTerm("range");
            NonTerminal confFile = new NonTerminal("conf-file");
            StringLiteral fixedAddressParameter = new StringLiteral("fixed-address-parameter", "\"");
            NonTerminal parameters = new NonTerminal("parameters");
 
            NonTerminal declarations = new NonTerminal("declarations");
            KeyTerm CLIENT_HOSTNAME = ToTerm("client-hostname");
            KeyTerm ENDS = ToTerm("ENDS");
            StringLiteral hexNumbers = new StringLiteral("hex-numbers", "\"");
            KeyTerm HOSTNAME = ToTerm("hostname");
            KeyTerm LEASE = ToTerm("lease");
            KeyTerm NETMASK = ToTerm("netmask");
            StringLiteral net = new StringLiteral("net", "\"");
            StringLiteral netmask = new StringLiteral("netmask", "\"");
            KeyTerm STARTS = ToTerm("starts");
            KeyTerm TEMPORARY = ToTerm("temporary");
            KeyTerm UID = ToTerm("uid");
            KeyTerm TIMESTAMP = ToTerm("timestamp");
            KeyTerm UNKNOWN_CLIENTS = ToTerm("unknown-clients");
            NonTerminal leaseFile = new NonTerminal("lease-file");
            NonTerminal leaseStatements = new NonTerminal("lease-statements");
            NonTerminal statement = new NonTerminal("statement");
            NonTerminal parameter = new NonTerminal("parameter");
            NonTerminal declaration = new NonTerminal("declaration");
            NonTerminal hostDeclaration = new NonTerminal("host-declaration");
            NonTerminal classDeclaration = new NonTerminal("class-declaration");
            NonTerminal sharedNetworkDeclaration = new NonTerminal("shared-network-declaration");
            NonTerminal subnetDeclaration = new NonTerminal("subnet-declaration");
            NonTerminal subnet6Declaration = new NonTerminal("subnet6-declaration");
            NonTerminal groupDeclaration = new NonTerminal("group-declaration");
            NonTerminal fixedAddrParameter = new NonTerminal("fixed-addr-parameter");
            NonTerminal ipAddrsOrHostnames = new NonTerminal("ip-addrs-or-hostnames");
            NonTerminal leaseDeclaration = new NonTerminal("lease_declaration");
            NonTerminal leaseParameters = new NonTerminal("lease_parameters");
            NonTerminal leaseParameter = new NonTerminal("lease_parameter");
            NonTerminal addressRangeDeclaration = new NonTerminal("address-range-declaration");
            NonTerminal addressRange6Declaration = new NonTerminal("address-range6-declaration");
            NonTerminal fixedPrefix6 = new NonTerminal("fixed-prefix6");
            NonTerminal allowDenyKeyword = new NonTerminal("allow-deny-keyword");
            KeyTerm leaseDeclarations = ToTerm("lease-declarations");

            

            StringLiteral bits = new StringLiteral("bits", "\"");
            var leaseTime = new NumberLiteral("leaseTime");

            /* conf-file :== parameters declarations END_OF_FILE
             * conf-file :== parameters declarations END_OF_FILE
             */
            confFile.Rule = parameters + declarations + Eof;

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

            statement.Rule = parameter | declaration;


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
                            SERVER_NAME + stringParameter | 
                            hardwareParameter |
                            fixedAddressParameter | 
                            ALLOW + allowDenyKeyword |
                            DENY + allowDenyKeyword |
                            USE_LEASE_ADDR_FOR_DEFAULT_ROUTE + BOOLEAN |
                            AUTHORITATIVE |
                            NOT_AUTHORITATIVE;
                
            declaration.Rule = hostDeclaration |
                               groupDeclaration |
                               sharedNetworkDeclaration |
                               subnetDeclaration |
                               VENDOR_CLASS + classDeclaration |
                               USER_CLASS + classDeclaration |
                               RANGE + addressRangeDeclaration;

            /* host-declaration :== hostname RBRACE parameters declarations LBRACE */

            hostDeclaration.Rule = hostname + RBRACE + parameters + declarations + LBRACE;

            /* class-declaration :== STRING LBRACE parameters declarations RBRACE
            */

            classDeclaration.Rule = STRING + LBRACE + parameters + declarations + RBRACE;

            /* shared-network-declaration :==
                        hostname LBRACE declarations parameters RBRACE */

            sharedNetworkDeclaration.Rule = hostname + LBRACE + declarations + parameters + RBRACE;

            /* subnet-declaration :==
                net NETMASK netmask RBRACE parameters declarations LBRACE */

            subnetDeclaration.Rule = net + NETMASK + netmask + RBRACE + parameters + declarations + LBRACE;

            /* subnet6-declaration :==
                net / bits RBRACE parameters declarations LBRACE */

            subnet6Declaration.Rule = net | bits + RBRACE + parameters + declarations + LBRACE;

            /* group-declaration :== RBRACE parameters declarations LBRACE */

            groupDeclaration.Rule = RBRACE + parameters + declarations + LBRACE;


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
                                  HOSTNAME + hostname + SEMI |
                                  CLIENT_HOSTNAME + hostname + SEMI |
                                  CLASS + identifier + SEMI |
                                  DYNAMIC_BOOTP + SEMI;


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


            allowDenyKeyword.Rule = BOOTP | BOOTING | DYNAMIC_BOOTP + UNKNOWN_CLIENTS;



        }



    }
}
