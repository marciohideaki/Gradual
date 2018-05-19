using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens.Lib.Enum;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.Ordens.Persistencia.Lib;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.Risco.Lib.Info;
using Gradual.OMS.Risco.Persistencia.Lib;
using System.Collections.Specialized;
using System.Configuration;


namespace Gradual.OMS.Ordens
{
    public partial class ServicoOrdens
    {

        private OrdemFIXResponse<OrdemCancelamentoInfo> ParsearOrdemCancelamentoCliente(ClienteCancelamentoInfo pParametro)
        {
            OrdemFIXResponse<OrdemCancelamentoInfo> Response =
            new OrdemFIXResponse<OrdemCancelamentoInfo>();


            try
            {
        
                OrdemInfo OrdemInfo = new PersistenciaOrdens().SelecionarOrdemCliente(pParametro.OrderID);

                OrdemCancelamentoInfo CancelamentoInfo =
                    new OrdemCancelamentoInfo();

                CancelamentoInfo.ClOrdID = CtrlNumber;
                CancelamentoInfo.OrigClOrdID = OrdemInfo.ClOrdID;
                CancelamentoInfo.ChannelID = OrdemInfo.ChannelID;
                CancelamentoInfo.Account = OrdemInfo.Account;                                 
                CancelamentoInfo.OrderID = OrdemInfo.ExchangeNumberID;
                CancelamentoInfo.OrderQty = OrdemInfo.OrderQty;
                CancelamentoInfo.Side = OrdemInfo.Side;
                CancelamentoInfo.Symbol = OrdemInfo.Symbol;

                if ((OrdemInfo.OrdStatus != OrdemStatusEnum.NOVA) || (OrdemInfo.OrdStatus == OrdemStatusEnum.SUBSTITUIDA)
                    || (OrdemInfo.OrdStatus == OrdemStatusEnum.PARCIALMENTEEXECUTADA))
                {
                    Lib.Info.PipeLineCriticaInfo info = new Lib.Info.PipeLineCriticaInfo();

                    info.Critica = "Não é possível cancelar uma oferta em processamento. Esta oferta pode estar cancelada ou cancelando / executada ou executando. ";
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    Response.CriticaInfo = new List<Lib.Info.PipeLineCriticaInfo>();

                    Response.CriticaInfo.Add(info);
                    Response.StatusResposta = CriticaRiscoEnum.ErroNegocio;
                    Response.DataResposta = DateTime.Now;
                    Response.DescricaoResposta = "O Sistema de risco encontrou <" + Response.CriticaInfo.Count.ToString() + "> item(s) a serem verificados";

                    return Response;

                }
          
    

                if (OrdemInfo.Symbol.Substring(OrdemInfo.Symbol.Length - 1, 1) == "F"){
                    OrdemInfo.Symbol = OrdemInfo.Symbol.Remove(OrdemInfo.Symbol.Length - 1);
                }

                // Defini o Exchange pelo cadastro de papeis
                CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis = new ServicoCadastroPapeis().ObterInformacoesIntrumento(
                                                                        new CadastroPapeisRequest()
                                                                        {
                                                                            Instrumento = OrdemInfo.Symbol
                                                                        });

                if (CadastroPapeis.Objeto == null)
                {
                    Lib.Info.PipeLineCriticaInfo info = new Lib.Info.PipeLineCriticaInfo();

                    info.Critica = "Instrumento não encontrado";
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    Response.CriticaInfo = new List<Lib.Info.PipeLineCriticaInfo>();

                    Response.CriticaInfo.Add(info);
                    Response.StatusResposta = CriticaRiscoEnum.ErroNegocio;
                    Response.DataResposta = DateTime.Now;
                    Response.DescricaoResposta = "O Sistema de risco encontrou <" + Response.CriticaInfo.Count.ToString() + "> item(s) a serem verificados";

                    return Response;

                }

                if (CadastroPapeis.Objeto.TipoMercado == OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FUTURO)
                {
                    CancelamentoInfo.Exchange = "BMF";               
                    CancelamentoInfo.SecurityID = new PersistenciaCadastroAtivos().ObterSecurityList(OrdemInfo.Symbol);
                }
                else
                {
                    CancelamentoInfo.Exchange = "BOVESPA";
                }


                Response.Objeto = CancelamentoInfo;

                Response.StatusResposta = CriticaRiscoEnum.Sucesso;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Ordem parseado com sucesso";
            }
            catch(Exception ex)
            {

                Lib.Info.PipeLineCriticaInfo info = new Lib.Info.PipeLineCriticaInfo();            

                info.Critica = ex.Message;
                info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                info.DataHoraCritica = DateTime.Now;

                Response.CriticaInfo = new List<Lib.Info.PipeLineCriticaInfo>();

                Response.CriticaInfo.Add(info);

                Response.StatusResposta = CriticaRiscoEnum.Exception;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Ocorreu um erro ao parsear a mensagem";

            }

            return Response;
        }

