<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R002.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.Relatorios.R002" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Exercício de opção</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left; width:5%;">     Código              </td>
                <td style="text-align:left; width: 15%">    Nome do Cliente     </td>
                <td style="text-align:center; width:7%;">   Assessor            </td>
                <td style="text-align:center; width:15%">   Nome do Assessor    </td>
                <td style="text-align:left; width:7%">      Instrumento         </td>
                <td style="text-align:center;width:5%;">    Strike              </td>
                <td style="text-align:center; width:15%;">  Vencimento          </td>
                <td style="text-align:center;">             Quantidade Abertura </td>
                <td style="text-align:center;">             Operações Dia       </td>
                <td style="text-align:center;">             Qtde. Exercício     </td>
                <td style="text-align:center;">             Qtde. Atual         </td>
                <td style="text-align:center;">             Carteira            </td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="14">&nbsp;</td>
            </tr>
        </tfoot>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            
            <tr class="Sistema_Relatorio_FundoLinha_<%# Eval("CodCliente")          %>">
                <td style="text-align:left;">         <%# Eval("CodCliente")          %></td>
                <td style="text-align:left;">         <%# Eval("NomeCliente")         %></td>
                <td style="text-align:left;">         <%# Eval("CodAssessor")         %></td>
                <td style="text-align:left;">         <%# Eval("NomeAssessor")        %></td>
                <td style="text-align:left;">         <%# Eval("Instrumento")         %></td>
                <td style="text-align:center;">       <%# Eval("Strike")              %></td>
                <td style="text-align:center;">       <%# Eval("Vencimento")          %></td>
                <td style="text-align:center;">       <%# Eval("QuantidadeAbertura")  %></td>
                <td style="text-align:center;">       <%# Eval("OperacoesDia")        %></td>
                <td style="text-align:center;">       <%# Eval("QtdeExercicio")       %></td>
                <td style="text-align:center;">       <%# Eval("QuantidadeAtual")     %></td>
                <td style="text-align:center;">       <%# Eval("Carteira")            %></td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>
    </form>
</body>
</html>
