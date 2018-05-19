using Gradual.FIDC.Adm.DbLib.Dados;
using System.Collections.Generic;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteCotistaFidcProcurador
    {

        #region Propriedades
        public int IdCotistaFidcProcurador { get; set; }
        public int IdCotistaFidc { get; set; }
        public string NomeProcurador { get; set; }
        public string CPF { get; set; }
        #endregion

        #region Construtores
        public TransporteCotistaFidcProcurador() { }
        #endregion

        #region Métodos
        public List<TransporteCotistaFidcProcurador> TraduzirLista(List<CotistaFidcProcuradorInfo> pInfo)
        {
            var lRetorno = new List<TransporteCotistaFidcProcurador>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteCotistaFidcProcurador()
                    {
                        IdCotistaFidcProcurador = info.IdCotistaFidcProcurador,
                        IdCotistaFidc = info.IdCotistaFidc,
                        NomeProcurador = info.NomeProcurador,
                        CPF = info.CPF
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}