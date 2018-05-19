using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Comandos
{
    /// <summary>
    /// Lança o controle solicitado em nova janela em uma existente.
    /// </summary>
    public class ComandoIniciarControle : IComando
    {
        private Comando _item = null;

        #region IInterfaceBase<Comando> Members

        public void Inicializar(Comando itemBase)
        {
            _item = itemBase;
        }

        public void Executar()
        {
            // Pega config
            ComandoIniciarControleParametro parametros =
                (ComandoIniciarControleParametro)_item.Info.Parametros.Objeto;

            // Pega referencia ao objeto de parametros da janela, caso exista
            object parametrosJanela = null;
            if (parametros.ParametrosJanela != null)
                parametrosJanela = parametros.ParametrosJanela.Objeto;

            // Pega servico de interface
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();

            // Cria ou pega janela
            JanelaInfo janelaInfo = new JanelaInfo();
            servicoInterface.CriarJanela(janelaInfo, parametrosJanela);

            // Adiciona o controle
            servicoInterface.AdicionarControle(janelaInfo.Id, parametros.ControleInfo);
        }

        public MensagemInterfaceResponseBase ProcessarMensagem(MensagemInterfaceRequestBase parametros)
        {
            return null;
        }

        #endregion
    }
}
