<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monitoramento_OrdensSpider.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Buscas.Monitoramento_OrdensSpider" %>

<form id="form1" runat="server">

    <div id="pnlBusca_Monitoramento_OrdensSpider_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_3Linhas">

        <p>
            <label for="txtBusca_Monitoramento_OrdensSpider_DataInicial">Data Inicial:</label>
            <input name="txtBusca_Monitoramento_OrdensSpider_DataInicial" id="txtBusca_Monitoramento_OrdensSpider_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" />
        </p>
        
        <p class="LadoALado_Pequeno" style="width:168px;margin-left:-12px">
            <label for="txtBusca_Monitoramento_OrdensSpider_DataFinal">Data Final:</label>
            <input  id="txtBusca_Monitoramento_OrdensSpider_DataFinal" type="text"  value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="width: 64px;" />
        </p>
        
        <p class="LadoALado_Pequeno" style="width: 14em;">
            <label for="txtBusca_Monitoramento_OrdensSpider_HoraInicial">Hora Inicial:</label>
            <input  id="txtBusca_Monitoramento_OrdensSpider_HoraInicial" type="text" maxlength="5" style="width: 69px;" Propriedade="HoraInicial" class="Mascara_Hora" value="08:00" />
        </p>
        
        <p class="LadoALado_Pequeno" style="width: 14.5em;">
            <label for="txtBusca_Monitoramento_OrdensSpider_HoraFinal">Hora Final:</label>
            <input  id="txtBusca_Monitoramento_OrdensSpider_HoraFinal" type="text" maxlength="5" style="width: 80px;" Propriedade="HoraFinal" class="Mascara_Hora" value="18:00" />
        </p>

        <p class="LadoALado_Pequeno">
            <label for="txtBusca_Monitoramento_OrdensSpider_Origem">Origem:</label>
            <select id="txtBusca_Monitoramento_OrdensSpider_Origem" style="width:88px" Propriedade="Origem" onchange="javascript:EscondeBotaoCancelar(this)">
                <option value="FixServer">FixServer</option>
                <%--<option value="HB">Homebroker</option>--%>
                <%--<option value="Sinacor">Sinacor</option>
                <option value="tryd">TRYD</option>--%>
            </select>
        </p>

        <%--<p class="LadoALado_Pequeno" style="width:156px">
            <label for="txtBusca_Monitoramento_Ordens_Bolsa">Bolsa:</label>
            <select id="txtBusca_Monitoramento_Ordens_Bolsa" Propriedade="Bolsa" style="width: 118px;">
                <option value="Bovespa">Bovespa</option>
                <option value="BMF">BMF</option>
            </select>
        </p>--%>

        <p>
            <label for="txtBusca_Monitoramento_OrdensSpider_Papel">Papel:</label>
            <input  id="txtBusca_Monitoramento_OrdensSpider_Papel" type="text" Propriedade="Papel" maxlength="10" style="width:5.4em" />
            <button onclick="return false;" class="IconButton IconePesquisar"></button>
        </p>

        <%--<p class="LadoALado_Pequeno" style="margin-left:10px">
            <label for="txtBusca_Monitoramento_Ordens_Porta">Porta:</label>
            <select id="txtBusca_Monitoramento_Ordens_Porta" Propriedade="Porta" style="width:84px">
                <option value="">[Selecione]</option>
                <option value="200">200</option>
            </select>
        </p>--%>

        <p class="LadoALado_Pequeno">
            <label for="txtBusca_Monitoramento_OrdensSpider_Status">Status:</label>
            <select id="txtBusca_Monitoramento_OrdensSpider_Status" Propriedade="Status" style="width:94px;">
                <option value="" selected="selected">Todos</option>
                <option value="0">Aberta</option>
                <option value="1">Parcialmente Executada</option>
                <option value="2">Executada</option>
                <option value="4">Cancelada</option>
                <option value="5">Substituída</option>
                <option value="8">Rejeitada</option>
                <option value="9">Suspensa</option>
                <option value="10">Pendente</option>
                <option value="11">Expirada</option>
            </select>
        </p>
        
        <p class="LadoALado_Pequeno" style="padding-left: 33px; width: 14em;">
            <label for="cboBusca_Monitoramento_OrdensSpider_Sentido">Sentido:</label>
            <select id="cboBusca_Monitoramento_OrdensSpider_Sentido" style="width: 76px" Propriedade="Sentido" >
                <option value="">Todos</option>
                <option value="C">Compra</option>
                <option value="V">Venda</option>
            </select>
        </p>

        <p class="LadoALado_Pequeno" style="padding-left: 5px">
            <label for="cboBusca_Monitoramento_OrdensSpider_Bolsa">Bolsa:</label>
            <select id="cboBusca_Monitoramento_OrdensSpider_Bolsa" style="width: 88px;" Propriedade="Bolsa" >
                <%--<option value="">Todas</option>--%>
                <option value="BOL">Bovespa</option>
                <option value="BMF">BMF</option>
            </select>
        </p>

        <p class="LadoALado_Pequeno" style="width: 16em; margin-left: -4px;">
            <label for="cboBusca_Monitoramento_OrdensSpider_Sistema">Sistema:</label>
            <select id="cboBusca_Monitoramento_OrdensSpider_Sistema" style="width: 88px;" disabled="disabled" Propriedade="Sistema" >
                <option value="">Todos</option>
                <asp:repeater id="rptBusca_Monitoramento_OrdensSpider_Sistema" runat="server">
                    <itemtemplate>
                        <option value='<%# Eval("IdSistema") %>'><%# Eval("DsSistema")%></option>
                    </itemtemplate>
                </asp:repeater>
            </select>
        </p>

        <p style="width:510px">
            <label for="txtBusca_Monitoramento_OrdensSpider_Termo_Tipo">Cliente:</label>
            <select id="txtBusca_Monitoramento_OrdensSpider_Termo_Tipo" Propriedade="TermoTipo" style="float:left;margin-right:6px">
                <option value="0" selected="true">Código Bovespa</option>
                <%--<option value="3">Código BMF</option>--%>
                <%--<option value="1">Cpf Cnpj</option>
                <option value="4">Login</option>
                <option value="2">Nome do Cliente</option>--%>
            </select>
            
            <input id="txtBusca_Monitoramento_OrdensSpider_Termo" type="text" Propriedade="TermoDeBusca" style="width:151px" />
            
            <button onclick="return false;" class="IconButton IconePesquisar"></button>
        </p>

        <p style="margin-right:6px;text-align:right;float:right">
            <button class="btnBusca" onclick="return btnBusca_Click(this)">Buscar</button>
        </p>

    </div>
    
    <%--btnBusca_Monitoramento_Ordens_Click
    btnBusca_Click--%>
</form>
