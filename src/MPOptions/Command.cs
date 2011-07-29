using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.ElementTree;
using MPOptions.Parser;

namespace MPOptions
{
    public class Command: Element, IGeneralFlow<Command>
    {
        private readonly StateBag StateBag2 = new StateBag();


        public Command(string name, string token):base(name)
        {
            this.Token = token;
            //StateBag2.BaseCommand = this;
        }

        public Command Add(params Command[] commands) //MP: could be an extension method
        {
            foreach (Command command in commands)
            {
                Commands.Add(command); //MP: test the attaching: validation of options

                //if this command or new command has new global options then revalidate
                bool thisnewvalidate = command.StateBag2.GlobalOptions.Count >0;
                bool theirnewvalidate = StateBag2.GlobalOptions.Count > 0;

                this.StateBag2.MergeIn(command.StateBag2);

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

        public Command Add(Argument argument)
        {
            Arguments.Add(argument);
            return this;
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
            }
        }

        private CommandCollection _Commands;
        public ICommandCollection Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = new CommandCollection(this.StateBag2, Name + " ");
                return _Commands;
            }
        }

        private OptionCollection _Options;
        public  IOptionCollection Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionCollection(this.StateBag2,Name+" ");
                return _Options;
            }
        }

        private ArgumentCollection _Arguments;
        public IArgumentCollection Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new ArgumentCollection(this.StateBag2, Name + " ");
                return _Arguments;
            }
        }


       
    }
}
