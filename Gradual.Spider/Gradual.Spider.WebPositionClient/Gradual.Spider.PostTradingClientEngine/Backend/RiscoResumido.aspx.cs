using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Spider.PositionClient.DbLib;
using Gradual.Spider.PositionClient.Lib.Messages;
using Gradual.Spider.PostTradingClientEngine.App_Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using appCodigo = Gradual.Spider.PostTradingClientEngine.App_Codigo;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Lib.Dados;
using Gradual.Spider.PostTradingClientEngine.App_Codigo.TransporteJSon;

namespace Gradual.Spider.PostTradingClientEngine.Backend
{
    /// <summary>
    /// Página para consulta de risco de posção do cliente online 
    /// de Risco Resumido
    /// </summary>
    public partial class RiscoResumido : PaginaBase
    {
        #region Atributos

        #endregion

        #region Propriedades
        /// <summary>
        /// Código de cliente para filtro
        /// </summary>
        public int CodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Código de Instrumento para filtro
        /// </summary>
        public string CodigoInstrumento
        {
            get
            {
                var lRetorno = default(string);

                if (this.Request["CodigoInstrumento"] != null)
                {
                    lRetorno = this.Request["CodigoInstrumento"].ToString();
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção PL Somente Lucro
        /// </summary>
        public bool OpcaoPLSomenteComLucro
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPLSomenteComLucro"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPLSomenteComLucro"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção PL Negativo
        /// </summary>
        public bool OpcaoPLSomentePLnegativo
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPLSomentePLnegativo"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPLSomentePLnegativo"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção SFP Atingido Até 25%
        /// </summary>
        public bool OpcaoSFPAtingidoAte25
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoSFPAtingidoAte25"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoSFPAtingidoAte25"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção SFP Atingido Entre 25% e 50%
        /// </summary>
        public bool OpcaoSFPAtingidoEntre25e50
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoSFPAtingidoEntre25e50"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoSFPAtingidoEntre25e50"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção SFP Atingido Entre 50% e 75%
        /// </summary>
        public bool OpcaoSFPAtingidoEntre50e75
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoSFPAtingidoEntre50e75"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoSFPAtingidoEntre50e75"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção SFP Atingido Acima 75%
        /// </summary>
        public bool OpcaoSFPAtingidoAcima75
        {
            get
            {
                
                var lRetorno = default(bool);

                if (this.Request["OpcaoSFPAtingidoAcima75"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoSFPAtingidoAcima75"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Até 2K
        /// </summary>
        public bool OpcaoPrejuizoAtingidoAte2K
        {
            get
            {
                
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoAte2K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoAte2K"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Entre 2k e 5k
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre2Ke5K
        {
            get
            {
                
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoEntre2Ke5K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoEntre2Ke5K"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Entre 5k e 10k
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre5Ke10K
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoEntre5Ke10K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoEntre5Ke10K"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Entre 10k e 20k
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre10Ke20K
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoEntre10Ke20K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoEntre10Ke20K"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Entre 20k e 50k
        /// </summary>
        public bool OpcaoPrejuizoAtingidoEntre20Ke50K
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoEntre20Ke50K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoEntre20Ke50K"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Prejuízo Atingido Acima de 50k
        /// </summary>
        public bool OpcaoPrejuizoAtingidoAcima50K
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoPrejuizoAtingidoAcima50K"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoPrejuizoAtingidoAcima50K"]);
                }

                return lRetorno;
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento de load da pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax(new string[] {
                "CarregarRiscoResumido"
                }, new ResponderAcaoAjaxDelegate[] 
                {
                    ResponderCarregarRiscoResumido
                });


            }
            catch (Exception ex)
            {
                gLogger.Error("Erro ao carregar pagina de risco resumido->", ex);
                //throw;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que efetua de busca de operações no banco de dados
        /// </summary>
        /// <returns>Retorna uma string serializado json</returns>
        public string ResponderCarregarRiscoResumido()
        {
            string lRetorno = string.Empty;

            try
            {
                var lServico = Ativador.Get<IPositionClientRiscoResumido>();

                var lRequest = new BuscarRiscoResumidoRequest();

                lRequest.CodigoCliente = CodigoCliente;


                if (this.OpcaoPLSomentePLnegativo)
                {
                    lRequest.OpcaoPL |= OpcaoPL.SomentePLnegativo;
                }

                if (this.OpcaoPLSomenteComLucro)
                {
                    lRequest.OpcaoPL |= OpcaoPL.SomenteComLucro;
                }

                if (this.OpcaoSFPAtingidoAte25)
                {
                    lRequest.OpcaoSFPAtingido |= OpcaoSFPAtingido.Ate25;
                }

                if (this.OpcaoSFPAtingidoEntre25e50)
                {
                    lRequest.OpcaoSFPAtingido |= OpcaoSFPAtingido.Entre25e50;
                }

                if (this.OpcaoSFPAtingidoEntre50e75)
                {
                    lRequest.OpcaoSFPAtingido |= OpcaoSFPAtingido.Entre50e75;
                 }

                if (this.OpcaoSFPAtingidoAcima75)
                {
                    lRequest.OpcaoSFPAtingido |= OpcaoSFPAtingido.Acima75;
                }

                if (this.OpcaoPrejuizoAtingidoAte2K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Ate2K;
                }

                if (this.OpcaoPrejuizoAtingidoEntre2Ke5K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Entre2Ke5K;
                }

                if (this.OpcaoPrejuizoAtingidoEntre5Ke10K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Entre5Ke10K;
                }

                if (this.OpcaoPrejuizoAtingidoEntre10Ke20K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Entre10Ke20K ;
                }

                if (this.OpcaoPrejuizoAtingidoEntre20Ke50K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Entre20Ke50K;
                }

                if (this.OpcaoPrejuizoAtingidoAcima50K)
                {
                    lRequest.OpcaoPrejuizoAtingido |= OpcaoPrejuizoAtingido.Acima50K;
                }
                var lResponse = lServico.BuscarRiscoResumido(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    var lTrans = new TransporteRiscoResumido(lResponse.ListRiscoResumido);

                    lRetorno = RetornarSucessoAjax(lTrans.ListaTransporte, "Foram encontrados [{0}] operacoes" + lTrans.ListaTransporte.Count, lTrans.ListaTransporte.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}