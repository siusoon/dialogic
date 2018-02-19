//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Dialogic.g4 by ANTLR 4.6.4

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Dialogic.Antlr {
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.4")]
public partial class DialogicLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		DELIM=10, LB=11, RB=12, SP=13, NEWLINE=14, OPS=15, WORD=16, COMMENT=17, 
		LINE_COMMENT=18, ERROR=19;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"DELIM", "LB", "RB", "SP", "NEWLINE", "OPS", "WORD", "COMMENT", "LINE_COMMENT", 
		"ERROR"
	};


	public DialogicLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'CHAT'", "'SAY'", "'WAIT'", "'DO'", "'ASK'", "'OPT'", "'GO'", "'FIND'", 
		"'SET'", null, "'{'", "'}'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, "DELIM", "LB", 
		"RB", "SP", "NEWLINE", "OPS", "WORD", "COMMENT", "LINE_COMMENT", "ERROR"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete("Use IRecognizer.Vocabulary instead.")]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Dialogic.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2\x15\x94\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x3\x2\x3"+
		"\x2\x3\x2\x3\x2\x3\x2\x3\x3\x3\x3\x3\x3\x3\x3\x3\x4\x3\x4\x3\x4\x3\x4"+
		"\x3\x4\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\a\x3\a\x3"+
		"\b\x3\b\x3\b\x3\t\x3\t\x3\t\x3\t\x3\t\x3\n\x3\n\x3\n\x3\n\x3\v\a\vP\n"+
		"\v\f\v\xE\vS\v\v\x3\v\x3\v\a\vW\n\v\f\v\xE\vZ\v\v\x3\f\x3\f\x3\r\x3\r"+
		"\x3\xE\x3\xE\x3\xF\x5\xF\x63\n\xF\x3\xF\x3\xF\x6\xFg\n\xF\r\xF\xE\xFh"+
		"\x3\x10\x6\x10l\n\x10\r\x10\xE\x10m\x3\x11\x6\x11q\n\x11\r\x11\xE\x11"+
		"r\x3\x12\x3\x12\x3\x12\x3\x12\x3\x12\a\x12z\n\x12\f\x12\xE\x12}\v\x12"+
		"\x3\x12\x3\x12\x3\x12\x3\x12\x3\x12\x3\x13\x3\x13\x3\x13\x3\x13\a\x13"+
		"\x88\n\x13\f\x13\xE\x13\x8B\v\x13\x3\x13\x3\x13\x5\x13\x8F\n\x13\x3\x13"+
		"\x3\x13\x3\x14\x3\x14\x4{\x89\x2\x2\x15\x3\x2\x3\x5\x2\x4\a\x2\x5\t\x2"+
		"\x6\v\x2\a\r\x2\b\xF\x2\t\x11\x2\n\x13\x2\v\x15\x2\f\x17\x2\r\x19\x2\xE"+
		"\x1B\x2\xF\x1D\x2\x10\x1F\x2\x11!\x2\x12#\x2\x13%\x2\x14\'\x2\x15\x3\x2"+
		"\x5\x4\x2\v\v\"\"\x3\x2>@\v\x2#$&\')+.\x30\x32=\x41\x41\x43\\\x63|~~\x9E"+
		"\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2"+
		"\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2"+
		"\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2"+
		"\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3"+
		"\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2\x3)\x3\x2\x2"+
		"\x2\x5.\x3\x2\x2\x2\a\x32\x3\x2\x2\x2\t\x37\x3\x2\x2\x2\v:\x3\x2\x2\x2"+
		"\r>\x3\x2\x2\x2\xF\x42\x3\x2\x2\x2\x11\x45\x3\x2\x2\x2\x13J\x3\x2\x2\x2"+
		"\x15Q\x3\x2\x2\x2\x17[\x3\x2\x2\x2\x19]\x3\x2\x2\x2\x1B_\x3\x2\x2\x2\x1D"+
		"\x66\x3\x2\x2\x2\x1Fk\x3\x2\x2\x2!p\x3\x2\x2\x2#t\x3\x2\x2\x2%\x83\x3"+
		"\x2\x2\x2\'\x92\x3\x2\x2\x2)*\a\x45\x2\x2*+\aJ\x2\x2+,\a\x43\x2\x2,-\a"+
		"V\x2\x2-\x4\x3\x2\x2\x2./\aU\x2\x2/\x30\a\x43\x2\x2\x30\x31\a[\x2\x2\x31"+
		"\x6\x3\x2\x2\x2\x32\x33\aY\x2\x2\x33\x34\a\x43\x2\x2\x34\x35\aK\x2\x2"+
		"\x35\x36\aV\x2\x2\x36\b\x3\x2\x2\x2\x37\x38\a\x46\x2\x2\x38\x39\aQ\x2"+
		"\x2\x39\n\x3\x2\x2\x2:;\a\x43\x2\x2;<\aU\x2\x2<=\aM\x2\x2=\f\x3\x2\x2"+
		"\x2>?\aQ\x2\x2?@\aR\x2\x2@\x41\aV\x2\x2\x41\xE\x3\x2\x2\x2\x42\x43\aI"+
		"\x2\x2\x43\x44\aQ\x2\x2\x44\x10\x3\x2\x2\x2\x45\x46\aH\x2\x2\x46G\aK\x2"+
		"\x2GH\aP\x2\x2HI\a\x46\x2\x2I\x12\x3\x2\x2\x2JK\aU\x2\x2KL\aG\x2\x2LM"+
		"\aV\x2\x2M\x14\x3\x2\x2\x2NP\x5\x1B\xE\x2ON\x3\x2\x2\x2PS\x3\x2\x2\x2"+
		"QO\x3\x2\x2\x2QR\x3\x2\x2\x2RT\x3\x2\x2\x2SQ\x3\x2\x2\x2TX\a%\x2\x2UW"+
		"\x5\x1B\xE\x2VU\x3\x2\x2\x2WZ\x3\x2\x2\x2XV\x3\x2\x2\x2XY\x3\x2\x2\x2"+
		"Y\x16\x3\x2\x2\x2ZX\x3\x2\x2\x2[\\\a}\x2\x2\\\x18\x3\x2\x2\x2]^\a\x7F"+
		"\x2\x2^\x1A\x3\x2\x2\x2_`\t\x2\x2\x2`\x1C\x3\x2\x2\x2\x61\x63\a\xF\x2"+
		"\x2\x62\x61\x3\x2\x2\x2\x62\x63\x3\x2\x2\x2\x63\x64\x3\x2\x2\x2\x64g\a"+
		"\f\x2\x2\x65g\a\xF\x2\x2\x66\x62\x3\x2\x2\x2\x66\x65\x3\x2\x2\x2gh\x3"+
		"\x2\x2\x2h\x66\x3\x2\x2\x2hi\x3\x2\x2\x2i\x1E\x3\x2\x2\x2jl\t\x3\x2\x2"+
		"kj\x3\x2\x2\x2lm\x3\x2\x2\x2mk\x3\x2\x2\x2mn\x3\x2\x2\x2n \x3\x2\x2\x2"+
		"oq\t\x4\x2\x2po\x3\x2\x2\x2qr\x3\x2\x2\x2rp\x3\x2\x2\x2rs\x3\x2\x2\x2"+
		"s\"\x3\x2\x2\x2tu\a\x31\x2\x2uv\a,\x2\x2v{\x3\x2\x2\x2wz\x5#\x12\x2xz"+
		"\v\x2\x2\x2yw\x3\x2\x2\x2yx\x3\x2\x2\x2z}\x3\x2\x2\x2{|\x3\x2\x2\x2{y"+
		"\x3\x2\x2\x2|~\x3\x2\x2\x2}{\x3\x2\x2\x2~\x7F\a,\x2\x2\x7F\x80\a\x31\x2"+
		"\x2\x80\x81\x3\x2\x2\x2\x81\x82\b\x12\x2\x2\x82$\x3\x2\x2\x2\x83\x84\a"+
		"\x31\x2\x2\x84\x85\a\x31\x2\x2\x85\x89\x3\x2\x2\x2\x86\x88\v\x2\x2\x2"+
		"\x87\x86\x3\x2\x2\x2\x88\x8B\x3\x2\x2\x2\x89\x8A\x3\x2\x2\x2\x89\x87\x3"+
		"\x2\x2\x2\x8A\x8E\x3\x2\x2\x2\x8B\x89\x3\x2\x2\x2\x8C\x8F\x5\x1D\xF\x2"+
		"\x8D\x8F\a\x2\x2\x3\x8E\x8C\x3\x2\x2\x2\x8E\x8D\x3\x2\x2\x2\x8F\x90\x3"+
		"\x2\x2\x2\x90\x91\b\x13\x2\x2\x91&\x3\x2\x2\x2\x92\x93\v\x2\x2\x2\x93"+
		"(\x3\x2\x2\x2\xE\x2QX\x62\x66hmry{\x89\x8E\x3\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace Dialogic.Antlr
