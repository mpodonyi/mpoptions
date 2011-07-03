namespace MPOptions.InternalValidation
{
    interface IValidator
    {
        void Validate();
        void PostValidate();
    }
}