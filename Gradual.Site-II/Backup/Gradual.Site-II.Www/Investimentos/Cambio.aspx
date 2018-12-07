<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Cambio.aspx.cs" Inherits="Gradual.Site.Www.Investimentos.Cambio" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>
<%@ Register src="../Resc/UserControls/ItemCambio.ascx" tagname="ItemCambio" tagprefix="ucItemCambio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section id="pnlConteudoNormal" runat="server" class="PaginaConteudo">

    <div class="row" style="margin-bottom: 24px">
        <div class="col1">
            <img src="../Resc/Skin/Default/Img/banner_interno_cambio.jpg" alt="Câmbio" />
        </div>
    </div>

    <ul style="" class="abas-menu abas-simples">
        <li class="ativo" data-tipolink="Embutida" data-idconteudo="desc-turismo"> 
            <a href="#" id="Aba-DescTurismo">Turismo</a> 
        </li>
        <li data-tipolink="Embutida" data-idconteudo="desc-comercial">
            <a href="#" id="Aba-DescComercial">Comercial</a> 
        </li>
        <li data-tipolink="Embutida" data-idconteudo="desc-contatos">
            <a href="#" id="Aba-DescContatos">Contatos</a> 
        </li>
    </ul>

    <div data-idconteudo="desc-turismo" class="row">

        <div class="col1">
            <p>
               A Gradual oferece ainda mais tranquilidade às suas viagens, com as operações de câmbio turismo. Agora você conta com um serviço diferenciado, 
               além das melhores taxas do mercado.
            </p>
        </div>

        <div>

            <ul style="" class="abas-menu abas-simples">
                <li class="ativo" data-tipolink="Embutida" data-idconteudo="venda-cambio"> 
                    <a href="#" id="Aba-Cambio">Câmbio</a> 
                </li>
                <li data-tipolink="Embutida" data-idconteudo="venda-travel-card-visa">
                    <a href="#" id="Aba-Travel-Card">Visa Travel Money</a> 
                </li>
                <!--li data-tipolink="Embutida" data-idconteudo="venda-travel-card-master">
                    <a href="#" id="Aba-Travel-Card">Comercial - Master</a> 
                </li-->
            </ul>

            <div data-idconteudo="venda-cambio">

                <ul class="ProdutosCambio">

                    <asp:repeater id="rptProdutosCambio" runat="server">
                    <itemtemplate>
                        <li>
                            <ucItemCambio:ItemCambio id="itCambio" modo="Moeda" runat="server" />
                        </li>
                    </itemtemplate>
                    </asp:repeater>
                </ul>

            </div> 

            <div data-idconteudo="venda-travel-card-visa" style="display:none">
    
                <ul class="ProdutosCambio">

                    <asp:repeater id="rptProdutosTravelVisa" runat="server">
                    <itemtemplate>
                        <li>
                            <ucItemCambio:ItemCambio id="itCambio" modo="Visa" runat="server" />
                        </li>
                    </itemtemplate>
                    </asp:repeater>
                </ul>

            </div>

        </div>

    </div>

    <div data-idconteudo="desc-comercial" class="row" style="display:none">
        <div class="col1">

            <p>
            Faça suas transações de câmbio comercial  e transferências financeiras do e para o exterior 
            com o apoio de uma equipe especializada em todas as etapas que envolvem as operações de comércio exterior a preços competitivos.
            </p>

            <p>A Área de Câmbio da Gradual oferece diversos serviços para auxiliá-lo nesta negociação, desde a intermediação à assessoria completa, como:</p>

            <ul class="lista lista-ticada">
                <li>Contratos de Importação e de Exportação;</li>
                <li>Câmbio de transferências financeiras internacionais tais como lucros, dividendos, royalties, ingressos e amortizações de juros sobre empréstimos externos;</li>
                <li>Análises, cadastros e registros das operações junto aos órgãos responsáveis;</li>
                <li>Assessoria para remessa de lucros e dividendos para o exterior;</li>
                <li>Operações Financeiras estruturadas com financiamento externo;</li>
                <li>Terceirização de processos administrativos de câmbio.</li>
            </ul>

        </div>
    </div>
    
    <div data-idconteudo="desc-contatos" class="row" style="display:none">
        <div class="col1">

            <p>
                Para mais informações, entre em contato com a equipe de Câmbio.
            </p>
            <p>
                <img src="../Resc/Skin/Default/Img/img-contato-telefones.gif" /><br />
                Estamos à sua disposição para contato de<br />
                segunda a sexta-feira, das 9h às 18h.
            </p>
            <p>
                <em style="font-weight:bold;color:#222">Telefone</em><br />
                (21) 3388-9090 ou (11) 3074-1244<br /><br />

                <em style="font-weight:bold;color:#222">E-mail</em><br />
                <a href="mailto:cambiorj@gradualinvestimentos.com.br">cambiorj@gradualinvestimentos.com.br</a> ou 
                <a href="mailto:cambiosp@gradualinvestimentos.com.br">cambiosp@gradualinvestimentos.com.br</a> 
                <br /><br />

                <em style="font-weight:bold;color:#222">Skypes</em><br />
                gradualcambioturismo<br />
                gradualcambiocomercial<br />
            </p>

            <div class="row">
                <div class="col3" style="margin-left:31%">

                    <a href="#" onclick="return lnkSejaCorrespondente_Click(this)" title="Seja um Correspondente Cambial">
                        <img src="../Resc/Skin/Default/Img/botao-seja-correspondente.gif" alt="Seja um Correspondente Cambial" />
                    </a>

                </div>
            </div>
	        
            <p style="text-align:center">
                    Acesse <a target="_blank" href="/Resc/Upload/PDFs/CorrespondentesCambiais.pdf" title="Seja um Correspondente Cambial"> aqui </a> e confira a relação de correspondentes cambiais Gradual
	        </p>

            

        </div>
    </div>
    

    <!--div data-idconteudo="venda-travel-card-master" style="display:none">
    
        <ul class="ProdutosCambio">

            <asp:repeater id="rptProdutosTravelMaster" runat="server">
            <itemtemplate>
                <li>
                    <ucItemCambio:ItemCambio id="itCambio" modo="Master" runat="server" />
                </li>
            </itemtemplate>
            </asp:repeater>
        </ul>

    </div-->

