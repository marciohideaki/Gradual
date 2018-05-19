using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteEndereco : ITransporteJSON
    {
        #region | Propriedades
        
        /// <summary>
        /// Permissão para exclusão
        /// </summary>
        public bool Exclusao { get; set; }

        /// <summary>
        /// Permissão para alteração
        /// </summary>
        public bool Alteracao { get; set; }

        /// <summary>
        /// Código do Endereço
        /// </summary>
        public Nullable<int> Id { get; set; }

        /// <summary>
        /// Código Tipo do Endereço
        /// </summary>
        public int Tipo { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>        
        public Int32 ParentId { get; set; }

        /// <summary>
        /// Número de CEP
        /// </summary>        
        public string CEP { get; set; }

        /// <summary>
        /// Número de CEP Extensao
        /// </summary>        
        public int NrCepExt { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>        
        public string Logradouro { get; set; }

        /// <summary>
        /// Complemento do endereço
        /// </summary>       
        public string Complemento { get; set; }

        /// <summary>
        /// Bairro 
        /// </summary>       
        public string Bairro { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>       
        public string Cidade { get; set; }

        /// <summary>
        /// Estado
        /// </summary>       
        public string UF { get; set; }

        /// <summary>
        /// País 
        /// </summary>       
        public string Pais { get; set; }

        /// <summary>
        /// Número
        /// </summary>        
        public string Numero { get; set; }

        /// <summary>
        /// Tipo de Item
        /// </summary>
        public string TipoDeItem { get { return "Endereco"; } }

        /// <summary>
        /// Endereço para correspondência
        /// </summary>
        public bool FlagCorrespondencia { get; set; }

        #endregion

        #region | Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public TransporteEndereco() { }

        /// <summary>
        /// Construtor TransporteEndereco
        /// </summary>
        /// <param name="pEndereco">Recebe entidade de ClienteEnderecoInfo como parametro</param>
        public TransporteEndereco(ClienteEnderecoInfo pEndereco, bool pExcluir)
        {
            this.Pais                   = pEndereco.CdPais;
            this.UF                     = pEndereco.CdUf;
            this.Bairro                 = pEndereco.DsBairro;
            this.Cidade                 = pEndereco.DsCidade;
            this.Complemento            = pEndereco.DsComplemento;
            this.Logradouro             = pEndereco.DsLogradouro;
            this.Numero                 = pEndereco.DsNumero;
            this.ParentId               = pEndereco.IdCliente;
            this.Id                     = pEndereco.IdEndereco;
            this.Tipo                   = pEndereco.IdTipoEndereco;
            this.CEP                    = string.Concat(pEndereco.NrCep.ToString().PadLeft(5,'0'),"-",pEndereco.NrCepExt.ToString().PadLeft(3,'0'));
            this.NrCepExt               = pEndereco.NrCepExt;
            this.FlagCorrespondencia    = pEndereco.StPrincipal;
            this.Id                     = pEndereco.IdEndereco;
            this.Exclusao               = pExcluir;
        }

        #endregion

        #region | Métodos 

        public ClienteEnderecoInfo ToClienteEnderecoInfo()
        {
            ClienteEnderecoInfo lRetorno = new ClienteEnderecoInfo();

            lRetorno.CdPais         = this.Pais;
            lRetorno.CdUf           = this.UF;
            lRetorno.DsBairro       = this.Bairro;
            lRetorno.DsCidade       = this.Cidade;
            lRetorno.DsComplemento  = this.Complemento;
            lRetorno.DsLogradouro   = this.Logradouro;
            lRetorno.NrCep          = Int32.Parse(this.CEP.Split('-')[0]);
            lRetorno.NrCepExt       = Int32.Parse(this.CEP.Split('-')[1]);
            lRetorno.DsNumero       = this.Numero;
            lRetorno.IdTipoEndereco = this.Tipo;
            lRetorno.IdEndereco     = this.Id;
            lRetorno.IdCliente      = this.ParentId;
            lRetorno.StPrincipal    = this.FlagCorrespondencia;

            return lRetorno;
        }

        #endregion
    }
}