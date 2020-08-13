using System;
using System.ComponentModel;

namespace OpenBr.Inss.Business.Enums
{

    /// <summary>
    /// Enumeração de tipo de tabela de contribuição
    /// </summary>

    [Flags]
    public enum TipoTabelaInss
    {

        [Description("Tabela para Empregado, Empregado Doméstico e Trabalhador Avulso")]
        Trabalhador = 1,

        [Description("Tabela para Contribuinte Individual e Facultativo")]
        Individual = 2,

        [Description("Tabela para Sócio Administrador de empresa (pró-labore)")]
        SocioAdministrador = 3
    }
}
