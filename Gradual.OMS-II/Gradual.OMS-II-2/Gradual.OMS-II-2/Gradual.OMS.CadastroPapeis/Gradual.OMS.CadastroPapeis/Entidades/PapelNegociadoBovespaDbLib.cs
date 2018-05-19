using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Library;
using System.Runtime.CompilerServices;
using System.Data.Common;
using Gradual.OMS.CadastroPapeis.Entidades;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.Generico.Dados;
using log4net;
using Gradual.OMS.Persistencia;

namespace Gradual.OMS.CadastroPapeis.Entidades
{
    public class PapelNegociadoBovespaDbLib : IEntidadeDbLib<PapelNegociadoBovespaInfo>
    {
        #region Propriedades
        private const string _ConnectionStringName = "Trade";

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                logger.Error(ex.Message, ex);
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
                logger.Error(ex.Message, ex);
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

                lRetorno.CodISIN            = dr["CD_CODISI"].DBToString();
                lRetorno.CodNegociacao = dr["CD_CODNEG"].DBToString();
                lRetorno.NomeEmpresa = dr["NM_NOMPRE"].DBToString();
                lRetorno.TipoMercado        = dr["CD_TPMERC"].DBToInt32();
                lRetorno.DescMercado = dr["NM_NOMERC"].DBToString();
                lRetorno.Dismex             = dr["NR_DISMEX"].DBToInt32();
                lRetorno.CodSetorAtividade = dr["CD_CODSET"].DBToString();
                lRetorno.DescSetorAtividade = dr["NM_NOMSET"].DBToString();
                lRetorno.PrecoExercicio     = Convert.ToDouble(dr["VL_PREEXE"].DBToDouble()) / 100;
                lRetorno.DataVencimento     = dr["DT_DATVEN"].DBToDateTime();
                lRetorno.PrecoFechamento    = Convert.ToDouble(dr["VL_PREFEC"].DBToDouble()) / 100;
                lRetorno.DataFechamento     = dr["DT_DATFCH"].DBToDateTime();
                lRetorno.PrecoMedio         = Convert.ToDouble(dr["VL_PRMDAN"].DBToDouble()) / 100;
                lRetorno.FatorCotacao       = dr["NR_FATCOT"].DBToInt32();
                lRetorno.LoteNegociacao     = dr["NR_LOTNEG"].DBToInt32();

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
