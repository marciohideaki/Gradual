<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Login" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/PainelDeLogin.ascx" tagname="PainelDeLogin" tagprefix="uc1" %>
<%@ Register src="~/Resc/UserControls/BannerLateral.ascx" tagname="BannerLateral" tagprefix="ucBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo" style="background-image:none">

    <div class="inner" style="margin-top:-24px">

        <ul id="breadcrumb">
            <li>
                <a href="#">Inicial</a>
            </li>
            <li class="ativo">
                <a href="#">Login</a>
            </li>
        </ul>

        <h2>Login</h2>

        <div class="row">
            <div class="col1">
                <p>Informe abaixo os seus dados de usuário para acessar a sua conta.</p>
            </div>
        </div>

        <div class="form-padrao form-login">
            <div class="row">
                <div class="col3-2 col-login">
                    <div class="box quadro-cinza clear">
                        <div class="ladoA">


                            <uc1:PainelDeLogin ID="PainelDeLogin1" runat="server" ExibirSemID="true" AparecerVisivel="true" ExibirInline="true" />


                        </div>
                        <div class="ladoB">
                            <h5>Ainda não é cliente Gradual?</h5>
                            <p>Realize o seu cadastro e aproveite todas as nossas vantagens. Não perca tempo!</p>
                            <p><a class="botao btn-padrao btn-erica" href="NovoCadastro.aspx">Cadastre-se</a></p>
                        </div>
                    </div>
                </div>

                <div class="col4">

                    <ucBL:BannerLateral ID="bnlEast" runat="server" Posicao="L" />

                </div>
            </div>
        </div>

    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Login" />

</asp:Content>


