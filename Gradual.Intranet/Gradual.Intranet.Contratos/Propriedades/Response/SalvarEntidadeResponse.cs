using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response
{
    public class SalvarEntidadeResponse<T> : SalvarObjetoResponse<T>
    {
        public Nullable<int> Codigo { set; get; }

        //public  T Objeto { set; get; }
        public int IdLogin { get; set; }
        public string DsLogin { get; set; }
    
    } 
}
