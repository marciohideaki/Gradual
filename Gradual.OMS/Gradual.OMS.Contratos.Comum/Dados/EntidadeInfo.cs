using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Classe de dados para carregar informações de entidade.
    /// Utilizado para persistir entidades em arquivos.
    /// </summary>
    [Serializable]
    public class EntidadeInfo
    {
        /// <summary>
        /// Codigo da entidade
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Data de criação, ou de primeira entrada da entidade na persistencia
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data da ultima alteração da entidade na persistencia
        /// </summary>
        public DateTime DataUltimaAlteracao { get; set; }

        /// <summary>
        /// Indica o tipo do objeto da entidade. Este é o tipo do objeto
        /// informado no momento em que foi feito o pedido para salvar.
        /// </summary>
        public string TipoObjeto { get; set; }

        /// <summary>
        /// Indica o tipo do objeto real salvo. Pode ser um subtipo do
        /// objeto informado no momento de salvar.
        /// </summary>
        public string TipoObjetoSalvo { get; set; }

        /// <summary>
        /// Indica o assembly do objeto.
        /// Utilizado para desserializar om objeto via serialização xml
        /// </summary>
        public string AssemblyObjeto { get; set; }

        /// <summary>
        /// Indica o assembly do objeto salvo.
        /// </summary>
        public string AssemblyObjetoSalvo { get; set; }

        /// <summary>
        /// Tipo de serialização da entidade
        /// </summary>
        public EntidadeTipoSerializacaoEnum TipoSerializacao { get; set; }

        /// <summary>
        /// Serializacao da entidade
        /// </summary>
        public byte[] Serializacao { get; set; }
    }
}
