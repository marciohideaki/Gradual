using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa uma operação simples em bolsa de valores.
	/// </summary>
    public class OperacaoBolsaInfo : OperacaoInfo
    {
		/// <summary>
		/// Indica a carteira que deve ser valorizada a operação.
		/// </summary>
        public string CodigoCarteira { get; set; }

		/// <summary>
		/// Indica a corretora onde a operação foi executada.
		/// </summary>
        public string CodigoCorretora { get; set; }

		/// <summary>
		/// Indica a conta que será sensibilizada por esta operação, caso haja movimentação
		/// financeira.
		/// </summary>
        public string CodigoConta { get; set; }

		/// <summary>
		/// Indica o valor dos custos de corretagem.
		/// </summary>
        public PrecoInfo CustoCorretagem { get; set; }

		/// <summary>
		/// Indica o valor com os custos de emolumentos.
		/// </summary>
        public PrecoInfo CustoEmolumentos { get; set; }

		/// <summary>
		/// Indica a direcao da ordem: compra ou venda.
		/// </summary>
        public string Direcao { get; set; }

		/// <summary>
		/// Indica o instrumento da operação.
		/// </summary>
        public InstrumentoInfo Instrumento { get; set; }

		/// <summary>
		/// Indica o preço da operação.
		/// </summary>
        public PrecoInfo Preco { get; set; }

		/// <summary>
		/// Indica a quantidade da operação.
		/// </summary>
        public double Quantidade { get; set; }
    }
}
