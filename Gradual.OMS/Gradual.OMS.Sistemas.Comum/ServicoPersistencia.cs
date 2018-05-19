using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
	/// <summary>
	/// Implementação do serviço de persistencia utilizando bancos de dados SQL Server.
	/// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class ServicoPersistencia : IServicoPersistencia
    {
        /// <summary>
        /// Objeto de configurações
        /// </summary>
        private ServicoPersistenciaConfig _config = GerenciadorConfig.ReceberConfig<ServicoPersistenciaConfig>();

        /// <summary>
        /// Dicionário relacionando o tipo do objeto com a persistencia que irá tratar
        /// </summary>
        private Dictionary<Type, ServicoPersistenciaItemHelper> _persistencias = new Dictionary<Type, ServicoPersistenciaItemHelper>();

        /// <summary>
        /// Indica a persistencia default
        /// </summary>
        private ServicoPersistenciaItemHelper _persistenciaDefault = null;

        /// <summary>
        /// Espécie de cache para indicar a ultima resolução do tipo para o helper.
        /// Auxilia em casos de loops, aonde o mesmo tipo de objeto será resolvido várias vezes
        /// </summary>
        private KeyValuePair<Type, ServicoPersistenciaItemHelper> _ultimaResolucao = new KeyValuePair<Type,ServicoPersistenciaItemHelper>(null, null);

        /// <summary>
        /// Indica o status do servico
        /// </summary>
        private ServicoStatus _status = ServicoStatus.Parado;

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoPersistencia()
        {
            // Carrega a lista de persistencias por tipo de objeto
            foreach (PersistenciaInfo persistenciaInfo in _config.Persistencias)
                this.AdicionarPersistencia(persistenciaInfo);
        }

        #endregion

        #region IServicoPersistencia Members

        public void AdicionarPersistencia(PersistenciaInfo persistenciaInfo)
        {
            // Cria o helper
            ServicoPersistenciaItemHelper helper = new ServicoPersistenciaItemHelper();
            helper.PersistenciaInfo = persistenciaInfo;
            
            // Cria a persistencia com ou sem o construtor de config
            if (persistenciaInfo.Config != null && persistenciaInfo.Config.Objeto != null)
                helper.Instancia = 
                    (IPersistencia)
                        Activator.CreateInstance(
                            persistenciaInfo.TipoPersistencia, 
                            new object[] { persistenciaInfo.Config.Objeto });
            else
                helper.Instancia =
                    (IPersistencia)
                        Activator.CreateInstance(
                            persistenciaInfo.TipoPersistencia);

            // Chama inicialização da persistencia
            helper.Instancia.IniciarServico();

            // Adiciona para todos os tipos a serem tratados por essa persistencia
            if (!persistenciaInfo.Default)
            {
                // Varre os tipos informados
                if (persistenciaInfo.TipoObjetos != null)
                    foreach (Type tipo in persistenciaInfo.TipoObjetos)
                        _persistencias.Add(tipo, helper);

                // Varre os namespaces informados
                if (persistenciaInfo.NamespacesObjetos != null)
                {
                    foreach (string ns in persistenciaInfo.NamespacesObjetos)
                    {
                        // Quebra
                        string[] ns2 = ns.Split(',');
                        ns2[0] = ns2[0].Trim();

                        // Pega o assembly
                        Assembly assembly = Assembly.Load(ns2[1]);

                        // Varre os tipos
                        foreach (Type tipo in assembly.GetTypes())
                            if (tipo.Namespace.StartsWith(ns2[0]))
                                _persistencias.Add(tipo, helper);
                    }
                }
            }
            else
            {
                _persistenciaDefault = helper;
            }
        }

        /// <summary>
        /// Solicita atualização de metadados.
        /// Esta operação solicita que a persistencia faça a atualização de 
        /// metadados, por exemplo, inserindo os Lista/ListaItem, a lista
        /// de permissões, etc. Cada persistencia trabalha com os metadados
        /// que necessitar
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            // Pega lista de persistencias
            List<ServicoPersistenciaItemHelper> persistencias =
                (from p in _persistencias
                 select p.Value).Distinct().ToList();

            // Varre as persistencias solicitando a atualização de metadados
            foreach (ServicoPersistenciaItemHelper persistenciaHelper in persistencias)
                persistenciaHelper.Instancia.AtualizarMetadados(parametros);

            // A resposta é uma só para todas as atualizações, indicando que a operação foi disparada
            return 
                new AtualizarMetadadosResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }
        
        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            // Repassa a mensagem
            return localizarHelper(typeof(T)).Instancia.ConsultarObjetos<T>(parametros);
        }

        public ListarTiposResponse ListarTipos(ListarTiposRequest parametros)
        {
            // Resultado
            ListarTiposResponse resposta = new ListarTiposResponse();

            // Pede a lista para todas as persistencias
            List<IPersistencia> persistencias = new List<IPersistencia>();
            foreach (KeyValuePair<Type, ServicoPersistenciaItemHelper> item in _persistencias)
                // Se ainda nao tem na coleção significa que nao pediu para parar
                if (!persistencias.Contains(item.Value.Instancia))
                    // Pede lista dos tipos
                    foreach (Type tipo in item.Value.Instancia.ListarTipos(new ListarTiposRequest()).Resultado)
                        if (!resposta.Resultado.Contains(tipo))
                            resposta.Resultado.Add(tipo);

            // Pede lista para a persistencia default
            foreach (Type tipo in _persistenciaDefault.Instancia.ListarTipos(new ListarTiposRequest()).Resultado)
                if (!resposta.Resultado.Contains(tipo))
                    resposta.Resultado.Add(tipo);

            // Retorna
            return resposta;
        }

        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Repassa a mensagem
            return localizarHelper(typeof(T)).Instancia.ReceberObjeto<T>(parametros);
        }

        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Repassa a mensagem
            return localizarHelper(typeof(T)).Instancia.SalvarObjeto<T>(parametros);
        }

        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Repassa a mensagem
            return localizarHelper(typeof(T)).Instancia.RemoverObjeto<T>(parametros);
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            // Sinaliza
            _status = ServicoStatus.EmExecucao;
        }

        public void PararServico()
        {
            // Lista todas as persistencias
            List<IPersistencia> persistencias = new List<IPersistencia>();
            foreach (KeyValuePair<Type, ServicoPersistenciaItemHelper> item in _persistencias)
            {
                // Se ainda nao tem na coleção significa que nao pediu para parar
                if (!persistencias.Contains(item.Value.Instancia))
                {
                    // Pede para parar e adiciona na coleção para sinalizar
                    item.Value.Instancia.PararServico();
                    persistencias.Add(item.Value.Instancia);
                }
            }

            // Pede para parar a persistencia default
            _persistenciaDefault.Instancia.PararServico();

            // Sinaliza
            _status = ServicoStatus.Parado;
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion

        private ServicoPersistenciaItemHelper localizarHelper(Type tipo)
        {
            // Verifica se bate com o ultimo acesso
            if (_ultimaResolucao.Key == tipo)
                return _ultimaResolucao.Value;

            // Tenta localizar o tipo
            if (_persistencias.ContainsKey(tipo))
            {
                _ultimaResolucao = new KeyValuePair<Type, ServicoPersistenciaItemHelper>(tipo, _persistencias[tipo]);
                return _ultimaResolucao.Value;
            }
            else
            {
                if (tipo.BaseType != null)
                    return localizarHelper(tipo.BaseType);
                else
                    return _persistenciaDefault;
            }
        }
    }
}
