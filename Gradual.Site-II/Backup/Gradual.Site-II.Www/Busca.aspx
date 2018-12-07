<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="Gradual.Site.Www.Busca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h1>Resultados da Busca</h1>

    <div class="row">
        <div class="col3 form-padrao">
            <div class="campo-basico">
                <label for="txtBusca_RealizarNovaBusca">Realizar nova busca:</label>
                <input  id="txtBusca_RealizarNovaBusca" type="text" value="<% =Request["termo"] %>"  onkeydown="return txtBusca_Termo_KeyDown(this, event)" />
            </div>
        </div>
        <div class="col3 form-padrao">
            <div class="campo-basico">
                <label>&nbsp;</label>
                <button onclick="return btnBusca_Click(this)" class="botao btn-padrao btn-erica">ok</button>
            </div>
        </div>
    </div>

    <section class="ResultadosDaBusca">

        <p id="lblNenhumItem" runat="server" visible="false" style="text-align:center">
            Nenhuma página encontrada...
        </p>

        <asp:Repeater runat="server"  ID="rptConteudo">

        <ItemTemplate>
                
            <h3> 
                <a href='<%# Eval("DescURL")%>'><%# Eval("NomePagina")%></a> 
            </h3>

        </ItemTemplate>

        </asp:Repeater>

    </section>

</section>


</asp:Content>
