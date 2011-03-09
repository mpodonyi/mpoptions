using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    public interface IOptionResult
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
    }

    internal interface IOptionResultInternal : IOptionResult
    {
        new bool IsSet
        {
            get;
            set;
        }

        ICollection<string> _Values
        { get; }

        IOptionValueValidator OptionValueValidator
        { get; }
    }



    internal class OptionResult : IOptionResultInternal
    {
        private Option _Option;

        internal OptionResult(Option option)
        {
            _Option = option;
        }

        public string[] Values
        {
            get
            {
                return __Values.ToArray();
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

        public IOptionValueValidator OptionValueValidator
        {
            get
            {
                return _Option.OptionValueValidator;
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
                return _Option.Name;
            }
        }
    }
}
