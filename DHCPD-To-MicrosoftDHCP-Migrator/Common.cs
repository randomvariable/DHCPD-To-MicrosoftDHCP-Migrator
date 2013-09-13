using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using Irony.Parsing.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DHCPDParser
{
    class Common : Grammar
    {
        protected KeyTerm SEMI;
        protected ConstantTerminal nil;
        protected ConstantTerminal BOOLEAN;
        protected KeyTerm LBRACE;
        protected KeyTerm RBRACE;
        protected NonTerminal stringParameter;
        protected KeyTerm STRING;
        protected NonTerminal hostname;
        protected NonTerminal ipAddrOrHostname;
        protected NonTerminal ipAddress;
        protected KeyTerm DOT;
        protected KeyTerm SLASH;
        protected KeyTerm NUMBER;
        protected NonTerminal leaseTime;
        protected StringLiteral ip6Address;
        protected NonTerminal ipAddressWithSubnet;
		protected NonTerminal hardwareParameter;
		protected StringLiteral hardwareType;
        protected NonTerminal ip6Prefix;
        protected NonTerminal date;
        protected KeyTerm COLON;
        protected NonTerminal optionName;
        protected NonTerminal ocd;
        protected NonTerminal ocsd;
        protected StringLiteral ocsdTypeSequence;
        protected StringLiteral oscdTypes;
        protected StringLiteral ocsdType;
        protected StringLiteral ocsdSimpleType;
        protected NonTerminal base64;
        protected NonTerminal colonSeparatedHexList;
        protected NonTerminal executableStatements;
        protected NonTerminal executableStatement;
        protected NonTerminal zoneStatements;
        protected NonTerminal zoneStatement;
        protected KeyTerm PRIMARY;
        protected KeyTerm SECONDARY;
        protected KeyTerm PRIMARY6;
        protected KeyTerm SECONDARY6;
        protected NonTerminal keyReference;
        protected NonTerminal keyStatements;
        protected NonTerminal keyStatement;
        protected KeyTerm ALGORITHM;
        protected NonTerminal secretDefinition;
        protected StringLiteral base64Val;
        protected NonTerminal onStatement;
        protected NonTerminal eventTypes;
        protected NonTerminal eventType;
        protected NonTerminal switchStatement;
        protected StringLiteral expr;
        protected KeyTerm RPAREN;
        protected NonTerminal caseStatement;
        protected KeyTerm CASE;
        protected KeyTerm EXTRACT_INT;
        protected NonTerminal ifStatement;
        protected NonTerminal booleanExpression;
        protected NonTerminal elseStatement;
        protected KeyTerm ELSE;
        protected KeyTerm ELSEIF;
        protected KeyTerm CHECK;
        protected NonTerminal dataExpression;
        protected KeyTerm EQUAL;
        protected KeyTerm EQUALS;
        protected KeyTerm BANG;
        protected KeyTerm REGEX_MATCH;
        protected KeyTerm AND;
        protected KeyTerm OR;
        protected KeyTerm EXISTS;
        protected KeyTerm OPTION_NAME;
        protected KeyTerm SUBSTRING;
        protected KeyTerm LPAREN;
        protected KeyTerm COMMA;
        protected NonTerminal numericExpression;
        protected KeyTerm CONCAT;
        protected KeyTerm SUFFIX;
        protected KeyTerm LCASE;
        protected KeyTerm UCASE;
        protected KeyTerm OPTION;
        protected KeyTerm HARDWARE;
        protected KeyTerm PACKET;
        protected NonTerminal dnsExpression;
        protected KeyTerm UPDATE;
        protected KeyTerm DELETE;
        protected KeyTerm NOT;
        protected KeyTerm IN;
        protected KeyTerm CHAOS;
        protected KeyTerm NS;
        protected StringLiteral nsClass;
        protected StringLiteral nsType;
        protected KeyTerm A;
        protected KeyTerm PTR;
        protected KeyTerm MX;
        protected KeyTerm TXT;
        protected NonTerminal optionStatement;
        protected KeyTerm EPOCH;
        protected KeyTerm NEVER;
        protected KeyTerm ADD;
        protected KeyTerm BREAK;
        protected KeyTerm CLIENTHOSTNAME;
        protected KeyTerm DYNAMIC_BOOTP;
        protected KeyTerm IF;
        protected KeyTerm PREPEND;
        protected KeyTerm SUPERSEDE;
        protected StringLiteral optionParameter;
        protected NonTerminal ipAddresses;
        protected KeyTerm ARRAY;
        protected KeyTerm OF;
        protected KeyTerm KEY;
        protected KeyTerm SECRET;
        protected KeyTerm EXPIRY;
        protected KeyTerm COMMIT;
        protected KeyTerm RELEASE;
        protected StringLiteral ocsdSimpleTypes;
        protected NonTerminal ocsdSimpleTypeSequence;
        protected KeyTerm IDENTIFIER;
		protected IdentifierTerminal identifier;
        protected NumberLiteral number;
        protected StringLiteral syntax;

		public Common()
		{
            SEMI = ToTerm(";");
            nil = new ConstantTerminal(("nil"),typeof(LiteralValueNode));
			nil.Add("nil",null);
            BOOLEAN = new ConstantTerminal("BOOLEAN");
            BOOLEAN.Add("true", true);
            BOOLEAN.Add("false", false);
            BOOLEAN.Add("on", true);
            BOOLEAN.Add("off", false);
            LBRACE = ToTerm("{");
            RBRACE = ToTerm("}");
            COLON = ToTerm(":");
            ip6Address = new StringLiteral("ip6Address", "\"");
			identifier = new IdentifierTerminal("identifier");
            DOT = ToTerm(".");
            number = new NumberLiteral("number");
            MarkPunctuation(LBRACE, RBRACE);
            SLASH = ToTerm("/");
            syntax = new StringLiteral("syntax", "\"");
            base64Val = new StringLiteral("base64Val", "\"");
            A = ToTerm("A");
            ADD = ToTerm("add");
            ALGORITHM = ToTerm("algorithm");
            AND = ToTerm("AND");
            ARRAY = ToTerm("array");
            BANG = ToTerm("!");
            BREAK = ToTerm("BREAK");
            CASE = ToTerm("CASE");
            CHAOS = ToTerm("CHAOS");
            CHECK = ToTerm("check");
            CONCAT = ToTerm("+");
            DELETE = ToTerm("delete");
            DYNAMIC_BOOTP = ToTerm("dynamic-bootp");
            ELSE = ToTerm("ELSE");
            ELSEIF = ToTerm("ELSEIF");
            EPOCH = ToTerm("epoch");
            EQUAL = ToTerm("=");
            EQUALS = ToTerm("==");
            EXPIRY = ToTerm("expiry");
            expr = new StringLiteral("expr");
            EXTRACT_INT = ToTerm("Eh");
            CLIENTHOSTNAME = ToTerm("client-hostname");
            COMMA = ToTerm(",");
            COMMIT = ToTerm("commit");
            EXISTS = ToTerm("exists");
            HARDWARE = ToTerm("hardware");
            IDENTIFIER = ToTerm("identifier");
            IF = ToTerm("if");
            IN = ToTerm("in");
            KEY = ToTerm("key");
            LCASE = ToTerm("lcase");
            LPAREN = ToTerm("(");
            MX = ToTerm("MX");
            NEVER = ToTerm("never");
            NOT = ToTerm("not");
            NS = ToTerm("ns");
            NUMBER = ToTerm("int");
            OF = ToTerm("of");
            OPTION = ToTerm("option");
            OPTION_NAME = ToTerm("option-name");
            OR = ToTerm("OR");
            PACKET = ToTerm("packet");
            PREPEND = ToTerm("prepend");
            PRIMARY = ToTerm("primary");
            PRIMARY6 = ToTerm("primary-6");
            PTR = ToTerm("ptr");
            REGEX_MATCH = ToTerm("regex-match");
            RELEASE = ToTerm("release");
            RPAREN = ToTerm("(");
            SECONDARY = ToTerm("secondary");
            SECONDARY6 = ToTerm("secondary6");
            SECRET = ToTerm("secret");
            SUBSTRING = ToTerm("substring");
            SUFFIX = ToTerm("suffix");
            SUPERSEDE = ToTerm("supersede");
            TXT = ToTerm("txt");
            UCASE = ToTerm("ucase");
            UPDATE = ToTerm("update");
            STRING = ToTerm("\"");
            hardwareType = new StringLiteral("hardware-type", "\"");
            nsClass = new StringLiteral("ns-class", "\"");
            nsType = new StringLiteral("ns-type", "\"");
            ocsdSimpleType = new StringLiteral("oscd-simple-type", "\"");
            ocsdSimpleTypes = new StringLiteral("ocsd-simple-type", "\"");
            ocsdType = new StringLiteral("ocsd-type", "\"");
            ocsdTypeSequence = new StringLiteral("ocsd-type-sequence","\"");
            optionParameter = new StringLiteral("optionParameter","\"");
            oscdTypes = new StringLiteral("oscd-types");

            /* string-parameter :== STRING SEMI */

            /*
             * hostname :== IDENTIFIER
             *		| IDENTIFIER DOT
             *		| hostname DOT IDENTIFIER
             */

            stringParameter = new NonTerminal("stringParameter");
            stringParameter.Rule = STRING + SEMI;
            hostname = new NonTerminal("hostname");
			hostname.Rule = IDENTIFIER | IDENTIFIER + DOT | hostname + DOT + IDENTIFIER;

            /* ip-addr-or-hostname :== ip-address | hostname
               ip-address :== NUMBER DOT NUMBER DOT NUMBER DOT NUMBER
   
               Parse an ip address or a hostname.   If uniform is zero, put in
               an expr_substring node to limit hostnames that evaluate to more
               than one IP address.

               Note that RFC1123 permits hostnames to consist of all digits,
               making it difficult to quickly disambiguate them from ip addresses.
            */

            /*
             * ip-address :== NUMBER DOT NUMBER DOT NUMBER DOT NUMBER
             */

            ipAddress = new NonTerminal("ipAddress");
            ipAddress.Rule = NUMBER + DOT + NUMBER + DOT + NUMBER + DOT + NUMBER;

            ipAddrOrHostname = new NonTerminal("ipAddrOrHostname");
            ipAddrOrHostname.Rule = ipAddress | hostname;


            /* lease-time :== NUMBER SEMI */

            /*
             * ip-address6 :== (complicated set of rules)
             *
             * See section 2.2 of RFC 1884 for details.
             *
             * We are lazy for this. We pull numbers, names, colons, and dots 
             * together and then throw the resulting string at the inet_pton()
             * function.
             */

            leaseTime = new NonTerminal("leaseTime");
            leaseTime.Rule = NUMBER + SEMI;

            /*
             * ip6-prefix :== ip6-address "/" NUMBER
             */

            ip6Prefix = new NonTerminal("ip6Prefix");
            ip6Prefix.Rule = ip6Address + SLASH + NUMBER;

            /*
             * ip-address-with-subnet :== ip-address |
             *                          ip-address "/" NUMBER
             */

            ipAddressWithSubnet = new NonTerminal("ipAddressWithSubnet");
            ipAddressWithSubnet.Rule = ipAddress | ipAddress + SLASH + NUMBER;

            /*
             * colon-separated-hex-list :== NUMBER |
             *				NUMBER COLON colon-separated-hex-list
             */

            colonSeparatedHexList = new NonTerminal("colonSeparatedHexList");
            colonSeparatedHexList.Rule = NUMBER | NUMBER + COLON + colonSeparatedHexList;


            /*
             * hardware-parameter :== HARDWARE hardware-type colon-separated-hex-list SEMI
             * hardware-type :== ETHERNET | TOKEN_RING | TOKEN_FDDI | INFINIBAND
             * Note that INFINIBAND may not be useful for some items, such as classification
             * as the hardware address won't always be available.
             */

            hardwareParameter = new NonTerminal("hardwareParameter");
            hardwareParameter.Rule = HARDWARE + hardwareType + colonSeparatedHexList + SEMI;



            /*
             * date :== NUMBER NUMBER SLASH NUMBER SLASH NUMBER 
             *		NUMBER COLON NUMBER COLON NUMBER |
             *          NUMBER NUMBER SLASH NUMBER SLASH NUMBER 
             *		NUMBER COLON NUMBER COLON NUMBER NUMBER |
             *          EPOCH NUMBER |
             *	    NEVER
             *
             * Dates are stored in UTC or with a timezone offset; first number is day
             * of week; next is year/month/day; next is hours:minutes:seconds on a
             * 24-hour clock, followed by the timezone offset in seconds, which is
             * optional.
             */

            date = new NonTerminal("date");
            date.Rule = NUMBER + NUMBER + SLASH + NUMBER + SLASH + NUMBER + NUMBER + COLON + NUMBER + COLON + NUMBER |
                        NUMBER + NUMBER + SLASH + NUMBER + SLASH + NUMBER |
                        NUMBER + COLON + NUMBER + COLON + NUMBER + NUMBER |
                        EPOCH + NUMBER |
                        NEVER;

            /*
             * Wrapper to consume the semicolon after the date
             * :== date semi
             */


            /*
             * option-name :== IDENTIFIER |
                       IDENTIFIER . IDENTIFIER
             */

            optionName = new NonTerminal("optionName");
			optionName.Rule = IDENTIFIER + DOT + IDENTIFIER;

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
                        ENCAPSULATE identifier */

            ocd = new NonTerminal("ocd");
            ocd.Rule = NUMBER + EQUALS + ocsd + SEMI;

            ocsd = new NonTerminal("ocsd");
            ocsd.Rule = ocsdTypeSequence | ARRAY + OF + ocsdSimpleTypeSequence;

            ocsdSimpleTypeSequence = new NonTerminal("ocsdSimpleTypeSequence");
            ocsdSimpleTypeSequence.Rule = LBRACE + ocsdSimpleTypes + RBRACE;


            /*
             * base64 :== NUMBER_OR_STRING
             */

            base64 = new NonTerminal("base64");
            base64.Rule = NUMBER | STRING;


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

            executableStatements = new NonTerminal("executableStatements");
			executableStatements.Rule = executableStatement + executableStatement | executableStatement;

			executableStatement = new NonTerminal("executableStatement");
            executableStatement.Rule = IF + ifStatement | ADD + ifStatement | BREAK + SEMI | 
										OPTION + optionParameter + SEMI |
										SUPERSEDE + optionParameter + SEMI |
										PREPEND + optionParameter + SEMI;

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
                        KEY identifier */

            zoneStatements = new NonTerminal("zoneStatements");
            zoneStatements.Rule = zoneStatement | zoneStatement + zoneStatements;

            zoneStatement = new NonTerminal("zoneStatement");
            zoneStatement.Rule = PRIMARY + ipAddresses + SEMI |
                                SECONDARY + ipAddresses + SEMI |
                                PRIMARY6 + ip6Address + SEMI |
                                SECONDARY6 + ip6Address + SEMI |
                                keyReference + SEMI;

            ipAddresses = new NonTerminal("ipAddresses");
            ipAddresses.Rule = ipAddrOrHostname | ipAddrOrHostname + COMMA + ipAddresses;
            keyReference = new NonTerminal("keyReference");
            keyReference.Rule = KEY + STRING | KEY + identifier;

            /* key-statements :== key-statement |
                          key-statement key-statements
               key-statement :==
                ALGORITHM host-name SEMI |
                secret-definition SEMI
               secret-definition :== SECRET base64val |
                         SECRET STRING */

            keyStatements = new NonTerminal("keyStatements");
            keyStatements.Rule = keyStatement | keyStatement + keyStatements;

            keyStatement = new NonTerminal("keyStatement");
            keyStatement.Rule = ALGORITHM + hostname + SEMI |
                                secretDefinition + SEMI;

            secretDefinition = new NonTerminal("secretDefinition");
            secretDefinition.Rule = SECRET + base64Val |
                                SECRET + STRING;

            /*
             * on-statement :== event-types LBRACE executable-statements RBRACE
             * event-types :== event-type OR event-types |
             *		   event-type
             * event-type :== EXPIRY | COMMIT | RELEASE
             */

            onStatement = new NonTerminal("onStatement");
            onStatement.Rule = eventTypes + LBRACE + executableStatements + RBRACE;
            eventTypes = new NonTerminal("eventTypes");
            eventTypes.Rule = eventType | eventTypes;
            eventType = new NonTerminal("eventType");
            eventType.Rule = EXPIRY | COMMIT | RELEASE;

            /*
             * switch-statement :== LPAREN expr RPAREN LBRACE executable-statements RBRACE
             *
             */

            switchStatement = new NonTerminal("switchStatement");
            switchStatement.Rule = LPAREN + expr + RPAREN + LBRACE + executableStatements + RBRACE;


            /*
             * case-statement :== CASE expr COLON
             *
             */

            caseStatement = new NonTerminal("caseStatement");
            caseStatement.Rule = CASE + expr + COLON;

            /*
             * if-statement :== boolean-expression LBRACE executable-statements RBRACE
             *						else-statement
             *
             * else-statement :== <null> |
             *		      ELSE LBRACE executable-statements RBRACE |
             *		      ELSE IF if-statement |
             *		      ELSIF if-statement
             */

            elseStatement = new NonTerminal("elseStatement");
            elseStatement.Rule = ELSE + LBRACE + executableStatements + RBRACE |
                                ELSE + ifStatement |
                                ELSEIF + ifStatement;

            ifStatement = new NonTerminal("ifStatement");
            ifStatement.Rule = booleanExpression + LBRACE + executableStatement + RBRACE + elseStatement;

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

            booleanExpression = new NonTerminal("booleanExpression");
            booleanExpression.Rule = CHECK + STRING |
                                    NOT + booleanExpression |
                                    dataExpression + EQUAL + dataExpression |
                                    dataExpression + BANG + EQUAL + dataExpression |
                                    dataExpression + REGEX_MATCH + dataExpression |
                                    booleanExpression + AND + booleanExpression |
                                    booleanExpression + OR + booleanExpression + EXISTS + OPTION_NAME;



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

            dataExpression = new NonTerminal("dataExpression");
            dataExpression.Rule = SUBSTRING + LPAREN + dataExpression + COMMA + numericExpression + COMMA + numericExpression + RPAREN |
                                CONCAT + LPAREN + dataExpression + COMMA + dataExpression + RPAREN |
                                SUFFIX + LPAREN + dataExpression + COMMA + numericExpression + RPAREN |
                                LCASE + LPAREN + dataExpression + RPAREN |
                                UCASE + LPAREN + dataExpression + RPAREN |
                                OPTION + optionName |
                                HARDWARE |
                                PACKET + LPAREN + numericExpression + COMMA + numericExpression + RPAREN |
                                STRING |
                                colonSeparatedHexList;

            /*
             * numeric-expression :== EXTRACT_INT LPAREN data-expression
             *					     COMMA number RPAREN |
             *			  NUMBER
             */

            numericExpression = new NonTerminal("numericExpression");
            numericExpression.Rule = EXTRACT_INT + LPAREN + dataExpression + COMMA + number + RPAREN | NUMBER;

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

            dnsExpression = new NonTerminal("dnsExpression");
            dnsExpression.Rule = EXTRACT_INT + LPAREN + dataExpression + COMMA + number + RPAREN | NUMBER;

            /*
             * numeric-expression :== EXTRACT_INT LPAREN data-expression
             *					     COMMA number RPAREN |
             *			  NUMBER
             */



            /* option-statement :== identifier DOT identifier <syntax> SEMI
                          | identifier <syntax> SEMI

               Option syntax is handled specially through format strings, so it
               would be painful to come up with BNF for it.   However, it always
               starts as above and ends in a SEMI. */
            optionStatement = new NonTerminal("optionStatement");
            optionStatement.Rule = identifier + DOT + identifier + syntax + SEMI | identifier + syntax + SEMI;


		}

    }
}
