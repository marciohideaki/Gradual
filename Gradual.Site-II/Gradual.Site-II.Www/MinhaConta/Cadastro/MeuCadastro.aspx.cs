using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Email.Lib;
using System.IO;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Seguranca.Lib;
using System.Globalization;
using Gradual.Servico.FichaCadastral.Lib;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados.Portal;
using Gradual.Site.Www.Transporte;
using Gradual.Site.DbLib.Mensagens.IntegracaoFundos;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class MeuCadastro : PaginaFundos
    {
        #region Globais

        private Dictionary<int, int[]> gPontuacaoDasRespostas;

        #endregion

        #region Propriedades

        public string LabelProximo1
        {
            get
            {
                if (SessaoClienteLogado != null && SessaoClienteLogado.Passo.HasValue && SessaoClienteLogado.Passo.Value < 4)
                {
                    return "Salvar Dados";
                }

                return "Avançar >";
            }
        }

        public string LabelProximo2
        {
            get
            {
                if (SessaoClienteLogado != null && SessaoClienteLogado.Passo.HasValue && SessaoClienteLogado.Passo.Value >= 2)
                {
                    return "Salvar Dados";
                }

                return "Avançar >";
            }
        }

        public string LabelProximo3
        {
            get
            {
                if (SessaoClienteLogado != null && SessaoClienteLogado.Passo.HasValue && SessaoClienteLogado.Passo.Value >= 3)
                {
                    return "Salvar Dados";
                }

                return "Avançar >";
            }
        }

        public bool ModoDeTeste
        {
            get
            {
                return ConfiguracoesValidadas.AplicacaoEmModoDeTeste;
            }
        }

        #endregion

        #region Métodos Private - Cadastro

        private void CarregarPontuacaoDoQuestionario()
        {
            this.gPontuacaoDasRespostas = new Dictionary<int, int[]>();
            this.gPontuacaoDasRespostas.Add(1 , new int[] { 1, 2, 3, 5      });
            this.gPontuacaoDasRespostas.Add(2 , new int[] { 1, 2, 4, 5      });
            this.gPontuacaoDasRespostas.Add(3 , new int[] { 1, 2, 4, 5      });
            this.gPontuacaoDasRespostas.Add(4 , new int[] { 1, 2, 3, 4, 5   });
            this.gPontuacaoDasRespostas.Add(5 , new int[] { 1, 2, 3, 4, 5   });
            this.gPontuacaoDasRespostas.Add(6 , new int[] { 1, 2, 2, 2      });
            this.gPontuacaoDasRespostas.Add(7 , new int[] { 1, 2, 4, 5, 5   });
            this.gPontuacaoDasRespostas.Add(8 , new int[] { 1, 2, 2, 3, 4   });
            this.gPontuacaoDasRespostas.Add(9 , new int[] { 0, 0, 0, 0, 0   }); //0.25 0.25 0.5 1.5 2.5
            this.gPontuacaoDasRespostas.Add(10, new int[] { 5, 4, 3, 2      });
            this.gPontuacaoDasRespostas.Add(11, new int[] { 5, 4, 3         });
        }

        private string VerificarPontuacaoDoSuitability(TransporteCadastroSuitability pRespostas)
        {
            string lResposta;
            decimal lSomatoria = 0;

            this.CarregarPontuacaoDoQuestionario();

            lSomatoria += gPontuacaoDasRespostas[1] [pRespostas.Resp1];
            lSomatoria += gPontuacaoDasRespostas[2] [pRespostas.Resp2];
            lSomatoria += gPontuacaoDasRespostas[3] [pRespostas.Resp3];
            lSomatoria += gPontuacaoDasRespostas[4] [pRespostas.Resp4];
            lSomatoria += gPontuacaoDasRespostas[5] [pRespostas.Resp5];
            lSomatoria += gPontuacaoDasRespostas[6] [pRespostas.Resp6];
            lSomatoria += gPontuacaoDasRespostas[7] [pRespostas.Resp7];
            lSomatoria += gPontuacaoDasRespostas[8] [pRespostas.Resp8];
            //lSomatoria += gPontuacaoDasRespostas[9] [pRespostas.Resp9];
            lSomatoria += gPontuacaoDasRespostas[10][pRespostas.Resp10];
            lSomatoria += gPontuacaoDasRespostas[11][pRespostas.Resp11];

            String[] lOpcoes = pRespostas.Resp9.Split('|');

            foreach (String lOpcao in lOpcoes)
            {

                if (!String.IsNullOrEmpty(lOpcao))
                {
                    Int32 lQuest = Int32.Parse(lOpcao) - 1;

                    switch (lQuest.ToString())
                    {
                        case "0":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.25;
                            break;
                        case "1":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.25;
                            break;
                        case "2":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.5;
                            break;
                        case "3":
                            lSomatoria = (decimal)lSomatoria + (decimal)1.5;
                            break;
                        case "4":
                            lSomatoria = (decimal)lSomatoria + (decimal)2.5;
                            break;
                    }
                }
            }

            /*
            RESULTADOS
                Conservador  De 0 a 15 pontos
                Moderado     De 15 a 22 pontos
                Arrojado     Acima de 22 pontos
            */

            if (lSomatoria <= 15)
            {
                lResposta = "Conservador";
            }
            else if (lSomatoria > 15 && lSomatoria <= 22)
            {
                lResposta = "Moderado";
            }
            else 
            {
                lResposta = "Arrojado";
            }

            return lResposta;
        }

        private void CarregarAssessorCadastro()
        {
            string lCodigoAcessor = Request["a"];

            if (!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3))
            {
                Session["CodigoAssessorCadastro"] = 18;
                Session["NomeAssessorCadastro"] = "0018 - 18 - PA - PEDRO PAULO - PF - SP";
            }
            else
            {
                Session["CodigoAssessorCadastro"] = 602;
                Session["NomeAssessorCadastro"] = "0602 - 602 - MESA CAMBIO COMERCIAL";
            }

            if (!string.IsNullOrEmpty(lCodigoAcessor))
            {
                foreach (SinacorListaInfo lAcessor in DadosDeAplicacao.Assessores)
                {
                    if (lAcessor.Id == lCodigoAcessor)
                    {
                        Session["CodigoAssessorCadastro"] = lAcessor.Id;
                        Session["NomeAssessorCadastro"] = lAcessor.Value;

                        break;
                    }
                }
            }

            //txtCadastro_PFPasso1_Assessor.Value = Session["CodigoAssessorCadastro"].ToString();
            //lblCadastro_PFPasso1_AssessorInicial.Text = Session["NomeAssessorCadastro"].ToString();
        }

        private string CarregarDados()
        {
            CarregarAssessorCadastro();

            try
            {
                cboCadastro_PFPasso2_Nacionalidade.DataSource = ListaComSelecione(DadosDeAplicacao.Nacionalidades);
                cboCadastro_PFPasso2_Nacionalidade.DataBind();

                cboCadastro_PFPasso2_PaisNascimento.DataSource = ListaComSelecione(DadosDeAplicacao.Paises);
                cboCadastro_PFPasso2_PaisNascimento.DataBind();

                cboCadastro_PFPasso2_PaisNascimento.SelectedValue = "BRA";

                cboCadastro_PFPasso2_EstadoNascimento.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_EstadoNascimento.DataBind();

                cboCadastro_PFPasso2_EstadoEmissao.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_EstadoEmissao.DataBind();

                cboCadastro_PFPasso2_Endereco1_Estado.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_Endereco1_Estado.DataBind();

                cboCadastro_PFPasso2_EstadoCivil.DataSource = ListaComSelecione(DadosDeAplicacao.EstadosCivis);
                cboCadastro_PFPasso2_EstadoCivil.DataBind();

                cboCadastro_PFPasso2_Profissao.DataSource = ListaComSelecione(DadosDeAplicacao.Profissoes, "OUTROS");
                cboCadastro_PFPasso2_Profissao.DataBind();

                cboCadastro_PFPasso2_TipoDocumento.DataSource = ListaComSelecione(DadosDeAplicacao.TiposDeDocumento);
                cboCadastro_PFPasso2_TipoDocumento.DataBind();

                cboCadastro_PFPasso2_OrgaoEmissor.DataSource = ListaComSelecione(DadosDeAplicacao.OrgaosEmissores);
                cboCadastro_PFPasso2_OrgaoEmissor.DataBind();

                cboCadastro_PFPasso2_EstadoEmissao.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_EstadoEmissao.DataBind();

                cboCadastro_PFPasso2_Endereco1_Estado.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_Endereco1_Estado.DataBind();

                cboCadastro_PFPasso2_Endereco1_Pais.DataSource = ListaComSelecione(DadosDeAplicacao.Paises);
                cboCadastro_PFPasso2_Endereco1_Pais.DataBind();

                cboCadastro_PFPasso2_Endereco2_Estado.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso2_Endereco2_Estado.DataBind();

                cboCadastro_PFPasso2_Endereco2_Pais.DataSource = ListaComSelecione(DadosDeAplicacao.Paises);
                cboCadastro_PFPasso2_Endereco2_Pais.DataBind();

                cboCadastro_PFPasso3_ContasBancarias_TipoConta.DataSource = ListaComSelecione(DadosDeAplicacao.TiposDeContaBancaria);
                cboCadastro_PFPasso3_ContasBancarias_TipoConta.DataBind();

                cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.DataSource = ListaComSelecione(DadosDeAplicacao.TiposDeContaBancaria);
                cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.DataBind();

                cboCadastro_PFPasso3_ContasBancarias_Banco.DataSource = ListaComSelecione(DadosDeAplicacao.Bancos);
                cboCadastro_PFPasso3_ContasBancarias_Banco.DataBind();

                cboCadastro_PFPasso3_ContasBancarias_BancoCambio.DataSource = ListaComSelecione(DadosDeAplicacao.Bancos);
                cboCadastro_PFPasso3_ContasBancarias_BancoCambio.DataBind();

                cboCadastro_PFPasso3_RepSituacaoLegal.DataSource = ListaComSelecione(DadosDeAplicacao.SituacoesLegais);
                cboCadastro_PFPasso3_RepSituacaoLegal.DataBind();

                cboCadastro_PFPasso3_RepSituacaoLegalCambio.DataSource = ListaComSelecione(DadosDeAplicacao.SituacoesLegais);
                cboCadastro_PFPasso3_RepSituacaoLegalCambio.DataBind();

                cboCadastro_PFPasso3_RepTipoDocumento.DataSource = ListaComSelecione(DadosDeAplicacao.TiposDeDocumento);
                cboCadastro_PFPasso3_RepTipoDocumento.DataBind();

                cboCadastro_PFPasso3_RepOrgaoEmissor.DataSource = ListaComSelecione(DadosDeAplicacao.OrgaosEmissores);
                cboCadastro_PFPasso3_RepOrgaoEmissor.DataBind();

                cboCadastro_PFPasso3_RepEstadoEmissao.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso3_RepEstadoEmissao.DataBind();

                cboCadastro_PFPasso3_RepTipoDocumentoCambio.DataSource = ListaComSelecione(DadosDeAplicacao.TiposDeDocumento);
                cboCadastro_PFPasso3_RepTipoDocumentoCambio.DataBind();

                cboCadastro_PFPasso3_RepOrgaoEmissorCambio.DataSource = ListaComSelecione(DadosDeAplicacao.OrgaosEmissores);
                cboCadastro_PFPasso3_RepOrgaoEmissorCambio.DataBind();

                cboCadastro_PFPasso3_RepEstadoEmissaoCambio.DataSource = ListaComSelecione(DadosDeAplicacao.Estados);
                cboCadastro_PFPasso3_RepEstadoEmissaoCambio.DataBind();

                PreencherComDadosDoCliente();

                RodarJavascriptOnLoad("GradSite_Cadastro_Iniciar();");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Cadastro_PF_Passo2.aspx > CarregarDados(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }

            return string.Empty;
        }

        private void PreencherComDadosDoCliente()
        {
            TransporteCadastro lDados = new TransporteCadastro(base.SessaoClienteLogado);

            txtCadastro_PFPasso1_NomeCompleto.Value     = lDados.NomeCompleto;
            txtCadastro_PFPasso1_Email.Value            = lDados.Email;
            txtCadastro_PFPasso1_EmailConfirmacao.Value = lDados.Email;

            txtCadastro_PFPasso1_CPF.Value = lDados.CPF;

            txtCadastro_PFPasso1_DataNascimento.Value   = lDados.DataNascimento;
            cboCadastro_PFPasso1_Sexo.Value             = lDados.Sexo[0].ToString();
            cboCadastro_PFPasso1_Conheceu.Value         = lDados.ComoConheceu;

            
            SessaoClienteLogado.PerfilSuitability = lDados.PerfilSuitability;

            if (lDados.ComoConheceu == "ASSESSOR")
            {
                pnlAssessor.Attributes["style"]           = "display:block";
                txtCadastro_PFPasso1_Assessor.Value       = lDados.Assessor;

                lblCadastro_PFPasso1_AssessorInicial.Text = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Assessor, lDados.Assessor);
            }
            else
            {
                pnlAssessor.Attributes["style"] = "display:none";
            }

            if (lDados.Telefones.Count > 0)
            {
                foreach (TransporteCadastroTelefone lTel in lDados.Telefones)
                {
                    if (lTel.IdTipo == 3)
                    {
                        txtCadastro_PFPasso1_Cel_DDD.Value          = lTel.DDD;
                        txtCadastro_PFPasso1_Cel_Numero.Value       = lTel.Numero;
                    }
                    else
                    {
                        txtCadastro_PFPasso1_Tel_DDD.Value          = lTel.DDD;
                        txtCadastro_PFPasso1_Tel_Numero.Value       = lTel.Numero;
                        cboCadastro_PFPasso1_TipoTelefone.Value     = lTel.IdTipo.ToString();
                    }
                }
            }
            
            liDadosPessoais.Attributes["data-desabilitado"] = null;

            //cboCadastro_PFPasso1_Conheceu

            //pnlSenhas.Visible = false;

            if (SessaoClienteLogado.Passo.Value >= 2)
            {
                //quando o cliente está em passo 2 ou maior

                cboCadastro_PFPasso2_Nacionalidade.SelectedValue    = lDados.Nacionalidade;
                cboCadastro_PFPasso2_PaisNascimento.SelectedValue   = lDados.PaisNascimento;

                if (cboCadastro_PFPasso2_PaisNascimento.SelectedValue == "")
                    cboCadastro_PFPasso2_PaisNascimento.SelectedValue = "BRA";

                cboCadastro_PFPasso2_EstadoNascimento.SelectedValue = lDados.EstadoNascimento;
                txtCadastro_PFPasso2_CidadeNascimento.Value         = lDados.CidadeNascimento;
                cboCadastro_PFPasso2_EstadoCivil.SelectedValue      = lDados.EstadoCivil;
                txtCadastro_PFPasso2_Conjuge.Value                  = lDados.Conjuge;
                cboCadastro_PFPasso2_Profissao.SelectedValue        = lDados.Profissao;
                txtCadastro_PFPasso2_CargoFuncao.Value              = lDados.CargoFuncao;
                txtCadastro_PFPasso2_Empresa.Value                  = lDados.Empresa;
                cboCadastro_PFPasso2_TipoDocumento.SelectedValue    = lDados.TipoDocumento;
                txtCadastro_PFPasso2_NumeroDocumento.Value          = lDados.NumeroDocumento;
                cboCadastro_PFPasso2_OrgaoEmissor.SelectedValue     = lDados.OrgaoEmissor;
                cboCadastro_PFPasso2_EstadoEmissao.SelectedValue    = lDados.EstadoEmissao;
                txtCadastro_PFPasso2_DataEmissao.Value              = lDados.DataEmissao;
                txtCadastro_PFPasso2_NomeMae.Value                  = lDados.NomeMae;
                txtCadastro_PFPasso2_NomePai.Value                  = lDados.NomePai;

                if (lDados.Nacionalidade == "3")
                {
                    //estrangeiro
                    pnlPaisDeNascimento.Attributes["style"] = "width:28%; display:block;";

                    txtCadastro_PFPasso2_EstadoNascimento.Value = lDados.EstadoNascimento;
                }
                else
                {
                    pnlPaisDeNascimento.Attributes["style"] = "width:28%; display:none;";
                }

                foreach (TransporteCadastroEndereco lEnd in lDados.Enderecos)
                {
                    if (lEnd.TipoEndereco == "2")
                    {
                        //residencial
                        txtCadastro_PFPasso2_Endereco1_CEP.Value            = lEnd.CEP;
                        txtCadastro_PFPasso2_Endereco1_Logradouro.Value     = lEnd.Logradouro;
                        txtCadastro_PFPasso2_Endereco1_Numero.Value         = lEnd.Numero;
                        txtCadastro_PFPasso2_Endereco1_Complemento.Value    = lEnd.Complemento;
                        txtCadastro_PFPasso2_Endereco1_Bairro.Value         = lEnd.Bairro;
                        txtCadastro_PFPasso2_Endereco1_Cidade.Value         = lEnd.Cidade;
                        cboCadastro_PFPasso2_Endereco1_Estado.SelectedValue = lEnd.Estado;
                        cboCadastro_PFPasso2_Endereco1_Pais.SelectedValue   = lEnd.Pais;

                        hidCadastro_PFPasso2_IdEndereco1.Value = lEnd.IdEndereco.Value.ToString();
                    }
                    else
                    {
                        //comercial/outro
                        txtCadastro_PFPasso2_Endereco2_CEP.Value            = lEnd.CEP;
                        txtCadastro_PFPasso2_Endereco2_Logradouro.Value     = lEnd.Logradouro;
                        txtCadastro_PFPasso2_Endereco2_Numero.Value         = lEnd.Numero;
                        txtCadastro_PFPasso2_Endereco2_Complemento.Value    = lEnd.Complemento;
                        txtCadastro_PFPasso2_Endereco2_Bairro.Value         = lEnd.Bairro;
                        txtCadastro_PFPasso2_Endereco2_Cidade.Value         = lEnd.Cidade;
                        cboCadastro_PFPasso2_Endereco2_Estado.SelectedValue = lEnd.Estado;
                        cboCadastro_PFPasso2_Endereco2_Pais.SelectedValue   = lEnd.Pais;

                        hidCadastro_PFPasso2_IdEndereco2.Value = lEnd.IdEndereco.Value.ToString();

                        if (lEnd.TipoEndereco == "1")
                        {
                            //comercial
                            rdoCadastro_PFPasso2_End2.Checked = true;
                            pnlSegundoEndereco.Visible = true;
                            pnlSegundoEndereco.Attributes["style"] = "float:left;";
                        }
                        else
                        {
                            //outro
                            rdoCadastro_PFPasso2_End3.Checked = true;
                            pnlSegundoEndereco.Visible = true;
                            pnlSegundoEndereco.Attributes["style"] = "float:left;"; //tirando o display:none
                        }
                    }

                    // se tiver mais de um, paciência...
                }
                
                liDadosFinanceiros.Attributes["data-desabilitado"] = null;
                liDadosFinanceiros.Attributes["class"] = null;

                RodarJavascriptOnLoad(" $('#ContentPlaceHolder1_liDadosFinanceiros a').click(); ");
            }

            if (SessaoClienteLogado.Passo.Value >= 3)
            {
                //quando o cliente está em passo 3 ou maior

                txtCadastro_PFPasso3_SalarioProlabore.Value = lDados.SituacaoFinanceira.VlTotalSalarioProLabore;
                txtCadastro_PFPasso3_ValorTotalEmAplicacoesFinanceiras.Value = lDados.SituacaoFinanceira.VlTotalAplicacaoFinanceira;
                txtCadastro_PFPasso3_OutrosRendimentos.Value = lDados.SituacaoFinanceira.VlTotalOutrosRendimentos;
                txtCadastro_PFPasso3_OutrosRendimentosDesc.Value = lDados.SituacaoFinanceira.VlTotalOutrosRendimentosDesc;
                txtCadastro_PFPasso3_TotalEmBensImoveis.Value = lDados.SituacaoFinanceira.VlTotalBensImoveis;
                txtCadastro_PFPasso3_TotalEmBensMoveis.Value = lDados.SituacaoFinanceira.VlTotalBensMoveis;

                hidCadastro_PFPasso3_ContasBancarias.Value = JsonConvert.SerializeObject(lDados.Contas);

                rptContasBancarias.Visible = true;
                rptContasBancarias.DataSource = lDados.Contas;
                rptContasBancarias.DataBind();

                trNenhumItem.Visible = false;

                if (lDados.OperaPorContaPropria == "Sim")
                {
                    rdoCadastro_PFPasso3_OperaContaPropria_Sim.Checked = true;
                    rdoCadastro_PFPasso3_OperaContaPropria_Nao.Checked = false;

                    pnlDadosClienteOpera.Attributes["style"] = "display:none";

                    txtCadastro_PFPasso3_CliNomeCompleto.Attributes["class"] = "EstiloCampoObrigatorio";
                    txtCadastro_PFPasso3_CliCPF.Attributes["class"] = "ProibirLetras EstiloCampoObrigatorio";
                }
                else
                {
                    rdoCadastro_PFPasso3_OperaContaPropria_Sim.Checked = false;
                    rdoCadastro_PFPasso3_OperaContaPropria_Nao.Checked = true;

                    pnlDadosClienteOpera.Attributes["style"] = "display:block";

                    txtCadastro_PFPasso3_CliNomeCompleto.Value = lDados.NomeCliente;
                    txtCadastro_PFPasso3_CliCPF.Value = lDados.CPFCliente;

                    txtCadastro_PFPasso3_CliNomeCompleto.Attributes["class"] = "EstiloCampoObrigatorio validate[required]";
                    txtCadastro_PFPasso3_CliCPF.Attributes["class"] = "ProibirLetras EstiloCampoObrigatorio validate[required,custom[validatecpfcnpj]]";
                }

                if (lDados.PessoaPoliticamenteExposta == "Sim")
                {
                    rdoCadastro_PFPasso3_PPE_Sim.Checked = true;
                    rdoCadastro_PFPasso3_PPE_Nao.Checked = false;
                }
                else
                {
                    rdoCadastro_PFPasso3_PPE_Sim.Checked = false;
                    rdoCadastro_PFPasso3_PPE_Nao.Checked = true;
                }

                if (lDados.PessoaVinculada == "1")
                {
                    rdoCadastro_PFPasso3_PessoaVinculada_Sim.Checked = true;
                    
                    rdoCadastro_PFPasso3_PessoaVinculada_SimG.Checked = false;
                    rdoCadastro_PFPasso3_PessoaVinculada_Nao.Checked = false;
                }
                else if (lDados.PessoaVinculada == "2")
                {
                    rdoCadastro_PFPasso3_PessoaVinculada_SimG.Checked = true;
                    
                    rdoCadastro_PFPasso3_PessoaVinculada_Sim.Checked = false;
                    rdoCadastro_PFPasso3_PessoaVinculada_Nao.Checked = false;
                }
                else
                {
                    rdoCadastro_PFPasso3_PessoaVinculada_Nao.Checked = true;
                    
                    rdoCadastro_PFPasso3_PessoaVinculada_Sim.Checked = false;
                    rdoCadastro_PFPasso3_PessoaVinculada_SimG.Checked = false;
                }

                if (lDados.Procurador == "Sim" && lDados.Representante != null)
                {
                    rdoCadastro_PFPasso3_Procurador_Sim.Checked = true;
                    rdoCadastro_PFPasso3_Procurador_Nao.Checked = false;

                    pnlDadosRepresentanteLegal.Attributes["style"] = "display:block";

                    cboCadastro_PFPasso3_RepSituacaoLegal.SelectedValue = lDados.Representante.SituacaoLegal;
                    txtCadastro_PFPasso3_RepNomeCompleto.Value = lDados.Representante.Nome;
                    txtCadastro_PFPasso3_RepCPF.Value = lDados.Representante.CPF;
                    txtCadastro_PFPasso3_RepDataNascimento.Value = lDados.Representante.DataNascimento;
                    cboCadastro_PFPasso3_RepTipoDocumento.SelectedValue = lDados.Representante.TipoDocumento;
                    cboCadastro_PFPasso3_RepOrgaoEmissor.SelectedValue = lDados.Representante.OrgaoEmissor;
                    txtCadastro_PFPasso3_RepNumeroDocumento.Value = lDados.Representante.NumeroDocumento;
                    cboCadastro_PFPasso3_RepEstadoEmissao.SelectedValue = lDados.Representante.EstadoEmissor;
                    
                    cboCadastro_PFPasso3_RepSituacaoLegal.Attributes["class"]   = "EstiloCampoObrigatorio validate[required]";
                    txtCadastro_PFPasso3_RepNomeCompleto.Attributes["class"]    = "EstiloCampoObrigatorio validate[required]";
                    txtCadastro_PFPasso3_RepCPF.Attributes["class"]             = "EstiloCampoObrigatorio Mascara_CPF ProibirLetras validate[required]";
                    txtCadastro_PFPasso3_RepDataNascimento.Attributes["class"]  = "EstiloCampoObrigatorio validate[required]";
                    cboCadastro_PFPasso3_RepTipoDocumento.Attributes["class"]   = "EstiloCampoObrigatorio validate[required]";
                    cboCadastro_PFPasso3_RepOrgaoEmissor.Attributes["class"]    = "EstiloCampoObrigatorio validate[required]";
                    txtCadastro_PFPasso3_RepNumeroDocumento.Attributes["class"] = "EstiloCampoObrigatorio validate[required]";
                    cboCadastro_PFPasso3_RepEstadoEmissao.Attributes["class"]   = "EstiloCampoObrigatorio validate[required]";
                }
                else
                {
                    rdoCadastro_PFPasso3_Procurador_Sim.Checked = false;
                    rdoCadastro_PFPasso3_Procurador_Nao.Checked = true;

                    pnlDadosRepresentanteLegal.Attributes["style"] = "display:none";
                }

                if (lDados.USPerson == "Sim")
                {
                    rdoCadastro_PFPasso3_USPerson_Sim.Checked = true;
                    rdoCadastro_PFPasso3_USPerson_Nao.Checked = false;
                }
                else if (lDados.USPerson == "Não")
                {
                    rdoCadastro_PFPasso3_USPerson_Sim.Checked = false;
                    rdoCadastro_PFPasso3_USPerson_Nao.Checked = true;
                }
                else
                {
                    rdoCadastro_PFPasso3_USPerson_Sim.Checked = false;
                    rdoCadastro_PFPasso3_USPerson_Nao.Checked = false;
                }

                chkCadastro_PFPasso3_Ciente_Doc_Prospecto.Checked = (lDados.CienteProspecto == "Sim");

                chkCadastro_PFPasso3_Ciente_Doc_Regulamento.Checked = (lDados.CienteRegulamento == "Sim");
                
                chkCadastro_PFPasso3_Ciente_Doc_Lamina.Checked = (lDados.CienteLamina == "Sim");

                txtCadastro_PFPasso3_Proposito.Value = lDados.PropositoGradual;

                if (!string.IsNullOrEmpty(lDados.PerfilSuitability) && lDados.PerfilSuitability != "n/d")
                {
                    hidCadastro_PFPasso3_JaPreencheuSuit.Value = "true";

                    pnlFormPerfil.Attributes["style"] = "display:none";

                    List<Transporte_IntegracaoFundos> lListaFundos = base.PesquisarFundosSuitability(new PesquisarIntegracaoFundosRequest() 
                    {
                        IdPerfilSuitability = base.GetIdPerfilSuitability
                    });

                    FiltrarFundosProibidos(ref lListaFundos);

                    //rptFundosParaAplicar.DataSource = lListaFundos;
                    //rptFundosParaAplicar.DataBind();

                    /*
                    pnlImagemPerfil.Attributes["style"] = "display:block";

                    if (lDados.PerfilSuitability == "Conservador")
                    {
                        divResultado_Conservador.Attributes["style"] = "";
                    }
                    else if (lDados.PerfilSuitability == "Moderado")
                    {
                        divResultado_Moderado.Attributes["style"] = "";
                    }
                    else
                    {
                        divResultado_Arrojado.Attributes["style"] = "";
                    }*/
                }
                else
                {
                    pnlFormPerfil.Attributes["style"] = "display:block";
                    //pnlImagemPerfil.Attributes["style"] = "display:none";
                }

                liDadosContratuais.Attributes["class"] = null;
                liDadosContratuais.Attributes["data-desabilitado"] = null;

                //lnkCadastro_P4.Attributes["class"] = "Habilitado";

            }

            if (SessaoClienteLogado.Passo.Value == 4)
            {
                //quando o cliente está em passo 4
                //mostra toda a tela desabilitada, tira a parte do suitability de dentro do fluxo do cadastro, mostra a aba "meu perfil"

                txtCadastro_PFPasso1_Assessor.Disabled = true;
                txtCadastro_PFPasso1_Cel_DDD.Disabled = true;
                txtCadastro_PFPasso1_Cel_Numero.Disabled = true;
                txtCadastro_PFPasso1_CPF.Disabled = true;
                txtCadastro_PFPasso1_DataNascimento.Disabled = true;
                txtCadastro_PFPasso1_Email.Disabled = true;
                txtCadastro_PFPasso1_EmailConfirmacao.Disabled = true;
                txtCadastro_PFPasso1_NomeCompleto.Disabled = true;
                txtCadastro_PFPasso1_Tel_DDD.Disabled = true;
                txtCadastro_PFPasso1_Tel_Numero.Disabled = true;
                txtCadastro_PFPasso2_CargoFuncao.Disabled = true;
                txtCadastro_PFPasso2_CargoFuncao.Disabled = true;
                txtCadastro_PFPasso2_CidadeNascimento.Disabled = true;
                txtCadastro_PFPasso2_Conjuge.Disabled = true;
                txtCadastro_PFPasso2_DataEmissao.Disabled = true;
                txtCadastro_PFPasso2_Empresa.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_Bairro.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_CEP.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_Cidade.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_Complemento.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_Logradouro.Disabled = true;
                txtCadastro_PFPasso2_Endereco1_Numero.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_Bairro.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_CEP.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_Cidade.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_Complemento.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_Logradouro.Disabled = true;
                txtCadastro_PFPasso2_Endereco2_Numero.Disabled = true;
                txtCadastro_PFPasso2_EstadoNascimento.Disabled = true;
                txtCadastro_PFPasso2_NomeMae.Disabled = true;
                txtCadastro_PFPasso2_NomePai.Disabled = true;
                txtCadastro_PFPasso2_NumeroDocumento.Disabled = true;
                txtCadastro_PFPasso3_CliCPF.Disabled = true;
                txtCadastro_PFPasso3_CliNomeCompleto.Disabled = true;
                txtCadastro_PFPasso3_OutrosRendimentos.Disabled = true;
                txtCadastro_PFPasso3_OutrosRendimentosDesc.Disabled = true;
                txtCadastro_PFPasso3_RepCPF.Disabled = true;
                txtCadastro_PFPasso3_RepDataNascimento.Disabled = true;
                txtCadastro_PFPasso3_RepNomeCompleto.Disabled = true;
                txtCadastro_PFPasso3_RepNumeroDocumento.Disabled = true;
                txtCadastro_PFPasso3_SalarioProlabore.Disabled = true;
                txtCadastro_PFPasso3_TotalEmBensImoveis.Disabled = true;
                txtCadastro_PFPasso3_TotalEmBensMoveis.Disabled = true;
                txtCadastro_PFPasso3_ValorTotalEmAplicacoesFinanceiras.Disabled = true;
                
                cboCadastro_PFPasso1_Conheceu.Disabled = true;
                cboCadastro_PFPasso1_Sexo.Disabled = true;
                cboCadastro_PFPasso1_TipoTelefone.Disabled = true;
                cboCadastro_PFPasso2_Endereco1_Estado.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_Endereco1_Pais.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_Endereco2_Estado.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_Endereco2_Pais.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_EstadoCivil.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_EstadoEmissao.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_EstadoNascimento.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_Nacionalidade.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_OrgaoEmissor.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_PaisNascimento.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_Profissao.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso2_TipoDocumento.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_ContasBancarias_Banco.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_ContasBancarias_TipoConta.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_RepEstadoEmissao.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_RepOrgaoEmissor.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_RepSituacaoLegal.Attributes["disabled"] = "disabled";
                cboCadastro_PFPasso3_RepTipoDocumento.Attributes["disabled"] = "disabled";
                
                rdoCadastro_PFPasso2_End1.Disabled = true;
                rdoCadastro_PFPasso2_End2.Disabled = true;
                rdoCadastro_PFPasso2_End3.Disabled = true;
                rdoCadastro_PFPasso3_Emancipado_Nao.Disabled = true;
                rdoCadastro_PFPasso3_Emancipado_Sim.Disabled = true;
                rdoCadastro_PFPasso3_OperaContaPropria_Nao.Disabled = true;
                rdoCadastro_PFPasso3_OperaContaPropria_Sim.Disabled = true;
                rdoCadastro_PFPasso3_PessoaVinculada_Nao.Disabled = true;
                rdoCadastro_PFPasso3_PessoaVinculada_Sim.Disabled = true;
                rdoCadastro_PFPasso3_PessoaVinculada_SimG.Disabled = true;
                rdoCadastro_PFPasso3_PPE_Nao.Disabled = true;
                rdoCadastro_PFPasso3_PPE_Sim.Disabled = true;
                rdoCadastro_PFPasso3_Procurador_Nao.Disabled = true;
                rdoCadastro_PFPasso3_Procurador_Sim.Disabled = true;

                pnlFormNovaContaBancaria.Visible = false;

                pnlFormPerfil.Visible = false;

                pnlBotaoSalvarDadosPasso1.Visible = 
                pnlBotaoSalvarDadosPasso2.Visible = 
                pnlBotaoSalvarDadosPasso3.Visible = false;

                pnlBotaoAlterarDadosPasso1.Visible = 
                pnlBotaoAlterarDadosPasso2.Visible = 
                pnlBotaoAlterarDadosPasso3.Visible = true;

                //pnlEscolherAplicacao.Visible = false;

                pnlContratos.Attributes["style"] = "";

                //quando o cliente está em passo 4
                pnlRodapePasso4.Visible = false;
                pnlBotaoFinalizar.Visible = false;
            }
        }

        private MensagemResponseStatusEnum EnviarEmailCadastralParaCliente(TransporteCadastro pDados)
        {
            //if (ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            //    return MensagemResponseStatusEnum.OK;

            string lCorpoEmail = string.Empty;
            string lRemetente = ConfiguracoesValidadas.Email_Atendimento;

            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            lVariaveis.Add("###NOME###",        pDados.NomeCompleto);
            lVariaveis.Add("###LOGIN###",       pDados.Email);
            lVariaveis.Add("###SENHA###",       pDados.Senha);
            lVariaveis.Add("###ASSINATURA###",  pDados.AssEletronica);

            lCorpoEmail = "CadastroPasso1Completo_Padrao.html";

            return base.EnviarEmail(pDados.Email, "Bem-vindo à Gradual", lCorpoEmail, lVariaveis, eTipoEmailDisparo.Todos);
        }

        private void EnviarEmailComPerfilDoInvestidor()
        {
            string lNomeArquivoEmail = string.Empty;

            List<EmailAnexoInfo> lAnexos = new List<EmailAnexoInfo>();

            string lFundos = (SessaoClienteLogado.DesejaAplicar == "FUNDOS") ? "Fundos-" : "";

            switch (SessaoClienteLogado.PerfilSuitability)
            {
                case "Arrojado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-{0}Arrojado.html", lFundos);
                    break;

                case "Conservador":
                    lNomeArquivoEmail = string.Format("EmailSuitability-{0}Conservador.html", lFundos);
                    break;

                case "Moderado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-{0}Moderado.html", lFundos);
                    break;
            }

            List<string> lDestinatarioOculto = new List<string>();

            //lDestinatarioOculto.Add(ConfiguracoesValidadas.Email_CopiaDeEnvioDoCadastro);

            if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            {
                gLogger.ErrorFormat("Chamada para email de suitability para [{0}], tipo [{1}], arquivo [{2}]", SessaoClienteLogado.Email, SessaoClienteLogado.PerfilSuitability, lNomeArquivoEmail);
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Perfil do Investidor | Confira o seu portfólio recomendado", lNomeArquivoEmail, new Dictionary<string, string>(), Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, null, lDestinatarioOculto);
            }
        }

        private void EnviarEmailPasso4()
        {
            string lCorpoEmail = string.Empty;

            List<EmailAnexoInfo> lAnexos = new List<EmailAnexoInfo>();
            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
            
            lVariaveis.Add("###NOME###", base.SessaoClienteLogado.Nome);
            lVariaveis.Add("###LOGIN###", base.SessaoClienteLogado.Email);
            //lVariaveis.Add("###linkDocumentacao###", "http://www.gradualinvestimentos.com.br");
            
            string lCaminho1, lCaminho2, lCaminho3;
            lCaminho1 = String.Empty;
            lCaminho2 = String.Empty;
            lCaminho3 = String.Empty;

            if (!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3))
            {
                lCaminho1 = string.Concat(ConfiguracoesValidadas.PathVirtualPortal, Session["ArquivoFicha"].ToString());
                
                lAnexos.Add(new EmailAnexoInfo()
                {
                    Nome = "Contrato.pdf",
                    Arquivo = File.ReadAllBytes(lCaminho1)
                });
                
                if (!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(2) && !this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3) && !this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(7))
                {
                    lCaminho2 = string.Concat(ConfiguracoesValidadas.PathVirtualPortal, Session["ArquivoTermo"].ToString());

                    lAnexos.Add(new EmailAnexoInfo()
                    {
                        Nome = "Termo de Adesão.pdf",
                        Arquivo = File.ReadAllBytes(lCaminho2)
                    });
                }

                lCaminho3 = String.Empty;

                if (this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(4) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(6) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(7))
                {
                    lCaminho3 = string.Concat(ConfiguracoesValidadas.PathVirtualPortal, Session["ArquivoFichaCambio"].ToString());

                    lAnexos.Add(new EmailAnexoInfo()
                    {
                        Nome = "ContratoCambio.pdf",
                        Arquivo = File.ReadAllBytes(lCaminho3)
                    });
                }


                //if (this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(7) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(5) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(2))
                //{
                //    lCaminho3 = String.Empty;
                //    lCaminho3 = Server.MapPath(Session["ArquivoTermoFundo"].ToString());

                //    lAnexos.Add(new EmailAnexoInfo()
                //    {
                //        Nome = "Termo de Adesão ao Fundo.pdf",
                //        Arquivo = File.ReadAllBytes(lCaminho3)
                //    });
                //}

                gLogger.InfoFormat("Passo 4: Anexando [{0}] [{1}] [{2}]", lCaminho1, lCaminho2, lCaminho3);
            }
            else
            {
                if (this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(4) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(6) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(7))
                {
                    lCaminho3 = String.Empty;
                    lCaminho3 = string.Concat(ConfiguracoesValidadas.PathVirtualPortal, Session["ArquivoFichaCambio"].ToString());

                    lAnexos.Add(new EmailAnexoInfo()
                    {
                        Nome = "ContratoCambio.pdf",
                        Arquivo = File.ReadAllBytes(lCaminho3)
                    });
                }
            }

            lCorpoEmail = "CadastroPasso3ConclusaoCadastro.html";

            //if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            //{
                base.EnviarEmail(base.SessaoClienteLogado.Email, "Cadastro Gradual", lCorpoEmail, lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos);
            //}

                try
                {
                    if (this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(4) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(6) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(7))
                    {
                        string lCaminho = string.Concat(ConfiguracoesValidadas.PathVirtualPortal, Session["ArquivoFichaCambio"].ToString());
                        List<EmailAnexoInfo> lAnexo = new List<EmailAnexoInfo>();
                        List<ClienteTelefoneInfo> lTelefones = SessaoClienteLogado.BuscarTelefones();
                        String lListaTelefones = String.Empty;

                        if (lTelefones.Count > 0)
                        {
                            foreach (ClienteTelefoneInfo lTelefone in lTelefones)
                            {
                                lListaTelefones += String.Format("\r\n({0}){1}", lTelefone.DsDdd, lTelefone.DsNumero);
                            }

                            lVariaveis.Add("###TELEFONES###", lListaTelefones);
                        }
                            
                        lAnexo.Add(new EmailAnexoInfo()
                        {
                            Nome = "ContratoCambio.pdf",
                            Arquivo = File.ReadAllBytes(lCaminho)
                        });

                        base.EnviarEmail(ConfiguracoesValidadas.Email_CadastroCambio, "Cadastro Gradual - Novo cliente cadastrado", "Cadastro-ConclusaoCadastroCambio.html", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexo);
                    }
                }
                catch (Exception ex)
                {
                    gLogger.ErrorFormat("Erro em EnviarEmailPasso4: [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
                }
        }


        private ClienteInfo BuscarDadosDoCliente()
        {
            ReceberEntidadeCadastroRequest<ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>();
            ReceberEntidadeCadastroResponse<ClienteInfo> lResponse;

            lRequest.IdUsuarioLogado        = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;
            lRequest.EntidadeCadastro       = new ClienteInfo() { IdCliente = base.SessaoClienteLogado.IdCliente };

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(IdUsuarioLogado: [{0}], DescricaoUsuarioLogado: [{1}], EntidadeCadastro: [{2}]) em Cadastro_PF_Passo3.aspx > RecuperarDadosClienteInfo > [{3}]\r\n{4}"
                                    , lRequest.IdUsuarioLogado
                                    , lRequest.DescricaoUsuarioLogado
                                    , lRequest.EntidadeCadastro.IdCliente
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }

            return lResponse.EntidadeCadastro;
        }

        private string SalvarPasso1DoCliente(TransporteCadastro pDados)
        {
            string lRetorno;

            SalvarEntidadeCadastroRequest<Passo1Info> lRequest = new SalvarEntidadeCadastroRequest<Passo1Info>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = pDados.ToPasso1Info();

            lRequest.EntidadeCadastro.IdAssessorInicial =  Convert.ToInt32(Session["CodigoAssessorCadastro"]);

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<Passo1Info>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                MensagemResponseStatusEnum lResponseEnvioEmail = this.EnviarEmailCadastralParaCliente(pDados);

                SessaoClienteLogado = new TransporteSessaoClienteLogado();

                SessaoClienteLogado.Senha = Criptografia.CalculateMD5Hash(lRequest.EntidadeCadastro.CdSenha);
                SessaoClienteLogado.Email = lRequest.EntidadeCadastro.DsEmail;

                gLogger.InfoFormat("Cadastro Passo 1: Carregando cliente em sessão [{0}]", SessaoClienteLogado.Email);

                base.CarregarClienteEmSessao(lResponse.DescricaoResposta);

                gLogger.InfoFormat("Cadastro Passo 1: Buscando código de sessão para [{0}]", SessaoClienteLogado.Email);

                base.BuscarCodigoDeSessaoParaUsuarioLogado();

                gLogger.InfoFormat("Cadastro Passo 1: Dados salvos com sucesso para [{0}]", SessaoClienteLogado.Email);

                lRetorno = base.RetornarSucessoAjax("Dados salvos com sucesso.");
            }
            else
            {
                if (lResponse.DescricaoResposta.ToLower().Contains("e-mail já cadastrado"))
                {
                    lRetorno = base.RetornarSucessoAjax("erro: email já cadastrado");
                }
                else if (lResponse.DescricaoResposta.ToLower().Contains("cpf/cnpj já cadastrado"))
                {
                    lRetorno = base.RetornarSucessoAjax("erro: cpf já cadastrado");
                }
                else
                {
                    lRetorno = base.RetornarErroAjax("Erro de retorno do serviço em ResponderSalvarDados():\r\n{0}\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta);
                }
            }

            return lRetorno;
        }

        private void SalvarDadosDoCliente(TransporteCadastro pDados, byte pPasso)
        {
            ReceberEntidadeCadastroRequest<ClienteInfo>  lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>();
            ReceberEntidadeCadastroResponse<ClienteInfo> lResponse;

            ClienteInfo lDadosPraSalvar;
            
            lRequest.IdUsuarioLogado = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;
            lRequest.EntidadeCadastro = new ClienteInfo() { IdCliente = base.SessaoClienteLogado.IdCliente };

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                SalvarEntidadeCadastroRequest<ClienteInfo> lRequestSalvarCliente = new SalvarEntidadeCadastroRequest<ClienteInfo>();
                SalvarEntidadeCadastroResponse lResponseSalvarCliente;

                lDadosPraSalvar = lResponse.EntidadeCadastro;

                if (pPasso == 1)
                {
                    pDados.TransferirClienteInfoPasso1(ref lDadosPraSalvar);

                    pDados.ReavaliarTelefones();
                }

                if (pPasso == 2)
                {
                    if (SessaoClienteLogado.Passo < 2)  // ao editar os dados não pode voltar atrás o passo
                    {
                        lDadosPraSalvar.StPasso = 2;
                        lDadosPraSalvar.DtPasso2 = DateTime.Now;
                    }

                    pDados.TransferirClienteInfoPasso2(ref lDadosPraSalvar);

                }

                lRequestSalvarCliente.IdUsuarioLogado        = this.SessaoClienteLogado.IdLogin;
                lRequestSalvarCliente.DescricaoUsuarioLogado = this.SessaoClienteLogado.Nome;
                lRequestSalvarCliente.EntidadeCadastro       = lDadosPraSalvar;

                if (lRequestSalvarCliente.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria != null)
                {
                    if (lRequestSalvarCliente.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.IdClienteNaoOperaPorContaPropria == 0 && string.IsNullOrEmpty(lRequestSalvarCliente.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado))
                    {
                        lRequestSalvarCliente.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria = null;
                    }
                }

                lResponseSalvarCliente = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(lRequestSalvarCliente);

                if (lResponseSalvarCliente.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarDadosDoCliente() -> SalvarEntidadeCadastro<ClienteInfo>(lRequestSalvarCliente) [{0}]\r\n[{1}]"
                                        , lResponseSalvarCliente.StatusResposta
                                        , lResponseSalvarCliente.DescricaoResposta);

                    throw new Exception(lResponseSalvarCliente.DescricaoResposta);
                }

                if (pPasso == 2 && SessaoClienteLogado.Passo < 2)
                {
                    base.SessaoClienteLogado.Passo = 2;
                }
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarDadosDoCliente() -> ReceberEntidadeCadastro<ClienteInfo>(lRequest) [{0}]\r\n[{1}]"
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void SalvarEnderecosDoCliente(TransporteCadastro pDados)
        {
            SalvarEntidadeCadastroRequest<ClienteEnderecoInfo> lRequest;
            SalvarEntidadeCadastroResponse lResponse;

            ClienteEnderecoInfo lEndereco;

            List<ClienteEnderecoInfo> lEnderecosExistentes = SessaoClienteLogado.BuscarEnderecos();

            List<int> lIdsExistentes = new List<int>();

            foreach (TransporteCadastroEndereco lEnd in pDados.Enderecos)
            {
                lEndereco = lEnd.ToEnderecoInfo();

                if (lEnd.IdEndereco.HasValue)
                {
                    lIdsExistentes.Add(lEnd.IdEndereco.Value);
                }

                lEndereco.IdCliente = SessaoClienteLogado.IdCliente.Value;

                lRequest = new SalvarEntidadeCadastroRequest<ClienteEnderecoInfo>();

                lRequest.IdUsuarioLogado        = base.SessaoClienteLogado.IdLogin;
                lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;
                lRequest.EntidadeCadastro       = lEndereco;

                lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarEnderecosDoCliente(): [{0}]\r\n[{1}]\r\n{2}"
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta
                                        , JsonConvert.SerializeObject(lEnd));

                    throw new Exception(lResponse.DescricaoResposta);
                }
            }

            RemoverEntidadeCadastroRequest<ClienteEnderecoInfo> lRequestExclusao = new RemoverEntidadeCadastroRequest<ClienteEnderecoInfo>();
            RemoverEntidadeCadastroResponse lResponseExclusao;

            foreach (ClienteEnderecoInfo lInfo in lEnderecosExistentes)
            {
                if (!lIdsExistentes.Contains(lInfo.IdEndereco.Value))
                {
                    //remove
                    lRequestExclusao.IdUsuarioLogado = SessaoClienteLogado.IdCliente.Value;
                    lRequestExclusao.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;
                    lRequestExclusao.EntidadeCadastro = new ClienteEnderecoInfo() { IdEndereco = lInfo.IdEndereco.Value };

                    lResponseExclusao = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteEnderecoInfo>(lRequestExclusao);

                    if (lResponseExclusao.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarEnderecosDoCliente({2}) > Exclusao: [{0}]\r\n[{1}]"
                                            , lResponseExclusao.StatusResposta
                                            , lResponseExclusao.DescricaoResposta
                                            , lInfo.IdEndereco);
                    }
                }
            }
        }

        private void SalvarTelefonesDoCliente(TransporteCadastro pDados)
        {
            SalvarEntidadeCadastroRequest<ClienteTelefoneInfo> lRequest;
            SalvarEntidadeCadastroResponse lResponse;

            ClienteTelefoneInfo lTelefone;
            
            List<ClienteTelefoneInfo> lTelefonesExistentes = SessaoClienteLogado.BuscarTelefones();

            List<int> lIdsExistentes = new List<int>();

            foreach (TransporteCadastroTelefone lTel in pDados.Telefones)
            {
                lTelefone = lTel.ToClienteTelefoneInfo();
                
                if (lTel.IdTelefone.HasValue)
                {
                    lIdsExistentes.Add(lTel.IdTelefone.Value);
                }

                lTelefone.IdCliente = SessaoClienteLogado.IdCliente.Value;

                lRequest = new SalvarEntidadeCadastroRequest<ClienteTelefoneInfo>();

                lRequest.IdUsuarioLogado = SessaoClienteLogado.IdLogin;
                lRequest.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;

                lRequest.EntidadeCadastro = lTelefone;

                lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteTelefoneInfo>(lRequest);

                if(lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarDadosTelefonicos(): [{0}]\r\n[{1}]"
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    throw new Exception(lResponse.DescricaoResposta);
                }
            }
            
            RemoverEntidadeCadastroRequest<ClienteTelefoneInfo> lRequestExclusao = new RemoverEntidadeCadastroRequest<ClienteTelefoneInfo>();
            RemoverEntidadeCadastroResponse lResponseExclusao;

            foreach (ClienteTelefoneInfo lInfo in lTelefonesExistentes)
            {
                if (!lIdsExistentes.Contains(lInfo.IdTelefone.Value))
                {
                    //remove
                    lRequestExclusao.IdUsuarioLogado = SessaoClienteLogado.IdCliente.Value;
                    lRequestExclusao.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;
                    lRequestExclusao.EntidadeCadastro = new ClienteTelefoneInfo() { IdTelefone = lInfo.IdTelefone.Value };

                    lResponseExclusao = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteTelefoneInfo>(lRequestExclusao);

                    if (lResponseExclusao.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarTelefonesDoCliente({2}) > Exclusao: [{0}]\r\n[{1}]"
                                            , lResponseExclusao.StatusResposta
                                            , lResponseExclusao.DescricaoResposta
                                            , lInfo.IdTelefone);
                    }
                }
            }
        }

        private void SalvarSituacaoFinanceiraPatrimonial(TransporteCadastro pDados)
        {
            ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lRequestSit = new ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
            ReceberEntidadeCadastroResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lResponseSit;

            lRequestSit.EntidadeCadastro = new ClienteSituacaoFinanceiraPatrimonialInfo();

            lRequestSit.EntidadeCadastro.IdCliente = SessaoClienteLogado.IdCliente.Value;

            lResponseSit = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(lRequestSit);

            if (lResponseSit.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                ClienteSituacaoFinanceiraPatrimonialInfo lInfo;

                if (lResponseSit.EntidadeCadastro != null && lResponseSit.EntidadeCadastro.IdSituacaoFinanceiraPatrimonial.HasValue)
                {
                    lInfo = lResponseSit.EntidadeCadastro;

                    pDados.SituacaoFinanceira.TransferirClienteSituacaoFinanceiraPatrimonialInfo(ref lInfo);

                    lInfo.DtAtualizacao = DateTime.Now;
                }
                else
                {
                    lInfo = pDados.SituacaoFinanceira.ToClienteSituacaoFinanceiraPatrimonialInfo();

                    lInfo.IdCliente = SessaoClienteLogado.IdCliente.DBToInt32();
                    lInfo.DtAtualizacao = DateTime.Now; //data cadastro?
                }

                //TODO: Não sei o que são esses parametros, alguma coisa da intranet... precisa ver depois...
                lInfo.DtCapitalSocial = DateTime.Now;
                lInfo.DtPatrimonioLiquido = DateTime.Now;

                SalvarEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lRequest = new SalvarEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
                SalvarEntidadeCadastroResponse lResponse;

                lRequest.IdUsuarioLogado        = SessaoClienteLogado.IdLogin;
                lRequest.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;
                lRequest.EntidadeCadastro       = lInfo;

                lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarSituacaoFinanceiraPatrimonial(): [{0}]\r\n[{1}]\r\n{2}"
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta
                                        , JsonConvert.SerializeObject(pDados));

                    throw new Exception(lResponse.DescricaoResposta);
                }
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, RequisitarSituacaoFinanceiraPatrimonial(): [{0}]\r\n[{1}]\r\n{2}"
                                    , lResponseSit.StatusResposta
                                    , lResponseSit.DescricaoResposta
                                    , JsonConvert.SerializeObject(pDados));

                throw new Exception(lResponseSit.DescricaoResposta);
            }

        }

        private void SalvarDadosBancarios(TransporteCadastro pDados)
        {
            SalvarEntidadeCadastroRequest<ClienteBancoInfo> lRequest;
            SalvarEntidadeCadastroResponse lResponse;

            List<int> lIdsExistentes = new List<int>();
            
            List<ClienteBancoInfo> lContasExistentes = SessaoClienteLogado.BuscarContasBancarias();

            bool lPrincipal = true; //principal vai ser sempre a primeira da lista

            foreach (TransporteCadastroContaBancaria lConta in pDados.Contas)
            {
                if (lConta.IdConta.HasValue)
                {
                    lIdsExistentes.Add(lConta.IdConta.Value);
                }

                lRequest = new SalvarEntidadeCadastroRequest<ClienteBancoInfo>();

                lRequest.IdUsuarioLogado        = SessaoClienteLogado.IdLogin;
                lRequest.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;
                lRequest.EntidadeCadastro       = lConta.ToClienteBancoInfo();
                lRequest.EntidadeCadastro.StPrincipal = lPrincipal;

                lRequest.EntidadeCadastro.IdCliente = SessaoClienteLogado.IdCliente.Value;

                lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteBancoInfo>(lRequest);

                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarDadosBancarios(): [{0}]\r\n[{1}]\r\n{2}"
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta
                                        , JsonConvert.SerializeObject(lConta));

                    throw new Exception(lResponse.DescricaoResposta);
                }

                lPrincipal = false;
            }

            RemoverEntidadeCadastroRequest<ClienteBancoInfo> lRequestExclusao = new RemoverEntidadeCadastroRequest<ClienteBancoInfo>();
            RemoverEntidadeCadastroResponse lResponseExclusao;

            foreach (ClienteBancoInfo lInfo in lContasExistentes)
            {
                if (!lIdsExistentes.Contains(lInfo.IdBanco.Value))
                {
                    //remove
                    lRequestExclusao.IdUsuarioLogado = SessaoClienteLogado.IdCliente.Value;
                    lRequestExclusao.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;
                    lRequestExclusao.EntidadeCadastro = new ClienteBancoInfo() { IdBanco = lInfo.IdBanco.Value };

                    lResponseExclusao = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteBancoInfo>(lRequestExclusao);

                    if (lResponseExclusao.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        gLogger.ErrorFormat("Resposta com erro em CadastroPF.aspx, SalvarDadosBancariosDoCliente({2}) > Exclusao: [{0}]\r\n[{1}]"
                                            , lResponseExclusao.StatusResposta
                                            , lResponseExclusao.DescricaoResposta
                                            , lInfo.IdBanco);
                    }
                }
            }
        }

        private void SalvarDeclaracoes(TransporteCadastro pDados)
        {
            SalvarEntidadeCadastroRequest<ClienteInfo> lRequest = new SalvarEntidadeCadastroRequest<ClienteInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = this.BuscarDadosDoCliente();

            if (lRequest.EntidadeCadastro.StPasso < 3)
            {
                lRequest.EntidadeCadastro.StPasso        = 3;
                lRequest.EntidadeCadastro.DtPasso3       = DateTime.Now;
            }

            lRequest.EntidadeCadastro.StEmancipado       = (pDados.Emancipado == "Sim");
            lRequest.EntidadeCadastro.StPessoaVinculada  = Convert.ToInt32(pDados.PessoaVinculada);
            lRequest.EntidadeCadastro.StPPE              = (pDados.PessoaPoliticamenteExposta == "Sim");
            lRequest.EntidadeCadastro.StCarteiraPropria  = (pDados.OperaPorContaPropria == "Sim");
            lRequest.EntidadeCadastro.StCVM387           = false;

            if (lRequest.EntidadeCadastro.StCarteiraPropria.HasValue && lRequest.EntidadeCadastro.StCarteiraPropria == false)
            {
                lRequest.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria = new Intranet.Contratos.Dados.Cadastro.ClienteNaoOperaPorContaPropriaInfo();

                lRequest.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.IdCliente = SessaoClienteLogado.IdCliente.Value;
                lRequest.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado = pDados.NomeCliente;
                lRequest.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado = pDados.CPFCliente;
            }
            else
            {
                lRequest.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria = null;
            }

            if (pDados.USPerson == "Sim")
            {
                lRequest.EntidadeCadastro.StUSPerson = true;
            }
            else if(pDados.USPerson == "Não")
            {
                lRequest.EntidadeCadastro.StUSPerson = false;
            }

            /// <summary>
            /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
            /// </summary>
            if (pDados.CienteRegulamento == "Sim" && pDados.CienteProspecto == "Sim" && pDados.CienteLamina == "Sim")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 7;
            }
            else if (pDados.CienteRegulamento == "Sim" && pDados.CienteProspecto == "Sim" && pDados.CienteLamina == "Não")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 6;
            }
            else if (pDados.CienteRegulamento == "Sim" && pDados.CienteProspecto == "Não" && pDados.CienteLamina == "Sim")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 5;
            }
            else if (pDados.CienteRegulamento == "Sim" && pDados.CienteProspecto == "Não" && pDados.CienteLamina == "Não")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 4;
            }
            else if (pDados.CienteRegulamento == "Não" && pDados.CienteProspecto == "Sim" && pDados.CienteLamina == "Sim")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 3;
            }
            else if (pDados.CienteRegulamento == "Não" && pDados.CienteProspecto == "Sim" && pDados.CienteLamina == "Não")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 2;
            }
            else if (pDados.CienteRegulamento == "Não" && pDados.CienteProspecto == "Não" && pDados.CienteLamina == "Sim")
            {
                lRequest.EntidadeCadastro.StCienteDocumentos = 1;
            }

            lRequest.EntidadeCadastro.DsPropositoGradual = pDados.PropositoGradual;

            lRequest.IdUsuarioLogado = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;

            lResponse = ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(IdUsuarioLogado: [{0}], StPasso: [{1}], DtPasso3: [{2}], StEmancipado: [{3}], StPessoaVinculada: [{4}], StPPE: [{5}], StCarteiraPropria: [{6}], StCVM387: [{7}]) em Cadastro_PF_Passo3.aspx > SalvarDeclaracoes > [{8}]\r\n{9}"
                                    , lRequest.IdUsuarioLogado
                                    , lRequest.EntidadeCadastro.StPasso
                                    , lRequest.EntidadeCadastro.DtPasso3
                                    , lRequest.EntidadeCadastro.StEmancipado
                                    , lRequest.EntidadeCadastro.StPessoaVinculada
                                    , lRequest.EntidadeCadastro.StPPE
                                    , lRequest.EntidadeCadastro.StCarteiraPropria
                                    , lRequest.EntidadeCadastro.StCVM387
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }

            if (SessaoClienteLogado.Passo < 3)
            {
                SessaoClienteLogado.Passo = 3;
            }
        }

        private string SalvarSuitability(TransporteCadastroSuitability pSuitability)
        {
            StringBuilder lRespostasQuestionario = new StringBuilder();

            lRespostasQuestionario.AppendFormat("1:{0},"    , pSuitability.Resp1    )
                                  .AppendFormat("2:{0},"    , pSuitability.Resp2    )
                                  .AppendFormat("3:{0},"    , pSuitability.Resp3    )
                                  .AppendFormat("4:{0},"    , pSuitability.Resp4    )
                                  .AppendFormat("5:{0},"    , pSuitability.Resp5    )
                                  .AppendFormat("6:{0},"    , pSuitability.Resp6    )
                                  .AppendFormat("7:{0},"    , pSuitability.Resp7    )
                                  .AppendFormat("8:{0},"    , pSuitability.Resp8    )
                                  .AppendFormat("9:{0},"    , pSuitability.Resp9    )
                                  .AppendFormat("10:{0},"   , pSuitability.Resp10   )
                                  .AppendFormat("11:{0},"   , pSuitability.Resp11   );

            string lResultado = this.VerificarPontuacaoDoSuitability(pSuitability);

            string lFonte = "Portal-Cadastro";

            /*if()
            {
                lFonte = "Portal-Ferramentas";
            }*/

            SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequest = new SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = new ClienteSuitabilityInfo();

            lRequest.EntidadeCadastro.ds_fonte          = lFonte;
            lRequest.EntidadeCadastro.ds_loginrealizado = SessaoClienteLogado.Nome;
            lRequest.EntidadeCadastro.ds_perfil         = lResultado;
            lRequest.EntidadeCadastro.ds_respostas      = lRespostasQuestionario.ToString();
            lRequest.EntidadeCadastro.ds_status         = "Finalizado";
            lRequest.EntidadeCadastro.dt_realizacao     = DateTime.Now;
            lRequest.EntidadeCadastro.IdCliente         = SessaoClienteLogado.IdCliente.Value;

            lRequest.EntidadeCadastro.st_preenchidopelocliente = true;

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                SessaoClienteLogado.PerfilSuitability = lResultado;
                EnviarEmailComPerfilDoInvestidor();

            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSuitabilityInfo>(ds_fonte: [{0}], ds_loginrealizado: [{1}], ds_perfil: [{2}], ds_respostas: [{3}], ds_status: [{4}], dt_realizacao: [{5}], IdCliente: [{6}], st_preenchidopelocliente: [{7}]) em CadastroPF.aspx > ResponderReavaliarSuitability > [{8}]\r\n{9}"
                                    , lRequest.EntidadeCadastro.ds_fonte
                                    , lRequest.EntidadeCadastro.ds_loginrealizado
                                    , lRequest.EntidadeCadastro.ds_perfil
                                    , lRequest.EntidadeCadastro.ds_respostas
                                    , lRequest.EntidadeCadastro.ds_status
                                    , lRequest.EntidadeCadastro.dt_realizacao
                                    , lRequest.EntidadeCadastro.IdCliente
                                    , lRequest.EntidadeCadastro.st_preenchidopelocliente
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }

            return lResultado;
        }

        private void SalvarRepresentante(TransporteCadastro pDados)
        {
            SalvarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo> lRequest = new SalvarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.IdUsuarioLogado        = SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = SessaoClienteLogado.Nome;

            lRequest.EntidadeCadastro = new ClienteProcuradorRepresentanteInfo();

            lRequest.EntidadeCadastro.IdCliente         = SessaoClienteLogado.IdCliente;
            lRequest.EntidadeCadastro.DsNome            = pDados.Representante.Nome.ToUpper();
            lRequest.EntidadeCadastro.NrCpfCnpj         = pDados.Representante.CPF.Replace(".", "").Replace("-", "");
            lRequest.EntidadeCadastro.DtNascimento      = DateTime.ParseExact(pDados.Representante.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            lRequest.EntidadeCadastro.DsNumeroDocumento = pDados.Representante.NumeroDocumento.ToUpper();
            lRequest.EntidadeCadastro.CdOrgaoEmissor    = pDados.Representante.OrgaoEmissor.ToUpper();
            lRequest.EntidadeCadastro.CdUfOrgaoEmissor  = pDados.Representante.EstadoEmissor.ToUpper();
            lRequest.EntidadeCadastro.TpDocumento       = pDados.Representante.TipoDocumento.ToUpper();
            lRequest.EntidadeCadastro.TpSituacaoLegal   = Convert.ToInt32(pDados.Representante.SituacaoLegal);

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

            if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>() em CadastroPF.aspx > SalvarRepresentante > [{0}]\r\n{1}\r\n{2}"
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta
                                    , JsonConvert.SerializeObject(pDados.Representante));

                throw new Exception(lResponse.DescricaoResposta);
            }
        }


        private string ResponderSalvarPasso1()
        {
            string lRetorno;

            try
            {
                TransporteCadastro lDados = JsonConvert.DeserializeObject<TransporteCadastro>(Request["DadosDeCadastro"]);

                if (SessaoClienteLogado == null)
                {
                    lRetorno = SalvarPasso1DoCliente(lDados);
                }
                else
                {
                    SalvarDadosDoCliente(lDados, 1);

                    SalvarTelefonesDoCliente(lDados);
                    
                    lRetorno = base.RetornarSucessoAjax("Dados salvos com sucesso.");
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso1()", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarPasso2()
        {
            string lRetorno;

            try
            {
                if (SessaoClienteLogado != null)
                {
                    TransporteCadastro lDados = JsonConvert.DeserializeObject<TransporteCadastro>(Request["DadosDeCadastro"]);

                    SalvarDadosDoCliente(lDados, 2);

                    SalvarEnderecosDoCliente(lDados);

                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    lRetorno = RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso2()", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarPasso3()
        {
            string lRetorno;

            try
            {
                if (SessaoClienteLogado != null)
                {
                    TransporteCadastro lDados = JsonConvert.DeserializeObject<TransporteCadastro>(Request["DadosDeCadastro"]);

                    SalvarSituacaoFinanceiraPatrimonial(lDados);

                    SalvarDadosBancarios(lDados);

                    SalvarDeclaracoes(lDados);

                    if (!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3))
                    {
                        SalvarSuitability(lDados.Suitability);
                    }

                    if (lDados.Procurador == "Sim")
                    {
                        SalvarRepresentante(lDados);
                    }

                    if (SessaoClienteLogado.Passo <= 3)
                    {
                        SessaoClienteLogado.Passo = 3;
                    }

                    lRetorno = ResponderBuscarListaDeFundosRecomendados();
                }
                else
                {
                    lRetorno = RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso3()", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarPasso3Cambio()
        {
            string lRetorno;

            try
            {
                if (SessaoClienteLogado != null)
                {
                    TransporteCadastro lDados = JsonConvert.DeserializeObject<TransporteCadastro>(Request["DadosDeCadastro"]);

                    SalvarSituacaoFinanceiraPatrimonial(lDados);

                    SalvarDadosBancarios(lDados);

                    SalvarDeclaracoes(lDados);

                    if (!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(3))
                    {
                        SalvarSuitability(lDados.Suitability);
                    }

                    if (lDados.Procurador == "Sim")
                    {
                        SalvarRepresentante(lDados);
                    }

                    if (SessaoClienteLogado.Passo <= 3)
                    {
                        SessaoClienteLogado.Passo = 3;
                    }

                    lRetorno = ResponderBuscarListaDeFundosRecomendados();
                }
                else
                {
                    lRetorno = RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso3Cambio()", ex);
            }


            try
            {
                if (SessaoClienteLogado != null)
                {
                    string lURL = "";

                    IServicoFichaCadastral lServico = InstanciarServicoDoAtivador<IServicoFichaCadastral>();

                    ReceberEntidadeRequest<FichaCadastralGradualInfo> lRequest = new ReceberEntidadeRequest<FichaCadastralGradualInfo>();
                    OMS.Persistencia.ReceberObjetoResponse<FichaCadastralGradualInfo> lResponse;

                    lRequest.Objeto = new FichaCadastralGradualInfo();

                    lRequest.Objeto.IdCliente = base.SessaoClienteLogado.IdCliente.Value;
                    lRequest.Objeto.SitemaOrigem = SistemaOrigem.Portal;

                    lResponse = lServico.GerarFichaCadastralPF(lRequest);

                    Session["ArquivoFicha"] = "";

                    if (lResponse.Objeto != null)
                    {
                        //SessaoClienteLogado.NomeArquivoFichaCadastral = string.Format("{0}/Resc/PDFs/FichaCadastral/{1}"
                        //                                                               , this.HostERaiz
                        //                                                               , lResponse.Objeto.PathFichaCadastral);

                        SessaoClienteLogado.NomeArquivoFichaCadastral = String.Format("{0}?Tipo={1}", System.Configuration.ConfigurationManager.AppSettings["FileLoader"], "95a7f5db23b383b98e1ca6b61cf2c6c1");//lResponse.Objeto.PathFichaCadastral;

                        if (!String.IsNullOrEmpty(lResponse.Objeto.PathFichaCadastralCambio))
                        {
                            //SessaoClienteLogado.NomeArquivoFichaCadastralCambio = string.Format("{0}/Resc/PDFs/FichaCadastral/{1}"
                            //                                       , this.HostERaiz
                            //                                       , lResponse.Objeto.PathFichaCadastralCambio);

                            SessaoClienteLogado.NomeArquivoFichaCadastralCambio = String.Format("{0}?Tipo={1}", System.Configuration.ConfigurationManager.AppSettings["FileLoader"], "1dac8b6c1d8adbaa1a0af55f91bf4e31");

                            //Session["ArquivoFichaCambio"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastralCambio);
                            Session["ArquivoFichaCambio"] = lResponse.Objeto.PathFichaCadastralCambio;
                        }

                        //Session["ArquivoFicha"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastral);
                        Session["ArquivoFicha"] = lResponse.Objeto.PathFichaCadastral;

                    }

                    //EnviarEmailComPerfilDoInvestidor();

                    EnviarEmailPasso4();



                    if (!String.IsNullOrEmpty(lResponse.Objeto.PathFichaCadastralCambio))
                    {
                        lRetorno = RetornarSucessoAjax(1, string.Format("{0},{1},{2}", SessaoClienteLogado.NomeArquivoFichaCadastral, lURL, SessaoClienteLogado.NomeArquivoFichaCadastralCambio));
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax(1, string.Format("{0},{1}", SessaoClienteLogado.NomeArquivoFichaCadastral, lURL));
                    }
                    
                }
                else
                {
                    lRetorno = RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ResponderSalvarPasso4(): [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso4()", ex);
            }

            return lRetorno;
        }

        private string ResponderSalvarPasso4()
        {
            string lRetorno;

            try
            {
                if (SessaoClienteLogado != null)
                {
                    string lURL = "";

                    IServicoFichaCadastral lServico = InstanciarServicoDoAtivador<IServicoFichaCadastral>();

                    ReceberEntidadeRequest<FichaCadastralGradualInfo> lRequest = new ReceberEntidadeRequest<FichaCadastralGradualInfo>();
                    OMS.Persistencia.ReceberObjetoResponse<FichaCadastralGradualInfo> lResponse;

                    lRequest.Objeto = new FichaCadastralGradualInfo();

                    lRequest.Objeto.IdCliente    = base.SessaoClienteLogado.IdCliente.Value;
                    lRequest.Objeto.SitemaOrigem = SistemaOrigem.Portal;

                    lResponse = lServico.GerarFichaCadastralPF(lRequest);

                    Session["ArquivoFicha"] = "";

                    if (lResponse.Objeto != null)
                    {
                        //SessaoClienteLogado.NomeArquivoFichaCadastral = string.Format(   "{0}/Resc/PDFs/FichaCadastral/{1}"
                        //                                                               , this.HostERaiz
                        //                                                               , lResponse.Objeto.PathFichaCadastral   );

                        SessaoClienteLogado.NomeArquivoFichaCadastral = String.Format("{0}?Tipo={1}", System.Configuration.ConfigurationManager.AppSettings["FileLoader"], "95a7f5db23b383b98e1ca6b61cf2c6c1");//lResponse.Objeto.PathFichaCadastral; 

                        //Session["ArquivoFicha"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastral);
                        Session["ArquivoFicha"] = lResponse.Objeto.PathFichaCadastral;

                        if (!String.IsNullOrEmpty(lResponse.Objeto.PathFichaCadastralCambio))
                        {
                            //SessaoClienteLogado.NomeArquivoFichaCadastralCambio = string.Format("{0}/Resc/PDFs/FichaCadastral/{1}"
                            //                                       , this.HostERaiz
                            //                                       , lResponse.Objeto.PathFichaCadastralCambio);

                            SessaoClienteLogado.NomeArquivoFichaCadastralCambio = String.Format("{0}?Tipo={1}", System.Configuration.ConfigurationManager.AppSettings["FileLoader"], "1dac8b6c1d8adbaa1a0af55f91bf4e31");//lResponse.Objeto.PathFichaCadastralCambio;

                            //Session["ArquivoFichaCambio"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastralCambio);
                            Session["ArquivoFichaCambio"] = lResponse.Objeto.PathFichaCadastralCambio;
                        }

                    }

                    //if (SessaoClienteLogado.DesejaAplicar != "FUNDOS")
                    if(!this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(2))
                    {
                        //sendo fundos o arquivo de termo está salvo já
                        Session["ArquivoTermo"] = "";

                        lURL = "../../Resc/PDFs/Adesao_Contrato_Intermediacao_20jun13.doc";

                        ReceberEntidadeRequest<TermoAdesaoGradualInfo> lRequestTermo = new ReceberEntidadeRequest<TermoAdesaoGradualInfo>();
                        OMS.Persistencia.ReceberObjetoResponse<TermoAdesaoGradualInfo> lResponseTermo;

                        lRequestTermo.Objeto = new TermoAdesaoGradualInfo();

                        lRequestTermo.Objeto.IdCliente    = base.SessaoClienteLogado.IdCliente.Value;
                        lRequestTermo.Objeto.SitemaOrigem = SistemaOrigem.Portal;

                        lResponseTermo = lServico.GerarTermoDeAdesao(lRequestTermo);

                        if (lResponseTermo.Objeto != null)
                        {
                            //lURL = string.Format(  "{0}/Resc/PDFs/FichaCadastral/{1}", this.HostERaiz, lResponseTermo.Objeto.PathDownloadPdf   );
                            lURL = String.Format("{0}?Tipo={1}", System.Configuration.ConfigurationManager.AppSettings["FileLoader"], "b5f831311230fa38b177d9e768a35cbf");//lResponseTermo.Objeto.PathDownloadPdf;

                            //Session["ArquivoTermo"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponseTermo.Objeto.PathDownloadPdf);
                            Session["ArquivoTermo"] = lResponseTermo.Objeto.PathDownloadPdf;
                        }
                    }

                    EnviarEmailComPerfilDoInvestidor();

                    //TODO: alterar o path dos atalhos
                    EnviarEmailPasso4();

                    System.String lFileLoader = System.Configuration.ConfigurationManager.AppSettings["pathFileLoader"];

                    if (!String.IsNullOrEmpty(SessaoClienteLogado.NomeArquivoFichaCadastralCambio))
                    {
                        lRetorno = RetornarSucessoAjax(1, string.Format("{0},{1},{2}", SessaoClienteLogado.NomeArquivoFichaCadastral, lURL, SessaoClienteLogado.NomeArquivoFichaCadastralCambio));
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax(1, string.Format("{0},{1}", SessaoClienteLogado.NomeArquivoFichaCadastral, lURL));
                    }

                }
                else
                {
                    lRetorno = RetornarErroAjax(CONST_MENSAGEM_SEM_USUARIO_LOGADO);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em ResponderSalvarPasso4(): [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso4()", ex);
            }

            return lRetorno;
        }

        private string ResponderReavaliarSuitability()
        {
            StringBuilder lRespostasQuestionario = new StringBuilder();

            TransporteCadastroSuitability lSuitability = JsonConvert.DeserializeObject<TransporteCadastroSuitability>(Request["Suitability"]);

            string lResultado = SalvarSuitability(lSuitability);

            return RetornarSucessoAjax(lResultado);
        }

        private string ResponderSalvarDesejaAplicar()
        {
            string lRetorno = RetornarSucessoAjax("ok");

            //try
            //{
            //        int lIdFundo = Convert.ToInt32(Request["CodFundo"]);
            //        List<Transporte_IntegracaoFundos> lDadosFundo = PesquisarFundos(new PesquisarIntegracaoFundosRequest() { IdProduto = lIdFundo });

            //        if (lDadosFundo.Count > 0)
            //        {
            //            Session["ArquivoTermoFundo"] = string.Format("~/Resc/PDFs/AdesaoFundos/{0}", lDadosFundo[0].PathTermoPF);
            //        }

            //        if (this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(1) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(4) || this.SessaoClienteLogado.CodigoTipoOperacaoCliente.Equals(6))
            //        {
            //            lRetorno = RetornarSucessoAjax("ok_bov");
            //        }
            //        else
            //        {
            //            lRetorno = RetornarSucessoAjax("ok_fundos");
            //        }
            //}
            //catch (Exception ex)
            //{
            //    lRetorno = RetornarErroAjax("Erro ao salvar tipo de aplicação", ex);
            //}

            return lRetorno;
        }

        private string ResponderBuscarAssessor()
        {
            string lRetorno = RetornarSucessoAjax("NAO_ENCONTRADO");

            string lAssessor = Request["Assessor"];

            string lNome = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Assessor, lAssessor);

            if (!string.IsNullOrEmpty(lNome))
            {
                Session["CodigoAssessorCadastro"] = lAssessor;

                lRetorno = RetornarSucessoAjax(lNome);
            }

            return lRetorno;
        }

        private string ResponderEnviarEmailSuitability()
        {
            EnviarEmailComPerfilDoInvestidor();

            return RetornarSucessoAjax(string.Format("Email enviado com sucesso para {0}", SessaoClienteLogado.Email));
        }

        private string ResponderBuscarListaDeFundosRecomendados()
        {
            string lRetorno;

            List<Transporte_IntegracaoFundos> lListaFundos = base.PesquisarFundosSuitability(new PesquisarIntegracaoFundosRequest() 
            {
                IdPerfilSuitability = base.GetIdPerfilSuitability
            });

            FiltrarFundosProibidos(ref lListaFundos);

            lRetorno = RetornarSucessoAjax(lListaFundos, "ok");

            return lRetorno;
        }

        private string ResponderTestarFichaCadastral()
        {
            string lRetorno, lURL;

            IServicoFichaCadastral lServico = InstanciarServicoDoAtivador<IServicoFichaCadastral>();

            ReceberEntidadeRequest<FichaCadastralGradualInfo> lRequest = new ReceberEntidadeRequest<FichaCadastralGradualInfo>();
            OMS.Persistencia.ReceberObjetoResponse<FichaCadastralGradualInfo> lResponse;

            lRequest.Objeto = new FichaCadastralGradualInfo();

            lRequest.Objeto.IdCliente    = 238055; // SessaoClienteLogado.IdCliente.Value;  //PJ para testar: 213193;
            lRequest.Objeto.SitemaOrigem = SistemaOrigem.Portal;

            //lResponse = lServico.GerarFichaCadastralPJ(lRequest);

            lResponse = lServico.GerarFichaCadastralPF(lRequest);

            Session["ArquivoFicha"] = "";

            if (lResponse.Objeto != null)
            {
                SessaoClienteLogado.NomeArquivoFichaCadastral = string.Format(   "{0}/Resc/PDFs/FichaCadastral/{1}"
                                                                                , this.HostERaiz
                                                                                , lResponse.Objeto.PathFichaCadastral   ); 

                Session["ArquivoFicha"] = string.Format("~/Resc/PDFs/FichaCadastral/{0}", lResponse.Objeto.PathFichaCadastral);

            }

            lRetorno = RetornarSucessoAjax(1, string.Format("{0}", SessaoClienteLogado.NomeArquivoFichaCadastral));

            return lRetorno;
        }

        private string ResponderTestarEmail()
        {
            string lCorpoEmail = string.Empty;

            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
            
            lVariaveis.Add("###NOME###", base.SessaoClienteLogado.Nome);
            lVariaveis.Add("###LOGIN###", base.SessaoClienteLogado.Email);
            //lVariaveis.Add("###linkDocumentacao###", "http://www.gradualinvestimentos.com.br");

            lCorpoEmail = "CadastroPasso3ConclusaoCadastro.html";

            base.EnviarEmail(base.SessaoClienteLogado.Email, "Cadastro Gradual", lCorpoEmail, lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, null);

            return "Email enviado sem os anexos de ficha, somente para teste";
        }

        private string ResponderOk()
        {
            return RetornarSucessoAjax("ok");
        }

        #endregion

        #region Métodos Private - Alteração de Senha

        private MensagemResponseStatusEnum EnviarEmailConfirmacaoAlteracao()
        {
            Dictionary<string, string> lParametrosDoEmail = new Dictionary<string, string>();

            lParametrosDoEmail.Add("###NOME###", base.SessaoClienteLogado.Nome);
            lParametrosDoEmail.Add("###SENHA###", Request["SenhaNova"]);

            return base.EnviarEmail(base.SessaoClienteLogado.Email, "Alteração de Senha | Gradual Investimentos", "EmailNovaSenha.html", lParametrosDoEmail, eTipoEmailDisparo.Todos);
        }

        private MensagemResponseStatusEnum EnviarEmailConfirmacaoAlteracaoAssinatura()
        {
            Dictionary<string, string> lVariaveisDoEmail = new Dictionary<string, string>();

            lVariaveisDoEmail.Add("###NOME###", base.SessaoClienteLogado.Nome);

            return base.EnviarEmail(base.SessaoClienteLogado.Email, "Alteração de Assinatura Eletrônica | Gradual Investimentos", "EmailNovaAssiEletronica.html", lVariaveisDoEmail, eTipoEmailDisparo.Todos);
        }

        /*
        private void ConfigurarTela()
        {
            //pnlCadastro_AlterarSenha_Renovacao.Visible = (this.Request.QueryString["t"] != null);

            RodarJavascriptOnLoad("window.setInterval(MinhaConta_ExecutarFakeKeyPress, 1000);");
        }
        */

        private void ValidarHistoricoDeSenha()
        {
            ConsultarEntidadeCadastroRequest<HistoricoSenhaInfo>  lRequest = new ConsultarEntidadeCadastroRequest<HistoricoSenhaInfo>();
            ConsultarEntidadeCadastroResponse<HistoricoSenhaInfo> lResponse;

            lRequest.EntidadeCadastro = new HistoricoSenhaInfo();

            lRequest.EntidadeCadastro.CdSenha = Request["SenhaNova"];
            lRequest.EntidadeCadastro.IdLogin = base.SessaoClienteLogado.IdLogin;

            //lRequest.EntidadeCadastro.IdLogin = 222044;

            lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<HistoricoSenhaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count > 0)
                    throw new Exception("JA_UTILIZADA");
            }
            else
            {
                gLogger.ErrorFormat("Erro em AlterarSenha.aspx > ValidarHistoricoDeSenha(IdLogin [{0}]) [{1}]\r\n{2}"
                                    , lRequest.EntidadeCadastro.IdLogin
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void ValidarHistoricoDeSenhaDinamica() { }

        private void GravarHistoricoSenha()
        {
            SalvarEntidadeCadastroRequest<HistoricoSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<HistoricoSenhaInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = new HistoricoSenhaInfo();

            lRequest.EntidadeCadastro.CdSenha = Request["SenhaNova"];
            lRequest.EntidadeCadastro.IdLogin = base.SessaoClienteLogado.IdLogin;

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                gLogger.InfoFormat("Histórico de Nova senha para usuário [{0}] gravado com sucesso.", lRequest.EntidadeCadastro.IdLogin);
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(pIdLogin: [{0}]) em AlterarSenha.aspx > GravarHistoricoSenha() > [{1}]\r\n{2}"
                                    , lRequest.EntidadeCadastro.IdLogin
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private string ResponderAlterarSenha()
        {
            SalvarEntidadeCadastroRequest<AlterarSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<AlterarSenhaInfo>();

            SalvarEntidadeCadastroResponse lResponse;

            string lSenhaAtual, lSenhaNova, lSenhaNovaC;

            string lRetorno = "";

            lSenhaAtual = Request["SenhaAtual"];
            lSenhaNova = Request["SenhaNova"];
            lSenhaNovaC = Request["SenhaNovaC"];

            lRequest.IdUsuarioLogado = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;

            lRequest.EntidadeCadastro = new AlterarSenhaInfo();

            lRequest.EntidadeCadastro.CdSenhaAntiga = lSenhaAtual;
            lRequest.EntidadeCadastro.CdSenhaNova = lSenhaNova;
            lRequest.EntidadeCadastro.IdLogin = base.SessaoClienteLogado.IdLogin;

            //lRequest.IdUsuarioLogado = 222044;

            if (lSenhaNova == lSenhaNovaC)
            {
                if (lSenhaNova.Length >= 6 && lSenhaNova.Length <= 15)
                {
                    try
                    {
                        ValidarHistoricoDeSenha();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "JA_UTILIZADA")
                        {
                            lRetorno = RetornarSucessoAjax("JA_UTILIZADA");

                            return lRetorno;
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlterarSenhaInfo>(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
                        {
                            MensagemResponseStatusEnum lRetornoEnvioEmail = this.EnviarEmailConfirmacaoAlteracao();

                            if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                            {
                                lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua senha.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                            }
                            else
                            {
                                lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de senha. Verifique seu e-mail.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                            }
                        }
                        else
                        {
                            lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. O email não será enviado porque a aplicação está em teste.");
                        }

                        GravarHistoricoSenha();

                        base.RetirarClienteDaSessao();
                    }
                    else
                    {
                        if (lResponse.DescricaoResposta.Contains("já utilizada anteriormente"))
                        {
                            lRetorno = RetornarSucessoAjax("JA_UTILIZADA");
                        }
                        else if (lResponse.DescricaoResposta.Contains("senha atual não confere"))
                        {
                            lRetorno = RetornarSucessoAjax("SENHA_ERRADA");
                        }
                        else
                        {
                            gLogger.ErrorFormat("Erro em AlterarSenha.aspx > RealizarAlteracaoDeSenha(IdLogin [{0}]) [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdLogin
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta);

                            lRetorno = RetornarSucessoAjax("Erro ao alterar a senha, favor tentar novamente ou entrar em contato com o atendimento.");
                        }
                    }
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("A nova senha deve ter entre 6 e 15 caracteres.");

                }
            }
            else
            {
                lRetorno = RetornarSucessoAjax("A confirmação da senha não confere!");
            }

            return lRetorno;
        }

        private string ResponderAlterarSenhaDinamica()
        {
            SalvarEntidadeCadastroRequest<AlterarSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<AlterarSenhaInfo>();
            SalvarEntidadeCadastroRequest<AlterarSenhaDinamicaInfo> lRequestDinamico = new SalvarEntidadeCadastroRequest<AlterarSenhaDinamicaInfo>();
            SalvarEntidadeCadastroResponse lResponse; 
            Gradual.Site.DbLib.Mensagens.TipoTecladoResponse lTipoTecladoResponse = null;

            string lSenhaAtual, lSenhaNova, lSenhaNovaC, lTipoTeclado;

            string lRetorno = "";

            lSenhaAtual     = Request["SenhaAtual"];
            lSenhaNova      = Request["SenhaNova"];
            lSenhaNovaC     = Request["SenhaNovaC"];
            lTipoTeclado    = Request["TipoTeclado"];


            if (lSenhaNova == lSenhaNovaC)
            {
                if (lSenhaNova.Length == 6)
                {
                    try
                    {
                        ValidarHistoricoDeSenhaDinamica();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "JA_UTILIZADA")
                        {
                            lRetorno = RetornarSucessoAjax("JA_UTILIZADA");

                            return lRetorno;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    
                    lRequestDinamico.EntidadeCadastro                   = new AlterarSenhaDinamicaInfo();
                    lRequestDinamico.EntidadeCadastro.CdSenhaAntiga     = lSenhaAtual;
                    lRequestDinamico.EntidadeCadastro.CdSenhaNova       = lSenhaNova;
                    lRequestDinamico.EntidadeCadastro.IdLogin           = base.SessaoClienteLogado.IdLogin;
                    lRequestDinamico.IdUsuarioLogado                    = base.SessaoClienteLogado.IdLogin;
                    lRequestDinamico.DescricaoUsuarioLogado             = base.SessaoClienteLogado.Nome;

                    if (lTipoTeclado.Equals(1) || lTipoTeclado.Equals(2))
                    {
                        lRequestDinamico.EntidadeCadastro.SenhaDinamica = new SenhaInfo(lSenhaAtual);
                    }

                    lRequestDinamico.EntidadeCadastro.SenhaDinamicaNova = new SenhaInfo(lSenhaNova);

                    lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlterarSenhaDinamicaInfo>(lRequestDinamico);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
                        {

                            MensagemResponseStatusEnum lRetornoEnvioEmail = this.EnviarEmailConfirmacaoAlteracao();

                            if (Session["TipoTeclado"] != null)
                            {
                                lTipoTecladoResponse = (Gradual.Site.DbLib.Mensagens.TipoTecladoResponse)Session["TipoTeclado"];
                            }

                            if (lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO) || lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO_ASSINATURA))
                            {
                                lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. <br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                try
                                {
                                    GravarHistoricoSenha();
                                }
                                catch (Exception ex)
                                {
                                    gLogger.ErrorFormat("Erro MeuCadastro.aspx.cs > ResponderAlterarSenhaDinamica(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
                                }

                                base.RetirarClienteDaSessao();
                            }
                            else
                            {
                                if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                                {
                                    //lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua senha.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                    lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua senha.");
                                }
                                else
                                {
                                    //lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de senha. Verifique seu e-mail.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                    lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de senha. Verifique seu e-mail.");
                                }
                            }
                        }
                        else
                        {
                            lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. O email não será enviado porque a aplicação está em teste.");
                        }
                    }
                    else
                    {
                        if (lResponse.DescricaoResposta.Contains("já utilizada anteriormente"))
                        {
                            lRetorno = RetornarSucessoAjax("JA_UTILIZADA");
                        }
                        else if (lResponse.DescricaoResposta.Contains("senha atual não confere"))
                        {
                            lRetorno = RetornarSucessoAjax("SENHA_ERRADA");
                        }
                        else
                        {
                            gLogger.ErrorFormat("Erro em AlterarSenha.aspx > RealizarAlteracaoDeSenha(IdLogin [{0}]) [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdLogin
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta);

                            lRetorno = RetornarSucessoAjax("Erro ao alterar a senha, favor tentar novamente ou entrar em contato com o atendimento.");
                        }
                    }
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("A nova senha deve ter 6 caracteres numéricos.");
                }
            }
            else
            {
                lRetorno = RetornarSucessoAjax("A confirmação da senha não confere!");
            }

            return lRetorno;
        }

        #endregion

        #region Métodos Private - Alteração de Assinatura

        private string ResponderAlterarAssinatura()
        {
            gLogger.InfoFormat("O usuário {0} solicitou a alteração da assinatura!", this.SessaoClienteLogado.CodigoPrincipal);

            SalvarEntidadeCadastroRequest<AlterarAssinaturaEletronicaInfo> lRequest = new SalvarEntidadeCadastroRequest<AlterarAssinaturaEletronicaInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            string lRetorno, lAssinaturaAtual, lAssinaturaNova, lAssinaturaNovaC;
            
            lAssinaturaAtual = Request["AssinaturaAtual"];
            lAssinaturaNova  = Request["AssinaturaNova"];
            lAssinaturaNovaC = Request["AssinaturaNovaC"];

            lRequest.IdUsuarioLogado          = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado   = base.SessaoClienteLogado.Nome;
            lRequest.EntidadeCadastro = new AlterarAssinaturaEletronicaInfo();

            lRequest.EntidadeCadastro.CdAssinaturaAntiga  = lAssinaturaAtual;
            lRequest.EntidadeCadastro.CdAssinaturaNova    = lAssinaturaNova;
            lRequest.EntidadeCadastro.IdLogin             = base.SessaoClienteLogado.IdLogin;
            lRequest.EntidadeCadastro.CodigoPrincipal     = base.SessaoClienteLogado.CodigoPrincipal;
            //lRequest.EntidadeCadastro.IdLogin = 222044;

            if (lAssinaturaNova.Length == 6)
            {

                lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlterarAssinaturaEletronicaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    MensagemResponseStatusEnum lRetornoEnvioEmail = EnviarEmailConfirmacaoAlteracaoAssinatura();

                    Gradual.Site.DbLib.Mensagens.TipoTecladoResponse lTipoTecladoResponse = null;

                    if (Session["TipoTeclado"] != null)
                    {
                        lTipoTecladoResponse = (Gradual.Site.DbLib.Mensagens.TipoTecladoResponse)Session["TipoTeclado"];
                    }

                    if (lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO) || lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO_SENHA))
                    {
                        base.RetirarClienteDaSessao();

                        if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua assinatura.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                        }
                        else
                        {
                            lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de assinatura. Verifique seu e-mail.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                        }
                    }
                    else
                    {
                        if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua assinatura.");
                        }
                        else
                        {
                            lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de assinatura. Verifique seu e-mail.");
                        }
                    }
                }
                else
                {
                    if (lResponse.DescricaoResposta.Contains("já utilizada"))
                    {
                        lRetorno = RetornarSucessoAjax("Esta assinatura já foi utilizada. Escolha uma assinatura diferente das última 6 assinaturas utilizadas no sistema.");
                    }
                    else if (lResponse.DescricaoResposta.Contains("assinatura eletrônica atual não confere"))
                    {
                        lRetorno = RetornarSucessoAjax("Assinatura eletrônica atual não confere.");
                    }
                    else if (lResponse.DescricaoResposta.Contains("dados informados estão incorretos"))
                    {
                        lRetorno = RetornarSucessoAjax("Seus dados de CPF/CPNJ ou data de nascimento/fundação não conferem com os registrados no sistema, favor entrar em contato com o atendimento.");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0001-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nUse uma senha com no mínimo [8] e no máximo [15] caracteres!");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0002-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nA Senha não deve ser igual ao Login!");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0003-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nNão use caracteres iguais em sequência!");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0004-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nNão use sequência de caracteres!");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0005-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos.\r\nUse uma senha alfanumérica!");
                    }
                    else if (lResponse.DescricaoResposta.Contains("0006-"))
                    {
                        lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nSenha já utilizada anteriormente");
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                    }
                }
            }
            else
            {
                lRetorno = RetornarSucessoAjax("A nova assinatura deve ter 6 caracteres numéricos.");
            }

            return lRetorno;
        }

        private string ResponderAlterarAssinaturaDinamica()
        {
            SalvarEntidadeCadastroRequest<AlterarAssinaturaEletronicaDinamicaInfo> lRequest = new SalvarEntidadeCadastroRequest<AlterarAssinaturaEletronicaDinamicaInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            string lRetorno, lAssinaturaAtual, lAssinaturaNova, lAssinaturaNovaC, lTipoTeclado;

            lAssinaturaAtual                                    = Request["AssinaturaAtual"];
            lAssinaturaNova                                     = Request["AssinaturaNova"];
            lAssinaturaNovaC                                    = Request["AssinaturaNovaC"];
            lTipoTeclado                                        = Request["TipoTeclado"];

            lRequest.IdUsuarioLogado                            = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado                     = base.SessaoClienteLogado.Nome;
            lRequest.EntidadeCadastro                           = new AlterarAssinaturaEletronicaDinamicaInfo();

            lRequest.EntidadeCadastro.CdAssinaturaAntiga        = lAssinaturaAtual;
            lRequest.EntidadeCadastro.CdAssinaturaNova          = lAssinaturaNova;
            lRequest.EntidadeCadastro.IdLogin                   = base.SessaoClienteLogado.IdLogin;
            lRequest.EntidadeCadastro.CodigoPrincipal           = base.SessaoClienteLogado.CodigoPrincipal;
            
            if (!lTipoTeclado.Equals("0"))
            {
                lRequest.EntidadeCadastro.AssinaturaDinamica = new AssinaturaInfo(lAssinaturaAtual);
            }

            lRequest.EntidadeCadastro.AssinaturaDinamicaNova    = new AssinaturaInfo(lAssinaturaNova);

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlterarAssinaturaEletronicaDinamicaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                MensagemResponseStatusEnum lRetornoEnvioEmail = EnviarEmailConfirmacaoAlteracaoAssinatura();

                Gradual.Site.DbLib.Mensagens.TipoTecladoResponse lTipoTecladoResponse = null;
            
                if(Session["TipoTeclado"] != null)
                {
                    lTipoTecladoResponse = (Gradual.Site.DbLib.Mensagens.TipoTecladoResponse)Session["TipoTeclado"];
                }

                if (lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO) || lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO_SENHA))
                {
                    base.RetirarClienteDaSessao();

                    if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua assinatura.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de assinatura. Verifique seu e-mail.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                    }
                }
                else
                {
                    if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua assinatura.");
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax("Sua assinatura foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de assinatura. Verifique seu e-mail.");
                    }
                }
            }
            else
            {
                if (lResponse.DescricaoResposta.Contains("já utilizada"))
                {
                    lRetorno = RetornarSucessoAjax("Esta assinatura já foi utilizada. Escolha uma assinatura diferente das última 6 assinaturas utilizadas no sistema.");
                }
                else if (lResponse.DescricaoResposta.Contains("assinatura eletrônica atual não confere"))
                {
                    lRetorno = RetornarSucessoAjax("Assinatura eletrônica atual não confere.");
                }
                else if (lResponse.DescricaoResposta.Contains("dados informados estão incorretos"))
                {
                    lRetorno = RetornarSucessoAjax("Seus dados de CPF/CPNJ ou data de nascimento/fundação não conferem com os registrados no sistema, favor entrar em contato com o atendimento.");
                }
                else if (lResponse.DescricaoResposta.Contains("0001-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nUse uma senha com no mínimo [8] e no máximo [15] caracteres!");
                }
                else if (lResponse.DescricaoResposta.Contains("0002-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nA Senha não deve ser igual ao Login!");
                }
                else if (lResponse.DescricaoResposta.Contains("0003-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nNão use caracteres iguais em sequência!");
                }
                else if (lResponse.DescricaoResposta.Contains("0004-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nNão use sequência de caracteres!");
                }
                else if (lResponse.DescricaoResposta.Contains("0005-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema e/ou possui caracteres inválidos.\r\nUse uma senha alfanumérica!");
                }
                else if (lResponse.DescricaoResposta.Contains("0006-"))
                {
                    lRetorno = RetornarSucessoAjax("Sua Senha é considerada fraca pelo sistema.\r\nSenha já utilizada anteriormente");
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }

            return lRetorno;
        }


        #endregion

        #region Event Handlers

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Cadastro";
            this.PaginaMaster.Crumb3Text = "Meus Dados";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String lAcao = String.Empty;

            if (!String.IsNullOrEmpty(Request.Params["Acao"]))
            {
                lAcao = Request.Params["Acao"];
            }

            if (!lAcao.Equals("AlterarSenhaDinamica") && !lAcao.Equals("AlterarAssinaturaDinamica"))
            {
                base.ValidarSessao();
            }
            else
            {
                if (this.SessaoClienteLogado == null)
                {
                    Session["RedirecionamentoPorFaltaDeLogin"] = Request.Url.PathAndQuery;

                    this.Response.Redirect(HostERaizFormat("MinhaConta/Login.aspx"));
                }
            }

            base.RegistrarRespostasAjax(new string[] {   "SalvarPasso1"
                                                       , "SalvarPasso2"
                                                       , "SalvarPasso3"
                                                       , "SalvarPasso4"
                                                       , "ReavaliarSuitability"
                                                       , "SalvarDesejaAplicar"
                                                       , "BuscarAssessor"
                                                       , "EnviarEmailSuitability"
                                                       , "AlterarSenha"
                                                       , "AlterarAssinatura"
                                                       , "BuscarListaDeFundosRecomendados"
                                                       , "TestarFicha"
                                                       , "TestarEmail"
                                                       , "SalvarPasso3Cambio"
                                                       , "AlterarSenhaDinamica"
                                                       , "AlterarAssinaturaDinamica"
                                                       , CONST_FUNCAO_CASO_NAO_HAJA_ACTION 
                                                     }
                   , new ResponderAcaoAjaxDelegate[] {   ResponderSalvarPasso1
                                                       , ResponderSalvarPasso2
                                                       , ResponderSalvarPasso3
                                                       , ResponderSalvarPasso4
                                                       , ResponderReavaliarSuitability
                                                       , ResponderSalvarDesejaAplicar
                                                       , ResponderBuscarAssessor
                                                       , ResponderEnviarEmailSuitability
                                                       , ResponderAlterarSenha
                                                       , ResponderAlterarAssinatura
                                                       , ResponderBuscarListaDeFundosRecomendados
                                                       , ResponderTestarFichaCadastral
                                                       , ResponderTestarEmail
                                                       , ResponderSalvarPasso3Cambio
                                                       , ResponderAlterarSenhaDinamica
                                                       , ResponderAlterarAssinaturaDinamica
                                                       , CarregarDados
                                                     } );
        }

        #endregion
    }
}