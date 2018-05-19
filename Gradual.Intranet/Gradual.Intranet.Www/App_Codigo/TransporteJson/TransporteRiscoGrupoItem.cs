using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoGrupoItem
    {
        public TransporteRiscoGrupoItem() { }

        public TransporteRiscoGrupoItem(GrupoItemInfo pGrupoItemInfo)
        {
            this.Id = pGrupoItemInfo.CodigoGrupoItem.ToString();
            this.Nome = pGrupoItemInfo.NomeGrupoItem;
        }

        #region Propriedades

        public string Id { get; set; }

        public string Nome { get; set; }

        /// <summary>
        /// (get) Descrição completa para aparecer na listagem
        /// </summary>
        public string Descricao
        {
            get
            {
                return this.Nome;
            }
        }

        public GrupoItemInfo ToGrupoItemInfo()
        {
            GrupoItemInfo lRetorno = new GrupoItemInfo(
                new GrupoInfo()
                {
                    CodigoGrupo = this.ParentId
                });

            lRetorno.NomeGrupoItem = this.Nome;
            int idretorno = 0;
            if (int.TryParse(this.Id, out idretorno))
                lRetorno.CodigoGrupoItem = idretorno;

            return lRetorno;
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto
        {
            get { return "ItemGrupo"; }
        }

        #endregion

        public int ParentId
        {
            get;
            set;
        }
    }
}