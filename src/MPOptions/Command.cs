using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class Command: Element, IGeneralFlow<Command>
    {
        //public static Command GetRoot()
        //{
        //    return new Command();
        //}

        private StateBag StateBag2 = new StateBag();


        public Command(string name, string token):base(name)
        {
            this.Token = token;
            StateBag2.BaseCommand = this;
        }

        public Command Add(params Command[] commands) //MP: could be an extension method
        {
            foreach (Command command in commands)
            {
                Commands.Add(command); //MP: test the attaching: validation of options

                bool thisnewvalidate = command.StateBag2.GlobalOptions.Count >0;
                bool theirnewvalidate = StateBag2.GlobalOptions.Count > 0;

                if(thisnewvalidate)
                {
                    foreach (Option option in command.StateBag2.GlobalOptions)
                    {
                        StateBag2.GlobalOptions.Add(option);
                    }
                }

                command.StateBag2.GlobalOptions = this.StateBag2.GlobalOptions;
                command.StateBag2.BaseCommand= this;

                if (thisnewvalidate)
                    ReValidate();

                if (theirnewvalidate)
                    command.ReValidate();
            }

            return this;
        }

        private void ReValidate()
        {
            //throw new NotImplementedException();
        }

        
        //MP: could be an extension method
        //MP: maybe derive a class GlobalOption from Option
        public Command Add(params Option[] options)
        {
            foreach (Option option in options)
            {
                Options.Add(option);
            }

            return this;
        }



        internal override string Path
        {
            get
            {
                return IsRoot ? string.Empty : ParentCommand.Path + Name + " ";
            }
        }

        internal Command():base(new StateBag(), null,null)
        {
            //StateBag.RootCommand = this;
        }

        internal Command(Command parentCommand, string name, string token)
            : base(parentCommand.StateBag,parentCommand, name)
        {
            this.Token = token;
        }

        public string Token
        {
            get;
            private set;
        }

        public virtual bool IsRoot
        {
            get
            {
                return false;
                //return this==StateBag.RootCommand;
            }
        }

        private CommandCollection _Commands=new CommandCollection();
        public CommandCollection Commands
        {
            get
            {
                return _Commands;
                //return new CommandCollection(this);
            }
        }

        private OptionCollection _Options;
        public virtual IOptionCollection Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionCollection(this.StateBag2,Name+" ");
                return _Options;
            }
        }

        public ArgumentCollection Arguments
        {
            get
            {
                return new ArgumentCollection(this);
            }
        }


        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <exception cref="ParserException">Thrown when the Commandline can not be parsed successful.</exception>
        /// <returns>Return the current command.</returns>
        public Command Parse()
        {
            var parser = new Parser(this);
            ParserErrorContext errorContext = parser.Parse();
            if (errorContext != null)
                ThrowHelper.ThrowParserException(errorContext);
            return this;
        }

        ///// <summary>
        ///// Parses this instance.
        ///// </summary>
        ///// <param name="parserErrorContext">The Error Reported by the Parser. Otherwise Null.</param>
        ///// <returns>Return the current command.</returns>
        //public Command Parse(out ParserErrorContext parserErrorContext)
        //{
        //    var parser = new Parser(this);
        //    parserErrorContext = parser.Parse();
        //    return this;
        //}


        internal Command Parse(string commandLine, out ParserErrorContext parserErrorContext)
        {
            var parser = new Parser(this, commandLine);
            parserErrorContext = parser.Parse();
            return this;
        }
    }
}
