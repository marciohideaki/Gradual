<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes._Default" %>

<ul id="lstItensSelecionados_VendasDeFerramentas" class="ListaDeObjetos">
    
    <li class="NovoItem" style="display:none">
        <h3>
            <span>Novo</span>
            
            <button onclick="return lstItensSelecionados_btnCancelarNovo_OnClick(this, 'seguranca')"  title="Cancelar" class="IconButton Cancelar"><span>Cancelar</span></button>
        </h3>
    </li>

    <li class="Template Template_VendasDeFerramentas ItemDeUmaLinha" style="display:none">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>
        <input type="hidden" Propriedade="json" />

        <h3 class="Solicitacoes_Venda" onclick="return lstClientesSelecionados_H3_OnClick(this)">
            <span Propriedade="Id" style="width:auto;" title="ID da Venda"></span> <span style="width:auto;padding:0em 0.2em 0em 0.2em">-</span><span Propriedade="CBLC" style="width:auto" title="CBLC do Cliente"></span>
            <span Propriedade="DescProduto"></span>
        </h3>
    </li>

</ul>

<ul id="lstItemMenu_VendasDeFerramentas" class="MenuDoObjeto" style="display:none">
<li class="Topo TopoFundo"></li>

    <li class="SubTitulo ExibirEmNovo">Dados</li>
    
    <li>
        <a href="Formularios/Dados/DadosDeVenda.aspx" id="lnkItemMenu_VendasDeFerramentas_DadosDeVenda" onclick="return lstMenuCliente_A_OnClick(this)">Dados da Venda</a>
    </li>
    <li>
        <a href="Formularios/Dados/LogsDePagamento.aspx" id="lnkItemMenu_VendasDeFerramentas_LogsDePagto" onclick="return lstMenuCliente_A_OnClick(this)">Logs de Pagto</a>
    </li>

    <li class="SubTitulo">Ações</li>

    <li>
        <a href="Formularios/Acoes/AlterarVenda.aspx" id="lnkItemMenu_VendasDeFerramentas_Acoes_AlterarVenda" onclick="return lstMenuCliente_A_OnClick(this, GradIntra_Solicitacoes_PrepararFormularioDeAlteracao)">Alterar Venda</a>
    </li>


<li class="Fundo TopoFundo"></li>
</ul>

<div class="pnlFormulario" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>
    
    <div id="pnlSolicitacoes_Formularios_Dados_DadosDeVenda"></div>
    <div id="pnlSolicitacoes_Formularios_Dados_LogsDePagamento"></div>
    <div id="pnlSolicitacoes_Formularios_Acoes_AlterarVenda"></div>
</div>
