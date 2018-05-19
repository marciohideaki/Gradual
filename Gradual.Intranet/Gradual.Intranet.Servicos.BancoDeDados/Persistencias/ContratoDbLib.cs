using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<ContratoInfo> ConsultarContrato(ConsultarEntidadeRequest<ContratoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ContratoInfo> resposta =
                    new ConsultarObjetosResponse<ContratoInfo>();

                pParametros.Condicoes.Add(new CondicaoInfo());

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "contrato_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {

                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {

                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["id_contrato"] = (lDataTable.Rows[i]["id_contrato"]).DBToInt32();
                                linha["ds_contrato"] = (lDataTable.Rows[i]["ds_contrato"]).DBToString();
                                linha["ds_path"] = (lDataTable.Rows[i]["ds_path"]).DBToString();
                                linha["st_obrigatorio"] = bool.Parse(lDataTable.Rows[i]["st_obrigatorio"].ToString());

                                resposta.Resultado.Add(CriarRegistroContrato(linha));
                            }

                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ContratoInfo> ReceberContrato(ReceberEntidadeRequest<ContratoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ContratoInfo> resposta =
                    new ReceberObjetoResponse<ContratoInfo>();

                resposta.Objeto = new ContratoInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "contrato_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdContrato = (lDataTable.Rows[0]["id_contrato"]).DBToInt32();
                        resposta.Objeto.DsContrato = (lDataTable.Rows[0]["ds_contrato"]).DBToString();
                        resposta.Objeto.DsPath = (lDataTable.Rows[0]["ds_path"]).DBToString();
                        resposta.Objeto.StObrigatorio = bool.Parse(lDataTable.Rows[0]["st_obrigatorio"].ToString());
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static ContratoInfo CriarRegistroContrato(DataRow linha)
        {
            ContratoInfo lContratoInfo = new ContratoInfo();

            lContratoInfo.DsContrato = linha["ds_contrato"].DBToString();
            lContratoInfo.DsPath = linha["ds_path"].DBToString();
            lContratoInfo.IdContrato = linha["id_contrato"].DBToInt32();
            lContratoInfo.StObrigatorio = bool.Parse(linha["st_obrigatorio"].ToString());

            return lContratoInfo;

        }

        public static RemoverObjetoResponse<ContratoInfo> RemoverContrato(RemoverEntidadeRequest<ContratoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "contrato_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ContratoInfo> response = new RemoverEntidadeResponse<ContratoInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ContratoInfo> SalvarContrato(SalvarObjetoRequest<ContratoInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "contrato_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_contrato", DbType.String, pParametros.Objeto.DsContrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_path", DbType.String, pParametros.Objeto.DsPath);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_obrigatorio", DbType.Boolean, pParametros.Objeto.StObrigatorio);


                    lAcessaDados.AddOutParameter(lDbCommand, "@id_contrato", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ContratoInfo> response = new SalvarEntidadeResponse<ContratoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_contrato"].Value)
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(ContratoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ContratoInfo> lEntrada = new ReceberEntidadeRequest<ContratoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ContratoInfo> lRetorno = ReceberContrato(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
