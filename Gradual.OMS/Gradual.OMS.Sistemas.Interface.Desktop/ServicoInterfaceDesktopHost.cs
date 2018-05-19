using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    [Serializable]
    public class ServicoInterfaceDesktopHost : IServicoInterfaceDesktopHost
    {
        public HostInfo HostInfo { get; set; }

        public List<Janela> Janelas { get; set; }

        public ServicoInterfaceDesktopHost()
        {
            this.Janelas = new List<Janela>();
        }

        #region IServicoInterfaceHost Members

        public void CriarJanela(JanelaInfo janelaInfo)
        {
            // Repassa a chamada
            this.CriarJanela(janelaInfo, null);
        }
        
        public void CriarJanela(JanelaInfo janelaInfo, object parametros)
        {
            // Cria a janela e pede a inicializacao
            Janela janela = new Janela(janelaInfo);
            janelaInfo.IdHost = this.HostInfo.Id;

            // Pede inicializacao da janela
            if (parametros != null)
                janela.Instancia.CarregarParametros(parametros, EventoManipulacaoParametrosEnum.Persistencia);

            // Adiciona na coleção de janelas do host
            this.Janelas.Add(janela);
        }

        public void FecharJanela(string idJanela)
        {
            // Acha a janela e pede para fechar
            Janela janela = this.ReceberJanela(idJanela);
            janela.Instancia.Fechar();
        }

        public void RemoverJanela(string idJanela)
        {
            // Acha a janela
            Janela janela = this.ReceberJanela(idJanela);

            // Remove da colecao
            this.Janelas.Remove(janela);
        }

        public void AtivarJanela(string idJanela)
        {
            // Acha a janela e pede para ativar
            Janela janela = this.ReceberJanela(idJanela);
            janela.Instancia.Ativar();
        }

        public void AdicionarControle(string idJanela, ControleInfo controleInfo, object parametros)
        {
            // Pede para a janela executar a ação de registrar o controle
            this.ReceberJanela(idJanela).AdicionarControle(controleInfo, parametros);
        }

        public void Inicializar(HostInfo hostInfo)
        {
            // Cria o host
            this.HostInfo = hostInfo;
        }

        public void EsconderJanela(string idJanela)
        {
            this.ReceberJanela(idJanela).EsconderJanela();
        }

        public void MostrarJanela(string idJanela)
        {
            this.ReceberJanela(idJanela).MostrarJanela();
        }

        public JanelaSerializacaoInfo SerializarJanela(string idJanela)
        {
            // Pega a janela
            Janela janela = ReceberJanela(idJanela);

            // Cria info da serializacao
            JanelaSerializacaoInfo serializacaoInfo = new JanelaSerializacaoInfo();

            // Salva informacoes da janela
            serializacaoInfo.JanelaInfo = janela.Info;
            serializacaoInfo.JanelaParametros = new ObjetoSerializado(janela.Instancia.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia));
            
            // Salva informacoes dos controles
            foreach (Controle controle in janela.Controles)
                serializacaoInfo.Controles.Add(
                    new ControleSerializacaoInfo() 
                    { 
                        ControleInfo = controle.Info,
                        ControleParametros = new ObjetoSerializado(controle.Instancia.SalvarParametros(EventoManipulacaoParametrosEnum.Persistencia))
                    });

            // Retorna
            return serializacaoInfo;
        }

        public JanelaInfo DesserializarJanela(JanelaSerializacaoInfo parametros)
        {
            // Seta o id do host
            parametros.JanelaInfo.IdHost = this.HostInfo.Id;

            // Cria a janela
            Janela janela = new Janela(parametros.JanelaInfo);
            if (parametros.JanelaParametros != null)
                janela.Instancia.CarregarParametros(
                    parametros.JanelaParametros.Objeto, EventoManipulacaoParametrosEnum.Persistencia);
            this.Janelas.Add(janela);

            // Registra os controles
            foreach (ControleSerializacaoInfo controleSerializacao in parametros.Controles)
                janela.AdicionarControle(
                    controleSerializacao.ControleInfo, 
                    controleSerializacao.ControleParametros.Objeto);

            // Retorna
            return parametros.JanelaInfo;
        }

        public void RegistrarJanela(Janela janela)
        {
            this.Janelas.Add(janela);
        }

        public void CarregarParametrosDeJanela(string idJanela, object parametros)
        {
            if (parametros != null)
                this.ReceberJanela(idJanela).Instancia.CarregarParametros(parametros, EventoManipulacaoParametrosEnum.Persistencia);
        }

        public object ReceberJanelaInstancia(string idJanela)
        {
            return this.ReceberJanela(idJanela).Instancia;
        }

        public List<ConsultaControlesHelper> ListarControles()
        {
            // Retorno
            List<ConsultaControlesHelper> controles = 
                new List<ConsultaControlesHelper>();
            
            // Varre as janelas adicionando os controles
            foreach (Janela janela in this.Janelas)
                foreach (Controle controle in janela.Controles)
                    controles.Add(
                        new ConsultaControlesHelper() 
                        { 
                            Controle = controle.Info,
                            Janela = janela.Info,
                            Host = this.HostInfo
                        });

            // Retorna
            return controles;
        }

        public MensagemInterfaceResponseBase EnviarMensagemParaControle(MensagemInterfaceRequestBase mensagem, string idControle)
        {
            // Inicializa
            MensagemInterfaceResponseBase resposta = null;
            
            // Localiza o controle
            Controle controle =
                (from j in this.Janelas
                 from c in j.Controles
                 where c.Info.Id == idControle
                 select c).FirstOrDefault();

            // Se achou, envia a mensagem
            if (controle != null)
                resposta = controle.Instancia.ProcessarMensagem(mensagem);

            // Retorna
            return resposta;
        }

        #endregion

        public Janela ReceberJanela(string idJanela)
        {
            // Faz a consulta
            Janela janela = (from j in this.Janelas
                             where j.Info.Id == idJanela
                             select j).FirstOrDefault();

            // Retorna
            return janela;
        }
    }
}
