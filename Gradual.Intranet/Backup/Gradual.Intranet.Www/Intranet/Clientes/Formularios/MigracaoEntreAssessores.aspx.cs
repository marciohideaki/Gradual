using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios
{
    public partial class MigracaoEntreAssessores : PaginaBaseAutenticada
    {
        #region | Propriedades

        public int RequestIdAssessorDe { get { return Convert.ToInt32(Request.Form["AssessorDe"]); } }

        public int RequestIdAssessorPara { get { return Convert.ToInt32(Request.Form["AssessorPara"]); } }

        private bool IsMigrarTodosOsClientes
        {
            get
            {
                var lRetorno = false;

                bool.TryParse(this.Request.Form["MigrarTodosOsClientes"], out lRetorno);

                return lRetorno;
            }
        }

        private string SessionUltimoIdAssessorBusca
        {
            get { return Convert.ToString(this.Session["UltimoIdAssessorBusca"]); }
            set { this.Session["UltimoIdAssessorBusca"] = value; }
        }

        private List<ClienteResumidoMigracaoInfo> SessionUltimoResultadoDeBusca
        {
            get { return (List<ClienteResumidoMigracaoInfo>)this.Session["UltimoResultadoDeBuscaDeMigracaoEntreAssessores"]; }
            set { this.Session["UltimoResultadoDeBuscaDeMigracaoEntreAssessores"] = value; }
        }

        private string GetIdAssessor
        {
            get { return this.Request["IdAssessor"]; }
        }

        private List<int> gIDsDosClientes = null;

        public List<int> GetIDsDosClientes
        {
            get
            {
                if (gIDsDosClientes == null)
                {
                    string[] lIds = Request.Form["IDsDosClientes"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    gIDsDosClientes = new List<int>();

                    foreach (string lId in lIds)
                    {
                        gIDsDosClientes.Add(lId.DBToInt32());
                    }
                }

                return gIDsDosClientes;
            }
        }

        private List<string> gCdBmfBovespaDosClientesSelecionados = null;

        public List<string> GetCdBmfBovespaDosClientesSelecionados
        {
            get
            {
                if (gCdBmfBovespaDosClientesSelecionados == null)
                {
                    string[] lIds = Request.Form["CdBmfBovespaDosClientesSelecionados"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    gCdBmfBovespaDosClientesSelecionados = new List<string>();

                    foreach (string lId in lIds)
                    {
                        gCdBmfBovespaDosClientesSelecionados.Add(lId);
                    }
                }

                return gCdBmfBovespaDosClientesSelecionados;
            }
        }

        private List<string> gCdSistema = null;

        public List<string> GetCdSistema
        {
            get
            {
                if (gCdSistema == null)
                {
                    string[] lIds = this.Request.Form["CdSistema"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    gCdSistema = new List<string>();

                    foreach (string lId in lIds)
                    {
                        gCdSistema.Add(lId);
                    }
                }

                return gCdSistema;
            }
        }

        public List<string> GetListTodosClientesBMFBovespaDoAssessorAtual
        {
            get
            {
                var lRetorno = new List<string>();

                if (null != this.SessionUltimoResultadoDeBusca)
                {
                    this.SessionUltimoResultadoDeBusca.ForEach(delegate(ClienteResumidoMigracaoInfo cri)
                    {
                        if (!string.IsNullOrWhiteSpace(cri.CodBovespa))
                            lRetorno.Add(cri.CodBovespa);
                    });
                }

                return lRetorno;
            }
        }

        private List<int> GetListaTodosClientesDoAssessorAtual
        {
            get
            {
                var lRetorno = new List<int>();

                if (null != this.SessionUltimoResultadoDeBusca)
                {
                    this.SessionUltimoResultadoDeBusca.ForEach(delegate(ClienteResumidoMigracaoInfo cri)
                    {
                        lRetorno.Add(cri.IdCliente);
                    });
                }

                return lRetorno;
            }
        }
        private int gTotalRegistros = 0;
        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.PopularControleComListaDoSinacor(eInformacao.Assessor, rptAssessorPara);

            base.RegistrarRespostasAjax(new string[] { "RealizarMigracao"
                                                     , "Paginar"
                                                     }
                   , new ResponderAcaoAjaxDelegate[] { ResponderRealizarMigracao
                                                     , ResponderPaginar
                                                     });
        }

        #endregion

        #region | Métodos

        private void BustarItens()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteResumidoMigracaoInfo>();
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteResumidoMigracaoInfo>();
            var lDadosDeBusca = new ClienteResumidoMigracaoInfo();

            this.SessionUltimoIdAssessorBusca = this.GetIdAssessor;

            {   //--> Configurando a busca.
                lDadosDeBusca.TipoDeConsulta = TipoDeConsultaClienteResumidoInfo.ClientesPorAssessor;
                lDadosDeBusca.TermoDeBusca = this.GetIdAssessor;
                lRequest.EntidadeCadastro = lDadosDeBusca;
                lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                lRequest.EntidadeCadastro.PaginaCorrente = 1;
            }

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoMigracaoInfo>(lRequest);

            gTotalRegistros = lResponse.Resultado[0].TotalRegistros;

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lResponse.Resultado.ForEach(cri => cri.NomeCliente = cri.NomeCliente.ToStringFormatoNome());

                this.SessionUltimoResultadoDeBusca = lResponse.Resultado;
            }
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina, string pCampoDeOrdenacao, string pDirecaoDeOrdenacao)
        {
            var lRetorno = new TransporteDeListaPaginada();
            var lLista = new List<ClienteResumidoMigracaoInfo>();
            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteResumidoMigracaoInfo>();
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteResumidoMigracaoInfo>();
            var lDadosDeBusca = new ClienteResumidoMigracaoInfo();

            lDadosDeBusca.TipoDeConsulta = TipoDeConsultaClienteResumidoInfo.ClientesPorAssessor;
            lDadosDeBusca.TermoDeBusca = this.GetIdAssessor;
            lRequest.EntidadeCadastro = lDadosDeBusca;
            lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
            lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
            lRequest.EntidadeCadastro.PaginaCorrente = pPagina;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoMigracaoInfo>(lRequest);
            
            

            switch (pCampoDeOrdenacao.ToLower())
            {
                case "idcliente":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => (a.IdCliente - b.IdCliente));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => (b.IdCliente - a.IdCliente));
                    break;
                case "cpf":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(a.CPF, b.CPF));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(b.CPF, a.CPF));
                    break;
                case "nomecliente":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(a.NomeCliente, b.NomeCliente));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(b.NomeCliente, a.NomeCliente));
                    break;
                case "tipocliente":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(a.TipoCliente, b.TipoCliente));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(b.TipoCliente, a.TipoCliente));
                    break;
                case "status":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(a.Status, b.Status));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => string.Compare(b.Status, a.Status));
                    break;
                case "datacadastrostring":
                    if ("asc".Equals(pDirecaoDeOrdenacao))
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => DateTime.Compare(a.DataCadastro, b.DataCadastro));
                    else
                        this.SessionUltimoResultadoDeBusca.Sort((a, b) => DateTime.Compare(b.DataCadastro, a.DataCadastro));
                    break;
            }

            lLista.AddRange(lResponse.Resultado);               

            lRetorno = new TransporteDeListaPaginada(lLista);

            //lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            //lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));

            if (lResponse.Resultado.Count > 0)
            {
                lRetorno.TotalDeItens = lResponse.Resultado[0].TotalRegistros;
                //lRetorno.TotalDePaginas = lResponse.Resultado[0].TotalRegistros / 20;
                lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lResponse.Resultado[0].TotalRegistros / (double)20));
            }

            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            var numCount = default(int);
            var lPagina = default(int);
            var lRetorno = string.Empty;
            var lListaPaginada = new TransporteDeListaPaginada();

            if (string.IsNullOrWhiteSpace(this.SessionUltimoIdAssessorBusca) || this.SessionUltimoIdAssessorBusca != this.GetIdAssessor)
            {
                this.BustarItens();
            }

            if (int.TryParse(this.Request["page"], out lPagina) && null != this.SessionUltimoResultadoDeBusca)
            {
                lListaPaginada = this.BuscarPaginaDeResultados(lPagina, this.Request["sidx"], this.Request["sord"]);
                numCount = this.SessionUltimoResultadoDeBusca.Count;
            }

            lRetorno = JsonConvert.SerializeObject(lListaPaginada); //base.RetornarSucessoAjax(lListaPaginada, "{0} Clientes(s) encontrado(s)", numCount);//--> O grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private string ResponderRealizarMigracao()
        {
            string lRetorno;

            SalvarEntidadeCadastroRequest<MigracaoClienteAssessorInfo> lRequest = new SalvarEntidadeCadastroRequest<MigracaoClienteAssessorInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = new MigracaoClienteAssessorInfo();
            try
            {
                if (this.IsMigrarTodosOsClientes)
                {
                    lRequest.EntidadeCadastro.Acao = MigracaoClienteAssessorAcao.MigrarClienteTodos;
                    lRequest.EntidadeCadastro.IdsClientes = this.GetListaTodosClientesDoAssessorAtual;
                    lRequest.EntidadeCadastro.CdBmfBovespaClientes = this.GetListTodosClientesBMFBovespaDoAssessorAtual;
                    lRequest.EntidadeCadastro.CdSistema = this.GetCdSistema;

                }
                else if (1.Equals(this.GetIDsDosClientes.Count))
                {
                    lRequest.EntidadeCadastro.Acao = MigracaoClienteAssessorAcao.MigrarClienteUnico;
                    lRequest.EntidadeCadastro.IdCliente = this.GetIDsDosClientes[0];
                    lRequest.EntidadeCadastro.CdBmfBovespaClientes = this.GetCdBmfBovespaDosClientesSelecionados;
                    lRequest.EntidadeCadastro.CdSistema = this.GetCdSistema;
                }
                else
                {
                    lRequest.EntidadeCadastro.Acao = MigracaoClienteAssessorAcao.MigrarClienteParcial;
                    lRequest.EntidadeCadastro.IdsClientes = this.GetIDsDosClientes;
                    lRequest.EntidadeCadastro.CdBmfBovespaClientes = this.GetCdBmfBovespaDosClientesSelecionados;
                    lRequest.EntidadeCadastro.CdSistema = this.GetCdSistema;
                }

                lRequest.EntidadeCadastro.IdAssessorOrigem = this.RequestIdAssessorDe;
                lRequest.EntidadeCadastro.IdAssessorDestino = this.RequestIdAssessorPara;
                lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                try
                {
                    lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<MigracaoClienteAssessorInfo>(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax("Migração realizada com sucesso!");
                        this.SessionUltimoIdAssessorBusca = null;

                        base.RegistrarLogAlteracao(string.Format("Migração de assessores de (id) {0} para {1}", this.RequestIdAssessorDe.ToString(), this.RequestIdAssessorPara.ToString()));
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(string.Format("Erro durante a execução da migração: [{0}]", lResponse.StatusResposta.ToString(), lResponse.DescricaoResposta));
                    }
                }
                catch (Exception exRequest)
                {
                    lRetorno = RetornarErroAjax("Erro ao realizar migração", exRequest);
                }
            }
            catch (Exception exParams)
            {
                lRetorno = RetornarErroAjax("Erro de parametrização", exParams);
            }

            return lRetorno;
        }

        #endregion
    }
}
