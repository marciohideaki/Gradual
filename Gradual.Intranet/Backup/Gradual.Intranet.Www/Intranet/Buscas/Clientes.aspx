<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Clientes" %>


<form id="form1" runat="server">

<div id="pnlBusca_Clientes_Form" class="Busca_Formulario">

    <p>
        <label for="cboBusca_Clientes_BuscarPor">Buscar Por:</label>
        <select id="cboBusca_Clientes_BuscarPor" style="width:9.5em">
            <option value="CodBovespa" selected ="selected">Código Bovespa</option>
            <option value="CodBovespa" >Conta Investimento</option>
            <option value="CpfCnpj">CPF/CNPJ</option>
            <option value="Email">Email</option>
            <option value="NomeCliente">Nome do Cliente</option>
        </select>
    </p>

    <p style="width:auto;margin:-2.3em 0 0 18em">

        <label for="txtBusca_Clientes_Termo" style="display:none">Termo de busca:</label>
        <input type="text" id="txtBusca_Clientes_Termo" maxlength="30" style="width:13.6em;margin-right:0.4em" />

        <button class="btnBusca" onclick="return btnBusca_Click()">Buscar</button>
    </p>

    <p>
        <label for="cboBusca_Clientes_Tipo">Tipo:</label>
        <select id="cboBusca_Clientes_Tipo" style="width:9.5em">
            <option value="">Todos</option>
            <option value="PF">Pessoa Física</option>
            <option value="PJ">Pessoa Jurídica</option>
        </select>
    </p>

    <p>
       <label>Login:</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Status_Ativo" checked="checked" />
       <label for="chkBusca_Clientes_Status_Ativo" style="margin-right:2.8em">Ativo</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Status_Inativo" checked="checked" />
       <label for="chkBusca_Clientes_Status_Inativo">Inativo</label> 
    </p>
    
    <p>
       <label>Passo:</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Passo_Visitante" checked="checked" />
       <label for="chkBusca_Clientes_Passo_Visitante">Visitante</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Passo_Cadastrado" checked="checked" />
       <label for="chkBusca_Clientes_Passo_Cadastrado" style="margin-right:0.2em">Cadastrado</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Passo_ExportadoSinacor" checked="checked" />
       <label for="chkBusca_Clientes_Passo_ExportadoSinacor">Exportado p/ Sinacor</label> 
    </p>
    <p>
       <label>Pendências:</label>  
       
       <input type="checkbox" id="chkBusca_Clientes_Pendencia_ComPendenciaCadastral" />
       <label for="chkBusca_Clientes_Pendencia_ComPendenciaCadastral" style="margin-right:1.4em">Com pendência cadastral</label> 
       
       <input type="checkbox" id="chkBusca_Clientes_Pendencia_ComSolicitacaoAlteracao" />
       <label for="chkBusca_Clientes_Pendencia_ComSolicitacaoAlteracao">Com solicitação de alteração</label>
    </p>

</div>

<div id="pnlBusca_Clientes_Resultados" class="Busca_Resultados">

    <table id="tblBusca_Clientes_Resultados"></table>
    <div id="pnlBusca_Clientes_Resultados_Pager"></div>

    <button class="ExpandirPraBaixo" title="Clique para expandir o painel e ver mais resultados" onclick="return btnResultadoBusca_ExpandirPainel_Click(this, 'tblBusca_Clientes_Resultados')" style="">
        <span>Expandir tabela</span>
    </button>

</div>

</form>

