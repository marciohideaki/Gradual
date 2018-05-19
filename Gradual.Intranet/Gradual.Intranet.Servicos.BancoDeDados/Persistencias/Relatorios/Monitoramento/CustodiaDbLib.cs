using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Monitoramento
{
    public class CustodiaDbLib
    {
        public static string gNomeConexao = "SINACOR";

        #region | ConsultarCustodia
        /// <summary>        
        /// Lista os clientes com os codigos na Gradual e na corretora externa
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteDeParaInfo</param>
        /// <returns>Retorna a lista de clientes</returns>
        public static Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo> ConsultarCustodia(Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request.ConsultarEntidadeRequest<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo> pParametros)
        {
            Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo> lResposta = new Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexao;

            using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_CUSTODIA_LST"))
            {
                if (null != pParametros.Objeto.CodigoAssessor && !pParametros.Objeto.CodigoAssessor.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_ASSESSOR", System.Data.DbType.Int32, pParametros.Objeto.CodigoAssessor);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_ASSESSOR", System.Data.DbType.Int32, null);
                }

                if (null != pParametros.Objeto.CodigoCliente && !pParametros.Objeto.CodigoCliente.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_CLIENTE", System.Data.DbType.Int32, pParametros.Objeto.CodigoCliente);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_CLIENTE", System.Data.DbType.Int32, null);
                }

                if (null != pParametros.Objeto.CodigoAtivo && !String.IsNullOrEmpty(pParametros.Objeto.CodigoAtivo))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_NEGOCIO", System.Data.DbType.String, pParametros.Objeto.CodigoAtivo);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CODIGO_NEGOCIO", System.Data.DbType.String, null);
                }

                if (null != pParametros.Objeto.CodigoMercado && !String.IsNullOrEmpty(pParametros.Objeto.CodigoMercado) && !pParametros.Objeto.CodigoMercado.Equals("(Todos)"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "TIPO_MERCADO", System.Data.DbType.String, pParametros.Objeto.CodigoMercado);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "TIPO_MERCADO", System.Data.DbType.String, null);
                }

                System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lResposta = new OMS.Persistencia.ConsultarObjetosResponse<Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo>();

                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lResposta.Resultado.Add(CriarRegistroCustodiaInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lResposta;
        }

        #endregion

        #region | Métodos de Apoio

        private static Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo CriarRegistroCustodiaInfo(System.Data.DataRow linha)
        {
            return new Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento.CustodiaInfo()
            {
                CodigoAssessor          = linha["CodigoAssessor"].DBToInt32(),
                CodigoCliente           = linha["CodigoCliente"].DBToInt32(),
                CodigoAtivo             = linha["CodigoAtivo"].DBToString(),
                CodigoMercado           = linha["CodigoMercado"].DBToString(),
                CodigoCarteira          = linha["CodigoCarteira"].DBToInt32(),
                QuantidadeDisponivel    = linha["QuantidadeDisponivel"].DBToInt32(),
                QuantidadeD1            = linha["QuantidadeD1"].DBToInt32(),
                QuantidadeD2            = linha["QuantidadeD2"].DBToInt32(),
                QuantidadeD3            = linha["QuantidadeD3"].DBToInt32(),
                QuantidadePendente      = linha["QuantidadePendente"].DBToInt32(),
                QuantidadeALiquidar     = linha["QuantidadeALiquidar"].DBToInt32(),
                QuantidadeTotal         = linha["QuantidadeTotal"].DBToInt32()

            };
        }

        #endregion


    }
}



