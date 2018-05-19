<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Restricoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.DefaultRestricoes" %>

<ul id="lstItemMenu_Risco_Restricoes" class="MenuDoObjeto" style="display:none">
    <li class="Topo TopoFundo"></li>
    <li class="SubTitulo ExibirEmNovo">Dados</li>

    <asp:Repeater runat="server" id="rpt_Menu_RiscoDados_Restricoes">
        <ItemTemplate>
            <li class='<%# Eval("Tag").ToString().Split(';')[1] %>'>
              <a href='<%# Eval("Tag").ToString().Split(';')[0] %>' onclick="return lstMenuCliente_A_OnClick(this)"><%# Eval("Nome") %></a>
            </li>
        </ItemTemplate>
    </asp:Repeater>

    <li class="Fundo TopoFundo"></li>
</ul>
<div class="pnlFormulario" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>
    <div id="pnlRisco_Formularios_Dados_DadosGrupoRestricao"></div>
    <div id="pnlRisco_Formularios_Dados_AtivosGruposRestricoes"></div>
    <div id="pnlRisco_Formularios_Dados_ManutencaoRestricoes"></div>
    <div id="pnlRisco_Formularios_Dados_TravaExposicao"></div>
</div>