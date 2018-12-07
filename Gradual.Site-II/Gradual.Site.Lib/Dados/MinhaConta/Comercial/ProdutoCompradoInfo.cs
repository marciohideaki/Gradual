using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.Lib.Dados.MinhaConta.Comercial
{
    public class ProdutoCompradoInfo
    {
        #region Propriedades

        public int IdProduto { get; set; }

        public string DescricaoProduto { get; set; }

        public string NomeCompactado
        {
            get
            {
                if (this.DescricaoProduto != null)
                {
                    string lDesc = this.DescricaoProduto.ToLower();

                    if (lDesc.Contains("trader") && lDesc.Contains("interface"))
                    {
                        return "DesktopTraderInterface";
                    }
                    else if (lDesc.Contains("gráfico"))
                    {
                        return "GradualGrafico";
                    }
                    else if (lDesc.Contains("calculadora"))
                    {
                        return "CalculadoraIR";
                    }
                    else
                    {
                        return lDesc;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public int IdVenda { get; set; }

        public string CodigoDeReferenciaDaVenda { get; set; }

        public int CodigoCblc { get; set; }

        public string CpfCnpj { get; set; }

        public int Status { get; set; }

        public string StatusDesc
        {
            get
            {
                switch (this.Status)
                {
                    case 1: return "Aguardando Pagamento";
                    case 2: return "Em Análise";
                    case 3: return "Pago";
                    case 4: return "Disponível";
                    case 5: return "Em Disputa";
                    case 6: return "Devolvido";
                    case 7: return "Cancelado";

                    default:
                        return string.Format("Status_{0}", this.Status);
                }
            }
        }

        public DateTime DataDaCompra { get; set; }

        #endregion
    }
}