        private OrdemFIXResponse<OrdemInfo> ParsearOrdemCliente(ClienteOrdemInfo pParametro)
        {
            OrdemFIXResponse<OrdemInfo> Response =
                new OrdemFIXResponse<OrdemInfo>();

            OrdemInfo OrdemInfo =
                   new OrdemInfo();  
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
                else if (pParametro.ValidadeOrdem == RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaAteSerCancelada){
                        //ORDEM VAC ( VALIDA ATE O CANCELAMENTO)
                        OrdemInfo.ExpireDate = null;
                    }
                else if (pParametro.ValidadeOrdem == RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidoAteDeterminadaData)
                {
                    logger.Info("Data Enviada: " + pParametro.DataValidade.Value.ToString());
                    OrdemInfo.ExpireDate = pParametro.DataValidade.Value;
                }
    
                #endregion

                OrdemInfo.StopStartID = pParametro.CodigoStopStart;
                OrdemInfo.MinQty = pParametro.QuantidadeMinima;

                if (string.IsNullOrEmpty(pParametro.NumeroControleOrdem)){
                    OrdemInfo.ClOrdID = CtrlNumber;
                }
                else{
                    OrdemInfo.ClOrdID = CtrlNumber;
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

                // Defini o Exchange pelo cadastro de papeis
                CadastroPapeisResponse<CadastroPapelInfo> CadastroPapeis = new ServicoCadastroPapeis().ObterInformacoesIntrumento(
                                                                        new CadastroPapeisRequest()
                                                                        {
                                                                            Instrumento = OrdemInfo.Symbol
                                                                        });


                if (CadastroPapeis.Objeto == null)
                {

                    Lib.Info.PipeLineCriticaInfo info = new Lib.Info.PipeLineCriticaInfo();             

                    info.Critica = "Instrumento não encontrado";
                    info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                    info.DataHoraCritica = DateTime.Now;

                    Response.CriticaInfo = new List<Lib.Info.PipeLineCriticaInfo>();

                    Response.CriticaInfo.Add(info);
                    Response.StatusResposta = CriticaRiscoEnum.ErroNegocio;
                    Response.DataResposta = DateTime.Now;
                    Response.DescricaoResposta = "O Sistema de risco encontrou <" + Response.CriticaInfo.Count.ToString() + "> item(s) a serem verificados";

                    return Response;

                }
                
                if (CadastroPapeis.Objeto.TipoMercado == OMS.CadastroPapeis.Lib.Enum.TipoMercadoEnum.FUTURO)
                {
                    OrdemInfo.Exchange = "BMF";
                    OrdemInfo.SecurityID = new PersistenciaCadastroAtivos().ObterSecurityList(OrdemInfo.Symbol);
                }
                else
                {
                    OrdemInfo.Exchange = "BOVESPA";
                }

                Response.Objeto = OrdemInfo;         
            
                Response.StatusResposta = CriticaRiscoEnum.Sucesso;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Ordem parseado com sucesso";
            }
            catch(Exception ex)
            {
                Lib.Info.PipeLineCriticaInfo info = new Lib.Info.PipeLineCriticaInfo();
                logger.Error("ERRO : "+ ex.Message, ex);

                info.Critica = ex.Message;
                info.CriticaTipo = CriticaRiscoEnum.ErroNegocio;
                info.DataHoraCritica = DateTime.Now;

                Response.CriticaInfo = new List<Lib.Info.PipeLineCriticaInfo>();
                Response.CriticaInfo.Add(info);

                Response.StatusResposta = CriticaRiscoEnum.Exception;
                Response.DataResposta = DateTime.Now;
                Response.DescricaoResposta = "Ocorreu um erro ao parsear a mensagem";

            }

            return Response;
        }             
    }
}
