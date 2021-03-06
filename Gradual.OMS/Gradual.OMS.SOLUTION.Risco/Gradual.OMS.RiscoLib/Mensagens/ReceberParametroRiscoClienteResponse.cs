///////////////////////////////////////////////////////////
//  ReceberParametroRiscoClienteResponse.cs
//  Implementation of the Class ReceberParametroRiscoClienteResponse
//  Generated by Enterprise Architect
//  Created on:      26-jul-2010 17:43:27
//  Original author: amiguel
///////////////////////////////////////////////////////////




using Gradual.OMS.Risco.RegraLib.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Risco.RegraLib.Mensagens {
	public class ReceberParametroRiscoClienteResponse : MensagemResponseBase {

		public ReceberParametroRiscoClienteResponse(){

		}

		~ReceberParametroRiscoClienteResponse(){

		}

		public ParametroRiscoClienteInfo ParametroRiscoCliente{
			get;
			set;
		}

	}//end ReceberParametroRiscoClienteResponse

}//end namespace Mensagens