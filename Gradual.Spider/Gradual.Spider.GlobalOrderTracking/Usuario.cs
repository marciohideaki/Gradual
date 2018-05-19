using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
{
    public enum TipoAcesso
    {
        Cliente = 0,
        Cadastro = 1,
        Assessor = 2,
        Atendimento = 3,
        TeleMarketing = 4,
        PontaMesa = 5
    }

    public class Usuario
    {
        #region Globais

        private string gCodigoSessao;

        #endregion

        #region Propriedades

        public TipoAcesso TipoAcesso { get; set; }

        public string CodBMF { get; set; }

        public string CodBovespa { get; set; }

        public int CodAssessor { get; set; }

        public List<int> CodAssessoresAgregados { get; set; }

        public string NomeUsuario { get; set; }

        public string Email { get; set; }

        public string CodLogin { get; set; }

        public List<Cliente> Clientes { get; set; }

        public List<string> CodigoClientes { get; set; }

        public Cliente Cliente { get; set; }

        public string Status { get; set; }

        #endregion

        #region Construtores

        public Usuario(string pCodigoSessao)
        {
            this.gCodigoSessao = pCodigoSessao;
            this.Clientes = new List<Cliente>();
            this.CodigoClientes = new List<string>();
            this.RetornarUsuarioDaSessao();
        }

        #endregion

        #region Métodos Private

        private static readonly log4net.ILog gLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void RetornarUsuarioDaSessao()
        {
            Gradual.OMS.Seguranca.Lib.IServicoSeguranca lServicoSeguranca = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.Seguranca.Lib.IServicoSeguranca>();

            Gradual.OMS.Seguranca.Lib.ReceberSessaoResponse lSessaoResponse = lServicoSeguranca.ReceberSessao(new Gradual.OMS.Seguranca.Lib.ReceberSessaoRequest()
            {
                CodigoSessao = this.gCodigoSessao,
                CodigoSessaoARetornar = this.gCodigoSessao
            });

            Gradual.OMS.Library.ContextoOMSInfo lContextoOMS = lSessaoResponse.Usuario.Complementos.ReceberItem<Gradual.OMS.Library.ContextoOMSInfo>();
            //if (!System.String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ContaMaster"]))
            //{
            //    this.TipoAcesso = TipoAcesso.Assessor;
            //}
            //else
            //{
            this.TipoAcesso = (TipoAcesso)lSessaoResponse.Usuario.CodigoTipoAcesso;
            //}
            this.CodAssessor = lSessaoResponse.Usuario.CodigoAssessor;
            this.CodBMF = lContextoOMS.CodigoBMF;
            this.CodBovespa = lContextoOMS.CodigoCBLC;
            this.NomeUsuario = lSessaoResponse.Usuario.Nome;
            this.Email = lSessaoResponse.Usuario.Email;
            this.CodLogin = lSessaoResponse.Usuario.CodigoUsuario;
            this.Status = lSessaoResponse.Usuario.Status.ToString();
        }

        #endregion

        #region Métotodos Públicos

        public int ReceberCodigoDoUsuario()
        {
            int Codigo = 0;

            switch (TipoAcesso)
            {
                case TipoAcesso.Assessor:
                    Codigo = ContextoGlobal.Usuario.CodAssessor;
                    break;
                case TipoAcesso.Cliente:
                    Codigo = string.IsNullOrEmpty(ContextoGlobal.Usuario.Cliente.CodBovespa) ? int.Parse(ContextoGlobal.Usuario.Cliente.CodBMF) : int.Parse(ContextoGlobal.Usuario.Cliente.CodBovespa);
                    break;
                case TipoAcesso.PontaMesa:
                    Codigo = Convert.ToInt32(ContextoGlobal.CodigoUsuario);
                    break;
                default:
                    int.TryParse(this.CodBovespa, out Codigo);
                    break;
            }

            return Codigo;
        }

        #endregion

    }
}
