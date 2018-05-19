<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R002.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.Relatorios.R0021" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Autorizações de Cadastro</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio" style="font-size:9px;">
    
        <thead>
            <tr>
                <td style="text-align:left; width:15%;">        Nome do Cliente  </td>
                <td style="text-align:left;">                   CPF  </td>
                <td style="text-align:left;white-space: nowrap;">Dt. Export.</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Diretor 1</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Diretor 2</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Procurador 1</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Procurador 2</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Testemunha 1</td>
                <td style="text-align:center;white-space: nowrap" colspan="2">Aut Testemunha 2</td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="15">&nbsp;</td>
            </tr>
        </tfoot>

        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>

            <tr>
                <td><%# Eval("NomeCliente") %></td>
                <td><%# Eval("CPF") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("DataExportacao") %></td>

                <td><%# Eval("DataAutorizacao_D1") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("Nome_D1") %></td>

                <td><%# Eval("DataAutorizacao_D2") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("Nome_D2") %></td>

                <td><%# Eval("DataAutorizacao_P1") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("Nome_P1") %></td>

                <td><%# Eval("DataAutorizacao_P2") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("Nome_P2") %></td>

                <td><%# Eval("DataAutorizacao_T1") %></td>
                <td style="border-right:1px solid #f2f2f2"><%# Eval("Nome_T1") %></td>

                <td><%# Eval("DataAutorizacao_T2") %></td>
                <td><%# Eval("Nome_T2") %></td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="15">Nenhum item encontrado.</td>
            </tr>
        </tbody>

    </table>

</form>
