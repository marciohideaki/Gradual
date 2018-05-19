<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Default" %>

<input type="hidden" id="hdMenuClientesPermissaoAlterarPasso_3_4" runat="server" />

<ul id="lstClientesSelecionados" class="ListaDeObjetos">
    
    <li class="NovoItem" style="display:none">
        <h3>
            <span>Novo Cliente</span>
            
            <button onclick="return lstItensSelecionados_btnCancelarNovo_OnClick(this, 'cliente')"  title="Cancelar Clientes"   class="IconButton Cancelar"><span>Cancelar</span></button>
        </h3>
    </li>

    <li class="Template Template_Individual" style="display:none;">
        <button class="btnFecharItem" onclick="return lstItensSelecionados_btnFechar_Click(this)" title="Remover item da lista"><span>Fechar</span></button>

        <input type="hidden" Propriedade="json" />

        <h3 onclick="return lstClientesSelecionados_H3_OnClick(this)" class="Cliente_M">
            <span Propriedade="NomeCliente">Nome do Cliente</span>
        </h3>

        <ul class="Detalhes">
            <li class="Item_Bovespa">  <label>Bovespa:</label>       <div Propriedade="CodBovespaComConta"></div></li>
            <li class="Item_BMF">      <label>BMF:</label>           <div style="margin-left: -20px;" Propriedade="CodBMFComConta"></div></li> 
            
            <li class="MensagemAlertaDadosCliente" style="float: left;padding: 3em 0em 0em;margin-bottom: -7em;display:none;" onclick="FadeOutMensagemAlertaCliente(this);">
                <label style="cursor:pointer;font-size:larger;color:#FF0000;width:100%;">Cliente Com Pendência</label>
                <div Propriedade="Cise"></div>
            </li>
                                       
            <li class="Item_Doc" style="margin-top: -10px;"> <label>CPF/CNPJ:</label>      <div Propriedade="CPF"></div></li>
            <li class="Item_Assessor">                       <label>Cód. Assessor:</label> <div Propriedade="CodAssessor"></div></li>
                                         
            <li class="Item_DataCad">                        <label>Data Cad.:</label>     <div Propriedade="DataCadastro"></div></li>
            <li class="Item_DataRecad">                      <label>Data Recad.:</label>   <div Propriedade="DataRecadastro"></div></li>
            <li class="Item_Email">                          <label>Email:</label>         <div Propriedade="Email"></div></li>
        </ul>                                                

    </li>
</ul>


<ul id="lstMenuCliente" class="MenuDoObjeto" style="display:none">
    <li class="Topo TopoFundo">
        <a href="#" onmouseover="return lnkMenuPersonalizado_Click(this)" style="display:none"><span>Scroll do Menu</span></a>
    </li>

    <li class="MenuRolavelContainer">

        <ul class="MenuRolavel">

            <li class="SubTitulo ExibirEmNovo Tipo_Condicional_Bloquear_Passo3">Dados Cadastrais</li>

            <asp:Repeater runat="server" id="rpt_Menu_ClienteDados">
                <ItemTemplate>
                    <li class='<%# Eval("Tag").ToString().Split(';')[1]  %>'><a href='<%# Eval("Tag").ToString().Split(';')[0] %>' onclick="return lstMenuCliente_A_OnClick(this)"><%# Eval("Nome") %></a></li>
                </ItemTemplate>
            </asp:Repeater>

            <li class="SubTitulo">Consultas</li>

            <asp:Repeater runat="server" id="rpt_Menu_ClienteAcoes">
                <ItemTemplate>
                    <li class='<%# Eval("Tag").ToString().Split(';')[1]  %>'><a href='<%# Eval("Tag").ToString().Split(';')[0] %>' onclick="return lstMenuCliente_A_OnClick(this)"><%# Eval("Nome") %></a></li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>

    </li>

    <li class="Fundo TopoFundo">
        <a href="#" onmouseover="return lnkMenuPersonalizado_Click(this, -250)" style="display:none"><span>Scroll do Menu</span></a>
    </li>
