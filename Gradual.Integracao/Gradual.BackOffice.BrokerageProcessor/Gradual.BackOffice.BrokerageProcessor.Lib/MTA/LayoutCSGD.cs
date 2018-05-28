using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.MTA
{
    class LayoutCSGD
    {
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_OO_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        // Os 2 campos a seguir definem o nome do arquivo
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] CodigoArquivo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] AgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CodigoOrigem;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] CodigoDestino;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] NumeroMovimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataGeracaoArquivo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataMovimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 153)]
        public byte[] Filler;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_01_ID_INVESTIDOR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        // Os 2 campos a seguir definem o nome do arquivo
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodigoAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DVCodigoAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] CodigoInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DVCodigoInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] CPFCNPJInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataNascFundacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] CodigoDependencia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] DescricaoDependencia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] NomeInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] NomeAdministrador;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodAgenteCustodiaFinal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] CodigoInvestidorFinal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 55)]
        public byte[] Filler;
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_02_ID_SALDO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] CodigoInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodigoCarteira;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] DescricaoCarteira;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoISIN;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Distribuicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] NomeEmissor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Especificacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public byte[] QtdeTotalAtivosCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] QtdeTotalAtivosBloqueados;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoNegociacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] IndicadorSaldoAnalitico;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] TipoAtivo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] Filler;

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_03_ID_SALDOS_BLOQUEADOS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] CodigoInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodigoCarteira;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoISIN;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Distribuicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] TipoBloqueio;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] DescricaoBloqueio;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataPregaoCompra;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataInicioBloqueio;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataFimBloqueio;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] QtdeAtivosBloqueados;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodAgenteCompensacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataAquisicao;

        // N(9)v6
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] PrecoAquisicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
        public byte[] Filler;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_04_ID_SALDO_ANALITICO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodAgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] CodInvestidor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] CodCarteira;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] DescricaoCarteira;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoISIN;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Distribuicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] NomeEmissor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Especificacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public byte[] QtdeAtivosCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] QtdeAtivosBloqueados;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodigoNegociacao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataAquisicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] PrecoAquisicao;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 61)]
        public byte[] Filler;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CSGD_99_TRAILER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoRegistro;

        // Os 2 campos a seguir definem o nome do arquivo
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] CodigoArquivo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] AgenteCustodia;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CodigoOrigem;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] CodigoDestino;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] NumeroMovimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataGeracaoArquivo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] TotalRegistros;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataMovimento;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
        public byte[] Filler;
    }
}
