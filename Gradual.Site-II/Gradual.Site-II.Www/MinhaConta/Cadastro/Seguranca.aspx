<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Seguranca.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.Seguranca" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasMeuCadastro.ascx"  tagname="AbasMeuCadastro"  tagprefix="ucAbasM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language='javascript'>
    $(document).ready(function ()
    {
        Seguranca_VerificacaoTeclado();
        GradSite_AtivarInputs('section.PaginaConteudo');
    });
    
</script>


<section class="PaginaConteudo">

    <h2>Meu Cadastro</h2>

    <ucAbasM:AbasMeuCadastro id="ucAbasMeuCadastro1" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Segurança</h3>
            </div>
        </div>
    </div>

    <div class="row">

        <div id="pnlDadosSeguranca">

            <div class="form-padrao">
                <div class="row">
                    <div class="col1">
                        <div class="menu-exportar clear">
                            <h5>Alterar Senha</h5>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label for="txtCadastro_PFPasso4_SenhaAtual">Senha Atual:</label>
                            <input id="txtCadastro_PFPasso4_SenhaAtual" <%--type="password"--%> onclick="javascript:return showKeyboard($('#btnSeguranca_SenhaAtual'), event, { Controle: '#txtCadastro_PFPasso4_SenhaAtual', Mensagem: 'Senha Atual' } );" class="senha mostrar-teclado teclado-dinamico" />
                            <%--<button class="teclado-virtual" type="button"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>--%>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_SenhaAtual" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_SenhaAtual', Mensagem: 'Senha Atual' });"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label for="txtCadastro_PFPasso4_SenhaNova">Nova Senha:</label>
                            <input id="txtCadastro_PFPasso4_SenhaNova" <%--type="password"--%> class="teclado-dinamico" onclick="javascript:return showKeyboard($('#btnSeguranca_SenhaNova'), event, { Controle: '#txtCadastro_PFPasso4_SenhaNova', Mensagem: 'Nova Senha' } );" onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly/>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_SenhaNova" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_SenhaNova', Mensagem: 'Nova Senha' } );"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>

                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label for="txtCadastro_PFPasso4_SenhaNovaC">Confirmar Nova Senha:</label>
                            <input id="txtCadastro_PFPasso4_SenhaNovaC" <%--type="password"--%> class="teclado-dinamico" onclick="javascript:return showKeyboard($('#btnSeguranca_SenhaConfirmacao'), event, { Controle: '#txtCadastro_PFPasso4_SenhaNovaC', Mensagem: 'Confirmação de Senha' } );"onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly/>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_SenhaConfirmacao" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_SenhaNovaC', Mensagem: 'Confirmação de Senha' });"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>
                </div>
                            
                <div class="row">
                    <div class="col6">
                        <a class="botao btn-padrao" href="#" id="btnSeguranca_PasswordChange" onclick="javascript: return PasswordChange_Click(this, event, '');" style="width:220px">Alterar Senha</a>
                    </div>
                </div>

            </div>

            <div class="form-padrao" style="padding-top:40px">

                <div class="row">
                    <div class="col1">
                        <div class="menu-exportar clear">
                            <h5>Alterar Assinatura Eletrônica</h5>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Assinatura Eletrônica Atual:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaAtual" <%--type="password"--%> onclick="javascript:return showKeyboard($('#btnSeguranca_AssinaturaAtual'), event, { Controle: '#txtCadastro_PFPasso4_AssinaturaAtual', Mensagem: 'Assinatura Atual' } );" class="senha mostrar-teclado teclado-dinamico"/>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_AssinaturaAtual" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_AssinaturaAtual', Mensagem: 'Assinatura Atual' });"><img alt="Teclado Virtual" src="../../../Resc/Skin/Default/Img/teclado.png"></button>
                        </div>
                    </div>
                    <div class="col2">
                        <div class="campo-basico campo-senha" style="padding-top:22px">
                            <a href="EsqueciAssinaturaSeguranca.aspx">Esqueci minha assinatura</a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Nova Assinatura Eletrônica:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaNova" <%--type="password"--%> class="teclado-dinamico" onclick="javascript:return showKeyboard($('#btnSeguranca_Assinatura'), event, { Controle: '#txtCadastro_PFPasso4_AssinaturaNova', Mensagem: 'Nova Assinatura' } );" onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly/>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_Assinatura" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_AssinaturaNova', Mensagem: 'Nova Assinatura' });"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>

                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Confirmar Nova Assinatura Eletrônica:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaNovaC" <%--type="password" --%>class="teclado-dinamico"  onclick="javascript:return showKeyboard($('#btnSeguranca_AssinaturaConfirmacao'), event, { Controle: '#txtCadastro_PFPasso4_AssinaturaNovaC', Mensagem: 'Confirmação de Assinatura' } );" onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly/>
                            <button class="teclado-virtual" type="button" id="btnSeguranca_AssinaturaConfirmacao" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_AssinaturaNovaC', Mensagem: 'Confirmação de Assinatura' });"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col6">
                        <a class="botao btn-padrao" href="#" id="btnSeguranca_SignatureChange" onclick="return btnCadastro_PFPasso4_AlterarAssinatura_Click(this)" style="width:220px">Alterar Assinatura</a>
                    </div>
                </div>

            </div>

        </div>

    </div>
    
    <div id="pnlLoader" style="display:none">
        <div class="Mensagem">
            <span>Gravando dados, aguarde...</span>
            <p class="BotaoOk" style="display:none">
                <button class="BotaoVerde" onclick="return btnCadastro_Ok_Click(this)">OK</button>
            </p>
        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Seguranca" />

</asp:Content>
