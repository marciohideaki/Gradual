<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="DadosCompletos_PJ.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.DadosCompletos_PJ" %>

<form id="frmDadosCompletos_PJ" runat="server">

    <input type="hidden" id="hddIdAssessorLogado" value="<%= glIdAsessorLogado %>" />
    
    <h4>Dados do Cliente - PJ <% if(this.ELocal) { %> <a href="#" onclick="return Cadastro_PJ_PreencherTeste(this)">preencher</a> <% } %></h4>
    
    <div class="PainelScroll">
    
    <input type="hidden" id="hidDadosCompletos_PJ" class="DadosJson" runat="server" value="" />
    
    <input type="hidden" id="txtClientes_DadosCompletos_TipoCliente" Propriedade="TipoCliente" value="PJ" />

    <input type="hidden" id="txtClientes_DadosCompletos_Id" Propriedade="Id" />

    <input type="hidden" id="txtClientes_DadosCompletos_IdLogin" Propriedade="IdLogin" />

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
        <label for="cboClientes_DadosCompletos_PaisDeNascimento">País:</label>
        <select id="cboClientes_DadosCompletos_PaisDeNascimento" Propriedade="Pais" class="validate[required]" onchange="VerificaValidacaoCNPJPorPais();">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_PaisDeNascimento" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>     
        </select>
    </p>

    <p class="LabelEmLinhaPropria">
        <label for="cboClientes_DadosCompletos_ObjetivoSocial">Objetivo Social da Empresa (Tipo de Investidor):</label>
        <select id="cboClientes_DadosCompletos_ObjetivoSocial" class="validate[required]" Propriedade="RamoAtividade">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_ObjetivoSocial" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>
        </select>

    </p>

    <p>
        <label for="txtClientes_DadosCompletos_RazaoSocial">Denomina&ccedil;&atilde;o Social:</label>
        <input type="text" id="txtClientes_DadosCompletos_RazaoSocial" maxlength="60" Propriedade="NomeCliente" class="validate[required,length[5,60]]" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_NomeFantasia">Nome Fantasia:</label>
        <input type="text" id="txtClientes_DadosCompletos_NomeFantasia" maxlength="100" Propriedade="NomeFantasia" class="validate[required,length[5,100]]" />
    </p>

    <p>
        <label for="cboClientes_DadosCompletos_Assessor">Assessor:</label>
        <select id="cboClientes_DadosCompletos_Assessor" Propriedade="Assessor" class="validate[required]" onchange="GradIntra_Cadastro_Assessores_Gerais_Load(this)">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_Assessor" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>        
        </select>
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_Email">Email:</label>
        <input type="text" id="txtClientes_DadosCompletos_Email" maxlength="80" Propriedade="Email" class="validate[required,custom[Email]]" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_CNPJ">CNPJ:</label>
        <input type="text" id="txtClientes_DadosCompletos_CNPJ" maxlength="15" Propriedade="CPF_CNPJ" class="validate[required,funcCall[validatecpfcnpj]] " />
    </p>
    <p>
        <label for="txtClientes_DadosCompletos_DataDeConstituicao">Data de Constitui&ccedil;&atilde;o:</label>
        <input type="text" id="txtClientes_DadosCompletos_DataDeConstituicao" Propriedade="DataDeConstituicao" maxlength="10" class="validate[custom[data]] Mascara_Data" />
    </p>

    <p>
        <label for="txtClientes_DadosCompletos_NIRE">NIRE:</label>
        <input type="text" id="txtClientes_DadosCompletos_NIRE" maxlength="15" Propriedade="NIRE" class="validate[length[5,15],custom[onlyNumber]] ProibirLetras" />
    </p>
    
    <p>
        <label for="cboClientes_DadosCompletos_PrincipalAtividade">Principal Atividade:</label>
        <select id="cboClientes_DadosCompletos_PrincipalAtividade" Propriedade="PrincipalAtividade" class="validate[required]">
            <option value="">[ Selecione ]</option>
            <asp:Repeater id="rptClientes_DadosCompletos_PrincipalAtividade" runat='server'>
                <ItemTemplate>
                    <option value='<%# Eval("Id") %>'><%# Eval("Value") %></option>
                </ItemTemplate>
            </asp:Repeater>
            
        </select>
    </p>
    
    <p>
        <label for="txtClientes_DadosCompletos_FormaDeConstituicao">Constituição:</label>
        <input type="text" id="txtClientes_DadosCompletos_FormaDeConstituicao" Propriedade="FormaConstituicao" maxlength="15" class="validate[required,length[2,15]]" />
    </p>
    
    <p>
        <label for="txtClientes_DadosCompletos_FormaDeConstituicao">Propósito com a Gradual:</label>
        <input type="text" id="txtClientes_DadosCompletos_PropositoGradual" Propriedade="PropositoGradual" maxlength="40" class="validate[required,length[2,40]]" />
    </p>
    
    <p class="RadioSimNao">
        <label class="Primeiro">Declaro estar ciente:</label>
        <input type="checkbox" id="chkCadastro_DadosCompletos_CienteRegulamento<%= this.IncIDjs %>" Propriedade="CienteRegulamento" />
        <label for="chkCadastro_DadosCompletos_CienteRegulamento<%= this.IncIDjs %>">Regulamento</label>
        
        <input type="checkbox" id="chkCadastro_DadosCompletos_CienteProspecto<%= this.IncIDjs %>" Propriedade="CienteProspecto" />
        <label for="chkCadastro_DadosCompletos_CienteProspecto<%= this.IncIDjs %>">Prospecto</label>
        
        <input type="checkbox" id="chkCadastro_DadosCompletos_CienteLamina<%= this.IncIDjs %>" Propriedade="CienteLamina" />
        <label for="chkCadastro_DadosCompletos_CienteLamina<%= this.IncIDjs %>">Lâmina</label>
    </p>
    
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">Pessoa Vinculada:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_PessoaVinculada_Sim<%= this.IncIDjs %>" Propriedade="Flag_PessoaVinculada" name="rdoCadastro_DadosCompletos_PessoaVinculada<%= this.IncIDjs %>" />
        <label for="rdoCadastro_DadosCompletos_PessoaVinculada_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_PessoaVinculada_Nao<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_PessoaVinculada<%= this.IncIDjs %>" checked="checked" />
        <label for="rdoCadastro_DadosCompletos_PessoaVinculada_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">PPE:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_PPE_Sim<%= this.IncIDjs %>" Propriedade="Flag_PPE" name="rdoCadastro_DadosCompletos_PPE<%= this.IncIDjs %>" />
        <label for="rdoCadastro_DadosCompletos_PPE_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_PPE_Nao<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_PPE<%= this.IncIDjs %>" checked="checked" />
        <label for="rdoCadastro_DadosCompletos_PPE_Nao<%= this.IncIDjs %>">Não</label>
        
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">Opera por Conta Própria:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim<%= this.IncIDjs %>" Propriedade="Flag_OperaPorContaPropria" name="rdoCadastro_DadosCompletos_OperaPorContaPropria<%= this.IncIDjs %>" checked="checked" />
        <label for="rdoCadastro_DadosCompletos_OperaPorContaPropria_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_OperaPorContaPropria<%= this.IncIDjs %>" />
        <label for="rdoCadastro_DadosCompletos_OperaPorContaPropria_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">CVM 387:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_CVM387_Sim<%= this.IncIDjs %>" Propriedade="Flag_CVM387" name="rdoCadastro_DadosCompletos_CVM387<%= this.IncIDjs %>" checked="checked" />
        <label for="rdoCadastro_DadosCompletos_CVM387_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_CVM387_Nao<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_CVM387<%= this.IncIDjs %>" />
        <label for="rdoCadastro_DadosCompletos_CVM387_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">Autoriza a Transmissão de Ordens por Procurador ou Representante:</label>
        <input type="radio" id="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim<%= this.IncIDjs %>" Propriedade="Flag_AutorizaTransmissaoPorProcurador" name="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador<%= this.IncIDjs %>" checked="checked" />
        <label for="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao<%= this.IncIDjs %>" checked="checked" name="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador<%= this.IncIDjs %>" />
        <label for="rdoCadastro_DadosCompletos_AutorizaTransmissaoPorProcurador_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro" style="width:214px !important">Vai aplicar em:</label>
        <!-- //TODO: Alterar a nomenclatura dos campos para que se adeque ao identificador de checkbox-->
        <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_DesejaAplicar" checked="checked" value="BOVESPA" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PJ_Change(this);"/>
        <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Bovespa<%= this.IncIDjs %>">Bovespa / BM&amp;F</label>

        <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" value="FUNDOS" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PJ_Change(this);"/>
        <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Fundos<%= this.IncIDjs %>">Fundos</label>

        <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_DesejaAplicar" Propriedade="DesejaAplicar" value="CAMBIO" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PJ_Change(this);"/>
        <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Cambio<%= this.IncIDjs %>">Câmbio</label>

        <input type="checkbox" id="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_DesejaAplicar" value="AMBOS" onclick="return rdoCadastro_DadosCompletos_DesejaAplicar_PJ_Change(this);"/>
        <label for="rdoCadastro_DadosCompletos_DesejaAplicar_Ambos<%= this.IncIDjs %>">Todas</label>
    </p>

    <p class="RadioSimNao" style="margin: 15px 20px 25px 40px; text-align: right; padding: 0px 0px 10px 60px; width: 468px;">
        Com base na Regulamentação Tributária dos EUA sobre pessoa NÃO domiciliada ("NON US Person") ou pessoa domiciliada ("US Person"), 
        existem Administradores, Diretores ou Representantes Legais nas condições discriminadas a seguir:
    </p>
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">É cidadão dos EUA? (nacionalidade dupla ou única)</label>

        <input type="radio" id="rdoCadastro_DadosCompletos_USPersonNacional_Sim<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_USPersonNacional<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)" data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonNacional_Detalhes" Propriedade="Flag_USPersonNacional" />
        <label for="rdoCadastro_DadosCompletos_USPersonNacional_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_DadosCompletos_USPersonNacional_Nao<%= this.IncIDjs %>" name="rdoCadastro_DadosCompletos_USPersonNacional<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)" data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonNacional_Detalhes" checked="checked"  />
        <label for="rdoCadastro_DadosCompletos_USPersonNacional_Nao<%= this.IncIDjs %>">Não</label>
    </p>

    <div  id="pnlCadastro_DadosCompletos_USPersonNacional_Detalhes" style="display:none">
        <p>
            <label for="txtClientes_DadosCompletos_USPersonNacional_Nome">Nome Completo:</label>
            <input  id="txtClientes_DadosCompletos_USPersonNacional_Nome" type="text" maxlength="80" Propriedade="USPersonNacional_Nome" class="" />
        </p>
        <p>
            <label for="txtClientes_DadosCompletos_USPersonNacional_Nacionalidades">Relacione as nacionalidades:</label>
            <input  id="txtClientes_DadosCompletos_USPersonNacional_Nacionalidades" type="text"  maxlength="200" Propriedade="USPersonNacional_Nacionalidades" class="" />
        </p>
    </div>

    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">É residente permanente nos EUA? ("US resident alien")</label>

        <input type="radio" id="rdoCadastro_PFPasso3_USPersonResidente_Sim<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonResidente<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)"  data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonResidente_Detalhes" Propriedade="Flag_USPersonResidente" />
        <label for="rdoCadastro_PFPasso3_USPersonResidente_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_PFPasso3_USPersonResidente_Nao<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonResidente<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)" data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonResidente_Detalhes" checked="checked"  />
        <label for="rdoCadastro_PFPasso3_USPersonResidente_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <div  id="pnlCadastro_DadosCompletos_USPersonResidente_Detalhes" style="display:none">
        <p>
            <label for="txtClientes_DadosCompletos_USPersonResidente_Nome">Nome Completo:</label>
            <input  id="txtClientes_DadosCompletos_USPersonResidente_Nome" type="text" maxlength="80" Propriedade="USPersonResidente_Nome" class="" />
        </p>
    </div>

    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">É titular de Green Card?</label>

        <input type="radio" id="rdoCadastro_PFPasso3_USPersonGreen_Sim<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonGreen<%= this.IncIDjs %>" Propriedade="Flag_USPersonGreen" />
        <label for="rdoCadastro_PFPasso3_USPersonGreen_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_PFPasso3_USPersonGreen_Nao<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonGreen<%= this.IncIDjs %>" checked="checked"  />
        <label for="rdoCadastro_PFPasso3_USPersonGreen_Nao<%= this.IncIDjs %>">Não</label>
    </p>

    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">Atende requisitos da chamada "Persença Física Substancial"?</label>

        <input type="radio" id="rdoCadastro_PFPasso3_USPersonPresenca_Sim<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonPresenca<%= this.IncIDjs %>" Propriedade="Flag_USPersonPresenca" />
        <label for="rdoCadastro_PFPasso3_USPersonPresenca_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_PFPasso3_USPersonPresenca_Nao<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonPresenca<%= this.IncIDjs %>" checked="checked"  />
        <label for="rdoCadastro_PFPasso3_USPersonPresenca_Nao<%= this.IncIDjs %>">Não</label>
    </p>
    
    <p class="RadioSimNao RadioSimNaoLongo">
        <label class="Primeiro">Nasceu nos EUA?</label>

        <input type="radio" id="rdoCadastro_PFPasso3_USPersonNascido_Sim<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonNascido<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)" data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonRenuncia_Detalhes" Propriedade="Flag_USPersonNascido" />
        <label for="rdoCadastro_PFPasso3_USPersonNascido_Sim<%= this.IncIDjs %>">Sim</label>
        
        <input type="radio" id="rdoCadastro_PFPasso3_USPersonNascido_Nao<%= this.IncIDjs %>" name="rdoCadastro_PFPasso3_USPersonNascido<%= this.IncIDjs %>" onclick="rdoCadastro_PFPasso3_USPerson_Click(this)" data-PainelDoSim="pnlCadastro_DadosCompletos_USPersonRenuncia_Detalhes" checked="checked"  />
        <label for="rdoCadastro_PFPasso3_USPersonNascido_Nao<%= this.IncIDjs %>">Não</label>
    </p>

    <div  id="pnlCadastro_DadosCompletos_USPersonRenuncia_Detalhes" style="display:none">
        <p style="text-align: right; margin: 15px 20px 5px 40px; width: 400px; padding: 0px 0px 10px 130px;">
            Se nasceu nos EUA e julga NÃO ser um "US Person" indique o motivo e apresente documentação que comprove a renúncia
        </p>
        <p>
            <label for="txtClientes_DadosCompletos_USPersonRenuncia_Motivo">Motivo:</label>
            <input  id="txtClientes_DadosCompletos_USPersonRenuncia_Motivo" type="text" maxlength="80" Propriedade="USPersonRenuncia_Motivo" class="" />
        </p>
        <p>
            <label for="txtClientes_DadosCompletos_USPersonRenuncia_Documento">Tipo Documento em anexo:</label>
            <input  id="txtClientes_DadosCompletos_USPersonRenuncia_Documento" type="text"  maxlength="200" Propriedade="USPersonRenuncia_Documento" class="" />
        </p>
    </div>

    <p class="BotoesSubmit" style="float:left">
        <button id="btnSalvar" runat="server" onclick="return btnSalvar_Clientes_DadosCompletos_PJ_Click(this)">Salvar Alterações</button>
    </p>

    <script type="text/javascript">
        $.ready( function() { GradIntra_Cadastro_Assessores_Gerais_Load(); GradItra_Clientes_PJ_CarregarDetalhesUS(); } );
    </script>
</form>
