<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="MeuPerfil.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.MeuPerfil" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasMeuCadastro.ascx"  tagname="AbasMeuCadastro"  tagprefix="ucAbasM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h2>Meu Cadastro</h2>

    <ucAbasM:AbasMeuCadastro id="ucAbasMeuCadastro1" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Meu Perfil</h3>
            </div>
        </div>
    </div>

    <div id="pnlResultado" runat="server" class="row">

        <div id="divResultado_Conservador" runat="server" style="display:none" class="col1">
            <h4 class="perfil-usuario">Seu Perfil é <strong>Conservador</strong></h4>

<%--            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
            com base nas diretrizes dos órgãos reguladores do mercado financeiro e no questionário respondido  
            após o seu cadastro na Gradual.</p>
            <ul class="lista lista-quadrado-amarelo">
                <li>Tesouro Direto</li>
                <li>Títulos Públicos</li>
                <li>CDB - Certificado de Depósito Bancário</li>
                <li>LCI - Letra de Crédito Imobiliário</li>
                <li>LCA - Letra de Crédito do Agronegócio</li>
                <li>LF - Letra Financeira</li>
                <li>LC - Letra de Câmbio</li>
                <li>CRI - Certificação de Recebível Imobiliário (Investidor Qualificado)</li>
                <li>Clubes de Investimentos</li>
                <li>Fundos de Investimento Renda Fixa</li>
            </ul>--%>

            <p>Para o perfil <b>Conservador</b>, devido à baixa tolerância a riscos, é considerada aceitável a rentabilidade da carteira entre <b>75% do CDI</b> e <b>125% do CDI</b>.</p>
        </div>

        <div id="divResultado_Moderado" runat="server" style="display:none" class="col1">
            <h4 class="perfil-usuario">Seu Perfil é <strong>Moderado</strong></h4>
<%--            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
            com base nas diretrizes dos órgãos reguladores do mercado financeiro e no questionário respondido 
            após o seu cadastro na Gradual.</p>
            <ul class="lista lista-quadrado-amarelo">
                <li>Tesouro Direto</li>
                <li>Títulos Públicos</li>
                <li>CDB - Certificado de Depósito Bancário</li>
                <li>LCI - Letra de Crédito Imobiliário</li>
                <li>LCA - Letra de Crédito do Agronegócio</li>
                <li>LF - Letra Financeira</li>
                <li>LC - Letra de Câmbio</li>
                <li>Fundos de Investimento Renda Fixa</li>
                <li>Aluguel de Ações</li>
                <li>Fundos de Investimento Multimercado</li>
                <li>Debêntures (Investidor Qualificado)</li>
                <li>CRI - Certificação de Recebível Imobiliário (Investidor Qualificado)</li>
                <li>FIDC - Fundo de Investimento em Diretos Creditórios (Investidor Qualificado)</li>
                <li>Fundo de Ações</li>
                <li>Clubes de Investimentos</li>
                <li>Ações</li>
            </ul>--%>
            <p>Para o perfil <b>Moderado</b>, devido à média tolerância a riscos, é considerada aceitável a rentabilidade da carteira entre <b>-5% do CDI</b> e <b>200% do CDI</b>.</p>
        </div>

        <div id="divResultado_Arrojado" runat="server" style="display:none" class="col1">
            <h4 class="perfil-usuario">Seu Perfil é <strong>Arrojado</strong></h4>
