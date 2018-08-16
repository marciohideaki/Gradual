<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Sistema_ObjetosDoSistema.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Sistema_ObjetosDoSistema" %>

<form id="form1" runat="server">

    <div class="Busca_Formulario Busca_Formulario_1Linha">

        <p style="width:340px">
            <label for="cboBusca_Sistema_ObjetosDoSistema">Objeto:</label>
            <select id="cboBusca_Sistema_ObjetosDoSistema" style="width:250px">
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