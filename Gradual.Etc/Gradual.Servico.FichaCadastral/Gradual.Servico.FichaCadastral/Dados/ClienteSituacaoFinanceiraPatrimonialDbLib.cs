using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using Gradual.OMS.Library;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteSituacaoFinanceiraPatrimonialDbLib: DbLibBase
    {
        public ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo> ConsultarClienteSituacaoFinanceiraPatrimonial(ConsultarEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo> resposta =
                    new ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

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

                            resposta.Resultado.Add(CriarRegistroClienteSituacaoFinanceiraPatrimonialInfo(linha));
                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteSituacaoFinanceiraPatrimonialInfo CriarRegistroClienteSituacaoFinanceiraPatrimonialInfo(DataRow linha)
        {
            var lClienteSituacaoFinanceiraPatrimonialInfo = new ClienteSituacaoFinanceiraPatrimonialInfo();

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
       
        public ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> ReceberClienteSituacaoFinanceiraPatrimonial(ReceberEntidadeRequest<ClienteSituacaoFinanceiraPatrimonialInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo> resposta =
                    new ReceberObjetoResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();

                resposta.Objeto = new ClienteSituacaoFinanceiraPatrimonialInfo();

                var lAcessaDados = new AcessaDados();

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
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }
    }
}
