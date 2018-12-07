<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gradual.Site.Www.Integracao.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="Token" runat="server" />
    </div>
    </form>
</body>
    <script type="text/javascript">
        function RedirecionarHB()
        {
            $.redirect('http://localhost:51533/IntegracaoPortalHB.aspx', { 'Host': 'hb.gradualinvestimentos.com.br', 'TokenType': 'Integration2', 'Token': $("#Token").val() });
        }
    </script>
</html>
