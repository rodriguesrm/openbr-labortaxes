using System;
using System.ComponentModel;

namespace OpenBr.LaborTaxes.Contract.Enums
{

    /// <summary>
    /// Contribution table type enumeration
    /// </summary>
    [Flags]
    public enum InssType
    {

        /// <summary>
        /// Worker taxpayer (Empregado, Empregado Doméstico e Trabalhador Avulso)
        /// </summary>

        [Description("Tabela para Empregado, Empregado Doméstico e Trabalhador Avulso")]
        Worker = 1,

        /// <summary>
        /// Individual taxpayer (Contribuinte Individual e Facultativo)
        /// </summary>
        [Description("Tabela para Contribuinte Individual e Facultativo")]
        Individual = 2,

        /// <summary>
        /// Managing partner (Sócio Administrador de empresa (pró-labore)
        /// </summary>
        [Description("Tabela para Sócio Administrador de empresa (pró-labore)")]
        ManagingPartner = 3
    }
}
