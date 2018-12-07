<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcompanhamentoOrdens.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Operacoes.AcompanhamentoOrdens" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/AbasOperacoes.ascx" tagname="AbasOperacoes" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="PaginaConteudo">
<div class="row">
    <div class="col2">
        <uc1:AbasOperacoes ID="AbasOperacoes1" Modo="menu" runat="server" />
    </div>
</div>
                
<div class="row">
    <div class="col1">
        <div class="menu-exportar clear">
            <h3>Acompanhamento de Ordens <span class="icon3">(de <%=DateTime.Now.ToString("dd/MM/yyyy") %> às <%=DateTime.Now.ToString("HH:mm") %>)</span></h3>
            <ul>
                <li class="conta">
                    <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                    <div class="conta_msg" style="display:none">
                    Confira os dados bancários da Gradual:<br />
                    Banco BM&F - 096<br />
                    Agência: 001<br />
                    C/C: 326-5<br />
                    Favorecido: Gradual CCTVM S/A<br />
                    CNPJ: 33.918.160/0001-73
                    </div>
                </li>
                <li class="interrogacao"><input type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')" /><div class="ajuda">Para consultar o extrato das suas negociações na Gradual, você pode buscar pelo período de 7, 15, 30 dias, ou selecionar as datas de início e fim, conforme desejar.</div></li>
                <li class="exportar">
                    <input title="Exportar" type="button" />
                    <ul>
                        <li><asp:Button ID="btnImprimirExtrato" runat="server" CssClass="pdf" Text="PDF" OnClick="btnImprimirPDF_Click" /></li>
                        <li><asp:Button ID="Button1" runat="server" CssClass="excel" Text="Excel" OnClick="btnImprimirExcel_Click" /></li>
                    </ul>
                </li>
                <li class="email"><asp:Button ID="btnEnviarEmail" runat="server" OnClick="btnEnviarEmail_Click" title="Enviar por e-mail" /></li>
            </ul>
        </div>
    </div>
</div>
                
