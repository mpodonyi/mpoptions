using System;
using System.ComponentModel;

namespace MPOptions.Validator
{
    interface IValidator
    {
        void Validate();
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

        #endregion
    }
}