/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

grammar DHCPDConfig;
import DHCPLexer;

options
{
    language=CSharp_v4_5;
}
 

config: statement*;

stringParameter: QUOTE STRING QUOTE | STRING;

parameter: DEFAULT_LEASE_TIME leaseTime SEMI |
           MAX_LEASE_TIME leaseTime SEMI |
           DYNAMIC_BOOTP_LEASE_CUTOFF Date SEMI |
           DYNAMIC_BOOTP_LEASE_LENGTH leaseTime SEMI |
           DDNS_UPDATE_STYLE stringParameter SEMI |
           BOOT_UNKNOWN_CLIENTS Boolean  SEMI|
           ONE_LEASE_PER_CLIENT Boolean  SEMI|
           USE_HOST_DECL_NAME Boolean  SEMI|
           NEXT_SERVER stringParameter SEMI|          
           SERVER_IDENTIFIER stringParameter SEMI |
           FILENAME stringParameter  SEMI|
           hardwareParameter  SEMI|
           fixedAddressParameter SEMI |
           ALLOW AllowDenyKeyword SEMI|
           DENY AllowDenyKeyword SEMI |
           USE_LEASE_ADDR_FOR_DEFAULT_ROUTE Boolean SEMI |
           AUTHORITATIVE SEMI|
           NOT_AUTHORITATIVE SEMI|
           PRIMARY SEMI |
           SECONDARY SEMI|
            leaseParameter SEMI|
            optionStatement SEMI |
            peerStatement SEMI |
            SERVERDUIDSTRING |
            OPTION optionStatement SEMI;
           
declaration: hostDeclaration |
             groupDeclaration |
             sharedNetworkDeclaration |
             subnetDeclaration |
             VENDOR_CLASS classDeclaration |
             USER_CLASS classDeclaration |
             failoverDeclaration |
             failoverStateDeclaration |
             addressRangeDeclaration|
             leaseDeclaration |
             poolDeclaration ;
           


statement: (parameter | declaration) ;

statements: statement*;

hostname: QUOTE STRING QUOTE | STRING | NUMBER;

hostDeclaration: HOST hostname LBRACE statements RBRACE;

klass: QUOTE STRING QUOTE | STRING;

classDeclaration: CLASS klass LBRACE statements RBRACE;

sharedNetwork: QUOTE STRING QUOTE | STRING;

sharedNetworkDeclaration: SHARED_NETWORK sharedNetwork LBRACE statements RBRACE;

subnet: ip4Address;

netmask: ip4Address;

subnetDeclaration: SUBNET subnet NETMASK netmask LBRACE statements RBRACE;

subnet6: QUOTE STRING QUOTE | STRING;

subnet6Declaration:  SUBNET6 stringParameter LBRACE statements RBRACE;


groupDeclaration: GROUP LBRACE statements RBRACE;

fixedAddress: ip4Address;

fixedAddressParameter: FIXED_ADDRESS fixedAddress ;

ipAddrOrHostnames: (QUOTE STRING QUOTE | STRING)+ ;

lease: QUOTE STRING QUOTE | STRING;

leaseAddress: ip4Address;

leaseDeclaration: LEASE leaseAddress LBRACE leaseParameters RBRACE;

leaseParameters: leaseParameter*;

ip6net: QUOTE STRING QUOTE | STRING;

ip4Address: Ip4Address;

hostnameOrIpAddress: hostname | ip4Address;

timestamp: (TSTP | TSFP | ATSFP | CLTT) NUMBER Date;

startEnd: (STARTS | ENDS) NUMBER Date ;
           


leaseParameter: startEnd  SEMI |
                timestamp SEMI |
                hardwareParameter SEMI |
                UIDSTRING |
                CLIENT_HOSTNAME stringParameter SEMI |
                BINDING STATE state SEMI|
                NEXT BINDING STATE state SEMI |
                CLASS STRING SEMI |
                DYNAMIC_BOOTP SEMI ;



poolDeclaration: POOL LBRACE statements RBRACE;

rangeLow: ip4Address;

rangeHigh: ip4Address;

addressRangeDeclaration: RANGE rangeLow rangeHigh SEMI |
                        DYNAMIC_BOOTP stringParameter stringParameter SEMI;

rangeLow6: QUOTE STRING QUOTE | STRING;

rangeHigh6: QUOTE STRING QUOTE | STRING;

addressRange6Declaration: rangeLow6 rangeHigh6 SEMI |
                          stringParameter SLASH NUMBER SEMI |
                          stringParameter LBRACKET SLASH NUMBER RBRACKET TEMPORARY SEMI;
                          

ip6Address: QUOTE STRING QUOTE | STRING;

fixedPrefix6: ip6Address SLASH NUMBER SEMI;


                  
failoverDeclaration: FAILOVER PEER hostnameOrIpAddress LBRACE statements RBRACE 
                     | FAILOVER PEER hostnameOrIpAddress SEMI ;

failoverStateDeclaration: FAILOVER PEER hostnameOrIpAddress STATE LBRACE failoverStateStatement* RBRACE ;

state: QUOTE STRING QUOTE | STRING;

date: Date |STRING STRING;

failoverStateStatement: MY STATE state AT NUMBER date SEMI|
                        PARTNER STATE state AT NUMBER date SEMI;

peerStatement: PEER ADDRESS hostnameOrIpAddress |
               PEER PORT NUMBER;


leaseTime : NUMBER ;

ipAddressWithSubnet: stringParameter | stringParameter SLASH NUMBER;
ip6Prefix : stringParameter SLASH NUMBER;
hardwareParameter: HARDWARE HARDWARE_TYPES ColonSeparatedList ;

optionParam: (stringParameter)*;

optionStatement: optionParam
                | optionParam NUMBER
                | optionParam Boolean
                | optionParam hostnameOrIpAddress
                | optionParam ColonSeparatedList
                | PORT NUMBER
                | ADDRESS Ip4Address;
                    
optionOptionStatement: OPTION optionStatement;



