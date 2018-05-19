using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes
{
    public partial class Suitability : PaginaBaseAutenticada
    {
        #region Globais

        private Dictionary<int, int[]> gPontuacaoDasRespostas;

        #endregion

        #region Propriedades

        private Dictionary<byte, int> _RequestRespostas = null;

        public Dictionary<byte, int> RequestRespostas
        {
            get
            {
                if (_RequestRespostas == null)
                {
                    string lRespostas = Request.Form["Respostas"];

                    if (!string.IsNullOrEmpty(lRespostas))
                    {
                        string[] lResp = lRespostas.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] lResposta;
                        
                        _RequestRespostas = new Dictionary<byte, int>();

                        for (int a = 0; a < lResp.Length; a++)
                        {
                            lResposta = lResp[a].Split(':');
                            
                            _RequestRespostas.Add(Convert.ToByte(lResposta[0]), Convert.ToInt32(lResposta[1].Replace("|","")));
                        }
                    }
                }

                return _RequestRespostas;
            }
        }
        
        public int RequestIdCliente
        {
            get
            {
                return Convert.ToInt32(Request.Form["Id"]);
            }
        }

        #endregion

        #region Métodos Private

        // Intranet
        private void CarregarPontuacaoDoQuestionario()
        {
            this.gPontuacaoDasRespostas = new Dictionary<int, int[]>();
            this.gPontuacaoDasRespostas.Add(1 , new int[] { 1, 2, 3, 5      });
            this.gPontuacaoDasRespostas.Add(2 , new int[] { 1, 2, 4, 5      });
            this.gPontuacaoDasRespostas.Add(3 , new int[] { 1, 2, 4, 5      });
            this.gPontuacaoDasRespostas.Add(4 , new int[] { 1, 2, 3, 4, 5   });
            this.gPontuacaoDasRespostas.Add(5 , new int[] { 1, 2, 3, 4, 5   });
            this.gPontuacaoDasRespostas.Add(6 , new int[] { 1, 2, 2, 2      });
            this.gPontuacaoDasRespostas.Add(7 , new int[] { 1, 2, 4, 5, 5   });
            this.gPontuacaoDasRespostas.Add(8 , new int[] { 1, 2, 2, 3, 4   });
            this.gPontuacaoDasRespostas.Add(9 , new int[] { 0, 0, 0, 0, 0   }); //0.25 0.25 0.5 1.5 2.5
            this.gPontuacaoDasRespostas.Add(10, new int[] { 5, 4, 3, 2      });
            this.gPontuacaoDasRespostas.Add(11, new int[] { 5, 4, 3         });
        }

        private string ResponderCarregarHtmlComDados()
        {
            this.pnlCliente_Suitability_Questionario.Visible 
                = this.pnlCliente_Suitability_BotaoEnviar.Visible 
                = this.pnlCliente_Suitability_BotaoRefazerTeste.Visible
                = UsuarioPode("Efetivar", "6346025B-D61B-46ee-BD71-2BD862135B1B");

            ClienteSuitabilityInfo lDadosDoCliente = new ClienteSuitabilityInfo();

            ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>  lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> lResponse; 

            lDadosDoCliente.IdCliente = int.Parse(Request["Id"]);

            lRequest.EntidadeCadastro = lDadosDoCliente;
            
            lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

            lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count > 0)
                {
                    //cliente já tinha feito um suitability antes; a proc está retornando por data decrescente, então o primeiro
                    //resultado tem que ser o mais novo (o que está válido)
                    
                    if (lResponse.Resultado[0].ds_perfil.ToLower().Contains("medio"))   //  "convertendo" o valores antigos de médio com/médio sem para o novo "moderado"
                    {
                        lResponse.Resultado[0].ds_perfil = "Moderado";
                    }

                    if (lResponse.Resultado[0].ds_perfil.ToLower().Contains("baixo"))   //  "convertendo" o valores antigos de médio com/médio sem para o novo "moderado"
                    {
                        lResponse.Resultado[0].ds_perfil = "Conservador";
                    }

                    hidCliente_Suitability_Resultado.Value = JsonConvert.SerializeObject( new TransporteSuitability( lResponse.Resultado[0]) );
                }
                else
                {
                    //cliente nunca fez suitability, cadastrar como "acessado" ?
                }
            }
            else
            {
                throw new Exception(string.Format("Erro ao consultar suitability do cliente: {0}", lResponse.DescricaoResposta));
            }

            return string.Empty;    //só para obedecer assinatura
        }

        private string VerificarPontuacaoDoSuitability()
        {
            string lResposta;
            decimal lSomatoria = 0;

            this.CarregarPontuacaoDoQuestionario();

            lSomatoria += gPontuacaoDasRespostas[1] [this.RequestRespostas[1]];
            lSomatoria += gPontuacaoDasRespostas[2] [this.RequestRespostas[2]];
            lSomatoria += gPontuacaoDasRespostas[3] [this.RequestRespostas[3]];
            lSomatoria += gPontuacaoDasRespostas[4] [this.RequestRespostas[4]];
            lSomatoria += gPontuacaoDasRespostas[5] [this.RequestRespostas[5]];
            lSomatoria += gPontuacaoDasRespostas[6] [this.RequestRespostas[6]];
            lSomatoria += gPontuacaoDasRespostas[7] [this.RequestRespostas[7]];
            lSomatoria += gPontuacaoDasRespostas[8] [this.RequestRespostas[8]];
            //lSomatoria += gPontuacaoDasRespostas[9] [this.RequestRespostas[9]];
            lSomatoria += gPontuacaoDasRespostas[10][this.RequestRespostas[10]];
            lSomatoria += gPontuacaoDasRespostas[11][this.RequestRespostas[11]];

            //String[] lOpcoes = this.RequestRespostas[9].ToString();
            String[] lOpcoes = this.RequestRespostas[9].ToString().ToCharArray().Select(digit => digit.ToString()).ToArray();

            foreach (String lOpcao in lOpcoes)
            {

                if (!String.IsNullOrEmpty(lOpcao))
                {
                    Int32 lQuest = Int32.Parse(lOpcao) -1;

                    switch (lQuest.ToString())
                    {
                        case "0":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.25;
                            break;
                        case "1":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.25;
                            break;
                        case "2":
                            lSomatoria = (decimal)lSomatoria + (decimal)0.5;
                            break;
                        case "3":
                            lSomatoria = (decimal)lSomatoria + (decimal)1.5;
                            break;
                        case "4":
                            lSomatoria = (decimal)lSomatoria + (decimal)2.5;
                            break;
                    }
                }
            }

            /*
            RESULTADOS
                Conservador  De 0 a 15 pontos
                Moderado     De 15 a 22 pontos
                Arrojado     Acima de 22 pontos
            */

            if (lSomatoria <= 15)
            {
                lResposta = "Conservador";
            }
            else if (lSomatoria > 15 && lSomatoria <= 22)
            {
                lResposta = "Moderado";
            }
            else
            {
                lResposta = "Arrojado";
            }

            return lResposta;
        }

        private string ResponderSalvarQuestionario()
        {
            string lResposta = string.Empty;

            try
            {
                var lInfo = new ClienteSuitabilityInfo();

                lInfo.ds_status                = "Finalizado";
                lInfo.ds_fonte                 = "Intranet";
                lInfo.dt_realizacao            = DateTime.Now;
                lInfo.IdCliente                = this.RequestIdCliente;
                lInfo.ds_loginrealizado        = this.UsuarioLogado.Nome;
                lInfo.ds_respostas             = this.Request.Form["Respostas"];
                lInfo.st_preenchidopelocliente = false;

                lInfo.ds_perfil = VerificarPontuacaoDoSuitability();
                try
                {
                    var lRequest = new SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>();

                    lRequest.EntidadeCadastro = lInfo;

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    var lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

                    if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                    {
                        lResposta = RetornarSucessoAjax(new TransporteSuitability(lInfo), "Suitability efetuado com sucesso!");

                        base.RegistrarLogInclusao(new Contratos.Dados.Cadastro.LogIntranetInfo()
                        {   //--> Registrando o Log
                            IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente,
                            DsObservacao = string.Concat("Suitability do cliente: id_cliente = ", lRequest.EntidadeCadastro.IdCliente)
                        });

                        EnviarEmailComPerfilDoInvestidor(this.Request.Form["Email"], lRequest.EntidadeCadastro.IdCliente, lInfo.ds_perfil);
                    }
                    else
                    {
                        lResposta = RetornarErroAjax("Erro de retorno do serviço de cadastro: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
                    }
                }
                catch (Exception ex2)
                {
                    lResposta = RetornarErroAjax("Erro ao enviar resposta do suitability", ex2);
                }
            }
            catch (Exception ex)
            {
                lResposta = RetornarErroAjax("Erro ao carregar dados de resposta", ex);
            }

            return lResposta;
        }

        private string ResponderUploadDeArquivo()
        {
            string lRetorno = "";

            try
            {
                //List<string> lListaDeURLs = new List<string>();

                string lDiretorio;

                string lUrl = "";
                string lCaminho  = "";

                for (int a = 0; a < Request.Files.Count; a++)
                {
                    string lPath = "~/";

                    if (ConfiguracoesValidadas.PastaDeUpload_DeclaracaoSuitability.StartsWith("/"))
                        lPath = "~";

                    lCaminho = Path.Combine(Server.MapPath( lPath + ConfiguracoesValidadas.PastaDeUpload_DeclaracaoSuitability ), Path.GetFileName(Request.Files[a].FileName));

                    if (File.Exists(lCaminho))
                    {
                        lCaminho = lCaminho.Insert(lCaminho.LastIndexOf('.'), "-" + DateTime.Now.Ticks.ToString());

                        //File.Delete(lCaminho);
                    }
                    
                    lUrl = lPath + ConfiguracoesValidadas.PastaDeUpload_DeclaracaoSuitability + "/" + Path.GetFileName(lCaminho);

                    lDiretorio = Path.GetDirectoryName(lCaminho);

                    if(!Directory.Exists(lDiretorio))
                    {
                        //gLogger.InfoFormat("Diretório inexistente; tentando criar [{0}]...", lDiretorio);

                        Directory.CreateDirectory(lDiretorio);

                        //gLogger.InfoFormat("Diretório criado com sucesso");
                    }

                    Request.Files[a].SaveAs(lCaminho);
                }

                
                ClienteSuitabilityInfo lDadosDoCliente = new ClienteSuitabilityInfo();

                SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>  lRequest = new SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
                SalvarEntidadeCadastroResponse lResponse;

                lRequest.DescricaoUsuarioLogado = "UPD_SUITABILITY";    //HACKerson.

                lDadosDoCliente.IdCliente = int.Parse(Request["Id"]);
                lDadosDoCliente.ds_arquivo_ciencia = lUrl;

                lRequest.EntidadeCadastro = lDadosDoCliente;

                lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax((object)(lUrl), "ok");
                }
                else
                {
                    lRetorno = RetornarErroAjax("Upload realizado, erro ao atualizar: [{1}] [{0}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao realizar upload", ex);
            }

            return lRetorno;
        }

        #endregion

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (Request.Files.Count > 0)
            {
                Response.Clear();

                Response.Write(ResponderUploadDeArquivo());

                Response.End();
            }
            else
            {
                RegistrarRespostasAjax(new string[] { 
                                                        "CarregarHtmlComDados"
                                                        , "Salvar"
                                                    },
                        new ResponderAcaoAjaxDelegate[] { 
                                                        ResponderCarregarHtmlComDados
                                                        ,  ResponderSalvarQuestionario
                                                    });
            }
        }

        private void EnviarEmailComPerfilDoInvestidor(String pEmailDestino, Nullable<int> pIdCliente, String pPerfil)
        {
            string lNomeArquivoEmail = string.Empty;
            
            switch (pPerfil)
            {
                case "Arrojado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Arrojado.html");
                    break;

                case "Conservador":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Conservador.html");
                    break;

                case "Moderado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Moderado.html");
                    break;
            }

            List<String> lDestinatarios = new List<String>() { pEmailDestino };

            base.EnviarEmailSuitability(pIdCliente, pPerfil, lDestinatarios, "Perfil do Investidor | Confira o seu portfólio recomendado", lNomeArquivoEmail, new Dictionary<string, string>(), Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Compliance, null);

            
        }
    }
}
