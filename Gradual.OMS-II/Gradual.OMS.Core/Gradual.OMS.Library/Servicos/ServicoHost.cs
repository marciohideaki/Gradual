using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using log4net;
using System.Collections;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// É o host de serviços.
    /// Lê arquivo de configuração, ou permite o cadastro manual de serviços.
    /// </summary>
    public class ServicoHost
    {
        public string IdConfigCarregado { get; set; }
        private Dictionary<string, Servico> _servicos = new Dictionary<string, Servico>();
        private Hashtable _srvsControlaveis = new Hashtable();

        private List<string> _baseAddress = new List<string>();
        private string _mexBaseAddress;

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServicoHost()
        {
        }

        public Dictionary<string, Servico> Servicos
        {
            get { return _servicos; }
        }

        public T ReceberServico<T>()
        {
            try
            {
                string key = typeof(T).FullName + "-";

                T obj = default(T);

                lock (this.Servicos)
                {
                    if (this.Servicos.ContainsKey(key))
                        obj = (T) this.Servicos[key].Instancia;
                }

                return obj;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ReceberServico(): ", ex);
                throw ex;
            }
        }

        public T ReceberServico<T>(string id)
        {
            try
            {
                if (id == null)
                    id = "";

                T obj = default(T);

                string key = typeof(T).FullName + "-" + id;

                lock (this.Servicos)
                {
                    if (this.Servicos.ContainsKey(key))
                        obj = (T) this.Servicos[key].Instancia;
                }

                return obj;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em ReceberServico<" + typeof(T) + ">(): ", ex);
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
                logger.Error("Erro em RegistrarServico(" + servicoInfo.NomeInterface + "): ", ex);
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
                foreach (string Interface in servico.ServicoInfo.NomeInterface)
                {
                    string nomeServico = Interface.Split(',')[0];
                    string key = nomeServico + "-" + id;
                    lock (this.Servicos)
                    {
                        if (this.Servicos.ContainsKey(key))
                            this.Servicos[key] = servico;
                        else
                            this.Servicos.Add(key, servico);
                    }
                }


                // Ativa o servico
                logger.Info("Ativando servico: " + servico.ServicoInfo.NomeInstancia);

                // Acrescenta os enderecos de enpoint da base mais os declarados para o servico
                if ( _baseAddress.Count > 0 )
                    servico.BaseAddress.AddRange(_baseAddress);

                if ( servicoInfo.WCFBaseAddress.Count > 0 )
                    servico.BaseAddress.AddRange(servicoInfo.WCFBaseAddress);

                servico.MexBaseAddress = _mexBaseAddress;

                // Efetua a ativacao do servico
                servico.Ativar();

                // Verifica se é servico controlavel
                IServicoControlavel controlavel = servico.Instancia as IServicoControlavel;
                if (controlavel != null)
                {
                    logger.Info("Servico " + servicoInfo.NomeInstancia + " é controlavel.");
                    if(!_srvsControlaveis.ContainsKey(servicoInfo.NomeInstancia))
                        _srvsControlaveis.Add(servicoInfo.NomeInstancia, controlavel);
                }

                // Retorna
                return servico;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em RegistrarServico(" + servicoInfo.NomeInterface + "): ", ex);
                throw ex;
            }
        }

        public void RemoverServico(Type servicoInterface)
        {
            try
            {
                Servico servico = null;
                string key = servicoInterface.FullName + "-";

                // Acha o servico
                // e remove da colecao
                lock (this.Servicos)
                {
                    if (this.Servicos.ContainsKey(key))
                    {
                        servico = this.Servicos[key];

                        // Desativa
                        if (servico != null)
                            servico.Desativar();

                        this.Servicos.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em RemoverServico(" + servicoInterface.FullName + "): ", ex);
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

                string key = servicoInterface.FullName + "-" + id;
                Servico servico = null;

                // Acha o servico
                lock (this.Servicos)
                {
                    if (this.Servicos.ContainsKey(key))
                    {
                        servico = this.Servicos[key];

                        // Desativa
                        if (servico != null)
                            servico.Desativar();

                        this.Servicos.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em RemoverServico(" + servicoInterface.FullName + "): ", ex);
                throw ex;
            }
        }

        

        /// <summary>
        /// Efetua a carga das configuracoes definidas na secao [Gradual.OMS.Library.Servicos.ServicoHostConfig-xxxx]
        /// onde xxxx é o id da secao a ser carregada (normalmente "Default");
        /// </summary>
        /// <param name="id">id da secao a ser carregada (normalmente "Default")</param>
        public void CarregarConfig(string id)
        {
            String lNomeServico;
            try
            {
                logger.Debug("CarregarConfig(): Carregando configuracoes para (" + id + ")");

                // Pára serviços que possam estar carregados
                this.PararServicos();

                // Limpa coleção de serviços
                _servicos.Clear();

                // Carrega os serviços do config
                this.IdConfigCarregado = id;
                ServicoHostConfig config =
                    GerenciadorConfig.ReceberConfig<ServicoHostConfig>(id);
                if (config != null)
                {
                    _baseAddress = config.BaseAddress;
                    _mexBaseAddress = config.MexBaseAddress;

                    logger.Info( "WCF Base Address: " + _baseAddress);
                    logger.Info( "MEX Base Address: " +  _mexBaseAddress);


                    foreach (ServicoInfo servicoInfo in config.Servicos)
                    {
                        if (servicoInfo.Habilitado)
                        {
                            lNomeServico = servicoInfo.NomeInstancia;
                            logger.Info("Registrando servico: [" + servicoInfo.NomeInstancia + "]");
                            this.RegistrarServico(servicoInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em CarregarConfig(): ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Invoca o metodo IniciarServico() em todas as instancias do tipo IServicoControlavel
        /// abrigadas por esse Hoster.
        /// </summary>
        public void IniciarServicos()
        {
            try
            {
                foreach (IServicoControlavel servicoControlavel in this._srvsControlaveis.Values)
                {
                    logger.Info("Iniciando Serviço: " + servicoControlavel.GetType().FullName );
                    servicoControlavel.IniciarServico();
                }

                logger.Info("Todos os serviços iniciados");
            }
            catch (Exception ex)
            {
                logger.Error("Erro em CarregarConfig(): ", ex);
                throw ex;
            }
        }

        public void PararServicos()
        {
            try
            {
                foreach (IServicoControlavel servicoControlavel in this._srvsControlaveis.Values)
                {
                    logger.Info("Parando Serviço: " + servicoControlavel.GetType().FullName);
                    servicoControlavel.PararServico();
                }

                logger.Info("*** Todos os serviços parados ***");
            }
            catch (Exception ex)
            {
                logger.Error("Erro em PararServicos(): ", ex);
                throw ex;
            }
        }
    }
}
