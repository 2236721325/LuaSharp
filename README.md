# LuaSharp
## 参考书籍
  自己动手实现Lua （ 虚拟机、编译器和标准库）
  https://book.douban.com/subject/30348061/
## 愿景
	写一个比较完善的lua编译器来了解创造编程语言所需要的一切
	用C#编写。（快速实现想法）


## lua 语言的  ebnf

chunk ::= {stat [`;´]} [laststat[`;´]]
	block ::= chunk
	stat ::=  varlist1 `=´ explist1  | 
		 functioncall  | 
		 do block end  | 
		 while exp do block end  | 
		 repeat block until exp  | 
		 if exp then block {elseif exp then block} [else block] end  | 
		 for Name `=´ exp `,´ exp [`,´ exp] do block end  | 
		 for namelist in explist1 do block end  | 
		 function funcname funcbody  | 
		 local function Name funcbody  | 
		 local namelist [`=´ explist1] 

	laststat ::= return [explist1]  |  break

	funcname ::= Name {`.´ Name} [`:´ Name]

	varlist1 ::= var {`,´ var}

	var ::=  Name  |  prefixexp `[´ exp `]´  |  prefixexp `.´ Name 

	namelist ::= Name {`,´ Name}

	explist1 ::= {exp `,´} exp

	exp ::=  nil  |  false  |  true  |  Number  |  String  |  `...´  | 
		 function  |  prefixexp  |  tableconstructor  |  exp binop exp  |  unop exp 

	prefixexp ::= var  |  functioncall  |  `(´ exp `)´

	functioncall ::=  prefixexp args  |  prefixexp `:´ Name args 

	args ::=  `(´ [explist1] `)´  |  tableconstructor  |  String 

	function ::= function funcbody

	funcbody ::= `(´ [parlist1] `)´ block end

	parlist1 ::= namelist [`,´ `...´]  |  `...´

	tableconstructor ::= `{´ [fieldlist] `}´

	fieldlist ::= field {fieldsep field} [fieldsep]

	field ::= `[´ exp `]´ `=´ exp  |  Name `=´ exp  |  exp

	fieldsep ::= `,´  |  `;´

	binop ::= `+´  |  `-´  |  `*´  |  `/´  |  `^´  |  `%´  |  `..´  | 
		 `<´  |  `<=´  |  `>´  |  `>=´  |  `==´  |  `~=´  | 
		 and  |  or

	unop ::= `-´  |  not  |  `#´