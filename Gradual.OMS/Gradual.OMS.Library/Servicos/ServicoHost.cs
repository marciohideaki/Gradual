using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// É o host de serviços.
    /// Lê arquivo de configuração, ou permite o cadastro manual de serviços.
    /// </summary>
    public class ServicoHost
    {
        public ServicoHost()
        {
        }

        private Dictionary<string, Servico> _servicos = new Dictionary<string, Servico>();
        public Dictionary<string, Servico> Servicos
        {
            get { return _servicos; }
        }

        public T ReceberServico<T>()
        {
            try
            {
                string id = typeof(T).FullName + "-";
                if (this.Servicos.ContainsKey(id))
                    return (T)this.Servicos[id].Instancia;
                else
                    return default(T);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public T ReceberServico<T>(string id)
        {
            try
            {
                if (id == null)
                    id = "";
                return (T)this.Servicos[typeof(T).FullName + "-" + id].Instancia;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public Servico RegistrarServico(ServicoInfo servicoInfo)
        {
            try
            {
                return RegistrarServico(servicoInfo, null);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public Servico RegistrarServico(ServicoInfo servicoInfo, object instancia)
        {
            try
            {
                // Cria servico
                Servico servico = new Servico();
                servico.ServicoInfo = servicoInfo;
                servico.Instancia = instancia;

                // Seta o id do servico
                if (servico.ServicoInfo.ID != null)
                    ((IServicoID)instancia).SetarID(servico.ServicoInfo.ID);

                // Adiciona
                string id = servico.ServicoInfo.ID == null ? "" : servico.ServicoInfo.ID;
                string nomeServico = servico.ServicoInfo.NomeInterface.Split(',')[0];
                this.Servicos.Add(
                    nomeServico + "-" + id, servico);

                // Ativa o servico
                servico.Ativar();

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public void RemoverServico(Type servicoInterface)
        {
            try
            {
                // Acha o servico
                Servico servico = this.Servicos[servicoInterface.FullName + "-"];

                // Desativa
                servico.Desativar();

                // Remove da colecao
                this.Servicos.Remove(servicoInterface.FullName + "-");
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public void RemoverServico(Type servicoInterface, string id)
        {
            try
            {
                // Ajusta id
                if (id == null)
                    id = "";

                // Acha o servico
                Servico servico = this.Servicos[servicoInterface.FullName + "-" + id];

                // Desativa
                servico.Desativar();

                // Remove da colecao
                this.Servicos.Remove(servicoInterface.FullName + "-" + id);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public string IdConfigCarregado { get; set; }
        
        public void CarregarConfig(string id)
        {
            try
            {
                // Pára serviços que possam estar carregados
                this.PararServicos();

                // Limpa coleção de serviços
                _servicos.Clear();

                // Carrega os serviços do config
                this.IdConfigCarregado = id;
                ServicoHostConfig config =
                    GerenciadorConfig.ReceberConfig<ServicoHostConfig>(id);
                if (config != null)
                    foreach (ServicoInfo servicoInfo in config.Servicos)
                        if (servicoInfo.Habilitado)
                            this.RegistrarServico(servicoInfo);
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public void IniciarServicos()
        {
            try
            {
                foreach (KeyValuePair<string, Servico> item in this.Servicos)
                {
                    IServicoControlavel servicoControlavel = item.Value.Instancia as IServicoControlavel;
                    if (servicoControlavel != null)
                    {
                        Log.EfetuarLog("Iniciando Serviço: " + servicoControlavel.GetType().FullName, LogTipoEnum.Passagem, "OMS.Library.Servicos");
                        servicoControlavel.IniciarServico();
                    }
                }
                Log.EfetuarLog("Todos os serviços iniciados", LogTipoEnum.Passagem, "OMS.Library.Servicos");
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }

        public void PararServicos()
        {
            try
            {
                foreach (KeyValuePair<string, Servico> item in this.Servicos)
                {
                    IServicoControlavel servicoControlavel = item.Value.Instancia as IServicoControlavel;
                    if (servicoControlavel != null)
                    {
                        Log.EfetuarLog("Parando Serviço: " + servicoControlavel.GetType().FullName, LogTipoEnum.Passagem, "OMS.Library.Servicos");
                        servicoControlavel.PararServico();
                    }
                }
                Log.EfetuarLog("Todos os serviços parados", LogTipoEnum.Passagem, "OMS.Library.Servicos");
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, null, "OMS.Library.Servicos");
                throw ex;
            }
        }
    }
}
