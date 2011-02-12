using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.NewStyle;

namespace MPOptions.Internal
{
    internal class StateBag
    {
        internal StateBag()
        {
            GlobalOptions = new CollectionAdapter<Option>(Options, "::");
        }


        internal RootCommand RootCommand
        {
            get; 
            set;
        }

        internal readonly IDictionary<string, Command> Commands=new Dictionary<string, Command>();

        internal readonly IDictionary<string, Option> Options = new Dictionary<string, Option>();

        internal readonly IDictionary<string, Argument> Arguments = new Dictionary<string, Argument>();

        //internal  OptionCollection GlobalOptions = new OptionCollection();
        internal Command BaseCommand;



        internal CollectionAdapter<Option> GlobalOptions;


      


    }
}