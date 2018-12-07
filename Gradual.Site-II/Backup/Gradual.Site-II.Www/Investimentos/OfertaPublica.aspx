<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="OfertaPublica.aspx.cs" Inherits="Gradual.Site.Www.Investimentos.OfertaPublica" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>
<%@ Register src="../Resc/UserControls/RenderizadorDeWidgets.ascx" tagname="RenderizadorDeWidgets" tagprefix="ucRenderizador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="PaginaConteudo">

        <ucRenderizador:RenderizadorDeWidgets id="rdrConteudo" runat="server" />
        
        <section id="pnlDadosDeCompra" runat="server" class="ContainerDadosDeCompra">

            <div id="pnlDadosDeCompra_SemLogin" runat="server" visible="false">

                <h1>Adquira Agora essa Oferta Pública!</h1>
                <p>
                    Para poder adquirir esse produto, <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/CadastroPF.aspx">cadastre-se</a> ou <a href="<%= this.HostERaizHttps %>/MinhaConta/Login.aspx?r=~/Investimentos/OfertaPublica.aspx">faça login</a>.
                </p>

            </div>
            
            <div id="pnlDadosDeCompra_SemPasso" runat="server" visible="false">

                <h1>Adquira Agora essa Oferta Pública!</h1>
                <p>
                    Para poder adquirir esse produto, <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/Default.aspx">complete seu cadastro</a>.
                </p>

            </div>

            <div id="pnlDadosDeCompra_SemISIN" runat="server" visible="false">

                <!--h1>Sem dados de ISIN para a Oferta</h1-->
                &nbsp;

            </div>
            
            <div id="pnlDadosDeCompra_DataPassada" runat="server" visible="false">

                <h1>Essa Oferta Pública já expirou sua validade em <asp:literal id="lblDadosDeCompra_DataPassada" runat="server"></asp:literal> </h1>

            </div>

            <div id="pnlDadosDeCompra_RealizarCompra" runat="server" visible="false">

                <h1>Adquira Agora a Oferta</h1>

                <fieldset class="FormularioPadrao">

                    <p class="BotoesDeReserva" style="text-align:center">
                        <button id="btnPedidoReservaNormal" runat="server" class="botao btn-padrao" style="width:40%" visible="false" onclick="return btnPedidoDeReserva_Click(this)">Realizar Pedido de Reserva</button>
                        <button id="btnPedidoPrioritarioPN" runat="server" class="botao btn-padrao" style="width:40%" visible="false" onclick="return btnPedidoDeReserva_Click(this)">Realizar Pedido de Reserva Prioritário - Ações Prioritárias</button>
                        <button id="btnPedidoVarejoPN"      runat="server" class="botao btn-padrao" style="width:40%" visible="false" onclick="return btnPedidoDeReserva_Click(this)">Realizar Pedido de Reserva de Varejo - Ações Prioritárias</button>
                        <button id="btnPedidoPrioritarioON" runat="server" class="botao btn-padrao" style="width:40%" visible="false" onclick="return btnPedidoDeReserva_Click(this)">Realizar Pedido de Reserva Prioritário - Ações Ordinárias</button>
                        <button id="btnPedidoVarejoON"      runat="server" class="botao btn-padrao" style="width:40%" visible="false" onclick="return btnPedidoDeReserva_Click(this)">Realizar Pedido de Reserva de Varejo - Ações Ordinárias</button>
                    </p>

                </fieldset>

                <input id="hidIPO_URL" type="hidden" runat="server" />

            </div>

        </section>

    </section>

    <div class="BotoesOfertaPublica" style="display:none">

    </div>

    <ucSauron:Sauron id="Sauron1" runat="server" />

</asp:Content>

