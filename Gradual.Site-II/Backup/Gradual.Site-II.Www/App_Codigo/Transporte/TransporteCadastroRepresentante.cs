using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using System.Globalization;

namespace Gradual.Site.Www
{
    public class TransporteCadastroRepresentante
    {
        #region Propriedades

        public int? IdCliente { get; set; }
        public int? IdRepresentante { get; set; }

        public string Nome              { get; set; }
        public string TipoDocumento     { get; set; }
        public string TipoDocumentoDesc { get; set; }
        public string OrgaoEmissor      { get; set; }
        public string OrgaoEmissorDesc  { get; set; }
        public string EstadoEmissor     { get; set; }
        public string EstadoEmissorDesc { get; set; }
        public string NumeroDocumento   { get; set; }
        public string DataNascimento    { get; set; }
        public string DataValidade      { get; set; }
        public string CPF               { get; set; }
        public string SituacaoLegal     { get; set; }
        public string SituacaoLegalDesc { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastroRepresentante() { }

        public TransporteCadastroRepresentante(ClienteProcuradorRepresentanteInfo pInfo)
        {
            this.IdCliente = pInfo.IdCliente;
            this.IdRepresentante = pInfo.IdProcuradorRepresentante;

            this.Nome = pInfo.DsNome;
            this.CPF = pInfo.NrCpfCnpj;

            if(pInfo.DtNascimento.HasValue)
                this.DataNascimento = pInfo.DtNascimento.Value.ToString("dd/MM/yyyy");

            this.NumeroDocumento = pInfo.DsNumeroDocumento;
            this.OrgaoEmissor = pInfo.CdOrgaoEmissor;
            this.EstadoEmissor = pInfo.CdUfOrgaoEmissor;
            this.TipoDocumento = pInfo.TpDocumento;
            this.SituacaoLegal = pInfo.TpSituacaoLegal.ToString();
        }

        #endregion

        #region Métodos Públicos

        public ClienteProcuradorRepresentanteInfo ToClienteProcuradorRepresentanteInfo()
        {
            ClienteProcuradorRepresentanteInfo pInfo = new ClienteProcuradorRepresentanteInfo();
            
            pInfo.DsNome            = this.Nome;
            pInfo.NrCpfCnpj         = this.CPF;

            if(!string.IsNullOrEmpty(this.DataNascimento))
                pInfo.DtNascimento      = DateTime.ParseExact(this.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            pInfo.DsNumeroDocumento = this.NumeroDocumento;
            pInfo.CdOrgaoEmissor    = this.OrgaoEmissor;
            pInfo.CdUfOrgaoEmissor  = this.EstadoEmissor;
            pInfo.TpDocumento       = this.TipoDocumento;
            pInfo.TpSituacaoLegal   = Convert.ToInt32(this.SituacaoLegal);

            return pInfo;
        }

        #endregion
    }
}