<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="SolicitarAlteracao.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.SolicitarAlteracao" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h2>Solicitar Alteração</h2>

    <div class="form-padrao">
        <div class="row">
            <div class="col3-2">
                <div class="row">
                    <div class="col2">
                        <div class="campo-consulta">
                            <label>Tipo:</label>
                            <select id="cboCadastro_SolicitarAlteracao_Tipo" type="select-one" runat="server" onchange="return cboCadastro_SolicitarAlteracao_Tipo_OnChange(this);" class="validate[required]" style="width: 280px;">
                                <option value="" selected="selected">Selecione</option>
                                <option value="A">Alteração</option>
                                <option value="I">Inclusão</option>
                                <option value="E">Exclusão</option>
                            </select>
                        </div>
                    </div>

                    <div class="col2">
                        <div class="campo-consulta">
                            <label>Informação a ser alterada:</label>

                            <select id="cboCadastro_SolicitarAlteracao_InformacaoAlterada" type="select-one" onchange="cboCadastro_SolicitarAlteracao_Informacao_OnChange(this)" runat="server" class="validate[required]" style="width: 280px;">
                                <option value="" selected="selected">Selecione</option>

                                <option data-rel="A" value="Conta Bancária" style="display:none">Conta Bancária</option>
                                <option data-rel="A" value="Adesão ao Contrato de Intermediação" style="display:none">Adesão ao Contrato de Intermediação e Custódia</option>
                                <option data-rel="A" value="Dados Cadastrais" style="display:none">Dados Cadastrais</option>
                                <option data-rel="A" value="Dados do Representante Legal/Procurador" style="display:none">Dados do Representante Legal/Procurador</option>
                                <option data-rel="A" value="Documento" style="display:none">Documento</option>
                                <option data-rel="A" value="Endereço" style="display:none">Endereço</option>
                                <option data-rel="A" value="Telefone" style="display:none">Telefone</option>
                                <option data-rel="A" value="Conta Bancária" style="display:none">Conta Bancária</option>
                
                                <option data-rel="E" value="Conta Bancária" style="display:none">Conta Bancária</option>
                                <option data-rel="E" value="Telefone" style="display:none">Telefone</option>

                                <option data-rel="I" value="Conta Bancária" style="display:none">Conta Bancária</option>
                                <option data-rel="I" value="Endereço" style="display:none">Endereço</option>
                                <option data-rel="I" value="Telefone" style="display:none">Telefone</option>

                            </select>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col3-2">
                        <div class="campo-consulta">
                            <label>Descrição / Motivo:</label>
                            <textarea id="txtCadastro_SolicitarAlteracao_DescricaoMotivo" runat="server"></textarea>
                        </div>

                        <p class="red">Todos os campos são obrigatórios</p>
                    </div>
                </div>
                
                <div id="pnlAlertaAlteracaoContrato" class="row" style="display:none">
                    <div class="col3-2">
                        <p>
                            Leia atentamente o contrato de Intermediação e Custódia e em seguida preencha e assine o 
                            termo de adesão ao contrato de Intermediação e Custódia. Para acessar este documento
                             <a href="/Resc/PDFs/Contrato_de_Intermediacao_1501.pdf" target="_blank"> clique aqui.</a>
                         </p>
                        <p>
                            Envie a documentação por correio para endereço abaixo ou se, preferir, 
                            digitalize-a e envie para: <a href="mailto:cadastrodigital@gradualinvestimentos.com.br.".>cadastrodigital@gradualinvestimentos.com.br.</a>
                        </p>
                        <p>
                            Gradual CCTVM S.A.<br />
                            A/C Cadastro<br />
                            Avenida Juscelino Kubitschek, 50 - 5º e 6º andares<br />
                            Vila Nova Conceição – São Paulo - SP – CEP: 04543-011<br />
                        </p>

                    </div>
                <//div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col6">
            <asp:Button runat="server" ID="btnCadastro_SolicitarAlteracao_GravarDados" Text="Enviar" onclick="btnCadastro_SolicitarAlteracao_GravarDados_Click" onclientclick="return btnCadastro_SolicitarAlteracao_GravarDados_Validar(this);" cssclass="botao btn-padrao btn-erica" />
        </div>
    </div>

</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Solicitar Alteração" />

</asp:Content>
