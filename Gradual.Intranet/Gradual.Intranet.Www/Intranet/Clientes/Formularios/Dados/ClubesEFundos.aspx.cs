using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;


namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class ClubesEFundos : PaginaBaseAutenticada
    {
        #region | Propriedades

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

                return lRetorno;
            }
        }

        private int GetIdCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["Id"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetCpfCnpj
        {
            get
            {
                return this.Request.Form["CpfCnpj"].Replace(".","").Replace("-","").Replace("/","");
            }
        }
        #endregion

        #region | Métodos

        public string ResponderCarregarHtmlComDados() { return string.Empty; }

        public string ResponderCarregarFundosItauFinancial()
        {
            string lRetorno = string.Empty;

            List<Transporte_PosicaoCotista> ListaPosicao = base.PosicaoFundosSumarizado(this.GetIdCliente, this.GetCpfCnpj);

            var lRequestRendaFixa = new ConsultarEntidadeCadastroRequest<RendaFixaInfo>();

            lRequestRendaFixa.EntidadeCadastro = new RendaFixaInfo();

            lRequestRendaFixa.EntidadeCadastro.CodigoCliente = this.GetIdCliente;

            var lPosicaoRendaFixa = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RendaFixaInfo>(lRequestRendaFixa);

            var lRendaFixaLista = lPosicaoRendaFixa.Resultado;

            var lClubesEFundos = new TransporteRelatorioClubesEFundos();

            lClubesEFundos.ListaFundos = new TransporteRelatorioFundos().TraduzirListaParaTransporteRelatorioFundos(ListaPosicao);

            lClubesEFundos.ListaRendaFixa = new TransporteRelatorioRendaFixa().TraduzirLista(lRendaFixaLista,null);

            lRetorno = base.RetornarSucessoAjax(lClubesEFundos, "Dados carregados com sucesso.");

            return lRetorno;
        }

        public string ResponderCarregarClubesEFundos()
        {
            string lRetorno = string.Empty;

            try
            {
                if (this.GetIdCliente != 0)
                {
                    var lConsultaClubes = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteClubesInfo>(
                         new ConsultarEntidadeCadastroRequest<ClienteClubesInfo>()
                         {
                             EntidadeCadastro = new ClienteClubesInfo()
                             {
                                 IdCliente = this.GetIdCliente,
                             }
                             ,
                             IdUsuarioLogado = base.UsuarioLogado.Id
                             ,
                             DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                         });

                    var lConsultaFundos = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteFundosInfo>(
                        new ConsultarEntidadeCadastroRequest<ClienteFundosInfo>()
                        {
                            EntidadeCadastro = new ClienteFundosInfo()
                            {
                                IdCliente = this.GetIdCliente,
                            }
                             ,
                            IdUsuarioLogado = base.UsuarioLogado.Id
                             ,
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                        });

                    if (MensagemResponseStatusEnum.OK.Equals(lConsultaClubes.StatusResposta))
                    {
                        var lClubesEFundos = new TransporteRelatorioClubesEFundos();

                        lClubesEFundos.ListaClubes = new TransporteRelatorioClubes().TraduzirListaParaTransporteRelatorioClubes(lConsultaClubes.Resultado);
                        lClubesEFundos.ListaFundos = new TransporteRelatorioFundos().TraduzirListaParaTransporteRelatorioFundos(lConsultaFundos.Resultado);

                        lRetorno = base.RetornarSucessoAjax(lClubesEFundos, "Dados carregados com sucesso.");
                    }
                    else
                    {
                        lRetorno = base.RetornarErroAjax("Erro ao selecionar Clubes e Fundos: {0} {1}", lConsultaClubes.StatusResposta, lConsultaClubes.DescricaoResposta);
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro ao selecionar Clubes e Fundos", ex);
            }

            return lRetorno;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                  "CarregarHtmlComDados",
                                                  "CarregarClubesEFundos"
                                                },
            new ResponderAcaoAjaxDelegate[] { 
                                                ResponderCarregarHtmlComDados,
                                                ResponderCarregarFundosItauFinancial
                                            });
        }

        #endregion
    }
}