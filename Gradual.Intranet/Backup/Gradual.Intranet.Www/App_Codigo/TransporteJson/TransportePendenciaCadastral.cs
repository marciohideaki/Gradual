using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransportePendenciaCadastral : ITransporteJSON
    {
        #region Members

        public Nullable<int> Id { get; set; }
        public Int32 ParentId { get; set; }
        public string Tipo { get; set; }
        public string TipoDesc { get; set; }
        public string DataPendencia { get; set; }
        public string Descricao { get; set; }
        public string Resolucao { get; set; }
        public string DataResolucao { get; set; }
        public bool FlagResolvido { get; set; }
        public string TipoDeItem { get { return "PendenciaCadastral"; } }
        public bool Exclusao { get; set; }
        public Nullable<int> IdLogin { get; set; }
        public string DsLogin { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor default
        /// </summary>
        public TransportePendenciaCadastral() { }

        /// <summary>
        /// Construtor para preenchimento
        /// </summary>
        /// <param name="pInfo"></param>
        public TransportePendenciaCadastral(ClientePendenciaCadastralInfo pInfo, bool pExclusao)
        {
            this.Id = pInfo.IdPendenciaCadastral.DBToInt32();
            this.Tipo = pInfo.IdTipoPendencia.ToString();
            this.ParentId = pInfo.IdCliente.DBToInt32();
            this.Descricao = pInfo.DsPendencia;
            this.DataPendencia = pInfo.DtPendencia.ToString();
            this.DataResolucao = pInfo.DtResolucao.ToString();
            this.Resolucao = pInfo.DsResolucao;
            this.FlagResolvido = (pInfo.DtResolucao != null) ? true : false;
            this.Exclusao = pExclusao;
            this.IdLogin = pInfo.IdLoginRealizacao;
            this.DsLogin = pInfo.DsLoginRealizacao;
            this.TipoDesc = pInfo.DsTipoPendencia;
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Método para cast da entidade de transporte
        /// </summary>
        /// <returns></returns>
        public ClientePendenciaCadastralInfo ToClientePendenciaCadastralInfo(int? pIdLoginRealizacao = null)
        {
            var lRetorno = new ClientePendenciaCadastralInfo();

            lRetorno.DsPendencia = this.Descricao;
            lRetorno.DsResolucao = this.Resolucao;
            lRetorno.DtPendencia = this.DataPendencia.DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite);
            lRetorno.DtResolucao = (this.FlagResolvido) ? DateTime.Now : new Nullable<DateTime>();
            lRetorno.IdCliente = this.ParentId.DBToInt32();
            lRetorno.IdTipoPendencia = this.Tipo.DBToInt32();
            lRetorno.IdPendenciaCadastral = this.Id.DBToInt32(Contratos.Dados.Enumeradores.eIntNull.Zero);
            lRetorno.IdLoginRealizacao = this.IdLogin;
            lRetorno.DsLoginRealizacao = this.DsLogin;
            lRetorno.DsTipoPendencia = this.TipoDesc;
            return lRetorno;
        }

        #endregion
    }
}