<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroDeCambio.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados.CadastroDeCambio" %>


<form id="form1" runat="server">

    <table id="tblProdutosCambio_ListaDeProdutos" class="GridIntranet" cellspacing="0" style="float: left; width: 55%;">

        <thead>
            <tr>
                <td>Cód.</td>
                <td>Nome</td>
                <td>Tx. Cb. Moeda</td>
                <td>Tx. Cb. Cartão</td>
                <td>IOF Moeda</td>
                <td>IOF Cartão</td>
                <td style="display:none">Url</td>
                <td style="display:none">Url2</td>
                <td style="display:none">Url3</td>
                <td style="display:none">Url4</td>
                <td style="display:none">Suspenso</td>
                <td></td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="7">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDeProdutos" runat="server">
            <itemtemplate>

            <tr rel="<%# Eval("IdProduto") %>">
                <td><%# Eval("IdProduto") %></td>
                <td style="white-space:nowrap"><%# Eval("DsNomeProduto") %> </td>
                <td style="white-space:nowrap"><%# Eval("VlPreco", "{0:n}") %> </td>
                <td style="white-space:nowrap"><%# Eval("VlPrecoCartao", "{0:n}") %> </td>
                <td style="white-space:nowrap"><%# Eval("VlTaxa", "{0:n}") %> </td>
                <td style="white-space:nowrap"><%# Eval("VlTaxa2", "{0:n}") %> </td>
                <td style="display:none"><%# Eval("UrlImagem") %> </td>
                <td style="display:none"><%# Eval("UrlImagem2") %> </td>
                <td style="display:none"><%# Eval("UrlImagem3") %> </td>
                <td style="display:none"><%# Eval("UrlImagem4") %> </td>
                <td style="display:none"><%# Eval("FlSuspenso") %> </td>
                <td> <button class="IconButton Editar" title="Editar Produto" onclick="return btnSolicitacoes_Cambio_Editar_Click(this)" style="margin-top:2px;margin-bottom:2px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="5">Nenhum produto encontrado.</td>
            </tr>
        </tbody>

    </table>

    <div id="pnlFormulario_Cambio" style="float:right;width:44%" class="pnlFormulario_Campos" >

        <input type="hidden" id="hidSolicitacoes_Cambio_ListaJson" class="ListaJson" runat="server" value="" />

        <h5 style="margin-top: 0.2em;">Cadastro de Produtos de Câmbio</h5>

        <input type="hidden" id="hidProduto_Cambio_Id" Propriedade="IdProduto" runat="server" />

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_Nome">Nome:</label>
            <input  id="txtProduto_Cambio_Nome" type="text" Propriedade="NomeDoProduto" class="validate[required]" maxlength="100" value="" style="float:left;"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_Preco">Tx. Câmbio Moeda:</label>
            <input  id="txtProduto_Cambio_Preco" type="text" Propriedade="Preco" class="validate[required]" maxlength="7" value="" style="float:left;"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_PrecoCartao">Tx. Câmbio Cartão:</label>
            <input  id="txtProduto_Cambio_PrecoCartao" type="text" Propriedade="PrecoCartao" class="validate[required]" maxlength="7" value="" style="float:left;"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_Taxas">IOF Moeda:</label>
            <input  id="txtProduto_Cambio_Taxas" type="text" Propriedade="Taxas" class="validate[required]" maxlength="7" value="" style="float:left;"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_Taxas2">IOF Cartão:</label>
            <input  id="txtProduto_Cambio_Taxas2" type="text" Propriedade="Taxas2" class="validate[required]" maxlength="7" value="" style="float:left;"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_UrlImagem">Url da Imagem:</label>
            <input  id="txtProduto_Cambio_UrlImagem" type="text" Propriedade="UrlImagem" class="validate[required]" value="" style="float:left;"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_UrlImagem2">Url da Imagem 2:</label>
            <input  id="txtProduto_Cambio_UrlImagem2" type="text" Propriedade="UrlImagem2" class="" value="" style="float:left;"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_Cambio_UrlImagem3">Url da Imagem 3:</label>
            <input  id="txtProduto_Cambio_UrlImagem3" type="text" Propriedade="UrlImagem3" class="" value="" style="float:left;"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral" style="padding-left:28%;width:42%">
            <label for="chkProduto_Cambio_Suspenso">Produto Suspenso</label>
            <input  id="chkProduto_Cambio_Suspenso" type="checkbox" Propriedade="ProdutoSuspenso" class=""  />
        </p>

        <p class="BotoesSubmit">

            <button onclick="return btnProduto_Cambios_IncluirEditar_Click(this)" style="font-size:1em">Salvar</button>

        </p>
    </div>

</form>
