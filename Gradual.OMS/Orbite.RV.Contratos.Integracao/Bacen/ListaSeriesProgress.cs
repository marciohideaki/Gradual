using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.Integracao.Bacen;

namespace Orbite.RV.Contratos.Integracao.Bacen
{
    public class ListaSeriesProgress
    {
        public delegate void NotificaEtapaDelegate(string etapa, int numeroFonte);
        public delegate void NotificaQuantidadeFontesDelegate(int quantidade);
        public delegate void NotificaFimDelegate();

        public event NotificaEtapaDelegate NotificaEtapaEvent;
        public event NotificaQuantidadeFontesDelegate NotificaQuantidadeFontesEvent;
        public event NotificaFimDelegate NotificaFimEvent;

        public void NotificaEtapa(string etapa, int numeroFonte)
        {
            if (NotificaEtapaEvent != null)
                NotificaEtapaEvent(etapa, numeroFonte);
        }

        public void NotificaQuantidadeFontes(int quantidade)
        {
            if (NotificaQuantidadeFontesEvent != null)
                NotificaQuantidadeFontesEvent(quantidade);
        }

        public void NotificaFim()
        {
            if (NotificaFimEvent != null)
                NotificaFimEvent();
        }
    }
}
