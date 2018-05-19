using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public interface ITransporteJSON
    {
        /// <summary>
        /// Id do Investidor não residente
        /// </summary>       
        Nullable<int> Id { get; set; }

        /// <summary>
        /// Código do Cliente
        /// </summary>       
        int ParentId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        string TipoDeItem { get;}
    }
}