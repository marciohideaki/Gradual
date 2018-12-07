using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados
{
    [Serializable]
    [DataContract]
    public class PaginaInfo
    {
        /// <summary>
        /// ID da tabela
        /// </summary>
        [DataMember]
        public Nullable<int> CodigoPagina { get; set; }

        /// <summary>
        /// Nome da Pagina
        /// </summary>
        [DataMember]
        public string NomePagina { get; set; }

        /// <summary>
        /// Descrição da URL
        /// </summary>
        [DataMember]
        public string DescURL { get; set; }

        /// <summary>
        /// Galho em que esta página está dentro da árvora do site
        /// </summary>
        [DataMember]
        public string Galho
        {
            get
            {
                string lRetorno = "";

                if (this.DescURL.Contains('/'))
                {
                    lRetorno = this.DescURL.Substring(0, this.DescURL.LastIndexOf('/'));
                }

                return lRetorno + "/";
            }
        }

        /// <summary>
        /// Tipo de estrutura
        /// </summary>
        [DataMember]
        public string TipoEstrutura { get; set; }

        /// <summary>
        /// Lista de versões
        /// </summary>
        [DataMember]
        public List<VersaoInfo> Versoes { get; set; }

        public VersaoInfo VersaoPublicada
        {
            get
            {
                if (this.Versoes != null)
                {
                    foreach (VersaoInfo lInfo in this.Versoes)
                    {
                        if (lInfo.Publicada)
                            return lInfo;
                    }
                }

                return null;
            }
        }

        public VersaoInfo BuscarVersao(string pIdentificador, bool pRetornarVersaoPublicadaCasoNaoEncontre = false)
        {
            if (this.Versoes == null)
                return null;

            foreach (VersaoInfo lVersao in this.Versoes)
            {
                if (!string.IsNullOrEmpty(pIdentificador))
                {
                    if (lVersao.CodigoDeIdentificacao == pIdentificador)
                        return lVersao;
                }
                else
                {
                    if (lVersao.Publicada == true)
                        return lVersao;
                }

            }

            if (!pRetornarVersaoPublicadaCasoNaoEncontre)
            {
                return null;
            }
            else
            {
                return this.VersaoPublicada;
            }
        }
    }
}
