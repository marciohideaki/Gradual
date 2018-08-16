<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="OrdensStop.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Monitoramento_OrdensStop" %>

<form id="form1" runat="server">

    <div id="pnlBusca_Monitoramento_OrdensStop_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas">

        <p>
            <label for="txtBusca_Monitoramento_OrdensStop_Data">Data:</label><%--value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>"--%>
            <input  id="txtBusca_Monitoramento_OrdensStop_Data" type="text" value="<%= DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="Data" class="Mascara_Data Picker_Data" />
        </p>
        
        <p class="LadoALado_Pequeno">
            <label for="txtBusca_Monitoramento_OrdensStop_Papel">Papel:</label>
            <input  id="txtBusca_Monitoramento_OrdensStop_Papel" type="text" Propriedade="Papel" maxlength="10" style="width:5em" />
            <button onclick="return false;" class="IconButton IconePesquisar"></button>
        </p>
        
        <p class="LadoALado_Pequeno" style="width: 25em;">
            <label for="txtBusca_Monitoramento_OrdensStop_Status">Status:</label>
            <select id="txtBusca_Monitoramento_OrdensStop_Status" Propriedade="Status">
                <option value="">Todos</option>
                <option value="3">Aberta</option>
                <option value="4">Rejeitada</option>
                <option value="6">Cancelada</option>
                <option value="9">Cancelamento rejeitado</option>
                <option value="10">Disparada</option>
                <option value="5">Executada</option>
            </select>
        </p>

        <p style="width: 350px;">
            <label for="txtBusca_Monitoramento_OrdensStop_Termo_Tipo">Cliente:</label>
            <select id="txtBusca_Monitoramento_OrdensStop_Termo_Tipo" Propriedade="Cliente" style="float:left;margin-right:6px">
                <option value="0" selected="true">Código Bovespa</option>
            </select>
            
            <input id="txtBusca_Monitoramento_OrdensStop_Termo" type="text" Propriedade="TermoDeBusca" style="width:118px" />
            
            <button onclick="return false;" class="IconButton IconePesquisar"></button>
        </p>

        <p class="LadoALado_Pequeno" style="width: 21em;">
            <label for="cboBusca_Monitoramento_Ordens_Sistema">Sistema:</label>
            <select id="cboBusca_Monitoramento_Ordens_Sistema" style="width: 148px;" disabled="disabled" Propriedade="Sistema" >
                <option value="">Todos</option>
                <asp:repeater id="rptBusca_Monitoramento_Ordens_Sistema" runat="server">
                    <itemtemplate>
                        <option value='<%# Eval("IdSistema") %>'><%# Eval("DsSistema")%></option>
                    </itemtemplate>
                </asp:repeater>
            </select>
        </p>

        <p style="margin-right: 6px;float:right;text-align:right">
            <button class="btnBusca" onclick="return btnBusca_Click(this)">Buscar</button>
        </p>

    </div>

</form>