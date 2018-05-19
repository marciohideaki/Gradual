using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using OpenFAST;
using Gradual.MDS.Core.Lib;
using System.Globalization;

namespace Gradual.MDS.Core.Sinal
{
    public class NegocioBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NEGDadosNegocio Negocio { get; set; }
        public List<LNGDadosNegocio> LivroNegocios { get; set; }
        public List<RNKDadosCorretora> RankCompradora { get; set; }
        public List<RNKDadosCorretora> RankVendedora { get; set; }
        public List<string> IndicesAtrelados { get; set; }

        public int CasasDecimais { get; set; }
        public string Instrumento { get; set; }
        public string TipoBolsa { get; set; }
        public Decimal CoeficienteMultiplicacao { get; set; }
        public string Especificacao { get; set; }
        public string GrupoCotacao { get; set; }
        public string SegmentoMercado { get; set; }
        public string FaseNegociacao { get; set; }
        public string CodigoISIN { get; set; }
        public string CodigoPapelObjeto { get; set; }
        public int LotePadrao { get; set; }
        public int FormaCotacao { get; set; }
        public string DataVencimento { get; set; }
        public Decimal PrecoExercicio { get; set; }
        public string IndicadorOpcao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string SecurityIDSource { get; set; }

        public NegocioBase(string instrumento)
        {
            this.Instrumento = instrumento;
            this.Negocio = new NEGDadosNegocio(Instrumento);
            this.LivroNegocios = new List<LNGDadosNegocio>();
            this.RankCompradora = new List<RNKDadosCorretora>();
            this.RankVendedora = new List<RNKDadosCorretora>();
            this.IndicesAtrelados = new List<string>();
            this.FaseNegociacao = ConstantesMDS.FASE_NEGOCIACAO_SUSPENDER_ANALISE_GRAFICA;
        }


        public static void AdicionarLivroNegocios(NegocioBase registroNegocios, int maxItensLNG)
        {
            LNGDadosNegocio dadosLNG = new LNGDadosNegocio();

            dadosLNG.Compradora = registroNegocios.Negocio.Compradora;
            dadosLNG.Hora = registroNegocios.Negocio.Hora;
            dadosLNG.NumeroNegocio = registroNegocios.Negocio.NumeroNegocio;
            dadosLNG.Preco = registroNegocios.Negocio.Preco;
            dadosLNG.Quantidade = registroNegocios.Negocio.Quantidade;
            dadosLNG.Vendedora = registroNegocios.Negocio.Vendedora;

            registroNegocios.LivroNegocios.Insert(0,dadosLNG);

            if (registroNegocios.LivroNegocios.Count > maxItensLNG)
                registroNegocios.LivroNegocios.RemoveRange(maxItensLNG - 1, registroNegocios.LivroNegocios.Count - maxItensLNG);
        }

        public static int BuscarPosicaoNegocioCancelado(string numeroNegocioFormatado, NegocioBase registroNegocios)
        {
            for (int i = 0; i < registroNegocios.LivroNegocios.Count; i++)
            {
                if (registroNegocios.LivroNegocios[i].NumeroNegocio.Equals(numeroNegocioFormatado))
                {
                    return i;
                }
            }

            return -1;
        }

        public static void RemoverNegocioCancelado(int posicao, NegocioBase registroNegocios)
        {
            if (posicao < registroNegocios.LivroNegocios.Count)
                registroNegocios.LivroNegocios.RemoveAt(posicao);
        }


