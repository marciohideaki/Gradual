using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PoupeDirect.Lib;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using Gradual.OMS.PoupeDirect.Lib.Util;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Solicitacoes;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.PoupeDirect
{
    public partial class PoupeAplicacaoResgate : PaginaBaseAutenticada
    {
        private List<int> GetIdsSelecionadosResgate
        {
            get
            {
                var lRetorno = new List<int>();

                string lIdsResgate = this.Request.Form["IdsResgate"];

                if (!string.IsNullOrWhiteSpace(lIdsResgate))
                {
                    var lIdsResgateArray = lIdsResgate.TrimEnd(',').Split(',');

                    for (int i = 0; i < lIdsResgateArray.Length; i++)
                        lRetorno.Add(lIdsResgateArray[i].DBToInt32());
                }

                return lRetorno;
            }
        }

        private List<AplicacaoInfo> GetSelecaoAplicacao
        {
            get
            {
                return (List<AplicacaoInfo>)Session["SelecaoAplicacao"];
            }
            set
            {
                Session["SelecaoAplicacao"] = value;
            }
        }

        private List<ResgateInfo> GetSelecaoResgate
        {
            get
            {
                return (List<ResgateInfo>)Session["SelecaoResgate"];
            }
            set
            {
                Session["SelecaoResgate"] = value;
            }
        }

        private DateTime GetDataInicial
        {
            get 
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(Request["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(Request["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }
        
        private string GetStaus
        {
            get
            {

                return Request["status"];
            }
        }

        private string GetCodigoCliente
        {
            get
            {

                return Request["CodigoCliente"];
            }
        }
        
        private List<int> GetIdsSelecionadosAplicacao
        {
            get
            {
                var lRetorno = new List<int>();

                string lIdsAplicacao = this.Request.Form["IdsAplicacao"];

                if (!string.IsNullOrWhiteSpace(lIdsAplicacao))
                {
                    var lIdsAplicacaoArray = lIdsAplicacao.TrimEnd(',').Split(',');

                    for (int i = 0; i < lIdsAplicacaoArray.Length; i++)
                        lRetorno.Add(lIdsAplicacaoArray[i].DBToInt32());
                }

                return lRetorno;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            this.btnSolicitacao_AtualizarAplicacao.Visible  = base.UsuarioPode("Salvar", "f4c25f54-8736-405b-a871-c182f38fab26");
            this.btnSolicitacao_AtualizarResgate.Visible    = base.UsuarioPode("Salvar", "f4c25f54-8736-405b-a871-c182f38fab26");



            base.RegistrarRespostasAjax(new string[] { "AtualizarResgate",
                                                        "AtualizarAplicacao",
                                                        "SelecionarAprovacoes",
                                                        "CarregarAplicacao",
                                                        "CarregarResgate"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { AtualizarResgate,
                                                        AtualizarAplicacao,
                                                        SelecionarAprovacoes,
                                                        BuscarAprovacaoAplicacao,
                                                        BuscarAprovacaoResgate
                                                     });


            
            if (!this.IsPostBack)
            {
                    this.CarregarStatus();
            }
        }

        public string AtualizarAplicacao()
        {
            string lRetorno = string.Empty;

            List<int> CodigoAprovado = new List<int>();

            List<int> CodigoResgateSelecionado = this.GetIdsSelecionadosAplicacao;

            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            AplicacaoRequest request = new AplicacaoRequest();

            if (CodigoResgateSelecionado.Count > 0)
            {
                foreach (int item in CodigoResgateSelecionado)//varre todas as aplicações selecionadas
                {
                    request.Aplicacao = new AplicacaoInfo();

                    request.Aplicacao.CodigoAplicacao = item;

                    var lRetornoAplicacao = lServico.SelecionarAplicacao(request);//seleciona os dados de uma aplicação

                    if (lRetornoAplicacao.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        if (this.AprovarAplicacao(lRetornoAplicacao.ListaAplicacao[0]))//altera e salva uma aplicação.
                        {
                            lRetorno = "Dados alterados com sucesso";
                            CodigoAprovado.Add((int)item);
                        }
                        else
                            lRetorno = "Erro ao salvar Aplicação.";
                    }
                    else
                        lRetorno = "Erro ao selecionar Aplicação.";
                }
            }
            else
                lRetorno = "Por favor, selecione um item.";

            return RetornarSucessoAjax(CodigoAprovado, lRetorno);
        }

        private string AtualizarResgate()
        {
            string lRetorno = string.Empty;
            List<int> CodigoAprovado =  new List<int>();

            List<int> CodigoResgateSelecionado = this.GetIdsSelecionadosResgate;

            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            ResgateRequest request = new ResgateRequest();

            try
            {
                if (CodigoResgateSelecionado.Count > 0)
                {
                    foreach (int item in CodigoResgateSelecionado)//varre todos os resgates selecionados
                    {
                        request.Resgate = new ResgateInfo();
                        request.Resgate.CodigoResgate = item;

                        var lRetornoResgate = lServico.SelecionarResgate(request);//busca os dados do resgate a ser alterado.

                        if (lRetornoResgate.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                        {
                            if (this.AprovarResgate(lRetornoResgate.ListaResgate[0]))//altera e salva um resgate.
                            {
                                CodigoAprovado.Add((int)item);
                                lRetorno = "Dados alterados com sucesso.";
                            }
                            else
                                lRetorno = "Erro ao selecionar o resgate.";
                        }
                        else
                            lRetorno = "Erro ao salvar o resgate.";
                    }
                }
                else
                    lRetorno = "Por favor, selecione um resgate.";
            }
            catch
            {
                lRetorno = "Erro ao salvar o resgate.";
            }

            return RetornarSucessoAjax(CodigoAprovado, lRetorno);
        }

        private string SelecionarAprovacoes()
        {

            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            TransporteDeListaPaginada listaAplicacao = new TransporteDeListaPaginada(); 

            var lRetornoAplicacao = lServico.SelecionarAplicacao(this.PreencherAplicacaoComFiltrosTela());
            
            var lRetornoResgate = lServico.SelecionarResgate(this.PreencherResgateComFiltrosTela());
                 
            if (lRetornoAplicacao.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK && lRetornoResgate.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                
                this.GetSelecaoAplicacao = lRetornoAplicacao.ListaAplicacao;

                this.GetSelecaoResgate = lRetornoResgate.ListaResgate;

                return RetornarSucessoAjax(new TransporteDeListaPaginada(new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(lRetornoAplicacao.ListaAplicacao), new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(lRetornoResgate.ListaResgate)), "Dados Carregados com sucesso!");
                
            }
            else
                return RetornarSucessoAjax("Erro");


            
        }

        private ResgateRequest PreencherResgateComFiltrosTela()
        {
            ResgateRequest requesResgate = new ResgateRequest();
            requesResgate.Resgate = new ResgateInfo();

            if (this.GetStaus != string.Empty)
                requesResgate.Resgate.CodigoStatus = (EnumPoupeDirect.EnumStatus)Enum.Parse(typeof(EnumPoupeDirect.EnumStatus), this.GetStaus);
            if (this.GetCodigoCliente != string.Empty)
                requesResgate.Resgate.CodigoCliente = Convert.ToInt32(this.GetCodigoCliente);

            requesResgate.Resgate.DtInicialSolicitacao = this.GetDataInicial;

            requesResgate.Resgate.DtFinalSolicitacao = this.GetDataFinal;

            return requesResgate;
        }

        private AplicacaoRequest PreencherAplicacaoComFiltrosTela()
        {
            AplicacaoRequest request = new AplicacaoRequest();
            request.Aplicacao = new AplicacaoInfo();

            if (this.GetStaus != string.Empty)
                request.Aplicacao.CodigoStatus = (EnumPoupeDirect.EnumStatus)Enum.Parse(typeof(EnumPoupeDirect.EnumStatus), this.GetStaus);
            if (this.GetCodigoCliente != string.Empty)
                request.Aplicacao.CodigoCliente = Convert.ToInt32(this.GetCodigoCliente);

            request.Aplicacao.DtInicialSolicitacao = this.GetDataInicial;

            request.Aplicacao.DtFinalSolicitacao = this.GetDataFinal;

            return request;

            
        }

        private String BuscarAprovacaoAplicacao()
        {

            return JsonConvert.SerializeObject(new TransporteAplicacaoResgate(new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(this.GetSelecaoAplicacao), new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(this.GetSelecaoResgate)));
           
        }

        private string BuscarAprovacaoResgate()
        {
            return JsonConvert.SerializeObject(new TransporteAplicacaoResgate(new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(this.GetSelecaoAplicacao), new TransporteAplicacaoResgate().ToListTransporteResultadoAplicacaoResgate(this.GetSelecaoResgate)));
        }
        
        private bool AprovarResgate(ResgateInfo pResgate)
        {
            bool retorno = false;
            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();
            ResgateRequest request = new ResgateRequest();

            try
            {
                request.Resgate = pResgate;
                request.Resgate.CodigoStatus = EnumPoupeDirect.EnumStatus.EFETIVADO;
                request.Resgate.DtEfetivacao = DateTime.Now;

                ResgateResponse lResposta = lServico.InserirAtualizarResgate(request);

                if (lResposta.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    retorno = true;
            }
            catch
            {
                retorno = false;
            }

            

            return retorno;
        }

        private bool AprovarAplicacao(AplicacaoInfo pAplicacao)
        {
            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();
            AplicacaoRequest request = new AplicacaoRequest();
            bool retorno = false;

            try
            {

                request.Aplicacao = pAplicacao;

                request.Aplicacao.CodigoStatus = EnumPoupeDirect.EnumStatus.EFETIVADO;
                request.Aplicacao.DtEfetivacao = DateTime.Now;

                AplicacaoResponse lResposta = lServico.InserirAtualizarAplicacao(request);

                if (lResposta.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    retorno = true;
            }
            catch (Exception)
            {
                retorno = false;
            }

            return retorno;
        }

        private void CarregarStatus()
        {
            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            StatusRequest request = new StatusRequest();
            request.Status = new StatusInfo();
            
            var lRetornoResgate = lServico.SelecionarStatusAplicacaoResgate(request);

            if (lRetornoResgate.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                this.rptStatus_FiltroAplicacaoResgate.DataSource = lRetornoResgate.ListaStatus;
                this.rptStatus_FiltroAplicacaoResgate.DataBind();
            }

        }
    }
}
