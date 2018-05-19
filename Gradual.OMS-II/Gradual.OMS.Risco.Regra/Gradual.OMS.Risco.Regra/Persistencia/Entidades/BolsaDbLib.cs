using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using log4net;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Persistencia;
using System.Data;

namespace Gradual.OMS.Risco.Regra.Persistencia.Entidades
{
    public class BolsaDbLib : IEntidadeDbLib<BolsaBancoInfo>
    {

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        #region IEntidadeDbLib<BolsaBancoInfo> Members

        public OMS.Persistencia.ConsultarObjetosResponse<BolsaBancoInfo> ConsultarObjetos(OMS.Persistencia.ConsultarObjetosRequest<BolsaBancoInfo> lRequest)
        {
            ConsultarObjetosResponse<BolsaBancoInfo> lRetorno = new ConsultarObjetosResponse<BolsaBancoInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_bolsa_lst", new object[] { });
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

        public OMS.Persistencia.ReceberObjetoResponse<BolsaBancoInfo> ReceberObjeto(OMS.Persistencia.ReceberObjetoRequest<BolsaBancoInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        public OMS.Persistencia.RemoverObjetoResponse<BolsaBancoInfo> RemoverObjeto(OMS.Persistencia.RemoverObjetoRequest<BolsaBancoInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        public OMS.Persistencia.SalvarObjetoResponse<BolsaBancoInfo> SalvarObjeto(OMS.Persistencia.SalvarObjetoRequest<BolsaBancoInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        private BolsaBancoInfo MontarObjeto(DataRow dr)
        {
            BolsaBancoInfo lRetorno = new BolsaBancoInfo();
            lRetorno.CodigoBolsa = (int)dr["id_bolsa"];
            lRetorno.DescricaoBolsa = dr["ds_bolsa"].ToString();

            return lRetorno;
        }
        
    }
}
