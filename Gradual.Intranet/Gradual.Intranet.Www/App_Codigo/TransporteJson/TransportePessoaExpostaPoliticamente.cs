using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransportePessoaExpostaPoliticamente
    {
        #region Propriedades

        public int Id { get; set; }
        
        public string Nome { get; set; }

        public string Identificacao { get; set; }

        public string Documento { get; set; }

        public string DataImportacao { get; set; }

        #endregion

        #region Construtor
        
        public TransportePessoaExpostaPoliticamente() { }

        
        public TransportePessoaExpostaPoliticamente(PessoaExpostaPoliticamenteInfo pPessoa) 
        {
            this.Id = pPessoa.IdPEP.Value;

            this.Nome = pPessoa.DsNome.ToStringFormatoNome();
            this.Documento = pPessoa.DsDocumento.ToCpfCnpjString();
            this.DataImportacao = pPessoa.DtImportacao.ToString("dd/MM/yyyy");
        }

        #endregion
    }
}