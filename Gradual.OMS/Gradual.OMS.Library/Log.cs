using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Utilitário para fornecer funções de log de mensagens.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Indica se o log foi inicializado nesta execução.
        /// </summary>
        private static bool _inicializado = false;

        /// <summary>
        /// Aponta para a classe de configurações do log
        /// </summary>
        private static LogConfig _config = null;

        /// <summary>
        /// Dicionário associando as origens com a referencia das origens de log.
        /// A chave é o valor da propriedade da classe de origens e o valor é o que 
        /// tem que ser informado no EventLog como origem.
        /// </summary>
        private static Dictionary<string, string> _dicionarioOrigens = new Dictionary<string, string>();

        /// <summary>
        /// Efetua o log. Overload que recebe diretamente uma excessão
        /// </summary>
        /// <param name="ex"></param>
        public static void EfetuarLog(Exception ex)
        {
            // Repassa a chamada
            Log.EfetuarLog(ex, null);
        }

        /// <summary>
        /// Efetuar log. Faz o log de uma excessão serializando o parametro e assumindo origem default.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="parametro"></param>
        /// <param name="referenciaOrigem"></param>
        public static void EfetuarLog(Exception ex, object parametro)
        {
            // Repassa a chamada
            EfetuarLog(ex, parametro, (string)null);
        }
        
        /// <summary>
        /// Efetua o log. Overload que recebe diretamente uma excessão
        /// </summary>
        /// <param name="ex"></param>
        public static void EfetuarLog(Exception ex, object parametro, string referenciaOrigem)
        {
            // Repassa a chamada
            if (parametro.GetType() != typeof(string))
                Log.EfetuarLog(ex, Serializador.TransformarEmString(parametro), referenciaOrigem);
            else
                Log.EfetuarLog(ex, (string)parametro, referenciaOrigem);
        }

        /// <summary>
        /// Efetua o log. Overload que loga excessão junto com mensagem utilizando origem default
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="mensagem"></param>
        public static void EfetuarLog(Exception ex, string mensagem)
        {
            // Repassa a chamada
            EfetuarLog(ex, mensagem, (string)null);
        }

        /// <summary>
        /// Efetua o log. Overload que recebe diretamente uma excessão
        /// </summary>
        /// <param name="ex"></param>
        public static void EfetuarLog(Exception ex, string mensagem, string referenciaOrigem)
        {
            // Cria a string
            StringBuilder msg = new StringBuilder(ex.ToString());
            if (mensagem != null)
                msg.Append("\n--------\n" + mensagem);
            
            // Verifica se a inicialização já foi realizada
            if (!_inicializado)
                inicializar();

            // Acha a string da origem
            string origem = _config.NomeOrigemLogDefault;
            if (referenciaOrigem != null && _dicionarioOrigens.ContainsKey(referenciaOrigem))
                origem = _dicionarioOrigens[referenciaOrigem];

            // Faz o log no console
            if (_config.LogarEmConsole)
                Console.WriteLine(retornarCaractereInicio(LogTipoEnum.Erro) + " " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg.ToString());

            // Faz o log no event viewer
            if (_config.LogarEmEventViewer)
                EventLog.WriteEntry(origem, msg.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Efetua o log. Overload que faz o log de uma mensagem passando a origem default
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="tipo"></param>
        /// <param name="referenciaOrigem"></param>
        public static void EfetuarLog(string mensagem, LogTipoEnum tipo)
        {
            // Repassa a chamada
            EfetuarLog(mensagem, tipo, (string)null);
        }

        /// <summary>
        /// Efetua o log
        /// </summary>
        public static void EfetuarLog(string mensagem, LogTipoEnum tipo, string referenciaOrigem)
        {
            // Verifica se a inicialização já foi realizada
            if (!_inicializado)
                inicializar();

            // Acha a string da origem
            string origem = _config.NomeOrigemLogDefault;
            if (referenciaOrigem != null && _dicionarioOrigens.ContainsKey(referenciaOrigem))
                origem = _dicionarioOrigens[referenciaOrigem];

            // Faz o log no console
            if (_config.LogarEmConsole)
                Console.WriteLine(retornarCaractereInicio(tipo) + " " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + mensagem);

            // Faz o log no event viewer
            if (_config.LogarEmEventViewer)
                EventLog.WriteEntry(origem, mensagem, traduzirEventLogEntryType(tipo));
        }

        /// <summary>
        /// Faz a inicialização do log.
        /// </summary>
        private static void inicializar()
        {
            // Carrega arquivo de configuração
            _config = GerenciadorConfig.ReceberConfig<LogConfig>();
            if (_config == null)
                _config = new LogConfig();

            // Verifica se a origem default está criada no event viewer
            if (_config.LogarEmEventViewer && !EventLog.SourceExists(_config.NomeOrigemLogDefault))
                EventLog.CreateEventSource(_config.NomeOrigemLogDefault, "Application");

            // Carrega as origens do tipo informado, caso exista
            if (_config.TipoComOrigensDeLog != null)
                // Varre as constantes do tipo informado verificando a existencia do atributo de log
                foreach (FieldInfo member in _config.TipoComOrigensDeLog.GetFields())
                {
                    // Verifica existencia do atributo
                    object[] attrs = member.GetCustomAttributes(typeof(LogOrigemAttribute), false);
                    if (attrs.Length > 0)
                    {
                        // Pega o atributo
                        LogOrigemAttribute attr = (LogOrigemAttribute)attrs[0];

                        // Pega valor da origem
                        string origem = attr.Origem;

                        // Pega a referencia para a origem
                        string referenciaOrigem = 
                            (string)
                                _config.TipoComOrigensDeLog.InvokeMember(
                                    member.Name, BindingFlags.GetField, null, null, (object[])null);

                        // Se a origem não foi informada, utiliza a string da propria referencia
                        if (origem == null)
                            origem = referenciaOrigem;

                        // Garante que a origem esteja criada no event viewer
                        if (_config.LogarEmEventViewer && !EventLog.SourceExists(origem))
                            EventLog.CreateEventSource(origem, "Application");

                        // Adiciona no dicionário
                        _dicionarioOrigens.Add(referenciaOrigem, origem);
                    }
                }

            // Sinaliza
            _inicializado = true;
        }

        /// <summary>
        /// Retorna o caractere utilizado no inicio das mensagens do console
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private static string retornarCaractereInicio(LogTipoEnum tipo)
        {
            switch (tipo)
            {
                case LogTipoEnum.Passagem:
                    return "#";
                    break;
                case LogTipoEnum.Aviso:
                    return "!";
                    break;
                case LogTipoEnum.Erro:
                    return "@";
                    break;
                default:
                    return "?";
                    break;
            }
        }

        /// <summary>
        /// Faz a tradução do tipo proprietário para o EventLogEntryType para poder
        /// efetuar o log no event viewer corretamente.
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private static EventLogEntryType traduzirEventLogEntryType(LogTipoEnum tipo)
        {
            switch (tipo)
            {
                case LogTipoEnum.Aviso:
                    return EventLogEntryType.Warning;
                    break;
                case LogTipoEnum.Erro:
                    return EventLogEntryType.Error;
                    break;
                case LogTipoEnum.Passagem:
                    return EventLogEntryType.Information;
                    break;
                default:
                    return EventLogEntryType.Error;
                    break;
            }
        }
    }
}
