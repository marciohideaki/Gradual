using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Gradual.Spider.PositionClient.DbLib
{
    public class PersistenciaBase
    {
        #region Properties
        
        public static string gNomeConexaoCadastro
        {
            get { return "Cadastro"; }
        }

        public static string gNomeFilialAssessor
        {
            get { return "Filial"; }
        }

        public static string gNomeConexaoRendaFixa
        {
            get { return "RendaFixa"; }
        }

        public static string gNomeConexaoSinacor
        {
            get { return "SinacorConsulta"; }
        }

        public static string gNomeConexaoSinacorConsulta
        {
            get { return "SinacorConsulta"; }
        }

        public static string gNomeComexaoOMS
        {
            get { return "OMS"; }
        }

        public static string gNomeConexaoRisco
        {
            get { return "RISCO"; }
        }

        public static string gNomeConexaoRiscoNovoOMS
        {
            get { return "RISCO_GRADUALOMS"; }
        }

        public static string gNomeConexaoRiscoSpider
        {
            get { return "GradualSpider"; }
        }

        public static string gNomeConexaoSinacorTrade
        {
            get { return "SINACOR"; }
        }

        public static string gNomeConexaoClubesSisfinance
        {
            get { return "Clubes"; }
        }

        public static string gNomeConexaoFundos
        {
            get { return "PlataformaInviXX"; }
        }

        public static string gNomeConexaoSpiderRMS
        {
            get { return "SpiderRMS"; }
        }

        public static string gNomeConexaoSpiderMDS
        {
            get { return "SpiderMDS"; }
        }
        #endregion

        #region Atributes
        protected static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Enum com as condições de ação que o usuário está efetuando
        /// </summary>
        public enum eAcao
        {
            Inserir,
            Consultar,
            Receber,
            Alterar,
            Excluir,
            Exportar,
            Importar,
            FalhaLogin,
            EfetuarIPO
        }
        #endregion

        #region Construtores
        public PersistenciaBase()
        {

        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que recebe os dados e monta o cabeçalho em uma string do log que irá ser gravado 
        /// </summary>
        /// <param name="Entidade">Entidade usada como classe info</param>
        /// <param name="IdUsuarioLogado">ID do usuário gravado</param>
        /// <param name="DescricaoUsuarioLogado">Descrição do usuário logado </param>
        /// <param name="Acao">Ação que o usuário está efetuand no momento da gravação do log</param>
        /// <returns>Retorna uma string com os dados montando o log que irá ser gravado.</returns>
        private static string GetCabecalho(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao)
        {
            return string.Format("[Entidade] {0} ; [Ação] {1} ; [IdUsuarioLogado (ID_LOGIN)] {2} ; [DescricaoUsuarioLogado] {3}"
                                , Entidade.GetType().Name, Acao.ToString(), IdUsuarioLogado.DBToString(), DescricaoUsuarioLogado);
        }
        public static void Logar(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao, Exception Ex = null)
        {
            string lCabecalho = GetCabecalho(Entidade, IdUsuarioLogado, DescricaoUsuarioLogado, Acao);

            string lConteudo = "";

            try
            {

                System.Reflection.PropertyInfo[] propriedades = Entidade.GetType().GetProperties();

                foreach (System.Reflection.PropertyInfo item in propriedades)
                {
                    lConteudo += " ; [" + item.Name + "] ";
                    lConteudo += item.GetValue(Entidade, null).DBToString();
                }
                if (null == Ex)
                    gLogger.Info(lCabecalho + lConteudo);
                else
                    gLogger.Error(lCabecalho + lConteudo, Ex);
            }
            catch (Exception ex)
            {
                gLogger.Error("[Erro ao Salvar Log] ", ex);
            }
        }
        #endregion
    }
}
