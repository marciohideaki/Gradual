<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="Inativar.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.Inativar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ação - Inativar Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
    <h4>
        Ativar/Inativar</h4>
         <input type="hidden" id="hdAcoes_Inativar_Id_Cliente" runat="server" />
          <input type="hidden" id="hdAcoes_Inativar_Transporte" class="DadosJson" runat="server" value="" />
    <h4>
    Acesso ao Portal
    </h4>
    
    <p>
        <input  id="chkAcoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin" type="checkbox" propriedade="ZerarTentativasInvalidasDeLogin" onclick="return GradIntra_Acoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin_Click(this);" />
        <label for="chkAcoes_AtivarInativar_Login_ZerarTentativasInvalidasDeLogin" class="CheckLabel" title="Clique aqui para zerar o número de tentativas inválidas de login.">Zerar Tentativas Inválidas de Login</label>
    </p>

    <p>
        <input type="checkbox" id="chkAcoes_AtivarInativar_Login_Confirma" Propriedade="LoginAtivo" />
        <label  class="CheckLabel" for="chkAcoes_AtivarInativar_Login_Confirma">
            Login no Portal Ativo</label>
    </p>
    <br/><br/><br/>
       <%-- 
       <h4>
    Acesso ao HB
    </h4>
    
    <p>
        <input type="checkbox" id="chkAcoes_AtivarInativar_HB_Confirma" Propriedade="HbAtivo" />
        <label  class="CheckLabel" for="chkAcoes_AtivarInativar_HB_Confirma">
            Permissão para acessar o HB</label>
    </p>
    <br/><br/><br/>
    --%>
        <h4>
    Atividades no Sinacor - Permissão para Operar
    </h4>

    <p>
        <input type="checkbox" id="chkAcoes_AtivarInativar_CliGer_Confirma" Propriedade="CliGerAtivo"   />
        <label  class="CheckLabel" for="chkAcoes_AtivarInativar_CliGer_Confirma">
            Ativo na Atividade Cliente Geral</label>
    </p>
    <table class="GridIntranet" cellspacing="0" style="float: left; width: 99%">
        <thead>
            <tr>
                <td>
                    Código
                </td>
                <td>
                    Bovespa
                </td>
                <td>
                    BM&F
                </td>
                <td>
                    Conta Corrente
                </td>
                <td>
                    Custódia
                </td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="5">
                </td>
            </tr>
        </tfoot>
        <tbody>
            <!--LOOP-->
            <asp:Repeater runat="server" ID="rptClientes_AtivarInativar">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("Codigo")%>
                        </td>
                        <td>
                            <%#Eval("GetCkbBovespa")%><!--ckbBov-->
                        </td>
                        <td>
                            <%#Eval("GetCkbBmf")%><!--ckbBmf-->
                        </td>
                        <td>
                            <%#Eval("GetCkbCc")%><!--ckbCc-->
                        </td>
                        <td>
                            <%#Eval("GetCkbCustodia")%><!--ckbCus-->
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <br/><br/>
    <p id="pnlDataAtivacaoInativacao" runat="server" style="font-size: 1.1em; text-align: center">
    </p>
    <p class="BotoesSubmit">
        <button id="btnSalvar" runat="server" onclick="return btnAcoes_AtivarInativarCliente_Salvar_Click(this)">
            Atualizar e Sincronizar com Sinacor</button>
    </p>
    </form>
</body>
</html>
