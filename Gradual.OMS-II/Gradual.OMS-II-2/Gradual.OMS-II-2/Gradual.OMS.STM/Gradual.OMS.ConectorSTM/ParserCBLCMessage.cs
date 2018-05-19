using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ConectorSTM.Eventos;
using log4net;
using Gradual.OMS.ConectorSTM.Lib.Mensagens;
using Gradual.OMS.Library;
using System.Globalization;
using System.Collections;

namespace Gradual.OMS.ConectorSTM
{
    public class ParserCBLCMessage: XmlParser
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string strTempBuffer = "";
        private CBLCCriacaoPapeisMegabolsaInfo infoan53 = null;
        private string msgID53="";

        public const string TIPO_MSG_AN = "<AN  >";
        public const string TIPO_MSG_ANF= "<ANF >";
        public const string TIPO_MSG_AN53= "<AN53>";

        public void Parse(EventoSTM evento)
        {
            switch (evento.Cabecalho)
            {
                case TIPO_MSG_AN:
                    _parseAN(evento);
                    break;
                case TIPO_MSG_ANF:
                    _parseANF(evento);
                    break;
                case TIPO_MSG_AN53:
                    msgID53 = evento.MsgID;
                    Parse(evento.Corpo);
                    break;
                default:
                    logger.Warn("Mensagem desconhecida: [" + evento.Cabecalho + "]");
                    break;
            }
        }


        /// <summary>
        /// Confirmacao Negocio Megabolsa
        /// </summary>
        /// <param name="evento"></param>
        private void _parseAN(EventoSTM evento)
        {
            try
            {
                int offset = 0;
                CBLCConfirmacaoNegocioMegaBolsaInfo infoan = new CBLCConfirmacaoNegocioMegaBolsaInfo();

                infoan.Cabecalho = evento.Cabecalho;

                infoan.NaturezaOperacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD001_TAM_NATUREZA_OPERACAO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD001_TAM_NATUREZA_OPERACAO;

                if ( !infoan.NaturezaOperacao.Equals("60") &&
                    !infoan.NaturezaOperacao.Equals("61") &&
                    !infoan.NaturezaOperacao.Equals("64") &&
                    !infoan.NaturezaOperacao.Equals("65") )
                {
                    logger.Info("Ignorando Msg [" + evento.Corpo + "]");
                    return;
                }

                infoan.CodigoMontagem = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD002_TAM_CODIGO_MONTAGEM);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD002_TAM_CODIGO_MONTAGEM;

                infoan.CodigoNegociacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD003_TAM_CODIGO_NEGOCIACAO).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD003_TAM_CODIGO_NEGOCIACAO;

