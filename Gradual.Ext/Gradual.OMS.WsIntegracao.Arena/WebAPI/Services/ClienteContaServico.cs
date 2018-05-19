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
    }
}