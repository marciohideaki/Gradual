<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detalhes.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Fundos.Detalhes" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

<div id="pnlComDados" runat="server">

    <h2><%=this.NomeFundo %></h2>

    <div class="row">
        <div class="col3-2 saibamais-area">

            <asp:Literal ID="litConteudoCms" runat="server" ></asp:Literal>

            <p><strong>Perfil:</strong><br /> <img src="<%=this.PerfilImagem %>"  /></p>
                        
            <table class="tabela alignCenter" style="width:600px; font-size:11px">
                <tr class="tabela-titulo">
                    <td colspan="14">Rentabilidade <%=this.NomeFundo %></td>
                </tr>
                            
                <tr class="tabela-area">
                    <td>%</td>
                    <td>Jan</td>
                    <td>Fev</td>
                    <td>Mar</td>
                    <td>Abr</td>
                    <td>Mai</td>
                    <td>Jun</td>
                    <td>Jul</td>
                    <td>Ago</td>
                    <td>Set</td>
                    <td>Out</td>
                    <td>Nov</td>
                    <td>Dez</td>
                    <td>Ano</td>
                </tr>
                
                <asp:Repeater ID="rptRentabilidadeFundo" runat="server">
                <ItemTemplate>
                <tr>
                    <td><strong><%# Eval("Ano") %></strong></td>
                    <td><%# Eval("PercentualJan") %></td>
                    <td><%# Eval("PercentualFev") %></td>
                    <td><%# Eval("PercentualMar") %></td>
                    <td><%# Eval("PercentualAbr") %></td>
                    <td><%# Eval("PercentualMai") %></td>
                    <td><%# Eval("PercentualJun") %></td>
                    <td><%# Eval("PercentualJul") %></td>
                    <td><%# Eval("PercentualAgo") %></td>
                    <td><%# Eval("PercentualSet") %></td>
                    <td><%# Eval("PercentualOut") %></td>
                    <td><%# Eval("PercentualNov") %></td>
                    <td><%# Eval("PercentualDez") %></td>
                    <td><%# Eval("PercentualAno") %></td>
                </tr>
                </ItemTemplate>
                </asp:Repeater>
                <tr id="trNenhumRentabilidadeItem" runat="server">
                    <td colspan="14">Nenhuma rentabilidade encontrada</td>
                </tr>
            </table>
        </div>
                    
        <div class="col3 saibamais-barra">
            <h4 class="titulo-cinza">Aplicação</h4>
            <p><strong>Mínimo para 1ª aplicação:</strong><br />
            R$ <%=AplicacaoMinima%></p>

            <p><strong>Mínimo para aplicações adicionais:</strong><br />
            R$ <%=this.AplicacaoMinimaAdicional%></p> 
            
            <p><strong>Dias para conversão da aplicação:</strong><br />
            <%=this.DiasConversaoAplicacao%></p>
            <%=this.BotaoAplicar %>
                        
            <h4 class="titulo-cinza">Resgate</h4>
            <p><strong>Mínimo para resgatar:</strong><br />
            R$ <%=this.ResgateMinimo%></p>
            <p><strong>Saldo mínimo de permanência:</strong><br />
            R$ <%=this.SaldoMinimoPermanencia%></p> 
            <p><strong>Dias para conversão do resgate:</strong><br />
            <%=this.DiasConversaoResgate%></p>

            <!--p><strong>Dias para conversão do resgate antecipado:</strong><br />
            <%=this.DiasConversaoResgateAntecipado%></p--> 

            <p><strong>Dias para pagamento do resgate:</strong><br />
            <%--D+1 (dia útil) da data de conversão de resgate--%>
            <%=this.DiasPagamentoResgate%></p>

            <h4 class="titulo-cinza">Taxas</h4>

            <p><strong>Administração:</strong><br />
            <%=this.TaxaAdministracao%> %
            </p> 

            <!--p><strong>Administração máxima:</strong><br />
            <%=this.TaxaAdministracaoMaxima%></p--> 

            <p><strong>Performance:</strong><br />
            <%=this.TaxaPerformance%></p> 

            <!--p><strong>Resgate antecipado:</strong><br />
            <%=this.ResgateAntecipado%></p-->

            <%--<h4 class="titulo-cinza">Patrimônio Líquido</h4>
            <p><strong>PL médio dos últimos 12 meses:</strong><br />
            <%=this.PatrimonioLiquido%></p>--%>
        </div>
    </div>
</div>

<div id="pnlSemDados" runat="server" visible="false" style="text-align:center; padding-top:4em;">

    <p>Não foram encontrados dados para o fundo.</p>

</div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - Fds - Dealhes" />

</asp:Content>