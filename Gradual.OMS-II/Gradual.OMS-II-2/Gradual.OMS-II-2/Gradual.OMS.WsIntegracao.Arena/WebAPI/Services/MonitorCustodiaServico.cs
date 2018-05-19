using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.OMS.WsIntegracao.Arena.Models;
using log4net;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public class MonitorCustodiaServico : ServicoBase
    {
        #region Prorpiedades
        public IServicoMonitorCustodia gServicoCustodia;
        #endregion

        #region Construtores
        public MonitorCustodiaServico()
        {
            gServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();
     
        }
        #endregion

        #region Métodos
        public MonitorCustodiaInfo GetPosicaoCustodiaCliente(int CodigoBovespa)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            try
            {
                MonitorCustodiaRequest lRequest = new MonitorCustodiaRequest();

                MonitorCustodiaResponse lResponse = new MonitorCustodiaResponse();

                lRequest.CodigoCliente = CodigoBovespa;

                lResponse = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = lResponse.MonitorCustodia;


                }
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        private Cliente RetornaCodigoBovespaBmfClienteGradual(int IdClienteGradual)
        {
            Cliente lRetorno = null;

            try
            {

            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }

            return lRetorno;
        }

        public static List<int> ListarClientesComCustodiaCC()
        {
            var lListaCliente = new List<int>();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUSTODIA_ABERTURA_CLI_LST"))
                {
                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        gLogger.InfoFormat("Foram encontrados [{0}] Cliente(s) com custódia", lDataTable.Rows.Count);

                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lListaCliente.Add(lDataTable.Rows[i]["cd_cliente"].DBToInt32());
                        }
                    }
                }

                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            gLogger.InfoFormat("Foram Encontrados [{0}] clientes para carregar custodia de abertura", lListaCliente.Count);

            return lListaCliente;
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaAbertura(Nullable<int> CodigoCliente)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_POSI"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            if (lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPF") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("FUT") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPD"))
                            {
                                continue;
                            }

                            lRetorno.ListaCustodia.Add(new MonitorCustodiaInfo.CustodiaPosicao()
                            {
                                CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                                CodigoCarteira    = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                                DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                                TipoMercado       = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                                TipoGrupo         = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                                IdCliente         = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                                QtdeAtual         = lDataTable.Rows[i]["QTDE_ATUAL"].DBToDecimal(),
                                QtdeLiquidar      = lDataTable.Rows[i]["QTDE_LIQUID"].DBToDecimal(),
                                QtdeDisponivel    = lDataTable.Rows[i]["QTDE_DISP"].DBToDecimal(),
                                QtdeAExecVenda    = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToDecimal(),
                                QtdeAExecCompra   = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToDecimal(),
                                NomeEmpresa       = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                                ValorPosicao      = lDataTable.Rows[i]["VAL_POSI"].DBToDecimal(),
                                DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(Extensions.eDateNull.Permite),
                                QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                                QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                                QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                                CodigoSerie       = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                                FatorCotacao      = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                                QtdeDATotal       = lDataTable.Rows[i]["QTDE_DATOTAL"].DBToDecimal(),
                            });
                        }
                }
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno.ListaCustodia;
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ConsultarCustodiaPosicaoDiaBMF(MonitorCustodiaInfo pParametros)
        {
            var lRetorno = new MonitorCustodiaInfo();

            try
            {

                var lAcessaDados = new AcessaDados();

                lRetorno.ListaPosicaoDiaBMF = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_DIA"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                            lPosicao.CodigoInstrumento = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lPosicao.TipoMercado       = "FUT";
                            lPosicao.TipoGrupo         = "FUT";
                            lPosicao.IdCliente         = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                            lPosicao.QtdeAtual         = 0;
                            lPosicao.QtdeDisponivel    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                            lPosicao.CodigoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();

                            string lSentido = lDataTable.Rows[i]["CD_NATOPE"].DBToString();

                            lPosicao.Sentido = lSentido;

                            if (lSentido.Equals("V"))
                            {
                                lPosicao.QtdeAExecVenda    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                                lPosicao.PrecoNegocioVenda = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                            }
                            else if (lSentido.Equals("C"))
                            {
                                lPosicao.QtdeAExecCompra    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                                lPosicao.PrecoNegocioCompra = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                            }

                            lRetorno.ListaPosicaoDiaBMF.Add(lPosicao);
                        }
                }

                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return lRetorno.ListaPosicaoDiaBMF;
        }
        #endregion
    }
}