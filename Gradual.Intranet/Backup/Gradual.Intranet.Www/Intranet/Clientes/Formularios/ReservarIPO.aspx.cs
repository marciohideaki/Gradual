using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Clientes.Formularios
{
    public partial class ReservarIPO : PaginaBaseAutenticada
    {
        private List<ClienteResumidoInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<ClienteResumidoInfo>)Session["UltimoResultadoDeBuscaDeClientesIPO"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDeClientesIPO"] = value;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                , "Paginar"
                                                , "ReservarIPO"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderBuscarItensParaListagemSimples
                                                , ResponderPaginar
                                                , ResponderReservarIPO
                                                });
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<TransporteDadosResumidosCliente> lLista = new List<TransporteDadosResumidosCliente>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(new TransporteDadosResumidosCliente(this.SessionUltimoResultadoDeBusca[a]));
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            if (this.SessionUltimoResultadoDeBusca != null)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    lLista = BuscarPaginaDeResultados(lPagina);
                }
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = "Erro...";

            ConsultarEntidadeCadastroRequest<ClienteResumidoInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteResumidoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteResumidoInfo> lResponse = new ConsultarEntidadeCadastroResponse<ClienteResumidoInfo>();

            ClienteResumidoInfo lDadosDeBusca = new ClienteResumidoInfo();

            string lTermoDeBusca, lBuscarPor, lTipo;

            lTermoDeBusca = Request.Form["TermoDeBusca"];
            lBuscarPor = Request.Form["BuscarPor"];
            lTipo = Request.Form["Tipo"];

            lDadosDeBusca.TipoCliente = lTipo;

            lDadosDeBusca.TermoDeBusca = lTermoDeBusca;

            OpcoesBuscarPor lOpcoesBuscarPor;

            if (Enum.TryParse<OpcoesBuscarPor>(lBuscarPor, true, out lOpcoesBuscarPor))
                lDadosDeBusca.OpcaoBuscarPor = lOpcoesBuscarPor;
            else
                lDadosDeBusca.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;

            lDadosDeBusca.OpcaoStatus = OpcoesStatus.Ativo | OpcoesStatus.Inativo;

            lDadosDeBusca.OpcaoPasso = OpcoesPasso.Exportado;

            lDadosDeBusca.OpcaoPendencia = OpcoesPendencia.ComPendenciaCadastral | OpcoesPendencia.ComSolicitacaoAlteracao | 0;

            lRequest.EntidadeCadastro = lDadosDeBusca;

            try
            {
                ReceberEntidadeCadastroRequest<LoginInfo> lEntradaLogin = new ReceberEntidadeCadastroRequest<LoginInfo>();
                lEntradaLogin.EntidadeCadastro = new LoginInfo() { IdLogin = base.UsuarioLogado.Id };
                ReceberEntidadeCadastroResponse<LoginInfo> lRetornoLogin = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lEntradaLogin);
                lRequest.EntidadeCadastro.CodAssessor = lRetornoLogin.EntidadeCadastro.CdAssessor;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    lResponse.Resultado.ForEach(cri => cri.NomeCliente = cri.NomeCliente.ToStringFormatoNome()); //--> Normalizando o nome dos clientes.

                    lResponse.Resultado.Sort((a, b) => string.Compare(a.NomeCliente.Trim(), b.NomeCliente.Trim())); //--> Ordenando o resultado por nome.

                    this.SessionUltimoResultadoDeBusca = lResponse.Resultado;
                    rowLinhaDeNenhumItem.Visible = !(lResponse.Resultado.Count > 0);

                    TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                    lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontrados [{0}] clientes", lResponse.Resultado.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro durante a busca.", string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}", lResponse.StatusResposta, lResponse.StatusResposta, lResponse.DescricaoResposta));
                }
            }
            catch (Exception exBusca)
            {
                RetornarErroAjax("Erro durante a busca", exBusca);
            }

            return lRetorno;
        }

        private string ResponderReservarIPO()
        {
            string CodBovespa   = Request["CodBovespa"];
            string lNomecliente = Request["NomeCliente"];
            string lCpfCnpj     = Request["CpfCnpj"];

            string url = new System.Configuration.AppSettingsReader().GetValue("UrlReservaIPO", typeof(string)).ToString() + CodBovespa;

            url += "&NOME=" + lNomecliente + "&CPFCNPJ=" + lCpfCnpj;

            SalvarEntidadeCadastroRequest<ReservaIPOInfo> lLogReserva = new SalvarEntidadeCadastroRequest<ReservaIPOInfo>()
            {   //Log
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                EntidadeCadastro = new ReservaIPOInfo()
                {
                    CdCodigo = int.Parse(CodBovespa)
                }
            };

            string lRetorno = "";

            try
            {
                var lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ReservaIPOInfo>(lLogReserva);
                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(url);

                    base.RegistrarLogInclusao(string.Concat("Reservando IPO para cliente: cd_cblc = ", CodBovespa));
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception exEnvioRequest)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do request para salvar os dados", exEnvioRequest);
            }
            return lRetorno;
        }
    }
}
