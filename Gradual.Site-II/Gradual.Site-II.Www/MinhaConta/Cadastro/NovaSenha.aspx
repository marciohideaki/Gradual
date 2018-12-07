<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="NovaSenha.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.NovaSenha" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section id="Conteudo" class="PaginaConteudo" runat="server">

    <div class="row">
        <div class="col1">
            <h2>Nova Senha</h2>

            <p>Favor efetuar a troca de sua senha nos campos abaixo</p>

        </div>
    </div>

    <div class="form-padrao">
        <div class="row">
            <div class="col2">
                <div class="campo-basico campo-senha campo-longo-teclado">
                    <label for="txtCadastro_PFPasso4_SenhaNova">Nova Senha:</label>
                    <input id="txtCadastro_PFPasso4_SenhaNova" class="teclado-dinamico" onclick="javascript:return showKeyboard($('#btnSeguranca_SenhaNova'), event, { Controle: '#txtCadastro_PFPasso4_SenhaNova', Mensagem: 'Nova Senha' } );" onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly />
                    <button class="teclado-virtual" type="button" id="btnSeguranca_SenhaNova" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_SenhaNova', Mensagem: 'Nova Senha' } );"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col2">
                <div class="campo-basico campo-senha campo-longo-teclado">
                    <label for="txtCadastro_PFPasso4_SenhaNovaC">Confirmar Nova Senha:</label>
                    <input id="txtCadastro_PFPasso4_SenhaNovaC" class="teclado-dinamico" onclick="javascript:return showKeyboard($('#btnSeguranca_SenhaConfirmacao'), event, { Controle: '#txtCadastro_PFPasso4_SenhaNovaC', Mensagem: 'Confirmação de Senha' } );" onkeydown="javascript:ValidarTamanhoCampo(this, event);" maxlength="6" readonly />
                    <button class="teclado-virtual" type="button" id="btnSeguranca_SenhaConfirmacao" onclick="javascript:return showKeyboard(this, event, { Controle: '#txtCadastro_PFPasso4_SenhaNovaC', Mensagem: 'Confirmação de Senha' });"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                </div>
            </div>
        </div>

    </div>

    <div class="row">
        <div class="col6">

<%--        <asp:Button runat="server"
            ID="btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar" 
            Text="Enviar" 
            onclientclick="return btnGenerico_Validar()"
            onclick="btnSiteGradual_NovaSenha_Click" cssclass="botao btn-padrao" style="margin:0px 1em 0px 0.8em" />--%>

            <a class="botao btn-padrao" href="#" id="btnNovaSenha_PasswordChange" onclick="javascript: return NewPassword_Click(this, event, '');" style="width:220px">Alterar Senha</a>
        </div>
    </div>
    
    <div class="row">
        <div class="col1">
                <p>Em caso de dúvidas, entre em contato com a Central de Relacionamento pelo 4007-1873 (Região Metropolitana) ou 0800 655 1873 (Demais Regiões).</p>
        </div>
    </div>

</section>

<section id="Expirado" class="PaginaConteudo" runat="server" visible="false">
    <div class="row">
        <div class="col1">
            <h2>Token Expirado</h2>

            <p>Desculpe, mas o token utilizado para a troca de senha encontra-se expirado.</p>
            <p>Para trocar sua senha, será necessário gerar um novo token na opção "Esqueceu seus dados de acesso?" na parte superior da página.</p>
            <p>Em caso de dúvidas, entre em contato com a Central de Relacionamento pelo 4007-1873 (Região Metropolitana) ou 0800 655 1873 (Demais Regiões).</p>
        </div>
    </div>
</section>

<section id="Inexistente" class="PaginaConteudo" runat="server" visible="false">
    <div class="row">
        <div class="col1">
            <h2>Token Inexistente</h2>

            <p>Desculpe, mas não pudemos atender sua requisição pois um token não foi fornecido.</p>
            
            <p>Para trocar sua senha, será necessário gerar um novo token na opção "Esqueceu seus dados de acesso?" na parte superior da página.</p>
            <p>Em caso de dúvidas, entre em contato com a Central de Relacionamento pelo 4007-1873 (Região Metropolitana) ou 0800 655 1873 (Demais Regiões).</p>
        
        </div>
    </div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Nova Senha" />

<div id="pnlMensagemContainer" style="display:none">
    <div id="pnlMensagem">
        <a href="#" onclick="return GradSite_RetornarMensagemAoEstadoNormal()" title="Fechar" class="Fechar"> x </a>

        <p>Mensagem de Alerta porque alguma coisa aconteceu.</p>

        <p>
            <button class="botao btn-padrao btn-erica" onclick="return GradSite_RetornarMensagemAoEstadoNormal()">ok</button>
        </p>
    </div>

    <div id="pnlMensagemAdicional" style="display:none">

        <textarea readonly="readonly">Varios outros erros são possíveis</textarea>

    </div>
</div>



</asp:Content>
