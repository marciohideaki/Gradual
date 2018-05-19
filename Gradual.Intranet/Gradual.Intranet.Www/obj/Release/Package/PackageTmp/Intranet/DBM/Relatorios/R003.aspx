<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R003.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R003" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao" style="float:left;height:auto;">

    <h1>Relatório de <span>Resumo do Cliente</span></h1>

    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>
    
    <div id="divNenhumClienteEncontrado" runat="server" style="float: left;">
        <h3 style="padding-left: 15px; padding-top: 10px;">Nenhum Cliente Encontrado.</h3>
    </div>

    <div id="divDadosCadastrais" runat="server" style="float: left;">

    <h3 style="margin: 12px;">Dados cadastrais</h3>
    
    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_NomeDoCliente" class="CamposRelatorioDBMResumoDoCliente">Nome do cliente:</label>
        <label  id="txtDBM_ResumoDoCliente_NomeDoCliente" class="CamposRelatorioDBMResumoDoCliente" style="width: 60%; text-align: left;"><%= NomeCliente %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Tipo" class="CamposRelatorioDBMResumoDoCliente">Tipo:</label>
        <label  id="txtDBM_ResumoDoCliente_Tipo" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Tipo %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_DataDeCadastro" class="CamposRelatorioDBMResumoDoCliente">Data de cadastro:</label>
        <label  id="txtDBM_ResumoDoCliente_DataDeCadastro" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= DataDeCadastro %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_DataDaUltimaOperacao" class="CamposRelatorioDBMResumoDoCliente">Data da última operação:</label>
        <label  id="txtDBM_ResumoDoCliente_DataDaUltimaOperacao" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= DataUltimaOperacao %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Email" class="CamposRelatorioDBMResumoDoCliente">e-Mail:</label>
        <label  id="txtDBM_ResumoDoCliente_Email" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Email %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Logradouro" class="CamposRelatorioDBMResumoDoCliente">Logradouro:</label>
        <label  id="txtDBM_ResumoDoCliente_Logradouro" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Logradouro %></label>
    </p>

    <p style="width: 30em; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Numero" class="CamposRelatorioDBMResumoDoCliente">Número:</label>
        <label  id="txtDBM_ResumoDoCliente_Numero" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Numero %></label>
    </p>

    <p style="width: 70%; float:left; margin-top: 5px; margin-left: -5em;">
        <label for="txtDBM_ResumoDoCliente_Complemento" class="CamposRelatorioDBMResumoDoCliente">Complemento:</label>
        <label  id="txtDBM_ResumoDoCliente_Complemento" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Complemento %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Bairro" class="CamposRelatorioDBMResumoDoCliente">Bairro:</label>
        <label  id="txtDBM_ResumoDoCliente_Bairro" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Bairro %></label>
    </p>

    <p style="width: 30em; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Cidade" class="CamposRelatorioDBMResumoDoCliente">Cidade:</label>
        <label  id="txtDBM_ResumoDoCliente_Cidade" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Cidade %></label>
    </p>

    <p style="width: 70%; float:left; margin-top: 5px; margin-left: -5em;">
        <label for="txtDBM_ResumoDoCliente_UF" class="CamposRelatorioDBMResumoDoCliente">Estado:</label>
        <label  id="txtDBM_ResumoDoCliente_UF" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Estado %></label>
    </p>

    <p style="width: 30em; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Telefone" class="CamposRelatorioDBMResumoDoCliente">Telefone:</label>
        <label  id="txtDBM_ResumoDoCliente_Telefone" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Telefone %></label>
    </p>

    <p style="width: 70%; float: left; margin-top: 5px; margin-left: -5em;">
        <label for="txtDBM_ResumoDoCliente_Ramal" class="CamposRelatorioDBMResumoDoCliente">Ramal:</label>
        <label  id="txtDBM_ResumoDoCliente_Ramal" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Ramal %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Celular1" class="CamposRelatorioDBMResumoDoCliente">Celular 1:</label>
        <label  id="txtDBM_ResumoDoCliente_Celular1" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Celular1 %></label>
    </p>

    <p style="width: 100%; float:left; margin-top: 5px;">
        <label for="txtDBM_ResumoDoCliente_Celular2" class="CamposRelatorioDBMResumoDoCliente">Celular 2:</label>
        <label  id="txtDBM_ResumoDoCliente_Celular2" class="txtDBM_ResumoDoCliente_Tipo" style="width: 60%; text-align: left;"><%= Celular2 %></label>
    </p>

    <p class="CamposRelatorioDBMDadosCadastrais" style="padding-top: 15px; width: 100%;">
        <strong style="margin-left: 12em;>
            <label class="CamposRelatorioDBMResumoDoCliente"></label>
            <label id="txtDBM_ResumoDoCliente_Corretagem">Corretagem (R$)</label>
            <label id="txtDBM_ResumoDoCliente_Volume">Volume (R$)</label>
        </strong>
    </p>

    <p class="CamposRelatorioDBMDadosCadastrais" style="width: 100%">
        <label class="CamposRelatorioDBMResumoDoCliente">No mês:</label>
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_NoMes_Corretagem" propriedade="CorretagemNoMes" value="<%= CorretagemNoMes %>" class="CamposRelatorioDBMResumoDoClienteCorretagem" />
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_NoMes_Volume" propriedade="VolumeNoMes" value="<%= VolumeNoMes %>" style="text-align: right;" />
    </p>

    <p class="CamposRelatorioDBMDadosCadastrais" style="width: 100%">
        <label class="CamposRelatorioDBMResumoDoCliente">Média no ano:</label>
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_MediaNoAno_Corretagem" propriedade="CorretagemMediaNoAno" value="<%= CorretagemMediaNoAno %>" class="CamposRelatorioDBMResumoDoClienteCorretagem" />
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_MediaNoAno_Volume" propriedade="VolumeMediaNoAno" value="<%= VolumeMediaNoAno %>" style="text-align: right;" />
    </p>

    <p class="CamposRelatorioDBMDadosCadastrais" style="width: 100%">
        <label class="CamposRelatorioDBMResumoDoCliente">Em 12 meses:</label>
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_Em12Meses_Corretagem" propriedade="CorretagemEm12Meses" value="<%= CorretagemEm12Meses %>" class="CamposRelatorioDBMResumoDoClienteCorretagem" />
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_Corretagem_Em12Meses_Volume"  propriedade="VolumeEm12Meses" value="<%= VolumeEm12Meses %>" style="text-align: right;" />
    </p>

    <h3 style="float: left; margin: 12px; width: 90%">Conta Corrente</h3>

    <p class="CamposRelatorioDBMDadosCadastrais" style="width: 100%">
        <label class="CamposRelatorioDBMResumoDoCliente">Disponível (R$):</label>
        <input type="text" disabled="disabled" id="txtDBM_ResumoDoCliente_ContaCorrente_Disponivel" propriedade="CorretagemEm12Meses" value="<%= ContaCorrenteDisponivel %>" class="CamposRelatorioDBMResumoDoClienteCorretagem" />
    </p>

    <h3 style="float: left; margin: 12px; width: 90%">Posição em carteira</h3>

    <div runat="server" id="divDBM_ResumoDoCliente_Carteira" visible="false" style="float: left;">
        <table class="GridRelatorio" style="float: left; height: 12px; border: 1px solid black; width: 51em;">
            <thead>
                <tr>
                    <td style="text-align: left;"> Carteira</td>
                    <td style="text-align: center;">R$</td>
                    <td style="text-align: center;">Quantidade</td>
                </tr>
            </thead>
            <tbody>
                <asp:repeater runat="server" id="rptDBM_ResumoDoCliente_Carteira">
                    <itemtemplate>
                        <tr>
                            <td style="text-align: left;"> <%# Eval("Carteira")%></td>
                            <td style="text-align: right;"><%# Eval("Valor")%></td>
                            <td style="text-align: right;"><%# Eval("Quantidade")%></td>
                        </tr>
                    </itemtemplate>
                </asp:repeater>
            </tbody>
        </table>
    </div>

    <span runat="server" id="divDBM_ResumoDoCliente_Carteira_Nenhum" visible="false" style="float: left; margin-left: 4em;">Nenhum item encontrado</span>

    </div>

    <div style="margin-bottom: 65em;" style="float: left;">
    
    </div>

</div>
</form>