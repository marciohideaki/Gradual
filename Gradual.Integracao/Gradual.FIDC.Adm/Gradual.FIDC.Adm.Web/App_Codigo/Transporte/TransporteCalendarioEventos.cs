using Gradual.FIDC.Adm.DbLib.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.App_Codigo.Transporte
{
    public class TransporteCalendarioEventos
    {
        #region Propriedades

        public int IdCalendarioEvento { get; set; }
        public int IdFundoCadastro { get; set; }
        public string NomeFundo { get; set; }
        public string DtEvento { get; set; }
        public string DescEvento { get; set; }
        public string EmailEvento { get; set; }
        public string EnviarNotificacaoDia { get; set; }
        public string MostrarHome { get; set; }

        #endregion

        #region Construtores
        public TransporteCalendarioEventos() { }

        public TransporteCalendarioEventos(CalendarioEventoInfo pInfo)
        {
            this.NomeFundo = pInfo.NomeFundo.ToString();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método para Tradução de lista para calendario de eventos
        /// </summary>
        /// <param name="pInfo">Info de calendario de eventos</param>
        /// <returns>Retorna uma lista de calendario de eventos</returns>
        public List<TransporteCalendarioEventos> TraduzirLista(List<CalendarioEventoInfo> pInfo)
        {
            var lRetorno = new List<TransporteCalendarioEventos>();

            if (pInfo != null && pInfo.Count > 0)
            {
                pInfo.ForEach(info =>
                {
                    lRetorno.Add(new TransporteCalendarioEventos()
                    {
                        IdCalendarioEvento = info.IdCalendarioEvento,
                        IdFundoCadastro = info.IdFundoCadastro,
                        NomeFundo = info.NomeFundo,
                        DtEvento = info.DtEvento.ToString("dd/MM/yyyy HH:mm"),
                        DescEvento = info.DescEvento,
                        EmailEvento = info.EmailEvento,
                        EnviarNotificacaoDia = (info.EnviarNotificacaoDia) ? "<span class='glyphicon glyphicon-ok'></span>" : "<span class='glyphicon glyphicon-remove'></span>",
                        MostrarHome = (info.MostrarHome) ? "<span class='glyphicon glyphicon-ok'></span>" : "<span class='glyphicon glyphicon-remove'></span>"
                    });
                });
            }

            return lRetorno;
        }
        #endregion
    }
}