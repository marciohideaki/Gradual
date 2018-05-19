using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using System.Data;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using log4net;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class GrupoItemDbLib : IEntidadeDbLib<GrupoItemInfo>
    {

        private GrupoInfo gGrupo;

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GrupoItemDbLib(){}

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
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_lst", "@id_grupo", this.gGrupo.CodigoGrupo, "@tp_grupo", lRequest.Objeto.NomeGrupoItem);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GrupoItemInfo lGrupoInfo = MontarObjeto(dr);
                    lRetorno.Resultado.Add(lGrupoInfo);
                }
                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
                logger.Error(lRequest, ex);
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
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<GrupoItemInfo> SalvarObjeto(SalvarObjetoRequest<GrupoItemInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<GrupoItemInfo> lResponse = new SalvarObjetoResponse<GrupoItemInfo>();

            paramsProc.Add("@id_grupo", lRequest.Objeto.CodigoGrupo);
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
                logger.Error(lRequest, ex);
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
            lRetorno.CodigoGrupo = (int)dr["id_grupo"];
            return lRetorno;
        }
    }
}
