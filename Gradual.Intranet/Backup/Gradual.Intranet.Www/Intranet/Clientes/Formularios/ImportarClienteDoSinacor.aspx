<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportarClienteDoSinacor.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.ImportarClienteDoSinacor" %>

<form id="form1" runat="server">

    <h4>Importação de Cliente do Sinacor</h4>

    <p>
        <label for="txtClientes_Importar_CPF">CPF/CNPJ:</label>
        <input type="text" id="txtClientes_Importar_CPF" Propriedade="CPF_CNPJ" maxlength="15" class="validate[funcCall[validatecpfcnpj]] ProibirLetras" />
    </p>

     <p>
        <label for="txtClientes_Importar_DataNascimento">Data de Nascimento:</label>
        <input type="text" id="txtClientes_Importar_DataNascimento" Propriedade="DataNascimento" maxlength="10" class="validate[custom[data]] Mascara_Data" />
    </p>


    <p>
        <label for="txtClientes_Importar_CondicaoDependente">Condição de Dependente:</label>
        <input type="text" id="txtClientes_Importar_CondicaoDependente" Propriedade="CondicaoDependente" maxlength="2" value="1" disabled="disabled"  />
    </p>


   <p id="pnlClientes_Importar_Submit"  runat="server" class="BotoesSubmit">
        
        <button id = "btnClientes_Importar_Submit" onclick="return btnClientes_Importar_Click(this)">Importar Cliente do SINACOR</button>
        
   </p>
 
</form>

