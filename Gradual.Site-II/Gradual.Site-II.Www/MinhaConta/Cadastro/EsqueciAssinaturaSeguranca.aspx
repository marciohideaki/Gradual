<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="EsqueciAssinatura.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.EsqueciAssinatura" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <div class="row">
        <div class="col3-2">
            <h2>Esqueci Minha Assinatura Eletrônica</h2>

            <p>Confirme alguns dados para que a sua nova assinatura seja encaminhada para o seu email cadastrado na Gradual</p>
            <p>Em caso de dúvidas, entre em contato com a Central de Relacionamento pelo 4007-1873 (Região Metropolitana) ou 0800 655 1873 (Demais Regiões).</p>
        </div>
    </div>

    <div id="frmEsqueciAssinatura" class="form-padrao">
        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>E-mail:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Email" runat="server" type="text" maxlength="80" class="validate[required,custom[Email]]" />
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>CPF / CNPJ:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_CPFCNPJ" runat="server" type="text" maxlength="18" class="validate[required,custom[validatecpfcnpj]] ProibirLetras EstiloCampoObrigatorio" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col3">
                <div class="campo-basico">
                    <label>Data de Nascimento / Fundação:</label>
                    <input  id="txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_DataNascimentoFundacao" runat="server" type="text" maxlength="10" class="validate[required,custom[data]] Mascara_Data" />
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col6">

            <asp:Button runat="server" ID="btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Enviar" Text="Enviar" onclientclick="return btnGenerico_Validar()" onclick="btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Enviar_Click" cssclass="botao btn-padrao" />

        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Esqueci Assinatura" />

</asp:Content>
