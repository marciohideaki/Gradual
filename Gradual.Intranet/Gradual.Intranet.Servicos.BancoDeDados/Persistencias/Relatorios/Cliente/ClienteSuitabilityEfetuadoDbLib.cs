using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region | ConsultarClienteSuitabilityEfetuado

        /// <summary>
        /// Lista os clientes que efetuaram o suitability
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteSuitabilityEfetuadoInfo</param>
        /// <returns>Retorna uma lista de clientes que efetuaram o suitability em um certo período</returns>
        public static ConsultarObjetosResponse<ClienteSuitabilityEfetuadoInfo> ConsultarClienteSuitabilityEfetuado(ConsultarEntidadeRequest<ClienteSuitabilityEfetuadoInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteSuitabilityEfetuadoInfo> lResposta =
                new ConsultarObjetosResponse<ClienteSuitabilityEfetuadoInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_clientes_efetuaram_suitability_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@StRealizado", DbType.Boolean, pParametros.Objeto.StRealizado);
                if (pParametros.Objeto.CodigoAssessor.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, null);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                }
                lAcessaDados.AddInParameter(lDbCommand, "@CpfCnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                

                if (pParametros.Objeto.IdCliente.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, null);
                }
                else
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@IdCliente", DbType.Int32, pParametros.Objeto.IdCliente);
                }

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteSuitabilityEfetuadoInfo(lDataTable.Rows[i]));

            }
            return lResposta;
        }



        #endregion

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para criação/preenchimento de entidade do tipo ClienteSuitabilityEfetuadoInfo
        /// </summary>
        /// <param name="linha">DataRow dos clientes que efetuaram o suitability</param>
        /// <returns>Retorna uma entidade do tipo ClienteSuitabilityEfetuadoInfo preenchida</returns>
        private static ClienteSuitabilityEfetuadoInfo CriarRegistroClienteSuitabilityEfetuadoInfo(DataRow linha)
        {
            return new ClienteSuitabilityEfetuadoInfo()
            {
                IdCliente               = linha["id_cliente"].DBToInt32(),
                DsNomeCliente           = linha["ds_nome"].DBToString(),
                TipoPessoa              = "F",
                DsPerfil                = linha["ds_perfil"].DBToString(),
                DsFonte                 = linha["ds_fonte"].DBToString(),
                DsCpfCnpj               = linha["ds_cpfcnpj"].DBToString(),
                CodigoBovespa           = linha["cd_codigo"].DBToInt32(),
                CodigoAssessor          = linha["cd_assessor"].DBToInt32(),
                DsLoginRealizado        = linha["Ds_LoginRealizado"].DBToString(),
                DsArquivoCiencia        = linha["ds_arquivo_ciencia"].DBToString(),
                DtArquivoCiencia        = linha["dt_arquivo_upload"].DBToString(),
                StPreenchidoPeloCliente = GetBoolNulable(linha["St_PreenchidoPeloCliente"]),
                DtRealizacao            = linha["Dt_Realizacao"].DBToDateTime(),
                DsStatus                = linha["ds_status"].DBToString(),
                DsRespostas             = linha["ds_respostas"].DBToString(),
                Peso                    = Decimal.Parse(linha["Peso"].DBToString()).ToString("N2")
            };
        }

        private static System.Nullable<Boolean> GetBoolNulable(object pBoolNulable)
        {
            if (null == pBoolNulable  || pBoolNulable.ToString().Length == 0)
                return null;
            else if (Boolean.Parse(pBoolNulable.ToString()))
                return true;
            else
                return false;
        }


        #endregion
    }
}
