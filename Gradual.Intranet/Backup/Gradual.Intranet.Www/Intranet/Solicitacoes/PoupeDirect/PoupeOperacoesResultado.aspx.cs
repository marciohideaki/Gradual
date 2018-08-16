using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.PoupeDirect.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.Intranet.Www.App_Codigo;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.PoupeDirect
{
    public partial class PoupeOperacoesResultado : PaginaBaseAutenticada
    {
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


        private List<int> GetIdsSelecionados
        {
            get
            {
                var lRetorno = new List<int>();

                string lIdsSelecionados = this.Request.Form["lIdsSelecionados"];

                if (!string.IsNullOrWhiteSpace(lIdsSelecionados))
                {
                    var lIdsSelecionadosArray = lIdsSelecionados.TrimEnd(',').Split(',');

                    for (int i = 0; i < lIdsSelecionadosArray.Length; i++)
                        lRetorno.Add(lIdsSelecionadosArray[i].DBToInt32());
                }

                return lRetorno;
            }
        }


        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);


            if (Request["Acao"] == "SelecionarProdutos")
                    this.SelecionarProdutos();
            else if (Request["Acao"] == "AtualizarProduto")
                this.AtualizarProduto();
        }

        private string SelecionarProdutos()
        {
            string Retorno = "";
            rowLinhaCarregandoMais.Visible = true;
            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            ProdutoClienteRequest request = new ProdutoClienteRequest();
            request.ProdutoCliente = this.CarregarFiltro();

            var lRetornoProdutoCliente = lServico.SelecionarProdutoClienteOperador(request);

            if (lRetornoProdutoCliente.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lRetornoProdutoCliente.ListaProdutoCliente.Count > 0)
                {
                    this.rptRelatorio.DataSource = lRetornoProdutoCliente.ListaProdutoCliente;
                    this.rptRelatorio.DataBind();
                    rowLinhaDeNenhumItem.Visible = false;
                    Retorno = "Dados carregados com sucesso.";
                }
                else
                {
                    Retorno = "Consulta não retornou dados.";
                    rowLinhaDeNenhumItem.Visible = true;
                }

            }
            else
            {
                Retorno = "Erro: " + lRetornoProdutoCliente.DescricaoResposta;
            }
            rowLinhaCarregandoMais.Visible = false;

            return RetornarSucessoAjax(Retorno);
        }

        private string AtualizarProduto()
        {
            string retorno = "";

            IServicoPoupeDirect lServico = Ativador.Get<IServicoPoupeDirect>();

            ClienteVencimentoRequest VencimentoRequest;
            ClienteVencimentoResponse vencimentoResponse;
            ClienteVencimentoRequest VencimentoAtuzalizarRequest;
            

            foreach (int item in this.GetIdsSelecionados)
            {
                VencimentoRequest = new ClienteVencimentoRequest();
                
                VencimentoRequest.ClienteVencimento = new ClienteVencimentoInfo();
                VencimentoRequest.ClienteVencimento.IdClienteVencimento = item;
                vencimentoResponse = lServico.SelecionarClienteVencimento(VencimentoRequest);

                if (vencimentoResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (vencimentoResponse.ListaClienteVencimento.Count > 0)
                    {
                        vencimentoResponse.ListaClienteVencimento[0].DtCompra = DateTime.Now;
                        VencimentoAtuzalizarRequest = new ClienteVencimentoRequest();
                        VencimentoAtuzalizarRequest.ClienteVencimento = vencimentoResponse.ListaClienteVencimento[0];
                        vencimentoResponse = lServico.InserirClienteVencimento(VencimentoAtuzalizarRequest);

                        if (vencimentoResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                            retorno = "Dados Atualizados com Sucesso.";

                    }
                }
            }

            return RetornarSucessoAjax(retorno);
        }

        private ProdutoClienteInfo CarregarFiltro()
        {
            ProdutoClienteInfo lRetorno = new ProdutoClienteInfo();


            if (this.GetCodigoCliente != string.Empty)
                lRetorno.CodigoCliente = Convert.ToInt32(this.GetCodigoCliente);

            if (this.GetStaus != string.Empty)
                lRetorno.CheckOperador = Convert.ToBoolean(Convert.ToInt32(this.GetStaus));

            lRetorno.DataInicio = this.GetDataInicial;
            lRetorno.DataFim = this.GetDataFinal;

            

            return lRetorno;
        }

        public string ToCodigoClienteComDigito(object pObject)
        {
            int lDigito = 0;

            int lCodigoCorretora = 227;

            lDigito = (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            lDigito = lDigito % 11;

            if (lDigito == 0 || lDigito == 1)
            {
                lDigito = 0;
            }

            else
            {
                lDigito = 11 - lDigito;
            }

            return string.Format("{0} - {1}", pObject.ToString(), lDigito);
        }
    }
}