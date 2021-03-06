///////////////////////////////////////////////////////////
//  ListarParametrosRiscoClienteResponse.cs
//  Implementation of the Class ListarParametrosRiscoClienteResponse
//  Generated by Enterprise Architect
//  Created on:      26-jul-2010 17:43:16
//  Original author: amiguel
///////////////////////////////////////////////////////////




using Gradual.OMS.Risco.RegraLib.Dados;

using System.Collections.Generic;
using Gradual.OMS.Library;
namespace Gradual.OMS.Risco.RegraLib.Mensagens {
	public class ListarParametrosRiscoClienteResponse : MensagemResponseBase {

		public ListarParametrosRiscoClienteResponse(){

		}

		~ListarParametrosRiscoClienteResponse(){

		}

		public List<ParametroRiscoClienteInfo> ParametrosRiscoCliente{
			get;
			set;
		}

	}//end ListarParametrosRiscoClienteResponse

}//end namespace Mensagens