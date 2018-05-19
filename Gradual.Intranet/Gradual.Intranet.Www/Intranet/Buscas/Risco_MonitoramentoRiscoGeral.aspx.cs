using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Risco_MonitoramentoRiscoGeral : PaginaBase
    {

        public string gIdAsessorLogado { get; set;}

        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "BuscarItensParaSelecao",
                                                       "DeletarJanela",
                                                       "ListarJanelasParametros",
                                                       "BuscarAssessorConectado"
                                                     },
                     new ResponderAcaoAjaxDelegate[] {  this.ResponderBuscarItensParaListagemSimples,
                                                        this.RespoderDeletarJanela,
                                                        this.ResponderListarJanelasParametros,
                                                        this.ResponderBuscarAssessorConectado
                                                     });
            
            base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptBM_FiltroRelatorio_CodAssessor);

        }

        private string ResponderConfigurarAssessorCombo()
        {
            string lResposta = string.Empty;


            return lResposta;
        }

        private string ResponderListarJanelasParametros()
        { 
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lRequest = new ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRequest.Objeto = new MonitoramentoRiscoLucroPrejuizoParametrosInfo();

            lRequest.Objeto.IdUsuario = base.UsuarioLogado.Id;

            var lResponse = new PersistenciaDbIntranet().ConsultarObjetos<MonitoramentoRiscoLucroPrejuizoParametrosInfo>(lRequest);

            lRetornoLista = new TransporteDeListaPaginada(lResponse.Resultado);

            lRetorno = base.RetornarSucessoAjax(lResponse.Resultado, "Paginas Listadas com sucesso!!");

            return lRetorno;
        }

        private string ResponderBuscarAssessorConectado()
        {
            if (base.UsuarioLogado == null)
            {
                return base.RetornarErroAjax(RESPOSTA_SESSAO_EXPIRADA);
            }
            else if (base.CodigoAssessor != null)
            {
                gIdAsessorLogado = base.CodigoAssessor.ToString();

                return base.RetornarSucessoAjax((object)gIdAsessorLogado,"Há assessor conectado");
            }

            return base.RetornarSucessoAjax(base.CodigoAssessor.HasValue ? base.CodigoAssessor.ToString() :  "Não há assessor conectado");
        }

        private string RespoderDeletarJanela()
        {
            string lRetorno = string.Empty;

            return lRetorno;
        }

        private string ResponderBuscarItensParaListagemSimples()
        {
            string lRetorno = string.Empty;

            ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo> lRequest = new ConsultarEntidadeRequest<MonitoramentoRiscoLucroPrejuizoParametrosInfo>();

            lRequest.Objeto.IdUsuario = base.UsuarioLogado.Id;

            var lResponse = new PersistenciaDbIntranet().ConsultarObjetos<MonitoramentoRiscoLucroPrejuizoParametrosInfo>(lRequest);

            //new TransporteParametrosMonitoramentoRisco(lResponse.Resultado)

            lRetorno = JsonConvert.SerializeObject(lResponse); 

            return lRetorno;
        }
    }
}