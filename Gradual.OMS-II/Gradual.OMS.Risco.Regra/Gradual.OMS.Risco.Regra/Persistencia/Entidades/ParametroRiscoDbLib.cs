using System;
using System.Collections.Generic;
using System.Data;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using log4net;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class ParametroRiscoDbLib : IEntidadeDbLib<ParametroRiscoInfo>
    {

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IEntidadeDbLib<ParametroRiscoInfo> Members

        public ConsultarObjetosResponse<ParametroRiscoInfo> ConsultarObjetos(ConsultarObjetosRequest<ParametroRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<ParametroRiscoInfo> lRetorno = new ConsultarObjetosResponse<ParametroRiscoInfo>();
            try
            {
                var parametros = new Dictionary<string, object>();

                foreach (CondicaoInfo ci in lRequest.Condicoes)
                    parametros.Add(ci.Propriedade, ci.Valores[0]);

                DataSet ds = _dbLib.ExecutarConsulta("prc_parametro_risco_lst", parametros);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    lRetorno.Resultado.Add(MontarObjeto(ds.Tables[0].Rows[i]));

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public ReceberObjetoResponse<ParametroRiscoInfo> ReceberObjeto(ReceberObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            var lRetorno = new ReceberObjetoResponse<ParametroRiscoInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_sel", "@id_parametro", int.Parse(lRequest.CodigoObjeto));
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                    lRetorno.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public RemoverObjetoResponse<ParametroRiscoInfo> RemoverObjeto(RemoverObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            var lRetorno = new RemoverObjetoResponse<ParametroRiscoInfo>();

            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_del", "@id_parametro", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<ParametroRiscoInfo> SalvarObjeto(SalvarObjetoRequest<ParametroRiscoInfo> lRequest)
        {
            var paramsProc = new Dictionary<string, object>();
            var lResponse = new SalvarObjetoResponse<ParametroRiscoInfo>();

            paramsProc.Add("@id_parametro", lRequest.Objeto.CodigoParametro);
            paramsProc.Add("@ds_parametro", lRequest.Objeto.NomeParametro);
            paramsProc.Add("@id_bolsa", lRequest.Objeto.Bolsa);

            DataSet ds = _dbLib.ExecutarProcedure("prc_parametro_risco_salvar", paramsProc);

            if (ds.Tables[0].Rows.Count > 0)
                lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);

            return lResponse;
        }

        #endregion


        private ParametroRiscoInfo MontarObjeto(DataRow dr)
        {
            var lRetorno = new ParametroRiscoInfo();

            lRetorno.Bolsa = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoParametro = (int)dr["id_parametro"];
            lRetorno.NomeParametro = dr["dscr_parametro"].ToString();

            return lRetorno;
        }
    }
}
