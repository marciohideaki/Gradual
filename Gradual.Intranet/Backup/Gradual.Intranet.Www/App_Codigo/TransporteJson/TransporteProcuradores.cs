#region includes
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.Excessoes;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteProcuradores : TransporteRepresentantesBase, ITransporteJSON
    {
        #region Members

        /// <summary>
        /// Data de nascimento
        /// </summary>
        public string DataNascimento { get; set; }

        /// <summary>
        /// Tipo de Item
        /// </summary>
        public string TipoDeItem { get { return "Procurador"; } }

        /// <summary>
        /// Tipo deo documento
        /// </summary>
        public string TipoDocumento { get; set; }

        /// <summary>
        /// Tipo do procurador/representante
        /// </summary>
        public int TipoDoRepresentante { get; set; }

        /// <summary>
        /// Tipo de situação legal do cliente
        /// </summary>
        public int TipoSituacaoLegal { get; set; }

        /// <summary>
        /// Data de Validade
        /// </summary>
        public string DataValidade { get; set; }
        
        #endregion

        #region Constructor
        public TransporteProcuradores() { }

        public TransporteProcuradores(ClienteProcuradorRepresentanteInfo pProcurador, bool pExclusao)
        {
            this.CPF                 = Convert.ToInt64(pProcurador.NrCpfCnpj).ToCpfCnpjString();
            this.DataNascimento      = null == pProcurador.DtNascimento || DateTime.MinValue == pProcurador.DtNascimento ? "" : pProcurador.DtNascimento.Value.ToString("dd/MM/yyyy");
            this.DataValidade        = pProcurador.DtValidade == null ? "" : pProcurador.DtValidade.Value.ToString("dd/MM/yyyy");
            this.Identidade          = pProcurador.DsNumeroDocumento;
            this.Id                  = pProcurador.IdProcuradorRepresentante;
            this.ParentId            = pProcurador.IdCliente.DBToInt32();
            this.UfOrgaoEmissor      = pProcurador.CdUfOrgaoEmissor;
            this.OrgaoEmissor        = pProcurador.CdOrgaoEmissor;
            this.TipoDocumento       = pProcurador.TpDocumento;
            this.Nome                = pProcurador.DsNome;
            this.TipoSituacaoLegal   = pProcurador.TpSituacaoLegal;
            this.Exclusao            = pExclusao;
            this.TipoDoRepresentante = (int)pProcurador.TpProcuradorRepresentante;
        }
        #endregion

        #region Métodos Públicos
        public ClienteProcuradorRepresentanteInfo ToClienteRepresentanteInfo()
        {
            ClienteProcuradorRepresentanteInfo lRetorno = new ClienteProcuradorRepresentanteInfo();

            lRetorno.DsNome                    = this.Nome;
            lRetorno.DsNumeroDocumento         = this.Identidade;
            lRetorno.DtNascimento              = this.DataNascimento.DBToDateTime();
            lRetorno.DtValidade                = this.DataValidade.DBToDateTime() == DateTime.MinValue ? new System.Nullable<DateTime>() : this.DataValidade.DBToDateTime();
            lRetorno.IdCliente                 = this.ParentId;
            lRetorno.IdProcuradorRepresentante = this.Id;
            lRetorno.NrCpfCnpj                 = this.CPF.Replace("-","").Replace(".","").Replace("/","");
            lRetorno.CdOrgaoEmissor            = this.OrgaoEmissor;
            lRetorno.CdUfOrgaoEmissor          = this.UfOrgaoEmissor;
            lRetorno.TpDocumento               = this.TipoDocumento;
            lRetorno.TpSituacaoLegal           = this.TipoSituacaoLegal;
            lRetorno.TpProcuradorRepresentante = (TipoProcuradorRepresentante)this.TipoDoRepresentante;

            return lRetorno;
        }
        #endregion
    }
}
