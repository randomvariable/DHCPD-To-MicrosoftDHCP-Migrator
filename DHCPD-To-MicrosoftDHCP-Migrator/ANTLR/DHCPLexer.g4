 /* To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
lexer grammar DHCPLexer;
DOT: '.';
SLASH: '/';
COLON: ':';
SEMI: ';';
LPAREN: '(';
MY: 'my';
PARTNER: 'partner';
AT: 'at';
RPAREN: ')';
LBRACE: '{';
RBRACE: '}';
HOST: 'host';
CLASS: 'class';
SHARED_NETWORK: 'shared-network';
SUBNET: 'subnet';
SUBNET6: 'subnet6';
GROUP: 'group';
FIXED_ADDRESS: 'fixed-address';
LEASE: 'lease';
STARTS: 'starts';
ENDS: 'ends';
TIMESTAMP: 'timestamp';
HARDWARE: 'hardware';
CLIENT_HOSTNAME: 'client-hostname';
DYNAMIC_BOOTP: 'dynamic-bootp';
POOL: 'pool';
RANGE: 'range';
TEMPORARY: 'temporary';
LBRACKET: '[';
RBRACKET: ']';
NETMASK: 'netmask';
AllowDenyKeyword: 'bootp' |
                  'booting' |
                  'dynamic-bootp' |
                  'dynamic bootp clients'|
                  'unknown-clients'|
                  'unknown clients' |
                  'dynamic bootp' |
                  'client-updates' |
                  'client updates' |
                  'dynamic';
FAILOVER: 'failover';
FAILOVER_PEER: 'failoverpeer';
PEER: 'peer';
ADDRESS: 'address';
PORT: 'port';
NEVER: 'never';
DEFAULT_LEASE_TIME: 'default-lease-time';
MAX_LEASE_TIME: 'max-lease-time';
DYNAMIC_BOOTP_LEASE_CUTOFF: 'dynamic-bootp-lease-cutoff';
DYNAMIC_BOOTP_LEASE_LENGTH: 'dynamic-bootp-lease-length';
DDNS_UPDATE_STYLE: 'ddns-update-style';
BOOT_UNKNOWN_CLIENTS: 'boot-unknown-clients';
ONE_LEASE_PER_CLIENT: 'one-lease-per-client';
USE_HOST_DECL_NAME: 'use-host-decl-name';
NEXT_SERVER: 'next-server';
SERVER_IDENTIFIER: 'server-identifier';
FILENAME: 'filename';
ALLOW: 'allow';
DENY: 'deny';
USE_LEASE_ADDR_FOR_DEFAULT_ROUTE: 'use-lease-addr-for-default-route';
AUTHORITATIVE: 'authoritative';
NOT_AUTHORITATIVE: 'notauthoritative';
PRIMARY: 'primary';
SECONDARY: 'secondary';
OPTION: 'option';
VENDOR_CLASS: 'vendorclass';
USER_CLASS: 'userclass';
STATE: 'state';
TSTP: 'tstp';
TSFP: 'tsfp';
ATSFP: 'atsfp';
BINDING: 'binding';
NEXT: 'next';
CLTT: 'cltt';
SERVER_DUID: 'server-duid';


HARDWARE_TYPES: 'ethernet' | 'token-ring'| 'token-fddi' | 'infiniband';
Boolean: 'on' | 'off' | 'true' | 'false';
QUOTE: '"';

ColonSeparatedList: (STRING COLON)+ STRING;

Date: [0-9][0-9][0-9][0-9] SLASH [0-9][0-9] SLASH  [0-9][0-9] ' ' [0-9][0-9] COLON [0-9][0-9] COLON [0-9][0-9] |
      [0-9][0-9] COLON [0-9][0-9] COLON [0-9][0-9] |
      NEVER;

NUMBER: [0-9]+;

Ip4Address: ('0'..'9')+ '.' ('0'..'9')+ '.' ('0'..'9')+ '.' ('0'..'9')+;


BACKSLASH: '\\';
UID: 'uid';
UIDSTRING: UID  ~[\r\n]* '\r'? '\n';
SERVERDUIDSTRING: SERVER_DUID ~[\r\n]* '\r'? '\n';
STRING :  ~(';' | '\r' | '\n'| '\t' | '"' | '{' | '}' | ' '| ':' |'/' | '\\' )+;


WS : [ \t\r\n\-]+ -> skip;

COMMENT: '#' ~[\r\n]* '\r'? '\n'-> skip;