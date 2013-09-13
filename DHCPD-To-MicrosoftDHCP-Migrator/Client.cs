using Irony.Ast;
using Irony.Parsing;
using Irony.Parsing.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHCPDParser
{
    class Client : Common
    {

        public Client()
        {
            NonTerminal clientConfFile = new NonTerminal("client-conf-file");
            NonTerminal clientDeclarations = new NonTerminal("client-declarations");
            clientConfFile.Rule = clientDeclarations + Eof;
            //clientDeclarations.Rule = nil | clientDeclaration | clientDeclarations + clientDeclaration;
            NonTerminal leaseFile = new NonTerminal("client-lease-statements");
            //leaseFile.Rule = clientLeaseStatements + Eof;
            /* client-conf-file :== client-declarations END_OF_FILE
               client-declarations :== <nil>
                         | client-declaration
                         | client-declarations client-declaration */

            /* lease-file :== client-lease-statements END_OF_FILE
               client-lease-statements :== <nil>
                         | client-lease-statements LEASE client-lease-statement */

            /* client-declaration :== 
                SEND option-decl |
                DEFAULT option-decl |
                SUPERSEDE option-decl |
                PREPEND option-decl |
                APPEND option-decl |
                hardware-declaration |
                ALSO REQUEST option-list |
                ALSO REQUIRE option-list |
                REQUEST option-list |
                REQUIRE option-list |
                TIMEOUT number |
                RETRY number |
                REBOOT number |
                SELECT_TIMEOUT number |
                SCRIPT string |
                VENDOR_SPACE string |
                interface-declaration |
                LEASE client-lease-statement |
                ALIAS client-lease-statement |
                KEY key-definition */

            /* option-list :== option_name |
                       option_list COMMA option_name */
            /* option-list :== option_name |
                       option_list COMMA option_name */

            /* interface-declaration :==
                INTERFACE string LBRACE client-declarations RBRACE */
            /* interface-declaration :==
                INTERFACE string LBRACE client-declarations RBRACE */

            /* client-lease-statement :==
                LBRACE client-lease-declarations RBRACE

                client-lease-declarations :==
                    <nil> |
                    client-lease-declaration |
                    client-lease-declarations client-lease-declaration */

            /* client-lease-statement :==
                LBRACE client-lease-declarations RBRACE

                client-lease-declarations :==
                    <nil> |
                    client-lease-declaration |
                    client-lease-declarations client-lease-declaration */

            /* client-lease-statement :==
                LBRACE client-lease-declarations RBRACE

                client-lease-declarations :==
                    <nil> |
                    client-lease-declaration |
                    client-lease-declarations client-lease-declaration */


            /* client-declaration :== 
                SEND option-decl |
                DEFAULT option-decl |
                SUPERSEDE option-decl |
                PREPEND option-decl |
                APPEND option-decl |
                hardware-declaration |
                ALSO REQUEST option-list |
                ALSO REQUIRE option-list |
                REQUEST option-list |
                REQUIRE option-list |
                TIMEOUT number |
                RETRY number |
                REBOOT number |
                SELECT_TIMEOUT number |
                SCRIPT string |
                VENDOR_SPACE string |
                interface-declaration |
                LEASE client-lease-statement |
                ALIAS client-lease-statement |
                KEY key-definition */

            /* client-lease-declaration :==
                BOOTP |
                INTERFACE string |
                FIXED_ADDR ip_address |
                FILENAME string |
                SERVER_NAME string |
                OPTION option-decl |
                RENEW time-decl |
                REBIND time-decl |
                EXPIRE time-decl |
                KEY id */

            /* allow-deny-keyword :== BOOTP
                        | BOOTING
                        | DYNAMIC_BOOTP
                        | UNKNOWN_CLIENTS */
        }



    }
}
