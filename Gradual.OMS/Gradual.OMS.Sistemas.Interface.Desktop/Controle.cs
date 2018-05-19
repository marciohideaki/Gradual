using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Dados;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    /// <summary>
    /// Wrapper para controles que implementam IControle.
    /// </summary>
    public class Controle : ItemBase
    {
        public Janela Janela { get; set; }

        public ControleInfo Info { get; set; }

        public IControle Instancia { get; set; }

        public void SalvarDefault()
        {
            // Inicializa
            IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();
            string tipoObjeto = this.Info.TipoInstanciaString;

            // Recebe os parametros
            object parametros = this.Instancia.SalvarParametros(EventoManipulacaoParametrosEnum.SalvarModelo);

            // Salva na coleção
            if (servicoInterface.ParametrosDefault.ContainsKey(tipoObjeto))
                servicoInterface.ParametrosDefault[tipoObjeto] = parametros;
            else
                servicoInterface.ParametrosDefault.Add(tipoObjeto, parametros);
        }

        public Controle(ControleInfo controleInfo)
        {
            // Inicializa
            this.Info = controleInfo;

            // Cria a instancia do objeto
            this.Instancia = (IControle)this.CriarObjeto(this.Info, this);
        }

        public Controle(ControleInfo controleInfo, Janela janela)
        {
            // Inicializa
            this.Info = controleInfo;
            this.Janela = janela;

            // Cria a instancia do objeto
            this.Instancia = (IControle)this.CriarObjeto(this.Info, this);
        }
    }
}
