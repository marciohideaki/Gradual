using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;

namespace Gradual.FIDC.Adm.Web
{
    [Serializable]
    public class Usuario
    {
        #region Propriedades
        /// <summary>
        /// TIpo Acesso usuário
        /// </summary>
        public TipoAcesso TipoAcesso { get; set; }
        /// <summary>
        /// Sessão do cliente obtida na autenticação do usuário.
        /// </summary>
        public string CodigoDaSessao { get; set; }

        public int CodAssessor { get; set; }

        /// <summary>
        /// Código de conta corrente do cliente.
        /// </summary>
        public string IdDoUsuario { get; set; }

        public int IdLogin { get; set; }

        /// <summary>
        /// (get) ID do Usuário já convertido pra Int32
        /// </summary>
        public int IdDoUsuarioTipoInt
        {
            get
            {
                return Convert.ToInt32(this.IdDoUsuario);
            }
        }

        public string CodBovespa { get; set; }

        public int CodBovespaTipoInt
        {
            get
            {
                int lCodBMF = 0;

                if (this.CodBovespa != null && int.TryParse(this.CodBovespa.Trim(), out lCodBMF))
                {
                    return lCodBMF;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int CodBmf { get; set; }

        public string Nome { get; set; }

        public string PrimeiroNome
        {
            get
            {
                if (this.Nome.Contains(" "))
                {
                    return this.Nome.Substring(0, this.Nome.IndexOf(" "));
                }
                else
                {
                    return this.Nome;
                }
            }
        }

        /// <summary>
        /// Essa propriedade deve estar depreciada; o HB só vai usar CC. Está aqui para refatoramento posterior
        /// </summary>
        public string CodigoDaContaInvestimento
        {
            get
            {
                /*
                if (Session["UserIDCI"] == null)
                {
                    BuscarContaInvestimentoResponse lRespostaContaInvestimento;

                    lRespostaContaInvestimento = ServicoDeCliente.BuscarContaInvestimento(new BuscarContaInvestimentoRequest()
                                                                                          {
                                                                                              CodigoCliente = this.IdDoUsuarioLogadoTipoInt
                                                                                          });

                    if (lRespostaContaInvestimento.Sucesso)
                    {
                        Session["UserIDCI"] = lRespostaContaInvestimento.Resposta;
                    }
                    else
                    {
                        Session["UserIDCI"] = "-1";
                    }
                }

                return Session["UserIDCI"].ToString();
                 */

                return "00000000";
            }
        }

        //public TransporteSaldoDeConta SaldoDaConta { get; set; }

        public int ErrosDeBuscarResposta { get; set; }

        #endregion

        #region Construtores

        public Usuario()
        {
            //this.SaldoDaConta = new TransporteSaldoDeConta();
        }

        /// <summary>
        /// Construtor da classe do Usuario
        /// </summary>
        /// <param name="pCodigoDaSessao">Sessão obtida na autenticação do cliente.</param>
        public Usuario(string pCodigoDaSessao):this()
        {
            this.CodigoDaSessao = pCodigoDaSessao;
        }

        #endregion
    }
    public enum TipoAcesso
    {
        Cliente       = 0,
        Cadastro      = 1,
        Assessor      = 2,
        Atendimento   = 3,
        TeleMarketing = 4,
        Outros        = 5,
        Officer       = 49,
        PontaMesa     = 50
    }
}
