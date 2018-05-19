using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.OMS.AutomacaoOrdensTeste
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Header_MDS
    {
        /// <summary>
        /// Tipo da mensagem - NE/LF/LO/
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoMensagem;

        /// <summary>
        /// Tipo de bolsa - BF/BV
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TipoBolsa;

        /// <summary>
        /// DataHora yyyyMMddHHmmssfff
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] DataHora;

        /// <summary>
        /// Instrumento
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Instrumento;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_LivroNegocio_MDS
    {
        /// <summary>
        /// DataHora yyyyMMddHHmmssfff
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public byte[] DataHora;

        /// <summary>
        /// CorretoraCompradora
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CorretoraCompradora;

        /// <summary>
        /// CorretoraVendedora
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CorretoraVendedora;

        /// <summary>
        /// Preco
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] Preco;

        /// <summary>
        /// Quantidade
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] Quantidade;

        /// <summary>
        /// Maximo  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] Maximo;

        /// <summary>
        /// Minimo  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] Minimo;

        /// <summary>
        /// VolumeAcumulado  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] VolumeAcumulado;

        /// <summary>
        /// NumeroNegocios  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] NumeroNegocios;

        /// <summary>
        /// Variacao  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] Variacao;
        
        /// <summary>
        /// EstadoPapel  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] EstadoPapel;
    }


    class EstruturasMDS
    {
    }
}
