<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Custodia.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Financeiro.Informe.Custodia" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasFinanceiro.ascx"  tagname="AbasFinanceiro"  tagprefix="ucAbasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro1" modo="menu" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Informe de Rendimentos <!--span class="icon3">(de 19/01/2014 às 18:01)</span--></h3>

                <ul>
<%--                    <li class="conta">
                        <input type="button" onmouseover="Mostra_divAjuda('conta_msg')" onmouseout="Esconde_divAjuda('conta_msg')" />
                        <div class="conta_msg" style="display:none">
                        Confira os dados bancários da Gradual:<br />
                        Banco BM&F - 096<br />
                        Agência: 001<br />
                        C/C: 326-5<br />
                        Favorecido: Gradual CCTVM S/A<br />
                        CNPJ: 33.918.160/0001-73
                        </div>
                    </li>--%>
                    <li class="interrogacao"><button type="button" onmouseover="Mostra_divAjuda('ajuda')" onmouseout="Esconde_divAjuda('ajuda')">?</button><div class="ajuda">No demonstrativo, são listados os seus rendimentos em renda fixa e variável, Saldo Financeiro, em Custódia e saldo em Fundos no final do ano.</div></li>
                    <%--<li class="exportar">
                        <button type="button" title="Exportar">Exportar</button>
                        <ul>
                            <li><a class="pdf" href="#">PDF</a></li>
                            <li><a class="excel" href="#">Excel</a></li>
                        </ul>
                    </li>--%>
                    <li class="email"><button type="button" title="Enviar por e-mail">Enviar por email</button></li>
                </ul>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col1">
            <div id="abas-generico" class="abas-counte">

                <ucAbasF:AbasFinanceiro id="ucAbasFinanceiro2" modo="submenu" runat="server" />

                <div id="abas-conteudo" class="abas-conteudo">
                    <div class="aba">
                        <p>Escolha um ano de exercício para fazer a consulta de seu extrato de custódia.</p>

                        <div class="form-padrao">
                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Ano de exercício</label>
                                        <asp:DropDownList id = "cboAnoDeExercicio" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3">
                                    <input class="botao btn-padrao btn-erica" type="submit" value="Consultar" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Custódia" />

</asp:Content>
