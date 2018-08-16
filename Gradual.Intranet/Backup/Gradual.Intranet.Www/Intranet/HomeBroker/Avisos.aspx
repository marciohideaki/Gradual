<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Avisos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.HomeBroker.Avisos" %>


<form id="form1" runat="server">


    <table id="tblAvisosHomeBroker_ListaDeAvisos" class="GridIntranet" cellspacing="0" style="float: left; width: 55%;">

        <thead>
            <tr>
                <td>Código</td>
                <td>Data de Entrada</td>
                <td>Data de Saída</td>
                <td>Texto</td>
                <td title="Forçar exibição independente da data">Exibir</td>
                <td>Sistema</td>
                <td></td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="7">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDeAvisos" runat="server">
            <itemtemplate>

            <tr rel="<%# Eval("CodigoAviso") %>">
                <td><%# Eval("CodigoAviso") %></td>
                <td style="white-space:nowrap"><%# Eval("DataEntrada") %> <%# Eval("HoraEntrada") %></td>
                <td style="white-space:nowrap"><%# Eval("DataSaida") %> <%# Eval("HoraSaida") %></td>
                <td title="<%# Eval("Texto") %>"><%# Eval("TextoTruncado") %></td>
                <td><%# Eval("FlagAtivacaoManual")%></td>
                <td><%# Eval("IdSistema") %> </td>
                <td> <button class="IconButton Editar" title="Editar Aviso" onclick="return btnHomeBroker_Avisos_Editar_Click(this)" style="margin-top:2px;margin-bottom:2px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="7">Nenhum aviso encontrado.</td>
            </tr>
        </tbody>

    </table>

    <div style="float:right;width:44%" class="pnlFormulario_Campos" >

        <input type="hidden" id="hidHomeBroker_Avisos_ListaJson" class="ListaJson" runat="server" value="" />

        <h5 style="margin-top: 0.2em;">Cadastro de Avisos para o HomeBroker</h5>

        <input type="hidden" id="hidHomeBroker_Aviso_Id" runat="server" />
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtHomeBroker_Aviso_DataDeEntrada">Data de Entrada:</label>
            <input  id="txtHomeBroker_Aviso_DataDeEntrada" type="text" Propriedade="DataDeEntrada" class="Mascara_Data Picker_Data" maxlength="10" value="<%= DateTime.Now.AddDays(1).ToString("dd/MM/yyyy") %>" style="float:left;width:6em"  />

            <label for="txtHomeBroker_Aviso_HoraDeEntrada" style="width:auto;margin-left:1em">Hora:</label>
            <input  id="txtHomeBroker_Aviso_HoraDeEntrada" type="text" Propriedade="HoraDeEntrada" class="Mascara_Hora" maxlength="5" style="width:4em;text-align:center" value="01:00" />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtHomeBroker_Aviso_DataDeSaida">Data de Saída:</label>
            <input  id="txtHomeBroker_Aviso_DataDeSaida" type="text" Propriedade="DataDeSaida" class="Mascara_Data Picker_Data" maxlength="10" value="<%= DateTime.Now.AddDays(11).ToString("dd/MM/yyyy") %>" style="float:left;width:6em"  />

            <label for="txtHomeBroker_Aviso_HoraDeSaida" style="width:auto;margin-left:1em">Hora:</label>
            <input  id="txtHomeBroker_Aviso_HoraDeSaida" type="text" Propriedade="HoraDeSaida" class="Mascara_Hora" maxlength="5" style="width:4em;text-align:center" value="23:00" />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral" style="height:12em;">
            <label   for="txtHomeBroker_Aviso_DataDeSaida">Aviso:</label>
            <textarea id="txtHomeBroker_Aviso_Texto" style="float:left; width:20em; height:10em;"></textarea>
        </p>

        <p class="Form_TiposDePendenciaCadastral" style="height:12em;">
            <label   for="txtHomeBroker_Aviso_CBLCs">CBLCs:</label>
            <textarea id="txtHomeBroker_Aviso_CBLCs" style="float:left; width:20em; height:10em;"></textarea>
        </p>

        <p class="Form_TiposDePendenciaCadastral">

             <label for="chkHomeBroker_Aviso_Sistema">Sistema:</label>
             <select id="chkHomeBroker_Aviso_Sistema" style="float:left;width:9.5em" Propriedade="IdSistema">
                <option value="1" selected="selected">Homebroker</option>
                <option value="2">Desktop Trader</option>
            </select>

            <%--<input  id="chkHomeBroker_Aviso_FlagAtivacaoManual" type="checkbox" Propriedade="FlagAtivacaoManual" runat="server"  />
            <label for="chkHomeBroker_Aviso_FlagAtivacaoManual"  class="CheckLabel" >Forçar exibição agora</label>--%>
        </p>

        <p class="BotoesSubmit">

            <button onclick="return btnHomeBroker_Avisos_IncluirEditar_Click(this)" style="font-size:1em">Incluir</button>

        </p>
    </div>

</form>
