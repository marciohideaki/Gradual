using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteEnderecoInfo : ICodigoEntidade
    {
        #region Propriedades

        /// <summary>
        /// Código do Endereço
        /// </summary>
        public Nullable<int> IdEndereco { get; set; }

        /// <summary>
        /// Código Tipo do Endereço
        /// </summary>
        public int IdTipoEndereco { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>        
        public int IdCliente { get; set; }

        /// <summary>
        /// Flag de Endereço principal 
        /// </summary>        
        public Boolean StPrincipal { get; set; }

        /// <summary>
        /// Número de CEP
        /// </summary>        
        public int NrCep { get; set; }

        /// <summary>
        /// Número de CEP Extensao
        /// </summary>        
        public int NrCepExt { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>        
        public string DsLogradouro { get; set; }


        /// <summary>
        /// Complemento do endereço
        /// </summary>       
        public string DsComplemento { get; set; }


        /// <summary>
        /// Bairro 
        /// </summary>       
        public string DsBairro { get; set; }


        /// <summary>
        /// Cidade
        /// </summary>       
        public string DsCidade { get; set; }

        /// <summary>
        /// Estado
        /// </summary>       
        public string CdUf { get; set; }

        /// <summary>
        /// País 
        /// </summary>       
        public string CdPais { get; set; }

        /// <summary>
        /// Número
        /// </summary>        
        public string DsNumero { get; set; }

        #endregion

        #region construtor

        public ClienteEnderecoInfo() { }

        public ClienteEnderecoInfo(string pIdCliente)
        {
            this.IdCliente = int.Parse(pIdCliente);
        }

        public ClienteEnderecoInfo(Cadastro.ClienteEnderecoDeCustodiaInfo pEnderecoCustodia)
        {
            this.IdEndereco = pEnderecoCustodia.IdEndereco;
            this.IdTipoEndereco = pEnderecoCustodia.IdTipoEndereco;
            this.IdCliente = pEnderecoCustodia.IdCliente;
            this.StPrincipal = pEnderecoCustodia.StPrincipal;
            this.NrCep = pEnderecoCustodia.NrCep;
            this.NrCepExt = pEnderecoCustodia.NrCepExt;
            this.DsLogradouro = pEnderecoCustodia.DsLogradouro;
            this.DsComplemento = pEnderecoCustodia.DsComplemento;
            this.DsBairro = pEnderecoCustodia.DsBairro;
            this.DsCidade = pEnderecoCustodia.DsCidade;
            this.CdUf = pEnderecoCustodia.CdUf;
            this.CdPais = pEnderecoCustodia.CdPais;
            this.DsNumero = pEnderecoCustodia.DsNumero;
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
