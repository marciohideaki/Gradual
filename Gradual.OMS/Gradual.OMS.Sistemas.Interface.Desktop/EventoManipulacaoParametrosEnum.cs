using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Interface.Desktop
{
    /// <summary>
    /// Enumerador para indicar qual evento está ocorrendo para justificar
    /// o pedido de salvar ou carregar parametros para o controle, janela, etc.
    /// </summary>
    public enum EventoManipulacaoParametrosEnum
    {
        /// <summary>
        /// Utilizado para indicar o momento da persistencia ou carga da persistencia
        /// </summary>
        Persistencia,   
        
        /// <summary>
        /// Utilizado para indicar o momento de mostrar tela de configuracoes ou carregar após configurações
        /// </summary>
        Configuracao, 

        /// <summary>
        /// Utilizado para indicar o momento de salvar um modelo de parametros
        /// </summary>
        SalvarModelo   
    }
}
