<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PessoasVinculadas.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.PessoasVinculadas" %>
<form id="form2" runat="server" enctype="multipart/form-data">

    <table id="tblPessoaVinculadas_ListaDePessoasVinculadas" class="GridIntranet" cellspacing="0" style="float: left; width: 49%;">

        <thead>
            <tr>
                <td >Código</td>
                <td >CPF/CNPJ</td>
                <td >Pessoa Vinculada</td>
                <td >Código Pessoa Responsável</td>
                <td >Código do Cliente</td>
                <td> </td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDePessoasVinculadas" runat="server">
            <itemtemplate>

            <tr rel="<%# Eval("CodigoPessoaVinculada") %>">
                <td><%# Eval("CodigoPessoaVinculada") %></td>
                <td><%# Eval("CPFCNPJ") %></td>
                <td><%# Eval("Nome") %></td>
                <td rel="<%# Eval("CodigoPessoaVinculadaResponsavel") %>"><%# Eval("CodigoPessoaVinculadaResponsavel")%></td>
                <td><%# Eval("CodigoCliente")%></td>
                <td> <button class="IconButton Excluir" title="Excluir Pessoa" onclick="return btnSistema_PessoasVinculadas_Excluir_Click('Sistema/PessoasVinculadas.aspx', this)" style="margin-top:2px;margin-bottom:2px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="6">Nenhuma Pessoa encontrada.</td>
            </tr>
        </tbody>

    </table>

    <div style="float:right;width:49%" class="pnlFormulario_Campos" >

        <input type="hidden" id="hidPessoasVinculadas_ListaJson" class="ListaJson" runat="server" value="" />

        <h5 style="margin-top: 0.2em;">Cadastro de Pessoas Vinculadas</h5>

        <input type="hidden" id="hidSistema_PessoasVinculadas_Id" runat="server" />

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtPessoasVinculadas_CodigoCliente">Código do Cliente:</label>
            <input  id="txtPessoasVinculadas_CodigoCliente" type="text" Propriedade="CodigoCliente" runat="server" class="validate[custom[onlyNumber]]" style="float:left"  />
            <button class="IconButton IconePesquisar" onclick="return btnSistema_PessoasVinculadas_BuscarCliente_Click(this)"></button>
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <span id="txtPessoasVinculadas_Nome_Cliente" Propriedade="NomeCliente" runat="server" style="float:left;margin-left:144px"></span>
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtPessoasVinculadas_Nome">Nome:</label>
            <input  id="txtPessoasVinculadas_Nome" type="text" Propriedade="Nome" class="validate[required,length[5,60]]" runat="server" />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtPessoasVinculadas_CPFCNPJ" >CPF/CNPJ:</label>
            <input  id="txtPessoasVinculadas_CPFCNPJ" type="text" Propriedade="CPFCNPJ" class="validate[required,funcCall[validatecpfcnpj]] text-input" runat="server"/>
        </p>

        <p class="Form_TiposDePendenciaCadastral" style="display:none;">
            <label for="txtPessoasVinculadas_CodigoPessoaVinculada" >Código Pessoa Vinculada Responsável:</label>
            <input  id="txtPessoasVinculadas_CodigoPessoaVinculada" type="text" Propriedade="CodigoPessoaVinculada" runat="server" class="validate[custom[onlyNumber]]" />
        </p>

        <p class="Form_TiposDePendenciaCadastral" style="margin-left:140px; width:60%;">
            <input type="checkbox" id="txtPessoasVinculadas_Ativo" Propriedade="FlagAtivo" runat="server"  />
            <label for="txtPessoasVinculadas_Ativo"  class="CheckLabel" >Ativo</label>
        </p>

        <p class="BotoesSubmit">
            <button onclick="return btnSistema_PessoasVinculadas_Incluir_Click(this)" style="font-size:1em;">Incluir</button>
        </p>
    </div>
</form>