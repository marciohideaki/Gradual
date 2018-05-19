using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.Spider.LimiteRestricao.Lib;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;
using System.ServiceModel;
using log4net;
using Gradual.Spider.LimiteRestricao.DbLib;

namespace Gradual.Spider.LimiteRestricao
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSpiderLimiteRestricao : IServicoSpiderLimiteRestricao, IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private RiscoGrupoDbLib gDbGrupo     = new RiscoGrupoDbLib();
        private RiscoLimiteDbLib gDbLimite   = new RiscoLimiteDbLib();
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
            ServicoStatus lRetorno = ServicoStatus.Parado;
            
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

        #region Construtores
        public ServicoSpiderLimiteRestricao()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        public RiscoListarParametrosClienteResponse ListarLimitePorClienteSpider(RiscoListarParametrosClienteRequest pParametro)
        {
            var lRetorno = new RiscoListarParametrosClienteResponse();

            try
            {
                lRetorno = gDbLimite.ListarLimitePorClienteSpider(pParametro);

                gLogger.InfoFormat("ListarLimitePorClienteSpider-> [{0}] " , lRetorno);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}",ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarPermissoesResponse ListarPermissoesRiscoSpider(RiscoListarPermissoesRequest pParametro)
        {
            var lRetorno = new RiscoListarPermissoesResponse();

            try
            {
                lRetorno = gDbLimite.ListarPermissoesRiscoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarPermissoesClienteResponse ListarPermissoesRiscoClienteSpider(RiscoListarPermissoesClienteRequest pParametro)
        {
            var lRetorno = new RiscoListarPermissoesClienteResponse();
            
            try
            {
                lRetorno = gDbLimite.ListarPermissoesRiscoClienteSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;

        }

        public RiscoListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteSpider(RiscoListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoListarBloqueiroInstrumentoResponse();

            try
            {
                lRetorno = gDbLimite.ListarBloqueioPorClienteSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarClienteParametroGrupoResponse ListarClienteParametroGrupoSpider(RiscoListarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new RiscoListarClienteParametroGrupoResponse();

            try
            {
                lRetorno = gDbLimite.ListarClienteParametroGrupoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoReceberParametroClienteResponse RiscoReceberParametroCliente(RiscoReceberParametroClienteRequest pParametro, bool pEfetuarLog = false)
        {

            var lRetorno = new RiscoReceberParametroClienteResponse();

            try
            {
                lRetorno = gDbLimite.RiscoReceberParametroCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarParametroClienteResponse SalvarParametroRiscoClienteSpider(RiscoSalvarParametroClienteRequest pParametro)
        {
            var lRetorno = new RiscoSalvarParametroClienteResponse();

            try
            {
                lRetorno = gDbLimite. SalvarParametroRiscoClienteSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarParametroClienteResponse SalvarPermissoesRiscoAssociadasSpider(RiscoSalvarPermissoesAssociadasRequest pParametro)
        {
            var lRetorno = new RiscoSalvarParametroClienteResponse();

            try
            {
                lRetorno = gDbLimite.SalvarPermissoesRiscoAssociadasSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarParametroClienteResponse SalvarExpirarLimiteSpider(RiscoSalvarParametroClienteRequest pParametro)
        {
            var lRetorno = new RiscoSalvarParametroClienteResponse();

            try
            {
                lRetorno = gDbLimite.SalvarExpirarLimiteSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarGrupoItemResponse ListarGrupoItensSpider(RiscoListarGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoListarGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo .ListarGrupoItensSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoRemoverGrupoResponse RemoverGrupoRiscoSpider(RiscoRemoverGrupoRequest pRequest)
        {
            var lRetorno = new RiscoRemoverGrupoResponse();

            try
            {
                lRetorno = gDbGrupo.RemoverGrupoRiscoSpider(pRequest);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarGrupoItemResponse SalvarGrupoItemSpider(RiscoSalvarGrupoItemRequest lRequest)
        {
            var lRetorno = new RiscoSalvarGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.SalvarGrupoItemSpider(lRequest);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarGruposResponse ListarGruposSpider(RiscoListarGruposRequest lRequest)
        {
            var lRetorno = new RiscoListarGruposResponse();

            try
            {
                lRetorno = gDbGrupo.ListarGruposSpider(lRequest);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno; 
        }

        public RiscoRemoverRegraGrupoItemResponse RemoverRegraGrupoItemSpider(RiscoRemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoRemoverRegraGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.RemoverRegraGrupoItemSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoRemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecaoSpider(RiscoRemoverClienteBloqueioRequest pParametro)
        {
            var lRetorno = new RiscoRemoverBloqueioInstumentoResponse();

            try
            {
                lRetorno = gDbGrupo.RemoverBloqueioClienteInstrumentoDirecaoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoRemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobalSpider(RiscoRemoverRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoRemoverRegraGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.RemoverRegraGrupoItemGlobalSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarRegraGrupoItemResponse ListarRegraGrupoItemGlobalSpider(RiscoListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoListarRegraGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.ListarRegraGrupoItemGlobalSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecaoSpider(RiscoListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoListarBloqueiroInstrumentoResponse();

            try
            {
                lRetorno = gDbGrupo.ListarBloqueioClienteInstrumentoDirecaoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarRegraGrupoItemResponse ListarRegraGrupoItemSpider(RiscoListarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoListarRegraGrupoItemResponse();
            try
            {
                lRetorno = gDbGrupo.ListarRegraGrupoItemSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecaoSpider(RiscoSalvarBloqueioInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarBloqueioInstrumentoResponse();

            try
            {
                lRetorno = gDbGrupo.SalvarClienteBloqueioInstrumentoDirecaoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobalSpider(RiscoSalvarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRegraGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.SalvarRegraGrupoItemGlobalSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarRegraGrupoItemResponse SalvarRegraGrupoItemSpider(RiscoSalvarRegraGrupoItemRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRegraGrupoItemResponse();

            try
            {
                lRetorno = gDbGrupo.SalvarRegraGrupoItemSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarTravaExposicaoResponse SalvarTravaExposicaoSpider(RiscoSalvarTravaExposicaoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarTravaExposicaoResponse();

            try
            {
                lRetorno = gDbGrupo.SalvarTravaExposicaoSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }


        public RiscoListarLimiteAlocadoResponse ConsultarRiscoLimitePorClienteSpider(RiscoListarLimiteAlocadoRequest pParametro)
        {
            var lRetorno = new RiscoListarLimiteAlocadoResponse();

            try
            {
                lRetorno = new RiscoLimiteDbLib().ConsultarRiscoLimitePorClienteSpider(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }


        public RiscoListarLimiteBMFResponse ObterSpiderLimiteBMFCliente(RiscoListarLimiteBMFRequest pParametro)
        {
            var lRetorno = new RiscoListarLimiteBMFResponse();

            try
            {
                lRetorno = new RiscoLimiteBmfDbLib().ObterSpiderLimiteBMFCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoInserirLimiteClienteBMFResponse AtualizarSpiderLimiteBMF(RiscoInserirLimiteClienteBMFRequest pParametro)
        {
            var lRetorno = new RiscoInserirLimiteClienteBMFResponse();

            try
            {
                lRetorno = new RiscoLimiteBmfDbLib().AtualizarSpiderLimiteBMF(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoInserirLimiteBMFInstrumentoResponse AtualizarSpiderLimiteInstrumentoBMF(RiscoInserirLimiteBMFInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoInserirLimiteBMFInstrumentoResponse();

            try
            {
                lRetorno = new RiscoLimiteBmfDbLib().AtualizarSpiderLimiteInstrumentoBMF(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoRemoveLimiteBMFInstrumentoResponse RemoverSpiderLimiteInstrumentoBMF(RiscoRemoveLimiteBMFInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoRemoveLimiteBMFInstrumentoResponse();

            try
            {
                lRetorno = new RiscoLimiteBmfDbLib().RemoverSpiderLimiteInstrumentoBMF(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarRestricaoGlobalResponse ListarRestricaoGlobalCliente(RiscoListarRestricaoGlobalRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoGlobalResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().ListarRestricaoGlobalCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarRestricaoGrupoResponse ListarRestricaoGrupoCliente(RiscoListarRestricaoGrupoRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoGrupoResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().ListarRestricaoGrupoCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoListarRestricaoAtivoResponse ListarRestricaoAtivoCliente(RiscoListarRestricaoAtivoRequest pParametro)
        {
            var lRetorno = new RiscoListarRestricaoAtivoResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().ListarRestricaoAtivoCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoGlobalResponse SalvarRestricaoGlobalCliente(RiscoSalvarRestricaoGlobalRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoGlobalResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().SalvarRestricaoGlobalCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoGrupoResponse SalvarRestricaoGrupoCliente(RiscoSalvarRestricaoGrupoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoGrupoResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().SalvarRestricaoGrupoCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }

        public RiscoSalvarRestricaoAtivoResponse SalvarRestricaoAtivoCliente(RiscoSalvarRestricaoAtivoRequest pParametro)
        {
            var lRetorno = new RiscoSalvarRestricaoAtivoResponse();

            try
            {
                lRetorno = new RiscoGrupoDbLib().SalvarRestricaoAtivoCliente(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }


        public RiscoRemoveLimiteBMFResponse RemoverSpiderLimiteBMF(RiscoRemoveLimiteBMFRequest pParametro)
        {
            var lRetorno = new RiscoRemoveLimiteBMFResponse();

            try
            {
                lRetorno = new RiscoLimiteBmfDbLib().RemoverSpiderLimiteBMF(pParametro);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro - {0} - {1}", ex, ex.StackTrace);
            }

            return lRetorno;
        }
    }
}
