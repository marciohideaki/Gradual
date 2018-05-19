#region Includes
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Gradual.Intranet.Contratos.Dados.Monitoramento;
using Gradual.Intranet.Contratos.Mensagens;
#endregion 

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class MonitoramentoDesbloqueioCustodiaDbLib
    {
        #region Propriedades
        public static string gNomeConexaoRisco
        {
            get { return "Risco"; }
        }
        #endregion

        #region DesbloqueiaCustodia
        public static SalvarEntidadeCadastroResponse DesbloqueiaCustodia(SalvarEntidadeCadastroRequest<MonitoramentoDesbloqueioCustodiaInfo> pParametros)
        {
            SalvarEntidadeCadastroResponse lRetorno = 
                new SalvarEntidadeCadastroResponse();
            
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoRisco;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_desbloqueia_custodia_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@QtdeSolicitada", DbType.Int32, pParametros.EntidadeCadastro.Quantidade);

                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, pParametros.EntidadeCadastro.CodBovespa);

                    lAcessaDados.AddInParameter(lDbCommand, "@Instrumento", DbType.String, pParametros.EntidadeCadastro.Instrumento);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.DataResposta = DateTime.Now;

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                    lRetorno.DescricaoResposta = "Desbloqueio efetuado com sucesso";
                }

            }
            catch (SqlException sqlEx)
            {
                lRetorno.DataResposta = DateTime.Now;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroNegocio;

                lRetorno.DescricaoResposta = sqlEx.Message;
            }
            catch (Exception ex)
            {
                lRetorno.DataResposta = DateTime.Now;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;

                lRetorno.DescricaoResposta = ex.Message;
            }

            return lRetorno;
        }
        #endregion
    }
}
