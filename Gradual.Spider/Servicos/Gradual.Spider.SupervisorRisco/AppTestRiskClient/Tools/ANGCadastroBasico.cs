using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AppTestRiskClient.Tools
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ANGCadastroBasico
    {
        /// <summary>
        /// Tipo mensagem ANG (CB-CadastroBasico)
        /// Layout mensagem CADASTRO BASICO:
        ///
        /// Tipo de Mensagem          X(2)
        /// Data                      N(8)       Formato AAAAMMDD
        /// Hora                      N(9)       Formato HHMMSSmmm (mmm = milisegundos)
        /// Código Instrumento        X(20)
        ///
        /// Código ISIN               X(20)
        /// Instrumento Principal     X(20)
        /// Lote padrão               N(8)
        /// Segmento Mercado          X(8)
        /// Forma Cotação             N(8)
        /// Grupo Cotação             X(4)
        /// Data Vencimento           N(8)
        /// Preço Exercício           N(13)
        /// Indicador Opção           X(1)
        /// Coeficiente Multiplicação N(13)
        /// Data último negócio       N(8)       Formato AAAAMMDD
        /// Hora último negócio       N(6)       Formato HHMMSS
        /// Security Id Source        X(4)
        /// Especificação             X(100)
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoMsg;

        /// <summary>
        /// Timestamp da geracao da mensagem
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] DataHora;

        /// <summary>
        /// Instrumento
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Instrumento;

        /// <summary>
        /// Codigo ISIN
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] CodigoIsin;

        /// <summary>
        /// Papel Base
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] CodigoPapelObjeto;

        /// <summary>
        /// Lote padrao
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] LotePadrao;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SegmentoMercado;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] FormaCotacao;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] GrupoCotacao;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DataVencimento;

        /// <summary>
        /// Preco de exercicio
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] PrecoExercicio;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] IndicadorOpcao;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] CoeficienteMultiplicacao;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] DataHoraNegocio;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] SecurityIDSource;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] Especificacao;
    }
}
