using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using log4net;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Implementação do serviço de persistencia utilizando 
    /// serialização binária em arquivos
    /// </summary>
    public class PersistenciaArquivo : IPersistencia
    {
        #region Variaveis Locais

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Referência à classe de configuração
        /// </summary>
        private PersistenciaArquivoConfig _config = null;

        /// <summary>
        /// Lista de entidades carregadas
        /// </summary>
        private PersistenciaArquivoHelper _persistenciaHelper = new PersistenciaArquivoHelper();

        /// <summary>
        /// Indica o status do servico
        /// </summary>
        private ServicoStatus _servicoStatus = ServicoStatus.Parado;

        /// <summary>
        /// Timer do salvamento automático
        /// </summary>
        private Timer _timerSalvamento = null;

        /// <summary>
        /// Lista de hooks carregados
        /// </summary>
        private List<IPersistenciaArquivoHook> _hooks = new List<IPersistenciaArquivoHook>();

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Consultar
        /// Permite o hook interferir no processamento de mensagens de consulta
        /// </summary>
        public event EventHandler<PersistenciaArquivoEventoEventArgs> EventoConsultar;

        /// <summary>
        /// Evento Receber
        /// Permite o hook interferir no processamento de mensagens de receber
        /// </summary>
        public event EventHandler<PersistenciaArquivoEventoEventArgs> EventoReceber;

        /// <summary>
        /// Evento Remover
        /// Permite o hook interferir no processamento de mensagens de remover
        /// </summary>
        public event EventHandler<PersistenciaArquivoEventoEventArgs> EventoRemover;

        /// <summary>
        /// Evento Salvar
        /// Permite o hook interferir no processamento de mensagens de salvar
        /// </summary>
        public event EventHandler<PersistenciaArquivoEventoEventArgs> EventoSalvar;

        #endregion

        #region Construtores e Destrutor

        /// <summary>
        /// Construtor default. Faz a carga do(s) arquivo(s) binários de persistência.
        /// </summary>
        public PersistenciaArquivo()
        {
            // Pega config do arquivo de configuracoes
            _config = GerenciadorConfig.ReceberConfig<PersistenciaArquivoConfig>();

            // Inicializa
            inicializar();
        }

        /// <summary>
        /// Construtor que recebe o arquivo de config. Faz a carga do(s) arquivo(s) binários de persistência.
        /// </summary>
        public PersistenciaArquivo(PersistenciaArquivoConfig config)
        {
            // Referencia para o arquivo de config
            _config = config;

            // Inicializa
            inicializar();
        }

        private void inicializar()
        {
            // Faz a carga do arquivo
            carregarArquivo();

            // Inicia os hooks
            foreach (Type tipo in _config.HooksTipos)
            {
                // Cria a instancia do hook
                IPersistenciaArquivoHook hook = (IPersistenciaArquivoHook)Activator.CreateInstance(tipo);

                // Pede a inicializacao
                hook.Inicializar(this);

                // Adiciona na coleção... apenas para manter referencia
                _hooks.Add(hook);
            }

            // Se for pedido para salvar automaticamente, cria timer
            if (_config.SalvarAutomaticamente)
            {
                TimeSpan tempo = new TimeSpan(0, 0, _config.TempoSalvamentoAutomatico);
                _timerSalvamento =
                    new Timer(
                        new TimerCallback(
                            delegate(object parametros)
                            {
                                try
                                {
                                    salvarArquivo();
                                }
                                catch (Exception ex)
                                {
                                    logger.Error("Erro ao salvar arquivo no timer da PersistenciaBin.", ex);  
                                }
                            }), null, tempo, tempo);
            }
        }

        #endregion

        #region IServicoPersistencia Members

        /// <summary>
        /// Solicita atualização de metadados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            // Não precisa fazer nada
            return new AtualizarMetadadosResponse();
        }

        /// <summary>
        /// Lista os objetos do tipo informado que obedecem às condições.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condicoes"></param>
        /// <returns></returns>
        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            // Permite que o hook interfira no processamento
            if (this.EventoConsultar != null)
            {
                // Cria eventargs para permitir que o hook crie sua resposta
                PersistenciaArquivoEventoEventArgs eventArgs =
                    new PersistenciaArquivoEventoEventArgs()
                    {
                        MensagemRequest = parametros
                    };
                
                // Faz a chamada do hook
                this.EventoConsultar(this, eventArgs);

                // Se tem resposta, é a que deve ser retornada
                if (eventArgs.MensagemResponse != null)
                    return eventArgs.MensagemResponse as ConsultarObjetosResponse<T>;
            }

            // Lista todas as entidades do tipo informado
            IEnumerable<EntidadeInfo> entidadesInfo =
                from e in _persistenciaHelper.Entidades
                where e.Value.TipoObjeto == typeof(T).FullName
                select e.Value;

            // Inicializa
            ConsultarObjetosResponse<T> retorno = new ConsultarObjetosResponse<T>();
            retorno.Resultado = desserializar<T>(entidadesInfo);

            // Faz os filtros
            foreach (CondicaoInfo condicao in parametros.Condicoes)
            {
                // Faz o filtro
                retorno.Resultado =
                    retorno.Resultado.Where<T>(
                        delegate(T obj)
                        {
                            // Inicializa
                            bool retorno2 = false;

                            // Pega o valor da propriedade
                            PropertyInfo ppInfo = typeof(T).GetProperty(condicao.Propriedade);
                            Type tipoValor = ppInfo.PropertyType;
                            object valor = ppInfo.GetValue(obj, null);

                            // Testa a condicao
                            switch (condicao.TipoCondicao)
                            {
                                case CondicaoTipoEnum.Igual:
                                    if (valor != null)
                                        retorno2 = valor.Equals(condicao.Valores[0]);
                                    else
                                        retorno2 = valor == condicao.Valores[0];
                                    break;
                                case CondicaoTipoEnum.Diferente:
                                    if (valor != null)
                                        retorno2 = !valor.Equals(condicao.Valores[0]);
                                    else
                                        retorno2 = !(valor == condicao.Valores[0]);
                                    break;
                                case CondicaoTipoEnum.Maior:
                                    if (valor != null)
                                        retorno2 = ((IComparable)valor).CompareTo(Convert.ChangeType(condicao.Valores[0], tipoValor)) > 0;
                                    break;
                                case CondicaoTipoEnum.MaiorIgual:
                                    if (valor != null)
                                        retorno2 = ((IComparable)valor).CompareTo(Convert.ChangeType(condicao.Valores[0], tipoValor)) >= 0;
                                    break;
                                case CondicaoTipoEnum.Menor:
                                    if (valor != null)
                                        retorno2 = ((IComparable)valor).CompareTo(Convert.ChangeType(condicao.Valores[0], tipoValor)) < 0;
                                    break;
                                case CondicaoTipoEnum.MenorIgual:
                                    if (valor != null)
                                        retorno2 = ((IComparable)valor).CompareTo(Convert.ChangeType(condicao.Valores[0], tipoValor)) <= 0;
                                    break;
                            }

                            // Retorna
                            return retorno2;
                        }).ToList<T>();
            }

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Lista todos os tipos que existem no repositório, ou os tipos que o repositório
        /// trabalha
        /// </summary>
        /// <returns></returns>
        public ListarTiposResponse ListarTipos(ListarTiposRequest parametros)
        {
            // Inicializa
            ListarTiposResponse retorno = new ListarTiposResponse();
            
            // Faz o agrupamento
            var tipos =
                from e in _persistenciaHelper.Entidades.Values.ToList()
                group e by e.TipoObjeto + ", " + e.AssemblyObjeto into g
                select g.Key;

            // Transforma em Type
            foreach (string tipo in tipos)
                retorno.Resultado.Add(Type.GetType(tipo));

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Recebe o objeto desejado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Permite que o hook interfira no processamento
            if (this.EventoReceber != null)
            {
                // Cria eventargs para permitir que o hook crie sua resposta
                PersistenciaArquivoEventoEventArgs eventArgs =
                    new PersistenciaArquivoEventoEventArgs()
                    {
                        MensagemRequest = parametros
                    };

                // Faz a chamada do hook
                this.EventoReceber(this, eventArgs);

                // Se tem resposta, é a que deve ser retornada
                if (eventArgs.MensagemResponse != null)
                    return eventArgs.MensagemResponse as ReceberObjetoResponse<T>;
            }

            // Verifica se existe e retorna
            string chave = gerarChaveEntidade(typeof(T), parametros.CodigoObjeto);
            if (_persistenciaHelper.Entidades.ContainsKey(chave))
                return new ReceberObjetoResponse<T>() { Objeto = desserializar<T>(_persistenciaHelper.Entidades[chave]) };
            else
                return new ReceberObjetoResponse<T>() { Objeto = default(T) };
        }

        /// <summary>
        /// Salva um objeto na persistencia.
        /// Overload do método para trabalhar com mensagens
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Permite que o hook interfira no processamento
            if (this.EventoSalvar != null)
            {
                // Cria eventargs para permitir que o hook crie sua resposta
                PersistenciaArquivoEventoEventArgs eventArgs =
                    new PersistenciaArquivoEventoEventArgs()
                    {
                        MensagemRequest = parametros
                    };

                // Faz a chamada do hook
                this.EventoSalvar(this, eventArgs);

                // Se tem resposta, é a que deve ser retornada
                if (eventArgs.MensagemResponse != null)
                    return eventArgs.MensagemResponse as SalvarObjetoResponse<T>;
            }

            // Pega o código da entidade
            string codigoEntidade = ((ICodigoEntidade)parametros.Objeto).ReceberCodigo();

            // Serializa
            EntidadeInfo entidadeInfo = serializar<T>(parametros.Objeto);

            // Verifica se já existe para adicionar ou alterar
            string chave = gerarChaveEntidade(entidadeInfo);
            lock (_persistenciaHelper)
                if (_persistenciaHelper.Entidades.ContainsKey(chave))
                    _persistenciaHelper.Entidades[chave] = entidadeInfo;
                else
                    _persistenciaHelper.Entidades.Add(chave, entidadeInfo);

            // Retorna
            return 
                new SalvarObjetoResponse<T>() 
                { 
                    Objeto = parametros.Objeto 
                };
        }

        /// <summary>
        /// Remove um objeto da persistencia
        /// </summary>
        /// <param name="codigo"></param>
        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            // Permite que o hook interfira no processamento
            if (this.EventoRemover != null)
            {
                // Cria eventargs para permitir que o hook crie sua resposta
                PersistenciaArquivoEventoEventArgs eventArgs =
                    new PersistenciaArquivoEventoEventArgs()
                    {
                        MensagemRequest = parametros
                    };

                // Faz a chamada do hook
                this.EventoRemover(this, eventArgs);

                // Se tem resposta, é a que deve ser retornada
                if (eventArgs.MensagemResponse != null)
                    return eventArgs.MensagemResponse as RemoverObjetoResponse<T>;
            }

            // Remove da coleção
            _persistenciaHelper.Entidades.Remove(gerarChaveEntidade(typeof(T), parametros.CodigoObjeto));

            // Retorna
            return new RemoverObjetoResponse<T>();
        }

        #endregion

        #region Métodos Locais

        /// <summary>
        /// Gera uma chave string para a entidadeInfo ser colocada
        /// em dicionários.
        /// </summary>
        /// <param name="entidadeInfo"></param>
        /// <returns></returns>
        private string gerarChaveEntidade(EntidadeInfo entidadeInfo)
        {
            return entidadeInfo.TipoObjeto + ";" + entidadeInfo.Codigo;
        }

        /// <summary>
        /// Gera uma chave string para a entidadeInfo ser colocada
        /// em dicionários.
        /// Overload que recebe o tipo do objeto e o código
        /// </summary>
        private string gerarChaveEntidade(Type tipoObjeto, string codigo)
        {
            return tipoObjeto.FullName + ";" + codigo;
        }

        /// <summary>
        /// Faz a inicialização do serviço
        /// </summary>
        private void carregarArquivo()
        {
            // Carrega o arquivo, se existir
            if (File.Exists(_config.ArquivoPersistencia))
            {
                // Carrega o arquivo
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(_config.ArquivoPersistencia);
                _persistenciaHelper = (PersistenciaArquivoHelper)formatter.Deserialize(stream);
                stream.Close();
            }
        }

        /// <summary>
        /// Executa as rotinas de término do serviço
        /// </summary>
        private void salvarArquivo()
        {
            // Salva para um arquivo temporario
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_config.ArquivoPersistencia + ".tmp", FileMode.Create, FileAccess.Write);
            lock (_persistenciaHelper) 
                formatter.Serialize(stream, _persistenciaHelper);
            stream.Close();

            // Se não deu erro, substitui o atual
            if (File.Exists(_config.ArquivoPersistencia))
                File.Delete(_config.ArquivoPersistencia);
            File.Copy(_config.ArquivoPersistencia + ".tmp", _config.ArquivoPersistencia);
        }

        /// <summary>
        /// Faz a serializacao do objeto. Retorna um entidadeInfo com o objeto
        /// serializado. Este overload cria um entidadeInfo vazio e o preenche
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        private EntidadeInfo serializar<T>(T objeto) where T : ICodigoEntidade
        {
            // Inicializa
            Type tipoObjeto = typeof(T);
            Assembly assemblyObjeto = tipoObjeto.Assembly;
            Type tipoObjetoSalvo = objeto.GetType();
            Assembly assemblyObjetoSalvo = tipoObjetoSalvo.Assembly;

            // Pega o código da entidade
            string codigoEntidade = ((ICodigoEntidade)objeto).ReceberCodigo();

            // Cria nova entidadeInfo
            EntidadeInfo entidadeInfo = 
                new EntidadeInfo() 
                { 
                    Codigo = codigoEntidade,
                    DataCriacao = DateTime.Now,
                    DataUltimaAlteracao = DateTime.Now,
                    TipoSerializacao = EntidadeTipoSerializacaoEnum.Binaria,
                    TipoObjeto = tipoObjeto.FullName,
                    AssemblyObjeto = assemblyObjeto.FullName,
                    TipoObjetoSalvo = tipoObjetoSalvo.FullName,
                    AssemblyObjetoSalvo = assemblyObjetoSalvo.FullName
                };

            // Repassa
            return serializar<T>(objeto, entidadeInfo);
        }

        /// <summary>
        /// Faz a serializacao do objeto. Retorna um entidadeInfo com o objeto
        /// serializado. Este overload utiliza um entidadeInfo previamente criado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        private EntidadeInfo serializar<T>(T objeto, EntidadeInfo entidadeInfo) where T : ICodigoEntidade
        {
            // Faz a serializacao binaria do objeto
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, objeto);

            // Salva na entidade
            ms.Position = 0;
            BinaryReader reader = new BinaryReader(ms);
            entidadeInfo.Serializacao = reader.ReadBytes(Convert.ToInt32(ms.Length));

            // Salva outras propriedades
            entidadeInfo.TipoSerializacao = EntidadeTipoSerializacaoEnum.Binaria;
            entidadeInfo.TipoObjeto = typeof(T).FullName;

            // Retorna
            return entidadeInfo;
        }

        /// <summary>
        /// Faz a desserialização de uma lista de entidadesInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entidadesInfo"></param>
        /// <returns></returns>
        private List<T> desserializar<T>(IEnumerable<EntidadeInfo> entidadesInfo) where T : ICodigoEntidade
        {
            // Inicializa
            List<T> retorno = new List<T>();
            
            // Varre desserializando
            foreach (EntidadeInfo entidadeInfo in entidadesInfo)
                retorno.Add(desserializar<T>(entidadeInfo));
            
            // Retorna
            return retorno;
        }

        /// <summary>
        /// Faz a desserialização de um entidadeInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entidadeInfo"></param>
        /// <returns></returns>
        private T desserializar<T>(EntidadeInfo entidadeInfo) where T : ICodigoEntidade
        {
            // Faz a desserializacao do objeto
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(entidadeInfo.Serializacao);
            T retorno = (T)formatter.Deserialize(ms);

            // Retorna
            return retorno;
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            // Efetua log
            logger.Info("Gradual.OMS.Sistemas.Comum.ServicoPersistenciaBin.iniciarServico() - Arquivo " + _config.ArquivoPersistencia + " carregado.");
        }

        public void PararServico()
        {
            // Finaliza o timer, caso exista
            if (_timerSalvamento != null)
                _timerSalvamento.Dispose();

            // Salva o arquivo
            salvarArquivo();

            // Efetua log
            logger.Info("Gradual.OMS.Sistemas.Comum.ServicoPersistenciaBin.pararServico() - Arquivo " + _config.ArquivoPersistencia + " salvo.");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _servicoStatus;
        }

        #endregion
    }
}
