using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.AutomacaoDesktop
{
    [Serializable]
    public class MonitorConfig
    {
        public string Nome { get; set; }
        public string Instancia { get; set; }
    }


    [Serializable]
    public class AutomacaoConfig
    {
        [XmlElement(IsNullable = true)]
        public string ArquivoQuickFix { get; set; }

        public int PortaGerenciadorMDS { get; set; }

        public long AtrasoMilisegundosExecucaoAdapters {get; set;}

        [XmlElement(IsNullable = true)]
        public string ServidorRetransmissorBMF { get; set; }

        public int PortaRetransmissorBMF { get; set; }

        public long IntervaloReconexaoRetransmissorBMF { get; set; }

        public int TimeoutConexaoRetransmissorBMF { get; set; }

        [XmlElement(IsNullable = true)]
        public string ServidorRetransmissorBovespa { get; set; }

        public int PortaRetransmissorBovespa { get; set; }

        public long IntervaloReconexaoRetransmissorBovespa { get; set; }

        public int TimeoutConexaoRetransmissorBovespa { get; set; }

        [XmlElement(IsNullable = true)]
        public string ArquivoTextoSimuladorBMF { get; set; }

        public long AtrasoMilisegundosSimuladorBMF { get; set; }

        public int AtrasoTipoSimuladorBMF { get; set; }

        [XmlElement(IsNullable = true)]
        public string ArquivoBinarioSimuladorBovespa { get; set; }

        [XmlElement(IsNullable = true)]
        public string ArquivoTextoSimuladorBovespa { get; set; }

        public long AtrasoMilisegundosSimuladorBovespa { get; set; }

        public long AtrasoTipoSimuladorBovespa { get; set; }

        [XmlElement(IsNullable = true)]
        public string ConexaoMDS { get; set; }

        [XmlElement(IsNullable = true)]
        public string ConexaoOMS { get; set; }

        [XmlElement(IsNullable = true)]
        public string ConexaoRisco { get; set; }

        [XmlElement(IsNullable = true)]
        public string UsuarioMDS { get; set; }

        [XmlElement(IsNullable = true)]
        public string UsuarioOMS { get; set; }

        [XmlElement(IsNullable = true)]
        public string UsuarioRisco { get; set; }

        [XmlElement(IsNullable = true)]
        public string SenhaMDS { get; set; }

        [XmlElement(IsNullable = true)]
        public string SenhaOMS { get; set; }

        [XmlElement(IsNullable = true)]
        public string SenhaRisco { get; set; }

        public int NumeroItensHomeBroker { get; set; }

        public int NumeroItensLivroOfertas { get; set; }

        public int NumeroItensLivroOfertasHomeBroker { get; set; }

        [XmlElement(IsNullable = true)]
        public string FormatoDataHoraGMT { get; set; }

        [XmlElement(IsNullable = true)]
        public string DiretorioDB { get; set; }

        [XmlElement(IsNullable = true)]
        public string ExecutarRetomada { get; set; }

        [XmlElement(IsNullable = true)]
        public string DataRetomada { get; set; }

        public int PortaConexaoClientes { get; set; }

        public int PortaConexaoCentralizadorHomeBroker { get; set; }

        #region Parametros Bovespa
        public int IntervaloGravacaoLivroOfertasBovespa { get; set; }

        [XmlElement(IsNullable = true)]
        public string DiretorioDump { get; set; }

        [XmlElement(IsNullable = true)]
        public string ServidorProxyDiff { get; set; }

        public int PortaProxyDiff { get; set; }

        public bool DebugLOFBovespa { get; set; }

        [XmlElement(IsNullable = true)]
        public string DebugLOFBovPapel { get; set; }
        #endregion // Parametros Bovespa

        /// <summary>
        /// Lista dos monitores a serem carregados.
        /// </summary>
        public List<MonitorConfig> Monitores{ get;set;}

        /// <summary>
        ///  Configuracao do MarketData BFM
        /// </summary>
        public FixConfig BMFMarketDataConfig;

        public AutomacaoConfig()
        {
            ArquivoQuickFix = ARQUIVO_QUICKFIX_PADRAO;

            ConexaoMDS = BANCO_DADOS_MDS_CONEXAO_BANCO_PADRAO;

            ConexaoOMS = BANCO_DADOS_OMS_CONEXAO_BANCO_PADRAO;

            ConexaoRisco = BANCO_DADOS_RISCO_CONEXAO_BANCO_PADRAO;

            UsuarioMDS = BANCO_DADOS_MDS_USUARIO_PADRAO;

            UsuarioOMS = BANCO_DADOS_OMS_USUARIO_PADRAO;

            UsuarioRisco = BANCO_DADOS_RISCO_USUARIO_PADRAO;

            SenhaMDS = BANCO_DADOS_MDS_SENHA_PADRAO;

            SenhaOMS = BANCO_DADOS_OMS_SENHA_PADRAO;

            SenhaRisco = BANCO_DADOS_RISCO_SENHA_PADRAO;

            ServidorRetransmissorBMF = RETRANSMISSOR_BMF_SERVIDOR_PADRAO;

            PortaRetransmissorBMF = RETRANSMISSOR_BMF_PORTA_PADRAO;

            IntervaloReconexaoRetransmissorBMF = RETRANSMISSOR_BMF_INTERVALO_RECONEXAO_PADRAO;

            TimeoutConexaoRetransmissorBMF = RETRANSMISSOR_BMF_TIMEOUT_CONEXAO_PADRAO;

            ServidorRetransmissorBovespa = RETRANSMISSOR_BOVESPA_SERVIDOR_PADRAO;

            PortaRetransmissorBovespa = RETRANSMISSOR_BOVESPA_PORTA_PADRAO;

            IntervaloReconexaoRetransmissorBovespa = RETRANSMISSOR_BOVESPA_INTERVALO_RECONEXAO_PADRAO;

            TimeoutConexaoRetransmissorBovespa = RETRANSMISSOR_BOVESPA_TIMEOUT_CONEXAO_PADRAO;

            ArquivoTextoSimuladorBMF = SIMULADOR_BMF_ARQUIVO_TEXTO_PADRAO;

            AtrasoMilisegundosSimuladorBMF = SIMULADOR_BMF_ATRASO_MILISEGUNDOS_PADRAO;

            AtrasoTipoSimuladorBMF = SIMULADOR_BMF_ATRASO_A_PARTIR_DE_PADRAO;

            ArquivoBinarioSimuladorBovespa = SIMULADOR_ARQUIVO_BINARIO_PADRAO;

            AtrasoMilisegundosSimuladorBovespa = SIMULADOR_ATRASO_MILISEGUNDOS_PADRAO;

            AtrasoTipoSimuladorBovespa = SIMULADOR_ATRASO_A_PARTIR_DE_PADRAO;

            ArquivoTextoSimuladorBovespa = SIMULADOR_ARQUIVO_TEXTO_PADRAO;

            PortaGerenciadorMDS = GERENCIADOR_MDS_PORTA_PADRAO;

            AtrasoMilisegundosExecucaoAdapters = GERENCIADOR_MDS_ATRASO_EXECUCAO_ADAPTERS_PADRAO;

            NumeroItensHomeBroker = RANKING_NEGOCIOS_NUMERO_ITENS_HOMEBROKER_PADRAO;

            NumeroItensLivroOfertas = LIVRO_OFERTAS_NUMERO_ITENS_PADRAO;

            FormatoDataHoraGMT = DADOS_BMF_DATAHORA_GMT_PADRAO;

            DiretorioDB = CONFIG_CAMINHO_BASES_PERSISTENTES_PADRAO;

            NumeroItensLivroOfertasHomeBroker = LIVRO_OFERTAS_NUMERO_ITENS_HOMEBROKER_PADRAO;

            ExecutarRetomada = RETOMADA_EXECUTAR;

            DataRetomada = RETOMADA_DATA;

            PortaConexaoClientes = CONEXAO_CLIENTES_PORTA_PADRAO;

            PortaConexaoCentralizadorHomeBroker = CONEXAO_CENTRALIZADORHOMEBROKER_PORTA_PADRAO;

            IntervaloGravacaoLivroOfertasBovespa = INTERVALO_GRAVACAO_LIVRO_BOVESPA_PADRAO;

            DiretorioDump = DIRETORIO_DUMP_PADRAO;

            ServidorProxyDiff = SERVIDOR_PROXYDIFF_PADRAO;
            PortaProxyDiff = PORTA_PROXYDIFF_PADRAO;
        }


        /*
         * Arquivo de parametros
         */
        public const string ARGUMENTO_ARQUIVO_PARAMETROS_MDS = "gradualmds.parametros";
        public const string ARGUMENTO_ARQUIVO_QUICKFIX = "gradualmds.quickfix";
        public const string ARQUIVO_PARAMETROS_MDS_PADRAO = "parametros.xml";
        public const string ARQUIVO_QUICKFIX_PADRAO = "RetransmissorBMF.cfg";

        /*
         * Parametros XML
         */
        public const string XML_GERENCIADOR_MDS_PORTA = "gerenciador-mds.porta";
        public const string XML_GERENCIADOR_MDS_ATRASO_EXECUCAO_ADAPTERS = "gerenciador-mds.atraso-milisegundos-execucao-adapters";
        public const string XML_CONEXAO_CLIENTES_PORTA = "conexao-clientes.porta";
        public const string XML_CONEXAO_CENTRALIZADORHOMEBROKER_PORTA = "conexao-centralizador-homebroker.porta";
        public const string XML_LIVRO_OFERTAS_NUMERO_ITENS = "livro-ofertas.numero-itens";
        public const string XML_LIVRO_OFERTAS_NUMERO_ITENS_HOMEBROKER = "livro-ofertas.numero-itens-homebroker";
        public const string XML_RANKING_NEGOCIOS_NUMERO_ITENS_HOMEBROKER = "ranking-negocios.numero-itens-homebroker";
        public const string XML_DADOS_BMF_DATAHORA_GMT = "dados-bmf.datahora-gmt";
        public const string XML_SIMULADOR_ARQUIVO_BINARIO = "simulador.arquivo-binario";
        public const string XML_SIMULADOR_ARQUIVO_TEXTO = "simulador.arquivo-texto";
        public const string XML_SIMULADOR_ATRASO_MILISEGUNDOS = "simulador.atraso-milisegundos";
        public const string XML_SIMULADOR_ATRASO_A_PARTIR_DE = "simulador.atraso-a-partir-de";
        public const string XML_SIMULADOR_BMF_ARQUIVO_TEXTO = "simulador-bmf.arquivo-texto";
        public const string XML_SIMULADOR_BMF_ATRASO_MILISEGUNDOS = "simulador-bmf.atraso-milisegundos";
        public const string XML_SIMULADOR_BMF_ATRASO_A_PARTIR_DE = "simulador-bmf.atraso-a-partir-de";
        public const string XML_CONFIG_CAMINHO_BASES_PERSISTENTES = "caminho-bases-persistentes.caminho";
        public const string XML_RETOMADA_EXECUTAR = "retomada.executar";
        public const string XML_RETOMADA_DATA = "retomada.data-aaaammdd";
        public const string XML_BANCO_DADOS_MDS_CONEXAO_BANCO = "banco-dados-mds.conexao";
        public const string XML_BANCO_DADOS_OMS_CONEXAO_BANCO = "banco-dados-oms.conexao";
        public const string XML_BANCO_DADOS_RISCO_CONEXAO_BANCO = "banco-dados-risco.conexao";
        public const string XML_BANCO_DADOS_MDS_USUARIO = "banco-dados-mds.usuario";
        public const string XML_BANCO_DADOS_OMS_USUARIO = "banco-dados-oms.usuario";
        public const string XML_BANCO_DADOS_RISCO_USUARIO = "banco-dados-risco.usuario";
        public const string XML_BANCO_DADOS_MDS_SENHA = "banco-dados-mds.senha";
        public const string XML_BANCO_DADOS_OMS_SENHA = "banco-dados-oms.senha";
        public const string XML_BANCO_DADOS_RISCO_SENHA = "banco-dados-risco.senha";
        public const string XML_RETRANSMISSOR_BMF_SERVIDOR = "retransmissor-bmf.servidor";
        public const string XML_RETRANSMISSOR_BMF_PORTA = "retransmissor-bmf.porta";
        public const string XML_RETRANSMISSOR_BMF_INTERVALO_RECONEXAO = "retransmissor-bmf.intervalo-reconexao";
        public const string XML_RETRANSMISSOR_BMF_TIMEOUT_CONEXAO = "retransmissor-bmf.timeout-conexao";
        public const string XML_RETRANSMISSOR_BOVESPA_SERVIDOR = "retransmissor-bovespa.servidor";
        public const string XML_RETRANSMISSOR_BOVESPA_PORTA = "retransmissor-bovespa.porta";
        public const string XML_RETRANSMISSOR_BOVESPA_INTERVALO_RECONEXAO = "retransmissor-bovespa.intervalo-reconexao";
        public const string XML_RETRANSMISSOR_BOVESPA_TIMEOUT_CONEXAO = "retransmissor-bovespa.timeout-conexao";
        public const string XML_INTERVALO_GRAVACAO_LIVRO_BOVESPA = "persistencia-livro.intervalo-gravacao";
        public const string XML_DIRETORIO_DUMP = "dump.diretorio";

        /*
         * Valores padroes para os parametros XML
         */
        public const int GERENCIADOR_MDS_PORTA_PADRAO = 5555;
        public const int GERENCIADOR_MDS_ATRASO_EXECUCAO_ADAPTERS_PADRAO = 2000;
        public const int CONEXAO_CLIENTES_PORTA_PADRAO = 4444;
        public const int CONEXAO_CENTRALIZADORHOMEBROKER_PORTA_PADRAO = 6666;
        public const int LIVRO_OFERTAS_NUMERO_ITENS_PADRAO = 10;
        public const int LIVRO_OFERTAS_NUMERO_ITENS_HOMEBROKER_PADRAO = 10;
        public const int RANKING_NEGOCIOS_NUMERO_ITENS_HOMEBROKER_PADRAO = 10;
        public const string DADOS_BMF_DATAHORA_GMT_PADRAO = "GMT";
        public const string SIMULADOR_ARQUIVO_BINARIO_PADRAO = "sinal.bin";
        public const string SIMULADOR_ARQUIVO_TEXTO_PADRAO = "sinal.txt";
        public const long SIMULADOR_ATRASO_MILISEGUNDOS_PADRAO = 10;
        public const int SIMULADOR_ATRASO_A_PARTIR_DE_PADRAO = 02;
        public const string SIMULADOR_BMF_ARQUIVO_TEXTO_PADRAO = "bmf.txt";
        public const long SIMULADOR_BMF_ATRASO_MILISEGUNDOS_PADRAO = 10;
        public const int SIMULADOR_BMF_ATRASO_A_PARTIR_DE_PADRAO = 2;
        public const string CONFIG_CAMINHO_BASES_PERSISTENTES_PADRAO = @".\DB";
        public const string RETOMADA_EXECUTAR = "SIM";
        public const string RETOMADA_DATA = "";
        public const string BANCO_DADOS_MDS_CONEXAO_BANCO_PADRAO = "jdbc:jtds:sqlserver://192.168.254.14/MDS";
        public const string BANCO_DADOS_OMS_CONEXAO_BANCO_PADRAO = "jdbc:jtds:sqlserver://192.168.254.14/OMS";
        public const string BANCO_DADOS_RISCO_CONEXAO_BANCO_PADRAO = "jdbc:jtds:sqlserver://192.168.254.14/Risco";
        public const string BANCO_DADOS_MDS_USUARIO_PADRAO = "oms";
        public const string BANCO_DADOS_OMS_USUARIO_PADRAO = "oms";
        public const string BANCO_DADOS_RISCO_USUARIO_PADRAO = "sa";
        public const string BANCO_DADOS_MDS_SENHA_PADRAO = "gradual123*";
        public const string BANCO_DADOS_OMS_SENHA_PADRAO = "gradual123*";
        public const string BANCO_DADOS_RISCO_SENHA_PADRAO = "gradual123*";
        public const string RETRANSMISSOR_BMF_SERVIDOR_PADRAO = "localhost";
        public const int RETRANSMISSOR_BMF_PORTA_PADRAO = 50002;
        public const int RETRANSMISSOR_BMF_INTERVALO_RECONEXAO_PADRAO = 60000;
        public const int RETRANSMISSOR_BMF_TIMEOUT_CONEXAO_PADRAO = 400000;
        public const string RETRANSMISSOR_BOVESPA_SERVIDOR_PADRAO = "localhost";
        public const int RETRANSMISSOR_BOVESPA_PORTA_PADRAO = 50000;
        public const long RETRANSMISSOR_BOVESPA_INTERVALO_RECONEXAO_PADRAO = 60000;
        public const int RETRANSMISSOR_BOVESPA_TIMEOUT_CONEXAO_PADRAO = 90000;
        public const int INTERVALO_GRAVACAO_LIVRO_BOVESPA_PADRAO = 0;
        public const string DIRETORIO_DUMP_PADRAO = "dump";
        public const string SERVIDOR_PROXYDIFF_PADRAO = "10.100.91.32";
        public const int PORTA_PROXYDIFF_PADRAO = 15000;


    }
}
