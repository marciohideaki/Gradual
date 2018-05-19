<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PessoasExpostasPoliticamente.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.PessoasExpostasPoliticamente" %>

<form id="form1" runat="server" enctype="multipart/form-data">

    <p style="width:90%; padding:1em 0em 1em 0em">
        <label for="txtPessoaExpostasPoliticamente_Buscar" style="margin-top:6px;width:80px">Buscar PEP:</label>
        <input  id="txtPessoaExpostasPoliticamente_Buscar" type="text" />

        <button onclick="return btnSistema_PEP_btnBuscar_Click(this)" style="margin-left:1em">Buscar</button>
    </p>
    
    <p id="pnlPessoaExpostasPoliticamente_Resultado" style="display:none">

        <label class="Resultado_Nome"></label>
        <label class="Resultado_Documento"></label>

        <a href="#" onclick="return btnSistema_PEP_lnkEnviarEmail_Click(this)">Enviar email de alerta para Compliance</a>

    </p>

    <div style="float:left;">
        <!-- o Grid está funcionando, foi "retirado" por pedido para que não fosse mostrado todos  -->

        <table id="tblPessoaExpostasPoliticamente_ListaDePessoas" cellspacing="0"></table>
        <div id="pnlPessoaExpostasPoliticamente_ListaDePessoas_Pager"></div>

    </div>

<%--    <p style="margin-top:1em">
        <label for="txtPessoaExpostasPoliticamente_Upload" style="width:14em">Importar novo arquivo de PEP:</label>
        <input  id="txtPessoaExpostasPoliticamente_Upload" type="file" runat="server" />

        <button onclick="return btnSistema_PEP_EnviarArquivoParaImportacao_Click(this)" style="margin-left:1em">Enviar Arquivo</button>
    </p>--%>

</form>