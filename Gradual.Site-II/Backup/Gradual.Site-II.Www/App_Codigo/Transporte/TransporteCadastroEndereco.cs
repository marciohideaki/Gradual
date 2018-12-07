using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www
{
    public class TransporteCadastroEndereco
    {
        #region Propriedades

        public string Pais { get; set; }

        public string PaisDesc { get; set; }

        public string Estado { get; set; }

        public string EstadoDesc { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Complemento { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string TipoEndereco { get; set; }

        public string TipoEnderecoDesc { get; set; }

        public string CEP { get; set; }

        //public int NrCepExt { get; set; }

        public string Principal { get; set; }

        public int? IdEndereco { get; set; }

        public int IdCliente { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastroEndereco() { }

        public TransporteCadastroEndereco(TransporteSessaoClienteLogado pClienteBase, ClienteEnderecoInfo pEnderecoBase)
        {
            this.IdCliente      = pEnderecoBase.IdCliente;

            this.Estado         = pEnderecoBase.CdUf;
            this.EstadoDesc     = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Estado, this.Estado);;

            this.Bairro         = pEnderecoBase.DsBairro;
            this.Cidade         = pEnderecoBase.DsCidade;
            this.Complemento    = pEnderecoBase.DsComplemento;
            this.Logradouro     = pEnderecoBase.DsLogradouro;
            this.Numero         = pEnderecoBase.DsNumero;
            this.IdEndereco     = pEnderecoBase.IdEndereco;
            this.TipoEndereco   = pEnderecoBase.IdTipoEndereco.ToString();
            //this.NrCepExt       = pEnderecoBase.NrCepExt;
            this.Principal      = pEnderecoBase.StPrincipal ? "Sim" : "Não";

            this.Pais           = pEnderecoBase.CdPais;
            this.PaisDesc       = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Pais, pEnderecoBase.CdPais);


            this.CEP = pEnderecoBase.NrCep.ToString();

            if (this.CEP.Length < 5)
            {
                this.CEP = this.CEP.PadLeft(5, '0');
            }

            this.CEP = this.CEP + "-" + Convert.ToString(pEnderecoBase.NrCepExt).PadLeft(3, '0');

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

        #region Métodos Públicos

        public ClienteEnderecoInfo ToEnderecoInfo()
        {
            ClienteEnderecoInfo lRetorno = new ClienteEnderecoInfo();

            lRetorno.CdPais = this.Pais.ToUpper();
            lRetorno.CdUf = this.Estado.ToUpper();
            lRetorno.DsBairro = this.Bairro.ToUpper();
            lRetorno.DsCidade = this.Cidade.ToUpper();
            lRetorno.DsComplemento = this.Complemento.ToUpper();
            lRetorno.DsLogradouro = this.Logradouro.ToUpper();
            lRetorno.DsNumero = this.Numero.ToUpper();
            //lRetorno.IdCliente = this.idcli;

            if(this.IdEndereco.HasValue)
                lRetorno.IdEndereco = this.IdEndereco.Value;

            lRetorno.IdTipoEndereco = Convert.ToInt32(this.TipoEndereco);

            lRetorno.NrCep = Convert.ToInt32(this.CEP.Substring(0, 5));

            lRetorno.NrCepExt = Convert.ToInt32(this.CEP.Substring(6, 3));

            lRetorno.StPrincipal = (this.Principal == "Sim");

            return lRetorno;
        }

        #endregion
    }
}