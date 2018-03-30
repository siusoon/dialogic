using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Dialogic
{
    /// <summary>
    /// Superclass for all Commands. When created by the parser, the default constructor is called first,
    /// followed by Init(text,label,meta), followed by any app-specific validators, followed by PostValidate().
    /// </summary>
    public abstract class Command : Meta
    {
        internal static readonly Command NOP = new NoOp();
        protected static int IDGEN = 0;

        protected internal IActor actor { get; protected set; }
        protected internal double delay { get; protected set; }

        public string text;

        protected internal readonly int id;
        protected internal Chat parent;

        protected Command()
        {
            this.delay = 0;
            this.id = ++IDGEN;
            this.realized = new Dictionary<string, object>();
            ExtractMetaMeta();
        }

        protected internal void MetaToProperties(ChatRuntime rt) // ?
        {
            if (HasMeta()) // OPT: reflection
            {
                foreach (KeyValuePair<string, object> pair in meta)
                {
                    string key = pair.Key;
                    //Console.WriteLine("META: '" + key+"'");
                    object val = pair.Value;
                    if (metameta.ContainsKey(key))
                    {
                        if (key == Meta.ACTOR)
                        {
                            this.Actor(rt, (string)val);
                        }
                        else
                        {
                            var propInfo = metameta[key];
                            val = Util.ConvertTo(propInfo.PropertyType, val);
                            //Console.WriteLine("DYN: " + val + " " + val.GetType());
                            propInfo.SetValue(this, val, null);
                        }
                    }
                }
            }
        }

        protected void ExtractMetaMeta()
        {
            metameta = new Dictionary<string, PropertyInfo>();

            //Console.WriteLine("\n" + TypeName() + " ========================");
            var props = GetType().GetProperties(BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo pi in props)
            {
                //Console.WriteLine(pi.Name);
                metameta.Add(pi.Name, pi);
            }
        }

        protected double Delay()
        {
            return delay;
        }

        protected Command Delay(double seconds)
        {
            delay = seconds;
            SetMeta(Meta.DELAY, seconds.ToString());
            return this;
        }

        internal static Command Create(Type type, string text, string label, string[] metas)
        {
            //Console.WriteLine("'"+type + "' '"+text+ "' '"+ label+"' "+Util.Stringify(metas));
            Command cmd = (Command)Activator.CreateInstance(type);
            cmd.Init(text, label, metas);
            return cmd;
        }

        public virtual void Init(string text, string label, string[] metas)
        {
            this.text = text.Length > 0 ? text : label;
            ParseMeta(metas);
            //HandleMetaTiming();
            Validate();
        }

        protected void ValidateTextLabel()
        {
            if (String.IsNullOrEmpty(text)) throw BadArg
                (TypeName().ToUpper() + " requires a literal #Label");

            if (text.StartsWith("#", Util.IC)) text = text.Substring(1);
        }

        public string Text(bool real = false)
        {
            return real ? (string)realized[Meta.TEXT] : text;
        }

        public IActor Actor()
        {
            return actor;
        }

        public Command Actor(IActor actor)
        {
            this.actor = actor;
            return this;
        }

        public Command Actor(ChatRuntime rt, string actorName)
        {
            return Actor(rt.FindActor(actorName));
        }

        protected internal virtual Command Validate()
        {
            return this;
        }

        public virtual string TypeName()
        {
            return this.GetType().Name;
        }

        public virtual void Realize(IDictionary<string, object> globals)
        {
            realized.Clear();

            RealizeMeta(globals);

            if (this is ISendable)
            {
                realized[Meta.TEXT] = Realizer.Do(text, globals);
                realized[Meta.ACTOR] = Actor().Name();
                realized[Meta.TYPE] = TypeName();
            }

            //return realized; // convenience
        }

        protected virtual void RealizeMeta(IDictionary<string, object> globals)
        {
            if (HasMeta())
            {
                IEnumerable sorted = null; // TODO: cache these key-sorts ?

                foreach (KeyValuePair<string, object> pair in meta)
                {
                    object val = pair.Value;

                    if (val is string)
                    {
                        string tmp = (string)val;

                        if (tmp.IndexOf('$') > -1)
                        {
                            if (sorted == null) sorted = Util.SortByLength(globals.Keys);
                            foreach (string s in sorted)
                            {
                                tmp = tmp.Replace("$" + s, globals[s].ToString());
                            }
                        }

                        val = tmp;
                    }
                    else if (!(val is Constraint))
                    {
                        throw new DialogicException("Unexpected meta-value: " + val + " " + val.GetType());
                    }

                    realized[pair.Key] = val;
                }
            }
        }

        protected Exception BadArg(string msg)
        {
            throw new ParseException(msg);
        }

        protected internal virtual int ComputeDuration()
        {
            return Util.ToMillis(delay);
        }

        public override string ToString()
        {
            return (TypeName().ToUpper() + " "
                + text).Trim() + (" " + MetaStr()).TrimEnd();
        }
    }

    public interface ISendable { }

    public class NoOp : Command { }

    public class Say : Command, ISendable
    {
        protected string lastSpoken;

        public Say() : base()
        {
            this.delay = Util.ToMillis(Defaults.SAY_DURATION);
        }

        protected internal override Command Validate()
        {
            if (text.Length < 1) throw new ParseException("SAY requires text");
            return this;
        }

        public override void Realize(IDictionary<string, object> globals)
        {
            base.Realize(globals);
            Recombine(globals);
            lastSpoken = Text(true);
            //lastSpokenTime = Util.EpochMs();
            //return realized;
        }

        private void Recombine(IDictionary<string, object> globals)
        {
            if (IsRecombinant()) // try to say something different than last time
            {
                int tries = 0;
                while (lastSpoken == Text(true) && ++tries < 100)
                {
                    realized[Meta.TEXT] = Realizer.Do(text, globals);
                }
            }
        }

        protected bool IsRecombinant()
        {
            return text.IndexOf('|') > -1;
        }

        /**
         * Determine milliseconds to wait after sending the event, using:
         *      a. Line-length
         *      b. Meta-data modifiers
         *      c. Character mood (TODO)
         */
        protected internal override int ComputeDuration() // Config?
        {
            return Util.Round(GetTextLenScale() * GetMetaSpeedScale() * delay);
        }

        protected double GetTextLenScale()
        {
            return Util.Map(text.Length,
                Defaults.SAY_MIN_LINE_LEN, Defaults.SAY_MAX_LINE_LEN,
                Defaults.SAY_MIN_LEN_MULT, Defaults.SAY_MAX_LEN_MULT);
        }

        protected double GetMetaSpeedScale()
        {
            double val = 1.0;
            if (meta != null && meta.ContainsKey("speed"))
            {
                switch ((string)meta["speed"])
                {
                    case "fast":
                        val *= Defaults.SAY_FAST_MULT;
                        break;
                    case "slow":
                        val *= Defaults.SAY_SLOW_MULT;
                        break;
                }
            }
            return val;
        }
    }

    public class Gram : Command
    {
        public Grammar grammar;

        public override void Init(string text, string label, string[] metas)
        {
            //Console.WriteLine("Gram.init: " + Util.Stringify(meta)+"\n"+String.Join("\n", meta));
            grammar = new Grammar(String.Join("\n", metas));
        }

        public override string ToString()
        {
            return "[" + TypeName().ToUpper() + "] >\n" + grammar;
        }
    }

    public class Do : Command, ISendable
    {
        protected internal override Command Validate()
        {
            ValidateTextLabel();
            //if (HasMeta(Meta.DELAY))
            //{
            //    delay = Util.ToMillis(Defaults.DO_DURATION);
            //}
            return this;
        }

        public override string ToString()
        {
            return TypeName().ToUpper() + " #" + text;
        }
    }

    public class Set : Command // TODO: rethink
    {
        public string Value;

        public Set() : base() { }

        internal Set(string name, string value) : base() // tests only
        {
            this.text = name;
            this.Value = value;
        }

        public override void Init(string text, string label, string[] metas)
        {
            string[] parts = ParseSetArgs(text);
            this.text = parts[0];
            this.Value = parts[1];
        }

        private string[] ParseSetArgs(string s)
        {
            if (s.Length < 1) throw BadArg("ParseSetArgs");

            var pair = Regex.Split(s, @"\s*=\s*"); // TODO: compile
            if (pair.Length != 2) pair = Regex.Split(s, @"\s+");

            if (pair.Length != 2) throw BadArg("SET requires NAME and VALUE");

            if (pair[0].StartsWith("$", StringComparison.Ordinal))
            {
                pair[0] = pair[0].Substring(1); // tmp: leading $ is optional
            }

            return pair;
        }

        public override string ToString()
        {
            return TypeName().ToUpper() + " $" + text + '=' + Value;
        }
    }

    public class Wait : Command, ISendable
    {
        public override void Init(string text, string label, string[] metas)
        {
            base.Init(text, label, metas);
            delay = text.Length == 0 ? DefaultDuration() : Convert.ToDouble(text);
        }

        protected virtual double DefaultDuration()
        {
            return Defaults.WAIT_DURATION;
        }
    }

    public class Ask : Say, ISendable
    {
        internal int selectedIdx;

        protected List<Opt> options = new List<Opt>();

        protected internal double timeout { get; protected set; }

        public Ask()
        {
            this.delay = Defaults.ASK_DURATION;
            this.timeout = Defaults.ASK_TIMEOUT;
        }

        protected double Timeout()
        {
            return timeout;
        }

        protected Ask Timeout(double seconds)
        {
            timeout = seconds;
            SetMeta(Meta.TIMEOUT, seconds.ToString());
            return this;
        }

        public List<Opt> Options()
        {
            if (options.Count < 1)
            {
                options.Add(new Opt("Yes"));
                options.Add(new Opt("No"));
            }
            return options;
        }

        public string OptionsJoined(string delim = "\n")
        {
            var s = String.Empty;
            var opts = Options();
            for (int i = 0; i < opts.Count; i++)
            {
                s += opts[i].Text(true);
                if (i < opts.Count - 1) s += delim;
            }
            return s;
        }

        public Opt Selected()
        {
            return Options()[selectedIdx];
        }

        public Opt Selected(int i)
        {
            this.selectedIdx = i;
            if (i >= 0 && i < options.Count) return Selected();
            return null;
        }

        public void AddOption(Opt o)
        {
            options.Add(o);
        }

        public override void Realize(IDictionary<string, object> globals)
        {
            base.Realize(globals);
            Options().ForEach(o => o.Realize(globals));
            realized[Meta.TIMEOUT] = timeout.ToString();
            realized[Meta.OPTS] = OptionsJoined();
            //return realized;
        }

        //protected override void HandleMetaTiming()
        //{
        //    if (HasMeta(Meta.TIMEOUT))
        //    {
        //        timeout = Util.SecStrToMs((string)meta[Meta.TIMEOUT]);
        //    }
        //}

        public override string ToString()
        {
            string s = base.ToString();
            if (options != null) options.ForEach(o => s += "\n  " + o);
            return s;
        }
    }

    public class Opt : Say
    {
        internal Command action;

        public Opt() : this(String.Empty, NOP) { }

        public Opt(string text) : this(text, NOP) { }

        public Opt(string text, Command action) : base()
        {
            this.text = text;
            this.action = action;
        }

        public override void Init(string text, string label, string[] metas)
        {
            this.text = text;

            if (label.Length > 0 && !label.StartsWith("#", Util.IC))
            {
                throw BadArg("OPT requires a literal #Label");
            }

            this.action = label.Length > 0 ?
                Command.Create(typeof(Go), String.Empty, label, metas) : NOP;

            Validate();
        }

        protected internal override Command Validate()
        {
            this.action.Validate();
            return this;
        }

        public override string ToString()
        {
            return TypeName().ToUpper() + " " + text
                + (action is NoOp ? String.Empty : " #" + action.text);
        }
    }

    public class Find : Command
    {
        //public double stalenessThreshold;

        public Find() : base() { }

        internal Find(params Constraint[] cnts) : base()
        {
            this.meta = Constraint.AsDict(cnts);
        }

        public Find Init(string metadata)
        {
            meta.Clear();
            Init(null, null, metadata.Trim().TrimEnds('{', '}').Split(','));
            return this;
        }

        public override void Init(string text, string label, string[] metas)
        {
            ParseMeta(metas);
            Validate();
        }

        public override void Realize(IDictionary<string, object> globals)
        {
            realized.Clear();

            RealizeMeta(globals);

            //return realized; // for convenience
        }

        protected internal override Command Validate()
        {
            //Console.WriteLine("STALE: "+ GetMeta(Meta.STALENESS)+ GetMeta(Meta.STALENESS).GetType());
            if (!HasMeta(Meta.STALENESS))
            {
                SetMeta(new Constraint(Operator.LT, Meta.STALENESS,
                    Defaults.FIND_STALENESS.ToString()));
            }

            return this;
        }

        public override void SetMeta(string key, object val, bool throwIfKeyExists = false)
        {
            if (val is string || val.IsNumber())
            {
                val = new Constraint(key, val.ToString());
            }
            base.SetMeta(key, val, throwIfKeyExists);
        }

        protected override void ParseMeta(string[] pairs)
        {
            for (int i = 0; pairs != null && i < pairs.Length; i++)
            {
                if (String.IsNullOrEmpty(pairs[i])) throw new ParseException
                    ("Invalid query: {empty}");

                Match match = RE.FindMeta.Match(pairs[i]);
                //Util.ShowMatch(match);

                if (match.Groups.Count != 4) throw new ParseException
                    ("Invalid query: '" + pairs[i] + "'");

                string key = match.Groups[1].Value;
                ConstraintType ctype = ConstraintType.Soft;

                if (Util.TrimFirst(ref key, Constraint.TypeSetChar))
                {
                    ctype = ConstraintType.Hard;
                    if (Util.TrimFirst(ref key, '!'))
                    {
                        ctype = ConstraintType.Absolute;
                    }
                }

                if (meta == null) meta = new Dictionary<string, object>();

                meta.Add(key, new Constraint(Operator.FromString
                    (match.Groups[2].Value), key, match.Groups[3].Value, ctype));
            }
        }

        protected override string MetaStr()
        {
            string s = String.Empty;
            if (HasMeta())
            {
                s += "{";
                foreach (var ct in meta.Values) s += ct + ",";
                s = s.Substring(0, s.Length - 1) + "}";
            }
            return s;
        }

        public override string ToString()
        {
            return (TypeName().ToUpper() + " " + MetaStr()).Trim();
        }
    }

    public class Go : Find
    {
        public override void Init(string text, string label, string[] metas)
        {
            this.text = text.Length > 0 ? text : label;
            Validate();
        }

        public new Go Init(string label)
        {
            Init(String.Empty, label, null);
            return this;
        }

        protected internal override Command Validate()
        {
            ValidateTextLabel();
            return this;
        }

        public override string ToString()
        {
            return TypeName().ToUpper() + " #" + text;
        }
    }

    public class Meta
    {
        public const string TYPE = "__type__";
        public const string TEXT = "__text__";
        public const string OPTS = "__opts__";

        public const string ACTOR = "actor";
        public const string DELAY = "delay";
        public const string TIMEOUT = "timeout";
        public const string STALENESS = "staleness";
        public const string INTERRUPTIBLE = "interruptible";
        public const string STALENESS_INCR = "stalenessIncr";
        public const string RESUME_AFTER_INT = "resumeAfterInt";

        internal IDictionary<string, object> meta, realized;
        protected IDictionary<string, PropertyInfo> metameta;

        public virtual bool HasMeta()
        {
            return meta != null && meta.Count > 0;
        }

        public bool HasMeta(string key)
        {
            return meta != null && meta.ContainsKey(key);
        }

        public object GetMeta(string key, object defaultVal = null)
        {
            return meta != null && meta.ContainsKey(key) ? meta[key] : defaultVal;
        }

        public object GetRealized(string key, object defaultVal = null)
        {
            return realized.ContainsKey(key) ? realized[key] : defaultVal;
        }

        public void SetMeta(Constraint constraint)
        {
            this.SetMeta(constraint.name, constraint, true);
        }

        public virtual void SetMeta(string key, object val, bool throwIfKeyExists = false)
        {
            if (meta == null) meta = new Dictionary<string, object>();

            if (throwIfKeyExists)
            {
                meta.Add(key, val);
            }
            else
            {
                meta[key] = val;
            }
        }

        public IDictionary<string, object> GetMeta()
        {
            return meta;
        }

        public List<KeyValuePair<string, object>> ToList()
        {
            return meta != null ? meta.ToList() : null;
        }

        protected virtual string MetaStr()
        {
            string s = String.Empty;
            if (HasMeta())
            {
                s += "{";
                foreach (var key in meta.Keys) s += key + "=" + meta[key] + ",";
                s = s.Substring(0, s.Length - 1) + "}";
            }
            return s;
        }

        protected virtual void ParseMeta(string[] pairs)
        {
            for (int i = 0; pairs != null && i < pairs.Length; i++)
            {
                //Console.WriteLine(i+") "+pairs[i]);
                if (!string.IsNullOrEmpty(pairs[i]))
                {
                    string[] parts = pairs[i].Split('=');
                    ValidateMetaPairs(parts);
                    SetMeta(parts[0], parts[1]);
                }
            }
        }

        protected void ValidateMetaPairs(string[] prs)
        {
            if (prs.Length != 2) throw new ParseException
                ("Expected 2 parts for meta key/val, but found "
                 + prs.Length + ": " + prs.Stringify());

            for (int i = 0; i < prs.Length; i++) prs[i] = prs[i].Trim();

            if (prs[0].IndexOf(' ') > -1) throw new ParseException
                ("Meta keys cannot contains spaces: '" + prs[0] + "'");

            if (prs[1].IndexOf(' ') > -1) throw new ParseException
                ("Meta values cannot contains spaces: '" + prs[1] + "'");
        }
    }

}