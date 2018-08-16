<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Suitability.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.Suitability" %>

<form id="form1" runat="server">

    <script language="javascript">

        $.ready( GradIntra_Clientes_Suitability_VerificarExistencia() );

    </script>

    <h4>Suitability do Cliente</h4>

    <input type="hidden" id="hidCliente_Suitability_Resultado" runat="server" />
    <input type="hidden" id="hidCliente_Suitability_Resultado_DataDaRealizacao" runat="server" />
    <input type="hidden" id="hidCliente_Suitability_Resultado_Sistema" runat="server" />
    <input type="hidden" id="hidCliente_Suitability_Resultado_Usuario" runat="server" />

    <div id="pnlCliente_Suitability_Resultado" class="PainelResultado" style="display:none">

        <img class="Resultado_Conservador" src="<%= this.RaizURL %>Skin/<%= this.SkinEmUso %>/Img/Imagem-Suitability-Conservador.jpg" alt="Conservador" style="display:none; margin:10px 0 0 80px" />

        <img class="Resultado_Moderado" src="<%= this.RaizURL %>Skin/<%= this.SkinEmUso %>/Img/Imagem-Suitability-Moderado.jpg" alt="Moderado" style="display:none; margin:10px 0 0 80px" />

        <img class="Resultado_Arrojado" src="<%= this.RaizURL %>Skin/<%= this.SkinEmUso %>/Img/Imagem-Suitability-Arrojado.jpg" alt="Arrojado" style="display:none; margin:10px 0 0 80px" />

        <p id="id_Cliente_Suitability_Resultado" style="width:88%"></p>

        <div class="BarraDeOpcoes">

            <span id="lblArquivoDeCienciaSuitability"></span>

            <button onclick="return btnClientes_Suitability_Refazer_Click(this)" id="pnlCliente_Suitability_BotaoRefazerTeste" runat="server">Refazer o Teste</button>

            <button onclick="return btnClientes_Suitability_DownloadPDF_Click(this)">Download PDF</button>
            <button onclick="return btnClientes_Suitability_EnviarPorEmail_Click(this)">Enviar por Email</button>

            <button onclick="return btnClientes_Suitability_UploadDeclaracao_Click(this)">Declaração de Ciência</button>

        </div>

        <div id="pnlClientes_Suitability_UploadDeclaracao" style="display:none; background:#32404F;float:left;width:100%;padding:4px 0px;margin:-28px 0px 0px 0px; text-align:center">

            <label for="txtClientes_Suitability_UploadDeclaracao" style="float:none;padding-right:3px;color:#fff">Selecione o arquivo:</label>
            <input id="txtClientes_Suitability_UploadDeclaracao" type="file" style="width:240px; padding:0px; border:none" />

            <a href="#" onclick="return btnClientes_Suitability_UploadDeclaracao_Fechar_Click()" style="color:#fff; border:none; margin-left:30px">cancelar</a>

        </div>

    </div>

    <div id="pnlCliente_Suitability_Questionario" class="PainelScroll PainelSuitability" style="display:none" runat="server">

        <div>
            <h5><span>1</span> Há quanto tempo você investe no mercado?</h5>

            <input  id="rdoSuit_01_A" type="radio" name="rdoSuit_01" value="0" /><label for="rdoSuit_01_A">Não invisto</label>
            <input  id="rdoSuit_01_B" type="radio" name="rdoSuit_01" value="1" /><label for="rdoSuit_01_B">Menos de dois anos</label>
            <input  id="rdoSuit_01_C" type="radio" name="rdoSuit_01" value="2" /><label for="rdoSuit_01_C">Entre 2 e 5 anos</label>
            <input  id="rdoSuit_01_D" type="radio" name="rdoSuit_01" value="3" /><label for="rdoSuit_01_D">Mais de 5 anos</label>
        </div>

        <div>
            <h5><span>2</span> Como você descreveria seu perfil ao investir?</h5>

            <input  id="rdoSuit_02_A" type="radio" name="rdoSuit_02" value="0" /><label for="rdoSuit_02_A">Nunca investe, e se o fizesse seria na poupança</label>
            <input  id="rdoSuit_02_B" type="radio" name="rdoSuit_02" value="1" /><label for="rdoSuit_02_B">Investe periodicamente 10% de sua renda, mas só em produtos de renda fixa</label>
            <input  id="rdoSuit_02_C" type="radio" name="rdoSuit_02" value="2" /><label for="rdoSuit_02_C">Investe todo mês até 20% de sua renda em uma cesta de produtos diversos</label>
            <input  id="rdoSuit_02_D" type="radio" name="rdoSuit_02" value="3" /><label for="rdoSuit_02_D">Investe toda semana, sobretudo, na compra e venda de ações</label>
        </div>

        <div>
            <h5><span>3</span> Qual o seu conhecimento do mercado financeiro?</h5>

            <input  id="rdoSuit_03_A" type="radio" name="rdoSuit_03" value="0" /><label for="rdoSuit_03_A">Nenhum</label>
            <input  id="rdoSuit_03_B" type="radio" name="rdoSuit_03" value="1" /><label for="rdoSuit_03_B">Pouco, conheço apenas Poupança, Tesouro Direto e alguns produtos de renda fixa</label>
            <input  id="rdoSuit_03_C" type="radio" name="rdoSuit_03" value="2" /><label for="rdoSuit_03_C">Bom, já investi em vários produtos de renda fixa, e comprei ações algumas vezes</label>
            <input  id="rdoSuit_03_D" type="radio" name="rdoSuit_03" value="3" /><label for="rdoSuit_03_D">Ótimo, invisto em renda fixa e renda variável, e já realizei operações com derivativos.</label>
        </div>

        <div>
            <h5><span>4</span> O que você pretende quando investe no mercado financeiro?</h5>

            <input  id="rdoSuit_04_A" type="radio" name="rdoSuit_04" value="0" /><label for="rdoSuit_04_A">Proteger seu dinheiro da inflação</label>
            <input  id="rdoSuit_04_B" type="radio" name="rdoSuit_04" value="1" /><label for="rdoSuit_04_B">Ganhar da Poupança</label>
            <input  id="rdoSuit_04_C" type="radio" name="rdoSuit_04" value="2" /><label for="rdoSuit_04_C">Ganhar dos produtos dos bancos (CDB, Capitalização)</label>
            <input  id="rdoSuit_04_D" type="radio" name="rdoSuit_04" value="3" /><label for="rdoSuit_04_D">Investir com isenção de imposto de renda</label>
            <input  id="rdoSuit_04_E" type="radio" name="rdoSuit_04" value="4" /><label for="rdoSuit_04_E">Buscar bons e rápidos rendimentos</label>

        </div>

        <div>
            <h5><span>5</span> Em qual destes ativos você aplicaria a maior parte do seu dinheiro?</h5>

            <input  id="rdoSuit_05_A" type="radio" name="rdoSuit_05" value="0" /><label for="rdoSuit_05_A">Imóveis.</label>
            <input  id="rdoSuit_05_B" type="radio" name="rdoSuit_05" value="1" /><label for="rdoSuit_05_B">Fundos de Renda fixa e/ou produtos com isenção de IR (LCI ou LCA).</label>
            <input  id="rdoSuit_05_C" type="radio" name="rdoSuit_05" value="2" /><label for="rdoSuit_05_C">Fundos Multimercados.</label>
            <input  id="rdoSuit_05_D" type="radio" name="rdoSuit_05" value="3" /><label for="rdoSuit_05_D">Ações e/ou Fundos de Ações.</label>
            <input  id="rdoSuit_05_E" type="radio" name="rdoSuit_05" value="4" /><label for="rdoSuit_05_E">Derivativos.</label>

        </div>

        <div>
            <h5><span>6</span> Qual a sua renda mensal disponível para aplicação?</h5>

            <input  id="rdoSuit_06_A" type="radio" name="rdoSuit_06" value="0" /><label for="rdoSuit_06_A">Até R$ 5mil</label>
            <input  id="rdoSuit_06_B" type="radio" name="rdoSuit_06" value="1" /><label for="rdoSuit_06_B">Entre 5mil e R$ 20 mil</label>
            <input  id="rdoSuit_06_C" type="radio" name="rdoSuit_06" value="2" /><label for="rdoSuit_06_C">Entre R$ 20 - 50mil</label>
            <input  id="rdoSuit_06_D" type="radio" name="rdoSuit_06" value="3" /><label for="rdoSuit_06_D">Acima de R$ 50mil</label>

        </div>

        <div>
            <h5><span>7</span> Quanto você estaria disposto a perder por uma decisão equivocada?</h5>

            <input  id="rdoSuit_07_A" type="radio" name="rdoSuit_07" value="0" /><label for="rdoSuit_07_A">Nada, não aceitaria perdas.</label>
            <input  id="rdoSuit_07_B" type="radio" name="rdoSuit_07" value="1" /><label for="rdoSuit_07_B">Até R$ 5.000,00.</label>
            <input  id="rdoSuit_07_C" type="radio" name="rdoSuit_07" value="2" /><label for="rdoSuit_07_C">Entre R$ 5.000,00 e 50.000,00.</label>
            <input  id="rdoSuit_07_D" type="radio" name="rdoSuit_07" value="3" /><label for="rdoSuit_07_D">Entre R$ 50.000,00 e R$ 200.000,00.</label>
            <input  id="rdoSuit_07_E" type="radio" name="rdoSuit_07" value="4" /><label for="rdoSuit_07_E">Acima de R$ 200.000,00.</label>
        </div>

        <div>
            <h5><span>8</span> Qual o valor do seu patrimônio acumulado?</h5>

            <input  id="rdoSuit_08_A" type="radio" name="rdoSuit_08" value="0" /><label for="rdoSuit_08_A">Até R$ 50.000,00.</label>
            <input  id="rdoSuit_08_B" type="radio" name="rdoSuit_08" value="1" /><label for="rdoSuit_08_B">Entre R$ 50.000,00 e 200.000,00.</label>
            <input  id="rdoSuit_08_C" type="radio" name="rdoSuit_08" value="2" /><label for="rdoSuit_08_C">Entre R$ 200.000,00 e R$ 500.000,00.</label>
            <input  id="rdoSuit_08_D" type="radio" name="rdoSuit_08" value="3" /><label for="rdoSuit_08_D">Entre R$ 500.000,00 e R$ 1.000.000,00.</label>
            <input  id="rdoSuit_08_E" type="radio" name="rdoSuit_08" value="4" /><label for="rdoSuit_08_E">Acima de R$ 1.000.000,00.</label>
        </div>

        <div>
            <h5><span>9</span> Em quais ativos seu patrimônio está alocado?</h5>

            <p class="CheckBoxAEsquerda">
                <input  id="rdoSuit_09_A" type="checkbox" name="rdoSuit_09" value="0" /><label for="rdoSuit_09_A">Bens imóveis</label>
            </p>
            <p class="CheckBoxAEsquerda">
                <input  id="rdoSuit_09_B" type="checkbox" name="rdoSuit_09" value="1" /><label for="rdoSuit_09_B">Poupança, CDB, Tesouro Direto e Fundos de Renda Fixa</label>
            </p>
            <p class="CheckBoxAEsquerda">
                <input  id="rdoSuit_09_C" type="checkbox" name="rdoSuit_09" value="2" /><label for="rdoSuit_09_C">Bens móveis</label>
            </p>
            <p class="CheckBoxAEsquerda">
                <input  id="rdoSuit_09_D" type="checkbox" name="rdoSuit_09" value="3" /><label for="rdoSuit_09_D">Ações, Fundos Multimercado e fundos de ações</label>
            </p>
            <p class="CheckBoxAEsquerda">
                <input  id="rdoSuit_09_E" type="checkbox" name="rdoSuit_09" value="4" /><label for="rdoSuit_09_E">Outras aplicações financeiras de alto risco</label>
            </p>
        </div>

        <div>
            <h5><span>10</span> Qual o tempo disponível que você tem para manter seu dinheiro aplicado?</h5>

            <input  id="rdoSuit_10_A" type="radio" name="rdoSuit_10" value="0" /><label for="rdoSuit_10_A">Menos de 3 meses.</label>
            <input  id="rdoSuit_10_B" type="radio" name="rdoSuit_10" value="1" /><label for="rdoSuit_10_B">Entre 3 e 6 meses.</label>
            <input  id="rdoSuit_10_C" type="radio" name="rdoSuit_10" value="2" /><label for="rdoSuit_10_C">Entre 6 meses e 1 ano.</label>
            <input  id="rdoSuit_10_D" type="radio" name="rdoSuit_10" value="3" /><label for="rdoSuit_10_D">Mais de 1 ano.</label>
        </div>

        <div>
            <h5><span>11</span> Informe sobre sua formação acadêmica:</h5>

            <input  id="rdoSuit_11_A" type="radio" name="rdoSuit_11" value="0" /><label for="rdoSuit_11_A">Nível Superior em Economia ou em áreas relacionadas à Finanças.</label>
            <input  id="rdoSuit_11_B" type="radio" name="rdoSuit_11" value="1" /><label for="rdoSuit_11_B">Formação superior em outras áreas.</label>
            <input  id="rdoSuit_11_C" type="radio" name="rdoSuit_11" value="2" /><label for="rdoSuit_11_C">Não possui formação superior.</label>
        </div>
    </div>
    
    <p id="pnlCliente_Suitability_BotaoEnviar" class="BotoesSubmit" style="display:none" runat="server">

        <button onclick="return btnCliente_Suitability_Enviar_Click(this)">Verificar Suitability</button>

    </p>
    
    

</form>
