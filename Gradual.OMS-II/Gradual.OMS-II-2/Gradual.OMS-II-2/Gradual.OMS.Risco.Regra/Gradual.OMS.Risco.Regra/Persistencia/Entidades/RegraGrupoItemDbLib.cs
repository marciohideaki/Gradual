using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using log4net;
using Gradual.OMS.Persistencia;
using System.Data;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;

namespace Gradual.OMS.Risco.Persistencia.Entidades
{
    public class RegraGrupoItemDbLib : IEntidadeDbLib<RegraGrupoItemInfo>
    {
        private RegraGrupoItemInfo gRegraGrupo;

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RegraGrupoItemDbLib(){}

        //public GrupoItemDbLib(GrupoInfo pGrupo)
        //{
        //    this.gGrupo = pGrupo;
        //}
        #region Regras Grupo Item DB
        public ConsultarObjetosResponse<RegraGrupoItemInfo> ConsultarObjetos(ConsultarObjetosRequest<RegraGrupoItemInfo> lRequest)
        {

            ConsultarObjetosResponse<RegraGrupoItemInfo> lRetorno = new ConsultarObjetosResponse<RegraGrupoItemInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_grupo_regra_lst", "@id_grupo", lRequest.Objeto.CodigoGrupo);
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RegraGrupoItemInfo lRegraGrupoInfo = MontarObjeto(dr);
                    lRetorno.Resultado.Add(lRegraGrupoInfo);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw (ex);
            }
        }



        public ReceberObjetoResponse<RegraGrupoItemInfo> ReceberObjeto(ReceberObjetoRequest<RegraGrupoItemInfo> lRequest)
        {
            ReceberObjetoResponse<RegraGrupoItemInfo> lRetorno = new ReceberObjetoResponse<RegraGrupoItemInfo>();
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

        public RemoverObjetoResponse<RegraGrupoItemInfo> RemoverObjeto(RemoverBloqueioInstrumentoRequest lRequest)
        {
            RemoverObjetoResponse<RegraGrupoItemInfo> lRetorno = new RemoverObjetoResponse<RegraGrupoItemInfo>();
            
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_item_grupo_del", "@id_cliente", "@cd_ativo", "@ds_direcao", lRequest.Objeto.IdCliente, lRequest.Objeto.CdAtivo, lRequest.Objeto.Direcao);
            
                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<RegraGrupoItemInfo> SalvarObjeto(SalvarObjetoRequest<RegraGrupoItemInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<RegraGrupoItemInfo> lResponse = new SalvarObjetoResponse<RegraGrupoItemInfo>();

            paramsProc.Add("@id_grupo"      , lRequest.Objeto.CodigoGrupo);
            paramsProc.Add("@id_acao"       , lRequest.Objeto.CodigoAcao);
            paramsProc.Add("@ds_direcao"    , lRequest.Objeto.Sentido);

            try
            {
                DataSet ds = null;

                if (!lRequest.Objeto.CodigoCliente.HasValue)
                {
                    paramsProc.Add("@id_cliente", lRequest.Objeto.CodigoCliente.Value);

                    ds = _dbLib.ExecutarProcedure("prc_cliente_grupo_regra_ins", paramsProc);

                }else
                {
                    paramsProc.Add("@id_grupo_regra", lRequest.Objeto.CodigoGrupoRegra);
                    paramsProc.Add("@id_usuario", lRequest.Objeto.CodigoUsuario);

                    ds = _dbLib.ExecutarProcedure("prc_grupo_regra_ins", paramsProc);
                }

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

        private RegraGrupoItemInfo MontarObjeto(DataRow dr)
        {
            RegraGrupoItemInfo lRetorno = new RegraGrupoItemInfo();

            lRetorno.CodigoAcao       = (int)dr["id_acao"];
            lRetorno.CodigoCliente    = (int)dr["id_cliente"];
            lRetorno.CodigoGrupo      = (int)dr["id_grupo"];
            lRetorno.CodigoUsuario    = (int)dr["id_usuario"];
            lRetorno.Sentido          = dr["direcao"].ToString();
            //lRetorno.CodigoGrupoRegra = dr[""]

            return lRetorno;
        }

        

        #endregion


        #region IEntidadeDbLib<RegraGrupoItemInfo> Members


        public RemoverObjetoResponse<RegraGrupoItemInfo> RemoverObjeto(RemoverRegraGrupoItemRequest lRequest)
        {
            RemoverObjetoResponse<RegraGrupoItemInfo> lRetorno = new RemoverObjetoResponse<RegraGrupoItemInfo>();
            try
            {
                Dictionary<string, object> paramsProc = new Dictionary<string, object>();
                
                paramsProc.Add("@id_grupo_regra", lRequest.Objeto.CodigoGrupoRegra);  

                DataSet ds = _dbLib.ExecutarProcedure("prc_grupo_regra_del", paramsProc);

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        #endregion

        #region IEntidadeDbLib<RegraGrupoItemInfo> Members


        public RemoverObjetoResponse<RegraGrupoItemInfo> RemoverObjeto(RemoverObjetoRequest<RegraGrupoItemInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
