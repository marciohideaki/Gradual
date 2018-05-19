using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Gradual.SerieHistorica.Dados
{
    class SerieHistoricaDados
    {
        private SqlConnection conexao = null;
        private SqlTransaction transacao = null;
        private SqlCommand comando = null;
        private String nomeTransacao = null;
        public String bancoDados = null;

        public SerieHistoricaDados()
        {
        }

        public bool AbrirConexao(String connectionString)
        {
            try
            {
                conexao = new SqlConnection(connectionString);
                conexao.Open();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool FecharConexao()
        {
            try
            {
                conexao.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool IniciarTransacao(String nomeTransacao)
        {
            try
            {
                this.nomeTransacao = nomeTransacao;
                transacao = conexao.BeginTransaction(nomeTransacao);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool FinalizarTransacao()
        {
            try
            {
                if ( transacao != null )
                    transacao.Commit();
                if (comando != null)
                    comando.Connection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool CancelarTransacao()
        {
            try
            {
                if (transacao != null)
                    transacao.Rollback(nomeTransacao);
                if ( comando != null)
                    comando.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public void InserirHistorico(List<String> colunas, String registro)
        {
            List<String> campos = new List<string>(registro.Split(';'));

            comando = new SqlCommand();
            comando.Connection = conexao;
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            if ( transacao != null )
                comando.Transaction = transacao;
            comando.CommandText = "prc_ins_tbHistorico";

            String ds_ativo = null;
            String ds_bolsa = null;
            double vl_abertura = 0;
            double vl_maximo = 0;
            double vl_minimo = 0;
            double vl_medio = 0;
            double vl_fechamento = 0;
            double vl_oscilacao = 0;
            double vl_oferta_compra = 0;
            double vl_oferta_venda = 0;
            long vl_total_negocios = 0;
            double vl_quantidade = 0;
            double vl_volume = 0;
            DateTime dt_cotacao = DateTime.MinValue;

            try
            {
                switch (campos.Count)
                {
                    // 15 colunas - Layout CotPapel
                    case 15:
                        ds_ativo = campos[colunas.IndexOf("Codigo")].Trim();
                        ds_bolsa = DESC_BOVESPA;
                        vl_abertura = double.Parse(campos[colunas.IndexOf("Preabe")]);
                        vl_maximo = double.Parse(campos[colunas.IndexOf("Premax")]);
                        vl_minimo = double.Parse(campos[colunas.IndexOf("Premin")]);
                        vl_medio = double.Parse(campos[colunas.IndexOf("Premed")]);
                        vl_fechamento = double.Parse(campos[colunas.IndexOf("Preult")]);
                        vl_oscilacao = double.Parse(campos[colunas.IndexOf("SinOSC")].
                            Replace('?', ' ').Replace('.', ' ').Replace('/', ' ').Replace('=', ' ').Trim() +
                            campos[colunas.IndexOf("Oscila")]);
                        vl_oferta_compra = double.Parse(campos[colunas.IndexOf("Preocf")]);
                        vl_oferta_venda = double.Parse(campos[colunas.IndexOf("Preofv")]);
                        vl_total_negocios = long.Parse(campos[colunas.IndexOf("Totneg")]);
                        vl_quantidade = double.Parse(campos[colunas.IndexOf("Quatot")]);
                        vl_volume = double.Parse(campos[colunas.IndexOf("Voltot")]);
                        dt_cotacao = DateTime.Parse(campos[colunas.IndexOf("data")]);
                        break;

                    // 34 colunas - Layout CotIndice
                    case 34:
                        ds_ativo = campos[colunas.IndexOf("Codigo")].Trim();
                        ds_bolsa = DESC_BOVESPA;
                        vl_abertura = double.Parse(campos[colunas.IndexOf("idcabe")]);
                        vl_maximo = double.Parse(campos[colunas.IndexOf("idcmax")]);
                        vl_minimo = double.Parse(campos[colunas.IndexOf("idcmin")]);
                        vl_medio = double.Parse(campos[colunas.IndexOf("idcmed")]);
                        vl_fechamento = double.Parse(campos[colunas.IndexOf("idcfec")]);
                        vl_oscilacao = double.Parse(campos[colunas.IndexOf("sinevo")].Replace('=', ' ').Trim() +
                            campos[colunas.IndexOf("evoind")]);
                        vl_oferta_compra = 0;
                        vl_oferta_venda = 0;
                        vl_total_negocios = long.Parse(campos[colunas.IndexOf("nngind")]);
                        vl_quantidade = double.Parse(campos[colunas.IndexOf("qtdind")]);
                        vl_volume = double.Parse(campos[colunas.IndexOf("volind")]);
                        dt_cotacao = DateTime.Parse(campos[colunas.IndexOf("data")]);
                        break;

                    // 54 colunas - Layout CotBMF
                    case 54:
                        ds_ativo = campos[colunas.IndexOf("Codigo")].Trim();
                        ds_bolsa = DESC_BMF;
                        vl_abertura = double.Parse(campos[colunas.IndexOf("CotAbe")]);
                        vl_maximo = double.Parse(campos[colunas.IndexOf("CotMax")]);
                        vl_minimo = double.Parse(campos[colunas.IndexOf("CotMin")]);
                        vl_medio = double.Parse(campos[colunas.IndexOf("CotMed")]);
                        vl_fechamento = double.Parse(campos[colunas.IndexOf("CotUlt")]);
                        vl_oscilacao = double.Parse(campos[colunas.IndexOf("SinOsc")].
                            Replace('?', ' ').Replace('.', ' ').Replace('/', ' ').Replace('=', ' ').Trim() +
                            campos[colunas.IndexOf("Oscila")]);
                        vl_oferta_compra = double.Parse(campos[colunas.IndexOf("CotOfc")]);;
                        vl_oferta_venda = double.Parse(campos[colunas.IndexOf("CotOfv")]);;
                        vl_total_negocios = long.Parse(campos[colunas.IndexOf("QtdNeg")]);
                        vl_quantidade = double.Parse(campos[colunas.IndexOf("QtdCon")]);
                        vl_volume = double.Parse(campos[colunas.IndexOf("VolExeR")]);
                        dt_cotacao = DateTime.Parse(campos[colunas.IndexOf("Datapregao")]);
                        break;

                    default:
                        throw new Exception("Quantidade de campos inválido: " + campos.Count);
                }

                comando.Parameters.AddWithValue("@ds_ativo", ds_ativo);
                comando.Parameters.AddWithValue("@ds_bolsa", ds_bolsa);
                comando.Parameters.AddWithValue("@vl_abertura", vl_abertura);
                comando.Parameters.AddWithValue("@vl_maximo", vl_maximo);
                comando.Parameters.AddWithValue("@vl_minimo", vl_minimo);
                comando.Parameters.AddWithValue("@vl_medio", vl_medio);
                comando.Parameters.AddWithValue("@vl_fechamento", vl_fechamento);
                comando.Parameters.AddWithValue("@vl_oscilacao", vl_oscilacao);
                comando.Parameters.AddWithValue("@vl_oferta_compra", vl_oferta_compra);
                comando.Parameters.AddWithValue("@vl_oferta_venda", vl_oferta_venda);
                comando.Parameters.AddWithValue("@vl_total_negocios", vl_total_negocios);
                comando.Parameters.AddWithValue("@vl_quantidade", vl_quantidade);
                comando.Parameters.AddWithValue("@vl_volume", vl_volume);
                comando.Parameters.AddWithValue("@dt_cotacao", dt_cotacao);

                comando.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                comando.Dispose();
            }
        }

        public void AlterarHistorico(List<String> colunas, String registro)
        {
            List<String> campos = new List<string>(registro.Split(';'));

            comando = new SqlCommand();
            comando.Connection = conexao;
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            if (transacao != null)
                comando.Transaction = transacao;
            comando.CommandText = "prc_upd_tbHistorico";

            String ds_ativo = null;
            double vl_ajuste = 0;
            double vl_quantidade = 0;
            DateTime dt_cotacao = DateTime.MinValue;

            try
            {
                switch (campos.Count)
                {
                    // 54 colunas - Layout CotBMF
                    case 54:
                        ds_ativo = campos[colunas.IndexOf("Codigo")].Trim();
                        dt_cotacao = DateTime.Parse(campos[colunas.IndexOf("Datapregao")]);
                        vl_ajuste = double.Parse(campos[colunas.IndexOf("CotAju")]);
                        vl_quantidade = double.Parse(campos[colunas.IndexOf("QtdCon")]);
                        break;

                    default:
                        throw new Exception("Quantidade de campos inválido: " + campos.Count);
                }

                comando.Parameters.AddWithValue("@ds_ativo", ds_ativo);
                comando.Parameters.AddWithValue("@dt_cotacao", dt_cotacao);
                comando.Parameters.AddWithValue("@vl_ajuste", vl_ajuste);
                comando.Parameters.AddWithValue("@vl_volume", vl_quantidade);

                comando.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                comando.Dispose();
            }
        }

        private const String DESC_BOVESPA = "BOV";
        private const String DESC_BMF = "BMF";
    }
}
