using OpenBr.Inss.Business.Enums;
using System;
using System.Collections.Generic;

namespace OpenBr.Inss.Business.Documents
{

    /// <summary>
    /// Contém as informações da tabela de inss
    /// </summary>
    public class Inss : DocumentBase
    {

        /// <summary>
        /// Tipo de tabela de contribuição
        /// </summary>
        public TipoTabelaInss Tipo { get; set; }

        /// <summary>
        /// Data de início da vigência da tabela
        /// </summary>
        public DateTime VigenciaInicio { get; set; }

        /// <summary>
        /// Data do término da vigência da tabela
        /// </summary>
        public DateTime? VigenciaTermino { get; set; }

        /// <summary>
        /// Valor limite (teto) de contribuição da tabela
        /// </summary>
        public decimal Limite { get; set; }

        /// <summary>
        /// Indica se a tabela está ativa ou inativa
        /// </summary>
        public bool Inativo { get; set; }

        /// <summary>
        /// Faixa dos valores para contribuição
        /// </summary>
        public IEnumerable<InssFaixa> Faixa { get; set; }

    }
}
