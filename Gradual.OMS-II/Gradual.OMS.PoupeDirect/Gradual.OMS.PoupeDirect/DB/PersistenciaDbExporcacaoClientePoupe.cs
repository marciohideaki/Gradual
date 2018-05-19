using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.PoupeDirect.App_Codigo;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using log4net;

namespace Gradual.OMS.PoupeDirect.DB
{
    public class PersistenciaDbExporcacaoClientePoupe
    {
        #region | Atributos

        private int gCdEmpresa = default(int);

        private int gUsuarioSinacor = default(int);

        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<CriticasExportacao> gCriticasExportacao = new List<CriticasExportacao>();

        private List<ClienteExportacaoInfo> gClienteExportacaoTodosInfo = new List<ClienteExportacaoInfo>();

        private List<ClienteExportacaoInfo> gClienteExportacaoSinacorInfo = new List<ClienteExportacaoInfo>();

        private DbTransaction gDbTransactionOracle;

        private DbConnection gDbConnectionOracle;

        private DbTransaction gDbTransactionSql;

        private DbConnection gDbConnectionSql;

        #endregion

        #region | Propriedades

        private DbTransaction GetDbTransactionOracle
        {
            get
            {
                if (null == this.gDbTransactionOracle || null == this.gDbTransactionOracle.Connection) //--> Criando a transação.
                    this.gDbTransactionOracle = this.GetDbConnectionOracle.BeginTransaction();

                return this.gDbTransactionOracle;
            }
        }

        private DbConnection GetDbConnectionOracle
        {
            get
            {
                if (null == this.gDbConnectionOracle)
                {
                    var lConexao = new Conexao();
                    lConexao._ConnectionStringName = PersistenciaDB.ConexaoOracle;
                    this.gDbConnectionOracle = lConexao.CreateIConnection();
                    this.gDbConnectionOracle.Open();
                }

                return this.gDbConnectionOracle;
            }
        }

        private DbTransaction GetDbTransactionSql
        {
            get
            {
                if (null == this.gDbTransactionSql || null == this.gDbTransactionSql.Connection) //--> Criando a transação.
                    this.gDbTransactionSql = this.GetDbConnectionSql.BeginTransaction();

                return this.gDbTransactionSql;
            }
        }

        private DbConnection GetDbConnectionSql
        {
            get
            {
                if (null == this.gDbConnectionSql)
                {
                    var lConexao = new Conexao();
                    lConexao._ConnectionStringName = PersistenciaDB.ConexaoSQL;
                    this.gDbConnectionSql = lConexao.CreateIConnection();
                    this.gDbConnectionSql.Open();
                }

                return this.gDbConnectionSql;
            }
        }

        private int GetUsuarioSinacor
        {
            get
            {
                if (this.gUsuarioSinacor == 0)
                    this.gUsuarioSinacor = ConfigurationManager.AppSettings["CD_USUARIO_SINACOR"].DBToInt32();

                return this.gUsuarioSinacor;
            }
        }

        private int GetCdRelatorio
        {
            get { return 2; }
        }

        private int GetCdEmpresa
        {
            get
            {
                if (this.gCdEmpresa == 0)
                    this.gCdEmpresa = ConfigurationManager.AppSettings["CD_EMPRESA"].DBToInt32();

                return this.gCdEmpresa;
            }
        }

        private int GetCdOrigem
        {
            get { return 1; }
        }

        #endregion

        #region | Métodos Logs

        private void GravarLogDeErro()
        {
            if (null != this.gCriticasExportacao && this.gCriticasExportacao.Count > 0)
                this.gCriticasExportacao.ForEach(lCritica =>
                {
                    gLogger.Error(string.Format("Tabela: {0} - Cliente {1}", lCritica.Sistema, lCritica.ClienteExportacaoInfo.CdCliente.DBToString()), lCritica.Excecao);
                });

            this.gCriticasExportacao.Clear();
        }

