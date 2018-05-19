using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de configurações do serviço de Persistencia Binaria
    /// </summary>
    public class PersistenciaArquivoConfig
    {
        /// <summary>
        /// Indica o caminho do arquivo de persistencia
        /// </summary>
        public string ArquivoPersistencia { get; set; }

        /// <summary>
        /// Indica se a persistencia deve ser salva automaticamente
        /// de tempos em tempos.
        /// </summary>
        public bool SalvarAutomaticamente { get; set; }

        /// <summary>
        /// Indica o tempo, em segundos, que irá ocorrer o salvamento
        /// automático.
        /// </summary>
        public int TempoSalvamentoAutomatico { get; set; }

        /// <summary>
        /// Tipo dos objetos a serem passados para essa persistencia
        /// </summary>
        [XmlIgnore]
        public Type[] HooksTipos
        {
            get
            {
                List<Type> retorno = new List<Type>();
                if (this.Hooks != null)
                    foreach (string tipoObjeto in this.Hooks)
                        retorno.Add(Type.GetType(tipoObjeto));
                return retorno.ToArray();
            }
            set
            {
                List<string> tiposString = new List<string>();
                if (value != null)
                    foreach (Type tipo in value)
                        tiposString.Add(tipo.FullName + ", " + tipo.Assembly.FullName);
                this.Hooks = tiposString;
            }
        }

        /// <summary>
        /// Propriedade auxiliar para serialização
        /// </summary>
        public List<string> Hooks { get; set; }

        /// <summary>
        /// Construtor default.
        /// </summary>
        public PersistenciaArquivoConfig()
        {
            this.SalvarAutomaticamente = false;
            this.TempoSalvamentoAutomatico = 60;
            this.Hooks = new List<string>();
        }
    }
}
