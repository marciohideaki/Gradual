using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Gradual.OMS.TesouroDireto.App_Codigo;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Compra;
using log4net;

namespace Gradual.OMS.TesouroDireto.AcessoWS
{
    public class TesouroDiretoCompra : ObjetosBase
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CompraVerificacaoDeCondicaoDeCompraResponse CompraVerificarCondicaoDeCompra(CompraVerificacaoDeCondicaoDeCompraRequest pParametro)
        {
            var lRetorno = new CompraVerificacaoDeCondicaoDeCompraResponse();

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraVerifCondCompra(pParametro.ConsultaCPFNegociador, pParametro.ConsultaMercado);

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraVerifCondCompra(ConsultaCPFNegociador [{0}], ConsultaMercado [{1}]):\r\n{2}"
                                    , pParametro.ConsultaCPFNegociador
                                    , pParametro.ConsultaMercado
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("MERCADOS") != null)
                {
                    XElement elemMercado = root.Element("MERCADOS").Element("MERCADO");

                    if (elemMercado == null)
                        return lRetorno;

                    lRetorno.DataInicial = elemMercado.Element("DATA_INICIAL") != null ? elemMercado.Element("DATA_INICIAL").Value.DBToDateTime() : DateTime.MaxValue;
                    lRetorno.DataFinal = elemMercado.Element("DATA_FINAL") != null ? elemMercado.Element("DATA_FINAL").Value.DBToDateTime() : DateTime.MaxValue;
                    lRetorno.DataProrrogacao = elemMercado.Element("DATA_PRORROGACAO") != null ? elemMercado.Element("DATA_PRORROGACAO").Value.DBToDateTime() : DateTime.MaxValue;
                    lRetorno.IdProrrogacao = elemMercado.Element("ID_PRORROGACAO") != null ? elemMercado.Element("ID_PRORROGACAO").Value.DBToInt32() : 0;
                    lRetorno.Suspenso = elemMercado.Element("SUSPENSO") != null ? elemMercado.Element("SUSPENSO").Value.DBToInt32() : 0;

                    if ((elemMercado.Element("MERCADO") != null) && (elemMercado.Element("MERCADO").Value != ""))
                        lRetorno.CodigoMercado = elemMercado.Element("MERCADO").Value.DBToInt32();
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraVerificarCondicaoDeCompra", ex);
            }

