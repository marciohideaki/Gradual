using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data.Common;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {

        public static SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> SalvarClienteSituacaoFinanceiraPatrimonial(DbTransaction pTrans, SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            return Salvar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> SalvarClienteSituacaoFinanceiraPatrimonial(SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            if (pParametros.Objeto.IdSituacaoFinanceiraPatrimonial > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        public static SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> Salvar(SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Salvar(trans, pParametros);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
                if (!ConnectionState.Closed.Equals(conn.State)) conn.Close();
                conn.Dispose();
                conn = null;
            }
            return lRetorno;
        }

        public static SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> Salvar(DbTransaction pTrans, SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
               ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalbensmoveis", DbType.Decimal, pParametros.Objeto.VlTotalBensMoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalbensimoveis", DbType.Decimal, pParametros.Objeto.VlTotalBensImoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalaplicacaofinanceira", DbType.Decimal, pParametros.Objeto.VlTotalAplicacaoFinanceira);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalsalarioprolabore", DbType.Decimal, pParametros.Objeto.VlTotalSalarioProLabore);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totaloutrosrendimentos", DbType.Decimal, pParametros.Objeto.VlTotalOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_capitalsocial", DbType.Decimal, pParametros.Objeto.VTotalCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_patrimonioliquido", DbType.Decimal, pParametros.Objeto.VlTotalPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_patrimonioliquido", DbType.DateTime, pParametros.Objeto.DtPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_capitalsocial", DbType.DateTime, pParametros.Objeto.DtCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_outrosrendimentos", DbType.AnsiString, pParametros.Objeto.DsOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_dataatualizacao", DbType.DateTime, pParametros.Objeto.DtAtualizacao);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_sfp", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> response = new SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_sfp"].Value)
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
               
                    return response;
                }
            }
            catch (Exception ex)
            {
                               LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir,ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> Atualizar(SalvarObjetoRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalbensmoveis", DbType.Decimal, pParametros.Objeto.VlTotalBensMoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalbensimoveis", DbType.Decimal, pParametros.Objeto.VlTotalBensImoveis);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalaplicacaofinanceira", DbType.Decimal, pParametros.Objeto.VlTotalAplicacaoFinanceira);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totalsalarioprolabore", DbType.Decimal, pParametros.Objeto.VlTotalSalarioProLabore);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_totaloutrosrendimentos", DbType.Decimal, pParametros.Objeto.VlTotalOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_outrosrendimentos", DbType.String, pParametros.Objeto.DsOutrosRendimentos);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_capitalsocial", DbType.Decimal, pParametros.Objeto.VTotalCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_patrimonioliquido", DbType.Decimal, pParametros.Objeto.VlTotalPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_dataatualizacao", DbType.Date, pParametros.Objeto.DtAtualizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_capitalsocial", DbType.Date, pParametros.Objeto.DtCapitalSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_patrimonioliquido", DbType.Date, pParametros.Objeto.DtPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_sfp", DbType.Int32, pParametros.Objeto.IdSituacaoFinanceiraPatrimonial);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> ReceberClienteSituacaoFinanceiraPatrimonial(ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> resposta =
                    new ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();

                resposta.Objeto = new ClienteSituacaoFinanceiraPatrimonialInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_sel_porcliente_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.DsOutrosRendimentos = (lDataTable.Rows[0]["ds_outrosrendimentos"]).DBToString();
                        resposta.Objeto.DtAtualizacao = (lDataTable.Rows[0]["dt_dataatualizacao"]).DBToDateTime();
                        resposta.Objeto.DtCapitalSocial = lDataTable.Rows[0]["dt_capitalsocial"].DBToDateTime();
                        resposta.Objeto.DtPatrimonioLiquido = (lDataTable.Rows[0]["dt_patrimonioliquido"]).DBToDateTime();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdSituacaoFinanceiraPatrimonial = (lDataTable.Rows[0]["id_sfp"]).DBToInt32();
                        resposta.Objeto.VlTotalAplicacaoFinanceira = (lDataTable.Rows[0]["vl_totalaplicacaofinanceira"]).DBToDecimal();
                        resposta.Objeto.VTotalCapitalSocial = (lDataTable.Rows[0]["vl_capitalsocial"]).DBToDecimal();
                        resposta.Objeto.VlTotalOutrosRendimentos = (lDataTable.Rows[0]["vl_totaloutrosrendimentos"]).DBToDecimal();
                        resposta.Objeto.VlTotalPatrimonioLiquido = (lDataTable.Rows[0]["vl_patrimonioliquido"]).DBToDecimal();
                        resposta.Objeto.VlTotalSalarioProLabore = (lDataTable.Rows[0]["vl_totalsalarioprolabore"]).DBToDecimal();
                        resposta.Objeto.VlTotalBensImoveis = (lDataTable.Rows[0]["vl_totalbensimoveis"]).DBToDecimal();
                        resposta.Objeto.VlTotalBensMoveis = (lDataTable.Rows[0]["vl_totalbensmoveis"]).DBToDecimal();
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

        public static ClienteSituacaoFinanceiraPatrimonialInfo GetClienteSituacaoFinanceiraPatrimonialPorIdCliente(ClienteInfo pParametros)
        {

            ClienteSituacaoFinanceiraPatrimonialInfo resposta = new ClienteSituacaoFinanceiraPatrimonialInfo();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_sel_porcliente_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.IdCliente);
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    resposta.DsOutrosRendimentos = (lDataTable.Rows[0]["ds_outrosrendimentos"]).DBToString();
                    resposta.DtAtualizacao = (lDataTable.Rows[0]["dt_dataatualizacao"]).DBToDateTime();
                    resposta.DtCapitalSocial = lDataTable.Rows[0]["dt_capitalsocial"].DBToDateTime();
                    resposta.DtPatrimonioLiquido = (lDataTable.Rows[0]["dt_patrimonioliquido"]).DBToDateTime();
                    resposta.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                    resposta.IdSituacaoFinanceiraPatrimonial = (lDataTable.Rows[0]["id_sfp"]).DBToInt32();
                    resposta.VlTotalAplicacaoFinanceira = (lDataTable.Rows[0]["vl_totalaplicacaofinanceira"]).DBToDecimal();
                    resposta.VTotalCapitalSocial = (lDataTable.Rows[0]["vl_capitalsocial"]).DBToDecimal();
                    resposta.VlTotalOutrosRendimentos = (lDataTable.Rows[0]["vl_totaloutrosrendimentos"]).DBToDecimal();
                    resposta.VlTotalPatrimonioLiquido = (lDataTable.Rows[0]["vl_patrimonioliquido"]).DBToDecimal();
                    resposta.VlTotalSalarioProLabore = (lDataTable.Rows[0]["vl_totalsalarioprolabore"]).DBToDecimal();
                    resposta.VlTotalBensImoveis = (lDataTable.Rows[0]["vl_totalbensimoveis"]).DBToDecimal();
                    resposta.VlTotalBensMoveis = (lDataTable.Rows[0]["vl_totalbensmoveis"]).DBToDecimal();
                }
            }
            return resposta;
        }

        public static ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo> ConsultarClienteSituacaoFinanceiraPatrimonial(ConsultarEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo> resposta =
                    new ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);
                    }

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["ds_outrosrendimentos"] = (lDataTable.Rows[i]["ds_outrosrendimentos"]).DBToString();
                            linha["dt_dataatualizacao"] = (lDataTable.Rows[i]["dt_dataatualizacao"]).DBToDateTime();
                            linha["dt_capitalsocial"] = (lDataTable.Rows[i]["dt_capitalsocial"]).DBToDateTime();
                            linha["dt_patrimonioliquido"] = (lDataTable.Rows[i]["dt_patrimonioliquido"]).DBToDateTime();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_sfp"] = (lDataTable.Rows[i]["id_sfp"]).DBToInt32();
                            linha["vl_totalaplicacaofinanceira"] = (lDataTable.Rows[i]["vl_totalaplicacaofinanceira"]).DBToDecimal();
                            linha["vl_capitalsocial"] = (lDataTable.Rows[i]["vl_capitalsocial"]).DBToDecimal();
                            linha["vl_totaloutrosrendimentos"] = (lDataTable.Rows[i]["vl_totaloutrosrendimentos"]).DBToDecimal();
                            linha["vl_patrimonioliquido"] = (lDataTable.Rows[i]["vl_patrimonioliquido"]).DBToDecimal();
                            linha["vl_totalsalarioprolabore"] = (lDataTable.Rows[i]["vl_totalsalarioprolabore"]).DBToDecimal();
                            linha["vl_totalbensimoveis"] = (lDataTable.Rows[i]["vl_totalbensimoveis"]).DBToDecimal();
                            linha["vl_totalbensmoveis"] = (lDataTable.Rows[i]["vl_totalbensmoveis"]).DBToDecimal();

                            resposta.Resultado.Add(
                                CriarRegistroClienteSituacaoFinanceiraPatrimonialInfo(
                                linha
                                )
                             );
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

        public static RemoverObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> RemoverClienteSituacaoFinanceiraPatrimonial(RemoverEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_situacaofinanceirapatrimonial_del_sp"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@id_sfp", DbType.Int32, pParametros.Objeto.IdSituacaoFinanceiraPatrimonial);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo> response = new RemoverEntidadeResponse<ClienteSituacaoFinanceiraPatrimonialInfo>()
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

        private static ClienteSituacaoFinanceiraPatrimonialInfo CriarRegistroClienteSituacaoFinanceiraPatrimonialInfo(DataRow linha)
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lClienteSituacaoFinanceiraPatrimonialInfo = new ClienteSituacaoFinanceiraPatrimonialInfo();

            lClienteSituacaoFinanceiraPatrimonialInfo.DsOutrosRendimentos = (linha["ds_outrosrendimentos"]).DBToString();
            lClienteSituacaoFinanceiraPatrimonialInfo.DtAtualizacao = (linha["dt_dataatualizacao"]).DBToDateTime();
            lClienteSituacaoFinanceiraPatrimonialInfo.DtCapitalSocial = (linha["dt_capitalsocial"]).DBToDateTime();
            lClienteSituacaoFinanceiraPatrimonialInfo.DtPatrimonioLiquido = (linha["dt_patrimonioliquido"]).DBToDateTime();
            lClienteSituacaoFinanceiraPatrimonialInfo.IdCliente = (linha["id_cliente"]).DBToInt32();
            lClienteSituacaoFinanceiraPatrimonialInfo.IdSituacaoFinanceiraPatrimonial = (linha["id_sfp"]).DBToInt32();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalAplicacaoFinanceira = (linha["vl_totalaplicacaofinanceira"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VTotalCapitalSocial = (linha["vl_capitalsocial"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalOutrosRendimentos = (linha["vl_totaloutrosrendimentos"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalPatrimonioLiquido = (linha["vl_patrimonioliquido"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalSalarioProLabore = (linha["vl_totalsalarioprolabore"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalBensImoveis = (linha["vl_totalbensimoveis"]).DBToDecimal();
            lClienteSituacaoFinanceiraPatrimonialInfo.VlTotalBensMoveis = (linha["vl_totalbensmoveis"]).DBToDecimal();

            return lClienteSituacaoFinanceiraPatrimonialInfo;

        }

        private static void LogarModificacao(ClienteSituacaoFinanceiraPatrimonialInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lEntrada = new ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lRetorno = ReceberClienteSituacaoFinanceiraPatrimonial(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
