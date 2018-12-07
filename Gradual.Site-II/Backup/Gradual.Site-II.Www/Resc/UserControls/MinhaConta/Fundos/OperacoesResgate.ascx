<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesResgate.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesResgate" %>
<h4>Resgate</h4>
                                    
<p>Para resgatar os seus recursos investidos em Fundos, confira a sua carteira abaixo e realize a retirada conforme desejar. </p>
                                    
<%--<div class="form-padrao clear">
    <div class="row">
        <div class="col4">
            <div class="campo-consulta">
                <label>Produto</label>
                <select ID="ddlListaFundos" runat="server">
                <option id="-1">TODOS</option>
                <asp:Repeater runat="server" ID="rptListaDeFiltroFundos">
                <itemtemplate>
                    <option id="<%# Eval("idProduto")%>"><%# Eval("Fundo")%></option> 
                </itemtemplate>
                </asp:Repeater> 
                </select>
            </div>
        </div>
                                            
        <div class="col2 colBot">
            <asp:Button type="submit" Text="Buscar" Id="btnBuscarFiltroOperacoesResgate" 
                runat="server" class="botao btn-padrao btn-erica" 
                onclick="btnBuscarFiltroOperacoesResgate_Click"  />
        </div>
    </div>
</div>--%>
                                    
<table class="tabela">
    <tr class="tabela-titulo">
        <td>Fundo</td>
        <td>Risco</td>
        <td>Vlr. Cota</td>
        <td>Qtd. Cotas</td>
        <td>Vlr. Bruto</td>
        <td>IR</td>
        <td>Vlr. Líq.</td>
        <td>Proc. Em</td>
        <td class="alignCenter">Resgatar</td>
    </tr>
<asp:Repeater runat="server" ID="rptListaDePosicaoConsolidada" 
        onitemdatabound="rptListaDePosicaoConsolidada_ItemDataBound"  >
    <itemtemplate>
        <tr class="tabela-type-small">
            <td><%# Eval("NomeFundo")%></td>
            <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde'" : "class='bolinha-amarelo'"  %> ></span></td>
            <td><%# Eval("ValorCota")%></td>
            <td><%# Eval("QtdCotas")%></td>
            <td><%# Eval("ValorBruto")%></td>
            <td><%# Eval("IR")%></td>
            <td><%# Eval("ValorLiquido")%></td>
            <td><%# Eval("DataAtualizacao")%></td>
            <td class="alignCenter"><asp:Button CssClass="botao btn-invista" ID="btnResgatar" runat="server" Text="Resgate" OnClick="btnResgatar_Click" /></td>
        </tr>
    </itemtemplate>
    </asp:Repeater> 
    <tr id="trNenhumPosicaoConsolidada" runat="server" visible="false">
        <td colspan="9">Nenhum item encontrado.</td>
    </tr>                                         
</table>