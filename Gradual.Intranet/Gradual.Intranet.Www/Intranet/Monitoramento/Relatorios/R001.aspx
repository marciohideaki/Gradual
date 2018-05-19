<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R001.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Monitoramento.Relatorios.R001" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Cliente Posição de Opção</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:right; width:7%;">    Código do Cliente  </td>
                <td style="text-align:left;  width:25%;">   Nome do Cliente    </td>
                <td style="text-align:right;">              Código Assessor    </td>
                <td style="text-align:center;">             Nome Assessor      </td>
                <td style="text-align:center;">             Instrumento        </td>
                <td style="text-align:right;">              Quant. Abert.      </td>
                <td style="text-align:right;">              Quant. Atual       </td>
                <td style="text-align:right;">              Quant. D1          </td>
                <td style="text-align:center;">             Dt.Venc.(Strike)   </td>
                <td style="text-align:center;">             Carteira           </td>
                <td style="text-align:center;">             Preço Exercício    </td>
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
            
            <tr>
                <td style="text-align:right;"> <%# Eval("CodCliente")  %>      </td>
                <td style="text-align:left;">  <%# Eval("NomeCliente") %>      </td>
                <td style="text-align:center;"><%# Eval("CodAssessor") %>      </td>
                <td style="text-align:left;">  <%# Eval("NomeAssessor")%>      </td>
                <td style="text-align:center;"><%# Eval("Instrumento") %>      </td>
                <td style="text-align:center;"><%# Eval("QuantidadeAbertura")%></td>
                <td style="text-align:center;"><%# Eval("QuantidadeAtual")%>   </td>
                <td style="text-align:center;"><%# Eval("QuantidadeD1")%>      </td>
                <td style="text-align:center;"><%# Eval("DtStrike")    %>      </td>
                <td style="text-align:center;"><%# Eval("Carteira")    %>      </td>
                <td style="text-align:center;"><%# Eval("PrecoExercicio")%>    </td>
            </tr>
               
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>

</form>
