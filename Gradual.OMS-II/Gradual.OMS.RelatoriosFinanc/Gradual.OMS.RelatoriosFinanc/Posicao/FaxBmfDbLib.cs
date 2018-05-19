using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using System.Data.Common;
using System.Data;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class FaxBmfDbLib
    {
        #region | Atributos

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<DateTime> gDataFeriados = new List<DateTime>();

        #endregion

        #region | Propriedades
        private List<DateTime> GetDataFeriados
        {
            get
            {
                if (gDataFeriados.Count == 0 || gDataFeriados[gDataFeriados.Count - 1].Year != DateTime.Today.Year)
                {
                    var lAcessaDados = new AcessaDados();
                    lAcessaDados.ConnectionStringName = "Sinacor";

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_feriado_por_periodo"))
                    {
                        gDataFeriados = new List<DateTime>();

                        lAcessaDados.AddInParameter(lDbCommand, "pDtInicio", DbType.Date, new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1));
                        lAcessaDados.AddInParameter(lDbCommand, "pDtFim", DbType.Date, new DateTime(DateTime.Now.AddYears(1).Year, 1, 1));

                        var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        gDataFeriados.Clear();

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                            foreach (DataRow lLinha in lDataTable.Rows)
                                gDataFeriados.Add(lLinha["DT_FERIADO"].DBToDateTime());
                    }
                }

                return gDataFeriados;
            }
        }
        #endregion

        #region | Métodos de Fax Bmf
        public FaxResponse ObterFaxBmf(FaxRequest lRequest)
        {
            var lRetorno = new FaxResponse();

            var lAcessaDados = new AcessaDados();

            log4net.Config.XmlConfigurator.Configure();

            try
            {
                lAcessaDados.ConnectionStringName = "CORRWIN";

                this.BuscarFax_Cabecalho(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFax_DataLiquidacao(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFax_SOMA(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFax_NET(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFax_Papel(ref lRequest, ref lAcessaDados, ref lRetorno);

            }
            catch (Exception ex)
            {
                gLogger.Error("ObterFaxBmf", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }

            return lRetorno;
        }

        private void BuscarFax_Cabecalho(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_CAB_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData",          DbType.Date,  pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.RelatorioBmf.CodigoCliente = lDataTable.Rows[0]["CD_CLIENTE"].DBToInt32();
                        plRetorno.RelatorioBmf.NomeCliente   = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                        plRetorno.RelatorioBmf.Telefone      = lDataTable.Rows[0]["TELEFONE"].DBToString();
                        plRetorno.RelatorioBmf.Fax           = lDataTable.Rows[0]["FAX"].DBToString();
                        plRetorno.RelatorioBmf.DataPregao    = pParametros.ConsultaDataMovimento;
                    }
                }
            }
        }

        private void BuscarFax_DataLiquidacao(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_DTLIQ_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.RelatorioBmf.DataLiquidacao = lDataTable.Rows[0]["DT_LIQUIDACAO"].DBToDateTime();
                        
                    }
                }
            }
        }

        private void BuscarFax_SOMA(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_SOMA_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    plRetorno.RelatorioBmf.CabecalhoGridBmf = new List<FaxBmfCabecalhoGridInfo>();

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            FaxBmfCabecalhoGridInfo lCabBmf = new FaxBmfCabecalhoGridInfo();

                            lCabBmf.CabecalhoSentido     = lDataTable.Rows[i]["CD_NATOPE"].DBToString();
                            lCabBmf.CabecalhoCommod      = lDataTable.Rows[i]["CD_COMMOD"].DBToString();
                            lCabBmf.CabecalhoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();
                            lCabBmf.CabecalhoTipoMercado = lDataTable.Rows[i]["CD_MERCAD"].DBToString();
                            lCabBmf.SomaCodigoNegocio    = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lCabBmf.SomaPreco            = lDataTable.Rows[i]["PRECO_MEDIO"].DBToDecimal();
                            lCabBmf.SomaQuantidade       = lDataTable.Rows[i]["QUANTIDADE"].DBToInt32();

                            plRetorno.RelatorioBmf.CabecalhoGridBmf.Add(lCabBmf);
                        }
                    }
                }
            }
        }

        private void BuscarFax_NET(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_NET_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData",          DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lCabBmf = plRetorno.RelatorioBmf.CabecalhoGridBmf.FindAll(papel =>
                            {
                                return (papel.CabecalhoCommod + papel.CabecalhoSerie) == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString() ;
                            });

                            for (int j = 0; j < lCabBmf.Count; j++)
                            {
                                lCabBmf[j].NetCodigoNegocio = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                                lCabBmf[j].NetPreco         = lDataTable.Rows[i]["VL_NET"].DBToDecimal();
                                lCabBmf[j].NetQuantidade    = lDataTable.Rows[i]["QT_NET"].DBToInt32();
                            }
                        }
                    }
                }
            }
        }

        private void BuscarFax_Papel(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_PAPEL_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData",          DbType.Date,  pParametros.ConsultaDataMovimento);


                for (int i = 0; i < plRetorno.RelatorioBmf.CabecalhoGridBmf.Count; i++)
                {
                    plRetorno.RelatorioBmf.CabecalhoGridBmf[i].DetalhesBmf = new List<FaxBmfDetalheInfo>();
                }

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        //plRetorno.RelatorioBmf.ca.DetalhesBmf = new List<FaxBmfDetalheInfo>();

                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lCabBmf = plRetorno.RelatorioBmf.CabecalhoGridBmf.FindAll(papel =>
                            {
                                return (papel.CabecalhoCommod + papel.CabecalhoSerie) == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString() &&
                                    papel.CabecalhoSentido == lDataTable.Rows[i]["SENTIDO"].DBToString();
                            });



                            for (int j = 0; j < lCabBmf.Count; j++)
                            {
                                var lDetBmf = new FaxBmfDetalheInfo();

                                lDetBmf.PapelSentido = lDataTable.Rows[i]["SENTIDO"].DBToString();
                                lDetBmf.PapelCodigoNegocio = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                                lDetBmf.PapelPreco = lDataTable.Rows[i]["PRECO"].DBToDecimal();
                                lDetBmf.PapelQuantidade = lDataTable.Rows[i]["QUANTIDADE"].DBToInt32();

                                lCabBmf[j].DetalhesBmf.Add(lDetBmf);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region | Métodos de Fax Bmf de Volatilidade
        public FaxResponse ObterFaxBmfVolatilidade(FaxRequest lRequest)
        {
            var lRetorno = new FaxResponse();

            var lAcessaDados = new AcessaDados();

            log4net.Config.XmlConfigurator.Configure();

            try
            {
                lAcessaDados.ConnectionStringName = "CORRWIN";

                this.BuscarFaxVol_Cabecalho(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFaxVol_DataLiquidacao(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFaxVol_SOMA(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFaxVol_NET(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFaxVol_Papel(ref lRequest, ref lAcessaDados, ref lRetorno);

                //this.BuscarFaxBol_PreparaNet(ref lRetorno);

            }
            catch (Exception ex)
            {
                gLogger.Error("ObterFaxBmfVolatilidade", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }

            return lRetorno;
        }

        private void BuscarFaxVol_Cabecalho(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_CAB_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.RelatorioBmf.CodigoCliente = lDataTable.Rows[0]["CD_CLIENTE"].DBToInt32();
                        plRetorno.RelatorioBmf.NomeCliente = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                        plRetorno.RelatorioBmf.Telefone = lDataTable.Rows[0]["TELEFONE"].DBToString();
                        plRetorno.RelatorioBmf.Fax = lDataTable.Rows[0]["FAX"].DBToString();
                        plRetorno.RelatorioBmf.DataPregao = pParametros.ConsultaDataMovimento;
                    }
                }
            }
        }

        private void BuscarFaxVol_DataLiquidacao(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_DTLIQ_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.RelatorioBmf.DataLiquidacao = lDataTable.Rows[0]["DT_LIQUIDACAO"].DBToDateTime();

                    }
                }
            }
        }

        private void BuscarFaxVol_SOMA(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_VOL_SOMA_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    plRetorno.RelatorioBmf.CabecalhoGridBmf = new List<FaxBmfCabecalhoGridInfo>();

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            FaxBmfCabecalhoGridInfo lCabBmf = new FaxBmfCabecalhoGridInfo();

                            lCabBmf.CabecalhoSentido     = lDataTable.Rows[i]["CD_NATOPE"].DBToString();
                            lCabBmf.CabecalhoCommod      = lDataTable.Rows[i]["CD_COMMOD"].DBToString();
                            lCabBmf.CabecalhoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();
                            lCabBmf.CabecalhoTipoMercado = lDataTable.Rows[i]["CD_MERCAD"].DBToString();
                            lCabBmf.SomaCodigoNegocio    = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lCabBmf.SomaPreco            = lDataTable.Rows[i]["PRECO_MEDIO"].DBToDecimal();
                            lCabBmf.SomaQuantidade       = lDataTable.Rows[i]["QUANTIDADE"].DBToInt32();

                            plRetorno.RelatorioBmf.CabecalhoGridBmf.Add(lCabBmf);
                        }
                    }
                }
            }
        }

        private void BuscarFaxVol_NET(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_VOL_NET_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lCabBmf = plRetorno.RelatorioBmf.CabecalhoGridBmf.FindAll(papel =>
                            {
                                return papel.CabecalhoCommod == lDataTable.Rows[i]["CD_COMMOD"].DBToString() &&
                                       papel.CabecalhoSerie  == lDataTable.Rows[i]["CD_SERIE"].DBToString();
                            });

                            for (int j = 0; j < lCabBmf.Count; j++)
                            {
                                lCabBmf[j].NetCodigoNegocio = lCabBmf[j].SomaCodigoNegocio;
                                lCabBmf[j].NetPreco         = lDataTable.Rows[i]["VL_NET"].DBToDecimal();
                                lCabBmf[j].NetQuantidade    = lDataTable.Rows[i]["QT_NET"].DBToInt32();
                            }
                        }
                    }
                }
            }
        }

        private void BuscarFaxVol_Papel(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_VOL_PAPEL_BMF"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoClienteBmf.Value);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                for (int i = 0; i < plRetorno.RelatorioBmf.CabecalhoGridBmf.Count; i++)
                {
                    plRetorno.RelatorioBmf.CabecalhoGridBmf[i].DetalhesBmf = new List<FaxBmfDetalheInfo>();
                }

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lCabBmf = plRetorno.RelatorioBmf.CabecalhoGridBmf.FindAll(papel =>
                            {
                                return (papel.CabecalhoCommod + papel.CabecalhoSerie).Trim() == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString().Trim() &&
                                       papel.CabecalhoSentido  == lDataTable.Rows[i]["SENTIDO"].DBToString();
                            });

                            for (int j = 0; j < lCabBmf.Count; j++)
                            {
                                var lDetBmf = new FaxBmfDetalheInfo();

                                lDetBmf.PapelSentido       = lDataTable.Rows[i]["SENTIDO"].DBToString();
                                lDetBmf.PapelCodigoNegocio = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                                lDetBmf.PapelPreco         = lDataTable.Rows[i]["PRECO"].DBToDecimal();
                                lDetBmf.PapelQuantidade    = lDataTable.Rows[i]["QUANTIDADE"].DBToInt32();

                                lCabBmf[j].DetalhesBmf.Add(lDetBmf);
                            }
                        }
                    }
                }
            }
        }

        private void BuscarFaxBol_PreparaNet (ref FaxResponse plRetorno)
        {
            List<string> lListaPapel         = new List<string>();
            List<string> lListaPapelRepetido = new List<string>();

            ///Pega os papeis repetidos
            plRetorno.RelatorioBmf.CabecalhoGridBmf.ForEach(papel => { 
                
                string lPapel = papel.CabecalhoCommod + papel.CabecalhoSerie;

                if (lListaPapel.Contains(lPapel))
                {
                    lListaPapelRepetido.Add(lPapel);
                }

                lListaPapel.Add(lPapel);

            });

            var lCabBmf = from a in plRetorno.RelatorioBmf.CabecalhoGridBmf
                          where lListaPapelRepetido.Contains(a.CabecalhoCommod + a.CabecalhoSerie) select a;

            List<AuxNet> Lista = new List<AuxNet>();

            foreach (var a in lCabBmf)
            {
                var net = new AuxNet();
                
                if (a.CabecalhoSentido == "V")
                {
                    net.CodigoNegocio      = a.CabecalhoCommod + a.CabecalhoSerie;
                    net.PrecoVenda         = a.NetPreco;
                    net.QuantidadeVenda    = a.NetQuantidade;

                }
                else if (a.CabecalhoSentido == "C")
                {
                    net.CodigoNegocio       = a.CabecalhoCommod + a.CabecalhoSerie;
                    net.PrecoCompra         = a.NetPreco;
                    net.QuantidadeCompra    = a.NetQuantidade;
                }

                Lista.Add(net);
            }
        }
        #endregion
    }
    public class AuxNet
    {
        public string CodigoNegocio { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantidadeVenda { get; set; }
        public decimal PrecoCompra { get; set; }
        public int QuantidadeCompra { get; set; }

    }
}
