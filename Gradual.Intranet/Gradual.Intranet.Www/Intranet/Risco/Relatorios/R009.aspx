<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R009.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R009" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Composição de Saldo Bloqueado</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="text-align:left;   width: 008em"> Código Bovespa            </td>
                <td style="text-align:left; min-width: 33%"> Cliente                   </td>
                <td style="text-align:left;   width: 012em"> CPF / CNPJ                </td>
                <td style="text-align:left;   width: auto;"> Ativo                     </td>
                <td style="text-align:left;   width: auto;"> Tipo de Ordem             </td>
                <td style="text-align:left;   width: auto;"> Status da Ordem           </td>
                <td style="text-align:right;  width: auto;"> Quantidade                </td>
                <td style="text-align:right;  width: auto;"> Preço (R$)                </td>
                <td style="text-align:right;  width: auto;"> Bloqueado Semi Total (R$) </td>
                <td style="text-align:center; width: auto;"> Data/Hora Transação       </td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="7">&nbsp;</td>
                <td colspan="2" align="right">
                    <span style="font-weight:bolder;text-align:right;">Total Bloqueado: &nbsp; R$ <asp:label id="lblTotalBloqueado" runat="server"></asp:label></span>
                </td>
                <td>&nbsp;</td>
            </tr>
        </tfoot>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            
            <tr>
                <td style="text-align:right;">  <%# Eval("CodigoBovespa") %>      </td>
                <td style="text-align:left;">   <%# Eval("NomeCliente") %>        </td>
                <td style="text-align:left;">   <%# Eval("CpfCnpj") %>            </td>
                <td style="text-align:left;">   <%# Eval("Ativo") %>              </td>
                <td style="text-align:left;">   <%# Eval("TipoOrdem") %>          </td>
                <td style="text-align:left;">   <%# Eval("StatusOrdem") %>        </td>
                <td style="text-align:right;">  <%# Eval("Quantidade") %>         </td>
                <td style="text-align:right;">  <%# Eval("Preco") %>              </td>
                <td style="text-align:right;">  <%# Eval("BloqueadoSemiTotal") %> </td>
                <td style="text-align:center;"> <%# Eval("Data") %>               </td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    
    </table>
    
</form>