namespace OpenBr.Inss.Business.Documents
{

    /// <summary>
    /// Contém detalhes da faixa de contribuição
    /// </summary>
    public class RetirementRange
    {

        /// <summary>
        /// Range start value (revenue)
        /// </summary>
        public decimal StartValue { get; set; }

        /// <summary>
        /// Range end value (revenue)
        /// </summary>
        public decimal EndValure { get; set; }

        /// <summary>
        /// Contribution rate (percentage to be applied)
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Amount to be deducted (for progressive contributions)
        /// </summary>
        public decimal DeductedAmount { get; set; }

    }
}
