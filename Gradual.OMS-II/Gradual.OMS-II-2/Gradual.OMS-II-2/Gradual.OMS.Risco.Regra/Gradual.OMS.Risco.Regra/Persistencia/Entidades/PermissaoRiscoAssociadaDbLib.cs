using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using System.Data;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using log4net;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class PermissaoRiscoAssociadaDbLib : IEntidadeDbLib<PermissaoRiscoAssociadaInfo>
    {
        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IEntidadeDbLib<PermissaoRiscoAssociadaInfo> Members

        public ConsultarObjetosResponse<PermissaoRiscoAssociadaInfo> ConsultarObjetos(ConsultarObjetosRequest<PermissaoRiscoAssociadaInfo> lRequest)
        {
            ConsultarObjetosResponse<PermissaoRiscoAssociadaInfo> lRetorno = new ConsultarObjetosResponse<PermissaoRiscoAssociadaInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_permissao_lst", "@id_cliente", lRequest.Condicoes[0].Valores[0]);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lRetorno.Resultado.Add(MontarObjeto(ds.Tables[0].Rows[i]));
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public ReceberObjetoResponse<PermissaoRiscoAssociadaInfo> ReceberObjeto(ReceberObjetoRequest<PermissaoRiscoAssociadaInfo> lRequest)
        {
            ReceberObjetoResponse<PermissaoRiscoAssociadaInfo> lRetorno = new ReceberObjetoResponse<PermissaoRiscoAssociadaInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_permissao_sel", "@id_cliente_permissao", int.Parse(lRequest.CodigoObjeto));
            try
            {
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

        public RemoverObjetoResponse<PermissaoRiscoAssociadaInfo> RemoverObjeto(RemoverObjetoRequest<PermissaoRiscoAssociadaInfo> lRequest)
        {
            RemoverObjetoResponse<PermissaoRiscoAssociadaInfo> lRetorno = new RemoverObjetoResponse<PermissaoRiscoAssociadaInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_permissao_del", "@id_cliente", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<PermissaoRiscoAssociadaInfo> SalvarObjeto(SalvarObjetoRequest<PermissaoRiscoAssociadaInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<PermissaoRiscoAssociadaInfo> lResponse = new SalvarObjetoResponse<PermissaoRiscoAssociadaInfo>();

            paramsProc.Add("@id_cliente_permissao", lRequest.Objeto.CodigoPermissaoRiscoAssociada);
            paramsProc.Add("@id_cliente", lRequest.Objeto.CodigoCliente);
            paramsProc.Add("@id_permissao", lRequest.Objeto.PermissaoRisco.CodigoPermissao);

            if (lRequest.Objeto.Grupo != null)
                paramsProc.Add("@id_grupo", lRequest.Objeto.Grupo.CodigoGrupo);

            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_permissao_salvar", paramsProc);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }

                return lResponse;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }

        }

        #endregion

        private PermissaoRiscoAssociadaInfo MontarObjeto(DataRow dr)
        {
            PermissaoRiscoAssociadaInfo lRetorno = new PermissaoRiscoAssociadaInfo();

            lRetorno.CodigoCliente = (int)dr["id_cliente"];
            lRetorno.CodigoPermissaoRiscoAssociada = (int)dr["id_cliente_permissao"];
            if (!Convert.IsDBNull(dr["id_grupo"]))
                lRetorno.Grupo = new GrupoDbLib().ReceberObjeto(new ReceberObjetoRequest<GrupoInfo>(){CodigoObjeto = dr["id_grupo"].ToString()}).Objeto;
            lRetorno.PermissaoRisco = new PermissaoRiscoDbLib().ReceberObjeto(new ReceberObjetoRequest<PermissaoRiscoInfo>() { CodigoObjeto = dr["id_permissao"].ToString() }).Objeto;

            return lRetorno;
        }
    }
}
