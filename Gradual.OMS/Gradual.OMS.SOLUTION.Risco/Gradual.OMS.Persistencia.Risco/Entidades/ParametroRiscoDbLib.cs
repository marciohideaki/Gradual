using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Persistencia.Risco.DB;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia.Risco.Entidades
{
    public class ParametroRiscoDbLib : IEntidadeDbLib<ParametroRiscoInfo>
    {

        private SqlDbLib _dbLib = new SqlDbLib("RISCO");

        #region IEntidadeDbLib<ParametroRiscoInfo> Members

        public ConsultarObjetosResponse<ParametroRiscoInfo> ConsultarObjetos(ConsultarObjetosRequest<ParametroRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<ParametroRiscoInfo> lRetorno = new ConsultarObjetosResponse<ParametroRiscoInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_lst", new object[]{});
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
