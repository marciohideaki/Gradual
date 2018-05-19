using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class CustodiaDbLib
    {
        #region | Atributos

        private string gNomeConexaoOracle = "Sinacor";

        #endregion

        #region | Métodos

        public PosicaoCustodiaResponse ConsultarCustodia(PosicaoCustodiaRequest pParametros)
        {
            var lRetorno = new PosicaoCustodiaResponse();

            lRetorno.Objeto.ListaMovimento.AddRange(this.ConsultarCustodiaNormal(pParametros));
            lRetorno.Objeto.ListaMovimento.AddRange(this.ConsultarCustodiaBTC(pParametros));

            lRetorno.DescricaoResposta = string.Format("Posição em custódia do cliente: {0} carregado com sucesso", pParametros.ConsultaCdClienteBovespa.DBToString());
            lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return lRetorno;
        }

        private List<PosicaoCustodiaInfo.CustodiaMovimento> ConsultarCustodiaNormal(PosicaoCustodiaRequest pParametros)
        {
            var lRetorno = new List<PosicaoCustodiaInfo.CustodiaMovimento>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.ConsultaCdClienteBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Add(new PosicaoCustodiaInfo.CustodiaMovimento()
                        {
                            CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                            CodigoCarteira = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                            DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                            TipoMercado = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                            TipoGrupo = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                            IdCliente = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                            QtdeAtual = lDataTable.Rows[i]["QTDE_ATUAL"].DBToInt32(),
                            QtdeDisponivel = lDataTable.Rows[i]["QTDE_DISP"].DBToInt32(),
                            QtdeAExecVenda = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToInt32(),
                            QtdeAExecCompra = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToInt32(),
                            NomeEmpresa = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                            ValorPosicao = lDataTable.Rows[i]["VAL_POSI"].DBToDouble(),
                            DtVencimento = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            QtdeD1 = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                            QtdeD2 = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                            QtdeD3 = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                        });
            }

            if (null != pParametros.ConsultaDtVencimentoTermo && lRetorno.Count > 0)
                lRetorno = lRetorno.FindAll(mov => { return mov.DtVencimento.Value.Date == pParametros.ConsultaDtVencimentoTermo.Value.Date; });

            return lRetorno;
        }

        private List<PosicaoCustodiaInfo.CustodiaMovimento> ConsultarCustodiaBTC(PosicaoCustodiaRequest pParametros)
        {
            var lRetorno = new List<PosicaoCustodiaInfo.CustodiaMovimento>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_BTC"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.ConsultaCdClienteBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Add(new PosicaoCustodiaInfo.CustodiaMovimento()
                        {
                            CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                            CodigoCarteira = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                            DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString(),
                            DsTomador = lDataTable.Rows[i]["DS_TOMADOR"].DBToString(),
                            CdISIN = lDataTable.Rows[i]["COD_ISIN"].DBToString(),
                            DtAbertura = lDataTable.Rows[i]["DATA_ABER"].DBToDateTime(),
                            DtOrigem = lDataTable.Rows[i]["DATA_ORI"].DBToDateTime(),
                            DtVencimento = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            TipoMercado = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                            IdCliente = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                            QtdeAtual = lDataTable.Rows[i]["QTDE_ACOE"].DBToInt32(),
                            VlPrecoMedio = lDataTable.Rows[i]["PREC_MED"].DBToDecimal(),
                            VlTaxaRemuneracao = lDataTable.Rows[i]["TAXA_REMU"].DBToDecimal(),
                            VlLiquido = lDataTable.Rows[i]["VAL_BRUT"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }

        //public ClienteAtividadeBmfResponse ObterCodigoBmfCliente(ClienteAtividadeBmfRequest pParametro)
        //{
        //    AcessaDados lAcessaDados = new AcessaDados();
        //    ClienteAtividadeBmfResponse lResponse = new ClienteAtividadeBmfResponse();

        //    lResponse.CodigoBmf = 0;

        //    try
        //    {
        //        lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

        //        gLogger.Info("Inicia consulta para gerar código de BMF do cliente.");

        //        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "proc_sel_cliente_contabmf"))
        //        {
        //            lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pParametro.CodigoBase);

        //            DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

        //            if (null != lDataTable && lDataTable.Rows.Count > 0)
        //            {
        //                if ((lDataTable.Rows[0]["CODCLI"]) != null)
        //                {
        //                    lResponse = new ClienteAtividadeBmfResponse();
        //                    gLogger.Info("Código BMF Gerado por sucesso.");

        //                    lResponse.CodigoBmf = (lDataTable.Rows[0]["CODCLI"]).DBToInt32();
        //                    lResponse.DataResposta = DateTime.Now;
        //                    lResponse.StatusResposta = Library.MensagemResponseStatusEnum.OK;
        //                    lResponse.DescricaoResposta = "Código BMF gerado com sucesso";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        gLogger.Error(string.Concat("Ocorreu um erro ao gerar o código BMF do cliente. ", ex.ToString()), ex);
        //        lResponse.DataResposta = DateTime.Now;
        //        lResponse.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
        //        lResponse.DescricaoResposta = string.Concat("Ocorreu um erro ao gerar o código BMF do cliente. ", ex.ToString());
        //    }

        //    return lResponse;
        //}

        #endregion
    }
}
