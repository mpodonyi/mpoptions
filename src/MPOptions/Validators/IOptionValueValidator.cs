namespace MPOptions.Validators
{
    /// <summary>
    /// Provides functionality to validate OptionValues.
    /// </summary>
    internal interface IOptionValueValidator
    {
        /// <summary>
        /// Determines whether the specified value is match.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is match; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(string value);

        /// <summary>
        /// Gets the maximum value how many optionvalues a option can have.
        /// </summary>
        /// <value>The maximum occurrence.</value>
        int MaximumOccurrence
        { get; }

        /// <summary>
        /// Gets a value indicating whether the value of an option is optional.
        /// </summary>
        /// <value><c>true</c> if option can also be called without value; otherwise, <c>false</c>.</value>
        bool ValueOptional
        { get; }
    }
}