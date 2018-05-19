using System;

using Gradual.OMS.DepositosRetiradas.Lib;
using Gradual.OMS.DepositosRetiradas.Lib.Mensagens;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.OMS.DepositosRetiradas.Lib.Dados;
using Gradual.Generico.Geral;
using log4net;

namespace Gradual.OMS.DepositosRetiradas
{
    public class ServicoDepositosRetiradas : IServicoDepositosRetiradas, IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IServicoDepositosRetiradas Members

        public BuscarContasBancariasDoClienteResponse BuscarContasBancariasDoCliente(BuscarContasBancariasDoClienteRequest pRequest)
        {
            AcessaDados lDados = null;
            DataTable lTable = null;
            DbCommand lCommand = null;

            BuscarContasBancariasDoClienteResponse lResponse = new BuscarContasBancariasDoClienteResponse();
            ContaBancaria lConta;

            lResponse.StatusResposta = Library.MensagemResponseStatusEnum.OK;

            try
            {
                lDados = new AcessaDados("ACCOUNT");
                lDados.ConnectionStringName = "SINACOR";

                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_SINACOR_SEL_CONTAS");

                logger.Info("Buscando informacoes de contas do cliente [" + pRequest.CodigoCblcDoCliente + "]");

                lDados.AddInParameter(lCommand, "cd_cliente", DbType.Int32, pRequest.CodigoCblcDoCliente);

                lTable = lDados.ExecuteOracleDataTable(lCommand);

                logger.Info("Cliente [" + pRequest.CodigoCblcDoCliente + "] buscou " + lTable.Rows.Count + " registros");

                foreach (DataRow lRow in lTable.Rows)
                {
                    lConta = new ContaBancaria();

                    lConta.CodigoDaEmpresa = Conversao.ToInt32(lRow["cd_empresa"]);

                    lConta.NumeroDaAgencia = Conversao.ToString(lRow["cd_agencia"]);
                    lConta.DigitoDaAgencia = Conversao.ToString(lRow["dv_agencia"]);
                    lConta.NumeroDaConta   = Conversao.ToString(lRow["nr_conta"]);
                    lConta.DigitoDaConta   = Conversao.ToString(lRow["dv_conta"]);
                    lConta.NomeDoBanco     = Conversao.ToString(lRow["nm_banco"]);
                    lConta.CodigoDoBanco   = Conversao.ToString(lRow["cd_banco"]);

                    lResponse.Contas.Add(lConta);
                }
            }
            catch (Exception ex)
            {
                lResponse.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
                lResponse.DescricaoResposta = string.Format("Erro ao buscar contas bancárias: [{0}]\r\n{1}", ex.Message, ex.StackTrace);
                logger.Error("BuscarContasBancariasDoCliente()" + ex.Message, ex);
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
