<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Filtros.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.Filtros" %>


<form id="form1" runat="server">

    <input type="hidden" id="hddIdAssessorLogado" value="<%= gIdAsessorLogado %>" />

    <div id="pnlClientes_FiltroRelatorio_FF1" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p style="width:98%">
            <label for="cboClientes_FiltroRelatorio_Relatorio">Relatório:</label>
            <select id="cboClientes_FiltroRelatorio_Relatorio" style="width:504px" onchange="cboRelatorios_TipoDeRelatorio_Change(this)">
                <option value="">[ Selecione ]</option>
                <option value="R001">R-001: Clientes cadastrados por período</option>
                <option value="R002">R-002: Pendências cadastrais</option>
                <option value="R003">R-003: Solicitação de alteração cadastral</option>
                <option value="R004">R-004: Clientes em período de renovação cadastral</option>
                <option value="R005">R-005: Clientes exportados/importados do Sinacor</option>
                <option value="R006">R-006: Clientes que realizaram o Suitability (Pessoa Física)</option>
                <option value="R007">R-007: Clientes suspeitos</option>
                <asp:literal runat="server" id="litRelAssessor">
                    <option value="R008">R-008: Emails disparados por período</option>
                    <option value="R009">R-009: Clientes que não realizaram o primeiro acesso</option>
                    <option value="R010">R-010: Clientes inativos (Exportados)</option>
                    <option value="R011">R-011: Clientes Direct</option>
                </asp:literal>
                <option value="R012">R-012: Clientes Migrados x Corretagem Anual</option>
                <option value="R013">R-013: Clientes por distribuição regional</option>
                <option value="R014">R-014: Posição Consolidada Por Ativo</option>
                <option value="R015">R-015: Venda de Produtos</option>
                <option value="R016">R-016: Clientes Poupe Direct Gradual</option>
                <option value="R017">R-017: Nota de Corretagem por Assessor</option>
                <option value="R018">R-018: Conta Corrente por Assessor</option>
                <option value="R019">R-019: Custódia por Assessor</option>
                <option value="R020">R-020: Total de Clientes Cadastrados por Assessor e Período</option>
                <option value="R021">R-021: Proventos de Clientes </option>
                <option value="R022">R-022: Papel por Cliente </option>
                <%--<option value="R023">R-023: Rebate de Fundos </option>--%>
                <option value="R024">R-024: Clientes por assessor</option>
                <option value="R025">R-025: Consolidado renda fixa por assessor</option>
                <option value="R026">R-026: Posição de Custódia e Financeiro</option>
                <option value="R027">R-027: Conversão de contas Plural x Gradual</option>
                <option value="R028">R-028: Clientes em período de renovação cadastral (Câmbio)</option>
                <option value="R029">R-029: Monitoramento de TED's</option>
            </select>
        </p>


        <!-- Linhas Variaveis -->

        <p class="R001 R002 R003 R004 R005 R006 R007 R008 R009 R011 R015 R016 R017 R018 R020 R021 R022 R023 R026 R028 R029" style="width: 17em; display: none;">
            <label for="txtClientes_FiltroRelatorio_DataInicial">Data Inicial:</label>
            <input  id="txtClientes_FiltroRelatorio_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
        </p>

        <p class="R001 R002 R003 R004 R005 R006 R007 R008 R009 R011 R015 R016 R017 R018 R020 R021 R022 R023 R026 R028 R029" style="width: 17em; display: none">
            <label for="txtClientes_FiltroRelatorio_DataFinal" style="width:6em;">Data Final:</label>
            <input  id="txtClientes_FiltroRelatorio_DataFinal" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" />
        </p>

        <p class="R025" style="width: 17em;  display: none;">
            <label for="cmbBusca_FiltroRelatorio_Vencimento_RendaFixa">Vencimento:</label>
            <select id="cmbBusca_FiltroRelatorio_Vencimento_RendaFixa" propriedade="VencimentoRendaFixa" style="width: 168px;" >
                <option value="0">[Todos]</option>
                <option value="3">[Próximos 3 dias]</option>
                <option value="7">[Próximos 7 dias]</option>
                <option value="15">[Próximos 15 dias]</option>
                <option value="30">[Próximos 30 dias]</option>
                <option value="60">[Próximos 60 dias]</option>
                <option value="90">[Próximos 90 dias]</option>
                <option value="120">[Próximos 120 dias]</option>
                <option value="150">[Próximos 150 dias]</option>
                <option value="180">[Próximos 180 dias]</option>
            </select>
        </p>

        <p class="R001 R002 R003 R004 R005 R006 R007 R009 R010 R014 R023 R024 R025 R028" style="width: 17em; float: left; display: none;">
            <label for="cmbBusca_FiltroRelatorio_Assessor">Assessor:</label>
            <select id="cmbBusca_FiltroRelatorio_Assessor" propriedade="Assessor" style="width: 250px;" onchange="GradIntra_Relatorios_Gerais_Load(this);">
                <option value="">[Todos]</option>
                <asp:repeater id="rptRisco_FiltroRelatorio_Assessor" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p class="R018" style="width: 0; float: left; display: none;">
            <label for="cmbRelatorio018_Assessores"></label>
            <select id="cmbRelatorio018_Assessores" propriedade="Assessor" style="width: 175px;" onchange="Carregar_FiltroClientesPorAssessor(this);">
                <%--<option value="">[Todos]</option>--%>
                <asp:repeater id="rptRelatorio018_Assessores" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p class="R008" style="display:none; width:19em">
            <label for="cboClientes_FiltroRelatorio_TipoEmailDisparado">Tipo E-mail:</label>
            <select id="cboClientes_FiltroRelatorio_TipoEmailDisparado" style="width:100px" Propriedade="TipoEmail">
                <option value="">[ Todas ]</option>
                <option value="1">Assessor</option>
                <option value="2">Compliance</option>
            </select>
        </p>

        <!-- Adicionado aqui para que seja exibido ao lado do campo Tipo E-mail -->
        <p class="R008" style="display:none; width:25em" >
            <label for="txtClientes_FiltroRelatorio_CodCliente_R008">Cod. Cliente:</label>
            <input  id="txtClientes_FiltroRelatorio_CodCliente_R008" type="text" propriedade="CodCliente_R008" class="ProibirLetras" style="width:5em" />
        </p>

        <p class="R008" style="display:none; clear: left; width: 34.6em">
            <label for="txtClientes_FiltroRelatorio_Email_Disparado">E-Mail:</label>
            <input  id="txtClientes_FiltroRelatorio_Email_Disparado" type="text"  Propriedade="EmailDisparado" style="width:150px" />
        </p>

        <p class="R002 " style="display:none; ">
            <label for="cboClientes_FiltroRelatorio_TipoDePendencia" style="width:7em;">Tipo:</label>
            <select id="cboClientes_FiltroRelatorio_TipoDePendencia" Propriedade="TipoDePendencia" style="width:10em">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_TipoDePendencia" runat="server">
                <ItemTemplate>
                    <option value='<%# Eval("IdTipoPendencia") %>'><%# Eval("DsPendencia")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R002 R003" style="display:none;">
            <label for="cboClientes_FiltroRelatorio_Resolvida" style="width:7em;">Resolvida:</label>
            <select id="cboClientes_FiltroRelatorio_Resolvida" Propriedade="PendenciaResolvida">
                <option value="">[ Todos ]</option>
                <option value="1">Sim</option>
                <option value="0">Não</option>
            </select>
        </p>
        

        
        <p class="R006" style="display:none;">
        <!--
            <label for="cboClientes_FiltroRelatorio_PendenciaResolvida" style="width:7em;">Realizado:</label>
            <select id="cboClientes_FiltroRelatorio_PendenciaResolvida" Propriedade="Realizado">
                <option value="1">Sim</option>
                <option value="0" selected="selected">Não</option>
            </select>
        -->
        </p>
        


        <p class="R001 R003 R005 R006 R008 R010" style="display:none;">
            <label for="txtClientes_FiltroRelatorio_CpfCnpj">CPF/CNPJ:</label>
            <input  id="txtClientes_FiltroRelatorio_CpfCnpj" type="text" maxlength="14"  Propriedade="CpfCnpj" style="width: 7.5em;" />
        </p>

        <!-- Este combo recebe tratamento especial de filtro de atividades cadastradas na BlackList, OBSERVE a restrição antes de usá-lo em outro relatório -->
        <p class="R007 " style="display:none;  width: 375px;">
            <label for="cboClientes_FiltroRelatorio_AtividadeIlicita">Atividade:</label>
            <select id="Select1" style="width:20.2em" Propriedade="AtividadeIlicita">
                <option value="">[ Todas ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_AtividadeIlicita" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("IdAtividadeIlicita") %>'><%# Eval("CdAtividade")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <!-- Este combo recebe tratamento especial de filtro de países cadastrados na BlackList, OBSERVE a restrição antes de usá-lo em outro relatório -->
        <p class="R007 " style="display:none; width:34.6em">
            <label for="cboClientes_FiltroRelatorio_Pais" style="width:7em">País:</label>
            <select id="cboClientes_FiltroRelatorio_Pais" style="width:11em" Propriedade="Pais">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_Pais" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("CdPais") %>'><%# Eval("DsNomePais")%></option>
                </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R001 R002 R003 R004 R005 R007 R008 R009 R010 R028" style="display:none">
            <label for="cboClientes_FiltroRelatorio_TipoPessoa" style="width:7em">Tipo pessoa:</label>
            <select id="cboClientes_FiltroRelatorio_TipoPessoa" style="width:100px" Propriedade="TipoPessoa">
                <option value="">[ Todas ]</option>
                <option value="F">Física</option>
                <option value="J">Jurídica</option>
            </select>
        </p>

        <p class="R006 R011 R015 R016 R021 R022 R026 R029" style="display:none">
            <label for="txtClientes_FiltroRelatorio_CodCliente">Cod. Cliente:</label>
            <input  id="txtClientes_FiltroRelatorio_CodCliente" type="text" Propriedade="CodCliente" class="ProibirLetras" style="width:5em" />
        </p>

        <p class="R027" style="display:none; width:100%;">
            <label for="txtCodigoGradual">Cod. Gradual:</label><input  id="txtCodigoGradual" type="text" Propriedade="CodigoGradual" class="ProibirLetras" style="width:5em" />
            <label for="txtCodigoExterno">Cod. Externo:</label><input  id="txtCodigoExterno" type="text" Propriedade="CodigoExterno" class="ProibirLetras" style="width:5em" />
        </p>

        <p class="R011 R015 " style="display:none;  width: 375px;">
            <label for="cboClientes_FiltroRelatorio_Produtos">Produtos:</label>
            <select id="cboClientes_FiltroRelatorio_Produtos" style="width:20.2em" Propriedade="Produtos">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_Produtos" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("IdProduto") %>'><%# Eval("DsProduto")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R016 " style="display:none;  width: 375px;">
            <label for="cboClientes_FiltroRelatorio_ProdutosPoupe">Produtos:</label>
            <select id="Select2" style="width:20.2em" Propriedade="ProdutosPoupe">
                <option value="">[ Todos ]</option>
                <asp:Repeater id="rptClientes_FiltroRelatorio_ProdutosPoupe" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("IdProduto") %>'><%# Eval("DsProduto")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R011" style="display:none;  width: 375px;">
            <label for="cboClientes_FiltroRelatorio_Passo">Clientes:</label>
            <select id="cboClientes_FiltroRelatorio_Passo" style="width:20.2em" Propriedade="ClientePasso">
                <option value="1">Clientes com Ficha Cadastral</option>
                <option value="2">Clientes Visitante</option>
                <option value="3">Clientes de outros assessores</option>
                <option value="4">Clientes já exportados p/ Sinacor</option>
            </select>
        </p>

        <p class="R014 R022" style="display:none;">
            <label for="txtClientes_FiltroRelatorio_Papel">Papel:</label>
            <input  id="txtClientes_FiltroRelatorio_Papel" type="text" maxlength="8" Propriedade="Papel" style="width: 5.5em; text-transform:uppercase" />
        </p>

        <p class="R006" style="display: block; width: 876px; text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R006')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>
        
        <p class="R008" style="display: block; width: 150px; text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R008')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>

        <p class="R011" style="display:none;  width: 185px;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R011')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>
        <p class="R012" style="display: block; width: 876px;text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R012')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>
        <p class="R013" style="display: block; width: 876px;text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R013')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>

        <p class="R015" style="display: block; width: 530px;text-align:right;">
             <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R015')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>
        
        <p class="R027" style="display: none; width: 750px; text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R027')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>

        <p class="R029" style="display: none; width: 750px; text-align:right;">
            <button class="btnBusca_" onclick="return btnClientes_Relatorios_Direct_Click('R029')" style="width:auto;margin-left:8px;">Exportar para Excel</button>
        </p>
        
        <p class="R017 R019" id="FiltroAssessor" style="width: 85em; display:none; margin-bottom: 65px; margin-left: 31px;">
            <label for="cmbClientes_FiltroAssessor" style="width: 58px; margin-left: -9px;">Consultar:</label>
            <select id="cmbClientes_FiltroAssessor" size="5" propriedade="cliente" style="width: 45.75em;">
                <option value="" selected="selected" style="margin-top: 1px;">Selecionar Todos os Clientes</option>
                <asp:Repeater id="rptClientes_FiltroAssessor" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("codigo") %>'><%# Eval("nome")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>
        
        <p class="R018" id="P1" style="width: 85em; display:none; margin-bottom: 65px; margin-left: 31px;">
            <label for="lstRelatorio018_ClientesAssessor" style="width: 58px; margin-left: -9px;">Consultar:</label>
            <select multiple id="lstRelatorio018_ClientesAssessor" propriedade="cliente" size="5" style="width: 45.75em;">
                <%--<option value="" selected="selected" style="margin-top: 1px;">Selecionar Todos os Clientes</option>--%>
                <asp:Repeater id="rptRelatorio018_ClientesAssessor" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("codigo") %>'><%# Eval("nome")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>

        <p class="R017 " style="display: none; text-align: right; margin-top: 5px; width: 20em;">
            <label style="margin-left: -11px; width: 8em;">Tipo Mercado:</label>

            <select id="cmbNotaDeCorretagemTipoMercado" size="1" propriedade="NotaDeCorretagemTipoMercado" style="width: 12em;">
                <option value="VIS">À Vista</option>
                <option value="OPC">Opção</option>
            </select>
        </p>

        <p class="R018 " style="display: none; width: 80%; text-align: right; margin-top: 5px;">
            <label style="margin-left: -11px; width: 8em;">Tipo Extrato:</label>

            <input type="radio" name="TipoMercado" propriedade="TipoMercado" value="liq" id="rdbRelatorio_Cliente_NotaCorretagem_TipoMercado_Liquidacao" style="margin-top: 6px;" checked="true" runat="server" />
            <label id="Label1" for="rdbRelatorio_Cliente_NotaCorretagem_TipoMercado_Liquidacao" runat="server">Liquidação</label>

            <input type="radio" name="TipoMercado" propriedade="TipoMercado" value="mov" id="rdbRelatorio_Cliente_NotaCorretagem_TipoMercado_Movimento" style="margin-top: 6px;" runat="server" />
            <label id="Label2" for="rdbRelatorio_Cliente_NotaCorretagem_TipoMercado_Movimento" runat="server">Movimento</label>
        </p>

        <p class="R019 " style="display:none; width:25em">
            <label for="cboClientes_FiltroRelatorio_Custodia_Bolsa">Bolsa:</label>
            <select id="cboClientes_FiltroRelatorio_Custodia_Bolsa" style="width:100px" Propriedade="Bolsa">
                <option value="bov">Bovespa</option>
                <option value="bmf">BM&F</option>
            </select>
        </p>

        <p class="R019 " style="display:none; width:25em">
            <label for="cboClientes_FiltroRelatorio_Custodia_Mercado">Mercado:</label>
            <select id="cboClientes_FiltroRelatorio_Custodia_Mercado" style="width:100px" onchange="return cboClientes_FiltroRelatorio_Custodia_Mercado_OnChange(this);" Propriedade="CustodiaMercado">
                <option value="">[ Todos ]</option>
                <option value="VIS">À vista</option>
                <option value="BTC">BTC</option>
                <option value="FUT">Futuro</option>
                <option value="OPC">Opção</option>
                <option value="TER">Termo</option>
            </select>
        </p>

        <p class="" id="divClientes_FiltroRelatorio_Custodia_DataVencimentoTermo" style="width: 55em; display: none;">
            <label for="txtClientes_FiltroRelatorio_Custodia_DataVencimentoTermo">Vencimento do Termo:</label>
            <input  id="txtClientes_FiltroRelatorio_Custodia_DataVencimentoTermo" type="text" maxlength="10" Propriedade="DataVencimentoTermo" class="Mascara_Data Picker_Data" />
        </p>
        
        <!--
        <p class="R023 " style="width: 85em; display:none; margin-bottom: 5px; margin-left: 31px;">
            <label for="cmbClientes_FiltroIndice" style="width: 58px; margin-left: -9px;">ÍNDICE:</label>
            <select id="cmbClientes_FiltroIndice" propriedade="CodigoIndice" style="width: 45.75em;">
                <option value="IBOVESPA">IBOVESPA               </option>
                <option value="CDI">CDI                         </option>
                <option value="DÓLAR">DÓLAR                     </option>
                <option value="DÓLAR-COMPRA">DÓLAR-COMPRA       </option>
                <option value="EURO">EURO                       </option>
                <option value="IBOVESPA MÉDIO">IBOVESPA MÉDIO   </option>
                <option value="IBRA">IBRA                       </option>
                <option value="IBRX">IBRX                       </option>
                <option value="IBRX MÉDIO">IBRX MÉDIO           </option>
                <option value="IBRX50">IBRX50                   </option>
                <option value="IBRX50 MÉDIO">IBRX50 MÉDIO       </option>
                <option value="IGPM">IGPM                       </option>
                <option value="IMA_B">IMA_B                     </option>
                <option value="IMA_B 5">IMA_B 5                 </option>
                <option value="IMA_B 5+">IMA_B 5+               </option>
                <option value="IMA_C">IMA_C                     </option>
                <option value="IMA_GERAL">IMA_GERAL             </option>
                <option value="IMA_S">IMA_S                     </option>
                <option value="INPC">INPC                       </option>
                <option value="IPCA">IPCA                       </option>
                <option value="IRFM">IRFM                       </option>
                <option value="IRFM 1">IRFM 1                   </option>
                <option value="IRFM 1+">IRFM 1+                 </option>
                <option value="OURO">OURO                       </option>
                <option value="OURO MÉDIO">OURO MÉDIO           </option>
                <option value="PTAX REF">PTAX REF               </option>
                <option value="PTAX REF 2 DIAS">PTAX REF 2 DIAS </option>
                <option value="SELIC">SELIC                     </option>
                <option value="TR">TR                           </option>
            </select>
        </p>
        -->
        <!--  /  Linhas Variaveis -->
        
        <p style="margin-right: 6px; text-align: right; float: right; display: none;">
            <button class="btnBusca" onclick="return btnClientes_Relatorios_FiltrarRelatorio_Click(this)">Buscar</button>
            <button class="btnBusca" onclick="btnClientes_Relatorios_ImprimirRelatorio_Click(this);return false;" id="btnClienteRelatorioImprimir" style="width:auto;margin-left:8px;" disabled="disabled">Imprimir/PDF</button>
            <img id="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao" alt="" src="../Skin/Default/Img/Ico_Ajuda.gif" onclick="btnClientes_Relatorios_ExibirMensagemConfigurarImpressao_Click(this);" style="margin:3px 0px 0px 5px;float:right;cursor:pointer;">
        </p>

        <div id="divClientes_Relatorios_TextoConfigurarImpressao" class="PopUpDeAjuda" style="display:none; left: 684px; top: 200px; width: 280px;">
            <div>
                <div style="text-align: center;width: auto; text-decoration: underline; margin-bottom: 1em;">Instruções de impressão</div>
                <div style="text-align: justify; margin-top: 5px;">Para imprimir este relatório sem cabeçalho e rodapé padrão do navegador siga os seguintes passos:</div>
                <div>
                <ol style="margin:15px 0px 0px 15px;list-style-type:decimal">
                    <li style="list-style-type: decimal; margin-bottom: 1em">No menu superior do navagador clique em 'Arquivo' (<span style="text-decoration:underline">caso não esteja aparecendo, pressione primeiramente a tecla 'Alt' do seu teclado e aguarde o menu aparecer</span>);</li>
                    <li style="list-style-type: decimal; margin-bottom: 1em">Clique em 'Configurar página';</li>
                    <li style="list-style-type: decimal; margin-bottom: 1em">Selecione o item 'Paisagem' em Orientação de impressão;</li>
                    <li style="list-style-type: decimal; margin-bottom: 1em">Exclua o conteúdo dos campos 'Cabeçalho' e 'Rodapé';</li>
                    <li style="list-style-type: decimal; margin-bottom: 1em">Clique em 'OK'.</li>
                    <li style="list-style-type: decimal; margin-bottom: 1em">Imprima novamente.</li>
                </ol>
                </div>
                <div style="margin-top:15px;">* Estas instruções são compatíveis com a versão 8.0 do MS Internet Explorer.</div>
            </div>
        </div>

    </div>

    <script>
        $.ready(GradIntra_Relatorios_Gerais_Load(this));
    </script>
</form>