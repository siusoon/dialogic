//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/dhowe/git/dialogic/dialogic/Dialogic.g4 by ANTLR 4.6.4

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Dialogic {
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.4")]
public partial class DialogicLexer : Lexer {
	public const int
		COMMAND=1, SPACE=2, DELIM=3, NEWLINE=4, WORD=5, ERROR=6;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"COMMAND", "SPACE", "DELIM", "NEWLINE", "WORD", "ERROR"
	};


	public DialogicLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
	};
	private static readonly string[] _SymbolicNames = {
		null, "COMMAND", "SPACE", "DELIM", "NEWLINE", "WORD", "ERROR"
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
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2\bP\b\x1\x4\x2\t"+
		"\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x3\x2\x3\x2\x3"+
		"\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2"+
		"\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3"+
		"\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x5\x2\x30\n\x2\x3\x3\x3\x3\x3"+
		"\x4\a\x4\x35\n\x4\f\x4\xE\x4\x38\v\x4\x3\x4\x3\x4\a\x4<\n\x4\f\x4\xE\x4"+
		"?\v\x4\x3\x5\x5\x5\x42\n\x5\x3\x5\x3\x5\x6\x5\x46\n\x5\r\x5\xE\x5G\x3"+
		"\x6\x6\x6K\n\x6\r\x6\xE\x6L\x3\a\x3\a\x2\x2\x2\b\x3\x2\x3\x5\x2\x4\a\x2"+
		"\x5\t\x2\x6\v\x2\a\r\x2\b\x3\x2\x4\x4\x2\v\v\"\"\f\x2#$&\')+.\x30\x32"+
		"=??\x41\x41\x43\\\x63|~~^\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3"+
		"\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x3/\x3\x2"+
		"\x2\x2\x5\x31\x3\x2\x2\x2\a\x36\x3\x2\x2\x2\t\x45\x3\x2\x2\x2\vJ\x3\x2"+
		"\x2\x2\rN\x3\x2\x2\x2\xF\x10\a\x45\x2\x2\x10\x11\aJ\x2\x2\x11\x12\a\x43"+
		"\x2\x2\x12\x30\aV\x2\x2\x13\x14\aU\x2\x2\x14\x15\a\x43\x2\x2\x15\x30\a"+
		"[\x2\x2\x16\x17\aY\x2\x2\x17\x18\a\x43\x2\x2\x18\x19\aK\x2\x2\x19\x30"+
		"\aV\x2\x2\x1A\x1B\a\x46\x2\x2\x1B\x30\aQ\x2\x2\x1C\x1D\a\x43\x2\x2\x1D"+
		"\x1E\aU\x2\x2\x1E\x30\aM\x2\x2\x1F \aQ\x2\x2 !\aR\x2\x2!\x30\aV\x2\x2"+
		"\"#\aI\x2\x2#\x30\aQ\x2\x2$%\aU\x2\x2%&\aG\x2\x2&\x30\aV\x2\x2\'(\aR\x2"+
		"\x2()\a\x43\x2\x2)*\a\x45\x2\x2*\x30\aG\x2\x2+,\a\x46\x2\x2,-\aK\x2\x2"+
		"-.\aU\x2\x2.\x30\aR\x2\x2/\xF\x3\x2\x2\x2/\x13\x3\x2\x2\x2/\x16\x3\x2"+
		"\x2\x2/\x1A\x3\x2\x2\x2/\x1C\x3\x2\x2\x2/\x1F\x3\x2\x2\x2/\"\x3\x2\x2"+
		"\x2/$\x3\x2\x2\x2/\'\x3\x2\x2\x2/+\x3\x2\x2\x2\x30\x4\x3\x2\x2\x2\x31"+
		"\x32\t\x2\x2\x2\x32\x6\x3\x2\x2\x2\x33\x35\x5\x5\x3\x2\x34\x33\x3\x2\x2"+
		"\x2\x35\x38\x3\x2\x2\x2\x36\x34\x3\x2\x2\x2\x36\x37\x3\x2\x2\x2\x37\x39"+
		"\x3\x2\x2\x2\x38\x36\x3\x2\x2\x2\x39=\a%\x2\x2:<\x5\x5\x3\x2;:\x3\x2\x2"+
		"\x2<?\x3\x2\x2\x2=;\x3\x2\x2\x2=>\x3\x2\x2\x2>\b\x3\x2\x2\x2?=\x3\x2\x2"+
		"\x2@\x42\a\xF\x2\x2\x41@\x3\x2\x2\x2\x41\x42\x3\x2\x2\x2\x42\x43\x3\x2"+
		"\x2\x2\x43\x46\a\f\x2\x2\x44\x46\a\xF\x2\x2\x45\x41\x3\x2\x2\x2\x45\x44"+
		"\x3\x2\x2\x2\x46G\x3\x2\x2\x2G\x45\x3\x2\x2\x2GH\x3\x2\x2\x2H\n\x3\x2"+
		"\x2\x2IK\t\x3\x2\x2JI\x3\x2\x2\x2KL\x3\x2\x2\x2LJ\x3\x2\x2\x2LM\x3\x2"+
		"\x2\x2M\f\x3\x2\x2\x2NO\v\x2\x2\x2O\xE\x3\x2\x2\x2\n\x2/\x36=\x41\x45"+
		"GL\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace Dialogic
