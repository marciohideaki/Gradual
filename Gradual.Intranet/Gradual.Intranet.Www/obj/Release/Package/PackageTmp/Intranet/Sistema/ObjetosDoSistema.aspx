<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="ObjetosDoSistema.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.ObjetosDoSistema" %>

<form id="form1" runat="server" enctype="multipart/form-data">

    <input type="hidden" id="hidObjetosDoSistema_TipoDeObjetoAtual" value="" />

    <table id="tblObjetosdoSistema_ListaDeObjetos" class="GridIntranet" cellspacing="0" style="float: left; width: 49%;">

        <thead>
            <tr>
                <td style="width:92%">Objeto</td>
                <td></td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDeItens" runat="server">
            <itemtemplate>

            <tr rel="<%# Eval("Id") %>">
                <td> <%# Eval("Descricao") %> </td>
                <td> <button class="IconButton Excluir" runat="server" id="btnExcluirObjeto" title="Excluir objeto" onclick="return btnSistema_ObjetosDoSistema_Excluir_Click('Sistema/ObjetosDoSistema.aspx', this)" style="margin-top:2px;margin-bottom:2px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="2">Nenhum item encontrado.</td>
            </tr>
        </tbody>

    </table>

    <div style="float:right;width:49%">

        <input type="hidden" id="hidObjetosDoSistema_ListaJson" class="ListaJson" runat="server" value="" />

        <h5 style="margin-top: 0.2em;">Incluir Novo</h5>

        <p class="Form_AtividadesIlicitas" style="display:none">
            <label  for="cboObjetosDoSistema_AtividadesIlicitas_Atividade">Ramo de Atividade</label>
            <select  id="cboObjetosDoSistema_AtividadesIlicitas_Atividade">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptObjetosDoSistema_AtividadesIlicitas_Atividade" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater> 
            </select>
        </p>


        <p class="Form_PaisesEmListaNegra" style="display:none">
            <label  for="cboObjetosDoSistema_PaisesEmListaNegra_Pais">País</label>
            <select  id="cboObjetosDoSistema_PaisesEmListaNegra_Pais">
                <option value="">[ Selecione ]</option>
                <asp:Repeater id="rptObjetosDoSistema_PaisesEmListaNegra_Pais" runat='server'>
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </p>


        <p class="Form_Contratos" style="display:none">
            <label for="txtObjetosDoSistema_Contratos_Termo">Termo</label>
            <input  id="txtObjetosDoSistema_Contratos_Termo" type="text" />
        </p>
        <p class="Form_Contratos" style="display:none">
            <label  for="cboObjetosDoSistema_Contratos_Tipo">Tipo</label>
            <select  id="cboObjetosDoSistema_Contratos_Tipo">
                <option value="">[ Selecione ]</option>
                <option value="1">Lei</option>
                <option value="2">Contrato</option>
            </select>
        </p>
        <p class="Form_Contratos" style="display:none">
            <label for="txtObjetosDoSistema_Contratos_Arquivo">Arquivo</label>
            <input  id="txtObjetosDoSistema_Contratos_Arquivo" type="file" runat="server" />
        </p>


        <p class="Form_TiposDePendenciaCadastral" style="display:none">
            <label for="txtObjetosDoSistema_TiposDePendenciaCadastral_Descricao">Descrição</label>
            <input  id="txtObjetosDoSistema_TiposDePendenciaCadastral_Descricao" type="text" />
        </p>


        <p style="margin-left: 23%; width: 60%;display:none;" class="Form_TiposDePendenciaCadastral">
            <input type="checkbox" id="chkObjetosDoSistema_AtivarInativar_TiposDePendenciaCadastral_Autimatica" Propriedade="PendenciaAutomatica" />
            <label  class="CheckLabel" for="chkObjetosDoSistema_AtivarInativar_TiposDePendenciaCadastral_Autimatica">Cadastrada Automaticamente</label>
        </p>


        <p class="Form_TaxasDeTermo" style="display:none">
            <label for="txtObjetosDoSistema_TaxasDeTermo_ValorTaxa">Valor da Taxa:</label>
            <input  id="txtObjetosDoSistema_TaxasDeTermo_ValorTaxa" Propriedade="ValorTaxa" type="text" />
        </p>
        <p class="Form_TaxasDeTermo" style="display:none">
            <label for="txtObjetosDoSistema_TaxasDeTermo_ValorRolagem">Valor da Rolagem:</label>
            <input  id="txtObjetosDoSistema_TaxasDeTermo_ValorRolagem" Propriedade="ValorRolagem" type="text" />
        </p>
        <p class="Form_TaxasDeTermo" style="display:none">
            <label for="txtObjetosDoSistema_TaxasDeTermo_NumeroDias">Número de Dias:</label>
            <input  id="txtObjetosDoSistema_TaxasDeTermo_NumeroDias" Propriedade="NumeroDias" type="text" />
        </p>


        <p class="BotoesSubmit">

            <button onclick="return btnSistema_ObjetosDoSistema_Incluir_Click(this)" style="font-size:1em">Incluir</button>

        </p>
    </div>



</form>