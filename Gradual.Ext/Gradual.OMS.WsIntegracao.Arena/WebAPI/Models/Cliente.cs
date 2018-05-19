using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class Cliente
    {
        public int ID               { get; set; }

        public int CodigoBovespa    { get; set; }

        public int CodigoBmf        { get; set; }

        public string NomeCliente   { get; set; }

        public string CpfCnpj       { get; set; }

        public string TipoCliente   { get; set; }

        public string CodigoAssessor{ get; set; }

        public string Assessor      { get; set; }

        public bool StatusBovespa   { get; set; }

        public bool StatusBmf       { get; set; }

        public string Email         { get; set; }

        public string Phone         { get; set; }
    }
}