#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteOrdemDetalhe
    {
        public string CodigoMensagem { get; set; }
        
        public double QuantidadeSolicitada { get; set; }

        public double QuantidadeRemanescente { get; set; }

        public double QuantidadeExecutada { get; set; }

        public string Situacao { get; set; }

        public string Data { get; set; }
        
        public string Descricao { get; set; }

        public string NumeroIdentificador { get; set; }

        public string Preco { get; set; }

        public TransporteOrdemDetalhe(AcompanhamentoOrdemInfo info)
        {
            this.CodigoMensagem         = info.CodigoResposta;
            this.QuantidadeSolicitada   = info.QuantidadeSolicitada;
            this.QuantidadeRemanescente = info.QuantidadeRemanescente;
            this.QuantidadeExecutada    = info.QuantidadeExecutada;
            this.Situacao               = info.StatusOrdem.ToString(); ;
            this.Data                   = info.DataAtualizacao.ToString("dd/MM/yyyy HH:mm:ss"); ;
            this.Descricao              = info.Descricao;
            this.Preco                  = info.Preco.ToString("N2");
            //this.NumeroIdentificador    ="";
        }


        public TransporteOrdemDetalhe() { }
    }
}