using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa uma carteira. A carteira é um agrupamento gerencial de operações e
	/// resultados. As carteiras podem ser representadas hierarquicamente, ou seja,
	/// pode existir carteiras dentro de carteiras.
	/// </summary>
    public class CarteiraInfo
    {
		/// <summary>
		/// Referência para a carteira pai, caso exista.
		/// </summary>
        public CarteiraInfo CarteiraPai { get; set; }

		/// <summary>
		/// Lista de carteiras filhas, caso existam.
		/// </summary>
        public List<CarteiraInfo> CarteirasFilhas { get; set; }

		/// <summary>
		/// Código da carteira.
		/// </summary>
        public string CodigoCarteira { get; set; }

		/// <summary>
		/// Descrição da carteira.
		/// </summary>
        public string Descricao { get; set; }

		/// <summary>
		/// Nome da carteira.
		/// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Construtor.
        /// </summary>
        public CarteiraInfo()
        {
            this.CarteirasFilhas = new List<CarteiraInfo>();
        }

        /// <summary>
        /// Retorna a carteira raiz.
        /// Desce na hierarquia até achar uma carteira sem pai.
        /// </summary>
        /// <returns></returns>
        public CarteiraInfo ReceberCarteiraRaiz()
        {
            if (this.CarteiraPai == null)
                return this;
            else
                return this.CarteiraPai.ReceberCarteiraRaiz();
        }
    }
}
