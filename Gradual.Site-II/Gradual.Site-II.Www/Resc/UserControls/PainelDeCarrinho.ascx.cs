using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class PainelDeCarrinho : UserControlBase
    {
        #region Propriedades

        public string ListaDeEstados
        {
            get
            {
                return JsonConvert.SerializeObject(ConfiguracoesValidadas.EstadosPermitidosEntregaCambio);
            }
        }

        public string ListaDeEstadosLegivel
        {
            get
            {
                return this.ListaDeEstados.Replace("\"", "").Replace("[", "").Replace("]", "");
            }
        }

        public bool EnderecoDaContaPermitido { get; set; }

        #endregion

        #region Métodos Private

        private void CarregarDados()
        {
            List<Intranet.Contratos.Dados.SinacorListaInfo> lEstados = new List<Intranet.Contratos.Dados.SinacorListaInfo>();

            for (int a = 0; a < DadosDeAplicacao.Estados.Count; a++)
            {
                if (ConfiguracoesValidadas.EstadosPermitidosEntregaCambio.Contains(DadosDeAplicacao.Estados[a].Id))
                {
                    lEstados.Add(DadosDeAplicacao.Estados[a]);
                }
            }

            rptEndEntrega_Estado.DataSource = this.PaginaBase.ListaComSelecione(lEstados);

            //rptEndEntrega_Estado.DataSource = this.PaginaBase.ListaComSelecione(DadosDeAplicacao.Estados);
            rptEndEntrega_Estado.DataBind();

            this.EnderecoDaContaPermitido = true;

            if (SessaoClienteLogado != null)
            {
                pnlCarrinho_NomeEmail.Visible = false;

                List<Intranet.Contratos.Dados.ClienteEnderecoInfo> lEnderecos = SessaoClienteLogado.BuscarEnderecos();

                List<Intranet.Contratos.Dados.ClienteTelefoneInfo> lTelefones = SessaoClienteLogado.BuscarTelefones();

                if (lEnderecos.Count > 0)
                {
                    foreach (Intranet.Contratos.Dados.ClienteEnderecoInfo lEndereco in lEnderecos)
                    {
                        if (lEndereco.StPrincipal)
                        {
                            lblEnderecoDaConta.Text = string.Format("{0}, {1}, {2} - {3}/{4}"
                                                                    , lEndereco.DsLogradouro
                                                                    , lEndereco.DsNumero + " " + lEndereco.DsComplemento
                                                                    , lEndereco.DsBairro
                                                                    , lEndereco.DsCidade
                                                                    , lEndereco.CdUf);

                            if (!ConfiguracoesValidadas.EstadosPermitidosEntregaCambio.Contains(lEndereco.CdUf))
                            {
                                this.EnderecoDaContaPermitido = false;
                            }

                            Session["Carrinho_EnderecoDaConta"] = lEndereco;
                        }
                    }
                }

                if (lTelefones.Count > 0)
                {
                    foreach (Intranet.Contratos.Dados.ClienteTelefoneInfo lTel in lTelefones)
                    {
                        if (lTel.IdTipoTelefone == 3)
                        {
                            txtCarrinho_Cel_DDD.Value = lTel.DsDdd;
                            txtCarrinho_Cel_Numero.Value = lTel.DsNumero;
                        }
                        else
                        {
                            txtCarrinho_Tel_DDD.Value = lTel.DsDdd;
                            txtCarrinho_Tel_Numero.Value = lTel.DsNumero;
                        }
                    }
                }
            }
            else
            {
                pnlCarrinho_NomeEmail.Visible = true;
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CarregarDados();
            }
        }

        #endregion
    }
}