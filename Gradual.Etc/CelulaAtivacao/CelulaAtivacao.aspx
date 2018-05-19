<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CelulaAtivacao.aspx.cs" Inherits="CelulaAtivacao._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gradual Investimentos << Celula de Ativação >></title>
    <style type="text/css">
        .style1
        {
            width: 48%;
            height: 88px;
        }
        .style2
        {
            width: 103px;
        }
        .style3
        {
            width: 21%;
        }
        .style4
        {
            height: 20px;
        }
        .style5
        {
            width: 100%;
        }
        .style6
        {
            width: 103px;
            height: 10px;
        }
        .style7
        {
            height: 10px;
        }
    </style>
</head>
<body>
<script language="javascript" type="text/jscript">
function FormataData(campo, evento) {
    var tecla = window.event ? evento.keyCode : evento.which;

    //alert("evento.keyCode: " + evento.keyCode + "\nevento.which: " + evento.which);
    if (tecla != 8 && tecla != 0) {
        if (tecla < 48 || tecla > 57) {
            evento.preventDefault ? evento.preventDefault() : evento.returnValue = false;
            evento.keyCode = 0;
            return false;
        }
        else {
            if (campo.value.length == 2 || campo.value.length == 5) {
                campo.value += "/";
            }
        }
    }

    return true;
}
</script>
    <form id="form1" runat="server">
    <div>
    
    &nbsp;<table align="center" class="style3">
            <tr>
                <td align="center" class="style4">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Verdana" 
                        Font-Size="Small" Text="Consulta de clientes:"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
        <table align="center" class="style1">
            <tr>
                <td class="style6">
                    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="XX-Small" 
                        Text="Status Cadastro"></asp:Label>
                </td>
                <td class="style7">
    
        <asp:DropDownList ID="DDLMomento" runat="server" AutoPostBack="True" 
            Font-Names="Verdana" Font-Size="XX-Small" 
            onselectedindexchanged="DDLMomento_SelectedIndexChanged" Height="26px" Width="127px">
            <asp:ListItem Value="M1">M1</asp:ListItem>
            <asp:ListItem Value="M2">M2</asp:ListItem>
            <asp:ListItem Value="M3">M3</asp:ListItem>
        </asp:DropDownList>
    
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="XX-Small" 
                        Text="DataInicial:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDataInicial" runat="server" Font-Names="Verdana" 
                        Font-Size="XX-Small" MaxLength="10" Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Font-Size="XX-Small" 
                        Text="DataFinal:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDataFinal" runat="server" Font-Names="Verdana" 
                        Font-Size="XX-Small" MaxLength="10" Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Font-Size="XX-Small" 
                        Text="Código Bovespa:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoBovespa" runat="server" Enabled="False" 
                        Font-Names="Verdana" Font-Size="XX-Small" MaxLength="10" Width="120px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="XX-Small" 
                        Text="Código Assessor:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DdlAssessor" runat="server" Font-Names="Verdana" 
                        Font-Size="XX-Small" Width="450px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td align="right">
                    <asp:LinkButton ID="lnkConsultar" runat="server" Font-Bold="True" 
                        Font-Names="Verdana" Font-Size="XX-Small" ForeColor="#990000" 
                        onclick="lnkConsultar_Click">Consultar</asp:LinkButton>
&nbsp;
                    <asp:LinkButton ID="lnkExcel" runat="server" Enabled="False" Font-Bold="True" 
                        Font-Names="Verdana" Font-Size="XX-Small" ForeColor="#990000" 
                        onclick="lnkExcel_Click">GerarExcel</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
    
    </div>
    <table class="style5">
        <tr>
            <td align="center" height="100%" width="100%">
    <asp:GridView  runat="server" ID="GvDados" AllowSorting="True" AllowPaging="True" BackColor="White" 
        BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4"       
       PageSize="50" onpageindexchanging="GvDados_PageIndexChanging" Height="100%" 
                    Width="100%" onsorted="GvDados_Sorted" onsorting="GvDados_Sorting">
        <FooterStyle BackColor="#FFFFCC" Font-Names="Verdana" Font-Size="XX-Small" 
            ForeColor="#330099" />
        <RowStyle BackColor="White" Font-Names="Verdana" Font-Size="XX-Small" 
            ForeColor="#330099" />
        <PagerStyle BackColor="White" ForeColor="#990000" HorizontalAlign="Center" 
            Font-Names="Verdana" Font-Size="XX-Small" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" Font-Names="Verdana" 
            Font-Size="XX-Small" ForeColor="#FFFFCC" />
    </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
    <br />
    </form>
</body>
</html>
