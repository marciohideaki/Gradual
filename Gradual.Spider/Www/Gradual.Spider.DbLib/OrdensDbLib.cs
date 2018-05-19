using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Gradual.Generico.Dados;
using System.Text;
using System.Threading.Tasks;
using Gradual.Core.OMS.DropCopy.Lib;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using log4net;
using Gradual.Spider.Lib;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.Spider.DbLib
{
    public class OrdensDbLib
    {
        private const string ConexaoSpider = "GradualSpider";

        private const string ConexaoCadastro = "GradualCadastro";

        public BuscarOrdensResponse BuscarOrdensSpider(BuscarOrdensRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoSpider;

            BuscarOrdensResponse lRetorno = new BuscarOrdensResponse();

            lRetorno.Ordens = new List<OrdemInfo>();

            var lResponseOrdens = new List<OrdemInfo>();

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

                        lOrdem.IdOrdem             = Convert.ToInt32(lRow["OrderId"]);
                        lOrdem.Account             = lRow["Account"].DBToInt32();
                        lOrdem.ChannelID           = lRow["ChannelId"].DBToInt32();
                        lOrdem.ClOrdID             = lRow["ClOrdId"].DBToString();
                        lOrdem.ExchangeNumberID    = lRow["ExchangeNumberId"].DBToString();
                        lOrdem.ExecBroker          = lRow["ExecBroker"].DBToString();
                        lOrdem.ExpireDate          = lRow["ExpireDate"].DBToDateTime();
                        lOrdem.MaxFloor            = lRow["MaxFloor"].DBToDouble();
                        lOrdem.MinQty              = lRow["MinQty"].DBToDouble();
                        lOrdem.OrderQty            = lRow["OrderQty"].DBToInt32();
                        lOrdem.OrderQtyRemmaining  = lRow["OrderQtyRemaining"].DBToInt32();
                        lOrdem.CumQty              = lRow["CumQty"].DBToInt32();
                        lOrdem.OrdStatus           = (OrdemStatusEnum) lRow["OrdStatusId"].DBToInt32();
                        lOrdem.OrdType             = (OrdemTipoEnum)lRow["OrdTypeId"].DBToInt32();
                        lOrdem.CompIDOMS           = lRow["systemID"].DBToString();
                        lOrdem.RegisterTime        = lRow["RegisterTime"].DBToDateTime();
                        lOrdem.SecurityExchangeID  = lRow["SecurityExchangeID"].DBToString();
                        lOrdem.SecurityID          = lRow["SecurityExchangeId"].DBToString();
                        lOrdem.Side                = (OrdemDirecaoEnum) lRow["Side"].DBToInt32();
                        lOrdem.StopStartID         = lRow["StopStartID"].DBToInt32();
                        lOrdem.Symbol              = lRow["Symbol"].DBToString();
                        lOrdem.TimeInForce         = (OrdemValidadeEnum)lRow["TimeInForce"].DBToInt32();
                        lOrdem.TransactTime        = lRow["TransactTime"].DBToDateTime();
                        lOrdem.FixMsgSeqNum        = lRow["FixMsgSeqNum"].DBToInt64();

                        if (lRow["Price"] != DBNull.Value)
                            lOrdem.Price = lRow["Price"].DBToDouble();

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

        public void BuscarAssessoresFiltro(int? cd_assessor, ref List<OrdemInfo> list)
        {
            if (list.Count().Equals(0) || cd_assessor == null)
                return;

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoCadastro;

            List<int> CodBovespaBmf = new List<int>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesAssessoresVinculadosRisco_lst_sp"))
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

                IEnumerable<OrdemInfo> listTemp = from a in list where CodBovespaBmf.Contains(a.Account) select a;

                list = listTemp.ToList<OrdemInfo>();
            }
        }
    }
}
