using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public class ServicoInterfaceDesktop : IServicoInterfaceDesktop
    {
        #region Construtores

        public ServicoInterfaceDesktop()
        {
            // Cria as coleções
            this.Desktops = new List<DesktopInfo>();
            this.Janelas = new List<JanelaInfo>();
            this.Hosts = new List<Host>();
            this.Contexto = new ColecaoTipoInstancia();
            this.ParametrosDefault = new Dictionary<string, object>();
        }

        #endregion

        #region IServicoInterfaceHost Members

        public List<DesktopInfo> Desktops { get; set; }
        public List<JanelaInfo> Janelas { get; set; }
        public List<Host> Hosts { get; set; }
        public JanelaInfo Launcher { get; set; }
        public DesktopInfo DesktopAtivo { get; set; }
        public ColecaoTipoInstancia Contexto { get; set; }
        public Dictionary<string, object> ParametrosDefault { get; set; }

        public void IniciarLauncher()
        {
            // Cria janelaInfo
            this.Launcher = 
                new JanelaInfo()
                {
                    ItemTipo = ItemTipoEnum.JanelaLauncher
                };

            // Cria um host para o launcher
            IServicoInterfaceDesktopHost servicoInterfaceHost = new ServicoInterfaceDesktopHost();
            HostInfo hostLauncherInfo = new HostInfo() { InicializacaoTipo = HostInicializacaoTipoEnum.Normal };
            Host hostLauncher = new Host(hostLauncherInfo, servicoInterfaceHost);
            this.Hosts.Add(hostLauncher);
            
            // Cria a janela 
            hostLauncher.Instancia.CriarJanela(this.Launcher);
        }

        public void CriarJanela(JanelaInfo janelaInfo)
        {
            // Repassa a chamada
            this.CriarJanela(janelaInfo, null);
        }
        
        public void CriarJanela(JanelaInfo janelaInfo, object parametros)
        {
            // Se nao informou o desktop, usa o desktop ativo
            if (janelaInfo.IdDesktop == null)
                janelaInfo.IdDesktop = this.DesktopAtivo.Id;

            // Usa regra para identificar o host
            Host host = this.ReceberHostPorRegra();
            janelaInfo.IdHost = host.Info.Id;

            // Pede criação da janela no host
            host.Instancia.CriarJanela(janelaInfo, parametros);

            // Adiciona janela na coleção de janelas
            this.Janelas.Add(janelaInfo);

            // Adiciona janela na coleção de janelas do desktop
            DesktopInfo desktopInfo = this.ReceberDesktopInfo(janelaInfo.IdDesktop);

            // Se a janela faz parte do desktop ativo, mostra a janela
            if (this.DesktopAtivo != null && janelaInfo.IdDesktop == this.DesktopAtivo.Id)
                host.Instancia.MostrarJanela(janelaInfo.Id);

            // Avisa criação da janela
            this.SinalizarJanelaAbrindo(janelaInfo);
        }

        public void RemoverJanela(string idJanela)
        {
            // Pega janelaInfo
            JanelaInfo janelaInfo = this.ReceberJanelaInfo(idJanela);

            // Repassa a chamada para o host correspondente
            this.ReceberHost(janelaInfo.IdHost).Instancia.RemoverJanela(idJanela);

            // Remove da lista de janelas
            this.Janelas.Remove(janelaInfo);
        }

        public void AtivarJanela(string idJanela)
        {
            // Pega janelaInfo
            JanelaInfo janelaInfo = this.ReceberJanelaInfo(idJanela);

            // Repassa a chamada para o host correspondente
            this.ReceberHost(janelaInfo.IdHost).Instancia.AtivarJanela(idJanela);
        }

        public void FecharJanela(string idJanela)
        {
            // Pega janelaInfo
            JanelaInfo janelaInfo = this.ReceberJanelaInfo(idJanela);

            // Repassa a chamada para o host correspondente
            this.ReceberHost(janelaInfo.IdHost).Instancia.FecharJanela(idJanela);
        }

        public void AdicionarControle(string idJanela, ControleInfo controleInfo)
        {
            // Acha a janela para achar o host
            JanelaInfo janelaInfo = this.ReceberJanelaInfo(idJanela);

            // Verifica se este controle possui parametros default
            object parametros = (from p in this.ParametrosDefault
                                 where controleInfo.TipoInstanciaString.StartsWith(p.Key)
                                 select p.Value).FirstOrDefault();

            // Pede o registro do controle para o host
            this.ReceberHost(janelaInfo.IdHost).Instancia.AdicionarControle(idJanela, controleInfo, parametros);
        }

        public void MostrarDesktop(string idDesktop)
        {
            // Esconde desktop atual
            if (this.DesktopAtivo != null)
                EsconderDesktopAtivo();

            // Acha a lista de janelas do desktop solicitado
            var janelas = from j in this.Janelas
                          where j.IdDesktop == idDesktop
                          select j;

            // Mostra as janelas do desktop
            foreach (JanelaInfo janelaInfo in janelas)
                this.ReceberHost(janelaInfo.IdHost).Instancia.MostrarJanela(janelaInfo.Id);

            // Informa desktop ativo
            this.DesktopAtivo = this.ReceberDesktopInfo(idDesktop);
        }

        public void EsconderDesktopAtivo()
        {
            // Se tem desktop ativo, esconde todas as janelas
            if (this.DesktopAtivo != null)
            {
                var janelas = from j in this.Janelas
                              where j.IdDesktop == this.DesktopAtivo.Id
                              select j;
                foreach (JanelaInfo janelaInfo in janelas)
                    this.ReceberHost(janelaInfo.IdHost).Instancia.EsconderJanela(janelaInfo.Id);
            }

            // Informa nenhum desktop ativo
            this.DesktopAtivo = null;
        }

        public void SalvarConfiguracoes()
        {
            // Le nome do arquivo nas configuracoes
            ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
            SalvarConfiguracoes(config.ArquivoConfiguracoes);
        }

        public void SalvarConfiguracoes(string nomeArquivo)
        {
            // Inicializa
            InterfaceSerializacaoInfo interfaceSerializacao = new InterfaceSerializacaoInfo();
            if (this.DesktopAtivo != null)
                interfaceSerializacao.IdDesktopDefault = this.DesktopAtivo.Id;

            // Salva os desktops
            interfaceSerializacao.Desktops = this.Desktops;

            // Salva o launcher
            interfaceSerializacao.JanelaLauncher =
                this.ReceberHost(this.Launcher.IdHost).Instancia.SerializarJanela(this.Launcher.Id);

            // Varre todas as janelas pedindo para serializar
            foreach (JanelaInfo janelaInfo in this.Janelas)
                interfaceSerializacao.Janelas.Add(
                    this.ReceberHost(janelaInfo.IdHost).Instancia.SerializarJanela(janelaInfo.Id));

            // Serializa para o arquivo
            BinaryFormatter formatter = new BinaryFormatter();

            FileInfo lTempFile = new FileInfo(nomeArquivo);

            if (!Directory.Exists(lTempFile.DirectoryName))
            {
                Directory.CreateDirectory(lTempFile.DirectoryName);
            }

            FileStream fs = new FileStream(nomeArquivo + ".tmp", FileMode.Create);
            formatter.Serialize(fs, interfaceSerializacao);
            fs.Close();

            // Se deu tudo certo, copia para o arquivo original
            File.Copy(nomeArquivo + ".tmp", nomeArquivo, true);
        }

        public void SalvarConfiguracoesDefault()
        {
            // Le nome do arquivo nas configuracoes
            ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
            SalvarConfiguracoesDefault(config.ArquivoConfiguracoesDefault);
        }

        public void SalvarConfiguracoesDefault(string nomeArquivo)
        {
            // Serializa para o arquivo
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(nomeArquivo + ".tmp", FileMode.Create);
            formatter.Serialize(fs, this.ParametrosDefault);
            fs.Close();

            // Se deu tudo certo, copia para o arquivo original
            File.Copy(nomeArquivo + ".tmp", nomeArquivo, true);
        }

        public void CarregarConfiguracoes()
        {
            // Le nome do arquivo nas configuracoes
            ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
            CarregarConfiguracoes(config.ArquivoConfiguracoes);
        }

        public void CarregarConfiguracoes(string nomeArquivo)
        {
            // Apenas se o arquivo existir
            if (!File.Exists(nomeArquivo))
                return;
            
            // Remove todas as janelas abertas
            foreach (JanelaInfo janelaInfo in this.Janelas.ToArray())
                this.FecharJanela(janelaInfo.Id);

            // Abre o arquivo e desserializa o objeto
            FileStream fs = File.Open(nomeArquivo, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            InterfaceSerializacaoInfo interfaceSerializacao = 
                (InterfaceSerializacaoInfo)formatter.Deserialize(fs);
            fs.Close();

            // Carrega os desktops
            this.Desktops.Clear();
            this.Desktops.AddRange(interfaceSerializacao.Desktops);

            // Acha o host do launcher e pede para a janela carregar parametros
            this.ReceberHost(this.Launcher.IdHost).Instancia.CarregarParametrosDeJanela(
                this.Launcher.Id,
                interfaceSerializacao.JanelaLauncher.JanelaParametros.Objeto);

            // Carrega as janelas, aqui apenas decide qual host irá criar a janela
            foreach (JanelaSerializacaoInfo janelaSerializacao in interfaceSerializacao.Janelas)
                this.Janelas.Add(
                    this.ReceberHostPorRegra().Instancia.DesserializarJanela(janelaSerializacao));

            // Mostra o desktop default
            if (interfaceSerializacao.IdDesktopDefault != null)
                this.MostrarDesktop(interfaceSerializacao.IdDesktopDefault);
        }

        public void CarregarConfiguracoesDefault()
        {
            // Le nome do arquivo nas configuracoes
            ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
            CarregarConfiguracoesDefault(config.ArquivoConfiguracoesDefault);
        }

        public void CarregarConfiguracoesDefault(string nomeArquivo)
        {
            // Apenas se o arquivo existir
            if (!File.Exists(nomeArquivo))
                return;

            // Abre o arquivo e desserializa o objeto
            FileStream fs = File.Open(nomeArquivo, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            this.ParametrosDefault =
                (Dictionary<string, object>)formatter.Deserialize(fs);
            fs.Close();
        }

        public void RemoverDesktop(string idDesktop)
        {
            // Acha o desktop
            DesktopInfo desktopInfo = this.ReceberDesktopInfo(idDesktop);
            
            // Varre todas as janelas fechando as que pertencem ao desktop
            foreach (JanelaInfo janelaInfo in this.Janelas.ToArray())
                if (janelaInfo.IdDesktop == idDesktop)
                    this.FecharJanela(janelaInfo.Id);

            // Remove o desktop da lista
            this.Desktops.Remove(desktopInfo);
        }

        public object ReceberJanelaLauncher()
        {
            // Retorna instancia do launcher
            return this.ReceberHost(this.Launcher.IdHost).Instancia.ReceberJanelaInstancia(this.Launcher.Id);
        }

        public void SinalizarJanelaAbrindo(JanelaInfo janelaInfo)
        {
            if (EventoJanelaAbrindo != null)
                EventoJanelaAbrindo(this, new EventoJanelaEventArgs() { JanelaInfo = janelaInfo });
        }

        public void SinalizarJanelaFechando(JanelaInfo janelaInfo)
        {
            if (EventoJanelaFechando != null)
                EventoJanelaFechando(this, new EventoJanelaEventArgs() { JanelaInfo = janelaInfo });
        }

        public event EventHandler<EventoJanelaEventArgs> EventoJanelaAbrindo;
        public event EventHandler<EventoJanelaEventArgs> EventoJanelaFechando;

        /// <summary>
        /// Envia mensagem para controle
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public EnviarMensagemParaControleResponse EnviarMensagemParaControle(EnviarMensagemParaControleRequest parametros)
        {
            // Inicializa
            List<ConsultaControlesHelper> controles = new List<ConsultaControlesHelper>();
            MensagemInterfaceResponseBase mensagemResposta = null;

            // Monta lista de controles de todos os hosts
            foreach (Host host in this.Hosts)
                controles.AddRange(host.Instancia.ListarControles());

            // Tenta localizar o controle
            ConsultaControlesHelper controleHelper = null;
            if (parametros.IdControle != null)
            {
                controleHelper =
                    (from c in controles
                     where c.Controle.Id == parametros.IdControle
                     select c).FirstOrDefault();
            }
            else
            {
                if (parametros.IdDesktop == null)
                    parametros.IdDesktop = this.DesktopAtivo.Id;
                controleHelper =
                    (from c in controles
                     where c.Controle.TipoInstancia == parametros.ControleTipo &&
                           c.Janela.IdDesktop == parametros.IdDesktop
                     select c).FirstOrDefault();
            }

            // Se nao encontrou, verifica se deve criar
            if (controleHelper == null && parametros.CriarCasoNaoEncontrado)
            {
                // Cria nova janela
                JanelaInfo janelaInfo = new JanelaInfo();
                this.CriarJanela(janelaInfo);

                // Adiciona o controle
                ControleInfo controleInfo = new ControleInfo()
                {
                    TipoInstancia = parametros.ControleTipo,
                    Titulo = parametros.TituloJanela
                };
                this.AdicionarControle(janelaInfo.Id, controleInfo);

                // Cria o helper
                controleHelper = 
                    new ConsultaControlesHelper() { 
                        Controle = controleInfo, 
                        Janela = janelaInfo, 
                        Host = this.ReceberHost(janelaInfo.IdHost).Info 
                    };
            }

            // Se tem o controle, repassa a mensagem
            if (controleHelper != null)
                mensagemResposta = 
                    this.ReceberHost(controleHelper.Host.Id).Instancia.EnviarMensagemParaControle(
                        parametros.MensagemRequest, controleHelper.Controle.Id);

            // Retorna
            return
                new EnviarMensagemParaControleResponse()
                {
                    MensagemResposta = mensagemResposta
                };
        }

        #endregion

        #region Rotinas Auxiliares

        public Host ReceberHostPorRegra()
        {
            if (this.Hosts.Count == 0)
            {
                // Cria o info
                HostInfo hostInfo = new HostInfo()
                {
                    Id = Guid.NewGuid().ToString()
                };

                // Cria o host
                Host host = new Host(hostInfo);

                // Adiciona na colecao
                this.Hosts.Add(host);
            }

            // Retorna
            return this.Hosts.First();
        }

        public JanelaInfo ReceberJanelaInfo(string idJanela)
        {
            // Faz a consulta
            JanelaInfo janela = (from j in this.Janelas
                                 where j.Id == idJanela
                                 select j).FirstOrDefault();
            // Retorna
            return janela;
        }

        public Host ReceberHost(string idHost)
        {
            // Faz a consulta
            Host host = (from h in this.Hosts
                         where h.Info.Id == idHost
                         select h).FirstOrDefault();
            // Retorna
            return host;
        }

        public DesktopInfo ReceberDesktopInfo(string idDesktop)
        {
            // Adiciona na coleção do desktop correspondente
            DesktopInfo desktop = (from d in this.Desktops
                                   where d.Id == idDesktop
                                   select d).FirstOrDefault();

            // Se o desktop não existir, cria
            if (desktop == null)
            {
                desktop = new DesktopInfo()
                {
                    Id = idDesktop
                };
                this.Desktops.Add(desktop);
            }

            // Retorna
            return desktop;
        }

        #endregion
    }
}

