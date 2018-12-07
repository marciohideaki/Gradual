using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados.MinhaConta;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.Library;

using System.Web.UI.HtmlControls;
using System.IO;
using Ionic.Zip;
using Gradual.Site.Www.Transporte;

namespace Gradual.Site.Www.MinhaConta.Financeiro
{
    public partial class NotasDeCorretagem : PaginaBase
    {
        #region Globais

        private int gCodigoCorretora = 2271;    //Magic Number!

        #endregion

        #region Propriedades

        private int CBLC
        {
            get
            {
                return base.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            }
        }

        private int CodigoBMF
        {
            get
            {
                return base.SessaoClienteLogado.CodigoBMF;
            }
        }

        public DateTime DataInicial
        {
            get
            {
                DateTime lData = DateTime.MinValue;

                DateTime.TryParseExact(txtDataInicial.Text, "dd/MM/yyyy", gCultureInfoBR, DateTimeStyles.None, out lData);

                return lData;
            }
        }

        public DateTime DataFinal
        {
            get
            {
                DateTime lData = DateTime.MaxValue;

                DateTime.TryParseExact(txtDataFinal.Text, "dd/MM/yyyy", gCultureInfoBR, DateTimeStyles.None, out lData);

                return lData;
            }
        }
        
        private DateTime GetDataInicialPaginacao
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicialPaginacao"], out lRetorno);

