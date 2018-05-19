using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using com.espertech.esper.client;
using Gradual.OMS.AutomacaoDesktop.Listeners;

namespace Gradual.OMS.AutomacaoDesktop.Monitors
{
    public class BMFLivroOfertasMonitor: AutomacaoMonitorBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BMFLivroOfertasMonitor(DadosGlobais dadosGlobais)
            : base(dadosGlobais)
        { }

        public override void Start()
        {

            EPAdministrator epAdmin = _epService.EPAdministrator;

            try
            {
                String consultaEsper = "select * from EventoBMF where Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BMF_INSTRUMENTO + "' or Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_COMPRA + "' or Tipo='" +
                    ConstantesMDS.TIPO_REQUISICAO_BMF_LIVRO_OFERTAS_VENDA + "'";

                EPStatement comandoEsper = epAdmin.CreateEPL(consultaEsper);
                UpdateListener listenerLivro =
                    new BMFLivroOfertasListener(this._dadosGlobais,
                        this._config.NumeroItensLivroOfertas);

                comandoEsper.AddListener(listenerLivro);

                logger.Info("Consulta [" + consultaEsper + "] cadastrada no ESPER!");
            }

            catch (EPException epex)
            {
                logger.Error("Exception in createEPL - " + epex.Message, epex);
                return;
            }
        }
    }
}
