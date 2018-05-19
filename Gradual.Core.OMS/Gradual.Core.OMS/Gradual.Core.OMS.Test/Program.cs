using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.Ordens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.Core.Ordens;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.Core.OrdensMonitoracao.ADM.Lib;
using Gradual.Core.OrdensMonitoracao.ADM.Lib.Mensagens;


namespace Gradual.Core.OMS.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            EnviarOrdemRequest  request  = new EnviarOrdemRequest();
            EnviarOrdemResponse response = new EnviarOrdemResponse();

            ServicoOrdens ServicoOrdens = new ServicoOrdens();

            request.ClienteOrdemInfo.Account       = 31940;            
            request.ClienteOrdemInfo.TransactTime  = DateTime.Now;
            request.ClienteOrdemInfo.Side          = OrdemDirecaoEnum.Venda;
            request.ClienteOrdemInfo.Symbol        = "BVMF3";
            request.ClienteOrdemInfo.ChannelID     = 800;
            request.ClienteOrdemInfo.Price         = 12;                 
            request.ClienteOrdemInfo.OrderQty      = 150;
            request.ClienteOrdemInfo.CumQty        = 0;
            request.ClienteOrdemInfo.MinQty        = 0;
            request.ClienteOrdemInfo.OrdType       = OrdemTipoEnum.Limitada;
            request.ClienteOrdemInfo.TimeInForce   = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;

            response = ServicoOrdens.EnviarOrdem(request);            
        }
    }
}
