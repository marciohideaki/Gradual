using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Risco.RegraLib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class ParametroRiscoDbLib : IEntidadeDbLib<ParametroRiscoInfo>
    {

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO");

        #region IEntidadeDbLib<ParametroRiscoInfo> Members

        public ConsultarObjetosResponse<ParametroRiscoInfo> ConsultarObjetos(ConsultarObjetosRequest<ParametroRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<ParametroRiscoInfo> lRetorno = new ConsultarObjetosResponse<ParametroRiscoInfo>();
            try
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();

                foreach (CondicaoInfo ci in lRequest.Condicoes)
                {
                    parametros.Add(ci.Propriedade, ci.Valores[0]);
                }
                DataSet ds = _dbLib.ExecutarConsulta("prc_parametro_risco_lst", parametros);

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

        public ReceberObjetoResponse<ParametroRiscoInfo> ReceberObjeto(ReceberObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            ReceberObjetoResponse<ParametroRiscoInfo> lRetorno = new ReceberObjetoResponse<ParametroRiscoInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_sel", "@id_parametro", int.Parse(lRequest.CodigoObjeto));
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

        public RemoverObjetoResponse<ParametroRiscoInfo> RemoverObjeto(RemoverObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            RemoverObjetoResponse<ParametroRiscoInfo> lRetorno = new RemoverObjetoResponse<ParametroRiscoInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_del", "@id_parametro", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<ParametroRiscoInfo> SalvarObjeto(SalvarObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<ParametroRiscoInfo> lResponse = new SalvarObjetoResponse<ParametroRiscoInfo>();

            paramsProc.Add("@id_parametro", lRequest.Objeto.CodigoParametro);
            paramsProc.Add("@dscr_parametro", lRequest.Objeto.NomeParametro);
            paramsProc.Add("@id_bolsa", lRequest.Objeto.Bolsa);
            paramsProc.Add("@url_namespace", lRequest.Objeto.NameSpace);
            paramsProc.Add("@nome_metodo", lRequest.Objeto.Metodo);

            DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_salvar", paramsProc);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
            }

            return lResponse;
        }

        #endregion


        private ParametroRiscoInfo MontarObjeto(DataRow dr)
        {
            ParametroRiscoInfo lRetorno = new ParametroRiscoInfo();

            lRetorno.Bolsa = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoParametro = (int)dr["id_parametro"];
            lRetorno.NomeParametro = dr["dscr_parametro"].ToString();
            lRetorno.NameSpace = dr["url_namespace"].ToString();
            lRetorno.Metodo = dr["nome_metodo"].ToString();

            return lRetorno;
        }
    }
}
