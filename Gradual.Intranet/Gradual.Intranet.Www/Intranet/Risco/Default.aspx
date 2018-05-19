<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Default" %>


<ul id="lstItensSelecionados_Risco" class="ListaDeObjetos">
    
    <li class="NovoItem" style="display:none">
        <h3>
            <span>Novo</span>

            <button onclick="return lstItensSelecionados_btnCancelarNovo_OnClick(this, 'risco')"  title="Cancelar" class="IconButton Cancelar"><span>Cancelar</span></button>
        </h3>
    </li>  

    <li class="Template Template_Risco_Perfil ItemDeUmaLinha" style="display:none">
      <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>

        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)" class="Risco_P">
            <span Propriedade="Descricao">Descrição do Perfil</span>
        </h3>
    </li>
</ul>

<ul id="lstItemMenu_Risco" class="MenuDoObjeto" style="display:none">
    <li class="Topo TopoFundo"></li>
    <li class="SubTitulo ExibirEmNovo">Dados</li>

    <asp:Repeater runat="server" id="rpt_Menu_RiscoDados">
        <ItemTemplate>
            <li class='<%# Eval("Tag").ToString().Split(';')[1] %>'>
              <a href='<%# Eval("Tag").ToString().Split(';')[0] %>' onclick="return lstMenuCliente_A_OnClick(this)"><%# Eval("Nome") %></a>
            </li>
        </ItemTemplate>
    </asp:Repeater>

    <li class="Fundo TopoFundo"></li>
</ul>

<%--<ul id="lstItemMenu_Risco_Restricoes" class="MenuDoObjeto" style="display:none">
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
</ul>--%>

<div class="pnlFormulario" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>
    <%--<div id="pnlRisco_Formularios_Dados_DadosGrupoRestricao"></div>
    <div id="pnlRisco_Formularios_Dados_AtivosGruposRestricoes"></div>
    <div id="pnlRisco_Formularios_Dados_ManutencaoRestricoes"></div>
    <div id="pnlRisco_Formularios_Dados_TravaExposicao"></div>--%>
    <div id="pnlRisco_Formularios_Dados_DadosCompletos"></div>
    <div id="pnlRisco_Formularios_Dados_Itens"></div>
    <div id="pnlRisco_Formularios_Dados_GrupoAlavancagem"></div>
    <div id="pnlRisco_Formularios_Dados_ParametroAlavancagem"></div>
    <div id="pnlRisco_Formularios_Dados_ClienteGrupo"></div>
    <div id="pnlRisco_Formularios_Dados_ManutencaoCliente"></div>

</div>
<div id="pnlRisco_Formularios_Dados"></div>