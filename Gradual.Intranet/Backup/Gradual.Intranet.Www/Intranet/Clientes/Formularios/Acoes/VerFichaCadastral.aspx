<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="VerFichaCadastral.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.VerFichaCadastral" %>

<form id="form1" runat="server" class="pnlFichaCadastral">

    <h4>Dados Cadastrais de Cliente</h4>

    <div class="PainelScroll" style="background:#fff;border:1px solid #999">

    <table cellspacing="0" class="FichaCadastral">

        <tr>
            <th colspan="2">Dados Cadastrais de Cliente</th>
        </tr>

        <tr>
            <td class="Label">Nome Completo</td>
            <td>
                <asp:literal id="lblDadosBasicos_NomeCompleto" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Email</td>
            <td>
                <asp:literal id="lblDadosBasicos_Email" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Código DUC</td>
            <td>
                <asp:literal id="lblDadosBasicos_CodigoDUC" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Data do Cadastro</td>
            <td>
                <asp:literal id="lblDadosBasicos_DataDoCadastro" runat="server"></asp:literal>
            </td>
        </tr>
<%--
        <tr>
            <td class="Label">Liberado para Operar</td>
            <td>
                <asp:literal id="lblDadosBasicos_LiberadoParaOperar" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Assessor</td>
            <td>
                <asp:literal id="lblDadosBasicos_Assessor" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Filial</td>
            <td>
                <asp:literal id="lblDadosBasicos_Filial" runat="server"></asp:literal>
            </td>
        </tr>--%>

        <tr>
            <td class="Label">Data de Nascimento</td>
            <td>
                <asp:literal id="lblDadosBasicos_DataDeNascimento" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Estado de Nascimento</td>
            <td>
                <asp:literal id="lblDadosBasicos_EstadoDeNascimento" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Cidade de Nascimento</td>
            <td>
                <asp:literal id="lblDadosBasicos_CidadeDeNascimento" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trNomeDoPai" runat="server">
            <td class="Label">Nome do Pai</td>
            <td>
                <asp:literal id="lblDadosBasicos_NomeDoPai" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trNomeMae" runat="server">
            <td class="Label">Nome da Mãe</td>
            <td>
                <asp:literal id="lblDadosBasicos_NomeDaMae" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trSexo" runat="server">
            <td class="Label">Sexo</td>
            <td>
                <asp:literal id="lblDadosBasicos_Sexo" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trEstadoCivil" runat="server">
            <td class="Label">Estado Civil</td>
            <td>
                <asp:literal id="lblDadosBasicos_EstadoCivil" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trConjugue" runat="server">
            <td class="Label">Cônjuge</td>
            <td>
                <asp:literal id="lblDadosBasicos_Conjuge" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">CPF/CNPJ</td>
            <td>
                <asp:literal id="lblDadosBasicos_CPF" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trTipoDocumento" runat="server">
            <td class="Label">Tipo de Documento</td>
            <td>
                <asp:literal id="lblDadosBasicos_TipoDeDocumento" runat="server"></asp:literal>
            </td>
        </tr>
        
        <tr id="trOrgaoEmissor" runat="server">
            <td class="Label">Órgão Emissor / UF de Emissão</td>
            <td>
                <asp:literal id="lblDadosBasicos_OrgaoUfDeEmissao" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trDesejaAplicar" runat="server">
            <td class="Label">Deseja Aplicar</td>
            <td>
                <asp:literal id="lblDadosBasicos_DesejaAplicar" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trNumeroDocumento" runat="server">
            <td class="Label">Número</td>
            <td>
                <asp:literal id="lblDadosBasicos_Numero" runat="server"></asp:literal>
            </td>
        </tr>

        <tr id="trDataEmissaoDocumento" runat="server">
            <td class="Label">Data de Emissão</td>
            <td>
                <asp:literal id="lblDadosBasicos_DataDeEmissao" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Informações Comerciais</th>
        </tr>

        <tr>
            <td class="Label">Empresa</td>
            <td>
                <asp:literal id="lblInformacoesComerciais_Empresa" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Profissão</td>
            <td>
                <asp:literal id="lblInformacoesComerciais_Profissao" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Cargo Atual ou Função</td>
            <td>
                <asp:literal id="lblInformacoesComerciais_CargoAtualOuFuncao" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Email</td>
            <td>
                <asp:literal id="lblInformacoesComerciais_Email" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Dados para Contato</th>
        </tr>

        <tr>
            <td class="Label">Endereços</td>
            <td>
                <asp:literal id="lblDadosParaContato_Enderecos" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Telefones</td>
            <td>
                <asp:literal id="lblDadosParaContato_Telefones" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Contas Gradual</th>
        </tr>

        <tr>
            <td class="Label">Contas</td>
            <td>
                <asp:literal id="lblDados_Contas" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Dados Bancários</th>
        </tr>

        <tr>
            <td class="Label">Contas</td>
            <td>
                <asp:literal id="lblDadosBancarios_Contas" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Procurador / Diretor / Controlador</th>
        </tr>

        <tr>
            <td class="Label">Procuradores</td>
            <td>
                <asp:literal id="lblProcuradores_Nome" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Diretores</td>
            <td>
                <asp:literal id="lblDiretores_Nome" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Controladores</td>
            <td>
                <asp:literal id="lblControladores_Nome" runat="server"></asp:literal>
            </td>
        </tr>

        <%--<tr>
            <td class="Label">Sexo</td>
            <td>
                <asp:literal id="lblRepresentanteLegal_Sexo" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Telefone</td>
            <td>
                <asp:literal id="lblRepresentanteLegal_Telefone" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Celular</td>
            <td>
                <asp:literal id="lblRepresentanteLegal_Celular" runat="server"></asp:literal>
            </td>
        </tr>--%>
        
        <tr>
            <th colspan="2">Informações Patrimoniais</th>
        </tr>

        <tr>
            <th colspan="2">Rendimentos</th>
        </tr>

        <tr>
            <td class="Label">Salário/Pró-labore</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_Salario" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Outros Rendimentos</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_OutrosRendimentos" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <td class="Label">Total de Outros Rendimentos</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_TotalDeOutrosRendimentos" runat="server"></asp:literal>
            </td>
        </tr>
        
        <tr>
            <th colspan="2">Bens Imóveis</th>
        </tr>

        <tr>
            <td class="Label">Bens Imóveis</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_BensImoveis" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Bens Móveis</th>
        </tr>

        <tr>
            <td class="Label">Bens Móveis</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_BensMoveis" runat="server"></asp:literal>
            </td>
        </tr>
        
        <tr>
            <th colspan="2">Aplicações</th>
        </tr>

        <tr>
            <td class="Label">Aplicações</td>
            <td>
                <asp:literal id="lblInformacoesPatrimoniais_Aplicacoes" runat="server"></asp:literal>
            </td>
        </tr>

        <tr>
            <th colspan="2">Declarações e Autorizações</th>
        </tr>

        <tr>
            <td colspan="2">
                <asp:literal id="lblInformacoesPatrimoniais_DeclaracoesEAutorizacoes" runat="server"></asp:literal>
            </td>
        </tr>

    </table>

    </div>

   
    <h4><br/>OBS. A conclusão do Cadastro é realizada na primeira vez que a ficha cadastral é gerada. Neste momento, as regras de cadastro são validadas, por exemplo, é verificado se o cliente possui Conta Bancária, Telefone, Endereço.<br/>
    Apenas na primeira geração da Ficha, o cliente recebe por email a Ficha Cadastral, o Contrato e a Solicitação para enviar estes documantos devidamente assinados e autenticados para a Gradual, para que o Departamento de Cadastro exporte o cliente para o Sinacor e o mesmo possa iniciar suas operações.   
    </h4>
    

    <p class="BotoesSubmit" style="padding: 10px;">

        <button id="btn_imprimir_ficha" onclick="btn_imprimir_ficha_click();return false;">Imprimir Ficha Cadastral</button>
<%
    if (this.lblDadosBasicos_DesejaAplicar.Text.ToUpper().Contains("CAM") || this.lblDadosBasicos_DesejaAplicar.Text.ToUpper().Equals("TODOS") || this.lblDadosBasicos_DesejaAplicar.Text.ToUpper().Equals("TODAS"))
    {
%>        
        <button id="btn_imprimir_ficha_cambio" onclick="btn_imprimir_ficha_cambio_click();return false;">Imprimir Ficha Câmbio</button>
<%
    }
%>   
        <div id="pnlBotoes" runat="server" style="text-align: center">
          <button id="btn_imprimir_contratoPF" onclick="return btn_imprimir_contratoPF_click();">Visualizar Contrato PF</button>

          <button id="btn_imprimir_contratoPJ" onclick="return btn_imprimir_contratoPJ_click();">Visualizar Contrato PJ</button>
        </div>
    </p>

</form>