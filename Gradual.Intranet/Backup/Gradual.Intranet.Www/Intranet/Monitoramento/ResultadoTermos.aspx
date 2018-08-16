<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoTermos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.ResultadoTermos" %>


<form id="form1" runat="server">

        <table cellspacing="0" style="width:98%;" class="GridIntranet">
        <thead>
            <tr>
                <td style="text-align:center">Detalhes</td>
                <td style="text-align:center">Cliente</td>
                <td style="text-align:center">Assessor</td>
                <td style="text-align:center">Papel</td>
                <td style="text-align:center">Solicitação</td>
                <td style="text-align:center">Vencimento</td>
                <td style="text-align:center">Margem</td>
                <td style="text-align:center">Qtd.</td>
                <td style="text-align:center">Valor</td>
                <td style="text-align:center">Valor Max</td>
                <td style="text-align:center">Valor Min</td>
                <td style="text-align:center">Preço Direto</td>
                <td style="text-align:center">Total Mercado</td>
                <td style="text-align:center">Total Termo</td>
                <td style="text-align:center">Status</td>
                <td style="text-align:center">Tipo Solicitação</td>
                <td style="text-align:center; min-width:40px">Ação</td>
            </tr>
        </thead>
    
        <!--tfoot>
            <tr>
                <td colspan="14" style="text-align:right; padding-top:1em">
                    <button class="" id="btnCancelarOrdensStop" onclick="return btnMonitoramento_ExcluirOrdensStartStopSelecionados_Click(this)" runat="server">Cancelar Selecionados</button>
                </td>
            </tr>
        </tfoot-->
        <tbody id="tblTermos">

            <asp:repeater id="rptResultadoTermos" runat="server">
            <itemtemplate>

            <tr cla__ss="<%# Eval("ClasseDaLinha") %>" data-IdTermo="<%# Eval("IdOrdemTermo") %>" data-IdCliente="<%# Eval("IdCliente") %>" data-TipoSolicitacao="<%# Eval("TipoSolicitacao") %>">
                <td class="GradGrid_Ignore"><%# Eval("BotaoLucroPrejuizo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("IdCliente") %></td>
                <td class="GradGrid_Ignore"><%# Eval("IdAssessor") %></td>
                <td class="GradGrid_Ignore"><%# Eval("Instrumento") %></td>
                <td class="GradGrid_Ignore"><%# Eval("DataSolicitacao") %></td>
                <td class="GradGrid_Ignore"><%# Eval("DataVencimento") %></td>
                <td class="GradGrid_Ignore"><%# Eval("MargemRequerida") %></td>
                <td class="GradGrid_Ignore"><%# Eval("Quantidade") %></td>
                <td class="GradGrid_Ignore"><%# Eval("PrecoDireto") %></td>
                <td class="GradGrid_Ignore"><%# Eval("PrecoMaximo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("PrecoMinimo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("PrecoTermo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("TotalMercado") %></td>
                <td class="GradGrid_Ignore"><%# Eval("TotalTermo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("StatusOrdemTermo") %></td>
                <td class="GradGrid_Ignore"><%# Eval("TipoSolicitacao") %></td>
                <td class="GradGrid_Ignore" style="white-space:nowrap">

                    <%# Eval("BotaoEfetuar") %>
                    <%# Eval("BotaoCancelar") %>

                </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="16">Nenhum termo encontrado.</td>
            </tr>
        </tbody>

    </table>

</form>