                return lRetorno;
            }
            
        }

        private Nullable<int> BotaoSelecionadoPaginacao
        {
            get
            {
                var lRetorno = default(int);

                if (ViewState["BotaoSelecionado"] == null) return null;

                Int32.TryParse(ViewState["BotaoSelecionado"].ToString(), out lRetorno );

                return lRetorno;
            }
        }

        private Nullable<DateTime> DataSelecionada
        {
            get
            {
                var lRetorno = default(DateTime);

                if (ViewState["DataSelecionado"] == null) return null;

                DateTime.TryParse(ViewState["DataSelecionado"].ToString(), out lRetorno);

                return lRetorno;
            }
        }

        public string Descricao { get; set; }

        public string cssLiquidoAB { get; set; }

        private static List<DateTime> ListaNegociacoesBov
        {
            get;
            set;
        }

        private static List<DateTime> ListaNegociacoesBmf
        {
            get;
            set;
        }
        #endregion

        #region Metodos Private
        private void CarregarOperacoesClientes()
        {
            UltimasNegociacoesRequest lRequest = new UltimasNegociacoesRequest();

            lRequest.UltimasNegociacoes              = new UltimasNegociacoesInfo();
            
            lRequest.UltimasNegociacoes.CdCliente    = this.SessaoClienteLogado.CodigoPrincipal.DBToInt32();
            
            lRequest.UltimasNegociacoes.CdClienteBmf = this.SessaoClienteLogado.CodigoBMF;

            lRequest.UltimasNegociacoes.DataInicial  = this.DataInicial;

            lRequest.UltimasNegociacoes.DataFinal    = this.DataFinal;

            UltimasNegociacoesResponse lResposta     = base.ServicoPersistenciaSite.ConsultarUltimasNegociacoesCliente(lRequest);

            if (lResposta.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                ListaNegociacoesBov = new List<DateTime>();

                ListaNegociacoesBmf = new List<DateTime>();

                foreach (var date in  lResposta.ListaUltimasNegociacoes)
                {
                    if (date.TipoBolsa.Equals("BOL"))
                    {
                        ListaNegociacoesBov.Add(date.DtUltimasNegociacoes.Value);
                    }
                    else if (date.TipoBolsa.Equals("BMF"))
                    {
                        ListaNegociacoesBmf.Add(date.DtUltimasNegociacoes.Value);
                    }
                }
                
            }
        }

        private void ResponderArquivoCSV()
        {
            StringBuilder lBuilder = new StringBuilder();

            IServicoRelatoriosFinanceiros lServicoAtivador = InstanciarServicoDoAtivador<IServicoRelatoriosFinanceiros>();

            OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest lRequest = new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest();
            OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoResponse lResponse;



            lRequest.ConsultaCodigoCliente   = this.CBLC;
            lRequest.ConsultaCodigoCorretora = gCodigoCorretora; 
            lRequest.ConsultaDataMovimento   = this.DataSelecionada.HasValue ? this.DataSelecionada.Value : this.DataInicial;
            lRequest.ConsultaTipoDeMercado   = this.cboTipoMercado.SelectedValue;
            lRequest.ConsultaProvisorio      = false;
            lRequest.ConsultaCodigoClienteBmf = base.SessaoClienteLogado.CodigoBMF;
            



            lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                var lNotaCorretagem = lResponse.Relatorio;

                lBuilder.AppendLine("Relatório: Notas de Corretagem");

                lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", lNotaCorretagem.CodigoCliente, lNotaCorretagem.CabecalhoCliente.NomeCliente);

                //lBuilder.AppendFormat("Conta: {0};Agência: {1};Banco:;{2}\r\n", lNotaCorretagem.CabecalhoCliente.ContaCorrente, lNotaCorretagem.CabecalhoCliente.Agencia, lNotaCorretagem.CabecalhoCliente.NrBanco);

                lBuilder.AppendLine("Praça;C/V;Tipo de Mercado;Espec. do Título;Obs.;Quantidade;Preço;Valor (R$);D/C");

                if (lNotaCorretagem.ListaNotaDeCorretagemExtratoInfo.Count > 0)
                {
                    foreach (TransporteExtratoNotaDeCorretagem lItem in TransporteExtratoNotaDeCorretagem.TraduzirLista(lNotaCorretagem.ListaNotaDeCorretagemExtratoInfo))
                    {
                        lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8}\r\n"
                                             , lItem.NomeBolsa
                                             , lItem.TipoOperacao
                                             , lItem.TipoMercado
                                             , lItem.EspecificacaoTitulo
                                             , lItem.Observacao
                                             , lItem.Quantidade
                                             , lItem.ValorNegocio
                                             , lItem.ValorTotal
                                             , lItem.DC);
                    }
                }
                else
                {
                    lBuilder.AppendLine("(0 lançamentos encontrados)");
                }

                lBuilder.AppendLine("");

                lBuilder.AppendLine("Resumo dos Negócios");

                lBuilder.AppendFormat("Debêntures:;{0}\r\n",            lNotaCorretagem.Rodape.Debentures.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Vendas à Vista:;{0}\r\n",        lNotaCorretagem.Rodape.VendaVista.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Compras à Vista:;{0}\r\n",       lNotaCorretagem.Rodape.CompraVista.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Opções - Compras:;{0}\r\n",      lNotaCorretagem.Rodape.CompraOpcoes.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Opções - Vendas:;{0}\r\n",       lNotaCorretagem.Rodape.VendaOpcoes.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Operações a Termo:;{0}\r\n",     lNotaCorretagem.Rodape.OperacoesTermo.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Operações a Futuro:;{0}\r\n",    lNotaCorretagem.Rodape.OperacoesFuturo.ToString("N2", gCultureInfoBR));

                lBuilder.AppendFormat("Valor das Oper. com Tit. Publ.:;{0}\r\n", lNotaCorretagem.Rodape.OperacoesTitulosPublicos.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Valor das Operações:;{0}\r\n",            lNotaCorretagem.Rodape.ValorDasOperacoes.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Valor do Ajuste p/Futuro:;{0}\r\n",       lNotaCorretagem.Rodape.ValorAjusteFuturo.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("IR Sobre Corretagem:;{0}\r\n",            lNotaCorretagem.Rodape.IRSobreCorretagem.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("IRRF Sobre Day Trade:;{0}\r\n",           lNotaCorretagem.Rodape.IRRFSobreDayTrade.ToString("N2", gCultureInfoBR));

                lBuilder.AppendLine("");

                lBuilder.AppendLine("Resumo Financeiro;;D/C");

                lBuilder.AppendFormat("Valor Líquido das Operações(1):;{0};{1}\r\n",    Math.Abs(lNotaCorretagem.Rodape.ValorLiquidoOperacoes).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D");
                lBuilder.AppendFormat("Taxa de Registro(3):;{0};{1}\r\n",               Math.Abs(lNotaCorretagem.Rodape.TaxaDeRegistro).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaDeRegistro_DC);
                lBuilder.AppendFormat("Taxa de Liquidação(2):;{0};{1}\r\n",             Math.Abs(lNotaCorretagem.Rodape.TaxaLiquidacao).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaLiquidacao_DC);
                lBuilder.AppendFormat("Total (1+2+3) A:;{0};{1}\r\n",                   Math.Abs(lNotaCorretagem.Rodape.Total_123_A).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.Total_123_A_DC);
                lBuilder.AppendFormat("Taxa de Termo/Opções/Futuro:;{0};{1}\r\n",       Math.Abs(lNotaCorretagem.Rodape.TaxaTerOpcFut).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaTerOpcFut_DC);
                lBuilder.AppendFormat("Taxa A.N.A:;{0};{1}\r\n",                        Math.Abs(lNotaCorretagem.Rodape.TaxaANA).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaANA_DC);
                lBuilder.AppendFormat("Emolumentos:;{0};{1}\r\n",                       Math.Abs(lNotaCorretagem.Rodape.Emolumentos).ToString("N2", gCultureInfoBR), "D");
                lBuilder.AppendFormat("Total Bolsa B:;{0};{1}\r\n",                     Math.Abs(lNotaCorretagem.Rodape.TotalBolsaB).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TotalBolsaBPosNeg > 0 ? "C" : "D");
                lBuilder.AppendFormat("Corretagem:;{0};{1}\r\n",                        Math.Abs(lNotaCorretagem.Rodape.Corretagem).ToString("N2", gCultureInfoBR), "D");
                lBuilder.AppendFormat("ISS:;{0};{1}\r\n",                               Math.Abs(lNotaCorretagem.Rodape.ISS).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.ISS_DC);

                lBuilder.AppendFormat("I.R.R.F. s/ operações, base {0}:;{1};{2}\r\n",   lNotaCorretagem.Rodape.VLBaseOperacoesIRRF.ToString("C2", gCultureInfoBR), Math.Abs(lNotaCorretagem.Rodape.IRRFOperacoes).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.IRRFOperacoes_DC);
                lBuilder.AppendFormat("Outras:;{0};{1}\r\n",                            Math.Abs(lNotaCorretagem.Rodape.Outras).ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.Outras > 0 ? "C" : "D");
                lBuilder.AppendFormat("Liquido (A+B) {0}:;{1};{2}",                     lNotaCorretagem.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy"));

                Response.Clear();

                Response.ContentType = "text/csv";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeNotasDeCorretagem.csv");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            else
            {
                ExibirMensagemJsOnLoad("A", "Não há nota de corretagem disponível no período.");
            }
        }

        private void ResponderArquivoBmfCSV()
        {
            StringBuilder lBuilder = new StringBuilder();

            var DtMovimento = this.DataSelecionada.HasValue ? this.DataSelecionada.Value : DataInicial;

            var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

            var lResponse = lServicoAtivador.ConsultarNotaDeCorretagemBmf(
                    new NotaDeCorretagemExtratoRequest()
                    {
                        ConsultaCodigoCliente    = SessaoClienteLogado.CodigoBMF,
                        ConsultaCodigoCorretora  = gCodigoCorretora,
                        ConsultaDataMovimento    = DtMovimento,
                        ConsultaTipoDeMercado    = cboTipoMercado.SelectedValue,
                        ConsultaProvisorio       = false, // this.GetProvisorio,
                        ConsultaCodigoClienteBmf = SessaoClienteLogado.CodigoBMF,
                    });

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                var lNotaCorretagem = lResponse.RelatorioBmf;

                lBuilder.AppendLine("Relatório: Notas de Corretagem BMF");

                lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", lNotaCorretagem.CodigoClienteBmf, SessaoClienteLogado.Nome);

                //lBuilder.AppendFormat("Conta: {0};Agência: {1};Banco:;{2}\r\n", lNotaCorretagem.CabecalhoCliente.ContaCorrente, lNotaCorretagem.CabecalhoCliente.Agencia, lNotaCorretagem.CabecalhoCliente.NrBanco);

                lBuilder.AppendLine("C/V;Mercado;Vencimento;Quantidade;Preço/Ajuste;Tipo de Negocio;Vlr. de Operação/Ajuste;D/C;Taxa Operacional");

                if (lNotaCorretagem.ListaNotaDeCorretagemExtratoBmfInfo.Count > 0)
                {
                    var lTrans = new TransporteExtratoNotaDeCorretagemBmf();

                    foreach (TransporteExtratoNotaDeCorretagemBmf lItem in lTrans.TraduzirLista(lNotaCorretagem.ListaNotaDeCorretagemExtratoBmfInfo))
                    {
                        lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8}\r\n"
                                             , lItem.Sentido
                                             ,lItem.Mercadoria + lItem.Mercadoria_Serie
                                             , lItem.Vencimento
                                             , lItem.Quantidade
                                             , lItem.PrecoAjuste
                                             , lItem.TipoNegocio
                                             , lItem.ValorOperacao
                                             , lItem.DC
                                             , lItem.TaxaOperacional);
                    }
                }
                else
                {
                    lBuilder.AppendLine("(0 lançamentos encontrados)");
                }

                lBuilder.AppendLine("");

                lBuilder.AppendLine("Resumo dos Negócios");

                lBuilder.AppendFormat("Venda Disponível:;{0};Compra Dispoível:;{1};\r\n",         lNotaCorretagem.Rodape.VendaDisponivel.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.CompraDisponivel.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Venda Opções:;{0};Compra Opções:;{1};\r\n",                lNotaCorretagem.Rodape.VendaOpcoes.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.CompraOpcoes.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Valor dos Negócios:;{0};IRRF:;{1};\r\n",                   lNotaCorretagem.Rodape.ValorNegocios.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.IRRF.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("IRRF Day Trade(Projeção):;{0};Taxa Operacional:;{1};\t\r\n", lNotaCorretagem.Rodape.IRRFDayTrade.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaOperacional.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Taxa Registro Bmf:;{0};Taxas Bmf:;{1};\r\n",               lNotaCorretagem.Rodape.TaxaRegistroBmf.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TaxaBmf.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("ISS:;{0};Ajuste de Posição:;{1};\r\n",                     lNotaCorretagem.Rodape.ISS.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.CompraDisponivel.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Ajuste Day Trade:;{0};Total de Despesas:;{1};\r\n",        lNotaCorretagem.Rodape.AjusteDayTrade.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TotalDespesas.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("IRRF Corretagem:;{0};Total Conta Normal:;{1};\r\n",        lNotaCorretagem.Rodape.IrrfCorretagem.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TotalContaNormal.ToString("N2", gCultureInfoBR));
                lBuilder.AppendFormat("Total Líquido:;{0};Total Líquido da Nota:;{1};\r\n",       lNotaCorretagem.Rodape.TotalLiquido.ToString("N2", gCultureInfoBR), lNotaCorretagem.Rodape.TotalLiquidoNota.ToString("N2", gCultureInfoBR));

                Response.Clear();

                Response.ContentType = "text/csv";

                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                Response.Charset = "iso-8859-1";

                Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeNotasDeCorretagemBmf.csv");

                Response.Write(lBuilder.ToString());

                Response.End();
            }
            else
            {
                ExibirMensagemJsOnLoad("A", "Não há nota de corretagem disponível no período.");
            }
        }

        private void CarregarDadosRelatorio(Nullable<DateTime> DtMovimento )
        {
            IServicoRelatoriosFinanceiros lServicoAtivador = InstanciarServicoDoAtivador<IServicoRelatoriosFinanceiros>();

            OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest  lRequest = new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest();
            OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoResponse lResponse;

            lRequest.ConsultaCodigoCliente   = CBLC;
            lRequest.ConsultaCodigoCorretora = gCodigoCorretora;
            lRequest.ConsultaDataMovimento = (DtMovimento.HasValue) ? DtMovimento.Value : DataInicial;
            lRequest.ConsultaTipoDeMercado   = cboTipoMercado.SelectedValue;
            lRequest.ConsultaProvisorio      = false;
            
            lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                pnlRelatorio.Visible = true;

                var NotaCorretagem = lResponse.Relatorio;

                this.lblData.Text = lRequest.ConsultaDataMovimento.ToString("dd/MM/yyyy");// GetDataInicialPaginacao.ToString();  // "De " + DataInicial.ToString("dd/MM/yyyy") + " até " + DataFinal.ToString("dd/MM/yyyy");// cboData.SelectedValue.Replace(" 00:00:00", "");
                this.lblCodigoCliente.Text                  = CBLC.ToString();
                this.lblNomeCliente.Text                    = NotaCorretagem.CabecalhoCliente.NomeCliente;
                this.lblCabecalho_Cliente_NumeroDaNota.Text = string.Format("{0}", NotaCorretagem.CabecalhoCliente.NrNota);
                this.lblCodigoDoCliente.Text                = CBLC.ToString();
                this.lblCpfCnpjCliente.Text                 = NotaCorretagem.CabecalhoCliente.CpfCnpj;

                //lblCliente.Text      = CBLC.ToString() + " - " + NotaCorretagem.CabecalhoCliente.NomeCliente;
                //lblNumeroDaNota.Text = string.Format("{0}", NotaCorretagem.CabecalhoCliente.NrNota);

                //lblConta.Text   = NotaCorretagem.CabecalhoCliente.ContaCorrente;
                //lblAgencia.Text = NotaCorretagem.CabecalhoCliente.Agencia;
                //lblBanco.Text   = NotaCorretagem.CabecalhoCliente.NrBanco;

                rptCorretagem.DataSource = TransporteExtratoNotaDeCorretagem.TraduzirLista(NotaCorretagem.ListaNotaDeCorretagemExtratoInfo);
                rptCorretagem.DataBind();

                trNeNhumItem.Visible = (NotaCorretagem.ListaNotaDeCorretagemExtratoInfo.Count == 0);

                lblRodape_Debentures.Text               = NotaCorretagem.Rodape.Debentures.ToString("N2", gCultureInfoBR);
                lblRodape_ValorLiquidoDasOperacoes.Text = Math.Abs(NotaCorretagem.Rodape.ValorLiquidoOperacoes).ToString("N2", gCultureInfoBR);
                lblRodape_VendasAVista.Text             = NotaCorretagem.Rodape.VendaVista.ToString("N2", gCultureInfoBR);
                lblRodape_TaxaDeRegistro.Text           = Math.Abs(NotaCorretagem.Rodape.TaxaDeRegistro).ToString("N2", gCultureInfoBR);
                lblRodape_ComprasAVista.Text            = NotaCorretagem.Rodape.CompraVista.ToString("N2", gCultureInfoBR);
                lblRodape_TaxaDeLiquidacao.Text         = Math.Abs(NotaCorretagem.Rodape.TaxaLiquidacao).ToString("N2", gCultureInfoBR);
                lblRodape_OpcoesCompras.Text            = NotaCorretagem.Rodape.CompraOpcoes.ToString("N2", gCultureInfoBR);
                lblRodape_Total_123_A.Text              = Math.Abs(NotaCorretagem.Rodape.Total_123_A).ToString("N2", gCultureInfoBR);

                lblRodape_OpcoesVendas.Text             = NotaCorretagem.Rodape.VendaOpcoes.ToString("N2", gCultureInfoBR);
                lblRodape_TaxasTermo_Opcoes_Futuro.Text = Math.Abs(NotaCorretagem.Rodape.TaxaTerOpcFut).ToString("N2", gCultureInfoBR);
                lblRodape_OperacoesTermo.Text           = NotaCorretagem.Rodape.OperacoesTermo.ToString("N2", gCultureInfoBR);
                lblRodape_TaxaANA.Text                  = Math.Abs(NotaCorretagem.Rodape.TaxaANA).ToString("N2", gCultureInfoBR);

                lblRodape_OperacoesFuturo.Text          = NotaCorretagem.Rodape.OperacoesFuturo.ToString("N2", gCultureInfoBR);
                lblRodape_Emolumentos.Text              = Math.Abs(NotaCorretagem.Rodape.Emolumentos).ToString("N2", gCultureInfoBR);
                lblRodape_ValorOperacoesTitPub.Text     = NotaCorretagem.Rodape.OperacoesTitulosPublicos.ToString("N2", gCultureInfoBR);
                lblRodape_Total_Bolsa_B.Text            = Math.Abs(NotaCorretagem.Rodape.TotalBolsaB).ToString("N2", gCultureInfoBR);
                lblRodape_ValorDasOperacoes.Text        = NotaCorretagem.Rodape.ValorDasOperacoes.ToString("N2", gCultureInfoBR);
                lblRodape_Corretagem.Text               = Math.Abs(NotaCorretagem.Rodape.Corretagem).ToString("N2", gCultureInfoBR);

                lblRodape_AjusteFuturo.Text             = NotaCorretagem.Rodape.ValorAjusteFuturo.ToString("N2", gCultureInfoBR);
                lblRodape_ISS.Text                      = Math.Abs(NotaCorretagem.Rodape.ISS).ToString("N2", gCultureInfoBR);
                lblRodape_IRSobreCorretagem.Text        = NotaCorretagem.Rodape.IRSobreCorretagem.ToString("N2", gCultureInfoBR);
                lblRodape_IRRF_BaseOperacoes.Text       = NotaCorretagem.Rodape.VLBaseOperacoesIRRF.ToString("C2", gCultureInfoBR);
                lblRodape_IRRF_SobreOperacoes.Text      = Math.Abs(NotaCorretagem.Rodape.IRRFOperacoes).ToString("N2", gCultureInfoBR);
                lblRodape_IRRF_SobreDayTrade.Text       = NotaCorretagem.Rodape.IRRFSobreDayTrade.ToString("N2", gCultureInfoBR);
                lblRodape_IRRF_SobreDayTradeBase.Text     = NotaCorretagem.Rodape.IRRFSobreDayTradeBase.ToString("N2", gCultureInfoBR);
                lblRodape_IRRF_SobreDayTradeProjecao.Text = NotaCorretagem.Rodape.IRRFSobreDayTradeProjecao.ToString("N2", gCultureInfoBR);

                lblRodape_Outras.Text                   = Math.Abs(NotaCorretagem.Rodape.Outras).ToString("N2", gCultureInfoBR);
                lblRodape_Outras_DC.Text                = NotaCorretagem.Rodape.Outras > 0 ? "C" : "D";

                lblRodape_Liquido_Para.Text             = NotaCorretagem.Rodape.DataLiquidoPara.ToString("dd/MM/yyyy"); // RecuperarDataLiquidoPara();
                lblRodape_Liquido_AB.Text               = Math.Abs(NotaCorretagem.Rodape.ValorLiquidoNota).ToString("N2", gCultureInfoBR);
                lblRodape_Liquido_AB_DC.Text            = NotaCorretagem.Rodape.ValorLiquidoNota > 0 ? "C" : "D";
                this.cssLiquidoAB =                     DefinirCorDoValor(NotaCorretagem.Rodape.ValorLiquidoNota);
                //lblRodape_Liquido_ABPosNeg.Text         = DefinirCorDoValor(NotaCorretagem.Rodape.ValorLiquidoNota);

                lblRodape_ValorLiquidoDasOperacoes_DC.Text      = NotaCorretagem.Rodape.ValorLiquidoOperacoes > 0 ? "C" : "D";
                lblRodape_TaxaDeRegistro_DC.Text                = NotaCorretagem.Rodape.TaxaDeRegistro_DC;
                lblRodape_TaxaDeLiquidacao_DC.Text              = NotaCorretagem.Rodape.TaxaLiquidacao_DC;
                lblRodape_Total_123_A_DC.Text                   = NotaCorretagem.Rodape.Total_123_A_DC;
                lblRodape_TaxasTermo_Opcoes_Futuro_DC.Text      = NotaCorretagem.Rodape.TaxaTerOpcFut_DC;
                lblRodape_TaxaANA_DC.Text                       = NotaCorretagem.Rodape.TaxaANA_DC;
                lblRodape_Emolumentos_DC.Text                   = "D";//NotaCorretagem.Rodape.Emolumentos_DC;

                //lblRodape_Total_Bolsa_BPosNeg.Text              = NotaCorretagem.Rodape.TotalBolsaBPosNeg.ToString("N2", gCultureInfoBR);
                lblRodape_Total_Bolsa_B_DC.Text                 = NotaCorretagem.Rodape.TotalBolsaBPosNeg > 0 ? "C" : "D";

                lblRodape_Corretagem_DC.Text                    = "D"; //NotaCorretagem.Rodape.Corretagem_DC;
                lblRodape_ISS_DC.Text                           = NotaCorretagem.Rodape.ISS_DC;
                lblRodape_IRRF_SobreOperacoes_DC.Text           = NotaCorretagem.Rodape.IRRFOperacoes_DC;

                //lblRodape_ValorLiquidoDasOperacoesPosNeg.Text   = NotaCorretagem.Rodape.ValorLiquidoOperacoesPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_TaxaDeRegistroPosNeg.Text             = NotaCorretagem.Rodape.TaxaDeRegistroPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_TaxaDeLiquidacaoPosNeg.Text           = NotaCorretagem.Rodape.TaxaLiquidacaoPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_Total_123_APosNeg.Text                = NotaCorretagem.Rodape.Total_123_APosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_TaxasTermo_Opcoes_FuturoPosNeg.Text   = NotaCorretagem.Rodape.TaxaTerOpcFutPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_TaxaANAPosNeg.Text                    = NotaCorretagem.Rodape.TaxaANAPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_EmolumentosPosNeg.Text                = NotaCorretagem.Rodape.EmolumentosPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_CorretagemPosNeg.Text                 = NotaCorretagem.Rodape.CorretagemPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_ISSPosNeg.Text                        = NotaCorretagem.Rodape.ISSPosNeg.ToString("N2", gCultureInfoBR);
                //lblRodape_IRRF_SobreOperacoesPosNeg.Text        = NotaCorretagem.Rodape.IRRFOperacoesPosNeg.ToString("N2", gCultureInfoBR);
            }
            else
            {
                this.pnlRelatorio.Visible = false;

                ExibirMensagemJsOnLoad("A", "Não há nota de corretagem disponível no período.");
            }
        }

        public string DefinirCorDoValor(object pParametro)
        {
            decimal lParametro = default(decimal);

            decimal.TryParse(pParametro.ToString(), out lParametro);

            if (lParametro > 0L)
            {
                return "ValorPositivo_Azul";
            }
            else if (lParametro < 0L)
            {
                return "ValorNegativo_Vermelho";
            }
            else
            {
                return string.Empty;
            }
        }

        private void CarregarDadosRelatorioBmf(Nullable<DateTime> DtMovimento)
        {
            var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

            var lResponse = lServicoAtivador.ConsultarNotaDeCorretagemBmf(
                    new NotaDeCorretagemExtratoRequest()
                    {
                        ConsultaCodigoCliente    = SessaoClienteLogado.CodigoBMF,
                        ConsultaCodigoCorretora  = gCodigoCorretora,
                        ConsultaDataMovimento    = (DtMovimento.HasValue) ? DtMovimento.Value : DataInicial,
                        ConsultaTipoDeMercado    = cboTipoMercado.SelectedValue,
                        ConsultaProvisorio       = false, // this.GetProvisorio,
                        ConsultaCodigoClienteBmf = SessaoClienteLogado.CodigoBMF,
                    });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.pnlRelatorioBmf.Visible = true;

                lResponse.RelatorioBmf.CabecalhoCliente = lResponse.Relatorio.CabecalhoCliente;
                lResponse.RelatorioBmf.CabecalhoCorretora = lResponse.Relatorio.CabecalhoCorretora;

                var lRelatorio = lResponse.RelatorioBmf;

                this.rptLinhasDoRelatorioBmf.DataSource = new TransporteExtratoNotaDeCorretagemBmf().TraduzirLista(lRelatorio.ListaNotaDeCorretagemExtratoBmfInfo);
                this.rptLinhasDoRelatorioBmf.DataBind();

                this.trNenhumItemBmf.Visible = lRelatorio.ListaNotaDeCorretagemExtratoBmfInfo.Count == 0;

                this.lblDataBmf.Text            = ((DtMovimento.HasValue) ? DtMovimento.Value : DataInicial).ToString("dd/MM/yyyy");
                this.lblCodigoClienteBmf.Text   = SessaoClienteLogado.CodigoBMF.ToString().PadLeft(10, '0');
                this.lblNomeClienteBmf.Text     = SessaoClienteLogado.Nome.ToStringFormatoNome();
                this.lblNumeroNotaBmf.Text      = lRelatorio.Rodape.NumeroDaNota;
                this.lblCpfCnpjClienteBmf.Text  = lRelatorio.Rodape.DsCpfCnpj;
                this.lblCodigoDoClienteBmf.Text = SessaoClienteLogado.CodigoBMF.ToString().PadLeft(10, '0');

                this.lblRodape_VendaDisponivel.Text  = Math.Abs(lRelatorio.Rodape.VendaDisponivel).ToString("N2");
                this.lblRodape_CompraDisponivel.Text = Math.Abs(lRelatorio.Rodape.CompraDisponivel).ToString("N2");
                this.lblRodape_VendaOpcoes.Text      = Math.Abs(lRelatorio.Rodape.VendaOpcoes).ToString("N2");
                this.lblRodape_CompraOpcoes.Text     = Math.Abs(lRelatorio.Rodape.CompraOpcoes).ToString("N2");
                this.lblRodape_ValorNegocio.Text     = Math.Abs(lRelatorio.Rodape.ValorNegocios).ToString("N2");
                this.lblRodape_IRRF.Text             = Math.Abs(lRelatorio.Rodape.IRRF).ToString("N2");
                this.lblRodape_IRRFDayTrade.Text     = Math.Abs(lRelatorio.Rodape.IRRFDayTrade).ToString("N2");
                this.lblRodape_TaxaOperacional.Text  = Math.Abs(lRelatorio.Rodape.TaxaOperacional).ToString("N2");
                this.lblRodape_TaxaRegistroBmf.Text  = Math.Abs(lRelatorio.Rodape.TaxaRegistroBmf).ToString("N2");
                this.lblRodape_TaxasBmf.Text         = Math.Abs(lRelatorio.Rodape.TaxaBmf).ToString("N2");
                this.lblRodape_ISS.Text              = Math.Abs(lRelatorio.Rodape.ISS).ToString("N2");
                this.lblRodape_AjustePosicao.Text    = Math.Abs(lRelatorio.Rodape.AjustePosicao).ToString("N2");
                this.lblRodape_AjusteDayTrade.Text   = Math.Abs(lRelatorio.Rodape.AjusteDayTrade).ToString("N2");
                this.lblRodape_TotalDespesas.Text    = Math.Abs(lRelatorio.Rodape.TotalDespesas).ToString("N2");
                this.lblRodape_IRRFCorretagem.Text   = Math.Abs(lRelatorio.Rodape.IrrfCorretagem).ToString("N2");
                this.lblRodape_TotalContaNormal.Text = Math.Abs(lRelatorio.Rodape.TotalContaNormal).ToString("N2");
                this.lblRodape_TotalLiquido.Text     = Math.Abs(lRelatorio.Rodape.TotalLiquido).ToString("N2");
                this.lblRodape_TotalLiquidoNota.Text = Math.Abs(lRelatorio.Rodape.TotalLiquidoNota).ToString("N2");
            }
            else
            {
                this.pnlRelatorioBmf.Visible = false;
                ExibirMensagemJsOnLoad("A", "Não há nota de corretagem disponível no período.");
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /*
        private string RecuperarDataLiquidoPara()
        {
            string lTipoMercado = cboTipoMercado.SelectedValue;

            DateTime lDataFinal = DataFinal;

            DateTime lDataRetorno = (lTipoMercado == "OPC") ? lDataFinal.AddDays(1) : lDataFinal.AddDays(3);

            if ((lDataFinal.DayOfWeek == DayOfWeek.Wednesday)
            ||  (lDataFinal.DayOfWeek == DayOfWeek.Thursday)
            ||  (lDataFinal.DayOfWeek == DayOfWeek.Friday)
            )
            {
                lDataRetorno = lDataRetorno.AddDays(2); //--> Adicionando o período do final de semana.
            }

            return lDataRetorno.ToString("dd/MM/yyyy");
        }
        */

        public bool ValidaFiltro()
        {
            bool lRetorno = true;

            if (this.txtDataInicial.Text == "")
            {
                ExibirMensagemJsOnLoad("I", "Informe a data inicial para o filtro");
                return false;
                //lRetorno = false;
            }

            if (this.txtDataFinal.Text == "")
            {
                ExibirMensagemJsOnLoad("I", "Informe a data final para o filtro");
                return false;
                //lRetorno = false;
            }

            DateTime lDataInicial = new DateTime();
            DateTime lDataFinal   = new DateTime();

            try
            {
                lDataInicial = Convert.ToDateTime(this.txtDataInicial.Text);
                lDataFinal   = Convert.ToDateTime(this.txtDataFinal.Text);
            }
            catch
            {
                //lRetorno = false;
                ExibirMensagemJsOnLoad("I", "Insira uma data válida");
                return false;
            }

            //if (lDataInicial == lDataFinal)
            //{
            //    //ExibirMensagemJsOnLoad("I", "Data inicial e final estão iguais.");
                
            //    lDataInicial = new DateTime(lDataInicial.Year, lDataInicial.Month, lDataInicial.Day, 0, 0, 1);
            //    lDataFinal = new DateTime(lDataInicial.Year, lDataInicial.Month, lDataInicial.Day, 23, 59, 59);

            //    return false;
            //}

            if (lDataInicial > lDataFinal)
            {
                ExibirMensagemJsOnLoad("I", "Data inicial inválida");
                return false;
            }

            double lTotalDays = (lDataFinal - lDataInicial).TotalDays;

            if (lTotalDays > 31)
            {
                //lRetorno = false;
                ExibirMensagemJsOnLoad("I", "Insira um filtro de data de menos de 31 dias");
                return false;
            }

            return lRetorno;
        }

        private byte[] GerarRelatorio(DateTime pDataCorretagem, 
            string pNumeroNotaCorretagem,
            string pCaminhoRelatorio, 
            string pCaminhoDoArquivo,
            List<TransporteExtratoNotaDeCorretagem> pCorpoNota,
            NotaDeCorretagemExtratoInfo pNotaInfo, out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            //Endereço
            lReport.ReportPath = pCaminhoRelatorio;

            //Parametro
            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamDataNotaCorretagem = new ReportParameter("pDataNotaCorretagem", pDataCorretagem.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamDataNotaCorretagem);

            ReportParameter lParamCliente            = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoPrincipal + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamNumeroNota         = new ReportParameter("pNumeroNota", pNumeroNotaCorretagem);
            lParametros.Add(lParamNumeroNota);

            ReportParameter lParamDebenteures = new ReportParameter("pDebentures", pNotaInfo.Rodape.Debentures.ToString("N2"));
            lParametros.Add(lParamDebenteures);

            ReportParameter lParamVendaVista = new ReportParameter("pVendasVista", pNotaInfo.Rodape.VendaVista.ToString("N2"));
            lParametros.Add(lParamVendaVista);

            ReportParameter lParamCompraVista = new ReportParameter("pComprasVista", pNotaInfo.Rodape.CompraVista.ToString("N2"));
            lParametros.Add(lParamCompraVista);

            ReportParameter lParamOpcoesCompra = new ReportParameter("pOpcoesCompras", pNotaInfo.Rodape.CompraOpcoes.ToString("N2"));
            lParametros.Add(lParamOpcoesCompra);

            ReportParameter lParamOpcoesVenda = new ReportParameter("pOpcoesVendas", pNotaInfo.Rodape.VendaOpcoes.ToString("N2"));
            lParametros.Add(lParamOpcoesVenda);

            ReportParameter lParamOperacoesTermo = new ReportParameter("pOperacoesTermo", pNotaInfo.Rodape.OperacoesTermo.ToString("N2"));
            lParametros.Add(lParamOperacoesTermo);

            ReportParameter lParamOperacoesFuturo = new ReportParameter("pOperacoesFuturo", pNotaInfo.Rodape.OperacoesFuturo.ToString("N2"));
            lParametros.Add(lParamOperacoesFuturo);

            ReportParameter lParamOperacoesTitulosPublicos = new ReportParameter("pValorOperacoesTituloPublico", pNotaInfo.Rodape.OperacoesTitulosPublicos.ToString("N2"));
            lParametros.Add(lParamOperacoesTitulosPublicos);

            ReportParameter lParampValorOperacoes = new ReportParameter("pValorOperacoes", pNotaInfo.Rodape.ValorDasOperacoes.ToString("N2"));
            lParametros.Add(lParampValorOperacoes);

            ReportParameter lParamppValorAjusteFuturo = new ReportParameter("pValorAjusteFuturo", pNotaInfo.Rodape.ValorAjusteFuturo.ToString("N2"));
            lParametros.Add(lParamppValorAjusteFuturo);

            ReportParameter lParamIrSobreCorretagem = new ReportParameter("pIrSobreCorretagem", pNotaInfo.Rodape.IRSobreCorretagem.ToString("N2"));
            lParametros.Add(lParamIrSobreCorretagem);

            ReportParameter lParamIRRFSobreDayTrade = new ReportParameter("pIrrfSobreDayTrade", pNotaInfo.Rodape.IRRFSobreDayTrade.ToString("N2"));
            lParametros.Add(lParamIRRFSobreDayTrade);

            ReportParameter lParamIRRFSobreDayTradeBase = new ReportParameter("pIrrfSobreDayTradeBase", pNotaInfo.Rodape.IRRFSobreDayTradeBase.ToString("N2"));
            lParametros.Add(lParamIRRFSobreDayTradeBase);

            ReportParameter lParamIRRFSobreDayTradeBaseProjecao = new ReportParameter("pIrrfSobreDayTradeProjecao", pNotaInfo.Rodape.IRRFSobreDayTradeProjecao.ToString("N2"));
            lParametros.Add(lParamIRRFSobreDayTradeBaseProjecao);

            ReportParameter lParamValorLiquidoOperacoes = new ReportParameter("pValorLiquidoOperacoes", pNotaInfo.Rodape.ValorLiquidoOperacoes.ToString("N2"));
            lParametros.Add(lParamValorLiquidoOperacoes);

            ReportParameter lParamTaxaDeRegistro = new ReportParameter("pTaxaRegistro", pNotaInfo.Rodape.TaxaDeRegistro.ToString("N2"));
            lParametros.Add(lParamTaxaDeRegistro);

            ReportParameter lParamTaxaLiquidacao = new ReportParameter("pTaxaLiquidacao", pNotaInfo.Rodape.TaxaLiquidacao.ToString("N2"));
            lParametros.Add(lParamTaxaLiquidacao);

            ReportParameter lParamTotal_123_A = new ReportParameter("pTotal123A", pNotaInfo.Rodape.Total_123_A.ToString("N2"));
            lParametros.Add(lParamTotal_123_A);

            ReportParameter lParamTaxaTerOpcFut = new ReportParameter("pTaxaTermoOpcoesFuturo", pNotaInfo.Rodape.TaxaTerOpcFut.ToString("N2"));
            lParametros.Add(lParamTaxaTerOpcFut);

            ReportParameter lParamTaxaANA = new ReportParameter("pTaxaAna", pNotaInfo.Rodape.TaxaANA.ToString("N2"));
            lParametros.Add(lParamTaxaANA);

            ReportParameter lParamEmolumentos = new ReportParameter("pEmolumentos", pNotaInfo.Rodape.Emolumentos.ToString("N2"));
            lParametros.Add(lParamEmolumentos);

            ReportParameter lParamTotalBolsaB = new ReportParameter("pTotalBolsaB", pNotaInfo.Rodape.TotalBolsaB.ToString("N2"));
            lParametros.Add(lParamTotalBolsaB);

            ReportParameter lParamCorretagem = new ReportParameter("pCorretagem", pNotaInfo.Rodape.Corretagem.ToString("N2"));
            lParametros.Add(lParamCorretagem);

            ReportParameter lParamISS = new ReportParameter("pISS", pNotaInfo.Rodape.ISS.ToString("N2"));
            lParametros.Add(lParamISS);

            ReportParameter lParamIrrfSemOperacoes = new ReportParameter("pIrrfSemOperacoes", pNotaInfo.Rodape.IRRFSemOperacoesValor.ToString("N2"));
            lParametros.Add(lParamIrrfSemOperacoes);

            ReportParameter lParamOutras = new ReportParameter("pOutras", pNotaInfo.Rodape.Outras.ToString("N2"));
            lParametros.Add(lParamOutras);

            ReportParameter lParamLiquidoAB = new ReportParameter("pLiquidoAB", pNotaInfo.Rodape.ValorLiquidoNota.ToString("N2"));
            lParametros.Add(lParamLiquidoAB);

            ReportParameter lParamCpfCnpjCliente = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj);
            lParametros.Add(lParamCpfCnpjCliente);

            ReportDataSource lSource = new ReportDataSource("ENotaCorretagem", pCorpoNota);

            lReport.DataSources.Add(lSource);

            lReport.SetParameters(lParametros);

            string lReportType,  lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>9.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>0.5in</MarginLeft> <MarginRight>0.5in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out pMymeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            return lRenderedBytes;
        }

        private byte[] GerarRelatorioBmf(DateTime pDataCorretagem,
            string pNumeroNotaCorretagem,
            string pCaminhoRelatorio,
            string pCaminhoDoArquivo,
            NotaDeCorretagemExtratoResponse lResponse,
            out string pMymeType)
        {
            string lNome = base.SessaoClienteLogado.Nome;

            LocalReport lReport = new LocalReport();

            //Endereço
            lReport.ReportPath = pCaminhoRelatorio;

            var lRelatorio = lResponse.RelatorioBmf;

            var lTrans = new TransporteExtratoNotaDeCorretagemBmf().TraduzirLista(lRelatorio.ListaNotaDeCorretagemExtratoBmfInfo);

            //Parametro
            List<ReportParameter> lParametros = new List<ReportParameter>();

            ReportParameter lParamDataNotaCorretagem = new ReportParameter("pDataNotaCorretagem", pDataCorretagem.ToString("dd/MM/yyyy"));
            lParametros.Add(lParamDataNotaCorretagem);

            ReportParameter lParamCliente = new ReportParameter("pCliente", base.SessaoClienteLogado.CodigoBMF + "-" + base.SessaoClienteLogado.Nome);
            lParametros.Add(lParamCliente);

            ReportParameter lParamCpfCnpj = new ReportParameter("pCpfCnpj", base.SessaoClienteLogado.CpfCnpj );
            lParametros.Add(lParamCpfCnpj);

            ReportParameter lParamNumeroNota = new ReportParameter("pNumeroNota", pNumeroNotaCorretagem);
            lParametros.Add(lParamNumeroNota);

            ReportParameter lParamVendaDisponivel = new ReportParameter("pVendaDisponivel", lRelatorio.Rodape.VendaDisponivel.ToString("N2"));
            lParametros.Add(lParamVendaDisponivel);

            ReportParameter lParamCompraDisponivel = new ReportParameter("pCompraDisponivel", lRelatorio.Rodape.CompraDisponivel.ToString("N2"));
            lParametros.Add(lParamCompraDisponivel);

            ReportParameter lParamVendaOpcoes = new ReportParameter("pVendaOpcoes", lRelatorio.Rodape.VendaOpcoes.ToString("N2"));
            lParametros.Add(lParamVendaOpcoes);

            ReportParameter lParamCompraOpcoes = new ReportParameter("pCompraOpcoes", lRelatorio.Rodape.CompraOpcoes.ToString("N2"));
            lParametros.Add(lParamCompraOpcoes);

            ReportParameter lParamValorNegocios = new ReportParameter("pValorNegocios", lRelatorio.Rodape.ValorNegocios.ToString("N2"));
            lParametros.Add(lParamValorNegocios);

            ReportParameter lParamIRRF = new ReportParameter("pIRRF", lRelatorio.Rodape.IRRF.ToString("N2"));
            lParametros.Add(lParamIRRF);

            ReportParameter lParamIRRFDayTrade = new ReportParameter("pIRRFDayTrade", lRelatorio.Rodape.IRRFDayTrade.ToString("N2"));
            lParametros.Add(lParamIRRFDayTrade);

            ReportParameter lParamTaxaOperacional = new ReportParameter("pTaxaOperacional", lRelatorio.Rodape.TaxaOperacional.ToString("N2"));
            lParametros.Add(lParamTaxaOperacional);

            ReportParameter lParampTaxaRegistroBmf = new ReportParameter("pTaxaRegistroBmf", lRelatorio.Rodape.TaxaRegistroBmf.ToString("N2"));
            lParametros.Add(lParampTaxaRegistroBmf);

            ReportParameter lParamTaxaBmf = new ReportParameter("pTaxaBmf", lRelatorio.Rodape.TaxaBmf.ToString("N2"));
            lParametros.Add(lParamTaxaBmf);

            ReportParameter lParamISS = new ReportParameter("pISS", lRelatorio.Rodape.ISS.ToString("N2"));
            lParametros.Add(lParamISS);

            ReportParameter lParamAjusteDayTrade = new ReportParameter("pAjusteDayTrade", lRelatorio.Rodape.AjusteDayTrade.ToString("N2"));
            lParametros.Add(lParamAjusteDayTrade);

            ReportParameter lParamAjustePosicao = new ReportParameter("pAjustePosicao", lRelatorio.Rodape.AjustePosicao.ToString("N2"));
            lParametros.Add(lParamAjustePosicao);

            ReportParameter lParamTotalDespesas = new ReportParameter("pTotalDespesas", lRelatorio.Rodape.TotalDespesas.ToString("N2"));
            lParametros.Add(lParamTotalDespesas);

            ReportParameter lParamIrrfCorretagem = new ReportParameter("pIrrfCorretagem", lRelatorio.Rodape.IrrfCorretagem.ToString("N2"));
            lParametros.Add(lParamIrrfCorretagem);

            ReportParameter lParamTotalContaNormal = new ReportParameter("pTotalContaNormal", lRelatorio.Rodape.TotalContaNormal.ToString("N2"));
            lParametros.Add(lParamTotalContaNormal);

            ReportParameter lParamTotalLiquido = new ReportParameter("pTotalLiquido", lRelatorio.Rodape.TotalLiquido.ToString("N2"));
            lParametros.Add(lParamTotalLiquido);

            ReportParameter lParamTotalLiquidoNota = new ReportParameter("pTotalLiquidoNota", lRelatorio.Rodape.TotalLiquidoNota.ToString("N2"));
            lParametros.Add(lParamTotalLiquidoNota);

            ReportDataSource lSource = new ReportDataSource("ENotaCorretagem", lTrans);

            lReport.DataSources.Add(lSource);

            lReport.SetParameters(lParametros);

            string lReportType, lEncoding, lFileNameExtension, lFileName, lDeviceInfo;

            Warning[] lWarnings;
            string[] lStreams;
            byte[] lRenderedBytes;

            lReportType = "PDF";
            lFileName = pCaminhoDoArquivo;

            lDeviceInfo =
            "<DeviceInfo> <OutputFormat>PDF</OutputFormat> <PageWidth>9.5in</PageWidth> <PageHeight>11in</PageHeight> <MarginTop>0.5in</MarginTop> <MarginLeft>0.5in</MarginLeft> <MarginRight>0.5in</MarginRight> <MarginBottom>0.5in</MarginBottom> </DeviceInfo>";

            //Render the report
            lRenderedBytes = lReport.Render(lReportType, lDeviceInfo, out pMymeType, out lEncoding, out lFileNameExtension, out lStreams, out lWarnings);

            return lRenderedBytes;
        }

        private Dictionary<string,byte[]> GeraPdfBovespa(out string lNomeDoArquivo, out string lMimeType)
        {
            lMimeType             = "";
            lNomeDoArquivo        = "";
            byte[] lRenderedBytes = new byte[3000000];

            //List<byte> lStream = new List<byte>();
            MemoryStream lStream = new MemoryStream();
            Dictionary<string, byte[]> lRetorno = new Dictionary<string, byte[]>();
            try
            {
                IServicoRelatoriosFinanceiros lServicoAtivador = InstanciarServicoDoAtivador<IServicoRelatoriosFinanceiros>();

                Panel lPanelPaginacao = pnlPaginacao;

                foreach (var control in lPanelPaginacao.Controls)
                {
                    if (control is Button)
                    {
                        if (((Button)control).CommandArgument == "" || 
                            ((Button)control).CommandArgument == "-1" || 
                            ((Button)control).CommandArgument == "+1") continue;

                        Nullable<DateTime> DtMovimento = ((Button)control).CommandArgument.DBToDateTime() ;// this.DataSelecionada;

                        OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest lRequest = new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest();
                        OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoResponse lResponse;

                        lRequest.ConsultaCodigoCliente   = CBLC;
                        lRequest.ConsultaCodigoCorretora = gCodigoCorretora;
                        lRequest.ConsultaDataMovimento   = (DtMovimento.HasValue) ? DtMovimento.Value : DataInicial;
                        lRequest.ConsultaTipoDeMercado   = cboTipoMercado.SelectedValue;
                        lRequest.ConsultaProvisorio      = false;

                        lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(lRequest);

                        if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                        {
                            var NotaCorretagem = lResponse.Relatorio;

                            List<TransporteExtratoNotaDeCorretagem> pCorpoNota = new List<TransporteExtratoNotaDeCorretagem>();

                            pCorpoNota = TransporteExtratoNotaDeCorretagem.TraduzirLista(NotaCorretagem.ListaNotaDeCorretagemExtratoInfo);

                            DateTime pDataCorretagem = lRequest.ConsultaDataMovimento;

                            string lNumeroNotaCorretagem = NotaCorretagem.CabecalhoCliente.NrNota.ToString();

                            lNomeDoArquivo = string.Format("NCBovespa_{0}_{1}", lNumeroNotaCorretagem, DateTime.Now.ToString("yyyyMMddHHmmss"));

                            byte[] lRenderTemp = this.GerarRelatorio(pDataCorretagem,
                                lNumeroNotaCorretagem,
                                Server.MapPath(@"..\Reports\NCBovespa.rdlc"),
                                lNomeDoArquivo,
                                pCorpoNota,
                                NotaCorretagem,
                                out lMimeType);

                            lRetorno.Add(lNomeDoArquivo, lRenderTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar PDFa Nota de Corretagem Bovespa.");
            }
            
            //return lRenderedBytes;
            return lRetorno;
        }

        private Dictionary<string, byte[]> GeraPdfBmf(out string lNomeDoArquivo, out string lMimeType)
        {
            lMimeType = "";
            lNomeDoArquivo = "";
            byte[] lRenderedBytes = null;
            Dictionary<string, byte[]> lRetorno= new Dictionary<string, byte[]>();

            try
            {
                var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();

                Panel lPanelPaginacao = ((Panel)pnlPaginacaoBmf) ;

                foreach (var control in lPanelPaginacao.Controls)
                {
                    if (control is Button)
                    {
                        if (((Button)control).CommandArgument == "" ||
                            ((Button)control).CommandArgument == "-1" ||
                            ((Button)control).CommandArgument == "+1") continue;

                        Nullable<DateTime> lDtMovimento = this.DataSelecionada;

                        var lResponse = lServicoAtivador.ConsultarNotaDeCorretagemBmf(
                            new NotaDeCorretagemExtratoRequest()
                            {
                                ConsultaCodigoCliente = SessaoClienteLogado.CodigoBMF,
                                ConsultaCodigoCorretora = gCodigoCorretora,
                                ConsultaDataMovimento = (lDtMovimento.HasValue) ? lDtMovimento.Value : DataInicial,
                                ConsultaTipoDeMercado = cboTipoMercado.SelectedValue,
                                ConsultaProvisorio = false, // this.GetProvisorio,
                                ConsultaCodigoClienteBmf = SessaoClienteLogado.CodigoBMF,
                            });

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            this.pnlRelatorioBmf.Visible = true;

                            lResponse.RelatorioBmf.CabecalhoCliente = lResponse.Relatorio.CabecalhoCliente;
                            lResponse.RelatorioBmf.CabecalhoCorretora = lResponse.Relatorio.CabecalhoCorretora;

                            var lRelatorio = lResponse.RelatorioBmf;

                            string lNumeroNotaCorretagem = lResponse.RelatorioBmf.Rodape.NumeroDaNota;

                            lNomeDoArquivo = string.Format("NCBmf_{0}_{1}", lNumeroNotaCorretagem, DateTime.Now.ToString("yyyyMMddHHmmss"));

                            lRenderedBytes =
                                this.GerarRelatorioBmf(!lDtMovimento.HasValue ? DataInicial : lDtMovimento.Value,
                                lNumeroNotaCorretagem,
                                Server.MapPath(@"..\Reports\NCBmf.rdlc"),
                                lNomeDoArquivo,
                                lResponse,
                                out lMimeType);

                            lRetorno.Add(lNomeDoArquivo, lRenderedBytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar PDFa Nota de Corretagem Bmf.");
            }
            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.ValidarSessao())
                {
                    if (!this.IsPostBack)
                    {
                        if (string.IsNullOrEmpty(base.SessaoClienteLogado.CodigoPrincipal))
                        {
                            base.ExibirMensagemJsOnLoad("I", "Você ainda não possui o código de conta na Gradual. <br/>Para acessar essa área, finalize seu Cadastro.");
                        }
                        //this.CarregarComboData();
                        //CarregarOperacoesClientes();
                    }
                    else
                    {
                        this.FormatarPaginacao(null);
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao carregar Nota de Corretagem.");
            }
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Financeiro";
            this.PaginaMaster.Crumb3Text = "Notas de Corretagem";
            
        }

        private void FormatarPaginacaoSetaEsquerda(Panel lPanelPaginacao)
        {
            Button btn = new Button();

            btn.ID = "btnPesquisar_Esquerda";

            btn.CommandArgument = "-1";

            btn.CssClass = "btn_paginacao_seta_e";

            btn.Text = "";

            btn.Command += new CommandEventHandler(btnPesquisar_Click);

            btn.Visible = true;

            lPanelPaginacao.Controls.Add(btn);
        }

        private void FormatarPaginacaoSetaDireita(Panel lPanelPaginacao)
        {
            Button btn = new Button();

            btn.ID = "btnPesquisar_Direita";

            btn.CommandArgument = "+1";

            btn.CssClass = "btn_paginacao_seta_d";

            btn.Text = "";

            btn.Command += new CommandEventHandler(btnPesquisar_Click);

            btn.Visible = true;

            lPanelPaginacao.Controls.Add(btn);
        }

        private void FormatarPaginacao(Nullable<DateTime> pDtMovimento)
        {
            try
            {
                if (this.DataFinal == DateTime.MinValue || this.DataInicial == DateTime.MinValue) return;

                if (this.DataInicial > this.DataFinal) return;

                DateTime dtFinal = this.DataFinal;
                DateTime dtInicial = this.DataInicial;

                double lTotalDays = (dtFinal - dtInicial).TotalDays;

                if (lTotalDays == 0) return;

                if (lTotalDays > 31)
                {
                    //lRetorno = false;
                    ExibirMensagemJsOnLoad("I", "Insira um filtro de data de menos de 31 dias");
                    return;
                }

                CarregarOperacoesClientes();
                
                int lCount = 0;

                string lMontaPaginacao = string.Empty;

                string lBolsa = this.cboBolsa.SelectedValue;

                Panel lPanelPaginacao = ((lBolsa.Equals("bov")) ? this.pnlPaginacao : this.pnlPaginacaoBmf);

                foreach (Control ctrl in lPanelPaginacao.Controls)
                {
                    if (ctrl is Button)
                    {
                        lPanelPaginacao.Controls.Remove(ctrl);
                    }
                }

                this.FormatarPaginacaoSetaEsquerda(lPanelPaginacao);

                List<DateTime> lListaNegociacoes = lBolsa.Equals("bov") ? ListaNegociacoesBov : ListaNegociacoesBmf;

                do
                {
                    if (lListaNegociacoes.Contains(dtInicial))
                    {
                        lCount++;

                        Button btn = new Button();

                        btn.ID = "btnPesquisar" + lCount;

                        btn.Command += new CommandEventHandler(btnPesquisar_Click);

                        btn.CommandArgument = dtInicial.ToString("dd/MM/yyyy");

                        btn.Text = lCount.ToString();

                        btn.CssClass = "btn_paginacao";

                        btn.Visible = true;

                        lPanelPaginacao.Controls.Add(btn);
                    }

                    dtInicial = dtInicial.AddDays(1);

                    if (dtInicial == this.DataFinal)
                    {
                        if (lListaNegociacoes.Contains(dtInicial))
                        {
                            lCount++;

                            Button btn = new Button();

                            btn.ID = "btnPesquisar" + lCount;

                            btn.Command += new CommandEventHandler(btnPesquisar_Click);

                            btn.CommandArgument = dtInicial.ToString("dd/MM/yyyy");

                            btn.Text = lCount.ToString();

                            btn.Visible = true;

                            btn.CssClass = "btn_paginacao";

                            lPanelPaginacao.Controls.Add(btn);
                        }
                    }

                } while (dtInicial != this.DataFinal);

                this.FormatarPaginacaoSetaDireita(lPanelPaginacao);

                //Esconder Botões de setas
                if (lCount.Equals(0) || lCount.Equals(1))
                {
                    lPanelPaginacao.Controls.Remove(lPanelPaginacao.FindControl("btnPesquisar_Direita"));
                    lPanelPaginacao.Controls.Remove(lPanelPaginacao.FindControl("btnPesquisar_Esquerda"));
                }

                IServicoRelatoriosFinanceiros lServicoAtivador = InstanciarServicoDoAtivador<IServicoRelatoriosFinanceiros>();

                OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest  lRequest = new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest();
                OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoResponse lResponse;

                lRequest.ConsultaCodigoCliente   = CBLC;
                lRequest.ConsultaCodigoCorretora = gCodigoCorretora;
                lRequest.ConsultaDataMovimento   = DataInicial;
                lRequest.ConsultaTipoDeMercado   = cboTipoMercado.SelectedValue;
                lRequest.ConsultaProvisorio      = false;
            
                lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if ((lResponse.Relatorio.ListaNotaDeCorretagemExtratoInfo.Count > 0 || 
                        lResponse.RelatorioBmf.ListaNotaDeCorretagemExtratoBmfInfo.Count > 0) && lCount == 0)
                    {
                        Button btn = new Button();

                        btn.ID = "btnPesquisar1";

                        btn.Command += new CommandEventHandler(btnPesquisar_Click);

                        btn.CommandArgument = this.DataInicial.ToString("dd/MM/yyyy");

                        btn.Text = "1";

                        btn.Visible = true;

                        btn.CssClass = "btn_paginacao";

                        lPanelPaginacao.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao formatar paginação de Nota de Corretagem.", false, ex.StackTrace);
            }
        }

        protected void btnPesquisar_Click(object sender, CommandEventArgs e)
        {
            try
            {
                if (!this.ValidaFiltro()) return;

                string lBolsa = this.cboBolsa.SelectedValue;

                Panel lPanelPaginacao = ((lBolsa.Equals("bov")) ? pnlPaginacao : pnlPaginacaoBmf);

                this.pnlRelatorio.Visible    = lBolsa.Equals("bov");
                this.pnlRelatorioBmf.Visible = lBolsa.Equals("bmf");

                Nullable<DateTime> dtMovimento = null;

                int lCountBotao = 0;
                int lCountMaximo = 0;

                if (e.CommandArgument.ToString() == "")
                {
                    ViewState["BotaoSelecionado"] = null;
                    ViewState["DataSelecionado"] = null;

                    lCountBotao = 1;

                    Button lButtonSelecionado = lPanelPaginacao.FindControl("btnPesquisar" + lCountBotao) as Button;

                    foreach (Control ctrl in lPanelPaginacao.Controls)
                    {
                        if (ctrl is Button)
                        {
                            if (((Button)ctrl).Text != string.Empty && (((Button)ctrl).CssClass == "btn_paginacao" || ((Button)ctrl).CssClass == "btn_paginacao_ativo"))
                            {
                                ((Button)ctrl).CssClass = "btn_paginacao";
                                //break;
                            }
                        }
                    }

                    if (lButtonSelecionado != null)
                    {
                        lButtonSelecionado.CssClass = "btn_paginacao_ativo";
                        dtMovimento = lButtonSelecionado.CommandArgument.DBToDateTime();
                    }
                }

                if (this.BotaoSelecionadoPaginacao.HasValue)
                {
                    lCountBotao = this.BotaoSelecionadoPaginacao.Value;

                    Button lButtonSelecionado = lPanelPaginacao.FindControl("btnPesquisar" + lCountBotao) as Button;

                    lButtonSelecionado.CssClass = "btn_paginacao";
                }

                ViewState["BotaoSelecionado"] = lCountBotao;
                ViewState["DataSelecionado"] = dtMovimento.ToString();

                if (e.CommandArgument.ToString() == "-1" || e.CommandArgument.ToString() == "+1")
                {
                    foreach (Control ctrl in lPanelPaginacao.Controls)
                    {
                        if (ctrl is Button)
                        {
                            if (((Button)ctrl).Text != string.Empty)
                            {
                                lCountMaximo++;
                            }
                        }
                    }

                    lCountBotao = lCountBotao != 0 ? lCountBotao + int.Parse(e.CommandArgument.ToString()) : lCountBotao;

                    lCountBotao = lCountBotao == 0 ? 1 : lCountBotao;

                    lCountBotao = lCountBotao > lCountMaximo ? lCountMaximo : lCountBotao;

                    Button lButtonSelecionado = lPanelPaginacao.FindControl("btnPesquisar" + lCountBotao) as Button;

                    if (lButtonSelecionado != null)
                    {
                        lButtonSelecionado.CssClass = "btn_paginacao_Ativo";

                        dtMovimento = Convert.ToDateTime(lButtonSelecionado.CommandArgument);

                        ViewState["BotaoSelecionado"] = lButtonSelecionado.Text;
                        ViewState["DataSelecionado"] = dtMovimento.ToString();
                    }
                }
                else if (((Button)sender).Text != "Visualizar")
                {
                    dtMovimento = Convert.ToDateTime(e.CommandArgument);

                    foreach (Control ctrl in lPanelPaginacao.Controls)
                    {
                        if (ctrl is Button )
                        {
                            if (((Button)ctrl).Text != string.Empty && (((Button)ctrl).CssClass == "btn_paginacao" || ((Button)ctrl).CssClass == "btn_paginacao_ativo"))
                            {
                                ((Button)ctrl).CssClass = "btn_paginacao";
                                //break;
                            }
                        }
                    }

                    ((Button)sender).CssClass = "btn_paginacao_ativo";

                    ViewState["BotaoSelecionado"] = ((Button)sender).Text;
                    ViewState["DataSelecionado"] = dtMovimento.ToString();
                }

                if (lBolsa.Equals("bov"))
                {
                    this.CarregarDadosRelatorio(dtMovimento);
                }
                else if (lBolsa.Equals("bmf"))
                {
                    this.CarregarDadosRelatorioBmf(dtMovimento);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao carregar Nota de Corretagem.", false, ex.StackTrace);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboBolsa.SelectedValue.Equals("bov"))
                {
                    this.ResponderArquivoCSV();
                }
                else if (cboBolsa.SelectedValue.Equals("bmf"))
                {
                    this.ResponderArquivoBmfCSV();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao carregar Nota de Corretagem.");
            }
        }

        protected void btnImprimirPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var lBase = this.Page as PaginaBase;

                string lBolsa = this.cboBolsa.SelectedValue;

                string lNomeDoArquivo = string.Empty;

                string lMimeType = string.Empty;

                Dictionary<string,byte[]> lListBytes = (lBolsa.Equals("bov") ? GeraPdfBovespa(out lNomeDoArquivo, out lMimeType) : GeraPdfBmf(out lNomeDoArquivo, out lMimeType));

                //Clear the response stream and write the bytes to the outputstream  //Set content-disposition to "attachment" so that user is prompted to take an action  //on the file (open or save)
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/zip";// lMimeType;

                ZipFile zip = new ZipFile();

                foreach (KeyValuePair<string,byte[]> lBytes in lListBytes)
                {
                    MemoryStream lStream = new MemoryStream();
                    lStream.Write(lBytes.Value, 0, lBytes.Value.Length);
                    zip.AddFileStream(lBytes.Key + ".pdf", "", lStream);
                }

                Response.AppendHeader("content-disposition", "attachment; filename=" + "NotaDeCorretagem.zip");
                zip.TempFileFolder = Server.MapPath("~/Resc/Upload/");
                zip.Save(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao gerar a Nota de Corretagem.");
            }
            
        }

        protected void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                var lBase = this.Page as PaginaBase;

                string lBolsa = this.cboBolsa.SelectedValue;

                string lNomeDoArquivo = string.Empty;

                string lMimeType = string.Empty;

                Dictionary<string, byte[]> lListBytes = (lBolsa.Equals("bov") ? GeraPdfBovespa(out lNomeDoArquivo, out lMimeType) : GeraPdfBmf(out lNomeDoArquivo, out lMimeType));

                ZipFile zip = new ZipFile();

                foreach (KeyValuePair<string, byte[]> lBytes in lListBytes)
                {
                    MemoryStream lStream = new MemoryStream();
                    lStream.Write(lBytes.Value, 0, lBytes.Value.Length);
                    zip.AddFileStream(lBytes.Key + ".pdf", "", lStream);
                }
                zip.TempFileFolder = Server.MapPath("~/Resc/Upload/");
                if (File.Exists(zip.TempFileFolder + "NotaDeCorretagem.zip"))
                {
                    File.Delete(zip.TempFileFolder + "NotaDeCorretagem.zip");
                }
                zip.Save(zip.TempFileFolder + "NotaDeCorretagem.zip");
                

                //File.Move(zip.Name, zip.TempFileFolder + zip.Name);

                var lAnexos = new List<OMS.Email.Lib.EmailAnexoInfo>();
                EmailAnexoInfo lEmailInfo = new EmailAnexoInfo();
                lEmailInfo.Arquivo = File.ReadAllBytes(zip.Name); //  lRenderedBytes;
                lEmailInfo.Nome = zip.Name;
                lAnexos.Add(lEmailInfo);

                Dictionary<string, string> lVariaveis = new Dictionary<string, string>();
                //lBase.EnviarEmail("bribeiro@gradualinvestimentos.com.br", "Nota de Corretagem  - Gradual Investimentos", "NotaCorretagem.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos, null);
                lBase.EnviarEmail(base.SessaoClienteLogado.Email, "Nota de Corretagem  - Gradual Investimentos", "NotaCorretagem.htm", lVariaveis, Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos, lAnexos,null);

                base.ExibirMensagemJsOnLoad("I", "Um E-mail com o arquivo Pdf foi enviado para " + base.SessaoClienteLogado.Email);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
                base.ExibirMensagemJsOnLoad("E", "Erro ao Enviar por email a Nota de Corretagem." + ex.StackTrace);
            }
        }

        #endregion
    }
}