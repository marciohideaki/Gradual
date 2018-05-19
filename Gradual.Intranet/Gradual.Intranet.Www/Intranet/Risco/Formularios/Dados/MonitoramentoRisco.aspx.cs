using System;
using System.Text;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class MonitoramentoDeRisco : PaginaBase
    {
        #region | Propriedades

        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CdAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CdCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdGrupo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CdGrupo"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdParametro
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CdParametro"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private bool GetMaior75Menor100
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request["CkMaior75Menor100"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetMaior50Menor75
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request["CkMaior50Menos75"], out lRetorno);

                return lRetorno;
            }
        }

        private bool GetMenor50
        {
            get
            {
                var lRetorno = default(bool);

                bool.TryParse(this.Request["CkMenor50"], out lRetorno);

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao"
                                                     , "CarregarHtmlComDados"
                                                     , "ExportarParaExcel"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderBuscarItensParaListagemSimples
                                                     , this.ResponderExportarParaExcel
                                                     });

            if (string.IsNullOrWhiteSpace(base.Acao))
                this.ResponderCarregarHtmlComDados();
        }

        #endregion

        #region | Métodos

        private string ResponderExportarParaExcel()
        {
            var lConteudoArquivo = new StringBuilder();
            lConteudoArquivo.Append("Status\tCliente\tAssessor\tParâmetro\tGrupo\tLimite (R$)\tAlocado (R$)\tDisponível(R$)\n");

            var lResultado = base.ServicoRegrasRisco.ListarMonitoramentoDeRisco(
                new ListarMonitoramentoRiscoRequest()
                {
                    FiltroCodigoAssessor = this.GetCdAssessor,
                    FiltroCodigoCliente = this.GetCdCliente,
                    FiltroGrupoAlavancagem = this.GetCdGrupo,
                    FiltroParametro = this.GetCdParametro,
                });

            if (lResultado.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRiscoMonitoramento().TraduzirLista(lResultado.Resultado);

                this.FiltrarPorCriticidade(lTransporte).ListaExposicaoRisco.ForEach(exr =>
                {
                    lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\n"
                        , exr.Criticidade.Replace("Semaforo", string.Empty), exr.Cliente, exr.Assessor, exr.Parametro, exr.Grupo, exr.ValorLimite, exr.ValorAlocado, exr.ValorDisponivel);
                });
            }

            this.Response.Clear();

            this.Response.ContentType = "text/xls";

            this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            this.Response.Charset = "iso-8859-1";

            this.Response.AddHeader("content-disposition", string.Format("attachment;filename=MonitoramentoDeRisco_{0}.xls", DateTime.Now.ToString("dd-MM-yyyy_HH-mm")));

            this.Response.Write(lConteudoArquivo.ToString());

            this.Response.End();

            return base.RetornarSucessoAjax("Sucesso");
        }

        private string ResponderCarregarHtmlComDados()
        {
            {   //--> Montando o combo parâmetros.
                var lListaParametrosAlavancagem = base.ServicoRegrasRisco.ListarParametrosRisco(new ListarParametrosRiscoRequest());
                var lTransporteParametrosAlavancagem = new TransporteRiscoParametro().TraduzirLista(lListaParametrosAlavancagem.ParametrosRisco);

                this.rptRisco_Monitoramento_FiltroParametro.DataSource = lTransporteParametrosAlavancagem;
                this.rptRisco_Monitoramento_FiltroParametro.DataBind();
            }

            var lListaGruposAlavancagem = base.ServicoRegrasRisco.ListarGrupos(new ListarGruposRequest()
            {
                FiltroTipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem
            });

            var lTransporteGruposAlavancagem = new TransporteRiscoGrupo().TraduzirLista(lListaGruposAlavancagem.Grupos);

            this.rptRisco_Monitoramento_FiltroGrupoAlavancagem.DataSource = lTransporteGruposAlavancagem;
            this.rptRisco_Monitoramento_FiltroGrupoAlavancagem.DataBind();

            return string.Empty;
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            var lRetorno = string.Empty;

            var lResultado = base.ServicoRegrasRisco.ListarMonitoramentoDeRisco(
                new ListarMonitoramentoRiscoRequest()
                {
                    FiltroCodigoAssessor = this.GetCdAssessor,
                    FiltroCodigoCliente = this.GetCdCliente,
                    FiltroGrupoAlavancagem = this.GetCdGrupo,
                    FiltroParametro = this.GetCdParametro,
                });

            if (lResultado.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRiscoMonitoramento().TraduzirLista(lResultado.Resultado);

                lTransporte.DataHoraConsulta = string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm:ss"));

                lRetorno = base.RetornarSucessoAjax(FiltrarPorCriticidade(lTransporte), "Sucesso");
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição", lResultado.DescricaoResposta);
            }

            return lRetorno;
        }

        private TransporteRiscoMonitoramento FiltrarPorCriticidade(TransporteRiscoMonitoramento pParametro)
        {
            var lRetorno = new TransporteRiscoMonitoramento();

            lRetorno.DataHoraConsulta = pParametro.DataHoraConsulta;

            if (this.GetMaior75Menor100)
                lRetorno.ListaExposicaoRisco.AddRange(pParametro.ListaExposicaoRisco.FindAll(ler => { return ler.Criticidade == "SemaforoVermelho"; }));

            if (this.GetMaior50Menor75)
                lRetorno.ListaExposicaoRisco.AddRange(pParametro.ListaExposicaoRisco.FindAll(ler => { return ler.Criticidade == "SemaforoAmarelo"; }));

            if (this.GetMenor50)
                lRetorno.ListaExposicaoRisco.AddRange(pParametro.ListaExposicaoRisco.FindAll(ler => { return ler.Criticidade == "SemaforoVerde"; }));

            return lRetorno;
        }

        #endregion
    }
}
