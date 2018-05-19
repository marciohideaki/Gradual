using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public abstract class ItemBase
    {
        /// <summary>
        /// Método que cria instância de um objeto baseado em ItemInfoBase.
        /// Caso a instância criada tenha o método inicializar, faz a chamada do método
        /// passando como parâmetro o item que está pedindo a criação da instância.
        /// </summary>
        /// <param name="itemInfo"></param>
        /// <param name="itemPai"></param>
        /// <returns></returns>
        public object CriarObjeto(ItemInfoBase itemInfo, object itemPai)
        {
            // Inicializa
            Type tipoInstancia = itemInfo.ReceberTipoInstancia();

            // Caso não tenha o tipo informado, pega da factory
            if (tipoInstancia == null)
            {
                // Acha a relacao
                ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
                List<RelacaoItemControleInfo> relacaoLista =
                    config.ReceberSkinAtual().RelacaoItensControles;
                RelacaoItemControleInfo relacaoInfo = (from r in relacaoLista
                                                       where r.ItemTipo == itemInfo.ItemTipo
                                                       select r).FirstOrDefault();

                // Cria o tipo
                if (relacaoInfo != null)
                {
                    itemInfo.TipoInstanciaString = relacaoInfo.TipoObjetoDestino;
                    tipoInstancia = itemInfo.ReceberTipoInstancia();
                }
            }

            // Cria o objeto
            object instancia = Activator.CreateInstance(tipoInstancia);

            // Chama a inicializacao
            tipoInstancia.InvokeMember(
                "Inicializar",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                instancia,
                new object[] { itemPai });

            // Retorna 
            return instancia;
        }
    }
}
