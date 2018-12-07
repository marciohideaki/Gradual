<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReservaIPO.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.IPO.ReservaIPO" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>
<script src="../../../Resc/Js/Lib/jquery-latest.min.js" type="text/javascript">    </script>
<script src="../../../Resc/Js/Lib/jquery-ui-1.10.js"    type="text/javascript">       </script>
<script src="../../../Resc/Js/Site/01-Principal.js"     type="text/javascript"></script>
<script src="../../../Resc/Js/Site/00-Base.js"          type="text/javascript"></script>
<script src="../../../Resc/Js/Lib/unslider.min.js"      type="text/javascript"></script>
<script src="../../../Resc/Js/Site/03-MinhaConta.js" type="text/javascript"></script>
<script src="../../../Resc/Js/Lib/jquery.keyboard.js" type="text/javascript"></script>
<link href="../../../Resc/Skin/Default/01-Principal.css" rel="stylesheet" type="text/css" />
<link href="../../../Resc/Skin/Default/48-keyboard-New.css" rel="stylesheet" type="text/css" />
<link href="https://fonts.googleapis.com/css?family=Roboto:400,400italic,700italic,700" rel="stylesheet" type="text/css">
<body onload="Page_Load()">
<section class="PaginaConteudo">
<form id="Form1" method="post" runat="server">
<input type="hidden" runat="server" id="hddCodigoISIN" />
<input type="hidden" runat="server" id="hddListaIpoModalidade">
<input type="hidden" runat="server" id="hddListaIpoGarantia">
<div class="TituloPopupIPO">
</div>
<div class="row">
<div class="col3-2">
    <div class="box quadro-cinza quadro-aplicacao">
        <div class="form-padrao">
        <h5>Pedido de Reserva</h5>
            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>CONTA:</strong></label>
                        <asp:TextBox id="txt_IPO_Conta" runat="server" />
                    </div>
                </div>
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>NOME:</strong></label>
                        <asp:TextBox id="txt_IPO_Nome"  runat="server"  />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label><strong>CPF:</strong></label>
                        <asp:TextBox   id="txt_IPO_CPF"  runat="server"  />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>EMPRESA:</strong></label>
                        <asp:DropDownList   id="cbo_IPO_Empresa"  runat="server"  onchange="return SolicitacoesGerenciamentoIPO_ChangeIPO(this)" >
                            <asp:ListItem Text="Selecione" Value="0" Selected />
                        </asp:DropDownList>
                    </div>
                </div>
            
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>TIPO:</strong></label>
                        <asp:DropDownList id="cbo_IPO_Modalidade" runat="server" style="width:160px"  >
                            <asp:ListItem >Selecione</asp:ListItem>
                            <asp:ListItem Value="Primaria">Primária</asp:ListItem>
                            <asp:ListItem Value="Secundaria">Secundária</asp:ListItem>
                            <asp:ListItem Value="ON">ON</asp:ListItem>
                            <asp:ListItem Value="PN">PN</asp:ListItem>
                            <asp:ListItem Value="Primaria ON">Primária ON</asp:ListItem>
                            <asp:ListItem Value="Primaria PN">Primária PN</asp:ListItem>
                            <asp:ListItem Value="Secundaria ON">Secundaria ON</asp:ListItem>
                            <asp:ListItem Value="Secundaria PN">Secundária PN</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox   id="txt_IPO_Modalidade"  runat="server"  />--%>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>DATA DA RESERVA:</strong></label>
                        <asp:TextBox ID="txt_IPO_Data" runat="server" CssClass="calendario voa" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>VALOR DA RESERVA:</strong></label>
                        <asp:TextBox ID="txt_IPO_ValorReserva" runat="server"  />
                    </div>
                </div>
            
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>VALOR MÁXIMO:</strong></label>
                        <asp:TextBox ID="txt_IPO_ValorMaximo" runat="server"  />
                        
                    </div>
                </div>
                <div class="col1">
                    <div class="campo-consulta">
                    <label></label>
                    <input type="button" onmouseover="Mostra_divAjuda('ajudaValorMaximo')" onmouseout="Esconde_divAjuda('ajudaValorMaximo')" class="interrogacaoIPO"/>
                    <div class="ajudaValorMaximo">Campo obrigatório. Caso não queira condicionar a sua participação à um valor máximo preencha o campo com 0 (Zero).</div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col5">
                    <div class="campo-consulta">
                        <label><strong>TAXA MÁXIMA:</strong></label>
                        <asp:TextBox ID="txt_IPO_TaxaMaxima" runat="server"  />
                    </div>    
                </div>
                
                <div class="col1">
                    <div class="campo-consulta">
                    <label></label>
                    <input type="button" onmouseover="Mostra_divAjuda('ajudaTaxaMaxima')" onmouseout="Esconde_divAjuda('ajudaTaxaMaxima')" class="interrogacaoIPO"/>
                     <div class="ajudaTaxaMaxima">Campo obrigatório. Caso não queira condicionar a sua participação à uma taxa máxima preencha o campo com 0 (Zero).</div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>% GARANTIAS:</strong></label>
                        <asp:TextBox ID="txt_IPO_PercentualGarantias" runat="server" CssClass="calendario voa" />
                    </div>
                </div>
            
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>Saldo Disponível:</strong></label>
                        <asp:TextBox ID="txt_IPO_SaldoDisponivel" runat="server"  />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>Saldo em Fundos:</strong></label>
                        <asp:TextBox ID="txt_IPO_SaldoFundos" runat="server" />
                    </div>
                </div>
            
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>Valor em Custódia:</strong></label>
                        <asp:TextBox ID="txt_IPO_ValorCustodia" runat="server" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col4">
                    <div class="campo-consulta">
                        <label><strong>Limite Total</strong></label>
                        <asp:TextBox ID="txt_IPO_LimiteTotal" runat="server" />
                    </div>
                </div>
            
                <div class="col4" style="padding-top: 20px">

                <label class="checkbox" ><asp:checkbox ID="chk_IPO_PessoaVinculada" runat="server" Text ="Pessoa Vinculada" style="font-weight:bold"></asp:checkbox></label>
                    <%--<div class="campo-consulta">
                        
                    </div>--%>
                    <input type="button" onmouseover="Mostra_divAjuda('ajudaPessoaVinculada')" onmouseout="Esconde_divAjuda('ajudaPessoaVinculada')" class="interrogacaoIPO"/><div class="ajudaPessoaVinculada">Se você for vinculado à Empresa ofertante marque este campo.</div>
                </div>
                
            </div>
            <div class="row">
                <div class="col4" runat="server" id="pnlAssinaturaEletronica" style="display:none;">
                    <div class="campo-basico campo-senha campo-longo-teclado">
                        <label>Assinatura Eletrônica Atual:</label>
                        <asp:TextBox id="txt_IPO_AssinaturaEletronica" type="password" class="mostrar-teclado" runat="server" />
                        <button class="teclado-virtual-IPO" type="button"><img src="../../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                    </div>
                </div>
                <div class="col4" style="padding-top: 20px">
                    <asp:Button ID="btnConfirmar"  runat="server" cssclass= "botao btn-padrao btn-erica" Text= "CONFIRMAR" onclick="btnConfirmar_Click"  />
                </div>
            </div>
        </div>
    </div>
                        
    <div class="row">
        
    </div>
