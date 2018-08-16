using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Termo.Lib.Info;
using Gradual.OMS.Termo.Lib;
using Gradual.OMS.Termo.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using System.Globalization;

namespace Gradual.Intranet.Www.Intranet.Sistema
{
    public partial class ObjetosDoSistema : PaginaBaseAutenticada
    {
        #region Propriedades
        
        public string RequestTipoDeObjeto  { get { return Request["Tipo"]; } }

        public string RequestId            { get { return Request["Id"]; } }

        public string RequestDescricao     { get { return Request["Descricao"]; } }

        public string RequestTermo         { get { return Request["Termo"]; } }

        public bool RequestPendenciaAutomatica { get { return bool.Parse( Request["PendenciaAutomatica"]); } } 

        #endregion

        #region Métodos Private

        private void PopularRepeaterComListaConvertida<T>(List<T> pLista)
        {
            if (this.RequestTipoDeObjeto == "TiposDePendenciaCadastral")
            {
                foreach (var item in pLista)
                {
                    if ((item as TipoDePendenciaCadastralInfo).StAutomatica)
                    {
                        (item as TipoDePendenciaCadastralInfo).DsPendencia += " (Automática)";
                    }
                }
            }

            List<TransporteObjetoDoSistema> lLista;

            lLista = (from T item in pLista select new TransporteObjetoDoSistema(item)).ToList();

            List<SinacorListaInfo> lListaSinacor = null;

            if (this.RequestTipoDeObjeto == "AtividadesIlicitas")
            {
                lListaSinacor  = (List<SinacorListaInfo>)this.rptObjetosDoSistema_AtividadesIlicitas_Atividade.DataSource;
            }
            else
            {
                lListaSinacor = (List<SinacorListaInfo>)this.rptObjetosDoSistema_PaisesEmListaNegra_Pais.DataSource;
            }

            PegarNomeDoItem(lListaSinacor, lLista);

            rptListaDeItens.DataSource = lLista;

            rptListaDeItens.DataBind();

            rowLinhaDeNenhumItem.Visible = (pLista.Count == 0);
        }

        private void PegarNomeDoItem(List<SinacorListaInfo> pListaSinacor, List<TransporteObjetoDoSistema> pLista) 
        {
            var x = from n in pLista select n;
            foreach (TransporteObjetoDoSistema item in x)
            {
                SinacorListaInfo y = (from m in pListaSinacor where m.Id == item.Descricao select m).FirstOrDefault();
                if(y != null)
                    item.Descricao = y.Value;
            }
        }

        private SalvarEntidadeCadastroResponse InserirItemDeSistema<T>(SalvarEntidadeCadastroRequest<T> lRequest) where T : ICodigoEntidade
        {
            return this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<T>(lRequest);
        }

        private RemoverEntidadeCadastroResponse ExcluirItemDeSistema<T>(RemoverEntidadeCadastroRequest<T> lRequest) where T : ICodigoEntidade
        {
            return this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<T>(lRequest);
        }

        private List<TaxaTermoInfo> BuscarTaxasDeTermo()
        {
            List<TaxaTermoInfo> lRetorno = new List<TaxaTermoInfo>();
            
            IServicoTermo lServico;
            ConsultarTaxaTermoResponse lResponse;

            lServico = Ativador.Get<IServicoTermo>();

            lResponse = lServico.ConsultarTaxaTermoDia();

            if (lResponse.CriticaResposta == OMS.Termo.Lib.Info.StatusRespostaEnum.Sucesso)
            {
                lRetorno = lResponse.ListaTaxaTermo;
            }

            return lRetorno;
        }

        private string ResponderCarregarHtmlComDados()
        {
            this.PopularControleComListaDoSinacor(eInformacao.AtividadePFePJ, rptObjetosDoSistema_AtividadesIlicitas_Atividade);

            this.PopularControleComListaDoSinacor(eInformacao.Pais, rptObjetosDoSistema_PaisesEmListaNegra_Pais);

            switch (this.RequestTipoDeObjeto)
            {
                case "AtividadesIlicitas":

                    PopularRepeaterComListaConvertida<AtividadeIlicitaInfo>(this.BuscarListaDoCadastro<AtividadeIlicitaInfo>(null));

                    break;

                case "PaisesEmListaNegra" : 

                    PopularRepeaterComListaConvertida<PaisesBlackListInfo>(this.BuscarListaDoCadastro<PaisesBlackListInfo>(null));

                    break;

                case "Contratos": 

                    PopularRepeaterComListaConvertida<ContratoInfo>(this.BuscarListaDoCadastro<ContratoInfo>(null));

                    break;
                    
                case "TaxasDeTermo": 

                    PopularRepeaterComListaConvertida<TaxaTermoInfo>(BuscarTaxasDeTermo());

                    break;

                case "TiposDePendenciaCadastral":
                    PodeExcluirPendencia = UsuarioPode("Excluir", "ffa9a91b-8006-49f4-838f-f011f99654aa");

                    PopularRepeaterComListaConvertida<TipoDePendenciaCadastralInfo>(this.BuscarListaDoCadastro<TipoDePendenciaCadastralInfo>(null));

                    break;

                default:
                    break;
            }


            return string.Empty;
        }

