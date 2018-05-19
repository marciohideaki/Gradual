using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteOrigens
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public TransporteOrigens() { }

        public TransporteOrigens(DbLib.Dados.OrigemInfo pInfo)
        {
            this.Codigo = pInfo.Codigo;

            this.Descricao = pInfo.Descricao.ToString();
        }

        public List<TransporteOrigens> TraduzirLista(List<DbLib.Dados.OrigemInfo> pInfo)
        {
            var lRetorno = new List<TransporteOrigens>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteOrigens()
                    {
                        Codigo = info.Codigo,

                        Descricao = info.Descricao,
                    });
                });
            }

            return lRetorno;
        }
    }
}