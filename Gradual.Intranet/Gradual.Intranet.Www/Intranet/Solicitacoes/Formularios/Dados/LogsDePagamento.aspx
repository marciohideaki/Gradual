<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogsDePagamento.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados.LogsDePagamento" %>


<form id="form1" runat="server">

    <div class="PainelScroll" style="height:496px; overflow:scroll;">

        <table cellspacing="0" class="GridIntranet" style="width:1500px">
    
            <thead>
                <tr>
                    <td style="width: 4%">ID</td>
                    <td style="width:10%">Data / Hora</td>
                    <td style="width:16%">ID da Transação</td>
                    <td style="width:20%">Cód. Ref. da Venda</td>
                    <td style="width:5%">Direção</td>
                    <td style="width:24%">Mensagem</td>
                    <td style="width:19%">XML</td>
                </tr>
            </thead>
            <tfoot>
                <td colspan="7">&nbsp;</td>
            </tfoot>

            <tbody>

                <asp:repeater id="rptLog" runat="server">
                <itemtemplate>

                <tr>
                    <td><%# Eval("IdPagamentoLog") %></td>
                    <td><%# Eval("DtData") %></td>
                    <td><%# Eval("DsTransacao") %></td>
                    <td><%# Eval("DsCodigoReferenciaVenda") %></td>
                    <td><%# Eval("StDirecao") %></td>
                    <td><%# Eval("DsMensagem") %></td>
                    <td>
                        <textarea class="TextAreaComoLabel" readonly="readonly" onclick="return txtAreaLabel_Click(this)" wrap="off"><%# Eval("DsXML") %></textarea>
                    </td>
                </tr>

                </itemtemplate>
                </asp:repeater>

            </tbody>

        </table>

    </div>

</form>
