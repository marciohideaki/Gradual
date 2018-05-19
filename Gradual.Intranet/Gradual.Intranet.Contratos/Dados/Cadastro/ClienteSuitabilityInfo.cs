using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteSuitabilityInfo : ICodigoEntidade
    {
        #region | Propriedades

        public Nullable<int> IdClienteSuitability { get; set; }

        public Nullable<int> IdCliente { get; set; }

        public Nullable<int> CdCblc { get; set; }
        
        public string ds_perfil {get;set;}

        public string ds_status  {get;set;}

        public DateTime dt_realizacao  {get;set;}

        public bool st_preenchidopelocliente  {get;set;}

        public string ds_loginrealizado  {get;set;}

        public string ds_fonte  {get;set;}

        public string ds_respostas  {get;set;}
        
        public string ds_arquivo_ciencia  {get;set;}
        
        public Nullable<DateTime> dt_arquivo_upload {get;set;}

        #endregion

        #region | ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
