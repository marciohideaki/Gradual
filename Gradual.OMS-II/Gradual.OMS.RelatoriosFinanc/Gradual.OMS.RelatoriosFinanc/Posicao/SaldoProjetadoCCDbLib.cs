using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Linq;
using System.Text;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class SaldoProjetadoCCDbLib
    {
        #region | Atributos

        private string gNomeConexaoOracle = "Sinacor";

        #endregion
        public PosicaoCustodiaTesouroDiretoResponse ConsultarCustodiaComTesouro(PosicaoCustodiaTesouroDiretoRequest pParametros)
        {
            var lRetorno = new PosicaoCustodiaTesouroDiretoResponse();

            lRetorno.Objeto.AddRange(this.ConsultarCustodiaTesouroDireto(pParametros));
            lRetorno.Objeto.AddRange(this.ConsultarCustodiaBTC(pParametros));

            lRetorno.DescricaoResposta = string.Format("Posição em custódia do cliente: {0} carregado com sucesso", pParametros.ConsultaCdClienteBovespa.DBToString());
            
            lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            return lRetorno;
        }

        public SaldoProjetadoCCResponse ConsultarSaldoProjecoesEmContaCorrente(SaldoProjetadoCCRequest pParametro)
        {
            var lRetorno = new SaldoProjetadoCCResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_saldoprojcc"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.ConsultaCdAssesso);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_posicao", DbType.Date, pParametro.ConsultaDataOperacao);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                lRetorno.Resultado = new List<SaldoProjetadoCCInfo>();

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new SaldoProjetadoCCInfo()
                        {
                            CdAssessor   = lLinha["CD_ASSESSOR"].DBToInt32(),
                            CdCliente    = lLinha["CD_CLIENTE"].DBToInt32(),
                            NmAssessor   = lLinha["NM_ASSESSOR"].DBToString(),
                            NmCliente    = lLinha["NM_CLIENTE"].DBToString(),
                            VlALiquidar  = lLinha["A_LIQUIDAR"].DBToDecimal(),
                            VlDisponivel = lLinha["VL_DISPONIVEL"].DBToDecimal(),
                            VlProjetado1 = lLinha["VL_PROJET1"].DBToDecimal(),
                            VlProjetado2 = lLinha["VL_PROJET2"].DBToDecimal(),
                            VlTotal      = lLinha["VL_TOTAL"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }

        public SaldoProjetadoCCResponse ConsultarSaldoProjecoesEmContaCorrenteCliente(SaldoProjetadoCCRequest pParametro)
        {
            var lRetorno = new SaldoProjetadoCCResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_saldoprojcc_cliente"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.String, pParametro.ConsultaCdAssesso);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_posicao", DbType.Date, pParametro.ConsultaDataOperacao);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                lRetorno.Resultado = new List<SaldoProjetadoCCInfo>();

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new SaldoProjetadoCCInfo()
                        {
                            CdAssessor = lLinha["CD_ASSESSOR"].DBToInt32(),
                            CdCliente = lLinha["CD_CLIENTE"].DBToInt32(),
                            NmAssessor = lLinha["NM_ASSESSOR"].DBToString(),
                            NmCliente = lLinha["NM_CLIENTE"].DBToString(),
                            VlALiquidar = lLinha["A_LIQUIDAR"].DBToDecimal(),
                            VlDisponivel = lLinha["VL_DISPONIVEL"].DBToDecimal(),
                            VlProjetado1 = lLinha["VL_PROJET1"].DBToDecimal(),
                            VlProjetado2 = lLinha["VL_PROJET2"].DBToDecimal(),
                            VlTotal = lLinha["VL_TOTAL"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }

        private List<PosicaoCustodiaTesouroDiretoInfo> ConsultarCustodiaTesouroDireto(PosicaoCustodiaTesouroDiretoRequest pParametros)
        {
            var lLista = new List<PosicaoCustodiaTesouroDiretoInfo>();
            var lAcessaDados = new AcessaDados();
            var lRetorno = new PosicaoCustodiaTesouroDiretoResponse();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.ConsultaCdClienteBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lLista.Add(new PosicaoCustodiaTesouroDiretoInfo()
                        {
                            CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                            CodigoCarteira    = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                            DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                            TipoMercado       = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                            TipoGrupo         = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                            IdCliente         = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                            QtdeAtual         = lDataTable.Rows[i]["QTDE_ATUAL"].DBToDouble(),
                            QtdeDisponivel    = lDataTable.Rows[i]["QTDE_DISP"].DBToDouble(),
                            QtdeAExecVenda    = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToInt32(),
                            QtdeAExecCompra   = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToInt32(),
                            NomeEmpresa       = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                            ValorPosicao      = lDataTable.Rows[i]["VAL_POSI"].DBToDouble(),
                            DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                            QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                            QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                        });
            }

            if (null != pParametros.ConsultaDtVencimentoTermo && lLista.Count > 0)
            {
                lLista = lLista.FindAll(mov => { return mov.DtVencimento.Value.Date == pParametros.ConsultaDtVencimentoTermo.Value.Date; });
            }

            return lLista;
        }

        private List<PosicaoCustodiaTesouroDiretoInfo> ConsultarCustodiaBTC(PosicaoCustodiaTesouroDiretoRequest pParametros)
        {
            var lRetorno = new List<PosicaoCustodiaTesouroDiretoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_BTC"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.ConsultaCdClienteBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Add(new PosicaoCustodiaTesouroDiretoInfo()
                        {
                            CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                            CodigoCarteira    = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                            DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString(),
                            DsTomador         = lDataTable.Rows[i]["DS_TOMADOR"].DBToString(),
                            CdISIN            = lDataTable.Rows[i]["COD_ISIN"].DBToString(),
                            DtAbertura        = lDataTable.Rows[i]["DATA_ABER"].DBToDateTime(),
                            DtOrigem          = lDataTable.Rows[i]["DATA_ORI"].DBToDateTime(),
                            DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            TipoMercado       = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                            IdCliente         = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                            QtdeAtual         = lDataTable.Rows[i]["QTDE_ACOE"].DBToInt32(),
                            VlPrecoMedio      = lDataTable.Rows[i]["PREC_MED"].DBToDecimal(),
                            VlTaxaRemuneracao = lDataTable.Rows[i]["TAXA_REMU"].DBToDecimal(),
                            VlLiquido         = lDataTable.Rows[i]["VAL_BRUT"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }
    }
}
