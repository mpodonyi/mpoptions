using System;
using System.ComponentModel;

namespace MPOptions.Internal
{
    interface IValidator
    {
        void Validate();
        void PostValidate();
    }

    internal abstract class Validator<T> : IValidator
    {

        protected Validator(T obj)
        {
            this.obj = obj;
        }

        protected T obj
        {
            get;
            private set;
        }

        #region IValidator Members

        public virtual void Validate()
        {
        }

        public virtual void PostValidate()
        {
        }

        #endregion
    }
}