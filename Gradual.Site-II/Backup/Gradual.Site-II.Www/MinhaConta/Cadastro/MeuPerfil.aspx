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
            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
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
            </ul>
        </div>

        <div id="divResultado_Moderado" runat="server" style="display:none" class="col1">
            <h4 class="perfil-usuario">Seu Perfil é <strong>Moderado</strong></h4>
            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
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
            </ul>
        </div>

        <div id="divResultado_Arrojado" runat="server" style="display:none" class="col1">
            <h4 class="perfil-usuario">Seu Perfil é <strong>Arrojado</strong></h4>
            <p>Confira a seguir o portfólio de investimentos recomendado para o seu Perfil de Investidor, 
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
            </div>
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

                <p><strong>1. Há quanto tempo você investe no mercado financeiro?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_A" type="radio" value="0" name="rdoSuit_01" />Há mais de cinco anos;</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_B" type="radio" value="1" name="rdoSuit_01" />Entre dois e cinco anos;</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_C" type="radio" value="2" name="rdoSuit_01" />Há menos de dois anos;</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_01_D" type="radio" value="3" name="rdoSuit_01" />Ainda não invisto;</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>2. Qual o seu conhecimento sobre mercado financeiro?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio"><input  id="rdoSuit_02_A" type="radio" value="0" name="rdoSuit_02" /> Nenhum</label>
                    <label class="radio"><input  id="rdoSuit_02_B" type="radio" value="1" name="rdoSuit_02" /> Pouco</label>
                    <label class="radio"><input  id="rdoSuit_02_C" type="radio" value="2" name="rdoSuit_02" /> Bom</label>
                    <label class="radio"><input  id="rdoSuit_02_D" type="radio" value="3" name="rdoSuit_02" /> Ótimo</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>3. O que você pretende quando investe no mercado financeiro?</strong></p>

                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_A" type="radio" value="0" name="rdoSuit_03" /> Proteger seu dinheiro da inflação</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_B" type="radio" value="1" name="rdoSuit_03" /> Ganhar dos produtos dos bancos (CDB, Capitalização)</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_C" type="radio" value="2" name="rdoSuit_03" /> Ganhar da Poupança</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_D" type="radio" value="3" name="rdoSuit_03" /> Investir com isenção de Imposto de Renda</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_03_E" type="radio" value="3" name="rdoSuit_03" /> Buscar bons e rápidos rendimentos</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>4. Em qual destes ativos você aplicaria a maior parte do seu dinheiro?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_A" type="radio" value="0" name="rdoSuit_04" /> Imóveis</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_B" type="radio" value="1" name="rdoSuit_04" /> Fundos de Renda fixa e/ou produtos com isenção de IR (LCI ou LCA)</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_C" type="radio" value="2" name="rdoSuit_04" /> Fundos Multimercados;</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_D" type="radio" value="3" name="rdoSuit_04" /> Ações e/ou Fundos de Ações;</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_04_E" type="radio" value="4" name="rdoSuit_04" /> Derivativos</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>5. Quanto você pretende aplicar?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_A" type="radio" value="0" name="rdoSuit_05" /> Até R$ 5.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_B" type="radio" value="1" name="rdoSuit_05" /> De R$ 5.000,00 a R$ 300.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_C" type="radio" value="2" name="rdoSuit_05" /> De R$ 300.000,00 a R$ 600.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_D" type="radio" value="3" name="rdoSuit_05" /> Acima de R$ 600.000,00</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>6. Quanto você estaria disposto a perder por uma decisão equivocada?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_A" type="radio" value="0" name="rdoSuit_06" /> Nada, não aceitaria perdas</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_B" type="radio" value="1" name="rdoSuit_06" /> Até R$ 5.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_C" type="radio" value="2" name="rdoSuit_06" /> Entre R$ 5.000,00 e 50.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_D" type="radio" value="3" name="rdoSuit_06" /> Entre R$ 50.000,00 e 200.000,00</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_E" type="radio" value="4" name="rdoSuit_06" /> Acima de R$ 200.000,00</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>7. Qual o tempo disponível que você tem para manter seu dinheiro aplicado?</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_A" type="radio" value="0" name="rdoSuit_07" /> Menos de 3 meses</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_B" type="radio" value="1" name="rdoSuit_07" /> Entre 3 e 6 meses</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_C" type="radio" value="2" name="rdoSuit_07" /> Entre 6 meses e 1 ano</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_D" type="radio" value="3" name="rdoSuit_07" /> Mais de 1 ano</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <p><strong>8. Informe sobre sua formação acadêmia:</strong></p>
                            
                <div class="lista-radio">
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_A" type="radio" value="0" name="rdoSuit_08" /> Nível Superior em Economia ou em áreas relacionadas à Finanças</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_B" type="radio" value="1" name="rdoSuit_08" /> Formação superior em outras áreas</label>
                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_C" type="radio" value="2" name="rdoSuit_08" /> Não possui formação superior</label>
                </div>
            </div>
        </div>

        <p class="Continuar" style="padding-top:24px;width:90%">
            <button onclick="return GradSite_Suitability_Reavaliar(this)" class="botao btn-padrao btn-erica" style="width:auto; margin-left:16px">Salvar</button>
        </p>
        </div>

    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Meu Perfil" />

</asp:Content>
