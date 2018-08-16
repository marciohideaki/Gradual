<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SincronizarComSinacor.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.SincronizarComSinacor" %>

<form id="form1" runat="server">

    <p id="pnlClientes_SincronizarComSinacor_Requisicao"  class="TextoDeAtencao">

        <br /><br />

        Atenção! A sincronização com o Sinacor é um processo lento que pode demorar alguns minutos.

        <br /><br />

        Após fazer uma requisição, você irá receber mensagens de status conforme ela é processada
        e só poderá pedir outra sincronização quando a primeira terminar.        
    
        <br /><br />

        <span id="spnClientes_SincronizarComSinacor_PendenciaCadastral" runat="server" style="color:red"></span>
    
    </p>
    
    

    <br /><br />
    <p id="pnlClientes_SincronizarComSinacor_Resultado" style = "display:none;height:auto" >
         
    </p>

    <ul id="pnlClientes_SincronizarComSinacor_Mensagens" style="margin: 48px 24px; height:auto; display:none"></ul>
  

    <table id="tblClientes_SincronizarComSinacor_Resultado" style = "display:none;width:96%;margin:12px" class="GridIntranet" Cellspacing="0" CellPaging="0">
        <thead>
            <tr>
                <td>Tipo</td>
                <td>Mensagem</td>
            </tr>
        </thead>
        <tbody></tbody>
    </table>


    <p id="pnlClientes_SincronizarComSinacor_Submit"  runat="server" class="BotoesSubmit">
        
        <button id = "btnClientes_SincronizarComSinacor_Submit" onclick="return btnClientes_Acoes_SincronizarComSinacor_Click(this)">Sincronizar dados com o SINACOR</button>
        
    </p>

</form>
