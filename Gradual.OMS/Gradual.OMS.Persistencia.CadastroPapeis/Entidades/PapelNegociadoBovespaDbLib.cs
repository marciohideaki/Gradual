using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Data;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using System.Runtime.CompilerServices;
using System.Data.Common;
using Gradual.Generico.Dados;

namespace Gradual.OMS.Persistencia.CadastroPapeis.Entidades
{
    public class PapelNegociadoBovespaDbLib : IEntidadeDbLib<PapelNegociadoBovespaInfo>
    {
        #region Propriedades
        private const string _ConnectionStringName = "Trade";
        #endregion

        #region IEntidadeDbLib<PapelNegociadoDbLib> Members

        public ConsultarObjetosResponse<PapelNegociadoBovespaInfo> ConsultarObjetos(ConsultarObjetosRequest<PapelNegociadoBovespaInfo> lRequest)
        {
            ConsultarObjetosResponse<PapelNegociadoBovespaInfo> lRetorno = new ConsultarObjetosResponse<PapelNegociadoBovespaInfo>();
            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");

                acesso.ConnectionStringName = _ConnectionStringName;

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PAPEL_BOVESPA_LST"))
                {

                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    for (int i = 0; i < table.Rows.Count; i++)
                        lRetorno.Resultado.Add(MontarObjeto(table.Rows[i]));
                }
                
                return lRetorno;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, System.Reflection.Assembly.GetAssembly(typeof(PapelNegociadoBovespaInfo)).GetName());
                throw (ex);
            }
        }

        /// <summary>
        /// Retorna uma lista de todos os papeis negociados BMF
        /// </summary>
        /// <returns></returns>
        [MTAThread]
        public List<PapelNegociadoBovespaInfo> ListarPapelNegociadoBovespa()
        {
            List<PapelNegociadoBovespaInfo> lResposta = new List<PapelNegociadoBovespaInfo>();

            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");

                acesso.ConnectionStringName = _ConnectionStringName;

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PAPEL_BOVESPA_LST"))
                {

                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    for (int i = 0; i < table.Rows.Count; i++)
                        lResposta.Add(MontarObjeto(table.Rows[i]));
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, System.Reflection.Assembly.GetAssembly(typeof(PapelNegociadoBmfDbLib)).GetName());
                throw (ex);
            }
        }
        #endregion



        #region Métodos de apoio
        private PapelNegociadoBovespaInfo MontarObjeto(DataRow dr)
        {
            PapelNegociadoBovespaInfo lRetorno = new PapelNegociadoBovespaInfo();
            try
            {

                lRetorno.CodISIN = dr["CD_CODISI"].ToString();
                lRetorno.CodNegociacao = dr["CD_CODNEG"].ToString();
                lRetorno.NomeEmpresa = dr["NM_NOMPRE"].ToString();
                lRetorno.TipoMercado = Convert.ToInt32(dr["CD_TPMERC"]);
                lRetorno.DescMercado = dr["NM_NOMERC"].ToString();
                lRetorno.Dismex = Convert.ToInt32(dr["NR_DISMEX"]);
                lRetorno.CodSetorAtividade  = dr["CD_CODSET"].ToString();
                lRetorno.DescSetorAtividade = dr["NM_NOMSET"].ToString();
                lRetorno.PrecoExercicio = Convert.ToDouble(dr["VL_PREEXE"]);
                lRetorno.DataVencimento = Convert.ToDateTime(dr["DT_DATVEN"]);
                lRetorno.PrecoFechamento = Convert.ToDouble(dr["VL_PREFEC"]);
                lRetorno.DataFechamento = Convert.ToDateTime(dr["DT_DATFCH"]);
                lRetorno.PrecoMedio = Convert.ToDouble(dr["VL_PRMDAN"]);
                lRetorno.FatorCotacao = Convert.ToInt32(dr["NR_FATCOT"]);
                lRetorno.LoteNegociacao = Convert.ToInt32(dr["NR_LOTNEG"]);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lRetorno;
        }
        #endregion

        #region IEntidadeDbLib<PapelNegociadoBovespaInfo> Members


        public ReceberObjetoResponse<PapelNegociadoBovespaInfo> ReceberObjeto(ReceberObjetoRequest<PapelNegociadoBovespaInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverObjetoResponse<PapelNegociadoBovespaInfo> RemoverObjeto(RemoverObjetoRequest<PapelNegociadoBovespaInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarObjetoResponse<PapelNegociadoBovespaInfo> SalvarObjeto(SalvarObjetoRequest<PapelNegociadoBovespaInfo> lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
