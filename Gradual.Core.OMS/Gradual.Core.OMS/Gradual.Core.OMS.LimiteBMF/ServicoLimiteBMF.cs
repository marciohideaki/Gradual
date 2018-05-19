using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.Ordens.Persistencia;
using System.ServiceModel;

namespace Gradual.Core.OMS.LimiteBMF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServicoLimiteBMF:IServicoLimiteBMF
    {

        public InserirLimiteClienteBMFResponse AtualizarLimiteBMF(InserirLimiteClienteBMFRequest InserirLimiteClienteBMFRequest)
        {
            InserirLimiteClienteBMFResponse response = new InserirLimiteClienteBMFResponse();
            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.AtualizarLimiteBMF(InserirLimiteClienteBMFRequest);              
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
            } 

            return response;
        }

        public InserirLimiteBMFInstrumentoResponse AtualizarLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest InserirLimiteBMFInstrumentoRequest)
        {
            InserirLimiteBMFInstrumentoResponse response = new InserirLimiteBMFInstrumentoResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.AtualizarLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest);
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
            }

            return response;
        }

        public ListarLimiteBMFResponse ObterLimiteBMFCliente(ListarLimiteBMFRequest ListarLimiteBMFRequest)
        {
            ListarLimiteBMFResponse response = new ListarLimiteBMFResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.ObterLimiteBMFCliente(ListarLimiteBMFRequest);
            }
            catch (Exception ex)
            {
                
            }

            return response;
        }

        public ExcluirLimiteBMFResponse InativarLimiteInstrumento(ExcluirLimiteBMFRequest ExcluirLimiteBMFRequest)
        {
            ExcluirLimiteBMFResponse response = new ExcluirLimiteBMFResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.InativarLimiteInstrumento(ExcluirLimiteBMFRequest);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public InativarLimiteContratoResponse InativarLimiteContrato(InativarLimiteContratoRequest InativarLimiteContratoRequest)
        {
            InativarLimiteContratoResponse response = new InativarLimiteContratoResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.InativarLimiteCliente(InativarLimiteContratoRequest);
            }
            catch (Exception ex)
            {

            }

            return response;
        }


        #region IServicoLimiteBMF Members


        #endregion

        #region IServicoLimiteBMF Members


     

        #endregion


        public InserirLimiteClienteBMFResponse AtualizarSpiderLimiteBMF(InserirLimiteClienteBMFRequest InserirLimiteClienteBMFRequest)
        {
            InserirLimiteClienteBMFResponse response = new InserirLimiteClienteBMFResponse();
            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.AtualizarSpiderLimiteBMF(InserirLimiteClienteBMFRequest);
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
            }

            return response;
        }

        public InserirLimiteBMFInstrumentoResponse AtualizarSpiderLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest InserirLimiteBMFInstrumentoRequest)
        {
            InserirLimiteBMFInstrumentoResponse response = new InserirLimiteBMFInstrumentoResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.AtualizarSpiderLimiteInstrumentoBMF(InserirLimiteBMFInstrumentoRequest);
            }
            catch (Exception ex)
            {
                response.bSucesso = false;
            }

            return response;
        }

        public ListarLimiteBMFResponse ObterSpiderLimiteBMFCliente(ListarLimiteBMFRequest ListarLimiteBMFRequest)
        {
            ListarLimiteBMFResponse response = new ListarLimiteBMFResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.ObterSpiderLimiteBMFCliente(ListarLimiteBMFRequest);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public ExcluirLimiteBMFResponse InativarSpiderLimiteInstrumento(ExcluirLimiteBMFRequest ExcluirLimiteBMFRequest)
        {
            ExcluirLimiteBMFResponse response = new ExcluirLimiteBMFResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();
                response = PersistenciaOrdens.InativarSpiderLimiteInstrumento(ExcluirLimiteBMFRequest);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public InativarLimiteContratoResponse InativarSpiderLimiteContrato(InativarLimiteContratoRequest InativarLimiteContratoRequest)
        {
            InativarLimiteContratoResponse response = new InativarLimiteContratoResponse();

            try
            {
                PersistenciaOrdens PersistenciaOrdens = new PersistenciaOrdens();

                response = PersistenciaOrdens.InativarSpiderLimiteCliente(InativarLimiteContratoRequest);
            }
            catch (Exception ex)
            {

            }

            return response;
        }
    }
}
