<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Autorizacoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.Autorizacoes" %>

<form id="form1" runat="server">

    <p style="padding:1em; text-align:center">
        <button id="btnAutorizarCadastros" onclick="return btnAutorizarCadastros_Click(this)" disabled="disabled" style="font-size:1.2em; ">Autorizar</button>
    </p>

    <table id="tblAutorizacoes_ListaDeAutorizacoesPendentes" class="GridIntranet GridIntranetAlternado" cellspacing="0" style="float: left; width:100%;">

        <thead>
            <tr>
                <td>Código</td>
                <td>Nome</td>
                <td>CPF</td>
                <td style="white-space: nowrap">Dt. Export.</td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_11"> <label for="chkAut_Todos_11">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Dir. 1</span></td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_12"> <label for="chkAut_Todos_12">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Dir. 2</span></td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_21"> <label for="chkAut_Todos_21">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Proc. 1</span></td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_22"> <label for="chkAut_Todos_22">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Proc. 2</span></td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_31"> <label for="chkAut_Todos_31">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Test. 1</span></td>
                <td style="white-space: nowrap"><input type="checkbox" onclick="return chkAut_Todos_Click(this)" id="chkAut_Todos_32"> <label for="chkAut_Todos_32">&nbsp;</label><span style="padding:5px 0px 0px 6px;display:inline-block">Aut Test. 2</span></td>
                <td>Detalhes</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="11">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDeAutorizacoesPendentes" runat="server">
            <itemtemplate>

            <tr data-IdCliente="<%# Eval("IdCliente") %>">
                <td><%# Eval("CodigoBov") %></td>
                <td><%# Eval("NomeCliente") %></td>
                <td><%# Eval("CPF") %></td>
                <td><%# Eval("DataExportacao") %></td>

                <td title="<%# Eval("TituloCelula_D1") %>"><%# Eval("Botao_D1") %></td>
                <td title="<%# Eval("TituloCelula_D2") %>"><%# Eval("Botao_D2") %></td>

                <td title="<%# Eval("TituloCelula_P1") %>"><%# Eval("Botao_P1") %></td>
                <td title="<%# Eval("TituloCelula_P2") %>"><%# Eval("Botao_P2") %></td>

                <td title="<%# Eval("TituloCelula_T1") %>"><%# Eval("Botao_T1") %></td>
                <td title="<%# Eval("TituloCelula_T2") %>"><%# Eval("Botao_T2") %></td>

                <td> <button class="IconButton IconePesquisar" title="Autorizar" onclick="return btnSistema_Autorizacoes_Detalhes_Click(this)" style="margin:2px 0px 2px 12px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="11">Nenhuma autorização pendente encontrada.</td>
            </tr>
        </tbody>

    </table>

</form>