        private string ResponderReceberArquivo()
        {
            bool Ok = true;
            string lRetorno = string.Empty;
            SalvarEntidadeCadastroResponse lResposta = InserirItemDeSistema(
                new SalvarEntidadeCadastroRequest<ContratoInfo>()
                {
                    EntidadeCadastro = new ContratoInfo()
                    {
                        DsContrato = this.RequestTermo,
                        DsPath = this.RequestTipoDeObjeto,
                        StObrigatorio = true
                    }, 
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome, 
                    IdUsuarioLogado=base.UsuarioLogado.Id
                });

            if (lResposta.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                Ok = false;
                
            }
            if (Ok)
            {
                ArquivoContratoInfo lArquivoContrato = new ArquivoContratoInfo();
                HttpPostedFile lFile = Request.Files[0];
                byte[] lFileBytes = new byte[lFile.InputStream.Length];
                lFile.InputStream.Read(lFileBytes, 0, lFileBytes.Length);

                lArquivoContrato.IdContrato = int.Parse(lResposta.DescricaoResposta);
                lArquivoContrato.Arquivo = lFileBytes;
                lArquivoContrato.Extensao = Path.GetExtension(lFile.FileName).ToLower();
                lArquivoContrato.MIMEType = lFile.ContentType;
                lArquivoContrato.Nome = Path.GetFileNameWithoutExtension(lFile.FileName);
                lArquivoContrato.Tamanho = lFile.ContentLength;

                SalvarEntidadeCadastroResponse lResposta2 = InserirItemDeSistema(
                    new SalvarEntidadeCadastroRequest<ArquivoContratoInfo>()
                    {
                        EntidadeCadastro = lArquivoContrato
                    });

                if (lResposta2.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    Ok = false;
                }
            }

            if (Ok)
            {
                TransporteObjetoDoSistema lObjetoDeRetorno = new TransporteObjetoDoSistema();

                lObjetoDeRetorno.Id = lResposta.DescricaoResposta;

                lObjetoDeRetorno.Descricao = this.RequestTermo;

                lRetorno = RetornarSucessoAjax(lObjetoDeRetorno, "Registro e Arquivo salvos com sucesso!");

            }
            else
            {
                lRetorno = RetornarErroAjax("Erro ao gravar o arquivo.");
            }
            return lRetorno;
        }

