using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_Test.Models;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using System.Data;
using System.Data.Common;
using Gradual.OMS.WsIntegracao.Arena.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public class ClienteContaServico : ServicoBase
    {
        #region Propriedades
        
        #endregion

        #region Metodos
        public static Cliente ConsultarClienteConta(int IdClienteGradual)
        {
            var lRetorno = new Cliente();
            try
            {
                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_sel_conta_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, IdClienteGradual);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow lRow = lDataTable.Rows[i];

                            lRetorno.ID              = IdClienteGradual;
                            lRetorno.NomeCliente     = lRow["ds_nome"].DBToString();
                            //lRetorno.StatusCliente = lRow["st_ativo"].DBToBoolean();
                            lRetorno.TipoCliente     = lRow["tp_cliente"]   .DBToString();
                            lRetorno.Assessor        = lRow["cd_assessor"]  .DBToString();
                            lRetorno.CpfCnpj         = lRow["ds_cpfcnpj"]   .DBToString();
                            lRetorno.Phone           = lRow["Phone"]        .DBToString();

                            if (lRow["cd_sistema"].DBToString() == "BOL")
                            {
                                lRetorno.CodigoBovespa = lRow["cd_codigo"].DBToInt32();

                                ObterDadosClienteSinacor(lRetorno.CodigoBovespa, ref lRetorno);
                            }
                            else if (lRow["cd_sistema"].DBToString() == "BMF")
                            {
                                lRetorno.CodigoBmf = lRow["cd_codigo"].DBToInt32();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        public static Cliente ConsultarClienteContaLogin(int IdClienteLogin)
        {
            var lRetorno = new Cliente();
            try
            {
                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_sel_conta_login_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, IdClienteLogin);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow lRow = lDataTable.Rows[i];

                            lRetorno.ID            = lRow["id_cliente"].DBToInt32();
                            lRetorno.NomeCliente   = lRow["ds_nome"].DBToString();
                            //lRetorno.StatusCliente = lRow["st_ativo"].DBToBoolean();
                            lRetorno.TipoCliente   = lRow["tp_pessoa"].DBToString();
                            lRetorno.Assessor      = lRow["cd_assessor"].DBToString();
                            lRetorno.CpfCnpj       = lRow["ds_cpfcnpj"].DBToString();

                            if (lRow["cd_sistema"].DBToString() == "BOL")
                            {
                                lRetorno.CodigoBovespa = lRow["cd_codigo"].DBToInt32();

                                ObterDadosClienteSinacor(lRetorno.CodigoBovespa, ref lRetorno);
                            }
                            else if (lRow["cd_sistema"].DBToString() == "BMF")
                            {
                                lRetorno.CodigoBmf = lRow["cd_codigo"].DBToInt32();
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        public static void ObterDadosClienteSinacor(int? CodigoCliente, ref Cliente pRetorno)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente = (lDataTable.Rows[i]["Tipo"]).DBToString();

                            pRetorno.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            pRetorno.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            pRetorno.Assessor       = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                pRetorno.CodigoBovespa = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                pRetorno.StatusBovespa = (lDataTable.Rows[i]["situac"]).DBToString().ToUpper() == "A" ? true :false;
                            }
                            else
                            {
                                pRetorno.CodigoBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                pRetorno.StatusBmf = (lDataTable.Rows[i]["situac"]).DBToString().ToUpper() == "A"? true : false;
                            }

                            pRetorno.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            pRetorno.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }
        
        public static List<PosicaoCliente> ObterDadosClientePosicao()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            var lRetorno = new List<PosicaoCliente>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cliente_asse_monitor"))
                {
                    //lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            var lPosicao = new PosicaoCliente();

                            lPosicao.CodigoBovespaCliente = (lDataTable.Rows[i]["Account"]).DBToInt32();
                            
                            lPosicao.SaldoCustodiaBovespaCliente = new List<SaldoCustodiaBovespaCliente>();
                            lPosicao.SaldoCustodiaBmfCliente     = new List<SaldoCustodiaBmfCliente>();
                            lPosicao.SaldoFinanceiro             = new SaldoFinanceiroCliente();

                            

                            //lPosicao.SaldoCustodiaBovespaCliente.Add(lPosicaoCustodia);

                            lRetorno.Add(lPosicao);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;
        }

        private static void ObterPosicaoClientePosicao( ref PosicaoCliente pCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_POS_CLIENTE_ABERT_RISCO"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string lTipoGrupo =  (lDataTable.Rows[i]["Tipo_Grup"]).DBToString();

                            switch (lTipoGrupo)
                            {
                                case "ACAO": 
                                    {
                                        var lPosicaoBov = new SaldoCustodiaBovespaCliente();

                                        lPosicaoBov.Ativo          = (lDataTable.Rows[i]["COD_NEG"]).DBToString();
                                        lPosicaoBov.CodigoCarteira = (lDataTable.Rows[i]["COD_CART"]).DBToString();
                                        lPosicaoBov.SaldoAbertura  = (lDataTable.Rows[i]["QTDE_DISP"]).DBToInt32();
                                        lPosicaoBov.SaldoD0        = (lDataTable.Rows[i]["QTDE_DISP"]).DBToInt32();
                                        lPosicaoBov.SaldoD1        = (lDataTable.Rows[i]["QTDE_DA1"]).DBToInt32();
                                        lPosicaoBov.SaldoD2        = (lDataTable.Rows[i]["QTDE_DA2"]).DBToInt32();
                                        lPosicaoBov.SaldoD3        = (lDataTable.Rows[i]["QTDE_DA3"]).DBToInt32();
                                        lPosicaoBov.SaldoTotal     = lPosicaoBov.SaldoD0 + lPosicaoBov.SaldoD1 + lPosicaoBov.SaldoD2 +lPosicaoBov.SaldoD3;
                                        
                                        pCliente.SaldoCustodiaBovespaCliente.Add(lPosicaoBov);
                                    }
                                    break;

                                case "BMF": 

                                    pCliente.CodigoBmfCliente = (lDataTable.Rows[i]["Tipo_Grup"]).DBToInt32();
                                    
                                    var lPosicaoBmf = new SaldoCustodiaBmfCliente();

                                    lPosicaoBmf.Ativo              = (lDataTable.Rows[i]["COD_COMM"].DBToString() + lDataTable.Rows[i]["COD_SERI"]).DBToString();
                                    lPosicaoBmf.TipoMercadoria     = lDataTable.Rows[i]["NOME_COMM"].DBToString();
                                    lPosicaoBmf.Serie              = lDataTable.Rows[i]["COD_SERI"].DBToString();
                                    lPosicaoBmf.Ajuste             = 0;//lDataTable.Rows[i][]
                                    lPosicaoBmf.PU                 = 0;
                                    lPosicaoBmf.QuantidadeAbertura = lDataTable.Rows[i]["QTDE_DISP"].DBToInt32();
                                    lPosicaoBmf.QuantidadeAtual = lDataTable.Rows[i]["QTDE_DISP"].DBToInt32();

                                    pCliente.SaldoCustodiaBmfCliente.Add(lPosicaoBmf);
                                    break;

                                case "TEDI": 
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}