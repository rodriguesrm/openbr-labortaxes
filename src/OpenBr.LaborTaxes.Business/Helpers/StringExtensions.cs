namespace OpenBr.LaborTaxes.Business.Helpers
{

    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Convert string expression to camelCase format
        /// </summary>
        /// <param name="expression">Expression text to be convert</param>
        public static string ToCamelCase(this string expression)
            => char.ToLowerInvariant(expression[0]) + expression.Substring(1);

    }

}