</ul>

<div class="pnlFormulario" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>

    <div id="pnlClientes_Formularios_Dados_DadosCompletos"></div>
    <div id="pnlClientes_Formularios_Dados_Enderecos"></div>
    <div id="pnlClientes_Formularios_Dados_Telefones"></div>
    <div id="pnlClientes_Formularios_Dados_InformacoesPatrimoniais"></div>
    <div id="pnlClientes_Formularios_Dados_RendimentosSituacaoFinanceira"></div>
    <div id="pnlClientes_Formularios_Dados_InformacoesBancarias"></div>
    <div id="pnlClientes_Formularios_Dados_Procuradores"></div>
    <div id="pnlClientes_Formularios_Dados_Representantes"></div>
    <div id="pnlClientes_Formularios_Dados_RepresentantesLegais"></div>
    <div id="pnlClientes_Formularios_Dados_PessoasAutorizadas"></div>
    <div id="pnlClientes_Formularios_Dados_RepresentantesParaNaoResidente"></div>
    <div id="pnlClientes_Formularios_Dados_EmpresasColigadas"></div>
    <div id="pnlClientes_Formularios_Dados_Diretores"></div>
    <div id="pnlClientes_Formularios_Dados_Contratos"></div>
    <div id="pnlClientes_Formularios_Dados_Posicao"></div>    
    <div id="pnlClientes_Formularios_Dados_Custodia"></div>
    <div id="pnlClientes_Formularios_Dados_ContaCorrente"></div>
    <div id="pnlClientes_Formularios_Dados_ClubesEFundos"></div>    
    <div id="pnlClientes_Formularios_Dados_DadosPlanosCliente"></div>
    <div id="pnlClientes_Formularios_Acoes_Suitability"></div>
    <div id="pnlClientes_Formularios_Acoes_Inativar"></div>
    <div id="pnlClientes_Formularios_Acoes_AlterarAssessor"></div>
    <div id="pnlClientes_Formularios_Acoes_AlterarBolsa"></div>
    <div id="pnlClientes_Formularios_Acoes_VerLimites"></div>
    <div id="pnlClientes_Formularios_Acoes_VerPendencias"></div>
    <div id="pnlClientes_Formularios_Acoes_DocumentacaoEntregue"></div>
    <div id="pnlClientes_Formularios_Acoes_VerFichaCadastral"></div>
    <div id="pnlClientes_Formularios_Acoes_SincronizarComSinacor"></div>
    <div id="pnlClientes_Formularios_Acoes_EfetivarRenovacao"></div>
    <div id="pnlClientes_Formularios_Acoes_ConfigurarLimites"></div>
    <div id="pnlClientes_Formularios_Acoes_ConfigurarLimitesBMF"></div>
    <div id="pnlClientes_Formularios_Acoes_ConfigurarLimitesNovoOMS"></div>
    <div id="pnlClientes_Formularios_Acoes_SolicitacaoAlteracaoCadastral"></div>
    <div id="pnlClientes_Formularios_Acoes_AssociarPermissoesParametros"></div>
    <div id="pnlClientes_Formularios_Acoes_ImpostoDeRenda"></div>
    <div id="pnlClientes_Formularios_Acoes_SpiderLimitesBMF"></div>
    <div id="pnlClientes_Formularios_Acoes_SpiderLimitesBVSP"></div>
    <div id="pnlClientes_Formularios_Acoes_Resgate"></div>
</div>

<!--div class="pnlRelatorio" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>

    <div id="pnlClientes_Relatorios_Resultados" style="display:none">
    
    </div>
    
</div>

<div class="pnlFormularioExtendido" style="display:none">
    <span class="Aguarde">Carregando, por favor aguarde...</span>

    <div id="pnlClientes_MigracaoEntreAssessores" style="display:none">
    
    </div>
    
</div-->
