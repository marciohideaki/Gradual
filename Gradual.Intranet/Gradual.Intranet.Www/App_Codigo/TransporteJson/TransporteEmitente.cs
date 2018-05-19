using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteEmitente
    {
        #region | Propriedades

        /// <summary>
        /// Código do Endereço
        /// </summary>
        public Nullable<int> Id { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>        
        public Int32 ParentId { get; set; }

        /// <summary>
        /// Tipo de Item
        /// </summary>
        public string TipoDeItem { get { return "Emitente"; } }

        /// <summary>
        /// Nome do emitente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Cpf ou cnpj do emitente do cliente
        /// </summary>
        public string CPFCNPJ { get; set; }

        /// <summary>
        /// Data de nascimento do emitente
        /// </summary>
        public string DataNascimento { get; set; }

        /// <summary>
        /// Identidade do emitente
        /// </summary>
        public string Identidade { get; set; }

        /// <summary>
        /// Código da bolsa
        /// </summary>
        public string CodigoSistema { get; set; }

        /// <summary>
        /// E-mail do Emitente do cliente
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Flag de emitente principal
        /// </summary>
        public bool FlagPrincipal { get; set; }

        /// <summary>
        /// Exclusão
        /// </summary>
        public bool Exclusao { get; set; }

        #endregion

        #region | Construtores
        public TransporteEmitente() { }

        public TransporteEmitente(ClienteEmitenteInfo pEmitente, bool pExclusao)
        {
            this.CodigoSistema  = pEmitente.CdSistema;
            this.CPFCNPJ        = pEmitente.NrCpfCnpj;
            this.DataNascimento = pEmitente.DtNascimento.Value.ToString("dd/MM/yyyy");
            this.Email          = pEmitente.DsEmail;
            this.FlagPrincipal  = pEmitente.StPrincipal;
            this.Id             = pEmitente.IdPessoaAutorizada;
            this.Identidade     = pEmitente.DsNumeroDocumento;
            this.ParentId       = pEmitente.IdCliente;
            this.Nome           = pEmitente.DsNome;
            this.Exclusao       = pExclusao;
        }
        #endregion

        #region | Métodos

        public ClienteEmitenteInfo ToClienteEmitente()
        { 
            ClienteEmitenteInfo lRetorno = new ClienteEmitenteInfo();

            lRetorno.DsEmail            = this.Email;
            lRetorno.DsNome             = this.Nome;
            lRetorno.DsNumeroDocumento  = this.Identidade;
            lRetorno.DtNascimento       = DateTime.Parse(this.DataNascimento);
            lRetorno.IdCliente          = this.ParentId;
            lRetorno.IdPessoaAutorizada = this.Id;
            lRetorno.NrCpfCnpj          = this.CPFCNPJ;
            lRetorno.StPrincipal        = this.FlagPrincipal;
            lRetorno.CdSistema          = this.CodigoSistema;

            return lRetorno;
        }

        #endregion
    }
}