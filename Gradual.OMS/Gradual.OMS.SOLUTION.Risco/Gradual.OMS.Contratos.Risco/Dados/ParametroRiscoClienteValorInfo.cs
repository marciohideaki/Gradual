///////////////////////////////////////////////////////////
//  ParametroRiscoClienteValorInfo.cs
//  Implementation of the Class ParametroRiscoClienteValorInfo
//  Generated by Enterprise Architect
//  Created on:      26-jul-2010 17:43:26
//  Original author: amiguel
///////////////////////////////////////////////////////////




using Gradual.OMS.Contratos.Risco.Dados;
using System;
namespace Gradual.OMS.Contratos.Risco.Dados {
	public class ParametroRiscoClienteValorInfo {

		public ParametroRiscoClienteInfo m_ParametroRiscoClienteInfo;

		public ParametroRiscoClienteValorInfo(){

		}

		~ParametroRiscoClienteValorInfo(){

		}

		/// <summary>
		/// Codigo do valor do parametro do cliente.
		/// </summary>
		public int CodigoParametroClienteValor{
			get;
			set;
		}

		/// <summary>
		/// Psrsmetro do cliente.
		/// </summary>
		public ParametroRiscoClienteInfo ParametroCliente{
			get;
			set;
		}

		/// <summary>
		/// Valor alocado do cliente para um determinado parametro.
		/// </summary>
		public decimal ValorAlocado{
			get;
			set;
		}

		/// <summary>
		/// Valor disponivel para o cliente.
		/// </summary>
		public decimal ValorDisponivel{
			get;
			set;
		}

		/// <summary>
		/// Descrição do histórico das alterações.
		/// </summary>
        public string Descricao
        {
            get;
            set;
        }

        /// <summary>
        /// Data da movimentação
        /// </summary>
        public DateTime DataMovimento 
        { 
            get; 
            set; 
        }

	}//end ParametroRiscoClienteValorInfo

}//end namespace Dados