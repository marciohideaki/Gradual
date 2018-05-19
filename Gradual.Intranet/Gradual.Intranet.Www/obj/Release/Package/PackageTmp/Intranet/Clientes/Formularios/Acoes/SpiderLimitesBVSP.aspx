<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpiderLimitesBVSP.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.SpiderLimitesBVSP" %>

<form id="form1" runat="server">

    <div class="pnlCliente_Spider_Limites_Container">
    
        <input type="hidden" id="hidClienteLimites" runat="server" />

        <ul class="pnlFormulario_Abas_Container">
            <li class="Selecionada"><a href="#" rel="pnlCliente_Spider_Limites_Bovespa" onclick="return pnlFormulario_Abas_li_a_Click(this)">Parâmetros</a></li>
            <li><a href="#" rel="pnlCliente_Spider_Restricoes" onclick="return pnlFormulario_Abas_li_a_Click(this)">Restrições</a></li>
        </ul>

        <div id="pnlCliente_Spider_Limites_Bovespa">
            
            <button id="btnCliente_limites_Bovespa_AtualizarDados" onclick="return btnCliente_limites_Bovespa_AtualizarDados_Click(this);" style="float: left; position: absolute; width: 90px; margin-left: -162px; display: none;">Atualizar Tela</button>

            <div class="pnlCliente_Limites_Bovespa_Painel" id="pnlCliente_Limites_Bovespa_Painel_Esquerda" runat="server">
                <div class="divParametroLimite divParametroLimiteOperaAVista">
                    <p style="margin-top:1em">
                        <label for="chkCliente_Spider_Limites_Bosvespa_AVista_Opera" >Opera à Vista</label>
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_Opera_IncluirLimite" class="FlagIncluirLimite" value="01/01/1901" />
                        <input  id="chkCliente_Spider_Limites_Bosvespa_AVista_Opera" type="checkbox" runat="server" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(this)" />
                    </p>

                    <p class="OperaAVistaLimite">   
                        <label for="txtCliente_Spider_Limites_Bosvespa_AVista_Limite" id="lblCliente_Limites_Bosvespa_AVista_Limite">Limite R$</label>
                        <input id="hddCliente_Spider_Limites_Bosvespa_AVista_Limite" type="hidden" class="LimiteCadastrado" runat="server" />
                        <input id="txtCliente_Spider_Limites_Bosvespa_AVista_Limite" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" class="ValorMonetario ProibirLetras validate[funcCall[ValorLimiteBovespa]]" runat="server" />
                        <img src="../Skin/Default/Img/GradIntra_BotaoInformacao.png" id="btnCliente_Limites_Bosvespa_AVista_Limite" onmouseout="$('#divCliente_Spider_Limites_LimiteDisponivelAVista').hide();" onmouseover="GradIntra_Clientes_Spider_Limite_DetalheAlocado(this);$('#divCliente_Spider_Limites_LimiteDisponivelAVista').show();" style="margin: 0px 0px 4px 4px;cursor:pointer;" alt="" />
                    </p>

                    <div id="divCliente_Spider_Limites_LimiteDisponivelAVista" style="display: none;" class="Cliente_Limites_LimiteDisponivel">
                        <p style="padding-top: 10px;">
                            <span>Detalhes à vista</span>
                        </p>
                        <p>
                            <label>Limite R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Limite" />
                        </p>
                        <p>
                            <label>Valor Alocado R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Alocado" />
                        </p>
                        <p>
                            <label>Disponível R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Disponivel" />
                        </p>
                    </div>
                    
                    <p class="OperaAVistaDataDeVencimento">
                        <label for="txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimento" >Vencimento</label>
                        <input id="hddCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimento" class="DataVencimentoCadastrada" type="hidden" runat="server" value="01/01/1901" />
                        <input id="txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimento" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" maxlength="10" class="Mascara_Data Picker_Data validate[required,custom[data]]" style="float:left;width:8em" runat="server" />
                    </p>

                    <p id="pnlLinksParametroLimiteOperaAVista" class="LinksDeRenovacao" runat="server">
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_Opera_ExpirarLimite" class="FlagExpirarLimite" />
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_Opera_RenovarLimite" class="FlagRenovar" />
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(this);">Expirar</a>
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(this);" runat="server" id="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_OperaAVista" >Renovar</a>
                    </p>
                </div>

                <div class="divParametroLimite divParametroLimiteOperaAVistaDescoberto">
                    <p>
                        <label for="chkCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto">Descoberto</label>
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_IncluirLimite" class="FlagIncluirLimite" />
                        <input  id="chkCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto" type="checkbox" runat="server" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(this)" />
                    </p>
                    <p>
                        <label for="txtCliente_Spider_Limites_Bosvespa_AVista_LimiteDescoberto">Limite R$</label>
                        <input  id="hddCliente_Spider_Limites_Bosvespa_AVista_LimiteDescoberto" type="hidden" class="LimiteCadastrado" runat="server" />
                        <input  id="txtCliente_Spider_Limites_Bosvespa_AVista_LimiteDescoberto" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" class="ValorMonetario ProibirLetras validate[funcCall[ValorLimiteBovespa]]" runat="server" />
                        <img src="../Skin/Default/Img/GradIntra_BotaoInformacao.png" onmouseout="$('#divCliente_Spider_Limites_LimiteDisponivelAVistaDescoberto').hide();" onmouseover="GradIntra_Clientes_Spider_Limite_DetalheAlocado(this);$('#divCliente_Spider_Limites_LimiteDisponivelAVistaDescoberto').show();" style="margin: 0px 0px 4px 4px;cursor:pointer;" alt="" />
                    </p>
                    <div id="divCliente_Spider_Limites_LimiteDisponivelAVistaDescoberto" style="display: none; top:170px;" class="Cliente_Limites_LimiteDisponivel">
                        <p style="padding-top: 10px;">
                            <span>Detalhes à vista descoberto</span>
                        </p>
                        <p>
                            <label>Limite R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Descoberto_Limite" />
                        </p>
                        <p>
                            <label>Valor Alocado R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Descoberto_Alocado" />
                        </p>
                        <p>
                            <label>Disponível R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_AVista_Descoberto_Disponivel" />
                        </p>
                    </div><p>
                        <label for="txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto">Vencimento</label>
                        <input id="hddCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto" type="hidden" class="DataVencimentoCadastrada" runat="server" value="01/01/1901" />
                        <input id="txtCliente_Spider_Limites_Bosvespa_AVista_DataDeVencimentoDescoberto" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" maxlength="10" class="Mascara_Data Picker_Data validate[required,custom[data]]" style="float:left;width:8em" runat="server" />
                    </p>
                    <p id="pnlLinksParametroLimiteOperaAVistaDescoberto" class="LinksDeRenovacao" runat="server">
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_ExpirarLimite" class="FlagExpirarLimite" />
                        <input type="hidden" id="txtCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto_RenovarLimite" class="FlagRenovar" />
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(this);">Expirar</a>
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(this);" runat="server" id="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_AVistaDescoberto">Renovar</a>
                    </p>
                </div>

            </div>

            <div class="pnlCliente_Limites_Bovespa_Painel pnlCliente_Limites_Bovespa_Painel_direito" id="pnlCliente_Limites_Bovespa_Painel_direito" runat="server">
                <div class="divParametroLimite divParametroLimiteOperaOpcao">
                    <p style="margin-top:1em">
                        <label for="chkCliente_Spider_Limites_Bovespa_Opcao_Opera">Opera Opção</label>
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_Opera_IncluirLimite" class="FlagIncluirLimite" />
                        <input  id="chkCliente_Spider_Limites_Bovespa_Opcao_Opera" type="checkbox" runat="server" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(this)" />
                    </p>
                    <p>
                        <label for="txtCliente_Spider_Limites_Bovespa_Opcao_Limite">Limite R$</label>
                        <input  id="hddCliente_Spider_Limites_Bovespa_Opcao_Limite" type="hidden" class="LimiteCadastrado" runat="server" />
                        <input  id="txtCliente_Spider_Limites_Bovespa_Opcao_Limite" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" class="ValorMonetario ProibirLetras validate[funcCall[ValorLimiteBovespa]]" runat="server" />
                        <img src="../Skin/Default/Img/GradIntra_BotaoInformacao.png" onmouseout="$('#divCliente_Spider_Limites_LimiteDisponivelOpcao').hide();" onmouseover="GradIntra_Clientes_Spider_Limite_DetalheAlocado(this);$('#divCliente_Spider_Limites_LimiteDisponivelOpcao').show();" style="margin: 0px 0px 4px 4px;cursor:pointer;" alt="" />
                    </p>
                    <div id="divCliente_Spider_Limites_LimiteDisponivelOpcao" style="display: none; left: -22px" class="Cliente_Limites_LimiteDisponivel">
                        <p style="padding-top: 10px;">
                            <span>Detalhes para opções</span>
                        </p>
                        <p>
                            <label>Limite R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Limite" />
                        </p>
                        <p>
                            <label>Valor Alocado R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Alocado" />
                        </p>
                        <p>
                            <label>Disponível R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Disponivel" />
                        </p>
                    </div>
                    <p>
                        <label for="txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimento">Vencimento</label>
                        <input id="hddCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimento" class="DataVencimentoCadastrada" type="hidden" runat="server" value="01/01/1901" />
                        <input id="txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimento" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" maxlength="10" class="Mascara_Data Picker_Data  validate[required,custom[data]]" style="float:left;width:8em" runat="server" />
                    </p>
                    <p id="pnlLinksParametroLimiteOperaOpcao" class="LinksDeRenovacao" runat="server">
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_Opera_ExpirarLimite" class="FlagExpirarLimite" />
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_Opera_RenovarLimite" class="FlagRenovar" />
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(this);">Expirar</a>
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(this);"  runat="server" id="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_OperaOpcao">Renovar</a>
                    </p>
                </div>
                <div class="divParametroLimite divParametroLimiteOperaOpcaoDescoberto">
                    <p>
                        <label for="chkCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto">Descoberto</label>
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_IncluirLimite" class="FlagIncluirLimite" />
                        <input  id="chkCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto" type="checkbox" runat="server" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(this)" />
                    </p>
                    <p>
                        <label for="txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto">Limite R$</label>
                        <input  id="hddCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto" type="hidden" class="LimiteCadastrado" runat="server" />
                        <input  id="txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" class="ValorMonetario ProibirLetras validate[funcCall[ValorLimiteBovespa]]" runat="server" />
                        <img src="../Skin/Default/Img/GradIntra_BotaoInformacao.png" onmouseout="$('#divCliente_Spider_Limites_LimiteDisponivelOpcaoDescoberto').hide();" onmouseover="GradIntra_Clientes_Spider_Limite_DetalheAlocado(this);$('#divCliente_Spider_Limites_LimiteDisponivelOpcaoDescoberto').show();" style="margin: 0px 0px 4px 4px;cursor:pointer;" alt="" />
                    </p>
                    <div id="divCliente_Spider_Limites_LimiteDisponivelOpcaoDescoberto" style="display: none; left:-22px; top: 160px;" class="Cliente_Limites_LimiteDisponivel">
                        <p style="padding-top: 10px;">
                            <span>Detalhes para opções descoberto</span>
                        </p>
                        <p>
                            <label>Limite R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Descoberto_Limite" />
                        </p>
                        <p>
                            <label>Valor Alocado R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Descoberto_Alocado" />
                        </p>
                        <p>
                            <label>Disponível R$: </label>
                            <input type="text" class="ValorMonetario" disabled="disabled" runat="server" id="txtCliente_Spider_Detalhes_Opcoes_Descoberto_Disponivel" />
                        </p>
                    </div>
                    <p>
                        <label for="txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto">Vencimento</label>
                        <input id="hddCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto" type="hidden" class="DataVencimentoCadastrada" runat="server" value="01/01/1901" />
                        <input id="txtCliente_Spider_Limites_Bovespa_Opcao_DataDeVencimentoDescoberto" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" maxlength="10" class="Mascara_Data Picker_Data  validate[required,custom[data]]" style="float:left;width:8em" runat="server" />
                    </p>
                    <p id="pnlLinksParametroLimiteOperaOpcaoDescoberto" class="LinksDeRenovacao" runat="server">
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_ExpirarLimite" class="FlagExpirarLimite" />
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto_RenovarLimite" class="FlagRenovar" />
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(this);">Expirar</a>
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(this);"  runat="server" id="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_OpcaoDescoberto">Renovar</a>
                    </p>
                </div>
            </div>

            <div class="divParametroLimite divParametroLimiteValorMaximoDaOrdem" id="divParametroLimiteValorMaximoDaOrdem" runat="server">
                <p style="margin-top:0em;margin-bottom:2em">
                    <p style="margin-left:215px; width:70%">
                        <label for="chkCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem">Valor Máximo da Ordem</label>
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_IncluirLimite" class="FlagIncluirLimite" />
                        <input  id="chkCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem" type="checkbox" runat="server" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Click(this)" />
                    </p>
                    <p style="margin-left: 75px; width:70%">
                        <label for="txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem">Limite R$</label>
                        <input  id="hddCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem" type="hidden" class="LimiteCadastrado" runat="server" />
                        <input  id="txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem" style="width:130px;" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" class="ValorMonetario ProibirLetras validate[funcCall[ValorLimiteBovespa]]" runat="server" />
                    </p>
                    <p style="margin-left: 75px; width:70%">
                        <label for="txtCliente_Spider_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento">Vencimento</label>
                        <input id="hddCliente_Spider_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento" type="hidden" class="DataVencimentoCadastrada" runat="server" value="01/01/1901" />
                        <input id="txtCliente_Spider_Limites_Bosvespa_ValorMaximoDaOrdem_DataDeVencimento" onblur="return txtCliente_Spider_Limites_Bovespa_Opcao_LimiteDescoberto_OnBlur(this)" type="text" maxlength="10" class="Mascara_Data Picker_Data  validate[required,custom[data]]" style="float:left;width:8em" runat="server" />
                    </p>
                    <p id="pnlLinksParametroLimiteValorMaximoDaOrdem" style="margin-left:150px; width:70%" class="LinksDeRenovacao" runat="server">
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_ExpirarLimite" class="FlagExpirarLimite" />
                        <input type="hidden" id="txtCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem_RenovarLimite" class="FlagRenovar" />
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkExpirarLimite_Click(this);">Expirar</a>
                        <a href="#" class="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite" onclick="return pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_Click(this);" runat="server" id="pnlCliente_Limites_Bovespa_Painel_lnkRenovarLimite_MaximoDaOrdem">Renovar</a>
                    </p>
                </p>
            </div>

            <asp:repeater id="rptCliente_Spider_Permissoes" runat="server" onitemdatabound="rptCliente_Permissoes_ItemDataBound">
            <ItemTemplate>

            <p style="margin-left: 80px; width:70%">
                <label  id="lblCliente_Permissoes_Bovespa" style="text-align:left" runat="server"><%#Eval("NomePermissao")%></label>
                <input  id="chkCliente_Permissoes_Bovespa" type="checkbox" runat="server" />
            </p>

            </ItemTemplate>
            </asp:repeater>
            
            <p class="BotoesSubmit" id="btnClientes_Limites_Bovespa" runat="server" style="margin-top: 3em;">
                <a href="#" onclick="return btnClientes_Spider_Limites_Bovespa_Click(this)">Salvar Dados</a>
            </p>

        </div>

        <div id="pnlCliente_Spider_Restricoes" style="display:none; margin-left:10px;">
            
            <div id="pnlRestricoesPorGrupo" style="display: none;>
                <h4>Restrição por Grupo</h4>
    
                <h3 style="margin-left: 5%; width: 30em; text-align: left;">
                    <a id="RestricaoPorGrupo_AVista_ExpandCollapse" style="cursor:pointer;font-size:9px;" onclick="GradIntra_Clientes_Restricoes_RestricaoPorGrupo_ExpandCollapse_Click(this);">[ + ]</a>
                    <label for="RestricaoPorGrupo_AVista_ExpandCollapse">À Vista</label>
                </h3>

                <div class="ClienteRestricaoClienteParametroGrupo" style="display:none;">

                    <asp:repeater runat="server" id="rptCliente_ResticoesPorGrupoAVista" onitemdatabound="rptCliente_ResticoesPorGrupo_ItemDataBound">
                    <itemtemplate>

                        <p>
                            <label style="width:130px;"><%# Eval("NomeDoGrupo")%></label>

                            <input type="hidden" class="Cliente_ResticoesPorGrupoParametro" id="txtClienteParametro" value="12" runat="server" />
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoPermite1" value='<%# Eval("CodigoGrupo") %>' checked="true" />
                            <label id="lblResticaoPorGrupoPermite" runat="server">Permite</label>
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoRestringe1" class="Cliente_ResticoesPorGrupoRestringe" value='<%# Eval("CodigoGrupo") %>' />
                            <label id="lblResticaoPorGrupoRestringe" runat="server">Restringe</label>
                        
                        </p>

                    </itemtemplate>
                    </asp:repeater>
                
                </div>
                
                <h3 style="margin-left: 5%; width: 30em; text-align: left;">
                    <a style="cursor:pointer;font-size:9px;" onclick="GradIntra_Clientes_Restricoes_RestricaoPorGrupo_ExpandCollapse_Click(this);">[ + ]</a>
                    À Vista - Descoberto
                </h3>

                <div class="ClienteRestricaoClienteParametroGrupo" style="display:none;">

                    <asp:repeater runat="server" id="rptCliente_ResticoesPorGrupoAVistaDescoberto" onitemdatabound="rptCliente_ResticoesPorGrupo_ItemDataBound">
                    <itemtemplate>

                        <p>
                            <label style="width:130px;"><%# Eval("NomeDoGrupo")%></label>

                            <input type="hidden" class="Cliente_ResticoesPorGrupoParametro" id="Hidden1" value="5" runat="server" />
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoPermite2" value='<%# Eval("CodigoGrupo") %>' checked="true" />
                            <label id="Label3" runat="server">Permite</label>
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoRestringe2" class="Cliente_ResticoesPorGrupoRestringe" value='<%# Eval("CodigoGrupo") %>' />
                            <label id="Label4" runat="server">Restringe</label>
                        
                        </p>

                    </itemtemplate>
                    </asp:repeater>

                </div>
                
                <h3 style="margin-left: 5%; width: 30em; text-align: left;">
                    <a style="cursor:pointer;font-size:9px;" onclick="GradIntra_Clientes_Restricoes_RestricaoPorGrupo_ExpandCollapse_Click(this);">[ + ]</a>
                    Opções
                </h3>

                <div class="ClienteRestricaoClienteParametroGrupo" style="display:none;">

                    <asp:repeater runat="server" id="rptCliente_ResticoesPorGrupoOpcoes" onitemdatabound="rptCliente_ResticoesPorGrupo_ItemDataBound">
                    <itemtemplate>

                        <p>
                            <label style="width:130px;"><%# Eval("NomeDoGrupo")%></label>

                            <input type="hidden" class="Cliente_ResticoesPorGrupoParametro" id="Hidden2" value="13" runat="server" />
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoPermite3" value='<%# Eval("CodigoGrupo") %>' checked="true" />
                            <label id="Label5" runat="server">Permite</label>
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoRestringe3" class="Cliente_ResticoesPorGrupoRestringe" value='<%# Eval("CodigoGrupo") %>' />
                            <label id="Label6" runat="server">Restringe</label>
                        </p>

                    </itemtemplate>
                    </asp:repeater>

                </div>
                
                <h3 style="margin-left: 5%; width: 30em; text-align: left;">
                    <a style="cursor:pointer;font-size:9px;" onclick="GradIntra_Clientes_Restricoes_RestricaoPorGrupo_ExpandCollapse_Click(this);">[ + ]</a>
                    Opções - Descoberto
                </h3>

                <div class="ClienteRestricaoClienteParametroGrupo" style="display:none;">

                    <asp:repeater runat="server" id="rptCliente_ResticoesPorGrupoOpcoesDescoberto" onitemdatabound="rptCliente_ResticoesPorGrupo_ItemDataBound">
                    <itemtemplate>

                        <p>
                            <label style="width:130px;"><%# Eval("NomeDoGrupo")%></label>

                            <input type="hidden" class="Cliente_ResticoesPorGrupoParametro" id="Hidden3" value="7" runat="server" />
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoPermite4" value='<%# Eval("CodigoGrupo") %>' checked="true" />
                            <label id="Label7" runat="server">Permite</label>
                        
                            <input type="radio" name="ResticaoPorGrupo" runat="server" id="rdbResticaoPorGrupoRestringe4" class="Cliente_ResticoesPorGrupoRestringe" value='<%# Eval("CodigoGrupo") %>' />
                            <label id="Label8" runat="server">Restringe</label>
                        </p>

                    </itemtemplate>
                    </asp:repeater>

                </div>

            </div>
            
            <div id="pnlRestricoesPorCliente" style="padding-top:3%">

                <div id="pnlCliente_Restricoes_RestricaoPorAtivo">
                    <h4>Restrição por Ativos</h4>

                    <input type="text" id="txtCliente_Restricoes_RestricaoPorAtivo" style="width: 22em; text-transform:uppercase" maxlength="10" />
                    <input type="button" runat="server" id="btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Add" onclick="GradIntra_Clientes_Restricoes_Spider_RestricaoPorAtivos_Add(this);" value="Validar Papel" />
                
                    <p style="float: left; width: 100%; margin-left: -20%;" class="RadiosRestricaoPorAtivosSpider">
                        <label>Direção</label>

                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoAmbosSpider" value="A" checked="true" />
                        <label id="Label2" for="rdbRestricaoPorAtivoAmbosSpider">Ambos</label>

                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoCompraSpider" value="C" />
                        <label id="lblRestricaoPorAtivo" for="rdbRestricaoPorAtivoCompraSpider">Compra</label>
                
                        <input type="radio" name="RestricaoPorAtivo" id="rdbRestricaoPorAtivoVendaSpider" value="V" />
                        <label id="Label1" for="rdbRestricaoPorAtivoVendaSpider">Venda</label>
                    </p>
                </div>

                <div id="pnlListaRestricaoPorCliente" style="padding-top:15px;">
                    <table class="GridIntranet" style="float:left;">
                        <thead> 
                            <tr>
                                <td style="text-align:center;">Papel</td>
                                <td style="text-align:center;">C/V</td>
                                <td style="text-align:center;">Remover</td>
                            </tr>
                        </thead>
                        <tbody class="Clientes_Restricoes_RestricaoPorAtivosSpider">
                            <tr class="Template_RetricaoPorAtivo" style="display:none">
                                <td></td>
                                <td style="text-align:center;"></td>
                                <td align="center" style="text-align:center;"><button runat="server" id="btnGradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_click" class="IconButton Excluir" onclick="GradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_Spider_Click(this)"></button></td>
                            </tr>
                            <asp:repeater runat="server" id="rptCliente_RestricaoPorAtivo">
                            <itemtemplate>
                            <tr>
                                <td><%# Eval("DsInstrumento")%></td>
                                <td style="text-align:center;"><%# Eval("DsDirecao")%></td>
                                
                                <td align="center" style="text-align:center;"><button id="Button1" runat="server" visible=<%#PermissaoExcluir()%>   class="IconButton Excluir" onclick="GradIntra_Clientes_Restricoes_RestricaoPorAtivos_Remover_Spider_Click(this)"></button></td>
                            </tr>
                            </itemtemplate>
                            </asp:repeater>
                        </tbody>
                    </table>
                </div>

            </div>

            <p class="BotoesSubmit" style="margin-top:3em">
                <button runat="server" id="btnCliente_Restricoes" onclick="return btnCliente_Restricoes_Spider_Click(this)">Salvar Dados</button>
            </p>

        </div>

    </div>

    <script type="text/javascript">
        $.ready(pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load('#chkCliente_Spider_Limites_Bosvespa_AVista_Opera'));
        $.ready(pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load('#chkCliente_Spider_Limites_Bosvespa_AVista_OperaDescoberto'));
        $.ready(pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load('#chkCliente_Spider_Limites_Bovespa_Opcao_Opera'));
        $.ready(pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load('#chkCliente_Spider_Limites_Bovespa_Opcao_OperaDescoberto'));
        $.ready(pnlCliente_Limites_Bovespa_Painel_lnkHabilitarLimite_Load('#chkCliente_Spider_Limites_Bovespa_ValorMaximoDaOrdem'));
    </script>

</form>