        /// <summary>
        /// Monta um dicionario de valores para envio ao streamer correspondendo a uma
        /// entrada no livro de negocios.
        /// </summary>
        /// <param name="acao"></param>
        /// <param name="posicao"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Dictionary<string, string> montarItemLivroNegociosStreamer(int acao, int posicao, LNGDadosNegocio item, int casasDecimais)
        {
            Dictionary<string, string> mensagem = new Dictionary<String, String>();

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_ACAO, acao.ToString());

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_POSICAO, posicao.ToString());

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_NUMERO_NEGOCIO, (item.NumeroNegocio != null ? item.NumeroNegocio.Trim() : "0"));

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_HORA, item.Hora);

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_PRECO, String.Format("{0:f" + casasDecimais + "}", item.Preco).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_QUANTIDADE, item.Quantidade.ToString());

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_COMPRADORA, item.Compradora);

            mensagem.Add(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_VENDEDORA, item.Vendedora);

            return mensagem;
        }


        /// <summary>
        /// Monta um dicionario de valores correspondente a um sinal/evento de negocio
        /// para o streamer
        /// </summary>
        /// <param name="negocio"></param>
        /// <returns></returns>
        public static Dictionary<string, string> montarNegocioStreamer(NEGDadosNegocio negocio, int casasDecimais)
        {
            Dictionary<string, string> mensagem = new Dictionary<String, String>();

            // Corpo da mensagem Streamer de Negocio
            if (String.IsNullOrEmpty(negocio.Data))
                mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_DATA, "00000000"); //DateTime.Now.ToString("yyyyMMdd"));
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_DATA, negocio.Data);

            if (String.IsNullOrEmpty(negocio.Hora))
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_HORA, "000000000"); //DateTime.Now.ToString("HHmmssfff"));
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_HORA, negocio.Hora + "000");

            if ( String.IsNullOrEmpty(negocio.Compradora) )
                mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_COMPRADORA, "0");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_COMPRADORA, negocio.Compradora);

            if (String.IsNullOrEmpty(negocio.Vendedora))
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_VENDEDORA, "0");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_VENDEDORA, negocio.Vendedora);

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_PRECO, String.Format("{0:f" + casasDecimais + "}", negocio.Preco).Replace('.', ','));

            mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_QUANTIDADE, negocio.Quantidade.ToString());

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_PRECO_ABERTURA, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoAbertura).Replace('.', ','));


            // Preco de ajuste...revisar tratamento de BMF
            //if ( negocio.PrecoAjuste != Decimal.Zero )
            //    mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_PRECO_FECHAMENTO, String.Format("{0:f2}", negocio.PrecoAjuste).Replace('.', ','));
            //else
            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_PRECO_FECHAMENTO, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoFechamento).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MAXIMA_DIA, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoMaximo).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MINIMA_DIA, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoMinimo).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MEDIO, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoMedio).Replace('.', ','));


            //Decimal volume = Decimal.Zero;
            //if ( negocio.VolumeTotal > Decimal.Zero && negocio.PrecoMedio > Decimal.Zero )
            //     volume = negocio.VolumeTotal / negocio.PrecoMedio;
            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_VOLUME_ACUMULADO, String.Format("{0:f" + casasDecimais + "}", negocio.VolumeTotal).Replace('.', ','));

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_QUANTIDADE_ACUMULADA_PAPEIS, negocio.QtdeNegociadaDia.ToString());

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_NUMERO_NEGOCIOS, negocio.QtdeNegocios.ToString());

            
            if ( negocio.Variacao < Decimal.Zero )
                mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_INDICADOR_VARIACAO, "-");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_INDICADOR_VARIACAO, " ");

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_VARIACAO, String.Format("{0:f2}", Math.Abs(negocio.Variacao)).Replace('.', ','));

            mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_ESTADO_PAPEL, negocio.EstadoInstrumento.ToString());

            if (negocio.MelhorPrecoCompra == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE)
                mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_MELHOR_OFERTA_COMPRA, "0,00");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MELHOR_OFERTA_COMPRA, String.Format("{0:f" + casasDecimais + "}", negocio.MelhorPrecoCompra).Replace('.', ','));

            if (negocio.MelhorPrecoVenda == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE)
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MELHOR_OFERTA_VENDA, "0,00");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MELHOR_OFERTA_VENDA, String.Format("{0:f" + casasDecimais + "}", negocio.MelhorPrecoVenda).Replace('.', ','));

            mensagem.Add( ConstantesMDS.HTTP_NEGOCIO_MELHOR_QUANTIDADE_COMPRA, negocio.MelhorQuantidadeCompra.ToString());

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_MELHOR_QUANTIDADE_VENDA, negocio.MelhorQuantidadeVenda.ToString());

            // Informacoes do leilao
            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_PRECO_TEORICO_ABERTURA, String.Format("{0:f" + casasDecimais + "}", negocio.PrecoTeoricoAbertura).Replace('.', ','));

            if ( negocio.VariacaoTeorica < Decimal.Zero)
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_INDICADOR_VARIACAO_TEORICA, "-");
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_INDICADOR_VARIACAO_TEORICA, " ");

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_VARIACAO_TEORICA, String.Format("{0:f2}", Math.Abs(negocio.VariacaoTeorica)).Replace('.', ','));

            if (negocio.HorarioTeorico == DateTime.MinValue)
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_HORARIO_TEORICO, "00000000000000"); //negocio.HorarioTeorico.ToString("yyyyMMddHHmmss"));
            else
                mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_HORARIO_TEORICO, negocio.HorarioTeorico.ToString("yyyyMMddHHmmss"));

            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_ESTADO_GRUPO, negocio.EstadoGrupo);
            mensagem.Add(ConstantesMDS.HTTP_NEGOCIO_ESTADO_PAPEL2, negocio.EstadoPapel2);

            return mensagem;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="livroNegocios"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> montaLivroNegociosCompleto(List<LNGDadosNegocio> livroNegocios, int casasDecimais)
        {
            List<Dictionary<string, string>> retorno = new List<Dictionary<string, string>>();

            for (int i = 0; i < livroNegocios.Count; i++)
            {
                retorno.Add(
                    NegocioBase.montarItemLivroNegociosStreamer(ConstantesMDS.HTTP_LIVRO_NEGOCIOS_TIPO_ACAO_INCLUIR, i,
                        livroNegocios[i], casasDecimais));
            }

            return retorno;
        }

        /// <summary>
        /// Monta a string da mensagem de negocio pra envio ao HomeBroker
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarNegocioHomeBroker(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();
            NEGDadosNegocio negocio = registroNegocio.Negocio;


            builder.Append( MDSUtils.montaHeaderHomeBroker(registroNegocio.Instrumento,
                ConstantesMDS.TIPO_REQUISICAO_HB_NEGOCIO, registroNegocio.TipoBolsa));

            // Corpo da mensagem MDS de Negocio
            if (String.IsNullOrEmpty(negocio.Data))
                builder.Append("00000000");
            else
                builder.Append(negocio.Data);

            if (String.IsNullOrEmpty(negocio.Hora))
                builder.Append("000000000");
            else
                builder.Append(negocio.Hora.PadRight(9,'0'));

            if (String.IsNullOrEmpty(negocio.Compradora))
                builder.Append("00000000");
            else
                builder.Append(negocio.Compradora.PadLeft(8,'0'));

            if (String.IsNullOrEmpty(negocio.Vendedora))
                builder.Append("00000000");
            else
                builder.Append(negocio.Vendedora.PadLeft(8, '0'));

            if (negocio.Preco < 0)
                builder.Append("000000000,000");
            else
                builder.Append(negocio.Preco.ToString("000000000.000").Replace('.', ','));

            builder.Append(negocio.Quantidade.ToString().PadLeft(12, '0'));

            if (negocio.PrecoMaximo < 0)
                builder.Append("000000000,000");
            else
                builder.Append(negocio.PrecoMaximo.ToString("000000000.000").Replace('.', ','));

            if (negocio.PrecoMinimo < 0)
                builder.Append("000000000,000");
            else
                builder.Append(negocio.PrecoMinimo.ToString("000000000.000").Replace('.', ','));


            //BigDecimal volumeTotal = new BigDecimal(0);
            //try
            //{
            //    if (dadosInstrumentos.getTipoBolsa().equals(ConstantesMDS.DESCRICAO_DE_BOLSA_BMF))
            //    {
            //        if (dadosInstrumentos.getPrecoMedio() != null && !dadosInstrumentos.getPrecoMedio().trim().isEmpty())
            //        {
            //            BigDecimal precoMedio = new BigDecimal(
            //                    dadosInstrumentos.getPrecoMedio().replace(',', '.'));
            //            if (!precoMedio.equals(new BigDecimal(0)))
            //            {
            //                volumeTotal = dadosInstrumentos.getVolumeTotal().
            //                    divide(precoMedio, 6, RoundingMode.HALF_UP);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        volumeTotal = dadosInstrumentos.getVolumeTotal();
            //    }
            //}
            //catch (Exception ex) { }

            //builder.Append(String.format("%013d", volumeTotal.longValue()));
            builder.Append(negocio.VolumeTotal.ToString("0000000000000", CultureInfo.InvariantCulture));

            builder.Append(negocio.QtdeNegocios.ToString().PadLeft(8, '0'));

            Decimal variacao = Math.Abs(negocio.Variacao);
            if (negocio.Variacao >= Decimal.Zero)
            {
                builder.Append(" ");
            }
            else
            {
                builder.Append("-");
            }

            if (variacao > 99999 || variacao < -99999)
                builder.Append("00000,00");
            else
                builder.Append(variacao.ToString("00000.00", CultureInfo.InvariantCulture).Replace('.', ','));

            builder.Append(negocio.EstadoInstrumento.ToString());

            if (negocio.MelhorQuantidadeCompra < 0)
                builder.Append("000000000000");
            else
                builder.Append(negocio.MelhorQuantidadeCompra.ToString().PadLeft(12, '0'));

            if (negocio.MelhorQuantidadeVenda < 0)
                builder.Append("000000000000");
            else
                builder.Append(negocio.MelhorQuantidadeVenda.ToString().PadLeft(12, '0'));

            builder.Append(negocio.QtdeNegociadaDia.ToString().PadLeft(12, '0'));


            builder.Append(negocio.PrecoMedio.ToString("000000000.000", CultureInfo.InvariantCulture).Replace('.', ','));
            // Dados do Leilao
            builder.Append(negocio.PrecoTeoricoAbertura.ToString("000000000.000", CultureInfo.InvariantCulture).Replace('.', ','));

            if (negocio.Variacao >= Decimal.Zero)
            {
                builder.Append(" ");
            }
            else
            {
                builder.Append("-");
            }
            variacao = Math.Abs(negocio.VariacaoTeorica);

            builder.Append(variacao.ToString("00000.00", CultureInfo.InvariantCulture).Replace('.', ','));
            builder.Append(negocio.HorarioTeorico.ToString("yyyyMMddHHmmss"));

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarNegocioAnaliseGrafica(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS de Negocio
            builder.Append(registroNegocio.Instrumento.PadRight(8).Substring(0, 8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_NEGOCIO);
            if ( !String.IsNullOrEmpty(registroNegocio.TipoBolsa) )
                builder.Append(registroNegocio.TipoBolsa);
            else
                builder.Append("BV");
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registroNegocio.Instrumento.PadRight(20));

            // Corpo da mensagem MDS de Negocio
            builder.Append(String.IsNullOrEmpty(registroNegocio.Negocio.Data) ? "00000000" : registroNegocio.Negocio.Data);
            builder.Append(String.IsNullOrEmpty(registroNegocio.Negocio.Hora) ? "000000000" : registroNegocio.Negocio.Hora.PadRight(9, '0'));
            builder.Append(String.IsNullOrEmpty(registroNegocio.Negocio.Compradora) ? "00000000" : registroNegocio.Negocio.Compradora.PadLeft(8, '0'));
            builder.Append(String.IsNullOrEmpty(registroNegocio.Negocio.Vendedora) ? "00000000" : registroNegocio.Negocio.Vendedora.PadLeft(8, '0'));
            builder.Append(registroNegocio.Negocio.Preco.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registroNegocio.Negocio.Quantidade.ToString().PadLeft(12, '0'));
            builder.Append(registroNegocio.Negocio.PrecoMaximo.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registroNegocio.Negocio.PrecoMinimo.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registroNegocio.Negocio.VolumeTotal.ToString("0000000000000"));
            builder.Append(registroNegocio.Negocio.QtdeNegocios.ToString().PadLeft(8, '0'));
            builder.Append(String.Format("{0: 00000.00;-00000.00}", registroNegocio.Negocio.Variacao).Replace('.', ','));
            builder.Append(registroNegocio.Negocio.EstadoInstrumento.ToString());
            builder.Append(registroNegocio.FaseNegociacao.ToString());

            // Corpo da mensagem MDS de Negocio - Dados de Leilão
            builder.Append(registroNegocio.Negocio.PrecoTeoricoAbertura.ToString("000000000.000", CultureInfo.InvariantCulture).Replace('.', ','));
            builder.Append(String.Format("{0: 00000.00;-00000.00}", registroNegocio.Negocio.VariacaoTeorica).Replace('.', ','));
            builder.Append(registroNegocio.Negocio.HorarioTeorico.ToString("yyyyMMddHHmmss"));

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public static string montarSerieHistorica(NegocioBase registro)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS de Serie Historica
            builder.Append(registro.Instrumento.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_SERIE_HISTORICA);
            if (!String.IsNullOrEmpty(registro.TipoBolsa))
                builder.Append(registro.TipoBolsa);
            else
                builder.Append("BV");
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registro.Instrumento.PadRight(20));

            // Corpo da mensagem MDS de Serie Historica
            builder.Append(String.IsNullOrEmpty(registro.Negocio.Data) ? "00000000" : registro.Negocio.Data);
            builder.Append(String.IsNullOrEmpty(registro.Negocio.Hora) ? "000000000" : registro.Negocio.Hora.PadRight(9, '0'));
            builder.Append(registro.Negocio.PrecoAbertura.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registro.Negocio.PrecoFechamento.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registro.Negocio.PrecoMedio.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registro.Negocio.PrecoMaximo.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registro.Negocio.PrecoMinimo.ToString("0000000000.00").Replace('.', ','));
            builder.Append(String.Format("{0: 00000.00;-00000.00}", registro.Negocio.Variacao).Replace('.', ','));
            
            Decimal melhorPrecoCompra =
                (registro.Negocio.MelhorPrecoCompra == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE ||
                registro.Negocio.MelhorPrecoCompra == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE ? 0 : registro.Negocio.MelhorPrecoCompra);
            Decimal melhorPrecoVenda =
                (registro.Negocio.MelhorPrecoVenda == ConstantesMDS.PRECO_LIVRO_OFERTAS_MAX_VALUE ||
                registro.Negocio.MelhorPrecoVenda == ConstantesMDS.PRECO_LIVRO_OFERTAS_MIN_VALUE ? 0 : registro.Negocio.MelhorPrecoVenda);

            builder.Append(melhorPrecoCompra.ToString("0000000000.00").Replace('.', ','));
            builder.Append(melhorPrecoVenda.ToString("0000000000.00").Replace('.', ','));
            builder.Append(registro.Negocio.QtdeNegocios.ToString().PadLeft(8, '0'));
            builder.Append(registro.Negocio.QtdeNegociadaDia.ToString().PadLeft(12, '0'));
            builder.Append(Convert.ToInt64(registro.Negocio.VolumeTotal).ToString().PadLeft(13, '0'));
            builder.Append(registro.Negocio.PrecoAjuste.ToString("0000000000.00").Replace('.', ','));
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarCadastroBasico(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                // Cabecalho da mensagem MDS de Cadastro Basico
                builder.Append(registroNegocio.Instrumento.PadRight(8));
                builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_CADASTRO_BASICO);
                builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                builder.Append(registroNegocio.Instrumento.PadRight(20));

                // Corpo da mensagem MDS de Cadastro Basico
                builder.Append(registroNegocio.CodigoISIN.PadRight(20));
                builder.Append(registroNegocio.CodigoPapelObjeto.PadRight(20));
                builder.Append(String.Format("{0:d8}", registroNegocio.LotePadrao));
                builder.Append(registroNegocio.SegmentoMercado.PadRight(8));
                builder.Append(String.Format("{0:d8}", registroNegocio.FormaCotacao));
                builder.Append(registroNegocio.GrupoCotacao.PadRight(4));
                builder.Append(registroNegocio.DataVencimento.ToString().PadLeft(8, '0'));
                builder.Append(registroNegocio.PrecoExercicio.ToString("00000000.0000").Replace('.', ','));
                builder.Append(registroNegocio.IndicadorOpcao.PadRight(1));
                builder.Append(registroNegocio.CoeficienteMultiplicacao.ToString("00000000.0000").Replace('.', ','));
                builder.Append(registroNegocio.Negocio.Data.PadLeft(8));
                builder.Append(registroNegocio.Negocio.Hora.PadLeft(6));
                builder.Append(registroNegocio.SecurityIDSource.PadRight(4));
                builder.Append(registroNegocio.Especificacao.PadRight(100));
            }
            catch (Exception ex)
            {
                logger.Error("Falha em montarCadastroBasico: " + ex.Message);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarComposicaoIndice(string indice, List<string> composicao)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS
            builder.Append(indice.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_COMPOSICAO_INDICE);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(indice.PadRight(20));

            // Corpo da mensagem MDS
            builder.Append(String.Format("{0:d4}", composicao.Count));
            foreach (string item in composicao)
                builder.Append(item.PadRight(20));

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarPrecoAbertura(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS
            builder.Append(registroNegocio.Instrumento.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_PRECO_ABERTURA);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registroNegocio.Instrumento.PadRight(20));

            // Corpo da mensagem MDS
            builder.Append(registroNegocio.Negocio.Data.PadLeft(8));
            builder.Append(registroNegocio.Negocio.Hora.PadLeft(6));
            builder.Append(registroNegocio.Negocio.PrecoAbertura.ToString("00000000.0000").Replace('.', ','));
            if (!String.IsNullOrEmpty(registroNegocio.TipoBolsa))
                builder.Append(registroNegocio.TipoBolsa);
            else
                builder.Append("BV");

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarPrecoFechamento(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS
            builder.Append(registroNegocio.Instrumento.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_PRECO_FECHAMENTO);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registroNegocio.Instrumento.PadRight(20));

            // Corpo da mensagem MDS
            builder.Append(registroNegocio.Negocio.Data.PadLeft(8));
            builder.Append(registroNegocio.Negocio.Hora.PadLeft(6));
            builder.Append(registroNegocio.Negocio.PrecoFechamento.ToString("00000000.0000").Replace('.', ','));
            if (!String.IsNullOrEmpty(registroNegocio.TipoBolsa))
                builder.Append(registroNegocio.TipoBolsa);
            else
                builder.Append("BV");
            builder.Append(registroNegocio.Negocio.VolumeTotal.ToString("0000000000000"));
            builder.Append(registroNegocio.Negocio.QtdeNegocios.ToString().PadLeft(8, '0'));

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarPrecoAjuste(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS
            builder.Append(registroNegocio.Instrumento.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_PRECO_AJUSTE);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registroNegocio.Instrumento.PadRight(20));

            // Corpo da mensagem MDS
            builder.Append(registroNegocio.Negocio.Data.PadLeft(8));
            builder.Append(registroNegocio.Negocio.Hora.PadLeft(6));
            builder.Append(registroNegocio.Negocio.PrecoAjuste.ToString("00000000.0000").Replace('.', ','));
            if (!String.IsNullOrEmpty(registroNegocio.TipoBolsa))
                builder.Append(registroNegocio.TipoBolsa);
            else
                builder.Append("BV");

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarPrecoUnitario(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();

            // Cabecalho da mensagem MDS
            builder.Append(registroNegocio.Instrumento.PadRight(8));
            builder.Append(ConstantesMDS.TIPO_REQUISICAO_HB_PRECO_UNITARIO);
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            builder.Append(registroNegocio.Instrumento.PadRight(20));

            // Corpo da mensagem MDS
            builder.Append(registroNegocio.Negocio.Data.PadLeft(8));
            builder.Append(registroNegocio.Negocio.Hora.PadLeft(6));
            builder.Append(registroNegocio.Negocio.PrecoUnitario.ToString("00000000.0000").Replace('.', ','));
            if (!String.IsNullOrEmpty(registroNegocio.TipoBolsa))
                builder.Append(registroNegocio.TipoBolsa);
            else
                builder.Append("BV");

            return builder.ToString();
        }

        /// <summary>
        /// Monta a string da mensagem de negocio pra envio ao HomeBroker
        /// </summary>
        /// <param name="registroNegocio"></param>
        /// <returns></returns>
        public static string montarLivroNegociosHomeBroker(NegocioBase registroNegocio)
        {
            StringBuilder builder = new StringBuilder();
            NEGDadosNegocio negocio = registroNegocio.Negocio;


            builder.Append(MDSUtils.montaHeaderHomeBroker(registroNegocio.Instrumento,
                ConstantesMDS.TIPO_REQUISICAO_HB_LIVRO_NEGOCIOS, registroNegocio.TipoBolsa));

            int numItens = 30; //TODO: parametrizar
            int posicaoItem = 0;

		    while ( posicaoItem < numItens &&
				    posicaoItem < registroNegocio.LivroNegocios.Count )
		    {
                LNGDadosNegocio item = registroNegocio.LivroNegocios[posicaoItem];

                if (item != null)
                {
                    if ( !String.IsNullOrEmpty(item.NumeroNegocio) )
                        builder.Append(item.NumeroNegocio.PadLeft(8, '0'));
                    else
                        builder.Append("00000000");

                    if (!String.IsNullOrEmpty(item.Hora))
                        builder.Append(item.Hora.PadLeft(6, '0'));
                    else
                        builder.Append("000000");

                    builder.Append(item.Preco.ToString("0000000000.00"));
                    builder.Append(String.Format("{0:d12}", item.Quantidade));

                    if (!String.IsNullOrEmpty(item.Compradora))
                        builder.Append(item.Compradora.PadLeft(8, '0'));
                    else
                        builder.Append("00000000");

                    if (!String.IsNullOrEmpty(item.Vendedora))
                        builder.Append(item.Vendedora.PadLeft(8, '0'));
                    else
                        builder.Append("00000000");
                }
			
			    posicaoItem++;
		    }

            return builder.ToString();
        }
    }
}
