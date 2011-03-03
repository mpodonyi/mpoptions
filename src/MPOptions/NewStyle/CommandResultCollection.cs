using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{

    public interface ICommandResultCollection
    {
        ICommandResult this[string key]
        {
            get;
        }

    }

    internal interface ICommandResultCollectionInternal
    {
        ICommandResultInternal this[string key]
        {
            get;
        }

    }

   

    internal class CommandResultCollection : ICommandResultCollection, ICommandResultCollectionInternal
    {
        private CommandCollection _CommandCollection;
      

        internal CommandResultCollection(CommandCollection commandCollection)
        {
            _CommandCollection = commandCollection;
        }


        public ICommandResult this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        ICommandResultInternal ICommandResultCollectionInternal.this[string key]
        {
            get
            {
                return null;
            }
        }

    }

   
}
