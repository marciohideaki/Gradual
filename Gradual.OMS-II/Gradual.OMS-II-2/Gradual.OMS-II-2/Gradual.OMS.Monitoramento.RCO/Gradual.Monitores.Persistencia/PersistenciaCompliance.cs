using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Common;
using Gradual.Monitores.Compliance.Lib;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.Monitores.Compliance;
using System.Globalization;
using System.Diagnostics;
using System.Configuration;
using Gradual.OMS.Email.Lib;
using System.IO;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Compliance.Lib.Info;
namespace Gradual.Monitores.Persistencia
{
    /// <summary>
    /// Classe de banco de dados do serviço de monitramento de compliance.
    /// </summary>
    public class PersistenciaCompliance
    {

        private const string gNomeConexaoSinacor = "SINACOR";
        private const string gNomeConexaoSQL = "Risco";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";
        private const string gNomeConexaoCorrwin = "SinacorCorrwin";
        private const string gNomeConexaoGradualOMS = "GradualOMS";
        private const string gNomeConexaoCadastro = "Cadastro";

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Construtor vazio da classe
        /// </summary>
        public PersistenciaCompliance()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Método que busca no sinacor os cabeçalhos das ordens alteradas dos ultimos 3 meses
        /// Procedure: prc_header_ordensalteradas_c1
        /// </summary>
        /// <returns>Retorna ums listagem do objeto OrdensAlteradasCabecalhoInfo preenchidos com as ordens alteradas e suas justificativas</returns>
        public List<OrdensAlteradasCabecalhoInfo> ObterCabecalhoOrdensAlteradasDayTrade()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<OrdensAlteradasCabecalhoInfo> lstOrdensAlteradaCabecalho = new List<OrdensAlteradasCabecalhoInfo>();
            OrdensAlteradasCabecalhoInfo OrdensAlteradaCabecalho;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_header_ordensalteradas_c1"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdensAlteradaCabecalho = new OrdensAlteradasCabecalhoInfo();

                            string DayTarde = (lDataTable.Rows[i]["in_negocio"]).DBToString();


                            OrdensAlteradaCabecalho.NumeroSeqOrdem = (lDataTable.Rows[i]["NR_SEQORD"]).DBToInt32();
                            OrdensAlteradaCabecalho.DayTrade = true;
                            OrdensAlteradaCabecalho.Justificativa = (lDataTable.Rows[i]["nm_justif"]).DBToString();
                            OrdensAlteradaCabecalho.DataHoraOrdem = (lDataTable.Rows[i]["DT_horord"]).DBToDateTime();
                            OrdensAlteradaCabecalho.TipoMercado = (lDataTable.Rows[i]["CD_MERCAD"]).DBToString();