        private string CadastrarTaxaDeTermo()
        {
            string lRetorno = "";

            CultureInfo lInfo = new CultureInfo("pt-BR");

            decimal lValorTaxa, lValorRolagem;

            int lNumeroDias;

            string lRequestValorTaxa, lRequestValorRolagem, lRequestNumeroDias;

            lRequestNumeroDias = Request["NumeroDias"];
            lRequestValorRolagem = Request["ValorRolagem"];
            lRequestValorTaxa = Request["ValorTaxa"];

            if (decimal.TryParse(lRequestValorTaxa, NumberStyles.AllowDecimalPoint, lInfo, out lValorTaxa))
            {
                if (decimal.TryParse(lRequestValorRolagem, NumberStyles.AllowDecimalPoint, lInfo, out lValorRolagem))
                {
                    if (int.TryParse(lRequestNumeroDias, NumberStyles.AllowThousands, lInfo, out lNumeroDias))
                    {
                        IServicoTermo lServico = Ativador.Get<IServicoTermo>();

                        TaxaTermoRequest lRequest = new TaxaTermoRequest();
                        TaxaTermoResponse lResponse;

                        lRequest.TaxaTermoInfo = new TaxaTermoInfo();

                        lRequest.TaxaTermoInfo.DataReferencia = DateTime.Now;
                        lRequest.TaxaTermoInfo.NumeroDias = lNumeroDias;
                        lRequest.TaxaTermoInfo.ValorRolagem = lValorRolagem;
                        lRequest.TaxaTermoInfo.ValorTaxa = lValorTaxa;

                        lResponse = lServico.InserirTaxaTermo(lRequest);

                        if (lResponse.CriticaResposta == StatusRespostaEnum.Sucesso)
                        {
                            TransporteObjetoDoSistema lObjetoDeRetorno = new TransporteObjetoDoSistema(lRequest.TaxaTermoInfo);

                            lRetorno = RetornarSucessoAjax(lObjetoDeRetorno, "Objeto incluído com sucesso!");
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(string.Format("Resposta com erro do ServicoTermo.InserirTaxaTermo([{0}], [{1}], [{2}]) : [{3}] [{4}]"
                                                                        , lRequest.TaxaTermoInfo.NumeroDias
                                                                        , lRequest.TaxaTermoInfo.ValorRolagem
                                                                        , lRequest.TaxaTermoInfo.ValorTaxa
                                                                        , lResponse.CriticaResposta
                                                                        , lResponse.DescricaoResposta));
                        }
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax("Número inválido para Número de Dias");
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("Número inválido para Valor de Rolagem");
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Número inválido para Valor de Taxa");
            }

            return lRetorno;
        }

        private string ResponderIncluir()
        {
            SalvarEntidadeCadastroResponse lResposta = null;
            string lRetorno = string.Empty;

            try
            {
                switch (this.RequestTipoDeObjeto)
                {
                    case "AtividadesIlicitas":
                        lResposta = InserirItemDeSistema(
                            new SalvarEntidadeCadastroRequest<AtividadeIlicitaInfo>()
                            {
                                EntidadeCadastro = new AtividadeIlicitaInfo()
                                {
                                    CdAtividade = this.RequestId
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;

                    case "PaisesEmListaNegra":

                        lResposta = InserirItemDeSistema(
                            new SalvarEntidadeCadastroRequest<PaisesBlackListInfo>()
                            {
                                EntidadeCadastro = new PaisesBlackListInfo()
                                {
                                    CdPais = this.RequestId
                                }, DescricaoUsuarioLogado= base.UsuarioLogado.Nome, IdUsuarioLogado= base.UsuarioLogado.Id
                            });
                        break;

                    case "Contratos":

                        lResposta = InserirItemDeSistema(
                            new SalvarEntidadeCadastroRequest<ContratoInfo>()
                            {
                                EntidadeCadastro = new ContratoInfo()
                                {
                                    IdContrato = int.Parse(this.RequestId)
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;

                    case "TiposDePendenciaCadastral":

                        lResposta = InserirItemDeSistema(
                            new SalvarEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                            {
                                EntidadeCadastro = new TipoDePendenciaCadastralInfo()
                                {
                                    //IdTipoPendencia = int.Parse(this.RequestId),
                                    DsPendencia = this.RequestDescricao,
                                     StAutomatica = this.RequestPendenciaAutomatica
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;
                        
                    case "TaxasDeTermo":

                        lRetorno = CadastrarTaxaDeTermo();

                        return lRetorno;

                    default:
                        break;
                }

                TransporteObjetoDoSistema lObjetoDeRetorno = new TransporteObjetoDoSistema();

                lObjetoDeRetorno.Id = lResposta.DescricaoResposta;

                lObjetoDeRetorno.Descricao = this.RequestDescricao;

                lRetorno = RetornarSucessoAjax(lObjetoDeRetorno, "Objeto incluído com sucesso!");

                base.RegistrarLogInclusao(this.RequestTipoDeObjeto);
            }
            catch(Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao incluir o objeto: " + ex.Message);
            }

            return lRetorno;
        }

        private string ResponderExcluir()
        {
            RemoverEntidadeCadastroResponse lResposta = null;
            string lRetorno = string.Empty;
            try
            {
                switch (this.RequestTipoDeObjeto)
                {
                    case "AtividadesIlicitas":
                        lResposta = ExcluirItemDeSistema(
                            new RemoverEntidadeCadastroRequest<AtividadeIlicitaInfo>()
                            {
                                EntidadeCadastro = new AtividadeIlicitaInfo()
                                {
                                    IdAtividadeIlicita = int.Parse(this.RequestId)
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;

                    case "PaisesEmListaNegra":

                        lResposta = ExcluirItemDeSistema(
                            new RemoverEntidadeCadastroRequest<PaisesBlackListInfo>()
                            {
                                EntidadeCadastro = new PaisesBlackListInfo()
                                {
                                    IdPaisBlackList = int.Parse(this.RequestId)
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;

                    case "Contratos":

                        lResposta = ExcluirItemDeSistema(
                            new RemoverEntidadeCadastroRequest<ContratoInfo>()
                            {
                                EntidadeCadastro = new ContratoInfo()
                                {
                                    IdContrato = int.Parse(this.RequestId)
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;

                    case "TiposDePendenciaCadastral":

                        lResposta = ExcluirItemDeSistema(
                            new RemoverEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                            {
                                EntidadeCadastro = new TipoDePendenciaCadastralInfo()
                                {
                                    IdTipoPendencia = int.Parse(this.RequestId),
                                },
                                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                                IdUsuarioLogado = base.UsuarioLogado.Id
                            });
                        break;
                    default:
                        break;
                }

                lRetorno = RetornarSucessoAjax("Objeto excluido com sucesso!");
                base.RegistrarLogExclusao(this.RequestTipoDeObjeto);
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax(string.Concat("Erro ao Excluir o item: ", ex.Message));
            }

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rptListaDeItens.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rptListaDeItens_ItemDataBound);
        }

        void rptListaDeItens_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (this.RequestTipoDeObjeto == "TiposDePendenciaCadastral")
            {
                HtmlButton btnExcluirObjeto = (HtmlButton)e.Item.FindControl("btnExcluirObjeto");
                btnExcluirObjeto.Visible = PodeExcluirPendencia;
            }
        }

        bool PodeExcluirPendencia = false;
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                     , "ReceberArquivo"
                                                     , "Incluir"
                                                     , "Excluir"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados 
                                                     , ResponderReceberArquivo
                                                     , ResponderIncluir
                                                     , ResponderExcluir 
                                                     });
        }

        #endregion
    }
}
