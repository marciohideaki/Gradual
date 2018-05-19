<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParametrosGlobais.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.ParametrosGlobais" %>

<form id="form1" runat="server">
    <h4>Parametros Globais</h4>
    <div class="pnlFormulario_Campos">
        <ul class="pnlFormulario_Abas_Container" style="margin:-0.5em 0em 1em 13.5em">
            <li class="Selecionada"><a href="#" rel="pnlSeguranca_Parametros_Globais_HomeBroker" onclick="return pnlFormulario_Abas_li_a_Click(this)">HomeBroker</a></li>
            <li><a href="#" rel="pnlSeguranca_Parametros_Globais_GTI" onclick="return pnlFormulario_Abas_li_a_Click(this)">GTI</a></li>
            <li><a href="#" rel="pnlSeguranca_Parametros_Globais_Intranet" onclick="return pnlFormulario_Abas_li_a_Click(this)">Intranet</a></li>
        </ul>
    
        <div  id="pnlSeguranca_Parametros_Globais_HomeBroker" class="pnlFormulario_Campos" style="display:block;margin:-0.5em 0em 1em 13.5em">
            <p><input type="checkbox" id="Checkbox0" checked="checked" disabled/><label for="Checkbox0"> Expirar senha a cada 45 dias </label></p>
            <p><input type="checkbox" id="Checkbox1" checked="checked" disabled/><label for="Checkbox1"> Não reutilizar as 7 últimas senhas utilizadas </label></p>
            <p><input type="checkbox" id="Checkbox2" checked="checked" disabled/><label for="Checkbox2"> Bloquear depois de 3 Tentativas</label></p>
            <p><input type="checkbox" id="Checkbox9" checked="checked" disabled/><label for="Checkbox9"> Minimo 8 caracteres , devem conter um caractere maisculo , um caractere minisculo e um numero.</label></p>
        </div>
        <div  id="pnlSeguranca_Parametros_Globais_GTI" class="pnlFormulario_Campos" style="display:none;margin:-0.5em 0em 1em 13.5em">
    
            <p><input type="checkbox" id="Checkbox3"  checked="checked" disabled/> <label for="Checkbox3"> Expirar senha a cada 45 dias </label></p>
            <p><input type="checkbox" id="Checkbox4"  checked="checked" disabled/> <label for="Checkbox4"> Não reutilizar as 7 últimas senhas utilizadas </label></p>
            <p><input type="checkbox" id="Checkbox5"  checked="checked" disabled/> <label for="Checkbox5"> Bloquear depois de 3 Tentativas </label></p>
            <p><input type="checkbox" id="Checkbox10" checked="checked" disabled/><label for="Checkbox10"> Minimo 8 caracteres , devem conter um caractere maisculo , um caractere minisculo e um numero.</label></p>
        </div>
        <div  id="pnlSeguranca_Parametros_Globais_Intranet" class="pnlFormulario_Campos" style="display:none;margin:-0.5em 0em 1em 13.5em">
    
            <p><input type="checkbox" id="Checkbox6" checked="checked" disabled/> <label for="Checkbox6"> Expirar senha a cada 45 dias </label></p>
            <p><input type="checkbox" id="Checkbox7" checked="checked" disabled/> <label for="Checkbox7"> Não reutilizar as 7 últimas senhas utilizadas </label></p>
            <p><input type="checkbox" id="Checkbox8" checked="checked" disabled/> <label for="Checkbox8"> Bloquear depois de 3 Tentativas </label></p>
            <p><input type="checkbox" id="Checkbox11" checked="checked" disabled/><label for="Checkbox11"> Minimo 8 caracteres , devem conter um caractere maisculo , um caractere minisculo e um numero.</label></p>

        </div>

        <p class="BotoesSubmit">
            <button id="btnSeguranca_AlterarSenha_Salvar" onclick="return false">Salvar Parametros</button>
        </p>
        </div>
</form>
