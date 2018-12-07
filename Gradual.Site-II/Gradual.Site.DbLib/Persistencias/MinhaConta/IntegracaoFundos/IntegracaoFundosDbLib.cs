using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data;
using System.Data.Common;
using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Generico.Dados;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Dados.MinhaConta.Suitability;
using System.Globalization;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta.Fundos
{
    public class IntegracaoFundosDbLib
    {
        #region Atributos
        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private  CultureInfo gCultura = new  CultureInfo("pt-BR");
        #endregion

        #region Construtores

        public IntegracaoFundosDbLib()
        {

        }
        #endregion

        #region Métodos
        public int VerificaStatusOperacaoFinancial(int CodigoCliente, string CodigoAnbima, int TipoOperacao)
        {
            //FIN_GRADUAL
            int lRetorno = 0;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();

                    lAcessaDados.ConnectionStringName = "FIN_GRADUAL";

                    string lSql = "select * from OperacaoCotista where IdCotista =" + CodigoCliente + 
                        " and IdCarteira = '" + CodigoAnbima + "' " +
                        " and TipoOperacao=" + TipoOperacao;

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["idOPeracao"] != null)
                            {
                                lRetorno = dt.Rows[0]["idOPeracao"].DBToInt32();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método VerificaStatusOperacaoFinancial = {0}", ex);
            }

            return lRetorno;
        }
        public bool VerificaExistenciaClienteItau(int CodigoCliente)
        {
            bool lRetorno = false;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();

                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    string lSql = "select CodigoCotistaItau from tbCliente where codCliente='" + CodigoCliente + "'";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["CodigoCotistaItau"] != null)
                            {
                                lRetorno = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método VerificaExistenciaClienteItau = {0}", ex);
            }

            return lRetorno;
        }

        public Nullable<int> VerificaExistenciaFundoItau(string CodigoAnbima)
        {
            Nullable<int> lCodigoFundoItau = null ;
            try
            {

                lCodigoFundoItau = null;

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();

                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    string lSql = "select * from tbProduto where idCodigoAnbima='" + CodigoAnbima + "'";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["CodigoFundoItau"].DBToInt32() != 0)
                            {
                                lCodigoFundoItau = dt.Rows[0]["CodigoFundoItau"].DBToInt32();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método VerificaExistenciaFundoItau = {0}", ex);
            }

            return lCodigoFundoItau;
        }
        public List<IntegracaoFundosAplicacaoResgateInfo> SelecionaAplicacaoResgateParaEnvio()
        {
            List<IntegracaoFundosAplicacaoResgateInfo> lRetorno = new List<IntegracaoFundosAplicacaoResgateInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RESGATE_APLICACAO_EFETUAR_SEL"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                IntegracaoFundosAplicacaoResgateInfo info = new IntegracaoFundosAplicacaoResgateInfo();

                                string CodigoCotistaItau = string.Empty;

                                if (row["CodigoCotistaItau"] != null)
                                {
                                    CodigoCotistaItau = row["CodigoCotistaItau"].ToString();
                                }

                                info.CodigoCarteira       = int.Parse(row["codAnbima"].ToString());
                                info.CodigoAgencia        = CodigoCotistaItau.Substring(0, 4);
                                info.CodigoConta          = CodigoCotistaItau.Substring(4, 7).PadLeft(9, '0');
                                info.DigitoConferencia    = CodigoCotistaItau.Substring(11, 1);
                                info.CodigoCliente        = int.Parse(row["idcliente"].ToString());
                                info.CodigoSubConta       = " ";
                                info.VlrLiquidoSolicitado = decimal.Parse(row["vlsolicitado"].ToString(), gCultura);
                                info.TipoMovimento        = row["idTipoOperacao"].ToString() == "1" ? "030" : "105";
                                info.ContaCredito         = SelecionaContaCreditoResgate(int.Parse(row["idcliente"].ToString()));
                                info.CodigoInternoFundo   = int.Parse(row["idproduto"].ToString());
                                info.DataLancamento       = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DataInclusao         = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DtInclusao           = Convert.ToDateTime(row["dtInclusao"]);
                                info.CodigoFundo          = row["CodigoFundoItau"].ToString();
                                info.CodigoInternoFundo   = int.Parse(row["idproduto"].ToString());

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

        public void AtualizaClienteCotista(IntegracaoFundosClienteFinancialInfo pInfo)
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

        public IntegracaoFundosClienteCotistaItauInfo SelecionaNovoCotistaItau(int CodigoBovespa)
        {
            IntegracaoFundosClienteCotistaItauInfo lRetorno = new IntegracaoFundosClienteCotistaItauInfo();

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

        public List<FundoInfo> ConsultarClientesFundosItau(FundoInfo pParametros)
        {
            var lRetorno = new List<FundoInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "FundosItau";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                lAcessaDados.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, pParametros.CGC);

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    FundoInfo fundo = new FundoInfo();

                    fundo.IdCliente       = pParametros.IdCliente;
                    fundo.Cota            = dr["valorCota"].DBToDecimal();
                    fundo.DataAtualizacao = dr["dtReferencia"].DBToDateTime();
                    fundo.IOF             = dr["valorIOF"].DBToDecimal();
                    fundo.IR              = dr["valorIR"].DBToDecimal();
                    fundo.NomeFundo       = dr["dsRazaoSocial"].DBToString().Trim();
                    fundo.Quantidade      = dr["quantidadeCotas"].DBToDecimal();
                    fundo.ValorBruto      = dr["valorBruto"].DBToDecimal();
                    fundo.ValorLiquido    = dr["valorLiquido"].DBToDecimal();

                    lRetorno.Add(fundo);
                }
            }


            return lRetorno;
        }

        public IntegracaoFundosAplicacaoResgateInfo SelecionaAplicacaoResgateParaEnvio(int Codigo)
        {
            var lRetorno = new IntegracaoFundosAplicacaoResgateInfo();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RESGATE_APLICACAO_EFETUAR_CLIENTE_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@cd_cliente", DbType.Int32, Codigo);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                var info = new IntegracaoFundosAplicacaoResgateInfo();

                                string CodigoCotistaItau = string.Empty;

                                //if (row["CodigoCotistaItau"] != null)
                                //{
                                //    CodigoCotistaItau = row["CodigoCotistaItau"].ToString();
                                //}
                                //info.CodigoAgencia        = CodigoCotistaItau.Substring(0, 4);
                                //info.CodigoConta          = CodigoCotistaItau.Substring(4, 7).PadLeft(9, '0');
                                //info.DigitoConferencia    = CodigoCotistaItau.Substring(11, 1);
                                //info.CodigoSubConta       = " ";
                                info.TipoMovimento        = row["idTipoOperacao"].ToString() == "1" ? "030" : "105";
                                //info.ContaCredito         = SelecionaContaCreditoResgate(int.Parse(row["idcliente"].ToString()));
                                info.CodigoInternoFundo   = int.Parse(row["idproduto"].ToString());
                                //info.CodigoFundo          = row["CodigoFundoItau"].ToString();
                                //info.CodigoInternoFundo   = int.Parse(row["idproduto"].ToString());

                                info.CodigoCarteira       = int.Parse(row["codAnbima"].ToString());
                                info.CodigoCliente        = int.Parse(row["idcliente"].ToString());
                                info.VlrLiquidoSolicitado = decimal.Parse(row["vlsolicitado"].ToString(), gCultura);
                                info.DataLancamento       = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DataInclusao         = Convert.ToDateTime(row["dtInclusao"]).ToString("ddMMyyyy");
                                info.DtInclusao           = Convert.ToDateTime(row["dtInclusao"]);

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
                                lRetorno = info ;
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

        public string SelecionaContaCreditoResgate(int Codigo)
        {
            string lRetorno = string.Empty;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    string lSql = "select (banco.cd_banco +''+ banco.ds_conta +''+ banco.ds_agencia +''+ banco.ds_conta_digito) as ds_dados from tb_cliente_conta conta, tb_cliente_banco banco where banco.st_principal = 1 and  banco.id_cliente = conta.id_cliente and conta.cd_codigo =" + Codigo;

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
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

        public void AtualizaAplicacaoResgateEmProcessamento(IntegracaoFundosAplicacaoResgateInfo pInfo)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_EMPROCESSAMENTO_APLICACAO_RESGATE_UPD"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao",        DbType.Int32,       pInfo.TipoMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@idCliente",             DbType.Int32,       pInfo.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@idProduto",             DbType.Int32,       pInfo.CodigoInternoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@vlSolicitado",          DbType.Decimal,     pInfo.VlrLiquidoSolicitado);
                        lAcessaDados.AddInParameter(lCommand, "@dtInclusao",            DbType.DateTime,    pInfo.DtInclusao);
                        lAcessaDados.AddInParameter(lCommand, "@stAprovado",            DbType.String,      pInfo.StAprovado);
                        lAcessaDados.AddInParameter(lCommand, "@idStatusMovimento",     DbType.Int32,       pInfo.StatusMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@dsMotivoStatus",        DbType.String,      pInfo.DsMotivoStatus);
                        lAcessaDados.AddInParameter(lCommand, "@IdOperacaoFinancial",   DbType.Int32,       pInfo.IdOperacaoFinancial);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método AtualizaAplicacaoResgateEmProcessamento", ex);
            }
        }

        public IntegracaoFundosClienteFinancialInfo SelecionaNovoCotistaFinancial(int CodigoBovespa)
        {
            IntegracaoFundosClienteFinancialInfo lRetorno = new IntegracaoFundosClienteFinancialInfo();

            lRetorno.Endereco       = "";
            lRetorno.Numero         = "";
            lRetorno.Complemento    = "";
            lRetorno.Bairro         = "";
            lRetorno.Cidade         = "";
            lRetorno.CEP            = "";
            lRetorno.UF             = "";
            lRetorno.Pais           = "";
            lRetorno.EnderecoCom    = "";
            lRetorno.NumeroCom      = "";
            lRetorno.ComplementoCom = "";
            lRetorno.BairroCom      = "";
            lRetorno.CidadeCom      = "";
            lRetorno.CEPCom         = "";
            lRetorno.UFCom          = "";
            lRetorno.PaisCom        = "";
            lRetorno.EmailCom       = "";
            lRetorno.FoneCom        = "";

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

                                lRetorno.NomeCliente    = row["ds_nome"].ToString();
                                lRetorno.CodigoCliente  = CodigoBovespa;
                                lRetorno.DsCpfCnpj      = row["ds_cpfcnpj"].ToString();
                                lRetorno.Email          = row["ds_email"].ToString();
                                lRetorno.TipoPessoa     = row["tp_pessoa"].ToString();
                                lRetorno.TipoCliente    = row["tp_cliente"].ToString();
                                lRetorno.TipoCotistaCvm = int.Parse(row["tp_cliente"].ToString());
                                lRetorno.Fone           = string.Concat(row["ds_ddd"].ToString(), row["ds_numero"].ToString());

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

                                lRetorno.EstadoCivil    = int.Parse(row["cd_estadocivil"].ToString());
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

        public int ObterIDCliente(int CodigoBovespa)
        {
            int idcliente = 0;
            AcessaDados lAcessaDados = new AcessaDados();
            lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "Cadastro";

            string lSql = "Select id_cliente from tb_cliente_conta where cd_codigo = " + CodigoBovespa + " and st_principal = 1 and cd_sistema = 'BOL'";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    idcliente = int.Parse(lDataTable.Rows[0]["id_cliente"].ToString());
                }
            }
            return idcliente;
        }

        public PerfilSuitabilityIntegracaoFundosResponse ObterSuitabilityCliente(int IdClienteBov) // só suitability
        {
            int idCliente = ObterIDCliente(IdClienteBov);

            var lPerfil = new PerfilSuitabilityIntegracaoFundosResponse();

            // busca o perfil na intranet
            AcessaDados lAcessaDados          = new AcessaDados();
            lAcessaDados.Conexao              = new Conexao();
            lAcessaDados.ConnectionStringName = "Cadastro";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_suitability_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, idCliente);
                
                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lPerfil.IdClienteSuitability  = lDataTable.Rows[0]["id_cliente_suitability"].DBToInt32();
                lPerfil.IdCliente             = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                lPerfil.Status                = lDataTable.Rows[0]["ds_status"].DBToString();
                lPerfil.dtRealizacao          = lDataTable.Rows[0]["dt_realizacao"].DBToDateTime();
                lPerfil.PreenchidoPeloCliente = lDataTable.Rows[0]["st_preenchidopelocliente"].DBToString();
                lPerfil.LoginRealizado        = lDataTable.Rows[0]["ds_loginrealizado"].ToString();
                lPerfil.Fonte                 = lDataTable.Rows[0]["ds_fonte"].ToString();
                lPerfil.Respostas             = lDataTable.Rows[0]["ds_respostas"].ToString();


                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    string perfilRetorno = lDataTable.Rows[0]["ds_perfil"].DBToString().ToLower();

                    if (perfilRetorno.Contains("baixo") ||perfilRetorno.Contains("conservador"))
                    {
                        lPerfil.PerfilSuitability = "Conservador";
                    }
                    else if (perfilRetorno.Contains("medio") || perfilRetorno.Contains("moderado"))
                    {
                        lPerfil.PerfilSuitability = "Moderado";
                    }
                    else if (perfilRetorno.Contains("agressivo") || perfilRetorno.Contains("arrojado"))
                    {
                        lPerfil.PerfilSuitability = "Arrojado";
                    }
                }
            }

            // busca lista de produtos por perfil 
            using (AcessaDados lAcessaListaProdutos = new AcessaDados())
            {
                lAcessaListaProdutos.Conexao = new Conexao();
                lAcessaListaProdutos.Conexao._ConnectionStringName = "PlataformaInviXX";

                lAcessaListaProdutos.ConnectionStringName = "PlataformaInviXX";
                using (DbCommand lCommand = lAcessaListaProdutos.CreateCommand(CommandType.StoredProcedure, "PRC_ListaProdutos_Por_SuitabilityCadastro"))
                {
                    lAcessaListaProdutos.AddInParameter(lCommand, "@PerfilCadastro", DbType.String, lPerfil.PerfilSuitability);
                    DataTable lDataTable = lAcessaListaProdutos.ExecuteDbDataTable(lCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lPerfil.idPerfilSuitability = lDataTable.Rows[0]["idPerfil"].DBToInt32();
                    }
                }
            }

            return lPerfil;
        }
        
        public ClienteInfo AtualizaSuitability(ClienteInfo pParametros)
        {
            try
            {
                AcessaDados lAcessaDados = new AcessaDados();
                ClienteInfo lResponse = new ClienteInfo();

                lAcessaDados.ConnectionStringName = "Cadastro";

                lResponse.MudouSuitability = false;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_suitability_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",                  DbType.Int32,   pParametros.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_perfil",                   DbType.String,  pParametros.ds_perfil);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_status",                   DbType.String,  pParametros.ds_status);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_realizacao",               DbType.Date,    pParametros.dt_realizacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_preenchidopelocliente",    DbType.Boolean, pParametros.st_preenchidopelocliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_loginrealizado",           DbType.String,  pParametros.ds_loginrealizado);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_fonte",                    DbType.String,  pParametros.ds_fonte);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_respostas",                DbType.String,  pParametros.ds_respostas);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_suitability",      DbType.Int32,   pParametros.IdClienteSuitability);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.MudouSuitability = true;

                    return lResponse;
                }
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        public SalvarTermoIntegracaoFundosResponse SalvarTermoAdesao(SalvarTermoIntegracaoFundosRequest pRequest)
        {
            SalvarTermoIntegracaoFundosResponse lResponse = new SalvarTermoIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ADESAO_TERMO_SLV"))
                {
                    lAcessaDados.AddInParameter(lCommand, "@cod_Cliente",           DbType.Int32,       pRequest.Adesao.CodigoCliente);
                    lAcessaDados.AddInParameter(lCommand, "@id_produto",            DbType.Int32,       pRequest.Adesao.CodigoFundo);
                    lAcessaDados.AddInParameter(lCommand, "@dtHr_Adesao",           DbType.DateTime,    pRequest.Adesao.DtHoraAdesao);
                    lAcessaDados.AddInParameter(lCommand, "@CodigoUsuarioLogado",   DbType.Int32,       pRequest.Adesao.CodigoUsuarioLogado);
                    lAcessaDados.AddInParameter(lCommand, "@DsUsuarioLogado",       DbType.String,      pRequest.Adesao.DsUsuarioLogado);
                    lAcessaDados.AddInParameter(lCommand, "@Origem",                DbType.String,      pRequest.Adesao.Origem);

                    lAcessaDados.ExecuteNonQuery(lCommand);

                    lResponse.AdesaoOk = true;
                }
            }

            return lResponse;
        }

        public PesquisarTermoIntegracaoFundosResponse GetTermoFundosSituacao(PesquisarTermoIntegracaoFundosRequest pRequest)
        {
            var lRetorno = new PesquisarTermoIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                string lSql = "select a.id_produto, b.idCodigoAnbima, a.id_cliente from tbAdesaoTermo a, tbProduto b where a.id_produto = b.idproduto and a.id_produto = " + pRequest.CodigoFundo + " and a.id_cliente = " + pRequest.CodigoCliente;

                lRetorno.ListTermo = new List<IntegracaoFundosTermoAdesaoInfo>();

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var lTermo = new IntegracaoFundosTermoAdesaoInfo();

                        lTermo.CodigoAnbima  = dt.Rows[0]["idCodigoAnbima"].DBToString();
                        lTermo.CodigoCliente = dt.Rows[0]["id_cliente"].DBToString();
                        lTermo.CodigoFundo   = dt.Rows[0]["id_produto"].DBToInt32();

                        lRetorno.ListTermo.Add(lTermo);
                    }
                }

            }

            return lRetorno;
        }

        public IntegracaoFundosInfo GetNomeRiscoFundo(int CodigoFundoItau)
        {
            var lRetorno = new IntegracaoFundosInfo();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                string lSql = "Select b.dsNomeProduto, a.dsRisco, b.idProduto, b.idCodigoAnbima from tbProdutoAnbima a, tbProduto b where b.CodigoFundoItau=" + CodigoFundoItau +
                    " and a.CodigoAnbima = b.idCodigoAnbima";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lRetorno.Risco          = dt.Rows[0]["dsRisco"].DBToString();
                        lRetorno.NomeProduto    = dt.Rows[0]["dsNomeProduto"].DBToString();
                        lRetorno.IdProduto      = dt.Rows[0]["idProduto"].DBToInt32();
                        lRetorno.IdCodigoAnbima = dt.Rows[0]["idCodigoAnbima"].DBToString();
                    }
                }
            }

            return lRetorno;
        }

        public IntegracaoFundosInfo GetNomeRiscoFundo(string CodigoAnbima, int IdFundo)
        {
            var lRetorno = new IntegracaoFundosInfo();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                string lSql = string.Empty;

                if (CodigoAnbima == string.Empty)
                {
                    lSql = "Select a.dsProduto, a.dsRisco, b.idProduto, a.CodigoAnbima, c.dsDiasPagResgate, b.CodigoFundoItau, d.IdCategoria " +
                    "from tbProdutoAnbima a , tbProduto b, tbProdutoMovimentoCota c, tbCategoriaProduto d where b.idProduto='" + IdFundo + "' and a.CodigoAnbima = b.IdCodigoAnbima and b.idProduto = c.idProduto and d.idCategoria = a.idCategoria";
                }
                else
                {
                    lSql = "Select a.dsProduto, a.dsRisco, b.idProduto, a.CodigoAnbima , c.dsDiasPagResgate, b.CodigoFundoItau, d.IdCategoria " +
                    "from tbProdutoAnbima a , tbProduto b, tbProdutoMovimentoCota c, tbCategoriaProduto d where a.codigoAnbima='" + CodigoAnbima + "' and a.CodigoAnbima = b.IdCodigoAnbima and b.idProduto = c.idProduto and d.idCategoria = a.idCategoria";
                }
                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lRetorno.Risco                              = dt.Rows[0]["dsRisco"].DBToString();
                        lRetorno.NomeProduto                        = dt.Rows[0]["dsProduto"].DBToString();
                        lRetorno.IdProduto                          = dt.Rows[0]["idProduto"].DBToInt32();
                        lRetorno.IdCodigoAnbima                     = dt.Rows[0]["CodigoAnbima"].DBToString();
                        lRetorno.DadosMovimentacao                  = new IntegracaoFundosMovimentoInfo();
                        lRetorno.DadosMovimentacao.DsDiasPagResgate = dt.Rows[0]["dsDiasPagResgate"].DBToString();
                        lRetorno.CodigoFundoItau                    = dt.Rows[0]["CodigoFundoItau"].ToString();
                        lRetorno.Categoria.IdCategoria              = dt.Rows[0]["IdCategoria"].DBToInt32();
                    }
                }
            }

            return lRetorno;
        }

        public PesquisarIntegracaoFundosResponse PesquisarFundos(PesquisarIntegracaoFundosRequest request)
        {
            PesquisarIntegracaoFundosResponse lRes = new PesquisarIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_PESQUISAR_PRODUTO"))
                {
                    if (request.IdProduto > 0)
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, request.IdProduto);

                    if (!string.IsNullOrWhiteSpace(request.Cnpj))
                        lAcessaDados.AddInParameter(lCommand, "@Cnpj", DbType.String, request.Cnpj);

                    if (!string.IsNullOrWhiteSpace(request.NomeProduto))
                        lAcessaDados.AddInParameter(lCommand, "@dsProduto", DbType.String, request.NomeProduto);

                    if (request.IdCategoria > 0)
                        lAcessaDados.AddInParameter(lCommand, "@idCategoria", DbType.Int32, request.IdCategoria);

                    if (!string.IsNullOrWhiteSpace(request.Indexadores))
                        lAcessaDados.AddInParameter(lCommand, "@idIndexadores", DbType.String, request.Indexadores);

                    if (!string.IsNullOrEmpty(request.Perfis))
                        lAcessaDados.AddInParameter(lCommand, "@Perfil", DbType.String, request.Perfis);


                  DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    foreach (DataRow dr in dt.Rows)
                    {
                        IntegracaoFundosInfo lpi                       = new IntegracaoFundosInfo();
                        lpi.IdProduto                                  = dr["idProduto"].DBToInt32();
                        lpi.NomeProduto                                = dr["dsNomeProduto"].DBToString();
                        lpi.Risco                                      = dr["dsRisco"].DBToString();
                        lpi.HorarioLimite                              = dr["hrLimMovimento"].DBToString();
                        lpi.StApareceSite                              = dr["stApareceSite"].DBToString();

                        lpi.Categoria                                  = new IntegracaoFundosCategoriaInfo();
                        lpi.Categoria.Descricao                        = dr["dsCategoria"].DBToString();
                        lpi.Categoria.IdCategoria                      = dr["idCategoria"].DBToInt32();

                        lpi.Indexador                                  = new IntegracaoFundosIndexadorInfo();
                        lpi.Indexador.Descricao                        = dr["dsIndexador"].DBToString();
                        lpi.Indexador.IdIndexador                      = dr["idIndexador"].DBToInt32();

                        lpi.DadosMovimentacao                             = new IntegracaoFundosMovimentoInfo();
                        lpi.DadosMovimentacao.AplicacaoMinimaInicial      = dr["VlrMinAplicInicial"].DBToDecimal();
                        lpi.DadosMovimentacao.AplicacaoMinimaAdicional    = dr["VlrMinAplicAdicional"].DBToDecimal();
                        lpi.DadosMovimentacao.SaldoMinimoAplicado         = dr["VlrMinSaldo"].DBToDecimal();
                        lpi.DadosMovimentacao.ValorMinimoResgate          = dr["VlrMinResgate"].DBToDecimal();
                        lpi.DadosMovimentacao.DsDiasConvAplicacao         = dr["DsDiasConvAplicacao"].DBToString();
                        lpi.DadosMovimentacao.DsDiasConvResgate           = dr["DsDiasConvResgate"].DBToString();
                        lpi.DadosMovimentacao.DsDiasConvResgateAntecipado = dr["DsDiasConvResgateAntecipado"].DBToString();
                        lpi.DadosMovimentacao.DsDiasPagResgate            = dr["DsDiasPagResgate"].DBToString();
                        lpi.DadosMovimentacao.VlTaxaAdmin                 = dr["vlTaxaAdmin"].DBToDecimal();
                        lpi.DadosMovimentacao.VlTaxaAdminMaxima           = dr["vlTaxaAdminMaxima"].DBToDecimal();
                        lpi.DadosMovimentacao.VlTaxaPerformance           = dr["vlTaxaPerformance"].ToString();
                        lpi.DadosMovimentacao.VlTaxaResgateAntecipado     = dr["vlTaxaResgateAntecipado"].DBToDecimal();
                        lpi.DadosMovimentacao.VlPatrimonioLiquido         = dr["vlPatrimonioLiquido"].DBToDecimal();
                        lpi.IdCodigoAnbima                                = dr["IdCodigoAnbima"].DBToString();

                        lpi.Rentabilidade                              = new IntegracaoFundosRentabilidadeInfo();
                        lpi.Rentabilidade.Ultimos12Meses               = dr["Rentabil12Meses"].DBToDecimal();
                        lpi.Rentabilidade.Dia                          = dr["rentDia"].DBToDecimal();
                        lpi.Rentabilidade.Mes                          = dr["rentMes"].DBToDecimal();
                        lpi.Rentabilidade.Ano                          = dr["rentAno"].DBToDecimal();
                        lpi.Rentabilidade.Data                         = dr["data"].DBToDateTime();
                        lpi.NomeArquivoProspecto                       = dr["pathProspecto"].DBToString();
                        lpi.NomeArquivoLamina                          = dr["pathLamina"].DBToString();
                        lpi.NomeArquivoRegulamento                     = dr["pathRegulamento"].DBToString();
                        lpi.NomeArquivoTermoPF                         = dr["pathTermo"].DBToString();
                        lpi.NomeArquivoTermoPJ                         = dr["pathTermoPJ"].DBToString();
                        lpi.NomeArquivoDemonstracaoFin                 = dr["pathDemonstracaoFin"].DBToString();
                        lpi.CpfCnpj                                    = dr["CPFCNPJ"].DBToString();

                        lRes.ListaFundos.Add(lpi);
                    }
                }
            }
            return lRes;
        }

        public PesquisarIntegracaoFundosResponse PesquisarFundosSuitability(PesquisarIntegracaoFundosRequest Request)
        {
            PesquisarIntegracaoFundosResponse lResponse = new PesquisarIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao                       = new Conexao();
                lAcessaDados.Conexao._ConnectionStringName = "PlataformaInviXX";
                lAcessaDados.ConnectionStringName          = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_LISTA_PRODUTO_SUITABILITY_LST"))
                {
                    lAcessaDados.AddInParameter(lCommand, "@idPerfilSuitability", DbType.String, Request.IdPerfilSuitability);

                    using (DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand))
                    {
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            IntegracaoFundosRentabilidadeInfo lRentabilidade = new IntegracaoFundosRentabilidadeInfo();
                            lRentabilidade.Ultimos12Meses                    = dr1["Rentabil12Meses"].DBToDecimal();
                            lRentabilidade.Ano                               = dr1["rentAno"].DBToDecimal();
                            lRentabilidade.Mes                               = dr1["rentMes"].DBToDecimal();

                            IntegracaoFundosMovimentoInfo lMovimento = new IntegracaoFundosMovimentoInfo();
                            lMovimento.AplicacaoMinimaAdicional      = dr1["VlrMinAplicAdicional"].DBToDecimal();
                            lMovimento.AplicacaoMinimaInicial        = dr1["VlrMinAplicInicial"].DBToDecimal();
                            lMovimento.ValorMinimoResgate            = dr1["VlrMinResgate"].DBToDecimal();
                            lMovimento.SaldoMinimoAplicado           = dr1["VlrMinSaldo"].DBToDecimal();
                            lMovimento.VlTaxaAdmin                   = dr1["TaxaAdmin"].DBToDecimal();

                            lResponse.ListaFundos.Add(new IntegracaoFundosInfo()
                            {
                                IdProduto            = dr1["idProduto"].DBToInt32(),
                                NomeProduto          = dr1["dsNomeProduto"].DBToString(),
                                Risco                = dr1["dsRisco"].DBToString(),
                                CDI                  = dr1["CDI"].DBToDecimal() * 100,
                                Rentabilidade        = lRentabilidade,
                                NomeArquivoProspecto = dr1["pathProspecto"].DBToString(),
                                DadosMovimentacao    = lMovimento,
                                NomeArquivoTermoPF   = dr1["pathTermo"].DBToString(),
                                NomeArquivoTermoPJ   = dr1["pathTermoPJ"].DBToString(),
                                HorarioLimite        = dr1["hrLimMovimento"].DBToString(),
                                StApareceSite        = dr1["stApareceSite"].DBToString()
                            });
                        }
                    }
                }
                
            }
            return lResponse;
        }

        public SolicitarIntegracaoFundosOperacaoResponse SolicitarOperacao(SolicitarIntegracaoFundosOperacaoRequest pRequest)
        {
            var lRes = new SolicitarIntegracaoFundosOperacaoResponse();

            var lCriticas = ValidarOperacaoResgateAplicacao(pRequest);

            if (lCriticas.Criticas.Count > 0)
            {
                lRes.Criticas = lCriticas.Criticas;
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                return lRes;
            }
            
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MOVIMENTO_APLICACAO_RESGATE_INS"))
                {
                    lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao",        DbType.Int32, (int)(pRequest.Operacao.TipoOperacao));
                    lAcessaDados.AddInParameter(lCommand, "@idCliente",             DbType.Int32, pRequest.Operacao.IdCliente);
                    lAcessaDados.AddInParameter(lCommand, "@idProduto",             DbType.Int32, pRequest.Operacao.Produto.IdProduto);
                    lAcessaDados.AddInParameter(lCommand, "@vlSolicitado",          DbType.Decimal, pRequest.Operacao.ValorSolicitado);
                    lAcessaDados.AddInParameter(lCommand, "@dtAgendamento",         DbType.DateTime, !pRequest.Operacao.DataAgendamento.HasValue ? DBNull.Value : (object)pRequest.Operacao.DataAgendamento);
                    lAcessaDados.AddInParameter(lCommand, "@stAplicacaoProgramada", DbType.Boolean, pRequest.Operacao.AplicacaoProgramada);
                    lAcessaDados.AddInParameter(lCommand, "@stAnteciparNaoDiaUtil", DbType.Boolean, pRequest.Operacao.AntecipaAplicacao);
                    //lAcessaDados.AddInParameter(lCommand, "@dtProcessamento", DbType.DateTime, DBNull.Value);
                    //lAcessaDados.AddInParameter(lCommand, "@idModeloAlocacao", DbType.Int32, DBNull.Value);
                    lAcessaDados.AddInParameter(lCommand, "@DiaAplicacaoProgramada", DbType.Int32, pRequest.Operacao.DiaAplicacaoProgramada);
                    lAcessaDados.AddInParameter(lCommand, "@stResgateTotal",        DbType.Boolean, pRequest.Operacao.ResgateTotal);

                    lAcessaDados.ExecuteNonQuery(lCommand);
                }
            }
            return lRes;
        }

        public ValidarIntegracaoFundosOperacaoAplicacaoResponse ValidarOperacaoResgateAplicacao(SolicitarIntegracaoFundosOperacaoRequest pRequest)
        {
            ValidarIntegracaoFundosOperacaoAplicacaoResponse lRes = new ValidarIntegracaoFundosOperacaoAplicacaoResponse();

            //Validar se está dentro do horário limite.
            var lResProduto = this.PesquisarFundos(new PesquisarIntegracaoFundosRequest()
            {
                IdProduto = pRequest.Operacao.Produto.IdProduto
            });

            IntegracaoFundosInfo lProduto = null;

            if (lResProduto.ListaFundos.Count > 0)lProduto = lResProduto.ListaFundos[0];

            List<FundoInfo> Fundos = new List<FundoInfo>();

            List<FundoInfo> SomatoriaFundos = new List<FundoInfo>();

            if (pRequest.PosicaoCotista != null)
            {
                Fundos = pRequest.PosicaoCotista.FindAll(fundo => { return fundo.CodigoFundo == pRequest.Operacao.Produto.IdProduto; });
            }

            FundoInfo lFundos = null;

            bool EhAplicacaoNova = true;
            
            if (Fundos.Count > 0)
            {
                EhAplicacaoNova = false;

                foreach (FundoInfo fundo in Fundos)
                {
                    if (lFundos == null)
                    {
                        lFundos = fundo;
                    }
                    else
                    {
                        lFundos.ValorBruto += fundo.ValorBruto;
                    }
                }
            }

            if (pRequest.Operacao.TipoOperacao == IntegracaoFundosTipoOperacaoEnum.APLICACAO)
            {
                //Valida o valor minimo adicional e inicial
                if (EhAplicacaoNova)
                {
                    if (pRequest.Operacao.ValorSolicitado < lProduto.DadosMovimentacao.AplicacaoMinimaInicial)
                    {
                        lRes.Criticas.Add(new CriticaInfo() { Descricao = string.Format("O valor mínimo da aplicação é de {0}", lProduto.DadosMovimentacao.AplicacaoMinimaInicial.ToString("c")), Status = CriticaStatusEnum.ErroNegocio });
                    }
                }
                else
                {
                    if (pRequest.Operacao.ValorSolicitado < lProduto.DadosMovimentacao.AplicacaoMinimaAdicional)
                    {
                        lRes.Criticas.Add(new CriticaInfo() { Descricao = string.Format("O valor mínimo da aplicação é de {0}", lProduto.DadosMovimentacao.AplicacaoMinimaAdicional.ToString("c")), Status = CriticaStatusEnum.ErroNegocio });
                    }
                }
                //Não vai mais validar saldo de conta de cliente para aplicações de fundos 
                //de acordo com reunião com o pessoal da Controladoria e Pio. - 24/01/2014
                /*
                if (pRequest.TipoAcesso == "Cliente")
                { // Só valida se for cliente

                    //Validar o Saldo disponível
                    ServicoFinanceiro lServFin = new ServicoFinanceiro();
                    var lResSaldo = lServFin.ObterDadosContaCorrente(pRequest.Operacao.IdCliente);

                    //var lSaldoProjetado = 1000000;
                    var lSaldoProjetado = lResSaldo.SaldoD0 + lResSaldo.SaldoD1 + lResSaldo.SaldoD2 + lResSaldo.SaldoD3;

                    if (pRequest.Operacao.ValorSolicitado > lSaldoProjetado)
                    {
                        lRes.Criticas.Add(new CriticaInfo() { Descricao = "Você não possui saldo disponível para esta operação.", Status = CriticaStatusEnum.ErroNegocio });
                    }
                }
                */
            }
            else
            {
                if (!pRequest.Operacao.ResgateTotal)
                {
                    if (lProduto.DadosMovimentacao.SaldoMinimoAplicado > 0)
                    {
                        if ((lFundos.ValorBruto - pRequest.Operacao.ValorSolicitado) < lProduto.DadosMovimentacao.SaldoMinimoAplicado)
                        {
                            lRes.Criticas.Add(new CriticaInfo() { Descricao = string.Format("O saldo mínimo desse fundo é de {0}, realize o resgate total.", lProduto.DadosMovimentacao.SaldoMinimoAplicado.ToString("c")), Status = CriticaStatusEnum.ErroNegocio });
                        }
                    }

                    if (lProduto.DadosMovimentacao.ValorMinimoResgate > 0)
                    {
                        if (pRequest.Operacao.ValorSolicitado < lProduto.DadosMovimentacao.ValorMinimoResgate)
                        {
                            lRes.Criticas.Add(new CriticaInfo() { Descricao = string.Format("O valor mínimo de resgate é de {0}", lProduto.DadosMovimentacao.ValorMinimoResgate.ToString("c")), Status = CriticaStatusEnum.ErroNegocio });
                        }
                    }
                }
            }

            if (!pRequest.Operacao.AplicacaoProgramada)
            {
                if (!pRequest.Operacao.DataAgendamento.HasValue)
                {
                    //Valida o horário inicial
                    if (!string.IsNullOrWhiteSpace(lProduto.HorarioLimite))
                    {
                        if (lProduto.HorarioLimite.Contains(":"))
                        {
                            int horaLimiteFundo = int.Parse(lProduto.HorarioLimite.Replace(":", ""));
                            int horaAtual = int.Parse(String.Concat(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString()));
                            if (horaAtual > horaLimiteFundo)
                                lRes.Criticas.Add(new CriticaInfo() { Descricao = String.Format("O horário para aplicação neste fundo é até as {0} horas. Você pode usar a opção “Agendar para” para deixar agendada para próxima janela de aplicação.", lProduto.HorarioLimite), Status = CriticaStatusEnum.ErroNegocio });
                        }
                    }
                    //else
                    //{
                    //    lRes.Criticas.Add(new CriticaInfo() { Descricao = "Fundo sem horário de fechamento cadastrado.", Status = CriticaStatusEnum.ErroNegocio });
                    //}
                }
            }
            if (pRequest.Operacao.DataAgendamento.HasValue)
            {
                if (pRequest.Operacao.DataAgendamento.Value <= DateTime.Now)
                {
                    lRes.Criticas.Add(new CriticaInfo() { Descricao = "Data do agendamento deve ser maior que a data de hoje.", Status = CriticaStatusEnum.ErroNegocio });
                }
            }
            if (lRes.Criticas.Count > 0)
            {
                lRes.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }
            else
            {
                bool lIsWeekend = false;

                if (pRequest.Operacao.TipoOperacao == IntegracaoFundosTipoOperacaoEnum.APLICACAO)
                {
                    if (pRequest.Operacao.DataAgendamento.HasValue)
                    {
                        pRequest.Operacao.DataAgendamento = RetornarAntecipacaoDiaNaoUtil(pRequest.Operacao.DataAgendamento, pRequest.Operacao.AntecipaAplicacao, out lIsWeekend);

                        if (lIsWeekend && !pRequest.Operacao.AntecipaAplicacao)
                        {
                            lRes.DescricaoResposta = "Caso o agendamento caia em dia não útil, sua aplicação será executada no próximo dia útil.";
                        }
                        else if (lIsWeekend && pRequest.Operacao.AntecipaAplicacao)
                        {
                            lRes.DescricaoResposta = "Caso o agendamento caia em dia não útil, sua aplicação será antecipada para o dia útil anterior.";
                        }
                    }
                }

                lRes.Operacao = pRequest.Operacao;
            }

            return lRes;
        }

        private Nullable<DateTime> RetornarAntecipacaoDiaNaoUtil(Nullable<DateTime> pData, bool pAntecipaData, out bool pIsWeekend)
        {
            pIsWeekend = false;
            if (pData.HasValue)
            {
                if (pData.Value.DayOfWeek == DayOfWeek.Saturday)
                {
                    pIsWeekend = true;
                    if (pAntecipaData)
                        return pData.Value.AddDays(-1);
                    else
                        return pData.Value.AddDays(2);
                }
                if (pData.Value.DayOfWeek == DayOfWeek.Saturday)
                {
                    pIsWeekend = true;
                    if (pAntecipaData)
                        return pData.Value.AddDays(-2);
                    else
                        return pData.Value.AddDays(1);
                }
                return pData;
            }
            return null;
        }

        public RentabilidadeIntegracaoFundosResponse PesquisarRentabilidadeFundo(RentabilidadeIntegracaoFundosRequest pRequest)
        {
            RentabilidadeIntegracaoFundosResponse lRetorno = new RentabilidadeIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao              = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (int codigoProduto in pRequest.Produtos)
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_PRODUTO_RENTABILIDADE_ATUAL_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, codigoProduto);

                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            foreach (DataRow dr in lReader.Rows)
                            {
                                lRetorno.Resultado.Add(new IntegracaoFundosRentabilidadeAcumuludaInfo()
                                {
                                    Produto = new IntegracaoFundosInfo()
                                    {
                                        IdProduto   = dr["idProduto"].DBToInt32(),
                                        NomeProduto = dr["dsNomeProduto"].DBToString(),
                                        DataInicio  = dr["dataInicio"].DBToDateTime()
                                    }
                                    ,
                                    PatrimonioLiquido    = dr["patrLiquido"].DBToDecimal(),
                                    Rentabilidade12Meses = dr["rent12Meses"].DBToDecimal(),
                                    RentabilidadeAno     = dr["rentAno"].DBToDecimal(),
                                    RentabilidadeMes     = dr["rentMes"].DBToDecimal()
                                });
                            }
                        }
                    }
                }
            }
            return lRetorno;
        }

        public IntegracaoFundosIndexadorInfo RetornoDoIndicePorPeriodo(IndicePeriodoIntegracaoFundosRequest pRequest) 
        {
            IntegracaoFundosIndexadorInfo lIndice = new IntegracaoFundosIndexadorInfo();

            AcessaDados lAcessaDados          = new AcessaDados();
            lAcessaDados.Conexao              = new Conexao();
            lAcessaDados.ConnectionStringName = "PlataformaInviXX";

            if (!string.IsNullOrEmpty(pRequest.NomeIndexador))
            {

                string lSql = "SELECT CodInd idIndexador FROM tbANBIMAIndicadores WHERE (Descricao = '" + pRequest.NomeIndexador + "')";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lIndice.IdIndexador = lDataTable.Rows[0]["idIndexador"].DBToInt32();
                    }
                }

                string lSqlNome = "SELECT CodInd idIndexador FROM tbANBIMAIndicadores WHERE (Descricao = '" + pRequest.NomeIndexador + "')";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSqlNome))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lIndice.IdIndexador = lDataTable.Rows[0]["idIndexador"].DBToInt32();
                    }
                }

                lIndice.NomeIndexador = pRequest.NomeIndexador;
            }
            else
            {
                string lSqlId = "SELECT Descricao dsIndexador FROM tbANBIMAIndicadores WHERE (CodInd = '" + pRequest.IdIndexador + "')";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSqlId))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lIndice.NomeIndexador = lDataTable.Rows[0]["dsIndexador"].DBToString();
                    }
                }

                lIndice.IdIndexador = pRequest.IdIndexador;
            }

            DateTime dtFim = DateTime.Today;

            // para o acumulado do ano
            DateTime dtInicio = new DateTime(DateTime.Today.Year, 1, 1);
            lIndice.RetornoAno = this.BuscarRetorno(lIndice.NomeIndexador, dtInicio, dtFim);

            //para o acumulado no mês
            DateTime dtInicioAcumuladoMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            lIndice.RetornoMes = this.BuscarRetorno(lIndice.NomeIndexador, dtInicioAcumuladoMes, dtFim);

            // acumulado de 12 meses
            DateTime dtInicioAcumulado12Meses = DateTime.Now.AddDays(-365); // new DateTime(DateTime.Now.AddYears(-1), DateTime.Today.Month, DateTime.Today.Day); //
            lIndice.Retorno12Meses = this.BuscarRetorno(lIndice.NomeIndexador, dtInicioAcumulado12Meses, dtFim);

            //acumulado de 24 meses
            DateTime dtInicioAcumulado24Meses = DateTime.Now.AddDays(-365 * 2);
            lIndice.Retorno24Meses = this.BuscarRetorno(lIndice.NomeIndexador, dtInicioAcumulado24Meses, dtFim);

            //acumulado de 36 meses
            DateTime dtInicioAcumulado36Meses = DateTime.Now.AddDays(-365 * 3);
            lIndice.Retorno36Meses = this.BuscarRetorno(lIndice.NomeIndexador, dtInicioAcumulado36Meses, dtFim);

            //lIndice.Sharpe = 

            return lIndice;
        }

        public decimal BuscarRetorno(string nomeIndexador, DateTime dataInicio, DateTime dataFim)
        {
            decimal retorno = 0;
            decimal vlBase = 100;

            AcessaDados lAcessaDados = new AcessaDados();
            lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "PlataformaInviXX";


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_INDEXADORES_RENTABILIDADE"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@indexador", DbType.String, nomeIndexador);
                lAcessaDados.AddInParameter(lDbCommand, "@dtInicio", DbType.Date, dataInicio);
                lAcessaDados.AddInParameter(lDbCommand, "@dtFim", DbType.Date, dataFim);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    foreach (DataRow retornoDiario in lDataTable.Rows)
                    {
                        vlBase = vlBase + (vlBase * (retornoDiario["vlTaxaIndexador"].DBToDecimal() / 100));  //vlBase * (1 + (retornoDiario["vlTaxaIndexador"].DBToDecimal() / 100) );

                    }
                    retorno = vlBase - 100;
                }
            }
            return Math.Round(retorno, 2);
        }

        public CompararRentabilidadeIntegracaoFundosResponse ListarRentabilidadeMesDetalhes(CompararRentabilidadeIntegracaoFundosRequest pRequest)
        {
            CompararRentabilidadeIntegracaoFundosResponse lResponse = new CompararRentabilidadeIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();

                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (int codigoProduto in pRequest.Produtos)
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_LISTAR_RENTABILIDADE_MES_DET"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, codigoProduto);

                        lAcessaDados.AddInParameter(lCommand, "@periodo", DbType.Int32, pRequest.Periodo);

                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            List<IntegracaoFundosSimulacaoInfo> lResultadoSimulacao = new List<IntegracaoFundosSimulacaoInfo>();

                            foreach (DataRow dr in lReader.Rows)
                            {
                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();

                                //p.Produto.NomeProduto = dr["dsNomeProduto"].DBToString();
                                p.Produto.IdProduto   = dr["idProduto"].DBToInt32();
                                //p.Valor               = dr["Valor"].DBToDecimal();
                                p.Variacao            = dr["rentMes"].DBToDecimal();
                                p.VariacaoAno         = dr["rentAno"].DBToDecimal();
                                p.Data                = dr["data"].DBToDateTime();
                                lResultadoSimulacao.Add(p);
                            }

                            lResultadoSimulacao.Reverse();

                            lResponse.FundosSimulados.Add(lResultadoSimulacao);
                        }
                    }
                }
            }

            return lResponse;
        }

        public CompararRentabilidadeIntegracaoFundosResponse CompararRentabilidade(CompararRentabilidadeIntegracaoFundosRequest pRequest)
        {
            CompararRentabilidadeIntegracaoFundosResponse lResponse = new CompararRentabilidadeIntegracaoFundosResponse();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (int codigoProduto in pRequest.Produtos)
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SIMULAR_FUNDO_MES_LST"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, codigoProduto);

                        lAcessaDados.AddInParameter(lCommand, "@periodo", DbType.Int32, pRequest.Periodo);

                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            List<IntegracaoFundosSimulacaoInfo> lResultadoSimulacao = new List<IntegracaoFundosSimulacaoInfo>();

                            foreach (DataRow dr in lReader.Rows)
                            {
                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();

                                p.Produto.NomeProduto           = dr["dsNomeProduto"].DBToString();
                                p.Produto.IdProduto             = dr["idProduto"].DBToInt32();
                                p.Valor                         = dr["Valor"].DBToDecimal();
                                p.Variacao                      = dr["rentDia"].DBToDecimal();
                                p.VariacaoAno                   = dr["rentAno"].DBToDecimal();
                                p.Data                          = dr["data"].DBToDateTime();
                                lResultadoSimulacao.Add(p);
                            }
                            
                            lResultadoSimulacao.Reverse();

                            lResponse.FundosSimulados.Add(lResultadoSimulacao);
                        }
                    }
                }


                int duracao = pRequest.Periodo * (-1);

                int lDataToday = 0;

                if (DateTime.IsLeapYear(DateTime.Today.Year) && DateTime.Today.Month == 2 && DateTime.Today.Day==29)
                {
                    lDataToday = DateTime.Today.Day - 1;
                }else
                {
                    lDataToday = DateTime.Today.Day ;
                }

                DateTime dataInicio = new DateTime(DateTime.Today.AddYears(duracao).Year, DateTime.Today.Month, lDataToday);
                
                foreach (int codigoIndexador in pRequest.Indexadores) 
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SIMULAR_FUNDO_INDEXADORES_LST"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@indexador", DbType.Int32, codigoIndexador);
                        
                        lAcessaDados.AddInParameter(lCommand, "@dataInicio", DbType.DateTime, dataInicio);
                        
                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            List<IntegracaoFundosSimulacaoInfo> lResultadoSimulacao = new List<IntegracaoFundosSimulacaoInfo>();
                            
                            decimal valorAtualizado = 1;

                            foreach (DataRow dr in lReader.Rows)
                            {
                                valorAtualizado = valorAtualizado * (1 + (dr["vlTaxaIndexador"].DBToDecimal() / 100));

                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();

                                p.Produto.NomeProduto = dr["dsIndexador"].DBToString();
                                p.Produto.IdProduto   = dr["idIndexador"].DBToInt32();
                                p.Valor               = valorAtualizado;
                                p.Variacao            = dr["vlTaxaIndexador"].DBToDecimal();
                                p.Data                = dr["dtIndexador"].DBToDateTime();

                                lResultadoSimulacao.Add(p);
                            }
                            lResponse.FundosSimulados.Add(lResultadoSimulacao);
                        }
                    }
                }
            }
            return lResponse;
        }

        public SimularAplicacaoIntegracaoFundosResponse ListarSimularAplicacaoGrid(SimularAplicacaoIntegracaoFundosRequest request)
        {
            SimularAplicacaoIntegracaoFundosResponse lResponse = new SimularAplicacaoIntegracaoFundosResponse();

            lResponse.ListarProdutosSimulados = new List<IntegracaoFundosSimulacaoInfo>();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (int codigoProduto in request.Produtos)
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SIMULAR_FUNDO_GRID_LST"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, codigoProduto);
                        lAcessaDados.AddInParameter(lCommand, "@periodo", DbType.Int32, request.Periodo);
                        lAcessaDados.AddInParameter(lCommand, "@ValorInvestido", DbType.Decimal, request.Valor);

                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            

                            foreach (DataRow dr in lReader.Rows)
                            {
                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();

                                p.Ativo           = dr["Ativo"].ToString();
                                p.Retorno         = dr["Retorno"].DBToDecimal();
                                p.Volume          = dr["Volume"].DBToDecimal();
                                p.Sharpe          = dr["Sharpe"].DBToDecimal();
                                p.Patrimonio      = dr["Patrimonio"].DBToDecimal();
                                p.CDI             = dr["CDI"].DBToDecimal();
                                p.Resgate         = dr["Resgate"].DBToDecimal();
                                p.AplicacaoMinima = dr["AplicacaoMinima"].DBToDecimal();
                                p.Inicio          = dr["Inicio"].DBToDecimal();
                                p.Ultimo12Meses   = dr["Ultimo12Meses"].DBToDecimal();
                                p.AcumuladoAno    = dr["AcumuladoAno"].DBToDecimal();
                                p.MesAnterior     = dr["MesAnterior"].DBToDecimal();
                                
                                lResponse.ListarProdutosSimulados.Add(p);
                            }
                        }
                    }
                }
            }

            return lResponse;
        }

        public SimularAplicacaoIntegracaoFundosResponse SimularRentabilidade(SimularAplicacaoIntegracaoFundosRequest request)
        {
            SimularAplicacaoIntegracaoFundosResponse lResponse = new SimularAplicacaoIntegracaoFundosResponse();

            lResponse.ProdutosSimulados = new List<List<IntegracaoFundosSimulacaoInfo>>();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";
                foreach (int codigoProduto in request.Produtos)
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SIMULAR_FUNDO_LST"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, codigoProduto);
                        lAcessaDados.AddInParameter(lCommand, "@periodo", DbType.Int32, request.Periodo);
                        lAcessaDados.AddInParameter(lCommand, "@ValorInvestido", DbType.Decimal, request.Valor);
                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            List<IntegracaoFundosSimulacaoInfo> lResultadoSimulacao = new List<IntegracaoFundosSimulacaoInfo>();
                            
                            foreach (DataRow dr in lReader.Rows)
                            {
                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();
                                p.Produto.NomeProduto  = dr["dsNomeProduto"].DBToString();
                                p.Produto.IdProduto    = dr["idProduto"].DBToInt32();
                                p.Valor                = dr["valor"].DBToDecimal();
                                p.Variacao             = dr["rentDia"].DBToDecimal();
                                p.Data                 = dr["data"].DBToDateTime();
                                lResultadoSimulacao.Add(p);
                            }

                            lResponse.ProdutosSimulados.Add(lResultadoSimulacao);
                        }
                    }
                } // fim das simulações de produtos

                int duracao = request.Periodo * (-1);
                DateTime dataInicio = new DateTime(DateTime.Today.AddYears(duracao).Year, DateTime.Today.Month, DateTime.Today.Day);
                foreach (int codigoIndexador in request.Indexadores) // contorno para simular indexadores como se fossem produtos
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SIMULAR_FUNDO_INDEXADORES_LST"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@indexador", DbType.Int32, codigoIndexador);
                        lAcessaDados.AddInParameter(lCommand, "@dataInicio", DbType.DateTime, dataInicio);
                        
                        using (DataTable lReader = lAcessaDados.ExecuteDbDataTable(lCommand))
                        {
                            List<IntegracaoFundosSimulacaoInfo> lResultadoSimulacao = new List<IntegracaoFundosSimulacaoInfo>();

                            decimal valorAtualizado = request.Valor;

                            foreach (DataRow dr in lReader.Rows)
                            {
                                valorAtualizado = valorAtualizado * (1 + (dr["vlTaxaIndexador"].DBToDecimal() / 100));

                                IntegracaoFundosSimulacaoInfo p = new IntegracaoFundosSimulacaoInfo();

                                p.Produto.NomeProduto = dr["dsIndexador"].DBToString();
                                p.Produto.IdProduto   = dr["idIndexador"].DBToInt32();
                                p.Valor               = valorAtualizado;
                                p.Variacao            = dr["vlTaxaIndexador"].DBToDecimal();
                                p.Data                = dr["dtIndexador"].DBToDateTime();

                                lResultadoSimulacao.Add(p);
                            }

                            lResponse.ProdutosSimulados.Add(lResultadoSimulacao);
                        }
                    }
                }// fim das simulações de indexadores
            }

            return lResponse;
        }

        public PesquisarMovimentoOperacoesIntegracaoFundosResponse PesquisarMovimentoOperacoes(PesquisarMovimentoOperacoesIntegracaoFundosRequest pRequest)
        { // usado na widget 'movimentacao', telas 'solicitacoes em aberto', 'relatorio de movimentacao'
            PesquisarMovimentoOperacoesIntegracaoFundosResponse lResponse = new PesquisarMovimentoOperacoesIntegracaoFundosResponse();


            DateTime horarioIni = pRequest.ObterDataInicio();
            DateTime horarioFim = pRequest.ObterDataFim();

            this.ReceberHorario(pRequest.HorarioSolicitacaoIni, ref horarioIni);
            this.ReceberHorario(pRequest.HorarioSolicitacaoFim, ref horarioFim);

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MOVIMENTOAPLICRESCLIENTE_LST"))
                {
                    //lAcessaDados.AddInParameter(lCommand, "@CodCliente", DbType.String, string.IsNullOrWhiteSpace(lClientes) ? DBNull.Value : (object)lClientes);
                    lAcessaDados.AddInParameter(lCommand, "@idTipoOperacao",    DbType.String,      string.IsNullOrWhiteSpace(pRequest.TipoOperacao) ? DBNull.Value : (object)pRequest.TipoOperacao);
                    lAcessaDados.AddInParameter(lCommand, "@idStatusMovimento", DbType.String,      string.IsNullOrWhiteSpace(pRequest.Status) ? DBNull.Value : (object)pRequest.Status);
                    lAcessaDados.AddInParameter(lCommand, "@DataHoraIni",       DbType.DateTime,    horarioIni);
                    lAcessaDados.AddInParameter(lCommand, "@DataHoraFim",       DbType.DateTime,    horarioFim);
                    lAcessaDados.AddInParameter(lCommand, "@IdProduto",         DbType.Int32,       pRequest.IdProduto.Equals(0) ? DBNull.Value : (object)pRequest.IdProduto);
                    lAcessaDados.AddInParameter(lCommand, "@IdCasa",            DbType.Int32,       pRequest.IdCasa.Equals(0) ? DBNull.Value : (object)pRequest.IdCasa);
                    lAcessaDados.AddInParameter(lCommand, "@opcaoBusca",        DbType.String,      pRequest.BuscaDeCliente.BuscarPor.ToString());
                    lAcessaDados.AddInParameter(lCommand, "@termoBusca",        DbType.String,      pRequest.BuscaDeCliente.TermoDeBusca);

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    foreach (DataRow dr in dt.Rows)
                    {
                        IntegracaoFundosMovimentoOperacoesInfo lMoi = new IntegracaoFundosMovimentoOperacoesInfo();

                        lMoi.CodCliente                 = dr["idCliente"].DBToInt32();
                        lMoi.NomeCliente                = dr["nomeCliente"].DBToString();
                        lMoi.IdMovimento                = dr["idMovimento"].DBToInt32();
                        lMoi.ValorSolicitado            = dr["vlSolicitado"].DBToDecimal();
                        lMoi.Status                     = (IntegracaoFundosStatusOperacaoEnum)dr["idStatusMovimento"].DBToInt32();
                        lMoi.TipoOperacao               = (IntegracaoFundosTipoOperacaoEnum)dr["idTipoOperacao"].DBToInt32();
                        lMoi.Fundo.IdProduto            = dr["idProduto"].DBToInt32();
                        lMoi.Fundo.NomeProduto          = dr["dsNomeProduto"].DBToString();
                        lMoi.Fundo.NomeArquivoProspecto = dr["pathLamina"].DBToString();
                        lMoi.Fundo.CpfCnpj              = dr["CpfCnpj"].DBToString();
                        lMoi.DataAgendamento            = dr["dtAgendamento"].DBToDateTime();
                        lMoi.Casa                       = dr["casa"].DBToString();
                        lMoi.DtHrSolicitacao            = dr["dtInclusao"].DBToDateTime();
                        lMoi.Observacoes                = dr["DsObservacao"].DBToString();
                        lMoi.MotivoStatus               = dr["DsMotivoStatus"].DBToString();
                        lMoi.CodigoAnbima               = dr["CodigoAnbima"].DBToString();
                        //var lFundos                     = this.ObterFundosCliente(lMoi.CodCliente.ToString(), lMoi.Fundo.CpfCnpj);
                        //if (lFundos != null && lFundos.Count > 0) lMoi.SaldoAtual = lFundos[0].ValorBruto;
                        lMoi.SaldoCC = 0;
                        
                        lResponse.Resultado.Add(lMoi);
                    }
                }
            }

            return lResponse;
        }

        private void ReceberHorario(string pHorarioSolicitacao, ref DateTime pDataRetorno)
        {
            if (!string.IsNullOrWhiteSpace(pHorarioSolicitacao))
            {
                if (pHorarioSolicitacao.IndexOf(":") >= 0)
                {
                    int hora = 0;
                    int minuto = 0;

                    int.TryParse(pHorarioSolicitacao.Split(':')[0], out hora);
                    int.TryParse(pHorarioSolicitacao.Split(':')[1], out minuto);

                    pDataRetorno = new DateTime(pDataRetorno.Year, pDataRetorno.Month, pDataRetorno.Day, hora, minuto, 0);
                }
            }
        }

        public PesquisarSaldoClienteIntegracaoFundosResponse PesquisarSaldoCliente(PesquisarSaldoClienteIntegracaoFundosRequest pRequest)
        {
            PesquisarSaldoClienteIntegracaoFundosResponse lResponse = new PesquisarSaldoClienteIntegracaoFundosResponse();

            List<string> ListaCpfCnpj = new List<string>();
            
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DADOS_CLIENTE_SEL"))
                {
                    lAcessaDados.AddInParameter(lCommand, "@opcaoBusca", DbType.String, pRequest.BuscarPor.ToString());
                    lAcessaDados.AddInParameter(lCommand, "@termoBusca", DbType.String, pRequest.TermoDeBusca);
                    lAcessaDados.AddInParameter(lCommand, "@tipoPessoa", DbType.String, (pRequest.TipoCliente.HasValue) ? pRequest.TipoCliente.Value == IntegracaoFundosOpcaoTipoClienteEnum.PessoaFisica ? (object)"F" : (object)"J" : DBNull.Value);
                    lAcessaDados.AddInParameter(lCommand, "@idProduto", DbType.Int32, pRequest.CodigoProduto);

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                    foreach (DataRow dr in dt.Rows)
                    {
                        SaldoIntegracaoFundosInfo lSaldo = new SaldoIntegracaoFundosInfo();

                        
                        lSaldo.ClienteCodigo = dr["CodCliente"].DBToString();
                        lSaldo.ClienteNome = dr["nomeCliente"].DBToString();
                        lSaldo.ClienteTelefone = dr["Telefone"].DBToString();
                        //ListaCpfCnpj.Add(dr["CPFCNPJ"].DBToString());
                        //var lCC = this.ObterDadosContaCorrente(lSaldo.Cliente.Codigo);

                        //if (lCC != null)
                        //{
                        //    var lSaldoProjetado = lCC.SaldoD0 + lCC.SaldoD1 + lCC.SaldoD2 + lCC.SaldoD3;
                        //    lSaldo.SaldoBloqueado = lCC.SaldoBloqueado.DBToDecimal();
                        //    lSaldo.SaldoDisponivel = lSaldoProjetado;
                        //    lSaldo.SaldoTotal = lSaldoProjetado - lSaldo.SaldoBloqueado;
                        //}
                        lResponse.Resultado.Add(lSaldo);
                    }
                }
            }

            lResponse.ListaContaCorrente = this.ObterDadosContaCorrente(ListaCpfCnpj);

            return lResponse;
        }

        public List<ContaCorrenteInfo> ObterDadosContaCorrente(List<string> pListaCpfCnpj)
        {
            IServicoContaCorrente lServCC = Ativador.Get<IServicoContaCorrente>();

            var lResSaldo = lServCC.ObterSaldoContaCorrente(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
            {
                ConsultaClientesCpfCnpj = pListaCpfCnpj
            });

            if (lResSaldo != null && lResSaldo.ObjetoLista != null)
            {
                return lResSaldo.ObjetoLista;
            }

            return null;
        }

        public List<ContaCorrenteInfo> ObterDadosContaCorrente(int pCodigoCliente)
        {
            IServicoContaCorrente lServCC = Ativador.Get<IServicoContaCorrente>();

            var lResSaldo = lServCC.ObterSaldoContaCorrente(new Gradual.OMS.ContaCorrente.Lib.Mensageria.SaldoContaCorrenteRequest()
            {
                ConsultaContaMargem = false,
                IdCliente = pCodigoCliente
                //ConsultaClientesCpfCnpj = pListaCpfCnpj
            });

            if (lResSaldo != null)
            {
                return new List<ContaCorrenteInfo>() { lResSaldo.Objeto };
            }

            return null;
        }

        public List<RendaFixaInfo> ConsultarRendaFixa(RendaFixaInfo pParametros)
        {
            var lRetorno = new List<RendaFixaInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "MINICOM";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicao_rendafixa"))
            
            {
                lAcessaDados.AddInParameter(cmd, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente );

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    var RendaFixa = new RendaFixaInfo();

                    RendaFixa.CodigoCliente    = pParametros.CodigoCliente;
                    RendaFixa.Titulo           = dr["Titulo"].DBToString();
                    RendaFixa.Aplicacao        = dr["Aplicacao"].DBToDateTime();
                    RendaFixa.Vencimento       = dr["Vencimento"].DBToDateTime();
                    RendaFixa.Taxa             = dr["Taxa"].DBToDecimal();
                    RendaFixa.Quantidade       = dr["Quantidade"].DBToDecimal();
                    RendaFixa.ValorOriginal    = dr["ValorOriginal"].DBToDecimal();
                    RendaFixa.SaldoBruto       = dr["SaldoBruto"].DBToDecimal();
                    RendaFixa.IRRF             = dr["IRRF"].DBToDecimal();
                    RendaFixa.IOF              = dr["IOF"].DBToDecimal();
                    RendaFixa.SaldoLiquido     = dr["SaldoLiquido"].DBToDecimal();

                    lRetorno.Add(RendaFixa);
                }
            }

            return lRetorno;
        }

        public List<Gradual.Site.DbLib.Dados.CustodiaTesouro> ConsultarRendaFixaTitulosPublicos(RendaFixaInfo pParametros)
        {
            var lRetorno = new List<Gradual.Site.DbLib.Dados.CustodiaTesouro>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "MINICOM";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicao_rendafixa_titulos_publicos"))
            {
                lAcessaDados.AddInParameter(cmd, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    var RendaFixa = new Gradual.Site.DbLib.Dados.CustodiaTesouro();

                    RendaFixa.Instrumento = dr["Titulo"].DBToString();
                    RendaFixa.DataPosicao = dr["dt_atual"].DBToString();
                    RendaFixa.Quantidade = dr["Quantidade"].DBToDecimal();
                    RendaFixa.ValorPosicao = dr["SaldoLiquido"].DBToDecimal();
                    RendaFixa.Preco = RendaFixa.ValorPosicao / RendaFixa.Quantidade;

                    lRetorno.Add(RendaFixa);
                }
            }

            return lRetorno;
        }

        public List<RendaFixaInfo> ConsultarRendaFixaTitulosPrivados(RendaFixaInfo pParametros)
        {
            var lRetorno = new List<RendaFixaInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "MINICOM";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicao_rendafixa_titulos_privados"))
            {
                lAcessaDados.AddInParameter(cmd, "@cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    var RendaFixa = new RendaFixaInfo();

                    RendaFixa.CodigoCliente     = pParametros.CodigoCliente;
                    RendaFixa.Titulo            = dr["Titulo"].DBToString();
                    RendaFixa.Emissor           = dr["NM_EMISSOR"].DBToString();
                    RendaFixa.Aplicacao         = dr["dt_atual"].DBToDateTime();
                    RendaFixa.Vencimento        = dr["Vencimento"].DBToDateTime();
                    RendaFixa.Taxa              = dr["Taxa"].DBToDecimal();
                    RendaFixa.Quantidade        = dr["Quantidade"].DBToDecimal();
                    RendaFixa.ValorOriginal     = dr["ValorOriginal"].DBToDecimal();
                    RendaFixa.SaldoBruto        = dr["SaldoBruto"].DBToDecimal();
                    RendaFixa.IRRF              = dr["IRRF"].DBToDecimal();
                    RendaFixa.IOF               = dr["IOF"].DBToDecimal();
                    RendaFixa.SaldoLiquido      = dr["SaldoLiquido"].DBToDecimal();

                    lRetorno.Add(RendaFixa);
                }
            }

            return lRetorno;
        }

        #endregion
    }
}
