using System.Collections.Generic;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Implementação do serviço de persistencia de mensagens utilizando
    /// o serviço de persistencia de objetos (IServicoPersistencia)
    /// </summary>
    public class ServicoPersistenciaMensagens : IServicoPersistenciaMensagens
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #endregion

        #region Construtor e Destrutor

        /// <summary>
        /// Construtor. Carrega o arquivo de persistencia se existir.
        /// </summary>
        public ServicoPersistenciaMensagens()
        {
        }

        #endregion

        #region IServicoPersistenciaMensagens Members

        /// <summary>
        /// Salva a mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarMensagemResponse SalvarMensagem(SalvarMensagemRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<MensagemBase>(
                new SalvarObjetoRequest<MensagemBase>() 
                { 
                    Objeto = parametros.Mensagem 
                });

            // Retorna
            return new SalvarMensagemResponse();
        }

        /// <summary>
        /// Retorna uma mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberMensagemResponse ReceberMensagem(ReceberMensagemRequest parametros)
        {
            // Retorna a ordem da coleção
            return
                new ReceberMensagemResponse()
                {
                    Mensagem = 
                        _servicoPersistencia.ReceberObjeto<MensagemBase>(
                            new ReceberObjetoRequest<MensagemBase>() 
                            { 
                                CodigoObjeto = parametros.CodigoMensagem 
                            }).Objeto
                };
        }

        /// <summary>
        /// Lista as mensagens.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarMensagensResponse ListarMensagens(ListarMensagensRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            if (parametros.FiltroCodigoMensagemReferencia != null)
                condicoes.Add(new CondicaoInfo("CodigoMensagemReferencia", CondicaoTipoEnum.Igual, parametros.FiltroCodigoMensagemReferencia));

            // Retorna a lista de acordo com os filtros
            return
                new ListarMensagensResponse()
                {
                    Mensagens = 
                        _servicoPersistencia.ConsultarObjetos<MensagemBase>(
                            new ConsultarObjetosRequest<MensagemBase>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        #endregion
    }
}
