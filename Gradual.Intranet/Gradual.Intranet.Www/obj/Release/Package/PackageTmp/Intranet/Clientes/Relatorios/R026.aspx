<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R026.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R026" %>
<form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Posição de Custódia e Financeiro</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span><br />
            <span><%=string.Format("Codigo Cliente - {0} <br/> Nome Cliente - {1}",gCodigoCliente, gNomeCliente) %></span>
    </h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td colspan="13">Relatório de Posição de Custódia e Financeiro</td>
        </tr>
        
        <tr>
            <td colspan="5"> À Vista </td>
        </tr>
        
        <tr>
            <td align="left">   Título          </td>
            <td align="left">   Quantidade      </td>
            <td align="center"> Custo           </td>
            <td align="center"> Valor Atual     </td>
            <td align="center"> Variação        </td>
        </tr>
    </thead>

    <tbody>
        <asp:Repeater id="rptRelatorioPosicaoVista" runat="server">
            <ItemTemplate>
                <tr align="right" class="Template" style="font-size:x-small" >
                <td propriedade="Titulo"     align="left">           <%# Eval("Titulo")         %>  </td>
                <td propriedade="Quantidade" align="left">           <%# Eval("Quantidade")     %>  </td>
                <td propriedade="Custo"      align="center">           <%# Eval("Custo")          %>  </td>
                <td propriedade="ValorAtual" align="center">                       <%# Eval("ValorAtual")     %>  </td>
                <td propriedade="Variacao"   style="width:58px">     <%# Eval("Variacao")       %>  </td>
            </itemtemplate>
        </asp:repeater>

        <%--<tr id="rowsLinhaTotalRepasse" class="NenhumItem" runat="server">
            <td colspan="7" style="font-style:italic">Total de Repasse</td>
            <td style="font-style:italic"><b><%=TotalRepasse %></b></td>
        </tr>--%>
        <tr id="rowLinhaDeNenhumItemPosicaoVista" class="NenhumItem" runat="server">
            <td colspan="5">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="7">Opção</td>
        </tr>
        <tr>
            <td align="left">   Titulo          </td>
            <td align="left">   Exercício       </td>
            <td align="left">   Quantidade      </td>
            <td align="left">   Custo           </td>
            <td align="left">   Valor Exercício </td>
            <td align="left">   Custo Total     </td>
            <td align="left">   Valor Objeto    </td>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater id="rptRelatorioPosicaoOpcao" runat="server">
            <itemTemplate>
                <tr>
                <td ><%# Eval("Titulo")         %></td>
                <td ><%# Eval("Exercicio")  %></td>
                <td ><%# Eval("Quantidade")     %></td>
                <td ><%# Eval("Custo")     %></td>
                <td ><%# Eval("ValorExercicio")  %></td>
                <td ><%# Eval("CustoTotal")     %></td>
                <td ><%# Eval("ValorObjeto")    %></td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemPosicaoOpcao" class="NenhumItem" runat="server">
            <td colspan="7">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="5">Termo</td>
        </tr>
        <tr>
            <td align="center"> Título   </td>
            <td align="center"  >   Quantidade   </td>
            <td align="center"  >   Data Vencimento     </td>
            <td align="center"  >   Data Rolagem  </td>
            <td align="center"  >   Valor      </td>
            
        </tr>                    
    </thead>                     
    <tbody>
        <asp:Repeater id="rptRelatorioPosicaoTermo" runat="server">
            <itemTemplate>
                <tr>
                <td align="left"><%# Eval("Titulo")  %>  </td>
                <td align="center"  ><%# Eval("Quantidade")  %>  </td>
                <td align="center"  ><%# Eval("DataVencimento")    %>  </td>
                <td align="center"  ><%# Eval("DataRolagem") %>  </td>
                <td align="center"  ><%# Eval("ValorTermo")     %>  </td>
                
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemPosicaoTermo" class="NenhumItem" runat="server">
            <td colspan="7">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="4">Tesouro Direto</td>
        </tr>
        <tr>
            <td align="left"> Título          </td>
            <td align="center">   Quantidade      </td>
            <td align="center">   Data Vencimento </td>
            <td align="center">   Valor Original  </td>
            
        </tr>                    
    </thead>                     
    <tbody>
        <asp:Repeater id="rptRelatorioPosicaoTesouro" runat="server">
            <itemTemplate>
                <tr>
                <td align="left"  ><%# Eval("Titulo")         %>  </td>
                <td align="center"><%# Eval("Quantidade")     %>  </td>
                <td align="center"><%# Eval("DataVencimento") %>  </td>
                <td align="center"><%# Eval("ValorOriginal")  %>  </td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemPosicaoTesouro" class="NenhumItem" runat="server">
            <td colspan="4">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />






