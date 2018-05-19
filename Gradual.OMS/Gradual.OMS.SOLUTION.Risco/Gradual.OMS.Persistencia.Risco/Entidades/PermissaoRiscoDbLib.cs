using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Risco.Dados;
using System.Data;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Persistencia.Risco.DB;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia.Risco.Entidades
{
    public class PermissaoRiscoDbLib : IEntidadeDbLib<PermissoesRiscoInfo>
    {

        private SqlDbLib _dbLib = new SqlDbLib("RISCO");

        #region IEntidadeDbLib<PermissoesRiscoInfo> Members

        public Contratos.Comum.Mensagens.ConsultarObjetosResponse<PermissoesRiscoInfo> ConsultarObjetos(Contratos.Comum.Mensagens.ConsultarObjetosRequest<PermissoesRiscoInfo> lRequest)
        {
            ConsultarObjetosResponse<PermissoesRiscoInfo> lRetorno = new ConsultarObjetosResponse<PermissoesRiscoInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_permissao_lst", "@id_bolsa", lRequest.Condicoes[0].Valores[0]);
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

        public Contratos.Comum.Mensagens.ReceberObjetoResponse<PermissoesRiscoInfo> ReceberObjeto(Contratos.Comum.Mensagens.ReceberObjetoRequest<PermissoesRiscoInfo> lRequest)
        {
            ReceberObjetoResponse<PermissoesRiscoInfo> lRetorno = new ReceberObjetoResponse<PermissoesRiscoInfo>();
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

        public Contratos.Comum.Mensagens.RemoverObjetoResponse<PermissoesRiscoInfo> RemoverObjeto(Contratos.Comum.Mensagens.RemoverObjetoRequest<PermissoesRiscoInfo> lRequest)
        {
            RemoverObjetoResponse<PermissoesRiscoInfo> lRetorno = new RemoverObjetoResponse<PermissoesRiscoInfo>();
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

        public Contratos.Comum.Mensagens.SalvarObjetoResponse<PermissoesRiscoInfo> SalvarObjeto(Contratos.Comum.Mensagens.SalvarObjetoRequest<PermissoesRiscoInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<PermissoesRiscoInfo> lResponse = new SalvarObjetoResponse<PermissoesRiscoInfo>();

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

        private PermissoesRiscoInfo MontarObjeto(DataRow dr)
        {
            PermissoesRiscoInfo lRetorno = new PermissoesRiscoInfo();

            lRetorno.Bolsa = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoPermissao = (int)dr["id_permissao"];
            lRetorno.Metodo = dr["nome_metodo"].ToString();
            lRetorno.NameSpace = dr["url_namespace"].ToString();
            lRetorno.NomePermissao = dr["dscr_permissao"].ToString();
            return lRetorno;
        }
    }
}
