using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    /// <summary>
    /// Wrapper para elementos de janela que implementam IJanela
    /// </summary>
    public class Janela : ItemBase
    {
        /// <summary>
        /// Lista de controles registrados na janela.
        /// Geralmente será apenas 1 controle por janela, mas a janela
        /// pode ser implementada para aceitar mais de um controle.
        /// </summary>
        public List<Controle> Controles { get; set; }

        /// <summary>
        /// Comandos registrados nesta janela.
        /// Os comandos são representados como opções de menu ou botões 
        /// em toolbar.
        /// </summary>
        public List<Comando> Comandos { get; set; }

        /// <summary>
        /// Objeto com informações da janela. É a parte que é serializada
        /// da janela.
        /// </summary>
        public JanelaInfo Info { get; set; }

        /// <summary>
        /// Referencia para a instância da janela.
        /// </summary>
        public IJanela Instancia { get; set; }

        /// <summary>
        /// Construtor.
        /// Inicializa as coleções.
        /// Recebe como parâmetro informações da janela a ser criada.
        /// </summary>
        /// <param name="janelaInfo"></param>
        public Janela(JanelaInfo janelaInfo)
        {
            this.Info = janelaInfo;
            this.Controles = new List<Controle>();
            this.Comandos = new List<Comando>();
            inicializar();
        }

        /// <summary>
        /// Faz a inicialização da janela.
        /// Cria a instância e carrega comandos iniciais através do arquivo de config.
        /// </summary>
        private void inicializar()
        {
            // Pede a criação da instancia
            this.Instancia = (IJanela)this.CriarObjeto(this.Info, this);

            // Carrega eventuais comandos que esta janela tenha que ter inicialmente
            ServicoInterfaceDesktopConfig config = GerenciadorConfig.ReceberConfig<ServicoInterfaceDesktopConfig>();
            SkinInfo skinInfo = config.ReceberSkinAtual();
            JanelaInicializacaoInfo janelaInicializacaoInfo = (from i in skinInfo.InicializacaoJanelas
                                                               where i.TipoJanelaAlvo == this.Instancia.GetType().FullName
                                                               select i).FirstOrDefault();
            if (janelaInicializacaoInfo != null)
                foreach (ComandoInfo comandoInfo in janelaInicializacaoInfo.Comandos)
                    this.AdicionarComando(comandoInfo);
        }

        /// <summary>
        /// Adiciona um controle na janela.
        /// Repassa a chamada para o overload que recebe parâmetro informando nulo.
        /// </summary>
        /// <param name="controleInfo"></param>
        /// <returns></returns>
        public Controle AdicionarControle(ControleInfo controleInfo)
        {
            return AdicionarControle(controleInfo, null);
        }

        /// <summary>
        /// Adiciona um controle na janela.
        /// Cria o wrapper de controle e pede para esta janela adicionar o controle.
        /// Pede para o controle carregar eventuais parametros de inicialização.
        /// </summary>
        /// <param name="controleInfo"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Controle AdicionarControle(ControleInfo controleInfo, object parametros)
        {
            // Cria o wrapper e adiciona na coleção de controles
            Controle controle = new Controle(controleInfo, this);
            this.Controles.Add(controle);

            // Inicializa o controle
            if (parametros != null)
                controle.Instancia.CarregarParametros(parametros, EventoManipulacaoParametrosEnum.Persistencia);

            // Pede para a janela adicionar o controle
            this.Instancia.AdicionarControle(controle);

            // Retorna
            return controle;
        }

        /// <summary>
        /// Adiciona um comando na janela.
        /// Cria o wrapper de comando e pede para a janela adicionar o comando.
        /// </summary>
        /// <param name="comandoInfo"></param>
        public void AdicionarComando(ComandoInfo comandoInfo)
        {
            // Cria o wrapper e adiciona na coleção de comandos
            Comando comando = new Comando(comandoInfo, this);
            this.Comandos.Add(comando);

            // Pede para a janela adicionar o comando
            this.Instancia.AdicionarComando(comando);
        }

        /// <summary>
        /// Mostra a janela.
        /// </summary>
        public void MostrarJanela()
        {
            this.Instancia.MostrarJanela();
        }

        /// <summary>
        /// Esconde a janela.
        /// </summary>
        public void EsconderJanela()
        {
            this.Instancia.EsconderJanela();
        }
    }
}
