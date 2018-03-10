﻿using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Dialogic
{
    [TestFixture]
    public class SearchTests
    {
        [Test]
        public void TestFindNyNameAPI()
        {
            string[] lines = {
                "CHAT c1 {day=fri}",
                "CHAT c2 {dev=2,day=fri}",
                "CHAT c3"
            };
            string contents = String.Join("\n", lines);
            List<Chat> chats = ChatParser.ParseText(contents);
            List<Chat> result = new ChatRuntime(chats).FindAll
                (new Constraints("name", "c1", true));
            //chats.ForEach(c=>Console.WriteLine(c));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Text, Is.EqualTo("c1"));
        }

        [Test]
        public void TestFindByName()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            chats.Add(c = Chat.Create("c2"));
            chats.Add(c = Chat.Create("c3"));
            ChatRuntime cr = new ChatRuntime(chats);
            Chat res = new ChatRuntime(chats).Find(new Constraints("name", "c2"));
            Assert.That(res.Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFind()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));
            ChatRuntime cr = new ChatRuntime(chats);
            Chat res = new ChatRuntime(chats).Find(new Constraints("dev", "1"));
            Assert.That(res.Text, Is.EqualTo("c2"));
        }

        public void TestFindHard() // ???
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c4"));
            ChatRuntime cr = new ChatRuntime(chats);
            Chat res = new ChatRuntime(chats).Find(new Constraints("dev", "1").Add("day", "hello"));
            Assert.That(res.Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFindAllOpsAPI()
        {
            string[] lines = {
                "CHAT c1 {day=fri}",
                "CHAT c2 {dev=2,day=fri}",
                "CHAT c3 {}"
            };
            string contents = String.Join("\n", lines);
            List<Chat> chats = ChatParser.ParseText(contents);
            List<Chat> result = new ChatRuntime(chats).FindAll
                (new Constraints("dev", "1").Add("day", "fri"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Text, Is.EqualTo("c1"));
        }

        [Test]
        public void TestFindAll()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));
            ChatRuntime cr = new ChatRuntime(chats);
            chats = cr.FindAll(new Constraints("dev", "1"));
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));
            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(3));
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFindAllHard()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));
            ChatRuntime cr = new ChatRuntime(chats);
            chats = cr.FindAll(new Constraints("dev", "1", true));
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));
            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFindAll2()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            c.SetMeta("dev", "2");
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));

            ChatRuntime cr = new ChatRuntime(chats);
            chats = cr.FindAll(new Constraints("dev", "1"));
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));
            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(2));
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
            Assert.That(chats[1].Text, Is.EqualTo("c3"));
        }


        [Test]
        public void TestFindAllHard2()
        {
            Chat c;
            List<Chat> chats = new List<Chat>();
            chats.Add(c = Chat.Create("c1"));
            c.SetMeta("dev", "2");
            chats.Add(c = Chat.Create("c2"));
            c.SetMeta("dev", "1");
            c.SetMeta("day", "hello");
            chats.Add(c = Chat.Create("c3"));

            ChatRuntime cr = new ChatRuntime(chats);
            chats = cr.FindAll(new Constraints("dev", "1", true));
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));
            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFindAll3()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {dev=1,day=fri}",
                "CHAT c1 {day=fri}",
                "CHAT c2 {dev=2,day=fri}",
                "CHAT c3 {}"
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(3));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c1"));
        }


        [Test]
        public void TestFindAllHard3()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {!dev=1,day=fri}",
                "CHAT c1 {day=fri}",
                "CHAT c2 {dev=2,day=fri}",
                "CHAT c3 {}"
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(0));


            lines = new string[]{
                "CHAT c0",
                "FIND {dev=1,!day=fri}",
                "CHAT c1 {day=fri}",
                "CHAT c2 {dev=2,day=fri}",
                "CHAT c3 {}"
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c1"));
        }

        [Test]
        public void TestFindAllOp1()
        {
            string[] lines = { "CHAT c0 {dev=1}", "FIND {dev>2,day=fri}" };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(0));
        }


        [Test]
        public void TestFindAllOp2()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {dev>1,day=fri}",
                "CHAT c1 {dev=2,day=fri}",
                "CHAT c2 {dev=1,day=fri}",
                "CHAT c3 {}"
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(3));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c1"));
        }

        [Test]
        public void TestFindAllOpHard1()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {!dev>1,day=fri}",
                "CHAT c1 {dev=2,day=fri}",
                "CHAT c2 {dev=1,day=fri}",
                "CHAT c3 {}"
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c1"));
        }

        [Test]
        public void TestFindAllOpHard2()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {dev<2,!day=fri}",
                "CHAT c1 {dev=2,day=fri}",
                "CHAT c2 {dev=1,day=fri}",
                "CHAT c3 {}"
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
        }

        [Test]
        public void TestFindAllMulti()
        {
            string[] lines = {
                "CHAT c0 {day=sunday}",
                "FIND {emotion*=(hot|cool),day=fri}",
                "CHAT c1 {emotion=wet}",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=cool,day=fri}",
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(2));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2").Or.EqualTo("c4"));

            lines = new string[] {
                "CHAT c0 {day=sunday}",
                "FIND {emotion *= (hot|cool|blah), day*=(fri|wed)}",
                "CHAT c1 {emotion=wet}",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=cool,day=wed}",
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(2));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2").Or.EqualTo("c4"));
        }

        [Test]
        public void TestFindAllMultiHard1()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {!emotion*=(hot|cool),day=fri}",
                "CHAT c1 {emotion=wet}",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=cool,day=fri}",
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats.Count, Is.EqualTo(2));
            Assert.That(chats[0].Text, Is.EqualTo("c2").Or.EqualTo("c4"));


            lines = new string[] {
                "CHAT c0",
                "FIND {!emotion *= (hot|cool|blah), day*=(fri|wed)}",
                "CHAT c1 {emotion=wet}",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=cool,day=wed}",
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(2));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2").Or.EqualTo("c4"));
        }


        [Test]
        public void TestFindAllMultiHard2()
        {
            string[] lines = {
                "CHAT c0",
                "FIND {emotion*=(hot|cool), !day =fri}",
                "CHAT c1",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=dry,day=fri}",
            };

            string contents = String.Join("\n", lines);

            List<Chat> chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            Command finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            ChatRuntime cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2"));

            lines = new string[] {
                "CHAT c0 {day=sunday}",
                "FIND {emotion *= (hot|cool|blah), !day*=(fri|wed)}",
                "CHAT c1 {emotion=wet, day = wed }",
                "CHAT c2 {emotion=hot,day=fri}",
                "CHAT c3 {emotion=dry}",
                "CHAT c4 {emotion=cool,day=tues}",
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2"));
        }


        [Test]
        public void TestFindAllOp3()
        {
            string[] lines = new string[]{
                "CHAT c1 {emotion=cold}",
                "FIND {emotion*=ho}",
                "CHAT c2 {emotion=hot}"
            };

            var contents = String.Join("\n", lines);

            var chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            var finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            var cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2"));

            lines = new string[]{
                "CHAT c1 {emotion=cold}",
                "FIND {emotion^=h}",
                "CHAT c2 {emotion=hot}",
                "CHAT c3 {emotion=that}"
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c2"));

            lines = new string[]{
                "CHAT c1 {emotion=hot}",
                "FIND {emotion$=ld}",
                "CHAT c2 {emotion=hot}",
                "CHAT c3 {emotion=cold}"
            };

            contents = String.Join("\n", lines);

            chats = ChatParser.ParseText(contents);
            //chats.ForEach((ch) => Console.WriteLine(ch.ToTree()));
            finder = chats[0].commands[0];
            Assert.That(chats[0].commands[0].GetType(), Is.EqualTo(typeof(Find)));
            cr = new ChatRuntime(chats);

            chats = cr.FindAll((Find)finder);
            //chats.ForEach((obj) => Console.WriteLine(obj.Text));

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count, Is.EqualTo(1));

            Assert.That(chats[0], Is.Not.Null);
            Assert.That(chats[0].Text, Is.EqualTo("c3"));
        }
    }
}
