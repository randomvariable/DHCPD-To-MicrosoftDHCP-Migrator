/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

grammar DHCPDCommon;
import DHCPLexer;

stringParameter : STRING ;

syntax: STRING ;

hostName : IDENTIFIER 
         | IDENTIFIER DOT 
         | (IDENTIFIER DOT IDENTIFIER)+;

//IpAddress:  [0-9]+ DOT ([0-9.]*)+;


ipAddrOrHostname: STRING | hostName;

leaseTime : NUMBER ;

netmask: STRING | NUMBER;


ipAddressWithSubnet: IpAddress | IpAddress SLASH NUMBER;

ip6Prefix : IpAddress6 SLASH NUMBER;

colonSeparatedHexList : NUMBER | NUMBER COLON colonSeparatedHexList;


hardwareType: HARDWARE_TYPES;



hardwareParameter: HARDWARE hardwareType  colonSeparatedHexList ;




optionStatement: IDENTIFIER DOT IDENTIFIER syntax  | IDENTIFIER syntax ;
optionOptionStatement: OPTION IDENTIFIER STRING;




            /*
             * executable-statements :== executable-statement executable-statements |
             *			     executable-statement
             *
             * executable-statement :==
             *	IF if-statement |
             * 	ADD class-name SEMI |
             *	BREAK SEMI |
             *	OPTION option-parameter SEMI |
             *	SUPERSEDE option-parameter SEMI |
             *	PREPEND option-parameter SEMI |
             *	APPEND option-parameter SEMI
             */

            /* zone-statements :== zone-statement |
                           zone-statement zone-statements
               zone-statement :==
                PRIMARY ip-addresses SEMI |
                SECONDARY ip-addresses SEMI |
                PRIMARY6 ip-address6 SEMI |
                SECONDARY6 ip-address6 SEMI |
                key-reference SEMI
               ip-addresses :== ip-addr-or-hostname |
                      ip-addr-or-hostname COMMA ip-addresses
               key-reference :== KEY STRING |
                        KEY IDENTIFIER */

            /* key-statements :== key-statement |
                          key-statement key-statements
               key-statement :==
                ALGORITHM host-name SEMI |
                secret-definition SEMI
               secret-definition :== SECRET base64val |
                         SECRET STRING */

            /*
             * on-statement :== event-types LBRACE executable-statements RBRACE
             * event-types :== event-type OR event-types |
             *		   event-type
             * event-type :== EXPIRY | COMMIT | RELEASE
             */

    /*
             * switch-statement :== LPAREN expr RPAREN LBRACE executable-statements RBRACE
             *
             */


            /*
             * case-statement :== CASE expr COLON
             *
             */

            /*
             * if-statement :== boolean-expression LBRACE executable-statements RBRACE
             *						else-statement
             *
             * else-statement :== <null> |
             *		      ELSE LBRACE executable-statements RBRACE |
             *		      ELSE IF if-statement |
             *		      ELSIF if-statement
             */

            /*
             * boolean_expression :== CHECK STRING |
             *  			  NOT boolean-expression |
             *			  data-expression EQUAL data-expression |
             *			  data-expression BANG EQUAL data-expression |
             *			  data-expression REGEX_MATCH data-expression |
             *			  boolean-expression AND boolean-expression |
             *			  boolean-expression OR boolean-expression
             *			  EXISTS OPTION-NAME
             */

            /*
             * data_expression :== SUBSTRING LPAREN data-expression COMMA
             *					numeric-expression COMMA
             *					numeric-expression RPAREN |
             *		       CONCAT LPAREN data-expression COMMA 
             *					data-expression RPAREN
             *		       SUFFIX LPAREN data_expression COMMA
             *		       		     numeric-expression RPAREN |
             *		       LCASE LPAREN data_expression RPAREN |
             *		       UCASE LPAREN data_expression RPAREN |
             *		       OPTION option_name |
             *		       HARDWARE |
             *		       PACKET LPAREN numeric-expression COMMA
             *				     numeric-expression RPAREN |
             *		       STRING |
             *		       colon_separated_hex_list
             */

            /*
             * numeric-expression :== EXTRACT_INT LPAREN data-expression
             *					     COMMA number RPAREN |
             *			  NUMBER
             */

            /*
             * dns-expression :==
             *	UPDATE LPAREN ns-class COMMA ns-type COMMA data-expression COMMA
             *				data-expression COMMA numeric-expression RPAREN
             *	DELETE LPAREN ns-class COMMA ns-type COMMA data-expression COMMA
             *				data-expression RPAREN
             *	EXISTS LPAREN ns-class COMMA ns-type COMMA data-expression COMMA
             *				data-expression RPAREN
             *	NOT EXISTS LPAREN ns-class COMMA ns-type COMMA data-expression COMMA
             *				data-expression RPAREN
             * ns-class :== IN | CHAOS | HS | NUMBER
             * ns-type :== A | PTR | MX | TXT | NUMBER
             */

            /*
             * numeric-expression :== EXTRACT_INT LPAREN data-expression
             *					     COMMA number RPAREN |
             *			  NUMBER
             */

            /* option-statement :== IDENTIFIER DOT IDENTIFIER <syntax> SEMI
                          | IDENTIFIER <syntax> SEMI

               Option syntax is handled specially through format strings, so it
               would be painful to come up with BNF for it.   However, it always
               starts as above and ends in a SEMI. */

            /* This is faked up to look good right now.   Ideally, this should do a
               recursive parse and allow arbitrary data structure definitions, but for
               now it just allows you to specify a single type, an array of single types,
               a sequence of types, or an array of sequences of types.

               ocd :== NUMBER EQUALS ocsd SEMI

               ocsd :== ocsd_type |
                    ocsd_type_sequence |
                    ARRAY OF ocsd_simple_type_sequence

               ocsd_type_sequence :== LBRACE ocsd_types RBRACE

               ocsd_simple_type_sequence :== LBRACE ocsd_simple_types RBRACE

               ocsd_types :== ocsd_type |
                      ocsd_types ocsd_type

               ocsd_type :== ocsd_simple_type |
                     ARRAY OF ocsd_simple_type

               ocsd_simple_types :== ocsd_simple_type |
                         ocsd_simple_types ocsd_simple_type

               ocsd_simple_type :== BOOLEAN |
                        INTEGER NUMBER |
                        SIGNED INTEGER NUMBER |
                        UNSIGNED INTEGER NUMBER |
                        IP-ADDRESS |
                        TEXT |
                        STRING |
                        ENCAPSULATE IDENTIFIER */