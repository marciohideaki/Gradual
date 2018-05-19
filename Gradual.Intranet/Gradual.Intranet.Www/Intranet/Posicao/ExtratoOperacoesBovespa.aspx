<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtratoOperacoesBovespa.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Posicao.ExtratoOperacoesBovespa" %>

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
                <a href="#" onclick="GradItra_Clientes_Relatorios_Financeiro_Extrato_Bovespa_Click_Print(this);">Imprimir PDF</a> |
                <a href="#" onclick="GradIntra_Clientes_Financeiro_FecharRelatorio_Default(this);GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(this);">Fechar</a>
            </div>

            <input type="hidden" id="hddDatasPaginacao" runat="server" />
            <input type="hidden" id="hddBolsa" value="Bmf" />
             <br />

        <div style="padding-left:20px;" class="RelatorioImpressao">

        
            <h1 style="min-width: 586px;font-size:20px;">
                <span>Relatório de Ordens</span>
                <span><img src="../../Skin/Default/Img/HB01_Relatorio_Titulo_FundoGradual.png" /></span>
            </h1>
        
           <br />
                <table cellpadding="0" border="0" style="font-size:0.9em">
                <tbody>
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
                    </tbody>
                    </table>
                    
                    <asp:repeater id="rptLinhasDoRelatorio" runat="server" onitemdatabound="rptLinhasDoRelatorio_ItemDataBound">
                    <itemtemplate>
                    <table cellpadding="0" cellspacing="0" border="0" style="margin:0px 0px 0px 0px">
                        <tbody>
                        <tr>
                            <td colspan="2">ORDEM DE <%# DataBinder.Eval(Container.DataItem, "Sentido").ToString()  %><br/>
                                <%# DataBinder.Eval(Container.DataItem, "Status").ToString() == "D" ? "CANCELADA" : "" %>
                            </td>
                            <td width="10%">
                            Data : <%=DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") %><br />
                            Número : <%# DataBinder.Eval(Container.DataItem, "NumeroOrdem").ToString()%></td>
                        </tr>
                        <tr>
                        <td colspan="3" >
                            <table>
                            <thead>
                                <tr>
                                <td colspan="4"><span >Cliente:</span>             &nbsp;  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "NomeCliente").ToString() %></span>        </td>
                                <td><span> Codigo:</span>                          &nbsp;  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "CodigoCliente").ToString() %></span>                  </td>
                                </tr>                                                                                                                                                                                 
                                <tr>                                                                                                                                                                                  
                                <td><span>Mercado:</span>                         <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "TipoMercado").ToString() %></span>                    </td>
                                <td><span>Tipo de Ordem:</span>                   <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "TipoOrdem").ToString() %></span>                      </td>
                                <td><span>Prazo de Validade:</span>               <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Vencimento").ToString() %></span>          </td>
                                <td><span>Bolsa:</span>                           <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "TipoBolsa").ToString() %></span>                      </td>
                                <td><span>Pessoa Vinculada:</span>                <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "PessoaVinculada").ToString()%></span>                 </td>
                                </tr>                                                                                                                                                                                 
                                <tr>                                                                                                                                                                                  
                                <td><span>Companhia:</span>                        <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Companhia").ToString() %></span>                      </td>
                                <td><span>Tipo:</span>                             <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "TipoCompanhia").ToString() %></span>                  </td>
                                <td><span>Quantidade:</span>                       <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Quantidade").ToString() %></span>                     </td>
                                <td><span>Preço:</span>                            <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Preco").ToString() %></span>                          </td>
                                <td><span>Assessor:</span>                         <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "CodigoAssessor").ToString() %></span>                 </td>
                                </tr>                                                                                                                                                                                 
                                <tr>                                                                                                                                                                                  
                                <td><span>Especificações Títulos / Opções:</span>  <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Papel").ToString() %></span>                          </td>
                                <td><span>Prazo/ Vcto.</span>                      <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "PrazoVencimento").ToString() %></span>                </td>
                                <td><span>Preço de Exercício</span>                <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "PrecoExercicio").ToString() %></span>                 </td>
                                <td><span>Executado</span>                         <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "QuantidadeExecutada").ToString() %></span>            </td>
                                <td><span>Saldo</span>                             <br/>  <span style="font-size:1em"><%# DataBinder.Eval(Container.DataItem, "Saldo").ToString() %></span>                          </td>
                                </tr>
                            </thead>
                            </table>
                        </td>
                    </tr>
                </tbody>
                </table>
                <table cellspacing="0"  >
                    <thead>
                        <tr>
                            <td> Data</td>
                            <td >Operacao</td>
                            <td >Quantidade</td>
                            <td align="right" >Preço</td>
                            <td>C/Parte</td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:repeater id="rptLinhasDoRelatorioDetalhes" runat="server">
                        <itemtemplate>
                            <tr style="font-size:0.9em">
                                <td class="tdLabel" >              <%# DataBinder.Eval(Container.DataItem,  "Data")%>               </td>
                                <td class="tdLabel" >              <%# DataBinder.Eval(Container.DataItem,  "NumeroOperacao")%>     </td>
                                <td class="tdLabel ValorNUmerico" ><%# DataBinder.Eval(Container.DataItem,  "Quantidade")%>         </td>
                                <td class="tdLabel ValorNumerico" ><%# DataBinder.Eval(Container.DataItem,  "Preco")%>              </td>
                                <td class="tdValor" >              <%# DataBinder.Eval(Container.DataItem,  "CodigoContraParte")%>  </td>
                            </tr>                
                        </itemtemplate>
                        </asp:repeater>
                    </tbody>
                </table>
                <br />
            </itemtemplate>
        </asp:repeater>
        
        </div>
    </div>
    </form>
</body>
</html>


