using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Interface;
using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Contratos.Interface.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface
{
    /// <summary>
    /// Implementação do serviço de persistencia de interface (visual)
    /// </summary>
    public class ServicoInterfacePersistencia : IServicoInterfacePersistencia
    {
        /// <summary>
        /// Referencia para o serviço de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoInterfacePersistencia()
        {
        }

        #endregion

        #region IServicoInterfacePersistencia Members

        #region GrupoComandoInterface

        /// <summary>
        /// Consulta de grupos de comandos de interface de acordo com os filtros informados
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarGruposComandoInterfaceResponse ListarGruposComandoInterface(ListarGruposComandoInterfaceRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();

            // Retorna a lista de acordo com os filtros
            return
                new ListarGruposComandoInterfaceResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Resultado =
                        _servicoPersistencia.ConsultarObjetos<GrupoComandoInterfaceInfo>(
                            new ConsultarObjetosRequest<GrupoComandoInterfaceInfo>()
                            {
                                Condicoes = condicoes
                            }).Resultado
                };
        }

        /// <summary>
        /// Salva um GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarGrupoComandoInterfaceResponse SalvarGrupoComandoInterface(SalvarGrupoComandoInterfaceRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<GrupoComandoInterfaceInfo>(
                new SalvarObjetoRequest<GrupoComandoInterfaceInfo>()
                {
                    Objeto = parametros.GrupoComandoInterface
                });

            // Retorna
            return
                new SalvarGrupoComandoInterfaceResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Receber o detalhe de um GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberGrupoComandoInterfaceResponse ReceberGrupoComandoInterface(ReceberGrupoComandoInterfaceRequest parametros)
        {
            // Retorna o GrupoComandoInterface solicitado
            return
                new ReceberGrupoComandoInterfaceResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    GrupoComandoInterface =
                        _servicoPersistencia.ReceberObjeto<GrupoComandoInterfaceInfo>(
                            new ReceberObjetoRequest<GrupoComandoInterfaceInfo>()
                            {
                                CodigoObjeto = parametros.CodigoGrupoComandoInterface
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove um GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverGrupoComandoInterfaceResponse RemoverGrupoComandoInterface(RemoverGrupoComandoInterfaceRequest parametros)
        {
            // Remove o GrupoComandoInterface
            _servicoPersistencia.RemoverObjeto<GrupoComandoInterfaceInfo>(
                new RemoverObjetoRequest<GrupoComandoInterfaceInfo>()
                {
                    CodigoObjeto = parametros.CodigoGrupoComandoInterface
                });

            // Retorna
            return
                new RemoverGrupoComandoInterfaceResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        #endregion

        #endregion
    }
}
