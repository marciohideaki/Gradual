using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.ServiceModel;
using Gradual.Spider.LimiteRestricao.Lib;
using log4net;
using Gradual.Spider.LimiteRestricao.DbLib;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;

namespace Gradual.Spider.LimiteRestricao
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSpiderPlataforma : IServicoSpiderPlataforma, IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private RiscoPlataformaDbLib gPlataformaDbLib = new RiscoPlataformaDbLib(); 
        #endregion

        #region IServicoControlavel
        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            catch (Exception ex)
            {

            }
        }

        public void PararServico()
        {
            try
            {

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            var lRetorno = ServicoStatus.EmExecucao;
            try
            {

            }
            catch (Exception ex)
            {
                
                throw;
            }
            return lRetorno;
        }

        #endregion

        #region Métodos
        public RiscoSelecionarPlataformaClienteResponse SelecionarPlataformaClienteSpider(RiscoSelecionarPlataformaClienteRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaClienteResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SelecionarPlataformaClienteSpider(pParametro);

                gLogger.InfoFormat("SelecionarPlataformaClienteSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarPlataformaResponse ListaPlataformaSpider()
        {
            var lRetorno = new RiscoListarPlataformaResponse();

            try
            {
                lRetorno = gPlataformaDbLib.ListaPlataformaSpider();

                gLogger.InfoFormat("ListaPlataformaSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarOperadorResponse ListarOperadorSpider()
        {
            var lRetorno = new RiscoListarOperadorResponse();

            try
            {
                lRetorno = gPlataformaDbLib.ListarOperadorSpider();

                gLogger.InfoFormat("ListarOperadorSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSelecionarPlataformaContaMasterResponse SelecionarPlataformaContaMasterSpider(RiscoSelecionarPlataformaContaMasterRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaContaMasterResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SelecionarPlataformaContaMasterSpider(pParametro);

                gLogger.InfoFormat( "SelecionarPlataformaContaMasterSpider Tipo de Plataforma-> [{0}] da camada de serviço", lRetorno.Resultado.TipoPlataforma );
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSelecionarPlataformaAssessorResponse SelecionarPlataformaAssessorSpider(RiscoSelecionarPlataformaAssessorRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaAssessorResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SelecionarPlataformaAssessorSpider(pParametro);

                gLogger.InfoFormat("SelecionarPlataformaAssessorSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSelecionarPlataformaOperadorResponse SelecionarPlataformaOperadorSpider(RiscoSelecionarPlataformaOperadorRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaOperadorResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SelecionarPlataformaOperadorSpider(pParametro);

                gLogger.InfoFormat("SelecionarPlataformaOperadorSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaClienteResponse SalvarPlataformaClienteSpider(RiscoSalvarPlataformaClienteRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaClienteResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SalvarPlataformaClienteSpider(pParametro);

                gLogger.InfoFormat("SalvarPlataformaClienteSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaContaMasterResponse SalvarPlataformaContaMasterSpider(RiscoSalvarPlataformaContaMasterRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaContaMasterResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SalvarPlataformaContaMasterSpider(pParametro);

                gLogger.InfoFormat("SalvarPlataformaContaMasterSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaAssessorResponse SalvarPlataformaAssessorSpider(RiscoSalvarPlataformaAssessorRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaAssessorResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SalvarPlataformaAssessorSpider(pParametro);

                gLogger.InfoFormat("SalvarPlataformaContaMasterSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaOperadorResponse SalvarPlataformaOperadorSpider(RiscoSalvarPlataformaOperadorRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaOperadorResponse();

            try
            {
                lRetorno = gPlataformaDbLib.SalvarPlataformaOperadorSpider(pParametro);

                gLogger.InfoFormat("SalvarPlataformaOperadorSpider-> [{0}] ", lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion
    }
}
