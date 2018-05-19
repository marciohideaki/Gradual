using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Newtonsoft.Json;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class CadastroCotistas : PaginaBase
    {
        #region Propriedades

        private int GetIdCotistaFidc
        {
            get {
                try
                {
                    return Convert.ToInt32(Request["IdCotistaFidc"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        private string GetNomeCotista
        {
            get { return !string.IsNullOrEmpty(Request["NomeCotista"]) ? Request["NomeCotista"].ToUpper() : string.Empty; }
        }
        private string GetCpfCnpj
        {
            get { return !string.IsNullOrEmpty(Request["CpfCnpj"]) ? Request["CpfCnpj"] : string.Empty; }
        }
        private string GetEmail
        {
            get
            {
                return !string.IsNullOrEmpty(Request["Email"]) ? Request["Email"].ToUpper() : string.Empty;
            }
        }
        private DateTime GetDataNascFundacao
        {
            get
            {
                return Convert.ToDateTime(Request["DataNascFundacao"]);
            }
        }        
        private bool GetIsAtivo
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["IsAtivo"]))
                {
                    return Convert.ToBoolean(Request["IsAtivo"]);
                }

                return true;
            }
        }
        private int GetQtdCotas
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["QtdCotas"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        private DateTime GetDataVencimentoCadastro
        {
            get
            {
                return Convert.ToDateTime(Request["DataVencimentoCadastro"]);
            }
        }
        private string GetClasseCotas
        {
            get
            {
                return !string.IsNullOrEmpty(Request["ClasseCotas"]) ? Request["ClasseCotas"].ToUpper() : string.Empty;
            }
        }

        private int GetIdCotistaFidcProcurador
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdCotistaFidcProcurador"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        private string GetNomeProcurador
        {
            get
            {
                return !string.IsNullOrEmpty(Request["NomeProcurador"]) ? Request["NomeProcurador"].ToUpper() : string.Empty;
            }
        }
        private string GetCpfProcurador
        {
            get
            {
                return !string.IsNullOrEmpty(Request["CpfProcurador"]) ? Request["CpfProcurador"].ToUpper() : string.Empty;
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
                        "CarregarGridCotistas",
                        "AtualizarCotista",
                        "EditarCotista",
                        "GravarCotistaProcurador",
                        "CarregarGridCotistasProcuradores",
                        "SelecionarEditarProcurador",
                        "ExcluirProcurador"
                    },
                    new ResponderAcaoAjaxDelegate[]
                    {
                        ResponderCarregarGridCotistas,
                        ResponderAtualizarCotista,
                        ResponderEditarCotista,
                        ResponderGravarCotistaProcurador,
                        ResponderCarregarGridCotistasProcuradores,
                        ResponderSelecionarEditarProcurador,
                        ResponderExcluirProcurador
                    });

                CarregarDadosIniciais();
            }
            catch
            {
                //gLogger.Error("Erro ao carregar os dados de fundos na tela", ex);
            }
        }

        #endregion

        #region Métodos

        private void CarregarDadosIniciais()
        {
            if (Page.IsPostBack) return;
            TituloDaPagina = "Cadastro de Cotistas";
            LinkPreSelecionado = "lnkTL_CadastroCotistas";
        }

        public string ResponderCarregarGridCotistas()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroCotistasFidcRequest();

                var lResponse = SelecionarListaCotistasFidc(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCadastroCotistasFidc().TraduzirLista(lResponse.ListaCotistaFidc);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaCotistaFidc.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de cotista na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarGridCotistas ", ex);
            }

            return lRetorno;
        }

        public string ResponderAtualizarCotista()
        {
            try
            {
                var lRequest = new CadastroCotistasFidcRequest
                {
                    NomeCotista = GetNomeCotista,
                    Email = GetEmail,
                    CpfCnpj = GetCpfCnpj,
                    IsAtivo = GetIsAtivo,
                    DataNascFundacao = GetDataNascFundacao,
                    IdCotistaFidc = GetIdCotistaFidc,
                    QuantidadeCotas = GetQtdCotas,
                    ClasseCotas = GetClasseCotas,
                    DtVencimentoCadastro = GetDataVencimentoCadastro
                };

                #region Gravação Log4Net

                var mensagemLog = string.Empty;

                mensagemLog += "NomeCotista:" + lRequest.NomeCotista + ";";
                mensagemLog += "Email:" + lRequest.Email + ";";
                mensagemLog += "CpfCnpj:" + lRequest.CpfCnpj + ";";
                mensagemLog += "Ativo:" + (lRequest.IsAtivo ? "S" : "N") + ";";
                mensagemLog += "DataNasc/Fundacao:" + lRequest.DataNascFundacao + ";";

                mensagemLog += "QuantidadeCotas:" + lRequest.QuantidadeCotas + ";";
                mensagemLog += "ClasseCotas:" + lRequest.ClasseCotas + ";";
                mensagemLog += "DtVencimentoCadastro:" + lRequest.DtVencimentoCadastro + ";";

                mensagemLog += "TipoTransacao:" + (lRequest.IdCotistaFidc > 0 ? "UPDATE" : "INSERT") + ";";
                mensagemLog += "UsuarioTransacao:" + UsuarioLogado.Nome + ";";

                Logger.Info(mensagemLog);

                #endregion

                var lResponse = AtualizarCadastroCotista(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }

                return RetornarErroAjax("Erro ao cadastrar cotista.");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao cadastrar cotista", ex);

                return RetornarErroAjax("Erro no método ResponderAtualizarCotista ", ex);
            }
        }

        public string ResponderEditarCotista()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CadastroCotistasFidcRequest { IdCotistaFidc = GetIdCotistaFidc };

                //Preenchimento objeto Request
                var lResponse = SelecionarListaCotistasFidc(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaCotistaFidc);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao editar cotista. ", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderEditarCotista ", ex);
            }

            return lRetorno;
        }

        public string ResponderGravarCotistaProcurador()
        {
            try
            {
                var lRequest = new CotistaFidcProcuradorRequest
                {
                    IdCotistaFidc = GetIdCotistaFidc,
                    NomeProcurador = GetNomeProcurador,
                    CPF = GetCpfProcurador,
                    IdCotistaFidcProcurador = GetIdCotistaFidcProcurador
                };

                #region Gravação Log4Net
                var mensagemLog = string.Empty;

                mensagemLog += "IdCotistaFidc:" + lRequest.IdCotistaFidc + ";";
                mensagemLog += "NomeProcurador:" + lRequest.NomeProcurador + ";";
                mensagemLog += "CPF:" + lRequest.CPF + ";";

                mensagemLog += "TipoTransacao:" + (lRequest.IdCotistaFidcProcurador > 0 ? "UPDATE" : "INSERT") + ";";
                mensagemLog += "UsuarioTransacao:" + UsuarioLogado.Nome + ";";

                Logger.Info(mensagemLog);
                #endregion

                var lResponse = new CotistaFidcProcuradorResponse();

                if (lRequest.IdCotistaFidcProcurador > 0)
                    AtualizarCotistaFidcProcurador(lRequest);
                else
                    InserirCotistaFidcProcurador(lRequest);

                AnexarArquivos(lRequest.IdCotistaFidcProcurador);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    //erro de negócio
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }

                return RetornarErroAjax("Erro ao cadastrar procurador.");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao cadastrar procurador", ex);

                return RetornarErroAjax("Erro no método ResponderInserirCotistaProcurador", ex);
            }
        }

        public string ResponderCarregarGridCotistasProcuradores()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CotistaFidcProcuradorRequest
                {
                    IdCotistaFidc = GetIdCotistaFidc
                };

                var lResponse = SelecionarCotistaFidcProcurador(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteCotistaFidcProcurador().TraduzirLista(lResponse.ListaCotistaFidcProcurador);

                    var lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaCotistaFidcProcurador.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };

                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os dados de procuradores na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarGridCotistasProcuradores ", ex);
            }

            return lRetorno;
        }

        public string ResponderSelecionarEditarProcurador()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new CotistaFidcProcuradorRequest { IdCotistaFidcProcurador = GetIdCotistaFidcProcurador };

                //Preenchimento objeto Request
                var lResponse = SelecionarCotistaFidcProcurador(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaCotistaFidcProcurador);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao editar procurador. ", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderSelecionarEditarProcurador", ex);
            }

            return lRetorno;
        }

        public string ResponderExcluirProcurador()
        {
            var lRequest = new CotistaFidcProcuradorRequest
            {
                IdCotistaFidcProcurador = GetIdCotistaFidcProcurador
            };

            try
            {
                RemoverAnexosProcurador(lRequest.IdCotistaFidcProcurador);

                var lResponse = ExcluirCotistaFidcProcurador(lRequest);

                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    return RetornarSucessoAjax(lResponse.StatusResposta.ToString());
                }
                if (lResponse != null && lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.ErroNegocio)
                {
                    return RetornarErroAjax(lResponse.DescricaoResposta);
                }                
            }
            catch(Exception ex)
            {
                return RetornarErroAjax("Erro ao remover procurador.", ex);
            }
            return RetornarErroAjax("Erro ao remover procurador.");
        }

        #endregion

        #region Métodos privados
        private void AnexarArquivoDecreto(int idCotistaFidcProcurador)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoDecreto"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idCotistaFidcProcurador + "_" +
                                   "ProcuradorDecreto_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            var req = new CotistaFidcProcuradorAnexoRequest();
            req.IdCotistaFidcProcurador = idCotistaFidcProcurador;
            req.TipoAnexo = "DECRETO";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCotistaFidcProcuradorAnexo(req);
        }

        private void AnexarArquivoTermo(int idCotistaFidcProcurador)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoTermo"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idCotistaFidcProcurador + "_" +
                                   "ProcuradorTermo_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            
            var req = new CotistaFidcProcuradorAnexoRequest();
            req.IdCotistaFidcProcurador = idCotistaFidcProcurador;
            req.TipoAnexo = "TERMO";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCotistaFidcProcuradorAnexo(req);
        }

        private void AnexarArquivoProcuracao(int idCotistaFidcProcurador)
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) return; //só prossegue se houver arquivo

            var httpPostedFile = HttpContext.Current.Request.Files["ArquivoAnexoProcuracao"];

            if (httpPostedFile == null) return; //só prossegue se houver arquivo anexo

            //busca o caminho de destino dos arquivos no config
            var caminhoDestino = ConfigurationManager.AppSettings["RaizUploadFluxoAprovacaoFundo"];

            //cria o nome do arquivo com data e hora atual para evitar de sobrescrever arquivos com mesmo nome
            var nomeFinalArquivo = Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + "_" +
                                   idCotistaFidcProcurador + "_" +
                                   "ProcuradorProcuracao_" +
                                   DateTime.Now.ToString("ddMMyyyhhmmss") +
                                   Path.GetExtension(httpPostedFile.FileName);

            //obtém caminho destino
            var fileSavePath = Path.Combine(caminhoDestino, nomeFinalArquivo);

            //salva arquivo no diretório
            httpPostedFile.SaveAs(fileSavePath);

            var req = new CotistaFidcProcuradorAnexoRequest();
            req.IdCotistaFidcProcurador = idCotistaFidcProcurador;
            req.TipoAnexo = "PROCURACAO";
            req.CaminhoAnexo = fileSavePath;

            InserirAnexoCotistaFidcProcuradorAnexo(req);
        }

        private void AnexarArquivos(int idFundoCadastro)
        {
            AnexarArquivoDecreto(idFundoCadastro);
            AnexarArquivoTermo(idFundoCadastro);
            AnexarArquivoProcuracao(idFundoCadastro);            
        }

        private void RemoverAnexosProcurador(int idCotistaFidcProcurador)
        {
            var lRequest = new CotistaFidcProcuradorAnexoRequest
            {
                IdCotistaFidcProcurador = GetIdCotistaFidcProcurador
            };

            var lResponse = RemoverTodosOsAnexosPorProcurador(lRequest);
        }

        #endregion
    }
}