using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    /// <summary>
    /// Define uma regra de risco, vinculada a um agrupamento 
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RegraRiscoInfo : RegraInfo, ICodigoEntidade
    {
        /// <summary>
        /// Código da regra de risco
        /// </summary>
        public string CodigoRegraRisco { get; set; }

        /// <summary>
        /// Indica a data de criação da regra do risco
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Indica o agrupamento para o qual esta regra deve ser
        /// ativada.
        /// </summary>
        public RiscoGrupoInfo Agrupamento { get; set; }

        /// <summary>
        /// Indica uma data limite para validade desta regra.
        /// A partir de então, a regra passa a não validar qualquer tentativa de alocação.
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegraRiscoInfo()
        {
            this.CodigoRegraRisco = Guid.NewGuid().ToString();
            this.Agrupamento = new RiscoGrupoInfo();
            this.DataCriacao = DateTime.Now;
        }
        
        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoRegraRisco;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Recebe o tipo informado na classe e pega o atributo da regra
        /// </summary>
        /// <returns></returns>
        private RegraAttribute receberRegraAttribute()
        {
            // Tem o tipo?
            if (this.TipoRegra != null)
            {
                // Pega os atributos de regra
                object[] attrs =
                    this.TipoRegra.GetCustomAttributes(
                        typeof(RegraAttribute), true);

                // Se achou, retorna 
                if (attrs.Length > 0)
                    return (RegraAttribute)attrs[0];
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        #endregion

        public override string ToString()
        {
            return this.NomeRegra;
        }
    }
}
