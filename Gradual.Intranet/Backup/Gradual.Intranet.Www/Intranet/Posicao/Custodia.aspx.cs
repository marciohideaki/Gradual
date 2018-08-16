using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using log4net;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class Custodia : PaginaBase
    {
        #region | Atributos

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades

        public string GetMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Mercado"]))
                    return null;

                return this.Request["Mercado"];
            }
        }

        public string GetBolsa
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Bolsa"]))
                    return null;

                return this.Request["Bolsa"];
            }
        }

        private int? GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!this.GetBolsa.Equals("bmf") && !int.TryParse(this.Request["CdBovespaCliente"], out lRetorno))
                    return null;

                if (lRetorno == default(int))
                    int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetCodBmfCliente
        {
            get
            {
                var lRetorno = default(int);

                if (this.GetBolsa.Equals("bmf") && !int.TryParse(this.Request["CdBmfCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetNomeCliente
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["NomeCliente"]))
                    return null;

                return this.Request["NomeCliente"];
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Acao"] == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
            else
            {
                this.CarregarHtml();
            }
        }

        #endregion

        #region | Métodos de Apoio

        private List<TransporteCustodiaInfo> CarregarRelatorio()
        {
            var lRetorno = new List<TransporteCustodiaInfo>();
            var lServico = Ativador.Get<IServicoRelatoriosFinanceiros>();

            var lResponse = lServico.ConsultarCustodia(new PosicaoCustodiaRequest()
            {
                 ConsultaCdClienteBMF = this.GetCodBmfCliente,
                  ConsultaCdClienteBovespa = this.GetCodCliente
            });

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Objeto.ListaMovimento != null)
                {   //--> Aplicando o filtro de mercado para consulta.
                    switch (this.GetMercado)
                    {
                        case "FUT":
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => "FUT".Equals(cci.TipoMercado.ToUpper()));
                            break;
                        case "OPC":
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => "OPC".Equals(cci.TipoMercado.ToUpper()));
                            break;
                        case "TER":
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => "TER".Equals(cci.TipoMercado.ToUpper()));
                            break;
                        case "VIS":
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => "VIS".Equals(cci.TipoMercado.ToUpper()));
                            break;
                    }

                    switch (this.GetBolsa)
                    {
                        case "bmf":
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => "BMF".Equals(cci.TipoGrupo));
                            break;

                        default:
                            lResponse.Objeto.ListaMovimento = lResponse.Objeto.ListaMovimento.FindAll(cci => !"BMF".Equals(cci.TipoGrupo));
                            break;
                    }

                    base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });
                }

                lRetorno = TransporteCustodiaInfo.TraduzirCustodiaInfo(lResponse.Objeto.ListaMovimento);

                this.RecuperarValoresUltimaCotacao(ref lRetorno);

                return lRetorno;
            }
            else
            {
                throw new Exception(string.Format("Erro do serviço ao obter custódias: {0} {1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }
        }

        private void ResponderArquivoCSV()
        {
            List<TransporteCustodiaInfo> lRelatorio = this.CarregarRelatorio();

            StringBuilder lBuilder = new StringBuilder();

            CultureInfo lCultureInfo = new CultureInfo("pt-BR");

            ///*
            // //Exemplo de arquivo csv para esse relatorio:
            // //RelatorioCustodia.csv:

            //    Relatório: Custódia
            //    Cliente:0000066359 -
            //    Merc    Código      Empresa     Cart. 	Qtd. Abertura 	Compra 	Venda 	Qtd. Atual 	F. Anterior 	Valor

            //    Subtotal (Mercado à Vista): 	0,00
            //    Subtotal (Mercado Futuro): 	0,00
            //    Subtotal (Mercado de Opções): 	0,00
            //    Subtotal (Mercado de Termo): 	0,00

            //    Totais: 	0,00
            // */

            double dSubTotalMercadoTermo = 0D;
            double dSubTotalMercadoAVista = 0D;
            double dSubTotalMercadoFuturo = 0D;
            double dSubTotalMercadoDeOpcoes = 0D;
            double dTotal = 0D;
            double dSubTotalTemp = 0D;

            lBuilder.AppendLine("Relatório: Custódia");

            lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", (!this.Request["Bolsa"].Equals("bmf")) ? this.GetCodBmfCliente.ToCodigoClienteFormatado() : this.GetCodCliente.ToCodigoClienteFormatado(), UsuarioLogado.Nome.ToStringFormatoNome());

            lBuilder.AppendLine("Merc;Código;Empresa;Cart.;Qtd.Abertura;Compra;Venda;Qtd. Atual;Projetado D1;Projetado D2;Projetado D3;F. Anterior;Cotação (R$);Variação (%);Valor");

            if (lRelatorio.Count > 0)
            {
                foreach (TransporteCustodiaInfo lItem in lRelatorio)
                {
                    dSubTotalTemp = lItem.Cotacao.DBToDouble() * Convert.ToInt32(lItem.QtdAtual);

                    lItem.Resultado = dSubTotalTemp.ToString();

                    if (lItem.TipoMercado.Equals("VIS"))

                        dSubTotalMercadoAVista += dSubTotalTemp;

                    else if (lItem.TipoMercado.Equals("TER"))

                        dSubTotalMercadoTermo += dSubTotalTemp;

                    else if (lItem.TipoMercado.Equals("OPC"))

                        dSubTotalMercadoDeOpcoes += dSubTotalTemp;

                    else if (lItem.TipoMercado.Equals("FUT"))

                        dSubTotalMercadoFuturo += dSubTotalTemp;

                    dTotal += dSubTotalTemp;

                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14}\r\n"
                                         , lItem.TipoMercado
                                         , lItem.CodigoNegocio
                                         , lItem.Empresa
                                         , lItem.Carteira
                                         , lItem.QtdAbertura
                                         , lItem.QtdCompra
                                         , lItem.QtdVenda
                                         , lItem.QtdAtual
                                         , lItem.QtdD1
                                         , lItem.QtdD2
                                         , lItem.QtdD3
                                         , lItem.ValorDeFechamento
                                         , lItem.Cotacao
                                         , lItem.Variacao
                                         , lItem.Resultado);
                }
            }
            else
            {
                lBuilder.AppendLine("(0 lançamentos encontrados)");
            }

            lBuilder.AppendFormat("Subtotal (Mercado à Vista):;{0}\r\n", dSubTotalMercadoAVista.ToString("N2", lCultureInfo));

            lBuilder.AppendFormat("Subtotal (Mercado Futuro):;{0}\r\n", dSubTotalMercadoFuturo.ToString("N2", lCultureInfo));

            lBuilder.AppendFormat("Subtotal (Mercado de Opções):;{0}\r\n", dSubTotalMercadoDeOpcoes.ToString("N2", lCultureInfo));

            lBuilder.AppendFormat("Subtotal (Mercado de Termo):;{0}\r\n", dSubTotalMercadoTermo.ToString("N2", lCultureInfo));

            lBuilder.AppendFormat("Totais:;{0}\r\n", dTotal.ToString("N2", lCultureInfo));

            this.Response.Clear();

            this.Response.ContentType = "text/csv";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", "attachment;filename=RelatorioDeCustodia.csv");

            this.Response.Write(lBuilder.ToString());

            this.Response.End();
        }

        private void CarregarHtml()
        {
            var lCultureInfo = new CultureInfo("pt-BR");

            double lSubTotalMercadoTermo = 0D;
            double lSubTotalMercadoAVista = 0D;
            double ldSubTotalMercadoFuturo = 0D;
            double lSubTotalMercadoDeOpcoes = 0D;
            double lTotal = 0D;
            double lSubTotalTemp = 0D;

            var lCustodia = this.CarregarRelatorio();

            this.lblCodigoCliente.Text = this.GetCodCliente.ToCodigoClienteFormatado(); //lRespostaBusca.Relatorio.CodigoCliente.ToString().PadLeft(10, '0');
            this.lblNomeCliente.Text = this.GetNomeCliente;

            //if (!GetBolsa.Equals("bmf"))
            //    RecuperarValoresUltimaCotacao(ref lRetorno);

            this.rptLinhasDoRelatorio.DataSource = lCustodia;
            this.rptLinhasDoRelatorio.DataBind();

            lCustodia.ForEach(delegate(TransporteCustodiaInfo info)
            {
                if (info.Cotacao != "n/d")
                    lSubTotalTemp = Convert.ToDouble(info.Cotacao) * Convert.ToInt32(info.QtdAtual);
                else if (info.TipoGrupo == "TEDI")
                    lSubTotalTemp = Convert.ToDouble(info.ValorPosicao, new CultureInfo("pt-BR"));
                else
                    lSubTotalTemp = 0;

                info.Resultado = lSubTotalTemp.ToString();

                if (info.TipoMercado.Equals("VIS"))
                    lSubTotalMercadoAVista += lSubTotalTemp;

                else if (info.TipoMercado.Equals("TER"))
                    lSubTotalMercadoTermo += lSubTotalTemp;

                else if (info.TipoMercado.Equals("OPC"))
                    lSubTotalMercadoDeOpcoes += lSubTotalTemp;

                else if (info.TipoMercado.Equals("FUT"))
                    ldSubTotalMercadoFuturo += lSubTotalTemp;

                lTotal += lSubTotalTemp;
            });

            this.lblSubTotalMercadoTermo.Text = lSubTotalMercadoTermo.ToString("N2", lCultureInfo);
            this.lblSubTotalMercadoAVista.Text = lSubTotalMercadoAVista.ToString("N2", lCultureInfo);
            this.lblSubTotalMercadoFuturo.Text = ldSubTotalMercadoFuturo.ToString("N2", lCultureInfo);
            this.lblSubTotalMercadoDeOpcoes.Text = lSubTotalMercadoDeOpcoes.ToString("N2", lCultureInfo);

            this.lblTotal.Text = lTotal.ToString("N2", lCultureInfo);
        }

        private void RecuperarValoresUltimaCotacao(ref List<TransporteCustodiaInfo> pListaCustodias)
        {
            if (pListaCustodias != null)
            {
                Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio lCotacao = null;

                var lServico = this.InstanciarServico<IServicoCotacao>();

                foreach (TransporteCustodiaInfo lTransporteCustodia in pListaCustodias)
                {
                    try
                    {
                        if (!lTransporteCustodia.TipoGrupo.Equals("TEDI"))
                        {
                            lCotacao = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio(lServico.ReceberTickerCotacao(lTransporteCustodia.CodigoNegocio));

                            lTransporteCustodia.ValorDeFechamento = lCotacao.ValorFechamento;

                            lTransporteCustodia.Cotacao = Convert.ToDecimal(lCotacao.Preco).ToString("n");

                            lTransporteCustodia.Variacao = lCotacao.Variacao;
                        }

                        try
                        {
                            if (lTransporteCustodia.TipoGrupo.Equals("TEDI"))
                                lTransporteCustodia.Resultado = lTransporteCustodia.ValorPosicao;
                            else
                                lTransporteCustodia.Resultado = ((double)double.Parse(lCotacao.Preco) * double.Parse(lTransporteCustodia.QtdAtual)).ToString("n");
                        }
                        catch (Exception ex)
                        {
                            gLogger.Error("Erro na página Custodia.aspx", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        gLogger.Error("Erro em RecuperarValoresUltimaCotacao() na página Custodia.aspx", ex);
                    }
                }
            }
        }

        #endregion
    }
}
