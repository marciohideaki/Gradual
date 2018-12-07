<%@ Page Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Retiradas.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.Retiradas" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Retiradas<span><%=string.Concat("(de ", DateTime.Now.ToString("dd/MM/yyyy"), " às ", DateTime.Now.ToString("HH:mm"), ")") %></span></h3>

                <ul>
                    <li class="conta">
                        <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                        <div class="conta_msg" style="display:none">
                        Confira os dados bancários da Gradual:<br />
                        Banco BM&F - 096<br />
                        Agência: 001<br />
                        C/C: 326-5<br />
                        Favorecido: Gradual CCTVM S/A<br />
                        CNPJ: 33.918.160/0001-73
                        </div>
                    </li>
                    <li class="interrogacao"><button type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')">?</button><div class="ajuda">Informe a quantia que deseja resgatar e a conta bancária de destino, que deve estar cadastrada na Gradual.</div></li>
                    <%--<li class="email"><button type="button" title="Enviar por e-mail">Enviar por email</button></li>--%>
                </ul>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col2">
            <p>Para retirada no mesmo dia, a solicitação deve ser realizada até às 15h.</p>
        </div>
                    
        <div class="col2">
            <p><strong>Conta para transferência dos recursos:</strong></p>
        </div>
    </div>

    <div class="form-padrao clear">

        <div class="row">
            <div class="col2">
                <div class="form-padrao clear">
                    <div class="campo-consulta">
                        <label>Valor do Resgate: R$</label>
                        <asp:textbox id="txtValorParcial" runat="server"></asp:textbox>
                    </div>

                    <div class="campo-consulta">
                        <label>Valor Disponível: R$</label>
                        <asp:textbox id="txtValorTotal" runat="server" enabled="false"></asp:textbox>
                    </div>
                </div>
            </div>
            <div class="col2">
                <div  class="pnlRadios" id="form-selecao" >
                    <asp:repeater id="rptContasBancarias" runat="server">
                    <itemtemplate>
                    <div class="lista-radio-retirada">
                       <Label class="radio"> <input  id="rdoConta-<%# Eval("CodigoDoBanco")%>-<%# Eval("NumeroDaAgencia")%>-<%# Eval("NumeroDaConta")%>"  onclick="rdoConta_Selecionar_Click(this)"  type="radio" name="rdoConta" class="radio" />

                        <label for="rdoConta-<%# Eval("CodigoDoBanco")%>-<%# Eval("NumeroDaAgencia")%>-<%# Eval("NumeroDaConta")%>"  onclick="rdoConta_Selecionar_Click(this)">

                            <%# Eval("NomeDoBanco")%>(<%# Eval("CodigoDoBanco")%>) Ag.<%# Eval("NumeroDaAgencia")%>-<%# Eval("DigitoDaAgencia")%> Cta. <%# Eval("NumeroDaConta")%> - <%# Eval("DigitoDaConta")%>

                        </label>
                        </Label>
                        </div>
                    </itemtemplate>
                    </asp:repeater>
                    <input type="hidden" id="hidValorEscolhido" runat="server" />
                    <%--<div class="lista-radio">
                        <label class="radio"><input type="radio" name="radioteste" checked="checked" value="" /> BANCO BRADESCO SA(237) Ag.3636 - Cta. 969 - 5</label>
                    </div>--%>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col3">
                <div class="form-padrao">
                    <div class="campo-basico campo-senha">
                        <label>Assinatura Eletrônica:</label>
                        <asp:textbox id="txtAssDigital" runat="server" type="password" class="mostrar-teclado"></asp:textbox>

                        <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png"></button>
                    </div>
                    <small class="esqueceu-assinatura"><a href="../Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col3">
                <asp:button id="btnRetirada" runat="server" cssclass="botao btn-padrao btn-erica" text="Retirar" OnClick="btnRetirada_OnClick" />
            </div>
        </div>
                    
        <div class="row">
            <div class="col1">
                <p><small>Para sua segurança, seus recursos só poderão ser resgatados para a conta bancária cadastrada junto à Corretora</small></p>
            </div>
        </div>

    </div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Retiradas" />

</asp:Content>