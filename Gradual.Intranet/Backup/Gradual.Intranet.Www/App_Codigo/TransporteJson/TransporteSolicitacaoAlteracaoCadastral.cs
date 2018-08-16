#region Includes
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
using Gradual.Intranet.Contratos.Mensagens;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSolicitacaoAlteracaoCadastral : ITransporteJSON
    {
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
        public string TipoDeItem { get { return "SolicitacaoAlteracaoCadastral"; } }
        public string DsInformacao { get; set; }
        public string DsDescricao { get; set; }
        public string CdTipo { get; set; }
        public string DsTipo { get; set; }
        public string DtSolicitacao { get; set; }
        public string DtRealizacao { get; set; }
        public Nullable<int> IdLogin { get; set; }
        public Boolean StResolvido { get; set; }
        public string DsLoginRealizacao { get; set; }
        public string DsLoginSolicitante { get; set; }

        public TransporteSolicitacaoAlteracaoCadastral()
        { 
        }

        public TransporteSolicitacaoAlteracaoCadastral(ClienteAlteracaoInfo pInfo)
        {
            this.CdTipo = pInfo.CdTipo.ToString();

            if (pInfo.CdTipo == 'A')
                this.DsTipo = "Alteração";
            else
                if (pInfo.CdTipo == 'I')
                    this.DsTipo = "Inclusão";
                else
                    this.DsTipo = "Exclusão";

            this.DsDescricao = pInfo.DsDescricao;
            this.DsInformacao = pInfo.DsInformacao;
            if (pInfo.DtRealizacao != null) {
                this.DtRealizacao = pInfo.DtRealizacao.Value.ToString("dd/MM/yyyy");
            }
          
            this.DtSolicitacao = pInfo.DtSolicitacao.ToString("dd/MM/yyyy");
            this.Id = pInfo.IdAlteracao;
            this.IdLogin = pInfo.IdLoginRealizacao;
            this.ParentId = pInfo.IdCliente;
            this.StResolvido = (pInfo.DtRealizacao == null) ? false : true;

            this.DsLoginRealizacao = pInfo.DsLoginRealizacao;
            this.DsLoginSolicitante = pInfo.DsLoginSolicitante;
        }
      
        
        public ClienteAlteracaoInfo ToClienteAlteracaoCadastralInfo()
        {

            ClienteAlteracaoInfo lRetorno = new ClienteAlteracaoInfo();
            
            lRetorno.IdAlteracao = this.Id;
            lRetorno.IdCliente = this.ParentId;

            lRetorno.CdTipo = this.CdTipo[0];
            lRetorno.DsDescricao = this.DsDescricao;
            lRetorno.DsInformacao = this.DsInformacao;

            lRetorno.DtRealizacao = null;
            lRetorno.DtSolicitacao = DateTime.Now;


            lRetorno.DsLoginRealizacao = this.DsLoginRealizacao;
            lRetorno.DsLoginSolicitante = this.DsLoginSolicitante;
            

            
            //lRetorno.CdTipo = this.CdTipo[0];
            //lRetorno.DsDescricao = this.DsDescricao;
            //lRetorno.DsInformacao = this.DsInformacao;
            //lRetorno.DtSolicitacao = this.DtSolicitacao.DBToDateTime();
            //if (this.DtRealizacao.Trim().Length > 0) {
            //    lRetorno.DtRealizacao = this.DtRealizacao.DBToDateTime();
            //}
            //lRetorno.IdLogin = this.IdLogin;
                        
            return lRetorno;
            
        }


      


     
    }
}