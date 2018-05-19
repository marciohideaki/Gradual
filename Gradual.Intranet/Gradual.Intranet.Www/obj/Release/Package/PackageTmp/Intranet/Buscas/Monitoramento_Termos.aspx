<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monitoramento_Termos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Monitoramento_Termos" %>

<form id="form1" runat="server">

    <div id="pnlBusca_Monitoramento_Termos_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p>
            <label for="txtBusca_Monitoramento_Termos_Cliente">Cliente:</label>
            <input  id="txtBusca_Monitoramento_Termos_Cliente" type="text" Propriedade="Cliente" />
        </p>

        <p>
            <label for="txtBusca_Monitoramento_Termos_Papel">Papel:</label>
            <input  id="txtBusca_Monitoramento_Termos_Papel" type="text" Propriedade="Papel" maxlength="10" style="width:5.4em" />
        </p>

        <p class="LadoALado_Pequeno" style="width: 12em;">
            <label for="txtBusca_Monitoramento_Termos_HoraInicial">Hora Inicial:</label>
            <input  id="txtBusca_Monitoramento_Termos_HoraInicial" type="text" maxlength="5" style="width: 40px;" Propriedade="HoraInicial" class="Mascara_Hora" value="08:00" />
        </p>

        <p class="LadoALado_Pequeno" style="width: 12em;">
            <label for="txtBusca_Monitoramento_Termos_HoraFinal">Hora Final:</label>
            <input  id="txtBusca_Monitoramento_Termos_HoraFinal" type="text" maxlength="5" style="width: 40px;" Propriedade="HoraFinal" class="Mascara_Hora" value="18:00" />
        </p>

        <p class="LadoALado_Pequeno">
            <label for="txtBusca_Monitoramento_Termos_Status">Status:</label>
            <select id="txtBusca_Monitoramento_Termos_Status" Propriedade="Status" style="width:94px;">
                <option value="" selected="selected">Todos</option>
                <option value="SolicitacaoEnviada">Solicitado</option>
                <option value="SolicitacaoExecutada">Efetuado</option>
            </select>
        </p>
        
        <p style="width: 554px">
            <label for="cboBusca_Monitoramento_Assessor">Assessor:</label>
            <select id="cboBusca_Monitoramento_Assessor" style="width: 464px" Propriedade="IdAssessor">
                <option value="">[ Selecione ]</option>

                <asp:repeater id="rptAssessor" runat="server">
                <itemtemplate>
                <option value='<%# Eval("Id") %>'><%# Eval("Id").ToString().PadLeft(4, '0') %> - <%# Eval("Value") %></option>
                </itemtemplate>
                </asp:repeater>

            </select>
        </p>

        <p class="LadoALado_Pequeno" style="padding-left: 7px; width: 15em;">
            <label for="cboBusca_Monitoramento_Termos_Solicitacao">Solicitação:</label>
            <select id="cboBusca_Monitoramento_Termos_Solicitacao" style="width: 76px" Propriedade="Sentido" >
                <option value="">Todos</option>
                <option value="NovoTermo">Novo</option>
                <option value="Liquidacao">Liquidação</option>
                <option value="Rolagem">Rolagem</option>
                <option value="Cancelamento">Cancelamento</option>
            </select>
        </p>

        <p style="margin-right:6px;text-align:right;float:right">
            <button class="btnBusca" onclick="return btnBusca_Click(this)">Buscar</button>
        </p>

    </div>
    
</form>