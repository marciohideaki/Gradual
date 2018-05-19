using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading;
using Gradual.OMS.ConsolidadorRelatorioCCLib;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Enum;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.OMS.ConsolidadorRelatorioCC
{
    public class ServicoConsolidadorContaCorrente : IServicoConsolidadorRelatorioCC, IServicoControlavel
    {
        #region | Atributos

        private static int _temporizadorServico;

        private ServicoStatus _status;

        private static Thread _thread;

        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private static Timer _timer;

        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades

        private static int GetTemporizador
        {
            get 
            {
                if (0.Equals(_temporizadorServico))
                    _temporizadorServico = ConfigurationManager.AppSettings["TemporizadorServicoEmMinutos"].DBToInt32();

                return _temporizadorServico * 60000;
            }
        }

        private TimeSpan GetHorarioInicio
        {
            get 
            {
                var lRetorno = default(TimeSpan);

                TimeSpan.TryParse(ConfigurationManager.AppSettings["HorarioServicoInicio"], out lRetorno);

                return lRetorno;
            }
        }

        private TimeSpan GetHorarioFim
        {
            get
            {
                var lRetorno = default(TimeSpan);

                TimeSpan.TryParse(ConfigurationSettings.AppSettings["HorarioServicoFim"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Métotodos

        [MethodImpl(MethodImplOptions.PreserveSig)]        
        public void AlimentarConsultaDNDelegate(object pParametro) 
        {
            _logger.Debug(string.Concat("Iniciando  carga de dados - ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            var lRetorno = new SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>();

            var isHorarioValido = (!DateTime.Today.DayOfWeek.Equals(DayOfWeek.Saturday)
            && (!DateTime.Today.DayOfWeek.Equals(DayOfWeek.Sunday))
            && (DateTime.Now.TimeOfDay.CompareTo(this.GetHorarioInicio).Equals(1))
            && (DateTime.Now.TimeOfDay.CompareTo(this.GetHorarioFim).Equals(-1)));

            if (isHorarioValido)
            {
                lRetorno = AlimentarConsultaDN(new SaldoContaCorrenteRiscoRequest());

                if (CriticaMensagemEnum.OK.Equals(lRetorno.StatusResposta))
                    _logger.Info(lRetorno.DescricaoResposta);
                else
                    _logger.Error(lRetorno.DescricaoResposta);
            }
            _logger.Debug(string.Concat("Finalizada carga de dados - ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
        }

        private void CarregarServico()
        {
            TimerCallback callBack = AlimentarConsultaDNDelegate;

            if (null == _timer)
                _timer = new Timer(callBack, _autoResetEvent, 15000, GetTemporizador);

            _status = ServicoStatus.EmExecucao;

            _logger.Debug(string.Concat("Timer iniciado: ", _timer.ToString()));
        }

        private SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> AlimentarConsultaDN(SaldoContaCorrenteRiscoRequest pParametro)
        {
            return new PersistenciaContaCorrente().AlimentarConsultaDN(pParametro);
        }

        public SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> ConsultarSaldoCCProjetado(SaldoContaCorrenteRiscoRequest pParametro)
        {
            return new PersistenciaContaCorrente().ConsultarSaldoCCProjetado(pParametro);
        }

        #endregion

        #region | IServicoControlavel Members

        public void IniciarServico()
        {
            _thread = new Thread(new ThreadStart(CarregarServico));

            _thread.Name = "Thread servico ConsolidadorRelatorioCC";

            _thread.Start();

            _logger.Debug("Serviço iniciado com sucesso.");
        }

        public void PararServico()
        {
            _timer.Dispose();

            if (_thread.IsAlive)
                _thread.Abort();

            _status = ServicoStatus.Parado;

            _logger.Debug("Serviço forçado a parar pelo cliente.");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        public DateTime ConsultarDataHoraUltimaAtualizacao()
        {
            return new PersistenciaContaCorrente().ConsultarDataHoraUltimaAtualizacao();
        }

        #endregion
    }
}
