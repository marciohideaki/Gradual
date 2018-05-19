using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R020 : PaginaBaseAutenticada
    {
        #region Propriedades
        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = new DateTime();

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }
        private int? GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                return null;
            }
        }
        #endregion

        #region Métodos
        
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                Response.Clear();

                ResponderBuscarItensParaListagemSimples();
                //Response.Write(lResponse);

                Response.End();
            }
        }

        
        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<TotalClienteCadastradoAssessorPeriodoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<TotalClienteCadastradoAssessorPeriodoInfo>();

            try
            {
                TotalClienteCadastradoAssessorPeriodoInfo lInfo = new TotalClienteCadastradoAssessorPeriodoInfo() 
                {
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal.AddDays(1D),
                    CodigoAssessor = this.GetAssessor
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TotalClienteCadastradoAssessorPeriodoInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        List<TotalClienteCadastradoAssessorPeriodoInfo> lListaTemp = lResponse.Resultado.OrderBy(lp => lp.CodigoAssessor).ToList();  //.Sort( (lp1, lp2) => Comparer<int>.Default.Compare( lp1.CodigoAssessor.Value, lp2.CodigoAssessor.Value ) );

                        List<TotalClienteCadastradoAssessorPeriodoInfo> lListaBusca = new List<TotalClienteCadastradoAssessorPeriodoInfo>();

                        lListaTemp.ForEach(result =>
                            {
                                TotalClienteCadastradoAssessorPeriodoInfo lCad = new TotalClienteCadastradoAssessorPeriodoInfo();

                                lCad = result;

                                TotalClienteCadastradoAssessorPeriodoInfo lCadBusca = lListaBusca.Find(busca => busca.CodigoAssessor == lCad.CodigoAssessor && busca.DataCadastro == lCad.DataCadastro);

                                if (lCadBusca != null)
                                {
                                    lListaBusca.Remove(lCadBusca);

                                    lCadBusca.TotalCliente += lCad.TotalCliente;

                                    lListaBusca.Add(lCadBusca);
                                }
                                else
                                {
                                    lListaBusca.Add(lCad);
                                }
                            }
                            );

                        IEnumerable<TransporteRelatorio_020> lLista = from TotalClienteCadastradoAssessorPeriodoInfo i in lListaBusca select new TransporteRelatorio_020(i);

                        this.rptRelatorio.DataSource = lLista;
                        
                        this.rptRelatorio.DataBind();

                        rowLinhaDeNenhumItem.Visible = false;
                    }
                    else
                    {
                        rowLinhaDeNenhumItem.Visible = true;
                    }
                }
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }
        }

        #endregion
    }
}