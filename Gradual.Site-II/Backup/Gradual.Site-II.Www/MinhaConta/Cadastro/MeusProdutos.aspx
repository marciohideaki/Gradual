<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="MeusProdutos.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.MeusProdutos" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasMeuCadastro.ascx"  tagname="AbasMeuCadastro"  tagprefix="ucAbasM" %>
<%@ Register src="~/Resc/UserControls/BannerLateral.ascx" tagname="BannerLateral" tagprefix="ucBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h2>Meus Produtos</h2>

    <ucAbasM:AbasMeuCadastro id="ucAbasMeuCadastro1" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Meus Produtos</h3>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col3-2">
                        
            <ul class="acordeon acordeon-produto">
                <asp:repeater id="rptListaDeFerramentas" runat="server">
                <itemtemplate>
                    <li>
                        <div class="acordeon-opcao"><%# Eval("DescricaoProduto") %> <span>Adquirido em  <%# Convert.ToDateTime(Eval("DataDaCompra")).ToString("dd/MM/yyyy HH:mm") %></span></div>
                        <div class="acordeon-conteudo">
                            <%# Eval("Text") %>
                        </div>
                    </li>
                </itemtemplate>
                </asp:repeater>
                            
            </ul>

            <h5>Compras efetuadas</h5>

            <table class="tabela">
                <thead>
                    <tr class="tabela-titulo">
                        <td>Data</td>
                        <td>Status</td>
                        <td>Quantidade x Produto</td>
                        <td>Total da Compra</td>
                        <td>Método de Pagamento</td>
                    </tr>
                </thead>

                <tbody>

                    <asp:repeater id="rptListaDeCompras" runat="server">
                    <itemtemplate>
                    <tr>
                        <td><%# Eval("Dados_Data") %>         </td>
                        <td><%# Eval("Dados_Status") %>       </td>
                        <td><%# Eval("ProdutosLegiveis") %>   </td>
                        <td><%# Eval("Dados_Total") %>        </td>
                        <td><%# Eval("PagamentosLegiveis") %> </td>
                    </tr>
                    </itemtemplate>

                    </asp:repeater>

                </tbody>

            </table>

        </div>

        <div class="col3">

            <ucBL:BannerLateral ID="bnlLeste" runat="server" Posicao="L" />

        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Meus Produtos" />

</asp:Content>
