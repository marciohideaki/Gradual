///////////////////////////////////////////////////////////
//  ListarParametrosRiscoResponse.cs
//  Implementation of the Class ListarParametrosRiscoResponse
//  Generated by Enterprise Architect
//  Created on:      26-jul-2010 17:43:16
//  Original author: amiguel
///////////////////////////////////////////////////////////




using System;
using Gradual.OMS.Risco.RegraLib.Dados;

using System.Collections.Generic;
using Gradual.OMS.Library;

namespace Gradual.OMS.Risco.RegraLib.Mensagens
{
	public class ListarParametrosRiscoResponse : MensagemResponseBase 
    {

		public ListarParametrosRiscoResponse()
        {

		}

		~ListarParametrosRiscoResponse()
        {

		}

		public List<ParametroRiscoInfo> ParametrosRisco
        {
			get;
			set;
		}

	}//end ListarParametrosRiscoResponse

}//end namespace Mensagens