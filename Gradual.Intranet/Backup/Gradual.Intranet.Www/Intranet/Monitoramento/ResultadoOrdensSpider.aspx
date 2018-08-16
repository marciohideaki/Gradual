<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoOrdensSpider.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoOrdensSpider" %>

<form id="form1" runat="server">

    <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width:99%;">
        <thead>
            <div>
                <table id="tblBusca_OrdensSpider_Resultados"></table>
                <div id="pnlBusca_OrdensSpider_Resultados_Pager"></div>
            </div>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="14" style="text-align:right; padding-top:1em;">
                    <button class="btnBusca" id="btnCancelarOrdensSpider" onclick="return btnMonitoramento_ExcluirOrdensSelecionadasSpider_Click(this)" runat="server">Cancelar Selecionados</button>
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
