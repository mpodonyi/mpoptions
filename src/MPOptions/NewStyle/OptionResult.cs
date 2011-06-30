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

        string Value
        {
            get;
        }

        string Token
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
        private ResultStateBag _ResultStateBag;

        internal OptionResult(Option option, ResultStateBag resultStateBag)
        {
            _ResultStateBag = resultStateBag;
            _Option = option;
        }


        public string[] Values
        {
            get
            {
                return _ResultStateBag.HasError ? null :  __Values.ToArray();
            }
        }

        public string Value
        {
            get
            {
                return _ResultStateBag.HasError ? null : __Values.FirstOrDefault();
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

        private bool _IsSet;
        public bool IsSet
        {
            get
            {
                return _ResultStateBag.HasError ? false : _IsSet;
            }
            set
            {
                _IsSet = value;
            }
        }



        public string Name
        {
            get 
            {
                return _Option.Name;
            }
        }

        public string Token
        {
            get
            {
                return _Option.Token;
            }
        }



    }
}
