///////////////////////////////////////////////////////////
//  IServicoRegrasRisco.cs
//  Implementation of the Interface IServicoRegrasRisco
//  Generated by Enterprise Architect
//  Created on:      26-jul-2010 17:43:16
//  Original author: amiguel
///////////////////////////////////////////////////////////




using Gradual.OMS.Risco.RegraLib.Mensagens;



namespace Gradual.OMS.Risco.RegraLib 
{
	public interface IServicoRegrasRisco 
    {

		/// 
		/// <param name="lRequest"></param>
		SalvarParametroRiscoClienteResponse SalvarParametroRiscoCliente(SalvarParametroRiscoClienteRequest lRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        SalvarGrupoResponse SalvarGrupo(SalvarGrupoRequest lRequest);

		/// 
		/// <param name="lRequest"></param>
		ListarParametrosRiscoResponse ListarParametrosRisco(ListarParametrosRiscoRequest lRequest);

		/// 
		/// <param name="lRequest"></param>
		ListarParametrosRiscoClienteResponse ListarParametrosRiscoCliente(ListarParametrosRiscoClienteRequest lRequest);

		/// 
		/// <param name="lRequest"></param>
		ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente(ReceberParametroRiscoClienteRequest lRequest);

		/// 
		/// <param name="lRequest"></param>
        ListarPermissoesRiscoResponse ListarPermissoesRisco(ListarPermissoesRiscoRequest lRequest);

		/// 
		/// <param name="lRequest"></param>
		ListarGruposResponse ListarGrupos(ListarGruposRequest lRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        ReceberParametroRiscoResponse ReceberParametroRisco(ReceberParametroRiscoRequest lRequest);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        ReceberPermissaoRiscoResponse ReceberPermissaoRisco(ReceberPermissaoRiscoRequest lRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        ReceberGrupoResponse ReceberGrupo(ReceberGrupoRequest lRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        SalvarGrupoItemResponse SalvarGrupoItem(SalvarGrupoItemRequest lRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lRequest"></param>
        /// <returns></returns>
        RemoverGrupoItemResponse RemoverGrupoItem(RemoverGrupoItemRequest lRequest);

	}//end IServicoRegrasRisco

}//end namespace Risco