#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using log4net;
using Gradual.IntegracaoSolicitacoes.PlanoCliente.Lib;
using System.Configuration;
using System.Globalization;
#endregion

namespace Gradual.IntegracaoSolicitacoes.PlanoCliente
{
    public class PersistenciaDB
    {
        #region Properties
        private const string _ConnStringSinacor = "SINACOR";
        private const string _ConnStringControleAcesso = "ControleAcesso";
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double _mensalidade;
        public static double gMensalidade
        {
            get
            {
                if (_mensalidade == null || _mensalidade == 0)
                    _mensalidade = Convert.ToDouble(ConfigurationManager.AppSettings["ValorMensalidade"].ToString());

                return _mensalidade;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Lista os clientes com produtos Ativos e inativos
        /// </summary>
        /// <param name="pAtivado">Produto ativado ou inativo</param>
        /// <returns>Retorna uma lista de Clientes com produtos</returns>
        public List<ClienteInfo> ListarClientesComProdutos(char pAtivado)
        {
            List<ClienteInfo> lRetorno = new List<ClienteInfo>();

            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_ListarClientesComProduto_sel"))
            {
                lAcesso.AddInParameter(cmd, "@pAtivado", DbType.AnsiString, pAtivado);

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroListarClientesComProdutosInfo(dr));
            }

            return lRetorno; 
        }

        /// <summary>
        /// Cria a entidade com o registro recuperado da tabela
        /// </summary>
        /// <param name="row">Data row preenchida que vem do banco</param>
        /// <returns>Retorna uma entidade preenchida</returns>
        private ClienteInfo CriarRegistroListarClientesComProdutosInfo(DataRow row)
        {
            return new ClienteInfo() 
            {
                IdCliente = Convert.ToInt32(row["id_cliente"]),
                NomeCliente = ""
            };
        }

        /// <summary>
        /// Retorna o arquivo já no layout certo para geração do mesmo no diretório específico
        /// </summary>
        /// <param name="pRequest">Objeto de request para geração de arquivo</param>
        /// <returns>Retorno </returns>
        public List<string> ListarLinhasArquivo(GerarArquivoRequest pRequest)
        {
            List<string> lRetorno = new List<string>();

            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            //Header do arquivo
            lRetorno.Add("00OUTROS  OUT");

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "Prc_ClientesAtivos_lst"))
            {
                lAcesso.AddInParameter(cmd, "@pAtivo", DbType.AnsiString, pRequest.StAtivo);
                
                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    //Monta detalhe do arquivo
                    lRetorno.Add(this.MontaDetalheArquivo(dr["id_cliente"].ToString()));
                }
            }

            //Trailer do arquivo
            lRetorno.Add("99OUTROS  OUT");

            return lRetorno;
        }

        /// <summary>
        /// Seleciona o próximo dia útil para 
        /// </summary>
        /// <param name="pRange">-1 - Para dia anterior ,0 - para atual, 1 - para Próximo</param>
        /// <returns>Retorno o próximo datetime com o próximo dia útil a partir da data corrente</returns>
        public DateTime BuscarProximoDiaUtil(int pRange)
        {
            DateTime lRetorno = new DateTime();

            AcessaDados acesso = new AcessaDados();

            acesso.CursorRetorno = "Retorno";

            acesso.ConnectionStringName = _ConnStringSinacor;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PROXIMO_DIA_UTIL_SEL"))
            {
                acesso.AddInParameter(cmd, "pDia", DbType.Int16, 1);

                DataTable lTable = acesso.ExecuteOracleDataTable(cmd);

                foreach (DataRow row in lTable.Rows)
                    lRetorno = Convert.ToDateTime(row["dProxDia"]);
            }

            return lRetorno;
        }
        #endregion

        #region MontaDetalheArquivo
        private string MontaDetalheArquivo(string id_cliente)
        {
            StringBuilder lDetalhe = new StringBuilder();
            
            lDetalhe.Append("01");                                  //-- Tipo de registro FIXO '01'
            
            lDetalhe.Append(DateTime.Now.ToString("dd/MM/yyyy"));   //-- Data movimento dd/mm/yyyy

            lDetalhe.Append(id_cliente.PadLeft(7, '0'));            //-- Código do cliente '7'

            lDetalhe.Append("0152");                                //-- Histórico: está com 152 ma será outro número

            lDetalhe.Append(gMensalidade.ToString().PadLeft(15,'0')); //-- Lançamento

            lDetalhe.Append(string.Empty.PadLeft(94, ' '));

            lDetalhe.Append(string.Empty.PadLeft(95, ' '));

            lDetalhe.Append("OUTNOUT 000000000000000");

            return lDetalhe.ToString();
        }
        #endregion
    }
}
