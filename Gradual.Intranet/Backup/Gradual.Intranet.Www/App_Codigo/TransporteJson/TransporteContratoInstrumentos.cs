using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteContratoInstrumentos
    {
        #region Propriedades

        public List<TransporteContratoInstrumento> Contratos { get; set; }

        #endregion

        #region Construtor

        //public TransporteContratoInstrumentos(List<InstrumentoBMFInfo> pListaDeInstrumentos)
        //{
        //    //TODO: Ver se esse construtor está correto quanto às propriedades do InstrumentoBMFInfo que estamos lendo

        //    this.Contratos = new List<TransporteContratoInstrumento>();

        //    Dictionary<string, TransporteContratoInstrumento> lListaTemporaria = new Dictionary<string, TransporteContratoInstrumento>();

        //    foreach (InstrumentoBMFInfo lInstrumento in pListaDeInstrumentos)
        //    {
        //        if (!lListaTemporaria.ContainsKey(lInstrumento.CodigoMercadoria))
        //        {
        //            lListaTemporaria.Add(lInstrumento.CodigoMercadoria
        //                                 , new TransporteContratoInstrumento()
        //                                   {
        //                                       CodigoContrato = lInstrumento.CodigoMercadoria
        //                                     , NomeContrato   = lInstrumento.CodigoMercadoria
        //                                     , Instrumentos   = string.Format("{0}, ", lInstrumento.CodigoNegociacao)
        //                                   });

        //        }
        //        else
        //        {
        //            lListaTemporaria[lInstrumento.CodigoMercadoria].Instrumentos += string.Format("{0}, ", lInstrumento.CodigoNegociacao);
        //        }
        //    }

        //    foreach (string lContrato in lListaTemporaria.Keys)
        //    {
        //        lListaTemporaria[lContrato].Instrumentos = lListaTemporaria[lContrato].Instrumentos.TrimEnd(", ".ToCharArray());

        //        this.Contratos.Add(lListaTemporaria[lContrato]);
        //    }
        //}

        #endregion
    }

    public class TransporteContratoInstrumento
    {
        /// <summary>
        /// Código do contrato
        /// </summary>
        public string CodigoContrato { get; set; }
        
        /// <summary>
        /// Nome do Contrato
        /// </summary>
        public string NomeContrato { get; set; }

        /// <summary>
        /// String concatenada de instrumentos desse contrato, ex.: "DOL1, DOL2, DOL3"
        /// </summary>
        public string Instrumentos { get; set; }
    }
}