<%--            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
            com base nas diretrizes dos órgãos reguladores do mercado financeiro e no questionário respondido 
            após o seu cadastro na Gradual.</p>

            <div class="row">
                <div class="col2">
            
                    <ul class="lista lista-quadrado-amarelo">
                        <li>Aluguel de Ações</li>
                        <li>Debêntures</li>
                        <li>Opções</li>
                        <li>Termo</li>
                        <li>Mercadorias e Futuros</li>
                        <li>Tesouro Direto</li>
                        <li>Títulos Públicos</li>
                        <li>CDB - Certificado de Depósito Bancário</li>
                        <li>LCI - Letra de Crédito Imobiliário</li>
                        <li>LCA - Letra de Crédito do Agronegócio</li>
                        <li>LF - Letra Financeira</li>
                    </ul>
                </div>
                <div class="col2">
            
                    <ul class="lista lista-quadrado-amarelo">
                        <li>LC - Letra de Câmbio</li>
                        <li>Fundos de Investimento Renda Fixa</li>
                        <li>Aluguel de Ações</li>
                        <li>Fundos de Investimento Multimercado</li>
                        <li>Debêntures (Investidor Qualificado)</li>
                        <li>CRI - Certificação de Recebível Imobiliário (Investidor Qualificado)</li>
                        <li>FIDC - Fundo de Investimento em Diretos Creditórios (Investidor Qualificado)</li>
                        <li>Fundo de Ações</li>
                        <li>Fundos Imobiliários</li>
                        <li>Clubes de Investimentos</li>
                        <li>Ações</li>
                    </ul>
                </div>
            </div>--%>
            <p>Para o perfil <b>Arrojado</b>, devido à alta tolerância a riscos, é considerado aceitável qualquer nível de rentabilidade da carteira em relação ao CDI.</p>
        </div>

        <div id="divResultado_SemPerfil" runat="server" style="display:none">
            <p>Sem dados de Suitability.</p>
        </div>

        <p class="Continuar" style="padding-top:24px;width:90%">
            <button onclick="return GradSite_Suitability_Refazer(this)" class="botao btn-padrao btn-erica" style="width:auto; margin-left:16px">Refazer o Teste</button>
        </p>

    </div>

    <div id="pnlFormPerfil" runat="server" style="display:none">

        <div class="row">
            <div class="col1">
                <input type="hidden" id="hidCadastro_PFPasso3_JaPreencheuSuit" runat="server" value="false" />

                <p><strong>1. Há quanto tempo você investe no mercado?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_A" type="radio" value="0" name="rdoSuit_01" /> Não invisto        </label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_B" type="radio" value="1" name="rdoSuit_01" /> Menos de dois anos </label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_C" type="radio" value="2" name="rdoSuit_01" /> Entre 2 e 5 anos   </label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_D" type="radio" value="3" name="rdoSuit_01" /> Mais de 5 anos     </label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>2. Como você descreveria seu perfil ao investir?</strong></p>
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_02_A" type="radio" value="0" name="rdoSuit_02" /> Nunca investe, e se o fizesse seria na poupança</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_02_B" type="radio" value="1" name="rdoSuit_02" /> Investe periodicamente 10% de sua renda, mas só em produtos de renda fixa</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_02_C" type="radio" value="2" name="rdoSuit_02" /> Investe todo mês até 20% de sua renda em uma cesta de produtos diversos</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_02_D" type="radio" value="3" name="rdoSuit_02" /> Investe toda semana, sobretudo, na compra e venda de ações</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>3. Qual o seu conhecimento do mercado financeiro?</strong></p>

                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_A" type="radio" value="0" name="rdoSuit_03" /> Nenhum</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_B" type="radio" value="1" name="rdoSuit_03" /> Pouco, conheço apenas Poupança, Tesouro Direto e alguns produtos de renda fixa</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_C" type="radio" value="2" name="rdoSuit_03" /> Bom, já investi em vários produtos de renda fixa, e comprei ações algumas vezes</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_D" type="radio" value="3" name="rdoSuit_03" /> Ótimo, invisto em renda fixa e renda variável, e já realizei operações com derivativos.</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>4. O que você pretende quando investe no mercado financeiro?</strong></p>
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_A" type="radio" value="0" name="rdoSuit_04" /> Proteger seu dinheiro da inflação</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_B" type="radio" value="1" name="rdoSuit_04" /> Ganhar da Poupança</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_C" type="radio" value="2" name="rdoSuit_04" /> Ganhar dos produtos dos bancos (CDB, Capitalização)</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_D" type="radio" value="3" name="rdoSuit_04" /> Investir com isenção de imposto de renda</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_E" type="radio" value="4" name="rdoSuit_04" /> Buscar bons e rápidos rendimentos</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>5. Em qual destes ativos você aplicaria a maior parte do seu dinheiro?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_A" type="radio" value="0" name="rdoSuit_05" /> Imóveis</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_B" type="radio" value="1" name="rdoSuit_05" /> Fundos de Renda fixa e/ou produtos com isenção de IR (LCI ou LCA).</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_C" type="radio" value="2" name="rdoSuit_05" /> Fundos Multimercados.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_D" type="radio" value="3" name="rdoSuit_05" /> Ações e/ou Fundos de Ações.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_E" type="radio" value="3" name="rdoSuit_05" /> Derivativos</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>6. Qual a sua renda mensal disponível para aplicação?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_A" type="radio" value="0" name="rdoSuit_06" /> Até R$ 5mil</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_B" type="radio" value="1" name="rdoSuit_06" /> Entre 5mil e R$ 20 mil</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_C" type="radio" value="2" name="rdoSuit_06" /> Entre R$ 20 - 50mil</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_D" type="radio" value="3" name="rdoSuit_06" /> Acima de R$ 50mil</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>7. Quanto você estaria disposto a perder por uma decisão equivocada?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_A" type="radio" value="0" name="rdoSuit_07" /> Nada, não aceitaria perdas.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_B" type="radio" value="1" name="rdoSuit_07" /> Até R$ 5.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_C" type="radio" value="2" name="rdoSuit_07" /> Entre R$ 5.000,00 e 50.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_D" type="radio" value="3" name="rdoSuit_07" /> Entre R$ 50.000,00 e R$ 200.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_E" type="radio" value="4" name="rdoSuit_07" /> Acima de R$ 200.000,00.</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>8. Qual o valor do seu patrimônio acumulado?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_A" type="radio" value="0" name="rdoSuit_08" /> Até R$ 50.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_B" type="radio" value="1" name="rdoSuit_08" /> Entre R$ 50.000,00 e 200.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_C" type="radio" value="2" name="rdoSuit_08" /> Entre R$ 200.000,00 e R$ 500.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_D" type="radio" value="3" name="rdoSuit_08" /> Entre R$ 500.000,00 e R$ 1.000.000,00.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_E" type="radio" value="4" name="rdoSuit_08" /> Acima de R$ 1.000.000,00.</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>9. Em quais ativos seu patrimônio está alocado?</strong></p>
                            
                <div class="lista-radio">
                    <label class="checkbox" style="display:block;"><input  id="rdoSuit_09_A" type="checkbox" value="0|0.25" name="rdoSuit_09" /> Bens imóveis</label>
                    <label class="checkbox" style="display:block;"><input  id="rdoSuit_09_B" type="checkbox" value="1|0.25" name="rdoSuit_09" /> Poupança, CDB, Tesouro Direto e Fundos de Renda Fixa</label>
                    <label class="checkbox" style="display:block;"><input  id="rdoSuit_09_C" type="checkbox" value="2|0.5" name="rdoSuit_09" /> Bens móveis</label>
                    <label class="checkbox" style="display:block;"><input  id="rdoSuit_09_D" type="checkbox" value="3|1.5" name="rdoSuit_09" /> Ações, Fundos Multimercado e fundos de ações</label>
                    <label class="checkbox" style="display:block;"><input  id="rdoSuit_09_E" type="checkbox" value="4|2.5" name="rdoSuit_09" /> Outras aplicações financeiras de alto risco</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>10. Qual o tempo disponível que você tem para manter seu dinheiro aplicado?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_10_A" type="radio" value="0" name="rdoSuit_10" /> Menos de 3 meses.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_10_B" type="radio" value="1" name="rdoSuit_10" /> Entre 3 e 6 meses.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_10_C" type="radio" value="2" name="rdoSuit_10" /> Entre 6 meses e 1 ano.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_10_D" type="radio" value="3" name="rdoSuit_10" /> Mais de 1 ano.</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>11. Informe sobre sua formação acadêmica:</strong></p>
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_11_A" type="radio" value="0" name="rdoSuit_11" /> Nível Superior em Economia ou em áreas relacionadas à Finanças.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_11_B" type="radio" value="1" name="rdoSuit_11" /> Formação superior em outras áreas.</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_11_C" type="radio" value="2" name="rdoSuit_11" /> Não possui formação superior.</label>
              </div>
            </div>
        </div>

        <p class="Continuar" style="padding-top:24px;width:90%">
            <button onclick="return GradSite_Suitability_Reavaliar(this)" class="botao btn-padrao btn-erica" style="width:auto; margin-left:16px">Salvar</button>
        </p>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Meu Perfil" />

</asp:Content>
