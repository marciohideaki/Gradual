﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using Gradual.Core.Ordens.Lib;
//using Gradual.OMS.Ordens.Lib;
//using Gradual.OMS.Ordens.Lib.Info;
//using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Newtonsoft.Json;
using System.Configuration;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.Core.OMS.FixServerLowLatency.Lib;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;

namespace Gradual.Intranet.Www.Intranet.Monitoramento
{
    public partial class ResultadoOrdensSpider : PaginaBaseAutenticada
    {
        #region | Propriedades

        /// <summary>
        /// Request de busca de rodens
        /// </summary>
        BuscarOrdensRequest gRequest = new BuscarOrdensRequest();

        private int PortaBovespaGTIClienteHFT1
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ChannelIdBovespaGTI3Cliente"].ToString());
            }
        }

        private int PortaBovespaGTIClienteHFT2
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ChannelIdBovespaGTI4Cliente"].ToString());
            }
        }

        private int PortaBovespaGTICliente
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ChannelIdBovespaGTI2Cliente"].ToString());
            }
        }

        private int PortaBovespaGTIAssessor
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ChannelIdBovespaGTI2Assessor"].ToString());
            }

        }

        private int PortaBovespaHB
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ChannelIdBovespaHB"].ToString());
            }

        }

        /// <summary>
        /// Para ser usado no filtro da bolsa
        /// </summary>
        private List<OrdemInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<OrdemInfo>)Session["UltimoResultadoDeBuscaDeOrdensSpider"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDeOrdensSpider"] = value;
            }
        }

        private int? GetIdSistemaOrigem
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["sistema"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        /// <summary> 
        /// Páginação corrente do grid de ordens quando 
        /// efetuado o filtro de origem no sinacor
        /// </summary>
        private int GetPaginaCorrente
        {
            get
            {
                return ViewState["PaginaCorrente"] == null ? 1 : (int)ViewState["PaginaCorrente"];
            }
            set
            {

                ViewState.Add("PaginaCorrente", value);
            }
        }

        /// <summary>
        /// Quantidade limitada de registros por página no grid
        /// </summary>
        private int QtdeRegistros
        {
            get
            {
                return ViewState["QtdeRegistros"] == null ? 20 : (int)ViewState["QtdeRegistros"];
            }
            set
            {
                ViewState.Add("QtdeRegistros", value);
            }
        }

        /// <summary>
        /// Retorna a hora com a data inicial do filtro
        /// </summary>
        private DateTime GetDataHoraInicial
        {
            get
            {
                var lRetorno = DateTime.MinValue;
                var lDataHora = this.Request.Form["DataInicial"];

                if (!string.IsNullOrWhiteSpace(this.Request.Form["DataInicial"]))
                {
                    Session.Add("DataInicial", this.Request.Form["DataInicial"]);
                }

                if (Session["DataInicial"] != null)
                    lDataHora = Session["DataInicial"].ToString();

                if (!string.IsNullOrWhiteSpace(this.Request.Form["HoraInicial"]))
                {
                    Session.Add("HoraInicial", this.Request.Form["HoraInicial"]);
                }

                if (Session["HoraInicial"] != null)
                    lDataHora += string.Concat(" ", Session["HoraInicial"].ToString());

                DateTime.TryParse(lDataHora.Trim(), out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Retorna a data e a hora final do filtro
        /// </summary>
        private DateTime GetDataHoraFinal
        {
            get
            {
                var lRetorno = DateTime.MinValue;
                var lDataHora = this.Request.Form["DataFinal"];

                if (!string.IsNullOrWhiteSpace(this.Request.Form["DataFinal"]))
                {
                    Session.Add("DataFinal", this.Request.Form["DataFinal"]);
                }

                if (Session["DataFinal"] != null)
                    lDataHora = Session["DataFinal"].ToString();

                if (!string.IsNullOrWhiteSpace(this.Request.Form["HoraFinal"]))
                    Session.Add("HoraFinal", this.Request.Form["HoraFinal"]);

                else if (Session["HoraFinal"] == null)
                    Session.Add("HoraFinal", "23:59:59");

                if (Session["HoraFinal"] != null)
                    lDataHora += string.Concat(" ", Session["HoraFinal"].ToString());

                DateTime.TryParse(lDataHora.Trim(), out lRetorno);

                return lRetorno.AddDays(1);
            }
        }

        /// <summary>
        /// Retorna a origem do filtro
        /// </summary>
        private string GetOrigem
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Origem"]) && Session["Origem"] == null)
                    return null;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["Origem"]))
                    this.Session.Add("Origem", this.Request.Form["Origem"].Trim().ToUpper());

                return this.Session["Origem"].ToString();
            }
        }

        /// <summary>
        /// Retorna o tipo de bolsa efetuada no filtro
        /// </summary>
        private int? GetBolsa
        {
            get
            {
                int? lRetorno = null;

                if (this.Request.Form["Bolsa"].Trim().ToLower().Equals("bol"))
                {
                    if (this.GetOrigem.Equals("GTI") || this.GetOrigem.Equals("HB") || this.GetOrigem.Equals("FIXSERVER"))
                    {
                        if (base.CodigoAssessor != null)
                        {
                            lRetorno = this.PortaBovespaGTIAssessor;  //Desktop Assessor
                        }
                        else
                        {
                            lRetorno = this.PortaBovespaGTICliente;  // desktop Cliente
                        }
                    }
                }
                else
                {
                    lRetorno = 0;
                }

                Session.Add("Bolsa", lRetorno);

                return Session["Bolsa"].DBToInt32();
            }
        }

        private int GetBolsaSinacor
        {
            get
            {
                int lRetorno = 1;

                if (string.IsNullOrWhiteSpace(this.Request.Form["Bolsa"]) && Session["Bolsa"] == null)
                {
                    lRetorno = 1;

                    Session.Add("Bolsa", lRetorno);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this.Request.Form["Bolsa"]) && Session["Bolsa"] != null)
                        return Session["Bolsa"].DBToInt32();


                    if (this.Request.Form["Bolsa"].Trim().ToLower().Equals("bol"))
                        lRetorno = 1;
                    else
                        lRetorno = 0;



                    Session.Add("Bolsa", lRetorno);
                }

                return Session["Bolsa"].DBToInt32();
            }
        }

        /// <summary>
        /// Retorna a bolsa BOL/BMF/Ambas
        /// </summary>
        //private string GetBolsa
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(this.Request.Form["Bolsa"]))
        //            return string.Empty;
        //        else
        //            return this.Request.Form["Bolsa"];
        //    }
        //}

        /// <summary>
        /// Retorna o sentido (compra/Venda/Ambas)
        /// </summary>
        private OrdemDirecaoEnum GetSentido
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Sentido"]))
                    return OrdemDirecaoEnum.NaoInformado;
                else
                {
                    return (this.Request.Form["Sentido"].ToString().Equals("C")) ? OrdemDirecaoEnum.Compra : OrdemDirecaoEnum.Venda;
                }
            }
        }

        /// <summary>
        /// Retorna o papel do filtro
        /// </summary>
        private string GetPapel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Papel"]) && Session["Papel"] == null)
                    return null;
                if (!string.IsNullOrWhiteSpace(this.Request.Form["Papel"]))
                    Session.Add("Papel", this.Request.Form["Papel"].Trim().ToUpper());

                if (this.Request.Form["Papel"] != null && this.Request.Form["Papel"].Equals(string.Empty))
                {
                    Session.Add("Papel", null);
                    return null;
                }
                return Session["Papel"].ToString();
            }
        }

        /// <summary>
        /// Retorna o status do filtro
        /// </summary>
        private OrdemStatusEnum? GetStatus
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Status"]))
                    return null;

                return (OrdemStatusEnum)Convert.ToInt32(this.Request.Form["Status"]);
            }
        }

        /// <summary>
        /// Retorna o Termo da busca efetuado no filtro
        /// </summary>
        private int? GetTermoBusca
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["TermoDeBusca"]) && Session["TermoDeBusca"] == null)
                    return null;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["TermoDeBusca"]))
                    Session.Add("TermoDeBusca", this.Request.Form["TermoDeBusca"].Trim().ToUpper());

                if (this.Request.Form["TermoDeBusca"] != null && this.Request.Form["TermoDeBusca"].Equals(string.Empty))
                {
                    Session.Add("TermoDeBusca", null);
                    return null;
                }
                return Session["TermoDeBusca"].DBToInt32(eIntNull.Permite);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.btnCancelarOrdensSpider.Visible = base.UsuarioPode("Cancelar", "807cda55-811c-4ca9-8867-ad16d456c68d");

            base.RegistrarRespostasAjax(new string[]{ "ExcluirOrdens"
                                                    , "Paginar"
                                                    , "BuscarItensParaSelecao"
                                                    , "CarregarDetalhes"
                                                    },
                     new ResponderAcaoAjaxDelegate[]{ this.ResponderExcluirOrdens
                                                    , this.ResponderPaginar
                                                    , this.ResponderBuscarItensParaListagemSimples
                                                    , this.ResponderCarregarDetalhes
                                                    });
        }

        #endregion

        #region | Métodos

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            int lPagina;

            if (int.TryParse(Request["page"], out lPagina))
            {
                lLista = BuscarPaginaDeResultados(lPagina);
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<TransporteResultadoOrdens> lLista = new List<TransporteResultadoOrdens>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPaginaAcompanhamento);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPaginaAcompanhamento;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    lLista.Add(new TransporteResultadoOrdens(this.SessionUltimoResultadoDeBusca[a]));
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = (int)this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)this.SessionUltimoResultadoDeBusca.Count / (double)TransporteDeListaPaginada.ItensPorPaginaAcompanhamento));
            base.RegistrarLogConsulta("lRetorno.TotalDePaginas = " + lRetorno.TotalDePaginas);
            lRetorno.PaginaAtual = pPagina;
            //}
            base.RegistrarLogConsulta("(Convert.ToInt32(Math.Ceiling((decimal)this.SessionUltimoResultadoDeBusca.Count / (decimal)TransporteDeListaPaginada.ItensPorPagina))).ToString() = " + (Convert.ToInt32(Math.Ceiling((decimal)this.SessionUltimoResultadoDeBusca.Count / (decimal)TransporteDeListaPaginada.ItensPorPaginaAcompanhamento))).ToString());

            return lRetorno;
        }

        /// <summary>
        /// Retorna uma busca pelos itens da listagem simples
        /// </summary>
        /// <returns></returns>
        private string ResponderBuscarItensParaListagemSimples()
        {
            this.Session["HoraInicial"] = null;
            this.Session["HoraFinal"] = null;
            //lRequest.Canal = GetBolsa;
            gRequest.Instrumento         = GetPapel; 
            gRequest.Origem              = GetOrigem;
            gRequest.Status              = GetStatus;
            gRequest.DataDe              = GetDataHoraInicial;
            gRequest.DataAte             = GetDataHoraFinal;
            gRequest.ContaDoCliente      = GetTermoBusca;
            gRequest.CodigoAssessor      = CodigoAssessor;
            gRequest.PaginaCorrente      = 1;
            gRequest.QtdeLimiteRegistros = 20;
            gRequest.TotalRegistros      = 0;
            gRequest.IdSistemaOrigem     = this.GetIdSistemaOrigem;

            string lRetorno = string.Empty;

            try
            {
                TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

                MonitoramentoOrdemDbLib lServico = new MonitoramentoOrdemDbLib();

                BuscarOrdensResponse lResponse = null;

                if (GetOrigem != null && (GetOrigem.Equals("HB") || GetOrigem.Equals("GTI") || GetOrigem.Equals("FIXSERVER")))
                {
                    lResponse = lServico.BuscarOrdensSpider(gRequest);

                    lResponse.TotalItens = lResponse.Ordens.Count;

                    this.SessionUltimoResultadoDeBusca = this.FiltrarSentidoBolsaOrigem(lResponse.Ordens).ToList();
                }
                

                lLista = BuscarPaginaDeResultados(1);

                if (lLista.TotalDeItens > 0)
                {
                    lRetorno = RetornarSucessoAjax(lLista, string.Format("Foram encontrados {0} registros", lLista.TotalDeItens.ToString())); //O grid espera o objeto direto, sem estar encapsulado
                    base.RegistrarLogConsulta();
                }
                else
                    lRetorno = RetornarSucessoAjax(lLista, "Nenhum registro encontrado!");
            }
            catch (Exception exBusca)
            {
                base.RetornarErroAjax("Erro ao buscar dados de ordem", exBusca);
            }
            return lRetorno;
        }

        private IEnumerable<OrdemInfo> FiltrarSentidoBolsaOrigem(List<OrdemInfo> pLista)
        {
            IEnumerable<OrdemInfo> lRetorno = pLista;

            try
            {
                if (this.GetOrigem == "FIXSERVER")
                {
                    if (this.GetBolsa < 50)
                    {
                        lRetorno = from a in lRetorno where a.ChannelID < 50 && (a.CompIDOMS.Equals("GTI") || a.CompIDOMS.ToUpper().Equals("FIXSERVER"))  select a;
                    }
                    else
                    {
                        lRetorno = from a in lRetorno where ( a.ChannelID > 50) select a;
                                   //where
                                   //    (a.ChannelID.Equals(PortaBovespaGTIAssessor) ||
                                   //    a.ChannelID.Equals(PortaBovespaGTICliente) ||
                                   //    a.ChannelID.Equals(PortaBovespaGTIClienteHFT2) ||
                                   //    a.ChannelID.Equals(PortaBovespaGTIClienteHFT1)) && a.CompIDOMS.Equals("GTI")
                                   //select a;

                        //lRetorno = from a in lRetorno select a;
                    }
                }
                else if (this.GetOrigem == "HB")
                {
                    if (this.GetBolsa == 0)
                    {
                        lRetorno = from a in lRetorno where a.ChannelID.Equals(0) && a.CompIDOMS.Equals("HB") select a;
                    }
                    else
                    {
                        lRetorno = from a in lRetorno where (a.ChannelID.Equals(PortaBovespaGTIAssessor) || a.ChannelID.Equals(PortaBovespaGTICliente) || a.ChannelID.Equals(PortaBovespaHB)) && a.CompIDOMS.Equals("HB") select a;
                    }
                }

                if (this.GetSentido != OrdemDirecaoEnum.NaoInformado)
                {
                    lRetorno = from a in lRetorno where a.Side.Equals(this.GetSentido) select a;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("FiltrarSentidoBolsaOrigem - Exception: {0} - StackTrace: {1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        /// <summary>
        /// Exclui as ordens selecionadas
        /// </summary>
        /// <returns>Retorna uma string com mensagem de sucesso ou erro ao excluir as ordens</returns>
        private string ResponderExcluirOrdens()
        {
            string lRetorno = string.Empty;

            try
            {
                string lIds      = Request.Form["Ids"];

                string lPortas   = Request.Form["Portas"];

                string lAccounts = Request.Form["Accounts"];

                string lSymbols  = Request.Form["Symbols"];

                string lMensagem = "";

                List<string> lstIdOrdem = new List<string>();

                //IServicoOrdens lOrdem = Ativador.Get<IServicoOrdens>();

                Gradual.Core.Ordens.Lib.IServicoOrdens lOrdem = Ativador.Get<Gradual.Core.Ordens.Lib.IServicoOrdens>();

                Dictionary<string, string> lCriticas = new Dictionary<string, string>();

                ExecutarCancelamentoOrdemResponse lResponse = new ExecutarCancelamentoOrdemResponse();

                string[] Ordens   = lIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string[] Portas   = lPortas.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string[] Accounts = lAccounts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string[] Symbols  = lSymbols.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //OrdemCancelamentoInfo info;

                //EnviarCancelamentoOrdemRequest lRequestCancel;

                for (int i = 0; i < Ordens.Length; i++)
                {
                    IFixServerLowLatencyAdm fixAdm = Ativador.Get<IFixServerLowLatencyAdm>();
                    OrderCancelingRequest req = new OrderCancelingRequest();

                    req.Account     = Accounts[i];
                    req.OrigClOrdID = Ordens[i];
                    req.ChannelID   = Portas[i];
                    req.Symbol      = Symbols[i];

                    OrderCancelingResponse resp = fixAdm.CancelOrder(req);
                    
                    /*
                    info = new  OrdemCancelamentoInfo();

                    info.OrigClOrdID = Ordens[i];

                    info.ChannelID = int.Parse( Portas[i]);

                    lRequestCancel = new EnviarCancelamentoOrdemRequest()
                    {
                        ClienteCancelamentoInfo = info
                    };

                    lResponse = lOrdem.CancelarOrdem(lRequestCancel);
                    */
                    if (resp.StatusResponse == 0 && !string.IsNullOrEmpty(resp.DescricaoErro))
                    {
                        lCriticas.Add(req.OrigClOrdID, resp.DescricaoErro);
                    }

                }

                if (lCriticas.Count > 0)
                {
                    foreach (KeyValuePair<string, string> critica in lCriticas)
                        lMensagem += string.Concat("Ordem: ", critica.Key, "Crítica - ", critica.Value);

                    lRetorno = RetornarSucessoAjax(lMensagem);
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("Orden(s) cancelada(s) com sucesso");
                    base.RegistrarLogExclusao(string.Concat("Foram EXCLUÍDAS as seguintes ordens: ", lIds));
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro ao tentar cancelar ordem.", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega os detalhes
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarDetalhes()
        {
            string lRetorno = string.Empty;

            var lstAcompanhamentos = from lista in SessionUltimoResultadoDeBusca where lista.ClOrdID == Request["id"].ToString() select lista;

            List<TransporteOrdemDetalhe> lDetalhes = new List<TransporteOrdemDetalhe>();

            List<OrdemInfo> lOrdens = lstAcompanhamentos.ToList<OrdemInfo>();

            OrdemInfo lOrdem = lOrdens[0];

            if (null != lstAcompanhamentos)
                lOrdem.Acompanhamentos.ForEach(lAcompanhamento =>
                {
                    lDetalhes.Add(new TransporteOrdemDetalhe(lAcompanhamento));
                });

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada(lDetalhes);

            lRetorno = JsonConvert.SerializeObject(lLista);

            return lRetorno;
        }

        #endregion
    }
}