</section>


<section id="pnlSejaCorrespondenteContainer" runat="server" class="PaginaConteudo PaginaConteudoPop">

    <div class="row">
        <div class="col1">

            <div id="pnlSejaCorrespondente" class="form-padrao validationEngineContainer" style="display: no_ne;">

                <div class="row">
                    <div class="col1">
                        <div class="campo-basico">
                            <label>Nome</label>
                            <input name="nomecliente" id="nomecliente" placeholder="Nome" class="validate[required] EstiloCampoObrigatorio" type="text">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col6">
                        <div class="campo-basico">
                            <label>Celular (DDD)</label>
                            <input name="cel-ddd" id="cel-ddd" class="validate[required] EstiloCampoObrigatorio" type="text" maxlength="3" style="width:5em">
                        </div>
                    </div>
                    <div class="col6" style="margin-left:-42px;">
                        <div class="campo-basico">
                            <label>Celular (Número)</label>
                            <input name="cel-num" id="cel-num" class="validate[required] EstiloCampoObrigatorio" type="text" maxlength="10">
                        </div>
                    </div>
                    <div class="col6">
                        <div class="campo-basico">
                            <label>Telefone (DDD)</label>
                            <input name="tel-ddd" id="tel-ddd" class="validate[required] EstiloCampoObrigatorio" type="text" maxlength="3" style="width:5em">
                        </div>
                    </div>
                    <div class="col6" style="margin-left:-42px;">
                        <div class="campo-basico">
                            <label>Telefone (Número)</label>
                            <input name="tel-num" id="tel-num" class="validate[required] EstiloCampoObrigatorio" type="text" maxlength="10">
                        </div>
                    </div>
                    <div class="col3" style="width:42%">
                        <div class="campo-basico">
                            <label>E-mail</label>
                            <input name="emailcliente" id="emailcliente" class="validate[required,custom[EmailGambizento] EstiloCampoObrigatorio" type="text">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col6" style="margin-left:39%">
                        <input name="enviar" class="botao btn-padrao cadastro-formulario" value="Enviar" type="submit"> 
                    </div>
                </div>

                <input id="EnviarEmail" value="sim" type="hidden">
                <input id="EnviarEmailPara" value="cambiorj@gradualinvestimentos.com.br;cambiosp@gradualinvestimentos.com.br" type="hidden">
                <input id="AssuntoEmail" value="Cadastro Seja um Correspondente Cambial" type="hidden">
                <input id="TipoEmail" value="Correspondente Cambial" type="hidden">
                <input id="ArquivoDeTemplate" value="SejaCorrespondenteCambial.html" type="hidden">
                <input id="MensagemDeEnvio" value="Sua solicitação foi enviada com sucesso! Aguarde a contato em breve." type="hidden">


            </div>

        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" />

</asp:Content>
