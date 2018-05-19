using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
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
            this.gCodigoSessao  = pCodigoSessao;
            this.Clientes       = new List<Cliente>();
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
            if (TipoAcesso.Assessor == (TipoAcesso)this.TipoAcesso)
            {
                //if (!System.String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ContaMaster"]))
                //{
                //    RetornarClientesDoMaster();
                //}
                //else
                //{
                    RetornarClientesDoAssessor();
                //}
            }
            else
            {
                this.Cliente = new Cliente();

                this.Cliente.CodAssessor = this.CodAssessor;
                this.Cliente.CodBMF = this.CodBMF;
                this.Cliente.CodBovespa = this.CodBovespa;
                this.Cliente.Email = this.Email;
                this.Cliente.Nome = this.NomeUsuario;

                this.Clientes = new List<Cliente>();

                this.Clientes.Add(this.Cliente);

                if (!string.IsNullOrEmpty(this.CodBMF) && !this.CodigoClientes.Contains(this.CodBMF))
                {
                    this.CodigoClientes.Add(this.CodBMF);
                }

                if (!string.IsNullOrEmpty(this.CodBovespa) && !this.CodigoClientes.Contains(this.CodBovespa))
                {
                    this.CodigoClientes.Add(this.CodBovespa);
                }
            }
        }

        private void RetornarClientesDoAssessor()
        {
            //IServicoConsultaCliente lConsultaDeCliente = Ativador.Get<IServicoConsultaCliente>();

            Gradual.OMS.CadastroCliente.Lib.IServicoCadastroCliente lServico = Gradual.OMS.Library.Servicos.Ativador.Get<Gradual.OMS.CadastroCliente.Lib.IServicoCadastroCliente>();

            Gradual.OMS.CadastroCliente.Lib.BuscarClienteResumidoRequest lRequest = new Gradual.OMS.CadastroCliente.Lib.BuscarClienteResumidoRequest();
            Gradual.OMS.CadastroCliente.Lib.BuscarClienteResumidoResponse lResponse;

            lRequest.DadosDoClienteParaBusca = new Gradual.OMS.CadastroCliente.Lib.ClienteResumidoInfo();

            lRequest.DadosDoClienteParaBusca.TipoDeConsulta = Gradual.OMS.CadastroCliente.Lib.TipoDeConsultaClienteResumidoInfo.ClientesPorAssessor;
            lRequest.DadosDoClienteParaBusca.TermoDeBusca = this.CodAssessor.ToString();
            //lRequest.DadosDoClienteParaBusca.CodLogin = Int32.Parse(this.CodLogin.ToString());

            lResponse = lServico.BuscarClienteResumido(lRequest);

            if (lResponse.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                foreach (Gradual.OMS.CadastroCliente.Lib.ClienteResumidoInfo lInfo in lResponse.Resultados)
                {
                    this.Clientes.Add(new Cliente(lInfo) );

                    if (!string.IsNullOrEmpty(lInfo.CodBMF) && !this.CodigoClientes.Contains(lInfo.CodBMF))
                    {
                        this.CodigoClientes.Add(lInfo.CodBMF);
                    }

                    if (!string.IsNullOrEmpty(lInfo.CodBovespa) && !this.CodigoClientes.Contains(lInfo.CodBovespa))
                    {
                        this.CodigoClientes.Add(lInfo.CodBovespa);
                    }
                }

                this.CodigoClientes = this.CodigoClientes.OrderBy(c => c.ToString()).ToList();
                this.Clientes = this.Clientes.OrderBy(c => c.Nome.ToString()).ToList();

            }
            else
            {
                gLogger.ErrorFormat("UsuarioDoContexto > RetornarClientesDoAssessor", "Retorno de IServicoCadastroCliente: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
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
                    Codigo =  string.IsNullOrEmpty(ContextoGlobal.Usuario.Cliente.CodBovespa) ? int.Parse(ContextoGlobal.Usuario.Cliente.CodBMF) : int.Parse(ContextoGlobal.Usuario.Cliente.CodBovespa);
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
