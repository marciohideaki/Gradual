<%@ Page Language="C#"  MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="PlataformaDeFundos.aspx.cs" Inherits="Gradual.Site.Www.PlataformasDeNegociacao.PlataformaDeFundos" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <div>

        <a class="LinkDeImagem" href="../MinhaConta/NovoCadastro.aspx  ">
            <img style="" class="" alt="" src="../Resc/Upload/Imagens/2014-08/banner-interno-plataforma.jpg" >
        </a>

        <p>A Gradual oferece uma Plataforma com diferentes
        tipos de fundos para você avaliar e fazer a melhor escolha. </p>

        <p>
        A partir de uma metodologia diferenciada, que envolve a
        ponderação de variáveis quantitativas (retorno ajustado por risco, 
        volatilidade e liquidez) e qualitativas (Due Diligence, gestão
        transparente, controle de risco, fidelidade nas posições e visão de mercado),
        construímos a seleção dos melhores gestores preparados para gerir o seu patrimônio. <br>
        </p>

        <p>
            Além disso, esse investimento apresenta uma série de vantagens, como:<br>
        </p>

        <ul class="lista lista-ticada">
            <li>Variedade de opções para diferentes perfis de investidores;</li>
            <li>Desenvolvimento a partir de técnica proprietária;</li>
            <li>Possibilidade de menor custo para aplicação inicial.</li>
        </ul>

        <p>Conte com uma plataforma completa e uma variedade de opções de Fundos!</p>

    </div>

    <ul id="ulAbas" runat="server" class="menu-tabs menu-tabs-full menu-tabs-8" style="">
        <li id="liRecomendados" runat="server" class="ativo" data-idconteudo="plataforma-fundos-1" data-tipolink="Embutida"><a href="#">Recomendados</a> </li>
        <li id="liRendaFixa"    runat="server" data-idconteudo="plataforma-fundos-2" data-tipolink="Embutida"><a href="#">Renda Fixa</a> </li>
        <li data-idconteudo="plataforma-fundos-3" data-tipolink="Embutida"><a href="#">Multimercados</a> </li>
        <li data-idconteudo="plataforma-fundos-4" data-tipolink="Embutida"><a href="#">Ações</a> </li>
        <li data-idconteudo="plataforma-fundos-5" data-tipolink="Embutida"><a href="#">Long and Short </a> </li>
        <li data-idconteudo="plataforma-fundos-6" data-tipolink="Embutida"><a href="#">Cambial</a> </li>
        <li data-idconteudo="plataforma-fundos-7" data-tipolink="Embutida"><a href="#">Exterior</a> </li>
        <li data-idconteudo="plataforma-fundos-8" data-tipolink="Link"><a href="../MinhaConta/Produtos/Fundos/Simular.aspx">Simular</a> </li>
    </ul>
    
    <div id="pnlRecomendados" runat="server" data-idconteudo="plataforma-fundos-1">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeRecomendados">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>  
            <tr id="tdNenhumRecomendado" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>

    <div id="pnlTabelaRendafixa" runat="server" data-idconteudo="plataforma-fundos-2" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeRendaFixa">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>  
            <tr id="tdNenhumRendaFixa" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>


    <div data-idconteudo="plataforma-fundos-3" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeMultimercado">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>  
            <tr id="tdNenhumMultimercado" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>


    <div data-idconteudo="plataforma-fundos-4" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeAcoes">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>
            <tr id="tdNenhumAcoes" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>


    <div data-idconteudo="plataforma-fundos-5" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeReferenciado">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>
            <tr id="tdNenhumReferenciado" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>


    <div data-idconteudo="plataforma-fundos-6" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeCambial">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>  
            <tr id="tdNenhumCambial" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>


    <div data-idconteudo="plataforma-fundos-7" style="display:none">
        <table class="tabela">
            <tbody>
                <tr class="tabela-titulo">
                    <td>Fundos</td>
                    <td>Risco</td>
                    <td>Apl.<br> Inicial</td>
                    <td>Apl.<br> Adic.</td>
                    <td>Tx. Adm(%)</td>
                    <td>Resg. Mín.</td>
                    <td>Hor.</td>
                    <td>Rent. Mês(%)</td>
                    <td>12 Meses(%)</td>
                    <td>Detalhes</td>
                    <td class="alignCenter">Investir</td>
                </tr>
            <asp:Repeater runat="server" ID="rptListaDeExterior">
            <itemtemplate>
                <tr class="tabela-type-small">
                    <td><%# Eval("Fundo") %></td>
                    <td class="alignCenter"><span <%# Eval("Risco").ToString().ToLower() == "alto" ? "class='bolinha-vermelho' title='Arrojado'" :  Eval("Risco").ToString().ToLower() == "baixo" ? "class='bolinha-verde' title='Conservador'" : "class='bolinha-amarelo' title='Moderado'"  %> ></span></td>
                    <td><%# Eval("AplicacaoInicial")%></td>
                    <td><%# Eval("AplicacaoAdicional")%></td>
                    <td><%# Eval("TaxaAdministracao")%></td>
                    <td><%# Eval("ResgateMinimo")%></td>
                    <td><%# Eval("HorarioLimite")%></td>
                    <td><%# Eval("RentabilidadeMes")%></td>
                    <td><%# Eval("Rentabilidade12meses") %></td>
                    <td><a class="botao btn-saibamais" href="../MinhaConta/Produtos/Fundos/Detalhes.aspx?idFundo=<%#Eval("IdProduto")%>">Detalhes</a></td>
                    <td><%# Eval("BotaoAplicar") %></td>
                </tr>
            </itemtemplate>
            </asp:Repeater>  
            <tr id="tdNenhumExterior" runat="server" visible="false" class="NenhumItem">
                <td colspan="11">
                (Nenhum fundo encontrado)
                </td>
            </tr>
            </tbody>
        </table>
    </div>
    
    <ucSauron:Sauron id="Sauron1" runat="server" />

</section>

</asp:Content>