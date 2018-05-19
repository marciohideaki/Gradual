using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Risco.RegraLib.Dados
{
    [Serializable]
    public class GrupoItemInfo : ICodigoEntidade
    {

        public GrupoItemInfo(GrupoInfo pGrupo) 
        {
            CodigoGrupoItem = 0;
            NomeGrupoItem = string.Empty;
            //Grupo = pGrupo;
        }

        public int CodigoGrupoItem { get; set; }

        public string NomeGrupoItem { get; set; }

        //public GrupoInfo Grupo { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoGrupoItem.ToString();
        }

        #endregion
    }
}
