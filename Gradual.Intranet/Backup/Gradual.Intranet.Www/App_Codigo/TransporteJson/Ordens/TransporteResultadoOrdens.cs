using System.Collections.Generic;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Info;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Dados;
using Gradual.OMS.Ordens.Lib.Enum;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using System;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteResultadoOrdens
    {
        #region | Propriedades

        public string Id { get; set; }

        public string IdOrdem { get; set; }

        public string NumeroOrdem { get; set; }

        public string Hora { get; set; }

        public string Status { get; set; }

        public string CompraVenda { get; set; }

        public string Papel { get; set; }

        public string Quantidade { get; set; }

        public string Preco { get; set; }

        public string QuantidadeExecutada { get; set; }

        public string Tipo { get; set; }

        public string Validade { get; set; }

        public string Envida { get; set; }

        public string CodigoCliente { get; set; }

        public string Porta { get; set; }

        public string PrecoStartStop { get; set; }
        #endregion

        #region | Construtores

        public TransporteResultadoOrdens(OrdemInfo pInfo) 
        {
            Id                  = string.IsNullOrWhiteSpace(pInfo.ClOrdID.Trim()) ? pInfo.IdOrdem.ToString() : pInfo.ClOrdID;
            IdOrdem             = pInfo.IdOrdem.ToString();
            CompraVenda         = OrdemDirecaoEnum.Compra.Equals(pInfo.Side) ? "C" : "V";
            CodigoCliente       = pInfo.Account.ToString();
            Hora                = pInfo.RegisterTime.ToString("HH:mm");
            NumeroOrdem         = pInfo.ClOrdID;
            Papel               = pInfo.Symbol;
            Preco               = pInfo.Price.ToString("N2");
            Quantidade          = pInfo.OrderQty.DBToString();
            QuantidadeExecutada = (pInfo.OrderQty - pInfo.OrderQtyRemmaining).DBToString();
            Status              = this.TraduzirStatus(pInfo.OrdStatus);
            Tipo                = pInfo.TimeInForce.ToString();
            if (Status != "Executada")
            {
                QuantidadeExecutada = pInfo.CumQty.DBToString();
            }
            Validade            = (pInfo.ExpireDate.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? pInfo.ExpireDate.Value.ToString("dd/MM/yyyy") : "-";
            Porta               = pInfo.ChannelID.ToString();
            PrecoStartStop      = pInfo.StopPrice.ToString("N2");
        }

        public TransporteResultadoOrdens() { }
        #endregion

        #region | Métodos apoio

        public List<TransporteResultadoOrdens> ToListTransporteResultadoOrdens(BuscarOrdensResponse pInfo)
        {
            var lRetorno = new List<TransporteResultadoOrdens>();

            pInfo.Ordens.ForEach(delegate(OrdemInfo orfo)
            {
                lRetorno.Add(new TransporteResultadoOrdens()
                {
                    Id                  = string.IsNullOrWhiteSpace(orfo.ClOrdID.Trim()) ? orfo.IdOrdem.ToString() : orfo.ClOrdID,
                    IdOrdem             = orfo.IdOrdem.ToString(),
                    CompraVenda         = OrdemDirecaoEnum.Compra.Equals(orfo.Side) ? "C" : "V",
                    CodigoCliente       = orfo.Account.ToString(),
                    //Envida              = o,
                    Hora                = orfo.RegisterTime.ToString("HH:mm"),
                    NumeroOrdem         = orfo.ClOrdID,
                    Papel               = orfo.Symbol,
                    Preco               = orfo.Price.ToString("N2"),
                    Quantidade          = orfo.OrderQty.DBToString(),
                    QuantidadeExecutada = (orfo.OrderQty - orfo.OrderQtyRemmaining).DBToString(),
                    Status              = this.TraduzirStatus(orfo.OrdStatus),
                    Tipo                = orfo.TimeInForce.ToString(),
                    Validade            = (orfo.ExpireDate.Value.ToString("dd/MM/yyyy") != "01/01/0001") ? orfo.ExpireDate.Value.ToString("dd/MM/yyyy") : "-",
                    Porta               = orfo.ChannelID.ToString()
                });
            });

            return lRetorno;
        }

        private string TraduzirStatus(OrdemStatusEnum pInfo)
        {
            switch (pInfo)
            {
                case OrdemStatusEnum.CANCELADA:
                    return "Cancelada";

                case OrdemStatusEnum.ENVIADAPARAABOLSA:
                    return "Enviada para a bolsa";

                case OrdemStatusEnum.ENVIADAPARAOCANAL:
                    return "Enviada para o canal";

                case OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS:
                    return "Enviada para o roteador de ordens";

                case OrdemStatusEnum.EXECUTADA:
                    return "Executada";

                case OrdemStatusEnum.EXPIRADA:
                    return "Expirada";

                case OrdemStatusEnum.NOVA:
                    return "Aberta";

                //case OrdemStatusEnum.NovaPendente:
                //    return "Pendente";

                case OrdemStatusEnum.PARCIALMENTEEXECUTADA:
                    return "Parcialmente Executada";

                case OrdemStatusEnum.REJEITADA:
                    return "Rejeitada pela Bolsa";

                case OrdemStatusEnum.SUSPENSA:
                    return "Suspenso";

                case OrdemStatusEnum.SUBSTITUIDA:
                    return "Substituída";

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}