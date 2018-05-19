using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Gradual.OMS.Contratos.Automacao.Ordens;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Library;


namespace Gradual.OMS.Sistema.Automacao.Ordens
{
    public class ServicoAutomacaoOrdens : AutomacaoOrdensDados, IServicoAutomacaoOrdens
    { 
        #region Privates

        private string EventLogSource
        {
            get
            {
                return ConfigurationSettings.AppSettings["EventLogSource"].ToString();
            }
        }


        private void WriteEventLog(string Message){
            EventLog.WriteEntry(EventLogSource, Message, EventLogEntryType.Error);
        }

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
                req._AutomacaoOrdensInfo.IdStopStart = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}","ArmarStopSimultaneo: ", ex.Message));
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
            try{
                req._AutomacaoOrdensInfo.IdStopStart = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                return res;
            }
            catch (Exception ex){
                Log.EfetuarLog(ex, string.Format("{0}{1}", "ArmarStopLoss: ", ex.Message));
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
            try{
                req._AutomacaoOrdensInfo.IdStopStart = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                return res;
            }
            catch (Exception ex){
                Log.EfetuarLog(ex, string.Format("{0}{1}", "ArmarStopGain: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStopGain: ", ex.Message));
            }
        }

        /// <summary>
        /// Registra um start de compra.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        public ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req)
        {
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = base.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                ArmarStartStopResponse res = new ArmarStartStopResponse();
                res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "ArmarStartCompra: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "ArmarStartCompra: ", ex.Message));
            }
        }

        /// <summary>
        /// Cancela um ordem que esta em aberto no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        public CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req)
        {
            try{
                base.CancelaOrdemStopStart(req.IdStopStart, req.IdStopStartStatus);
                Formatador.CancelarOrdemStop(req.Instrument, req.IdStopStart);
                CancelarStartStopOrdensResponse res = new CancelarStartStopOrdensResponse();
                res.StatusResposta = Gradual.OMS.Contratos.Comum.Mensagens.MensagemResponseStatusEnum.OK;
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "CancelaOrdemStopStart: ", ex.Message));
                throw new Exception(string.Format("{0}{1}", "CancelaOrdemStopStart: ", ex.Message));
            }
        }

        public SelecionarOrdemResponse SelecionarOrdem(SelecionarOrdemRequest req)
        {
            throw new NotImplementedException();
        }

        public ListarOrdensResponse ListarOrdem(ListarOrdensRequest req)
        {
            throw new NotImplementedException();
        }

        #endregion
      
    }
}
