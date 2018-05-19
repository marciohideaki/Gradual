using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public abstract class TransporteRepresentantesBase
    {
        /// <summary>
        /// Código do Endereço
        /// </summary>
        public Nullable<int> Id { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>        
        public Int32 ParentId { get; set; }
        /// <summary>
        /// Nome do procurador representante
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Cpf do procurador ou representante
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Identidade do representante/procurador
        /// </summary>
        public string Identidade { get; set; }

        /// <summary>
        /// Orgão emissor
        /// </summary>
        public string OrgaoEmissor { get; set; }

        /// <summary>
        /// Código da uf do orgão Emissor
        /// </summary>
        public string UfOrgaoEmissor { get; set; }

        /// <summary>
        /// Exclusão 
        /// </summary>
        public bool Exclusao { get; set; }
    }
}