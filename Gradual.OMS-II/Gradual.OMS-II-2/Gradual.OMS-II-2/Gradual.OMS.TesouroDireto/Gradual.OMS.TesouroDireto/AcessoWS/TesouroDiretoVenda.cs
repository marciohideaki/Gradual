using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Gradual.OMS.TesouroDireto.App_Codigo;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Venda;
using log4net;

namespace Gradual.OMS.TesouroDireto.AcessoWS
{
    public class TesouroDiretoVenda : ObjetosBase
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VendaVerificaCondicaoDeVendaResponse VendaVerificarCondicao(VendaVerificaCondicaoDeVendaRequest pParametro)
        {
            var lRetorno = new VendaVerificaCondicaoDeVendaResponse();

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaVerifCondVenda(pParametro.CPFNegociador, pParametro.CodigoMercado);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaVerifCondVenda(CPFNegociador [{0}], CodigoMercado [{1}]):\r\n{2}"
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoMercado
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaVerificarCondicao", ex);
            }

            return lRetorno;
        }

        public VendaVerificaTituloMercadoResponse VendaVerificarTituloMercado(VendaVerificaTituloMercadoRequest pParametro)
        {
            var lRetorno = new VendaVerificaTituloMercadoResponse();
            TituloMercadoInfo tituloMercadoInfo;

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaVerifTitMercado(pParametro.CodigoMercado, pParametro.CodigoCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaVerifTitMercado(CodigoMercado [{0}], CodigoCesta [{1}]):\r\n{2}"
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
                        tituloMercadoInfo = new TituloMercadoInfo();

                        tituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";

                        lRetorno.Objeto.Add(tituloMercadoInfo);
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorTitulo + "\n"; });

                        gLogger.DebugFormat("VENDA - VERIFICAR TÍTULO NO MERCADO -->\n - CodigoCesta: {0}\n - CodigoMercado: {1}\n - Títulos: {2}", pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaVerificarTituloMercado", ex);
            }

            return lRetorno;
        }

        public VendaConsultaTituloDeVendaResponse VendaConsultarTitulo(VendaConsultaTituloDeVendaRequest pParametro)
        {
            var lRetorno = new VendaConsultaTituloDeVendaResponse();
            TituloMercadoInfo tituloMercadoInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaConsTitVenda(pParametro.CPFNegociador, pParametro.DataEmissao.DBToDateTimeString(), pParametro.DataVencimento.DBToDateTimeString(), pParametro.SELIC.ToString(), pParametro.ISIN, pParametro.CodigoTitulo, pParametro.CodigoMercado, pParametro.NotCesta, pParametro.TipoIndexador);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaConsTitVenda(CPFNegociador [{0}], DataEmissao [{1}], DataVencimento [{2}], SELIC [{3}], ISIN [{4}], CodigoTitulo [{5}], CodigoMercado [{6}], NotCesta [{7}], TipoIndexador [{8}]):\r\n{9}"
                                    , pParametro.CPFNegociador
                                    , pParametro.DataEmissao
                                    , pParametro.DataVencimento
                                    , pParametro.SELIC
                                    , pParametro.ISIN
                                    , pParametro.CodigoTitulo
                                    , pParametro.CodigoMercado
                                    , pParametro.NotCesta
                                    , pParametro.TipoIndexador
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {
                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        tituloMercadoInfo = new TituloMercadoInfo();

                        tituloMercadoInfo.TipoTitulo = titulo.Element("TIPO_TITULO") != null ? titulo.Element("TIPO_TITULO").Value : "";
                        tituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.DataEmissao = titulo.Element("DATA_EMISSAO") != null ? titulo.Element("DATA_EMISSAO").Value.DBToDateTime() : DateTime.MinValue;
                        tituloMercadoInfo.DataVencimento = titulo.Element("DATA_VENCIMENTO") != null ? titulo.Element("DATA_VENCIMENTO").Value.DBToDateTime() : DateTime.MinValue;
                        tituloMercadoInfo.ISIN = titulo.Element("ISIN") != null ? titulo.Element("ISIN").Value : "";
                        tituloMercadoInfo.ValorVenda = titulo.Element("VALOR_VENDA") != null ? titulo.Element("VALOR_VENDA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.SELIC = titulo.Element("SELIC") != null ? titulo.Element("SELIC").Value : "";
                        tituloMercadoInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";
                        tituloMercadoInfo.QuantidadeDisponivelVenda = titulo.Element("QUANTIDADE_DISPONIVEL_VENDA") != null ? titulo.Element("QUANTIDADE_DISPONIVEL_VENDA").Value.DBToDouble() : 0;
                        tituloMercadoInfo.ValorTaxaVenda = titulo.Element("VALOR_TAXA_VENDA") != null ? titulo.Element("VALOR_TAXA_VENDA").Value.DBToDecimal() : 0;
                        tituloMercadoInfo.Mercado = titulo.Element("MERCADO") != null ? titulo.Element("MERCADO").Value.DBToInt32() : 0;
                        tituloMercadoInfo.TipoIndexadorNome = titulo.Element("INDEXADOR") != null && titulo.Element("INDEXADOR").Element("NOME") != null ? titulo.Element("INDEXADOR").Element("NOME").Value : "";
                        tituloMercadoInfo.QuantidadeSaldo = titulo.Element("QUANTIDADE_SALDO") != null ? titulo.Element("QUANTIDADE_SALDO").Value.DBToDouble() : 0;

                        lRetorno.Objeto.Add(tituloMercadoInfo);
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorTitulo + "\n"; });

                        gLogger.DebugFormat("VENDA - CONSULTAR TITULO -->\n - CPF/CNPJ: {0}\n - CodigoMercado: {1}\n - Títulos: {2}", pParametro.CPFNegociador, pParametro.CodigoMercado, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaConsultarTitulo", ex);
            }

            return lRetorno;
        }

        public VendaConsultaValidadeDeTaxaProvisoriaResponse VendaConsultarValidadeTaxaProvisoria(VendaConsultaValidadeDeTaxaProvisoriaRequest pParametro)
        {
            var lRetorno = new VendaConsultaValidadeDeTaxaProvisoriaResponse();
            TaxaInfo taxaInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaConsValTxProvisoria(pParametro.CPFNegociador, pParametro.CodigoTitulo, pParametro.Quantidade);

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaConsValTxProvisoria(CPFNegociador [{0}], CodigoTitulo [{1}], Quantidade [{2}]):\r\n{3}"
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoTitulo
                                    , pParametro.Quantidade
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TAXAS") != null)
                {
                    foreach (XElement taxa in root.Element("TAXAS").Elements())
                    {
                        taxaInfo = new TaxaInfo();
                        taxaInfo.TaxaCBLC = taxa.Element("TAXA_CBLC") != null ? taxa.Element("TAXA_CBLC").Value.DBToDecimal() : 0;
                        taxaInfo.TaxaCorretor = taxa.Element("TAXA_AGENTE") != null ? taxa.Element("TAXA_AGENTE").DBToDecimal() : 0;

                        lRetorno.Taxas.Add(taxaInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaConsultarValidadeTaxaProvisoria", ex);
            }

            return lRetorno;
        }

        public VendaConsultaCestaResponse VendaConsultarCesta(VendaConsultaCestaRequest pParametro)
        {
            var lRetorno = new VendaConsultaCestaResponse();
            CompraConsultaCestaInfo compraConsultaCestaInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaConsCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoTitulo, pParametro.DataRecompra.DBToDateTimeString());

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaConsCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], CodigoTitulo [{3}], DataRecompra [{4}]):\r\n{5}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , pParametro.CodigoTitulo
                                    , pParametro.DataRecompra
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
                                compraConsultaCestaInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
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

                        gLogger.DebugFormat("VENDA - CONSULTAR CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - Títulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaConsultarCesta", ex);
            }

            return lRetorno;
        }

        public VendaConsultaCestaItensResponse VendaConsultarItensCesta(VendaConsultaCestaItensRequest pParametro)
        {
            var lRetorno = new VendaConsultaCestaItensResponse();
            TituloMercadoInfo tituloMercadoInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaConsCestaItens(pParametro.CodigoCesta, pParametro.CodigoTitulo);

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaConsCestaItens(CodigoCesta [{0}], CodigoTitulo [{1}]):\r\n{2}"
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
                                tituloMercadoInfo = new TituloMercadoInfo();
                                tituloMercadoInfo.CodigoCesta = titulo.Element("CODIGO_CESTA") != null ? titulo.Element("CODIGO_CESTA").Value : "";
                                tituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                                tituloMercadoInfo.QuantidadeVenda = titulo.Element("QUANTIDADE_VENDA") != null ? titulo.Element("QUANTIDADE_VENDA").Value.DBToDouble() : 0;
                                tituloMercadoInfo.QuantidadeDisponivelVenda = titulo.Element("QUANTIDADE_VENDA") != null ? titulo.Element("QUANTIDADE_VENDA").Value.DBToInt32() : 0;
                                tituloMercadoInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
                                tituloMercadoInfo.DataEmissao = titulo.Element("DATA_EMISSAO") != null ? titulo.Element("DATA_EMISSAO").Value.DBToDateTime() : DateTime.MinValue;
                                tituloMercadoInfo.DataVencimento = titulo.Element("DATA_VENCIMENTO") != null ? titulo.Element("DATA_VENCIMENTO").Value.DBToDateTime() : DateTime.MinValue;
                                tituloMercadoInfo.ISIN = titulo.Element("ISIN") != null ? titulo.Element("ISIN").Value : "";
                                tituloMercadoInfo.SELIC = titulo.Element("SELIC") != null ? titulo.Element("SELIC").Value : "";
                                tituloMercadoInfo.NomeTitulo = titulo.Element("NOME_TITULO") != null ? titulo.Element("NOME_TITULO").Value : "";
                                tituloMercadoInfo.DescricaoTitulo = titulo.Element("DESCRICAO_TITULO") != null ? titulo.Element("DESCRICAO_TITULO").Value : "";
                                tituloMercadoInfo.ValorTaxaCBLC = titulo.Element("VALOR_TAXA_CBLC") != null ? titulo.Element("VALOR_TAXA_CBLC").Value.DBToDecimal() : 0;
                                tituloMercadoInfo.ValorTaxaAC = titulo.Element("VALOR_TAXA_AC") != null ? titulo.Element("VALOR_TAXA_AC").Value.DBToDecimal() : 0;
                                tituloMercadoInfo.TipoTitulo = titulo.Element("TIPO_TITULO") != null ? titulo.Element("TIPO_TITULO").Value : "";

                                lRetorno.Objeto.Add(tituloMercadoInfo);
                            }
                        }
                    }

                    {   //--> Log
                        string lTitulos = string.Empty;

                        if (null != lRetorno.Objeto && lRetorno.Objeto.Count > 0)
                            lRetorno.Objeto.ForEach(lTitulo => { lTitulos += "Título: " + lTitulo.NomeTitulo + "; Valor: " + lTitulo.ValorVenda + "\n"; });

                        gLogger.DebugFormat("VENDA - CONSULTAR ITENS CESTA -->\n - CodigoCesta: {0}\n - Títulos: {1}", pParametro.CodigoCesta, lTitulos);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaConsultarItensCesta", ex);
            }

            return lRetorno;
        }

        public VendaInsereNovaCestaResponse VendaInserirCesta(VendaInsereNovaCestaRequest pParametro)
        {
            var lRetorno = new VendaInsereNovaCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaInsCesta(pParametro.CodigoMercado, pParametro.CPFNegociador);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaInsCesta(CodigoMercado [{0}], CPFNegociador [{1}]):\r\n{2}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("CESTA") != null)
                    lRetorno.Cesta = root.Element("CESTA").Value;

                //--> Log
                gLogger.DebugFormat("VENDA - NOVA CESTA INSERIDA --> \n - CPF/CNPJ: {0} \n - Código Cesta: {1}", pParametro.CPFNegociador, lRetorno.Cesta);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaInserirCesta", ex);
            }

            return lRetorno;
        }

        public VendaInsereItemNaCestaResponse VendaInserirItensCesta(VendaInscereItemNaCestaRequest pParametro)
        {
            var lRetorno = new VendaInsereItemNaCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaInsItemCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.TituloCodigoTitulo, pParametro.TituloQuantidadeVenda);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaInsCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], TituloCodigoTitulo [{3}], TituloQuantidadeVenda [{4}]):\r\n{5}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , pParametro.TituloCodigoTitulo
                                    , pParametro.TituloQuantidadeVenda
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                //--> Log
                gLogger.DebugFormat("VENDA - INSERIR ITENS -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - TituloCodigoTitulo: {3}\n - TituloQuantidadeVenda: {4}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado, pParametro.TituloCodigoTitulo, pParametro.TituloQuantidadeVenda);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaInserirItensCesta", ex);
            }

            return lRetorno;
        }

        public VendaAlteraItemDaCestaResponse VendaAlterarItensCesta(VendaAlteraItemDaCestaRequest pParametro)
        {
            var lRetorno = new VendaAlteraItemDaCestaResponse();
            TituloMercadoInfo tituloMercadoInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaAltItemCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontarXMLTituloMercado(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaAltItemCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , this.MontarXMLTituloMercado(pParametro.Titulos)
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

                    gLogger.DebugFormat("VENDA - ALTER ITENS NA CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n Títulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaAlterarItensCesta", ex);
            }

            return lRetorno;
        }

        public VendaFechaCestaResponse VendaFecharCesta(VendaFechaCestaRequest pParametro)
        {
            var lRetorno = new VendaFechaCestaResponse();
            TituloMercadoInfo tituloMercadoInfo;
            try
            {
                string lXml = ConexaoWS.WsVenda.VendaFecharCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontarXMLTituloMercado(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaFecharCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , this.MontarXMLTituloMercado(pParametro.Titulos)
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

                    gLogger.DebugFormat("VENDA - FECHAR CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n - Títulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaFecharCesta", ex);
            }

            return lRetorno;
        }

        public VendaExcluiCestaResponse VendaExcluirCesta(VendaExcluiItemCestaRequest pParametro)
        {
            var lRetorno = new VendaExcluiCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaExclCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaExclCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}]):\r\n{3}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.CodigoCesta
                                    , lXml);


                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                //--> Log
                gLogger.DebugFormat("VENDA - EXCLUIR CESTA --\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado);
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaExcluirCesta", ex);
            }

            return lRetorno;
        }

        public VendaExcluiItemCestaResponse VendaExcluirItemCesta(VendaExcluiItemCestaRequest pParametro)
        {
            var lRetorno = new VendaExcluiItemCestaResponse();

            try
            {
                string lXml = ConexaoWS.WsVenda.VendaExclItemCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.CodigoCesta, this.MontaXMLTituloCesta(pParametro.Titulos));
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsVenda.VendaExclItemCesta(CodigoMercado [{0}], CPFNegociador [{1}], CodigoCesta [{2}], Titulos [{3}]):\r\n{4}"
                                    , pParametro.CodigoMercado
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
                        pParametro.Titulos.ForEach(lTitulo => { lTitulos += "CodigoTitulo: " + lTitulo.CodigoTitulo + "; QuantidadeVenda: " + lTitulo.QuantidadeVenda + "\n"; });

                    gLogger.DebugFormat("VENDA - EXCLUIR ITEM DA CESTA -->\n - CPF/CNPJ: {0}\n - CodigoCesta: {1}\n - CodigoMercado: {2}\n Titulos: {3}", pParametro.CPFNegociador, pParametro.CodigoCesta, pParametro.CodigoMercado, lTitulos);
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("VendaExcluirItemCesta", ex);
            }

            return lRetorno;
        }

        private string MontarXMLTituloMercado(List<TituloMercadoInfo> pParametro)
        {
            var lRetorno = new StringBuilder("<TITULOS>");

            if (null != pParametro && pParametro.Count > 0) pParametro.ForEach(item =>
            {
                lRetorno.AppendFormat("<TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO><QUANTIDADE>{1}</QUANTIDADE></TITULO>", item.CodigoTitulo, item.QuantidadeVenda.DBToDecimal().DBToString().Replace(",", "."));
            });

            lRetorno.Append("</TITULOS>");

            return lRetorno.ToString();
        }

        private string MontaXMLTituloCesta(List<TituloMercadoInfo> pParametro)
        {
            var lRetorno = new StringBuilder("<TITULOS>");

            if (null != pParametro && pParametro.Count > 0) pParametro.ForEach(item =>
            {
                lRetorno.AppendFormat("<TITULO><CODIGO_TITULO>{0}</CODIGO_TITULO></TITULO>", item.CodigoTitulo);
            });

            lRetorno.Append("</TITULOS>");

            return lRetorno.ToString();
        }
    }
}
