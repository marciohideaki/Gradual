using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Gradual.OMS.CotacaoStreamer.Dados;
using log4net;

namespace Gradual.OMS.CotacaoStreamer
{
    public class DCotacoes
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, InstrumentoInfo> ObterListaInstrumentos()
        {
            SqlConnection conn = null;
            String procedure = "prc_lst_tbCadastroPapel";

            logger.Debug("Preparando Solicitacao " + procedure);

            Dictionary<string, InstrumentoInfo> listaInstrumento = new Dictionary<string, InstrumentoInfo>();
            SqlCommand command = new SqlCommand();
            try
            {
                conn = new SqlConnection(ConfigurationManager.AppSettings["MDS"]);
                conn.Open();

                command.Connection = conn;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = procedure;

                DataTable dtDados = new DataTable();
                DateTime dataRegistro = DateTime.Now;

                // Obter todos os instrumentos atualizados de até 5 dias atrás, e não apenas da data atual
                dataRegistro = dataRegistro.AddDays(-5);

                do
                {
                    command.Parameters.AddWithValue("@DataRegistro", dataRegistro);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDados);

                    if (dtDados.Rows.Count == 0)
                    {
                        command.Parameters.RemoveAt("@DataRegistro");
                        dataRegistro = dataRegistro.AddDays(-1);
                    }
                }
                while (dtDados.Rows.Count == 0);

                foreach (DataRow registro in dtDados.Rows)
                {
                    InstrumentoInfo info = new InstrumentoInfo();
                    info.Instrumento = registro["CodigoInstrumento"].ToString();
                    info.RazaoSocial = registro["RazaoSocial"].ToString();
                    info.GrupoCotacao = registro["GrupoCotacao"].ToString();
                    info.FormaCotacao = (registro["FormaCotacao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["FormaCotacao"].ToString()));
                    info.DataUltimoNegocio = (registro["DataUltimoNegocio"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataUltimoNegocio"].ToString()));
                    info.LotePadrao = (registro["LotePadrao"].ToString().Equals("") ? 0 : Convert.ToInt32(registro["LotePadrao"].ToString()));
                    info.IndicadorOpcao = registro["IndicadorOpcao"].ToString();
                    info.PrecoExercicio = (registro["PrecoExercicio"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["PrecoExercicio"].ToString()));
                    info.DataVencimento = (registro["DataVencimento"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataVencimento"].ToString()));
                    info.CodigoPapelObjeto = registro["CodigoPapelObjeto"].ToString();
                    info.SegmentoMercado = registro["SegmentoMercado"].ToString();
                    info.CoeficienteMultiplicacao = (registro["CoeficienteMultiplicacao"].ToString().Equals("") ? 0 : Convert.ToDouble(registro["CoeficienteMultiplicacao"].ToString()));
                    info.DataRegistro = (registro["DataRegistro"].ToString().Equals("") ? DateTime.MinValue : Convert.ToDateTime(registro["DataRegistro"].ToString()));
                    info.CodigoISIN = registro["CodigoISIN"].ToString();
                    info.NumeroNegocios = 0;
                    info.VolumeNegocios = 0;
                    info.QuantidadePapeis = 0;
                    info.Preco = 0;
                    info.Variacao = 0;
                    info.DictCompradorasMaisNegociadas = new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente());
                    info.DictCompradorasMaisNegociadasPorCorretora = new SortedDictionary<string, RankingInfo>();
                    info.DictVendedorasMaisNegociadas = new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente());
                    info.DictVendedorasMaisNegociadasPorCorretora = new SortedDictionary<string, RankingInfo>();
                    info.DictCompradorasMaioresVolumes = new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente());
                    info.DictCompradorasMaioresVolumesPorCorretora = new SortedDictionary<string, RankingInfo>();
                    info.DictVendedorasMaioresVolumes = new SortedDictionary<RankingInfo, string>(new ComparadorDecrescente());
                    info.DictVendedorasMaioresVolumesPorCorretora = new SortedDictionary<string, RankingInfo>();
                    info.DictMaioresVolumes = new SortedDictionary<CorretorasInfo, string>(new ComparadorDecrescenteCorretoras());
                    info.DictMaioresVolumesPorCorretora = new SortedDictionary<string, CorretorasInfo>();
                    info.DictMaioresVolumesPorCorretora.Add(MemoriaResumoCorretoras.TODAS_CORRETORAS, new CorretorasInfo(0, MemoriaResumoCorretoras.TODAS_CORRETORAS, 0, 0, 0));

                    listaInstrumento.Add(info.Instrumento, info);
                }
            }
            catch (Exception e)
            {
                logger.Error("Erro procedure[" + procedure + "] Mensagem: [" + e.Message + "]", e);
            }
            finally
            {
                conn.Close();
                conn.Dispose();

                command.Connection.Close();
                command.Dispose();
                command = null;
            }
            logger.Debug("Preparando Resposta " + procedure + " qtdItens[" + listaInstrumento.Count + "]");
            return listaInstrumento;
        }
    }
}
