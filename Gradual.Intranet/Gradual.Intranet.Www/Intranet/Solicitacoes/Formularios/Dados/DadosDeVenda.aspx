<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DadosDeVenda.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados.DadosDeVenda" %>


<form id="form1" runat="server">

    <h4>Dados da Venda</h4>
    
    <div class="PainelDeDados">
    
        <h5>Venda</h5>
        <p>
            <label>ID:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_ID" runat="server"></asp:label>

            <label>Código de Referência:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_CodigoDeReferencia" runat="server"></asp:label>
        </p>
        <p>
            <label>Data:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_Data" runat="server"></asp:label>

            <label>Status:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_Status" runat="server"></asp:label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_DescricaoStatus" runat="server"></asp:label>
        </p>

        <h5>Cliente</h5>
        <p>
            <label>CBLC:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_CBLC" runat="server"></asp:label>

            <label>CPF / CNPJ:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_CpfCnpj" runat="server"></asp:label>
        </p>


        <h5>Método de Pagamento</h5>
        <p>
            <label>ID:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_IdPagamento" runat="server"></asp:label>

            <label>Tipo:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_Tipo" runat="server"></asp:label>

            <label>Código:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_MetodoTipo" runat="server"></asp:label>

            <label>Descrição:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_MetodoDesc" runat="server"></asp:label>
        </p>


        <h5>Produtos</h5>

        <table cellspacing="0" class="GridIntranet" style="float:left;clear:both;width:98%;;margin-left:6px;margin-bottom:12px">
            <thead>
            <tr>
                <td>Código</td>
                <td>Nome</td>
                <td>Qtd.</td>
                <td>Preço Unit.</td>
                <td>Taxas</td>
                <td>Preço Tot.</td>
                <td>Obs.</td>
            </tr>
            </thead>
            <tbody>
            <asp:repeater id="rptProdutosDaVenda" runat="server">
            <itemtemplate>
            <tr>
                <td><%#Eval("IdProduto") %></td>
                <td><%#Eval("DescProduto") %></td>
                <td><%#Eval("Quantidade") %></td>
                <td><%#Eval("PrecoUnit") %></td>
                <td><%#Eval("ValorTaxasProduto") %></td>
                <td><%#Eval("Preco") %></td>
                <td><%#Eval("Obs") %></td>
            </tr>
            </itemtemplate>
            </asp:repeater>
            </tbody>
        </table>

        <h5>Valores</h5>
        <p>
            <!--
            <label>Qtd. do Produto:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_Quantidade" runat="server"></asp:label>
            -->

            <label>Total da Compra:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_ValorBruto" runat="server"></asp:label>

            <label>Parcelas:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_QuantidadeDeParcelas" runat="server"></asp:label>

            <label>Desconto:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_ValorDesconto" runat="server"></asp:label>

            <label>Taxas de Pgto:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_ValorTaxas" runat="server"></asp:label>

            <!--
            <label>Valor Líquido:</label>
            <asp:label id="txtSolicitacoes_DadosDaVenda_ValorLiquido" runat="server"></asp:label>
            -->
        </p>
        
        <h5>Endereço de entrega e telefones</h5>
        <p>
            <asp:literal id="lblEnderecoEntrega" runat="server"></asp:literal>
        </p>
        <p>
            <asp:literal id="lblTelefones" runat="server"></asp:literal>
        </p>

    </div>

</form>

