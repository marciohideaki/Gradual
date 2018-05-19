<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R001.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Sistema.Relatorios.R001" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Logs da Intranet</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left; width:15%;">  Nome do Usuário  </td>
                <td style="text-align:left;">             Login            </td>
                <td style="text-align:center; width:7%;"> IP               </td>
                <td style="text-align:left;">             Evento           </td>
                <td style="text-align:left;">             Nome da Tela     </td>
                <td style="text-align:center;width:15%;"> Data Evento      </td>
                <td style="text-align:left; width:15%;">  Nome Cliente     </td>
                <td style="text-align:center;">           CPF/CNPJ Cliente </td>
                <td style="text-align:left;">             Observação       </td>
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
            
            <tr class="Sistema_Relatorio_FundoLinha_<%# Eval("Evento")        %>">
                <td style="text-align:left;">       <%# Eval("UsuarioNome")   %></td>
                <td style="text-align:left;">       <%# Eval("UsuarioEmail")  %></td>
                <td style="text-align:right;">      <%# Eval("IP")            %></td>
                <td style="text-align:left;">       <%# Eval("Evento")        %></td>
                <td style="text-align:left;">       <%# Eval("NomeTela")      %></td>
                <td style="text-align:center;">     <%# Eval("Data")          %></td>
                <td style="text-align:left;">       <%# Eval("ClienteNome")   %></td>
                <td style="text-align:left;">       <%# Eval("CpfCnpj")       %></td>
                <td style="text-align:left;">       <%# Eval("Observacao")    %></td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>

</form>