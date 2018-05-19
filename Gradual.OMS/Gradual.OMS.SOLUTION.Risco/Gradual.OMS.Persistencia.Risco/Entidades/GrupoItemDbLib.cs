using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Risco.Dados;
using System.Data;
using Gradual.OMS.Persistencia.Risco.DB;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia.Risco.Entidades
{
    public class GrupoItemDbLib : IEntidadeDbLib<GrupoItemInfo>
    {

        private GrupoInfo gGrupo;

        private SqlDbLib _dbLib = new SqlDbLib("RISCO");

        public GrupoItemDbLib(GrupoInfo pGrupo)
        {
            this.gGrupo = pGrupo;
        }

        #region IEntidadeDbLib<GrupoItemInfo> Members

        public ConsultarObjetosResponse<GrupoItemInfo> ConsultarObjetos(ConsultarObjetosRequest<GrupoItemInfo> lRequest)
        {

            ConsultarObjetosResponse<GrupoItemInfo> lRetorno = new ConsultarObjetosResponse<GrupoItemInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_lst", "@id_grupo", this.gGrupo.CodigoGrupo);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GrupoItemInfo lGrupoInfo = MontarObjeto(dr);
                    lRetorno.Resultado.Add(lGrupoInfo);
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
                throw (ex);
            }
        }

        public ReceberObjetoResponse<GrupoItemInfo> ReceberObjeto(ReceberObjetoRequest<GrupoItemInfo> lRequest)
        {
            ReceberObjetoResponse<GrupoItemInfo> lRetorno = new ReceberObjetoResponse<GrupoItemInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_sel", "@id_grupoitem", int.Parse(lRequest.CodigoObjeto));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lRetorno.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public RemoverObjetoResponse<GrupoItemInfo> RemoverObjeto(RemoverObjetoRequest<GrupoItemInfo> lRequest)
        {
            RemoverObjetoResponse<GrupoItemInfo> lRetorno = new RemoverObjetoResponse<GrupoItemInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_del", "@id_grupoitem", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public Contratos.Comum.Mensagens.SalvarObjetoResponse<GrupoItemInfo> SalvarObjeto(Contratos.Comum.Mensagens.SalvarObjetoRequest<GrupoItemInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<GrupoItemInfo> lResponse = new SalvarObjetoResponse<GrupoItemInfo>();

            paramsProc.Add("@id_grupo", lRequest.Objeto.Grupo.CodigoGrupo);
            paramsProc.Add("@id_grupoitem", lRequest.Objeto.CodigoGrupoItem);
            paramsProc.Add("@ds_item", lRequest.Objeto.NomeGrupoItem);
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_salvar", paramsProc);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }

            return lResponse;
        }

        #endregion

        private GrupoItemInfo MontarObjeto(DataRow dr)
        {
            GrupoItemInfo lRetorno = new GrupoItemInfo();
            lRetorno.CodigoGrupoItem = (int)dr["id_grupoitem"];
            lRetorno.NomeGrupoItem = dr["ds_item"].ToString();
            lRetorno.Grupo = this.gGrupo;
            return lRetorno;
        }
    }
}
