using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoParametroCliente : ITransporteJSON
    {
        #region ITransporteJSON Members

        public TransporteRiscoParametroCliente()
        {
        }

        public TransporteRiscoParametroCliente(ParametroRiscoClienteInfo pParametroCli)
        {
            this.Id = pParametroCli.CodigoParametroCliente;
            
            this.ParentId = pParametroCli.CodigoCliente;

            this.Parametro = pParametroCli.Parametro.CodigoParametro.ToString();

            this.ParametroDesc = pParametroCli.Parametro.NomeParametro;

            this.DataValidade = pParametroCli.DataValidade.Value.ToString("dd/MM/yyyy");

            this.Valor = pParametroCli.Valor.Value.ToString("#.##");
        }

        public int? Id { get; set; }

        public int ParentId { get; set; }

        public string Descricao
        {
            get
            {
                return ParametroDesc;
            }
        }

        public string Parametro { get; set; }

        public string ParametroDesc { get; set; }

        public string Valor { get; set; }

        public string DataValidade { get; set; }

        public string CodBovespa { get; set; }

        public string CodBMF { get; set; }

        public string TipoDeItem
        {
            get { return "Parametro"; }
        }

        public ParametroRiscoClienteInfo ToParametroRiscoClienteInfo()
        {
            ParametroRiscoClienteInfo lP = new ParametroRiscoClienteInfo();

            lP.CodigoCliente = this.ParentId; //this.CodBovespa == "" ? this.CodBMF == "" ? 0 : int.Parse(this.CodBMF) : int.Parse(this.CodBovespa);
            lP.CodigoParametroCliente = this.Id == null ? 0 : this.Id.Value;
            lP.DataValidade = Convert.ToDateTime(DataValidade);
            lP.Parametro = new ParametroRiscoInfo()
            {
                NomeParametro = ParametroDesc,
                CodigoParametro = int.Parse(Parametro)
            };

            lP.Valor = decimal.Parse(Valor, new CultureInfo("en-US"));

            return lP;
        }

        #endregion
    }
}

