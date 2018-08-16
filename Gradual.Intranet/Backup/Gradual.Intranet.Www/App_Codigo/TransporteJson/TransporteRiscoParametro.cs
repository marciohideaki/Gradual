using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoParametro
    {
        public TransporteRiscoParametro() { }

        public TransporteRiscoParametro(ParametroRiscoInfo pParametroRiscoInfo)
        {
            this.Id = pParametroRiscoInfo.CodigoParametro.ToString();
            this.Descricao = pParametroRiscoInfo.NomeParametro;
            this.Bolsa = (int)pParametroRiscoInfo.Bolsa;
        }

        public ParametroRiscoInfo ToParametroRiscoInfo()
        {
            ParametroRiscoInfo lRetorno = new ParametroRiscoInfo();

            lRetorno.NomeParametro = this.Descricao;
            int id = 0;
            if(int.TryParse(this.Id, out id))
                lRetorno.CodigoParametro = id;
            lRetorno.Bolsa = (BolsaInfo)Bolsa;

            return lRetorno;
        }

        public List<TransporteRiscoParametro> TraduzirLista(List<ParametroRiscoInfo> pParametros)
        {
            var lRetorno = new List<TransporteRiscoParametro>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(pri =>
                {
                    lRetorno.Add(new TransporteRiscoParametro()
                    {
                        Id = pri.CodigoParametro.ToString(),
                        Descricao = pri.NomeParametro,
                        Bolsa = (int)pri.Bolsa
                    });
                });


            return lRetorno;
        }

        #region Propriedades

        public string Id { get; set; }

        public string Descricao { get; set; }

        public int Bolsa { get; set; }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Parametro"; } }

        #endregion
    }

}