<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resgate.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Fundos.Resgate" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">
<h2>Resgatar aplicação</h2>
<p>Para realizar o resgate do fundo, preencha os campos abaixo:</p>
<div class="row">
    <div class="col3-2">
        <div class="box quadro-cinza quadro-aplicacao">
            <div class="form-padrao">
                <h5><%=NomeFundo %></h5>

                <p><strong>Cliente:</strong> <asp:Label  ID="lblCliente" runat="server" /> </p>
                                
                 <input type="hidden" id="hddProduto" runat="server" />
                <p><strong>Resgatar:</strong></p>
                <div class="row">
                    <div class="lista-radio-resgatar aplicar-agendar col4">
                        <label class="radio">           <asp:RadioButton id="rdTotal" GroupName="resgatar"   runat="server" /> Total: </label>
                        <label class="radio clear">     <asp:RadioButton id="rdParcial" GroupName="resgatar" runat="server"  /> Parcial: </label>
                    </div>
                    <div class="lista-radio aplicar-agendar col3">
                        <asp:TextBox ID="txtTotalDisponivel" runat="server" ReadOnly="true" />
                        <asp:TextBox ID="txtParcial" runat="server" onkeydown="return valF2Num_OnKeyDown(event)" class="ValorNumerico"  />
                    </div>
                </div>
                <div class="row">
                    <div class="col3">
                        Agendar: <asp:TextBox ID="txtAgendar" runat="server" class="calendario voa" />
                    </div>
                </div>
                <div class="row">
                    <div class="col2" style="width:312px; margin-top:20px">
                        <div class="form-padrao">
                            <div class="campo-basico campo-senha">
                                <label>Assinatura Eletrônica:</label>
                                <asp:textbox id="txtAssDigital" runat="server" type="password" class="mostrar-teclado"></asp:textbox>
                                <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../../Resc/Skin/Default/Img/teclado.png"></button>
                            </div>
                            <small class="esqueceu-assinatura"><a href="../../Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col3">
                        <label></label>
                        <label></label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col4" >
                    <asp:Button ID="btnCancelar" runat="server" cssclass= "botao btn-padrao btn-erica" Text="CANCELAR" OnClientClick="javascript: window.history.go(-1);" />
                    <asp:Button ID="btnResgatar" runat="server" cssclass= "botao btn-padrao btn-erica" Text= "Resgatar" onclick="btnResgatar_Click"  />
                </div>
            </div>
        </div>
    </div>
</div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - Fds - Resgate" />

</asp:Content>
