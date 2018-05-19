#region Includes
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using Gradual.OMS.Contratos.CadastroPapeis.Mensagens;
using Gradual.OMS.Contratos.CadastroPapeis;
using Gradual.OMS.Persistencia.CadastroPapeis.Entidades;
#endregion

namespace Gradual.OMS.Host.Windows.CadastroPapeis
{
    public class Service 
    {
        //private static Hashtable ListaPapeisNegociados { get; set; }

        //public ConsultarPapelNegociadoResponse ConsultarPapelNegociado(ConsultarPapelNegociadoRequest Request)
        //{
        //    ConsultarPapelNegociadoResponse lResponse = new ConsultarPapelNegociadoResponse();

        //    PapelNegociadoBmfDbLib lPapelBmfDb = new PapelNegociadoBmfDbLib();

        //    PapelNegociadoBovespaDbLib lPapelBovespaDb = new PapelNegociadoBovespaDbLib();

        //    string keySearch = Request.Ativo;

        //    if (ListaPapeisNegociados.Contains(keySearch))
        //    {

        //    }
        //    else
        //    {
        //        lock (ListaPapeisNegociados)
        //        {
        //            ListaPapeisNegociados.Clear();

        //            //lPapelBmfDb.ListarPapelNegociadoBmf().ForEach(delegate(PapelNegociadoBmfInfo item)
        //            //{
        //            //    ListaPapeisNegociados.Add("string", item);
        //            //});

        //            //lPapelBovespaDb.ListarPapelNegociadoBovespa().ForEach(delegate(PapelNegociadoBovespaInfo item)
        //            //{
        //            //    ListaPapeisNegociados.Add("string", item);
        //            //});

        //        }
        //    }

        //    return lRetorno;
        //}
    }
}
