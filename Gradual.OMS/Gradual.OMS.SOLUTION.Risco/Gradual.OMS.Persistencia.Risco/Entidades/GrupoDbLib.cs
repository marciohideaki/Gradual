using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Persistencia.Risco.DB;
using System.Data;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia.Risco.Entidades
{
    public class GrupoDbLib : IEntidadeDbLib<GrupoInfo>
    {

        private SqlDbLib _dbLib = new SqlDbLib("RISCO");

        #region IEntidadeDbLib<GrupoInfo> Members

        public ConsultarObjetosResponse<GrupoInfo> ConsultarObjetos(ConsultarObjetosRequest<GrupoInfo> lRequest)
        {
            ConsultarObjetosResponse<GrupoInfo> lRetorno = new ConsultarObjetosResponse<GrupoInfo>();
            try
            {

                DataSet ds = _dbLib.ExecutarProcedure("prc_grupo_lst", new object[] { });
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

        public ReceberObjetoResponse<GrupoInfo> ReceberObjeto(ReceberObjetoRequest<GrupoInfo> lRequest)
        {
            ReceberObjetoResponse<GrupoInfo> lRetorno = new ReceberObjetoResponse<GrupoInfo>();
            DataSet ds = _dbLib.ExecutarProcedure("prc_grupo_sel", "@id_grupo", int.Parse(lRequest.CodigoObjeto));
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

        public RemoverObjetoResponse<GrupoInfo> RemoverObjeto(RemoverObjetoRequest<GrupoInfo> lRequest)
        {
            RemoverObjetoResponse<GrupoInfo> lRetorno = new RemoverObjetoResponse<GrupoInfo>();
            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_grupo_del", "@id_grupo", int.Parse(lRequest.CodigoObjeto));
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }

        }

        public SalvarObjetoResponse<GrupoInfo> SalvarObjeto(SalvarObjetoRequest<GrupoInfo> lRequest)
        {
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            SalvarObjetoResponse<GrupoInfo> lResponse = new SalvarObjetoResponse<GrupoInfo>();
            paramsProc.Add("@id_grupo", lRequest.Objeto.CodigoGrupo);
            paramsProc.Add("@dscr_grupo", lRequest.Objeto.NomeDoGrupo);

            try
            {
                DataSet ds = _dbLib.ExecutarProcedure("prc_grupo_salvar", paramsProc);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lResponse.Objeto = MontarObjeto(ds.Tables[0].Rows[0]);
                }

                SalvarItensGrupo(lRequest.Objeto
                    , ReceberObjeto(new ReceberObjetoRequest<GrupoInfo>()
                    {
                        CodigoObjeto = lRequest.Objeto.CodigoGrupo.ToString()
                    }).Objeto);

                return lResponse;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, lRequest);
                throw (ex);
            }
        }

        #endregion

        private void SalvarItensGrupo(GrupoInfo pGrupoOriginal, GrupoInfo pGrupoSalvo)
        {
            // Inicializa
            GrupoItemDbLib lGrupoItemDbLib = new GrupoItemDbLib(pGrupoOriginal);

            // Pega lista de grupos atuais
            List<GrupoItemInfo> gruposAtuais = pGrupoSalvo.GrupoItens;
                //usuarioGrupoDbLib.ConsultarObjetos(usuarioSalvo.CodigoUsuario);

            // Varre a lista de que foi pedido para salvar
            foreach (GrupoItemInfo lGrupoItem in pGrupoOriginal.GrupoItens)
                if (gruposAtuais.Find(p => p.CodigoGrupoItem == lGrupoItem.CodigoGrupoItem) == null)
                {
                    lGrupoItemDbLib.SalvarObjeto(new SalvarObjetoRequest<GrupoItemInfo>() { Objeto = lGrupoItem });
                }

            // Verifica se existem grupos a remover
            foreach (GrupoItemInfo lGrupoItem in gruposAtuais)
                if (pGrupoOriginal.GrupoItens.Find(g => g.CodigoGrupoItem == lGrupoItem.CodigoGrupoItem) == null)
                {
                    lGrupoItemDbLib.RemoverObjeto(new RemoverObjetoRequest<GrupoItemInfo>() { CodigoObjeto = lGrupoItem.CodigoGrupoItem.ToString() });
                }

            // Atribui a coleção ao cliente salvo
            pGrupoSalvo.GrupoItens = pGrupoOriginal.GrupoItens;
        }

        private GrupoInfo MontarObjeto(DataRow dr)
        {
            GrupoInfo lRetorno = new GrupoInfo();
            lRetorno.CodigoGrupo = (int)dr["id_grupo"];
            lRetorno.NomeDoGrupo = dr["dscr_grupo"].ToString();

            GrupoItemDbLib lGrupoItem = new GrupoItemDbLib(lRetorno);

            lRetorno.GrupoItens = lGrupoItem.ConsultarObjetos(new ConsultarObjetosRequest<GrupoItemInfo>()).Resultado;

            return lRetorno;
        }
    }
}

