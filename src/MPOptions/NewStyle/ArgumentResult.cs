using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    internal class ArgumentResult : IArgumentResultInternal
    {
        private Argument _Argument;
        private ResultStateBag _ResultStateBag;

        internal ArgumentResult(Argument argument, ResultStateBag resultStateBag)
        {
            _ResultStateBag = resultStateBag;
            _Argument = argument;
        }

        public string[] Values
        {
            get
            {
                return _ResultStateBag.HasError ? null : __Values.ToArray();
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

        public IArgumentValidator ArgumentValidator
        {
            get
            {
                return _Argument.ArgumentValidator;
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
                return _Argument.Name;
            }
        }
    }
}
