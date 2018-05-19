<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="AssociarPermissoes.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados.AssociarPermissoes" %>
<form id="form1" runat="server">

    <h4>Associar Permissões</h4>

        <input type="hidden" id="hidSeguranca_Grupos_ListaJson" class="ListaJson" runat="server" value="" />

        <input type="hidden" id="txtSeguranca_Grupos_Id" Propriedade="Id" />
        <input type="hidden" id="txtSeguranca_Grupos_ParentId" Propriedade="ParentId" />
        
        <p style="width: 80px;margin-left:150px">
            <label for="rdoSeguranca_Associacoes_Usuario" style="text-align:left">Usu&aacute;rios</label>
                <input name="rdoSeguranca_Associacoes_Grupo_Usuario" type="radio" Propriedade="EhUsuario" id="rdoSeguranca_Associacoes_Usuario" onclick="rboSeguranca_Associacoes_Usuario_Click()" checked="checked"  />
        </p>

        <p style="width:46%">
            <label for="rdoSeguranca_Associacoes_Grupo" style="text-align:left">Grupos</label>
                <input name="rdoSeguranca_Associacoes_Grupo_Usuario" type="radio" Propriedade="EhGrupo" id="rdoSeguranca_Associacoes_Grupo" onclick="rboSeguranca_Associacoes_Grupo_Click()" />
        </p>

        <p>
            <label for="cboSeguranca_Associacoes_Usuario">Usu&aacute;rios:</label>
            <select id="cboSeguranca_Associacoes_Usuario" Propriedade="Usuario" class="validate[required]" style="width:20.6em">
                <option value="">[ Selecione ]</option>
                <asp:repeater runat="server" id="rptSeguranca_Associacoes_Usuario">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoUsuario") %>'><%# Eval("Nome")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p style="display:none">
            <label for="cboSeguranca_Associacoes_Grupo">Grupos:</label>
            <select id="cboSeguranca_Associacoes_Grupo" Propriedade="Grupo" class="validate[required]" style="width:20.6em">
                <option value="">[ Selecione ]</option>
                <asp:repeater runat="server" id="rptSeguranca_Associacoes_Grupo">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoUsuarioGrupo") %>'><%# Eval("NomeUsuarioGrupo")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p>
            <label for="cboSeguranca_Associacoes_Subsistemas" >Subsistemas:</label>
            <select id="cboSeguranca_Associacoes_Subsistemas" Propriedade="Subsistema" class="validate[required]" style="width:20.6em" onChange="cboSeguranca_Associacoes_Subsistemas_Change(this)">
                <option value="">[ Selecione ]</option>
                <asp:repeater runat="server" id="rptSeguranca_Associacoes_Subsistemas">
                    <ItemTemplate>
                        <option value='<%# Eval("Nome") %>'><%# Eval("Nome")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
        </p>

        <p>
            <label for="cboSeguranca_Associacoes_Interfaces">Interfaces:</label>
            <select id="cboSeguranca_Associacoes_Interfaces" Propriedade="Interface" class="validate[required]" style="width:20.6em" onChange="cboSeguranca_Associacoes_Interfaces_Change(this)">
                <option value="">[ Selecione ]</option>
            </select>
        </p>

        <p style="height:80px;">
            <label for="chkSeguranca_Associacoes_Permissoes_Executar" style="text-align:left;">Executar</label>
                <input name="chkSeguranca_Associacoes_Permissoes" type="checkbox" Propriedade="Executar" id="chkSeguranca_Associacoes_Permissoes_Executar"  />
            <label for="chkSeguranca_Associacoes_Permissoes_Consultar" style="text-align:left;">Consultar</label>
                <input name="chkSeguranca_Associacoes_Permissoes" type="checkbox" Propriedade="Consultar" id="chkSeguranca_Associacoes_Permissoes_Consultar"  />
            <label for="chkSeguranca_Associacoes_Permissoes_Salvar" style="text-align:left;">Salvar</label>
                <input name="chkSeguranca_Associacoes_Permissoes" type="checkbox" Propriedade="Salvar" id="chkSeguranca_Associacoes_Permissoes_Salvar" />
            <label for="chkSeguranca_Associacoes_Permissoes_Excluir" style="text-align:left;">Excluir</label>
                <input name="chkSeguranca_Associacoes_Permissoes" type="checkbox" Propriedade="Excluir" id="chkSeguranca_Associacoes_Permissoes_Excluir" />
        </p>

        <p class="BotoesSubmit">
            <button id="btnSeguranca_Associacoes_Salvar" onclick="return btnSeguranca_Associacoes_Salvar_Click(this)">Salvar Alterações</button>
        </p>

</form>