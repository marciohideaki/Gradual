<%@ Page Language="C#" enableviewstate="false"  AutoEventWireup="true" CodeBehind="ResultadoUsuariosLogados.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoUsuariosLogados" %>

<form id="form1" runat="server">

        <%--<table cellspacing="0" style="width:98%;" >--%>
        <table cellspacing="0" style="width:98%;" class="pnlUsuariosLogados_Busca_Resultados GridIntranet GridIntranet_CheckSemFundo">
        <thead>
            <tr>
                <td style="text-align:center">Id</td>
                <td style="text-align:center">Id Login</td>
                <td style="text-align:center">Nome</td>
                <td style="text-align:center">Data Acesso</td>
                <td style="text-align:center">Data Saída</td>
                <td style="text-align:center">Sistema</td>
                <td style="text-align:center">Sessão</td>
                <td style="text-align:center">IP</td>
                <td style="text-align:center">Ativo</td>
            </tr>
        </thead>
    
        <tfoot>
            <tr>
                <td colspan="14" style="text-align:right;"><button class="" id="btnCancelarOrdensStop" onclick="return btnBusca_Monitoramento_UsuariosLogados_Click()">Atualizar</button></td>
            </tr>
        </tfoot>
    
        <tbody>
                        
            <asp:repeater id="rptUsuariosLogados" runat="server">
            
            <itemtemplate>

            <tr>
                <td><%# Eval("Id")%></td>
                <td><%# Eval("IdLogin")%></td>
                <td><%# Eval("dsUsuario")%></td>
                <td><%# Eval("DataLogIn")%></td>
                <td><%# Eval("DataLogOut")%></td>
                <td><%# Eval("Sistema")%></td>
                <td><%# Eval("CodigoSessao")%></td>
                <td><%# Eval("IpUsuario")%></td>
                <td><%# Eval("SessaoAtiva") %></td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum usu&aacute;rio encontrado.</td>
            </tr>
        </tbody>
    
    </table>

</form>
