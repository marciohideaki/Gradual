using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Spider.LimiteRestricao.Lib.Dados;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;
using Gradual.Generico.Dados;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Gradual.Spider.LimiteRestricao.DbLib
{
    public class RiscoLimiteBmfDbLib
    {
        #region Propriedades
        LogSpiderDbLib lDbLog = new LogSpiderDbLib();
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        public const string NomeConexaoSpider = "GradualSpider";

        #region Métodos Risco Limite BMF
        public RiscoListarLimiteBMFResponse ObterSpiderLimiteBMFCliente(RiscoListarLimiteBMFRequest lRquest)
        {
            List<ClienteParametroBMFInstrumentoInfo> lstInstrumento = new List<ClienteParametroBMFInstrumentoInfo>();

            AcessaDados lAcessaDados = new AcessaDados();
            RiscoListarLimiteBMFResponse response = new RiscoListarLimiteBMFResponse();
            ClienteParametroLimiteBMFInfo ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

            try
            {
                var conn = lAcessaDados.Conexao.AbrirConexao(NomeConexaoSpider);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)(conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_sel_limites_bmf";

                SqlParameter param = new SqlParameter();
                param.Value = lRquest.Account;
                param.DbType = DbType.Int32;
                param.ParameterName = "@account";

                cmd.Parameters.Add(param);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet dsLimites = new DataSet();
                da.Fill(dsLimites);

                if (dsLimites.Tables.Count > 0)
                {
                    DataTable tableLimite = dsLimites.Tables[0];
                    DataTable tableLimiteInstrumento = dsLimites.Tables[1];

                    if (tableLimite.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimite.Rows.Count - 1; i++)
                        {
                            ClienteParametroLimiteBMFInfo = new ClienteParametroLimiteBMFInfo();

                            ClienteParametroLimiteBMFInfo.Account                = (tableLimite.Rows[i]["account"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.idClienteParametroBMF  = (tableLimite.Rows[i]["idClienteParametroBMF"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.Contrato               = (tableLimite.Rows[i]["Contrato"]).DBToString();
                            ClienteParametroLimiteBMFInfo.Sentido                = (tableLimite.Rows[i]["Sentido"]).DBToString();
                            ClienteParametroLimiteBMFInfo.QuantidadeTotal        = (tableLimite.Rows[i]["qtTotal"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.QuantidadeDisponivel   = (tableLimite.Rows[i]["qtDisponivel"]).DBToInt32();
                            ClienteParametroLimiteBMFInfo.DataValidade           = (tableLimite.Rows[i]["dtValidade"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.DataMovimento          = (tableLimite.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta = (tableLimite.Rows[i]["qtMaxOferta"]).DBToInt32();

                            response.ListaLimites.Add(ClienteParametroLimiteBMFInfo);
                        }
                    }

                    if (tableLimiteInstrumento.Rows.Count > 0)
                    {

                        for (int i = 0; i <= tableLimiteInstrumento.Rows.Count - 1; i++)
                        {
                            ClienteParametroBMFInstrumentoInfo ClienteParametroBMFInstrumentoInfo = new ClienteParametroBMFInstrumentoInfo();

                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroBMF         = (tableLimiteInstrumento.Rows[i]["IdClienteParametroBMF"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.IdClienteParametroInstrumento = (tableLimiteInstrumento.Rows[i]["IdClienteParametroInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Instrumento                   = (tableLimiteInstrumento.Rows[i]["Instrumento"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.dtMovimento                   = (tableLimiteInstrumento.Rows[i]["dtMovimento"]).DBToDateTime();
                            ClienteParametroBMFInstrumentoInfo.QtTotalContratoPai            = (tableLimiteInstrumento.Rows[i]["QtTotalContratoPai"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtTotalInstrumento            = (tableLimiteInstrumento.Rows[i]["QtTotalInstrumento"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.QtDisponivel                  = (tableLimiteInstrumento.Rows[i]["QtDisponivel"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.ContratoBase                  = (tableLimiteInstrumento.Rows[i]["contrato"]).DBToString();
                            ClienteParametroBMFInstrumentoInfo.QuantidadeMaximaOferta        = (tableLimiteInstrumento.Rows[i]["qtMaxOferta"]).DBToInt32();
                            ClienteParametroBMFInstrumentoInfo.Sentido                       = (tableLimiteInstrumento.Rows[i]["sentido"]).DBToChar();

                            response.ListaLimitesInstrumentos.Add(ClienteParametroBMFInstrumentoInfo);

                        }

                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public RiscoInserirLimiteClienteBMFResponse AtualizarSpiderLimiteBMF(RiscoInserirLimiteClienteBMFRequest InserirLimiteClienteBMFRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            var resposta = new RiscoInserirLimiteClienteBMFResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = NomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF",   DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClientePermissao",      DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.idClientePermissao);
                    lAcessaDados.AddInParameter(lDbCommand, "@account",                 DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Account);
                    lAcessaDados.AddInParameter(lDbCommand, "@contrato",                DbType.AnsiString,  InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Contrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido",                 DbType.AnsiString,  InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.Sentido);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotal",                 DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeTotal);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel",            DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@stRenovacaoAutomatica",   DbType.AnsiString,  InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.RenovacaoAutomatica);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtValidade",              DbType.DateTime,    InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.DataValidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta",             DbType.Int32,       InserirLimiteClienteBMFRequest.ClienteParametroLimiteBMFInfo.QuantidadeMaximaOferta);

                    gLogger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            resposta.IdClienteParametroBMF = (lDataTable.Rows[i]["retorno"]).DBToInt32();
                        }
                    }

                    gLogger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao enviar a ordem para o banco de dados");
                gLogger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }

        }

        public RiscoInserirLimiteBMFInstrumentoResponse AtualizarSpiderLimiteInstrumentoBMF(RiscoInserirLimiteBMFInstrumentoRequest InserirLimiteBMFInstrumentoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            var resposta = new RiscoInserirLimiteBMFInstrumentoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = NomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_limite_bmf_instrumento"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@IdClienteParametroInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@idClienteParametroBMF", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.IdClienteParametroBMF);
                    lAcessaDados.AddInParameter(lDbCommand, "@instrumento", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Instrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalContratoPai", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalContratoPai);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtTotalInstrumento", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtTotalInstrumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtDisponivel", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QtDisponivel);
                    lAcessaDados.AddInParameter(lDbCommand, "@qtMaxOferta", DbType.Int32, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.QuantidadeMaximaOferta);
                    lAcessaDados.AddInParameter(lDbCommand, "@sentido", DbType.AnsiString, InserirLimiteBMFInstrumentoRequest.LimiteBMFInstrumento.Sentido);

                    gLogger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    gLogger.Info("Solicitação executada com sucesso.");

                    resposta.bSucesso = true;

                    return resposta;

                }
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao gravar o limite do instrumento no banco de dados");
                gLogger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }
        }

        public RiscoRemoveLimiteBMFInstrumentoResponse RemoverSpiderLimiteInstrumentoBMF(RiscoRemoveLimiteBMFInstrumentoRequest RemoverLimiteBMFInstrumentoRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            var resposta = new RiscoRemoveLimiteBMFInstrumentoResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = NomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_limite_bmf_instrumento"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdClienteParametroInstrumento", DbType.Int32, RemoverLimiteBMFInstrumentoRequest.IdClienteParametroInstrumento);

                    gLogger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    gLogger.Info("Solicitação executada com sucesso.");

                    return resposta;
                }
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao remover o limite do instrumento no banco de dados");
                gLogger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }
        }
        
        public RiscoRemoveLimiteBMFResponse RemoverSpiderLimiteBMF(RiscoRemoveLimiteBMFRequest RemoverLimiteBMFRequest)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();
            var resposta = new RiscoRemoveLimiteBMFResponse();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = NomeConexaoSpider;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_limite_bmf"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdClienteParametroBMF", DbType.Int32, RemoverLimiteBMFRequest.LimiteBMFInstrumento.idClienteParametroBMF);

                    gLogger.Info("Envia solicitação para o banco de dados executar a stored procedure.");

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    gLogger.Info("Solicitação executada com sucesso.");

                    return resposta;
                }
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao remover o limite do Contrato no banco de dados");
                gLogger.Info("Descrição do erro:    " + ex.Message);

                throw (ex);
            }
        }
        #endregion
    }
}
