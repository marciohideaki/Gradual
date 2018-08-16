<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R004.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R004" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Parâmetros por Cliente</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left; min-width: 20%"> Cliente         </td>
                <td style="text-align:left;    width: 12em"> CPF / CNPJ      </td>
                <td style="text-align:left;               ">Descr. Parâmetro</td>
                <td style="text-align:right;    width: 8em"> Bolsa           </td>
                <td style="text-align:right;    width: 8em"> Descr. Grupo    </td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
        </tfoot>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            
            <tr>
                <td style="text-align:left; "> <%# Eval("NomeCliente")%>       </td>
                <td style="text-align:left; "> <%# Eval("CpfCnpj")%>           </td>
                <td style="text-align:left; "><%# Eval("DescricaoParametro")%></td>
                <td style="text-align:right;"> <%# Eval("Bolsa")%>             </td>
                <td style="text-align:right;"> <%# Eval("DescricaoGrupo")%>    </td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="6">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>

</form>


