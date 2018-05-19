using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Newtonsoft.Json;
using Gradual.OMS.Library;
using System;
using System.IO;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class AssociacaoCostistasFidcXFundos : PaginaBase
    {
        #region Propriedades

        private string GetCaminhoDownload
        {
            get { return Request["CaminhoDownload"].ToString(); }
        }

        private int GetIdCotistaFidcFundo
        {
            get { return Convert.ToInt32(Request["IdCotistaFidcFundo"]); }
        }

        private int GetIdCotistaFidc
        {
            get 
            {
                try
                {
                    return Convert.ToInt32(Request["IdCotistaFidc"]);
                }
                catch { return 0; }
            }
        }

        private int GetIdFundoCadastro
        {
            get 
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoCadastro"]);
                }
                catch { return 0; }
            }
        }

        #endregion

        #region Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax(new[]
                    {
                        "CarregarSelectFundos",
                        "CarregarSelectCotistas",
                        "CarregarGrid",
                        "InserirAssociacao",
                        "RemoverAssociacao",
                        "ExportarDadosAssociacaoCsv"
                    },
                    new ResponderAcaoAjaxDelegate[]
                    {
                        ResponderCarregarSelectFundos,
                        ResponderCarregarSelectCotistas,
                        ResponderCarregarGrid,
                        ResponderInserirAssociacao,
                        ResponderRemoverAssociacao,
                        ResponderExportarDadosAssociacaoCsv
                    });

                CarregarDadosIniciais();
            }
            catch
            {
                
            }
        }

        #endregion

        #region Métodos

        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Associação Cotistas X Fundos";
            LinkPreSelecionado = "lnkTL_AssociacaoCotistasFundos";
        }

        public string ResponderCarregarSelectFundos()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroFundoRequest();

                var lResponse = BuscarFundosCadastrados(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectFundos", ex);
            }

            return lRetorno;
        }

        public string ResponderCarregarSelectCotistas()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroCotistasFidcRequest();

                var lResponse = SelecionarListaCotistasFidc(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaCotistaFidc);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de cotistas na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectCotistas", ex);
            }

            return lRetorno;
        }

        public string ResponderCarregarGrid()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new AssociacaoCotistaFidcFundoRequest
                {
                    IdFundoCadastro = GetIdFundoCadastro,
                    IdCotistaFidc = GetIdCotistaFidc
                };

                var lResponse = SelecionarGridCotistaFidcFundo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteAssociacaoCotistasFundos().TraduzirLista(lResponse.ListaCotistaFidcFundo);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaCotistaFidcFundo.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarGrid ", ex);
            }

            return lRetorno;
        }

        public string ResponderInserirAssociacao()
        {
            var lRequest = new AssociacaoCotistaFidcFundoRequest
            {
                IdFundoCadastro = GetIdFundoCadastro,
                IdCotistaFidc = GetIdCotistaFidc,
                DtInclusao = DateTime.Now
            };

            var lResponse = InserirAssociacaoCotistaFundo(lRequest);

            if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
            }
            if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return RetornarErroAjax("Erro ao inserir associação.");
        }

        public string ResponderRemoverAssociacao()
        {
            var lRequest = new AssociacaoCotistaFidcFundoRequest
            {
                IdCotistaFidcFundo = GetIdCotistaFidcFundo             
            };

            var lResponse = RemoverAssociacaoCotistaFundo(lRequest);

            if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
            }
            if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.ErroNegocio)
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }

            return RetornarErroAjax("Erro ao remover associação.");
        }

        public string ResponderExportarDadosAssociacaoCsv()
        {
            var file = @"C:\TesteUpload\Teste.csv";

            using (var stream = File.CreateText(file))
            {
                string col1 = "f";
                string col2 = "s";
                string col3 = "s";
                string csvRow = string.Format("{0};{1};{2}", col1, col2, col3);

                stream.WriteLine(csvRow);
            }

            return "";
        }

        #endregion
    }
}