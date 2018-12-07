<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="MinhaConta.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.MinhaConta" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/MinhaConta/Patrimonio_Dinheiro.ascx"               tagname="Dinheiro"               tagprefix="ucPatrimonio" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/MinhaConta/Patrimonio_RendaVariavel.ascx"          tagname="RendaVariavel"          tagprefix="ucPatrimonio" %>
<%--<%@ Register src="~/Resc/UserControls/MinhaConta/MinhaConta/Patrimonio_Proventos.ascx"              tagname="Proventos"              tagprefix="ucPatrimonio" %>--%>
<%@ Register src="~/Resc/UserControls/MinhaConta/MinhaConta/Patrimonio_FundosInvestimentos.ascx"    tagname="FundosInvestimentos"    tagprefix="ucPatrimonio" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/MinhaConta/Patrimonio_RendaFixa.ascx"              tagname="RendaFixa"              tagprefix="ucPatrimonio" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="PaginaConteudo">
        <div class="row">
            <div class="col1">
                <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />
                <div class="menu-exportar clear">
                    <h3>Posição Consolidada</h3>
                        <ul>
                            <li>Última atualização: <%= DateTime.Now.ToString("dd/MM/yyyy HH:mm") %><img src="../../Resc/Skin/Default/Img/refresh.gif" style="width:20px; height: 20px; padding: 0 0 0 5px; cursor: hand;" onclick="window.location.href='<%= this.HostERaiz %>/Minhaconta/Financeiro/MinhaConta.aspx'"/></li>
                        </ul>
                </div>
                <div style="text-align:right;"></div>
                <h2>Patrimônio: <%= String.Format("R$ {0}", CurrentTotalGeral.ToString("N2")) %></h2>

                <ucPatrimonio:Dinheiro              id="Patrimonio_Dinheiro"            modo="menu" runat="server" />
                <ucPatrimonio:RendaVariavel         id="Patrimonio_RendaVariavel"       modo="menu" runat="server" />
                <ucPatrimonio:FundosInvestimentos   id="Patrimonio_FundosInvestimentos" modo="menu" runat="server" />
                <ucPatrimonio:RendaFixa             id="Patrimonio_RendaFixa"           modo="menu" runat="server" />
                * Os valores acima discriminados são parciais, podendo apresentar divergências após o fechamento e cobrança de taxas
            </div>
        </div>
    </section>

    <style type="text/css">
        .MinhaConta_Negativo
        {
            color: Red;
        }
        
        [draggable=true] {
            cursor: move;
        }
        
        button-refresh 
        {
                    background-color: Transparent;
                    background-repeat:no-repeat;
                    background-image: <%= this.RaizDoSite %>/Resc/Skin/Default/Img/refresh.gif;
                    border: none;
                    cursor:pointer;
                    overflow: hidden;
                    outline:none;
        }
        

    </style>

    <script type="text/javascript">
    $(document).ready(function () {
     $('.collapse')
         .on('shown.bs.collapse', function() {
             $(this)
                 .parent()
                 .find(".glyphicon-plus")
                 .removeClass("glyphicon-plus")
                 .addClass("glyphicon-minus");
             })
         .on('hidden.bs.collapse', function() {
             $(this)
                 .parent()
                 .find(".glyphicon-minus")
                 .removeClass("glyphicon-minus")
                 .addClass("glyphicon-plus");
             });
         });

    </script>
</asp:Content>
