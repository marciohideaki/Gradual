using System;
using System.ServiceModel;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using Gradual.OMS.RelatoriosFinanc.Posicao;
using log4net;

namespace Gradual.OMS.RelatoriosFinanc
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoRelatoriosFinanceiros : IServicoRelatoriosFinanceiros
    {
        #region | Atributos

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Contrutores

        public ServicoRelatoriosFinanceiros()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion

        #region | Métodos Interface
        public MonitorRiscoResponse ConsultarDadosMonitorRisco(MonitorRiscoRequest pParametros)
        {
            var lRetorno = new MonitorRiscoResponse();

            try
            {
                lRetorno = new MonitorRiscoDbLib().ConsultarDadosMonitorRisco(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Monitor risco; Cliente: ", pParametros.CodigoCliente == null ? pParametros.CodigoClienteBmf.DBToString() : pParametros.CodigoCliente.DBToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public PosicaoCustodiaResponse ConsultarCustodia(PosicaoCustodiaRequest pParametros)
        {
            var lRetorno = new PosicaoCustodiaResponse();

            try
            {
                lRetorno = new CustodiaDbLib().ConsultarCustodia(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Custódia; Cliente: ", pParametros.ConsultaCdClienteBovespa == null ? pParametros.ConsultaCdClienteBovespa.DBToString() : pParametros.ConsultaCdClienteBMF.DBToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public NotaDeCorretagemExtratoResponse ConsultarNotaDeCorretagem(NotaDeCorretagemExtratoRequest pParametros)
        {
            var lRetorno = new NotaDeCorretagemExtratoResponse();

            try
            {
                lRetorno = new NotaDeCorretagemDbLib().ConsultarNotaDeCorretagem(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Nota de Corretagem; Cliente: ", pParametros.ConsultaCodigoCliente.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public NotaDeCorretagemExtratoResponse ConsultarNotaDeCorretagemBmf(NotaDeCorretagemExtratoRequest pParametros)
        {
            var lRetorno = new NotaDeCorretagemExtratoResponse();

            try
            {
                lRetorno = new NotaDeCorretagemDbLib().ConsultarNotaDeCorretagemBmf(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Nota de Corretagem de BMF ; Cliente: ", pParametros.ConsultaCodigoCliente.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public FaxResponse ObterFaxBovespa(FaxRequest pParametros)
        {
            var lRetorno = new FaxResponse();

            try
            {
                lRetorno = new FaxBovespaDbLib().ObterFaxBovespa(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Fax de Bovespa ; Cliente: ", pParametros.ConsultaCodigoCliente.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public FaxResponse ObterFaxBmf(FaxRequest pParametros)
        {
            var lRetorno = new FaxResponse();

            try
            {
                lRetorno = new FaxBmfDbLib().ObterFaxBmf(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Fax de BMF ; Cliente: ", pParametros.ConsultaCodigoClienteBmf.Value.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public FaxResponse ObterFaxBmfVolatilidade(FaxRequest pParametros)
        {
            var lRetorno = new FaxResponse();

            try
            {
                lRetorno = new FaxBmfDbLib().ObterFaxBmfVolatilidade(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Fax de BMF Volatilidade ; Cliente: ", pParametros.ConsultaCodigoClienteBmf.Value.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public ExtratoOperacaoResponse ObterExtratoOperacoesBovespa(ExtratoOperacaoRequest pParametros)
        {
            var lRetorno = new ExtratoOperacaoResponse();

            try
            {
                lRetorno = new ExtratoOperacaoDbLib().ObterExtratoOperacoesBovespa(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar Extrato Operação ; Cliente: ", pParametros.ConsultaCodigoClienteBmf.Value.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        #endregion

        #region IServicoRelatoriosFinanceiros Members


        public SaldoProjetadoCCResponse ConsultarSaldoProjecoesEmContaCorrente(SaldoProjetadoCCRequest pParametros)
        {
            var lRetorno = new SaldoProjetadoCCResponse();

            try
            {
                lRetorno = new SaldoProjetadoCCDbLib().ConsultarSaldoProjecoesEmContaCorrente(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar ConsultarSaldoProjecoesEmContaCorrente; Assessor/Cliente: ", pParametros.ConsultaCdAssesso.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public SaldoProjetadoCCResponse ConsultarSaldoProjecoesEmContaCorrenteCliente(SaldoProjetadoCCRequest pParametros)
        {
            var lRetorno = new SaldoProjetadoCCResponse();

            try
            {
                lRetorno = new SaldoProjetadoCCDbLib().ConsultarSaldoProjecoesEmContaCorrenteCliente(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar ConsultarSaldoProjecoesEmContaCorrenteCliente; Assessor/Cliente: ", pParametros.ConsultaCdAssesso.ToString()), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        #endregion

        #region IServicoRelatoriosFinanceiros Members


        public PosicaoCustodiaTesouroDiretoResponse ConsultarCustodiaComTesouro(PosicaoCustodiaTesouroDiretoRequest pParametros)
        {
            var lRetorno = new PosicaoCustodiaTesouroDiretoResponse();

            try
            {
                lRetorno = new SaldoProjetadoCCDbLib().ConsultarCustodiaComTesouro(pParametros);
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Concat("Erro ao consultar ConsultarCustodiaTesouroDireto; CodBov, CodBmf ", pParametros.ConsultaCdClienteBovespa, pParametros.ConsultaCdClienteBMF), ex);

                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = Library.MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        #endregion
    }
}
