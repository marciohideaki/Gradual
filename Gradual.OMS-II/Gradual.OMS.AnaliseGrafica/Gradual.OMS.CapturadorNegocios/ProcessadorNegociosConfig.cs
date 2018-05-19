﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.CapturadorNegocios
{
    public class ProcessadorNegociosConfig
    {
        /// <summary>
        /// Endereco IP do servidor do MDS
        /// </summary>
        public string MDSAddress { get; set; }

        /// <summary>
        ///  Porta de conexao ao MDS
        /// </summary>
        public int MDSPort { get; set; }

        /// <summary>
        /// String de conexao com a base da Analise Grafica
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// String de conexao com a 
        /// </summary>
        public string MDSConnectionString { get;set; }
    }
}