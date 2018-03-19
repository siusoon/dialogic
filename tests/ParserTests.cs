﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Dialogic
{
    [TestFixture]
    public class ParserTests
    {
        // NOTE: all tests here are done with the Tendar.Config validator
        static List<Chat> ParseText(string s)
        {
            DialogManager.TypeMap.Add("NVM", typeof(Tendar.Nvm));
            return ChatParser.ParseText(s, Tendar.Config.Validator);
        }

        [Test]
        public void TestPrompts()
        {
            List<Chat> chats = ParseText("ASK Want a game?\nOPT Y #Game\n\nOPT N #End");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Ask)));
			Ask ask = (global::Dialogic.Ask)chats[0].commands[0];
            Assert.That(ask.Text, Is.EqualTo("Want a game?"));
            Assert.That(ask.Options().Count, Is.EqualTo(2));

            var options = ask.Options();
            Assert.That(options[0].GetType(), Is.EqualTo(typeof(Opt)));
            Assert.That(options[0].Text, Is.EqualTo("Y"));
            Assert.That(options[0].action.GetType(), Is.EqualTo(typeof(Go)));
            Assert.That(options[1].GetType(), Is.EqualTo(typeof(Opt)));
            Assert.That(options[1].Text, Is.EqualTo("N"));
            Assert.That(options[1].action.GetType(), Is.EqualTo(typeof(Go)));
        }

        [Test]
        public void TestComments1()
        {
            string[] lines = {
                "CHAT c1",
                "SAY Thank you",
                "SAY Hello",
                "//SAY And Goodbye",
                "SAY Done1",
                "SAY And//Goodbye",
                "SAY Done2",
                "/*SAY And Goodbye*/",
                "SAY And /*Goodbye*/",
                "/*SAY And Goodbye",
                "SAY Done2*/",
                "SAY Done3",
                "/*SAY And Goodbye",
                "SAY */Done4",
                "SAY Done4"
            };

            string[] result = {
                "CHAT c1",
                "SAY Thank you",
                "SAY Hello",
                String.Empty,
                "SAY Done1",
                "SAY And",
                "SAY Done2",
                String.Empty,
                "SAY And",
                String.Empty,
                String.Empty,
                "SAY Done3",
                String.Empty,
                "Done4",
                "SAY Done4"
            };

            var parsed = ChatParser.StripComments(String.Join("\n", lines));

            Assert.That(parsed.Length, Is.EqualTo(lines.Length));
            for (int i = 0; i < parsed.Length; i++)
            {
                Assert.That(parsed[i], Is.EqualTo(result[i]));
            }
        }

        [Test]
        public void TestComments2()
        {
            var txt = "SAY Thank you/*\nSAY Hello //And Goodbye*/";
            var res = ChatParser.StripComments(txt);
            Assert.That(res[0], Is.EqualTo("SAY Thank you"));
            Assert.That(res[1], Is.EqualTo(""));

            List<Chat> chats;
            chats = ParseText(txt);
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            //Assert.That(parsed[14], Is.EqualTo("SAY Done4"));
            //Assert.That(ParseText("//\n//SAY Thank you\n//SAY Hello\n// And Goodbye").Count, Is.EqualTo(0));
            //Assert.That(ParseText("/*SAY Thank you\nSAY Hello\nAnd Goodbye*/").Count, Is.EqualTo(0));
            //Assert.That(ParseText("///").Count, Is.EqualTo(0));
            //Assert.That(ParseText("//").Count, Is.EqualTo(0));
            //Assert.That(ParseText(" //").Count, Is.EqualTo(0));
            //Assert.That(ParseText(" /* */").Count, Is.EqualTo(0));
            //Assert.That(ParseText("/* */").Count, Is.EqualTo(0));

            chats = ParseText("SAY Thank you\n//SAY Hello\n// And Goodbye");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you\nSAY Hello //And Goodbye");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[1].Text, Is.EqualTo("Hello"));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you\nSAY Hello /*And Goodbye*/");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[1].Text, Is.EqualTo("Hello"));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you\n//SAY Hello\nAnd Goodbye\n");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[1].Text, Is.EqualTo("And Goodbye"));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you\n//SAY Goodbye\nAnd Hello");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[1].Text, Is.EqualTo("And Hello"));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Say)));
        }

        [Test]
        public void TestFindSoft()
        {
            List<Chat> chats;

            chats = ParseText("FIND {num=1}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            Assert.That(chats[0].commands[0].GetMeta("num"), Is.Not.Null);
            var meta = chats[0].commands[0].GetMeta("num");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
			Constraint constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(false));

            chats = ParseText("FIND {a*=(hot|cool)}");
            var find = chats[0].commands[0];
            Assert.That(find.GetType(), Is.EqualTo(typeof(Find)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetMeta("a"), Is.Not.Null);
            meta = chats[0].commands[0].GetMeta("a");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(false));

            chats = ParseText("FIND {do=1}");
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            var finder = chats[0].commands[0];
            Assert.That(finder.GetType(), Is.EqualTo(typeof(Find)));
            meta = chats[0].commands[0].GetMeta("do");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(false));
        }

        [Test]
        public void TestFindSoftDynVar()
        {
            List<Chat> chats;

            chats = ParseText("FIND {num=$count}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));

            chats[0].commands[0].Realize(RealizeTests.globals);

            Assert.That(chats[0].commands[0].GetMeta("num"), Is.Not.Null);
            var meta = chats[0].commands[0].GetMeta("num");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            Constraint constraint = (Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(false));
            Assert.That(constraint.name, Is.EqualTo("num"));
            Assert.That(constraint.value, Is.EqualTo("$count"));
        }

        [Test]
        public void TestFindSoftDynGroup()
        {
            // "FIND {a*=(hot|cool)}" in regex, means hot OR cool, no subs
            var chats = ParseText("FIND {a*=(hot|cool)}");
            var find = chats[0].commands[0];
            find.Realize(RealizeTests.globals);

            Assert.That(find.GetType(), Is.EqualTo(typeof(Find)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetMeta("a"), Is.Not.Null);

            var meta = chats[0].commands[0].GetMeta("a");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            var constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(false));
            Assert.That(constraint.name, Is.EqualTo("a"));
            Assert.That(constraint.op, Is.EqualTo(Operator.RE));
            Assert.That(constraint.value, Is.EqualTo("(hot|cool)"));
            var real = chats[0].commands[0].GetRealized();
            Assert.That(real.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestFindHard()
        {
            List<Chat> chats;

            chats = ParseText("FIND {!num=1}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            Assert.That(chats[0].commands[0].GetMeta("num"), Is.Not.Null);
            var meta = chats[0].commands[0].GetMeta("num");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
			Constraint constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(true));

            chats = ParseText("FIND {!a*=(hot|cool)}");
            var find = chats[0].commands[0];
            Assert.That(find.GetType(), Is.EqualTo(typeof(Find)));
            Assert.That(chats[0].commands[0].Text, Is.Null);
            Assert.That(chats[0].commands[0].GetMeta("a"), Is.Not.Null);
            meta = chats[0].commands[0].GetMeta("a");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(true));

            chats = ParseText("FIND {!do=1}");
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            var finder = chats[0].commands[0];
            Assert.That(finder.GetType(), Is.EqualTo(typeof(Find)));
            meta = chats[0].commands[0].GetMeta("do");
            Assert.That(meta.GetType(), Is.EqualTo(typeof(Constraint)));
            constraint = (global::Dialogic.Constraint)meta;
            Assert.That(constraint.IsStrict(), Is.EqualTo(true));
        }

        [Test]
        public void TestWaitCommand()
        {
            List<Chat> chats;

            chats = ParseText("WAIT .5 {waitForAnimation=true}");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Wait)));
			Wait wait = (global::Dialogic.Wait)chats[0].commands[0];
            Assert.That(wait.Text, Is.EqualTo(".5"));
            Assert.That(wait.DelayMs, Is.EqualTo(500));
            Assert.That(wait.GetMeta("waitForAnimation"), Is.EqualTo("true"));
        }

        [Test]
        public void TestValidators()
        {
            List<Chat> chats;
            chats = ChatParser.ParseText("CHAT c1 {plot=a,stage=b}\nSAY Hello");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Hello"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
        }

        [Test]
        public void TestToScript()
        {
            string[] tests = {

                "CHAT c1 {plot=a,stage=b}",
            };

            for (int i = 0; i < tests.Length; i++)
            {
                Assert.That(ParseText(tests[i])[0].ToString(), Is.EqualTo(tests[i]));
            }

            tests = new[] {
                "SAY hay is for horses",
                "ASK hay is for horses?",
                "DO #hay",
                "FIND {!a=b,staleness=5}",
                "WAIT",
                "WAIT .5",
                "WAIT {ForAnimation=true}",
                "WAIT .5 {ForAnimation=true}",
                "NVM",
                "NVM {ForAnimation=true}",
            };

            for (int i = 0; i < tests.Length; i++)
            {
                Assert.That(ParseText(tests[i])[0].commands[0].ToString(), Is.EqualTo(tests[i]));
            }

            var s = "GO #hay";
            Assert.That(ParseText(s)[0].commands[0].ToString(), Is.EqualTo(s));
            Assert.That(ParseText("GO hay")[0].commands[0].ToString(), Is.EqualTo(s));

            s = "DO #hay";
            Assert.That(ParseText(s)[0].commands[0].ToString(), Is.EqualTo(s));
            Assert.That(ParseText("DO hay")[0].commands[0].ToString(), Is.EqualTo(s));

            //s = "SET a 4";
            //Assert.That(ParseText(s)[0].commands[0].ToString(), Is.EqualTo(s));

            s = "ASK hay is for horses?\nOPT Yes? #Yes\nOPT No? #No";
            var exp = s.Replace("\n","\n  ").Split('\n');
            var res = ParseText(s)[0].commands[0].ToString().Split('\n');

            Assert.That(res.Length, Is.EqualTo(exp.Length));
            for (int i = 0; i < exp.Length; i++)
            {
                Assert.That(res[i], Is.EqualTo(exp[i]));
            }
        }


        [Test]
        public void TestCommands()
        {
            List<Chat> chats;

            chats = ParseText("HAY is for horses");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("HAY is for horses"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("hello");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("hello"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("wei");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("wei"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY ...");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("..."));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("...");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("..."));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("GO #Twirl");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Twirl"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Go)));

            chats = ParseText("GO Twirl");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Twirl"));

            chats = ParseText("DO #Twirl");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Twirl"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Do)));

            chats = ParseText("DO Twirl");
            //Console.WriteLine(chats[0].ToTree());
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Twirl"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Do)));

            chats = ParseText("SAY Thank you");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you\n \t\nAnd Goodbye");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[1].Text, Is.EqualTo("And Goodbye"));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Say)));

            chats = ParseText("SAY Thank you { pace = fast}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[0].GetMeta("pace"), Is.EqualTo("fast"));

            chats = ParseText("SAY Thank you {pace=fast,count=2}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[0].HasMeta(), Is.EqualTo(true));
            Assert.That(chats[0].commands[0].GetMeta("pace"), Is.EqualTo("fast"));
            Assert.That(chats[0].commands[0].GetMeta("count"), Is.EqualTo("2"));

            chats = ParseText("SAY Thank you {}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].Text, Is.EqualTo("Thank you"));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Say)));
            Assert.That(chats[0].commands[0].HasMeta(), Is.EqualTo(false));
        }

        [Test]
        public void TestSimpleCommand()
        {
            string[] lines = {
                "DO #Twirl", "DO #Twirl {speed= fast}", "SAY Thank you", "WAIT", "WAIT .5",  "WAIT .5 {a=b}",
                "SAY Thank you {pace=fast,count=2}", "SAY Thank you", "FIND { num > 1, an != 4 }",
                "SAY Thank you { pace = fast}", "SAY Thank you {}", "Thank you"
            };
            var parser = new ChatParser();
            for (int i = 0; i < lines.Length; i++)
            {
                Command c = ParseText(lines[i])[0].commands[0];
                Assert.That(c is Command);
            }
        }


        [Test]
        public void TestGrammars()
        {
            List<Chat> chats = ParseText("GRAM { start: 'The <item>', item: cat }");
            Command gram = chats[0].commands[0];
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(1));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(gram.Text, Is.Null);
            Assert.That(gram.GetType(), Is.EqualTo(typeof(Gram)));
            //new ChatRuntime(chats).Run();
        }

        [Test]
        public void TestChats()
        {
            List<Chat> chats = ParseText("CHAT c1 {plot=a,stage=b}");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(0));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Chat chat = chats[0];
            Assert.That(chats[0].Text, Is.EqualTo("c1"));

            chats = ParseText("CHAT c1 {plot=a,stage=b}\nGO #c1\nDO #c1\n");
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Count, Is.EqualTo(2));
            Assert.That(chats[0].GetType(), Is.EqualTo(typeof(Chat)));
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Go)));
            Assert.That(chats[0].commands[1].GetType(), Is.EqualTo(typeof(Do)));
        }

        [Test]
        public void TestExceptions()
        {
            Assert.Throws<ParseException>(() => ParseText("CHAT Two Words"));
            Assert.Throws<ParseException>(() => ParseText("GO {no=label}"));
            Assert.Throws<ParseException>(() => ParseText("DO {no=label}"));
            Assert.Throws<ParseException>(() => ParseText("FIND {a = (b|c)}"));
            Assert.Throws<ParseException>(() => ParseText("FIND hello"));
            Assert.Throws<ParseException>(() => ParseText("WAIT {a}"));
            Assert.Throws<ParseException>(() => ParseText("OPT {a=b}"));
            Assert.Throws<ParseException>(() => ParseText("SAY")); // ?
            Assert.Throws<ParseException>(() => ParseText("SAY {a=b}"));
            Assert.Throws<ParseException>(() => ParseText("WAIT a {a=b}"));
            Assert.Throws<ParseException>(() => ParseText("NVM a {a=b}"));
            Assert.Throws<ParseException>(() => ParseText("CHAT c1"));

            string[] lines = {
                "CHAT c1 {plot=a,stage=b}",
                "SAY Thank you",
                "SAY Hello",
                "//SAY And Goodbye",
                "SAY Done1",
                "SAY And//Goodbye",
                "SAY Done2",
                "/*SAY And Goodbye*/",
                "SAY And /*Goodbye*/",
                "/*SAY And Goodbye",
                "SAY Done2*/",
                "SAY Done3",
                "/*SAY And Goodbye",
                "SAY */Done4",
                "SAY Done4 {a}"
            };

            Assert.Throws<ParseException>(() => ParseText(String.Join("\n", lines)));
            try
            {
                ParseText(String.Join("\n", lines));
            }
            catch (ParseException e)
            {
                //Console.WriteLine(e);
                Assert.That(e.lineNumber, Is.EqualTo(lines.Length));
            }
        }
    }
}
