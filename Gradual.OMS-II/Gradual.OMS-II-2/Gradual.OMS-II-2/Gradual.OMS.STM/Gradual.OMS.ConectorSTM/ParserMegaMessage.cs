using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ConectorSTM.Eventos;
using log4net;
using Gradual.OMS.ConectorSTM.Lib.Mensagens;
using System.Globalization;

namespace Gradual.OMS.ConectorSTM
{
    public class ParserMegaMessage
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string TIPO_MSG_0100 = "0100";
        public const string TIPO_MSG_0103 = "0103";
        public const string TIPO_MSG_0105 = "0105";
        public const string TIPO_MSG_0138 = "0138";
        public const string TIPO_MSG_0172 = "0172";
        public const string TIPO_MSG_0411 = "0411";
        public const string TIPO_MSG_0412 = "0412";
        public const string TIPO_MSG_0413 = "0413";
        public const string TIPO_MSG_0414 = "0414";
        public const string TIPO_MSG_0415 = "0415";
        public const string TIPO_MSG_0417 = "0417";

        public void Parse(EventoSTM evento)
        {
            logger.Debug("Parse(): " + evento.Cabecalho);

            switch (evento.Cabecalho)
            {
                case TIPO_MSG_0172:
                    _parse172(evento);
                    break;
                case TIPO_MSG_0105:
                    _parse105(evento);
                    break;
                default:
                    logger.Warn("Mensagem desconhecida: [" + evento.Cabecalho + "]");
                    break;
            }
        }


        /// <summary>
        ///  Notificacao de cancelamento de negocio
        /// </summary>
        /// <param name="evento"></param>
        private void _parse100(EventoSTM evento)
        {
            int offset = 0;
            MEGA0100NotificacaoCancelamentoNegocioInfo info100 = new MEGA0100NotificacaoCancelamentoNegocioInfo();

            info100.FunctionCode = evento.Cabecalho;

            info100.InternalReference = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD001_TAM_INTERNAL_REFERENCE);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD001_TAM_INTERNAL_REFERENCE;
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD002_TAM_FUNCTION_CODE;

