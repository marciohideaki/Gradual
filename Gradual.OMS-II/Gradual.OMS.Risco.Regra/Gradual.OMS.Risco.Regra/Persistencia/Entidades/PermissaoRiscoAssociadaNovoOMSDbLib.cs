using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Persistencia;
using log4net;
using System.Data;

namespace Gradual.OMS.Risco.Persistencia.Entidades
{
    public class PermissaoRiscoAssociadaNovoOMSDbLib : IEntidadeDbLib<PermissaoRiscoAssociadaNovoOMSInfo>
    {
        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IEntidadeDbLib<PermissaoRiscoAssociadaNovoOMSInfo> Members

        public ConsultarObjetosResponse<PermissaoRiscoAssociadaNovoOMSInfo> ConsultarObjetos(ConsultarObjetosRequest<PermissaoRiscoAssociadaNovoOMSInfo> lRequest)
        {
            ConsultarObjetosResponse<PermissaoRiscoAssociadaNovoOMSInfo> lRetorno = new ConsultarObjetosResponse<PermissaoRiscoAssociadaNovoOMSInfo>();
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

        public ReceberObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> ReceberObjeto(ReceberObjetoRequest<PermissaoRiscoAssociadaNovoOMSInfo> lRequest)
        {
            ReceberObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> lRetorno = new ReceberObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo>();
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

        public RemoverObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> RemoverObjeto(RemoverObjetoRequest<PermissaoRiscoAssociadaNovoOMSInfo> lRequest)
        {
            RemoverObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> lRetorno = new RemoverObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo>();
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

        public SalvarObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> SalvarObjeto(SalvarObjetoRequest<PermissaoRiscoAssociadaNovoOMSInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo> lResponse = new SalvarObjetoResponse<PermissaoRiscoAssociadaNovoOMSInfo>();

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

        private PermissaoRiscoAssociadaNovoOMSInfo MontarObjeto(DataRow dr)
        {
            PermissaoRiscoAssociadaNovoOMSInfo lRetorno = new PermissaoRiscoAssociadaNovoOMSInfo();

            lRetorno.CodigoCliente = (int)dr["id_cliente"];
            lRetorno.CodigoPermissaoRiscoAssociada = (int)dr["id_cliente_permissao"];
            if (!Convert.IsDBNull(dr["id_grupo"]))
                lRetorno.Grupo = new GrupoDbLib().ReceberObjeto(new ReceberObjetoRequest<GrupoInfo>() { CodigoObjeto = dr["id_grupo"].ToString() }).Objeto;
            lRetorno.PermissaoRisco = new PermissaoRiscoDbLib().ReceberObjeto(new ReceberObjetoRequest<PermissaoRiscoInfo>() { CodigoObjeto = dr["id_permissao"].ToString() }).Objeto;

            return lRetorno;
        }
    }
}
