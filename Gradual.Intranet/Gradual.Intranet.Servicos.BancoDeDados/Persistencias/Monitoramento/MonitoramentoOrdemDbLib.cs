using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Gradual.Generico.Dados;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public class MonitoramentoOrdemDbLib
    {
        #region Métodos de busca

        public BuscarOrdensResponse BuscarOrdensSpider(BuscarOrdensRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoSpider;

            BuscarOrdensResponse lRetorno = new BuscarOrdensResponse();

            lRetorno.Ordens = new List<OrdemInfo>();

            List<OrdemInfo> lResponseOrdens = new List<OrdemInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_fix_buscar_ordem_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@CodBmf", DbType.Int32, pRequest.CodigoBmfDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte", DbType.DateTime, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "@ChannelId", DbType.Int32, pRequest.Canal);
                lAcessaDados.AddInParameter(lDbCommand, "@Symbol", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "@OrdStatusId", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pRequest.CodigoAssessor);
                //lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pRequest.IdSistemaOrigem);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lResponseOrdens = PreencherOrdensInfoDaVwOrderNovoOMSDetails(lDataTable);

                this.BuscarAssessoresFiltro(pRequest.CodigoAssessor, ref lResponseOrdens);

                lRetorno.Ordens = lResponseOrdens;
            }
            return lRetorno;
        }

        public BuscarOrdensResponse BuscarOrdensNovoOMS(BuscarOrdensRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            BuscarOrdensResponse lRetorno = new BuscarOrdensResponse();

            lRetorno.Ordens = new List<OrdemInfo>();

            List<OrdemInfo> lResponseOrdens = new List<OrdemInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_buscar_ordens_gti_hb_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@CodBmf", DbType.Int32, pRequest.CodigoBmfDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte", DbType.DateTime, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "@ChannelId", DbType.Int32, pRequest.Canal);
                lAcessaDados.AddInParameter(lDbCommand, "@Symbol", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "@OrdStatusId", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pRequest.CodigoAssessor);
                //lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pRequest.IdSistemaOrigem);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lResponseOrdens = PreencherOrdensInfoDaVwOrderNovoOMSDetails(lDataTable);

                this.BuscarAssessoresFiltro(pRequest.CodigoAssessor, ref lResponseOrdens);

                lRetorno.Ordens = lResponseOrdens;
            }
            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAccount"></param>
        /// <param name="pCodBmf"></param>
        /// <param name="pDataDe"></param>
        /// <param name="pDataAte"></param>
        /// <param name="pChannelId"></param>
        /// <param name="pSymbol"></param>
        /// <param name="pOrderStatusId"></param>
        /// <param name="pCodigoAssessor"></param>
        /// <returns></returns>
        public BuscarOrdensResponse BuscarOrdens(BuscarOrdensRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            BuscarOrdensResponse lRetorno = new BuscarOrdensResponse();

            lRetorno.Ordens = new List<OrdemInfo>();

            List<OrdemInfo> lResponseOrdens = new List<OrdemInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_buscar_ordens"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@CodBmf", DbType.Int32, pRequest.CodigoBmfDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte", DbType.DateTime, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "@ChannelId", DbType.Int32, pRequest.Canal);
                lAcessaDados.AddInParameter(lDbCommand, "@Symbol", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "@OrdStatusId", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pRequest.CodigoAssessor);
                //lAcessaDados.AddInParameter(lDbCommand, "@id_sistema", DbType.Int32, pRequest.IdSistemaOrigem);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lResponseOrdens = PreencherOrdensInfoDaVwOrderDetails(lDataTable);

                this.BuscarAssessoresFiltro(pRequest.CodigoAssessor, ref lResponseOrdens);

                lRetorno.Ordens = lResponseOrdens;
            }
            return lRetorno;
        }

        /// <summary>
        /// Busca assessores para filtro 
        /// </summary>
        /// <param name="cd_assessor">Código do assessor</param>
        public void BuscarAssessoresFiltro(int? cd_assessor, ref List<OrdemInfo> list)
        {
            if (list.Count().Equals(0) || cd_assessor == null)
                return;

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            List<int> CodBovespaBmf = new List<int>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_assessores_monitoramento_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, cd_assessor);

                DataTable lDados = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                foreach (DataRow lRow in lDados.Rows)
                {
                    if (!CodBovespaBmf.Contains(lRow["cd_bovespa"].DBToInt32()))
                    {
                        if (!lRow["cd_bovespa"].DBToInt32().Equals(0))
                            CodBovespaBmf.Add(lRow["cd_bovespa"].DBToInt32());

                        if (!lRow["cd_bmf"].DBToInt32().Equals(0))
                            CodBovespaBmf.Add(lRow["cd_bmf"].DBToInt32());
                    }
                }

                IEnumerable<OrdemInfo> listTemp = from a in list where  CodBovespaBmf.Contains(a.Account) select a;

                list = listTemp.ToList<OrdemInfo>();
            }
        }

        /// <summary>
        /// Busca ordens inseridas no sinacor
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public BuscarOrdensResponse BuscarOrdensSinacor(BuscarOrdensRequest pRequest)
        {
            BuscarOrdensResponse lReturn = new BuscarOrdensResponse();

            OrdemInfo lOrdem = new OrdemInfo();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            ///PRC_ACOMP_ORDENS_BMF_LST
            //PRC_ACOMPANHAMENTO_ORDENS_LST
            //string lProc = (pRequest.Canal == 0) ? "PRC_ACOMP_ORDENS_BMF_LST" : "PRC_ACOMPANHAMENTO_ORDENS_LST";
            string lProc = (pRequest.Canal == 0) ? "PRC_ACOMP_ORDENS_BMF_LST" : "PRC_ACOMPA_ORDENS_LST";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProc))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDtDe", DbType.Date, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDtAte", DbType.Date, pRequest.DataAte);
                lAcessaDados.AddInParameter(lDbCommand, "pPapel", DbType.AnsiString, pRequest.Instrumento);
                lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, pRequest.ContaDoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pPaginaCorrente", DbType.Int32, pRequest.PaginaCorrente);
                lAcessaDados.AddInParameter(lDbCommand, "pQtdRegs", DbType.Int32, pRequest.QtdeLimiteRegistros);
                lAcessaDados.AddInParameter(lDbCommand, "pCodAssessor", DbType.Int32, pRequest.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pStatusOrdem", DbType.Int32, pRequest.Status == null ? new Nullable<int>() : (int)pRequest.Status);
                lAcessaDados.AddInParameter(lDbCommand, "pStPortaTryd", DbType.String, "TRYD".Equals(pRequest.Origem) ? pRequest.Origem : null);

                lAcessaDados.AddOutParameter(lDbCommand, "pTotalRegistros", DbType.Int32, 12);

                DataTable lDados = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                lReturn.Ordens = new List<OrdemInfo>();

                lReturn.TotalItens = lDados.Rows.Count;

                foreach (DataRow lRow in lDados.Rows)
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account            = lRow["CD_CLIENTE"].DBToInt32();
                    lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.ClOrdID            = lRow["OrderId"].ToString();
                    lOrdem.OrdType            = (OrdemTipoEnum)50;
                    lOrdem.RegisterTime       = DateTime.ParseExact(lRow["RegisterTime"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
                    lOrdem.Side               = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.Symbol             = lRow["SYMBOL"].DBToString();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = Convert.ToDouble( lRow["Price"]);

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lReturn.Ordens.Add(lOrdem);
                }

                List<OrdemInfo> lTempOrdens = lReturn.Ordens;

                this.BuscarAssessoresFiltro(pRequest.CodigoAssessor, ref lTempOrdens);

                lReturn.Ordens = lTempOrdens;
            }

            return lReturn;
        }

        private List<OrdemInfo> PreencherOrdensInfoDaVwOrderDetails(DataTable pTableResultado)
        {
            string lUltimoId = "";

            List<OrdemInfo> lRetorno = new List<OrdemInfo>();

            OrdemInfo lOrdem;
            AcompanhamentoOrdemInfo lAcompanhamento;

            foreach (DataRow lRow in pTableResultado.Rows)
            {
                if (lUltimoId != lRow["ClOrdId"].ToString())
                {
                    lOrdem = new OrdemInfo();

                    lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                    lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                    lOrdem.Account            = lRow["Account"].DBToInt32();
                    lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                    lOrdem.ClOrdID            = lRow["ClOrdId"].DBToString();
                    lOrdem.ExchangeNumberID   = lRow["ExchangeNumberId"].DBToString();
                    lOrdem.ExecBroker         = lRow["ExecBroker"].DBToString();
                    lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                    lOrdem.MaxFloor           = (lRow["MaxFloor"] != DBNull.Value) ?   Convert.ToDouble(lRow["MaxFloor"]) : new Nullable<double>() ;
                    lOrdem.MinQty             = (lRow["MinQty"] != DBNull.Value) ? Convert.ToDouble(lRow["MinQty"]) : new Nullable<double>();
                    lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                    lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                    lOrdem.CumQty             = lRow["CumQty"].DBToInt32();
                    lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                    lOrdem.OrdType            = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();
                    
                    //if (lRow["systemID"] != DBNull.Value )
                    //{
                    //    lOrdem.CompIDOMS = lRow["systemID"].DBToString();
                    //}
                    lOrdem.RegisterTime       = lRow["RegisterTime"].DBToDateTime();
                    lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                    lOrdem.SecurityID         = lRow["SecurityExchangeId"].DBToString();


                    lOrdem.Side         = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lOrdem.StopStartID  = lRow["StopStartID"].DBToInt32();
                    lOrdem.Symbol       = lRow["Symbol"].DBToString();
                    lOrdem.TimeInForce  = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                    lOrdem.TransactTime = lRow["TransactTime"].DBToDateTime();
                    //lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32().ToString();

                    if (lRow["Price"] != DBNull.Value)
                        lOrdem.Price = Convert.ToDouble(lRow["Price"]);

                    if (lRow["OrdTypeId"].DBToInt32().Equals(52))
                        lOrdem.StopPrice = Convert.ToDouble(lRow["OrderDetail_Price"]);

                    lRetorno.Add(lOrdem);

                    lUltimoId = lOrdem.ClOrdID;
                }

                //Não é dentro de 'else' porque a primeira linha tem o primeiro acompanhamento:

                lAcompanhamento                     = new AcompanhamentoOrdemInfo();
                lAcompanhamento.CanalNegociacao     = lRow["ChannelID"].DBToInt32();
                lAcompanhamento.CodigoDoCliente     = lRow["Account"].DBToInt32();
                lAcompanhamento.CodigoResposta      = lRow["OrderDetail_TransactId"].DBToString();
                lAcompanhamento.CodigoTransacao     = lRow["OrderDetail_TransactId"].DBToString();
                lAcompanhamento.DataAtualizacao     = lRow["OrderDetail_TransactTime"].DBToDateTime();
                lAcompanhamento.DataOrdemEnvio      = lRow["RegisterTime"].DBToDateTime();
                lAcompanhamento.DataValidade        = lRow["ExpireDate"].DBToDateTime();
                lAcompanhamento.Direcao             = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                lAcompanhamento.Instrumento         = lRow["Symbol"].DBToString();
                lAcompanhamento.NumeroControleOrdem = lRow["ClOrdId"].DBToString();
                lAcompanhamento.Descricao           = lRow["OrderDetail_Description"].DBToString();

                lAcompanhamento.QuantidadeRemanescente = lRow["OrderDetail_OrdQtyRemaining"].DBToInt32();
                lAcompanhamento.QuantidadeExecutada    = lRow["OrderDetail_CumQty"].DBToInt32();
                lAcompanhamento.QuantidadeSolicitada   = lRow["OrderDetail_OrderQty"].DBToInt32();
                lAcompanhamento.QuantidadeNegociada    = lRow["OrderDetail_TradeQty"].DBToInt32();

                lAcompanhamento.StatusOrdem  = (OrdemStatusEnum)lRow["OrderDetail_OrderStatusId"].DBToInt32();
                //lAcompanhamento.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32().ToString();

                if (lRow["OrderDetail_Price"] != DBNull.Value)
                    lAcompanhamento.Preco = lRow["OrderDetail_Price"].DBToDecimal();

                lRetorno[lRetorno.Count - 1].Acompanhamentos.Add(lAcompanhamento);

            }

            return lRetorno;
        }

        private List<OrdemInfo> PreencherOrdensInfoDaVwOrderNovoOMSDetails(DataTable pTableResultado)
        {
            List<OrdemInfo> lRetorno = new List<OrdemInfo>();
            
            try
            {
                string lUltimoId = "";

                OrdemInfo lOrdem;
                AcompanhamentoOrdemInfo lAcompanhamento;

                foreach (DataRow lRow in pTableResultado.Rows)
                {
                    if (lUltimoId != lRow["ClOrdId"].ToString())
                    {
                        lOrdem = new OrdemInfo();

                        lOrdem.Acompanhamentos = new List<AcompanhamentoOrdemInfo>();

                        lOrdem.IdOrdem            = Convert.ToInt32(lRow["OrderId"]);
                        lOrdem.Account            = lRow["Account"].DBToInt32();
                        lOrdem.ChannelID          = lRow["ChannelId"].DBToInt32();
                        lOrdem.ClOrdID            = lRow["ClOrdId"].DBToString();
                        lOrdem.ExchangeNumberID   = lRow["ExchangeNumberId"].DBToString();
                        lOrdem.ExecBroker         = lRow["ExecBroker"].DBToString();
                        lOrdem.ExpireDate         = lRow["ExpireDate"].DBToDateTime();
                        lOrdem.MaxFloor           = (lRow["MaxFloor"] != DBNull.Value) ? Convert.ToDouble(lRow["MaxFloor"]) : new Nullable<double>();
                        lOrdem.MinQty             = (lRow["MinQty"] != DBNull.Value) ? Convert.ToDouble(lRow["MinQty"]) : new Nullable<double>();
                        lOrdem.OrderQty           = lRow["OrderQty"].DBToInt32();
                        lOrdem.OrderQtyRemmaining = lRow["OrderQtyRemaining"].DBToInt32();
                        lOrdem.CumQty             = lRow["CumQty"].DBToInt32();
                        lOrdem.OrdStatus          = (OrdemStatusEnum)lRow["OrdStatusId"].DBToInt32();
                        lOrdem.OrdType            = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();
                        lOrdem.CompIDOMS          = lRow["systemID"].DBToString();
                        lOrdem.RegisterTime       = lRow["RegisterTime"].DBToDateTime();
                        lOrdem.SecurityExchangeID = lRow["SecurityExchangeID"].DBToString();
                        lOrdem.SecurityID         = lRow["SecurityExchangeId"].DBToString();
                        lOrdem.Side               = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                        lOrdem.StopStartID        = lRow["StopStartID"].DBToInt32();
                        lOrdem.Symbol             = lRow["Symbol"].DBToString();
                        lOrdem.TimeInForce        = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                        lOrdem.TransactTime       = lRow["TransactTime"].DBToDateTime();
                        //lOrdem.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32().ToString();

                        if (lRow["Price"] != DBNull.Value)
                            lOrdem.Price = Convert.ToDouble(lRow["Price"]);

                        if (lRow["OrdTypeId"].DBToInt32().Equals(52))
                            lOrdem.StopPrice = Convert.ToDouble(lRow["OrderDetail_Price"]);

                        lRetorno.Add(lOrdem);

                        lUltimoId = lOrdem.ClOrdID;
                    }

                    //Não é dentro de 'else' porque a primeira linha tem o primeiro acompanhamento:

                    lAcompanhamento = new AcompanhamentoOrdemInfo();
                    lAcompanhamento.CanalNegociacao        = lRow["ChannelID"].DBToInt32();
                    lAcompanhamento.CodigoDoCliente        = lRow["Account"].DBToInt32();
                    lAcompanhamento.CodigoResposta         = lRow["OrderDetail_TransactId"].DBToString();
                    lAcompanhamento.CodigoTransacao        = lRow["OrderDetail_TransactId"].DBToString();
                    lAcompanhamento.DataAtualizacao        = lRow["OrderDetail_TransactTime"].DBToDateTime();
                    lAcompanhamento.DataOrdemEnvio         = lRow["RegisterTime"].DBToDateTime();
                    lAcompanhamento.DataValidade           = lRow["ExpireDate"].DBToDateTime();
                    lAcompanhamento.Direcao                = (OrdemDirecaoEnum)lRow["Side"].DBToInt32();
                    lAcompanhamento.Instrumento            = lRow["Symbol"].DBToString();
                    lAcompanhamento.NumeroControleOrdem    = lRow["ClOrdId"].DBToString();
                    lAcompanhamento.Descricao              = lRow["OrderDetail_Description"].DBToString();
                    lAcompanhamento.QuantidadeRemanescente = lRow["OrderDetail_OrdQtyRemaining"].DBToInt32();
                    lAcompanhamento.QuantidadeExecutada    = lRow["OrderDetail_CumQty"].DBToInt32();
                    lAcompanhamento.QuantidadeSolicitada   = lRow["OrderDetail_OrderQty"].DBToInt32();
                    lAcompanhamento.QuantidadeNegociada    = lRow["OrderDetail_TradeQty"].DBToInt32();

                    lAcompanhamento.StatusOrdem = (OrdemStatusEnum)lRow["OrderDetail_OrderStatusId"].DBToInt32();
                    //lAcompanhamento.FixMsgSeqNum = lRow["FixMsgSeqNum"].DBToInt32().ToString();

                    if (lRow["OrderDetail_Price"] != DBNull.Value)
                        lAcompanhamento.Preco = lRow["OrderDetail_Price"].DBToDecimal();

                    lRetorno[lRetorno.Count - 1].Acompanhamentos.Add(lAcompanhamento);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lRetorno;
        }
        ///PRC_ACOMP_ORDENS_BMF_LST3

        public BuscarSistemaOrigemResponse BuscarSistemaOrigem(BuscarSistemaOrigemRequest pParametro)
        {
            var lRetorno = new BuscarSistemaOrigemResponse();
            var lAcessaDados = new AcessaDados();

            lRetorno.Resultado = new List<SistemaOrigemInfo>();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sistema_lst"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new SistemaOrigemInfo()
                        {
                             DsSistema = lLinha["ds_sistema"].DBToString(),
                             IdSistema = lLinha["id_sistema"].DBToInt32(),
                        });
            }

            return lRetorno;
        }

        #endregion
    }
}
