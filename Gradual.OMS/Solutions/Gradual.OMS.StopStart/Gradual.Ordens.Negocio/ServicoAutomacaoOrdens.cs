using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Automacao.Ordens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using log4net;
using System.ServiceModel;

namespace Gradual.OMS.Sistema.Automacao.Ordens
{
    public enum OrdemStopStatus : int
    {
        RegistradaAplicacao = 1,
        EnviadaMDS = 2,
        AceitoMDS = 3,
        RejeitadoMDS = 4,
        ExecutadoMDS = 5,
        CancelamentoRegistradoAplicacao = 6,
        CancelamentoEnviadoMDS = 7,
        CancelamentoAceitoMDS = 8,
        CancelamentoRejeitadoMDS = 9,
        Execucao = 10,
    };
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class ServicoAutomacaoOrdens : AutomacaoOrdensDados, IServicoAutomacaoOrdens, IDisposable
    { 
        #region Privates

        private string EventLogSource
        {
            get
            {
                return ConfigurationManager.AppSettings["EventLogSource"].ToString();
            }
        }

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region IServicoOrdens Members

        public ArmarStartStopResponse ArmarStopMovel(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            res.IdStopStart = 0;
            return res;
        }

        /// <summary>
        /// Registra um Stop Simultaneo no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        public ArmarStartStopResponse ArmarStopSimultaneo(ArmarStartStopRequest req)
        {
            try
            {
                req._AutomacaoOrdensInfo.StopStartID = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopSimultaneo ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                base.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopSimultaneo ", req._AutomacaoOrdensInfo.StopStartID," Enviado para o MDS"));
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "ArmarStopSimultaneo: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStopSimultaneo: ", ex.Message));
            }
        }

        /// <summary>
        /// Registra um Stop Loss no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        public ArmarStartStopResponse ArmarStopLoss(ArmarStartStopRequest req)
        {
            try
            {
                req._AutomacaoOrdensInfo.StopStartID = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopLoss ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                base.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopLoss ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));
                return res;

            }
            catch (Exception ex){
                
                logger.Error(string.Format("{0}{1}", "ArmarStopLoss: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStopLoss: ", ex.Message));
            }
        }

        /// <summary>
        /// Registra um Stop Gain no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        public ArmarStartStopResponse ArmarStopGain(ArmarStartStopRequest req)
        {
            try
            {
                req._AutomacaoOrdensInfo.StopStartID = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopGain ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                base.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStopGain ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));
                return res;
            }
            catch (Exception ex){

                logger.Error(string.Format("{0}{1}", "ArmarStopGain: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStopGain: ", ex.Message));
            }
        }

        /// <summary>
        /// Arma um StopStart de compra no MDS
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        public ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req)
        {
            try
            {
                req._AutomacaoOrdensInfo.StopStartID = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStartCompra ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                base.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);
                logger.Info(string.Format("{0}{1}{2}", "ArmarStartCompra ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "ArmarStartCompra: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStartCompra: ", ex.Message));
            }
        }

        /// <summary>
        /// Cancela um StopStart que já se encontra armado no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        public CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req)
        {
            try
            {
                base.CancelaOrdemStopStart(req.IdStopStart, req.IdStopStartStatus);
                logger.Info(string.Format("{0}{1}{2}", "Solicitação de Cancelamento do StopStart ", req.IdStopStart, " Registrado na aplicação"));
                Formatador.CancelarOrdemStop(req.Instrument, req.IdStopStart);
                //base.CancelaOrdemStopStart(req.IdStopStart, (int)OrdemStopStatus.CancelamentoEnviadoMDS);
                //logger.Info(string.Format("{0}{1}{2}", " Cancelamento do StopStart ", req.IdStopStart, " Enviado para o MDS"));
                CancelarStartStopOrdensResponse res = new CancelarStartStopOrdensResponse();
             
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "CancelaOrdemStopStart: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "CancelaOrdemStopStart: ", ex.Message));
            }
        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
