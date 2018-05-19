using System;
using System.Xml.Linq;
using Gradual.OMS.TesouroDireto.App_Codigo;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.Lib.Mensagens.Consultas;
using log4net;

namespace Gradual.OMS.TesouroDireto.AcessoWS
{
    public class TesouroDiretoConsulta : ObjetosBase
    {
        #region | Atributos

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Métodos

        public ConsultasConsultaMercadoResponse ConsultarMercado(ConsultasConsultaMercadoRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaMercadoResponse();

            try
            {
                gLogger.Debug("Iniciou a consulta");

                string lXml = ConexaoWS.WsConsulta.ConsultasConsMercado();

                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsMercado():\r\n{0}", lXml);

                gLogger.Debug("consultou");

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                gLogger.Debug(root.FirstAttribute.Name.LocalName + " " + root.FirstAttribute.Value + " " + root.LastAttribute.Value);

                if (root.Element("MERCADOS") != null)
                {
                    XElement elemMercado = root.Element("MERCADOS").Element("MERCADO");

                    if (elemMercado == null)
                        return new ConsultasConsultaMercadoResponse();

                    lRetorno.DataInicial = elemMercado.Element("DATA_INICIAL") != null ? elemMercado.Element("DATA_INICIAL").Value.DBToDateTime() : DateTime.MinValue;
                    lRetorno.DataFinal = elemMercado.Element("DATA_FINAL") != null ? elemMercado.Element("DATA_FINAL").Value.DBToDateTime() : DateTime.MinValue;
                    lRetorno.DataProrrogacao = elemMercado.Element("DATA_PRORROGACAO") != null ? elemMercado.Element("DATA_PRORROGACAO").Value.DBToDateTime() : DateTime.MinValue;
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
                gLogger.Error("ConsultarMercado", ex);
            }

            return lRetorno;
        }

        public ConsultasConsultaTipoTituloResponse ConsultarTipoTitulo(ConsultasConsultaTipoTituloRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaTipoTituloResponse();
            CodigoNomeInfo codigoNomeInfo;

            try
            {
                string lXml = ConexaoWS.WsConsulta.ConsultasConsTipoTitulo();
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsTipoTitulo():\r\n{0}", lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TIPOS") != null)
                {
                    foreach (XElement tipo in root.Element("TIPOS").Elements())
                    {
                        codigoNomeInfo = new CodigoNomeInfo();

                        codigoNomeInfo.Codigo = tipo.Element("CODIGO") != null ? tipo.Element("CODIGO").Value : "";
                        codigoNomeInfo.Nome = tipo.Element("DESCRICAO") != null ? tipo.Element("DESCRICAO").Value : "";
                        //_Tipos.Add(new KeyValuePair<String, String>(cod, descr));

                        lRetorno.Tipos.Add(codigoNomeInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("ConsultarTipoTitulo", ex);
            }

            return lRetorno;
        }

        public ConsultasConsultaTipoIndexadorResponse ConsultarTipoIndexador(ConsultasConsultaTipoIndexadorRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaTipoIndexadorResponse();
            CodigoNomeInfo codigoNomeInfo;

            try
            {
                string lXml = ConexaoWS.WsConsulta.ConsultasConsTipoIndexador();
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsTipoIndexador():\r\n{0}", lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("INDEXADORES") != null)
                {
                    foreach (XElement indexador in root.Element("INDEXADORES").Elements())
                    {
                        codigoNomeInfo = new CodigoNomeInfo();

                        codigoNomeInfo.Codigo = indexador.Element("CODIGO") != null ? indexador.Element("CODIGO").Value : "";
                        codigoNomeInfo.Nome = indexador.Element("DESCRICAO") != null ? indexador.Element("DESCRICAO").Value : "";

                        lRetorno.Indexadores.Add(codigoNomeInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("ConsultarTipoIndexador", ex);
            }

            return lRetorno;
        }

        public ConsultasConsultaExtratoMensalResponse ConsultarExtratoMensal(ConsultasConsultaExtratoMensalRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaExtratoMensalResponse();

            try
            {
                string lXml = ConexaoWS.WsConsulta.ConsultasConsExtratMensal(pParametro.CPFNegociador);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsExtratMensal({1}):\r\n{0}", lXml, pParametro.CPFNegociador);

                TituloMercadoInfo lTituloMercadoInfo;
                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("TITULOS") != null)
                {

                    foreach (XElement titulo in root.Element("TITULOS").Elements())
                    {
                        lTituloMercadoInfo = new TituloMercadoInfo();

                        lTituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                        lTituloMercadoInfo.NomeTitulo = titulo.Element("TITULO_NOME") != null ? titulo.Element("TITULO_NOME").Value : "";
                        lTituloMercadoInfo.SELIC = titulo.Element("SELIC") != null ? titulo.Element("SELIC").Value : "";
                        lTituloMercadoInfo.CodigoAC = titulo.Element("CODIGO_AC") != null ? titulo.Element("CODIGO_AC").Value : "";
                        lTituloMercadoInfo.NomeCorretor = titulo.Element("NM_CORRETOR") != null ? titulo.Element("NM_CORRETOR").Value : "";
                        lTituloMercadoInfo.DataEmissao = titulo.Element("DATA_EMISSAO") != null ? titulo.Element("DATA_EMISSAO").Value.DBToDateTime() : DateTime.MinValue;
                        lTituloMercadoInfo.DataVencimento = titulo.Element("DATA_VENCIMENTO") != null ? titulo.Element("DATA_VENCIMENTO").Value.DBToDateTime() : DateTime.MinValue;
                        lTituloMercadoInfo.ValorTaxaDevida = titulo.Element("TAXA_DEVIDA") != null ? titulo.Element("TAXA_DEVIDA").Value.DBToDecimal() : 0;
                        lTituloMercadoInfo.QuantidadeCredito = titulo.Element("QUANTIDADE_CREDITO") != null ? titulo.Element("QUANTIDADE_CREDITO").Value.DBToDouble() : 0;
                        lTituloMercadoInfo.QuantidadeDebito = titulo.Element("QUANTIDADE_DEBITO") != null ? titulo.Element("QUANTIDADE_DEBITO").Value.DBToDouble() : 0;
                        lTituloMercadoInfo.QuantidadeBloqueada = titulo.Element("QUANTIDADE_BLOQUEADA") != null ? titulo.Element("QUANTIDADE_BLOQUEADA").Value.DBToDouble() : 0;
                        lTituloMercadoInfo.Cliente = titulo.Element("CLIENTE") != null ? titulo.Element("CLIENTE").Value : "";
                        lTituloMercadoInfo.ValorBase = titulo.Element("VALOR_BASE") != null ? titulo.Element("VALOR_BASE").Value.DBToDecimal() : 0;
                        lTituloMercadoInfo.SaldoAnterior = titulo.Element("SALDO_ANTERIOR") != null ? titulo.Element("SALDO_ANTERIOR").Value.DBToDecimal() : 0;

                        lRetorno.Titulos.Add(lTituloMercadoInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("ConsultarExtratoMensal", ex);
            }

            return lRetorno;
        }

        public ConsultasConsultaCestaResponse ConsultarCesta(ConsultasConsultaCestaRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaCestaResponse();
            TituloMercadoInfo tituloMercadoInfo;

            try
            {
                string lXml = ConexaoWS.WsConsulta.ConsultasConsCesta(pParametro.CodigoMercado, pParametro.CPFNegociador, pParametro.Situacao, pParametro.Tipo, pParametro.CodigoCesta, pParametro.DataCompra.DBToDateTimeString(), pParametro.CodigoTitulo, pParametro.Cliente);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsCesta(CodigoMercado [{0}], CPFNegociador [{1}], Situacao [{2}], Tipo [{3}], CodigoCesta [{4}], DataCompra [{5}], CodigoTitulo [{6}], Cliente [{7}]):\r\n{8}"
                                    , pParametro.CodigoMercado
                                    , pParametro.CPFNegociador
                                    , pParametro.Situacao
                                    , pParametro.Tipo
                                    , pParametro.CodigoCesta
                                    , pParametro.DataCompra
                                    , pParametro.CodigoTitulo
                                    , pParametro.Cliente
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

                                tituloMercadoInfo.Cliente = titulo.Element("CLIENTE") != null ? titulo.Element("CLIENTE").Value : "";
                                tituloMercadoInfo.NomeTitulo = titulo.Element("TITULO_NOME") != null ? titulo.Element("TITULO_NOME").Value : "";
                                tituloMercadoInfo.CodigoCesta = titulo.Element("CODIGO_CESTA") != null ? titulo.Element("CODIGO_CESTA").Value : "";
                                tituloMercadoInfo.Mercado = titulo.Element("MERCADO") != null ? titulo.Element("MERCADO").Value.DBToInt32() : 0;

                                if (titulo.Element("NEGOCIADOR") != null)
                                {
                                    tituloMercadoInfo.CPFNegociador = titulo.Element("NEGOCIADOR").Element("CPF").Value;
                                    tituloMercadoInfo.CodigoACNegociador = titulo.Element("NEGOCIADOR").Element("CODIGO_AC").Value;
                                }

                                tituloMercadoInfo.DataCompra = titulo.Element("DATA_COMPRA") != null ? titulo.Element("DATA_COMPRA").Value.DBToDateTime() : DateTime.MinValue;
                                tituloMercadoInfo.Situacao = titulo.Element("SITUACAO") != null ? titulo.Element("SITUACAO").Value : "";
                                tituloMercadoInfo.TipoCesta = titulo.Element("TIPO_CESTA") != null ? titulo.Element("TIPO_CESTA").Value : "";
                                tituloMercadoInfo.CodigoNegociador = titulo.Element("ID_NEGOCIADOR") != null ? titulo.Element("ID_NEGOCIADOR").Value : "";
                                tituloMercadoInfo.CodigoTitulo = titulo.Element("CODIGO_TITULO") != null ? titulo.Element("CODIGO_TITULO").Value.DBToInt32() : 0;
                                tituloMercadoInfo.QuantidadeCompra = titulo.Element("QUANTIDADE_COMPRA") != null ? titulo.Element("QUANTIDADE_COMPRA").Value.DBToDouble() : 0;
                                tituloMercadoInfo.ValorTitulo = titulo.Element("VALOR_TITULO") != null ? titulo.Element("VALOR_TITULO").Value.DBToDecimal() : 0;
                                tituloMercadoInfo.ValorTaxaCBLC = titulo.Element("VALOR_TAXA_CBLC") != null ? titulo.Element("VALOR_TAXA_CBLC").Value.DBToDecimal() : 0;
                                tituloMercadoInfo.ValorTaxaAC = titulo.Element("VALOR_TAXA_AC") != null ? titulo.Element("VALOR_TAXA_AC").Value.DBToDecimal() : 0;

                                lRetorno.Titulos.Add(tituloMercadoInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("ConsultarCesta", ex);
            }

            return lRetorno;
        }

        public ConsultasConsultaTaxaProtocoloResponse ConsultarProtocolo(ConsultasConsultaTaxaProtocoloRequest pParametro)
        {
            var lRetorno = new ConsultasConsultaTaxaProtocoloResponse();
            TaxaInfo taxaInfo;
            try
            {
                string lXml = ConexaoWS.WsConsulta.ConsultasConsTaxaProtocolo(pParametro.CodigoTitulo, pParametro.CodigoCesta);
                
                gLogger.InfoFormat("Resposta recebida de ConexaoWS.WsConsulta.ConsultasConsTaxaProtocolo(CodigoMercado [{0}], CPFNegociador [{1}]):\r\n{2}"
                                    , pParametro.CodigoTitulo
                                    , pParametro.CodigoCesta
                                    , lXml);

                base.AtribDefaultValues();
                XElement root = null;
                base.GetStatus(lXml, out root);

                if (root.Element("VALORES") != null)
                {
                    foreach (XElement valor in root.Element("VALORES").Elements())
                    {
                        taxaInfo = new TaxaInfo();

                        taxaInfo.TaxaCorretor = valor.Element("TAXA_CORRETOR") != null ? valor.Element("TAXA_AGENTE").Value.DBToDecimal() : 0;
                        taxaInfo.TaxaCBLC = valor.Element("TAXA_CBLC") != null ? valor.Element("TAXA_CBLC").Value.DBToDecimal() : 0;

                        lRetorno.Taxas.Add(taxaInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroNegocio;
                lRetorno.DescricaoResposta = ex.ToString();
                gLogger.Error("ConsultarProtocolo", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}
