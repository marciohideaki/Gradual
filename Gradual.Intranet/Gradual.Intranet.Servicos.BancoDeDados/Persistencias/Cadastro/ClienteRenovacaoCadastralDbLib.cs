using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        #region | Métodos CRUD

        public static ReceberObjetoResponse<ClienteRenovacaoCadastralInfo> ReceberClienteRenovacaoCadastral(ReceberEntidadeRequest<ClienteRenovacaoCadastralInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteRenovacaoCadastralInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Objeto = new ClienteRenovacaoCadastralInfo();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_RENOV_CADAS_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cpfcnpj", DbType.Int64, pParametro.Objeto.DsCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""));

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto.DtRenovacao = lDataTable.Rows[0]["DT_VALIDADE"].DBToDateTime();
                    lRetorno.Objeto.DsCpfCnpj = lDataTable.Rows[0]["CD_CPFCGC"].DBToString();
                    lRetorno.Objeto.DtNascimentoFundacao = lDataTable.Rows[0]["DT_NASC_FUND"].DBToDateTime();
                }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<ClienteRenovacaoCadastralInfo> ConsultarClienteRenovacaoCadastral(ConsultarEntidadeRequest<ClienteRenovacaoCadastralInfo> pParametros)
        {
            try
            {
                DateTime lDataPesquisaInicio = default(DateTime);
                DateTime lDataPesquisaFim = default(DateTime);
                DataTable lDataTableOracle = new DataTable();
                DataTable lDataTableSql = new DataTable();
                ConsultarObjetosResponse<ClienteRenovacaoCadastralInfo> lResposta = new ConsultarObjetosResponse<ClienteRenovacaoCadastralInfo>();
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                //--> Recuperando a informação de configuração para 'PeriodoDeEmissaoDeRelatorio'

                int lQuantidadeMesesProgressos = ReceberConfiguracaoPorDescricao(
                    new ReceberEntidadeRequest<ConfiguracaoInfo>()
                    {
                        Objeto = new ConfiguracaoInfo()
                        {
                            Configuracao = EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral
                        }
                    }).Objeto.Valor.DBToInt32();

                //--> O sinacor já define como 24 meses para a renovação
                if (!String.IsNullOrEmpty(pParametros.Objeto.DsDesejaAplicar))
                {
                    if (pParametros.Objeto.DsDesejaAplicar.Equals("CAM"))
                    {
                        lQuantidadeMesesProgressos = 12 - lQuantidadeMesesProgressos;
                    }
                }
                else
                {
                    lQuantidadeMesesProgressos = 24 - lQuantidadeMesesProgressos;
                }

                lDataPesquisaInicio = pParametros.Objeto.DtPesquisa.Value;
                lDataPesquisaFim = pParametros.Objeto.DtPesquisaFim.Value;

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_renov_cadas_sel"))
                {   //--> Pesquisando no Sinacor os dados dos clientes em período de renovação.
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_pesquisaInicio", DbType.Date, lDataPesquisaInicio);
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_pesquisaFim", DbType.Date, lDataPesquisaFim);
                    lAcessaDados.AddInParameter(lDbCommand, "pqt_meses_progressos", DbType.Int32, lQuantidadeMesesProgressos);

                    lDataTableOracle = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                }

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                //using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_cpfcnpj_sel_sp"))
                //{   //--> Fazendo um de/para dos clientes para montar o objeto de Cliente usando como índice o CPF.
                //    if (null != lDataTableOracle && lDataTableOracle.Rows.Count > 0)
                //        for (int i = 0; i < lDataTableOracle.Rows.Count; i++)
                //        {
                //            lAcessaDados.ClearParameters(lDbCommand);

                //            lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToString());

                //            lDataTableSql = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                //            if (null != lDataTableSql && lDataTableSql.Rows.Count > 0)
                //                lResposta.Resultado.Add(CriarRegistroClienteRenovacaoCadastral(lDataTableSql.Rows[0], lDataTableOracle.Rows[i]["dt_validade"].DBToDateTime()));
                //        }
                //}

                var htCpfData = new Hashtable();

                var lIndice = default(int);

                //Fazendo em uma unica chamada ao banco
                var cpfs = new StringBuilder();
                try
                {
                    if (null != lDataTableOracle && lDataTableOracle.Rows.Count > 0)
                        for (int i = 0; i < lDataTableOracle.Rows.Count; i++)
                        {
                            lIndice = i;
                            if (!htCpfData.ContainsKey(lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToInt64()))
                            {
                                if (lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToString().Length > 11)
                                {
                                    cpfs.AppendFormat("{0},", lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToString().PadLeft(14, '0'));
                                }
                                else
                                {
                                    cpfs.AppendFormat("{0},", lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToString().PadLeft(11, '0'));
                                }
                                
                                htCpfData.Add(lDataTableOracle.Rows[i]["cd_cpfcgc"].DBToInt64(), lDataTableOracle.Rows[i]["dt_validade"].DBToDateTime());
                            }
                        }
                }
                catch (Exception ex)
                {
                    var exee = ex.ToString();

                    if (string.IsNullOrEmpty(exee))
                    {
 
                    }
                }

                if (cpfs.Length > 0)
                {
                    using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_cpfcnpj_lst_sp"))
                    {
                        lAcessaDados.ClearParameters(lDbCommand);
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, cpfs.ToString().Trim(','));
                        lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.String, pParametros.Objeto.TipoPessoa);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.String, pParametros.Objeto.CdAssessor);

                        if (!String.IsNullOrEmpty(pParametros.Objeto.DsDesejaAplicar))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_desejaaplicar", DbType.String, pParametros.Objeto.DsDesejaAplicar);
                        }

                        lDataTableSql = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    }
                }

                var lCliente = new ClienteRenovacaoCadastralInfo();
                foreach (DataRow item in lDataTableSql.Rows)
                {
                    lCliente = new ClienteRenovacaoCadastralInfo();

                    lCliente.DsNome        = item["ds_nome"].DBToString();
                    lCliente.DsCpfCnpj     = item["ds_cpfcnpj"].DBToString();
                    lCliente.IdCliente     = item["id_cliente"].DBToInt32();
                    lCliente.TpPessoa      = item["tp_pessoa"].DBToChar();
                    lCliente.DsTelefone    = string.Format("{0} {1}", item["ds_telefone_ddd"].DBToString(), item["ds_telefone_numero"].DBToString());
                    lCliente.DtRenovacao   = htCpfData[Int64.Parse(lCliente.DsCpfCnpj)].DBToDateTime();
                    lCliente.CodigoBovespa = item["cd_codigo"].DBToString();
                    lCliente.CdAssessor    = item["cd_assessor"].DBToString();
                    lCliente.Email         = item["ds_email"].DBToString();

                    if (lCliente.DtRenovacao != DateTime.MinValue)
                        lResposta.Resultado.Add(lCliente);
                }


                //[SP_CLIENTE_RENOVACAO_LST_SP]
                if (pParametros.Objeto.DsDesejaAplicar == "CAMBIO")
                {
                    using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_CLIENTE_RENOVACAO_LST_SP"))
                    {
                        lAcessaDados.ClearParameters(lDbCommand);
                        lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.String, pParametros.Objeto.TipoPessoa);
                        lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.String, pParametros.Objeto.CdAssessor);
                        lAcessaDados.AddInParameter(lDbCommand, "@DtInicio", DbType.Date, lDataPesquisaInicio);
                        lAcessaDados.AddInParameter(lDbCommand, "@DtFim", DbType.Date, lDataPesquisaFim);

                        if (!String.IsNullOrEmpty(pParametros.Objeto.DsDesejaAplicar))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_desejaaplicar", DbType.String, pParametros.Objeto.DsDesejaAplicar);
                        }

                        lDataTableSql = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    }

                    foreach (DataRow item in lDataTableSql.Rows)
                    {
                        lCliente = new ClienteRenovacaoCadastralInfo();

                        lCliente.DsNome = item["ds_nome"].DBToString();
                        lCliente.DsCpfCnpj = item["ds_cpfcnpj"].DBToString();
                        lCliente.IdCliente = item["id_cliente"].DBToInt32();
                        lCliente.TpPessoa = item["tp_pessoa"].DBToChar();
                        lCliente.DsTelefone = string.Format("{0} {1}", item["ds_telefone_ddd"].DBToString(), item["ds_telefone_numero"].DBToString());
                        lCliente.DtRenovacao = item["dt_renovacao"].DBToDateTime(); 
                        lCliente.CodigoBovespa = item["cd_codigo"].DBToString();
                        lCliente.CdAssessor = item["cd_assessor"].DBToString();
                        lCliente.Email = item["ds_email"].DBToString();

                        if (lCliente.DtRenovacao != DateTime.MinValue)
                            lResposta.Resultado.Add(lCliente);
                    }
                }
                //--> Ordenando por data de atualização cadastral.
                lResposta.Resultado.Sort(delegate(ClienteRenovacaoCadastralInfo cr1, ClienteRenovacaoCadastralInfo cr2) { return Comparer<DateTime>.Default.Compare(cr1.DtRenovacao, cr2.DtRenovacao); });

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteRenovacaoCadastralInfo> AtualizarDataValidadeClienteRenovacaoCadastral(SalvarObjetoRequest<ClienteRenovacaoCadastralInfo> pParametros)
        {
            try
            {
                // Caro Programador, o CPF sempre deve ser informado para atualizar a data de validade do período cadastral,            
                if (string.IsNullOrEmpty(pParametros.Objeto.DsCpfCnpj)) // pois é por ele que o cliente pode ser encontrado no Sinacor.
                    throw new NullReferenceException("Parâmetro de CPF/CNPJ não informado, não foi possível completar a operação.");

                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                var lConfiguracao = ReceberConfiguracaoPorDescricao(new ReceberEntidadeRequest<ConfiguracaoInfo>() { Objeto = new ConfiguracaoInfo() { Configuracao = EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral } });

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_upd_dt_validade"))
                {   //--> O parâmetro de filtro para insersão do Sinacor é o CPF do cliente.

                    lAcessaDados.AddInParameter(lDbCommand, "pCpjCgc", DbType.Int64, pParametros.Objeto.DsCpfCnpj.Replace(".", "").Replace("/", ""));
                    lAcessaDados.AddInParameter(lDbCommand, "pDataAtualizada", DbType.DateTime, pParametros.Objeto.DtRenovacao.AddMonths(lConfiguracao.Objeto.Valor.DBToInt32()));

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                return new SalvarEntidadeResponse<ClienteRenovacaoCadastralInfo>() { Objeto = pParametros.Objeto };
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        #endregion

        #region | Métodos de apoio

        private static ClienteRenovacaoCadastralInfo CriarRegistroClienteRenovacaoCadastral(DataRow linha, DateTime dtValidade)
        {
            return new ClienteRenovacaoCadastralInfo()
            {
                StCadastroPortal = linha["st_cadastroportal"].DBToBoolean(),
                DsCargo = linha["ds_cargo"].DBToString(),
                StCarteiraPropria = linha["st_carteirapropria"].DBToBoolean(),
                DsAutorizadoOperar = linha["ds_autorizadooperar"].DBToString(),
                StCVM387 = linha["st_cvm387"].DBToBoolean(),
                DsConjugue = linha["ds_conjugue"].DBToString(),
                DsCpfCnpj = linha["ds_cpfcnpj"].DBToString(),
                DtNascimentoFundacao = linha["dt_nascimentofundacao"].DBToDateTime(),
                DtPasso1 = linha["dt_passo1"].DBToDateTime(),
                DtPasso2 = linha["dt_passo2"].DBToDateTime(),
                DtPasso3 = linha["dt_passo3"].DBToDateTime(),
                DtPrimeiraExportacao = linha["dt_primeiraexportacao"].DBToDateTime(),
                DtUltimaAtualizacao = linha["dt_ultimaatualizacao"].DBToDateTime(),
                DtUltimaExportacao = linha["dt_ultimaexportacao"].DBToDateTime(),
                StEmancipado = linha["st_emancipado"].DBToBoolean(),
                DsEmpresa = linha["ds_empresa"].DBToString(),
                CdEscolaridade = linha["cd_escolaridade"].DBToInt32(),
                CdEstadoCivil = linha["cd_estadocivil"].DBToInt32(),
                DsFormaConstituicao = linha["ds_formaconstituicao"].DBToString(),
                IdAssessorInicial = linha["id_assessorinicial"].DBToInt32(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                IdLogin = linha["id_login"].DBToInt32(),
                CdNacionalidade = linha["cd_nacionalidade"].DBToInt32(),
                CdNire = linha["cd_nire"].DBToInt32(),
                DsNome = linha["ds_nome"].DBToString(),
                DsNomeFantasia = linha["ds_nomefantasia"].DBToString(),
                CdOrgaoEmissorDocumento = linha["cd_orgaoemissordocumento"].DBToString(),
                DsOrigemCadastro = linha["ds_origemcadastro"].DBToString(),
                CdPaisNascimento = linha["cd_paisnascimento"].DBToString(),
                StPasso = linha["st_passo"].DBToInt32(),
                StPPE = linha["st_ppe"].DBToBoolean(),
                CdProfissaoAtividade = linha["cd_profissaoatividade"].DBToInt32(),
                CdAtividadePrincipal = linha["cd_atividadePrincipal"].DBToInt32(),
                CdSexo = linha["cd_sexo"].DBToChar(),
                TpCliente = linha["tp_cliente"].DBToInt32(),
                TpDocumento = linha["tp_documento"].DBToString(),
                TpPessoa = linha["tp_pessoa"].DBToChar(),
                CdUfEmissaoDocumento = linha["cd_ufemissaodocumento"].DBToString(),
                CdUfNascimento = linha["cd_ufnascimento"].DBToString(),
                DsUfnascimentoEstrangeiro = linha["ds_ufnascimentoestrangeuro"].DBToString(),
                StInterdito = linha["st_interdito"].DBToBoolean(),
                StSituacaoLegalOutros = linha["st_situacaolegaloutros"].DBToBoolean(),
                DsTelefone = string.Format("{0} {1}", linha["ds_telefone_ddd"].DBToString(), linha["ds_telefone_numero"].DBToString()),
                DtRenovacao = dtValidade,
                CodigoBovespa = linha["cd_codigo"].DBToString(),
                CdAssessor = linha["cd_assessor"].DBToString(),
            };
        }

        #endregion
    }
}
