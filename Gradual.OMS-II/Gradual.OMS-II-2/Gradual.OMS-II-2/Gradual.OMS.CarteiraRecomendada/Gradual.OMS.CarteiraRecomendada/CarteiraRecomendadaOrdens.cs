using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens.Persistencia.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.Risco.Lib.Enum;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.OMS.CarteiraRecomendada
{
    public class CarteiraRecomendadaOrdens
    {
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CarteiraRecomendadaOrdens(){
            log4net.Config.XmlConfigurator.Configure();
        }

        #region ENVIO DE ORDENS

        public EnviarOrdemResponse EnviarOrdem(EnviarOrdemRequest pParametros , string CodigoCarteiraRecomendada)
        {
            EnviarOrdemResponse OrdemResposta = new EnviarOrdemResponse();

            OrdemFIXResponse<OrdemInfo> OrdemFixResponse = this.ParsearOrdemCliente(pParametros.ClienteOrdemInfo, CodigoCarteiraRecomendada);

            if (OrdemFixResponse.StatusResposta == OMS.Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso)
            {
                EnviarOrdemRoteadorRequest OrdemRoteadorRequest = new EnviarOrdemRoteadorRequest();
                OrdemRoteadorRequest.OrdemInfo = OrdemFixResponse.Objeto;

                ValidacaoRiscoRequest RiscoRequisicao = new ValidacaoRiscoRequest();
                RiscoRequisicao.EnviarOrdemRequest = OrdemRoteadorRequest;

                ValidacaoRiscoResponse RiscoResposta = this.ValidarPipeLineOrdem(RiscoRequisicao, CodigoCarteiraRecomendada);

                if (RiscoResposta.CriticaInfo.Count >= 0)
                {

                    OrdemResposta.CriticaInfo = new List<PipeLineCriticaInfo>();

                    foreach (var RiscoItem in RiscoResposta.CriticaInfo)
                    {

                        PipeLineCriticaInfo _PipeLineCriticaInfo = new PipeLineCriticaInfo();

                        OrdemResposta.CriticaInfo.Add(
                            new PipeLineCriticaInfo()
                            {
                                Critica = RiscoItem.Critica,
                                CriticaTipo = (OMS.Ordens.Lib.Enum.CriticaRiscoEnum)RiscoItem.CriticaTipo,
                                DataHoraCritica = RiscoItem.DataHoraCritica
                            });
                    }

                    OrdemResposta.DataResposta = RiscoResposta.DataResposta;
                    OrdemResposta.DescricaoResposta = RiscoResposta.DescricaoResposta;
                    OrdemResposta.StackTrace = RiscoResposta.StackTrace;
                    OrdemResposta.StatusResposta = (OMS.Ordens.Lib.Enum.CriticaRiscoEnum)RiscoResposta.StatusResposta;
                }
                else
                {
                    OrdemResposta.CriticaInfo = new List<PipeLineCriticaInfo>();

                    foreach (var RiscoItem in OrdemFixResponse.CriticaInfo)
                    {

                        PipeLineCriticaInfo _PipeLineCriticaInfo = new PipeLineCriticaInfo();

                        OrdemResposta.CriticaInfo.Add(
                            new PipeLineCriticaInfo()
                            {
                                Critica = RiscoItem.Critica,
                                CriticaTipo = (OMS.Ordens.Lib.Enum.CriticaRiscoEnum)RiscoItem.CriticaTipo,
                                DataHoraCritica = RiscoItem.DataHoraCritica
                            });
                    }

                    OrdemResposta.DataResposta = OrdemResposta.DataResposta;
                    OrdemResposta.DescricaoResposta = OrdemResposta.DescricaoResposta;
                    OrdemResposta.StackTrace = OrdemResposta.StackTrace;
                    OrdemResposta.StatusResposta = (OMS.Ordens.Lib.Enum.CriticaRiscoEnum)OrdemResposta.StatusResposta;
                }
            }

            return OrdemResposta;
        }

        #endregion

        #region VALIDACAO COMPRA E VENDA

        private ValidacaoRiscoResponse ComprarCarteiraRecomendada(ValidacaoRiscoRequest pParametros, string CodigoCarteiraRecomendada)
        {
            ValidacaoRiscoResponse    PipeLineResponse       = new ValidacaoRiscoResponse();
            List<PipeLineCriticaInfo> CriticaInfoCollection  = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo       CriticaInfo = null;

            logger.Info("Inicia Rotina de validação de compra de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            : Compra");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               : ORDEM A MERCADO ");
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());
            logger.Info("ClasseCliente       : Institucional");

            CadastroPapeisRequest _CadastroPapeisRequest = new CadastroPapeisRequest();
            _CadastroPapeisRequest.Instrumento = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;

            CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis = new PersistenciaCarteiraRecomendada().ObterInformacoesPapeis(_CadastroPapeisRequest);
                                                                                   
        
            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao       = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
            {

                // ENVIAR INTEGRAL
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }
                }

                if (ModuloLoteNegociacao > 0)
                {
                    // ENVIAR FRACIONARIO
                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                    // ENVIAR FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber(CodigoCarteiraRecomendada);

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }
                }
            }
            else
            {
                //FRACIONARIO
                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR                                    
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    logger.Info("Envia a ordem para o roteador de ordens");


                    var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }
            }

            return PipeLineResponse;

        }

        private ValidacaoRiscoResponse VenderCarteiraRecomendada(ValidacaoRiscoRequest pParametros, string CodigoCarteiraRecomendada)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();
            List<PipeLineCriticaInfo> CriticaInfoCollection = new List<PipeLineCriticaInfo>();
            PipeLineCriticaInfo CriticaInfo = null;


            logger.Info("Inicia Rotina de validação de compra de ações, cliente: " + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());

            logger.Info("Caracteristicas da ordem");
            logger.Info("Cliente             :" + pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString());
            logger.Info("Operação            : Compra");
            logger.Info("Instrumento         :" + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol);
            logger.Info("Quantidade          :" + pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty.ToString());
            logger.Info("Preco               : ORDEM A MERCADO ");
            logger.Info("DtVencimento        :" + pParametros.EnviarOrdemRequest.OrdemInfo.ExpireDate.ToString());
            logger.Info("ClasseCliente       : Institucional");


            CadastroPapeisRequest _CadastroPapeisRequest = new CadastroPapeisRequest();
            _CadastroPapeisRequest.Instrumento = pParametros.EnviarOrdemRequest.OrdemInfo.Symbol;

            CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis = new PersistenciaCarteiraRecomendada().ObterInformacoesPapeis(_CadastroPapeisRequest);             


            int FatorCotacao = int.Parse(CadastroPapeis.Objeto.FatorCotacao);

            // TRATAMENTO [ FRACIONARIO / INTEGRAL ]
            int LoteNegociacao = int.Parse(CadastroPapeis.Objeto.LoteNegociacao);
            int ModuloLoteNegociacao = pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty % LoteNegociacao;

            if (pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty >= LoteNegociacao)
            {

                // ENVIAR INTEGRAL
                pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty -= ModuloLoteNegociacao;
                logger.Info("Inicia operação de compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado Integral");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado integral efetuada com sucesso.");

                    logger.Info("Envia a ordem para o roteador de ordens");

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem institucional enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }
                }

                if (ModuloLoteNegociacao > 0)
                {
                    // ENVIAR FRACIONARIO
                    logger.Info("Calcula restante a ser enviado para o mercado fracionário");

                    // ENVIAR FRACIONARIO
                    pParametros.EnviarOrdemRequest.OrdemInfo.OrderQty = ModuloLoteNegociacao;
                    pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";
                    pParametros.EnviarOrdemRequest.OrdemInfo.ClOrdID = this.CtrlNumber(CodigoCarteiraRecomendada);

                    logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                    // Inseri a Ordem solicitada no banco de dados    
                    if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                    {
                        //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                        logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionario efetuada com sucesso.");

                        logger.Info("Envia a ordem para o roteador de ordens");

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                        ExecutarOrdemResponse RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                        pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                        // VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                        if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                        {
                            logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                            PipeLineResponse.DataResposta = DateTime.Now;
                            PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                        }
                        else
                        {
                            logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                            CriticaInfo = new PipeLineCriticaInfo();
                            CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                            CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                            CriticaInfo.DataHoraCritica = DateTime.Now;

                            // Adiciona as criticas no pipeline de risco.
                            CriticaInfoCollection.Add(CriticaInfo);

                        }
                    }
                }
            }
            else
            {
                //FRACIONARIO
                pParametros.EnviarOrdemRequest.OrdemInfo.Symbol += "F";

                logger.Info("Inicia compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário");

                // Inseri a Ordem solicitada no banco de dados    
                if (InserirOrdemCliente(pParametros.EnviarOrdemRequest))
                {
                    //TODO: COMPRA O PAPEL COM O SERVICO DE ORDENS                                 
                    logger.Info("Compra de: " + pParametros.EnviarOrdemRequest.OrdemInfo.Symbol + " no mercado fracionário efetuada com sucesso.");

                    //ENVIA ORDEM PARA O ROTEADOR                                    
                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = RetornaCodigoCliente(227, pParametros.EnviarOrdemRequest.OrdemInfo.Account);

                    logger.Info("Envia a ordem para o roteador de ordens");


                    var RespostaRoteador = this.EnviarOrdemRoteador(pParametros.EnviarOrdemRequest.OrdemInfo);

                    pParametros.EnviarOrdemRequest.OrdemInfo.Account = int.Parse(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Remove(pParametros.EnviarOrdemRequest.OrdemInfo.Account.ToString().Length - 1, 1));

                    //// VERIFICA SE A ORDEM FOI ENVIADA COM SUCESSO PARA O ROTEADOR
                    if (RespostaRoteador.DadosRetorno.StatusResposta == StatusRoteamentoEnum.Sucesso)
                    {
                        logger.Info("Ordem enviada com sucesso para o Roteador de Ordens");

                        PipeLineResponse.DataResposta = DateTime.Now;
                        PipeLineResponse.DescricaoResposta = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        PipeLineResponse.StatusResposta = OMS.Risco.Lib.Enum.CriticaRiscoEnum.Sucesso;
                    }
                    else
                    {
                        logger.Info("Ocorreu um erro ao enviar a ordem para o roteador de Ordens");

                        CriticaInfo = new PipeLineCriticaInfo();
                        CriticaInfo.Critica = RespostaRoteador.DadosRetorno.Ocorrencias[0].Ocorrencia;
                        CriticaInfo.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroRoteadorOrdem;
                        CriticaInfo.DataHoraCritica = DateTime.Now;

                        // Adiciona as criticas no pipeline de risco.
                        CriticaInfoCollection.Add(CriticaInfo);

                    }

                }
            }

            return PipeLineResponse;

        }

        #endregion

        #region VALIDACAO E PARSEAMENTO DA ORDEM ENVIADA

        private ValidacaoRiscoResponse ValidarPipeLineOrdem(ValidacaoRiscoRequest pParametros,string CodigoCarteiraRecomendada)
        {
            ValidacaoRiscoResponse PipeLineResponse = new ValidacaoRiscoResponse();

            if (pParametros.EnviarOrdemRequest.OrdemInfo.Side == RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Compra)
                PipeLineResponse = ComprarCarteiraRecomendada(pParametros, CodigoCarteiraRecomendada);

            if (pParametros.EnviarOrdemRequest.OrdemInfo.Side == RoteadorOrdens.Lib.Dados.OrdemDirecaoEnum.Venda)
                PipeLineResponse = VenderCarteiraRecomendada(pParametros, CodigoCarteiraRecomendada);

            return PipeLineResponse;
        }

        private OrdemFIXResponse<OrdemInfo> ParsearOrdemCliente(ClienteOrdemInfo pParametro, string CodigoCarteiraRecomendada)
        {
            OrdemFIXResponse<OrdemInfo> Response = new OrdemFIXResponse<OrdemInfo>();

            OrdemInfo OrdemInfo = new OrdemInfo();
            try
            {
                DateTime DataAux = DateTime.Now;

                #region [Vencimento da ordens]

                // VALIDA O VENCIMENTO DA ORDEM

                if (pParametro.ValidadeOrdem == RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia)
                {
                    // ORDEM VALIDA PARA O DIA
                    OrdemInfo.ExpireDate = new DateTime(DataAux.Year, DataAux.Month, DataAux.Day, 23, 59, 59);
                }
                else if (pParametro.ValidadeOrdem == RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada)
                {
                    //ORDEM VAC ( VALIDA ATE O CANCELAMENTO)
                    OrdemInfo.ExpireDate = null;
                }
                else if (pParametro.ValidadeOrdem == RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData)
                {
                    OrdemInfo.ExpireDate = pParametro.DataValidade.Value;
                }

                #endregion

                OrdemInfo.StopStartID = pParametro.CodigoStopStart;
                OrdemInfo.MinQty = pParametro.QuantidadeMinima;

                if (string.IsNullOrEmpty(pParametro.NumeroControleOrdem))
                {
                    OrdemInfo.ClOrdID = CtrlNumber(CodigoCarteiraRecomendada);
                }
                else
                {
                    OrdemInfo.ClOrdID = CtrlNumber(CodigoCarteiraRecomendada);
                    OrdemInfo.OrigClOrdID = pParametro.NumeroControleOrdem;
                }

                OrdemInfo.MaxFloor = pParametro.QuantidadeAparente;
                OrdemInfo.Account = pParametro.CodigoCliente;
                OrdemInfo.ChannelID = int.Parse(pParametro.PortaControleOrdem);
                OrdemInfo.ExecBroker = "227";
                OrdemInfo.ExpireDate = pParametro.DataValidade;
                OrdemInfo.OrderQty = pParametro.Quantidade;
                OrdemInfo.OrdStatus = OrdemStatusEnum.ENVIADAPARAOROTEADORDEORDENS;
                OrdemInfo.Price = pParametro.Preco;
                OrdemInfo.Side = pParametro.DirecaoOrdem;
                OrdemInfo.Symbol = pParametro.Instrumento;
                OrdemInfo.RegisterTime = DateTime.Now;
                OrdemInfo.OrdType = pParametro.TipoDeOrdem;
                OrdemInfo.Exchange = "BOVESPA";
                OrdemInfo.TimeInForce = pParametro.ValidadeOrdem;

                if (OrdemInfo.Symbol.Substring(OrdemInfo.Symbol.Length - 1, 1) == "F")
                {
                    OrdemInfo.Symbol = OrdemInfo.Symbol.Remove(OrdemInfo.Symbol.Length - 1);
                }

                OrdemInfo.Exchange = "BOVESPA";
                Response.Objeto = OrdemInfo;

                Response.StatusResposta = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso;
                Response.DataResposta = DateTime.Now;

                Response.DescricaoResposta = "Ordem parseado com sucesso";
            }
            catch (Exception ex)
            {
                PipeLineCriticaInfo info = new PipeLineCriticaInfo();

                info.Critica = ex.Message;
                info.CriticaTipo = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.ErroNegocio;
                info.DataHoraCritica = DateTime.Now;

                Response.CriticaInfo = new List<PipeLineCriticaInfo>();
                Response.CriticaInfo.Add(info);

                Response.StatusResposta = OMS.Ordens.Lib.Enum.CriticaRiscoEnum.Exception;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Ocorreu um erro ao parsear a mensagem";

            }

            return Response;
        }

        #endregion

        #region ROTEADOR DE ORDENS

        private ExecutarOrdemResponse EnviarOrdemRoteador(OrdemInfo OrdemInfo)
        {
            try
            {
                // Instancia servico de ordens
                logger.Info("Invoca servico de roteamento de ordens");
                IRoteadorOrdens ServicoRoteador = Ativador.Get<IRoteadorOrdens>();

                OrdemInfo.TransactTime = DateTime.Now;

                // Envia a ordem para o reteador e aguarda o retorno
                logger.Info("Envia a ordem para o roteador");
                ExecutarOrdemResponse RespostaOrdem =
                ServicoRoteador.ExecutarOrdem(new RoteadorOrdens.Lib.Mensagens.ExecutarOrdemRequest()
                {
                    info = OrdemInfo
                });

                return RespostaOrdem;

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao enviar a ordem para o roteador");
                logger.Info("Descrição     :" + ex.Message);

                throw (ex);
            }

        }

        #endregion

        #region METODOS AUXILIARES

        private string CtrlNumber(string CodigoCarteiraRecomendada)
        {
                return string.Format("{0}{1}{2}{3}{4}",
                        "#CR#",
                        CodigoCarteiraRecomendada + "#",
                        DateTime.Now.ToString("ddMMyyyyhhmmss").Replace("/", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty),
                        "-",
                        new Random().Next(0, 99999999).ToString());
        }

        private int RetornaCodigoCliente(int CodigoCorretora, int CodigoCliente)
        {

            int valor = 0;

            valor = (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5) +

            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8) +
            (int.Parse(CodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3) +
            (int.Parse(CodigoCliente.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            valor = valor % 11;

            if (valor == 0 || valor == 1)
            {
                valor = 0;
            }
            else
            {
                valor = 11 - valor;
            }

            return int.Parse(string.Format("{0}{1}", CodigoCliente, valor));

        }

        private bool InserirOrdemCliente(EnviarOrdemRoteadorRequest pParametros)
        {
            pParametros.OrdemInfo.OrderQtyRemmaining = pParametros.OrdemInfo.OrderQty;

            // Inseri a Ordem solicitada no banco de dados
            return new PersistenciaOrdens().InserirOrdemCliente(pParametros.OrdemInfo);

        }

        #endregion
   
    }
}
