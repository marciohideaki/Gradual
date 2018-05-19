using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using com.espertech.esper.client;
using Gradual.OMS.AutomacaoDesktop.Listeners;
using Gradual.OMS.Automacao.Lib;

namespace Gradual.OMS.AutomacaoDesktop.Monitors
{
    public class BovespaLivroOfertasMonitor : AutomacaoMonitorBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BovespaLivroOfertasMonitor(DadosGlobais dadosGlobais)
            : base(dadosGlobais)
        { }

        public override void Start()
        {

            EPAdministrator epAdmin = _epService.EPAdministrator;

            try
            {
                String consultaEsper = "select * from EventoBovespa where Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BOVESPA_RETRANSMISSAO_LIVRO_OFERTAS + "' or Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BOVESPA_ATUALIZACAO_LIVRO_OFERTAS + "' or Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BOVESPA_CANCELAMENTO_LIVRO_OFERTAS + "'";

                EPStatement comandoEsper = epAdmin.CreateEPL(consultaEsper);
                UpdateListener listenerLivro =
                    new BovespaLivroOfertasListener(
                            this._dadosGlobais, this._config.NumeroItensLivroOfertas);
                comandoEsper.AddListener(listenerLivro);

                logger.Info("Consulta [" + consultaEsper + "] cadastrada no ESPER!");
            }

            catch (EPException epex)
            {
                logger.Error("Exception in createEPL - " + epex.Message);
                return;
            }
        }
    }
}
