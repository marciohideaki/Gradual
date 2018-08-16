<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R021.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R021" %>

 <form id="form1" runat="server">
    <div class="CabecalhoRelatorio RelatorioImpressao">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Proventos de Clientes por período ou cliente </span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    - Período De: <span><%= DataInicial.ToString("dd/MM/yyyy")%> </span> Até <span><%= DataFinal.ToString("dd/MM/yyyy")%> </span>
    </h2>

</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 3em">
                Cliente
            </td>
            <td style="text-align: left; width: 3em">
                Nome do Cliente
            </td>
            <td style="text-align: center; width: 3em">
                Assessor
            </td>
            <td style="text-align: left; width: 3em">
                Nome do Assessor
            </td>
            <td style="text-align: center; width: 2em">
                ISIN
            </td>
            <!--td style="text-align: center; width: 2em">
                Distribuição
            </td>
            <td style="text-align: center; width: 2em">
                Carteira
            </td-->
            <td style="text-align: center; width: 2em">
                Ativo
            </td>
            <td style="text-align: center; width: 2em">
                Quantidade
            </td>
            <td style="text-align: center; width: 2em">
                Valor
            </td>
            <td style="text-align: center; width: 2em">
                % IR
            </td>
            <td style="text-align: center; width: 2em">
                Valor IR
            </td>
            <td style="text-align: center; width: 2em">
                Valor Líquido
            </td>
            <td style="text-align: center; width: 2em">
                Tipo Provento
            </td>
            <!--td style="text-align: center; width: 2em">
                Grupo provento
            </td-->
            <td style="text-align: center; width: 2em">
                Data Pgto.
            </td>
            <td style="text-align: center; width: 2em">
                Emitente
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="17">
                <span>Proventos de Clientes por período ou cliente </span><span>em
                    <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
            </td>
        </tr>
    </tfoot>
    <tbody style="font-size:0.8em">
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center"> <%# Eval("CodigoCliente") %>     </td>
                <td style="text-align:left">   <%# Eval("NomeCliente")   %>     </td>
                <td style="text-align:center"> <%# Eval("CodigoAssessor") %>    </td>
                <td style="text-align:left">   <%# Eval("NomeAssessor")  %>     </td>
                <td style="text-align:center"> <%# Eval("Isin") %>              </td>
                <!--td style="text-align:center"> <%# Eval("Distribuicao")   %>    </td>
                <td style="text-align:center"> <%# Eval("Carteira") %>          </td-->
                <td style="text-align:center"> <%# Eval("Ativo")  %>            </td>
                <td style="text-align:center"> <%# Eval("Quantidade") %>        </td>
                <td style="text-align:center"> <%# Eval("Valor")   %>           </td>
                <td style="text-align:center"> <%# Eval("PercentualIR") %>      </td>
                <td style="text-align:center"> <%# Eval("ValorIR")  %>          </td>
                <td style="text-align:center"> <%# Eval("ValorLiquido") %>      </td>
                <td style="text-align:center"> <%# Eval("TipoProvento")   %>    </td>
                <!--td style="text-align:center"> <%# Eval("GrupoProvento") %>     </td-->
                <td style="text-align:center"> <%# Eval("DataPagamento")  %>    </td>
                <td style="text-align:center"> <%# Eval("Emitente")  %>         </td>
            </tr>
            
            </itemtemplate>
        </asp:repeater>
        <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
            <td colspan="14">
                Nenhum item encontrado.
            </td>
        </tr>
        <%--<tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
            <td colspan="14">
                Carregando dados, favor aguardar...
            </td>
        </tr>--%>
    </tbody>
</table>
    </form>
