<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="EsqueciSenha.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.EsqueciSenha" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <div class="row">
        <div class="col3-2">
            <h2>Esqueci Minha Senha</h2>

            <p>Confirme alguns dados para que a sua nova senha seja encaminhada para o seu email cadastrado na Gradual</p>
            <p>Em caso de dúvidas, entre em contato com a Central de Relacionamento pelo 4007-1873 (Região Metropolitana) ou 0800 655 1873 (Demais Regiões).</p>
        </div>
    </div>

    <div class="form-padrao">
        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>E-mail:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_Email" runat="server" type="text" maxlength="80" class="validate[required,custom[Email]]" />
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>CPF / CNPJ:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_CPFCNPJ" runat="server" type="text" maxlength="18" class="validate[required,custom[validatecpfcnpj]] ProibirLetras EstiloCampoObrigatorio" />
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>Data de Nascimento / Fundação:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao" runat="server" type="text" maxlength="10" class="validate[required,custom[data]] Mascara_Data" />
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col6">

            <asp:Button runat="server" 
            ID="btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar" 
            Text="Enviar" 
            onclientclick="return btnGenerico_Validar()"
            onclick="btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar_Click" cssclass="botao btn-padrao" style="margin:0px 1em 0px 0.8em" />

        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Esqueci Senha" />

</asp:Content>
