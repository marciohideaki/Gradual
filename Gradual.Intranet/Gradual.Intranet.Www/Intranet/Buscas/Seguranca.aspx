<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Seguranca.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Seguranca" %>

<form id="form1" runat="server">

<div id="pnlBusca_Seguranca_Form" class="Busca_Formulario">

    <p>
        <label for="cboBusca_Seguranca_BuscarPor">Buscar Por:</label>
        <select id="cboBusca_Seguranca_BuscarPor" style="width:8.3em" Propriedade="BuscarPor">
            <option value="Usuario">Usuarios</option>
            <option value="Perfil">Perfis</option>
            <option value="Permissao">Permissão</option>
        </select>
    </p>
    
    <p style="width:200px;margin:-2.3em 0 0 17em">
        <label for="cboBusca_Seguranca_BuscarCampo">Campo:</label>
        <select id="cboBusca_Seguranca_BuscarCampo" style="width:8.3em" Propriedade="BuscarCampo">
            <option value="Codigo">Código</option>
            <option value="Descricao" selected>Descrição</option>
        </select>
    </p>

    <p>
        <label for="txtBusca_Seguranca_Termo">Valor:</label>
        <input type="text" id="txtBusca_Seguranca_Termo" Propriedade="TermoDeBusca" maxlength="256" style="width: 22.2em; margin-right: 8px;" />

        <button class="btnBusca" onclick="return btnBusca_Click()" style="margin-top:2px">Buscar</button>
    </p>

    <p>
       <label>Status:</label> 
       
       <input type="checkbox" id="chkBusca_Seguranca_Status_Ativo" checked="checked" Propriedade="Ativo" />
       <label for="chkBusca_Seguranca_Status_Ativo" style="margin-right:2.8em">Ativo</label> 
       
       <input type="checkbox" id="chkBusca_Seguranca_Status_Inativo" checked="checked" Propriedade="Inativo" />
       <label for="chkBusca_Seguranca_Status_Inativo">Inativo</label> 
    </p>

</div>

<div id="pnlBusca_Seguranca_Resultados" class="Busca_Resultados">

    <table id="tblBusca_Seguranca_Resultados"></table>
    
    <div id="pnlBusca_Seguranca_Resultados_Pager"></div>

    <button class="ExpandirPraBaixo" title="Clique para expandir o painel e ver mais resultados" onclick="return btnResultadoBusca_ExpandirPainel_Click(this, 'tblBusca_Seguranca_Resultados')" style="">
        <span>Expandir tabela</span>
    </button>

</div>

</form>