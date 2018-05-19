#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using log4net;
using Gradual.OMS.AtivacaoPlanoCliente.Lib;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library.Servicos;
using System.IO;
using Gradual.OMS.Library;
#endregion

namespace Gradual.OMS.AtivacaoPlanoCliente
{
    public class CobrancaDB
    {
        #region Properties
        private const string _ConnStringSinacor = "SINACOR";
        private const string _ConnStringControleAcesso = "ControleAcesso";
        private const string _ConnStringControleMyCapital = "MyCapital"; 
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Métodos

        /// <summary>
        /// Lista os clientes com produtos Ativos e inativos
        /// </summary>
        /// <param name="pAtivado">Produto ativado ou inativo</param>
        /// <returns>Retorna uma lista de Clientes com produtos</returns>
        public List<int> ListarClientesComProdutos(char pAtivado)
        {
            List<int> lRetorno = new List<int>();

            AcessaDados lAcesso = new AcessaDados();

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "prc_ListarClientesComProduto_sel"))
            {
                lAcesso.AddInParameter(cmd, "@pAtivado", DbType.AnsiString, pAtivado);

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(Convert.ToInt32(dr["id_cliente"]));
            }

            return lRetorno;
        }

        /// <summary>
        /// Retorna o arquivo já no layout certo para geração do mesmo no diretório específico
        /// </summary>
        /// <param name="pRequest">Objeto de request para geração de arquivo</param>
        /// <returns>Retorno </returns>
        public List<string> ListarLinhasArquivo(GerarArquivoRequest pRequest)
        {
            List<string> lRetorno = new List<string>();

            //Header do arquivo
            lRetorno.Add("00OUTROS  OUT".PadRight(250, ' '));

            lRetorno.AddRange( this.ListarLinhasArquivosIR(pRequest) );
            lRetorno.AddRange(this.ListarLinhasArquivosIRPlanoFechado(pRequest));

            //Trailer do arquivo
            lRetorno.Add("99OUTROSOUT".PadRight(250, ' '));

            return lRetorno;
        }

        public List<string> ListarLinhasArquivosIR(GerarArquivoRequest pRequest)
        {
            List<string> lRetorno = new List<string>();

            AcessaDados lAcesso = new AcessaDados();
            
            AcessaDados _AcessaDados = new AcessaDados();

            AcessaDados _AcessaDadosMyCapital = new AcessaDados("ResultSet");
            
            Conexao Conexao = new Generico.Dados.Conexao();
            
            DbCommand cmdIR;

            DbCommand cmdDataCobranca;

            DataTable tabelaIR;

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            _AcessaDados.ConnectionStringName = _ConnStringControleAcesso;

            _AcessaDadosMyCapital.ConnectionStringName = _ConnStringControleMyCapital;

           

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "Prc_ClientesAtivos_lst"))
            {
                lAcesso.AddInParameter(cmd, "@pSt_situacao", DbType.AnsiString, pRequest.StSituacao);

                lAcesso.AddInParameter(cmd, "@ID_PRODUTO_PLANO", DbType.Int32, GerarArquivoRequest.Plano.IRMensal); 

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                   cmdIR = _AcessaDadosMyCapital.CreateCommand(CommandType.StoredProcedure, "SP_MC_VERIFICA_INTEGRACAO");

                    _AcessaDadosMyCapital.AddInParameter(cmdIR, "piCodCliente", DbType.Int32, dr["CD_CBLC"]);

                    tabelaIR = _AcessaDadosMyCapital.ExecuteOracleDataTable(cmdIR);

                    if (tabelaIR.Rows.Count < 1) //significa que já foi importado para o sistema da mycapital
                    {
                        try
                        {
                            DateTime dataCobranca = this.BuscarProximoDiaUtil();

                            //Monta detalhe do arquivo
                            lRetorno.Add(this.MontaDetalheArquivo(dataCobranca, dr["cd_cblc"], dr["vl_preco"], "1014"));

                            cmdDataCobranca = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ATUALIZA_DATA_COBRANCA");

                            //Valor cobrado com virgula no número decimal
                            string valorCobrado = "0";

                            if (dr["vl_preco"].ToString() != string.Empty && dr["vl_preco"].ToString() != "0")
                            {
                                valorCobrado = dr["vl_preco"].ToString().Insert(dr["vl_preco"].ToString().Length - 2, ".");
                            }

                            _AcessaDados.AddInParameter(cmdDataCobranca, "@CODIGO_CBLC"     , DbType.Int32      , dr["CD_CBLC"]                     );
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@DATA_COBRANCA"   , DbType.DateTime   , dataCobranca                      );
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@ID_PRODUTO_PLANO", DbType.Int32      , GerarArquivoRequest.Plano.IRMensal);
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@VL_COBRADO"      , DbType.String     , valorCobrado                      );

                            _AcessaDados.ExecuteNonQuery(cmdDataCobranca);

                            //Envia e-mail notificando cliente
                            EnviaNotificacaoAtivacaoPlanoIR(Convert.ToInt32(dr["CD_CBLC"]), (int)GerarArquivoRequest.Plano.IRIntervalo);
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Erro ao atualizar a tabela de cobrança para o Plano IRAberto - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
                        }
                    }
                }
            }

            

            return lRetorno;
        }

        public List<string> ListarLinhasArquivosIRPlanoFechado(GerarArquivoRequest pRequest)
        {
            List<string> lRetorno = new List<string>();

            AcessaDados lAcesso = new AcessaDados();


            AcessaDados _AcessaDados = new AcessaDados();

            AcessaDados _AcessaDadosMyCapital = new AcessaDados("ResultSet");

            Conexao Conexao = new Generico.Dados.Conexao();

            DbCommand cmdIR;

            DbCommand cmdDataCobranca;

            DataTable tabelaIR;

            lAcesso.ConnectionStringName = _ConnStringControleAcesso;

            _AcessaDados.ConnectionStringName = _ConnStringControleAcesso;

            _AcessaDadosMyCapital.ConnectionStringName = _ConnStringControleMyCapital;

            

            using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "Prc_ClientesAtivos_lst"))
            {
                lAcesso.AddInParameter(cmd, "@pSt_situacao", DbType.AnsiString, pRequest.StSituacao);

                lAcesso.AddInParameter(cmd, "@ID_PRODUTO_PLANO", DbType.Int32, GerarArquivoRequest.Plano.IRIntervalo);

                DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    cmdIR = _AcessaDadosMyCapital.CreateCommand(CommandType.StoredProcedure, "SP_MC_VERIFICA_INTEGRACAO");

                    _AcessaDadosMyCapital.AddInParameter(cmdIR, "piCodCliente", DbType.Int32, dr["CD_CBLC"]);

                    tabelaIR = _AcessaDadosMyCapital.ExecuteOracleDataTable(cmdIR);

                    if (tabelaIR.Rows.Count < 1) //significa que já foi importado para o sistema da mycapital
                    {
                        try
                        {
                            //PEGA A PROXIMA DATA UTIL PARA COBRANÇA
                            DateTime dataCobranca = this.BuscarProximoDiaUtil();


                            //Monta detalhe do arquivo
                            lRetorno.Add(this.MontaDetalheArquivo(dataCobranca, dr["cd_cblc"], dr["vl_preco"], "1014"));

                            //Valor cobrado com virgula no número decimal
                            string valorCobrado = "0";

                            if (dr["vl_preco"].ToString() != string.Empty && dr["vl_preco"].ToString() != "0")
                            {
                                valorCobrado = dr["vl_preco"].ToString().Insert(dr["vl_preco"].ToString().Length - 2, ".");
                            }

                            cmdDataCobranca = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ATUALIZA_DATA_COBRANCA");

                            _AcessaDados.AddInParameter(cmdDataCobranca, "@CODIGO_CBLC"     , DbType.Int32      , dr["CD_CBLC"]                         );
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@DATA_COBRANCA"   , DbType.DateTime   , dataCobranca                          );
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@ID_PRODUTO_PLANO", DbType.Int32      , GerarArquivoRequest.Plano.IRIntervalo );
                            _AcessaDados.AddInParameter(cmdDataCobranca, "@VL_COBRADO"      , DbType.String     , valorCobrado                          );

                            _AcessaDados.ExecuteNonQuery(cmdDataCobranca);

                            //Envia e-mail notificando cliente
                            EnviaNotificacaoAtivacaoPlanoIR(Convert.ToInt32(dr["CD_CBLC"]), (int)GerarArquivoRequest.Plano.IRIntervalo);
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Erro ao atualizar a tabela de cobrança para o Plano IRFechado - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
                        }

                    }
                }
            }

            

            return lRetorno;
        }

        private void EnviaNotificacaoAtivacaoPlanoIR(int CodigoCBLC, int CodigoPlano)
        {
            try
            {
                var lServico = Ativador.Get<IServicoEmail>();

                AcessaDados lAcesso = new AcessaDados();

                logger.InfoFormat("Conseguiu instanciar o serviço Ativador.Get<IServicoEmail>");

                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailNotificacao"].ToString(), "NotificacaoAtivacaoPlanoIR.txt"));

                string lCorpoEmail = lStream.ReadToEnd();

                var lEmail = new EnviarEmailRequest();

                lEmail.Objeto = new EmailInfo();

                lEmail.Objeto.Assunto = "Notificação de ativação Plano IR";

                lAcesso.ConnectionStringName = _ConnStringControleAcesso;

                using (DbCommand cmd = lAcesso.CreateCommand(CommandType.StoredProcedure, "PRC_SELECIONAR_CLIENTES_PLANOIR"))
                {
                    lAcesso.AddInParameter(cmd, "@CD_CBLC"      , DbType.Int32 , CodigoCBLC );

                    lAcesso.AddInParameter(cmd, "@ID_PRODUTO"   , DbType.Int32 , CodigoPlano);

                    //recupera os dados para mandar e-mail
                    DataTable table = lAcesso.ExecuteDbDataTable(cmd);

                    if (table.Rows.Count > 0)
                    {

                        lEmail.Objeto.Destinatarios = new List<string>();

                        lEmail.Objeto.Destinatarios.Add(table.Rows[0]["DS_EMAIL"].ToString());

                        logger.InfoFormat(string.Format("Enviando e-mail de notificação de ativação de plano para para {0}", table.Rows[0]["DS_EMAIL"].ToString()));

                        lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacao"].ToString();

                        lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("###NOME###", table.Rows[0]["DS_NOME"].ToString());

                        logger.InfoFormat("Entrou no método de EnviaEmailAvisoPlanoIR - CBLC CodigoCBLC");

                        EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                        if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                        {
                            logger.Info("Email disparado com sucesso");
                        }
                        else
                        {
                            logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em EnviaEmailAvisoPlanoCliente  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public List<string> ListarLinhasArquivoTravelCard(GerarArquivoRequest pRequest)
        {
            List<string> lRetorno = new List<string>();

            //Header do arquivo
            lRetorno.Add("00OUTROS  OUT".PadRight(250, ' '));


            int lIdProduto = Convert.ToInt32(ConfigurationManager.AppSettings["IdProdutoTravelCard"]);

            AcessaDados lAcessaDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            DbCommand lCommand;


            DataTable lTabelaParaExportar;

            lAcessaDados.ConnectionStringName = _ConnStringControleAcesso;

            lCommand = lAcessaDados.CreateCommand(CommandType.Text, "SELECT * FROM vw_compras_para_exportar");

            logger.Info("Iniciando exportação de arquivo do TravelCard; Buscando dados em vw_compras_para_exportar...");

            lTabelaParaExportar = lAcessaDados.ExecuteDbDataTable(lCommand);

            logger.InfoFormat("[{0}] compras emcontradas", lTabelaParaExportar.Rows.Count);


            foreach (DataRow lRow in lTabelaParaExportar.Rows)
            {
                try
                {
                    DateTime lDataCobranca = this.BuscarProximoDiaUtil();

                    lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ATUALIZA_DATA_COBRANCA");

                    //Valor cobrado com virgula no número decimal
                    string lValorCobrado = "0";

                    if (lRow["VL_VALOR_BRUTO"].ToString() != string.Empty && lRow["VL_VALOR_BRUTO"].ToString() != "0")
                    {
                        lValorCobrado = lRow["VL_VALOR_BRUTO"].ToString().Insert(lRow["VL_VALOR_BRUTO"].ToString().Length - 2, ".");
                    }

                    lAcessaDados.AddInParameter(lCommand, "@CODIGO_CBLC"     , DbType.Int32      , lRow["CD_CBLC"] );
                    lAcessaDados.AddInParameter(lCommand, "@DATA_COBRANCA"   , DbType.DateTime   , lDataCobranca   );
                    lAcessaDados.AddInParameter(lCommand, "@ID_PRODUTO_PLANO", DbType.Int32      , lIdProduto      );
                    lAcessaDados.AddInParameter(lCommand, "@VL_COBRADO"      , DbType.String     , lValorCobrado   );

                    lAcessaDados.ExecuteNonQuery(lCommand);

                    lCommand.Dispose();

                    lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_venda_marcar_como_exportada");

                    lAcessaDados.AddInParameter(lCommand, "@ID_VENDA", DbType.Int32, lRow["ID_VENDA"]);

                    lAcessaDados.ExecuteNonQuery(lCommand);


                    //Monta detalhe do arquivo
                    lRetorno.Add(this.MontaDetalheArquivo(lDataCobranca, lRow["CD_CBLC"], lRow["VL_VALOR_BRUTO"], "1034"));

                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Erro ao processar linha para o TravelCard - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
                }
            }



            //Trailer do arquivo
            lRetorno.Add("99OUTROSOUT".PadRight(250, ' '));

            return lRetorno;
        }

        /// <summary>
        /// Seleciona o próximo dia útil para 
        /// </summary>
        /// <param name="pRange">-1 - Para dia anterior ,0 - para atual, 1 - para Próximo</param>
        /// <returns>Retorno o próximo datetime com o próximo dia útil a partir da data corrente</returns>
        private DateTime BuscarProximoDiaUtil()
        {
            DateTime lRetorno = DateTime.Now;

            AcessaDados acesso = new AcessaDados();

            acesso.CursorRetorno = "Retorno";

            acesso.ConnectionStringName = _ConnStringSinacor;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PROXIMO_DIA_UTIL_SEL"))
            {
                //acesso.AddInParameter(cmd, "pDt_operacao", DbType.DateTime, pData);

                DataTable lTable = acesso.ExecuteOracleDataTable(cmd);

                foreach (DataRow row in lTable.Rows)
                    lRetorno = Convert.ToDateTime(row["dProxDia"]);
            }

            return lRetorno;
        }

        #endregion

        #region MontaDetalheArquivo

        private string MontaDetalheArquivo(DateTime pDataCobranca, object pCBLC, object pPreco, string pCodigo)
        {
            StringBuilder lDetalhe = new StringBuilder();

            lDetalhe.Append("01");                                                            //-- Tipo de registro FIXO '01'                                                                  

            lDetalhe.Append(pDataCobranca.ToString("dd/MM/yyyy"));              //-- Data vencimento dd/mm/yyyy                                                                  

            lDetalhe.Append(pCBLC.ToString().PadLeft(7, '0'));                        //-- Código do cliente '7'                                                                  

            lDetalhe.Append(pCodigo);                                                          //-- Histórico: está com 152 ma será outro número                                                                  

            lDetalhe.Append(pPreco.ToString().PadLeft(15, '0'));                      //-- Lançamento

            lDetalhe.Append(string.Empty.PadLeft(94, ' '));                      

            lDetalhe.Append(string.Empty.PadLeft(95, ' '));                     

            lDetalhe.Append("OUTNOUT 000000000000000");                         

            return lDetalhe.ToString();                                         
        }

        #endregion
    }
}


