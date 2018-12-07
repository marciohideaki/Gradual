<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BannerLateral.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.BannerLateral" %>

<%  if(this.TemConteudo) 
    {
        if (PaginaBase.ModuloCMSSeraExibido(false))
        {
            // versão com conteúdo e CMS pra editar
 %>
        <div class="BannerLateralContainer">
            <button class="btnExcluirBanner" data-IdBannerLink="<%= this.Banner.IdBannerLink %>" data-IdDaPagina="<%= this.PaginaBase.PaginaMaster.IdDaPagina %>" onclick="return btnExcluirBanner_Click(this)"></button>
            <a href="<% = this.Banner.Link %>" title="<%= this.Banner.Titulo %>">
                <img src="<%= this.Banner.LinkParaArquivo %>"  />
            </a>
        </div>
<% 
    
        }
        else
        {
            // versão com conteúdo mas sem CMS pra editar
%>
        <div class="BannerLateralContainer">
            <a href="<% = this.Banner.Link %>" title="<%= this.Banner.Titulo %>">
                <img src="<%= this.Banner.LinkParaArquivo %>"  />
            </a>
        </div>
<%
        }
    }
    else
    {
       if(PaginaBase.ModuloCMSSeraExibido(false))
       {
            // versão sem conteúdo e CMS pra editar
%>

    <div class="pnlIncluirBannerLateral" data-Posicao="<%= this.Posicao %>" data-UrlDaPagina="<%= this.UrlDaPagina %>">
    
        <button class="btnIncluirBannerLateral" onclick="return btnIncluirBannerLateral_Click(this)"></button>
    
        <div class="pnlListaDeBanners" style="display:none;">

            <label>Escolha o Banner:</label>

            <select class="cboBanner">

                <%
                    Gradual.Site.Www.TransporteBannerLateral lItem;

                   foreach (int lChave in this.PaginaBase.BannersLateraisDisponiveis.Keys)
                   {
                       lItem = this.PaginaBase.BannersLateraisDisponiveis[lChave];
                %>

                <option value="<%= lItem.IdBanner  %>"><%= lItem.Titulo  %></option>

                <% } %>

            </select>

            <button onclick="return btnSalvarBanner_Click(this)">Salvar</button>

        </div>

    </div>

<%
    }
    else
    {
        // versão sem conteúdo e sem CMS não aparece nada mesmo
    }
} 

%>