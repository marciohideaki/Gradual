using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConversorTesouroDireto.Registros
{
    class LayoutTesouroDireto
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDTD_OO_HEADER
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDTD_01_IDENTIFICACAO_X
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] CodigoAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CodigoCliente;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] DigitoCodigoCliente;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] CpfCpnjCliente;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] TipoTitulo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataVctoTitulo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] CodigoSelic;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoISIN;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] TipoTransacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] IDContabilTransacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] QuantitadeTitulosTransacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] NumeroProtocolo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] Usuario;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] AgenteCustodiaContraparte;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] PrecoUnitarioTransacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorTransacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorTaxaAgente;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorTotal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] PrecoUnitarioOriginal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorOriginal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataPrecoOriginal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] OrigemSaldo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataOperacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataMovimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataFinalDoacaoCupom;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorTxSemBMFBovespa;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] ValorTxSemAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] PercentualReinvestimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ProtocoloAgendamento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataEmissaoTitulo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataPagtoCupomAnterior;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataPagtoPrimeiroCupom;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] PUPrimeiroCupomJurosPago;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataPgtoCupomAnteriorCompra;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] DataLiquidacaoCompra;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
        public byte[] Filler;

    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MDTD_99_TRAILER
    {
    }
}
