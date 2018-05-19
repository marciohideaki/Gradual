<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R010.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R010" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Custódia - Mercado x Vista</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left;   width: 008em"> Código Assessor           </td>
                <td style="text-align:left;   width: 008em"> Código Cliente            </td>
                <td style="text-align:left; min-width: 33%"> Nome Cliente              </td>
                <td style="text-align:left;   width: 008em"> Código Negocio            </td>
                <td style="text-align:left;   width: 008em"> QTDE. Total               </td>
                <td style="text-align:left;   width: 008em"> QTDE. D1                   </td>
                <td style="text-align:left;   width: 008em"> QTDE. D2                   </td>
                <td style="text-align:left;   width: 008em"> QTDE. D3                   </td>
            </tr>
        </thead>

       <%-- <tfoot>
            <tr>
                <td colspan="7">&nbsp;</td>
                <td colspan="2" align="right">
                    <span style="font-weight:bolder;text-align:right;">Total Bloqueado: &nbsp; R$ <asp:label id="lblTotalBloqueado" runat="server"></asp:label></span>
                </td>
                <td>&nbsp;</td>
            </tr>
        </tfoot>--%>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            
            <tr>
                <td style="text-align:right;">  <%# Eval("CodigoAssessor") %>   </td>
                <td style="text-align:right;">  <%# Eval("CodigoCliente") %>    </td>
                <td style="text-align:left;">   <%# Eval("NomeCliente") %>      </td>
                <td style="text-align:left;">   <%# Eval("CodigoNegocio") %>    </td>
                <td style="text-align:left;">   <%# Eval("QtdeTotal") %>        </td>
                <td style="text-align:left;">   <%# Eval("QtdeD1") %>           </td>
                <td style="text-align:left;">   <%# Eval("QtdeD2")%>            </td>
                <td style="text-align:left;">  <%# Eval("QtdeD3")%>             </td>
                
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="8">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>
    
</form>
