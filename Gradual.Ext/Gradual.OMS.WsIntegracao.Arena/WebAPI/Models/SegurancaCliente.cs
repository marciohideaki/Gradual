using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.WsIntegracao.Arena.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Models
{
    public class SegurancaCliente
    {
        public int IdClienteGradual             { get; set; }

        public bool StatusAutenticacaoRequisicao{ get; set; }

        public string TokenClienteAutenticacao  { get; set; }

        public string Usuario                   { get; set; }

        public string Senha                     { get; set; }

        public string Email { get; set; }

        public string IP { get; set; }

        public string CodigoSistema { get; set; }

        public int CodigoBovespaCliente         { get; set; }

        public int CodigoBmfcliente             { get; set; }

        public bool ClienteBloqueado            { get; set; }

        public DateTime DataUltimoLogin         { get; set; }

        public DateTime DataAutenticacao        { get; set; }

        public List<Criticas> Criticas          { get; set; }

        public Cliente ClienteAtributo          { get; set; }
    }
}