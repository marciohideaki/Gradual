<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaxBmfEN.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.FaxBmfEN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/gradintra-theme/jquery-ui-1.8.1.custom.css?v=<%= this.VersaoDoSite %>" />
    <link rel="stylesheet" type="text/css" media="screen" href="../Skin/Default/ui.jqgrid.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Principal.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/ValidationEngine.jquery.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../Skin/Default/Intranet/Default.aspx.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Intranet/Clientes.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="all"    href="../Skin/Default/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <link rel="Stylesheet" type="text/css" media="print"  href="../Skin/Default/Relatorio.print.css?v=<%= this.VersaoDoSite %>" />

    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Relatorio.css" />
    <link rel="Stylesheet" media="print" href="../../Skin/Default/Relatorio.Print.css" />
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Principal.css" />    
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Intranet/Clientes.css" />
    <link rel="Stylesheet" media="all"   href="../../Skin/Default/Intranet/Default.css" />

</head>
<body>
    <form id="form1" runat="server">
    <div class="Relatorio">
            <div id="divGradItra_Clientes_Relatorios_Financeiro_FecharRelatorio" class="FecharRelatorios">
                <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_ImprimirTodosBmf_Click(this)">Imprimir Todos</a> | --%>
                <%--<a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Relatorios_BaixarEmArquivo_Click(this)">Baixar em Arquivo</a> |--%>
                <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Fax_Bmf_EN_Click_Print(this);">Imprimir PDF</a> |
                <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this);">Fechar</a>
            </div>

            <input type="hidden" id="hddDatasPaginacao" runat="server" />
            <input type="hidden" id="hddBolsa" value="Bmf" />
             <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

            <h1 style="min-width: 586px;font-size:20px;">
                <span>Report: Fax Bmf:</span>
                <span><img src="../../Skin/Default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
            </h1>
        
           <br />
                <table cellpadding="0" border="0" style="font-size:0.9em">
                    <tr>
                        <td>
                            <div><label style="padding-right:17px"><b>Gradual C.C.T.V.M. S/A&nbsp; </b></label>&nbsp;</div>
                            <div><label>Av. Juscelino Kubitscheck, 50 6 andar ITAIM</label></div>
                            <div><label>Internet: www.gradualinvestimentos.com.br</label></div>
                            <div><label>Ouvidoria: tel.: 0800-655-1466</label></div>
                        </td>
                        <td>
                            <div>Fone: 11 33728300</div>
                            <div>Cep: 04543-000 São Paulo - SP</div>
                            <div>E-mail: atendimento@gradualinvestimentos.com.br</div>
                            <div>e-mail ouvidoria: ouvidoria@gradualinvestimentos.com.br</div>
                        </td>
                        <td>
                            C.N.P.J.: 33.918.160/0001-73<br />
                            Número da Corretora: 227-5
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <label>To:</label><br />
                            <label>Company:</label><br />
                            <label>Fax:</label><br /><br />

                            <label>From:</label><br />
                            <label>Company:</label><br />
                            <label>Phone:</label><br />
                            <label>Fax:</label><br />
                            <label>Data:</label><br />
                         </td>
                        <td align="left">
                            <br />
                            <asp:literal id="lblCodigoCliente" runat="server"></asp:literal> - <asp:literal id="lblNomeCliente" runat="server"></asp:literal><br />
                            <br /><br />
                            <asp:literal id="lblNomeAssessor" runat="server"></asp:literal><br />
                            GRADUAL CCTVM SA<br />
                            011-40071873<br />
                            011-33728301<br />
                            <asp:literal id="lblCabecalho_DataEmissao" runat="server"></asp:literal><br />
                        </td>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><label>lRef.: Trade Executions</label><br />
                        </td>
                        <td></td>
                    </tr>
                </table>
        <br />
       
        <asp:repeater id="rptLinhasDoRelatorio" runat="server" onitemdatabound="rptLinhasDoRelatorio_ItemDataBound">
            <itemtemplate>
                <label class=tdLabel"> Date Of Settle: <%# DataBinder.Eval(Container.DataItem, "DataLiquidacao")%>  </label><br />
                <table cellspacing="0">

                    <thead>
                        <tr style="font-size:0.8em">
                            <td><%# DataBinder.Eval(Container.DataItem, "CabecalhoSentido").ToString() == "C" ? "BOUGHT" : "SOLD" %></td>
                            <td style="font-weight:bold" > <%# DataBinder.Eval(Container.DataItem, "CabecalhoCommod").ToString() %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CabecalhoTipoMercado")%></td>
                            <td align="center" colspan="2"><%# DataBinder.Eval(Container.DataItem, "CabecalhoSerie")%></td>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td colspan="3">Instrument</td>
                            <td align="right">Quant</td>
                            <td align="right" >Price</td>
                        </tr>
                        <asp:repeater id="rptLinhasDoRelatorioDetalhes" runat="server">
                        <itemtemplate>
                            <tr style="font-size:0.9em">
                                <td class="tdLabel" colspan="3">   <%# DataBinder.Eval(Container.DataItem, "PapelCodigoNegocio")%>  </td>
                                <td class="tdLabel ValorNumerico" ><%# DataBinder.Eval(Container.DataItem, "PapelQuantidade")%>     </td>
                                <td class="tdValor ValorNumerico" ><%# DataBinder.Eval(Container.DataItem, "PapelPreco")%>          </td>
                            </tr>                
                        </itemtemplate>
                        </asp:repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="3">TOTAL GROSS</td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaQuantidade")%> </td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "SomaPreco")%>      </td>
                        </tr>
                        <tr>
                            <td colspan="3">NET</td>
                            <td class ="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "NetQuantidade")%>   </td>
                            <td class="tdLabel ValorNumerico"><%# DataBinder.Eval(Container.DataItem, "NetPreco")%>         </td>
                            <td></td>
                        </tr>
                    </tfoot>

                </table>
            </itemtemplate>
        </asp:repeater>

        </div>
    </div>
    </form>
</body>
</html>
