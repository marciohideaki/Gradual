using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Implementação do serviço de localização (interface IServicoLocalizador)
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoLocalizador : IServicoLocalizador
    {
        private Dictionary<string, Dictionary<string, ServicoInfo>> _servicos = 
            new Dictionary<string, Dictionary<string, ServicoInfo>>();
        
        #region IServicoLocalizador Members

        List<ServicoInfo> IServicoLocalizador.Consultar()
        {
            try
            {
                List<ServicoInfo> lista = new List<ServicoInfo>();
                foreach (KeyValuePair<string, Dictionary<string, ServicoInfo>> item1 in _servicos)
                    foreach (KeyValuePair<string, ServicoInfo> item2 in item1.Value)
                        lista.Add(item2.Value);
                return lista;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        ServicoInfo IServicoLocalizador.Consultar(string servicoInterface)
        {
            try
            {
                // Verifica se o seviço solicitado consta na lista
                if (!_servicos.ContainsKey(servicoInterface))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") não consta na lista de serviços. " +
                        "Isto indica que o serviço em questão não fez o registro no serviço de localização. " +
                        "Verifique o processo que inicia este serviço;";
                    Log.EfetuarLog(msg, LogTipoEnum.Aviso);
                    
                    // Retorna
                    return null;
                }
                else if (!_servicos[servicoInterface].ContainsKey(""))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") consta na lista de serviços mas não " +
                        "possui a instância com Id '' (default).";
                    Log.EfetuarLog(msg, LogTipoEnum.Aviso);

                    // Retorna
                    return null;
                }

                // Retorna o serviço solicitado
                return _servicos[servicoInterface][""];
            }
            catch (Exception ex)
            {
                Log.EfetuarLog("servicoInterface: " + servicoInterface + "; " + ex.ToString(), LogTipoEnum.Erro, "OMS.Library.Servicos");
                throw ex;
            }
        }

        ServicoInfo IServicoLocalizador.Consultar(string servicoInterface, string id)
        {
            try
            {
                // Verifica se o seviço solicitado consta na lista
                if (!_servicos.ContainsKey(servicoInterface))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") não consta na lista de serviços. " +
                        "Isto indica que o serviço em questão não fez o registro no serviço de localização. " +
                        "Verifique o processo que inicia este serviço;";
                    Log.EfetuarLog(msg, LogTipoEnum.Aviso);
                }
                else if (!_servicos[servicoInterface].ContainsKey(""))
                {
                    // Informa
                    string msg = "O serviço solicitado (interface: " + servicoInterface + ") consta na lista de serviços mas não " +
                        "possui a instância com Id '' (default).";
                    Log.EfetuarLog(msg, LogTipoEnum.Aviso);
                }

                // Retorna
                return _servicos[servicoInterface][id];
            }
            catch (Exception ex)
            {
                Log.EfetuarLog("servicoInterface: " + servicoInterface + "; id: " + id + "; " + ex.ToString(), LogTipoEnum.Erro, "OMS.Library.Servicos");
                throw ex;
            }
        }

        void IServicoLocalizador.Registrar(ServicoInfo servico)
        {
            try
            {
                lock (_servicos)
                {
                    string nomeServico = servico.NomeInterface.Split(',')[0];

                    string id = servico.ID == null ? "" : servico.ID;
                    if (!_servicos.ContainsKey(nomeServico))
                        _servicos.Add(nomeServico, new Dictionary<string, ServicoInfo>());
                    if (_servicos[nomeServico].ContainsKey(id))
                        _servicos[nomeServico][id] = servico;
                    else
                        _servicos[nomeServico].Add(id, servico);

                    // Faz o log do registro
                    Log.EfetuarLog("Serviço registrado: " + Serializador.TransformarEmString(servico), LogTipoEnum.Passagem, "OMS.Library.Servicos");
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        void IServicoLocalizador.Remover(string servicoInterface)
        {
            try
            {
                lock (_servicos)
                    _servicos[servicoInterface].Remove("");
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        void IServicoLocalizador.Remover(string servicoInterface, string id)
        {
            try
            {
                lock (_servicos)
                    _servicos[servicoInterface].Remove(id);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        #endregion
    }
}
