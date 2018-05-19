<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoOrdensNovoOMS.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoOrdensNovoOMS" %>

<form id="form1" runat="server">

    <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width:99%;">
        <thead>
            <div>          
                <table id="tblBusca_OrdensNovoOMS_Resultados"></table>
                <div id="pnlBusca_OrdensNovoOMS_Resultados_Pager"></div>
            </div>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="14" style="text-align:right; padding-top:1em;">
                    <button class="btnBusca" id="btnCancelarOrdensNovoOMS" onclick="return btnMonitoramento_ExcluirOrdensSelecionadas_Click(this)" runat="server">Cancelar Selecionados</button>
                </td>
            </tr>
        </tfoot>
    
        <tbody>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhuma ordem encontrada.</td>
            </tr>
        </tbody>
    </table>
</form>
