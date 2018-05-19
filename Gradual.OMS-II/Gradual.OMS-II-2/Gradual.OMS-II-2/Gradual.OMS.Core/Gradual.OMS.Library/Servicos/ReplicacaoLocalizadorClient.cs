using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Gradual.OMS.Library.Servicos
{
    class ReplicadorConn
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IServicoLocalizadorReplicacao Servico { get; set; }
        public bool Conectado { get; set; }
        public string Endereco { get; set; }

        public ReplicadorConn(string endereco)
        {
            Endereco = endereco;
        }

        public void Conectar()
        {
            if (Endereco == null || Endereco.Length == 0)
                return;

            try
            {
                // Cria via wcf
                Binding binding = Utilities.GetBinding(Endereco);

                IChannelFactory<IServicoLocalizadorReplicacao> canal = new ChannelFactory<IServicoLocalizadorReplicacao>(binding);
                Servico = canal.CreateChannel(new EndpointAddress(Endereco));

                ((IContextChannel)Servico).OperationTimeout = new TimeSpan(0, 10, 0);

                Conectado = true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Conectar(replicadores): ", ex);
            }
        }

        public void Desconectar()
        {
            try
            {
                Ativador.AbortChannel(Servico);
            }
            catch (Exception ex)
            {
                logger.Error("Erro em Desconectar(replicador): ", ex);
            }
            finally
            {
                Conectado = false;
            }

        }
    }

    public class ReplicacaoLocalizadorClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReplicacaoLocalizadorConfig _config;
        private List<ReplicadorConn> replicadores = new List<ReplicadorConn>();
        private bool bConectado = false;

        public int QtdeReplicadores
        {
            get { return replicadores.Count; }
        }

        public ReplicacaoLocalizadorClient()
        {
            _config = GerenciadorConfig.ReceberConfig<ReplicacaoLocalizadorConfig>();

            if (_config == null)
            {
                logger.Warn("Erro ao obter configuracao da replicacao dos localizadores. Verifique a secao <ReplicacaoLocalizadorConfig>");
            }
        }


        public void Conectar()
        {
            if (_config == null)
            {
                logger.Warn("Conectar(): Não ha replicadores configurados");
                return;
            }

            // Efetua conexao com 
            foreach (string endereco in _config.Replicadores )
            {
                ReplicadorConn replicador = new ReplicadorConn(endereco);
                replicador.Conectar();
                replicadores.Add(replicador);
            }
        }

        public void Desconectar()
        {
            foreach (ReplicadorConn replicador in replicadores)
            {
                replicador.Desconectar();
            }

            replicadores.Clear();
        }

        /// <summary>
        /// Registra um servico
        /// </summary>
        /// <param name="servico">objeto ServicoInfo</param>
        public void ReplicarRegistro(ServicoInfo servico)
        {
            foreach ( ReplicadorConn replicador in replicadores  )
            {
                try
                {
                    if (!replicador.Conectado)
                        replicador.Conectar();

                    replicador.Servico.ReplicarRegistro(servico);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em Registrar(): " + ex.Message, ex);
                    replicador.Desconectar();
                }
            }
        }


        /// <summary>
        /// Remove um servico da lista de servicos
        /// </summary>
        /// <param name="servicoInterface">Interface exposta pelo servico</param>
        public void ReplicarRemocao(string servicoInterface)
        {
            foreach (ReplicadorConn replicador in replicadores)
            {
                try
                {
                    if (!replicador.Conectado)
                        replicador.Conectar();

                    replicador.Servico.ReplicarRemocao(servicoInterface);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em Remover(" +  servicoInterface + "): " + ex.Message, ex);
                    replicador.Desconectar();
                }
            }
        }


        /// <summary>
        /// Remove um servico da lista de servicos
        /// </summary>
        /// <param name="servicoInterface">Interface exposta pelo servico</param>
        /// <param name="id">ID</param>
        public void ReplicarRemocaoID(string servicoInterface, string id)
        {
            foreach (ReplicadorConn replicador in replicadores)
            {
                try
                {
                    if (!replicador.Conectado)
                        replicador.Conectar();

                    replicador.Servico.ReplicarRemocaoID(servicoInterface, id);
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em Remover(" + servicoInterface + "," + id + "): " + ex.Message, ex);
                    replicador.Desconectar();
                }
            }
        }

        /// <summary>
        /// Retorna a lista de servicos do primeiro servico localizador disponivel
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, ServicoInfo>> ObterListaServicos()
        {
            foreach (ReplicadorConn replicador in replicadores)
            {
                try
                {
                    if (!replicador.Conectado)
                        replicador.Conectar();

                    return replicador.Servico.ObterListaServicos();
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em ObterListaServicos(): " + ex.Message, ex);
                    replicador.Desconectar();
                }
            }

            return null;
        }
    }
}
