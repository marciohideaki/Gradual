using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ
{
    [Serializable]
    public class RegistrosIMBARQ002
    {
        /// <summary>
        /// Header para o IMBARQ002
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ002_Header
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
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

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 933)]
            public byte[] Filler;
        }


        /// <summary>
        /// Trailler para o IMBARQ002
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ002_Trailler
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
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

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 924)]
            public byte[] Filler;
        }



        /// <summary>
        /// Registro 50 - detalhes das instrucoes de liquidadas
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMBARQ002_Registro50
        {
            /// <summary>
            /// Tipo do registro fixo 50
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
            /// Data do pregao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 10)]
            [IMBARQExport(true, 3)]
            public byte[] DataPregao;

            /// <summary>
            /// Data de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDateTime, "yyyy-MM-dd", 10)]
            [IMBARQExport(true, 4)]
            public byte[] DataLiquidacao;

            /// <summary>
            /// Tipo de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true, 5)]
            public byte[] TipoLiquidacao;

            /// <summary>
            /// Numero de instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            [IMBARQExport(true, 6)]
            public byte[] NumeroInstrucaoLiquidacao;

            /// <summary>
            /// Numero de instrucao de liquidacao Original
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            [IMBARQExport(true, 7)]
            public byte[] NumeroInstrucaoLiquidacaoOriginal;

            /// <summary>
            /// Carteira
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true, 8)]
            public byte[] Carteira;

            /// <summary>
            /// Codigo ISIN
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            [IMBARQExport(true, 9)]
            public byte[] CodigoISIN;

            /// <summary>
            /// Distribuicao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            [IMBARQExport(true, 9)]
            public byte[] Distribuicao;

            /// <summary>
            /// Natureza da Operacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            [IMBARQExport(true, 10)]
            public byte[] NaturezaOperacao;

            /// <summary>
            /// Preco medio de referencia
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 11)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] PrecoMedioReferencia;

            /// <summary>
            /// Quantidade total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 12)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeTotalInstrucaoLiquidacao;

            /// <summary>
            /// Volume total da instrucao de liquidacao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 13)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeTotalIntrucaoLiquidacao;

            /// <summary>
            /// Tipo de restricao
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            [IMBARQExport(true, 14)]
            public byte[] TipoRestricao;

            /// <summary>
            /// Situacao do resultado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true, 15)]
            public byte[] SituacaoResultado;

            /// <summary>
            /// Quantidade liquidada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 16)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeLiquidada;

            /// <summary>
            /// Volume financeiro liquidado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 17)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeFinanceiroLiquidado;

            /// <summary>
            /// Quantidade nao liquidada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 18)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeNaoLiquidada;

            /// <summary>
            /// Volume financeiro nao liquidado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 19)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeFinanceiroNaoLiquidado;

            /// <summary>
            /// Quantidade a ser liquidada
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 20)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeALiquidar;

            /// <summary>
            /// Volume financeiro a ser liquidado
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [IMBARQExport(true, 21)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 7)]
            public byte[] VolumeFinanceiroALiquidar;

            /// <summary>
            /// Quantidade aceita pelo custodiante
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 22)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeAceitaCustodiante;

            /// <summary>
            /// QuantidadeRestringivel
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 23)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QuantidadeRestringivel;

            /// <summary>
            /// Delivery restricted on position account level
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DeliveryRestrictedPositionAccountLevel;

            /// <summary>
            /// Delivery restricted on tp/dp level true or false
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DeliveryRestrictedTPDPLevel;

            /// <summary>
            /// Delivery restricted on CM Level
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] DeliveryRestrictedCMLevel;

            /// <summary>
            /// Quantidade da falha
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            [IMBARQExport(true, 24)]
            [IMBARQFieldDescription(IMBARQFieldType.FieldTypeDecimal, "", 6)]
            public byte[] QtdeFalha;

            /// <summary>
            /// Fallback settlement account
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] FallbackSettlementAccount;

            /// <summary>
            /// Informacao adicional de ETF
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            [IMBARQExport(true, 25)]
            public byte[] InformacaoAdicionalETF;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 434)]
            public byte[] Filler;
        }
    }
}
