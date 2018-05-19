using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Interface;
using Gradual.OMS.Interface.Dados;
using Gradual.OMS.Interface.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.OMS.Interface
{
    /// <summary>
    /// Implementação do serviço de interface (visual)
    /// </summary>
    public class ServicoInterface : IServicoInterface
    {
        #region Variáveis Locais

        /// <summary>
        /// Referencia para o serviço de persistencia da interface
        /// </summary>
        private IServicoInterfacePersistencia _servicoInterfacePersistencia = Ativador.Get<IServicoInterfacePersistencia>();

        /// <summary>
        /// Classe de configurações do serviço de interface
        /// </summary>
        private ServicoInterfaceConfig _config = GerenciadorConfig.ReceberConfig<ServicoInterfaceConfig>();

        #endregion

        #region GrupoComandoInterface

        /// <summary>
        /// Faz consulta de grupos de comandos de interface de acordo com o filtro especificado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarGruposComandoInterfaceResponse ListarGruposComandoInterface(ListarGruposComandoInterfaceRequest parametros)
        {
            // Prepara resposta
            ListarGruposComandoInterfaceResponse resposta =
                new ListarGruposComandoInterfaceResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Pega do arquivo de configuração?
            if (_config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.ArquivoConfiguracao ||
                _config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.Misto)
                resposta.Resultado.AddRange(
                    _config.GruposComandoInterface);

            // Pega da persistencia?
            if (_config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.Persistencia ||
                _config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.Misto)
                resposta.Resultado.AddRange(
                    _servicoInterfacePersistencia.ListarGruposComandoInterface(parametros).Resultado);

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Salva GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarGrupoComandoInterfaceResponse SalvarGrupoComandoInterface(SalvarGrupoComandoInterfaceRequest parametros)
        {
            // Salvar é na pesistencia... verifica se não existe um grupo com o mesmo código nas configurações
            if (_config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.ArquivoConfiguracao ||
                _config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.Misto)
            {
                // Verifica se existe um grupo com mesmo código no arquivo de configuração
                if (_config.GruposComandoInterface.Find(i => i.CodigoGrupoComandoInterface == parametros.GrupoComandoInterface.CodigoGrupoComandoInterface) != null)
                {
                    // Retorna erro
                    return 
                        new SalvarGrupoComandoInterfaceResponse() 
                        { 
                            CodigoMensagemRequest = parametros.CodigoMensagem,
                            StatusResposta = MensagemResponseStatusEnum.ErroNegocio,
                            DescricaoResposta = 
                                "O código de grupo de interface informado (" + 
                                parametros.GrupoComandoInterface.CodigoGrupoComandoInterface + 
                                ") já existe no arquivo de configurações. O grupo de comandos de interface não pode ser salvo."
                        };
                }
            }

            // Repassa a chamada
            return _servicoInterfacePersistencia.SalvarGrupoComandoInterface(parametros);
        }

        /// <summary>
        /// Recebe detalhe do GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberGrupoComandoInterfaceResponse ReceberGrupoComandoInterface(ReceberGrupoComandoInterfaceRequest parametros)
        {
            // Se deve, tenta procurar nas configurações
            if (_config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.ArquivoConfiguracao ||
                _config.TipoPersistenciaGrupoComandoInterface == GrupoComandoInterfacePersistenciaTipo.Misto)
            {
                // Procura pelo grupo
                GrupoComandoInterfaceInfo grupo = 
                    _config.GruposComandoInterface.Find(
                        i => i.CodigoGrupoComandoInterface == parametros.CodigoGrupoComandoInterface);
                if (grupo != null)
                {
                    // Achou, responde
                    return 
                        new ReceberGrupoComandoInterfaceResponse() 
                        { 
                            CodigoMensagemRequest = parametros.CodigoMensagem,
                            GrupoComandoInterface = (GrupoComandoInterfaceInfo)grupo.Clone()
                        };
                }
            }

            // Repassa a chamada
            return _servicoInterfacePersistencia.ReceberGrupoComandoInterface(parametros);
        }

        /// <summary>
        /// Remove GrupoComandoInterface
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverGrupoComandoInterfaceResponse RemoverGrupoComandoInterface(RemoverGrupoComandoInterfaceRequest parametros)
        {
            // Repassa a chamada
            return _servicoInterfacePersistencia.RemoverGrupoComandoInterface(parametros);
        }

        #endregion

        #region Árvore de Comandos

        /// <summary>
        /// Solicita o processamento dos comandos de interface. Verifica os comandos permitidos e retorna
        /// a árvore.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberArvoreComandosInterfaceResponse ReceberArvoreComandosInterface(ReceberArvoreComandosInterfaceRequest parametros)
        {
            // Referencia para o servico de seguranca
            IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
            
            // Prepara a resposta
            ReceberArvoreComandosInterfaceResponse resposta = 
                new ReceberArvoreComandosInterfaceResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    CodigoGrupoComandoInterface = parametros.CodigoGrupoComandoInterface
                };

            // Pega grupo de comandos
            ReceberGrupoComandoInterfaceResponse respostaReceber = 
                this.ReceberGrupoComandoInterface(
                    new ReceberGrupoComandoInterfaceRequest() 
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoGrupoComandoInterface = parametros.CodigoGrupoComandoInterface
                    });

            // Transforma a árvore de comandos em lista
            List<ComandoInterfaceInfo> listaComandos = respostaReceber.GrupoComandoInterface.ListarComandos();

            // Cria lista indicando quais comandos são grupo
            List<string> listaGrupos =
                (from c in listaComandos
                 where c.Filhos.Count > 0
                 select c.CodigoComandoInterface).ToList();

            // Salva o código do item na tag da segurança para enviar para validação
            foreach (ComandoInterfaceInfo item in listaComandos)
                item.Seguranca.Tag = item.CodigoComandoInterface;
            
            // Cria lista de itens de seguranca a validar
            List<ItemSegurancaInfo> itensSeguranca =
                (from c in listaComandos
                 select c.Seguranca).ToList();

            // Solicita validação dos itens
            ValidarItemSegurancaResponse respostaValidacao =
                servicoSeguranca.ValidarItemSeguranca(
                    new ValidarItemSegurancaRequest() 
                    { 
                        CodigoSessao = parametros.CodigoSessao,
                        ItensSeguranca = itensSeguranca
                    });

            // Cria dicionario com os itens validados para reassociar aos comandos
            Dictionary<string, ItemSegurancaInfo> dicionarioItens = new Dictionary<string, ItemSegurancaInfo>();
            foreach (ItemSegurancaInfo item in respostaValidacao.ItensSeguranca)
                 dicionarioItens.Add(item.Tag, item);

            // Reassocia itens de segurança validado aos comandos
            foreach (ComandoInterfaceInfo comando in listaComandos)
                if (dicionarioItens.ContainsKey(comando.CodigoComandoInterface))
                    comando.Seguranca = dicionarioItens[comando.CodigoComandoInterface];

            // Cria dicionário de pais e filhos
            // O dicionário está montado como codigoFilho, codigoPai
            Dictionary<string, ComandoInterfaceInfo> dicionarioPaisFilhos = new Dictionary<string, ComandoInterfaceInfo>();
            foreach (ComandoInterfaceInfo comandoPai in listaComandos)
                foreach (ComandoInterfaceInfo comandoFilho in comandoPai.Filhos)
                    dicionarioPaisFilhos.Add(comandoFilho.CodigoComandoInterface, comandoPai);

            // Remove os não válidados pela segurança
            List<ComandoInterfaceInfo> comandosRemover =
                (from c in listaComandos
                 where !c.Seguranca.Valido.HasValue || c.Seguranca.Valido.Value == false
                 select c).ToList();
            foreach (ComandoInterfaceInfo comandoRemover in comandosRemover)
                if (dicionarioPaisFilhos.ContainsKey(comandoRemover.CodigoComandoInterface))
                    dicionarioPaisFilhos[comandoRemover.CodigoComandoInterface].Filhos.Remove(comandoRemover);
                else
                    respostaReceber.GrupoComandoInterface.ComandosInterfaceRaiz.Remove(comandoRemover);

            // Remover os menus de grupo que não tem filhos
            List<ComandoInterfaceInfo> comandosRemover2 =
                (from c in listaComandos 
                 where listaGrupos.Contains(c.CodigoComandoInterface) && c.Filhos.Count == 0
                 select c).ToList();
            foreach (ComandoInterfaceInfo comandoRemover in comandosRemover2)
                if (dicionarioPaisFilhos.ContainsKey(comandoRemover.CodigoComandoInterface))
                    dicionarioPaisFilhos[comandoRemover.CodigoComandoInterface].Filhos.Remove(comandoRemover);
                else
                    respostaReceber.GrupoComandoInterface.ComandosInterfaceRaiz.Remove(comandoRemover);

            // Retorna a arvore
            resposta.ComandosInterfaceRaiz = respostaReceber.GrupoComandoInterface.ComandosInterfaceRaiz;
            
            // Retorna
            return resposta;
        }

        #endregion
        
        #region Funcionalidades

        /// <summary>
        /// Recebe a visao de funcionalidades para um usuário ou grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberVisaoFuncionalidadesResponse ReceberVisaoFuncionalidades(ReceberVisaoFuncionalidadesRequest parametros)
        {
            // Retorna
            return null;
        }

        /// <summary>
        /// Salva a visao de funcionalidades para um usuário ou grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarVisaoFuncionalidadesResponse SalvarVisaoFuncionalidades(SalvarVisaoFuncionalidadesRequest parametros)
        {
            // Retorna
            return null;
        }

        #endregion

    }
}