            return lRetorno;
        }

        public CompraConsultaTituloMercadoResponse CompraConsultarTituloMercado(CompraConsultaTituloMercadoRequest pParametro)
        {
            var lRetorno = new CompraConsultaTituloMercadoResponse();
            TituloMercadoInfo tituloMercadoInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraConsTitMercado(pParametro.CodigoMercado
                                                                     , pParametro.CodigoTitulo
                                                                     , pParametro.Tipo
                                                                     , pParametro.DataEmissao.DBToDateTimeString()
                                                                     , pParametro.DataVencimento.DBToDateTimeString()
                                                                     , pParametro.TipoIndexador
                                                                     , pParametro.SELIC.ToString()
                                                                     , pParametro.ISIN
                                                                     , pParametro.NotCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraConsTitMercado(CodigoMercado [{0}], CodigoTitulo [{1}], Tipo [{2}], DataEmissao [{3}], DataVencimento [{4}], TipoIndexador [{5}], SELIC [{6}], ISIN [{7}], NotCesta [{8}]):\r\n{9}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CodigoTitulo
                                    , pParametro.Tipo
                                    , pParametro.DataEmissao
                                    , pParametro.DataVencimento
                                    , pParametro.TipoIndexador
                                    , pParametro.SELIC
                                    , pParametro.ISIN
                                    , pParametro.NotCesta
                                    , lXml);

                base.AtribDefaultValues();

                XElement root = null;

                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS_MERCADO") != null)
                {
                    XElement elemTitulosMercado = root.Element("TITULOS_MERCADO");

                    foreach (XElement item in elemTitulosMercado.Elements())
                    {
                        tituloMercadoInfo = new TituloMercadoInfo();
                        tituloMercadoInfo.Mercado = item.Element("MERCADO") != null ? item.Element("MERCADO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.CodigoTitulo = item.Element("CODIGO_TITULO") != null ? item.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.NomeTitulo = item.Element("NOME_TITULO") != null ? item.Element("NOME_TITULO").Value : "";

                        if (item.Element("TIPO") != null)
                        {
                            tituloMercadoInfo.Tipo = new CodigoNomeInfo();
                            tituloMercadoInfo.Tipo.Codigo = item.Element("TIPO").Element("CODIGO").Value;
                            tituloMercadoInfo.Tipo.Nome = item.Element("TIPO").Element("NOME").Value;
                        }

                        tituloMercadoInfo.SELIC = item.Element("SELIC") != null ? item.Element("SELIC").Value : "";
                        tituloMercadoInfo.ISIN = item.Element("ISIN") != null ? item.Element("SELIC").Value : "";
                        tituloMercadoInfo.DataEmissao = item.Element("DATA_EMISSAO") != null ? item.Element("DATA_EMISSAO").Value.DBToDateTime() : DateTime.MinValue;
                        tituloMercadoInfo.DataVencimento = item.Element("DATA_VENCIMENTO") != null ? item.Element("DATA_VENCIMENTO").Value.DBToDateTime() : DateTime.MinValue;
                        tituloMercadoInfo.QuantidadeDisponivelCompra = item.Element("QUANTIDADE_DISPONIVEL_COMPRA") != null ? item.Element("QUANTIDADE_DISPONIVEL_COMPRA").Value.DBToInt32() : 0;
                        tituloMercadoInfo.QuantidadeDisponivelVenda = item.Element("QUANTIDADE_DISPONIVEL_VENDA") != null ? item.Element("QUANTIDADE_DISPONIVEL_VENDA").Value.DBToInt32() : 0;
                        tituloMercadoInfo.ValorBase = item.Element("VALOR_BASE") != null ? item.Element("VALOR_BASE").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorCompra = item.Element("VALOR_COMPRA") != null ? item.Element("VALOR_COMPRA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorTaxaCompra = item.Element("VALOR_TAXA_COMPRA") != null ? item.Element("VALOR_TAXA_COMPRA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorVenda = item.Element("VALOR_VENDA") != null ? item.Element("VALOR_VENDA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorTaxaVenda = item.Element("VALOR_TAXA_VENDA") != null ? item.Element("VALOR_TAXA_VENDA").Value.DBToDecimal() : 0;

                        if (item.Element("INDEXADOR") != null)
                        {
                            tituloMercadoInfo.Indexador = new CodigoNomeInfo();
                            tituloMercadoInfo.Indexador.Codigo = item.Element("INDEXADOR").Element("CODIGO") != null ? item.Element("INDEXADOR").Element("CODIGO").Value : "";
                            tituloMercadoInfo.Indexador.Nome = item.Element("INDEXADOR").Element("CODIGO") != null ? item.Element("INDEXADOR").Element("NOME").Value : "";
                        }

                        lRetorno.Titulos.Add(tituloMercadoInfo);
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Titulos && lRetorno.Titulos.Count > 0)
                            lRetorno.Titulos.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorTitulo + "\n"; });

                        gLogger.DebugFormat("COMPRA - CONSULTAR TITULO -->n - CodigoMercado: {0}\n - Títulos: {1}", pParametro.CodigoMercado, lTitulos);
                    }
                }
                lRetorno.DescricaoResposta = string.Concat("Registros encontrados: ", lRetorno.Titulos.Count.ToString());
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraConsultarTituloMercado", ex);
            }

            return lRetorno;
        }

        public CompraConsultaCestaResponse CompraConsultarCesta(CompraConsultaCestaRequest pParametro)
        {
            var lRetorno = new CompraConsultaCestaResponse();
            CompraConsultaCestaInfo compraConsultaCestaInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraConsCesta(pParametro.ConsultaMercado, pParametro.ConsultaNegociadorCPF, pParametro.ConsultaCodigoCesta, pParametro.ConsultaDataCompra.ToString(), pParametro.ConsultaCodigoTitulo, pParametro.ConsultaCliente);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraConsCesta(ConsultaMercado [{0}], ConsultaNegociadorCPF [{1}], ConsultaCodigoCesta [{2}], ConsultaDataCompra [{3}], ConsultaCodigoTitulo [{4}], ConsultaCliente [{5}]):\r\n{6}"
                                    , pParametro.ConsultaMercado
                                    , pParametro.ConsultaNegociadorCPF
                                    , pParametro.ConsultaCodigoCesta
                                    , pParametro.ConsultaDataCompra
                                    , pParametro.ConsultaCodigoTitulo
                                    , pParametro.ConsultaCliente
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("CESTAS") != null)
                {
                    foreach (XElement cesta in root.Element("CESTAS").Elements())
                    {
                        if (cesta.Element("TITULOS") != null)
                        {
                            foreach (XElement titulo in cesta.Element("TITULOS").Elements())
                            {
                                compraConsultaCestaInfo = new CompraConsultaCestaInfo();

                                compraConsultaCestaInfo.Cliente = titulo.Element("CLIENTE") != null ? titulo.Element("CLIENTE").Value : "";
                                compraConsultaCestaInfo.TituloNome = titulo.Element("TITULO_NOME") != null ? titulo.Element("TITULO_NOME").Value : "";
                                compraConsultaCestaInfo.CodigoCesta = titulo.Element("CODIGO_CESTA") != null ? titulo.Element("CODIGO_CESTA").Value : "";
                                compraConsultaCestaInfo.Mercado = titulo.Element("MERCADO") != null ? titulo.Element("MERCADO").Value : "";

                                if (titulo.Element("NEGOCIADOR") != null)
                                {
                                    compraConsultaCestaInfo.Negociador = new CodigoNomeInfo();
                                    compraConsultaCestaInfo.Negociador.CPF = titulo.Element("NEGOCIADOR").Element("CPF").Value;
                                    compraConsultaCestaInfo.Negociador.Codigo = titulo.Element("NEGOCIADOR").Element("CODIGO_AC").Value;
                                }

                                compraConsultaCestaInfo.DataCompra = titulo.Element("DATA_COMPRA") != null ? titulo.Element("DATA_COMPRA").Value.DBToDateTime() : DateTime.MinValue;
                                compraConsultaCestaInfo.Situacao = titulo.Element("SITUACAO") != null ? titulo.Element("SITUACAO").Value : "";
                                compraConsultaCestaInfo.TipoCesta = titulo.Element("TIPO_CESTA") != null ? titulo.Element("TIPO_CESTA").Value : "";
                                compraConsultaCestaInfo.IdNegociador = titulo.Element("ID_NEGOCIADOR") != null ? titulo.Element("ID_NEGOCIADOR").Value.DBToInt32() : 0;
                                compraConsultaCestaInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value : "";
                                compraConsultaCestaInfo.QuantidadeCompra = titulo.Element("QUANTIDADE_COMPRA") != null ? titulo.Element("QUANTIDADE_COMPRA").Value.DBToInt32() : 0;
                                compraConsultaCestaInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToInt32() : 0;
                                compraConsultaCestaInfo.ValorTaxaCBLC = titulo.Element("VALOR_TAXA_CBLC") != null ? titulo.Element("VALOR_TAXA_CBLC").Value.DBToDecimal() : 0;
                                compraConsultaCestaInfo.ValorTaxaAC = titulo.Element("VALOR_TAXA_AC") != null ? titulo.Element("VALOR_TAXA_AC").Value.DBToDecimal() : 0;

                                lRetorno.Objeto.Add(compraConsultaCestaInfo);
                            }
                        }
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.TituloNome + "; Valor: " + lTitulo.ValorTitulo + "\n"; });

                        gLogger.DebugFormat("COMPRA - CONSULTAR CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - Títulos: {3}", pParametro.ConsultaNegociadorCPF, pParametro.ConsultaCodigoCesta, pParametro.ConsultaMercado, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraConsultarCesta", ex);
            }

            return lRetorno;
        }

        public CompraInsereNovaCestaResponse CompraInserirNovaCesta(CompraInsereNovaCestaRequest pParametro)
        {
            var lRetorno = new CompraInsereNovaCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraInsCesta(pParametro.Mercado, pParametro.CPFNegociador);

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraInsCesta(Mercado [{0}], CPFNegociador [{1}]):\r\n{2}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("CESTA") != null)
                {
                    lRetorno.Objeto = new CompraInsereNovaCestaInfo();
                    lRetorno.Objeto.CodigoCesta = root.Element("CESTA").Value;
                }

                //--> Log
                gLogger.DebugFormat("VENDA - NOVA CESTA INSERIDA --> \n - CPF/CNPJ: {0} \n - Código Cesta: {1}", pParametro.CPFNegociador, lRetorno.Objeto.CodigoCesta);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraInserirNovaCesta", ex);
            }

            return lRetorno;
        }

        public CompraInsereItemNaCestaResponse CompraInserirItemNaCesta(CompraInsereItemNaCestaRequest pParametro)
        {
            var lRetorno = new CompraInsereItemNaCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraInsItemCesta(pParametro.Mercado, pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.TituloCodigoTitulo, pParametro.TituloQuantidadeCompra);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraInsItemCesta(Mercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], TituloCodigoTitulo [{3}], TituloQuantidadeCompra [{4}]):\r\n{5}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , pParametro.TituloCodigoTitulo
                                    , pParametro.TituloQuantidadeCompra
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                //--> Log
                gLogger.DebugFormat("COMPRA - INSERIR ITENS -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - TituloCodigoTitulo: {3}\n - TituloQuantidadeVenda: {4}"
                    , pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.Mercado, pParametro.TituloCodigoTitulo, pParametro.TituloQuantidadeCompra);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraInserirItemNaCesta", ex);
            }

            return lRetorno;
        }

        public CompraConsultaCestaItensResponse CompraConsultarCestaItens(CompraConsultaCestaItensRequest pParametro)
        {
            var lRetorno = new CompraConsultaCestaItensResponse();
            CompraConsultaCestaItemInfo compraConsultaCestaItemInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraConsCestaItens(pParametro.Mercado, pParametro.CodigoCesta, pParametro.CodigoTitulo);

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraConsCestaItens(Mercado [{0}], CPFNegociador [{1}], CodigoTitulo [{2}]):\r\n{3}"
                                    , pParametro.Mercado
                                    , pParametro.CodigoCesta
                                    , pParametro.CodigoTitulo
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("CESTAS") != null)
                {
                    foreach (XElement cesta in root.Element("CESTAS").Elements())
                    {
                        if (cesta.Element("TITULOS") != null)
                        {
                            foreach (XElement titulo in cesta.Element("TITULOS").Elements())
                            {
                                compraConsultaCestaItemInfo = new CompraConsultaCestaItemInfo();

                                compraConsultaCestaItemInfo.CodigoCesta = titulo.Element("CODIGO_CESTA") != null ? titulo.Element("CODIGO_CESTA").Value.DBToInt32() : 0;
                                compraConsultaCestaItemInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                                compraConsultaCestaItemInfo.QuantidadeCompra = titulo.Element("QUANTIDADE_COMPRA") != null ? titulo.Element("QUANTIDADE_COMPRA").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.ValorCBLC = titulo.Element("VALOR_CBLC") != null ? titulo.Element("VALOR_CBLC").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.ValorAC = titulo.Element("VALOR_AC") != null ? titulo.Element("VALOR_AC").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.ValorCompra = titulo.Element("VALOR_COMPRA") != null ? titulo.Element("VALOR_COMPRA").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.ValorVenda = titulo.Element("VALOR_VENDA") != null ? titulo.Element("VALOR_VENDA").Value.DBToDecimal() : 0;
                                compraConsultaCestaItemInfo.DataEmissao = titulo.Element("DATA_EMISSAO") != null ? titulo.Element("DATA_EMISSAO").Value.DBToDateTime() : DateTime.MinValue;
                                compraConsultaCestaItemInfo.DataVencimento = titulo.Element("DATA_VENCIMENTO") != null ? titulo.Element("DATA_VENCIMENTO").Value.DBToDateTime() : DateTime.MinValue;
                                compraConsultaCestaItemInfo.ISIN = titulo.Element("ISIN") != null ? titulo.Element("ISIN").Value : "";
                                compraConsultaCestaItemInfo.SELIC = titulo.Element("SELIC") != null ? titulo.Element("SELIC").Value.DBToInt32() : 0;
                                compraConsultaCestaItemInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";
                                compraConsultaCestaItemInfo.TipoTitulo = titulo.Element("TIPO_TITULO") != null ? titulo.Element("TIPO_TITULO").Value.DBToInt32() : 0;

                                lRetorno.Objeto.Add(compraConsultaCestaItemInfo);
                            }
                        }
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorVenda + "\n"; });

                        gLogger.DebugFormat("COMPRA - CONSULTAR ITENS CESTA -->\n - CodigoCesta: {0}\n - Títulos: {1}", pParametro.CodigoCesta, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraConsultarCestaItens", ex);
            }

            return lRetorno;
        }

        public CompraCalculaTaxaWSResponse CompraCalcularTaxaWs(CompraCalculaTaxaWSRequest pParametro)
        {
            var lRetorno = new CompraCalculaTaxaWSResponse();
            CompraCalculaTaxaWSInfo compraCalculaTaxaWSInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraCalcTaxaWS(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.XMLTitulo);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraConsCestaItens(CodigoMercado [{0}], CPFNegociador [{1}], XMLTitulo [{2}]):\r\n{3}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.XMLTitulo
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {
                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        compraCalculaTaxaWSInfo = new CompraCalculaTaxaWSInfo();

                        compraCalculaTaxaWSInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        compraCalculaTaxaWSInfo.ValorCBLC = titulo.Element("VALOR_CBLC") != null ? titulo.Element("VALOR_CBLC").Value.DBToDecimal() : 0;
                        compraCalculaTaxaWSInfo.ValorAC = titulo.Element("VALOR_AC") != null ? titulo.Element("VALOR_AC").Value.DBToDecimal() : 0;

                        lRetorno.Objeto.Add(compraCalculaTaxaWSInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraCalcularTaxaWs", ex);
            }

            return lRetorno;
        }

        public CompraVerificaTituloNoMercadoResponse CompraVerificarTituloNoMercado(CompraVerificaTituloNoMercadoRequest pParametro)
        {
            var lRetorno = new CompraVerificaTituloNoMercadoResponse();
            TituloInfo tituloInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraVerifTitMercado(pParametro.CodigoMercado, pParametro.CodigoCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraVerifTitMercado(CodigoMercado [{0}], CodigoCesta [{1}]):\r\n{2}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CodigoCesta
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {
                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        tituloInfo = new TituloInfo();
                        tituloInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        tituloInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";

                        lRetorno.Objeto.Add(tituloInfo);
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Quantidade: " + lTitulo.Quantidade + "\n"; });

                        gLogger.DebugFormat("COMPRA - VERIFICAR TÍTULO NO MERCADO -->\n - CodigoCesta: {0}\n - CodigoMercado: {1}\n - Títulos: {2}", pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraVerificarTituloNoMercado", ex);
            }

            return lRetorno;
        }

        public CompraFecharCestaResponse CompraFecharCesta(CompraFecharCestaRequest pParametro)
        {
            var lRetorno = new CompraFecharCestaResponse();
            TituloMercadoInfo tituloMercadoInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraFecharCesta(pParametro.Mercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontarXMLTitulo(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraFecharCesta(Mercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , this.MontarXMLTitulo(pParametro.Titulos)
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {
                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        tituloMercadoInfo = new TituloMercadoInfo();

                        tituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";
                        tituloMercadoInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorCompra = titulo.Element("VALOR_COMPRA") != null ? titulo.Element("VALOR_COMPRA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.ValorVenda = titulo.Element("VALOR_VENDA") != null ? titulo.Element("VALOR_VENDA").Value.DBToDecimal() : 0;

                        lRetorno.Objeto.Add(tituloMercadoInfo);
                    }
                }

                {   //--> Log
                    string lTitulos = string.Empty;

                    if (null != pParametro.Titulos && pParametro.Titulos.Count > 0)
                        lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorVenda + "\n"; });

                    gLogger.DebugFormat("COMPRA - FECHAR CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - Títulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.Mercado, lTitulos);
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraFecharCesta", ex);
            }

            return lRetorno;
        }

        public CompraExcluirCestaResponse CompraExcluirCesta(CompraExcluirCestaRequest pParametro)
        {
            var lRetorno = new CompraExcluirCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraExclCesta(pParametro.Mercado, pParametro.CPFNegociador, pParametro.CodigoCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraExclCesta(Mercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}]):\r\n{3}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                //--> Log
                gLogger.DebugFormat("COMPRA - EXCLUIR CESTA --\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.Mercado);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraExcluirCesta", ex);
            }

            return lRetorno;
        }

        public CompraExcluirItemDaCestaResponse CompraExcluirItemCesta(CompraExcluirItemDaCestaRequest pParametro)
        {
            var lRetorno = new CompraExcluirItemDaCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraExclItemCesta(pParametro.Mercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontaXMLTituloCesta(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraExclCesta(Mercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , this.MontaXMLTituloCesta(pParametro.Titulos)
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                {   //--> Log
                    string lTitulos = string.Empty;

                    if (null != pParametro.Titulos && pParametro.Titulos.Count > 0)
                        pParametro.Titulos.ForEach(lTitulo => { lTitulos += "CodigoTitulo: " + lTitulo.CodigoTitulo + "; QuantidadeVenda: " + lTitulo.Quantidade + "\n"; });

                    gLogger.DebugFormat("COMPRA - EXCLUIR ITEM DA CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n Titulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.Mercado, lTitulos);
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraExcluirItemCesta", ex);
            }

            return lRetorno;
        }

        public CompraAlteraItemDaCestaResponse CompraAlterarItemCesta(CompraAlteraItemDaCestaRequest pParametro)
        {
            var lRetorno = new CompraAlteraItemDaCestaResponse();
            CompraAlteraItemDaCestaInfo compraAlteraItemDaCestaInfo;

            try
            {
                string lXml = ConexaoWS.WsCompra.CompraAltItemCesta(pParametro.Mercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontarXMLTitulo(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsCompra.CompraAltItemCesta(Mercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.Mercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , this.MontarXMLTitulo(pParametro.Titulos)
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {
                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        compraAlteraItemDaCestaInfo = new CompraAlteraItemDaCestaInfo();

                        compraAlteraItemDaCestaInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        compraAlteraItemDaCestaInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";
                        compraAlteraItemDaCestaInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
                        compraAlteraItemDaCestaInfo.ValorCompra = titulo.Element("VALOR_COMPRA") != null ? titulo.Element("VALOR_COMPRA").Value.DBToDecimal() : 0;
                        compraAlteraItemDaCestaInfo.ValorVenda = titulo.Element("VALOR_VENDA") != null ? titulo.Element("VALOR_VENDA").Value.DBToDecimal() : 0;

                        lRetorno.Objeto.Add(compraAlteraItemDaCestaInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("CompraAlterarItemCesta", ex);
            }

            return lRetorno;
        }

        private string MontarXMLTitulo(List<TituloInfo> tituloInfo)
        {
            var lRetorno = new StringBuilder("<TITULOS>");

            if (null != tituloInfo && tituloInfo.Count > 0) tituloInfo.ForEach(item =>
                {
                    lRetorno.AppendFormat("<TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO><QUANTIDADE>{1}</QUANTIDADE></TITULO>", item.CodigoTitulo, item.Quantidade.DBToString().Replace(",", "."));
                });

            lRetorno.Append("</TITULOS>");

            return lRetorno.ToString();
        }

        private string MontaXMLTituloCesta(List<TituloInfo> tituloInfo)
        {
            var lRetorno = new StringBuilder("<TITULOS>");

            if (null != tituloInfo && tituloInfo.Count > 0) tituloInfo.ForEach(item =>
            {
                lRetorno.AppendFormat("<TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO></TITULO>", item.CodigoTitulo);
            });

            lRetorno.Append("</TITULOS>");

            return lRetorno.ToString();
        }
    }
}
