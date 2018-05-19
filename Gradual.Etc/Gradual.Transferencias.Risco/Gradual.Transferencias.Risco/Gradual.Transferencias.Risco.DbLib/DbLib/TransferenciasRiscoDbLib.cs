using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
//using Gradual.Generico.Geral;
using System.Data;
using System.Data.Common;
using Gradual.Transferencias.Risco.Lib.Dados;

namespace Gradual.Transferencias.Risco.DbLib
{
    /// <summary>
    /// Classe de persistencia de banco de dados de controle de tranferencia de arquivos 
    /// </summary>
    public class TransferenciasRiscoDbLib : TransferenciasRiscoDbLibBase
    {
        /// <summary>
        /// Efetua a consulta no banco de dados para ir buscar o código ISIN do papel da posição do cliente
        /// </summary>
        /// <param name="pCodigoNegocio">Código do Papel</param>
        /// <param name="pCodigoCliente">Código do cliente</param>
        /// <returns>Retorna o Código isin do papel da posição do cliente</returns>
        public TradeData ConsultarCodigoISIN(string pCodigoNegocio, int pCodigoCliente)
        {
            var lRetorno = new TradeData();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = base.ConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUSTODIA_BOV_ISIN"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pCodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoNegocio", DbType.String, pCodigoNegocio);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno.CodigoISIN         = lRow["cod_isin"].ToString();
                        lRetorno.CodigoDistribuicao = lRow["NUM_DIST"].ToString();
                        lRetorno.CodigoNegocio      = lRow["COD_NEG"].ToString();
                    }
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Consulta os dados de número de processo
        /// </summary>
        /// <param name="pCodigoNegocio">Código do negócio do provento</param>
        /// <param name="pCodigoCliente">Código do cliente</param>
        /// <returns>Retorna o código do processo para ser inserido no arquivo ITRE</returns>
        public string ConsultarNumeroProcessoProventos(string pCodigoNegocio, int pCodigoCliente )
        {
            var lRetorno = string.Empty;

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = base.ConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUSTODIA_PROV_NPROCESSO"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pCodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoNegocio", DbType.String, pCodigoNegocio);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno = lRow["num_proc_atlz"].ToString();
                    }
                }

            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar dados do Cliente
        /// </summary>
        /// <param name="CodigoCliente">Código do Cliente</param>
        /// <returns>Retorna os dados do cliente</returns>
        public List<ClientData> ConsultarDadosCliente(string ListaCodigoCliente)
        {
            var lRetorno = new List<ClientData>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = base.ConexaoSinacor;

            string lSql = @"SELECT BOL.cd_cliente, plural.cd_clie_OUTR_BOLSA,bol.dv_cliente,plural.DV_CLIE_OUTR_BOLSA FROM tscclibol bol "+
                          @" inner join   TSCCBOUTP plural on bol.cd_cliente = plural.cd_cliente"+
                          @" WHERE bol.cd_cliente in  ( " + ListaCodigoCliente + ")";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                var lDataTable = lAcessaDados.ExecuteOracleDataTableWithoutCursor(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        
                        var lData = new ClientData();

                        lData.CodigoClienteGradual = Convert.ToInt32(lRow["cd_cliente"]);
                        lData.CodigoClientePlural  = Convert.ToInt32(lRow["cd_clie_OUTR_BOLSA"]);
                        lData.DigitoClienteGradual = Convert.ToInt32(lRow["dv_cliente"]);
                        lData.DigitoClientePlural  = Convert.ToInt32(lRow["DV_CLIE_OUTR_BOLSA"]);

                        lRetorno.Add(lData);
                    }
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Consultar dados cliente 
        /// </summary>
        /// <param name="CodigoCliente">Código do cliente</param>
        /// <returns>Retorno um objeto clientData preenchido com os dados de contas gradual e plural e seu devidos digitos</returns>
        public ClientData ConsultarDadosCliente (int CodigoCliente)
        {
            var lRetorno = new ClientData();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = base.ConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_PLURAL_GRADUAL_SEL"))
            {
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lData = new ClientData();

                        lData.CodigoClienteGradual = Convert.ToInt32(lRow["cd_cliente"]);
                        lData.CodigoClientePlural  = Convert.ToInt32(lRow["cd_clie_OUTR_BOLSA"]);
                        lData.DigitoClienteGradual = Convert.ToInt32(lRow["dv_cliente"]);
                        lData.DigitoClientePlural  = Convert.ToInt32(lRow["DV_CLIE_OUTR_BOLSA"]);

                        lRetorno = lData;
                    }
                }
            }

            return lRetorno;

        }
    }
}
