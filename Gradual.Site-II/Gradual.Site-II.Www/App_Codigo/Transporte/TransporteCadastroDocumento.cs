using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using System.Globalization;

namespace Gradual.Site.Www
{
    public class TransporteCadastroDocumento
    {
        #region Propriedades
        
        public int? IdClienteDocumento { get; set; }

        public int IdCliente { get; set; }

        public string TipoDocumento        { get; set; }
        public string TipoDocumentoDesc    { get; set; }
        public string NumeroDocumento      { get; set; }
        public string OrgaoEmissor         { get; set; }
        public string OrgaoEmissorDesc     { get; set; }
        public string EstadoEmissao        { get; set; }
        public string EstadoEmissaoDesc    { get; set; }
        public string DataEmissao          { get; set; }
        public string CodigoSegurancaCNH   { get; set; }
        public string Principal { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastroDocumento() { }
        /*
        public TransporteCadastroDocumento(ClienteDocumentoInfo pInfo) : this()
        {
            this.IdClienteDocumento = pInfo.IdClienteDocumento;
            this.IdCliente = pInfo.IdCliente.Value;

            this.TipoDocumento = pInfo.TpDocumento;
            this.TipoDocumentoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.TipoDocumento, this.TipoDocumento);

            this.NumeroDocumento = pInfo.DsNumeroDocumento;

            this.OrgaoEmissor = pInfo.CdOrgaoEmissorDocumento;
            this.OrgaoEmissorDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.OrgaoEmissor, this.OrgaoEmissor);

            this.EstadoEmissao = pInfo.CdUfEmissaoDocumento;
            this.EstadoEmissaoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Estado, this.EstadoEmissao);

            if (pInfo.DtEmissao.HasValue)
            {
                this.DataEmissao = pInfo.DtEmissao.Value.ToString("dd/MM/yyyy");
            }

            this.Principal = (pInfo.StPrincipal.Value == true) ? "Sim" : "Não";

        }

        public ClienteDocumentoInfo ToClienteDocumentoInfo()
        {
            ClienteDocumentoInfo lRetorno = new ClienteDocumentoInfo();

            lRetorno.IdCliente = this.IdCliente;

            if(this.IdClienteDocumento.HasValue)
                lRetorno.IdClienteDocumento = this.IdClienteDocumento.Value;

            lRetorno.TpDocumento = this.TipoDocumento;
            lRetorno.DsNumeroDocumento = this.NumeroDocumento;
            lRetorno.CdOrgaoEmissorDocumento = this.OrgaoEmissor;
            lRetorno.CdUfEmissaoDocumento = this.EstadoEmissao;
            lRetorno.DtEmissao = DateTime.ParseExact(this.DataEmissao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            lRetorno.CodigoSegurancaCNH = this.CodigoSegurancaCNH;

            lRetorno.StPrincipal = (this.Principal == "Sim");

            return lRetorno;
        }*/

        #endregion
    }
}