<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R008.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R008" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Bloqueio por Grupo</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left;     width: 8em">   Código Bovespa </td>
                <td style="text-align:left; min-width: 33%">   Cliente        </td>
                <td style="text-align:left;     width:12em">   CPF / CNPJ     </td>
                <td style="text-align:left;     width: auto;"> Parâmetro      </td>
                <td style="text-align:left;     width: auto;"> Grupo          </td>
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
                <td style="text-align:left; "> <%# Eval("CodigoBovespa")%> </td>
                <td style="text-align:left; "> <%# Eval("NomeCliente")%>   </td>
                <td style="text-align:left; "> <%# Eval("CpfCnpj")%>       </td>
                <td style="text-align:left;">  <%# Eval("Parametro")%>     </td>
                <td style="text-align:left;">  <%# Eval("Grupo")%>         </td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>
    
</form>