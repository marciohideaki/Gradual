using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www.App_Codigo.Transporte
{
    public class Transporte_Clientes
    {
        public string IdLogin               { get; set; }

        public string Nome                  { get; set; }

        public string Email                 { get; set; }

        public string Assessor              { get; set; }

        public string CodigoBovespa         { get; set; }

        public string CodigoBmf             { get; set; }

        public string Sessao                { get; set; }

        public string Localidade            { get; set; }

        public string ContaMae              { get; set; }

        public string PesquisaBovespa       { get; set; } 
        
        public string PesquisaBmf           { get; set; }
        
        public string PesquisaContaMaster   { get; set; }

        public string PesquisaCodigo        { get; set; }

        public Transporte_Clientes Traduzir (ClienteInfo pCliente)
        {
            var lRetorno = new Transporte_Clientes();

            lRetorno.IdLogin       = pCliente.IdLogin.ToString();
            lRetorno.Nome          = pCliente.Nome;
            lRetorno.Email         = pCliente.Email;
            lRetorno.Assessor      = pCliente.CodigoAssessor.ToString();
            lRetorno.CodigoBovespa = pCliente.CodigoBovespa.ToString();
            lRetorno.CodigoBmf     = pCliente.CodigoBmf.ToString();
            lRetorno.Sessao        = pCliente.CodigoSessao.ToString();
            lRetorno.Localidade    = pCliente.CodigoLocalidade.ToString();
            //lRetorno.ContaMae      = pCliente.ContaMae.ToString();

            return lRetorno;
        }

        public ClienteInfo Traduzir(Transporte_Clientes pCliente)
        {
            var lRetorno = new ClienteInfo();

            lRetorno.IdLogin             = pCliente.IdLogin.DBToInt32();
            lRetorno.Nome                = pCliente.Nome;
            lRetorno.Email               = pCliente.Email;
            lRetorno.CodigoAssessor      = pCliente.Assessor.DBToInt32();
            lRetorno.CodigoBovespa       = pCliente.CodigoBovespa.DBToInt32();
            lRetorno.CodigoBmf           = pCliente.CodigoBmf.DBToInt32();
            lRetorno.CodigoSessao        = pCliente.Sessao.ToString();
            lRetorno.CodigoLocalidade    = pCliente.Localidade.DBToInt32();
            //lRetorno.ContaMae      = pCliente.ContaMae.ToString();

            return lRetorno;
        }
    }

    
}