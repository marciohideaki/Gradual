<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RodapeDoSite.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.RodapeDoSite" %>


    <!-- rodape -->
        
    <div id="footer" class="clear">
        <div class="inner">
            <div class="borda-footer clear">
                <ul id="mapa-site">
                    <li><a href="<%= this.HostERaiz %>/AnaliseseNoticias/Default/NoticiaseAvisos#Aba-Avisos">Avisos e Fatos Relevantes</a></li>
                    <li><a href="<%= this.HostERaiz %>/Atendimento/Default.aspx">Atendimento</a></li>
                    <li><a href="<%= this.HostERaiz %>/QuemSomos/GovernancaCorporativa.aspx#Aba-LegislacaoeRegras">Legislação e Regras</a></li>
                    <li><a href="<%= this.HostERaiz %>/QuemSomos/GovernancaCorporativa.aspx#Aba-SegurancaeRiscos">Segurança e Riscos</a></li>
                    <li><a href="<%= this.HostERaiz %>/MinhaConta/Ouvidoria.aspx">Ouvidoria</a></li>
                    <li><a href="<%= this.HostERaiz %>/Distribuidor/Default.aspx">Seja um Distribuidor</a></li>
                    <li><a href="<%= this.HostERaiz %>/TrabalheConosco/Default.aspx">Trabalhe Conosco</a></li>
                    <li><a href="<%= this.HostERaiz %>/Mapa">Mapa</a></li> 
                </ul>
                    
            </div>

            <ul id="rede-social">
                <li>
                    <iframe style="width: 61px; height: 20px;" data-twttr-rendered="true" title="Twitter Follow Button" class="twitter-follow-button twitter-follow-button" src="https://platform.twitter.com/widgets/follow_button.1407888064.html#_=1408504857523&amp;id=twitter-widget-0&amp;lang=en&amp;screen_name=gradualinvest&amp;show_count=false&amp;show_screen_name=false&amp;size=m" allowtransparency="true" scrolling="no" id="twitter-widget-0" frameborder="0" onload="GradSite_SociaisCarregados()"></iframe>
                    <script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+'://platform.twitter.com/widgets.js';fjs.parentNode.insertBefore(js,fjs);}}(document, 'script', 'twitter-wjs');</script>
                    <!--a href="https://twitter.com/intent/follow?original_referer=http%3A%2F%2Fwww.gradualinvestimentos.com.br&region=follow_link&screen_name=gradualinvest&tw_p=followbutton&variant=2.0"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/twitter.png" /></a-->

                    <iframe src="https://www.facebook.com/plugins/like.php?href=https%3A%2F%2Fwww.facebook.com%2Fgradualinvest&amp;width&amp;layout=button&amp;action=like&amp;show_faces=false&amp;share=true&amp;height=35" scrolling="no" style="border:none; overflow:hidden; height:20px;" allowtransparency="true" frameborder="0" onload="GradSite_SociaisCarregados()"></iframe>
                    <!--a href="https://www.facebook.com/gradualinvest"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/facebook.png" width="32px" height="32px;" /></a-->

                </li>
            </ul>

            <ul class="lista-bolsa">
                <li><a target="_blank" href="http://www.bmfbovespa.com.br/">BM&amp;F BOVESPA</a></li>
                <li><a target="_blank" href="http://www.bcb.gov.br/">BANCO CENTRAL DO BRASIL</a></li>
                <li><a target="_blank" href="http://www.tesouro.fazenda.gov.br/tesouro-direto">TESOURO DIRETO</a></li>
                <li><a target="_blank" href="http://www.cblc.com.br/">CBLC</a></li>
                <li><a target="_blank" href="http://www.cvm.gov.br/">CVM</a></li>
                <li><a target="_blank" href="http://www.bovespasupervisaomercado.com.br/home.asp">BSM</a></li>
                <li><a target="_blank" href="http://www.bvsa.org.br/">BVS&amp;A</a></li>
                <li><a target="_blank" href="http://www.ancord.org.br/">ANCORD</a></li>
            </ul>

            <div class="disclaimer">
                <p>A comunicação através da rede mundial de computadores esta sujeita a interrupções de sistemas, problemas oriundos de falhas e/ou intervenções de qualquer prestador serviços de comunicação ou de outra natureza, e, ainda,de falhas na disponibilidade e acesso ao sistema de operações e em sua rede, podendo impedir ou prejudicar o envio de<br/> ordens ou recepção de informação atualizadas, nos termos da instrução 380 da CVM.</p>
                <p><strong>Gradual C.C.T.V.M. S/A, instituição financeira autorizada a funcionar pelo Banco Central do Brasil</strong></p>
            </div>

            <ul class="imagens-footer clear">
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f1.png" /></a> </li>
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f2.png" /></a> </li>
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f3.png" /></a> </li>
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f4.png" /></a> </li>
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f5.png" /></a> </li>
                <li> <a href="http://www.bmfbovespa.com.br/pt-br/Participantes/PQO/Programa-de-qualificacao-operacional.aspx?idioma=pt-br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f6.png" /></a> </li>
                <li> <a href="http://portal.anbima.com.br/Pages/home.aspx" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f10.png" /></a> </li>
                <li> <a href="http://portal.anbima.com.br/Pages/home.aspx" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/f11.png" /></a> </li>
                <li> <a href="http://www.cetip.com.br" target="_blank"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/cetip.png" /></a> </li>
            </ul>

        </div>

    </div>

    <!-- fim rodape -->

    <a class="precisa-ajuda" href="#" onclick="return lnkAbrirOChat_Click(this)"><img src="<%= this.HostERaiz %>/Resc/Skin/Default/Img/ajuda.png" alt="Precisa de ajuda?" /></a>


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