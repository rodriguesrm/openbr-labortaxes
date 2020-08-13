namespace OpenBr.Inss.Business.Documents
{

    /// <summary>
    /// Contém detalhes da faixa de contribuição
    /// </summary>
    public class InssFaixa
    {

        /// <summary>
        /// Valor do início da faixa (rendimentos)
        /// </summary>
        public decimal Inicio { get; set; }

        /// <summary>
        /// Valor do término da faixa (rendimentos)
        /// </summary>
        public decimal Final { get; set; }

        /// <summary>
        /// Alíquota de conribuição (percentual a ser aplicado)
        /// </summary>
        public decimal Aliquota { get; set; }

        /// <summary>
        /// Valor a ser deduzido (para contribuições progressivas)
        /// </summary>
        public decimal Deducao { get; set; }

    }
}
