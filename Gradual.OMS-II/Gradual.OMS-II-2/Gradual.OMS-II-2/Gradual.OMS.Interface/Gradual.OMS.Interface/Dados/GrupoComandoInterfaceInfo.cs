using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Gradual.OMS.Library;

namespace Gradual.OMS.Interface.Dados
{
    /// <summary>
    /// Agrupa um conjunto de comandos de interface
    /// </summary>
    [Serializable]
    public class GrupoComandoInterfaceInfo : ICodigoEntidade, ICloneable
    {
        /// <summary>
        /// Código do grupo de comandos de interface.
        /// </summary>
        public string CodigoGrupoComandoInterface { get; set; }

        /// <summary>
        /// Lista de comandos de interface da raiz.
        /// </summary>
        public List<ComandoInterfaceInfo> ComandosInterfaceRaiz { get; set; }

        /// <summary>
        /// Transforma a árvore de comandos em uma lista
        /// </summary>
        /// <returns></returns>
        public List<ComandoInterfaceInfo> ListarComandos()
        {
            // Prepara a resposta
            List<ComandoInterfaceInfo> retorno = new List<ComandoInterfaceInfo>();

            // Adiciona todos os elementos da raiz
            foreach (ComandoInterfaceInfo item in this.ComandosInterfaceRaiz)
                retorno.AddRange(listarComandosRecursivo(item));

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Rotina recursiva para a lista de comandos
        /// </summary>
        /// <param name="raiz"></param>
        /// <returns></returns>
        private List<ComandoInterfaceInfo> listarComandosRecursivo(ComandoInterfaceInfo raiz)
        {
            // Prepara retorno
            List<ComandoInterfaceInfo> retorno = new List<ComandoInterfaceInfo>();

            // Adiciona a si próprio
            retorno.Add(raiz);

            // Varre os filhos pedindo as listas
            foreach (ComandoInterfaceInfo filho in raiz.Filhos)
                retorno.AddRange(listarComandosRecursivo(filho));

            // Retorna
            return retorno;
        }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public GrupoComandoInterfaceInfo()
        {
            this.ComandosInterfaceRaiz = new List<ComandoInterfaceInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoGrupoComandoInterface;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return this.ClonarObjeto<GrupoComandoInterfaceInfo>();
        }

        #endregion
    }
}
