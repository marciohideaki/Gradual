<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/Default.js"></script>
    <script src="Scripts/jquery-2.2.3.js"></script>
    <script src="Scripts/jquery.redirect.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="Token" runat="server" />
    <div>

        <input type="button" value="Redirecionar"/>&nbsp;

    </div>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </p>
    </form>

</body>
    <script type="text/javascript">
      $('input').click(function(){
          $.redirect('http://localhost:51533/IntegracaoPortalHB.aspx', { 'Host': 'hb.gradualinvestimentos.com.br', 'TokenType': 'Integration2', 'Token': $("#Token").val() });
      });
  </script>
</html>
