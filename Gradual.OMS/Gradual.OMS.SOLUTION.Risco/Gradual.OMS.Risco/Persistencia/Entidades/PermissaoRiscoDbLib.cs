using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.RegraLib.Dados;
using System.Data;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class PermissaoRiscoDbLib : IEntidadeDbLib<PermissaoRiscoInfo>
    {

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO");

        #region IEntidadeDbLib<PermissaoRiscoInfo> Members

        public ConsultarObjetosResponse<PermissaoRiscoInfo> ConsultarObjetos(ConsultarObjetosRequest<PermissaoRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<PermissaoRiscoInfo> lRetorno = new ConsultarObjetosResponse<PermissaoRiscoInfo>();
            try
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                foreach (CondicaoInfo ci in lRequest.Condicoes)
                {
                    parametros.Add(ci.Propriedade, ci.Valores[0]);
                }

                DataSet ds = _dbLib.ExecutarConsulta("prc_permissao_lst", parametros);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lRetorno.Resultado.Add(MontarObjeto(ds.Tables[0].Rows[i]));
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }

        }

        public ReceberObjetoResponse<PermissaoRiscoInfo> ReceberObjeto(ReceberObjetoRequest<PermissaoRiscoInfo> lRequest)
        {
            ReceberObjetoResponse<PermissaoRiscoInfo> lRetorno = new ReceberObjetoResponse<PermissaoRiscoInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_permissao_sel", "@id_permissao", int.Parse(lRequest.CodigoObjeto));
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
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public RemoverObjetoResponse<PermissaoRiscoInfo> RemoverObjeto(RemoverObjetoRequest<PermissaoRiscoInfo> lRequest)
        {
            RemoverObjetoResponse<PermissaoRiscoInfo> lRetorno = new RemoverObjetoResponse<PermissaoRiscoInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_permissao_del", "@id_permissao", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<PermissaoRiscoInfo> SalvarObjeto(SalvarObjetoRequest<PermissaoRiscoInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<PermissaoRiscoInfo> lResponse = new SalvarObjetoResponse<PermissaoRiscoInfo>();

            paramsProc.Add("@id_permissao", lRequest.Objeto.CodigoPermissao);
            paramsProc.Add("@dscr_permissao", lRequest.Objeto.NomePermissao);
            paramsProc.Add("@id_bolsa", lRequest.Objeto.Bolsa);
            paramsProc.Add("@url_namespace", lRequest.Objeto.NameSpace);
            paramsProc.Add("@nome_metodo", lRequest.Objeto.Metodo);

            DataSet ds = _dbLib.ExecutarProcedure("prc_permissao_salvar", paramsProc);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
            }

            return lResponse;
        }

        #endregion

        private PermissaoRiscoInfo MontarObjeto(DataRow dr)
        {
            PermissaoRiscoInfo lRetorno = new PermissaoRiscoInfo();

            lRetorno.Bolsa = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoPermissao = (int)dr["id_permissao"];
            lRetorno.Metodo = dr["nome_metodo"].ToString();
            lRetorno.NameSpace = dr["url_namespace"].ToString();
            lRetorno.NomePermissao = dr["dscr_permissao"].ToString();
            return lRetorno;
        }
    }
}
