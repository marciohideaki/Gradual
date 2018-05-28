using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ
{
    [Serializable]
    public class RegistrosIMBARQ001
    {

        /// <summary>
        /// Registro 00 - Header para o IMBARQ001
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Header
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// codigo do arquivo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] CodigoArquivo;

            /// <summary>
            /// Cog categoria usuario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] CodCatUsuario;

            /// <summary>
            /// Codigo do usuario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] CodigoUsuario;

            /// <summary>
            /// Codigo da Origem
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] CodigoOrigem;

            /// <summary>
            /// CodigoDestino
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoDestino;

            /// <summary>
            /// Numero do Movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] NumeroMovimento;

            /// <summary>
            /// Data da Geracao do Arquivo AAAAMMDD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] DataGeracaoArquivo;

            /// <summary>
            /// DataMovimento AAAAMMDD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] DataMovimento;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 931)]
            public byte[] Filler;
        }


        /// <summary>
        /// Registo 99 - Trailler para o IMBARQ001
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Trailler
        {
            /// <summary>
            /// Tipo do registro fixo 99
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// codigo do arquivo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] CodigoArquivo;

            /// <summary>
            /// Cog categoria usuario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] CodCatUsuario;

            /// <summary>
            /// Codigo do usuario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] CodigoUsuario;

            /// <summary>
            /// Codigo da Origem
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] CodigoOrigem;

            /// <summary>
            /// CodigoDestino
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoDestino;


            /// <summary>
            /// Numero do Movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] NumeroMovimento;

            /// <summary>
            /// Data da Geracao do Arquivo AAAAMMDD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] DataGeracaoArquivo;

            /// <summary>
            /// Total de registros gerados
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] TotalRegistros;

            /// <summary>
            /// DataMovimento AAAAMMDD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] DataMovimento;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 922)]
            public byte[] Filler;
        }



        /// <summary>
        /// Registro 01 - posicao de derivativos fungiveis
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro01
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo Origem identificacao instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,3)]
            public byte[] CodBolsaValor;

            /// <summary>
            /// Codigo negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,4)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,5)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true,6)]
            public byte[] Mercado;

            /// <summary>
            /// Lote Padrao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] LotePadrao;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] Mercadoria;

            /// <summary>
            /// Codigo Vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoVencimento;

            /// <summary>
            /// Indicador tipo de serie
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] IndicadorTipoSerie;

            /// <summary>
            /// Distribuicao da opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,7)]
            public byte[] DistribuicaoOpcao;

            /// <summary>
            /// ISIN Ativo objecto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] IsinAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Preco de Exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoExercicio;

            /// <summary>
            /// Tipo  da opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true, 9)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeOptionType, "", 7)]
            public byte[] TipoOpcao;

            /// <summary>
            /// Fator de cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] FatorCotacao;

            /// <summary>
            /// Estilo de opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] EstiloOpcao;

            /// <summary>
            /// Data de vencimento AAAA-MM-DD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Data da posicao AAAA-MM-DD
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataPosicao;

            /// <summary>
            /// Natureza da posicao inicial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            [IMBARQExport(true, 11)]
            public byte[] NaturezaPosicaoInicial;

            /// <summary>
            /// Valor da posicao inicial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 12)]
            public byte[] ValorPosicaoInicial;


            /// <summary>
            /// Natureza da posicao Vencida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            public byte[] NaturezaPosicaoVencida;

            /// <summary>
            /// Valor da posicao venciad
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoVencida;

            /// <summary>
            /// Natureza da posicao encerrada exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            [IMBARQExport(true, 13)]
            public byte[] NaturezaPosicaoEncerradaExerc;

            /// <summary>
            /// Valor da posicao encerrada exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 13)]
            public byte[] ValorPosicaoEncerradaExerc;

            /// <summary>
            /// Quantidade comprada no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 14)]
            public byte[] QuantidadeCompradaDia;

            /// <summary>
            /// QuantidadeVendidaDia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 15)]
            public byte[] QuantidadeVendidaDia;

            /// <summary>
            /// Natureza da posicao enviada por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            [IMBARQExport(true, 16)]
            public byte[] NaturezaPosicaoEnviadaTransfer;

            /// <summary>
            /// Valor da posicao posicao enviada por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 17)]
            public byte[] ValorPosicaoEnviadaTransfer;

            /// <summary>
            /// Valor da posicao comprada recebida por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] PosicaoCompradaRecebidaTransfer;

            /// <summary>
            /// Valor da posicao vendida recebida por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] PosicaoVendidaRecebidaTransfer;

            /// <summary>
            /// Natureza da posicao encerrada por entrega fisica
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            public byte[] NaturezaPosicaoEncerradaEntregaFisica;

            /// <summary>
            /// Valor da posicao encerrada por entrega fisica
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoPosicaoEncerradaEntregaFisica;

            /// <summary>
            /// Natureza da posicao atual
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            [IMBARQExport(true, 18)]
            public byte[] NaturezaPosicaoAtual;

            /// <summary>
            /// Valor da posicao atual
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoAtual;

            /// <summary>
            /// Posicao comprada bloqueada por exericisio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoCompradaBloqExerc;

            /// <summary>
            /// Natureza da posicao atual apos eventos corporativos
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            public byte[] NaturezaPosicaoAtualAposEvtCorp;

            /// <summary>
            /// Valor da posicao atual apos eventos corporativos
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoAtualAposEvtCorp;

            /// <summary>
            /// Valor da posicao coberta vendida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 20)]
            public byte[] ValorPosicaoCobertaVendida;

            /// <summary>
            /// Valor da posicao descoberta vendida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            [IMBARQExport(true, 21)]
            public byte[] ValorPosicaoDescobertaVendida;

            /// <summary>
            /// Valor da posicao BOX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 0)]
            public byte[] ValorPosicaoBOX;

            /// <summary>
            /// Natureza da posicao encerrada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            public byte[] NaturezaPosicaoEncerrada;

            /// <summary>
            /// Valor da posicao encerrada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] ValorPosicaoEncerrada;


            /// <summary>
            /// Valor da posicao comprada atual
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorPosicaoCompradaAtual;

            /// <summary>
            /// Valor da posicao vendida atual
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorPosicaoVendidaAtual;

            /// <summary>
            /// Natureza do ajuste diario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteDiario;

            /// <summary>
            /// Valo do ajuste diario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteDiario;

            /// <summary>
            /// Natureza do ajuste diario relacionado a posicao inicial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteDiarioRelatPosInicial;

            /// <summary>
            /// Valo do ajuste diario relacionado a posicao inicial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteDiarioRelatPosInicial;

            /// <summary>
            /// Natureza do ajuste diario relacionado a posicao recebida por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteDiarioRelatPosRecebTransfer;

            /// <summary>
            /// Valo do ajuste diario relacionado a posicao recebida por transferencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteDiarioRelatPosPosRecebTransfer;

            /// <summary>
            /// Natureza do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteDiarioRelatNegociosDia;

            /// <summary>
            /// Valo do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteDiarioRelatNegociosDia;

            /// <summary>
            /// Natureza do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteAcumulado;

            /// <summary>
            /// Valo do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteAcumulado;

            /// <summary>
            /// Natureza do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaAjusteAcumuladoSobrePosEncerrada;

            /// <summary>
            /// Valo do ajuste diario relacionado aos negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAjusteAcumuladoSobrePosEncerrada;

            /// <summary>
            /// Natureza do premio de opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            [IMBARQExport(true, 24)]
            public byte[] NaturezaPremioOpcao;

            /// <summary>
            /// Valo do premio de opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            [IMBARQExport(true, 25)]
            public byte[] ValorPremioOpcao;

            /// <summary>
            /// Codigo variavel do valor final
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] CodigoVariavelValorFinal;

            /// <summary>
            /// Natureza valor consolidado de negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorConsolidadoNegDia;

            /// <summary>
            /// Valo consolidado de negocios do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorConsolidadoNegDia;

            /// <summary>
            /// Natureza do valor recebido por transferencia no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorRecebTransfDia;

            /// <summary>
            /// Valos recebido por transferencia no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorRecebTransfDia;

            /// <summary>
            /// Natureza do valor enviado por transferencia no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorEnviadoTransfDia;

            /// <summary>
            /// Valor enviado por transferencia no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorEnviadoTransfDia;

            /// <summary>
            /// Natureza posicao atualizada no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaPosicaoAtualizadaDia;

            /// <summary>
            /// Valor da posicao atualizada no dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorPosicaoAtualizadaDia;

            /// <summary>
            /// Instrumento do cupom
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] InstrumentoCupom;

            /// <summary>
            /// Natureza valor consolidado negocios dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorConsolidadoNegociosDiaCupom;

            /// <summary>
            /// Valor consolidado negocios do dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorConsolidadoNegociosDiaCupom;

            /// <summary>
            /// Natureza valor recebido por transferencia no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorRecebTransferDiaCupom;

            /// <summary>
            /// Valor recebido por transferencia no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorRecebidoTransferDiaCupom;

            /// <summary>
            /// Natureza valor enviado por transferencia no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorEnviadoTransferDiaCupom;

            /// <summary>
            /// Valor recebido por transferencia no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorEnviadoTransferDiaCupom;

            /// <summary>
            /// Natureza posicao atualizada no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaPosicaoAtualizadaDiaCupom;

            /// <summary>
            /// Valor posicao atualizada no dia (cupom)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorPosicaoAtualizadaDiaCupom;

            /// <summary>
            /// Natureza valor de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeLaunch, "", 0)]
            public byte[] NaturezaValorLiquidacao;

            /// <summary>
            /// Valor liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorLiquidacao;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 02 - posicao do mercado a vista
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro02
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorCustodiante;

            /// <summary>
            /// Codigo instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo Origem identificacao instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,3)]
            public byte[] CodBolsaValor;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,5)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Codigo negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,4)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Codigo distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true, 6)]
            public byte[] Distribuicao;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true,7)]
            public byte[] Mercado;

            /// <summary>
            /// Fator de cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] FatorCotacao;

            /// <summary>
            /// Data pregao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true, 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataPregao;

            /// <summary>
            /// Data liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 9)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Codigo da carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true, 10)]
            public byte[] CodigoCarteira;

            /// <summary>
            /// Quantidade comprada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,11)]
            public byte[] QtdeComprada;

            /// <summary>
            /// Volume financeiro comprado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,13)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] VolumeFinanceiroComprado;

            /// <summary>
            /// Preco medio de compra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            [IMBARQExport(true, 12)]
            public byte[] PrecoMedioCompra;

            /// <summary>
            /// Quantidade vendida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 14)]
            public byte[] QtdeVendida;

            /// <summary>
            /// Volume financeiro vendido
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,16)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] VolumeFinanceiroVendido;

            /// <summary>
            /// Preco medio de compra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            [IMBARQExport(true, 15)]
            public byte[] PrecoMedioVenda;

            /// <summary>
            /// Preco medio de compra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            [IMBARQExport(true, 17)]
            public byte[] PosicaoCobertaVendida;

            /// <summary>
            /// Preco medio de compra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PosicaoDescobertaVendida;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 03 - posicao de entrega fisica
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro03
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true, 0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo Origem identificacao instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true, 3)]
            public byte[] CodBolsaValor;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 5)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Codigo negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true, 4)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true, 6)]
            public byte[] Mercado;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 8)]
            public byte[] Mercadoria;

            /// <summary>
            /// Codigo de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true, 9)]
            public byte[] CodigoVencimento;

            /// <summary>
            /// Data de alocacao do aviso de entrega
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataAlocacaoAvisoEntrega;

            /// <summary>
            /// Preco de entrega
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoEntrega;

            /// <summary>
            /// Natureza da posicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaPosicao;

            /// <summary>
            ///Posicao em procedimento de entrega fisica
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] PosicaoProcedimentoEntregaFisica;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 728)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 04 - posicao de recompra
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro04
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true, 0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorCustodiante;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 5)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] Distribuicao;

            /// <summary>
            /// Numero da recompra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroRecompra;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 0)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de encerramento da posicao de recompra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataEncerramentoPosicaoRecompra;

            /// <summary>
            /// Natureza da  Operacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaOperacao;

            /// <summary>
            /// Codigo da carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoCarteira;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeOriginal;

            /// <summary>
            /// Quantidade cancelada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeCancelada;

            /// <summary>
            /// Quantidade executada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeExecutada;

            /// <summary>
            /// Quantidade Revertida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeRevertida;

            /// <summary>
            /// Valor original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorOriginal;

            /// <summary>
            /// Valor cancelado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorCancelado;

            /// <summary>
            /// Valor Executado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorExecutado;

            /// <summary>
            /// Valor Revertido
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorRevertido;

            /// <summary>
            /// Preco medio entrega original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoMedioEntregaOriginal;

            /// <summary>
            /// Preco Reversao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoReversao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 592)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 05 - posicoes de derivativos nao fungiveis
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro05
        {
            /// <summary>
            /// Tipo do registro fixo 00
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true, 0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo Origem identificacao instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true, 3)]
            public byte[] CodBolsaValor;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 5)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Codigo negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true, 4)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true, 6)]
            public byte[] Mercado;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 8)]
            public byte[] Mercadoria;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] IsinAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Codigo de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true, 9)]
            public byte[] CodigoVencimento;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 13)]
            public byte[] DataVencimento;

            /// <summary>
            ///Numero do negocio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroNegocio;

            /// <summary>
            /// Numero do Contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza da operacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaOperacao;

            /// <summary>
            /// Quantidade
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 15)]
            public byte[] Quantidade;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeOriginal;

            /// <summary>
            /// Quantidade de negocios com posicao a vista
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeNegociosPosicaoVista;

            /// <summary>
            /// Quantidade de liquidacao antecipada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeLiquidacaoAntecipada;

            /// <summary>
            /// Quantidade coberta com falha entrega
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeCobertaFalhaEntrega;

            /// <summary>
            /// Quantidade coberta com recompra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeCobertaRecompra;

            /// <summary>
            /// Quantidade coberta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeCoberta;

            /// <summary>
            /// Quantidade descoberta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeDescoberta;

            /// <summary>
            /// Preco do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoContrato;

            /// <summary>
            /// Valor da posicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorPosicao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 501)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 06 - posicoes de aluguel de ativos
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro06
        {
            /// <summary>
            /// Tipo do registro fixo 06
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 3)]
            public byte[] DataMovimento;

            /// <summary>
            /// Codigo instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo Origem identificacao instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodBolsaValor;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,4)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Numero do Contrato Anterior
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,5)]
            public byte[] NumeroContratoAnterior;

            /// <summary>
            /// Natureza / Lado doador / vendedor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,6)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSide, "", 0)]
            public byte[] NaturezaDoadorVendedor;

            /// <summary>
            /// Codigo instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo Origem Identificacao instrumento ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodOrigemIdentInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo Bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,7)]
            public byte[] CodigoBolsaValorAtivoObjeto;

            /// <summary>
            /// ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,8)]
            public byte[] IsinAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,9)]
            public byte[] DistribuicaoAtivoObjeto;
            
            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Mercado;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,10)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 11)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 12)]
            public byte[] DataVencimento;

            /// <summary>
            /// Data de carencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 13)]
            public byte[] DataCarencia;

            /// <summary>
            /// Código da carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,14)]
            public byte[] CodigoCarteira;

            /// <summary>
            /// Preco referencia  do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoReferenciaAtivoObjeto;

            /// <summary>
            /// Fator Cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] FatorCotacao;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,16)]
            public byte[] QuantidadeOriginal;

            /// <summary>
            /// Quantidade em liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeLiquidacao;
            
            /// <summary>
            /// Quantidade total de titulos liquidados
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] QtdeTotalTitulosLiquidados;

            /// <summary>
            /// Quantidade coberta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,18)]
            public byte[] QtdeCoberta;

            /// <summary>
            /// Quantidade descoberta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,19)]
            public byte[] QtdeDescoberta;

            /// <summary>
            /// Quantidade Renovada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] QtdeRenovada;

            /// <summary>
            /// Quantidade atual
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,21)]
            public byte[] QtdeAtual;

            /// <summary>
            /// Volume
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] Volume;

            /// <summary>
            /// Tipo de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true, 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeSettlementType, "", 0)]
            public byte[] TipoLiquidacao;

            /// <summary>
            /// Indicador de liquidacao antecipada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,24)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeYesOrNo, "", 0)]
            public byte[] IndicadorLiquidacaoAntecipada;

            /// <summary>
            /// Indicador de liquidacao antecipada em caso de OPA
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,25)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeYesOrNo, "", 0)]
            public byte[] IndicadorLiquidacaoAntecipadaOPA;

            /// <summary>
            /// Taxa doadora ou tomadora
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQExport(true,26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] TaxaDoadoraTomadora;

            /// <summary>
            /// Partipante executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] ParticipanteExecutor;

            /// <summary>
            /// Investidor do partipante executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorParticipanteExecutor;

            /// <summary>
            /// Contraparte tributada JCP
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] ContraparteTributadaJCP;

            /// <summary>
            /// Contraparte tributada rendimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] ContraparteTributadaRendimento;

            /// <summary>
            /// Tipo de contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,27)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeContractType, "", 0)]
            public byte[] TipoContrato;

            /// <summary>
            /// Numero da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            [IMBARQExport(true,28)]
            public byte[] NumeroInstrucaoLiquidacao;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 268)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 07 - termo
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro07
        {
            /// <summary>
            /// Tipo do registro fixo 07
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,3)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 4)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de registro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 5)]
            public byte[] DataRegistro;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            [IMBARQExport(true, 6)]
            public byte[] DataVencimento;

            /// <summary>
            /// Valor da cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            [IMBARQExport(true,7)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorCotacao;

            /// <summary>
            /// Codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,8)]
            public byte[] CodVariavel;

            /// <summary>
            /// Descricao do codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            [IMBARQExport(true,9)]
            public byte[] DescricaoCodVariavel;

            /// <summary>
            /// Codigo do tipo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,10)]
            public byte[] CodTipoVariavel;

            /// <summary>
            /// Descricao do codigo do tipo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            [IMBARQExport(true,11)]
            public byte[] DescricaoCodTipoVariavel;

            /// <summary>
            /// Tamanho base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQExport(true,12)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] TamanhoBase;

            /// <summary>
            /// Natureza da posicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,13)]
            public byte[] NaturezaPosicao;

            /// <summary>
            /// Indicador de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,14)]
            public byte[] IndicadorGarantias;

            /// <summary>
            /// Codigo do negociador contraparte
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,15)]
            public byte[] CodNegociadorContraparte;

            /// <summary>
            /// Indicador de garantias da contraparte
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,16)]
            public byte[] IndicadorGarantiasContraparte;

            /// <summary>
            /// Codigo do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            [IMBARQExport(true,17)]
            public byte[] CodigoContrato;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 682)]
            public byte[] Filler;
        }
    
        /// <summary>
        /// Registro 08 - opcoes flexiveis
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro08
        {
            /// <summary>
            /// Tipo do registro fixo 08
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de registro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataRegistro;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataVencimento;

            /// <summary>
            /// Codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodVariavel;

            /// <summary>
            /// Descricao do codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoCodVariavel;

            /// <summary>
            /// Codigo do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] CodigoVContrato;

            /// <summary>
            /// Descricao do codigo do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoCodigoContrato;

            /// <summary>
            /// Codigo do tipo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] CodTipoVariavel;

            /// <summary>
            /// Codigo do tipo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoTipoVariavel;

            /// <summary>
            /// Tipo da opcao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoOpcao;

            /// <summary>
            /// Preco de exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoExercicio;

            /// <summary>
            /// Descricao do preco de exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoPrecoExercicio;

            /// <summary>
            /// Tamanho base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] TamanhoBase;

            /// <summary>
            /// Titular ou lancador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TitularLancador;

            /// <summary>
            /// Codigo do negociador contraparte
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoNegociadorContraparte;

            /// <summary>
            /// Premio Unitario
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PremioUnitario;

            /// <summary>
            /// Indicador de barreiras
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorBarreiras;

            /// <summary>
            /// Tipo de preco de exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoPrecoExercicio;

            /// <summary>
            /// Indicador de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorGarantias;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 532)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 09 - barreiras de opcoes flexiveis
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro09
        {
            /// <summary>
            /// Tipo do registro fixo 09
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Titular / Lancador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TitularLancador;

            /// <summary>
            /// Codigo da barreira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] CodBarreira;

            /// <summary>
            /// Descricao da barreira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescBarreira;

            /// <summary>
            /// Preco da barreira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoBarreira;

            /// <summary>
            /// Data de acionamento da barreira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataAcionamentoBarreira;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 805)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 10 - swap
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro10
        {
            /// <summary>
            /// Tipo do registro fixo 10
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Data de registro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataRegistro;

            /// <summary>
            /// Data base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataBase;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataVencimento;

            /// <summary>
            /// Codigo do Tipo da Variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] CodigoTipoVariavel;

            /// <summary>
            /// Descricao do tipo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoTipoVariavel;

            /// <summary>
            /// Codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoVariavel;

            /// <summary>
            /// Descricao da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoVariavel;

            /// <summary>
            /// Percentual da variaval
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PercentualVariavel;

            /// <summary>
            /// Tamanho Base ponta ativa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] TamanhoBasePontaAtiva;

            /// <summary>
            /// Valor atual ponta ativa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAtualPontaAtiva;

            /// <summary>
            /// ValorAtual da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorAtualPontaPassiva;

            /// <summary>
            /// codigo do tipo da variaval  da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] CodigoTipoVariavelPontaPassiva;

            /// <summary>
            /// Descricao do tipo da variavel  da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoTipoVariavelPontaPassiva;

            /// <summary>
            /// Codigo da variavel da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoVariavelPontaPassiva;

            /// <summary>
            /// Descricao da variavel da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoVariavelPontaPassiva;

            /// <summary>
            /// Percentual da variavel  da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PercentualVariavelPontaPassiva;

            /// <summary>
            /// Codigo do participante contraparte ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoParticipanteContraPartePontaPassiva;

            /// <summary>
            /// Sinal
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] Sinal;

            /// <summary>
            /// Valor dos juros
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorJuros;

            /// <summary>
            /// Indicador de garantias da ponta ativa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorGarantiasPontaAtiva;

            /// <summary>
            /// Indicador de garantias da ponta passiva
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorGarantiasPontaPassiva;

            /// <summary>
            /// Sinal MTM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] SinalMTM;

            /// <summary>
            /// MtM Fair Value
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] MTMFairValue;

            /// <summary>
            /// Codigo do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] CodigoContrato;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 467)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 11 - cesta de acoes do swap SB1 e SB2 - balcao
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro11
        {
            /// <summary>
            /// Tipo do registro fixo 11
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Codigo da variavel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoVariavel;

            /// <summary>
            /// Descricao do codigo da variavel 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescricaoCodigoVariavel;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Percentual de participacao da acao na carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] PercentualParticipacaoAcaoCarteira;

            /// <summary>
            /// Quantidade atualizada da acao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            public byte[] QuantidadeAtualizadaAcao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 785)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 12 - Liquidacao na data de efetivacao informada (movimento do dia)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro12
        {
            /// <summary>
            /// Tipo do registro fixo 12
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo do participante por conta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoParticipanteConta;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] Mercadoria;

            /// <summary>
            /// ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Data de referencia do movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataReferenciaMovimento;

            /// <summary>
            /// Data de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 8)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Codigo do lancamento financeiro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] CodigoLancamentoFinanceiro;

            /// <summary>
            /// Descricao do codigo de lancamento financeiro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescCodigoLancamentoFinanceiro;

            /// <summary>
            /// Tipo de lancamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoLancamento;

            /// <summary>
            /// Valor financeiro do lancamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorFinanceiroLancamento;

            /// <summary>
            /// Quantidade
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] Quantidade;

            /// <summary>
            /// Preco de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] PrecoNegociacao;

            /// <summary>
            /// Valor do imposto de renda
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorImpostoRenda;

            /// <summary>
            /// Numero do pagamento compensado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] NumeroPagamentoCompensado;

            /// <summary>
            /// Numero de contrato de emprestimo de ativos
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            public byte[] NumeroContratoEmprestimoAtivos;

            /// <summary>
            /// ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] IsinAtivoObjeto;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 674)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 13 - Lancamentos para efetivacao futura
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro13
        {
            /// <summary>
            /// Tipo do registro fixo 13
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo do participante por conta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoParticipanteConta;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] Mercadoria;

            /// <summary>
            /// ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Data de referencia do movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataReferenciaMovimento;

            /// <summary>
            /// Data de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Codigo do lancamento financeiro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] CodigoLancamentoFinanceiro;

            /// <summary>
            /// Descricao do codigo de lancamento financeiro
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] DescCodigoLancamentoFinanceiro;

            /// <summary>
            /// Tipo de lancamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoLancamento;

            /// <summary>
            /// Valor financeiro do lancamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorFinanceiroLancamento;

            /// <summary>
            /// Quantidade
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] Quantidade;

            /// <summary>
            /// Preco de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] PrecoNegociacao;

            /// <summary>
            /// Valor do imposto de renda
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorImpostoRenda;

            /// <summary>
            /// Numero do pagamento compensado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] NumeroPagamentoCompensado;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 707)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 14 - Informacao das liquidacoes e renovacoes de contratos de termo e aluguel de ativos
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro14
        {
            /// <summary>
            /// Tipo do registro fixo 14
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Numero do negocio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] NumeroNegocio;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeOriginal;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza da posicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaPosicao;

            /// <summary>
            /// Codigo do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo de origem de identificacao do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigIdentInstrumento;

            /// <summary>
            /// Codigo Bolsa Valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValor;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Mercado;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] Mercadoria;

            /// <summary>
            /// ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Distribuicao;

            /// <summary>
            /// Codigo do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo de origem da identificacao do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigIdentInstrAtivoObjeto;

            /// <summary>
            /// Codigo bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValorAtivoObjeto;

            /// <summary>
            /// Codigo de negociacao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISINAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Fator de cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] FatorCotacao;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] Carteira;

            /// <summary>
            /// Request ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] RequestID;

            /// <summary>
            /// Tipo de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoLiquidacao;

            /// <summary>
            /// Quantidade liquidada antecipadamente
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeLiquidadaAntecipadamente;

            /// <summary>
            /// Quantidade vencida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] QtdeVencida;

            /// <summary>
            /// Data da liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Data de requisicao da liquidacao antecipada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataReqLiquidacaoAntecipada;

            /// <summary>
            /// Status
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Status;

            /// <summary>
            /// Valor da liquidacai
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorLiquidacao;

            /// <summary>
            /// Preco do Termo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoTermo;

            /// <summary>
            /// Codigo de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoVencimento;

            /// <summary>
            /// Tipo de termo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoTermo;

            /// <summary>
            /// Tipo de contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoContrato;

            /// <summary>
            /// Ultimo dia possivel para liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] UltimoDiaPossivelLiquidacao;

            /// <summary>
            /// Taxa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] Taxa;

            /// <summary>
            /// Participante executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoParticipanteExecutor;

            /// <summary>
            /// Investidor do participante executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoInvestidorParticipanteExecutor;

            /// <summary>
            /// Indicador de anonimo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorAnonimo;

            /// <summary>
            /// Dias uteis entre abertuda do contrato e liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] DiasUteisAbertContratoLiquidacao;

            /// <summary>
            /// Valor pago pelo/para o investidor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorPagoPeloParaInvestidor;

            /// <summary>
            /// Preco de referencia do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoReferenciaAtivoObjeto;

            /// <summary>
            /// taxa doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] TaxaDoador;

            /// <summary>
            /// Imposto sobre o valor bruto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ImpostoSobreValorBruto;

            /// <summary>
            /// Valor bruto do doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorBrutoDoador;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 121)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 15 - Liquidacoes antecipadas de contratos de termo e aluguel de ativos
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro15
        {
            /// <summary>
            /// Tipo do registro fixo 15
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataNegociacao;

            /// <summary>
            /// Numero do negocio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] NumeroNegocio;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeOriginal;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza da posicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaPosicao;

            /// <summary>
            /// Codigo do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo de origem de identificacao do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigIdentInstrumento;

            /// <summary>
            /// Codigo Bolsa Valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValor;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Mercado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Mercado;

            /// <summary>
            /// Mercadoria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public byte[] Mercadoria;

            /// <summary>
            /// ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Distribuicao;

            /// <summary>
            /// Codigo do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo de origem da identificacao do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigIdentInstrAtivoObjeto;

            /// <summary>
            /// Codigo bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValorAtivoObjeto;

            /// <summary>
            /// Codigo de negociacao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISINAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Fator de cotacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] FatorCotacao;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] Carteira;

            /// <summary>
            /// Request ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] RequestID;

            /// <summary>
            /// Tipo de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] TipoLiquidacao;

            /// <summary>
            /// Quantidade liquidada antecipadamente
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeLiquidadaAntecipadamente;

            /// <summary>
            /// Quantidade Disponivel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeDisponivel;

            /// <summary>
            /// Dia da liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Data da requisicao da liquidacao antecipada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] DataRequisicaoLiqAntecipada;

            /// <summary>
            /// Valor de liquidacao antecipada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorLiquidacaoAntecipada;

            /// <summary>
            /// Preco do termo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoTermo;

            /// <summary>
            /// Codigo de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoVencimento;

            /// <summary>
            /// Tipo do termo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] TipoTermo;

            /// <summary>
            /// Indicador de cancelado ao final do dia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] IndicadorCanceladoFinalDia;

            /// <summary>
            /// Tipo de contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] TipoContrato;

            /// <summary>
            /// Ultimo dia possivel liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] UltimoDiaPossivelLiquidacao;

            /// <summary>
            /// Taxa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] Taxa;

            /// <summary>
            /// Participante Executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] ParticipanteExecutor;

            /// <summary>
            /// Investidor do participante executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorParticipanteExecutor;

            /// <summary>
            /// Preco de referencia do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoReferenciaAtivoObjeto;

            /// <summary>
            /// Codigo do participante contraparte
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoParticipanteContraparte;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 16 - Liquidacoes antecipadas de contratos de termo e aluguel de ativos
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro16
        {
            /// <summary>
            /// Tipo do registro fixo 16
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data do pregao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataPregao;

            /// <summary>
            /// Data de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Tipo de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] TipoLiquidacao;

            /// <summary>
            /// Numero de instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] NumeroInstrucaoLiquidacao;

            /// <summary>
            /// Numero de instrucao de liquidacao original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] NumeroInstrucaoLiquidacaoOriginal;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Carteira;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// DistribuicaoISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoISIN;

            /// <summary>
            /// Natureza da operacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaOperacao;

            /// <summary>
            /// Quantidade Total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeTotalInstrucaoLiquidacao;

            /// <summary>
            /// Preco medio de referencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoMedioReferencia;

            /// <summary>
            /// Volume total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeTotalInstrucaoLiquidacao;

            /// <summary>
            /// Quantidade Liquidada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeLiquidada;

            /// <summary>
            /// Qtde nao liquidada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeNaoLiquidada;

            /// <summary>
            /// Quantidade a liquidar
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeParaSerLiquidada;

            /// <summary>
            /// Quantidade restringivel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeRestringivel;

            /// <summary>
            /// Quantidade da falha
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeFalha;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 579)]
            public byte[] Reservado;
        }

        /// <summary>
        /// Registro 17 - Liquidacoes antecipadas de contratos de termo e aluguel de ativos
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro17
        {
            /// <summary>
            /// Tipo do registro fixo 17
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data do pregao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataPregao;

            /// <summary>
            /// Data de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Data de movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataMovimento;

            /// <summary>
            /// Numero de instrucao da liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] NumeroInstrucaoLiquidacao;

            /// <summary>
            /// Numero de instrucao da liquidacao original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] NumeroInstrucaoLiquidacaoOriginal;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Carteira;

            /// <summary>
            /// ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DistribuicaoISIN;

            /// <summary>
            /// Natureza da operacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaOperacao;

            /// <summary>
            /// Quantidade total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeTotalInstrucaoLiquidacao;

            /// <summary>
            /// Preco medio de referencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoMedioReferencia;

            /// <summary>
            /// Volume total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeTotalInstrucaoLiquidacao;

            /// <summary>
            /// Quantidade aceita
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 4)]
            public byte[] QtdeAceita;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 655)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 18 - Garantias aportadas
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro18
        {
            /// <summary>
            /// Tipo do registro fixo 18
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de referencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,3)]
            public byte[] DataReferencia;

            /// <summary>
            /// Codigo do tipo da garantia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,4)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeCollateralType, "", 10)]
            public byte[] CodigoTipoGarantia;

            /// <summary>
            /// Codigo do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            [IMBARQExport(true,5)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo de origem de identificacao do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValor;

            /// <summary>
            /// Finalidade de cobertura
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,6)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeCollateralCoverage, "", 10)]
            public byte[] FinalidadeCobertura;

            /// <summary>
            /// CodigoCategoriaTitular
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            [IMBARQExport(true,7)]
            public byte[] CodigoCategoriaTitular;

            /// <summary>
            /// Identificador de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true,8)]
            public byte[] IdentificadorGarantias;

            /// <summary>
            /// Identificador de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true,9)]
            public byte[] NumeroGarantia;

            /// <summary>
            /// Identificador de coberturas
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,10)]
            public byte[] IdentificadorCoberturas;

            /// <summary>
            /// Identificador de titularidade
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true,11)]
            public byte[] IdentificadorTitularidade;

            /// <summary>
            /// Data vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,12)]
            public byte[] DataVencimento;

            /// <summary>
            /// QuantidadeDepositada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            [IMBARQExport(true,13)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeDepositada;

            /// <summary>
            /// Quantidade aceita
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            [IMBARQExport(true,13)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeAceita;

            /// <summary>
            /// Qunatidade valorizada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true,15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QuantidadeValorizada;

            /// <summary>
            /// Quantidade Distribuida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true,16)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeDistribuida;

            /// <summary>
            /// Quantidade Bloqueada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true,17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeBloqueada;

            /// <summary>
            /// CodigoISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,18)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,19)]
            public byte[] Carteira;

            /// <summary>
            /// Valor Garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            [IMBARQExport(true,20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorGarantias;

            /// <summary>
            /// Valor Bloqueado para retirada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            [IMBARQExport(true,21)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] ValorBloqueadoRetirada;

            /// <summary>
            /// Codigo do custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,22)]
            public byte[] CodigoCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,23)]
            public byte[] CodigoInvestidorCustodiante;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 584)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 31 - Garantias aportadas
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro31
        {
            /// <summary>
            /// Tipo do registro fixo 31
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de referencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataReferencia;

            /// <summary>
            /// Codigo do tipo da garantia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoTipoGarantia;
            
            /// <summary>
            /// Codigo do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumento;

            /// <summary>
            /// Codigo da origem de indentificacao do instrumento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigemIdentInstrumento;

            /// <summary>
            /// Codigo bolsa valor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValor;

            /// <summary>
            /// Finalidade de cobertura
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] FinalidadeCobertura;

            /// <summary>
            /// Codigo de categoria titular
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoCategoriaTitular;

            /// <summary>
            /// Identificador de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] IdentificadorGarantias;

            /// <summary>
            /// Numero da garantia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] NumeroGarantia;

            /// <summary>
            /// Identificador de cubertura 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IdentificadorCobertura;

            /// <summary>
            /// Indicador de titularidade
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorTitularidade;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Quantidade Depositada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeDepositada;

            /// <summary>
            /// Quantidade Aceita
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeAceita;

            /// <summary>
            /// Quantidade valorizada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeValorizada;

            /// <summary>
            /// Quantidade distribuida
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeDistribuida;

            /// <summary>
            /// Quantidade bloqueada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] QtdeBloqueada;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Carteira;

            /// <summary>
            /// Valor Garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorGarantias;

            /// <summary>
            /// Valor Bloqueado para retirada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] ValorBloqueadoRetirada;

            /// <summary>
            /// Codigo do participante de negociacao pleno ou
            /// Participante de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipNegPlenoOuLiquidacao;

            /// <summary>
            /// Codigo do investidor no participante de negociacao pleno ou
            /// Participante de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestPartNegPlenoOuLiquidacao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 584)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 19 - Margem Requerida
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro19
        {
            /// <summary>
            /// Tipo do registro fixo 19
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Data de movimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataMovimento;

            /// <summary>
            /// Finalidade de cobertura
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] FinalidadeCobertura;

            /// <summary>
            /// SinalRiscoSemGarantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] SinalRiscoSemGarantias;

            /// <summary>
            /// Risco sem garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] RiscoSemGarantias;

            /// <summary>
            /// Chamada de margem inicial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ChamadaMargemInicial;

            /// <summary>
            /// Valor de margem adicional
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] ValorMargemAdicional;

            /// <summary>
            /// Valor total das garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] ValorTotalGarantias;

            /// <summary>
            /// Perdas permanentes
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PerdasPermanentes;

            /// <summary>
            /// Perdas transitorias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PerdasTransitorias;

            /// <summary>
            /// SinalSaldoGarantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] SinalSaldoGarantias;

            /// <summary>
            /// Saldo de garantias
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] SaldoGarantias;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 768)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 20 - Ofertas do book (aluguel de ativos)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro20
        {
            /// <summary>
            /// Tipo do registro fixo 20
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero da Oferta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] NumeroOferta;

            /// <summary>
            /// Situacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] SituacaoOferta;

            /// <summary>
            /// Tipo da Oferta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoOferta;

            /// <summary>
            /// Data de criacao da oferta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataCriacaoOferta;

            /// <summary>
            /// Participante doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ParticipanteDoador;

            /// <summary>
            /// Investidor no participante doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorParticipanteDoador;

            /// <summary>
            /// Custodiante doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] CustodianteDoador;

            /// <summary>
            /// Participante tomador executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] PartTomadorExecutor;

            /// <summary>
            /// Investidor no participante tomador executor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorPartTomadorExecutor;

            /// <summary>
            /// Custodiante tomador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] CustodianteTomador;

            /// <summary>
            /// Investidor no custodiante tomador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorCustodianteTomador;

            /// <summary>
            /// Participante Carrying
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ParticipanteCarrying;

            /// <summary>
            /// Investidor no participante carrying
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] InvestidorParticipanteCarrying;

            /// <summary>
            /// Codigo do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo de origem da identificacao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigemIdentInstAtivoObjeto;

            /// <summary>
            /// Codigo bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValorAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjeto;

            /// <summary>
            /// Codigo de distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoDistribuicaoAtivoObjeto;

            /// <summary>
            /// Codigo de negociacao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoNegociacaoAtivoObjeto;

            /// <summary>
            /// Quantidade original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] QtdeOriginal;

            /// <summary>
            /// Quantidade disponivel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public byte[] QtdeDisponivel;

            /// <summary>
            /// Carteira doador ou tomador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] CarteiraDoadorTomador;

            /// <summary>
            /// Taxa
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 5)]
            public byte[] Taxa;

            /// <summary>
            /// Data de carencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataCarencia;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Indicador reversivel ao doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorReversivelDoador;

            /// <summary>
            /// Indicador reversivel ao doador em caso de OPA
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorReversivelDoadorOPA;

            /// <summary>
            /// Indicador de contrato diferenciado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorContratoDiferenciado;

            /// <summary>
            /// Participante para fechar
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ParticipanteFechar;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 609)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 21 - Ofertas prioritarias (aluguel de ativos)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro21
        {
            /// <summary>
            /// Tipo do registro fixo 21
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Codigo do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Codigo de origem de identificacao do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigemIdentInstrAtivoObjeto;

            /// <summary>
            /// Codigo bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigoBolsaValorAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo Objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objetco
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// data de liquidacao da oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLiquidacaoOfertaPrioritaria;

            /// <summary>
            /// Ultimo dia pra participacao da oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] UltimoDiaParticipacaoOfertaPrioritaria;

            /// <summary>
            /// Quantidade direitos da oferta prioritaria requisitados
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosOfertaPrioritariaRequisitados;

            /// <summary>
            /// Quantidade na data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDataAtualizacaoCustodia;

            /// <summary>
            /// Quantidade de direitos da oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosOfertaPrioritaria;

            /// <summary>
            /// Preco da oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] PrecoOfertaPrioritaria;

            /// <summary>
            /// Valor financeiro da oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorFinanceiroOfertaPrioritaria;

            /// <summary>
            /// Data de inicio para participacao na oferta prioritaria
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataInicioParticipacaoOfertaPrioritaria;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 617)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 22 - SubscricoreOfertas prioritarias (aluguel de ativos)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro22
        {
            /// <summary>
            /// Tipo do registro fixo 22
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza / Lado doador / vendedor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaDoadorVendedor;

            /// <summary>
            /// CodigoCustodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoInvestidorCustodiante;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] Carteira;

            /// <summary>
            /// Data de vencimento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataVencimento;

            /// <summary>
            /// Codigo Instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoInstrumentoAtivoObjeto;

            /// <summary>
            /// Coidgo de origem identificacao do instrumento do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoOrigemIdentInstrAtivoObjeto;

            /// <summary>
            /// Codigo bolsa valor do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CodigBolsaValorAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjeto;

            /// <summary>
            /// Distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataAtualizacaoCustodia;

            /// <summary>
            /// Quantidade na data atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDataAtualizacaoCustodia;

            /// <summary>
            /// Fator da subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] FatorSubscricao;

            /// <summary>
            /// Qtde de direitos de subscricao na data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosSubscricaoDataAtualCustodia;

            /// <summary>
            /// Data inicial de solicitacao de direitos de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataInicialSolicitDireitosSubscricao;

            /// <summary>
            /// Data limite de solicitacao de direitos de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataLimiteSolicitDireitosSubscricao;

            /// <summary>
            /// Data de precificacao dos direitos de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataPrecificacaoDireitosSubscricao;

            /// <summary>
            /// Data de devolucao dos direitos de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataDevolucaoDireitosSubscricao;

            /// <summary>
            /// Quantidade solicitada de direitos de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeSolicitadaDireitosSubscricao;

            /// <summary>
            /// Quantidade de direitos de subscricao devolvidos ao doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosSubscricaoDevolvidosDoador;

            /// <summary>
            /// Quantidade de direitos de subscricao NAO devolvidos ao doador
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosSubscricaoNAODevolvidosDoador;

            /// <summary>
            /// Codigo ISIN do direito de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinDireitoSubscricao;

            /// <summary>
            /// Codigo de Distribuicao do direito de susbscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoDireitoSubscricao;

            /// <summary>
            /// Codigo de negociacao do direito de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoDireitoSubscricao;

            /// <summary>
            /// Preco por ativo da subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] PrecoAtivoSubscricao;

            /// <summary>
            /// Data solicitacao contratos filhotes
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataSolicitacaoContratosFilhote;

            /// <summary>
            /// Quantidade solicitada para criacao de contratos filhote
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeSolicitadaCriacaoContratosFilhote;

            /// <summary>
            /// Quantidade de direitos para liquidacao financeira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDireitosLiquidacaoFinanceira;

            /// <summary>
            /// ISIN do Recibo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinRecibo;

            /// <summary>
            /// Codigo de distribuicao do recibo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoRecibo;

            /// <summary>
            /// Codigo negociacao recibo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoRecibo;

            /// <summary>
            /// Preco a ser pago para subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] PrecoPagoSubscricao;

            /// <summary>
            /// Data de efetivacao da subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataEfetivacaoSubscricao;

            /// <summary>
            /// Valor financeiro da subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorFinanceiroSubscricao;

            /// <summary>
            /// Opcao para subscricao parcial
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] OpcaoSubscricaoParcial;

            /// <summary>
            /// Opcao para participacao em sobras
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] OpcaoParticipacaoSobras;

            /// <summary>
            /// Numero do contrato filhote
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContratoFilhote;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 180)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 23 - Eventos em dinheiro
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro23
        {
            /// <summary>
            /// Tipo do registro fixo 23
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo do participante por conta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteConta;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza lado doador/vendedor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaDoadorVendedor;

            /// <summary>
            /// Codigo do custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorCustodiante;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] Carteira;

            /// <summary>
            /// Codigo de negociacao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodNegociacaoAtivoObjeto;

            /// <summary>
            /// Codigo ISIN do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjeto;

            /// <summary>
            /// Codigo de distribuicao do ativo objeto
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoAtivoObjeto;

            /// <summary>
            /// Codigo do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public byte[] CodEventoCorporativo;

            /// <summary>
            /// Data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataAtualizacaoCustodia;

            /// <summary>
            /// Quantidade na data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDataAtualCustodia;

            /// <summary>
            /// Valor bruto do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorBrutoEventoCorporativo;

            /// <summary>
            /// Valor do imposto do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorImpostoEventoCorporativo;

            /// <summary>
            /// Valor liquido do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorLiquidoEventoCorporativo;

            /// <summary>
            /// Data de pagamento do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataPgtoEventoCorporativo;

            /// <summary>
            /// Situacao do evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] SituacaoEventoCorporativo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 575)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 24 - Eventos em acoes
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro24
        {
            /// <summary>
            /// Tipo do registro fixo 24
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Codigo do participante por conta
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteConta;

            /// <summary>
            /// Tipo de provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] TipoProvento;

            /// <summary>
            /// Numero do contrato
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContrato;

            /// <summary>
            /// Natureza / lado doador /vendedor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] NaturezaDoadorVendedor;

            /// <summary>
            /// Codigo do custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoCustodiante;

            /// <summary>
            /// Codigo do investidor no custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodigoInvestidorCustodiante;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] Carteira;

            /// <summary>
            /// Codigo de negociacao do atibo objeto EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoAtivoObjetoEX;

            /// <summary>
            /// Codigo ISIN do ativo objeto EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjetoEX;

            /// <summary>
            /// Codigo de distribuicao do ativo objeto EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoAtivoObjetoEX;

            /// <summary>
            /// Qunatidade na data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeDataAtualCustodia;

            /// <summary>
            /// Preco de referencia EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] PrecoReferenciaEX;

            /// <summary>
            /// Valor do contrato EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorContratoEX;

            /// <summary>
            /// Numero do contrato apos evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] NumeroContratoPosEvento;

            /// <summary>
            /// Codigo de negociacao do atibo objeto COM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
            public byte[] CodigoNegociacaoAtivoObjetoCOM;

            /// <summary>
            /// Codigo ISIN do ativo objeto COM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinAtivoObjetoCOM;

            /// <summary>
            /// Codigo de distribuicao do ativo objeto EX
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] DistribuicaoAtivoObjetoCOM;

            /// <summary>
            /// Quantidade atual COM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QuantidadeAtualCOM;

            /// <summary>
            /// Preco referencia COM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] PrecoReferenciaCOM;

            /// <summary>
            /// Valor do contrato COM
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] ValorContratoCOM;

            /// <summary>
            /// Quantidade de fracoes apos evento corporativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 11)]
            public byte[] QtdeFracoesPosEvento;

            /// <summary>
            /// Data de atualizacao da custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] DataAtualizacaoCustodia;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 458)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 25 - Subscricao
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro25
        {
            /// <summary>
            /// Tipo do registro fixo 25
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] Carteira;

            /// <summary>
            /// Descricao da Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] DescricaoCarteira;

            /// <summary>
            /// Codigo ISIN da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinBase;

            /// <summary>
            /// Distribuicao da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] DistribuicaoBase;

            /// <summary>
            /// Nome da sociedade emissora (base)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeSociedadeEmissoraBase;

            /// <summary>
            /// Especificacao da acao base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] EspecificacaoAcaoBase;

            /// <summary>
            /// Quantidade de acoes base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 3)]
            public byte[] QtdeAcoesBase;

            /// <summary>
            /// Data agendamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataAgendamento;

            /// <summary>
            /// Data de inicio do provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataInicioProvento;

            /// <summary>
            /// Numero do processo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] NumeroProcesso;

            /// <summary>
            /// Codigo ISIN do direito de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinDireitoSubscricao;

            /// <summary>
            /// Distribuicao do direito de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] DistribuicaoDireitoSubscricao;

            /// <summary>
            /// Nome da sociedade emissora
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeSociedadeEmissora;

            /// <summary>
            /// Especificacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Especificacao;

            /// <summary>
            /// Quantidade de direitos disponiveis de subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeDireitosDisponiveisSubscricao;

            /// <summary>
            /// Quantidade de direitos exercidos a vista
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeDireitosExercidosVista;

            /// <summary>
            /// Quantidade de direitos exercidos a prazo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeDireitosExercidosPrazo;

            /// <summary>
            /// Data de vencimento na sociedade emissora
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] DataVencimentoSociedadeEmissora;

            /// <summary>
            /// Preco da subscricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 10)]
            public byte[] PrecoSubscricao;

            /// <summary>
            /// Fator de cotacao de preco
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] FatorCotacaoPreco;
            
            /// <summary>
            /// Quantidade de parcelas
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] QtdeParcelas;

            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 721)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 26 - Dividendos - central depositaria
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro26
        {
            /// <summary>
            /// Tipo do registro fixo 26
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true, 0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true, 3)]
            public byte[] Carteira;

            /// <summary>
            /// Descricao da Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true, 4)]
            public byte[] DescricaoCarteira;

            /// <summary>
            /// Codigo ISIN da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 5)]
            public byte[] CodigoIsinBase;

            /// <summary>
            /// Distribuicao da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true, 6)]
            public byte[] DistribuicaoBase;

            /// <summary>
            /// Nome da sociedade emissora (base)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 7)]
            public byte[] NomeSociedadeEmissoraBase;

            /// <summary>
            /// Especificacao da acao base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true, 8)]
            public byte[] EspecificacaoAcaoBase;

            /// <summary>
            /// Quantidade de acoes base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 3)]
            [IMBARQExport(true, 9)]
            public byte[] QtdeAcoesBase;

            /// <summary>
            /// Data agendamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 3)]
            [IMBARQExport(true, 10)]
            public byte[] DataAgendamento;

            /// <summary>
            /// Data de inicio do provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataInicioProvento;

            /// <summary>
            /// Numero do processo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            [IMBARQExport(true, 11)]
            public byte[] NumeroProcesso;

            /// <summary>
            /// Ano de referencia do exercicio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] AnoReferenciaExercicio;

            /// <summary>
            /// data prevista para pagamento do dividendo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            [IMBARQExport(true, 13)]
            public byte[] DataPrevistaPagamentoDividendo;

            /// <summary>
            /// Valor bruto do dividendo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            [IMBARQExport(true, 14)]
            public byte[] ValorBrutoDividendo;

            /// <summary>
            /// Percentual do imposto de renda
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            [IMBARQExport(true, 15)]
            public byte[] PercentualImpostoRenda;

            /// <summary>
            /// Codigo do imposto de renda
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] CodigoImpostoRenda;

            /// <summary>
            /// Instituicao pagadora
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true, 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypePayingInstitution, "", 0)]
            public byte[] InstituicaoPagadora;

            /// <summary>
            /// Tipo de provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true, 18)]
            public byte[] TipoProvento;

            /// <summary>
            /// Descricao do provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            [IMBARQExport(true, 19)]
            public byte[] DescricaoProvento;

            /// <summary>
            /// data de aquisicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataAquisicao;

            /// <summary>
            /// Preco da aquisicao 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 2)]
            public byte[] PrecoAquisicao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 754)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 27 - Bonificacao - central depositaria
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro27
        {
            /// <summary>
            /// Tipo do registro fixo 27
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] Carteira;

            /// <summary>
            /// Descricao da Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] DescricaoCarteira;

            /// <summary>
            /// Codigo ISIN da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinBase;

            /// <summary>
            /// Distribuicao da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] DistribuicaoBase;

            /// <summary>
            /// Nome da sociedade emissora (base)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeSociedadeEmissoraBase;

            /// <summary>
            /// Especificacao da acao base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] EspecificacaoAcaoBase;

            /// <summary>
            /// Quantidade de acoes base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 3)]
            public byte[] QtdeAcoesBase;

            /// <summary>
            /// Data agendamento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataAgendamento;

            /// <summary>
            /// Data de inicio do provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataInicioProvento;

            /// <summary>
            /// Numero do processo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] NumeroProcesso;

            /// <summary>
            /// Codigo ISIN da bonificacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoIsinBonificacao;

            /// <summary>
            /// Distribuicao da bonificacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] DistribuicaoBonificacao;

            /// <summary>
            /// Nome da sociedade emissora
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeSociedadeEmissora;

            /// <summary>
            /// EspecificacaoBonificacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] EspecificacaoBonificacao;

            /// <summary>
            /// Quantidade de acoes previstas
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QuantidadeAcoesPrevistas;

            /// <summary>
            /// Tipo do provento
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] TipoProvento;

            /// <summary>
            /// Descricao dos proventos
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] DescricaoProventos;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 762)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 28 - Saldo de custodia - central depositaria
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro28
        {
            /// <summary>
            /// Tipo do registro fixo 28
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true,3)]
            public byte[] Carteira;

            /// <summary>
            /// Descricao da Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,4)]
            public byte[] DescricaoCarteira;

            /// <summary>
            /// Codigo ISIN da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,5)]
            public byte[] CodigoIsinBase;

            /// <summary>
            /// Distribuicao da base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            [IMBARQExport(true,6)]
            public byte[] DistribuicaoBase;

            /// <summary>
            /// Nome da sociedade emissora (base)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeSociedadeEmissoraBase;

            /// <summary>
            /// Especificacao da acao base
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true,8)]
            public byte[] EspecificacaoAcaoBase;

            /// <summary>
            /// Quantidade de acoes em custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQExport(true,9)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 3)]
            public byte[] QtdeAcoesCustodia;

            /// <summary>
            /// Quantidade total de acoes bloqueadas
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,10)]
            public byte[] QtdeTotalAcoesBloqueadas;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,7)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Indicador de saldo analitico
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] IndicadorSaldoAnalitico;

            /// <summary>
            /// Tipo de ativo
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] TipoAtivo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 831)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 29 - Saldo de custodia bloqueado - central depositaria
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro29
        {
            /// <summary>
            /// Tipo do registro fixo 29
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true,0)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,1)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQExport(true,2)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true,3)]
            public byte[] Carteira;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true,4)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Distribuicao;

            /// <summary>
            /// Tipo do bloqueio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true,5)]
            public byte[] TipoBloqueio;

            /// <summary>
            /// Descricao do bloqueio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            [IMBARQExport(true,6)]
            public byte[] DescricaoBloqueio;

            /// <summary>
            /// Data do pregao na compra
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQExport(true,7)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataPregaoCompra;

            /// <summary>
            /// Data de inicio do bloqueio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQExport(true,8)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataInicioBloqueio;

            /// <summary>
            /// Data de termino do bloqueio
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQExport(true,9)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataTerminoBloqueio;

            /// <summary>
            /// Codigo do agente de compensacao
            /// </summary>
            [IMBARQExport(true,10)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] CodigoAgenteCompensacao;

            /// <summary>
            /// Data de aquisicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            [IMBARQExport(true,11)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataAquisicao;

            /// <summary>
            /// Preco de aquisicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            [IMBARQExport(true,12)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] PrecoAquisicao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 779)]
            public byte[] Filler;
        }

        /// <summary>
        /// Registro 30 - Identificacao do saldo analitico - central depositaria
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ001_Registro30
        {
            /// <summary>
            /// Tipo do registro fixo 30
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] TipoRegistro;

            /// <summary>
            /// Codigo do participante solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitante;

            /// <summary>
            /// Codigo investidor solicitante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DigitoInvestidorSolicitante;

            /// <summary>
            /// Codigo do participante solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodParticipanteSolicitado;

            /// <summary>
            /// Codigo investidor solicitado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] CodInvestidorSolicitado;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] Carteira;

            /// <summary>
            /// Descricao da carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] DescricaoCarteira;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Distribuicao;

            /// <summary>
            /// Nome do emissor
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] NomeEmissor;

            /// <summary>
            /// Especificacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Especificacao;

            /// <summary>
            /// Quantidade de ativos em custodia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 3)]
            public byte[] QtdeAtivosCustodia;

            /// <summary>
            /// Quantidade total de ativos bloqueados
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] QtdeTotalAtivosBloqueados;

            /// <summary>
            /// Codigo de negociacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] CodigoNegociacao;

            /// <summary>
            /// Data de aquisicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyyMMdd", 8)]
            public byte[] DataAquisicao;

            /// <summary>
            /// Preco de aquisicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] PrecoAquisicao;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 812)]
            public byte[] Filler;

        }

    }
}
