using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteClienteAssessor
    {
        #region Propriedades
        public string CodigoClienteBov { get; set; }
        public string CpfCnpj          { get; set; }
        public string NomeCliente      { get; set; }
        public string CodigoAssessor   { get; set; }
        public string NomeAssessor     { get; set; }
        public string EmailCliente     { get; set; }
        #endregion

        #region Métodos
        public List<TransporteClienteAssessor> TraduzirLista(List<ClienteResumidoInfo> pParametros  )
        {
            var lRetorno = new List<TransporteClienteAssessor>();

            pParametros.ForEach(cliente => 
            {
                var lTrans = new TransporteClienteAssessor();

                lTrans.CodigoAssessor   = cliente.CodAssessor.Value.ToString();
                lTrans.CodigoClienteBov = cliente.CodBovespa;
                lTrans.CpfCnpj          = cliente.CPF;
                lTrans.NomeCliente      = cliente.NomeCliente;
                lTrans.EmailCliente     = cliente.Email;
                ///lTrans.NomeAssessor = clie

                lRetorno.Add(lTrans);
            });


            return lRetorno;
        }
        #endregion
    }
}