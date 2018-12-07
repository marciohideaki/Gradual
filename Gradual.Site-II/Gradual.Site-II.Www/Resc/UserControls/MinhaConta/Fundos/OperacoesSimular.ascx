<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesSimular.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesSimular" %>
<h4>Simular</h4>
<p>Conheça o nosso simulador de investimentos em Fundos e teste suas aplicações, antes mesmo de realizá-las. </p>
<form class="form-padrao">
    <div class="row">
        <div class="col3-2">
            <%--<div class="campo-consulta campo-produto clear">
                <input type="text" name="nome" value="Nome do Produto" />
                <input type="submit" value="Visualizar" name="enviar" class="botao btn-padrao btn-erica" />
            </div>--%>
                                                
            <ul class="lista lista-add" id="Simular_Produto_Lista_Add" style="height:300px; overflow:scroll">
                <asp:Repeater runat="server" ID="rptListaParaSimulacao">
                <itemtemplate>
                    <li id='fundo_<%# Eval("IdProduto") %>' onclick="Simular_Produto_Add('<%# Eval("IdProduto") %>', '<%# Eval("Fundo") %>');"><%# Eval("Fundo") %></li>
                </itemtemplate>
                </asp:Repeater>
                <%--
                <li id="fundo_1" onclick="Simular_Produto_Add('1', 'LEME IMA B FI RENDA FIXA PREVIDENCIARIO');">LEME IMA B FI RENDA FIXA PREVIDENCIARIO</li>
                <li id="fundo_2" onclick="Simular_Produto_Add('2', 'PIATA FI RF PREV CREDITO PRIVADO');">PIATA FI RF PREV CREDITO PRIVADO</li>
                <li id="fundo_3" onclick="Simular_Produto_Add('3', 'GRADIUS FI RENDA FIXA');">GRADIUS FI RENDA FIXA</li>
                <li id="fundo_4" onclick="Simular_Produto_Add('4', 'ITAU KEY RF FICFI');">ITAU KEY RF FICFI</li>
                <li id="fundo_5" onclick="Simular_Produto_Add('5', 'BRADESCO PRIVATE FIC DE FI RF EXECUTIVO');">BRADESCO PRIVATE FIC DE FI RF EXECUTIVO</li>
                <li id="fundo_6" onclick="Simular_Produto_Add('6', 'ITAU RENDA FIXA FI');">ITAU RENDA FIXA FI</li>
                --%>
            </ul>   
        </div>      
                                            
        <div class="col4">
            <p class="noMarginTop">Indexadores:</p>
                                                
            <ul class="lista-checkbox" >
                <li><label class="checkbox"><asp:CheckBox id="chkCDI"      runat="server" Text="CDI" /></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkDOLAR"    runat="server" Text="DOLAR" /></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkIBOVESPA" runat="server" Text="IBOVESPA" /> </label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkIBX"      runat="server" Text="IBX" /></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkIBA"     runat="server"  Text="IBA"/></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkIGPM"     runat="server" Text="IGPM"/></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkSELIC"    runat="server" Text="SELIC" /></label></li>
                <li><label class="checkbox"><asp:CheckBox id="chkEURO"     runat="server" Text="EURO" /> </label></li>
            </ul>
        </div>
    </div>
                                        
    <div class="row">
        <div class="col1">
            <p>Produtos adicionados</p>
                                                
            <div class="produtos-adicionados" >
                <ul class="lista lista-minus" id="produtos_adicionados">
                    
                </ul>
            </div>
            
            <input type="hidden" id="hddSelect_produtos_adicionados" runat="server" />
            <%--<asp:ListBox runat="server" SelectionMode="Multiple" ID="Select_produtos_adicionados" style="visibility:hidden" >
            </asp:ListBox>--%>
            <%--<select  style="visibility:hidden" id="Select_produtos_adicionados" runat="server"  multiple="multiple" >
            </select>--%>
        </div>
    </div>
</form>
                                    
