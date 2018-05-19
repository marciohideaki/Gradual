<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DadosGrupoRestricaoSpider.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.DadosGrupoRestricaoSpider" %>

<form id="form1" runat="server">

    <h4>Dados do Grupo de Restrição</h4>

    <div id="divHidden" style="display:none">
        <table cellspacing="0" cellpadding="0" border="0" id="tblRisco_ConfigurarGridDeResultados_Matriz" role="grid" aria-multiselectable="false" aria-labelledby="gbox_tblRisco_ConfigurarGridDeResultados_GrupoAlavancagem" class="ui-jqgrid-btable" style="width: 558px;">
	        <tbody>
		        <tr class="ui-widget-content jqgrow ui-row-ltr" role="row" id="Avicultura" style="">
	                <td aria-describedby="tblRisco_ConfigurarGridDeResultados_NomeDoGrupo" title="Avicultura" style="text-align: left; width: 448px;" role="gridcell">Gradual</td>
	                <td aria-describedby="tblRisco_ConfigurarGridDeResultados_CodigoGrupo" title="53" style="text-align: left; display: none; width: 1px;" role="gridcell"></td>
	                <td aria-describedby="tblRisco_ConfigurarGridDeResultados_Excluir" title="" style="text-align: center; width: 100px;" role="gridcell">
		                <button onclick="return Risco_ExcluirGrupoDeRisco(this);" id="GrupoDeRisco_184" style="margin-left: 45%; margin-top: -15px;" class="IconButton Excluir"></button>
	                </td>
                </tr>
	        </tbody>
        </table>
    </div>

    <input type="hidden" id="hidDadosCompletos_Risco_Grupo" class="DadosJson" runat="server" value="" />
        
    <input type="hidden" id="txtGrupo_Id" Propriedade="Id" runat="server" />

    <input type="hidden" id="txtGrupo_TIpo" Propriedade="TipoDeObjeto" value="Grupo" />
    
    <p style="width:100px;float:left;margin-left:25px;">
        <label for="txtRisco_DadosCompletos_Grupo_Descricao" style="width:100%">Descrição Grupo:</label>
    </p>
    <p style="width: 25.5em;float:left;">
        <input type="text" id="txtRisco_DadosCompletos_Grupo_Descricao_Spider" Propriedade="Descricao" maxlength="100" class="validate[required,length[5,100]]" style="width:100%" />
    </p>
    <p style="width: 30px; float: left; margin-top: -0.5px;">
        <button onclick="return btnSalvar_Risco_DadosGruposRestricoesSpider_Click(this)">Gravar</button>
    </p>
    
    <div style="margin: 71px 100px 0pt 37px;">
        <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width:99%;">
            <thead>
                <div>
                    <table id="tblRisco_ConfigurarGridDeResultadosSpider" style="overflow:hidden;"></table>
                    <div id="pnlRisco_ConfigurarGridDeResultadosSpider_Pager" style="overflow:hidden;"></div>
                </div>
            </thead>
        </table>
    </div>

    <p class="BotoesSubmit">


    </p>
    <script>Risco_BuscarItensGruposDeRiscoRestricoesSpider();</script>
</form>
