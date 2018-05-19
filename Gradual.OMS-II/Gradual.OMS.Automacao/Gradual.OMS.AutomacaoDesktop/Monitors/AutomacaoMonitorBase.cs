using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.AutomacaoDesktop;
using com.espertech.esper.client;

namespace Gradual.OMS.AutomacaoDesktop.Monitors
{
    public abstract class AutomacaoMonitorBase
    {
        protected EPServiceProvider _epService;
        protected DadosGlobais _dadosGlobais;
        protected AutomacaoConfig _config;

        public AutomacaoMonitorBase(DadosGlobais dadosglobais)
        {
            _epService = dadosglobais.EpService;
            _dadosGlobais = dadosglobais;
            _config = dadosglobais.Parametros;
        }

        /// <summary>
        /// Metodo invocado durante a inicializacao dos monitores
        /// </summary>
        public virtual void Start()
        {
            throw new NotImplementedException("Funcao AutomacaoMonitorBase:Start() não implementada");
        }
    }
}