<div class="form-padrao">
    <div class="row">
        <div class="col4">
            <div class="campo-consulta">
                <label>Período:</label>
                <asp:DropDownList runat="server" ID="PeriodoFiltroConsulta">
                    <asp:ListItem Value ="1" Text="12 Meses" />
                    <asp:ListItem Value ="2" Text="24 Meses" />
                    <asp:ListItem Value ="3" Text="36 Meses" />
                </asp:DropDownList>
            </div>
        </div>
                                            
        <div class="col4">
            <div class="campo-consulta">
                <label></label>
                <asp:DropDownList runat="server" ID="RentabilidadeFiltroConsulta">
                    <asp:ListItem Value ="Valor" Text="Valor" />
                    <asp:ListItem Value ="Rentabilidade" Text="Rentabilidade" />
                </asp:DropDownList>
            </div>
        </div>
                                            
        <div class="col4">
            <div class="campo-consulta">
                <label></label>
                <asp:TextBox ID="ValorFiltroConsulta" runat="server" Text="0,00" ></asp:TextBox>
            </div>
        </div>
                                            
        <div class="col4 colBot">
            <asp:Button ID="tbnFitroConsultaSimulacao" 
                CssClass="botao btn-padrao btn-erica"  runat="server" Text="Visualizar" 
                onclick="tbnFitroConsultaSimulacao_Click" OnClientClick="btnSimularFundos_Click(this,'AbaSimular');" />
        </div>
    </div>
</div>
<div style="visibility:hidden; height:0px" class="row">
    <table class="tabela_grafico_Simular"  style="visibility:collapse">
        <asp:Repeater runat="server" ID="rptListaDeValoresGrafico">
            <itemtemplate>
                <tr data-label="<%#Eval("NomeFundo") %>" data-valor="<%#Eval("Valor") %>">
                    <td> <%#Eval("NomeFundo") %>   </td>
                    <td class="grafico-valor"> R$ <%# Eval("Valor", "{0:N2}")%> </td>
                </tr>
            </itemtemplate>
        </asp:Repeater>
    </table>
</div> 
<div  id="chrtRentabilidadeSimular" style=" width: 900px; height:200px; margin-bottom:10px"></div>
<div id="chartLegend" style="margin-bottom: 20px"></div>  
<div style="overflow-x:scroll">
    <table class="tabela" style="width: 1200px">
        <tr class="tabela-titulo">
            <td>Cor</td>
            <td>Ativo</td>
            <td>Retorno(%)</td>
            <td>Vol(a.a)</td>
            <td>Sharpe</td>
            <td>Patrimonio</td>
            <td>%CDI</td>
            <td>Resgate(*)</td>
            <td>Início(%)</td>
            <td>Últ. 12(%)</td>
            <td>Acum.Ano(%)</td>
            <td>Mês Ant.(%)</td>
        </tr>
        <asp:Repeater runat="server" ID="rptListaDeSimulacao">
        <itemtemplate>                                
        <tr class="tabela-type-small">
            <td>000</td>
            <td><%# Eval("Ativo") %>        </td>
            <td><%# Eval("Retorno")%>       </td>
            <td><%# Eval("Volume")%>        </td>
            <td><%# Eval("Sharpe")%>        </td>
            <td><%# Eval("Patrimonio")%>    </td>
            <td><%# Eval("CDI")%>           </td>
            <td><%# Eval("Resgate")%>       </td>
            <td><%# Eval("Inicio")%>        </td>
            <td><%# Eval("Ultimo12Meses")%> </td>
            <td><%# Eval("AcumuladoAno")%>  </td>
            <td><%# Eval("MesAnterior")%>   </td>
        </tr>
    </itemtemplate>
    </asp:Repeater>
    <tr id="trNenhumSimularResultado" runat="server" visible="false">
        <td colspan="7">Nenhum item encontrado.</td>
    </tr>
    </table>
</div>
<script>    $('input[type="checkbox"], input[type="radio"]').iCheck();
    $("#ContentPlaceHolder1_OperacoesSimular1_ValorFiltroConsulta").maskMoney({ thousands: '.',
        decimal: ','
    });
</script>