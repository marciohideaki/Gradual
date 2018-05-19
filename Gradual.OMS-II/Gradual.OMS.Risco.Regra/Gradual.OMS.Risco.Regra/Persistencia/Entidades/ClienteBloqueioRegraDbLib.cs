using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Persistencia;
using System.Data;
using log4net;

namespace Gradual.OMS.Risco.Persistencia.Entidades
{
    public class ClienteBloqueioRegraDbLib : IEntidadeDbLib<ClienteBloqueioRegraInfo>
    {
        private ClienteBloqueioRegraInfo gRegraGrupo;

        private RegrasDbLib _dbLib = new RegrasDbLib("RISCO_GRADUALOMS");

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ClienteBloqueioRegraDbLib() { }

        #region IEntidadeDbLib<ClienteBloqueioRegraInfo> Members

        public ConsultarObjetosResponse<ClienteBloqueioRegraInfo> ConsultarObjetos(ConsultarObjetosRequest<ClienteBloqueioRegraInfo> lRequest)
        {
            ConsultarObjetosResponse<ClienteBloqueioRegraInfo> lRetorno = new ConsultarObjetosResponse<ClienteBloqueioRegraInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_bloqueio_lst");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ClienteBloqueioRegraInfo lClienteBloqueioRegraInfo = MontarObjetoClienteBloqueio(dr);
                    lRetorno.Resultado.Add(lClienteBloqueioRegraInfo);
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw (ex);
            }
        }

        public ReceberObjetoResponse<ClienteBloqueioRegraInfo> ReceberObjeto(ReceberObjetoRequest<ClienteBloqueioRegraInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverObjetoResponse<ClienteBloqueioRegraInfo> RemoverObjeto(RemoverObjetoRequest<ClienteBloqueioRegraInfo> lRequest)
        {
            RemoverObjetoResponse<ClienteBloqueioRegraInfo> lRetorno = new RemoverObjetoResponse<ClienteBloqueioRegraInfo>();

            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_bloqueio_del", "@id_cliente", lRequest.CodigoObjeto);
                
                return lRetorno;
            }
            catch (Exception ex)
            {
                logger.Error(lRequest, ex);
                throw (ex);
            }
        }

        public SalvarObjetoResponse<ClienteBloqueioRegraInfo> SalvarObjeto(SalvarObjetoRequest<ClienteBloqueioRegraInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<ClienteBloqueioRegraInfo> lResponse = new SalvarObjetoResponse<ClienteBloqueioRegraInfo>();

            paramsProc.Add("@id_cliente", lRequest.Objeto.CodigoCliente);
            paramsProc.Add("@cd_ativo"  , lRequest.Objeto.Ativo);
            paramsProc.Add("@ds_direcao", lRequest.Objeto.Direcao);

            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_cliente_bloqueio_ins", paramsProc);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lResponse.Objeto = MontarObjetoClienteBloqueio(ds.Tables[0].Rows[0]);
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

        #region MontarObjetoClienteBloqueio
        private ClienteBloqueioRegraInfo MontarObjetoClienteBloqueio(DataRow dr)
        {
            ClienteBloqueioRegraInfo lRetorno = new ClienteBloqueioRegraInfo();

            lRetorno.Ativo         = dr["cd_ativo"].ToString();
            lRetorno.CodigoCliente = (int)dr["id_cliente"];
            lRetorno.Direcao       = dr["Direcao"].ToString();

            return lRetorno;
        }
        #endregion
    }
}
