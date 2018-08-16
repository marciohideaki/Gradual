<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Default" %>


<ul id="lstItensSelecionados_Seguranca" class="ListaDeObjetos">
    
    <li class="NovoItem" style="display:none">
        <h3>
            <span>Novo</span>
            
            <button onclick="return lstItensSelecionados_btnCancelarNovo_OnClick(this, 'seguranca')"  title="Cancelar" class="IconButton Cancelar"><span>Cancelar</span></button>
        </h3>
    </li>

    <li class="Template Template_Seguranca_Usuario" style="display:none">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>
        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)">
            <span Propriedade="Nome">Nome do Usuário</span>
        </h3>
        
        <ul class="Detalhes" style="margin-left:56px">
            <li><label>Email:</label></li>
            <li><div Propriedade="Email"></div></li>
        </ul>

    </li>
    
    <li class="Template Template_Seguranca_Grupo ItemDeUmaLinha" style="display:none">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>
        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)">
            <span Propriedade="Nome">Nome do Grupo</span>
        </h3>
    </li>
    
    
    <li class="Template Template_Seguranca_Perfil ItemDeUmaLinha" style="display:none">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>
        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)">
            <span Propriedade="Nome">Nome do Perfil</span>
        </h3>
    </li>

    <li class="Template Template_Seguranca_PermissaoSeguranca" style="display:none">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>
        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)">
            <span Propriedade="Nome">Nome da Permissão</span>
        </h3>
        <ul class="Detalhes" style="margin-left:56px">
            <li><label>Descrição:</label></li>
            <li><div Propriedade="Descricao"></div></li>
        </ul>
    </li>
</ul>

<ul id="lstItemMenu_Seguranca" class="MenuDoObjeto" style="display:none">
<li class="Topo TopoFundo"></li>

    <li class="SubTitulo ExibirEmNovo">Dados</li>

    <li class="Tipo_U Tipo_G Tipo_P Tipo_PS ExibirEmNovo">
        <a href="Formularios/Dados/DadosCompletos.aspx" id="lnkItemMenu_Seguranca_DadosCompletos_Usuario" onclick="return lstMenuCliente_A_OnClick(this)">Dados Completos</a>
    </li>
    <li class="Tipo_G Tipo_P">
        <a href="Formularios/Dados/Usuarios.aspx"       onclick="return lstMenuCliente_A_OnClick(this)">Usuários</a>
    </li>
    <li class="Tipo_U Tipo_G Tipo_P">
        <a href="Formularios/Dados/Permissoes.aspx"     onclick="return lstMenuCliente_A_OnClick(this)">Permissões</a>
    </li>
    <li class="Tipo_U">
        <a href="Formularios/Dados/Grupos.aspx"      onclick="return lstMenuCliente_A_OnClick(this)">Grupos</a>
    </li>
    <li class="Tipo_U Tipo_G">
        <a href="Formularios/Dados/Perfis.aspx" onclick="return lstMenuCliente_A_OnClick(this)">Perfis</a>
    </li>
    
    <li class="Tipo_U Tipo_G">
        <a href="Formularios/Dados/Desbloqueio.aspx" onclick="return lstMenuCliente_A_OnClick(this)">Desbloqueios</a>
    </li>

    <li class="SubTitulo" class="Tipo_G Tipo_P Tipo_PS">Ações</li>

    <li class="Tipo_G Tipo_P Tipo_PS">
        <a href="#" onclick="return lstMenuSeguranca_Excluir_OnClick(this)">Excluir</a>
    </li>


<li class="Fundo TopoFundo"></li>
</ul>

<div class="pnlFormulario" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>

    <div id="pnlSeguranca_Formularios_Dados_DadosCompletos"></div>
    <div id="pnlSeguranca_Formularios_Dados_Usuarios"></div>
    <div id="pnlSeguranca_Formularios_Dados_Permissoes"></div>
    <div id="pnlSeguranca_Formularios_Dados_Grupos"></div>
    <div id="pnlSeguranca_Formularios_Dados_Perfis"></div>
    <div id="pnlSeguranca_Formularios_Dados_AssociarPermissoes"></div>
    <div id="pnlSeguranca_Formularios_Dados_Desbloqueio"></div>
</div>
