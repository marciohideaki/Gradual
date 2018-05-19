using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Automacao.Lib;

namespace Gradual.OMS.AutomacaoDesktop.Events
{
    public class EventoLOFAgrupado
    {
                private string instrumento;
        private string plataforma;
        private string cabecalho;
        private List<LivroOfertasEntry> livroOfertasCompra;
        private List<LivroOfertasEntry> livroOfertasVenda;

        public EventoLOFAgrupado(
                string instrumento,
                string plataforma,
                string cabecalho,
                List<LivroOfertasEntry> livroOfertasCompra,
                List<LivroOfertasEntry> livroOfertasVenda)
        {
            this.instrumento = instrumento;
            this.plataforma = plataforma;
            this.cabecalho = cabecalho;
            this.livroOfertasCompra = livroOfertasCompra;
            this.livroOfertasVenda = livroOfertasVenda;
            return;
        }

        public string Instrumento
        {
            get { return instrumento; }
        }

        public string Plataforma
        {
            get { return plataforma; }
        }

        public string Cabecalho
        {
            get { return cabecalho; }
        }

        public List<LivroOfertasEntry> LivroOfertasCompra
        {
            get { return livroOfertasCompra; }
        }

        public List<LivroOfertasEntry> LivroOfertasVenda
        {
            get { return livroOfertasVenda; }
        }

    }
}
