using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GradualForm
{
    public class MontagemFiltro
    {
        public Label TituloFiltro { get; set; }
        public Panel PainelFiltro { get; set; }
    }

    public class FilterSettings
    {
        public class CamposFiltro
        {
            public String Descricao { get; set; }
            public Size DescricaoTamanho { get; set; }
            public Object Conteudo { get; set; }
            public Size ConteudoTamanho { get; set; }
            public EventHandler<FilterEventArgs> Handler { get; set; }
            public CamposFiltro(String Descricao, Size DescricaoTamanho, Object Conteudo, Size ConteudoTamanho)
            {
                this.Descricao = Descricao;
                this.DescricaoTamanho = DescricaoTamanho;
                this.Conteudo = Conteudo;
                this.ConteudoTamanho = ConteudoTamanho;
            }

            public CamposFiltro(String Descricao, Size DescricaoTamanho, Object Conteudo, Size ConteudoTamanho, EventHandler<FilterEventArgs> Handler)
            {
                this.Descricao = Descricao;
                this.DescricaoTamanho = DescricaoTamanho;
                this.Conteudo = Conteudo;
                this.ConteudoTamanho = ConteudoTamanho;
                this.Handler = Handler;
            }
        }

        public class Filtro
        {
            public String Nome { get; set; }
            public bool Ascendente { get; set; }
            public Size Tamanho { get; set; }
            public Rectangle PosicaoCabecalhoGrid { get; set; }
            public List<CamposFiltro> CamposTextBox = new List<CamposFiltro>();
            public List<CamposFiltro> CamposComboBox = new List<CamposFiltro>();
            public List<CamposFiltro> CamposCheckBox = new List<CamposFiltro>();
            public List<CamposFiltro> CamposBuscaTextBox = new List<CamposFiltro>();
            public CamposFiltro BotaoOK = null;
        }

    }
}
