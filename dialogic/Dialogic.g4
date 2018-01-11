grammar Dialogic;

//////////////////////// PARSER /////////////////////////

tree: line+;
line: (command SPACE* | command SPACE+ args) (NEWLINE | EOF);
command: (CHAT | SAY | WAIT | DO | ASK | OPT | GO | CALL);
args: arg (delim arg)*;
delim: SPACE* POUND SPACE*;
arg: (str | num);
str:  WORD (SPACE WORD)*;
num: DECIMAL;
label: WORD;

//////////////////////// LEXER /////////////////////////

fragment LOWER: [a-z];
fragment UPPER: [A-Z];
fragment DIGIT: [0-9];
fragment PUNCT: [;:",`'!._?-];

SAY: 'SAY';
WAIT: 'WAIT';
CHAT: 'CHAT';
DO: 'DO';
ASK: 'ASK';
OPT: 'OPT'; // TODO
GO: 'GO';
CALL: 'CALL'; // TODO

POUND: '#';
RARROW: '=>';
DCOLON: '::';
SPACE: (' ' | '\t');
DECIMAL: '-'? [0-9]+ ('.' [0-9]+)?;
COMMENT: '/*' .*? '*/' -> skip;
NEWLINE: ('\r'? '\n' | '\r')+;
WORD: (LOWER | UPPER) (LOWER | UPPER | PUNCT | DIGIT)*;

// NOT YET USED
/*funCall: funName '(' funCallArgs ')';
IF: 'if';
THEN: 'then';
AND: 'and';
OR: 'or';
TRUE: 'true';
FALSE: 'false';
MULT: '*';
DIV: '/';
PLUS: '+';
MINUS: '-';
GT: '>';
GE: '>=';
LT: '<';
LE: '<=';
EQ: '=';
LPAREN: '(';
RPAREN: ')';
LB: '[';
RB: ']';
UNDER: '_';*/
