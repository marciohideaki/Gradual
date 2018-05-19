<%@ Page Title="" Language="C#" MasterPageFile="~/Intranet/Clientes/Formularios/Acoes/IntegracaoRocket/IntegracaoRocket.Master" AutoEventWireup="true" CodeBehind="Sumario.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.IntegracaoRocket.Sumario" %>

<asp:Content ID="Cabecalho_Sumario" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Sumario" ContentPlaceHolderID="Conteudo" runat="server">
    <div id="pnlBusca_Clientes_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_1Linha" style="width:100%;">
        <p class="LadoALado_Pequeno" style="width:175px;">
            <label for="txtIntegracaoRocket_Filtro_DataDe">Data Inicial:</label>
            <input  id="txtIntegracaoRocket_Filtro_DataDe" type="text" maxlength="10" Propriedade="DataDe" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
        </p>

        <p class="LadoALado_Pequeno" style="width:175px;">
            <label for="txtIntegracaoRocket_Filtro_DataAte">Data Final:</label>
            <input  id="txtIntegracaoRocket_Filtro_DataAte" type="text" maxlength="10" Propriedade="DataAte" style="width: 5.5em;" class="Mascara_Data Picker_Data" />
        </p>

        <p class="LadoALado_Pequeno" style="width:175px;">
            <label for="cboIntegracaoRocket_Filtro_Status">Status:</label>
            <select id="cboIntegracaoRocket_Filtro_Status" style="width:9.5em">
                <option value="TODOS" selected ="selected">[Qualquer]</option>
                <option value="1" >Aprovado</option>
                <option value="0">Reprovado</option>
            </select>
        </p>

        <p class="LadoALado_Pequeno" style="width:175px;">
            <label for="cboIntegracaoRocket_Filtro_Transacao">Transação:</label>
            <select id="cboIntegracaoRocket_Filtro_Transacao" style="width:9.5em">
                <option value="TODOS" selected ="selected">[Qualquer]</option>
                <option value="WT">Aguardando</option>
                <option value="SL" >Solicitado</option>
                <option value="PR">Em processamento</option>
                <option value="PD">Processo Disponível</option>
                <option value="RF">Relatório Finalizado</option>
            </select>
        </p>

        <p class="LadoALado_Pequeno" style="width:200px;">
            <label for="txtIntegracaoRocket_Filtro_Codigo">Código:</label>
            <input type="text" id="txtIntegracaoRocket_Filtro_Codigo" maxlength="30" style="width:6em;margin-right:0.4em" />
        </p>

        <p class="LadoALado_Pequeno" style="width:100px;">
            <button class="btnBusca" onclick="return GradIntra_IntegracaoRocket_btnIntegracaoRocket_Buscar();">Buscar</button>
        </p>

    </div>
       
    <div id="pnlSumario" class="PainelResultado">
        <table id="example" class="display" width="100%"></table>
    </div>

    <div id="processando" class="dataTables_processing panel panel-default" style="display: none;">Processando requisição.</div>

    <div id="detalheModal" class="w3-modal" style="z-index:99;">
        <div class="w3-modal-content w3-animate-top w3-card-4">
            <div id="header" class="w3-container w3-display-container"> 
                <span onclick="GradIntra_IntegracaoRocket_FecharDetalhe();" class="w3-button w3-display-topright w3-hover-red w3-hover-opacity" style="padding: 8px 16px;">×</span>
                <h4 id="headerText">STATUS : REPROVADO PELO VALIDADOR DO ROCKET ÀS 11:08:11 | SCORE 65 Pts</h4>
            </div>
            <div id="processoDetalhe" class="w3-container">
                <table id="tbDetalhe"  class="display" width="100%"></table>    
            </div>
            
            <div class="w3-container w3-light-grey">
                <h4 id="footerText"></h4>
                <div id="rodapeAprovado" style="display: none;"></div>
                <div id="rodapeReprovado" style="display: none;"><span id="bntSolicitacao" onclick="GradIntra_IntegracaoRocket_SolicitarDocumentacao();" class="w3-button w3-hover-dark-grey w3-hover-opacity" style="width:100%; margin:0 auto; display:block;height:20px;font-size:16px; background-color:#244062 !important; color:#fff;">SOLICITAR DOCUMENTOS AO CLIENTE</span></div>
            </div>

            <div class="w3-container w3-light-grey">
                <h4 id="H1"></h4>
                <div id="Div1" style="display: none;"></div>
                <div id="Div2" style="display: none;"><span id="Span1" onclick="GradIntra_IntegracaoRocket_SolicitarDocumentacao();" class="w3-button w3-hover-dark-grey w3-hover-opacity" style="width:100%; margin:0 auto; display:block;height:20px;font-size:16px; background-color:#244062 !important; color:#fff;">ENVIAR EMAIL DE BOAS VINDAS</span></div>
            </div>
        </div>
    </div>
</asp:Content>
