using System;
using System.ComponentModel;
using System.ServiceModel;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria;

namespace Gradual.OMS.ConsolidadorRelatorioCCLib
{
    [ServiceContract]
    public interface IServicoConsolidadorRelatorioCC
    {
        [OperationContract]
        [Category("ContaCorrente")]
        SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> ConsultarSaldoCCProjetado(SaldoContaCorrenteRiscoRequest pParametro);

        [OperationContract]
        [Category("ContaCorrente")]
        DateTime ConsultarDataHoraUltimaAtualizacao();
    }
}
