<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="ReservarIPO.aspx.cs" Inherits="Gradual.Intranet.Clientes.Formularios.ReservarIPO" %>

    <form id="form1" runat="server">

        <table cellspacing="0" class="GridIntranet GridIntranet_CheckSemFundo" style="width:99%;">
    
            <thead>

                <div>
                    <table id="tblBusca_ReservaIPO_Resultados"></table>
                    <div id="pnlBusca_ReservaIPO_Resultados_Pager"></div>
                </div>

            </thead>
            <tbody>
                <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                    <td colspan="14">Nenhum cliente encontrado.</td>
                </tr>
            </tbody>
        </table>
    </form>
