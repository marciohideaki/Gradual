using System;
using System.Configuration;
using Gradual.OMS.Library.Servicos;
using Gradual.Servico.GerenteHistClienteParamValor.Lib;
using Gradual.Servico.GerenteHistClienteParamValor.Lib.Mensagem;
using Gradual.Servico.GerenteHistClienteParamValor.Persistencias;
using log4net;

namespace Gradual.Servico.GerenteHistClienteParamValor
{
    public class ServicoGerenteHistoricoRiscoClienteParametroValor : IServicoControlavel, IServicoGerenteHistoricoRiscoClienteParametroValor
    {
        #region | Atributos

        private ServicoStatus ServicoStatus = ServicoStatus.Parado;

        private System.Timers.Timer gTimer = new System.Timers.Timer();

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades

        private long GetIntervaloExecucao
        {
            get
            {
                return 58    //--> 58 minutos
                     * 60    //--> convertendo segundo em minuto
                     * 1000; //--> convertendo milissegundos para segundos
            }
        }

        private int GetHoraDeEnvio
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(ConfigurationManager.AppSettings["HorarioDeDisparoDoServico"], out lRetorno))
                    return 22;

                return lRetorno.Hour;
            }
        }

        #endregion

        #region | Contrutores

        public ServicoGerenteHistoricoRiscoClienteParametroValor()
        {
            log4net.Config.XmlConfigurator.Configure();

            gTimer.Elapsed += new System.Timers.ElapsedEventHandler(gTimer_Elapsed);
        }

        #endregion

        #region | Eventos

        private void gTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour.Equals(this.GetHoraDeEnvio))
            {
                this.IniciarProcessoGuadarHistorico(new GerenteHistoricoRiscoClienteParametroValorRequest() { TipoRequisitante = TipoRequisitante.ServicoGerenteHistorico });
            }
        }

        #endregion 

        #region | Implementação IServicoControlavel

        public void IniciarServico()
        {
            gTimer.Interval = this.GetIntervaloExecucao;
            gTimer.Enabled = true;
            gTimer.Start();

            this.ServicoStatus = OMS.Library.Servicos.ServicoStatus.EmExecucao;

            gLogger.Info(string.Format("Seriço iniciado às: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
        }

        public void PararServico()
        {
            this.gTimer.Enabled = false;
            this.ServicoStatus = OMS.Library.Servicos.ServicoStatus.Parado;

            gLogger.Info(string.Format("Seriço parado às: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
        }

        public ServicoStatus ReceberStatusServico()
        {
            return this.ServicoStatus;
        }

        #endregion

        #region | Implementação IServicoGerenteHistoricoClienteParametroValor

        public GerenteHistoricoEstadoUltimoHistoricoResponse RecuperaStatusUltimoHistorico(GerenteHistoricoEstadoUltimoHistoricoRequest pParametro)
        {
            //--> Declaração de variável e chamada do Método
            GerenteHistoricoEstadoUltimoHistoricoResponse lRetorno =
                new GerenteHistoricoRiscoClienteParametroValorDbLib().RecuperaStatusUltimoHistorico(pParametro);

            if (lRetorno.StatusResposta != OMS.Library.MensagemResponseStatusEnum.OK)
            {   //--> Registrando log do erro
                gLogger.Error(string.Format("Erro às {0} em RecuperaStatusUltimoHistorico: {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), lRetorno.DescricaoResposta));
            }

            return lRetorno;
        }

        public GerenteHistoricoRiscoClienteParametroValorResponse IniciarProcessoGuadarHistorico(GerenteHistoricoRiscoClienteParametroValorRequest pParametro)
        {
            var lRetorno = new GerenteHistoricoRiscoClienteParametroValorDbLib().IniciarProcessoGuadarHistorico(pParametro);

            if (lRetorno.StatusResposta != OMS.Library.MensagemResponseStatusEnum.OK)
            {   //--> Registrando log do erro
                gLogger.Error(string.Format("Erro às {0} em IniciarProcessoGuadarHistorico: {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), lRetorno.DescricaoResposta));
            }

            return lRetorno;
        }

        #endregion
    }
}
