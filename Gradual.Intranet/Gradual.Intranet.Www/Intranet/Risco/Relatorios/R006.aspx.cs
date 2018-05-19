using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.ConsolidadorRelatorioCCLib;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R006 : PaginaBaseAutenticada
    {
        #region | Atributos

        public static DateTime gDataHoraUltimaAtualizacao;

        public static string gSubTotalD0;

        public static string gSubTotalD1;

        public static string gSubTotalD2;

        public static string gSubTotalD3;

        public static string gSubTotalCM;

        #endregion

        #region | Propriedades

        private int GetTipoBusca
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["TipoBusca"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetParametroBusca
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["ParametroBusca"]))
                    return string.Empty;

                return this.Server.HtmlDecode(this.Request.Form["ParametroBusca"]);
            }
        }

        private int? GetAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private bool GetD0
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["D0"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetD1
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["D1"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetD2
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["D2"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetD3
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["D3"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetContaMargem
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request.Form["CM"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetOrdenarPor
        {
            get
            {
                var lRetorno = string.Empty;

                lRetorno = this.Request.Form["OrdenarPor"];

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if (this.Acao == "BuscarItensParaListagemSimples")
                {
                    this.ResponderBuscarItensParaListagemSimples();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lServicoConsolidadorRelatorioCC = Ativador.Get<IServicoConsolidadorRelatorioCC>();
            var lClienteResumido = new ConsultarEntidadeCadastroResponse<ClienteResumidoInfo>();
            var lIdClientePesquisa = default(int);
            var lListaCpfCnpjConsulta = new List<string>();

            //--> Caro Programador, a condição abaixo realiza pesquisa por dados do cliente.
            if (!string.IsNullOrEmpty(this.GetParametroBusca))
                switch ((OpcoesBuscarPor)this.GetTipoBusca)
                {
                    case OpcoesBuscarPor.CodBovespa:
                        lIdClientePesquisa = 0.Equals(this.GetParametroBusca.DBToInt32()) ? -1 : this.GetParametroBusca.DBToInt32();
                        break;
                    case OpcoesBuscarPor.CpfCnpj:
                        lListaCpfCnpjConsulta.Add(this.GetParametroBusca);
                        break;
                    case OpcoesBuscarPor.NomeCliente: //--> Pesquisando no serviço de cliente resumido (mesmo do cadastro).
                        lClienteResumido = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoInfo>(
                            new ConsultarEntidadeCadastroRequest<ClienteResumidoInfo>(
                                new ClienteResumidoInfo()
                                {
                                    OpcaoBuscarPor = (OpcoesBuscarPor)this.GetTipoBusca,
                                    TermoDeBusca = this.GetParametroBusca,
                                }) {    DescricaoUsuarioLogado = base.UsuarioLogado.Nome , IdUsuarioLogado = base.UsuarioLogado.Id });

                        if (lClienteResumido.Resultado.Count > 0)
                            lClienteResumido.Resultado.ForEach(delegate(ClienteResumidoInfo cri) { lListaCpfCnpjConsulta.Add(cri.CPF); });

                        break;
                }

            //--> Realizando a consulta.
            var lConsulta = lServicoConsolidadorRelatorioCC.ConsultarSaldoCCProjetado(
                new Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria.SaldoContaCorrenteRiscoRequest()
                {
                    ConsultaPosicaoD0 = this.GetD0,
                    ConsultaPosicaoD1 = this.GetD1,
                    ConsultaPosicaoD2 = this.GetD2,
                    ConsultaPosicaoD3 = this.GetD3,
                    ConsultaContaMargem = this.GetContaMargem,
                    ConsultaIdAssessor = this.GetAssessor,
                    IdCliente = lIdClientePesquisa,
                    ConsultaClientesCpfCnpj = lListaCpfCnpjConsulta
                    
                });

            if (OMS.ConsolidadorRelatorioCCLib.Enum.CriticaMensagemEnum.OK.Equals(lConsulta.StatusResposta)
            && (null != lConsulta.ObjetoLista && lConsulta.ObjetoLista.Count > 0))
            {
                var lTransporteRelatorio_006 = new TransporteRelatorio_006();
                var listaTransporte = lTransporteRelatorio_006.TraduzirLista(lConsulta.ObjetoLista, this.GetOrdenarPor);

                {   //--> Atribuindo dados no grid.
                    base.PopularComboComListaGenerica<TransporteRelatorio_006>(listaTransporte, this.rptRelatorio);
                    this.rowLinhaDeNenhumItem.Visible = false;
                }
                {   //--> Definindo os valores dos subtotais.
                    gSubTotalD0 = lTransporteRelatorio_006.D0Total.ToString("N2");
                    gSubTotalD1 = lTransporteRelatorio_006.D1Total.ToString("N2");
                    gSubTotalD2 = lTransporteRelatorio_006.D2Total.ToString("N2");
                    gSubTotalD3 = lTransporteRelatorio_006.D3Total.ToString("N2");
                    gSubTotalCM = lTransporteRelatorio_006.CMTotal.ToString("N2");
                }
            }
            else
            {
                gSubTotalD0 = "0,00";
                gSubTotalD1 = "0,00";
                gSubTotalD2 = "0,00";
                gSubTotalD3 = "0,00";
                gSubTotalCM = "0,00";
                this.rowLinhaDeNenhumItem.Visible = true;
            }

            //--> Setando a data e hora da última atualização do relatorio
            gDataHoraUltimaAtualizacao = lServicoConsolidadorRelatorioCC.ConsultarDataHoraUltimaAtualizacao();
        }

        #endregion
    }
}