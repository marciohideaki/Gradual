<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <!-- CSS -->
        <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Roboto:400,100,300,500">
        <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
        <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">
		<link rel="stylesheet" href="assets/css/form-elements.css">
        <link rel="stylesheet" href="assets/css/style.css">

        <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
        <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
        <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->

        <!-- Favicon and touch icons -->
        <link rel="shortcut icon" href="assets/ico/favicon.png" />
        <link rel="apple-touch-icon-precomposed" sizes="144x144" href="assets/ico/apple-touch-icon-144-precomposed.png" />
        <link rel="apple-touch-icon-precomposed" sizes="114x114" href="assets/ico/apple-touch-icon-114-precomposed.png" />
        <link rel="apple-touch-icon-precomposed" sizes="72x72" href="assets/ico/apple-touch-icon-72-precomposed.png" />
        <link rel="apple-touch-icon-precomposed" href="assets/ico/apple-touch-icon-57-precomposed.png" />

    <title>Adm de FIDC</title>
</head>
<body onload="Page_Load()">
    <form id="form1" runat="server" class="formulario box-generic" style="padding: 20px 0px 20px 0px">
        <nav class="navbar navbar-inverse  navbar-fixed-top"">
            <div class="container">
                <div class="navbar-header">
                    <a class="navbar-brand text-hide" href="#" >Integração FIDC</a>
                </div>
            </div>
        </nav>

        <div class="top-content" style="padding-top:60px;">
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 text">
                        <h1>Sistema de Integração <strong>(FIDC)</strong></h1>
                    </div>
                </div>
                    
                <div class="row">
                    <div class="col-sm-6 col-sm-offset-3 form-box centered">
                        <div class="form-top">
                            <div class="form-top-left">
                                <h3>Efetue o seu login.</h3>
                                <p>Entre com o usuário e senha para logar:</p>
                            </div>
                            <div class="form-top-right">
                                <i class="fa fa-lock"></i>
                            </div>
                        </div>
                        <div class="form-bottom">
			                <div class="form-group">
			                    <label class="sr-only" for="form-username">Username</label>
                                <asp:textbox ID="txtLogin" name="form-username" placeholder="Usuário..." class="form-username form-control" runat="server" />
			                </div>
			                <div class="form-group">
			                    <label class="sr-only" for="form-password">Password</label>
                                <asp:TextBox TextMode="Password" ID="txtSenha" name="form-password" placeholder="Senha..." class="form-password form-control" runat="server" />
			                </div>
                            <asp:Button ID="btnAutenticar" runat="server"  class="btn btn-primary btn-small" Text="Autenticar" OnClick="btnAutenticar_Click" />

		                </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Sticky Footer -->
        <div id="footer" class="visible-desktop">
            <div class="wrap">
                <ul>
                    <li><a href="documentation.html?lang=en" class="glyphicons circle_question_mark text" title=""><i></i><span class="hidden-phone">Ajuda</span></a></li>
                </ul>
            </div>
        </div>
    </form>

    <script language="javascript">

    function Page_Load()
    {
        document.getElementById("btnAutenticar").focus();
    }

    </script>
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

    #footer {
    height: 35px;
    background: #272729;
    border-top: 1px solid #010101;
    position: fixed;
    bottom: 0;
    z-index: 20000;
    left: 0;
    right: 0;
}

    #footer .wrap {
    background: #272729;
    display: block;
    position: absolute;
    width: 100%;
    height: 100%;
    left: 0;
    padding: 0;
    border-top: 1px solid #494E53;
}

    #footer .wrap>ul {
    margin: 0;
    padding: 0;
    list-style: none;
}

    #footer .wrap>ul>li {
    float: left;
    display: block;
    border-right: 1px solid #232323;
    line-height: 35px;
    height: 35px;
}

</style>
</body>
</html>
