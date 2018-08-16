<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkParaProspect.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.LinkParaProspect" %>

<form id="form1" runat="server">

    <h4>Link para cadastramento de Cliente para um determinado Assessor no Portal</h4>

    <div runat="server" id="divClientes_LinkProspect_Pesquisa">
        <p>
            <label for="txtCliente_LinkProspect_Nome">Nome</label>
            <input  id="txtCliente_LinkProspect_Nome" type="text" />
        </p>
        <p id="pnlClientes_LinkProspect_Submit"  runat="server" class="BotoesSubmit">
            <button id = "btnClientes_LinkProspect_Submit" onclick="return btnClientes_LinkProspect_Click(this)">Consultar</button>
        </p>
    </div>
    <asp:Repeater id="rptClientes_LinkProspect_Resultado" runat='server'>
        <ItemTemplate>
            <p style="margin-left:20px;font-size:1.1em;line-height:1.6em">
                <span style="float:left;width:6em">Assessor:</span> <%# Eval("Value") %> <br/>
                <span style="float:left;width:6em;margin-right:-4px">Link:</span> 
                <input type="text" readonly="readonly" style="border:0;background:transparent;width:90%" value="https://www.gradualinvestimentos.com.br/MinhaConta/Cadastro/CadastroPF.aspx?a=<%# Eval("Id") %>"></input>
             </p>
        </ItemTemplate>
    </asp:Repeater> 
      <div id="pnlClientes_LinkProspect_Resultado" style = "display:none;height:auto"></div>
</form>
