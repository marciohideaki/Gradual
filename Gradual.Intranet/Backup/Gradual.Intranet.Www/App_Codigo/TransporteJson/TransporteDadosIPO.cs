using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Vendas;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    /// <summary>
    /// Classe de transporte para gerenciamento de IPO de clientes
    /// </summary>
    public class TransporteDadosIPO
    {
        /// <summary>
        /// Código do IPO 
        /// </summary>
        public string CodigoIPO { get; set; }

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
        public string DataInicial { get; set; }

        /// <summary>
        /// Data dinal da Oferta Pública (IPO)
        /// Data que irá parar de aparecer no site
        /// </summary>
        public string DataFinal { get; set; }

        /// <summary>
        /// Hora máxima que aceitará oferta no último dia
        /// </summary>
        public string HoraMaxima { get; set; }

        /// <summary>
        /// Valor máximo monetário a ser digitado no cadastro do IPO
        /// </summary>
        public string VlMaximo { get; set; }

        /// <summary>
        /// Valor mínimo monetário a ser digitado no cadastro do IPO
        /// </summary>
        public string VlMinimo { get; set; }

        /// <summary>
        /// Valor percentual de garantia a ser digitado
        /// </summary>
        public string VlPercentualGarantia { get; set; }

        /// <summary>
        /// Observações do IPO
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// Flag para checar se aparece no site ou não,(Ativo ou Inativo)
        /// </summary>
        public string StAtivo { get; set; }

        /// <summary>
        /// Método que fas o cast para a info em questão
        /// </summary>
        /// <returns>Retorna um objeto convertido</returns>
        public IPOInfo ToProdutoIPOClienteInfo()
        {
            var lRetorno = new IPOInfo();

            var lInfo = new CultureInfo("pt-BR");

            if (!string.IsNullOrEmpty(this.CodigoIPO))
            {
                lRetorno.CodigoIPO = Convert.ToInt32(this.CodigoIPO);
            }

            lRetorno.CodigoISIN           = this.CodigoISIN.ToUpper();
            lRetorno.Ativo                = this.Ativo.ToUpper();
            lRetorno.CodigoIPO            = this.CodigoIPO.DBToInt32();
            lRetorno.DataFinal            = this.DataFinal.DBToDateTime();
            lRetorno.DataInicial          = this.DataInicial.DBToDateTime();
            lRetorno.DsEmpresa            = this.DsEmpresa;
            lRetorno.HoraMaxima           = this.HoraMaxima;
            lRetorno.Modalidade           = this.Modalidade;
            lRetorno.VlMaximo             = Convert.ToDecimal(this.VlMaximo, lInfo);
            lRetorno.VlMinimo             = Convert.ToDecimal(this.VlMinimo, lInfo);
            lRetorno.VlPercentualGarantia = Convert.ToDecimal(this.VlPercentualGarantia, lInfo);
            lRetorno.Observacoes          = this.Observacoes;
            lRetorno.StAtivo              = this.StAtivo.DBToBoolean();
            
            return lRetorno;
        }
    }
}