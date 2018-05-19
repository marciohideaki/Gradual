using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Dados;
using log4net;
using Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO.Info;
using Gradual.OMS.InvXX.Fundos.Lib.UNIBANCO;
using System.Globalization;
using Gradual.OMS.InvXX.Fundos.Lib.FINANCIAL;
using Gradual.OMS.InvXX.Fundos.Lib.ITAUUNIBANCO;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ITAUUNIBANCO
{
    public class ImportacaoItauDbLib
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        #endregion

        #region Métodos
        public ClienteFinancialInfo SelecionaNovoCotistaFinancial(int CodigoBovespa)
        {
            ClienteFinancialInfo lRetorno = new ClienteFinancialInfo();

            lRetorno.Endereco    = "";
            lRetorno.Numero      = "";
            lRetorno.Complemento = "";
            lRetorno.Bairro      = "";
            lRetorno.Cidade      = "";
            lRetorno.CEP         = "";
            lRetorno.UF          = "";
            lRetorno.Pais = "";
            lRetorno.EnderecoCom    = "";
            lRetorno.NumeroCom      = "";
            lRetorno.ComplementoCom = "";
            lRetorno.BairroCom      = "";
            lRetorno.CidadeCom      = "";
            lRetorno.CEPCom         = "";
            lRetorno.UFCom          = "";
            lRetorno.PaisCom        = "";
            lRetorno.EmailCom       = "";
            lRetorno.FoneCom = "";

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_NOVO_COTISTA_FINANCIAL_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoBovespa", DbType.Int32, CodigoBovespa);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow row = dt.Rows[i];

                                lRetorno.NomeCliente     = row["ds_nome"].ToString();
                                lRetorno.CodigoCliente   = CodigoBovespa;
                                lRetorno.DsCpfCnpj       = row["ds_cpfcnpj"].ToString();
                                lRetorno.Email           = row["ds_email"].ToString();
                                lRetorno.TipoPessoa      = row["tp_pessoa"].ToString();
                                lRetorno.TipoCliente     = row["tp_cliente"].ToString();
                                lRetorno.TipoCotistaCvm = int.Parse(row["tp_cliente"].ToString());
                                lRetorno.Fone            = string.Concat( row["ds_ddd"].ToString(), row["ds_numero"].ToString()) ;

                                if (row["id_tipo_endereco"].ToString().Equals("2"))
                                {
                                    lRetorno.StResidentePais = row["cd_pais"].ToString() == "BRA" ? 1 : 2;

                                    lRetorno.Endereco    = row["ds_logradouro"].ToString();
                                    lRetorno.Numero      = row["ds_numero_end"].ToString();
                                    lRetorno.Complemento = row["ds_complemento"].ToString();
                                    lRetorno.Bairro      = row["ds_bairro"].ToString();
                                    lRetorno.Cidade      = row["ds_cidade"].ToString();
                                    lRetorno.CEP         = row["ds_cep"].ToString();
                                    lRetorno.UF          = row["cd_uf"].ToString();
                                    lRetorno.Pais        = row["cd_pais"].ToString();
                                }
                                else if (row["id_tipo_endereco"].ToString().Equals("1"))
                                {
                                    lRetorno.EnderecoCom    = row["ds_logradouro"].ToString();
                                    lRetorno.NumeroCom      = row["ds_numero_end"].ToString();
                                    lRetorno.ComplementoCom = row["ds_complemento"].ToString();
                                    lRetorno.BairroCom      = row["ds_bairro"].ToString();
                                    lRetorno.CidadeCom      = row["ds_cidade"].ToString();
                                    lRetorno.CEPCom         = row["ds_cep"].ToString();
                                    lRetorno.UFCom          = row["cd_uf"].ToString();
                                    lRetorno.PaisCom        = row["cd_pais"].ToString();
                                    lRetorno.EmailCom       = "";
                                    lRetorno.FoneCom        = "";
                                }

                                lRetorno.EstadoCivil = int.Parse(row["cd_estadocivil"].ToString());
                                lRetorno.NumeroRG       = row["ds_numerodocumento"].ToString();
                                lRetorno.EmissorRG      = row["cd_orgaoemissordocumento"].ToString();
                                lRetorno.DataEmissaoRG  = Convert.ToDateTime(row["dt_emissaodocumento"].ToString(), gCultura);
                                lRetorno.Sexo           = row["cd_sexo"].ToString();
                                lRetorno.DataNascimento = Convert.ToDateTime(row["dt_nascimentofundacao"].ToString(), gCultura);
                                lRetorno.Profissao      = row["cd_profissaoatividade"].ToString();
                                lRetorno.PaisNascimento = row["cd_paisnascimento"].ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaNovoCotistaFinancial = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public ClienteCotistaItauInfo SelecionaNovoCotistaItau(int CodigoBovespa)
        {
            ClienteCotistaItauInfo lRetorno = new ClienteCotistaItauInfo();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_NOVO_COTISTA_ITAU_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoBovespa", DbType.Int32, CodigoBovespa);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow row = dt.Rows[i];

                                lRetorno.NomeCliente                         = row["ds_nome"].ToString();
                                lRetorno.DataNascimento                      = Convert.ToDateTime(row["dt_nascimentofundacao"].ToString());
                                lRetorno.CodigoTipoPessoa                    = row["tp_pessoa"].ToString();
                                lRetorno.DsCpfCnpj                           = row["ds_cpfcnpj"].ToString();
                                lRetorno.TipoDocumento                       = row["tp_documento"].ToString();
                                lRetorno.NumeroDocumento                     = row["ds_numerodocumento"].ToString();
                                lRetorno.DataExpedicaoDocumento              = Convert.ToDateTime(row["dt_emissaodocumento"].ToString());
                                lRetorno.OrgaoEmissorDocumento               = row["cd_orgaoemissordocumento"].ToString();
                                lRetorno.EstadoEmissorDocumento              = row["cd_ufemissaodocumento"].ToString();
                                lRetorno.CodigoTributacao                    = "";
                                lRetorno.CodigoSituacaoLegal                 = "";
                                lRetorno.CodigoSexo                          = row["cd_sexo"].ToString();
                                lRetorno.CodigoEstadoCivil                   = row["cd_estadocivil"].ToString();
                                lRetorno.CodigoAtividadePessoaJuridica       = row["cd_profissaoatividade"].ToString();
                                lRetorno.CodigoAtividadePessoaFisica         = row["cd_profissaoatividade"].ToString();
                                lRetorno.CodigoFormaConstituicaoEmpresa      = "";
                                lRetorno.TipoEnderecoCorrespondencia         = "R";
                                lRetorno.CodigoTipRemessa                    = "C";
                                lRetorno.DDD                                 = row["ds_ddd"].ToString();
                                lRetorno.NumeroTelResidencial                = row["ds_numero"].ToString();
                                lRetorno.NUmeroRamal                         = row["ds_ramal"].ToString();
                                lRetorno.Email                               = row["ds_email"].ToString();
                                lRetorno.Assessor                            = row["cd_assessor"].ToString();
                                lRetorno.CodigoBovespa                       = CodigoBovespa.ToString();
                                lRetorno.EmissaoExtratoMensal                = 'S';
                                lRetorno.EmissaoAvisoConfirmacaoMovimentacao = 'N';
                                lRetorno.ValorRendaMensal                    = row["vl_totalsalarioprolabore"].ToString();
                                lRetorno.ValorPatrimonial                    = row["vl_patrimonioliquido"].ToString();
                                lRetorno.CodigoTipoCliente                   = row["tp_cliente"].ToString();
                                lRetorno.CodigoCetip                         = "";
                                lRetorno.CodigoDistribuidor                  = "";
                                lRetorno.RazaoSocialAdministrador            = "";
                                lRetorno.RazaoSocialGestor                   = "";
                                lRetorno.RazaoSocialCustodiante              = "";
                                lRetorno.NomePrimeiroContatoCustodiante      = "";
                                lRetorno.DDDPrimeiroContatoCustodiante       = "";
                                lRetorno.TelefonePrimeiroContatoCustodiante  = "";
                                lRetorno.RamalPrimeiroContatoCustodiante     = "";
                                lRetorno.EmailPrimeiroContatoCustodiante     = "";
                                lRetorno.NomeSegundoContatoCustodiante       = "";
                                lRetorno.DDDSegundoContatoCustodiante        = "";
                                lRetorno.TelefoneSegundoContatoCustodiante   = "";
                                lRetorno.RamalSegundoContatoCustodiante      = "";
                                lRetorno.EmailSegundoContatoCustodiante      = "";

                                if (row["id_tipo_endereco"].ToString().Equals("2"))
                                {
                                    lRetorno.EnderecoResidencial    = row["ds_logradouro"].ToString();
                                    lRetorno.NumeroResidencial      = row["ds_numero_end"].ToString();
                                    lRetorno.ComplementoResidencial = row["ds_complemento"].ToString();
                                    lRetorno.BairroResidencial      = row["ds_bairro"].ToString();
                                    lRetorno.CepResidencial         = string.Concat(int.Parse(row["ds_cep"].ToString()).ToString("00000"), int.Parse(row["ds_cep_ext"].ToString()).ToString("000"));
                                    lRetorno.CidadeResidencial      = row["ds_cidade"].ToString();
                                    lRetorno.EstadoResidencial      = row["cd_uf"].ToString();
                                }
                                else
                                    if (row["id_tipo_endereco"].ToString().Equals("1"))
                                    {
                                        lRetorno.EnderecoComercial    = row["ds_logradouro"].ToString();
                                        lRetorno.NumeroComercial      = row["ds_numero_end"].ToString();
                                        lRetorno.ComplementoComercial = row["ds_complemento"].ToString();
                                        lRetorno.BairroComercial      = row["ds_bairro"].ToString();
                                        lRetorno.CepComercial         = string.Concat(int.Parse(row["ds_cep"].ToString()).ToString("00000"), int.Parse(row["ds_cep_ext"].ToString()).ToString("000"));
                                        lRetorno.CidadeComercial      = row["ds_cidade"].ToString();
                                        lRetorno.EstadoComercial      = row["cd_uf"].ToString();
                                    }
                                    else
                                    {
                                        lRetorno.EnderecoAlternativo    = row["ds_logradouro"].ToString();
                                        lRetorno.NumeroAlternativo      = row["ds_numero_end"].ToString();
                                        lRetorno.ComplementoAlternativo = row["ds_complemento"].ToString();
                                        lRetorno.BairroAlternativo      = row["ds_bairro"].ToString();
                                        lRetorno.CepAlternativo         = string.Concat(int.Parse(row["ds_cep"].ToString()).ToString("00000"), int.Parse(row["ds_cep_ext"].ToString()).ToString("000"));
                                        lRetorno.CidadeAlternativo      = row["ds_cidade"].ToString();
                                        lRetorno.EstadoAlternativo      = row["cd_uf"].ToString();
                                    }

                                lRetorno.IdentificadorArquivo = "000000";
                                lRetorno.PessoaVinculada      = row["st_pessoavinculada"].ToString();
                                lRetorno.Emancipado           = row["st_emancipado"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaCotistaItau = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public AplicacaoResgateInfo SelecionaCodigoClienteFundo(string pCodigoBanco, string pCodigoAgencia, string pCodigoConta)
        {
            AplicacaoResgateInfo lRetorno = new AplicacaoResgateInfo();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_FUNDO_COD_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@codigoBanco", DbType.String, pCodigoBanco);
                        lAcessaDados.AddInParameter(lCommand, "@codigoAgencia", DbType.String, pCodigoAgencia);
                        lAcessaDados.AddInParameter(lCommand, "@codigoConta", DbType.String, pCodigoConta);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row                 = dt.Rows[0];
                            lRetorno.CodigoCliente      = int.Parse( row["CodCliente"].ToString());
                            lRetorno.CodigoInternoFundo = int.Parse(row["idProduto"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaCodigoClienteFundo = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<AplicacaoResgateInfo> SelecionaAplicacaoResgateParaEnvio(int Codigo)
        {
            var lRetorno = new List<AplicacaoResgateInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RESG_APLIC_SERVICO_CLIENTE_SEL"))
                    {
                        //lAcessaDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, Codigo);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                var info = new AplicacaoResgateInfo();

                                info.NomeCliente        = row["ds_nome"].ToString();
                                info.NomeFundo          = row["dsNomeProduto"].ToString();
                                info.EhResgateTotal     = row["stResgateTotal"].DBToBoolean();
                                info.DtAgendamento      = row["dtAgendamento"].DBToDateTime();
                                info.TipoMovimento      = row["idTipoOperacao"].ToString() == "1" ? "030" : "105";
                                info.CodigoInternoFundo = int.Parse(row["idproduto"].ToString());
                                info.DsDiasPagResgate   = row["DsDiasPagResgate"].ToString();
                                
                                if (row["CodigoFundoItau"] != null)
                                {
                                    info.CodigoFundoItau = row["CodigoFundoItau"].DBToString();
                                    info.EhFundoItau = true;
                                }
                                else
                                {
                                    info.EhFundoItau = false;
                                }
                                info.CodigoCarteira       = int.Parse(row["codAnbima"].ToString());
                                info.CodigoCliente        = int.Parse(row["idcliente"].ToString());
                                info.VlrLiquidoSolicitado = decimal.Parse(row["vlsolicitado"].ToString());
                                info.DataLancamento       = row["dtAgendamento"].DBToDateTime().ToString("ddMMyyyy");
                                info.DataInclusao         = row["dtInclusao"].DBToDateTime().ToString("ddMMyyyy");
                                info.DtInclusao           = row["dtInclusao"].DBToDateTime();

                                //Preenchimento para uso da financial
                                info.QtdeCotasMovimento           = "0";
                                info.VlrCotacaoMovimento          = 0.0M;
                                info.VlrBrutoMovimento            = Convert.ToDecimal(row["vlsolicitado"].ToString());
                                info.VlrLiquidoCalculadoMovimento = 0.0M;
                                info.VlrIRMovimento               = 0.0M;
                                info.VlrIOFMovimento              = 0.0M;
                                info.VlrTaxaPerfomance            = 0.0M;
                                info.VlrTaxaResgateAntecipado     = 0.0M;
                                info.DataProcessamento            = Convert.ToDateTime(row["dtInclusao"]).ToString("dd/MM/yyyy");
                                
                                lRetorno.Add( info);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaCodigoFundo = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public void AtualizaAplicacaoResgateEmProcessamento(AplicacaoResgateInfo pInfo)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_EMPROCESSAMENTO_APLICACAO_RESGATE_UPD"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao",    DbType.Int32,       pInfo.TipoMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@idCliente",         DbType.Int32,       pInfo.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@idProduto",         DbType.Int32,       pInfo.CodigoInternoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@vlSolicitado",      DbType.Decimal,     pInfo.VlrLiquidoSolicitado);
                        lAcessaDados.AddInParameter(lCommand, "@dtInclusao",        DbType.DateTime,    pInfo.DtInclusao);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado",        DbType.String,      pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@idStatusMovimento", DbType.Int32,       pInfo.StatusMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@dsMotivoStatus",    DbType.String,      pInfo.DsMotivoStatus);
                        lAcessaDados.AddInParameter(lCommand, "@idOperacaoCotista", DbType.Int32, pInfo.IdOperacaoFinancial);
                        
                        /*
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado", DbType.String, pInfo.StAprovado);
                        */

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaAplicacaoResgateMovimento", ex);
            }
        }

        public void AtualizaAplicacaoResgateMovimento(AplicacaoResgateInfo pInfo)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MOVIMENTO_APLICACAO_RESGATE_UPD"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao"   , DbType.Int32,      pInfo.TipoMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@idCliente"        , DbType.Int32,      pInfo.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@idProduto"        , DbType.Int32,      pInfo.CodigoInternoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@vlSolicitado"     , DbType.Decimal,    pInfo.VlrBrutoMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@dtInclusao"       , DbType.DateTime,   pInfo.DtInclusao);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado"       , DbType.String,     pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@idStatusMovimento", DbType.Int32,      pInfo.StatusMovimento);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaAplicacaoResgateMovimento", ex);
            }
        }

        public void AtualizaCotistaItau(ClienteCotistaItauInfo pInfo)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_COTISTA_ITAU_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idCotistaItau"  , DbType.String, pInfo.CodigoCotista);
                        lAcessaDados.AddInParameter(lCommand, "@NomeCliente"    , DbType.String, pInfo.NomeCliente);
                        lAcessaDados.AddInParameter(lCommand, "@StAtivo"        , DbType.String, pInfo.StAtivo);
                        lAcessaDados.AddInParameter(lCommand, "@DsCpfCnpj"      , DbType.String, pInfo.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lCommand, "@Banco"          , DbType.String, pInfo.Banco);
                        lAcessaDados.AddInParameter(lCommand, "@Agencia"        , DbType.String, pInfo.Agencia);
                        lAcessaDados.AddInParameter(lCommand, "@Conta"          , DbType.String, pInfo.Conta);
                        lAcessaDados.AddInParameter(lCommand, "@DigitoConta"    , DbType.String, pInfo.DigitoConta);
                        lAcessaDados.AddInParameter(lCommand, "@SubConta"       , DbType.String, pInfo.SubConta);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaClienteCotista", ex);
            }
        }

        public void AtualizaClienteCotista(ClienteFinancialInfo pInfo)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_COTISTA_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCliente",     DbType.Int32,  pInfo.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@NomeCliente",       DbType.String, pInfo.NomeCliente);
                        lAcessaDados.AddInParameter(lCommand, "@CodigoAssessor",    DbType.Int32,  pInfo.CodigoAssessor);
                        lAcessaDados.AddInParameter(lCommand, "@StAtivo",           DbType.String, pInfo.StAtivo);
                        lAcessaDados.AddInParameter(lCommand, "@Telefone",          DbType.String, pInfo.Telefone);
                        lAcessaDados.AddInParameter(lCommand, "@Email",             DbType.String, pInfo.Email);
                        lAcessaDados.AddInParameter(lCommand, "@DsCpfCnpj",         DbType.String, pInfo.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lCommand, "@TipoPessoa",        DbType.String, pInfo.TipoPessoa);
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCotistaItau", DbType.String, pInfo.CodigoCotistaItau);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaClienteCotista", ex);
            }
        }

        public string SelecionaContaCreditoResgate(int Codigo)
        {
            string lRetorno = string.Empty;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    string lSql  = "select (banco.cd_banco +''+ banco.ds_conta +''+ banco.ds_agencia +''+ banco.ds_conta_digito) as ds_dados from tb_cliente_conta conta, tb_cliente_banco banco where banco.st_principal = 1 and  banco.id_cliente = conta.id_cliente and conta.cd_codigo =" + Codigo;

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql ))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = row["ds_dados"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaContaCreditoResgate", ex);
            }

            return lRetorno;
        }

        public List<AplicacaoResgateInfo> SelecionaAplicacaoResgateParaEnvio()
        {
            List<AplicacaoResgateInfo> lRetorno = new List<AplicacaoResgateInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "PRC_RESGATE_APLICACAO_PROGRAMADA_EFETUAR_SEL"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                AplicacaoResgateInfo info = new AplicacaoResgateInfo();

                                string CodigoCotistaItau = string.Empty;

                                if (row["CodigoCotistaItau"] != null)
                                {
                                    CodigoCotistaItau = row["CodigoCotistaItau"].ToString();
                                }

                                info.CodigoCarteira               = int.Parse(row["codAnbima"].ToString());
                                info.CodigoAgencia                = CodigoCotistaItau.Substring(0, 4);
                                info.CodigoConta                  = CodigoCotistaItau.Substring(4, 7).PadLeft(9,'0');
                                info.DigitoConferencia            = CodigoCotistaItau.Substring(11, 1);
                                info.CodigoCliente                = int.Parse( row["idcliente"].ToString());
                                info.CodigoSubConta               = " ";
                                info.VlrLiquidoSolicitado         = decimal.Parse(row["vlsolicitado"].ToString(), gCultura);
                                info.TipoMovimento                = row["idTipoOperacao"].ToString() == "1" ? "030" : "105";
                                info.ContaCredito                 = SelecionaContaCreditoResgate( int.Parse(row["idcliente"].ToString()));
                                info.CodigoInternoFundo           = int.Parse(row["idproduto"].ToString());
                                info.DataLancamento               = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DataInclusao                 = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DtInclusao                   = Convert.ToDateTime(row["dtInclusao"]);
                                info.CodigoFundoItau              = row["CodigoFundoItau"].ToString();
                                info.CodigoInternoFundo           = int.Parse(row["idproduto"].ToString());

                                //Preenchimento para uso da financial
                                info.QtdeCotasMovimento           = "0";
                                info.VlrCotacaoMovimento          = 0.0M;
                                info.VlrBrutoMovimento            = Convert.ToDecimal(row["vlsolicitado"].ToString(), gCultura);
                                info.VlrLiquidoCalculadoMovimento = 0.0M;
                                info.VlrIRMovimento               = 0.0M;
                                info.VlrIOFMovimento              = 0.0M;
                                info.VlrTaxaPerfomance            = 0.0M;
                                info.VlrTaxaResgateAntecipado     = 0.0M;
                                info.DataProcessamento            = Convert.ToDateTime(row["dtInclusao"]).ToString("dd/MM/yyyy");
                                lRetorno.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaCodigoFundo = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public string SelecionaTipoLiquidacaoMovimento (int CodigoCliente, decimal lValorResgateSolicitado)
        {
            string lRetorno = string.Empty;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_conta_itau_sel"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCliente", DbType.Int32, CodigoCliente);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            lRetorno = TabelaLiquidacaoInfo.RESGATECREDITOEMCONTACORRENTEITAU;
                        }
                        else
                        {
                            lRetorno = TabelaLiquidacaoInfo.RESGATEDOCCOMPE;
                            
                            if (lValorResgateSolicitado > 1000)
                            {
                                lRetorno = TabelaLiquidacaoInfo.RESGATETEDSTR;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaTipoLiquidacaoMovimento ", ex);
            }

            return lRetorno;
        }

        public string SelecionaCodigoFundo(string Dscpfcnpj)
        {
            string lRetorno = "0";

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto from tbProduto where cpfcnpj = '" + Dscpfcnpj.Substring(1,14) + "'"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = row["idProduto"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoFundo ", ex);
            }

            return lRetorno;
        }

        public int SelecionaCodigoProduto(int idproduto)
        {
            int lRetorno = 0;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "ClubesFundos";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select dscnpj from tbCadastroFundo where idFundo = "+idproduto))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            gLogger.InfoFormat("Buscando código do fundo com o CPF/CNPJ [{0}]", row["dscnpj"].ToString());

                            lRetorno = Convert.ToInt32(SelecionaCodigoFundo(row["dscnpj"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoProduto", ex);
            }

            return lRetorno;
        }

        public int SelecionaCodigoCliente(string Dscpfcnpj)
        {
            int lRetorno = 0;
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    gLogger.InfoFormat("Buscando codigo de bolsa do cliente com cpf ou cnpj [{0}]", Dscpfcnpj);

                    string lDocumento = Dscpfcnpj.TrimStart(new Char[] { '0' });

                    string lSql = "select a.cd_codigo from tb_cliente_conta a, tb_cliente b where a.st_principal = 1 and a.cd_sistema = 'BOL' and a.id_cliente = b.id_cliente and b.ds_cpfcnpj = '" + lDocumento + "'";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql ))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno    = Convert.ToInt32(row["cd_codigo"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoCliente", ex);
            }
            return lRetorno;
        }

        public void DeletaImportaPosicaoClienteFundosITAU()
        {
            try
            {
                gLogger.Info("Deletando as posições de fundos para ser inseridas futuramente");

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "delete from tbClientePosicaoFundos"))
                    {
                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método DeletaImportaPosicaoClienteFundosITAU ", ex);
            }
        }

        public void SelecionaImportaPosicaoClienteFundosITAU()
        {
            try
            {
                this.DeletaImportaPosicaoClienteFundosITAU();

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "ClubesFundos";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_POSICAO_COTISTAS"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (string.IsNullOrEmpty(dr["dsCpfCnpj"].ToString()))
                            {
                                continue;
                            }

                            PosicaoClienteItauInfo lPosicao = new PosicaoClienteItauInfo();
                            lPosicao.CodigoCliente          = SelecionaCodigoCliente(dr["dsCpfCnpj"].ToString());
                            lPosicao.Angencia               = dr["agencia"].ToString();
                            lPosicao.Banco                  = dr["banco"].ToString();
                            lPosicao.Conta                  = dr["conta"].ToString();
                            lPosicao.DigitoConta            = dr["digitoConta"].ToString();
                            lPosicao.DsCpfCnpj              = dr["dsCpfCnpj"].ToString();
                            lPosicao.DtProcessamento        = Convert.ToDateTime(dr["dtProcessamento"]);
                            lPosicao.DtReferencia           = Convert.ToDateTime(dr["dtReferencia"]);
                            lPosicao.IdCotista              = dr["idCotista"].ToString();
                            lPosicao.IdFundo                = SelecionaCodigoProduto(Convert.ToInt32(dr["idFundo"]));
                            lPosicao.IdMovimento            = Convert.ToInt32(dr["idMovimento"].ToString());
                            lPosicao.IdProcessamento        = Convert.ToInt32(dr["idProcessamento"].ToString());
                            lPosicao.QuantidadeCotas        = Convert.ToDecimal(dr["quantidadeCotas"].ToString());
                            lPosicao.SubConta               = dr["subConta"].ToString();
                            lPosicao.ValorBruto             = Convert.ToDecimal(dr["valorBruto"].ToString());
                            lPosicao.ValorCota              = Convert.ToDecimal(dr["valorCota"].ToString());
                            lPosicao.ValorIOF               = Convert.ToDecimal(dr["valorIOF"].ToString());
                            lPosicao.ValorIR                = Convert.ToDecimal(dr["valorIR"].ToString());
                            lPosicao.ValorLiquido           = Convert.ToDecimal(dr["ValorLiquido"].ToString());

                            this.ImportaPosicaoClienteFundosITAU(lPosicao);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaImportaPosicaoClienteFundosITAU = [{0}]", ex.StackTrace);
            }
        }

        public void ImportaPosicaoClienteFundosITAU(PosicaoClienteItauInfo info)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    gLogger.InfoFormat("Importando dados de fundo do Cliente [{0}]", info.CodigoCliente);

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_INS_POSICAO_COTISTAS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCliente",     DbType.Int32,    info.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@Agencia",           DbType.String,   info.Angencia);
                        lAcessaDados.AddInParameter(lCommand, "@Banco",             DbType.String,   info.Banco);
                        lAcessaDados.AddInParameter(lCommand, "@Conta",             DbType.String,   info.Conta);
                        lAcessaDados.AddInParameter(lCommand, "@DigitoConta",       DbType.String,   info.DigitoConta);
                        lAcessaDados.AddInParameter(lCommand, "@DsCpfCnpj",         DbType.String,   info.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lCommand, "@DtProcessamento",   DbType.DateTime, info.DtProcessamento);
                        lAcessaDados.AddInParameter(lCommand, "@DtReferencia",      DbType.DateTime, info.DtReferencia);
                        lAcessaDados.AddInParameter(lCommand, "@IdCotista",         DbType.String,   info.IdCotista);
                        lAcessaDados.AddInParameter(lCommand, "@IdFundo",           DbType.Int32,    info.IdFundo);
                        lAcessaDados.AddInParameter(lCommand, "@IdMovimento",       DbType.Int32,    info.IdMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@IdProcessamento",   DbType.Int32,    info.IdProcessamento);
                        lAcessaDados.AddInParameter(lCommand, "@QuantidadeCotas",   DbType.Decimal,  info.QuantidadeCotas);
                        lAcessaDados.AddInParameter(lCommand, "@SubConta",          DbType.String,   info.SubConta);
                        lAcessaDados.AddInParameter(lCommand, "@ValorCota",         DbType.Decimal,  info.ValorCota);
                        lAcessaDados.AddInParameter(lCommand, "@ValorBruto",        DbType.Decimal,  info.ValorBruto);
                        lAcessaDados.AddInParameter(lCommand, "@ValorIR",           DbType.Decimal,  info.ValorIR);
                        lAcessaDados.AddInParameter(lCommand, "@ValorIOF",          DbType.Decimal,  info.ValorIOF);
                        lAcessaDados.AddInParameter(lCommand, "@ValorLiquido",      DbType.Decimal,  info.ValorLiquido);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método ImportarPosicaoClienteFundosITAU = [{0}]", ex.StackTrace);
            }
        }

        public void AtualizaAplicacaoAgendada()
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_APLICACAO_AGENDADA_UPD"))
                    {
                        /*
                        lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao"   , DbType.Int32,      pInfo.TipoMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@idCliente"        , DbType.Int32,      pInfo.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@idProduto"        , DbType.Int32,      pInfo.CodigoInternoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@vlSolicitado"     , DbType.Decimal,    pInfo.VlrLiquidoSolicitado);
                        lAcessaDados.AddInParameter(lCommand, "@dtInclusao"       , DbType.DateTime,   pInfo.DtInclusao);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado"       , DbType.String,     pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@idStatusMovimento", DbType.Int32,      pInfo.StatusMovimento);
                        */
                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaAplicacaoAgendada", ex);
            }
        }
        #endregion
    }
}
