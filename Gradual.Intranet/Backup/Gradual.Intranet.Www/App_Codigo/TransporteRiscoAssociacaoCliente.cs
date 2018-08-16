using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoAssociacaoCliente : ITransporteJSON
    {
        /// <summary>
        /// CodigoCliente
        /// </summary>
        public int ParentId { get; set; }
        public int TipoAssociacao { get; set; }

        /// <summary>
        /// Prametro=1,Permissao=2
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        public int CodigoGrupo { get; set; }
        
        /// <summary>
        /// Parametro
        /// </summary>
        public int CodigoClienteParametro { get; set; }
        public int CodigoParametro { get; set; }
        public string ValorParametro { get; set; }
        public string DataValidadeParametro { get; set; }
        //public string DescricaoParametro { get; set; }
        public string CodigoParametroDesc { get; set; }
        
        /// <summary>
        /// Permissao
        /// </summary>
        public int CodigoClientePermissao { get; set; }
        public List<string> Permissoes { get; set; }
        //public string DescricaoPermissao { get; set; }
        public string CodigoPermissaoDesc { get; set; }

        public string CodBovespa { get; set; }
        public string CodBMF { get; set; }

        public bool EhPermissao { get; set; }

        public bool EhParametro { get; set; }

        public bool EhRenovacaoLimite { get; set; }

        public bool EhExpirarLimite { get; set; }
       
        public TransporteRiscoAssociacaoCliente() { }

        public TransporteRiscoAssociacaoCliente(AssociacaoClienteRiscoInfo pAssociacao)
        {
            this.CodigoClienteParametro = pAssociacao.CodigoClienteParametro;
            this.CodigoClientePermissao = pAssociacao.CodigoClientePermissao;
            this.CodigoGrupo = pAssociacao.CodigoGrupo;
            this.CodigoParametro = pAssociacao.CodigoParametro;
            //this.Permissoes = pAssociacao.
            this.DataValidadeParametro = pAssociacao.DataValidadeParametro.ToString("dd/MM/yyyy");
            this.ParentId = pAssociacao.CodigoCliente;
            this.TipoAssociacao = (int)pAssociacao.TipoAssociacao;
            this.ValorParametro = Convert.ToDecimal(pAssociacao.ValorParametro).ToString("c").Replace("R$", string.Empty);//pAssociacao.ValorParametro.ToString("#.##");
            this.CodigoParametroDesc = pAssociacao.DescricaoParametro;
            this.CodigoPermissaoDesc = pAssociacao.DescricaoPermissao;
            this.Id = TipoAssociacao.ToString() + "." + CodigoClienteParametro.ToString();
        }

        public AssociacaoClienteRiscoInfo ToAssociacaoClienteRiscoInfo()
        {
            AssociacaoClienteRiscoInfo lAssociacao = new AssociacaoClienteRiscoInfo();

            lAssociacao.CodigoClienteParametro = this.CodigoParametro;
            lAssociacao.CodigoClientePermissao = this.CodigoClientePermissao;

            lAssociacao.CodigoGrupo = this.CodigoGrupo;
            lAssociacao.CodigoParametro = this.CodigoParametro;
            //lAssociacao.CodigoPermissao = this.CodigoPermissao;
            lAssociacao.DataValidadeParametro = this.DataValidadeParametro.DBToDateTime();
            lAssociacao.CodigoCliente = int.Parse(this.CodBovespa);
            lAssociacao.TipoAssociacao = (AssociacaoClienteRiscoInfo.eTipoAssociacao)this.TipoAssociacao;
            lAssociacao.ValorParametro = decimal.Parse(this.ValorParametro, new CultureInfo("pt-BR"));
            lAssociacao.DescricaoParametro = this.CodigoParametroDesc;
            lAssociacao.DescricaoPermissao = this.CodigoPermissaoDesc;
            

            return lAssociacao;
        }


        #region ITransporteJSON Members

        int? ITransporteJSON.Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TipoDeItem
        {
            get { return "Associacao"; }
        }

        #endregion
    }
}