<div class="row">
    <div class="col1">
        <div >
            <ul class="abas-menu">
                <li class="ativo" data-IdConteudo="OrdensOnline"><a href="#" id="li_AbaOrdensOnline"> Ordens Online</a></li>
                <li data-IdConteudo="OrdensHistorico"><a href="#" id="li_AbaOrdensHistorico"> Ordens em Histórico</a></li>
            </ul>
                            
            <div class="abas-conteudo">
                <div class="aba" data-IdConteudo="OrdensOnline" >
                                    
                    <div class="form-consulta form-padrao clear">
                        <div class="row">
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Papel</label>
                                    <asp:textbox id="txtAtivo" runat="server"></asp:textbox>
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Direção</label>
                                    <asp:dropdownlist id="cboDirecao" runat="server">
                                        <asp:listitem value="">(Todas)</asp:listitem>
                                        <asp:listitem value="compra">Compra</asp:listitem>
                                        <asp:listitem value="venda">Venda</asp:listitem>
                                    </asp:dropdownlist>
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Situação</label>
                                    <div class="clear">
                                        <asp:dropdownlist id="cboSituacao" runat="server">
                                            <asp:listitem value="">(Todas)</asp:listitem>
                                            <asp:listitem value="NOVA">Nova</asp:listitem>
                                            <asp:listitem value="PARCIALMENTEEXECUTADA">Parcialmente Executada</asp:listitem>
                                            <asp:listitem value="EXECUTADA">Executada</asp:listitem>
                                            <asp:listitem value="CANCELADA">Cancelada</asp:listitem>
                                            <asp:listitem value="SUBSTITUIDA">Substituida</asp:listitem>
                                            <asp:listitem value="REJEITADA">Rejeitada</asp:listitem>
                                            <asp:listitem value="SUSPENSA">Suspensa</asp:listitem>
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                            </div>
                        </div>
                                        
                        <div class="row">
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Hora de</label>
                                    <asp:textbox id="txtHoraDe"  runat="server" maxlength="10" class="DatePicker"></asp:textbox>
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Hora Até</label>
                                    <asp:textbox id="txtHoraAte" runat="server" maxlength="10" class="DatePicker"></asp:textbox>
                                </div>
                            </div>
                        </div>
                                        
                        <div class="row">
                            <div class="col3">
                                <asp:button id="btnBuscar" runat="server" text="Buscar" 
                                    cssclass="botao btn-padrao btn-erica btn-buscar" onclick="btnBuscar_Click" OnClientClick="btnAcompanhamentoHistorico_Click(this,'AbaOrdensOnline')"/>
                                <%--<input class="botao btn-padrao btn-erica" type="submit" value="Consultar" />--%>
                            </div>
                        </div>
                    </div>
                    <div style="overflow-y: scroll; height: 400px">                
                    <table class="tabela tabela-type-small" style="margin-top: 0px; width:900px;" >
                        <tr class="tabela-titulo">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td><asp:linkbutton id="lnkHeader_Ordem"          runat="server" onclick="lnkHeader_Click">Ordem        </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Ativo"          runat="server" onclick="lnkHeader_Click">Ativo        </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Direcao"        runat="server" onclick="lnkHeader_Click">Direção      </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Solicitado"     runat="server" onclick="lnkHeader_Click">Solic.       </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Executado"      runat="server" onclick="lnkHeader_Click">Exec.        </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Preco"          runat="server" onclick="lnkHeader_Click">Preço        </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_PrecoStopStart" runat="server" onclick="lnkHeader_Click">Pç Stop/Start</asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Situacao"       runat="server" onclick="lnkHeader_Click">Situação     </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Envio"          runat="server" onclick="lnkHeader_Click">Envio        </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Ultima"         runat="server" onclick="lnkHeader_Click">Últ. Atual.  </asp:linkbutton></td>
                            <td><asp:linkbutton id="lnkHeader_Validade"       runat="server" onclick="lnkHeader_Click">Validade     </asp:linkbutton></td>
                        </tr>
                        <asp:Repeater ID="rptOrdens" runat="server" OnItemDataBound="rptOrdens_ItemDataBound">
                        <ItemTemplate>
                        <%--<tr runat="server" id="tr_OrdemOnline" class="tabela-type-very-small">--%>
                        <tr runat="server" id="tr_OrdemOnline" class="tabela-type-very-small">
                            <td>
                                <button onclick="return AcompanhamentoDeOrdem_AbrirFecharHistorico(this)" class="IconButton IconExpandColap"></button>
                            </td>
                            <td><asp:literal id="lblBotaoCancelar" runat="server"></asp:literal></td>
                            <td><asp:Button id="btnBotaoAlterar" runat="server" Visible="false" OnCommand="btnBotaoAlterar_Click" cssClass="IconAlterar" ToolTip="Alterar Ordem" /></td>
                            <td><%# Eval("ClOrdID")%></td>
                            <td><%# Eval("Symbol")%></td>
                            <td><%# Eval("Side")%></td>
                            <td><%# Eval("OrderQty")%></td>
                            <td><%# Eval("CumQty")%></td>
                            <td><%# Eval("Price", "{0:n2}")%></td>
                            <td><%# Eval("StopPrice", "{0:n2}")%></td>
                            <td runat="server" id="td_ordemonline_status"><%# Eval("OrdStatus")%> </td>
                            <td><%# Eval("RegisterTime")%></td>
                            <td><%# Eval("TransactTime")%></td>
                            <td><%# Eval("ExpireDate","{0:dd/MM/yyyy}")%></td>
                        </tr>
                        <tr class="Historico" style="display:none">
                            <td colspan="14">
                                <table cellspacing="0" class="AcompanhamentoDeOrdem_Historico tabela-type-very-small"
                                    style="margin: 0px 0px 0px 0px">
                                    <thead>
                                        <tr>
                                            <td>Qtd Solicitada</td>
                                            <td>Qtd Remanescente</td>
                                            <td>Qtd Executada</td>
                                            <td>Situação</td>
                                            <td>Data</td>
                                            <td>Descrição</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    
                                        <asp:repeater id="rptAcompanhamentos" runat="server">
                                        <itemtemplate>
                                    
                                            <tr class="<%# Container.ItemIndex % 2 == 0 ? "trA" : "trB" %>">
                                                <td style="text-align:center">  <%# Eval("QuantidadeSolicitada")%>    </td>
                                                <td style="text-align:center">  <%# Eval("QuantidadeRemanescente")%>  </td>
                                                <td style="text-align:center">  <%# Eval("QuantidadeExecutada")%>     </td>
                                                <td>  <%# Eval("StatusOrdem")%>             </td>
                                                <td style="text-align:center" nowrap>  <%# Eval("DataAtualizacao")%>         </td>
                                                <td>  <%# Eval("Descricao")%>               </td>
                                            </tr>
                                    
                                        </itemtemplate>
                                        </asp:repeater>

                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        </ItemTemplate>
                        </asp:Repeater>

                        <tr id="trNenhumItemOrdens" runat="server">
                            <td colspan="12"> Nenhum item selecionado</td>
                        </tr>
                    </table>
                    </div>            
                </div>
                                
                <div class="aba" data-IdConteudo="OrdensHistorico">
                                    
                    <div class="form-consulta form-padrao clear">
                        <div class="row">
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Papel</label>
                                    <asp:TextBox ID="txtAtivoHist" runat="server" name="input-padrao" />
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Direção</label>
                                    <asp:dropdownlist id="cboDirecaoHist" runat="server">
                                        <asp:listitem value="">(Todas)</asp:listitem>
                                        <asp:listitem value="compra">Compra</asp:listitem>
                                        <asp:listitem value="venda">Venda</asp:listitem>
                                    </asp:dropdownlist>
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Situação</label>
                                    <div class="clear">
                                    <asp:dropdownlist id="cboSituacaoHist" runat="server">
                                            <asp:listitem value="">(Todas)</asp:listitem>
                                            <asp:listitem value="NOVA">Nova</asp:listitem>
                                            <asp:listitem value="PARCIALMENTEEXECUTADA">Parcialmente Executada</asp:listitem>
                                            <asp:listitem value="EXECUTADA">Executada</asp:listitem>
                                            <asp:listitem value="CANCELADA">Cancelada</asp:listitem>
                                            <asp:listitem value="SUBSTITUIDA">Substituida</asp:listitem>
                                            <asp:listitem value="REJEITADA">Rejeitada</asp:listitem>
                                            <asp:listitem value="SUSPENSA">Suspensa</asp:listitem>
                                        </asp:dropdownlist>
                                    
                                    </div>
                                </div>
                            </div>
                        </div>
                                        
                        <div class="row">
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Data de</label>
                                    <asp:TextBox  ID="txtDataDe" runat="server" class="calendario"  name="input-padrao"  />
                                </div>
                            </div>
                                            
                            <div class="col3">
                                <div class="campo-consulta">
                                    <label>Data Até</label>
                                    <asp:TextBox ID="txtDataAte" runat="server" Text=""  class="calendario"  name="input-padrao" />
                                </div>
                            </div>
                        </div>
                                        
                        <div class="row">
                            <div class="col3">
                                <asp:Button ID="btnConsultarHistorico" runat="server"  CssClass="botao btn-padrao btn-erica" Text="Consultar"
                                 onclick="btnConsultar_Click" OnClientClick="btnAcompanhamentoHistorico_Click(this,'AbaOrdensHistorico')" />
                            </div>
                        </div>
                    </div>
                    <input type="hidden" runat="server" id="Operacoes_Aba_selecionada" />                                    
                    <div style="overflow-y: scroll; height:400px">
                        <table class="tabela tabela-type-small" style="width:900px; margin-top:0px">
                            <tr class="tabela-titulo">
                                <td></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Ordem"          runat="server" onclick="lnkHeaderHist_Click">Ordem               </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Ativo"          runat="server" onclick="lnkHeaderHist_Click">Ativo               </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Direcao"        runat="server" onclick="lnkHeaderHist_Click">Direção             </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Solicitado"     runat="server" onclick="lnkHeaderHist_Click">Solic.              </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Executado"      runat="server" onclick="lnkHeaderHist_Click">Exec.               </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Preco"          runat="server" onclick="lnkHeaderHist_Click">Preço               </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_PrecoStopStart" runat="server" onclick="lnkHeaderHist_Click">Pç Stop/Start       </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Situacao"       runat="server" onclick="lnkHeaderHist_Click">Situação            </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Envio"          runat="server" onclick="lnkHeaderHist_Click">Envio               </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Ultima"         runat="server" onclick="lnkHeaderHist_Click">Última Atualização  </asp:linkbutton></td>
                                <td><asp:linkbutton id="lnkHeaderHist_Validade"       runat="server" onclick="lnkHeaderHist_Click">Validade           </asp:linkbutton></td> 
                                </tr> 
                                <asp:Repeater ID="rptOrdensHistorica" runat="server" OnItemDataBound ="rptOrdensHistorica_ItemDataBound">
                                <ItemTemplate>
                                <tr runat="server" id="tr_OrdemHistorica"  class="tabela-type-very-small">
                                    <td>
                                        <button onclick="return AcompanhamentoDeOrdem_AbrirFecharHistorico(this)" class="IconButton IconExpandColap"></button>
                                    </td>
                                    <td><%# Eval("ClOrdID")%></td>
                                    <td><%# Eval("Symbol")%></td>
                                    <td><%# Eval("Side")%></td>
                                    <td><%# Eval("OrderQty")%></td>
                                    <td><%# Eval("CumQty")%></td>
                                    <td><%# Eval("Price", "{0:n2}")%></td>
                                    <td><%# Eval("StopPrice", "{0:n2}")%></td>
                                    <td runat="server" id="td_ordemhistorico_status" ><%# Eval("OrdStatus")%></td>
                                    <td><%# Eval("RegisterTime")%></td>
                                    <td><%# Eval("TransactTime")%></td>
                                    <td><%# Eval("ExpireDate","{0:dd/MM/yyyy}")%></td>
                                </tr>
                                <tr class="Historico" style="display:none">
                                    <td colspan="14">
                                        <table cellspacing="0" class="AcompanhamentoDeOrdem_Historico tabela-type-very-small"
                                            style="margin: 0px 0px 0px 0px">
                                            <thead>
                                                <tr>
                                                    <td>Qtd Solicitada</td>
                                                    <td>Qtd Remanescente</td>
                                                    <td>Qtd Executada</td>
                                                    <td>Situação</td>
                                                    <td>Data</td>
                                                    <td>Descrição</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    
                                                <asp:repeater id="rptAcompanhamentosHistorico" runat="server">
                                                <itemtemplate>
                                    
                                                    <tr class="<%# Container.ItemIndex % 2 == 0 ? "trA" : "trB" %>">
                                                        <td style="text-align:center">  <%# Eval("QuantidadeSolicitada")%>    </td>
                                                        <td style="text-align:center">  <%# Eval("QuantidadeRemanescente")%>  </td>
                                                        <td style="text-align:center">  <%# Eval("QuantidadeExecutada")%>     </td>
                                                        <td>  <%# Eval("StatusOrdem")%>             </td>
                                                        <td style="text-align:center" nowrap>  <%# Eval("DataAtualizacao")%>         </td>
                                                        <td>  <%# Eval("Descricao")%>               </td>
                                                    </tr>
                                    
                                                </itemtemplate>
                                                </asp:repeater>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            </asp:Repeater> 
                            <tr id="trNenhumItemOrdensHistorico" runat="server"> 
                            <td colspan="11"> Nenhum item selecionado</td> 
                        </tr> 
                    </table>
                </div>
                </div>
            </div>
        </div>
    </div>
</div> 
</section> 
    </div>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Acompanhamento de Ordens" />

</asp:Content>

