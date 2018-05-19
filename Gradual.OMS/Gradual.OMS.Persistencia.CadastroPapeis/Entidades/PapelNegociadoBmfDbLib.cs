using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Data;
using Gradual.OMS.Library;
using Gradual.Generico.Dados;
using System.Data.Common;

namespace Gradual.OMS.Persistencia.CadastroPapeis.Entidades
{
    public class PapelNegociadoBmfDbLib 
    {
        #region Properties
        private const string _ConnectionStringName = "Trade";
        #endregion

        #region Métodos
        /// <summary>
        /// Retorna uma lista de todos os papeis negociados BMF
        /// </summary>
        /// <returns></returns>
        public List<PapelNegociadoBmfInfo> ListarPapelNegociadoBmf()
        {
            List<PapelNegociadoBmfInfo> lResposta = new List<PapelNegociadoBmfInfo>();

            try
            {
                AcessaDados acesso = new AcessaDados("Retorno");
                acesso.ConnectionStringName = _ConnectionStringName;

                using(DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_PAPEL_BMF_LST"))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private PapelNegociadoBmfInfo MontarObjeto(DataRow dr)
        {
            PapelNegociadoBmfInfo lRetorno = new PapelNegociadoBmfInfo();
           
            try
            {
                //lRetorno.TipoRegistro                           = Convert.ToInt32(dr["TIPOREG"]);
                //lRetorno.TipoNegociacao                         = dr["TIPONEG"].ToString();
                lRetorno.CodMercadoria = dr["CD_COMMOD"].ToString();
                //lRetorno.CodMercado                             = Convert.ToChar(dr["CODMER"]);
                //lRetorno.TipoSerie                              = Convert.ToChar(dr["TIPOSERIE"]);
                lRetorno.SerieVencimento = dr["CD_SERIE"].ToString();
                lRetorno.DataVencimento = (dr["DT_VENC"] == DBNull.Value) ? new Nullable<DateTime>() : Convert.ToDateTime(dr["DT_VENC"]);
                lRetorno.PrecoExercicio = (dr["PR_EXERC"] == DBNull.Value)? 0 : Convert.ToDouble(dr["PR_EXERC"]);
                //lRetorno.QuantidadeNegociadaDia                 = Convert.ToInt32(dr["QTDENEGDIA"]);
                //lRetorno.CotacaoMediaNegociadaDia               = Convert.ToInt32(dr["COTMEDNEGDIA"]);
                //lRetorno.CotacaoUltimoNegocioDia                = Convert.ToInt32(dr["COTULTNEGDIA"]);
                //lRetorno.HoraUltimoNegocioDia                   = Convert.ToDateTime(dr["HORAULTNEGDIA"]);
                //lRetorno.DataUltimaNegociacao                   = dr["DATAULTNEGDIA"] == DBNull.Value ?   new Nullable<DateTime>() :Convert.ToDateTime(dr["DATAULTNEGDIA"]);
                //lRetorno.SinalCotacaoFechamentoDia            = Convert.ToChar(dr["SINALCOTFECHDIA"]);
                //lRetorno.SinalCotacaoAjuste                   = Convert.ToChar(dr["SINALCOTAJUSTE"]);
                //lRetorno.CotacaoAjusteFuturo                    = Convert.ToDouble(dr["COTAJUSTEFUT"]);
                //lRetorno.PercentualOscilacao                    = Convert.ToDouble(dr["PERCENTOSCILACAO"]);
                //lRetorno.SinalOscilacao                       = Convert.ToChar(dr["SINALOSCILACAO"]);
                //lRetorno.QuantidadeDiasAteDataVencimento        = Convert.ToInt32(dr["QTDEDIASDATAVENC"]);
                //lRetorno.QuantidadeDiasUteisAteDataVencimento   = Convert.ToInt32(dr["QTDEDIASUTEISDTVENC"]);
                //lRetorno.VencimentoContratoObjeto               = dr["VENCCONTRATOOBJETO"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lRetorno;
        }
        #endregion
    }
}
