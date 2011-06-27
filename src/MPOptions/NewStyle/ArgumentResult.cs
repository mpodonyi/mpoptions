using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    public interface IArgumentResult
    {
        bool IsSet
        { get; }

        string Name
        {
            get;
        }

        string[] Values
        {
            get;
        }

        string Value
        {
            get;
        }
    }

    internal interface IArgumentResultInternal : IArgumentResult
    {
        new bool IsSet
        {
            get;
            set;
        }

        ICollection<string> _Values
        { get; }

        IArgumentValidator ArgumentValidator
        { get; }
    }



    internal class ArgumentResult : IArgumentResultInternal
    {
        private Argument _Argument;

        internal ArgumentResult(Argument argument, ResultStateBag resultStateBag)
        {
            _Argument = argument;
        }

        public string[] Values
        {
            get
            {
                return __Values.ToArray();
            }
        }

        public string Value
        {
            get
            {
                return __Values.FirstOrDefault();
            }
        }

        internal ICollection<string> __Values = new List<string>();
        public ICollection<string> _Values
        {
            get
            {
                return __Values;
            }
        }

        public IArgumentValidator ArgumentValidator
        {
            get
            {
                return _Argument.ArgumentValidator;
            }
        }

        public bool IsSet
        {
            get;
            set;
        }


        public string Name
        {
            get 
            {
                return _Argument.Name;
            }
        }
    }
}
