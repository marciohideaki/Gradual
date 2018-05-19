using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteAssociacaoCotistasFundos
    {
        
        #region Propriedades
        public int IdCotistaFidcFundo { get; set; }
        public int IdCotistaFidc { get; set; }
        public int IdFundoCadastro { get; set; }
        public DateTime DtInclusao { get; set; }
        public string NomeCotista { get; set; }
        public string NomeFundo { get; set; }
        public string EmailCotista { get; set; }  
        #endregion

        #region Construtores
        public TransporteAssociacaoCotistasFundos() { }

        #endregion

        #region Métodos
        public List<TransporteAssociacaoCotistasFundos> TraduzirLista(List<CotistaFidcFundoInfo> pInfo)
        {
            var lRetorno = new List<TransporteAssociacaoCotistasFundos>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteAssociacaoCotistasFundos()
                    {
                        IdCotistaFidcFundo = info.IdCotistaFidcFundo,
                        IdCotistaFidc = info.IdCotistaFidc,
                        IdFundoCadastro = info.IdFundoCadastro,
                        NomeCotista = info.NomeCotista,
                        NomeFundo = info.NomeFundo,
                        DtInclusao = info.DtInclusao,
                        EmailCotista = info.EmailCotista
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}