                infoan.CodigoISIN = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD004_TAM_CODIGO_ISIN).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD004_TAM_CODIGO_ISIN;

                infoan.NumeroDistribuicaoPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD005_TAM_NUMERO_DISTRIBUICAO_PAPEL);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD005_TAM_NUMERO_DISTRIBUICAO_PAPEL;

                infoan.NomeResumidoEmpresa = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD006_TAM_NOME_RESUMIDO_EMPRESA).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD006_TAM_NOME_RESUMIDO_EMPRESA;

                infoan.EspecificacaoPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD007_TAM_ESPEC_PAPEL).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD007_TAM_ESPEC_PAPEL;

                infoan.FatorCotacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD008_TAM_FATOR_COTACAO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD008_TAM_FATOR_COTACAO;

                infoan.CodigoMercado = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD009_TAM_CODIGO_MERCADO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD009_TAM_CODIGO_MERCADO;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD010_TAM_RESERVADO1;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD011_TAM_RESERVADO2;

                string preco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD012_TAM_PRECO_EXERCICIO_OPCOES);
                preco = preco.Insert(11, ".");
                infoan.PrecoExercicioOpcoes = Convert.ToDecimal(preco, CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD012_TAM_PRECO_EXERCICIO_OPCOES;

                infoan.CodigoNegociacaoPapelObjeto = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD013_TAM_CODIGO_NEGOCIACAO_PAPEL_OBJETO).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD013_TAM_CODIGO_NEGOCIACAO_PAPEL_OBJETO;

                infoan.NumeroSerie = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD014_TAM_NUMERO_SERIE);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD014_TAM_NUMERO_SERIE;

                infoan.CodigoBDI = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD015_TAM_CODIGO_BDI);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD015_TAM_CODIGO_BDI;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD016_TAM_RESERVADO3;

                infoan.NumeroNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD017_TAM_NUMERO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD017_TAM_NUMERO_NEGOCIO;

                infoan.HorarioFato = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD018_TAM_HORARIO_FATO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD018_TAM_HORARIO_FATO;

                infoan.TipoTransacaoNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD019_TAM_TIPO_TRANSACAO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD019_TAM_TIPO_TRANSACAO_NEGOCIO;

                preco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD020_TAM_PRECO_NEGOCIO);
                preco = preco.Insert(11, ".");
                infoan.PrecoNegocio = Convert.ToDecimal(preco, CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD020_TAM_PRECO_NEGOCIO;

                infoan.QuantidadeNegocio = Convert.ToInt64(evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD021_TAM_QUANTIDADE_NEGOCIO));
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD021_TAM_QUANTIDADE_NEGOCIO;

                preco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD022_TAM_PRECO_MEDIO_PAPEL);
                preco = preco.Insert(11, ".");
                infoan.PrecoMedioPapel = Convert.ToDecimal(preco, CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD022_TAM_PRECO_MEDIO_PAPEL;

                infoan.CodigoCliente = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD023_TAM_CODIGO_CLIENTE);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD023_TAM_CODIGO_CLIENTE;

                infoan.DVCliente = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD024_TAM_DIGITO_CLIENTE);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD024_TAM_DIGITO_CLIENTE;

                infoan.PrazoContradosTermo = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD025_TAM_PRAZO_CONTRATOS_TERMO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD025_TAM_PRAZO_CONTRATOS_TERMO;

                string dtvc = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD026_TAM_DATA_VENCIMENTO);
                if (dtvc.Equals("9999-12-31"))
                    infoan.DataVencimento = DateTime.MaxValue;
                else
                    infoan.DataVencimento = DateTime.ParseExact(dtvc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD026_TAM_DATA_VENCIMENTO;

                infoan.TipoPosicaoCliente = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD027_TAM_TIPO_POSICAO_CLIENTE);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD027_TAM_TIPO_POSICAO_CLIENTE;

                infoan.TipoLiquidacaoNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD028_TAM_TIPO_LIQUIDACAO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD028_TAM_TIPO_LIQUIDACAO_NEGOCIO;

                infoan.IndicadorEstrategiaOpcoes = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD029_TAM_INDICADOR_ESTRATEGIA_OPCOES);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD029_TAM_INDICADOR_ESTRATEGIA_OPCOES;

                preco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD030_TAM_PRECO_ULTIMO_NEGOCIO_PAPEL);
                preco = preco.Insert(11, ".");
                infoan.PrecoUltimoNegocioPapel = Convert.ToDecimal(preco, CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD030_TAM_PRECO_ULTIMO_NEGOCIO_PAPEL;

                infoan.QuantidadeNegociadaTotalPapel = Convert.ToInt64(evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD031_TAM_QTDE_TOTAL_NEGOCIADA_PAPEL));
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD031_TAM_QTDE_TOTAL_NEGOCIADA_PAPEL;

                infoan.VolumeNegociadoTotalPapel = Convert.ToInt64(evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD032_TAM_VOLUME_TOTAL_NEGOCIADO_PAPEL));
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD032_TAM_VOLUME_TOTAL_NEGOCIADO_PAPEL;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD033_TAM_RESERVADO4;

                infoan.IdentificacaoCorretoraDestino = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD034_TAM_IDENTIFICACAO_CORRETORA_DESTINO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD034_TAM_IDENTIFICACAO_CORRETORA_DESTINO;

                infoan.NumeroSequencialRegistro = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD035_TAM_NUMERO_SEQUENCIAL_REGISTRO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD035_TAM_NUMERO_SEQUENCIAL_REGISTRO;

                infoan.IdentificacaoCorretoraContraparte = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD036_TAM_IDENTIFICACAO_CORRETORA_CONTRAPARTE);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD036_TAM_IDENTIFICACAO_CORRETORA_CONTRAPARTE;

                dtvc = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD037_TAM_DATA_MOVIMENTO);
                infoan.DataMovimento = DateTime.ParseExact(dtvc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD037_TAM_DATA_MOVIMENTO;

                infoan.CodigoEmpresaEmissoraPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD038_TAM_CODIGO_EMPRESA_EMISSORA);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD038_TAM_CODIGO_EMPRESA_EMISSORA;

                infoan.TipoTituloPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD039_TAM_TIPO_TITULO_PAPEL);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD039_TAM_TIPO_TITULO_PAPEL;

                infoan.EspecieTituloPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD040_TAM_ESPECIA_TITULO_PAPEL);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD040_TAM_ESPECIA_TITULO_PAPEL;

                infoan.IndicadorOrigemNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD041_TAM_INDICADOR_ORIGEM_NEGOCIO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD041_TAM_INDICADOR_ORIGEM_NEGOCIO;

                infoan.CodigoClienteVendedor = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD042_TAM_CODIGO_CLIENTE_VENDEDOR).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD042_TAM_CODIGO_CLIENTE_VENDEDOR;

                infoan.CodigoClienteComprador = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD043_TAM_CODIGO_CLIENTE_COMPRADOR).Trim();
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD043_TAM_CODIGO_CLIENTE_COMPRADOR;

                infoan.CodigoSistema = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD044_TAM_CODIGO_SISTEMA);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD044_TAM_CODIGO_SISTEMA;

                infoan.FormaLiquidacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD045_TAM_FORMA_LIQUIDACAO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD045_TAM_FORMA_LIQUIDACAO;

                infoan.PrazoLiquidacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD046_TAM_PRAZO_LIQUIDACAO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD046_TAM_PRAZO_LIQUIDACAO;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD047_TAM_RESERVADO5;

                infoan.NumeroOfertaCompraMega = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD048_TAM_NUMERO_OFERTA_COMPRA_MEGA);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD048_TAM_NUMERO_OFERTA_COMPRA_MEGA;

                infoan.NumeroOfertaVendaMega = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD049_TAM_NUMERO_OFERTA_VENDA_MEGA);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD049_TAM_NUMERO_OFERTA_VENDA_MEGA;

                infoan.TipoTermo = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD050_TAM_TIPO_TERMO);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD050_TAM_TIPO_TERMO;

                infoan.OperadorComprador = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD051_TAM_OPERADOR_COMPRADOR);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD051_TAM_OPERADOR_COMPRADOR;

                infoan.OperadorVendedor = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD052_TAM_OPERADOR_VENDEDOR);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD052_TAM_OPERADOR_VENDEDOR;

                infoan.CodigoOfertaEstrategia1 = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD053_TAM_CODIGO_OFERTA_ESTRATEG1);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD053_TAM_CODIGO_OFERTA_ESTRATEG1;

                infoan.CodigoOfertaEstrategia2 = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD054_TAM_CODIGO_OFERTA_ESTRATEG2);
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD054_TAM_CODIGO_OFERTA_ESTRATEG2;
                offset += CBLCConfirmacaoNegocioMegaBolsaInfo.FIELD055_TAM_RESERVADO6;

                EventoCBLC newevent = new EventoCBLC(TIPO_MSG_AN, infoan, evento.MsgID);

                ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
            }
            catch (Exception ex)
            {
                logger.Error("Erro _parseAN()" + ex.Message, ex);
                logger.Error("Msg: [" + evento.Corpo + "]");
            }
        }

        /// <summary>
        /// Mensagens Bovespa FIX
        /// </summary>
        /// <param name="evento"></param>
        private void _parseANF(EventoSTM evento)
        {
            try
            {
                int offset = 0;
                CBLCConfirmacaoNegocioBovespaFixInfo infoanf = new CBLCConfirmacaoNegocioBovespaFixInfo();

                infoanf.Cabecalho = evento.Cabecalho;

                infoanf.CodigoSistemaNegociacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD001_TAM_CODIGO_SISTEMA_NEGOCIACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD001_TAM_CODIGO_SISTEMA_NEGOCIACAO;

                infoanf.DataNegociacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD002_TAM_DATA_NEGOCIACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD002_TAM_DATA_NEGOCIACAO;

                infoanf.TipoOperacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD003_TAM_TIPO_OPERACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD003_TAM_TIPO_OPERACAO;

                infoanf.CodigoInstrumento = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD004_TAM_CODIGO_INSTRUMENTO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD004_TAM_CODIGO_INSTRUMENTO;

                infoanf.CodigoISIN = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD005_TAM_CODIGO_ISIN);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD005_TAM_CODIGO_ISIN;

                infoanf.NumeroDistribuicao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD006_TAM_NUMERO_DISTRIBUICAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD006_TAM_NUMERO_DISTRIBUICAO;

                infoanf.NumeroSerie = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD007_TAM_NUMERO_SERIE);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD007_TAM_NUMERO_SERIE;

                infoanf.NomeResumidoSociedadeEmissora = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD008_TAM_NOME_SOCIEDADE_EMISSORA);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD008_TAM_NOME_SOCIEDADE_EMISSORA;

                infoanf.EspecificacaoPapel = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD009_TAM_ESPECIFICACAO_PAPEL);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD009_TAM_ESPECIFICACAO_PAPEL;

                infoanf.CodigoOperacaoOrigem = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD010_TAM_CODIGO_OPERACAO_ORIGEM);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD010_TAM_CODIGO_OPERACAO_ORIGEM;

                infoanf.CodigoRodaNegociacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD011_TAM_CODIGO_RODA_NEGOCIACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD011_TAM_CODIGO_RODA_NEGOCIACAO;

                infoanf.FatorCotacaoPreco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD012_TAM_FATOR_COTACAO_PRECO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD012_TAM_FATOR_COTACAO_PRECO;

                infoanf.CodigoMercado = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD013_TAM_CODIGO_MERCADO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD013_TAM_CODIGO_MERCADO;

                infoanf.PrecoExercicio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD014_TAM_PRECO_EXERCICIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD014_TAM_PRECO_EXERCICIO;

                infoanf.CodigoColocacaoMercado = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD015_TAM_CODIGO_COLOCACAO_MERCADO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD015_TAM_CODIGO_COLOCACAO_MERCADO;

                infoanf.TipoCotacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD016_TAM_TIPO_COTACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD016_TAM_TIPO_COTACAO;

                infoanf.CodigoBDI = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD017_TAM_CODIGO_BDI);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD017_TAM_CODIGO_BDI;

                infoanf.NumeroNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD018_TAM_NUMERO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD018_TAM_NUMERO_NEGOCIO;

                infoanf.HoraNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD019_TAM_HORA_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD019_TAM_HORA_NEGOCIO;

                infoanf.TipoTransacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD020_TAM_TIPO_TRANSACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD020_TAM_TIPO_TRANSACAO;

                infoanf.CodigoOrigemOperacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD021_TAM_CODIGO_ORIGEM_OPERACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD021_TAM_CODIGO_ORIGEM_OPERACAO;

                infoanf.PrecoNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD022_TAM_PRECO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD022_TAM_PRECO_NEGOCIO;

                infoanf.QuantidadeTitulosNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD023_TAM_QTDE_TITULOS_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD023_TAM_QTDE_TITULOS_NEGOCIO;

                infoanf.VolumeFinanceiroNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD024_TAM_VOL_FINANCEIRO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD024_TAM_VOL_FINANCEIRO_NEGOCIO;

                infoanf.PercentualOscilacaoPreco = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD025_TAM_PERCENTUAL_OSCILACAO_PRECO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD025_TAM_PERCENTUAL_OSCILACAO_PRECO;
                
                infoanf.SinalOscilacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD026_TAM_SINAL_OSCILACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD026_TAM_SINAL_OSCILACAO;

                infoanf.PrecoMedio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD027_TAM_PRECO_MEDIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD027_TAM_PRECO_MEDIO;

                infoanf.PrecoMedio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD027_TAM_PRECO_MEDIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD027_TAM_PRECO_MEDIO;

                infoanf.PrecoReferenciaInstrumento = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD028_TAM_PRECO_REFERENCIA_INSTRUMENTO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD028_TAM_PRECO_REFERENCIA_INSTRUMENTO;

                infoanf.Side = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD029_TAM_NATUREZA_OPERACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD029_TAM_NATUREZA_OPERACAO;

                infoanf.CodigoAgenteDestino = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD030_TAM_CODIGO_AGENTE_DESTINO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD030_TAM_CODIGO_AGENTE_DESTINO;

                infoanf.CodigoCorretoraDestino = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD031_TAM_CODIGO_CORRETORA_DESTINO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD031_TAM_CODIGO_CORRETORA_DESTINO;

                infoanf.CodigoAgenteContraparte = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD032_TAM_CODIGO_AGENTE_CONTRAPARTE);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD032_TAM_CODIGO_AGENTE_CONTRAPARTE;

                infoanf.CodigoCorretoraContraparte = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD033_TAM_CODIGO_CORRETORA_CONTRAPARTE);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD033_TAM_CODIGO_CORRETORA_CONTRAPARTE;

                infoanf.CodigoInvestidor = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD034_TAM_CODIGO_INVESTIDOR);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD034_TAM_CODIGO_INVESTIDOR;

                infoanf.PrazoLiquidacaoNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD035_TAM_PRAZO_LIQUIDACAO_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD035_TAM_PRAZO_LIQUIDACAO_NEGOCIO;

                infoanf.DataVencimento = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD036_TAM_DATA_VENCIMENTO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD036_TAM_DATA_VENCIMENTO;

                infoanf.FormaLiquidacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD037_TAM_FORMA_LIQUIDACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD037_TAM_FORMA_LIQUIDACAO;

                infoanf.NumeroOferta = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD038_TAM_NUMERO_OFERTA);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD038_TAM_NUMERO_OFERTA;

                infoanf.CodigoInstrumentoObjeto = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD039_TAM_CODIGO_INSTRUMENTO_OBJETO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD039_TAM_CODIGO_INSTRUMENTO_OBJETO;

                infoanf.NumeroOperacaoVinculada = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD040_TAM_NUMERO_OPERACAO_VINCULADA);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD040_TAM_NUMERO_OPERACAO_VINCULADA;

                infoanf.Brokeragem = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD041_TAM_BROKERAGEM);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD041_TAM_BROKERAGEM;

                infoanf.CodigoNegociacao = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD042_TAM_CODIGO_NEGOCIACAO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD042_TAM_CODIGO_NEGOCIACAO;

                infoanf.DataPrecoReferencia = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD043_TAM_DATA_PRECO_REFERENCIA);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD043_TAM_DATA_PRECO_REFERENCIA;

                infoanf.NomeResumidoSistema = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD044_TAM_NOME_RESUMIDO_SISTEMA);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD044_TAM_NOME_RESUMIDO_SISTEMA;

                infoanf.TaxaNegocio = evento.Corpo.Substring(offset, CBLCConfirmacaoNegocioBovespaFixInfo.FIELD045_TAM_TAXA_NEGOCIO);
                offset += CBLCConfirmacaoNegocioBovespaFixInfo.FIELD045_TAM_TAXA_NEGOCIO;

                EventoCBLC newevent = new EventoCBLC(TIPO_MSG_ANF, infoanf, evento.MsgID);

                ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);
            }
            catch (Exception ex)
            {
                logger.Error("Erro _parseANF()" + ex.Message, ex);
                logger.Error("Msg: [" + evento.Corpo + "]");
            }
        }

        #region XMLParser Overrides
        public void FillFromAttributes(string namespace1, string name, string name3, Hashtable attributes, bool hasInlineEnd)
        {
            StartElement(namespace1, name, name3, attributes, hasInlineEnd);
        }

        protected override void StartElement(string namespace1, string name, string name3, Hashtable attributes, bool hasInlineEnd)
        {
            strTempBuffer = "";

            if (name.Equals(CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_RAIZ))
            {
                infoan53 = new CBLCCriacaoPapeisMegabolsaInfo();
            }
        }

        protected override void EndElement(string param1, string param2, string param3)
        {
            if (infoan53 != null)
            {
                switch (param2)
                {
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_SISTEMA_NEGOCIACAO:
                        infoan53.CodigoSistemaNegociacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_ISIN:
                        infoan53.CodigoISIN = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_NUMERO_DISTRIBUICAO:
                        infoan53.NumeroDistribuicao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_NEGOCIACAO:
                        infoan53.CodigoNegociacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_NOME_RESUMIDO:
                        infoan53.NomeResumido = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_ESPECIFICACAO:
                        infoan53.CodigoEspecificacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_TIPO_MERCADO:
                        infoan53.TipoMercado = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_NUMERO_SERIE:
                        infoan53.NumeroSerie = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_DATA_INICIO_NEGOCIACAO:
                        infoan53.DataInicioNegociacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_DATA_LIMITE_NEGOCIACAO:
                        infoan53.DataLimitNegociacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_VALIDADE_PRECO_EXERCICIO:
                        infoan53.ValidadePrecoExercicio = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_DATA_VENCIMENTO:
                        infoan53.DataVencimento = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_FATOR_COTACAO:
                        infoan53.FatorCotacao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_QUANTIDADE_MINI_LOTE_PADRAO:
                        infoan53.QuantidadeMiniLotePadrao = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_CODIGO_NEGOCIACAO_PAPEL_OBJETO:
                        infoan53.CodigoNegociacaoPapelObjeto = strTempBuffer;
                        break;
                    case CBLCCriacaoPapeisMegabolsaInfo.XML_TAG_RAIZ:
                        {
                            // Reseta a "maquina de estado" e envia o evento

                            EventoCBLC newevent = new EventoCBLC(TIPO_MSG_AN53, infoan53, msgID53);

                            ServicoConectorSTM.epService.EPRuntime.SendEvent(newevent);

                            strTempBuffer = "";
                            infoan53 = null;
                        }
                        break;

                    default: break;
                }
            }
        }

        protected override void Characters(string param1, int param2, int param3)
        {
            strTempBuffer += param1.Substring(param2, param3);
        }
        #endregion //XMLParser Overrides
    }
}