                            lstOrdensAlteradaCabecalho.Add(OrdensAlteradaCabecalho);

                        }
                    }

                }

                return lstOrdensAlteradaCabecalho;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdensAlteradaCabecalho;

        }

        /// <summary>
        /// Método que busca no sinacor os corpos da sodens alteradas dos ultimos 3 meses.
        /// Procedure: prc_corpo_ordensalteradas_c1
        /// </summary>
        /// <returns>Retorna a listagem dos objetos OrdensAlteradasInfo preenchidos com os dados completos do</returns>
        public List<OrdensAlteradasInfo> ObterOrdensAlteradasIntraday()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<OrdensAlteradasInfo> lstOrdensAlterada = new List<OrdensAlteradasInfo>();
            OrdensAlteradasInfo OrdensAlterada;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_corpo_ordensalteradas_c1"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdensAlterada = new OrdensAlteradasInfo();

                            OrdensAlterada.NumeroSeqOrdem = (lDataTable.Rows[i]["NR_SEQORD"]).DBToInt32();

                            OrdensAlterada.DataAlteracao = (lDataTable.Rows[i]["DataAlteracao"]).DBToDateTime();
                            OrdensAlterada.Assessor = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            OrdensAlterada.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();

                            string ContaErro =
                                (lDataTable.Rows[i]["CTA_ERRO"]).DBToString();

                            if (ContaErro != "")
                            {
                                OrdensAlterada.ContaErro = true;
                            }
                            else
                            {
                                OrdensAlterada.ContaErro = false;
                            }

                            string Vinculado =
                                (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();

                            if (Vinculado == "S")
                            {
                                OrdensAlterada.Vinculado = true;
                            }
                            else
                            {
                                OrdensAlterada.Vinculado = false;
                            }

                            OrdensAlterada.DescontoCorretagem = (lDataTable.Rows[i]["PC_REDACR"]).DBToDecimal();
                            OrdensAlterada.Instrumento = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            OrdensAlterada.Quantidade = (lDataTable.Rows[i]["QT_ORDEM"]).DBToInt32();
                            OrdensAlterada.Sentido = (lDataTable.Rows[i]["CD_NATOPE"]).DBToString();
                            OrdensAlterada.TipoPessoa = (lDataTable.Rows[i]["TP_PESSOA"]).DBToString();
                            OrdensAlterada.Usuario = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            OrdensAlterada.UsuarioAlteracao = (lDataTable.Rows[i]["NM_USUARIO_ALT"]).DBToString();

                            lstOrdensAlterada.Add(OrdensAlterada);


                        }
                    }

                }

                return lstOrdensAlterada;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdensAlterada;

        }

        /// <summary>
        /// Método que busca no sinacor algumas informaçoes de daytrade de bovespa de clientes
        /// Procedure: prc_estatistica_daytrade_bov
        /// </summary>
        /// <returns></returns>
        public List<EstatisticaDayTradeBovespaInfo> ObterEstatisticaDayTradeBovespa()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<EstatisticaDayTradeBovespaInfo> lstOrdens = new List<EstatisticaDayTradeBovespaInfo>();
            EstatisticaDayTradeBovespaInfo OrdemInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_estatistica_daytrade_bov"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdemInfo = new EstatisticaDayTradeBovespaInfo();

                            OrdemInfo.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();

                            string TipoBolsa = (lDataTable.Rows[i]["bolsa"]).DBToString();

                            if (TipoBolsa == "BVSP")
                            {
                                OrdemInfo.TipoBolsa = EnumBolsaDayTrade.BOVESPA;
                            }
                            else
                            {
                                OrdemInfo.TipoBolsa = EnumBolsaDayTrade.BMF;
                            }

                            OrdemInfo.PessoaVinculada            = (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();
                            OrdemInfo.Idade                      = (lDataTable.Rows[i]["idade"]).DBToInt32();
                            OrdemInfo.NomeCliente                = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            OrdemInfo.CodigoAssessor             = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();
                            OrdemInfo.NomeAssessor               = (lDataTable.Rows[i]["nm_assessor"]).DBToString();
                            OrdemInfo.QuantidadeDayTrade         = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();
                            OrdemInfo.QuantidadeDayTradePositivo = (lDataTable.Rows[i]["QTDE_POSITIVO"]).DBToInt32();
                            OrdemInfo.PercentualPositivo         = (lDataTable.Rows[i]["PERCENT_POSITIVO"]).DBToDecimal();
                            OrdemInfo.ValorPositivo              = (lDataTable.Rows[i]["vlr_positivo"]).DBToDecimal();
                            OrdemInfo.ValorNegativo              = (lDataTable.Rows[i]["vlr_negativo"]).DBToDecimal();
                            OrdemInfo.NET                        = (lDataTable.Rows[i]["net"]).DBToDecimal();
                            OrdemInfo.DataNegocio                = (lDataTable.Rows[i]["dt_negocio"]).DBToDateTime();
                            lstOrdens.Add(OrdemInfo);

                        }
                    }

                }

                return lstOrdens;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdens;

        }

        /// <summary>
        /// Métodos que busca no sinacor informações de negocios diretos
        /// (Negócios efetuados entre o vendedor cliente gradual e comprador cliente gradual)
        /// Procedure: prc_operacoesDiretas
        /// </summary>
        /// <returns>Retorna a lista de objetos do tipo NegociosDiretosInfo </returns>
        public List<NegociosDiretosInfo> ObterNegociosDiretos()
        {

            AcessaDados lAcessaDados = new AcessaDados();
            List<NegociosDiretosInfo> lstOrdens = new List<NegociosDiretosInfo>();
            NegociosDiretosInfo OrdemInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_operacoesDiretas"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdemInfo = new NegociosDiretosInfo();

                            OrdemInfo.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();
                            OrdemInfo.DataNegocio = (lDataTable.Rows[i]["DT_NEGOCIO"]).DBToDateTime();
                            OrdemInfo.Instrumento = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            OrdemInfo.Sentido = (lDataTable.Rows[i]["cd_natope"]).DBToString();
                            OrdemInfo.NomeCliente = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            OrdemInfo.PessoaVinculada = (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();
                            OrdemInfo.NumeroNegocio = (lDataTable.Rows[i]["NR_NEGOCIO"]).DBToInt32();


                            lstOrdens.Add(OrdemInfo);

                        }
                    }

                }

                return lstOrdens;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdens;

        }

        /// <summary>
        /// Método que busca no sinacor(Corrwin) informações de churning avulso, usado para teste
        /// Procedure: PRC_TURNOVER_AVULSO_SEL - Sinacor (Corrwin)
        /// Procedure: prc_churning_intraday_ins - SQl
        /// </summary>
        /// <param name="pDataPosicao">Dara de posição para consulta</param>
        /// <returns>Retorna a listagem de objetos do tipo churninhIntradayInfo preenchidos</returns>
        public List<ChurningIntradayInfo> ImportarChurningIntradayAvulso(DateTime pDataPosicao)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ChurningIntradayInfo>();
            var lChurning = new ChurningIntradayInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoCorrwin;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_AVULSO_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pDataPosicao", DbType.DateTime, pDataPosicao);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lChurning = new ChurningIntradayInfo();

                            lChurning.Data           = (lDataTable.Rows[i]["DATA_POSI"]).DBToDateTime();
                            lChurning.CodigoCliente  = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();
                            lChurning.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToInt32();
                            lChurning.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            lChurning.NomeAssessor   = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            lChurning.VlCarteira     = (lDataTable.Rows[i]["CARTEIRA"]).DBToDecimal();
                            lChurning.VlCompra       = (lDataTable.Rows[i]["VL_COMPRAS"]).DBToDecimal();
                            lChurning.VlVendas       = (lDataTable.Rows[i]["VL_VENDAS"]).DBToDecimal();

                            lRetorno.Add(lChurning);
                        }
                    }
                }

                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                if (lRetorno.Count > 0)
                {
                    foreach (ChurningIntradayInfo info in lRetorno)
                    {

                        //decimal TR = 

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_churning_intraday_ins"))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@Data", DbType.DateTime, info.Data);
                            lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, info.CodigoCliente);
                            lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, info.CodigoAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente", DbType.String, info.NomeCliente);
                            lAcessaDados.AddInParameter(lDbCommand, "@NomeAssessor", DbType.String, info.NomeAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlCompras", DbType.Decimal, info.VlCompra);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlVendas", DbType.Decimal, info.VlVendas);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlCarteira", DbType.Decimal, info.VlCarteira);
                            //lAcessaDados.AddInParameter(lDbCommand, "@VlCarteiraMedia",       DbType.Decimal,  info.VlCarteiraMedia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualTRnoDia",     DbType.Decimal,  info.PercentualTRnoDia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualTRnoPeriodo", DbType.Decimal,  info.PercentualTRnoPeriodo);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualCEnoDia",     DbType.Decimal,  info.PercentualCEnoDia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualCEnoPeriodo", DbType.Decimal,  info.PercentualCEnoPeriodo);

                            lAcessaDados.ExecuteNonQuery(lDbCommand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que busca no sinacor(Corrwin) informações de churning avulso
        /// Procedure: PRC_TURNOVER_SEL - Sinacor (Corrwin)
        /// Procedure: prc_churning_intraday_ins - SQl
        /// </summary>
        /// <param name="pDataPosicao">Dara de posição para consulta</param>
        /// <returns>Retorna a listagem de objetos do tipo churninhIntradayInfo preenchidos</returns>
        public List<ChurningIntradayInfo> ImportarChurningIntraday()
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ChurningIntradayInfo>();
            var lChurning = new ChurningIntradayInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoCorrwin;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_SEL"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lChurning = new ChurningIntradayInfo();

                            lChurning.Data           = (lDataTable.Rows[i]["DATA_POSI"]).DBToDateTime();
                            lChurning.CodigoCliente  = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();
                            lChurning.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToInt32();
                            lChurning.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            lChurning.NomeAssessor   = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            lChurning.VlCarteira     = (lDataTable.Rows[i]["CARTEIRA"]).DBToDecimal();
                            lChurning.VlCompra       = (lDataTable.Rows[i]["VL_COMPRAS"]).DBToDecimal();
                            lChurning.VlVendas       = (lDataTable.Rows[i]["VL_VENDAS"]).DBToDecimal();

                            lRetorno.Add(lChurning);
                        }
                    }
                }

                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                if (lRetorno.Count > 0)
                {
                    foreach (ChurningIntradayInfo info in lRetorno)
                    {

                        //decimal TR = 

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_churning_intraday_ins"))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@Data", DbType.DateTime, info.Data);
                            lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, info.CodigoCliente);
                            lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, info.CodigoAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente", DbType.String, info.NomeCliente);
                            lAcessaDados.AddInParameter(lDbCommand, "@NomeAssessor", DbType.String, info.NomeAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlCompras", DbType.Decimal, info.VlCompra);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlVendas", DbType.Decimal, info.VlVendas);
                            lAcessaDados.AddInParameter(lDbCommand, "@VlCarteira", DbType.Decimal, info.VlCarteira);
                            //lAcessaDados.AddInParameter(lDbCommand, "@VlCarteiraMedia",       DbType.Decimal,  info.VlCarteiraMedia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualTRnoDia",     DbType.Decimal,  info.PercentualTRnoDia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualTRnoPeriodo", DbType.Decimal,  info.PercentualTRnoPeriodo);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualCEnoDia",     DbType.Decimal,  info.PercentualCEnoDia);
                            //lAcessaDados.AddInParameter(lDbCommand, "@PercentualCEnoPeriodo", DbType.Decimal,  info.PercentualCEnoPeriodo);

                            lAcessaDados.ExecuteNonQuery(lDbCommand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método de busca no sql GradualOMS para produtos suitability e seus devidos tipos de perfis.
        /// Sql - Text: select * from tb_cliente_produto_suitability order by nr_ordem asc
        /// </summary>
        /// <returns>Retorna uma listagem de produtos do suitability e seus tipo perfil</returns>
        private List<SuitabilityClienteProduto> ObterProdutoPerfilSuitability()
        {
            var lRetorno = new List<SuitabilityClienteProduto>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualOMS";

            string lSql = "select * from tb_cliente_produto_suitability order by nr_ordem asc";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lRetorno.Add(new SuitabilityClienteProduto()
                        {
                            Produto    = lRow["ds_produto"].ToString(),
                            TipoPerfil = lRow["tipo_perfil"].ToString().ToLower() == "baixo risco" ? "Conservador" : lRow["tipo_perfil"].ToString() ,
                            
                        });
                    }
                }

            }

            return lRetorno;
        }

        /// <summary>
        /// Método que gera a listagem de registros de siglas de produtos enquadrados de acordo com o perfil
        /// para a geração do arquivo fora perfil
        /// </summary>
        public void GeraListaExecutaBatchSuitability()
        {
            var lSb = new StringBuilder();

            string lCliente = string.Empty;

            try
            {
                var lRequest = new ClienteSuitabilityEfetuadoInfo();

                lRequest.DtDe = new DateTime(2008,1,1);

                lRequest.DtAte = DateTime.Now;

                List<SuitabilityClienteProduto> lListaPerfil = ObterProdutoPerfilSuitability();

                List<ClienteSuitabilityEfetuadoInfo> lListaSuitability = ObterClientesSuitability(lRequest);

                lListaSuitability.ForEach(lista =>
                {
                    if (lista.DsPerfil.ToLower() != "arrojado" && lista.DsPerfil.ToLower() != "agressivo" )
                    {
                        List<SuitabilityClienteProduto> lSiglas = lListaPerfil.FindAll(siglas =>
                        {
                            return siglas.TipoPerfil.ToLower() == lista.DsPerfil.ToLower();

                            /*
                             * Acessado                         - Conservador
                             * CadastroNaoFinalizado            - Conservador
                             * MedioRiscoComRendaVariavel       - Moderado
                             * MEDIO RISCO COM RENDA VARIAVEL   - Moderado
                             * MedioRiscoSemRendaVariavel       - Moderado
                             * NaoResponderAgora                - Conservador
                             * BaixoRisco                       - Conservador
                             * MedioRiscoSemRendaVariavel       - Moderado
                             */
                        });

                        lCliente = "   " + lista.CodigoBovespa.ToString().PadLeft(5, ' ');

                        lSiglas.ForEach(sigla =>
                        {
                            lCliente += sigla.Produto + "     ";
                        });

                        lCliente = lCliente.Remove(lCliente.LastIndexOf("     "), 5);

                        lSb.Append(lCliente + "\n");

                        lCliente = string.Empty;
                    }
                });

                this.GerarArquivo(lSb);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que busca no sql uma listagem de clientes com suitability efetuados ou não em um range de datas
        /// Procedure: rel_cliente_suitability_efetuados_lst_sp
        /// </summary>
        /// <param name="pRequest">Objeto de request de suitability efetuado ou não, range de data De até, e se está realizado ou não</param>
        /// <returns></returns>
        private List<ClienteSuitabilityEfetuadoInfo> ObterClientesSuitability(ClienteSuitabilityEfetuadoInfo pRequest)
        {
            var lRetorno = new List<ClienteSuitabilityEfetuadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_suitability_efetuados_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@DtDe",        DbType.DateTime, pRequest.DtDe);
                    lAcessaDados.AddInParameter(lDbCommand, "@DtAte",       DbType.DateTime, pRequest.DtAte);
                    lAcessaDados.AddInParameter(lDbCommand, "@StRealizado", DbType.Int32,    1);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            DataRow lRow = lDataTable.Rows[i];

                            if (lRow["cd_codigo"].ToString() != "")
                            {
                                lRetorno.Add(new ClienteSuitabilityEfetuadoInfo()
                                {
                                    CodigoAssessor          = lRow["cd_assessor"].ToString() == "" ? 0 : int.Parse(lRow["cd_assessor"].ToString()),
                                    CodigoBovespa           = int.Parse(lRow["cd_codigo"].ToString()),
                                    DsCpfCnpj               = lRow["ds_cpfcnpj"].ToString(),
                                    DsFonte                 = lRow["ds_fonte"].ToString(),
                                    DsLoginRealizado        = lRow["ds_loginrealizado"].ToString(),
                                    DsNomeCliente           = lRow["ds_nome"].ToString(),
                                    DsPerfil = lRow["ds_perfil"].ToString().ToLower() == "baixo risco" ? "Conservador" : lRow["ds_perfil"].ToString(),
                                    //lRow["ds_perfil"].ToString(),
                                    DsStatus                = lRow["ds_status"].ToString(),
                                    DtRealizacao            = GetDateTimeNulable(lRow["dt_realizacao"]),
                                    IdCliente               = int.Parse(lRow["id_cliente"].ToString()),
                                    StPreenchidoPeloCliente = GetBoolNulable(lRow["st_preenchidopelocliente"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que trata um datetime para nullable<datetime>
        /// </summary>
        /// <param name="pDateTimeNulable">DateTime a ser tratado</param>
        /// <returns>Retorna um NUllable<Datetime></returns>
        private static Nullable<DateTime> GetDateTimeNulable(object pDateTimeNulable)
        {
            if (null == pDateTimeNulable || pDateTimeNulable.ToString().Length == 0)
            {
                return null;
            }
            else
            {
                return DateTime.Parse(pDateTimeNulable.ToString());
            }
        }

        /// <summary>
        /// Método que trata um boolean para nullable<boolean>
        /// </summary>
        /// <param name="pBoolNulable">Boolean a ser tratado</param>
        /// <returns>Retrona um Nullable<Boolean></returns>
        private static System.Nullable<Boolean> GetBoolNulable(object pBoolNulable)
        {
            if (null == pBoolNulable || pBoolNulable.ToString().Length == 0)
                return null;
            else if (Boolean.Parse(pBoolNulable.ToString()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Método que gera o arquivo fora perfil
        /// </summary>
        /// <param name="pTexto">Texto que irá ser gravado no arquivo fora perfil</param>
        public void GerarArquivo(StringBuilder pTexto)
        {
            try
            {
                string lNomeArquivo = ConfigurationManager.AppSettings["PathFORAPERFIL"].ToString() + "FORA_PERFIL.prn";
                string lRenamedArquivo = ConfigurationManager.AppSettings["PathFORAPERFIL"].ToString() + "FORA_PERFIL_" + DateTime.Now.ToString("yyyyMMdd") +".prn";

                if (!System.IO.File.Exists(lNomeArquivo))
                {
                    System.IO.File.Create(lNomeArquivo).Close();
                }
                else
                {
                    System.IO.File.Move(lNomeArquivo, lRenamedArquivo);
                    System.IO.File.Create(lNomeArquivo).Close();
                }

                System.IO.TextWriter lArquivo = System.IO.File.AppendText(lNomeArquivo);
                lArquivo.Write(pTexto);

                lArquivo.Close();

                this.EnviarEmailForaPerfil(lNomeArquivo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que envia oarquivo para responsável em roda-lo na batch
        /// </summary>
        /// <param name="pArquivo">Texto com o conteúdo do arquivo</param>
        public void EnviarEmailForaPerfil(string pArquivo)
        {
            try
            {
                string lEmailForaPerfil = ConfigurationManager.AppSettings["EmailForaPerfil"];

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();

                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();

                lEmailInfo.Arquivo = File.ReadAllBytes( pArquivo );

                lEmailInfo.Nome = "FORA_PERFIL.prn";

                lAnexos.Add(lEmailInfo);

                IServicoEmail lServico = Ativador.Get<IServicoEmail>();

                var lRequest = new EnviarEmailRequest();

                lRequest.Objeto = new EmailInfo();

                lRequest.Objeto.Assunto = "Arquivo Fora Perfil";

                lRequest.Objeto.Destinatarios = new List<string>() { lEmailForaPerfil };

                lRequest.Objeto.Remetente ="bribeiro@gradualinvestimentos.com.br";

                lRequest.Objeto.CorpoMensagem = "Bom dia, Tia.</br>Segue em anexo o arquivo fora perfil para rodar.<br/>Valeus.";

                lRequest.Objeto.Anexos = new List<EmailAnexoInfo>() { lEmailInfo };

                var lResponse = lServico.Enviar(lRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que busca no sql na base do suitability todos os cliente e seus 
        /// perfis para geração do aquivo da fato.
        /// </summary>
        public void GeraListaSuitabilityFato()
        {
            try
            {
                StringBuilder lSb = new StringBuilder();

                var lListaSuitability = new List<ClienteSuitabilityFatoInfo>();
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "Cadastro";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_suitability_file_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            DataRow lRow = lDataTable.Rows[i];

                            lListaSuitability.Add(new ClienteSuitabilityFatoInfo()
                            {
                                CodigoCliente = lRow["Codigo"].DBToInt32(),
                                CpfCliente    = lRow["Cpf_cnpj"].ToString(),
                                NomeCliente   = lRow["Nome"].ToString(),
                                Perfil        = lRow["perfil"].ToString()
                            });
                        }
                    }
                }

                lListaSuitability.ForEach(suita =>
                {
                    lSb.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", suita.CodigoCliente, suita.NomeCliente, suita.CpfCliente, suita.Perfil);
                });

                this.GerarArquivoSuitability(lSb);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que gera arquivo de suitability para a fato 
        /// </summary>
        /// <param name="pTexto">Texto com o conteúdo di arquivo que irá ser gravado na pasta específica</param>
        public void GerarArquivoSuitability(StringBuilder pTexto)
        {
            try
            {
                string lNomeArquivo = ConfigurationManager.AppSettings["PathSUITABILITYFATO"].ToString() + "SUITABILITY"+ DateTime.Now.ToString("yyyyMMdd")+".txt";
                //string lRenamedArquivo = ConfigurationManager.AppSettings["PathSUITABILITYFATO"].ToString() + "suitability_clientes_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                //if (!System.IO.File.Exists(lNomeArquivo))
                //{
                System.IO.File.Create(lNomeArquivo).Close();
                //}
                //else
                //{
                //    System.IO.File.Move(lNomeArquivo, lRenamedArquivo);
                //    System.IO.File.Create(lNomeArquivo).Close();
                //}

                System.IO.TextWriter lArquivo = System.IO.File.AppendText(lNomeArquivo);
                lArquivo.Write(pTexto);

                lArquivo.Close();

                //this.EnviarEmailForaPerfil(lNomeArquivo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
