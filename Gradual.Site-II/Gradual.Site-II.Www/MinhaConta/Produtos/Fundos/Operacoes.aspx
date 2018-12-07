<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operacoes.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Fundos.Operacoes" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/AbasFundosInvestimentos.ascx" tagname="AbasFundosInvestimentos" tagprefix="uc1" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesAplicar.ascx" tagname="OperacoesAplicar" tagprefix="uc2" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesPosicaoConsolidada.ascx" tagname="OperacoesPosicaoConsolidada" tagprefix="uc3" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesResgate.ascx" tagname="OperacoesResgate" tagprefix="uc4" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesExtrato.ascx" tagname="OperacoesExtrato" tagprefix="uc5" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesSimular.ascx" tagname="OperacoesSimular" tagprefix="uc6" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesRelatorio.ascx" tagname="OperacoesRelatorio" tagprefix="uc7" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Fundos/OperacoesSaldo.ascx" tagname="OperacoesSaldo" tagprefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="PaginaConteudo">
<div class="row">
    <div class="col1">
        <uc1:AbasFundosInvestimentos ID="AbasFundosInvestimentos1" modo="menu" runat="server" />
    </div>
</div>
<div class="row">
    <div class="col1">
        <div class="menu-exportar clear">
            <h3>Operações</h3>
            
        </div>
    </div>
</div>
                
<div class="row">
    <div class="col1">
        <div >
            <ul class="abas-menu">
                <li class="ativo" data-IdConteudo="Aplicar"> <a href="#" id="Aplicar">Aplicar</a></li>
                <li data-IdConteudo="PosicaoConsolidada"><a href="#" id="PosicaoConsolidada">Posição Consolidada</a></li>
                <li data-IdConteudo="Resgate"><a href="#" id="Resgate">Resgate</a></li>
                <li data-IdConteudo="Extrato"><a href="#" id="Extrato">Extrato</a></li>
                <li data-IdConteudo="Simular"><a href="#" id="li_AbaSimular" >Simular</a></li>
                <li data-IdConteudo="Relatorio"><a href="#" id="li_AbaRelatorios">Relatório</a></li>
                <li data-IdConteudo="Saldo"><a href="#" id="Saldo">Saldo</a></li>
            </ul>
            <input type="hidden" id="Posicao_Aba_Simular_Selecionada" runat="server" />
            <%--<input type="hidden" id="Posicao_Aba_Relatorios_Selecionada" runat="server" />--%>
            <div class="abas-conteudo">
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Aplicar">
                    <uc2:OperacoesAplicar ID="OperacoesAplicar1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="PosicaoConsolidada">
                    <uc3:OperacoesPosicaoConsolidada ID="OperacoesPosicaoConsolidada1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Resgate">
                    <uc4:OperacoesResgate ID="OperacoesResgate1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Extrato">
                    <uc5:OperacoesExtrato ID="OperacoesExtrato1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Simular">
                <uc6:OperacoesSimular ID="OperacoesSimular1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Relatorio">
                    <uc7:OperacoesRelatorio ID="OperacoesRelatorio1" runat="server" />
                </div>
                <!-- fim ABA -->
                                
                <!-- ABA -->
                <div class="aba" data-IdConteudo="Saldo">
                    <uc8:OperacoesSaldo ID="OperacoesSaldo1" runat="server" />
                </div>
                <!-- fim ABA -->
            </div>
</div>
</div>
</div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Prod - Fds - Operações" />

</asp:Content>
