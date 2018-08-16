using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco
{
    public class TransporteRelatorio_010 : ICodigoEntidade
    {
        public string CodigoAssessor { get; set; }

        public string CodigoCliente { get; set; }

        public string NomeCliente   { get; set; }
            
        public string CodigoNegocio { get; set; }

        public string QtdeTotal     { get; set; }

        public string QtdeD1        { get; set; }

        public string QtdeD2        { get; set; }

        public string QtdeD3        { get; set; }

        public List<TransporteRelatorio_010> TraduzirLista(List<RiscoClienteMercadoVistaOpcaoRelInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_010>();

            pParametros.ForEach(rel => 
            {
                var lTrans = new TransporteRelatorio_010();

                lTrans.CodigoAssessor = rel.CodigoAssessor.ToString();
                lTrans.CodigoCliente  = rel.CodigoCliente.ToString();
                lTrans.NomeCliente    = rel.NomeCliente;
                lTrans.CodigoNegocio  = rel.CodigoNegocio;
                lTrans.QtdeTotal      = rel.QtdeTotal;
                lTrans.QtdeD1         = rel.QtdeD1;
                lTrans.QtdeD2         = rel.QtdeD2;
                lTrans.QtdeD3         = rel.QtdeD3;

                lRetorno.Add(lTrans);
            });

            return lRetorno;
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}