        private void AtualizarStatusDoClienteExportado(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando atualizar status dos clientes exportados");

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionSql, CommandType.StoredProcedure, "cliente_exportacao_status_solicitacao_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CdCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionSql);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TB_CLIENTE_PRODUTO",
                });
            }
        }

        private void GravarReferenciaDoCodigoDoClienteExportado(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando atualizar referência de códigos dos clientes exportados");

            try
            {
                var lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionSql, CommandType.StoredProcedure, "cliente_codigo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CdCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionSql);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TB_CLIENTE_PRODUTO",
                });
            }
        }

        private void GravarHistoricoExportacao(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando exportar para HISTÓRICO");

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionSql, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_SOLICITACAO_HISTORICO"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "P_ID_CLIENTE_PRODUTO", DbType.Int32, pParametro.Objeto.IdClienteProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "P_ID_SOLICITACAO", DbType.Int32, 2);
                    lAcessaDados.AddInParameter(lDbCommand, "P_ID_PRODUTO", DbType.Int32, pParametro.Objeto.IdProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "P_ID_CLIENTE", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "P_ID_ATIVO", DbType.String, pParametro.Objeto.IdAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "P_DS_HISTORICO", DbType.String, "Cliente Poupe exportado para o Sinacor");
                    lAcessaDados.AddInParameter(lDbCommand, "P_DT_SOLICITACAO", DbType.DateTime, pParametro.Objeto.DtSolicitacao);
                    lAcessaDados.AddInParameter(lDbCommand, "P_DT_EFETIVACAO", DbType.DateTime, DateTime.Now);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionSql);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TB_CLIENTE_SOLICITACAO_HISTORICO",
                });
            }
        }

        #endregion

        #region | Métodos Exportação

        private void CarregarClienteParaExportacao()
        {
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoSQL;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_produto_exportacao_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        foreach (DataRow lLinha in lDataTable.Rows)
                            gClienteExportacaoTodosInfo.Add(new ClienteExportacaoInfo()
                            {
                                CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                                CdCliente = lLinha["id_cliente"].DBToInt32(),
                                CdClientePoupe = lLinha["id_cliente"].DBToInt32() + 300000,
                                CdCpfCnpj = lLinha["ds_cpfcnpj"].DBToString().ToCpfCnpjString(),
                                CdEmpresa = this.GetCdEmpresa,
                                CdOrigem = this.GetCdOrigem,
                                CdRelatorio = this.GetCdRelatorio,
                                CdSistema = string.Empty,
                                CdUsuario = this.GetUsuarioSinacor,
                                DtNascimentoFundacao = lLinha["dt_nascimentofundacao"].DBToDateTime(),
                                DvCliente = lLinha["id_cliente"].ToCodigoClienteDigito(),
                                DvClientePoupe = (lLinha["id_cliente"].DBToInt32() + 300000).ToCodigoClienteDigito(),
                                InAdmCarteira = lLinha["ds_autorizadooperar"].DBToInt32() == 0 ? "N" : "S",
                                InCarteiraPropria = lLinha["st_carteirapropria"].DBToBoolean() ? "S" : "N",
                                InClienteEstrangeiro = lLinha["cd_paisnascimento"].DBToString() == "BRA" ? "N" : "S",
                                TpCliente = lLinha["tp_cliente"].DBToInt32(),
                                TpOcorrencia = string.Empty,
                                DtSolicitacao = lLinha["dt_solicitacao"].DBToDateTime(),
                                IdAtivo = lLinha["id_ativo"].DBToString(),
                                IdClienteProduto = lLinha["id_cliente_produto"].DBToInt32(),
                                IdProduto = lLinha["Id_Produto"].DBToInt32(),
                                IdSolicitacao = 2,
                                CdBanco = lLinha["cd_banco"].DBToString(),
                                DsAgencia = lLinha["ds_agencia"].DBToString(),
                                DsAgenciaDigito = lLinha["ds_agencia_digito"].DBToString(),
                                DsConta = lLinha["ds_conta"].DBToString(),
                                DsContaDigito = lLinha["ds_conta_digito"].DBToString(),
                                TpConta = lLinha["tp_conta"].DBToString(),
                            });
                }

                var lListaCodigoCliente = new List<int>();

                if (null != gClienteExportacaoTodosInfo && gClienteExportacaoTodosInfo.Count > 0)
                    gClienteExportacaoTodosInfo.ForEach(lClienteExportacao =>
                    {
                        if (!lListaCodigoCliente.Contains(lClienteExportacao.CdCliente))
                        {
                            this.gClienteExportacaoSinacorInfo.Add(lClienteExportacao);
                            lListaCodigoCliente.Add(lClienteExportacao.CdCliente);
                        }
                    });
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    Excecao = ex,
                    Sistema = "SQL Carga de dados",
                });
            }
        }

        public ExportarClientePoupeResponse ExportarClientePoupe(ExportarClientePoupeRequest pParametro)
        {
            var lRetorno = new ExportarClientePoupeResponse();

            this.CarregarClienteParaExportacao();

            this.gLogger.DebugFormat("Tentando migrar {0} clientes", this.gClienteExportacaoSinacorInfo.Count.ToString());

            if (this.gClienteExportacaoSinacorInfo.Count > 0)
            {
                this.gClienteExportacaoSinacorInfo.ForEach(lClienteExportacaoSinacor =>
                {   //--> Realizando a exportação
                    this.ExpotarClienteCC(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExpotarClienteBol(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExpotarClienteCus(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExportarEmail(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExportarAgentes(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExportarContasBancarias(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.ExportarReferenciasDeSistemas(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.GravarHistoricoExportacao(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.GravarReferenciaDoCodigoDoClienteExportado(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.AtualizarStatusDoClienteExportado(new ExportarClientePoupeRequest() { Objeto = lClienteExportacaoSinacor });
                    this.PesisitirAcoesNoBanco();

                    //gClienteExportacaoSinacorInfo.Clear();
                });

                this.GetDbTransactionSql.Dispose();
                this.GetDbTransactionOracle.Dispose();

                this.GetDbTransactionSql.Dispose();
                this.GetDbConnectionOracle.Dispose();

                if (this.GetDbConnectionSql.State != ConnectionState.Closed)
                    this.GetDbConnectionSql.Close();

                if (this.GetDbConnectionOracle.State != ConnectionState.Closed)
                    this.GetDbConnectionOracle.Close();

                this.gLogger.Debug(string.Format("Foram migrados {0} clientes em {1}", this.gClienteExportacaoSinacorInfo.Count.ToString(), DateTime.Now.ToString("dd/MM/yyyy")));
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExpotarClienteBol(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando exportar para TSCCLIBOL");

            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_exp_cli_bol_poupedirect"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente_poupe", DbType.Int32, pParametro.Objeto.DvClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente", DbType.Int32, pParametro.Objeto.DvCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cpfcgc", DbType.Int64, pParametro.Objeto.CdCpfCnpj.DBToInt64());
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_nasc_func", DbType.Date, pParametro.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_empresa", DbType.Int32, pParametro.Objeto.CdEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_usuario", DbType.Int32, pParametro.Objeto.CdUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_origem", DbType.Int32, pParametro.Objeto.CdOrigem);
                    lAcessaDados.AddInParameter(lDbCommand, "pin_adm_cart", DbType.String, pParametro.Objeto.InAdmCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "pin_clie_estr", DbType.String, pParametro.Objeto.InClienteEstrangeiro);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCLIBOL",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExpotarClienteCC(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando exportar para TSCCLICC");

            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_exp_cli_cc_poupedirect"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente_poupe", DbType.Int32, pParametro.Objeto.DvClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente", DbType.Int32, pParametro.Objeto.DvCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cpfcgc", DbType.Int64, pParametro.Objeto.CdCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_nasc_fund", DbType.Date, pParametro.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_empresa", DbType.Int32, pParametro.Objeto.CdEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_usuario", DbType.Int32, pParametro.Objeto.CdUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_origem", DbType.Int32, pParametro.Objeto.CdOrigem);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCLICC",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExpotarClienteCus(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando exportar para TSCCLICUS");

            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_exp_cli_cus_poupedirect"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_nasc_fund", DbType.Date, pParametro.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente_poupe", DbType.Int32, pParametro.Objeto.DvClientePoupe);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.CdAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pdv_cliente", DbType.Int32, pParametro.Objeto.DvCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_usuario", DbType.Int32, pParametro.Objeto.CdUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_empresa", DbType.Int32, pParametro.Objeto.CdEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cpfcgc", DbType.Int64, pParametro.Objeto.CdCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_origem", DbType.Int32, pParametro.Objeto.CdOrigem);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCLICUS",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExportarContasBancarias(ExportarClientePoupeRequest pParametro)
        {
            this.gLogger.Debug("Tentando exportar para TSCCLICTA");

            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exportacao_contabanc"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCLICTA",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExportarReferenciasDeSistemas(ExportarClientePoupeRequest pParametro)
        {
            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCCLISIS");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exportar_clisis"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCLISIS",
                });
            }

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCRELACATVD");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exp_relacaotvd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCRELACATVD",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExportarEmail(ExportarClientePoupeRequest pParametro)
        {
            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCEMAILBOL");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exporta_emailbol"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCEMAILBOL",
                });
            }

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCEMAILCC");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exporta_emailcc"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCEMAILCC",
                });
            }

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCEMAILCUS");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_poupe_exporta_emailcus"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCEMAILCUS",
                });
            }

            return lRetorno;
        }

        private ExportarClientePoupeResponse ExportarAgentes(ExportarClientePoupeRequest pParametro)
        {
            var lRetorno = new ExportarClientePoupeResponse();

            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = PersistenciaDB.ConexaoOracle;

            try
            {
                this.gLogger.Debug("Tentando exportar para TSCCBASAG");

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(this.GetDbTransactionOracle, CommandType.StoredProcedure, "prc_exp_cli_agente_poupedirect"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametro.Objeto.CdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente_poupe", DbType.Int32, pParametro.Objeto.CdClientePoupe);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, this.GetDbTransactionOracle);
                }
            }
            catch (Exception ex)
            {
                this.gCriticasExportacao.Add(new CriticasExportacao()
                {
                    ClienteExportacaoInfo = pParametro.Objeto,
                    Excecao = ex,
                    Sistema = "TSCCBASAG",
                });
            }

            return lRetorno;
        }

        private void PesisitirAcoesNoBanco()
        {   //--> Validando os error
            if (this.gCriticasExportacao.Count > 0)
            {
                this.GetDbTransactionOracle.Rollback();
                this.GetDbTransactionSql.Rollback();
                this.GravarLogDeErro();
            }
            else
            {
                this.GetDbTransactionOracle.Commit();
                this.GetDbTransactionSql.Commit();
            }
        }

        #endregion

        #region | Estruturas

        public struct CriticasExportacao
        {
            public ClienteExportacaoInfo ClienteExportacaoInfo { get; set; }

            public Exception Excecao { get; set; }

            public String Sistema { get; set; }
        }

        #endregion
    }
}
