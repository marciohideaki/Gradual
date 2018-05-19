using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class CadastroFundos : PaginaBase
    {
        #region Propriedades

        private string GetNomeFundo
        {
            get { return !string.IsNullOrEmpty(Request["NomeFundo"]) ? Request["NomeFundo"] : string.Empty; }
        }

        private string GetCnpjFundo
        {
            get { return !string.IsNullOrEmpty(Request["CNPJFundo"]) ? Request["CNPJFundo"] : string.Empty; }
        }

        private string GetNomeAdministrador
        {
            get
            {
                return !string.IsNullOrEmpty(Request["NomeAdministrador"]) ? Request["NomeAdministrador"] : string.Empty;
            }
        }

        private string GetCnpjAdministrador
        {
            get
            {
                return !string.IsNullOrEmpty(Request["CNPJAdministrador"]) ? Request["CNPJAdministrador"] : string.Empty;
            }
        }

        private string GetNomeCustodiante
        {
            get
            {
                return !string.IsNullOrEmpty(Request["NomeCustodiante"]) ? Request["NomeCustodiante"] : string.Empty;
            }
        }

        private string GetCnpjCustodiante
        {
            get
            {
                return !string.IsNullOrEmpty(Request["CNPJCustodiante"]) ? Request["CNPJCustodiante"] : string.Empty;
            }
        }

        private string GetNomeGestor
        {
            get { return !string.IsNullOrEmpty(Request["NomeGestor"]) ? Request["NomeGestor"] : string.Empty; }
        }

        private string GetCnpjGestor
        {
            get { return !string.IsNullOrEmpty(Request["CNPJGestor"]) ? Request["CNPJGestor"] : string.Empty; }
        }

        private decimal GetTxCustodia
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(Request["TxCustodia"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private decimal GetTxGestao
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(Request["TxGestao"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private decimal GetTxConsultoria
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(Request["TxConsultoria"]);
                }
                catch
                {
                    return 0;
                }
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
                catch
                {
                    return 0;
                }
            }
        }

        private bool GetIsAtivo
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["InativarFundo"]))
                {
                    return Request["InativarFundo"].ToUpper() != "TRUE";
                }

                return true;
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
                        "CarregarHtmlComDados",
                        "AtualizarValor",
                        "EditarFundo"
                    },
                    new ResponderAcaoAjaxDelegate[]
                    {
                        ResponderCarregarHtmlComDados,
                        ResponderAtualizarValor,
                        ResponderEditarFundo
                    });

                CarregarDadosIniciais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carregar dados iniciais da página de fundos
        /// </summary>
        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Cadastro de Fundos";
            LinkPreSelecionado = "lnkTL_CadastroFundos";
        }

        /// <summary>
        /// Atualiza ou insere valores de fundos
        /// </summary>
        /// <returns>Retorna string com a lista em Json</returns>
        public string ResponderAtualizarValor()
        {
            try
            {
                var lRequest = new CadastroFundoRequest
                {
                    NomeFundo = GetNomeFundo,
                    CNPJFundo = GetCnpjFundo.Replace("/", "").Replace(".", "").Replace("-", ""),
                    NomeAdministrador = GetNomeAdministrador,
                    CNPJAdministrador = GetCnpjAdministrador.Replace("/", "").Replace(".", "").Replace("-", ""),
                    NomeCustodiante = GetNomeCustodiante,
                    CNPJCustodiante = GetCnpjCustodiante.Replace("/", "").Replace(".", "").Replace("-", ""),
                    NomeGestor = GetNomeGestor,
                    CNPJGestor = GetCnpjGestor.Replace("/", "").Replace(".", "").Replace("-", ""),
                    IdFundoCadastro = GetIdFundoCadastro,
                    TxGestao = GetTxGestao,
                    TxCustodia = GetTxCustodia,
                    TxConsultoria = GetTxConsultoria,
                    IsAtivo = GetIsAtivo,
                    DescricaoUsuarioLogado = UsuarioLogado.Nome
                };

                #region Gravação Log4Net

                var mensagemLog = string.Empty;

                mensagemLog += "NomeFundo:" + lRequest.NomeFundo + ";";
                mensagemLog += "CNPJFundo:" + lRequest.CNPJFundo + ";";
                mensagemLog += "NomeAdministrador:" + lRequest.NomeAdministrador + ";";
                mensagemLog += "CNPJAdministrador:" + lRequest.CNPJAdministrador + ";";
                mensagemLog += "NomeCustodiante:" + lRequest.NomeCustodiante + ";";
                mensagemLog += "CNPJCustodiante:" + lRequest.CNPJCustodiante + ";";
                mensagemLog += "NomeGestor:" + lRequest.NomeGestor + ";";
                mensagemLog += "CNPJGestor:" + lRequest.CNPJGestor + ";";

                mensagemLog += "TxGestao:" + lRequest.TxGestao + ";";
                mensagemLog += "TxCustodia:" + lRequest.TxCustodia + ";";
                mensagemLog += "TxConsultoria:" + lRequest.TxConsultoria + ";";
                
                mensagemLog += "IsAtivo:" + lRequest.IsAtivo + ";";
                mensagemLog += "TipoTransacao:" + (lRequest.IdFundoCadastro > 0 ? "UPDATE" : "INSERT") + ";";
                mensagemLog += "UsuarioTransacao:" + lRequest.DescricaoUsuarioLogado + ";";

                Logger.Info(mensagemLog);

                #endregion

                var lResponse = AtualizarCadastroFundo(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    AnexarArquivos(lRequest.IdFundoCadastro);

                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }

                return RetornarErroAjax("Erro ao cadastrar fundo.");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao cadastrar fundo", ex);

                return RetornarErroAjax("Erro no método ResponderAtualizarValor ", ex);
            }
        }

        /// <summary>
        /// Carrega dados no grid de fundos
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarHtmlComDados()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroFundoRequest();

                var lResponse = BuscarFundosCadastrados(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroFundos().TraduzirLista(lResponse.ListaFundos);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaFundos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarHtmlComDados ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega dados do fundo selecionado na tela
        /// </summary>
        /// <returns></returns>
        public string ResponderEditarFundo()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroFundoRequest {IdFundoCadastro = GetIdFundoCadastro};

                //Preenchimento objeto Request
                var lResponse = BuscarFundosCadastrados(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao editar fundo. ", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderEditarFundo ", ex);
            }

            return lRetorno;
        }

        private void AnexarArquivoRegulamento(int idFundoCadastro)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoRegulamento"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idFundoCadastro + "_" +
                                   "Regulamento_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            var req = new FundoCadastroAnexoRequest();
            req.IdFundoCadastro = idFundoCadastro;
            req.TipoAnexo = "REGULAMENTO";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCadastroFundo(req);
        }

        private void AnexarArquivoContratoGestao(int idFundoCadastro)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoContratoGestao"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idFundoCadastro + "_" +
                                   "Gestao_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            var req = new FundoCadastroAnexoRequest();
            req.IdFundoCadastro = idFundoCadastro;
            req.TipoAnexo = "GESTAO";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCadastroFundo(req);
        }

        private void AnexarArquivoContratoCustodia(int idFundoCadastro)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoContratoCustodia"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idFundoCadastro + "_" +
                                   "Custodia_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            var req = new FundoCadastroAnexoRequest();
            req.IdFundoCadastro = idFundoCadastro;
            req.TipoAnexo = "CUSTODIA";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCadastroFundo(req);
        }

        private void AnexarArquivos(int idFundoCadastro)
        {
            AnexarArquivoRegulamento(idFundoCadastro);
            AnexarArquivoContratoGestao(idFundoCadastro);
            AnexarArquivoContratoCustodia(idFundoCadastro);
        }

        #endregion
    }
}