</div>
</div>
</form>
<script language='javascript'>

    function Page_Load_CodeBehind()
    {
        <asp:literal id="litJavascriptOnLoad" runat="server"></asp:literal>

        $('.teclado-virtual-IPO').click(function () {
            $(this).parent().find('input').getkeyboard().reveal();
            return false;
        });

        $(".mostrar-teclado").keyboard({
            // *** choose layout ***
            layout: 'qwerty',
            customLayout: { 'default': ['{cancel}'] },
            autoAccept: true,
            preventPaste: true,
            usePreview: false
        });

        jQuery.extend($.keyboard.keyaction, 
        {
            enter : function(kb) 
            {
                kb.close(true);

                $(kb.el).closest(".row").find("#btnConfirmar").click();

                return false;
            }
        });
    }

</script>
<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - IPO -  Reserva - IPO" />
</section>
    <div id="pnlMensagemContainer" style="display:none">
        <div id="pnlMensagem">
            <a href="#" onclick="return GradSite_RetornarMensagemAoEstadoNormal()" title="Fechar" class="Fechar"> x </a>

            <p>Mensagem de Alerta porque alguma coisa aconteceu.</p>

            <p>
                <button class="botao btn-padrao btn-erica" onclick="return GradSite_RetornarMensagemAoEstadoNormal()">ok</button>
            </p>
        </div>

        <div id="pnlMensagemAdicional" style="display:none">

            <textarea readonly="readonly">Varios outros erros são possíveis</textarea>

        </div>
    </div>
</body>
