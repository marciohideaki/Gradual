using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.AnaliseGrafica.Lib;
using Gradual.OMS.AnaliseGrafica.Lib.Dados;
using log4net;
using Gradual.OMS.Library;

namespace Gradual.OMS.ServicoAnaliseGrafica
{
    public class ServicoAnaliseGrafica  : IServicoAnaliseGrafica
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AnaliseGraficaConfig _config = null;

        public ServicoAnaliseGrafica()
        {
            _config = GerenciadorConfig.ReceberConfig<AnaliseGraficaConfig>();
        }
        
        #region IServicoAnaliseGrafica Members
        public ObterPontosIntradayResponse ObterPontosGraficoIntraday(ObterPontosIntradayRequest request)
        {
            logger.Debug("Solicitando DadosIntraday ativo[" + request.Ativo + "] intervalo[" + request.Intervalo + "]");
            ObterPontosIntradayResponse ret = new ObterPontosIntradayResponse();

            ANGPersistenciaDB db = new ANGPersistenciaDB();

            ret.cotacoes = db.ObterPontosIntraday(request.Ativo, request.Intervalo);

            logger.Debug("Respondendo DadosIntraday ativo[" + request.Ativo + "] intervalo[" + request.Intervalo + "]");
            return ret;
        }

        public ObterPontosIntradayResponse ObterPontosGraficoIntradayIncremental(ObterPontosIntradayRequest request)
        {
            logger.Debug("Solicitando Incremental ativo[" + request.Ativo + "] DataInicial[" + request.DataCotacaoInicial + "]");
            ObterPontosIntradayResponse ret = new ObterPontosIntradayResponse();

            ANGPersistenciaDB db = new ANGPersistenciaDB();

            ret.cotacoes = db.ObterPontosIntradayIncremental(request.Ativo, request.DataCotacaoInicial);

            logger.Debug("Respondendo Incremental ativo[" + request.Ativo + "] DataInicial[" + request.DataCotacaoInicial + "]");
            return ret;
        }

        public ObterPontosHistoricoResponse ObterPontosGraficoHistorico(ObterPontosHistoricoRequest request)
        {
            logger.Debug("Solicitando Historico ativo[" + request.Ativo + "] DataInicial[" + request.DataCotacaoInicial + "] DataFinal[" + request.DataCotacaoFinal + "]");
            ObterPontosHistoricoResponse ret = new ObterPontosHistoricoResponse();

            ANGPersistenciaDB db = new ANGPersistenciaDB();

            ret.pontos = db.ObterPontosSerieHistorica(request.Ativo, request.DataCotacaoInicial, request.DataCotacaoFinal);

            logger.Debug("Respondendo Historico ativo[" + request.Ativo + "] DataInicial[" + request.DataCotacaoInicial + "] DataFinal[" + request.DataCotacaoFinal + "]");
            return ret;
        }

        #endregion  //IServicoAnaliseGrafica Members
    }
}
