using System;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using log4net;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{

    public partial class ContaCorrente : PaginaBaseAutenticada
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int GetCodigoBMF
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodBMF"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodigoBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodBovespa"], out lRetorno);

                return lRetorno;
            }
        }

        public TransporteSaldoDeConta SaldoDeConta { get; set; }

        public new void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados" },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCarregarHtmlComDados });
        }

        private string ResponderCarregarHtmlComDados()
        {
            string lRetorno = string.Empty;

            try
            {
                this.SaldoDeConta = this.BuscarSaldoELimitesNoServico();
                //this.SaldoDeConta = this.BuscarSaldoEmContaNoServico();

                //this.hidSaldoEmContaJson.Value = Newtonsoft.Json.JsonConvert.SerializeObject(this.SaldoDeConta);

                lRetorno = base.RetornarSucessoAjax(/*lTransporte, */"Saldo atualizado com sucesso");
            }
            catch (Exception lErro)
            {
                lRetorno = RetornarErroAjax("Erro ao atualizar saldo", lErro);
            }

            return lRetorno;
        }

        private TransporteSaldoDeConta BuscarSaldoELimitesNoServico()
        {
            TransporteSaldoDeConta lSaldo = this.BuscarSaldoEmContaNoServico();

            var lLimitesDoCliente = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(new ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo>()
            {
                 EntidadeCadastro = new RiscoLimiteAlocadoInfo()
                 {
                        ConsultaIdCliente = this.GetCodigoBovespa,
                        NovoOMS = true
                 }
            });

            if (lLimitesDoCliente.StatusResposta== OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lSaldo.CarregarDadosDeLimite(lLimitesDoCliente.Resultado);
            }
            //else
            //{
            //    //TODO: Erro!
            //}

            return lSaldo;
        }


        private TransporteSaldoDeConta BuscarSaldoEmContaNoServico()
        {
            TransporteSaldoDeConta lRetorno = new TransporteSaldoDeConta();

            try
            {
                IServicoContaCorrente servicoCC = this.InstanciarServico<IServicoContaCorrente>();
                SaldoContaCorrenteResponse<ContaCorrenteInfo> resCC = servicoCC.ObterSaldoContaCorrente(new SaldoContaCorrenteRequest()
                {
                    IdCliente = this.GetCodigoBovespa
                });

                SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> resBMF = servicoCC.ObterSaldoContaCorrenteBMF(new SaldoContaCorrenteRequest()
                {
                    IdCliente = this.GetCodigoBMF
                });

                if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resCC.Objeto);
                }
                else if (resCC.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resBMF.Objeto);
                }
                else if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resCC.Objeto, resBMF.Objeto);
                }
                else
                {
                    gLogger.Error(string.Format("Erro: {0}\r\n{1}", resCC.StatusResposta, resCC.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            return lRetorno;
        }
    }
}