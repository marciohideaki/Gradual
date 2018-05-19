<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClubesEFundos.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.ClubesEFundos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <input type="hidden" id="hidClientes_Clubes_ListaJson" class="ListaJson" runat="server"/>
    <input type="hidden" id="hidClientes_Fundos_ListaJson" class="ListaJson" runat="server"/>

    <h4>Clubes e Fundos</h4>

    <h5>Selecione abaixo o per&iacute;odo de consulta para o relatório de Clubes e Fundos.</h5>
    
    <h4 style="margin: 2em 2em 1em 2em;">Posi&ccedil;&atilde;o de Clubes</h4>

    <table id="tblCliente_Clubes" class="GridIntranet" style="margin: 0em 3em 1em 3em; width: 91%;" cellspacing="0">
        <thead>
            <tr>
                <td>Nome do Clube</td>
                <td align="center">Cota</td>
                <td align="center">Quantidade</td>
                <td align="center">Valor Bruto (R$)</td>
                <td align="center">IR %</td>
                <td align="center">IOF %</td>
                <td align="right">Valor L&iacute;quido (R$)</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="7"></td>
            </tr>
        </tfoot>
        <tbody>
            <tr align="right" class="Template"">
                
                <td propriedade="NomeClube" align="left"></td>
                <td propriedade="Cota"></td>
                <td propriedade="Quantidade"></td>
                <td propriedade="ValorBruto"></td>
                <td propriedade="IR"></td>
                <td propriedade="IOF"></td>
                <td propriedade="ValorLiquido"></td>
            </tr>
            <tr class="Nenhuma">
                <td colspan="7">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    </table>
    
    <h4 style="margin: 2em 2em 1em 2em;">Posi&ccedil;&atilde;o de Fundos</h4>

    <table id="tblCliente_Fundos" class="GridIntranet" style="margin: 0em 3em 1em 3em; width: 91%;" cellspacing="0">
        <thead>
            <tr>
                <td align="center">CodigoAnbima</td>
                <td>Nome do Fundo</td>
                <td align="center">Cota</td>
                <td align="center">Quantidade</td>
                <td align="center">Valor Bruto (R$)</td>
                <td align="center">IR</td>
                <td align="center">IOF</td>
                <td align="right">Valor L&iacute;quido (R$)</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="8"></td>
            </tr>
        </tfoot>
        <tbody>
            <tr align="right" class="Template">
                <td propriedade="CodigoAnbima"></td>
                <td propriedade="NomeFundo" align="left"></td>
                <td propriedade="Cota"></td>
                <td propriedade="Quantidade"></td>
                <td propriedade="ValorBruto"></td>
                <td propriedade="IR"></td>
                <td propriedade="IOF"></td>
                <td propriedade="ValorLiquido"></td>
            </tr>
            <tr class="Nenhuma">
                <td colspan="8">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    </table>

     <h4 style="margin: 2em 2em 1em 2em;">Posi&ccedil;&atilde;o de Renda Fixa</h4>

    <table id="tbCliente_RendaFixa" class="GridRelatorio" style="margin: 0em 3em 1em 3em; width: 91%;" cellspacing="0">
        <thead>
            <tr>
                <td align="left">Título</td>
                <td align="center">Aplicação</td>
                <td align="center">Vencimento</td>
                <td align="center">Taxa</td>
                <td align="center">Quantidade</td>
                <td align="center">Valor Original</td>
                <td align="center">Saldo Bruto</td>
                <td align="center">IRRF</td>
                <td align="center">IOF</td>
                <td align="right">L&iacute;quido</td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="10"></td>
            </tr>
        </tfoot>
        <tbody>
            <tr align="right" class="Template" style="font-size:x-small" >
                <td propriedade="Titulo"        align="left"></td>
                <td propriedade="Aplicacao"     align="left"></td>
                <td propriedade="Vencimento"            ></td>
                <td propriedade="Taxa"          style="width:58px"       ></td>
                <td propriedade="Quantidade"            ></td>
                <td propriedade="ValorOriginal"         ></td>
                <td propriedade="SaldoBruto"            ></td>
                <td propriedade="IRRF"                  ></td>
                <td propriedade="IOF"                   ></td>
                <td propriedade="SaldoLiquido"          ></td>
            </tr>
            <tr class="Nenhuma">
                <td colspan="10">Nenhum item encontrado.</td>
            </tr>
        </tbody>
    </table>

    <script type="text/javascript">
        $.ready(CarregarRelatorioClubesEFundos_Click());
    </script>
    </div>
    </form>
</body>
</html>
