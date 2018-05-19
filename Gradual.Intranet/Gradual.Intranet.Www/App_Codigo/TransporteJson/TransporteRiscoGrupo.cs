using System;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoGrupo : ICodigoEntidade
    {
        #region | Propriedades

        public string Id { get; set; }

        public string Descricao { get; set; }

        public GrupoInfo ToGrupoInfo()
        {
            var lRetorno = new GrupoInfo();

            lRetorno.NomeDoGrupo = this.Descricao;

            int id = 0;

            if (int.TryParse(this.Id, out id))
                lRetorno.CodigoGrupo = id;

            return lRetorno;
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Grupo"; } }

        #endregion

        #region | Construtores

        public TransporteRiscoGrupo() { }

        public TransporteRiscoGrupo(GrupoInfo pUsuarioGrupoInfo)
        {
            this.Id = pUsuarioGrupoInfo.CodigoGrupo.ToString();
            this.Descricao = pUsuarioGrupoInfo.NomeDoGrupo;
        }

        #endregion

        #region | Métodos

        public List<TransporteRiscoGrupo> TraduzirLista(List<GrupoInfo> pParametros)
        {
            var lRetorno = new List<TransporteRiscoGrupo>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(gif =>
                {
                    lRetorno.Add(new TransporteRiscoGrupo()
                    {
                        Descricao = gif.NomeDoGrupo,
                        Id = gif.CodigoGrupo.DBToString()
                    });
                });

            return lRetorno;
        }

        #endregion

        #region | Implementaca Interfae

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}