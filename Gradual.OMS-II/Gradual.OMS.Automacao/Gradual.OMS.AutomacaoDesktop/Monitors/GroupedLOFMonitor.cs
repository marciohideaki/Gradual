using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.espertech.esper.client;
using log4net;
using Gradual.OMS.AutomacaoDesktop.Listeners;

namespace Gradual.OMS.AutomacaoDesktop.Monitors
{
    public class GroupedLOFMonitor :AutomacaoMonitorBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GroupedLOFMonitor(DadosGlobais dadosGlobais): base(dadosGlobais)
        {
        }

        public override void Start()
        {

            EPAdministrator epAdmin = _epService.EPAdministrator;

            try
            {
                String consultaEsper = "insert into EventoLOFAgrupado( instrumento, plataforma, cabecalho, LivroOfertasCompra, LivroOfertasVenda) ";
                consultaEsper += " select * from EventoAtualizacaoLivroOfertas  ";
                consultaEsper += " select * from EventoAtualizacaoLivroOfertas  ";


                EPStatement comandoEsper = epAdmin.CreateEPL(consultaEsper);
                UpdateListener listenerLivro = new GroupedLOFListenerClass1();

                //    new BovespaLivroOfertasListener(
                //            this._dadosGlobais, this._config.NumeroItensLivroOfertas);
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
