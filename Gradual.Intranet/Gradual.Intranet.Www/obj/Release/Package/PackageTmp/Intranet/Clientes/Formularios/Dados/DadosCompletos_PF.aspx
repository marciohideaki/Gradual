<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_PF.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.DadosCompletos_PF" %>


<form id="form1" runat="server">

    <input type="hidden" id="hddIdAssessorLogado" value="<%= glIdAsessorLogado %>" />

    <h4>Dados do Cliente - PF</h4>

    <div class="PainelScroll">
    
    <input type="hidden" id="txtClientes_Id" Propriedade="Id" />

    <input type="hidden" id="txtClientes_IdLogin" Propriedade="IdLogin" />

    <input type="hidden" id="txtClientes_Passo" Propriedade="Passo" />

    <input type="hidden" id="hidDadosCompletos_PF" class="DadosJson" runat="server" value="" />

    <input type="hidden" id="txtClientes_DadosCompletos_TipoCliente" Propriedade="TipoCliente" value="PF" />

    <p>
        <label for="cboClientes_DadosCompletos_Tipo">Tipo:</label>
        <select id="cboClientes_DadosCompletos_Tipo" Propriedade="Tipo" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_Tipo" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>
        </select>
    </p>
    <p>
        <label for="txtClientes_DadosCompletos_NomeCliente">Nome Completo:</label>
        <input type="text" id="txtClientes_DadosCompletos_NomeCliente" Propriedade="NomeCliente" maxlength="60" class="validate[required,length[5,60]]" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_CPF">CPF:</label>
        <input type="text" id="txtClientes_DadosCompletos_CPF" Propriedade="CPF_CNPJ" maxlength="15" class="validate[funcCall[validatecpf]] ProibirLetras" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Email">Email:</label>
        <input type="text" id="txtClientes_DadosCompletos_Email" Propriedade="Email" maxlength="80" class="validate[required,custom[Email]]" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_Assessor">Assessor:</label>
        <select id="cboClientes_DadosCompletos_Assessor" Propriedade="Assessor" class="validate[required]" onchange="GradIntra_Cadastro_Assessores_Gerais_Load(this)">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_Assessor" runat="server">
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_Nacionalidade">Nacionalidade:</label>
        <select id="cboClientes_DadosCompletos_Nacionalidade" Propriedade="Nacionalidade" class="validate[required]" onchange="return GradIntra_Cadastro_InabilitarEstadoQuandoEstrangeiro($('#cboClientes_DadosCompletos_Nacionalidade'), $('#cboClientes_DadosCompletos_EstadoDeNascimento'))">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_Nacionalidade" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Documento_DataNascimento">Data de Nascimento:</label>
        <input type="text" id="txtClientes_DadosCompletos_Documento_DataNascimento" Propriedade="DataNascimento" maxlength="10" class="validate[custom[data]] Mascara_Data" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_Sexo">Sexo:</label>
        <select id="cboClientes_DadosCompletos_Sexo" Propriedade="Sexo" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <option value="F">Feminino</option>
            <option value="M">Masculino</option>
        </select>
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_PaisDeNascimento">País de Nasc.:</label>
        <select id="cboClientes_DadosCompletos_PaisDeNascimento" Propriedade="PaisDeNascimento" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_PaisDeNascimento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>     
        </select>
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_EstadoDeNascimento">Estado de Nasc.:</label>
        <select id="cboClientes_DadosCompletos_EstadoDeNascimento" Propriedade="EstadoDeNascimento" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_EstadoDeNascimento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
            <option value="99">&lt; Fora do Brasil &gt;</option>
        </select>
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_CidadeDeNascimento">Cidade de Nasc.:</label>
        <input type="text" id="txtClientes_DadosCompletos_CidadeDeNascimento" Propriedade="CidadeDeNascimento" maxlength="20" class="validate[required,length[0,20]]" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_EstadoCivil">Estado Civil:</label>
        <select id="cboClientes_DadosCompletos_EstadoCivil" Propriedade="EstadoCivil" class="validate[required]" onchange="return GradIntra_Cadastro_EstadoCivil($(this), $(txtClientes_DadosCompletos_Conjuge))">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_EstadoCivil" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Conjuge">Cônjuge:</label>
        <input type="text" id="txtClientes_DadosCompletos_Conjuge" Propriedade="Conjuge" maxlength="60" class="validate[length[5,60]]" />
    </p>

    <p>
        <label id="lblClientes_DadosCompletos_Profissao" for="cboClientes_DadosCompletos_Profissao">Profissão:</label>
        <select id="cboClientes_DadosCompletos_Profissao" Propriedade="Profissao" class="validate[required]" onchange="return GradIntra_Cadastro_InabilitarDadosComerciaisPF($(this), $('#txtClientes_DadosCompletos_CargoAtual'), $('#txtClientes_DadosCompletos_Empresa'), $('#txtClientes_DadosCompletos_EmailComercial'));">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_Profissao" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>
    
    <p>
        <label id="lblClientes_DadosCompletos_CargoAtual" for="txtClientes_DadosCompletos_CargoAtual">Cargo Atual:</label>
        <input type="text" id="txtClientes_DadosCompletos_CargoAtual" Propriedade="CargoAtual" maxlength="40" class="validate[required,length[5,40]]" />
    </p>

    <p>
        <label id="lblClientes_DadosCompletos_Empresa" for="txtClientes_DadosCompletos_Empresa">Empresa:</label>
        <input type="text" id="txtClientes_DadosCompletos_Empresa" Propriedade="Empresa" maxlength="60" class="validate[required,length[5,60]]" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_NomeDaMae">Nome da Mãe:</label>
        <input type="text" id="txtClientes_DadosCompletos_NomeDaMae" Propriedade="NomeDaMae" maxlength="60" class="validate[required,length[3,60]]" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_NomeDoPai">Nome do Pai:</label>
        <input type="text" id="txtClientes_DadosCompletos_NomeDoPai" Propriedade="NomeDoPai" maxlength="60" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_TipoDeDocumento">Tipo de Documento:</label>
        <select id="cboClientes_DadosCompletos_TipoDeDocumento" Propriedade="TipoDeDocumento" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_TipoDeDocumento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_OrgaoEmissor">Órgão Emissor:</label>
        <select id="cboClientes_DadosCompletos_OrgaoEmissor" Propriedade="OrgaoEmissor" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_OrgaoEmissor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Documento_Numero">Número do Doc.:</label>
        <input type="text" id="txtClientes_DadosCompletos_Documento_Numero" Propriedade="Documento_Numero" maxlength="16" class="validate[required,length[5,16]]" />
    </p>

    <p style="display:none;">
        <label for="txtClientes_DadosCompletos_Documento_DataValidade">Data de Validade.:</label>
        <input type="text" id="txtClientes_DadosCompletos_Documento_DataValidade" Propriedade="Documento_DataValidade" maxlength="10" class="validate[custom[data]] Mascara_Data" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Documento_DataEmissao">Data de Emissão:</label>
        <input type="text" id="txtClientes_DadosCompletos_Documento_DataEmissao" Propriedade="Documento_DataEmissao" maxlength="10" class="validate[custom[data],funcCall[CompararDatas_nascimento_expedicao,txtClientes_DadosCompletos_Documento_DataNascimento]] Mascara_Data" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_Documento_EstadoEmissao">Estado de Emissão:</label>
        <select id="cboClientes_DadosCompletos_Documento_EstadoEmissao" Propriedade="Documento_EstadoEmissao" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_EstadoEmissao" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>
    
    <p>
        <label for="txtClientes_DadosCompletos_PropositoGradual">Propósito com a Gradual:</label>
        <input type="text" id="txtClientes_DadosCompletos_PropositoGradual" Propriedade="PropositoGradual" maxlength="40" class="validate[required]" />
    </p>

    <div style="float:left; magin:10px;">

        <p class="RadioSimNao">
            <label class="Primeiro">Opera por Conta Própria:</label>
            <input type="radio" id="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim" onclick="GradIntra_Cadastro_DadosCompletos_OperaPorContaPropria_Click(this);" name="rdoCadastro_DadosCompletos_OperaPorContaPropria" value="Sim" Propriedade="Flag_OperaPorContaPropria" checked="checked" />
            <label for="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim">Sim</label>
        
            <input type="radio" id="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao" onclick="GradIntra_Cadastro_DadosCompletos_OperaPorContaPropria_Click(this);" name="rdoCadastro_DadosCompletos_OperaPorContaPropria" value="Nao" />
            <label for="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao">Não</label>
        </p>

        <p class="RadioSimNao">
            <label class="Primeiro">Emancipado:</label>
            <input type="radio" id="rdoCadastro_DadosCompletos_Emancipado_Sim" Propriedade="Flag_Emancipado" name="rdoCadastro_DadosCompletos_Emancipado" value="Sim" />
            <label for="rdoCadastro_DadosCompletos_Emancipado_Sim">Sim</label>
        
            <input type="radio" id="rdoCadastro_DadosCompletos_Emancipado_Nao"  name="rdoCadastro_DadosCompletos_Emancipado" checked="checked" value="Nao" />
            <label for="rdoCadastro_DadosCompletos_Emancipado_Nao">Não</label>
        </p>

        <!--
    <p class="RadioSimNao">
        <label class="Primeiro">Possui Representante:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_PossuiRepresentante_Sim" Propriedade="Flag_PossuiRepresentante" name="rdoCadastro_DadosCompletos_PossuiRepresentante" checked="checked" value="Sim" />
        <label for="rdoCadastro_DadosCompletos_PossuiRepresentante_Sim">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_PossuiRepresentante_Nao" name="rdoCadastro_DadosCompletos_PossuiRepresentante" value="Nao" />
        <label for="rdoCadastro_DadosCompletos_PossuiRepresentante_Nao">Não</label>
    </p>
    -->
    
        <p class="RadioSimNao">
            <label class="Primeiro">PPE:</label>
            <input type="radio" id="rdoCadastro_DadosCompletos_PPE_Sim" Propriedade="Flag_PPE" name="rdoCadastro_DadosCompletos_PPE" value="Sim" />
            <label for="rdoCadastro_DadosCompletos_PPE_Sim">Sim</label>
        
            <input type="radio" id="rdoCadastro_DadosCompletos_PPE_Nao" name="rdoCadastro_DadosCompletos_PPE" checked="checked" value="Nao" />
            <label for="rdoCadastro_DadosCompletos_PPE_Nao">Não</label>
   
        </p>

        <p class="RadioSimNao">
            <label class="Primeiro">CVM 387:</label>
            <input type="radio" id="rdoCadastro_DadosCompletos_CVM387_Sim" Propriedade="Flag_CVM387" name="rdoCadastro_DadosCompletos_CVM387" checked="checked" value="Sim"/>
            <label for="rdoCadastro_DadosCompletos_CVM387_Sim">Sim</label>

            <input type="radio" id="rdoCadastro_DadosCompletos_CVM387_Nao" name="rdoCadastro_DadosCompletos_CVM387" value="Nao" />
            <label for="rdoCadastro_DadosCompletos_CVM387_Nao">Não</label>
        </p>

        <p class="RadioSimNao">
            <label class="Primeiro">US Person:</label>
            <input type="radio" id="rdoCadastro_DadosCompletos_USPerson_Sim" Propriedade="USPerson" name="rdoCadastro_DadosCompletos_USPerson" checked="checked" value="Sim"/>
            <label for="rdoCadastro_DadosCompletos_USPerson_Sim">Sim</label>
        
            <input type="radio" id="rdoCadastro_DadosCompletos_USPerson_Nao" name="rdoCadastro_DadosCompletos_USPerson" value="Nao" />
            <label for="rdoCadastro_DadosCompletos_USPerson_Nao">Não</label>
        </p>

        <p class="RadioSimNao">
            <label class="Primeiro">Pessoa Vinculada:</label>
        
            <input type="radio" id="rdoCadastro_DadosCompletos_PessoaVinculada_Nao" name="rdoCadastro_DadosCompletos_PessoaVinculada" RadioMultiplo="true"  value="0" checked="checked" />
            <label for="rdoCadastro_DadosCompletos_PessoaVinculada_Nao">Não</label>
            
            <input type="radio" id="rdoCadastro_DadosCompletos_PessoaVinculada_Sim" name="rdoCadastro_DadosCompletos_PessoaVinculada" Propriedade="Flag_PessoaVinculada" RadioMultiplo="true"  value="1" />
            <label for="rdoCadastro_DadosCompletos_PessoaVinculada_Sim">Sim, a outra corretora</label>
            
            <input type="radio" id="rdoCadastro_DadosCompletos_PessoaVinculada_SimG" name="rdoCadastro_DadosCompletos_PessoaVinculada" RadioMultiplo="true" value="2" />
            <label for="rdoCadastro_DadosCompletos_PessoaVinculada_SimG">Sim, à Gradual</label>
        </p>

        <p class="RadioSimNao">
            <label class="Primeiro">Vai aplicar em:</label>
            <!-- //TODO: Alterar a nomenclatura dos campos para que se adeque ao identificador de checkbox-->
            <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" checked="checked" value="BOVESPA" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PF_Change(this);"/>
            <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa">Bovespa / BM&amp;F</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" value="FUNDOS" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PF_Change(this);"/>
            <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos">Fundos</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" value="CAMBIO" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PF_Change(this);"/>
            <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio">Câmbio</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" value="AMBOS" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PF_Change(this);"/>
            <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos">Todas</label>
        </p>

        <p class="RadioSimNao" style="padding-bottom:3em"> <!-- padding só pro scroll poder descer um pouco mais -->
            <label class="Primeiro">Ciências de documentos:</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_Ciencia_Regulamento" name="rdoCadastro_DadosCompletos_Ciencia_Regulamento" Propriedade="CienteRegulamento"  />
            <label for="rdoCadastro_DadosCompletos_Ciencia_Regulamento">Regulamento</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_Ciencia_Prospecto" name="rdoCadastro_DadosCompletos_Ciencia_Prospecto" Propriedade="CienteProspecto" />
            <label for="rdoCadastro_DadosCompletos_Ciencia_Prospecto">Prospecto</label>

            <input type="checkbox" id="rdoCadastro_DadosCompletos_Ciencia_Lamina" name="rdoCadastro_DadosCompletos_Ciencia_Lamina" Propriedade="CienteLamina"  />
            <label for="rdoCadastro_DadosCompletos_Ciencia_Lamina">Lâmina</label>
        </p>

        <!--
    <p class="RadioSimNao">
        <label class="Primeiro">Autoriza a Transmissão de Ordens por Procurador ou Representante:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim" Propriedade="Flag_AutorizaTransmissaoPorProcurador" name="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador" checked="checked" value="Sim" />
        <label for="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao" checked="checked" name="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador" value="Nao" />
        <label for="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao">Não</label>
    </p>
    -->

    </div>

    <div id="divNaoOperaPorContaPropria" class="Cliente_GradIntra_DadosCompletos_PF_NaoOperaPorContaPropria" style="display:none">
        <fieldset style="height: 94px !important; width: 314px !important; ">
            <legend>Opera por conta de quem? (Dados do cliente)</legend>
            <p>
                <label for="txtClientes_DadosCompletos_NaoOperaPorContaPropria_Nome">Nome:</label>
                <input type="text" id="txtClientes_DadosCompletos_NaoOperaPorContaPropria_Nome" Propriedade="NaoOperaPorContaPropriaNome" maxlength="60" />
                <label for="txtClientes_DadosCompletos_NaoOperaPorContaPropria_CPF">CPF/CNPJ:</label>
                <input type="text" id="txtClientes_DadosCompletos_NaoOperaPorContaPropria_CPF" Propriedade="NaoOperaPorContaPropriaCPF_CNPJ" maxlength="15" class="ProibirLetras" />
            </p>
        </fieldset>
    </div>

    </div>

    <p class="BotoesSubmit">
        
        <button id="btnSalvar" runat="server" onclick="return btnSalvar_Clientes_DadosCompletos_PF_Click(this)">Salvar Alterações</button>
        
    </p>

    <script type="text/javascript">
        $.ready(GradIntra_Clientes_NovoClientePF_Load());
        $.ready(GradIntra_Cadastro_RemoverMultiplosRadiosDaTela());
        $.ready(GradIntra_Cadastro_Assessores_Gerais_Load());
    </script>

</form>
