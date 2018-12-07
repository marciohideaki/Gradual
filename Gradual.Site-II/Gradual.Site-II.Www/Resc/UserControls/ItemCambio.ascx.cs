using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Newtonsoft.Json;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class ItemCambio : UserControlBase
    {
        #region Propriedades

        private CultureInfo gCulture = new CultureInfo("pt-BR");

        public string NomeProduto
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    return this.DadosDoProduto.NomeProduto;
                }
                else
                {
                    return "";
                }
            }
        }

        public string TaxaDeCambio
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    if (this.Modo == "Moeda")
                    {
                        return "R$ " + this.DadosDoProduto.ValorProduto.ToString("n2");
                    }
                    else
                    {
                        return "R$ " + this.DadosDoProduto.ValorProdutoCartao.ToString("n2");
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string IOF
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    return this.DadosDoProduto.Taxa.ToString("n2") + "%";
                }
                else
                {
                    return "";
                }
            }
        }

        public string ImageSrc
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    return this.DadosDoProduto.ImagemExibida;
                }
                else
                {
                    return "";
                }
            }
        }

        public string dataTxCambio
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    if (this.Modo == "Moeda")
                    {
                        return this.DadosDoProduto.ValorProduto.ToString(gCulture).Replace(",", ".");
                    }
                    else
                    {
                        return this.DadosDoProduto.ValorProdutoCartao.ToString(gCulture).Replace(",", ".");
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string dataIOF
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    return this.DadosDoProduto.Taxa.ToString(gCulture).Replace(",", ".");
                }
                else
                {
                    return "";
                }
            }
        }

        public TransporteProduto DadosDoProduto { get; set; }

        public string ProdutoJSON
        {
            get
            {
                if (this.DadosDoProduto != null)
                {
                    return JsonConvert.SerializeObject(this.DadosDoProduto);
                }
                else
                {
                    return "";
                }
            }
        }

        public string Modo { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}