<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PainelDeLogin.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.PainelDeLogin" %>


<div id="<%= this.IdDoPainel %>" class="header-login <%= (this.ExibirInline) ? "PainelLoginInline" : "" %>" style="<%= this.Estilo %>">

    <div id="pnlNavegacaoLogin_DadosDoUsuario" class="DadosDeLogin" runat="server" visible="false">

        <a href="<%= this.PaginaBase.HostERaizHttps %>/MinhaConta/" title="Minha Conta" class="Minhaconta">

        <span class="SoTexto">Olá, </span>
        <span class="NomeDoUsuario"> <asp:literal id="lblUsuario_Nome" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Conta:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Conta" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Perfil:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Perfil" runat="server"></asp:literal> </span>
        <span class="TextoLabel"> Último Acesso:</span>
        <span class="CodigoDoUsuario"> <asp:literal id="lblUsuario_Acesso" runat="server"></asp:literal> </span>

        </a>

        <input type="button" onclick="return lnkGradSite_MinhaConta_Logout_Click(this)" class="btn-acesso" value="SAIR" />

        <a href="<%= this.PaginaBase.HostERaiz %>/Institucional/Default.aspx" style="margin-left:8px;"><span class="LabelInline" style="text-decoration:underline">Cliente Institucional</span></a>

        <!--a href="<%= this.PaginaBase.HostERaiz %>/Atendimento/Default.aspx" style="margin-left:8px;"><span class="LabelInline" style="text-decoration:underline">Atendimento</span></a-->

    </div>

    <asp:Panel runat="server" ID="pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogin">

        <p>
            <label class="sua-conta">
                <span class="LabelInline">Sua Conta</span>
                <span class="LabelForm">Usuário</span>
            </label>
            <!--input class="login" type="text" name="Login" value="" placeholder="Login" /-->
            <input  id="txtGradSite_Login_Usuario<%= this.SufixoInline %>" type="text"     maxlength="255" class="login" placeholder="Login" />
        </p>
        <p>
            <label class="sua-conta sua-conta-senha">
                <span class="LabelInline">&nbsp;</span>
                <span class="LabelForm">Senha</span>
            </label>

            <input  id="txtGradSite_Login_Senha<%= this.SufixoInline %>"   type="password" maxlength="15"  class="senha mostrar-teclado" placeholder="&#x25cf;&#x25cf;&#x25cf;&#x25cf;&#x25cf;"  />

            <a href="#" class="botao btn-padrao btn-erica" id="btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin" data-ClicarNoEnter="true" onclick="return btnGradSite_Login_EfetuarLogin(this, event)" style="">ACESSO</a>
        </p>

        <p class="esqueci-senha">

            <a id="btnGradSite_MinhaConta_Cadastro_Login_EsqueciSenha" href="#" OnClick="return btnGradSite_Login_EsqueciSenha_Click(this)">
                <span class="LabelInline">Esqueceu seus dados de acesso?</span>
                <span class="LabelForm">Esqueci minha senha</span>
            </a>

            <a href="<%= this.PaginaBase.HostERaiz %>/Institucional/Default.aspx" style="margin-left:28px;"><span class="LabelInline" style="text-decoration:underline">Cliente Institucional</span></a>

            <!--a href="<%= this.PaginaBase.HostERaiz %>/Atendimento/Default.aspx" style="margin-left:28px;"><span class="LabelInline" style="text-decoration:underline">Atendimento</span></a-->

        </p>


    </asp:Panel>

    <asp:Panel runat="server" ID="pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogado" visible="false">

        <p class="f5"><label runat="server" id="txtGradSite_MinhaConta_Cadastro_Login_UsuarioLogado_Nome"></label></p>

        <p class="f6"><asp:LinkButton runat="server" 
                ID="txtGradSite_MinhaConta_Cadastro_Login_Usuario_Logout" 
                onclick="txtGradSite_MinhaConta_Cadastro_Login_Usuario_Logout_Click">Logout</asp:LinkButton>
        </p>

    </asp:Panel>
    
    <div class="painel-topo-busca">
        <!--label class="label-atendimento">&nbsp;</label-->

        <button class="btnTopoCarrinho" onclick="return btnTopoCarrinho_Click(this)" style="display:none">&nbsp;</button>

        <button class="botao-busca" onclick="return btnBusca_Click(this)" title="Buscar" style="float:right; margin:12px 10px 0 -40px;">&nbsp;</button>
        <input  id="txtBusca_Termo" type="text" onkeydown="return txtBusca_Termo_KeyDown(this, event)" class="input-atendimento" style="float:right" />
    </div>

</div>


<% if (this.PaginaBase.SessaoClienteLogado == null) { %>

<script language="javascript">

/*
var gTecladoFoiAberto = false;

function VerificarPostAoFechar()
{
    try
    {
        var lTeclado = $(".ui-keyboard");

        if(lTeclado.length > 0)
        {
            gTecladoFoiAberto = true;
        }
        else if(gTecladoFoiAberto)
        {   // fechou depois de ter sido aberto, chama o postback:

            var lDivDoInputDeSenhaPreenchido = $("input[id$='_Senha'][value!='']").closest("div");

            var lBotao = lDivDoInputDeSenhaPreenchido.find("a[id$='_EfetuarLogin']");

            //o internet explorer precisa de um tempinho...
            window.setTimeout(function()
                               {
                                  lBotao.click();
                               }, 300);

            gTecladoFoiAberto = false;
        }

    }
    catch (erro) {  }
}

window.setInterval(VerificarPostAoFechar, 500);
*/

</script>

<% } %>