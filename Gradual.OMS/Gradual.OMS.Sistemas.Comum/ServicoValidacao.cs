using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Implementação do serviço de validação
    /// </summary>
    public class ServicoValidacao : IServicoValidacao
    {
        /// <summary>
        /// Referencia para o objeto de configurações do serviço de validação
        /// </summary>
        private ServicoValidacaoConfig _config = GerenciadorConfig.ReceberConfig<ServicoValidacaoConfig>();

        /// <summary>
        /// Dicionário com as regras por tipo de mensagem. Este dicionário é preenchido
        /// com informações do config. A chave do dicionário é o tipo da mensagem.
        /// </summary>
        private Dictionary<Type, RegrasPorTipoInfo> _regrasPorTipo = new Dictionary<Type, RegrasPorTipoInfo>();

        /// <summary>
        /// Dicionário com os geradores por tipo de mensagem. Quando a aplicação sobe, este dicionário
        /// é preenchido com informações do config. A chave do dicionário é o tipo da mensagem.
        /// </summary>
        private Dictionary<Type, List<IGeradorRegra>> _geradoresPorTipo = new Dictionary<Type, List<IGeradorRegra>>();

        /// <summary>
        /// Lista de regras que devem ser aplicados a todas as mensagens
        /// </summary>
        private List<RegraBase> _regrasGerais = new List<RegraBase>();

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoValidacao()
        {
        }

        private void inicializar()
        {
            // Cria dicionário com as regras por tipo
            foreach (RegrasPorTipoInfo regrasPorTipo in _config.RegrasPorTipo)
                _regrasPorTipo.Add(regrasPorTipo.TipoMensagem, regrasPorTipo);

            // Cria os geradores e atribui aos tipos solicitados
            foreach (GeradorRegraInfo geradorRegraInfo in _config.GeradoresRegra)
            {
                // Cria o gerador
                IGeradorRegra geradorRegra =
                    (IGeradorRegra)
                        Activator.CreateInstance(geradorRegraInfo.TipoGeradorRegra);

                // Adiciona por namespaces
                if (geradorRegraInfo.NamespacesAssociados != null)
                {
                    // Faz quebra por ponto e virgula (indicando multiplos namespaces)
                    string[] nss = geradorRegraInfo.NamespacesAssociados.Split(';');

                    // Varre
                    foreach (string ns in nss)
                    {
                        // Separa namespace do assembly
                        string[] ns2 = ns.Split(',');

                        // Pega o assembly
                        Assembly assembly = Assembly.Load(ns2[1].Trim());

                        // Varre os tipos adicionando no dicionario de geradores
                        foreach (Type tipo in assembly.GetTypes())
                            if (tipo.Namespace == ns2[0])
                            {
                                // Verifica se já existe o item na lista de geradores
                                if (!_geradoresPorTipo.ContainsKey(tipo))
                                    _geradoresPorTipo.Add(tipo, new List<IGeradorRegra>());

                                // Adiciona
                                _geradoresPorTipo[tipo].Add(geradorRegra);
                            }
                    }
                }

                // Adiciona por tipos
                if (geradorRegraInfo.TiposAssociados != null)
                {
                    // Faz quebra por ponto e virgula (indicando multiplos tipos)
                    string[] nss = geradorRegraInfo.TiposAssociados.Split(';');

                    // Varre
                    foreach (string ns in nss)
                    {
                        // Pega o tipo
                        Type tipo = Type.GetType(ns);

                        // Verifica se já existe o item na lista de geradores
                        if (!_geradoresPorTipo.ContainsKey(tipo))
                            _geradoresPorTipo.Add(tipo, new List<IGeradorRegra>());

                        // Adiciona na coleção
                        _geradoresPorTipo[tipo].Add(geradorRegra);
                    }
                }
            }
        }
        
        #region IServicoValidacao Members

        /// <summary>
        /// Realiza a validação da mensagem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ValidarMensagemResponse ValidarMensagem(ValidarMensagemRequest parametros)
        {
            // Referencia para a mensagem e para o tipo da mensagem
            object mensagem = parametros.Mensagem;
            Type tipoMensagem = mensagem.GetType();

            // Cria um contexto de validação
            ContextoValidacaoInfo contextoValidacao = 
                new ContextoValidacaoInfo() 
                { 
                    CodigoSessao = parametros.CodigoSessao,
                    Mensagem = parametros.Mensagem,
                    TipoMensagem = tipoMensagem
                };

            // Pega lista de regras associadas a esse tipo

            // Chama os geradores de sequencia de regras para esse tipo
            if (_geradoresPorTipo.ContainsKey(tipoMensagem))
                foreach (IGeradorRegra geradorRegra in _geradoresPorTipo[tipoMensagem])
                    geradorRegra.GerarSequencia(contextoValidacao);

            // Faz a chamada das regras
            contextoValidacao.MensagemValida = true;
            foreach (RegraBase regra in contextoValidacao.RegrasAValidar)
                contextoValidacao.MensagemValida = 
                    regra.Validar(contextoValidacao) && contextoValidacao.MensagemValida;

            // Se todas as regras não validaram, desfaz todas as validações
            if (!contextoValidacao.MensagemValida)
                foreach (RegraBase regra in contextoValidacao.RegrasAValidar)
                    regra.Desfazer(contextoValidacao);

            // Retorna
            return 
                new ValidarMensagemResponse() 
                { 
                    ContextoValidacao = contextoValidacao,
                    Criticas = contextoValidacao.Criticas
                };
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            inicializar();
        }

        public void PararServico()
        {
        }

        public Gradual.OMS.Library.Servicos.ServicoStatus ReceberStatusServico()
        {
            return Gradual.OMS.Library.Servicos.ServicoStatus.Indefinido;
        }

        #endregion
    }
}
