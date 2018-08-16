<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contratos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Contratos" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Cadastro - Contratos</title>
</head>
<body>
    <form id="form1" runat="server">
    <h4>Vinculação de Contratos</h4>

    <h5 style="margin-bottom:2em">Relação Contratos/Clientes</h5>

    <div id="pnlFormulario_Campos_Contratos" class="pnlFormulario_Contratos" style="display:block; height:auto  ">

        <input type="hidden" id="txtClientes_Contratos_ParentId" Propriedade="ParentId" runat="server" />
       
        <asp:Repeater id="rptClientes_Contratos" runat="server" >
        <ItemTemplate>
        <p class="CheckBoxAEsquerda">
            <input type="checkbox" id='chkClientes_Contratos_<%# Eval("IdContrato") %>' name='chkClientes_Contratos_<%# Eval("IdContrato") %>' value='<%# Eval("IdContrato") %>'  <%# Eval("Checked")%> />
            <label for='chkClientes_Contratos_<%# Eval("IdContrato") %>' style="margin-right:0.1em; text-align:left"><%# Eval("DsContrato") %></label> 
        </p>
        </ItemTemplate>
        </asp:Repeater>  

    </div>
    
    <p class="BotoesSubmit" style="margin-top:2em">
            <button id="btnSalvar" runat="server" onclick="return btnClientes_Contratos_Salvar_Click(this)">Salvar Alterações</button>
        </p>
    </form>
</body>
</html>

