using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        /// <summary>
        /// Relatório de clientes Suspeitos
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteSuspeitoInfo</param>
        /// <returns>Retorna uma lista de clientes suspeitos, atividades ilícitas, etc</returns>
        public static ConsultarObjetosResponse<ClientePendenciaCadastralRelInfo> ConsultarClientePendenciaCadastralRel(ConsultarEntidadeRequest<ClientePendenciaCadastralRelInfo> pParametros)
        {
            ConsultarObjetosResponse<ClientePendenciaCadastralRelInfo> lResposta =
                new ConsultarObjetosResponse<ClientePendenciaCadastralRelInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_com_pendencia_cadastral_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@IdTipoPendenciaCadastral", DbType.Int32, pParametros.Objeto.IdTipoPendenciaCadastral);
                lAcessaDados.AddInParameter(lDbCommand, "@StPendenciaResolvida", DbType.Byte, pParametros.Objeto.StResolvido);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.AnsiString, pParametros.Objeto.TipoPessoa);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroPendenciaCadastral(lDataTable.Rows[i]));

            }
            return lResposta;
        }

        /// <summary>
        /// Método de apoio para uso de preenchimento da entidade PendenciaCadastralInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de clientes com pendencia cadastral</param>
        /// <returns>Retorna uma entidade do tipo PendenciaCadastralInfo preenchida</returns>
        private static ClientePendenciaCadastralRelInfo CriarRegistroPendenciaCadastral(DataRow linha)
        {
            return new ClientePendenciaCadastralRelInfo()
            {
                CodigoAssessor = linha["cd_assessor"].DBToInt32(),
                DsCpfCnpj = linha["ds_cpfcnpj"].DBToString(),
                DsNomeCliente = linha["ds_nome"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                DtPendenciaCadastral = linha["dtpendencia"].DBToDateTime(),
                IdTipoPendenciaCadastral = linha["id_tipopendenciacadastral"].DBToInt32(),
                DsPendenciaCadastral = linha["ds_pendenciacadastral"].DBToString(),
                DsTipoPendenciaCadastral = linha["ds_tipopendencia"].DBToString(),
                TipoPessoa = linha["tp_pessoa"].DBToString(),
                CodigoBolsa = linha["cd_codigo"].DBToInt32(),
                DtResolucao = linha["dt_resolucao"].DBToDateTime()
            };
        }

        public static ConsultarObjetosResponse<ClienteSolicitacaoAlteracaoCadastralInfo> ConsultarClienteSolicitacaoAlteracaoCadastralRel(ConsultarEntidadeRequest<ClienteSolicitacaoAlteracaoCadastralInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteSolicitacaoAlteracaoCadastralInfo> lResposta =
                new ConsultarObjetosResponse<ClienteSolicitacaoAlteracaoCadastralInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_com_solicitacao_alteracao_cadastral_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte", DbType.DateTime, pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                lAcessaDados.AddInParameter(lDbCommand, "@StResolvida", DbType.Byte, pParametros.Objeto.StResolvido);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa", DbType.String, pParametros.Objeto.TipoPessoa);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroSolicitacaoAlteracaoCadastral(lDataTable.Rows[i]));
            }

            return lResposta;
        }
        
        private static ClienteSolicitacaoAlteracaoCadastralInfo CriarRegistroSolicitacaoAlteracaoCadastral(DataRow linha)
        {
            return new ClienteSolicitacaoAlteracaoCadastralInfo()
            {
                CodigoAssessor = linha["cd_assessor"].DBToInt32(),
                DsCpfCnpj = linha["ds_cpfcnpj"].DBToString(),
                DsNomeCliente = linha["ds_nome"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                DsInformacao = linha["Ds_Informacao"].DBToString(),
                DsTipo = linha["cd_tipo"].DBToString(),
                DtSolicitacao = linha["Dt_Solicitacao"].DBToDateTime(),
                TipoPessoa = linha["tp_pessoa"].DBToString(),
                CodigoBolsa = linha["cd_codigo"].DBToInt32(),
                DtResolucao = linha["dt_realizacao"].DBToDateTime()
            };
        }
        
        public static ConsultarObjetosResponse<ClienteMigradoCorretagemAnualInfo> ConsultarClienteMigradoCorretagemAnual(ConsultarEntidadeRequest<ClienteMigradoCorretagemAnualInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteMigradoCorretagemAnualInfo> lResposta =
                new ConsultarObjetosResponse<ClienteMigradoCorretagemAnualInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cli_migrado_corretagem"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteMigradoCorretagemAnualInfo(lDataTable.Rows[i]));
            }

            return lResposta;
        }

        private static ClienteMigradoCorretagemAnualInfo CriarRegistroClienteMigradoCorretagemAnualInfo(DataRow linha)
        {
            return new ClienteMigradoCorretagemAnualInfo()
            {
                NM_Cliente = linha["NM_Cliente"].DBToString(),
                NM_Assessor = linha["NM_Assessor"].DBToString(),
                DT_Criacao = linha["DT_Criacao"].DBToDateTime(),
                DT_Ult_Oper = linha["DT_Ult_Oper"].DBToDateTime(),
                Total = linha["Total"].DBToDecimal()
            };
        }
        
        
        public static ConsultarObjetosResponse<ClienteDistribuicaoRegionalInfo> ConsultarClienteDistribuicaoRegional(ConsultarEntidadeRequest<ClienteDistribuicaoRegionalInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteDistribuicaoRegionalInfo> lResposta =
                new ConsultarObjetosResponse<ClienteDistribuicaoRegionalInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cli_migrado_regiao"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "@DtDe", DbType.DateTime, pParametros.Objeto.DtDe);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteDistribuicaoRegionalInfo(lDataTable.Rows[i]));
            }

            return lResposta;
        }

        private static ClienteDistribuicaoRegionalInfo CriarRegistroClienteDistribuicaoRegionalInfo(DataRow linha)
        {
            return new ClienteDistribuicaoRegionalInfo()
            {
                CPF         = linha["CPF"].DBToString(),
                NM_Cliente  = linha["NM_Cliente"].DBToString(),
                NM_Assessor = linha["NM_Assessor"].DBToString(),
                SG_Estado   = linha["SG_Estado"].DBToString(),
                NM_Cidade   = linha["NM_Cidade"].DBToString(),
                DT_Criacao = linha["DT_Criacao"].DBToDateTime(),
                NmLogradouro = linha["NM_LOGRADOURO"].DBToString(),
                NrPredio = linha["NR_PREDIO"].DBToString(),
                DsCompEndereco = linha["NM_COMP_ENDE"].DBToString(),
                NmBairro = linha["NM_BAIRRO"].DBToString(),
                CEP = linha["CD_CEP"].DBToString(),
                CEPExt = linha["CD_CEP_EXT"].DBToString(),
                Telefone = linha["NR_TELEFONE"].DBToInt32(),
                TelefoneRamal = linha["NR_RAMAL"].DBToInt32(),
                Celular1 = linha["NR_CELULAR1"].DBToInt32(),
                Celular1DDD = linha["CD_DDD_CELULAR1"].DBToInt32(),
                Celular2 = linha["NR_CELULAR2"].DBToInt32(),
                Celular2DDD = linha["CD_DDD_CELULAR2"].DBToInt32(),
                Email = linha["NM_E_MAIL"].DBToString(),
            };
        }
    }
}
