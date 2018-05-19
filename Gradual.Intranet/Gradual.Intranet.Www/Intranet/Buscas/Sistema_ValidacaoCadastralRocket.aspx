<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sistema_ValidacaoCadastralRocket.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Sistema_ValidacaoCadastralRocket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Busca_Formulario Busca_Formulario_1Linha">

        <p style="width:340px">
            <label for="cboBusca_Sistema_Rocket_Status">Status:</label>
            <select id="cboBusca_Sistema_Rocket_Status" style="width:250px">
                <option value="">[Selecione]</option>
                <option value="AtividadesIlicitas">Atividades Ilícitas</option>
                <option value="PaisesEmListaNegra">Países em Lista Negra</option>
                <option value="Contratos">Contratos</option>
                <option value="TiposDePendenciaCadastral">Tipos de Pendência Cadastral</option>
                <option value="TaxasDeTermo">Taxas de Termo</option>
            </select>
        </p>

        <p  class="LadoALado_Pequeno" style="width: 88px;float:right">
            <button onclick="return btnBusca_Click()">Carregar Lista</button>
        </p>

    </div>
    </form>
</body>
</html>
