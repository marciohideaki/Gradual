#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.CadastroCliente.Lib;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using log4net;
using Gradual.OMS.CadastroCliente.Lib.Dados;
using System.Timers;
using Gradual.OMS.Library.Servicos;
#endregion

namespace Gradual.OMS.CadastroCliente
{
    public class ServicoCadastroCliente : IServicoCadastroCliente, IServicoControlavel
    {
        #region Porpriedades
        private static readonly log4net.ILog gLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        System.Timers.Timer gTimerNegocios;

        private int TemporizadorIntervaloVerificacao = 15000;

        private static List<NegociosDiaClienteInfo> gListaNegocios = new List<NegociosDiaClienteInfo>();

        private ServicoStatus gServicoStatus = ServicoStatus.Indefinido;

        #endregion

        #region Constutores
        public ServicoCadastroCliente()
        {
            
        }
        #endregion
        #region Métodos Private

        private ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_ViaAtributosDoCliente(ConsultarObjetosRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoInfo> lResposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();

                AcessaDados lAcessaDados = new AcessaDados();

                DbCommand lCommand;

                lAcessaDados.ConnectionStringName = "Cadastro";

                lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_sel_sp");

                MontarParametrosConsultaClienteResumido(ref lAcessaDados, lCommand, pParametros.Objeto);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteResumido(lDataTable.Rows[i], false));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                //gLogger.Error(ex.Message, ex);
                throw ex;
            }
        }

        private ClienteResumidoInfo CriarRegistroClienteResumido(DataRow pRow, bool pSomenteTabelaCliente)
        {
            ClienteResumidoInfo lRetorno = new ClienteResumidoInfo();

            lRetorno.Status      = (pRow["st_status"].DBToBoolean() ? "Ativo" : "Inativo");
            lRetorno.TipoCliente = string.Concat("P", pRow["tp_pessoa"].DBToString());
            lRetorno.CPF         = pRow["ds_cpfcnpj"].DBToString().ToCpfCnpjString();
            lRetorno.Sexo        = pRow["cd_sexo"].DBToString() == "1" ? "F" : "M";
            lRetorno.IdCliente   = pRow["id_cliente"].DBToInt32();
            lRetorno.Passo       = pRow["st_passo"].DBToString();
            lRetorno.NomeCliente = pRow["ds_nome"].DBToString();

            if (!pSomenteTabelaCliente)
            {
                lRetorno.FlagPendencia  = pRow["st_pendencia"].DBToString();
                lRetorno.DataNascimento = pRow["dt_nascimento"].DBToDateTime();
                lRetorno.DataCadastro   = pRow["dt_cadastro"].DBToDateTime();
                lRetorno.CodBMF         = pRow["cd_bmf"].DBToString();
                lRetorno.CodBovespa     = pRow["cd_bovespa"].DBToString();
                lRetorno.Email          = pRow["ds_email"].DBToString();
                lRetorno.CodAssessor    = pRow["cd_assessor"].DBToInt32();
            }
            else
            {
                lRetorno.DataNascimento = pRow["dt_nascimentofundacao"].DBToDateTime();
                lRetorno.DataCadastro   = pRow["dt_passo1"].DBToDateTime();
            }

            return lRetorno;
        }


        private ClienteResumidoInfo CriarRegistroClienteResumidoPorContaMaster(DataRow pRow, bool pSomenteTabelaCliente)
        {
            ClienteResumidoInfo lRetorno = new ClienteResumidoInfo();

            lRetorno.Status = "Ativo";
            lRetorno.TipoCliente = string.Concat("P", String.Empty);
            lRetorno.CPF = String.Empty;
            lRetorno.Sexo = String.Empty;
            lRetorno.IdCliente = pRow["idcliente"].DBToInt32();
            lRetorno.Passo = String.Empty;
            lRetorno.NomeCliente = pRow["dsCliente"].DBToString();

            if (!pSomenteTabelaCliente)
            {
                lRetorno.FlagPendencia = String.Empty;
                lRetorno.DataNascimento = DateTime.Now;
                lRetorno.DataCadastro = DateTime.Now;
                lRetorno.CodBMF = pRow["idCliente"].DBToString();
                lRetorno.CodBovespa = pRow["idCliente"].DBToString();
                lRetorno.Email = String.Empty;
                lRetorno.CodAssessor = 0;
            }
            else
            {
                lRetorno.DataNascimento = DateTime.Now;
                lRetorno.DataCadastro = DateTime.Now;
            }

            return lRetorno;
        }

        private ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_ViaIdDoAssessor(ConsultarObjetosRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoInfo> lResposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "Cadastro";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_resumido_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.TermoDeBusca.DBToInt32());

                    if (pParametros.Objeto.CodLogin.HasValue)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_login", DbType.Int32, pParametros.Objeto.CodLogin.DBToInt32());
                    }

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroClienteResumido(lDataTable.Rows[i], false));
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                //gLogger.Error(ex.Message, ex);
                throw ex;
            }
        }

        private ConsultarObjetosResponse<ClienteResumidoInfo> ConsultarClienteResumido_ViaContaMaster(ConsultarObjetosRequest<ClienteResumidoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteResumidoInfo> lResposta = new ConsultarObjetosResponse<ClienteResumidoInfo>();
                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "DirectTradeRisco";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_conta_master"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idContaBroker", DbType.Int32, pParametros.Objeto.TermoDeBusca.DBToInt32());

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            lResposta.Resultado.Add(CriarRegistroClienteResumidoPorContaMaster(lDataTable.Rows[i], false));
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                //gLogger.Error(ex.Message, ex);
                throw ex;
            }
        }

        private static void MontarParametrosConsultaClienteResumido(ref AcessaDados pAcessaDados, DbCommand pDbCommand, ClienteResumidoInfo pClienteResumidoInfo)
        {
            pAcessaDados.AddInParameter(pDbCommand, "@cd_bovespa", DbType.Int32, OpcoesBuscarPor.CodBovespa.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_cpfcnpj", DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim().Replace(".", string.Empty).Replace("-", string.Empty) : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_nome", DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);
            pAcessaDados.AddInParameter(pDbCommand, "@ds_email", DbType.String, OpcoesBuscarPor.Email.Equals(pClienteResumidoInfo.OpcaoBuscarPor) && !string.IsNullOrWhiteSpace(pClienteResumidoInfo.TermoDeBusca) ? (object)pClienteResumidoInfo.TermoDeBusca.Trim() : System.DBNull.Value);
            //pAcessaDados.AddInParameter(pDbCommand, "@tp_cliente", DbType.Byte, OpcoesTipo.ClientePF.Equals(pClienteResumidoInfo.OpcaoTipo) ? 1 : 2);

            if (null != pClienteResumidoInfo.CodAssessor)
            {
                pAcessaDados.AddInParameter(pDbCommand, "@cd_assessor", DbType.Byte, pClienteResumidoInfo.CodAssessor);
            }

            if (pClienteResumidoInfo.CodLogin != null)
            {
                pAcessaDados.AddInParameter(pDbCommand, "@cd_login", DbType.Byte, pClienteResumidoInfo.CodLogin);
            }

            if (pClienteResumidoInfo.OpcaoStatus.Equals(OpcoesStatus.Ativo))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_ativo", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoStatus.Equals(OpcoesStatus.Inativo))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_inativo", DbType.Byte, 0);
            }
            else if (pClienteResumidoInfo.OpcaoStatus.Equals(OpcoesStatus.Ativo | OpcoesStatus.Inativo))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_ativo", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_inativo", DbType.Byte, 0);
            }

            if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Cadastrado))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Exportado))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante | OpcoesPasso.Cadastrado))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Visitante | OpcoesPasso.Exportado))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 1);
            }
            else if (pClienteResumidoInfo.OpcaoPasso.Equals(OpcoesPasso.Cadastrado | OpcoesPasso.Exportado))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 1);
            }
            else
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_visitante", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_cadastrado", DbType.Byte, 1);
                pAcessaDados.AddInParameter(pDbCommand, "@st_exportadosinacor", DbType.Byte, 1);
            }

            if ("PJ".Equals(pClienteResumidoInfo.TipoCliente))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@tp_pessoa", DbType.String, "J");
            }
            else if ("PF".Equals(pClienteResumidoInfo.TipoCliente))
            {
                pAcessaDados.AddInParameter(pDbCommand, "@tp_pessoa", DbType.String, "F");
            }

            if (pClienteResumidoInfo.OpcaoPendencia != 0)
            {
                if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComPendenciaCadastral))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 1);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 0);
                }
                else if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComSolicitacaoAlteracao))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 0);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 1);
                }
                else if (pClienteResumidoInfo.OpcaoPendencia.Equals(OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao))
                {
                    pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 1);
                    pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 1);
                }
            }
            else
            {
                pAcessaDados.AddInParameter(pDbCommand, "@st_compendenciacadastral", DbType.Byte, 0);
                pAcessaDados.AddInParameter(pDbCommand, "@st_comsolicitacaoalteracao", DbType.Byte, 0);
            }
        }
        
        /// <summary>
        /// Salva as informações no banco de dados
        /// </summary>
        /// <param name="pRequest">Entidade de SalvarHistoricoOperacoesRequest</param>
        /// <returns>Retorna o objeto inserido no banco de dados</returns>
        private SalvarHistoricoOperacoesResponse IncluirHistoricoOperacoes(SalvarHistoricoOperacoesRequest pRequest)
        {
            SalvarHistoricoOperacoesResponse lReturn = new SalvarHistoricoOperacoesResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "DirectTrader";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_HISTORICO_OPERACOES_INS"))
            {

                lAcessaDados.AddInParameter(lDbCommand, "@pTitulo", DbType.AnsiString, pRequest.HistoricoOperacoes.DsTituloHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pRtf"   , DbType.Binary    , pRequest.HistoricoOperacoes.DsRtfHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pImagem", DbType.Binary      , pRequest.HistoricoOperacoes.DsImagemHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pData"  , DbType.DateTime  , pRequest.HistoricoOperacoes.DtDataHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pAtivo" , DbType.AnsiString, pRequest.HistoricoOperacoes.DsAtivoHistorico);
                //lAcessaDados.AddInParameter(lDbCommand, "@p",       DbType.Binary    , pRequest.HistoricoOperacoes.DsRtfHistorico);

                lAcessaDados.AddOutParameter(lDbCommand, "@pIdHistorico",  DbType.AnsiString, 5);

                lReturn.HistoricoOperacoes = pRequest.HistoricoOperacoes;

                lReturn.HistoricoOperacoes.IdHistorico = lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lReturn;
        }

        /// <summary>
        /// Altera o registro de Historico de operações
        /// </summary>
        /// <param name="pRequest">Entidade de SalvarHistoricoOperacoesRequest</param>
        /// <returns>Retorna o objeto Alterado no banco de dados</returns>
        private SalvarHistoricoOperacoesResponse AlterarHistoricoOperacoes(SalvarHistoricoOperacoesRequest pRequest)
        {
            SalvarHistoricoOperacoesResponse lReturn = new SalvarHistoricoOperacoesResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "DirectTrader";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_HISTORICO_OPERACOES_UPD"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@pTitulo",      DbType.AnsiString,  pRequest.HistoricoOperacoes.DsTituloHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pRtf"   ,      DbType.Binary,      pRequest.HistoricoOperacoes.DsRtfHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pImagem",      DbType.Binary,      pRequest.HistoricoOperacoes.DsImagemHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pData",        DbType.DateTime,    pRequest.HistoricoOperacoes.DtDataHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pAtivo",       DbType.AnsiString,  pRequest.HistoricoOperacoes.DsAtivoHistorico);
                lAcessaDados.AddInParameter(lDbCommand, "@pIdHistorico", DbType.Int32,       pRequest.HistoricoOperacoes.IdHistorico);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                lReturn.HistoricoOperacoes = pRequest.HistoricoOperacoes;
            }

            return lReturn;
        }

        private void VerificaOrdemExecutadaSinacor(object sender, EventArgs e)
        {
            AcessaDados lAcessaDados = new AcessaDados("Retorno");

            lAcessaDados.ConnectionStringName = "SINACOR";

            gLogger.Info("Entrando na rotina de verificação do negocios execultados no Sinacor");

            try
            {

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_RESUMO_NEGOCIOS"))
                {
                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    gLogger.InfoFormat("Encontrou {0} negocios realizados", lDataTable.Rows.Count);

                    NegociosDiaClienteInfo lInfo = new NegociosDiaClienteInfo();

                    lock (gListaNegocios)
                    {
                        gListaNegocios.Clear();

                        foreach (DataRow row in lDataTable.Rows)
                        {
                            lInfo = new NegociosDiaClienteInfo();

                            lInfo.CodigoCliente = row["CD_CLIENTE"].DBToInt32();

                            lInfo.TipoBolsa = row["TIPO_BOLSA"].DBToString();

                            lInfo.Quantidade = row["QT_NEGOCIO"].DBToInt32();

                            lInfo.DtPregao = row["DT_NEGOCIO"].DBToDateTime();

                            lInfo.Papel = row["CD_NEGOCIO"].ToString();

                            lInfo.Direcao = row["CD_NATOPE"].ToString();

                            lInfo.PrecoNegociado = row["VL_NEGOCIO"].ToString();

                            lInfo.FatorCotacao = row["VL_FATORCOTACAO"].DBToInt32();

                            gListaNegocios.Add(lInfo);
                        }

                        gLogger.InfoFormat("Encontrou {0} negocios realizados", gListaNegocios.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - ", ex);
            }
        }
        #endregion

        #region Métodos Públicos



        public ExcluirFiltroClienteResponse ExcluirFiltroClienteGTI(ExcluirFiltroClienteRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();

            ExcluirFiltroClienteResponse lRetorno = new ExcluirFiltroClienteResponse();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_gti_del");

                lDados.AddInParameter(lCommand, "@id_filtro", DbType.Int32, pParametros.IdFiltro);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("ExcluirFiltroClienteGTI - {0}", ex);
                throw;
            }

            return lRetorno;
        }

        public AtualizarFiltroClienteResponse AtualizarFiltroClienteGTI(AtualizarFiltroClienteRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            AtualizarFiltroClienteResponse lRetorno = new AtualizarFiltroClienteResponse();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_gti_upd");


                lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pParametros.Filtro.CodigoCliente);
                lDados.AddInParameter(lCommand, "@ds_confirmacoes", DbType.String, pParametros.Filtro.Confirmacoes);
                lDados.AddInParameter(lCommand, "@ds_nomefiltro", DbType.String, pParametros.Filtro.NomeFiltro);
                lDados.AddInParameter(lCommand, "@ds_filtros", DbType.String, pParametros.Filtro.Filtros);
                lDados.AddInParameter(lCommand, "@ds_filtros_clientes", DbType.String, pParametros.Filtro.FiltrosClientes);
                lDados.AddInParameter(lCommand, "@ds_cores", DbType.String, pParametros.Filtro.Cores);
                lDados.AddInParameter(lCommand, "@id_filtro", DbType.Int32, pParametros.Filtro.IdFiltro);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lRetorno.Filtro = pParametros.Filtro;
            }
            catch(Exception ex)
            {
                gLogger.ErrorFormat("AtualizarFiltroClienteGTI - {0}", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public AdicionarFiltroClienteResponse AdicionarFiltroClienteGTI(AdicionarFiltroClienteRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            AdicionarFiltroClienteResponse lRetorno = new AdicionarFiltroClienteResponse();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_gti_ins");

                lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pParametros.Filtro.CodigoCliente);
                lDados.AddInParameter(lCommand, "@ds_confirmacoes", DbType.String, pParametros.Filtro.Confirmacoes);
                lDados.AddInParameter(lCommand, "@ds_nomefiltro", DbType.String, pParametros.Filtro.NomeFiltro);
                lDados.AddInParameter(lCommand, "@ds_filtros", DbType.String, pParametros.Filtro.Filtros);
                lDados.AddInParameter(lCommand, "@ds_filtros_clientes", DbType.String, pParametros.Filtro.FiltrosClientes);
                lDados.AddInParameter(lCommand, "@ds_cores", DbType.String, pParametros.Filtro.Cores);
                lDados.AddOutParameter(lCommand, "@id_filtro", DbType.Int32, 4);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lRetorno.IdFiltro= Convert.ToInt32(lDados.GetParameterValue(lCommand, "@id_filtro"));
            }
            catch(Exception ex)
            {
                gLogger.ErrorFormat("AdicionarFiltroClienteGTI - {0} ", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public BuscarListaFiltroClienteResponse BuscarListaFiltroClienteGTI(BuscarListaFiltroClienteRequest pParametros)
        {
            AcessaDados lDados = null;
            DataTable lTable = null;
            DbCommand lCommand = null;

            BuscarListaFiltroClienteResponse lResponse = new BuscarListaFiltroClienteResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_gti_sel");

                lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                lResponse.Filtros = new List<FiltroClienteInfo>();

                if (lTable.Rows.Count > 0)
                {
                    FiltroClienteInfo lFiltro;

                    foreach (DataRow dr in lTable.Rows)
                    {
                        lFiltro = new FiltroClienteInfo();

                        lFiltro.IdFiltro        = dr["id_filtro"].DBToInt32();
                        lFiltro.Confirmacoes    = dr["ds_confirmacoes"].ToString();
                        lFiltro.Cores           = dr["ds_cores"].DBToString();
                        lFiltro.NomeFiltro      = dr["ds_nomefiltro"].DBToString();
                        lFiltro.CodigoCliente   = dr["cd_cliente"].DBToInt32();
                        lFiltro.Filtros         = dr["ds_filtros"].DBToString();
                        lFiltro.FiltrosClientes = dr["ds_filtros_cliente"].DBToString();

                        lResponse.Filtros.Add(lFiltro);
                    }
                }

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("BuscarListaFiltroClienteGTI - {0}", ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.StackTrace;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lResponse;
        }

        public BuscarBoletaGTIConfigResponse BuscarConfigBoletaGTI(BuscarBoletaGTIConfigRequest pRequest)
        {
            AcessaDados lDados = null;
            DataTable lTable = null;
            DbCommand lCommand = null;

            BuscarBoletaGTIConfigResponse lResponse = new BuscarBoletaGTIConfigResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_boleta_gti_sel");

                lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pRequest.BoletaGTIConfig.CodigoCliente);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                lResponse.BoletaGTIConfig = new BoletaGTIConfigInfo();

                if (lTable.Rows.Count > 0)
                {
                    BoletaGTIConfigInfo lConfiguracao;

                    foreach (DataRow dr in lTable.Rows)
                    {
                        lConfiguracao = new BoletaGTIConfigInfo();

                        lConfiguracao.CodigoConfiguracao                      = dr["id_configuracao"].DBToInt32();
                        lConfiguracao.ConfirmarOrdemBoletaEnvioImediato       = dr["st_ConfirmarOrdemBoletaEnvioImediato"].DBToBoolean();
                        lConfiguracao.ConfirmarOrdemBoletaNegocioDireto       = dr["st_ConfirmarOrdemBoletaNegocioDireto"].DBToBoolean();
                        lConfiguracao.ConfirmarOrdemBoletaUnificada           = dr["st_ConfirmarOrdemBoletaUnificada"].DBToBoolean();
                        lConfiguracao.CodigoCliente                           = dr["cd_cliente"].DBToInt32();
                        lConfiguracao.AmbosBotoesCV                           = dr["st_AmbosBotoesCV"].DBToBoolean();
                        lConfiguracao.CorFundoBoleta                          = dr["ds_corfundo"].DBToString();
                        lConfiguracao.CorLabelBoleta                          = dr["ds_corlabel"].DBToString();
                        lConfiguracao.EnviarMedioDia                          = dr["st_EnviarMedioDia"].DBToBoolean();
                        lConfiguracao.EnviarMelhorOferta                      = dr["st_EnviarMelhorOferta"].DBToBoolean();
                        lConfiguracao.EnviarSemPreco                          = dr["st_EnviarSemPreco"].DBToBoolean();
                        lConfiguracao.EnviarUltimaCotacao                     = dr["st_EnviarUltimaCotacao"].DBToBoolean();
                        lConfiguracao.EnviarMelhorQueOfertaAtual              = dr["st_EnviarMelhorQueOfertaAtual"].DBToBoolean();
                        lConfiguracao.ConfirmarOrdemBoletaUnificadaPreco      = dr["st_ConfirmarOrdemBoletaUnificadaPreco"].DBToBoolean();
                        lConfiguracao.ValorConfirmarOrdemBoletaUnificadaPreco = dr["vl_ConfirmarOrdemBoletaUnificadaPreco"].DBToDecimal();
                        lResponse.BoletaGTIConfig = lConfiguracao;
                    }
                }

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("BuscarConfigBoletaGTI - {0}", ex);
                lResponse.StatusResposta    = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.StackTrace;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }
            return lResponse;
        }

        public AtualizarBoletaGTIConfigResponse AtualizaConfigBoletaGTI(AtualizarBoletaGTIConfigRequest pRequest)
        {
            AtualizarBoletaGTIConfigResponse lResponse = new AtualizarBoletaGTIConfigResponse();
            AcessaDados lDados = new AcessaDados();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_boleta_gti_upd");

                lDados.AddInParameter(lCommand, "@cd_cliente"                            , DbType.Int32,    pRequest.BoletaGTIConfig.CodigoCliente);
                lDados.AddInParameter(lCommand, "@st_AmbosBotoesCV"                      , DbType.Boolean,  pRequest.BoletaGTIConfig.AmbosBotoesCV);
                lDados.AddInParameter(lCommand, "@st_ConfirmarOrdemBoletaEnvioImediato"  , DbType.Boolean , pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaEnvioImediato);
                lDados.AddInParameter(lCommand, "@st_ConfirmarOrdemBoletaNegocioDireto"  , DbType.Boolean,  pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaNegocioDireto);
                lDados.AddInParameter(lCommand, "@st_ConfirmarOrdemBoletaUnificada"      , DbType.Boolean,  pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaUnificada);
                lDados.AddInParameter(lCommand, "@ds_CorFundoBoleta"                     , DbType.String,   pRequest.BoletaGTIConfig.CorFundoBoleta);
                lDados.AddInParameter(lCommand, "@ds_CorLabelBoleta"                     , DbType.String,   pRequest.BoletaGTIConfig.CorLabelBoleta);
                lDados.AddInParameter(lCommand, "@id_configuracao"                       , DbType.Int32,    pRequest.BoletaGTIConfig.CodigoConfiguracao);
                lDados.AddInParameter(lCommand, "@st_EnviarMelhorOferta"                 , DbType.Boolean,  pRequest.BoletaGTIConfig.EnviarMelhorOferta);
                lDados.AddInParameter(lCommand, "@st_EnviarUltimaCotacao"                , DbType.Boolean,  pRequest.BoletaGTIConfig.EnviarUltimaCotacao);
                lDados.AddInParameter(lCommand, "@st_EnviarMedioDia"                     , DbType.Boolean,  pRequest.BoletaGTIConfig.EnviarMedioDia);
                lDados.AddInParameter(lCommand, "@st_EnviarSemPreco"                     , DbType.Boolean,  pRequest.BoletaGTIConfig.EnviarSemPreco);
                lDados.AddInParameter(lCommand, "@st_EnviarMelhorQueOfertaAtual"         , DbType.Boolean,  pRequest.BoletaGTIConfig.EnviarMelhorQueOfertaAtual);
                lDados.AddInParameter(lCommand, "@st_ConfirmarOrdemBoletaUnificadaPreco" , DbType.Boolean,  pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaUnificadaPreco);
                lDados.AddInParameter(lCommand, "@vl_ConfirmarOrdemBoletaUnificadaPreco" , DbType.Decimal,  pRequest.BoletaGTIConfig.ValorConfirmarOrdemBoletaUnificadaPreco);

                gLogger.InfoFormat("AtualizaConfigBoletaGTI -  st_EnviarMelhorOferta:{0} ; st_EnviarUltimaCotacao:{1} ; st_EnviarMedioDia: {2} ; st_EnviarSemPreco : {3} ; st_EnviarMelhorQueOfertaAtual: {4} ",
                    pRequest.BoletaGTIConfig.EnviarMelhorOferta,
                    pRequest.BoletaGTIConfig.EnviarUltimaCotacao,
                    pRequest.BoletaGTIConfig.EnviarMedioDia,
                    pRequest.BoletaGTIConfig.EnviarSemPreco,
                    pRequest.BoletaGTIConfig.EnviarMelhorQueOfertaAtual);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lResponse.BoletaGTIConfig = pRequest.BoletaGTIConfig;

                lResponse.BoletaGTIConfig.CodigoConfiguracao = Convert.ToInt32(lDados.GetParameterValue(lCommand, "@id_configuracao"));
            }
            catch (Exception  ex)
            {
                gLogger.ErrorFormat("AtualizaConfigBoletaGTI - {0}", ex);
                lResponse.StatusResposta    = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.StackTrace;
            }

            return lResponse;
        }

        public AdicionarBoletaGTIConfigResponse AdicionarConfigBoletaGTI(AdicionarBoletaGTIConfigRequest pRequest)
        {
            AdicionarBoletaGTIConfigResponse lResponse = new AdicionarBoletaGTIConfigResponse();
            AcessaDados lDados = new AcessaDados();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_config_boleta_gti_ins");

                lDados.AddInParameter(lCommand,  "@cd_cliente"                           , DbType.Int32,   pRequest.BoletaGTIConfig.CodigoCliente);
                lDados.AddInParameter(lCommand,  "@st_AmbosBotoesCV"                     , DbType.Boolean, pRequest.BoletaGTIConfig.AmbosBotoesCV);
                lDados.AddInParameter(lCommand,  "@st_ConfirmarOrdemBoletaEnvioImediato" , DbType.Boolean, pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaEnvioImediato);
                lDados.AddInParameter(lCommand,  "@st_ConfirmarOrdemBoletaNegocioDireto" , DbType.Boolean, pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaNegocioDireto);
                lDados.AddInParameter(lCommand,  "@st_ConfirmarOrdemBoletaUnificada"     , DbType.Boolean, pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaUnificada);
                lDados.AddInParameter(lCommand,  "@ds_CorFundoBoleta"                    , DbType.String,  pRequest.BoletaGTIConfig.CorFundoBoleta);
                lDados.AddInParameter(lCommand,  "@ds_CorLabelBoleta"                    , DbType.String,  pRequest.BoletaGTIConfig.CorLabelBoleta);
                lDados.AddInParameter(lCommand,  "@st_EnviarMelhorOferta"                , DbType.Boolean, pRequest.BoletaGTIConfig.EnviarMelhorOferta);
                lDados.AddInParameter(lCommand,  "@st_EnviarUltimaCotacao"               , DbType.Boolean, pRequest.BoletaGTIConfig.EnviarUltimaCotacao);
                lDados.AddInParameter(lCommand,  "@st_EnviarMedioDia"                    , DbType.Boolean, pRequest.BoletaGTIConfig.EnviarMedioDia);
                lDados.AddInParameter(lCommand,  "@st_EnviarSemPreco"                    , DbType.Boolean, pRequest.BoletaGTIConfig.EnviarSemPreco);
                lDados.AddInParameter(lCommand,  "@st_EnviarMelhorQueOfertaAtual"        , DbType.Boolean, pRequest.BoletaGTIConfig.EnviarMelhorQueOfertaAtual);
                lDados.AddInParameter(lCommand,  "@st_ConfirmarOrdemBoletaUnificadaPreco", DbType.Boolean, pRequest.BoletaGTIConfig.ConfirmarOrdemBoletaUnificadaPreco);
                lDados.AddInParameter(lCommand,  "@vl_ConfirmarOrdemBoletaUnificadaPreco", DbType.Decimal, pRequest.BoletaGTIConfig.ValorConfirmarOrdemBoletaUnificadaPreco);

                lDados.AddOutParameter(lCommand, "@id_configuracao", DbType.Int32, 4);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lResponse.BoletaGTIConfig = pRequest.BoletaGTIConfig;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("AdicionarConfigBoletaGTI - {0}", ex);
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.StackTrace;
            }

            return lResponse;
        }

        public BuscarListaDeCarteirasResponse BuscarListaDeCarteiras(BuscarListaDeCarteirasRequest pParametros)
        {
            AcessaDados lDados = null;
            DataTable lTable = null;
            DbCommand lCommand = null;

            BuscarListaDeCarteirasResponse lResponse = new BuscarListaDeCarteirasResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "GRADUAL_TRADE";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_lst");

                lDados.AddInParameter(lCommand, "@CD_CBLC", DbType.Int32, pParametros.CBLC);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                lResponse.Carteiras = new List<CarteiraInfo>();

                if (lTable.Rows.Count > 0)
                {
                    CarteiraInfo lCarteira;

                    foreach (DataRow dr in lTable.Rows)
                    {
                        lCarteira = new CarteiraInfo();

                        lCarteira.CodigoCarteira = dr["Id_Carteira"].DBToInt32();
                        lCarteira.Descricao      = dr["ds_carteira"].DBToString();
                        lCarteira.CodigoCBLC     = pParametros.CBLC;

                        lResponse.Carteiras.Add(lCarteira);
                    }
                }

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = ex.StackTrace;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lResponse;
        }

        public BuscarAtivosDaCarteiraResponse BuscarAtivosDaCarteira(BuscarAtivosDaCarteiraRequest pParametros)
        {
            AcessaDados lDados = null;
            DataTable   lTable = null;
            DbCommand   lCommand = null;

            BuscarAtivosDaCarteiraResponse lRetorno = new BuscarAtivosDaCarteiraResponse();

            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "GRADUAL_TRADE";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ATIVO_lst");
                lDados.AddInParameter(lCommand, "@id_carteira", DbType.Int32, pParametros.CodigoCarteira);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    lRetorno.Ativos = new List<CarteiraAtivoInfo>();

                    CarteiraAtivoInfo lCarteira;

                    foreach (DataRow dr in lTable.Rows)
                    {
                        lCarteira = new CarteiraAtivoInfo();

                        lCarteira.CodigoCarteiraAtivo = dr["id_carteira_ativo"].DBToInt32();
                        lCarteira.CodigoNegocio       = dr["cd_Negocio"].DBToString();
                        lCarteira.CodigoCarteira      = dr["Id_Carteira"].DBToInt32();

                        lRetorno.Ativos.Add(lCarteira);
                    }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lRetorno;
        }

        public SalvarCarteiraResponse SalvarCarteira(SalvarCarteiraRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();

            lDados.ConnectionStringName = "GRADUAL_TRADE";

            DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ins");

            SalvarCarteiraResponse lRetorno = new SalvarCarteiraResponse();

            lDados.AddInParameter(lCommand, "@cd_cblc",     DbType.Int32,  pParametros.Carteira.CodigoCBLC);
            lDados.AddInParameter(lCommand, "@ds_carteira", DbType.String, pParametros.Carteira.Descricao);

            // Executa a operação no banco.
            lRetorno.CodigoCarteira = Convert.ToInt32(lDados.ExecuteScalar(lCommand));

            lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return lRetorno;
        }

        public ExcluirCarteiraResponse ExcluirCarteira(ExcluirCarteiraRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            ExcluirCarteiraResponse lRetorno = new ExcluirCarteiraResponse();

            lDados.ConnectionStringName = "GRADUAL_TRADE";

            DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_del");

            lDados.AddInParameter(lCommand, "@Id_Carteira", DbType.Int32, pParametros.CodigoCarteira);
            lDados.AddInParameter(lCommand, "@cd_cblc",     DbType.Int32, pParametros.CodigoCliente);

            // Executa a operação no banco.
            lDados.ExecuteNonQuery(lCommand);

            lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return lRetorno;
        }

        public AdicionarAtivoACarteiraResponse AdicionarAtivoACarteira(AdicionarAtivoACarteiraRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            AdicionarAtivoACarteiraResponse lRetorno = new AdicionarAtivoACarteiraResponse();

            try
            {
                lDados.ConnectionStringName = "GRADUAL_TRADE";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ATIVO_ins");


                lDados.AddInParameter(lCommand,  "@Id_Carteira",        DbType.Int32, pParametros.Ativo.CodigoCarteira);
                lDados.AddInParameter(lCommand,  "@cd_Negocio",         DbType.String, pParametros.Ativo.CodigoNegocio);
                lDados.AddOutParameter(lCommand, "@id_carteira_ativo",  DbType.Int32, 4);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lRetorno.CodigoCarteiraAtivo = Convert.ToInt32(lDados.GetParameterValue(lCommand, "@id_carteira_ativo"));
            }
            catch
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ExcluirAtivoDaCarteiraResponse ExcluirAtivoDaCarteira(ExcluirAtivoDaCarteiraRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            ExcluirAtivoDaCarteiraResponse lRetorno = new ExcluirAtivoDaCarteiraResponse();

            lDados.ConnectionStringName = "GRADUAL_TRADE";

            DbCommand lCommand = null;

            try
            {
                if (pParametros.CodigoCarteiraAtivo > 0)
                {
                    lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ATIVO_del");

                    lDados.AddInParameter(lCommand, "@id_carteira_ativo", DbType.Int32, pParametros.CodigoCarteiraAtivo);
                    lDados.AddInParameter(lCommand, "@cd_cblc",           DbType.Int32, pParametros.CodigoCliente);

                    // Executa a operação no banco.
                    lDados.ExecuteNonQuery(lCommand);

                    lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    if (pParametros.CodigoCarteira != 0)
                    {
                        lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_ATIVO_del_by_ID_Neg");

                        lDados.AddInParameter(lCommand, "@id_carteira", DbType.Int32, pParametros.CodigoCarteira);
                        lDados.AddInParameter(lCommand, "@cd_Negocio", DbType.String, pParametros.CodigoNegocio);

                        // Executa a operação no banco.
                        lDados.ExecuteNonQuery(lCommand);
                        lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
                    }
                }
            }
            catch
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public AtualizarCarteiraResponse AtualizarCarteira(AtualizarCarteiraRequest pParametros)
        {
            AcessaDados lDados = new AcessaDados();
            AtualizarCarteiraResponse lRetorno = new AtualizarCarteiraResponse();

            lDados.ConnectionStringName = "GRADUAL_TRADE";

            DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_TB_CARTEIRA_upd");

            lDados.AddInParameter(lCommand, "@Id_Carteira", DbType.Int32,  pParametros.Carteira.CodigoCarteira);
            lDados.AddInParameter(lCommand, "@cd_cblc",     DbType.Int32,  pParametros.Carteira.CodigoCBLC);
            lDados.AddInParameter(lCommand, "@ds_carteira", DbType.String, pParametros.Carteira.Descricao);

            // Executa a operação no banco.
            lDados.ExecuteNonQuery(lCommand);

            lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return lRetorno;
        }

        public BuscarClienteResumidoResponse BuscarClienteResumido(BuscarClienteResumidoRequest pRequest)
        {
            BuscarClienteResumidoResponse lResponse = new BuscarClienteResumidoResponse();

            try
            {
                ConsultarObjetosRequest<ClienteResumidoInfo> lRequest = new ConsultarObjetosRequest<ClienteResumidoInfo>();

                lRequest.Objeto = pRequest.DadosDoClienteParaBusca;

                switch (pRequest.DadosDoClienteParaBusca.TipoDeConsulta)
                {
                    case TipoDeConsultaClienteResumidoInfo.Clientes:

                        lResponse.Resultados = ConsultarClienteResumido_ViaAtributosDoCliente(lRequest).Resultado;

                        gLogger.InfoFormat("BuscarClienteResumido > retornou [{0}] clientes", lResponse.Resultados.Count);

                        break;

                    case TipoDeConsultaClienteResumidoInfo.ClientesPorAssessor:

                        lResponse.Resultados = ConsultarClienteResumido_ViaIdDoAssessor(lRequest).Resultado;

                        gLogger.InfoFormat("BuscarClienteResumido > retornou [{0}] clientes", lResponse.Resultados.Count);

                        break;
                    
                    case TipoDeConsultaClienteResumidoInfo.ClientesPorContaMaster:

                        lResponse.Resultados = ConsultarClienteResumido_ViaContaMaster(lRequest).Resultado;

                        gLogger.InfoFormat("BuscarClienteResumido > retornou [{0}] clientes", lResponse.Resultados.Count);

                        break;
                }

                

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("Erro em ServicoCadastroCliente.BuscarClienteResumido: [{0}]\r\n[{1}]", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarProvisionadosResponse BuscarProvisionados(BuscarProvisionadosRequest pRequest)
        {
            AcessaDados lDados = null;
            DataTable   lTable = null;
            DbCommand lCommand = null;

            BuscarProvisionadosResponse lResponse = new BuscarProvisionadosResponse();
            
            try
            {
                lDados = new AcessaDados();
                lDados.ConnectionStringName = "SINACOR2";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_VCFPOSI_PROV_PREV");

                if (!string.IsNullOrEmpty(pRequest.Ativo))
                    lDados.AddInParameter(lCommand, "pCod_Neg", DbType.String, pRequest.Ativo);

                if (pRequest.CBLC != null && pRequest.CBLC.Value > 0)
                    lDados.AddInParameter(lCommand, "pCD_CLIENTE", DbType.String, pRequest.CBLC);

                lTable = lDados.ExecuteOracleDataTable(lCommand);

                if (lTable.Rows.Count > 0)
                {
                    DataRow lRow;

                    ProvisionadoInfo lProvisionado;

                    for (int i = 0; i < lTable.Rows.Count; i++)
                    {
                        lRow = lTable.Rows[i];

                        lProvisionado = new ProvisionadoInfo();

                        lProvisionado.CBLC                      = lRow["cod_cli"].DBToInt32();
                        lProvisionado.CodigoCarteira            = lRow["cod_cart"].DBToInt32();
                        lProvisionado.Ativo                     = lRow["cod_neg"].DBToString();
                        lProvisionado.DescricaoTipoDeProvento   = lRow["desc_tipo_prov"].DBToString();
                        lProvisionado.Quantidade                = lRow["qtde_prov"].DBToInt32();
                        lProvisionado.Valor                     = lRow["val_prov"].DBToDecimal();
                        lProvisionado.ValorLiquido              = lRow["val_prov_liq"].DBToDecimal();
                        lProvisionado.DataPagto                 = lRow["data_pgto_divi"].DBToDateTime();
                        lProvisionado.DataAtu                   = lRow["data_atu"].DBToDateTime();
                        lProvisionado.DataDebSubs               = lRow["data_deb_subs"].DBToDateTime();

                        lResponse.Provisionados.Add(lProvisionado);

                        lResponse.DescricaoResposta = "Lista de provisionados atualizada com sucesso.";
                        lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                    }
                }
                else
                {
                    lResponse.DescricaoResposta = "Nenhum item encontrado para o filtro. Ativo:" + pRequest.Ativo + "; Cliente:" + pRequest.CBLC.Value + "; Data De:" + pRequest.DataPgtoIni + "; Data Até:" + pRequest.DataPgtoFim;
                    lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                return lResponse;
            }
            catch (Exception ex)
            {
                lResponse.DescricaoResposta = ex.Message + "\n\n" + ex.StackTrace;
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }
            return lResponse;
        }

        public ConsultarPosicaoConsolidadaPorPapelResponse ConsultarPosicaoConsolidadaPorPapel(ConsultarPosicaoConsolidadaPorPapelRequest pRequest)
        {
            ConsultarPosicaoConsolidadaPorPapelResponse lResponse = new ConsultarPosicaoConsolidadaPorPapelResponse();

            //ConsultarObjetosResponse<PosicaoConsolidadaPorPapelInfo> lRetorno;

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            lResponse.PosicoesPorPapel = new List<PosicaoConsolidadaPorPapelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_PosConsolidadaPorPapel_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCod_Neg",     DbType.String, pRequest.DadosParaConsulta.ConsultaInstrumento);
                lAcessaDados.AddInParameter(lDbCommand, "pCd_Assessor", DbType.Int32,  pRequest.DadosParaConsulta.ConsultaCodigoAssessor);

                DataTable lDatatable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDatatable && lDatatable.Rows.Count > 0)
                {
                    PosicaoConsolidadaPorPapelInfo lInfo;

                    foreach (DataRow lLinha in lDatatable.Rows)
                    {
                        lInfo = new PosicaoConsolidadaPorPapelInfo();
                        
                        lInfo.AssessorCodigo       = lLinha["CD_ASSESSOR"].DBToInt32();
                        lInfo.AssessorNome         = lLinha["NM_ASSESSOR"].DBToString();
                        lInfo.ClienteCodigo        = lLinha["COD_CLI"].DBToInt32();
                        lInfo.ClienteNome          = lLinha["NOME_CLI"].DBToString();
                        lInfo.ClienteTipo          = lLinha["DS_TIPO_CLIENTE"].DBToString();
                        lInfo.CodigoNegocio        = lLinha["COD_NEG"].DBToString();
                        lInfo.DescricaoCarteira    = lLinha["DESC_CART"].DBToString();
                        lInfo.Locador              = lLinha["LOCADOR"].DBToInt32();
                        lInfo.QuantidadeD1         = lLinha["QTDE_DA1"].DBToInt32();
                        lInfo.QuantidadeD2         = lLinha["QTDE_DA2"].DBToInt32();
                        lInfo.QuantidadeD3         = lLinha["QTDE_DA3"].DBToInt32();
                        lInfo.QuantidadeDisponivel = lLinha["QTDE_DISP"].DBToInt32();
                        lInfo.QuantidadeTotal      = lLinha["QTDE_TOT"].DBToInt32();

                        lResponse.PosicoesPorPapel.Add(lInfo);
                    }
                }
            }

            lResponse.StatusResposta = MensagemResponseStatusEnum.OK;

            return lResponse;
        }

        public BuscarHistoricoOperacoesResponse BuscarHistoricoOperacoes(BuscarHistoricoOperacoesRequest pRequest)
        {
            BuscarHistoricoOperacoesResponse lResponse = new BuscarHistoricoOperacoesResponse();
            
            HistoricoOperacoesInfo lHistorico = new HistoricoOperacoesInfo();

            AcessaDados lAcessaDados = new AcessaDados();
            try
            {

                lAcessaDados.ConnectionStringName = "DirectTrader";

                lResponse.HistoricoOperacoes = new List<HistoricoOperacoesInfo>();

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_HISTORICO_OPERACOES_LST"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@pDtDe", DbType.Date, pRequest.DtDe);
                    lAcessaDados.AddInParameter(lDbCommand, "@pDtAte", DbType.Date, pRequest.DtAte);
                    lAcessaDados.AddInParameter(lDbCommand, "@pAtivo", DbType.AnsiString, pRequest.Ativo);

                    DataTable lDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    foreach (DataRow lRow in lDados.Rows)
                    {
                        lHistorico = new HistoricoOperacoesInfo();

                        lHistorico.IdHistorico       = Convert.ToInt32(lRow["id_historico"]);
                        lHistorico.DsTituloHistorico = lRow["ds_titulohistorico"].DBToString();
                        lHistorico.DsImagemHistorico = lRow["ds_imagemhistorico"].Equals(DBNull.Value) ? null :  (byte[])lRow["ds_imagemhistorico"];
                        lHistorico.DtDataHistorico   = lRow["dt_datahistorico"].DBToDateTime();
                        lHistorico.DsAtivoHistorico  = lRow["ds_ativohistorico"].DBToString();
                        lHistorico.DsRtfHistorico    = lRow["ds_rtfhistorico"].Equals(DBNull.Value) ? null: (byte[])lRow["ds_rtfhistorico"];

                        lResponse.HistoricoOperacoes.Add(lHistorico);

                    }
                }

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public ExcluirHistoricoOperacoesResponse ExcluirHistoricoOperacoes(ExcluirHistoricoOperacoesRequest pRequest)
        {
            ExcluirHistoricoOperacoesResponse lResponse = new ExcluirHistoricoOperacoesResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = "DirectTrader";

                int count = 0;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_HISTORICO_OPERACOES_DEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@pIdHistorico", DbType.Int32, pRequest.IdHistorico);

                    count = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lResponse.ExcluidoComSucesso = (count > 0);

                lResponse.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
            
            return lResponse;
        }

        public SalvarHistoricoOperacoesResponse SalvarHistoricoOperacoes(SalvarHistoricoOperacoesRequest pRequest)
        {
            SalvarHistoricoOperacoesResponse lResponse = new SalvarHistoricoOperacoesResponse();

            lResponse.StatusResposta = MensagemResponseStatusEnum.OK;

            try
            {
                //Verifica se altera ou se inclui
                if (pRequest.HistoricoOperacoes.IdHistorico == null)
                {
                    lResponse = IncluirHistoricoOperacoes(pRequest);
                }
                else
                {
                    lResponse = AlterarHistoricoOperacoes(pRequest);
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                lResponse.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lResponse;
        }

        public BuscarUltimasNegociacoesResponse ConsultarUltimasNegociacoesCliente(BuscarUltimasNegociacoesRequest pParametros)
        {
            BuscarUltimasNegociacoesResponse lRetorno = new BuscarUltimasNegociacoesResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CLIENTE_DT_ULT_NEG"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE", DbType.String, pParametros.CdClienteBov);

                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_BMF", DbType.String, pParametros.CdClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                UltimasNegociacoesInfo lInfo = new UltimasNegociacoesInfo();

                foreach (DataRow row in lDataTable.Rows)
                {
                    lInfo = new UltimasNegociacoesInfo();

                    lInfo.CdCliente = row["CD_CLIENTE"].DBToInt32();

                    lInfo.TipoBolsa = row["TIPO_BOLSA"].DBToString();

                    lInfo.DtUltimasNegociacoes = row["DT_NEGOCIO"].DBToDateTime();

                    lRetorno.Negociacoes.Add(lInfo);
                }

            }

            return lRetorno;
        }

        public BuscarNegociacoesDiaClienteResponse ConsultarNegociacoesDiaClienteOnLine (BuscarNegociacoesDiaClienteRequest pParametros)
        {
            BuscarNegociacoesDiaClienteResponse lRetorno = new BuscarNegociacoesDiaClienteResponse();

            lock(gListaNegocios)
            {
                lRetorno.NegociosDia = new List<NegociosDiaClienteInfo>();

                var lNegocios = from negocio in gListaNegocios where pParametros.LstCodigoCliente.Contains(negocio.CodigoCliente) select negocio;

                lRetorno.NegociosDia.AddRange(lNegocios.ToArray());

                gLogger.InfoFormat("Encontrou {0} negocios realizados", lRetorno.NegociosDia.Count);
            }

            return lRetorno;
        }

        public BuscarNegociacoesDiaClienteResponse ConsultarNegociacoesDiaCliente(BuscarNegociacoesDiaClienteRequest pParametros)
        {
            BuscarNegociacoesDiaClienteResponse lRetorno = new BuscarNegociacoesDiaClienteResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            foreach (CodigoClienteBVMFInfo info in pParametros.LstCodigos )
            {
                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CLIENTE_NEGOCIOS"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, info.CdClienteBov);

                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE_BMF", DbType.Int32, info.CdClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    NegociosDiaClienteInfo lInfo = new NegociosDiaClienteInfo();

                    foreach (DataRow row in lDataTable.Rows)
                    {
                        lInfo = new NegociosDiaClienteInfo();

                        lInfo.CodigoCliente = row["CD_CLIENTE"].DBToInt32();
                    
                        lInfo.TipoBolsa      = row["TIPO_BOLSA"].DBToString();
                        
                        lInfo.Quantidade     = row["QT_NEGOCIO"].DBToInt32();
                    
                        lInfo.DtPregao       = row["DT_NEGOCIO"].DBToDateTime();
                    
                        lInfo.Papel          = row["CD_NEGOCIO"].ToString();

                        lInfo.Direcao        = row["CD_NATOPE"].ToString();

                        lInfo.PrecoNegociado = row["VL_NEGOCIO"].ToString();

                        lInfo.FatorCotacao   = row["VL_FATORCOTACAO"].DBToInt32();

                        lRetorno.NegociosDia.Add(lInfo);
                    }
                }
            }

            return lRetorno;
        }

        public PosicaoGTIAbertaExecutadaResponse BuscarPosicaoNetGTI(PosicaoGTIAbertaExecutadaRequest pParametros)
        {
            PosicaoGTIAbertaExecutadaResponse lRetorno = new PosicaoGTIAbertaExecutadaResponse();
            List<string> lListaPapel = new List<string>();


            try
            {
                lRetorno.Posicao = new List<PosicaoGTIAbertaExecInfo>();

                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "DirectTradeRisco";

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_papel_operado_dia_gti_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente_bmf", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    foreach (DataRow row in lDataTable.Rows)
                    {
                        string lPapel = row["symbol"].DBToString();

                        if (lPapel.LastIndexOf("F").Equals(lPapel.Length - 1))
                        {
                            lPapel = lPapel.Substring(0, lPapel.Length - 1);
                        }

                        if (!lListaPapel.Contains(lPapel) && !lListaPapel.Contains(string.Concat(lPapel,'F')))
                        {
                            lListaPapel.Add(lPapel);
                        }
                    }
                }

                foreach (string papel in lListaPapel)
                {
                    pParametros.Ativo = papel;

                    using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_calcular_posicao_aberta_executada_gti"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@ativo", DbType.String, pParametros.Ativo);

                        lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                        lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente_bmf", DbType.Int32, pParametros.CodigoClienteBmf);

                        var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        PosicaoGTIAbertaExecInfo lInfo = new PosicaoGTIAbertaExecInfo();

                        foreach (DataRow row in lDataTable.Rows)
                        {
                            lInfo = new PosicaoGTIAbertaExecInfo();

                            lInfo.CodigoCliente = pParametros.CodigoCliente;
                            lInfo.Papel = pParametros.Ativo;

                            if (row["side"].DBToInt32() == 1)//se for compra
                            {
                                if (row["Tipo"].DBToString().ToLower() == "aberta")
                                {
                                    lInfo.QtdeAbertaCompra = row["qtde"].DBToInt32();
                                    lInfo.PrecoMedioCompra = row["medio"].DBToDecimal();

                                }
                                else if (row["Tipo"].DBToString().ToLower() == "exec")
                                {
                                    lInfo.QtdeExecutadaCompra = row["qtde"].DBToInt32();
                                    lInfo.PrecoMedioCompra = row["medio"].DBToDecimal();
                                }
                            }
                            else if (row["side"].DBToInt32() == 2) //se for venda
                            {
                                if (row["Tipo"].DBToString().ToLower() == "aberta")
                                {
                                    lInfo.QtdeAbertaVenda = row["qtde"].DBToInt32();
                                    lInfo.PrecoMedioVenda = row["medio"].DBToDecimal();
                                }
                                else if (row["Tipo"].DBToString().ToLower() == "exec")
                                {
                                    lInfo.QtdeExecutadaVenda = row["qtde"].DBToInt32();
                                    lInfo.PrecoMedioVenda = row["medio"].DBToDecimal();
                                }
                            }

                            lRetorno.Posicao.Add(lInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro {0}", ex);
            }

            return lRetorno;
        }

        public PosicaoGTIAbertaExecutadaResponse BuscarPosicaoGTIAbertaExecutada(PosicaoGTIAbertaExecutadaRequest pParametros)
        {
            PosicaoGTIAbertaExecutadaResponse lRetorno = new PosicaoGTIAbertaExecutadaResponse();

            try
            {
                lRetorno.Posicao = new List<PosicaoGTIAbertaExecInfo>();

                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "DirectTradeRisco";

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_calcular_posicao_aberta_executada_gti"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ativo", DbType.String, pParametros.Ativo);

                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente_bmf", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    PosicaoGTIAbertaExecInfo lInfo = new PosicaoGTIAbertaExecInfo();

                    foreach (DataRow row in lDataTable.Rows)
                    {
                        lInfo = new PosicaoGTIAbertaExecInfo();

                        lInfo.CodigoCliente = pParametros.CodigoCliente;
                        lInfo.Papel = pParametros.Ativo;

                        if (row["side"].DBToInt32() == 1)//se for compra
                        {
                            if (row["Tipo"].DBToString().ToLower() == "aberta")
                            {
                                lInfo.QtdeAbertaCompra = row["qtde"].DBToInt32();
                                lInfo.PrecoMedioCompra = row["medio"].DBToDecimal();
                                
                            }
                            else if (row["Tipo"].DBToString().ToLower() == "exec")
                            {
                                lInfo.QtdeExecutadaCompra = row["qtde"].DBToInt32();
                                lInfo.PrecoMedioCompra    = row["medio"].DBToDecimal();
                            }
                        }
                        else if (row["side"].DBToInt32() == 2) //se for venda
                        {
                            if (row["Tipo"].DBToString().ToLower() == "aberta")
                            {
                                lInfo.QtdeAbertaVenda = row["qtde"].DBToInt32();
                                lInfo.PrecoMedioVenda = row["medio"].DBToDecimal();
                            }
                            else if (row["Tipo"].DBToString().ToLower() == "exec")
                            {
                                lInfo.QtdeExecutadaVenda = row["qtde"].DBToInt32();
                                lInfo.PrecoMedioVenda    = row["medio"].DBToDecimal();
                            }
                        }

                        lRetorno.Posicao.Add(lInfo);
                    }

                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro {0}",ex);
            }

            return lRetorno;
        }

        public PontaMesaEnvioClienteResponse BuscarPontaMesaEnvioCliente(PontaMesaEnvioClienteRequest pParametros)
        {
            PontaMesaEnvioClienteResponse lRetorno = new PontaMesaEnvioClienteResponse();

            try
            {
                lRetorno.ListaClientePontaMesa = new List<PontaMesaEnvioClienteInfo>();

                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "DirectTradeRisco";

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ordem_cliente_ponta_mesa_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, pParametros.PontaMesa.DsEmail);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    PontaMesaEnvioClienteInfo lInfo ; 

                    foreach (DataRow row in lDataTable.Rows)
                    {
                        lInfo = new PontaMesaEnvioClienteInfo();

                        lInfo.CodigoCliente = row["id_cliente"].DBToInt32();

                        lInfo.DsEmail = row["ds_email"].DBToString();

                        lRetorno.ListaClientePontaMesa.Add(lInfo);
                    }
                }
            }
            catch(Exception ex)
            {
                gLogger.ErrorFormat("BuscarPontaMesaEnvioCliente {0}", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.StackTrace;
            }

            return lRetorno;
        }

        public PontaMesaEnvioClienteResponse InserirPontaMesaEnvioCliente(PontaMesaEnvioClienteRequest pParametros)
        {
            PontaMesaEnvioClienteResponse lRetorno = new PontaMesaEnvioClienteResponse();
            AcessaDados lDados = new AcessaDados();

            try
            {
                lDados.ConnectionStringName = "DirectTradeRisco";

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_ordem_cliente_ponta_mesa_ins");

                lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pParametros.PontaMesa.CodigoCliente);
                lDados.AddInParameter(lCommand, "@ds_email"  , DbType.String, pParametros.PontaMesa.DsEmail);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                lRetorno.ListaClientePontaMesa = new List<PontaMesaEnvioClienteInfo>();

                lRetorno.ListaClientePontaMesa.Add(pParametros.PontaMesa);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("InserirPontaMesaEnvioCliente - {0}", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.StackTrace;
            }
            return lRetorno;
        }

        public AcompanhamentoGTIOrdenacaoResponse BuscarAcompanhamentoGTIOrdenacao(AcompanhamentoGTIOrdenacaoRequest pParametros )
        {
            AcompanhamentoGTIOrdenacaoResponse lRetorno = new AcompanhamentoGTIOrdenacaoResponse();

            try
            {
                lRetorno.ListaOrdenacao = new List<AcompanhamentoGTIOrdenacaoInfo>();

                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "DirectTradeConfiguracoes";

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ordenacao_acompanhamento_gti_cliente_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.String, pParametros.ListaOrdenacao[0].CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    AcompanhamentoGTIOrdenacaoInfo lInfo;

                    foreach (DataRow row in lDataTable.Rows)
                    {
                        lInfo = new AcompanhamentoGTIOrdenacaoInfo();

                        lInfo.CodigoCliente = row["cd_cliente"].DBToInt32();
                        lInfo.DisplayIndex  = row["nr_displayindex"].DBToInt32();
                        lInfo.NomeColuna    = row["ds_nomecoluna"].DBToString();
                        lInfo.Visible       = row["st_visible"].DBToBoolean();

                        lRetorno.ListaOrdenacao.Add(lInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("BuscarPontaMesaEnvioCliente {0}", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.StackTrace;
            }

            return lRetorno;
        }

        public AcompanhamentoGTIOrdenacaoResponse InserirAcompanhamentoGTIOrdenacao(AcompanhamentoGTIOrdenacaoRequest pParametros) 
        {
            AcompanhamentoGTIOrdenacaoResponse lRetorno = new AcompanhamentoGTIOrdenacaoResponse();
            AcessaDados lDados = new AcessaDados();
            lRetorno.ListaOrdenacao = new List<AcompanhamentoGTIOrdenacaoInfo>();

            try
            {
                lDados.ConnectionStringName = "DirectTradeConfiguracoes";

                using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_ordenacao_acompanhamento_gti_cliente_del"))
                {
                    lDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, pParametros.ListaOrdenacao[0].CodigoCliente);

                    // Executa a operação no banco.
                    lDados.ExecuteNonQuery(lCommand);
                }

                foreach (AcompanhamentoGTIOrdenacaoInfo info in pParametros.ListaOrdenacao)
                {
                    using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_ordenacao_acompanhamento_gti_cliente_ins"))
                    {
                        lDados.AddInParameter(lCommand, "@cd_cliente",      DbType.Int32,   info.CodigoCliente);
                        lDados.AddInParameter(lCommand, "@ds_nomecoluna",   DbType.String,  info.NomeColuna);
                        lDados.AddInParameter(lCommand, "@nr_displayindex", DbType.Int32,   info.DisplayIndex);
                        lDados.AddInParameter(lCommand, "@st_visible",      DbType.Boolean, info.Visible);

                        // Executa a operação no banco.
                        lDados.ExecuteNonQuery(lCommand);

                        lRetorno.ListaOrdenacao.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("InserirPontaMesaEnvioCliente - {0}", ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.StackTrace;
            }
            return lRetorno;
        }
        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                //gTimerNegocios = new Timer(1000);

                //gTimerNegocios.Elapsed += new ElapsedEventHandler(VerificaOrdemExecutadaSinacor);

                //gTimerNegocios.Interval = TemporizadorIntervaloVerificacao;

                //gTimerNegocios.Enabled = true;

                gServicoStatus = ServicoStatus.EmExecucao;

                gLogger.InfoFormat("Iniciando o serviço de cadastro de clientes");

            }
            catch(Exception ex)
            {
                gLogger.ErrorFormat("Erro ao iniciar o serviço de cadastro de clientes - ", ex);
            }
            
        }

        public void PararServico()
        {
            try
            {
                gServicoStatus = ServicoStatus.Parado;

                gLogger.InfoFormat("Parando o serviço de cadastro de clientes");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat(" Erro ao parar o serviço de cadastro de clientes  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return gServicoStatus;
        }

        #endregion


        public ObterTelefonePrincipalResponse ObterTelefonePrincipal(ObterTelefonePrincipalRequest pParametros)
        {
            ObterTelefonePrincipalResponse lResposta = new ObterTelefonePrincipalResponse();
            ClienteTelefoneInfo lTelPrincipal = new ClienteTelefoneInfo();
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "Cadastro";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_telefone_pri_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lTelPrincipal = CriarRegistroTelefone(lDataTable.Rows[0]);
            }
            lResposta.TelefonePrincipal = lTelPrincipal;
            return lResposta;
        }

        private static ClienteTelefoneInfo CriarRegistroTelefone(DataRow linha)
        {
            return new ClienteTelefoneInfo()
            {
                DsDdd = linha["ds_ddd"].DBToString(),
                DsNumero = linha["ds_numero"].DBToString(),
                DsRamal = linha["ds_ramal"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                IdTelefone = linha["id_telefone"].DBToInt32(),
                IdTipoTelefone = linha["id_tipo_telefone"].DBToInt32(),
                StPrincipal = linha["st_principal"].DBToBoolean()
            };
        }
    }
}
