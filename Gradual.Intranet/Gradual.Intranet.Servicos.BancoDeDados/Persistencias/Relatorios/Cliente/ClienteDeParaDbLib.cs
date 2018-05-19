using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente
{
    public class ClienteDeParaDbLib
    {
        public static string gNomeConexao = "SINACOR";


        #region | ConsultarClienteDePara
        /// <summary>        
        /// Lista os clientes com os codigos na Gradual e na corretora externa
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteDeParaInfo</param>
        /// <returns>Retorna a lista de clientes</returns>
        public static Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo> ConsultarClienteDePara(Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request.ConsultarEntidadeRequest<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo> pParametros)
        {
            Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo> lResposta = new Gradual.OMS.Persistencia.ConsultarObjetosResponse<Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexao;

            using (System.Data.Common.DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "PRC_CLIENTE_DE_PARA_LST"))
            {
                if (null != pParametros.Objeto.CodigoGradual && !pParametros.Objeto.CodigoGradual.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_GRADUAL", System.Data.DbType.Int32, pParametros.Objeto.CodigoGradual);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_GRADUAL", System.Data.DbType.Int32, null);
                }

                if (null != pParametros.Objeto.CodigoExterno && !pParametros.Objeto.CodigoExterno.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_EXTERNO", System.Data.DbType.Int32, pParametros.Objeto.CodigoExterno);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_EXTERNO", System.Data.DbType.Int32, null);
                }

                if (null != pParametros.Objeto.CodigoAssessor && !pParametros.Objeto.CodigoAssessor.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_ASSESSOR", System.Data.DbType.String, pParametros.Objeto.CodigoAssessor);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_ASSESSOR", System.Data.DbType.String, null);
                }

                System.Data.DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    string[] lAssessores = pParametros.Objeto.CodigoAssessor.Split(',');

                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        if (lAssessores.Contains(lDataTable.Rows[i]["CD_ASSESSOR"].DBToString()) || pParametros.Objeto.CodigoAssessor.Equals(string.Empty))
                        {
                            lResposta.Resultado.Add(CriarRegistroClienteDeParaInfo(lDataTable.Rows[i]));
                        }
                    }
                }

            }

            return lResposta;
        }

        #endregion

        #region | Métodos de Apoio
        
        private static Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo CriarRegistroClienteDeParaInfo(System.Data.DataRow linha)
        {
            return new Gradual.Intranet.Contratos.Dados.Relatorios.Cliente.ClienteDeParaInfo()
            {
                CodigoGradual   = linha["CD_CLIENTE"].DBToInt32(),
                CodigoExterno   = linha["CD_CLIE_OUTR_BOLSA"].DBToInt32(),
                DigitoPlural    = linha["DV_CLIE_OUTR_BOLSA"].DBToInt32(),
                CodigoAssessor  = linha["CD_ASSESSOR"].DBToString(),
                Nome            = linha["NM_CLIENTE"].DBToString()
            };
        }

        #endregion
    }
}
