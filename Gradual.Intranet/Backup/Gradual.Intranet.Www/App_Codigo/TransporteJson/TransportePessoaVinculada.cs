#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Views;
#endregion

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransportePessoaVinculada
    {
        #region Members
        /// <summary>
        /// código da pessoa vinculada
        /// </summary>
        public string CodigoPessoaVinculada { get; set; }

        /// <summary>
        /// Nome da pessoa vinculada
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Pessoa vinculada Ativada/Inativada
        /// </summary>
        public bool FlagAtivo { get; set; }

        /// <summary>
        /// Cpf da pessoa Vinculada
        /// </summary>
        public string CPFCNPJ { get; set; }

        /// <summary>
        /// Código da pessoa vinculada dependente
        /// </summary>
        public string CodigoPessoaVinculadaResponsavel { get; set; }

        /// <summary>
        /// Código do cliente
        /// </summary>
        public string CodigoCliente { get; set; }
        #endregion

        #region Constructor
        public TransportePessoaVinculada() { }

        public TransportePessoaVinculada(ClientePessoaVinculadaInfo pInfo)
        {
            this.CodigoPessoaVinculada            = pInfo.IdPessoaVinculada.ToString();
            this.CodigoPessoaVinculadaResponsavel = pInfo.IdPessoaVinculadaResponsavel.ToString();
            this.CPFCNPJ                          = pInfo.DsCpfCnpj.ToString();
            this.FlagAtivo                        = pInfo.StAtivo;
            this.Nome                             = pInfo.DsNome;
            this.CodigoCliente                    = pInfo.IdCliente.ToString();
        }
        #endregion
    }
}