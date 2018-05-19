using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using log4net;
using MdsBayeuxClient;

using Gradual.Core.OMS.SmartTrader.Dados;
using Gradual.Core.OMS.SmartTrader.Lib.Dados;
namespace Gradual.Core.OMS.SmartTrader.Streamer
{
    public class SmartMonitor
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        Dictionary<string, SmartInstrument> _dicSymbols; // lista de instrumentos monitorados com respectivos parametros

        #endregion

        // Constructor / Destructor
        public SmartMonitor()
        {
            _dicSymbols = null; 

            
        }

        ~SmartMonitor()
        {
            
        }

        /// <summary>
        /// Iniciar o monitor (adicionado no metodo start para possiveis controles de threads)
        /// </summary>
        public void Start()
        {
            try
            {
                _dicSymbols = new Dictionary<string, SmartInstrument>();

                // Conectar ao streamer
                if (!this.ConnectToStreamer())
                {
                    logger.Info("Nao foi possivel conectar ao servidor Streamer");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao iniciar o SmartMonitor: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Parada do monitor
        /// </summary>
        public void Stop()
        {
            try
            {
                // Desconectar ao streamer
                if (!this.DisconnectStreamer())
                    logger.Info("Nao foi possivel desconectar o Streamer!");
                else
                    logger.Info("Streamer desconectado!");

                // TODO [FF]: Fazer desalocacao correta dos objetos de dentro do dicionario
                // (desassinar o evento e desalocar objetos)
                if (null != _dicSymbols)
                {
                    _dicSymbols.Clear();
                    _dicSymbols = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro na parada do SmartMonitor: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Efetuar instanciacao e conectar
        /// </summary>
        private bool ConnectToStreamer()
        {
            try
            {
                if (!MdsHttpClient.Conectado)
                {
                    MdsHttpClient.Instance.Conecta(ConfigurationManager.AppSettings["StreamerDeCotacao"]);
                }
                return MdsHttpClient.Conectado;
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel conectar ao streamer: " + ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Efetuar desconexao 
        /// </summary>
        /// <returns></returns>
        private bool DisconnectStreamer()
        {
            try
            {
                logger.Info("Desconectando streamer...");
                if (MdsHttpClient.Conectado)
                {
                    
                    MdsHttpClient.Instance.Desconecta();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel desconectar ao streamer: " + ex.Message, ex);
                return false;
            }


        }
        /// <summary>
        /// Adicao do instrumento na colecao
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public bool AddInstrument(string instrument, OrdemSmart parametros)
        {
            try
            {
                lock (_dicSymbols)
                {
                    SmartInstrument aux = null;
                    if (_dicSymbols.TryGetValue(instrument, out aux))
                    {
                        aux.Instrument = instrument;
                        aux.Order = parametros;
                    }
                    else
                    {
                        SmartInstrument smtInstr = new SmartInstrument(instrument, parametros);
                        _dicSymbols.Add(instrument, smtInstr);
                    }
                }
                // TODO [FF] - Montar a interface com streamer para assinar o ativo e evento
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Problemas na adicao do instrumento {0}: {1}", instrument, ex.Message), ex);
                return false;
            }
        }
        /// <summary>
        /// Remover o instrumento
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public bool RemoveInstrument(string instrument)
        {
            try
            {
                bool ret = false;
                lock (_dicSymbols)
                {
                    SmartInstrument aux = null;
                    if (_dicSymbols.TryGetValue(instrument, out aux))
                    {
                        // TODO [FF] - unassign event handler
                        //aux.OnNegociosHandler -=
                        aux = null;
                        ret = _dicSymbols.Remove(instrument);
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Problemas na remocao do instrumento {0}: {1}", instrument, ex.Message), ex);
                return false;
            }
        }

    }
}
