using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Fix;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.CanaisNegociacao.Servidor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCanaisNegociacaoServidorTeste : IServicoCanaisNegociacaoServidorTeste
    {
        private string _servicoId = null;
        private ServicoStatus _servicoStatus = ServicoStatus.Parado;
        private HostFix<AplicacaoFixServidorTeste> _hostFix = null;
        private AplicacaoFixServidorTeste _aplicacaoFix = null;

        #region IServicoControlavel Members

        void IServicoControlavel.IniciarServico()
        {
            // Carrega configurações
            ServicoCanaisNegociacaoServidorTesteConfig config =
                GerenciadorConfig.ReceberConfig<ServicoCanaisNegociacaoServidorTesteConfig>(_servicoId);

            // Sobe o servidor fix
            _hostFix = new HostFix<AplicacaoFixServidorTeste>(config.ArquivoConfig, false);
            _aplicacaoFix = _hostFix.AplicacaoFix;
            _hostFix.Iniciar();
            _aplicacaoFix.Iniciar();

            // Sinaliza
            _servicoStatus = ServicoStatus.EmExecucao;
        }

        void IServicoControlavel.PararServico()
        {
            // Finaliza o servidor fix
            _aplicacaoFix.Parar();
            _hostFix.Parar();

            // Sinaliza
            _servicoStatus = ServicoStatus.Parado;
        }

        ServicoStatus IServicoControlavel.ReceberStatusServico()
        {
            return _servicoStatus;
        }

        #endregion

        #region IServicoID Members

        void IServicoID.SetarID(string id)
        {
            _servicoId = id;
        }

        string IServicoID.ReceberID()
        {
            return _servicoId;
        }

        #endregion
    }
}
