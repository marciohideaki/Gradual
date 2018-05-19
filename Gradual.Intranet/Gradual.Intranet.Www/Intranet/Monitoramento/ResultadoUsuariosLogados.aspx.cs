using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.Intranet.Monitoramento
{
    public partial class ResultadoUsuariosLogados : PaginaBaseAutenticada
    {
        private string ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                var lRequest = new ListarSessoesRequest()
                {
                    CodigoSessao = this.CodigoSessao
                };

                ListarSessoesResponse lResponse = ServicoSeguranca.ListarSessoes(lRequest);

                if(lResponse.Sessoes.Count > 0)
                {
                    List<TransporteUsuariosLogados> lista = new TransporteUsuariosLogados().ToListTransporteUsuariosLogados(lResponse);

                    this.rowLinhaDeNenhumItem.Visible = false;
                    this.rptUsuariosLogados.DataSource = lista;
                    this.rptUsuariosLogados.DataBind();

                    base.RegistrarLogConsulta();
                }
                else
                {
                    this.rowLinhaDeNenhumItem.Visible = true;
                }
            }
            catch (Exception exBusca)
            {
                base.RetornarErroAjax("Erro ao buscar os usuários logados", exBusca);
            }

            return string.Empty;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            //if (this.Acao == "BuscarItensParaListagemSimples")
            //{
                ResponderBuscarItensParaListagemSimples();
            //}
            //else
            //{
            //    RegistrarRespostasAjax(new string[]{ 
            //                                    "ExcluirOrdens"
            //                                   },
            //        new ResponderAcaoAjaxDelegate[]{
            //                                     ResponderExcluirOrdens
            //                                   });
            //}

        }
    }
}