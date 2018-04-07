﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dialogic
{
    /// <summary>
    /// Each section of text in a Dialogic script is known as a Chat. Each Chat has a unique label and contains one or more commands. When a Chat is run, each command is executed in order, until all have been run, or the system jumps to a new Chat. The Chat command accepts a required label, followed, optionally, by metadata, which can be used with the Find command to search for Chats matching desired criteria.
    /// </summary>
    public class Chat : Command
    {
        internal List<Command> commands;

        protected internal double staleness { get; protected set; }
        protected internal bool interruptable { get; protected set; }
        protected internal bool resumeAfterInt { get; protected set; }
        protected internal double stalenessIncr { get; protected set; }

        internal int cursor = 0, lastRunAt = -1;
        internal bool allowSmoothingOnResume = true;

        public Chat() : base()
        {
            commands = new List<Command>();

            realized = null; // not relevant for chats
            interruptable = true;
            resumeAfterInt = true;
            stalenessIncr = Defaults.CHAT_STALENESS_INCR;
            staleness = Defaults.CHAT_STALENESS;
        }

        internal static Chat Create(string name)
        {
            Chat c = new Chat();
            c.Init(name, String.Empty, new string[0]);
            return c;
        }

        internal double Staleness()
        {
            return staleness;
        }

        internal double StalenessIncr()
        {
            return stalenessIncr;
        }

        internal bool Interruptable()
        {
            return interruptable;
        }

        internal bool ResumeAfterInterrupting()
        {
            return resumeAfterInt;
        }

        internal Chat Staleness(double d)
        {
            staleness = d;
            SetMeta(Meta.STALENESS, d.ToString());
            return this;
        }

        internal Chat IncrementStaleness()
        {
            Staleness(staleness + stalenessIncr);
            return this;
        }

        internal Chat StalenessIncr(double incr)
        {
            this.stalenessIncr = incr;
            SetMeta(Meta.STALENESS_INCR, incr.ToString());
            return this;
        }

        internal Chat Interruptable(bool val)
        {
            this.interruptable = val;
            SetMeta(Meta.INTERRUPTIBLE, val.ToString());

            return this;
        }

        internal Chat ResumeAfterInterrupting(bool val)
        {
            this.resumeAfterInt = val;
            SetMeta(Meta.RESUME_AFTER_INT, val.ToString());
            return this;
        }

        public int Count()
        {
            return commands.Count;
        }

        public void AddCommand(Command c)
        {
            c.parent = this;
            //c.IndexInChat = commands.Count; // ?
            this.commands.Add(c);
        }

        protected internal override void Realize(IDictionary<string, object> globals)
        {
            Console.WriteLine("[WARN] Chats need not be realized, doing commands instead");
            //commands.ForEach(c => { Console.WriteLine(c.TypeName() + ".Realize("+c.text+")"); c.Realize(globals); });
            commands.ForEach(c => c.Realize(globals));
        }

        ///  All Chats must have a valid unique label, and a staleness value
        protected internal override Command Validate()
        {
            if (text.IndexOf(' ') > -1) throw BadArg
                ("CHAT name '" + text + "' contains spaces");

            SetMeta(Meta.STALENESS, Defaults.CHAT_STALENESS.ToString(), true);

            return this;
        }

        protected internal override void Init(string txt, string lbl, string[] metas)
        {
            this.text = txt;
            ParseMeta(metas);
        }

        public string ToTree()
        {
            string s = TypeName().ToUpper() + " "
                + text + (" " + MetaStr()).TrimEnd();
            commands.ForEach(c => s += "\n  " + c);
            return s;
        }

        internal Chat LastRunAt(int ms)
        {
            this.lastRunAt = ms;
            return this;
        }

        internal Command Next()
        {
            return HasNext() ? commands[cursor++] : null;
        }

        internal bool HasNext()
        {
            return cursor > -1 && cursor < commands.Count;
        }

        internal void Run(bool resetCursor = true)
        {
            if (resetCursor)
            {
                this.cursor = 0;
                IncrementStaleness();
            }
            else
            {
                // reset half the added staleness on resume
                staleness -= (StalenessIncr() / 2.0);
            }

            LastRunAt(Util.EpochMs());
        }
    }
}
