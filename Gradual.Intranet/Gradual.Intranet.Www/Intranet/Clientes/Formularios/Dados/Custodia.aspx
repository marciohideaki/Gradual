<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Custodia.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.Custodia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <form id="form1" runat="server">
    <h4>Custódia</h4>
    <table cellpadding="0" cellspacing="0" border="0" class="GridIntranet" style="width:98%">
        <thead>
            <tr>
                <td class="GradGrid_Filter" align="center">Tipo de Mercado</td>
                <td class="GradGrid_Filter" align="center">Código</td>
                <td class="GradGrid_Filter" align="center">Empresa</td>
                <td class="GradGrid_Filter" align="center">Carteira</td>
                <td class="GradGrid_Ignore" align="center">Qt. Abertura</td>
                <td class="GradGrid_Ignore" align="center">Qtd. Compra</td>
                <td class="GradGrid_Ignore" align="center">Qtd. Venda</td>
                <td class="GradGrid_Ignore" align="center">Quantidade Atual</td>
                <td class="GradGrid_Ignore" align="center">F. Anterior</td>
                <td class="GradGrid_Ignore" align="center">Cotação</td>
                <td class="GradGrid_Ignore" align="center">Variação %</td>
                <td class="GradGrid_Ignore" align="center">Resultado</td>
            </tr>
        </thead>
        <tbody>
                 <tr class="Template" style="display:none;">
                    <td Propriedade="TipoMercado"></td>
                    <td Propriedade="CodigoNegocio"></td>
                    <td Propriedade="Empresa"></td>
                    <td Propriedade="Carteira"></td>
                    <td Propriedade="QtdAbertura"       align="right"></td>
                    <td Propriedade="QtdCompra"         align="right"></td>
                    <td Propriedade="QtdVenda"          align="right"></td>
                    <td Propriedade="QtdAtual"          align="right"></td>
                    <td Propriedade="ValorDeFechamento" align="right"></td>
                    <td Propriedade="Cotacao"           align="right"></td>
                    <td Propriedade="Variacao"          align="right"></td>
                    <td Propriedade="Resultado"         align="right"></td>
                </tr>
            <asp:Repeater id="rptCustodia" runat="server">
            <ItemTemplate>
                <tr class="trB">
                    <td Propriedade="TipoMercado">                     <%# DataBinder.Eval(Container.DataItem, "TipoMercado")      %></td>
                    <td Propriedade="CodigoNegocio">                   <%# DataBinder.Eval(Container.DataItem, "CodigoNegocio")    %></td>
                    <td Propriedade="Empresa">                         <%# DataBinder.Eval(Container.DataItem, "Empresa")          %></td>
                    <td Propriedade="Carteira">                        <%# DataBinder.Eval(Container.DataItem, "Carteira")         %></td>
                    <td Propriedade="QtdAbertura"       align="right"> <%# AbreviarNumero( DataBinder.Eval(Container.DataItem, "QtdAbertura") )     %></td>
                    <td Propriedade="QtdCompra"         align="right"> <%# AbreviarNumero( DataBinder.Eval(Container.DataItem, "QtdCompra")   )     %></td>
                    <td Propriedade="QtdVenda"          align="right"> <%# AbreviarNumero( DataBinder.Eval(Container.DataItem, "QtdVenda")    )     %></td>
                    <td Propriedade="QtdAtual"          align="right"> <%# AbreviarNumero( DataBinder.Eval(Container.DataItem, "QtdAtual")    )     %></td>
                    <td Propriedade="ValorDeFechamento" align="right"> <%# DataBinder.Eval(Container.DataItem, "ValorDeFechamento")%></td>
                    <td Propriedade="Cotacao"           align="right"> <%# DataBinder.Eval(Container.DataItem, "Cotacao")          %></td>
                    <td Propriedade="Variacao"          align="right"> <%# DataBinder.Eval(Container.DataItem, "Variacao")         %></td>
                    <td Propriedade="Resultado"         align="right"> <%# DataBinder.Eval(Container.DataItem, "Resultado")        %></td>  
                </tr>
            </ItemTemplate>
            </asp:Repeater>
        </tbody>
        </table> 

        <p class="BotoesSubmit" style="margin-top:4em;">
            <button onclick="return btnCustodia_AtualizarCustodia_Click(this);" class="BotaoPadrao">Atualizar</button>
        </p>

    </form>

