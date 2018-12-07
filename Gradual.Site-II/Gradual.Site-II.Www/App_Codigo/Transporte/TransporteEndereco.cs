using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www
{
    public class TransporteEndereco : TransporteComBuscaDoSinacor
    {
        #region Propriedades

        public string Pais { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public int IdCliente { get; set; }
        public int? IdEndereco { get; set; }
        public int IdTipoEndereco { get; set; }
        public string TipoEnderecoDesc { get; set; }
        public int Cep { get; set; }
        public int NrCepExt { get; set; }
        public bool Principal { get; set; }

        #endregion

        #region Construtores

        public TransporteEndereco(){}

        public TransporteEndereco(TransporteSessaoClienteLogado pClienteBase, ClienteEnderecoInfo pEnderecoBase)
        {
            this.Pais           = pEnderecoBase.CdPais;
            this.UF             = pEnderecoBase.CdUf;
            this.Bairro         = pEnderecoBase.DsBairro;
            this.Cidade         = pEnderecoBase.DsCidade;
            this.Complemento    = pEnderecoBase.DsComplemento;
            this.Logradouro     = pEnderecoBase.DsLogradouro;
            this.Numero         = pEnderecoBase.DsNumero;
            this.IdCliente      = pEnderecoBase.IdCliente;
            this.IdEndereco     = pEnderecoBase.IdEndereco;
            this.IdTipoEndereco = pEnderecoBase.IdTipoEndereco;
            this.Cep            = pEnderecoBase.NrCep;
            this.NrCepExt       = pEnderecoBase.NrCepExt;
            this.Principal      = pEnderecoBase.StPrincipal;
            
            this.Pais = base.RecuperarDadoDoSinacor(pClienteBase, eInformacao.Pais, pEnderecoBase.CdPais);

            this.TipoEnderecoDesc = "Outros";

            if (pEnderecoBase.IdTipoEndereco == 1)
            {
                this.TipoEnderecoDesc = "Comercial";
            }
            else if (pEnderecoBase.IdTipoEndereco == 2)
            {
                this.TipoEnderecoDesc = "Residencial";
            }
        }

        #endregion
    }
}