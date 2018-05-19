using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.MDS.Core.Sinal
{
    public class LNGDadosNegocio
    {
        public string NumeroNegocio { get; set; }
        public string Hora { get; set; }
        public Decimal Preco { get; set; }
        public long Quantidade { get; set; }
        public string Compradora {get;set; }
        public string Vendedora { get; set; }

        public LNGDadosNegocio() { }

        public LNGDadosNegocio(
			string numeroNegocio, 
			string hora, 
			Decimal preco,
			long quantidade, 
			string compradora, 
			string vendedora) 
	    {
		    this.NumeroNegocio = numeroNegocio;
		    this.Hora = hora;
		    this.Preco = preco;
		    this.Quantidade = quantidade;
		    this.Compradora = compradora;
		    this.Vendedora = vendedora;
	    }

    }
}
