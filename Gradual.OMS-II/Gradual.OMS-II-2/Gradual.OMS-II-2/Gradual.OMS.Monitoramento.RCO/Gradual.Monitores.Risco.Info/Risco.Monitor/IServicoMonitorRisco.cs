using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Risco.Lib;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.Monitores.Risco.Info;

namespace Gradual.Monitores.Risco.Lib
{    
    [ServiceContract(Namespace = "http://gradual")]   
    public interface IServicoMonitorRisco
    {
        [OperationContract]
        MonitorLucroPrejuizoResponse ObterMonitorLucroPrejuizo(MonitorLucroPrejuizoRequest pRequest);

        [OperationContract]
        MonitorPLDResponse ObterMonitorPLD(MonitorPLDRequest pRequest);

        [OperationContract]
        ObterDadosClienteResponse ObterDadosCliente(ObterDadosClienteRequest pRequest);

        [OperationContract]
        ObterPosicaoBtcResponse ObterPosicaoBTC(ObterPosicaoBtcRequest pRequest);

        [OperationContract]
        ObterPosicaoTermoResponse ObterPosicaoTermo(ObterPosicaoTermoRequest pRequest);

    }
}
