using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    public class Comando : ItemBase
    {
        public Janela Janela { get; set; }

        public ComandoInfo Info { get; set; }

        public IComando Instancia { get; set; }

        public Comando(ComandoInfo comandoInfo, Janela janela)
        {
            // Inicializa
            this.Info = comandoInfo;
            this.Janela = janela;

            // Cria a instancia do objeto
            this.Instancia = (IComando)this.CriarObjeto(this.Info, this);
        }
    }
}
