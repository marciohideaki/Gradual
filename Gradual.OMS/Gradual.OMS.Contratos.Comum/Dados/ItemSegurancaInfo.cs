using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Representa um item qualquer que terá sua acessibilidade testada.
    /// O teste se dará testando as permissões, perfis e grupos informados 
    /// para se descobrir se o acesso é ou não permitido.
    /// </summary>
    [Serializable]
    public class ItemSegurancaInfo
    {
        /// <summary>
        /// Código dos grupos que irão ativar este comando.
        /// </summary>
        [XmlIgnore]
        public List<string> Grupos 
        {
            get 
            {
                if (this.GruposString != null && this.GruposString != "")
                    return this.GruposString.Split(';').ToList();
                else
                    return new List<string>();
            }
            set 
            {
                this.GruposString = string.Join(";", value.ToArray());
            }
        }

        /// <summary>
        /// Auxílio para serialização
        /// </summary>
        [XmlAttribute("Grupos")]
        public string GruposString { get; set; }

        /// <summary>
        /// Código dos perfis que irão ativar este comando.
        /// </summary>
        public List<string> Perfis { get; set; }

        /// <summary>
        /// Permissões que irão ativar este comando.
        /// Pode-se informar tanto o código quando o tipo.
        /// </summary>
        [XmlIgnore]
        public List<string> Permissoes
        {
            get
            {
                if (this.PermissoesString != null && this.PermissoesString != "")
                    return this.PermissoesString.Split(';').ToList();
                else
                    return new List<string>();
            }
            set
            {
                this.PermissoesString = string.Join(";", value.ToArray());
            }
        }

        /// <summary>
        /// Código das permissões que irão ativar este comando.
        /// Pode-se informar tanto o código quando o tipo.
        /// Como o tipo tem separação de tipo, assembly, a separação das
        /// várias permissões é feita com ';'
        /// </summary>
        [XmlAttribute("Permissoes")]
        public string PermissoesString { get; set; }

        /// <summary>
        /// Indica o tipo da ativação do comando.
        /// Por exemplo: se será ativo quando satisfizer todas as condições informadas, 
        /// ou se será ativo quando satisfizer qualquer uma das condições.
        /// </summary>
        [XmlAttribute]
        public ItemSegurancaAtivacaoTipoEnum TipoAtivacao { get; set; }

        /// <summary>
        /// Propriedade que pode conter qualquer string e será rebatida pelo sistema de segurança.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Indica se o item é válido ou não.
        /// Contém valor após a validação
        /// </summary>
        public bool? Valido { get; set; }

        /// <summary>
        /// Construtor default.
        /// </summary>
        public ItemSegurancaInfo()
        {
            this.Grupos = new List<string>();
            this.Perfis = new List<string>();
            this.Permissoes = new List<string>();
            this.TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.NaoValidar;
        }
    }
}