<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="5">Extrato À Vista</td>
        </tr>
        <tr>
            <td align="left"> Título      </td>
            <td align="center">   Dia         </td>
            <td align="center">   Data Negócio</td>
            <td align="left">   Histórico   </td>
            <td align="center">   Quantidade  </td>
            
        </tr>                    
    </thead>                     
    <tbody>
        <asp:Repeater id="rptRelatorioExtratoVista" runat="server">
            <itemTemplate>
                <tr>
                <td align="left"><%# Eval("Titulo")         %>  </td>
                <td align="center"><%# Eval("Dia")            %>  </td>
                <td align="center"><%# Eval("DataNegocio")    %>  </td>
                <td align="left"><%# Eval("Historico")      %>  </td>
                <td align="center"><%# Eval("Quantidade")     %>  </td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemExtratoVista" class="NenhumItem" runat="server">
            <td colspan="5">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="5">Extrato Opção</td>
        </tr>
        <tr>
            <td align="left"> Título      </td>
            <td align="center">   Dia         </td>
            <td align="center">   Data Negócio</td>
            <td align="left">   Histórico   </td>
            <td align="center">   Quantidade  </td>
            
        </tr>                    
    </thead>                     
    <tbody>
        <asp:Repeater id="rptRelatorioExtratoOpcao" runat="server">
            <itemTemplate>
                <tr>
                <td align="left"><%# Eval("Titulo")  %>      </td>
                <td align="center"><%# Eval("Dia")  %>         </td>
                <td align="center"><%# Eval("DataNegocio")    %>  </td>
                <td align="left"><%# Eval("Historico")    %>  </td>
                <td align="center"><%# Eval("Quantidade")     %>  </td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemExtratoOpcao" class="NenhumItem" runat="server">
            <td colspan="5">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="5">Extrato Termo</td>
        </tr>
        <tr>
            <td align="left">   Título      </td>
            <td align="center">   Dia         </td>
            <td align="center">   Data Negócio</td>
            <td align="left">   Histórico   </td>
            <td align="center">   Quantidade  </td>
            
        </tr>                    
    </thead>                     
    <tbody>
        <asp:Repeater id="rptRelatorioExtratoTermo" runat="server">
            <itemTemplate>
                <tr>
                <td align="left"><%# Eval("Titulo")         %>  </td>
                <td align="center"><%# Eval("Dia")            %>  </td>
                <td align="center"><%# Eval("DataNegocio")    %>  </td>
                <td align="left"><%# Eval("Historico")      %>  </td>
                <td align="center"><%# Eval("Quantidade")     %>  </td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemExtratoTermo" class="NenhumItem" runat="server">
            <td colspan="5">
                Nenhum item encontrado.
             </td>
        </tr>
    </tbody>
</table>
<br />
<table cellspacing="0" class="GridRelatorio" width="500px">
    <thead>
        <tr>
            <td colspan="2"></td>
            <td style="text-align:right">Saldo Disponível:</td>
            <td><%=gSaldoAtual %></td>

            <td style="text-align:right">Saldo Anterior:</td>
            <td><%=gSaldoAnterior %></td>
        </tr>
        <tr>
            <td align="center">   Data de Liquidação  </td>
            <td align="center">   Data de Movimento   </td>
            <td align="center">   Histórico           </td>
            <td align="center">   Valor de Crédito    </td>
            <td align="center">   Valor de Débito     </td>
            <td align="center">   Valor Saldo         </td>
        </tr>
    </thead>

    <tbody>
    
        <asp:Repeater id="rptRelatorioExtratoMovimento" runat="server">
            <itemTemplate>
                <tr>
                <td ><%# Eval("DataLiquidacao")%>   </td>
                <td ><%# Eval("DataMovimento") %>   </td>
                <td ><%# Eval("Historico")%>        </td>
                <td ><%# Eval("ValorCredito")%>     </td>
                <td ><%# Eval("ValorDebito")%>      </td>
                <td ><%# Eval("ValorSaldo")%>       </td>
                </tr>
            </itemTemplate>
        </asp:Repeater>
        <tr id="rowLinhaDeNenhumItemExtratoMovimento" class="NenhumItem" runat="server">
            <td colspan="6">
                Nenhum item encontrado.
             </td>
        </tr>
        <tr>
            <td colspan="4"></td>
            <td style="text-align:right" >Saldo Total:</td>
            <td><%=gSaldoTotal%></td>
        </tr>
        
    </tbody>
    <tfoot>
        <tr>
            <td colspan="6">
                <span>Relatório de Posição de Extrato de Movimento</span><span>
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
</table>
</form>