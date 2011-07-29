using System.Collections.Generic;

namespace MPOptions.ElementTree
{
    internal class StateBag
    {
        //internal RootCommand RootCommand
        //{
        //    get; 
        //    set;
        //}

        private IDictionary<string, Command> _Commands;
        internal IDictionary<string, Command> Commands 
        {
            get
            {
                if(_Commands==null)
                    _Commands = new Dictionary<string, Command>();
                return _Commands;
            }
        }

        private IDictionary<string, Option> _Options;
        internal IDictionary<string, Option> Options
        {
            get
            {
                if (_Options == null)
                    _Options = new Dictionary<string, Option>();
                return _Options;
            }
        }

        private IDictionary<string, Argument> _Arguments;
        internal IDictionary<string, Argument> Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new Dictionary<string, Argument>();
                return _Arguments;
            }
        }

        private CollectionAdapter<Option> _GlobalOptions;
        internal CollectionAdapter<Option> GlobalOptions
        {
            get
            {
                if (_GlobalOptions == null)
                    _GlobalOptions = new CollectionAdapter<Option>(Options, "::");
                return _GlobalOptions;
            }
        }

        internal void MergeIn(StateBag stateBag)
        {
            //if this command or new command has new global options then revalidate

            bool thisnewvalidate = stateBag.GlobalOptions.Count > 0;
            bool theirnewvalidate = this.GlobalOptions.Count > 0;

            foreach (var item in stateBag.Options)
            {
                this.Options.Add(item); //MP: should here not be the PreKey in consideration
            }

            foreach (var item in stateBag.Commands)
            {
                this.Commands.Add(item); //MP: should here not be the PreKey in consideration
            }

            foreach (var item in stateBag.Arguments)
            {
                this.Arguments.Add(item); //MP: should here not be the PreKey in consideration
            }

            stateBag._Options = this.Options;
            stateBag._Commands = this.Commands;
            stateBag._Arguments = this.Arguments;
            stateBag._GlobalOptions = new CollectionAdapter<Option>(stateBag.Options, "::");
            //command.StateBag2.BaseCommand= this;

            //if (thisnewvalidate)
            //    ReValidate();

            //if (theirnewvalidate)
            //    command.ReValidate();
        }

       
      


    }
}