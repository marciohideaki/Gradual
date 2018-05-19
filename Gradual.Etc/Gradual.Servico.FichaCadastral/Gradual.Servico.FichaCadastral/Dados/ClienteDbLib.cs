using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using log4net;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteDbLib : DbLibBase
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReceberObjetoResponse<ClienteInfo> ReceberCliente(ReceberEntidadeRequest<ClienteInfo> pParametros)
        {
            try
            {
                var resposta = new ReceberObjetoResponse<ClienteInfo>();
                var lAcessaDados = new AcessaDados();

                resposta.Objeto = new ClienteInfo();
                gLogger.Info("Cria Conexao");
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_sel_sp"))
                {
                    gLogger.Info("Seta Parametro");
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    gLogger.Info("Executa");
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        resposta.Objeto = CriarRegistroCliente(lDataTable.Rows[0]);
                }

                if (resposta.Objeto.StCarteiraPropria != null && !resposta.Objeto.StCarteiraPropria.Value)
                {
                    var lCarteiraPropria = new ClienteNaoOperaPorContaPropriaDbLib().ConsultarClienteNaoOperaPorContaPropria(new ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>() { Objeto = new ClienteNaoOperaPorContaPropriaInfo() { IdCliente = pParametros.Objeto.IdCliente.Value } });
                    resposta.Objeto.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado = lCarteiraPropria.Objeto.DsCpfCnpjClienteRepresentado;
                    resposta.Objeto.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado = lCarteiraPropria.Objeto.DsNomeClienteRepresentado;
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                gLogger.Error("ReceberCliente", ex);
                throw ex;
            }
        }

        private static ClienteInfo CriarRegistroCliente(DataRow linha)
        {
            ClienteInfo lInfo = new ClienteInfo()
            {
                StCadastroPortal            = linha["st_cadastroportal"].DBToBoolean(),
                DsCargo                     = linha["ds_cargo"].DBToString(),
                StCarteiraPropria           = linha["st_carteirapropria"].DBToBoolean(),
                DsAutorizadoOperar          = linha["ds_autorizadooperar"].DBToString(),
                StCVM387                    = linha["st_cvm387"].DBToBoolean(),
                DsConjugue                  = linha["ds_conjugue"].DBToString(),
                DsCpfCnpj                   = linha["ds_cpfcnpj"].ToString(),
                DtNascimentoFundacao        = linha["dt_nascimentofundacao"].DBToDateTime(),
                DtPasso1                    = linha["dt_passo1"].DBToDateTime(),
                DtPasso2                    = linha["dt_passo2"].DBToDateTime(),
                DtPasso3                    = linha["dt_passo3"].DBToDateTime(),
                DtPrimeiraExportacao        = linha["dt_primeiraexportacao"].DBToDateTime(),
                DtUltimaAtualizacao         = linha["dt_ultimaatualizacao"].DBToDateTime(),
                DtUltimaExportacao          = linha["dt_ultimaexportacao"].DBToDateTime(),
                StEmancipado                = linha["st_emancipado"].DBToBoolean(),
                DsEmpresa                   = linha["ds_empresa"].DBToString(),
                CdEscolaridade              = linha["cd_escolaridade"].DBToInt32(),
                CdEstadoCivil               = linha["cd_estadocivil"].DBToInt32(),
                DsFormaConstituicao         = linha["ds_formaconstituicao"].DBToString(),
                IdAssessorInicial           = linha["id_assessorinicial"].DBToInt32(),
                IdCliente                   = linha["id_cliente"].DBToInt32(),
                IdLogin                     = linha["id_login"].DBToInt32(),
                CdNacionalidade             = linha["cd_nacionalidade"].DBToInt32(),
                CdNire                      = linha["cd_nire"].DBToInt64(),
                DsNome                      = linha["ds_nome"].DBToString(),
                DsNomeFantasia              = linha["ds_nomefantasia"].DBToString(),
                CdOrgaoEmissorDocumento     = linha["cd_orgaoemissordocumento"].DBToString(),
                DsOrigemCadastro            = linha["ds_origemcadastro"].DBToString(),
                CdPaisNascimento            = linha["cd_paisnascimento"].DBToString(),
                StPasso                     = linha["st_passo"].DBToInt32(),
                StPPE                       = linha["st_ppe"].DBToBoolean(),
                CdProfissaoAtividade        = linha["cd_profissaoatividade"].DBToInt32(),
                CdAtividadePrincipal        = linha["cd_atividadePrincipal"].DBToInt32(),
                CdSexo                      = linha["cd_sexo"].DBToChar(),
                TpCliente                   = linha["tp_cliente"].DBToInt32(),
                TpDocumento                 = linha["tp_documento"].DBToString(),
                TpPessoa                    = linha["tp_pessoa"].DBToChar(),
                CdUfEmissaoDocumento        = linha["cd_ufemissaodocumento"].DBToString(),
                CdUfNascimento              = linha["cd_ufnascimento"].DBToString(),
                DsUfnascimentoEstrangeiro   = linha["ds_ufnascimentoestrangeuro"].DBToString(),
                StInterdito                 = linha["st_interdito"].DBToBoolean(),
                StSituacaoLegalOutros       = linha["st_situacaolegaloutros"].DBToBoolean(),
                DsNomePai                   = linha["Ds_NomePai"].DBToString(),
                DsNomeMae                   = linha["Ds_NomeMae"].DBToString(),
                DtEmissaoDocumento          = linha["Dt_EmissaoDocumento"].DBToDateTime(),
                DsNaturalidade              = linha["Ds_Naturalidade"].DBToString(),
                NrInscricaoEstadual         = linha["Nr_InscricaoEstadual"].DBToString(),
                DsNumeroDocumento           = linha["ds_numerodocumento"].DBToString(),
                DsEmailComercial            = linha["ds_emailcomercial"].DBToString(),
                DsEmail                     = linha["ds_email"].DBToString(),
                StPessoaVinculada           = Convert.ToInt32(linha["st_pessoavinculada"]),
                DsUSPersonPJDetalhes        = linha["ds_usperson_pj_detalhes"].DBToString(),
                TpDesejaAplicar             = linha["tp_deseja_aplicar"].DBToString(),
                DsComoConheceu              = linha["ds_comoconheceu"].DBToString(),
                DadosClienteNaoOperaPorContaPropria = new ClienteNaoOperaPorContaPropriaInfo()
                
            };

            if(linha["st_ciente_documentos"] != DBNull.Value)
            {
                lInfo.StCienteDocumentos = Convert.ToInt16(linha["st_ciente_documentos"]);
            }

            if(linha["ds_proposito_gradual"] != DBNull.Value)
            {
                lInfo.DsPropositoGradual = linha["ds_proposito_gradual"].DBToString();
            }

            if(linha["st_usperson"] != DBNull.Value)
            {
                lInfo.StUSPerson = linha["st_usperson"].DBToBoolean();
            }

            return lInfo;
        }
    }
}
