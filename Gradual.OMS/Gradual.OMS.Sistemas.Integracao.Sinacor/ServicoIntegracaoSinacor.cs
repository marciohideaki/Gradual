using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Integracao.Sinacor
{
    /// <summary>
    /// Implementação do serviço de integração com o Sinacor
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoIntegracaoSinacor : IServicoIntegracaoSinacor
    {
        #region Variaveis Locais

        /// <summary>
        /// Referencia para as configurações do serviço
        /// </summary>
        private ServicoIntegracaoSinacorConfig _config = GerenciadorConfig.ReceberConfig<ServicoIntegracaoSinacorConfig>();

        #endregion

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoIntegracaoSinacor()
        {
        }

        #region IServicoIntegracaoSinacor Members

        /// <summary>
        /// Recebe informações do cliente
        /// </summary>
        /// <param name="codigoCBLC"></param>
        public ReceberClienteSinacorResponse ReceberClienteSinacor(ReceberClienteSinacorRequest parametros)
        {
            // Processa

            // Retorna
            return 
                new ReceberClienteSinacorResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Recebe informações de custódia de um cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberCustodiaSinacorResponse ReceberCustodiaSinacor(ReceberCustodiaSinacorRequest parametros)
        {
            // Prepara o retorno
            ReceberCustodiaSinacorResponse retorno = 
                new ReceberCustodiaSinacorResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem 
                };

            // Bloco de controle
            try
            {
                // Faz a consulta
                string sql = @"
                    select  cod_cart        
                    ,       tipo_grup       , cod_cli
                    ,       qtde_disp       
                    ,       qtde_aexe_cpa   , qtde_aexe_vda
                    ,       case tipo_merc
                                when 'FUT' then concat(cod_comm, cod_seri)
                                when 'OPD' then concat(cod_comm, cod_seri)
                                else cod_neg
                            end cod_neg
                    ,       (qtde_disp - qtde_blqd + qtde_alqd + 
                            qtde_da1 + qtde_da2 + qtde_da3 + qtde_exec_cpa + 
                            qtde_exec_vda) qtde_atual 
                    from    vcfposicao 
                    where   cod_cli = :codigo_cliente   ";
                DataSet ds = executarConsulta(_config.ConnectionString, sql, "codigo_cliente", parametros.CodigoClienteCBLC);

                // Transforma o resultado numa lista de CustodiaSinacorPosicaoInfo
                foreach (DataRow dr in ds.Tables[0].Rows)
                    retorno.Resultado.Add(
                        new CustodiaSinacorPosicaoInfo()
                        {
                            Carteira = dr["COD_CART"].ToString(),
                            CodigoAtivo = dr["COD_NEG"].ToString(),
                            CodigoBolsa = (string)dr["TIPO_GRUP"] == "BMF" ? "BMF" : "BOVESPA",
                            CodigoCliente = dr["COD_CLI"].ToString(),
                            QuantidadeAbertura = Convert.ToDouble(dr["QTDE_DISP"]),
                            QuantidadeAtual = Convert.ToDouble(dr["QTDE_ATUAL"]),
                            QuantidadeCompra = Convert.ToDouble(dr["QTDE_AEXE_CPA"]),
                            QuantidadeVenda = Convert.ToDouble(dr["QTDE_AEXE_VDA"])
                        });
            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloIntegracao);

                // Informa erro no retorno
                retorno.DescricaoResposta = ex.ToString();
                retorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Recebe informações de conta corrente do sinacor.
        /// Mais informações sobre a procedure utilizada estão na pasta Docs.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSaldoContaCorrenteSinacorResponse ReceberSaldoContaCorrenteSinacor(ReceberSaldoContaCorrenteSinacorRequest parametros)
        {
            // Prepara retorno
            ReceberSaldoContaCorrenteSinacorResponse resposta =
                new ReceberSaldoContaCorrenteSinacorResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Cria dicionário de parametros
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();
            paramsProc.Add("pcodcliente", parametros.CodigoClienteCBLC);
            paramsProc.Add("psaldo", 0);
            paramsProc.Add("pd1", 0);
            paramsProc.Add("pd2", 0);
            paramsProc.Add("pd3", 0);

            // Faz a chamada da procedure
            DataSet ds =
                executarProcedure(
                    _config.ConnectionString,
                    "pccsaldoprojetado", 
                    paramsProc, 
                    new List<string>() 
                    { 
                        "psaldo", "pd1", "pd2", "pd3"
                    });

            // Pega o resultado
            resposta.SaldoContaCorrenteSinacor = 
                new SaldoContaCorrenteSinacorInfo() 
                { 
                    CodigoClienteCBLC = parametros.CodigoClienteCBLC,
                    DataReferencia = DateTime.Now,
                    SaldoD0 = Convert.ToDouble(paramsProc["psaldo"]),
                    SaldoD1 = Convert.ToDouble(paramsProc["pd1"]),
                    SaldoD2 = Convert.ToDouble(paramsProc["pd2"]),
                    SaldoD3 = Convert.ToDouble(paramsProc["pd3"])
                };

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Recebe informações de saldo da conta margem do sinacor.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSaldoContaMargemSinacorResponse ReceberSaldoContaMargemSinacor(ReceberSaldoContaMargemSinacorRequest parametros)
        {
            // Prepara resposta
            ReceberSaldoContaMargemSinacorResponse resposta =
                new ReceberSaldoContaMargemSinacorResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Monta string sql
            StringBuilder sql = 
                new StringBuilder(@"
                    select  cd_cliente
                    ,       dt_limite
                    ,       vl_limite
                    ,       vl_deposito_cc
                    ,       vl_financiado
                    ,       vl_iof 
                    from    V_CONTA_MARGEM 
                    where   cd_cliente = :codigo_cliente ");

            // Se nao retorna histórico, coloca o filtro
            if (!parametros.RetornarHistorico)
                sql.Append(@" and dt_limite = 
                              (select   max(dt_limite) 
                              from      V_CONTA_MARGEM 
                              where cd_cliente = :codigo_cliente)  ");

            // Executa
            DataSet ds = executarConsulta(_config.ConnectionStringContaMargem, sql.ToString(), "codigo_cliente", parametros.CodigoClienteCBLC);

            // Monta lista
            foreach (DataRow dr in ds.Tables[0].Rows)
                resposta.SaldosContaMargemSinacor.Add(
                    new SaldoContaMargemSinacorInfo()
                    {
                        CodigoClienteCBLC = dr["CD_CLIENTE"].ToString(),
                        DataReferencia = (DateTime)dr["DT_LIMITE"],
                        ValorDepositoContaCorrente = Convert.ToDouble(dr["VL_LIMITE"]),
                        ValorFinanciado = Convert.ToDouble(dr["VL_DEPOSITO_CC"]),
                        ValorIOF = Convert.ToDouble(dr["VL_FINANCIADO"]),
                        ValorLimite = Convert.ToDouble(dr["VL_IOF"])
                    });

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Recebe lista de cblc´s de cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarCBLCsClienteSinacorResponse ListarCBLCsClienteSinacor(ListarCBLCsClienteSinacorRequest parametros)
        {
            // Prepara resposta
            ListarCBLCsClienteSinacorResponse resposta = 
                new ListarCBLCsClienteSinacorResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Faz a consulta
            DataTable tb = null;
            if (parametros.CodigoCBLC != null)
            {
                // Monta consulta por codigo cblc
                string sql = @"
                        select  cd_cliente 
                        ,       in_conta_inv 
                        from    TSCCLICC 
                        where   cd_cpfcgc = 
                                (   select  cd_cpfcgc 
                                    from    TSCCLICC 
                                    where   cd_cliente = :codigo_cliente )";
                // Executa
                tb = executarConsulta(_config.ConnectionString, sql, "codigo_cliente", parametros.CodigoCBLC).Tables[0];
            }
            else
            {
                // Monta consulta por cpf/cnpj
                string sql = @"
                        select  cd_cliente 
                        ,       in_conta_inv 
                        from    TSCCLICC 
                        where   cd_cpfcgc = :codigo_cpfcnpj ";

                // Executa
                tb = executarConsulta(_config.ConnectionString, sql, "codigo_cpfcnpj", parametros.CodigoCPFCNPJ).Tables[0];
            }

            // Monta o retorno
            foreach (DataRow dr in tb.Rows)
                resposta.ClientesCBLC.Add(
                    new ClienteCBLCInfo()
                    {
                        CodigoCBLC = dr["cd_cliente"].ToString(),
                        TipoConta = 
                            (string)dr["in_conta_inv"] == "S" ? 
                            ClienteCBLCTipoContaEnum.ContaInvestimento : 
                            ClienteCBLCTipoContaEnum.ContaCorrente
                    });

            // Retorna
            return resposta;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Executa uma procedure no banco de dados.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private DataSet executarProcedure(string connectionString, string nomeProcedure, Dictionary<string, object> parametros, List<string> outputParams)
        {
            // Recebe a conexao
            OracleConnection cn = receberConexao(connectionString);

            // Cria o comando
            OracleCommand cm = new OracleCommand(nomeProcedure, cn);
            cm.CommandType = CommandType.StoredProcedure;

            // Adiciona parametros, caso existam
            if (parametros != null)
                foreach (KeyValuePair<string, object> item in parametros)
                    cm.Parameters.AddWithValue(item.Key, item.Value);

            // Seta os parametros de output
            foreach (string outputParam in outputParams)
                if (cm.Parameters.Contains(outputParam))
                    cm.Parameters[outputParam].Direction = ParameterDirection.Output;

            // Executa
            OracleDataAdapter da = new OracleDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            // Preenche parametros de output
            foreach (OracleParameter param in cm.Parameters)
                if (param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output)
                    if (parametros.ContainsKey(param.ParameterName))
                        parametros[param.ParameterName] = param.Value;

            // Retorna
            return ds;
        }

        /// <summary>
        /// Executa uma consulta no banco de dados.
        /// Overload para receber os parametros como duplas de parametro e valor.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private DataSet executarConsulta(string connectionString, string sql, params object[] parametros)
        {
            // Transforma a lista de parametros em um dicionario
            Dictionary<string, object> parametros2 = new Dictionary<string, object>();
            for (int i = 0; i < parametros.Length; i += 2)
                parametros2.Add((string)parametros[i], parametros[i + 1]);

            // Repassa a chamada
            return executarConsulta(connectionString, sql, parametros2);
        }

        /// <summary>
        /// Executa uma consulta no sinacor
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private DataSet executarConsulta(string connectionString, string sql, Dictionary<string, object> parametros)
        {
            // Recebe a conexao
            OracleConnection cn = receberConexao(connectionString);

            // Cria o comando
            OracleCommand cm = new OracleCommand(sql, cn);
            cm.CommandType = CommandType.Text;

            // Adiciona parametros, caso existam
            if (parametros != null)
                foreach (KeyValuePair<string, object> item in parametros)
                    cm.Parameters.AddWithValue(item.Key, item.Value);

            // Executa
            OracleDataAdapter da = new OracleDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            // Retorna
            return ds;
        }

        /// <summary>
        /// Retorna uma conexão com o banco de dados do sinacor
        /// </summary>
        /// <returns></returns>
        private OracleConnection receberConexao(string connectionString)
        {
            // Cria a conexão
            OracleConnection cn = new OracleConnection(connectionString);
            
            // Abre
            cn.Open();

            // Retorna
            return cn;
        }

        #endregion
    }
}
