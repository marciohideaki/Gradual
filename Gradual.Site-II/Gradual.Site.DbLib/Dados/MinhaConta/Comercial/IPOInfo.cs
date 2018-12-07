using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Comercial
{
    /// <summary>
    /// Classe para IPO do cliente 
    /// </summary>
    public class IPOInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do IPO - Para controle do banco
        /// </summary>
        public Nullable<int> CodigoIPO { get; set; }

        /// <summary>
        /// Instrmento a ser negociado no IPO
        /// </summary>
        public string Ativo { get; set; }

        /// <summary>
        /// Código ISIN do IPO
        /// </summary>
        public string CodigoISIN { get; set; }

        /// <summary>
        /// Descrição do nome da empresa
        /// </summary>
        public string DsEmpresa { get; set; }

        /// <summary>
        /// Modalidade do IPO - 
        /// Primária, 
        /// Secundária, ON, 
        /// PN, 
        /// Primária ON, 
        /// Primária PN, 
        /// Secundaria ON, 
        /// Secundária PN
        /// </summary>
        public string Modalidade { get; set; }

        /// <summary>
        /// Data de inicio da Oferta Publica (IPO)
        /// Data que irá começar a aparecer no site
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Data dinal da Oferta Pública (IPO)
        /// Data que irá parar de aparecer no site
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Hora máxima que aceitará oferta no último dia
        /// </summary>
        public string HoraMaxima { get; set; }

        /// <summary>
        /// Valor máximo monetário a ser digitado no cadastro do IPO
        /// </summary>
        public decimal VlMaximo { get; set; }

        /// <summary>
        /// Valor mínimo monetário a ser digitado no cadastro do IPO
        /// </summary>
        public decimal VlMinimo { get; set; }

        /// <summary>
        /// Valor percentual de garantia a ser digitado
        /// </summary>
        public decimal VlPercentualGarantia { get; set; }

        /// <summary>
        /// Observações do IPO
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// Flag para checar se aparece no site ou não,(Ativo ou Inativo)
        /// </summary>
        public bool StAtivo { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
