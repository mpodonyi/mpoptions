using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.Internal
{
    internal class StateBag
    {
        internal Command RootCommand
        {
            get; 
            set;
        }

        internal readonly IDictionary<string, Command> Commands=new Dictionary<string, Command>();

        internal readonly IDictionary<string, Option> Options = new Dictionary<string, Option>();

        internal readonly IDictionary<string, Argument> Arguments = new Dictionary<string, Argument>();
    }
}