<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapaDoSite.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MapaDoSite" %>

<div id="pnlMapaDoSite" class="PainelOverlay" style="display:none">

    <fieldset>
        <label for="txtMapaDoSite_BuscaRapida">Busca Rápida:</label>
        <input  id="txtMapaDoSite_BuscaRapida" type="text" onfocus="txtMapaDoSite_BuscaRapida_Focus(this)" onkeydown="return txtMapaDoSite_BuscaRapida_KeyDown(this, event)" />
    </fieldset>

    <ul class="UmaLinha">

        <li class="SubTitulo"> Corporativo: </li>
        <li> <a href="http://www.gradualeducacional.com">Gradual Educacional</a> </li>

    </ul>

    <ul>

        <li class="SubTitulo"> <a href="<%= this.HostERaiz %>/Ferramentas/">Ferramentas</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Ferramentas/GradualTraderInterface.aspx">Gradual Trader Interface</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Ferramentas/HomeBroker.aspx">Home Broker</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Ferramentas/StockMarket.aspx">Gradual Stock Market</a> </li>

        <li class="SubTitulo"> <a href="#">Produtos</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/FundosDeInvestimento.aspx">Fundos de Investimento</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/ClubesDeInvestimento.aspx">Clubes de Investimento</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/ContaMargem.aspx">Conta Margem</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/Educacional.aspx">Educacional</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/TravelCard.aspx">Gradual Travel Card</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Produtos/Previdencia.aspx">Previdência</a> </li>
    </ul>

    <ul>

        <li class="SubTitulo"><a href="<%= this.HostERaiz %>/ComoInvestir/">Como Investir</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/AbraSuaConta.aspx"><asp:Label ID="lblMapaSiteComoInvestirAbraSuaConta" runat="server"></asp:Label></a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/ConhecaAGradual.aspx">Conheça a Gradual</a></li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/NossosEnderecos.aspx">Nossos Endereços</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/NossosDistribuidores.aspx">Nossos Distribuidores</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/MesasEspecializadas.aspx">Mesas Especializadas</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/InvestidorInstitucional.aspx">Investidor Institucional</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/CustosOperacionais.aspx">Custos Operacionais</a> </li>
        <li> <a href="<%= this.HostERaiz %>/ComoInvestir/ContratosEDocumentacao.aspx">Contratos e Documentação</a> </li>
        
        <li class="SubTitulo"> Aprendizado </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/Palestras.aspx">Palestras</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/Planejamento.aspx">Planejamento</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/LegislacaoERegras.aspx">Legislação e Regras</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/SegurancaERiscos.aspx">Segurança e Riscos</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/DuvidasFrequentes.aspx">Dúvidas Frequentes</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Aprendizado/Glossario.aspx">Glossário</a> </li>

    </ul>

    <ul>

        <li class="SubTitulo"> <a href="<%= this.HostERaiz %>/Investimentos/">Investimentos</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/Acoes.aspx">Ações</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/Opcoes.aspx">Opções</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/OfertaPublica.aspx">Oferta Pública</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/Termo.aspx">Termo</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/AluguelDeAcoes.aspx">Aluguel de Ações</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/MercadoriasEFuturos">Mercadorias e Futuros</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/TesouroDireto.aspx">Tesouro Direto</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/FundosImobiliarios.aspx">Fundos Imobiliários</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Investimentos/RendaFixa.aspx">Renda Fixa</a> </li>
        
    </ul>

    <ul>

        <li class="SubTitulo"> Análises e Mercado </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/Chats.aspx">Chats</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/Carteiras Recomendadas.aspx">Carteiras Recomendadas</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/AnalisesFundamentalistas.aspx">Análises Fundamentalistas</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/AnalisesEconomicas.aspx">Análises Econômicas</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/FerramentasDeAnalises.aspx">Ferramentas de Análises</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/Noticias.aspx">Notícias</a> </li>
        <li> <a href="<%= this.HostERaiz %>/AnalisesEMercado/Avisos.aspx">Avisos</a> </li>
        
    </ul>

    <ul class="UmaLinha">

        <li class="SubTitulo"> Institucional: </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/Atendimento.aspx">Atendimento</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/Ouvidoria.aspx">Ouvidoria</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/SejaUmDistribuidor.aspx">Seja um Distribuidor</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/TrabalheConosco.aspx">Trabalhe Conosco</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/SalaDeImprensa.aspx">Sala de Imprensa</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/ForeignInvestors.aspx">Foreign Investors</a> </li>
        <li> <a href="<%= this.HostERaiz %>/Institucional/NikkeiDesk.aspx">Nikkei Desk</a> </li>

    </ul>
    
    <ul class="UmaLinha">

        <li class="SubTitulo"> <a href="<%= this.HostERaizHttps %>/MinhaConta/">Minha Conta</a> </li>
        
    </ul>
    
    <ul class="MinhaConta">

        <li class="SubTitulo"> Financeiro </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Financeiro/SaldosLimites.aspx">Saldos / Limites</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Financeiro/Extrato.aspx">Extrato </a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Financeiro/Retirada.aspx">Retirada</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Financeiro/Deposito.aspx">Depósito</a> </li>

        <li class="SubTitulo"> Poupe Gradual </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/PoupeGradual/AderirPlanos.aspx">Aderir Planos</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/PoupeGradual/SolicitarRetirada.aspx">Solicitar Retirada</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/PoupeGradual/Rentabilidade.aspx">Rentabilidade</a> </li>

    </ul>
    
    <ul class="MinhaConta" style="width:184px">

        <li class="SubTitulo"> Bovespa / BM&amp;F </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Bovespa-BMF/PosicaoBovespa.aspx">Posição Bovespa</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Bovespa-BMF/NotaDeCorretagem.aspx">Nota de Corretagem</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Bovespa-BMF/PosicaoBMF.aspx">Posição BM&F</a> </li>

        <li class="SubTitulo"> Imposto de Renda </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ImpostoDeRenda/CalculadoraIR.aspx">Calculadora IR</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ImpostoDeRenda/InformesComprovantes.aspx">Informes e Comprovantes</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ImpostoDeRenda/RegrasGerais.aspx">Regras Gerais</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ImpostoDeRenda/DuvidasFrequentes.aspx">Dúvidas Frequentes</a> </li>
        
    </ul>
    
    <ul class="MinhaConta" style="width:148px">

        <li class="SubTitulo"> Clubes e Fundos </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ClubesFundos/PosicaoClubes.aspx">Posição Clubes</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ClubesFundos/HistoricoClubes.aspx">Histórico Clubes</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ClubesFundos/PosicaoFundos.aspx">Posição Fundos</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/ClubesFundos/HistoricoFundos.aspx">Histórico Fundos</a> </li>

        <li class="SubTitulo"> Operações </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Operacoes/EnvioDeOrdens.aspx">Envio de Ordens</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Operacoes/Acompanhamento.aspx">Acompanhamento</a> </li>
        
    </ul>
    
    <ul class="MinhaConta" style="width:146px">

        <li class="SubTitulo"> Tesouro Direto </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/TesouroDireto/Compra.aspx">Compra </a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/TesouroDireto/Venda.aspx">Venda</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/TesouroDireto/Consulta.aspx">Consulta </a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/TesouroDireto/Extrato.aspx">Extrato</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/TesouroDireto/ConsultarProtocolo.aspx">Consultar Protocolo</a> </li>
        
    </ul>
    
    <ul class="MinhaConta" style="width:192px">

        <li class="SubTitulo"> Cadastro </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/DadosCadastraisPF.aspx"><asp:Label ID="lblMapaSiteMinhaContaCadastroMeuCadastro" runat="server"></asp:Label></a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/SolicitarAlteracao.aspx">Alterar Cadastro</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Compras/MeusProdutos.aspx">Meus Produtos</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Compras/Carrinho.aspx">Carrinho de Compras</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/AlterarSenha.aspx">Alterar a Senha</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/AlterarAssinaturaEletronica.aspx">Alterar Assinatura Eletrônica</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/EsqueciMinhaSenha.aspx">Esqueci Minha Senha</a> </li>
        <li> <a href="<%= this.HostERaizHttps %>/MinhaConta/Cadastro/EsqueciMinhaAssinatura.aspx">Esqueci Ass. Eletrônica</a> </li>

    </ul>

</div>