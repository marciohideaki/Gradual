<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Posicao.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Posicao" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cliente - Posição</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="pnlCliente_Posicao_Relatorio" style="display: none;"></div>

    <div class="PosicaoConsolidadaCabecalhoConsulta">

        <h4>Posição consolidada</h4>
         
        <h5 style="margin-bottom: 2em">Selecione abaixo o relatório de posição do cliente.</h5>

        <div id="pnlRelatorio_Posicao" class="pnlFormulario_Contratos">

            <input type="hidden" id="txtClientes_Contratos_ParentId" propriedade="ParentId" runat="server" />

            <p style="width: 48em;">
                <label for="cmbRelatorio_Posicao_Carregar">Relatório:</label>
                <select id="cmbRelatorio_Posicao_Carregar" style="width: 25em;" onchange="cmbRelatorio_Posicao_Carregar_Change()">
                    <option value="-1" selected>Selecione...</option>
                    <option value="0">Extrato</option>
                    <option value="1">Notas de Corretagem</option>
                    <%--<option value="2">Custódia</option>--%>
                    <option value="3">Saldos e Limites</option>
                    <option value="6">Imposto de Renda</option>
                    <option value="7">Fax</option>
                    <option value="8">Extrato Operações</option>
                </select>
            </p>

        </div>

        <div style="padding-top: 10px;">

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_ExtratoDeConta" style="display: none;">
                <p style="width: 48em;">
                    <label for="txtRelatorio_Extrato_DataInicial">Data Inicial:</label>
                    <input type="text" id="txtRelatorio_Extrato_DataInicial" value='<%=DateTime.Now.ToString("01/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <p style="width: 48em;">
                    <label for="txtRelatorio_Extrato_DataFinal">Data Final:</label>
                    <input type="text" id="txtRelatorio_Extrato_DataFinal" value='<%=DateTime.Now.ToString(string.Concat(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString(), "/MM/yyyy")) %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <fieldset id="fdsTipoDeExtrato" style="margin: 1em 0pt 0pt 14.75em; height: 7em; width: 24.5em;">
                    <legend>Extrato por:</legend>
                    <p>
                        <input type="radio" name="TipoDeExtrato" runat="server" id="rdbExtratoPorLiquidacao" value="liq" checked="true" />
                        <label id="lblExtratoPorLiquidacao" for="rdbExtratoPorLiquidacao" runat="server">Liquidação</label>
                    </p>
                    <p>
                        <input type="radio" name="TipoDeExtrato" runat="server" id="rdbExtratoPorMovimento" value="mov" />
                        <label id="lblExtratoPorMovimento" for="rdbExtratoPorMovimento" runat="server">Movimento</label>
                    </p>
                </fieldset>
            </div>

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_NotasDeCorretagem" style="display: none">
                <p style="width: 48em;">
                    <%--<label for="txtRelatorio_ExtratoFatura_Data_Inicial">Data Início:</label>
                    <select id="txtRelatorio_ExtratoFatura_Data_Inicial" style="float:left;width: 8em">
                    <asp:Repeater ID="rptItensDatasOperacaoInicial" runat="server" >
                        <ItemTemplate>
                            <option value="<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DtUltimasNegociacoes")).ToString("dd/MM/yyyy")%>">
                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DtUltimasNegociacoes")).ToString("dd/MM/yyyy")%>
                            </option>
                        </ItemTemplate>
                    </asp:Repeater>
                    </select>--%>
                    <label for="txtRelatorio_ExtratoFatura_Data_Inicial">Data Inicial:</label>
                    <input type="text" id="txtRelatorio_ExtratoFatura_Data_Inicial" value='<%=DateTime.Now.ToString("01/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <p style="float: left; width: 48em;">
                    <%--<label for="txtRelatorio_ExtratoFatura_Data_Final">Data Fim:</label>
                    <select id="txtRelatorio_ExtratoFatura_Data_Final" style="float:left;width: 8em">
                    <asp:Repeater ID="rptItensDatasOperacaoFinal" runat="server" >
                        <ItemTemplate>
                            <option value="<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DtUltimasNegociacoes")).ToString("dd/MM/yyyy")%>">
                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DtUltimasNegociacoes")).ToString("dd/MM/yyyy")%>
                            </option>
                        </ItemTemplate>
                    </asp:Repeater>
                    </select>--%>
                    <label for="txtRelatorio_ExtratoFatura_Data_Final">Data Final:</label>
                    <input type="text" id="txtRelatorio_ExtratoFatura_Data_Final" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <p style="width: 48em;">
                    <label for="cboRelatorio_Custodia_Bolsa">Bolsa:</label>
                    <select id="cboRelatorio_Custodia_Bolsa" onchange="return FiltrarComboMercadoPorBolsa(this);">
                        <option value="bov" selected>Bovespa</option>
                        <option value="bmf">BM&amp;F</option>
                    </select>
                </p>
                <p style="float: left; width: 130px; margin-left: 30%; height: 85px; display:none;">
                    <label for="ckbRelatorio_NotaCorretagem_Provisorio">Provisório</label>
                    <input type="checkbox" id="ckbRelatorio_NotaCorretagem_Provisorio" propriedade="provisorio" />
                </p>
                <div style="width: 15em;" id="GrdIntra_Posicao_Cliente_Nota_Corretagem_PainelTipoMercado">
                    <fieldset style="float: left; height: 90px; margin-left: 14.5em; width: 24.25em;">
                        <legend>Tipo de mercado</legend>
                        <p style="padding: 5px 0px 0px 25px; width: 60%; width: 60%;">
                            <input type="radio" name="TipoMercado" runat="server" id="rdbRelatorio_NotaCorretagem_TipoMercado_AVista" class="Cliente_ResticoesPorGrupoRestringe Cliente_Relatorio_NotaCorretagem_TipoMercado" propriedade="TipoMercado" value='VIS' checked="true" />
                            <label id="lblRelatorio_NotaCorretagem_TipoMercado_AVista" for="rdbRelatorio_NotaCorretagem_TipoMercado_AVista" runat="server">A Vista</label>
                       </p>
                       <p style="padding-left: 25px; width: 60%;">  
                            <input type="radio" name="TipoMercado" runat="server" id="rdbRelatorio_NotaCorretagem_TipoMercado_Opcao" class="Cliente_ResticoesPorGrupoRestringe Cliente_Relatorio_NotaCorretagem_TipoMercado" propriedade="TipoMercado" value="OPC" />
                            <label id="lblRelatorio_NotaCorretagem_TipoMercado_Opcao" for="rdbRelatorio_NotaCorretagem_TipoMercado_Opcao" runat="server">Opção</label>
                       </p>
                   </fieldset>
               </div>
            </div>

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_Custodia" style="display: none">
                <p style="width: 48em;">
                    <label for="cboRelatorio_Custodia_Bolsa_Hide">Bolsa:</label>
                    <select id="cboRelatorio_Custodia_Bolsa_Hide" onchange="return FiltrarComboMercadoPorBolsa(this);">
                        <option value="bov" selected>Bovespa</option>
                        <option value="bmf">BM&amp;F</option>
                    </select>
                </p>
                <p style="display: none; width: 48em;">
                    <label for="cboRelatorio_Custodia_Agrupar">Agrupar:</label>
                    <select id="cboRelatorio_Custodia_Agrupar">
                        <option value="-1" selected>Anal&iacute;tica</option>
                    </select>
                </p>
                <p style="width: 48em;">
                    <label for="cboRelatorio_Custodia_Mercado">Mercado:</label>
                    <select id="cboRelatorio_Custodia_Mercado">
                        <option value="VIS" class="Visivel_Bovespa">&Agrave; vista</option>
                        <option value="FUT" class="Visivel_Bmf">Futuro</option>
                        <option value="OPC" class="Visivel_Bovespa">Op&ccedil;&atilde;o</option>
                        <option value="TER" class="Visivel_Bovespa">Termo</option>
                        <option value="" class="Visivel_Bovespa Visivel_Bmf" selected>Todos</option>
                    </select>
                </p>
            </div>

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_Financeiro" style="display: none">
                <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_Volume" style="display: none">
                    <p style="width: 48em;">
                        <label for="txtRelatorio_Volume_DataInicial">Data Inicial:</label>
                        <input type="text" id="txtRelatorio_Volume_DataInicial" value='<%=DateTime.Now.ToString("01/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                    </p>
                    <p style="width: 48em;">
                        <label for="txtRelatorio_Volume_DataFinal">Data Final:</label>
                        <input type="text" id="txtRelatorio_Volume_DataFinal" value='<%=DateTime.Now.ToString(string.Concat(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString(), "/MM/yyyy")) %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                    </p>
                </div>
            </div>

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_HistoricoMovimentacoes" style="display: none">
                <p style="width: 48em;">
                    <label for="txtRelatorio_HistoricoMovimentacoes_Data">Data:</label>
                    <input type="text" id="txtRelatorio_HistoricoMovimentacoes_Data" value='<%=DateTime.Now.ToString("01/MM/yyyy") %>'style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
            </div>

            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_ImpostoDeRenda" style="display: none">
                
                <p style="width: 99%; margin-top: 15px; width: 48em;">
                    <label for="cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">Ano de exercício:</label>
                    <select id="cmbGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">
                        <asp:repeater runat="server" id="rptGradIntra_Clientes_ImpostoDeRenda_AnoVigencia">
                            <itemtemplate>
                                <option value="<%# Eval("Key") %>"><%# Eval("Value") %></option>
                            </itemtemplate>
                        </asp:repeater>
                    </select>
                </p>

                <p class="BotoesSubmit" style="">
                    <button id="btnGradIntra_Clientes_ImpostoDeRenda_Operacoes" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Operações</button>
                    <button id="btnGradIntra_Clientes_ImpostoDeRenda_TesouroDireto" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Tesouro Direto</button>
                    <button id="btnGradIntra_Clientes_ImpostoDeRenda_DayTrade" onclick="return GradIntra_Clientes_ImpostoDeRenda_GerarComprovante_Click(this)">Day Trade</button>
                </p>
            </div>
            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_Fax" style="display: none">
                <p style="width: 48em;">
                    <label for="txtRelatorio_Fax_Data_Inicial">Data:</label>
                    <input type="text" id="txtRelatorio_Fax_Data_Inicial" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <%--<p style="float: left; width: 48em;">
                    <label for="txtRelatorio_Fax_Data_Final">Data Final:</label>
                    <input type="text" id="txtRelatorio_Fax_Data_Final" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>--%>
                <p style="width: 48em;">
                    <label for="cboRelatorio_Fax_Tipo">Tipo:</label>
                    <select id="cboRelatorio_Fax_Tipo" onchange="return FiltrarComboMercadoPorBolsa(this);">
                        <option value="bov_br" selected>Bovespa (PT)</option>
                        <option value="bov_resumido">Bovespa (Resumido)</option>
                        <option value="bov_en">Bovespa (EN)</option>
                        <option value="bmf_br">Bmf (PT)</option>
                        <option value="bmf_volatilidade">Bmf (Volatilidade)</option>
                        <option value="bmf_en">Bmf (EN)</option>
                        
                    </select>
                </p>
                
            </div>
            <div class="PainelEmTelaCheia_Opcoes_SubFormulario SubForm_ExtratoOperacoes" style="display: none">
                <p style="width: 48em;">
                    <label for="txtRelatorio_Operacoes_Data_Inicial">Data:</label>
                    <input type="text" id="txtRelatorio_Operacoes_Data_Inicial" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>
                <%--<p style="float: left; width: 48em;">
                    <label for="txtRelatorio_Fax_Data_Final">Data Final:</label>
                    <input type="text" id="txtRelatorio_Fax_Data_Final" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
                </p>--%>
                <p style="width: 48em;">
                    <label for="cboRelatorio_ExtratoOperacoes_Tipo">Tipo Operações:</label>
                    <select id="cboRelatorio_ExtratoOperacoes_Tipo" >
                        <option value="bov" selected>Operações Bovespa</option>
                        <option value="bmf">Operações Bmf</option>
                    </select>
                </p>
                
            </div>
            <p class="BotaoCarregarRelatorio BotoesSubmit" style="display: none;">
                <button class="BotaoPadrao BotaoPadraoFundoVerde" onclick="return CarregarRelatorioPosicaoCliente()">Carregar Relatório</button>
            </p>

            <div id="pnlRelatorio_Container" style="display: none;">
                <p class="BarraDeBotoes">
                    <a href="#" onclick="return false">Imprimir</a> <a href="#" onclick="return false"class="BaixarEmArquivo">Baixar em Arquivo</a>
                </p>

                <div class="PainelDoRelatorio">
                </div>

                <div class="RodapeDoRelatorio">
                    <p>Relatório de Extrato de Conta</p>
                </div>
            </div>
        </div>
    </div>
    
    </form>
</body>
</html>
