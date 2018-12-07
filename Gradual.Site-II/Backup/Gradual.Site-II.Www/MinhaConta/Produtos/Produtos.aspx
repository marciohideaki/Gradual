<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Produtos.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Produtos.Produtos" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/GTI.ascx" tagname="GTI" tagprefix="uc1" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/StockMarket.ascx" tagname="StockMarket" tagprefix="uc2" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/TravelCard.ascx" tagname="TravelCard" tagprefix="uc3" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/SeguroVida.ascx" tagname="SeguroVida" tagprefix="uc4" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/Previdencia.ascx" tagname="Previdencia" tagprefix="uc5" %>
<%@ Register src="~/Resc/UserControls/MinhaConta/Produtos/ProfitChart.ascx" tagname="ProfitChart" tagprefix="uc6" %>

<%@ Register src="~/Resc/UserControls/BannerLateral.ascx" tagname="BannerLateral" tagprefix="ucBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

<h2>Produtos</h2>

<div class="row">
    <div class="col1">
        <img class="atuacao-banner" src="../../Resc/Skin/Default/Img/Banner-Minhaconta-Produtos.jpg" alt="" />
    </div>
</div>

<p>Confira a seguir a relação de produtos oferecidos pela Gradual, que você ainda pode contratar, e todos os contratados até o momento.</p>
                
    <div class="row">
        <div class="col3-2">
            <div>
                <ul class="abas-menu">
                    <li data-IdConteudo="ProdutosContratar" class="ativo"> <a href="#" id="Aba-Contratar"><span class="carrinho">Contratar</span></a> </li>
                    <li data-IdConteudo="ProdutosContratados"> <a href="#" id="Aba-Contratados">Meus Produtos</a> </li>
                </ul>

                <div class="abas-conteudo">

                    <div data-IdConteudo="ProdutosContratar" class="aba">
                        <ul class="acordeon acordeon-produto">
                            <li>
                                <div class="acordeon-opcao"  > Stock Market  <span><asp:Label ID="lblAdquiridoStockMarket" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">
                                    
                                    <uc2:StockMarket ID="StockMarket1" runat="server" />
                                    
                                </div>
                            </li>

                            <li >
                                <div class="acordeon-opcao" >Vida <span><asp:Label ID="lblAdquiridoSeguroVida" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">
                                    
                                    <uc4:SeguroVida ID="SeguroVida1" runat="server" />
                                    
                                </div>
                            </li>

                            <li >
                                <div class="acordeon-opcao" >Previdência <span><asp:Label ID="lblAdquiridoPrevidencia" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">
                                    
                                    <uc5:Previdencia ID="Previdencia1" runat="server" />
                                    
                                </div>
                            </li>

                            <li >
                                <div class="acordeon-opcao" >ProfitChart <span><asp:Label ID="lblAdquiridoProfitChart" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">

                                    <uc6:ProfitChart ID="ProfitChart1" runat="server" />

                                </div>
                            </li>
                                        
                            <li>
                                <div class="acordeon-opcao" >Fundos de Investimento <span><asp:Label ID="lblAdquiridoFundosInvestimento" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">
                                    <h5>O que é?</h5>
                                    <p>Os Fundos de Investimento da Gradual têm excelentes taxas de rentabilidade são excelentes alternativas para todos os tipos de investidor: conservador, moderado ou arrojado. </p>
                                                
                                    <p>Confira algumas vantagens que os Fundos oferecem: </p>

                                    <ul style="list-style-type:circle;  margin-left:25px">
                                        <li>Comodidade de ter os seus recursos geridos por especialistas;</li>
                                        <li>Baixo aporte inicial;</li>
                                        <li>Diversificação do portfólio em um único investimento.</li>
                                    </ul>

                                    <p>
                                    Mas não se esqueça! Consulte as informações de cada fundo e veja o que melhor se adapta ao seu perfil e necessidade.
                                    </p>

                                    <h5>Quanto custa?</h5>

                                    <h6>Taxa de Administração</h6>

                                    <p>É cobrada de acordo com o regulamento de cada Fundo de Investimento. </p>

                                    <h6>Taxa de Performance</h6>

                                    <p>Pode ser cobrada, de acordo com o regulamento do produto. Trata-se de um percentual do que excede a rentabilidade de seu índice de referência (benchmark), recolhida semestralmente e acumulada diariamente.</p>

                                    <h5>Ajuda</h5>

                                    <p>Para investir em Fundos, basta:</p>

                                    <ul style="list-style-type:circle;  margin-left:25px; line-height:20px;">
                                        <li>Clicar abaixo em "Acessar";</li>
                                        <li>Escolher o Fundo de Investimento desejado e clicar em "Aplicar";</li>
                                        <li>Enviar os recursos da aplicação via DOC ou TED para a conta da Gradual no Banco BM&F (096): Agência: 001. C/C 326-5. Favorecido: Gradual CCTVM S/A. CNPJ: 33.918.160/0001-73.</li>
                                    </ul>
                                                
                                    <a class="botao btn-padrao" href="Fundos/Recomendados.aspx" >Acessar</a>
                                </div>
                            </li>
                                        
                            <li >
                                <div class="acordeon-opcao" >GTI <span><asp:Label ID="lblAdquiridoGTI" runat="server"  /></span></div>
                                <div class="acordeon-conteudo"> 
                                    
                                    <uc1:GTI ID="GTI1" runat="server" />
                                    
                                </div>
                            </li>
                            <li >
                                <div class="acordeon-opcao" >Tesouro Direto <span><asp:Label ID="lblAdquiridoTesouroDireto" runat="server"  /></span></div>
                                <div class="acordeon-conteudo">
                                    <h5>O que é?</h5>
                                     <p>Investir no Tesouro Direto é aplicar o seu dinheiro com risco mínimo no
                                      mercado e rentabilidade garantida pelo Tesouro Nacional.<br />
                                      Essa é uma ótima alternativa de Renda Fixa, que oferece comodidade e segurança aos investidores e ainda:
                                    </p>
                                    <ul style="list-style-type:circle; padding-left:25px">
                                        <li>Baixa taxa de administração;</li>
                                        <li>Aplicação ideal para longo prazo;</li>
                                        <li>Retorno competitivo comparado a outras aplicações de Renda Fixa.</li>
                                    </ul>
                                    <p>
                                        Para se cadastrar no Tesouro Direto, entre em contato com nosso Atendimento:
                                    </p>
                                    <p>
                                        4007-1873 | Capitais e Regiões Metropolitanas<br />
                                        0800 655 1873 | Demais Localidades <br />
                                        Caso já tenha se cadastrado, clique em acessar
                                    </p>

                                    <a class="botao btn-padrao" href="../Posicao/RendaFixa.aspx" >Acessar</a>
                                </div>
                            </li>
                        </ul>
                    </div>

                    <div data-IdConteudo="ProdutosContratados" class="aba">
                        <ul class="acordeon acordeon-produto acordeon-sem-expansao">
                            <asp:repeater id="rptListaDeFerramentas" runat="server">
                            <itemtemplate>
                                <li >
                                    <div class="acordeon-opcao" ><%# Eval("DescricaoProduto") %> <span>Adquirido em  <%# Convert.ToDateTime(Eval("DataDaCompra")).ToString("dd/MM/yyyy HH:mm") %></span></div>
                                    <!--div class="acordeon-conteudo">
                                        <%# Eval("Text") %>
                                    </div-->
                                </li>
                            </itemtemplate>
                            </asp:repeater>
                            
                            <li id="liNenhumProduto" class="NenhumItem" runat="server">
                                <div class="acordeon-opcao" >Nenhum produto contratado.</div>
                            </li>

                        </ul>

                        <h5>Compras efetuadas</h5>
                        <table class="tabela">
                        <thead>
                            <tr class="tabela-titulo">
                                <td>Data</td>
                                <td>Status</td>
                                <td>Quantidade x Produto</td>
                                <td>Total da Compra</td>
                                <td>Método de Pagamento</td>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:repeater id="rptListaDeCompras" runat="server">
                            <itemtemplate>
                            <tr>
                                <td><%# Eval("Dados_Data") %>         </td>
                                <td><%# Eval("Dados_Status") %>       </td>
                                <td><%# Eval("ProdutosLegiveis") %>   </td>
                                <td><%# Eval("Dados_Total") %>        </td>
                                <td><%# Eval("PagamentosLegiveis") %> </td>
                            </tr>
                            </itemtemplate>

                            </asp:repeater>

                            <tr id="trNenhumaCompra" class="NenhumItem" runat="server">
                                <td colspan="5"> Nenhuma compra realizada. </td>
                            </tr>

                        </tbody>

                    </table>
                    </div>

                </div>

            </div>
        </div>

        <div class="col3">

            <ucBL:BannerLateral ID="bnlLeste" runat="server" Posicao="L" />

        </div>
    </div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Produtos" />

</asp:Content>