            string dthr = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD003_TAM_DSEABSEVEN);
            info100.DataNegocio = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD003_TAM_DSEABSEVEN;

            info100.Papel = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD004_TAM_CVALISIN).Trim();
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD004_TAM_CVALISIN;

            dthr = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD005_TAM_DSAIOM);
            info100.DataRegistro = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD005_TAM_DSAIOM;

            info100.SequencialOrdem = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD006_TAM_NSEQOM);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD006_TAM_NSEQOM;

            info100.IndicadorFormatoPreco = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD007_TAM_IND_PRICE);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD007_TAM_IND_PRICE;

            string preco = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD009_TAM_PRICE);
            preco = STMUtilities.saidaFormatada(info100.IndicadorFormatoPreco[0], preco, false, true, false, preco.Length);
            info100.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD008_TAM_PRICE;

            info100.QuantidadeExecutadaNegocio = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD009_TAM_QTITTRAN));
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD009_TAM_QTITTRAN;

            info100.HoraExecucaoNegocio = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD010_TAM_HTRAN);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD010_TAM_HTRAN;
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD011_TAM_NMSGREPON;
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD012_TAM_HMSGREPON;

            info100.CorretoraContraparte = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD013_TAM_CLDADFCIE);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD013_TAM_CLDADFCIE;

            info100.NumeroNegocio = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD014_TAM_NTRAN);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD014_TAM_NTRAN;

            info100.Side = evento.Corpo.Substring(offset, MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD015_TAM_ISENOM);
            offset += MEGA0100NotificacaoCancelamentoNegocioInfo.FIELD015_TAM_ISENOM;

            EventoMega newevent = new EventoMega(TIPO_MSG_0100, info100, evento.MsgID);

            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);

        }

        /// <summary>
        ///  Notificacao de cancelamento de negocio
        /// </summary>
        /// <param name="evento"></param>
        private void _parse103(EventoSTM evento)
        {
            int offset = 0;
            MEGA0103CriacaoNegocioInfo info103 = new MEGA0103CriacaoNegocioInfo();

            info103.FunctionCode = evento.Cabecalho;

            info103.InternalReference = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD001_TAM_INTERNAL_REFERENCE);
            offset += MEGA0103CriacaoNegocioInfo.FIELD001_TAM_INTERNAL_REFERENCE;
            offset += MEGA0103CriacaoNegocioInfo.FIELD002_TAM_FUNCTION_CODE;

            string dthr = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD003_TAM_DSEABS);
            info103.DataNegocio = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0103CriacaoNegocioInfo.FIELD003_TAM_DSEABS;

            info103.Papel = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD004_TAM_CVALISIN).Trim();
            offset += MEGA0103CriacaoNegocioInfo.FIELD004_TAM_CVALISIN;

            info103.IndicadorFormato = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD005_TAM_INDICADOR_FORMATO);
            offset += MEGA0103CriacaoNegocioInfo.FIELD005_TAM_INDICADOR_FORMATO;

            string preco = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD006_TAM_PRECO);
            preco = STMUtilities.saidaFormatada(info103.IndicadorFormato[0], preco, false, true, false, preco.Length);
            info103.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
            offset += MEGA0103CriacaoNegocioInfo.FIELD006_TAM_PRECO;

            info103.QuantidadeNegocio = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD007_TAM_QTITTRAN);
            offset += MEGA0103CriacaoNegocioInfo.FIELD007_TAM_QTITTRAN;

            dthr = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD008_TAM_DHTRANBS);
            info103.DataHoraNegocio = DateTime.ParseExact(dthr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            offset += MEGA0103CriacaoNegocioInfo.FIELD008_TAM_DHTRANBS;

            info103.Side = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD009_TAM_ISENSOM);
            offset += MEGA0103CriacaoNegocioInfo.FIELD009_TAM_ISENSOM;

            info103.CorretoraContraparte = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD010_TAM_CLDADFCIE);
            offset += MEGA0103CriacaoNegocioInfo.FIELD010_TAM_CLDADFCIE;

            info103.CodigoOrigemOferta = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD011_TAM_YOM);
            offset += MEGA0103CriacaoNegocioInfo.FIELD011_TAM_YOM;
            offset += MEGA0103CriacaoNegocioInfo.FIELD012_TAM_NMSGREPON;
            offset += MEGA0103CriacaoNegocioInfo.FIELD013_TAM_HMSGREPON;

            info103.NumeroNegocio = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD014_TAM_NTRAN);
            offset += MEGA0103CriacaoNegocioInfo.FIELD014_TAM_NTRAN;

            info103.TipoOrdemOrigem = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD015_TAM_IORGINFMAR);
            offset += MEGA0103CriacaoNegocioInfo.FIELD015_TAM_IORGINFMAR;

            info103.CodigoOperadorMega = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD016_TAM_CLDNGSAIOM);
            offset += MEGA0103CriacaoNegocioInfo.FIELD016_TAM_CLDNGSAIOM;
            offset += MEGA0103CriacaoNegocioInfo.FIELD017_TAM_YCPTEMBROM;

            info103.CodigoClienteMegabolsa = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD018_TAM_NCPTEMBROM);
            info103.CodigoCliente = STMUtilities.RetornarNumeros(info103.CodigoClienteMegabolsa);
            offset += MEGA0103CriacaoNegocioInfo.FIELD018_TAM_NCPTEMBROM;
            offset += MEGA0103CriacaoNegocioInfo.FIELD019_TAM_CLDOMNG;

            dthr = evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD020_TAM_DHSAIOMADF);
            info103.DataHoraRegistro = DateTime.ParseExact(dthr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            offset += MEGA0103CriacaoNegocioInfo.FIELD020_TAM_DHSAIOMADF;

            info103.QuantidadePatas = Convert.ToInt32(evento.Corpo.Substring(offset, MEGA0103CriacaoNegocioInfo.FIELD021_TAM_ZPRODCPSTRAN));
            offset += MEGA0103CriacaoNegocioInfo.FIELD021_TAM_ZPRODCPSTRAN;

            for (int i = 0; i < info103.QuantidadePatas; i++)
            {
                PataEstrategiaNegocioInfo pata = new PataEstrategiaNegocioInfo();

                pata.CodigoIdentificacao = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD001_TAM_CISINPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD001_TAM_CISINPRODCPS;

                pata.SinalCompraVenda = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD002_TAM_CSIGNKMUPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD002_TAM_CSIGNKMUPRODCPS;

                pata.ProporcaoPata = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD003_TAM_KRAOCPSSTG);
                offset += PataEstrategiaNegocioInfo.FIELD003_TAM_KRAOCPSSTG;

                pata.FormatIndicator = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD004_TAM_FORMAT_INDICATOR);
                offset += PataEstrategiaNegocioInfo.FIELD004_TAM_FORMAT_INDICATOR;

                preco = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD005_TAM_DATA);
                preco = STMUtilities.saidaFormatada(pata.FormatIndicator[0], preco, false, true, false, preco.Length);
                pata.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
                offset += PataEstrategiaNegocioInfo.FIELD005_TAM_DATA;

                pata.NumeroNegocio = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD006_TAM_NTRANSCPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD006_TAM_NTRANSCPRODCPS;

                info103.Patas.Add(pata);

            }

            EventoMega newevent = new EventoMega(TIPO_MSG_0103, info103, evento.MsgID);

            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
        }


        /// <summary>
        /// Notificacao de execucao de Negocio
        /// </summary>
        /// <param name="evento"></param>
        private void _parse105(EventoSTM evento)
        {
            int offset = 0;
            MEGA0105NotificacaoExecucaoInfo info105 = new MEGA0105NotificacaoExecucaoInfo();

            info105.FunctionCode = evento.Cabecalho;

            info105.InternalReference = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD001_TAM_INTERNAL_REFERENCE);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD001_TAM_INTERNAL_REFERENCE;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD002_TAM_FUNCTION_CODE;

            string dthr = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD003_TAM_DSAIOM);
            info105.DataRegistro = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD003_TAM_DSAIOM;

            info105.NumeroSequencialOrdem = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD004_TAM_NSEQOM);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD004_TAM_NSEQOM;

            info105.Papel = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD005_TAM_CVALISIM).Trim();
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD005_TAM_CVALISIM;

            info105.GrupoPapel = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD006_TAM_CGRVALCOT);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD006_TAM_CGRVALCOT;

            info105.Side = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD007_TAM_ISENSOM);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD007_TAM_ISENSOM;

            info105.QuantidadeNegocio = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD008_TAM_QTITTRAN));
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD008_TAM_QTITTRAN;

            info105.IndicadorFormato = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD009_TAM_FORMAT_INDICATOR);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD009_TAM_FORMAT_INDICATOR;

            string preco = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD010_TAM_PRICE);
            preco = STMUtilities.saidaFormatada(info105.IndicadorFormato[0], preco, false, true, false, preco.Length);
            info105.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD010_TAM_PRICE;

            info105.IndicadorQtdeRemanescente = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD011_TAM_IPRSQTITREST);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD011_TAM_IPRSQTITREST;

            info105.QuantidadeRemanescente = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD012_TAM_QTITRESTOM));
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD012_TAM_QTITRESTOM;

            info105.CorretoraContraparte = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD013_TAM_CLDADFCIE);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD013_TAM_CLDADFCIE;

            info105.CodigoOrigemOferta = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD014_TAM_YOM);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD014_TAM_YOM;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD015_TAM_NMSGREPON;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD016_TAM_DMSGREPON;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD017_TAM_HMSGREPON;

            info105.NumeroNegocio = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD018_TAM_NTRAN);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD018_TAM_NTRAN;

            info105.DataNegocio = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD019_TAM_DTRAN);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD019_TAM_DTRAN;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD020_TAM_YPLIMSAIOM;

            info105.TipoValidadeOrdem = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD021_TAM_YVAIOMNSC);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD021_TAM_YVAIOMNSC;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD022_TAM_CGDSVAL;

            info105.CodigoOperadorMega = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD023_TAM_CLDNGSAIOM);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD023_TAM_CLDNGSAIOM;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD024_TAM_YCPTEMBROM;

            info105.CodigoClienteMegabolsa = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD025_TAM_NCPTEPOSIPTOM).Trim();
            info105.CodigoCliente = STMUtilities.RetornarNumeros(info105.CodigoClienteMegabolsa);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD025_TAM_NCPTEPOSIPTOM;
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD026_TAM_CLDNGSAIOM;

            dthr = evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD027_TAM_DHSAIOMADF);
            info105.DataHoraEntradaOrdem = DateTime.ParseExact(dthr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD027_TAM_DHSAIOMADF;

            info105.QuantidadePatas = Convert.ToInt32(evento.Corpo.Substring(offset, MEGA0105NotificacaoExecucaoInfo.FIELD028_TAM_ZPRODCPSTRAN));
            offset += MEGA0105NotificacaoExecucaoInfo.FIELD028_TAM_ZPRODCPSTRAN;

            for (int i = 0; i < info105.QuantidadePatas; i++)
            {
                PataEstrategiaNegocioInfo pata = new PataEstrategiaNegocioInfo();

                pata.CodigoIdentificacao = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD001_TAM_CISINPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD001_TAM_CISINPRODCPS;

                pata.SinalCompraVenda = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD002_TAM_CSIGNKMUPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD002_TAM_CSIGNKMUPRODCPS;

                pata.ProporcaoPata = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD003_TAM_KRAOCPSSTG);
                offset += PataEstrategiaNegocioInfo.FIELD003_TAM_KRAOCPSSTG;

                pata.FormatIndicator = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD004_TAM_FORMAT_INDICATOR);
                offset += PataEstrategiaNegocioInfo.FIELD004_TAM_FORMAT_INDICATOR;

                preco = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD005_TAM_DATA);
                preco = STMUtilities.saidaFormatada(pata.FormatIndicator[0], preco, false, true, false, preco.Length);
                pata.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
                offset += PataEstrategiaNegocioInfo.FIELD005_TAM_DATA;

                pata.NumeroNegocio = evento.Corpo.Substring(offset, PataEstrategiaNegocioInfo.FIELD006_TAM_NTRANSCPRODCPS);
                offset += PataEstrategiaNegocioInfo.FIELD006_TAM_NTRANSCPRODCPS;

                info105.Patas.Add(pata);
            }

            EventoMega newevent = new EventoMega(TIPO_MSG_0105, info105, evento.MsgID);

            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
        }


        /// <summary>
        ///  Notificacao de cancelamento de negocio
        /// </summary>
        /// <param name="evento"></param>
        private void _parse138(EventoSTM evento)
        {
            int offset = 0;
            MEGA0138OrdemEliminadaInfo info138 = new MEGA0138OrdemEliminadaInfo();

            info138.FunctionCode = evento.Cabecalho;

            info138.InternalReference = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD001_TAM_INTERNAL_REFERENCE);
            offset += MEGA0138OrdemEliminadaInfo.FIELD001_TAM_INTERNAL_REFERENCE;
            offset += MEGA0138OrdemEliminadaInfo.FIELD002_TAM_FUNCTION_CODE;

            string dthr = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD003_TAM_DSAIOM);
            info138.DataEntradaOrdem = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0138OrdemEliminadaInfo.FIELD003_TAM_DSAIOM;

            info138.NumeroSequencialOrdem = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD004_TAM_NSEQOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD004_TAM_NSEQOM;

            info138.StatusOrdem = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD005_TAM_IMAJOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD005_TAM_IMAJOM;

            info138.Papel = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD006_TAM_CVALISIN).Trim();
            offset += MEGA0138OrdemEliminadaInfo.FIELD006_TAM_CVALISIN;

            info138.QuantidadeRemanescente = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD007_TAM_QTITRESTOM));
            offset += MEGA0138OrdemEliminadaInfo.FIELD007_TAM_QTITRESTOM;

            info138.IndicadorFormato = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD008_TAM_FORMAT_INDICATOR);
            offset += MEGA0138OrdemEliminadaInfo.FIELD008_TAM_FORMAT_INDICATOR;

            string preco = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD009_TAM_PRICE);
            preco = STMUtilities.saidaFormatada(info138.IndicadorFormato[0], preco, false, true, false, preco.Length);
            info138.Preco = Convert.ToDecimal(preco, STMUtilities.ciPtBR);
            offset += MEGA0138OrdemEliminadaInfo.FIELD009_TAM_PRICE;

            info138.CorretoraEmitente = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD010_TAM_CLDADFEMETOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD010_TAM_CLDADFEMETOM;

            info138.TipoValidade = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD011_TAM_YVALIOMNSC);
            offset += MEGA0138OrdemEliminadaInfo.FIELD011_TAM_YVALIOMNSC;

            info138.FlagCliente = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD012_TAM_YCPTEOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD012_TAM_YCPTEOM;

            dthr = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD013_TAM_DVALIOM);
            info138.DataValidadeOrdem = DateTime.ParseExact(dthr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            offset += MEGA0138OrdemEliminadaInfo.FIELD013_TAM_DVALIOM;

            info138.Side = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD014_TAM_ISENSOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD014_TAM_ISENSOM;

            info138.TipoOrdem = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD015_TAM_YPLIMSAIOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD015_TAM_YPLIMSAIOM;
            offset += MEGA0138OrdemEliminadaInfo.FIELD016_TAM_NMSGREPON;
            offset += MEGA0138OrdemEliminadaInfo.FIELD017_TAM_HMSGREPON;

            info138.CodigoClienteMegabolsa = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD018_TAM_NCPTEPOSIPTOM).Trim();
            info138.CodigoCliente = STMUtilities.RetornarNumeros(info138.CodigoClienteMegabolsa);
            offset += MEGA0138OrdemEliminadaInfo.FIELD018_TAM_NCPTEPOSIPTOM;

            info138.CodigoOperadorMega = evento.Corpo.Substring(offset, MEGA0138OrdemEliminadaInfo.FIELD019_TAM_CLDNGSAIOM);
            offset += MEGA0138OrdemEliminadaInfo.FIELD019_TAM_CLDNGSAIOM;

            EventoMega newevent = new EventoMega(TIPO_MSG_0138, info138, evento.MsgID);

            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
        }

        /// <summary>
        /// Confirmacao de ordem
        /// </summary>
        /// <param name="evento"></param>
        private void _parse172(EventoSTM evento)
        {
            int offset =0;
            MEGA0172ConfirmacaoOrdemInfo info172 = new MEGA0172ConfirmacaoOrdemInfo();

            info172.FunctionCode = evento.Cabecalho;
            
            info172.InternalReference = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD001_TAM_INTERNAL_REFERENCE);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD001_TAM_INTERNAL_REFERENCE;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD002_TAM_FUNCTION_CODE;

            string dthr = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD003_TAM_DSAIOM);
            info172.DataEntradaOrdem = DateTime.ParseExact(dthr, "yyyyMMdd", CultureInfo.InvariantCulture);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD003_TAM_DSAIOM;

            info172.NumeroSequencialOrdem = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD004_TAM_NSEQOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD004_TAM_NSEQOM;

            info172.StatusOrdem = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD005_TAM_IMAJOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD005_TAM_IMAJOM;

            info172.Papel = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD006_TAM_CVALISIN).Trim();
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD006_TAM_CVALISIN;

            info172.QuantidadeOrdem = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD007_TAM_QTITTOTOM));
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD007_TAM_QTITTOTOM;

            info172.Side = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD008_TAM_ISENSOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD008_TAM_ISENSOM;

            info172.IndicadorFormato = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD009_TAM_FORMAT_INDICATOR);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD009_TAM_FORMAT_INDICATOR;

            string preco = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD010_TAM_PRICE);
            preco = STMUtilities.saidaFormatada(info172.IndicadorFormato[0], preco, false, true, false, preco.Length);
            info172.Preco = Convert.ToDecimal(preco,STMUtilities.ciPtBR);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD010_TAM_PRICE;

            info172.CorretoraEmitente = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD011_TAM_CLDAFTEMETOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD011_TAM_CLDAFTEMETOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD012_TAM_NMSGREPON;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD013_TAM_HMSGREPON;

            info172.TipoOrdem = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD014_TAM_YPLIMSAIOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD014_TAM_YPLIMSAIOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD015_TAM_QTITXTEINTROM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD016_TAM_CFONORG;

            info172.DataModificacao = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD017_TAM_DOMINI);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD017_TAM_DOMINI;

            info172.NumeroSequencialOriginal = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD018_TAM_NSEQOMINI);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD018_TAM_NSEQOMINI;

            info172.TipoValidadeOrdem = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD019_TAM_YVALIOMNSC);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD019_TAM_YVALIOMNSC;

            info172.DataValidadeOrdem = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD020_TAM_DVALIOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD020_TAM_DVALIOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD021_TAM_QTITMINOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD022_TAM_QTITDVLOM;

            info172.CodigoOrigemOferta = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD023_TAM_YOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD023_TAM_YOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD024_TAM_ICFMOM;

            info172.QuantidadeRemanescente = Convert.ToInt64(evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD025_TAM_QTITRESTOMINI));
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD025_TAM_QTITRESTOMINI;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD026_TAM_FORMAT_INDICATOR2;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD027_TAM_PRICE2;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD028_TAM_TVALIOM;

            info172.CodigoOperadorMega = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD029_TAM_CLDNGSAIOM);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD029_TAM_CLDNGSAIOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD030_TAM_YCPTEMBROM;

            info172.CodigoClienteMegabolsa = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD031_TAM_NCPTEPOSIPTOM).Trim();
            info172.CodigoCliente = STMUtilities.RetornarNumeros(info172.CodigoClienteMegabolsa);
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD031_TAM_NCPTEPOSIPTOM;
            offset += MEGA0172ConfirmacaoOrdemInfo.FIELD032_TAM_CLDOMNG;

            dthr = evento.Corpo.Substring(offset, MEGA0172ConfirmacaoOrdemInfo.FIELD033_TAM_DHSAIOMADF);
            info172.DataHoraCriacaoOrdem = DateTime.ParseExact(dthr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            EventoMega newevent = new EventoMega(TIPO_MSG_0172, info172, evento.MsgID);

            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
        }
    }
}
