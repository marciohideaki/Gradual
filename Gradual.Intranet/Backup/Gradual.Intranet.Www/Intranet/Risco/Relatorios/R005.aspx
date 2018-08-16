<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R005.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Relatorios.R005" %>

<form id="form1" runat="server">

    <div class="CabecalhoRelatorio RelatorioImpressao">

        <h1>Relatório de <span>Limite por Cliente</span></h1>

        <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

        <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

    </div>
    <div id="divRelatorio_LimitePorCliente_DataAtualizacao" style="width: 21em; float: right; display: none; padding-botton: 25px;">
        <h2>Data de vencimento</h2>
        <p>
            <input id="txtRelatorio_LimitePorCliente_DataAtualizacao" type="text" propriedade="DataAtualizacao" maxlength="10" style="width: 63px;" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" class="Mascara_Data Picker_Data" />
            <span style="padding-left: 5px;"><a onclick="GradIntra_Relatorio_Risco_DataVencimentoAtualizar_Click(this);" style="cursor:pointer">Atualizar</a></span>
            <div style="padding-left: 105px;"><a onclick="GradIntra_Relatorio_Risco_DataVencimentoCancelar_Click(this);" style="cursor:pointer">Cancelar</a></div>
        </p>
    </div>
    <table cellspacing="0" class="GridRelatorio">
    
        <thead>
            <tr>
                <td style="display:none;">IdClienteParametro</td>
                <td style="text-align:left;   width: auto;"></td>
                <td style="text-align:left;   width: auto;"> Cliente   </td>
                <td style="text-align:left;   width: 10em;"> Cpf/Cnpj              </td>
                <td style="text-align:left;">              Parâmetro             </td>
                <td style="text-align:right;  width: 6em;"> Valor Limite          </td>
                <td style="text-align:right;  width: 6em;"> Valor Alocado         </td>
                <td style="text-align:right;  width: 6em;"> Disponível            </td>
                <td style="text-align:center; width: 6em;"> Validade do Parâmetro </td>
            </tr>
        </thead>

        <tfoot>
            <tr>
                <td colspan="9" align="center">&nbsp;</td>
            </tr>
        </tfoot>
    
        <tbody>

            <asp:repeater id="rptRelatorio" runat="server" onitemdatabound="rptRelatorio_ItemDataBound">
            <itemtemplate>
            
            <tr>
                <td style="display:none;"><input type="hidden" id="txtRelatorio_LimitePorCliente_IdClienteParametro" value="<%# Eval("IdClienteParametro") %>" /></td>
                <td style="text-align:left;"><a style="cursor:pointer;" onclick="return GradIntra_Relatorio_Risco_ExpandCollapse(this);">[ + ]</a></td>
                <td style="text-align:left; "> <%# Eval("NomeCliente") %> </td>
                <td style="text-align:left; "> <%# Eval("CpfCnpj")     %> </td>
                <td style="text-align:left; "> <%# Eval("Parametro")   %> </td>
                <td style="text-align:right;white-space:nowrap;"> <%# Eval("ValorLimite")    %> </td>
                <td style="text-align:right;white-space:nowrap;"> <%# Eval("ValorAlocado")   %> </td>
                <td style="text-align:right;white-space:nowrap;"> <%# Eval("ValorDisponivel")%> </td>
                <td style="text-align:center;" class="Relatorio_LimitePorCliente_DtValidade"> <%# Eval("DataValidade")%> </td>
            </tr>

            <tr style="display:none;">
                <td colspan="9">
                    <table style="margin-left:25px;">
                        <thead>
                            <tr>
                                <td align="center" style="width: 6em;">Data do Movimento</td>
                                <td align="center" style="width: 6em;">Valor do Movimento</td>
                                <td align="center" style="width: 6em;">Valor Alocado</td>
                                <td align="center" style="width: 6em;">Valor Disponível no Movimento</td>
                                <td align="center" style="width: 60%;">Histórico</td>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <td colspan="5"></td>
                            </tr>
                        </tfoot>
                        <tbody>
                            <asp:repeater runat="server" id="rptRelatorioClienteLimiteMovimento">
                            <itemtemplate>
                            <tr>
                                <td align="center"><%# Eval("DataMovimento")%></td>
                                <td align="right"> <%# Eval("ValorMovimento")%></td>
                                <td align="right"> <%# Eval("ValorAlocado")%></td>
                                <td align="right"> <%# Eval("ValorDisponivel")%></td>
                                <td align="left">  <%# Eval("Historico")%></td>
                            </tr>
                            </itemtemplate>
                            </asp:repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
            
            </itemtemplate>
            </asp:repeater>
        
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="9">Nenhum item encontrado.o.</td>
            </tr>
        </tbody>

    </table>

</form>
