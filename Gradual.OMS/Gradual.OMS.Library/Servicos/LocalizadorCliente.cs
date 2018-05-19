using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Função semelhante ao do ativador mas apenas para o IServicoLocalizador.
    /// A rotina de ativação deste componente não consulta o localizador e sim 
    /// diretamente um arquivo de configuração.
    /// </summary>
    public static class LocalizadorCliente
    {
        private static IServicoLocalizador _servicoLocalizador = null;
        private static IChannelFactory<IServicoLocalizador> _canal = null;
        private static LocalizadorClienteConfig _config = null;

        public static void Inicializar(LocalizadorClienteConfig config)
        {
            _config = config;
        }

        public static IServicoLocalizador ServicoLocalizador
        {
            get 
            {
                try
                {
                    if (_servicoLocalizador == null)
                        Conectar();
                    return _servicoLocalizador;
                }
                catch (Exception ex)
                {
                    Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                    throw ex;
                }
            }
        }

        public static void Conectar()
        {
            try
            {
                // Inicializa
                bool local = true;

                // Verifica como o servico deve ser criado
                if (_config == null)
                    _config = GerenciadorConfig.ReceberConfig<LocalizadorClienteConfig>();
                if (_config != null && _config.AtivacaoTipo == ServicoAtivacaoTipo.WCF)
                    local = false;

                // Cria o servico
                if (local)
                {
                    // Cria local
                    _servicoLocalizador =
                        (IServicoLocalizador)
                            ServicoHostColecao.Default.ReceberServico<IServicoLocalizador>();
                }
                else
                {
                    // Cria via wcf
                    Binding binding =
                        (Binding)
                            typeof(BasicHttpBinding).Assembly.CreateInstance(
                                _config.EndPoint.NomeBindingType);
                    _canal = new ChannelFactory<IServicoLocalizador>(binding);
                    _servicoLocalizador =
                        _canal.CreateChannel(
                            new EndpointAddress(
                                _config.EndPoint.Endereco));
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public static void Desconectar()
        {
            if (_servicoLocalizador != null)
                _servicoLocalizador = null;
            if (_canal != null)
                _canal.Close();
        }

        public static ServicoInfo Consultar(Type servicoInterface)
        {
            try
            {
                return
                    LocalizadorCliente.ServicoLocalizador.Consultar(
                        servicoInterface.FullName);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public static ServicoInfo Consultar(Type servicoInterface, string id)
        {
            try
            {
                return
                    LocalizadorCliente.ServicoLocalizador.Consultar(
                        servicoInterface.FullName, id);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }


        public static void Registrar(ServicoInfo servico)
        {
            try
            {
                LocalizadorCliente.ServicoLocalizador.Registrar(servico);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public static void Remover(Type servicoInterface)
        {
            try
            {
                LocalizadorCliente.ServicoLocalizador.Remover(servicoInterface.FullName);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public static void Remover(Type servicoInterface, string id)
        {
            try
            {
                LocalizadorCliente.ServicoLocalizador.Remover(servicoInterface.FullName, id);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }
    }
}
