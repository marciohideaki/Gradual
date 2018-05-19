using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class LoginRequestDTO
    {
        public int IdClienteGradual             { get; set; }

        public string IP                        { get; set; }

        public string UserAgent                 { get; set; }

        public int CBLC                         { get; set; }

        public string CPF                       { get; set; }

        public string Name                      { get; set; }

        public string Email                     { get; set; }

        public string Phone                     { get; set; }

        public bool IsSuperQualified            { get; set; }

        public bool IsBovespaAllowed            { get; set; }

        public bool IsBmfAllowed                { get; set; }
    }
}