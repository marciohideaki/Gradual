<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Layout.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.Layout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="Resc/Skin/Theme/theme/css/style.min.css?1363272390" />
</head>
<body>
    <!-- menu topo -->
    <nav class="navbar navbar-inverse navbar-static-top example6">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar6" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand text-hide" href="#">Integração FIDC</a>
            </div>
            <div id="navbar6" class="navbar-collapse collapse" aria-expanded="false" style="height: 1px;">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Robô Downloads<span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li class="divider"></li>
                            <li><a href="RoboDownload/Carteiras.aspx">Carteiras</a></li>
                            <li><a href="RoboDownload/Mec.aspx">Mec</a></li>
                            <li><a href="RoboDownload/ExtratoCotista.aspx">Exrtato Cotistas</a></li>
                            <li><a href="RoboDownload/TitulosLiquidados.aspx">Títulos Liquidados</a></li>
                            <li class="divider"></li>
                        </ul>
                    </li>
                    <li><a href="TitulosLiquidados/TitulosLiquidados.aspx">Títulos Liquidados</a></li>
                    <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Configurações <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li class="dropdown-header">Configs. diversas</li>
                            <li><a href="#">Parametrização</a></li>
                            <li class="divider"></li>
                            <li class="dropdown-header">Configs. de Email</li>
                            <li><a href="#">Fluxo de Email</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
        <!--/.container-fluid -->
    </nav>

    <form id="form1" runat="server">
    <div>
    <div class="container-fluid">
            
        <!--</div>-->
        <%--<div id="wrapper">--%>
            
            <div id="content">
                
                    <ul id="lstDashboardAbas" class="breadcrumb" >
                        <li data-IdDoCliente="0" style="display:inline">
                            <a href="javascript:void(0);" class="glyphicons display"  ><i></i> <asp:literal id="lblTituloDaPagina" runat="server"></asp:literal></a>
                        </li>

                        <li style="display:inline">
                            <div style="float:right; display: none;" class="lblMensagens" id="pnlMensagem">
                                <button id="btnMensagem_Fechar" onclick="return Aux_RetornarMensagemAoEstadoNormal()"></button>
                                <span></span>
                            </div>
                            <div id="pnlMensagemAdicional" style="display: none; float: right">
                                <textarea readonly="readonly"></textarea>
                                <a href="#" onclick="return lnlMensagemAdicional_Fechar_Click(this)">fechar</a>
                            </div>
                        </li>

                    </ul>
                
                
                <%--<div class="separator"></div>--%>
                <%--<div class="heading-buttons">
                    <h3 id="lblTituloDashBoard" class="glyph_icons dis_play">  </h3>
                    <!--div class="buttons pull-right">
                        <a href="" class="btn btn-default btn-icon glyphicons edit"><i></i> Edit</a>
                    </div-->
                    <div class="clearfix"></div>
                </div>--%>
                <div class="separator bottom"></div> 
                <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server"></asp:ContentPlaceHolder>
            </div>
        </div>
    </div>
    </form>

    <!-- Sticky Footer -->
    <div id="footer" class="visible-desktop">
        <div class="wrap">
            <ul>
                <li><a href="documentation.html?lang=en" class="glyphicons circle_question_mark text" title=""><i></i><span class="hidden-phone">Ajuda</span></a></li>
            </ul>
        </div>
    </div>
<style type="text/css">
    #MainMenu
    {
        background: #333 ;
    }
    .navbar-brand 
    {
        background: url(Resc/images/gradual_logo.png) center / contain no-repeat;
        width: 119px;
    }

</style>
</body>
</html>
