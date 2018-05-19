<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="STIntegracaoPortalHB.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html{}
        body
        {
            margin: 0;
            padding: 0;
            background: #dbdad6;
            font-family: Arial, Helvetica, sans-serif;
        }
        *
        {
            margin: 0;
            padding: 0;
        }
        p
        {
            font-size: 12px;
            color: #373737;
            font-family: Arial, Helvetica, sans-serif;
            line-height: 18px;
        }
        p a
        {
            color: #218bdc;
            font-size: 12px;
            text-decoration: none;
        }
        a{
            outline: none;
        }
        .f-left
        {
            float:left;
        }
        .f-right
        {
            float:right;
        }
        .clear
        {
            clear: both;
            overflow: hidden;
        }
        #block_error
        {
            width: 845px;
            height: 384px;
            border: 1px solid #cccccc;
            margin: 72px auto 0;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            background: #fff url(images/exclamation.png) no-repeat 0 51px;
        }

        #block_error_content
        {
            padding: 100px 40px 0 186px;
        }
        #block_error div h2
        {
            color: #4d443d;
            font-size: 24px;
            display: block;
            padding: 0 0 14px 0;
            border-bottom: 1px solid #cccccc;
            margin-bottom: 12px;
            font-weight: normal;
        }
        #footer 
        { 
            background: url(images/footer-bg.jpg); 
            height: 330px; 
            background-repeat: repeat-x; 
            padding-bottom: 20px; 
            margin-top:20px; 

        }
        #logo
        {
            padding: 5px 5px 0 5px;
            background: #4e443c url(images/gradual_logo.png) no-repeat; 
            height: 36px; 
        }
        summary 
        {
            font-size: 12px;
            color: #373737;
            font-family: Arial, Helvetica, sans-serif;
            line-height: 18px;
        }

        details > p {
	        margin-left: 24px;
        }
        
        #error-description
        {
            font-size: 12px;
            color: #373737;
            font-family: Arial, Helvetica, sans-serif;
            height: 85px;
            overflow: scroll;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="logo"></div>
        <div id="block_error">
            <div id="block_error_content">
                <h2>Oops! Estamos passando por dificuldades!</h2>
                <p>
                    Estamos temporariamente impossibilitados de atender sua solicitação. <br />
                    Por favor, tente novemente em alguns instantes.
                </p>
                <br />
                <p>
                    Se você tiver qualquer dúvida, por favor entre em contato com o Suporte.
                </p>
                <br />
                <div>
                    <section>
                        <article>
                            <details>
                                <summary>Detalhes do Erro</summary>
                                <div id="error-description">Mensagem: <% Response.Write(lMessage);%> <br /> Stack: <% Response.Write(lStackTrace); %></div>
                            </details>
                        </article>
                    </section>
                </div>            
            </div>

        </div>
        <div id="footer"></div>
    </form>
</body>
</html>
