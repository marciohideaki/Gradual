<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="NovoCadastro.aspx.cs" Inherits="Gradual.Site.Www.NovoCadastro" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h2>Abra Sua Conta</h2>

    <div class="row">
        <div class="col2" style="width:73%">

            <p>
                Abra sua conta e aproveite todas as vantagens que só uma completa casa de investimentos oferece a você.
            </p>
            <p>
                Conheça já todos os benefícios de ser um Cliente Gradual.
            </p>
            <p>
                Bons negócios! 
            </p>

        </div>
        <!--div class="col4" style="float:right">
            <img src="../Resc/Skin/Default/Img/Carimbo_cadastro_novo.jpg" />
        </div-->
    </div>
 
    <div class="row">
        <div class="col1">

            <div id="ContentPlaceHolder1_pnlDadosBasicos" class="aba">

                <div class="menu-exportar clear">
                    <h3>Dados Básicos 
                    <% if (this.ModoDeTeste) { %>
                    <a href="#" onclick="return Cadastro_PreencherTeste_P1()" style="font-size:0.4em; font-weight:normal; color:Blue">preencher</a>
                    <% } %>
                    </h3>
                </div>

                <div class="form-padrao">

                    <div class="row">
                        <div class="col2">
                            <div class="campo-basico">
                                <label>Nome completo:</label>
                                <input  id="txtCadastro_PFPasso1_NomeCompleto" runat="server" type="text"  maxlength="60" class="input-padrao validate[required,length[5, 60]]" data-CaixaAlta="true" />
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col2">
                            <div class="campo-basico">
                                <label>E-mail:</label>
                                <input  id="txtCadastro_PFPasso1_Email" runat="server" type="text"   maxlength="80" class="input-padrao validate[required,custom[EmailGambizento]]" placeholder="login" />
                            </div>
                        </div>
                        
                        <div class="col2">
                            <div class="campo-basico">
                                <label>Confirmar E-mail:</label>
                                <input  id="txtCadastro_PFPasso1_EmailConfirmacao" runat="server" type="text" maxlength="80" class="input-padrao input-nopaste validate[required,custom[EmailGambizento],equals[ContentPlaceHolder1_txtCadastro_PFPasso1_Email]]" onkeydown="javascript:ValidarTamanhoCampo(this, event);"/>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col2">
                            <div class="campo-basico campo-senha">
                                <label>Senha:</label>
                                <button type="button" title="Preenchimento obrigatório. A senha deve ter 6 caracteres numéricos." class="interrogacao" tabindex="-1">?</button>

                                <input  id="txtCadastro_PFPasso1_Senha" runat="server" type="password"  maxlength="6" class="password validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras" onkeydown="javascript:ValidarTamanhoCampo(this, event);" />

                                <!--button type="button" class="teclado-virtual">
                                    <img src="../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual">
                                </button-->
                            </div>
                        </div>
                        
                        <div class="col2">
                            <div class="campo-basico campo-senha">
                                <label>Confirmar Senha:</label>
                                
                                <input  id="txtCadastro_PFPasso1_SenhaConfirmacao" runat="server" type="password" maxlength="6" class="password validate[required,custom[onlyNumber],custom[fixedLength],equals[ContentPlaceHolder1_txtCadastro_PFPasso1_Senha]] ProibirLetras" onkeydown="javascript:ValidarTamanhoCampo(this, event);"/>

                                <!--button type="button" class="teclado-virtual">
                                    <img src="../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual">
                                </button-->
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col2">
                            <div class="campo-basico campo-senha">
                                <label>Assinatura Eletrônica:</label>
                                <button type="button" title="Preenchimento obrigatório. A senha deve ter 6 caracteres numéricos." class="interrogacao" tabindex="-1">?</button>

                                <input  id="txtCadastro_PFPasso1_AssEletronica" runat="server" type="password" maxlength="6" class="validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras" onkeydown="javascript:ValidarTamanhoCampo(this, event);" />

                                <!--button type="button" class="teclado-virtual">
                                    <img src="../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual">
                                </button-->
                            </div>
                        </div>
                        
                        <div class="col2">
                            <div class="campo-basico campo-senha">
                                <label>Confirmar Assinatura Eletrônica:</label>

                                <input  id="txtCadastro_PFPasso1_AssEletronicaConfirmacao" runat="server" type="password" maxlength="6" class="password validate[required,custom[onlyNumber],custom[fixedLength],equals[ContentPlaceHolder1_txtCadastro_PFPasso1_AssEletronica]] ProibirLetras" onkeydown="javascript:ValidarTamanhoCampo(this, event);"/>

                                <!--button type="button" class="teclado-virtual">
                                    <img src="../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual">
                                </button-->
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col3">
                            <div class="campo-basico">
                                <label>CPF:</label>
                                <input  id="txtCadastro_PFPasso1_CPF" runat="server" type="text"  maxlength="14" data-CaixaAlta="true" class="validate[required,custom[validatecpf]] Mascara_CPF ProibirLetras EstiloCampoObrigatorio " />
                            </div>
                        </div>
                        
                        <div class="col3">
                            <div class="campo-basico">
                                <label>Nascimento:</label>
                                <input  id="txtCadastro_PFPasso1_DataNascimento" runat="server" type="text"  maxlength="10"  data-CaixaAlta="true" class="validate[required,custom[data],custom[DataNoPassado]] Mascara_Data EstiloCampoObrigatorio" />
                            </div>
                        </div>
                        
                        <div class="col3">
                            <div class="campo-basico">
                                <label>Sexo:</label>
                                <select id="cboCadastro_PFPasso1_Sexo" runat="server" type="select-one" class="validate[required]">
                                    <option value="" selected="selected">[SELECIONE]</option>
                                    <option value="F">Feminino</option>
                                    <option value="M">Masculino</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col3" style="width:80px">
                            <div class="campo-basico">
                                <label style="width:auto; white-space: nowrap">Celular (DDD):</label>
                                <div class="clear ContainerPequeno">

                                    <input  id="txtCadastro_PFPasso1_Cel_DDD" runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras" onblur="txtCadastro_PFPasso1_Cel_DDD_Blur(this)" style="width:72px;"  />

                                </div>
                            </div>
                        </div>
                        
                        <div class="col3" style="width:208px">
                            <div class="campo-basico">
                                <label>Celular (Número):</label>
                                <div class="clear ContainerMedio">
                                    
                                    <input  id="txtCadastro_PFPasso1_Cel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone Mascara_CEL ProibirLetras validate[required]" style="width:208px" />

                                </div>
                            </div>
                        </div>
                        
                        <div class="col3" style="width:80px">
                            <div class="campo-basico">
                                <label>Tel (DDD):</label>
                                <div class="clear ContainerPequeno">

                                    <input  id="txtCadastro_PFPasso1_Tel_DDD" runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras " style="width:72px;" />

                                </div>
                            </div>
                        </div>
                        
                        <div class="col3" style="width:208px">
                            <div class="campo-basico">
                                <label>Tel (Número):</label>
                                <div class="clear ContainerMedio">

                                    <!--label for="txtCadastro_PFPasso1_Tel_Numero" style="width:0.7em;">-</label-->
                                    <input  id="txtCadastro_PFPasso1_Tel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone validate[required] Mascara_TEL ProibirLetras" style="width:208px" />

                                </div>
                            </div>
                        </div>

                        <div class="col3">
                            <div class="campo-basico">
                                <label>Tipo de Telefone:</label>
                                <select id="cboCadastro_PFPasso1_TipoTelefone" runat="server" class="validate[required]" onchange="return cboCadastro_PFPasso1_TipoTelefone_Change(this)" >
                                    <option value="1" selected="selected">RESIDENCIAL</option>
                                    <option value="2">COMERCIAL</option>
                                    <option value="3">CELULAR</option>
                                    <option value="4">FAX</option>
                                </select>
                            </div>
                        </div>
                    </div>
                        
                    <div class="row">
                        <div class="col2">
                            <div class="campo-basico">
                                <label>Como conheceu a Gradual:</label>
                                <select id="cboCadastro_PFPasso1_Conheceu" runat="server" type="select-one" class="validate[required]" style="width:93%" onchange="cboCadastro_PFPasso1_Conheceu_Change(this)">
                                    <option value="">[SELECIONE]</option>
                                    <option value="ASSESSOR">CONTATO DE ASSESSOR</option>
                                    <option value="CLIENTE" selected="selected">CLIENTE GRADUAL</option>
                                    <option value="CURSO">CURSOS, PALESTRAS OU EVENTOS</option>
                                    <option value="EMAIL">EMAIL MARKETING</option>
                                    <option value="AMIGOS">INDICAÇÃO</option>
                                    <option value="NOTICIAS">NOTÍCIAS (RÁDIO, TV, INTERNET)</option>
                                    <option value="REDES SOCIAIS">REDES SOCIAIS E FÓRUNS</option>
                                    <option value="BUSCA">SITES DE BUSCA</option>
                                    <option value="OUTROS">OUTROS</option>
                                </select>
                            </div>
                        </div>

                        <div class="row">
                            <div id="pnlAssessor" runat="server" style="display:none" class="col2">
                                <div class="col2">
                                    <label>Assessor:</label>
                                    <input id="txtCadastro_PFPasso1_Assessor" runat="server" type="text" maxlength="10" class="validate[custom[onlyNumber]] ProibirLetras" style="width:18em !important" />
                                </div>
                                <div class="col2">
                                    <button class="botao btn-padrao btn-erica" onclick="return btnCadastro_VerificarAssessor_Click(this)" style="margin:11px 0px 0px 0px">Verificar</button>
                                </div>

                                <span id="lblNomeAssessor" style="float:left; margin:0; width:100%">
                                <asp:literal id="lblCadastro_PFPasso1_AssessorInicial" runat="server"></asp:literal>
                                </span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col2">
                                <div class="campo-basico">
                                    <label>Você deseja operar em:</label>
                                    <div class="col2" id="divOperarEm">
                                        <label class="checkbox"><input  id="chkOperarEmBolsa"  value="1" onclick="return chkOperarEm_Selecionar_Click(this)"  type="checkbox" name="chkOperarEm" class="validate[required] checkbox" />&nbsp;Bolsa </label>
                                        <label class="checkbox"><input  id="chkOperarEmFundos" value="2" onclick="return chkOperarEm_Selecionar_Click(this)"  type="checkbox" name="chkOperarEm" class="validate[required] checkbox" />&nbsp;Fundos</label>
                                        <label class="checkbox"><input  id="chkOperarEmCambio" value="3" onclick="return chkOperarEm_Selecionar_Click(this)"  type="checkbox" name="chkOperarEm" class="validate[required] checkbox" />&nbsp;Câmbio</label>
                                        <label class="checkbox"><input  id="chkOperarEmTodas"  value="4" onclick="return chkOperarEm_Selecionar_Click(this)"  type="checkbox" name="chkOperarEm" class="validate[required] checkbox" />&nbsp;Todas </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                    <div class="row">
                        <div class="col3-2">
                            <p class="red">Todos os campos são obrigatórios</p>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col6">
                            <button onclick="return GradSite_Cadastro_PFPasso1_Verificar(this)" class="botao btn-padrao btn-erica" type="button">Avançar</button>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>

    <div id="pnlLoader" style="display:none">
        <div class="Mensagem">
            <span>Gravando dados, aguarde...</span>

            <p class="BotaoOk" style="display:none">
                <button class="BotaoVerde" onclick="return btnCadastro_Ok_Click(this)">OK</button>
            </p>
        </div>
    </div>


</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Cadastro Novo" />

</asp:Content>
