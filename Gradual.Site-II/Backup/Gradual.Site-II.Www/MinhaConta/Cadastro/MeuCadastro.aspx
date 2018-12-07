<%@ Page Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="MeuCadastro.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Cadastro.MeuCadastro" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/AbasMeuCadastro.ascx"  tagname="AbasMeuCadastro"  tagprefix="ucAbasM" %>

<asp:content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<section class="PaginaConteudo">


    <h2>Meu Cadastro</h2>
    
    <ucAbasM:AbasMeuCadastro id="ucAbasMeuCadastro1" runat="server" />

    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Meus Dados</h3> 
            </div>
        </div>
    </div>
    
    <input type="hidden" id="hidPasso" runat="server" />

<div class="row">
    <div class="col1">

        <div class="abas abas-simples">

            <div class="row">
                <div class="col1">
                    <ul class="abas-menu" data-AbasSimples="true">
                        <li id="liDadosBasicos"     data-IdConteudo="DadosBasicos"     runat="server" class="ativo"><a href="#" id="Aba-DadosBasicos">Dados Básicos</a></li>
                        <li id="liDadosPessoais"    data-IdConteudo="DadosPessoais"    runat="server"><a href="#" id="Aba-DadosPessoais">Dados Pessoais</a></li>
                        <li id="liDadosFinanceiros" data-IdConteudo="DadosFinanceiros" runat="server" data-desabilitado="true" class="inativo"><a href="#" id="Aba-DadosFinanceiros">Dados Financeiros</a></li>
                        <li id="liDadosContratuais" data-IdConteudo="DadosContratuais" runat="server" data-desabilitado="true" class="inativo"><a href="#" id="Aba-DadosContratuais">Dados Contratuais</a></li>
                    </ul>

                    <div class="abas-conteudo" style="padding-bottom: 2em">

                    <div id="pnlDadosBasicos" data-IdConteudo="DadosBasicos" runat="server" class="aba"> 
                    
                        <div class="menu-exportar clear">
                            <h3>Dados Básicos</h3>
                        </div>

                        <div class="form-padrao">
                            <div class="row">
                                <div class="col3"> 
                                    <div class="campo-basico">
                                        <label>Nome completo:</label>
                                        <input  id="txtCadastro_PFPasso1_NomeCompleto" runat="server" type="text"  data-CaixaAlta="true" maxlength="60" class="input-padrao validate[required,length[5, 60]]" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>E-mail:</label>
                                        <input  id="txtCadastro_PFPasso1_Email" runat="server" type="text"  maxlength="80" class="input-padrao validate[required,custom[EmailGambizento]]" placeholder="login" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Confirmar E-mail:</label>
                                        <input  id="txtCadastro_PFPasso1_EmailConfirmacao" runat="server" type="text"  maxlength="80" class="input-padrao validate[required,custom[EmailGambizento],equals[ContentPlaceHolder1_txtCadastro_PFPasso1_Email]]"  />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label> CPF</label>
                                        <input  id="txtCadastro_PFPasso1_CPF"  runat="server" type="text"  maxlength="14" class="validate[custom[validatecpf]] Mascara_CPF ProibirLetras EstiloCampoObrigatorio " style="float:left" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Nascimento:</label>
                                        <input  id="txtCadastro_PFPasso1_DataNascimento" runat="server" type="text"  maxlength="10" class="validate[custom[data],custom[DataNoPassado]] Mascara_Data EstiloCampoObrigatorio " />
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
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Celular:</label>
                                        <div class="clear">
                                            <!--input type="text" value="DDD" name="input-padrao" class="input-ddd" />
                                            <input type="text" value="0000-0000" name="input-padrao" class="input-telefone" /-->

                                            <input  id="txtCadastro_PFPasso1_Cel_DDD" runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras " style=""  />

                                            <!--label for="txtCadastro_PFPasso1_Cel_Numero" style="width:0.7em;">-</label-->
                                            <input  id="txtCadastro_PFPasso1_Cel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone validate[required]] MesmaLinha ProibirLetras" style="" />

                                        </div>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Telefone:</label>
                                        <div class="clear">
                                            <!--input type="text" value="DDD" name="input-padrao" class="input-ddd" />
                                            <input type="text" value="0000-0000" name="input-padrao" class="input-telefone" /-->

                                            <input  id="txtCadastro_PFPasso1_Tel_DDD" runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras " style=""  />

                                            <!--label for="txtCadastro_PFPasso1_Tel_Numero" style="width:0.7em;">-</label-->
                                            <input  id="txtCadastro_PFPasso1_Tel_Numero" runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone validate[required] MesmaLinha ProibirLetras"  />

                                        </div>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Tipo de Telefone:</label>

                                        <select id="cboCadastro_PFPasso1_TipoTelefone" runat="server" class="validate[required]" onchange="return cboCadastro_PFPasso1_TipoTelefone_Change(this)">
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
                                        <select id="cboCadastro_PFPasso1_Conheceu" runat="server" type="select-one" class="validate[required]" onchange="cboCadastro_PFPasso1_Conheceu_Change(this)">
                                            <option value="" selected="selected">[SELECIONE]</option>
                                            <option value="ASSESSOR">CONTATO DE ASSESSOR</option>
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

                                <div id="pnlAssessor" runat="server" style="display:none" class="col2">
                                    <div class="col2">
                                        <label>Assessor:</label>
                                        <input id="txtCadastro_PFPasso1_Assessor" runat="server" type="text" maxlength="10" class="validate[custom[onlyNumber]] ProibirLetras" />
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
                                <div class="col1">
                                    <p class="red">Todos os campos são obrigatórios</p>
                                </div>
                            </div>

                            <div id="pnlBotaoSalvarDadosPasso1" runat="server" class="row">
                                <div class="col1">
                                    <button class="botao btn-padrao btn-erica" onclick="return GradSite_Cadastro_PFPasso1_Verificar(this)" type="button">Salvar</button>
                                </div>
                            </div>
                            
                            <div id="pnlBotaoAlterarDadosPasso1" runat="server" visible="false" class="row">
                                <div class="col3" style="margin-left: 312px;">
                                    <button class="botao btn-padrao" onclick="return GradSite_Cadastro_AlterarDadosRedirecionando(this)" type="button">Alterar Dados</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="pnlDadosPessoais" data-IdConteudo="DadosPessoais" runat="server" class="aba" style="display:none">
                    
                        <div class="menu-exportar clear">
                            <h3>Dados Pessoais 
                                <% if (this.ModoDeTeste) { %>
                                <a href="#" onclick="return Cadastro_PreencherTeste_P2()" style="font-size:0.4em; font-weight:normal; color:Blue">preencher</a>
                                <% } %>
                            </h3>
                        </div>

                        <div class="form-padrao">
                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Nacionalidade:</label>
                                        <asp:dropdownlist id="cboCadastro_PFPasso2_Nacionalidade" runat="server" type="select-one" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id" onchange="cboCadastro_PFPasso2_Nacionalidade_Change(this)">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div  id="pnlPaisDeNascimento" runat="server" style="display:none" class="col4">
                                    <div class="campo-basico">
                                    <label for="cboCadastro_PFPasso2_PaisNascimento">País de Nascimento:</label>

                                    <asp:dropdownlist id="cboCadastro_PFPasso2_PaisNascimento" runat="server" type="select-one" cssclass="EstiloCampoObrigatorio validate[required]" datatextfield="Value" datavaluefield="Id" onchange="cboCadastro_PFPasso2_PaisNascimento_Change(this)">
                                    </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Estado de Nascimento:</label>

                                        <input  id="txtCadastro_PFPasso2_EstadoNascimento" runat="server" type="text" maxlength="100" style="display:none" />

                                        <asp:dropdownlist id="cboCadastro_PFPasso2_EstadoNascimento" runat="server" type="select-one" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id">
                                        </asp:dropdownlist>

                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Cidade de Nascimento:</label>
                                        <input  id="txtCadastro_PFPasso2_CidadeNascimento" data-CaixaAlta="true" runat="server" type="text" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Estado Civil:</label>

                                        <asp:dropdownlist id="cboCadastro_PFPasso2_EstadoCivil" runat="server" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id" onchange="cboCadastro_PFPasso2_EstadoCivil_Change(this)">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Cônjuge:</label>
                                        <input  id="txtCadastro_PFPasso2_Conjuge" runat="server" data-CaixaAlta="true" type="text" maxlength="100" class="" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Profissão:</label>

                                        <asp:dropdownlist id="cboCadastro_PFPasso2_Profissao" runat="server" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id" onchange="cboCadastro_PFPasso2_Profissao_Change(this)">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Ocupação:</label>
                                        <input  id="txtCadastro_PFPasso2_CargoFuncao" runat="server" data-CaixaAlta="true" type="text" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Empresa:</label>
                                        <input  id="txtCadastro_PFPasso2_Empresa" runat="server" type="text" data-CaixaAlta="true" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col5">
                                    <div class="campo-basico">
                                        <label>Tipo Documento:</label>

                                        <asp:dropdownlist id="cboCadastro_PFPasso2_TipoDocumento" runat="server" type="select-one" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col5">
                                    <div class="campo-basico">
                                        <label>Nº do Documento:</label>
                                        <input  id="txtCadastro_PFPasso2_NumeroDocumento" runat="server" type="text" data-CaixaAlta="true" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>

                                <div class="col5">
                                    <div class="campo-basico">
                                        <label>Órgão Emissor:</label>
                                        <asp:dropdownlist id="cboCadastro_PFPasso2_OrgaoEmissor" type="select-one" runat="server" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col5">
                                    <div class="campo-basico">
                                        <label>Estado de Emissão:</label>
                                        <asp:dropdownlist id="cboCadastro_PFPasso2_EstadoEmissao" type="select-one" runat="server" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id">
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                                
                                <div class="col5">
                                    <div class="campo-basico">
                                        <label for="txtCadastro_PFPasso2_DataEmissao">Data de Emissão:</label>
                                        <input  id="txtCadastro_PFPasso2_DataEmissao" runat="server" type="text" maxlength="10" class="validate[required,custom[data]] Mascara_Data EstiloCampoObrigatorio " />
                                    </div>
                                </div>
                            </div>
 
                            <div class="row">
                                <div class="col2">
                                    <div class="campo-basico">
                                        <label>Nome da Mãe:</label>
                                        <input  id="txtCadastro_PFPasso2_NomeMae" runat="server" type="text" data-CaixaAlta="true" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>

                                <div class="col2">
                                    <div class="campo-basico">
                                        <label>Nome do Pai:</label>
                                        <input  id="txtCadastro_PFPasso2_NomePai" runat="server" type="text" data-CaixaAlta="true" maxlength="100" class="validate[required] EstiloCampoObrigatorio" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col1">
                                    <div class="menu-exportar clear">
                                        <h5>Endereço Residencial</h5>
                                    </div>
                                </div>
                            </div>

                            <input type="hidden" runat="server" id="hidCadastro_PFPasso2_IdEndereco1" />

                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>CEP:</label>
                                        <input  id="txtCadastro_PFPasso2_Endereco1_CEP" runat="server" type="text" maxlength="8" class="Mascara_CEP ProibirLetras EstiloCampoObrigatorio validate[required]" onkeyup="txtCEP_KeyUp(this, event)"  data-CEPGroup="end1" data-CEPProp="cep" title="CEP" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Logradouro:</label>
                                        <input  id="txtCadastro_PFPasso2_Endereco1_Logradouro" runat="server" type="text" data-CaixaAlta="true" maxlength="30" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="end1" data-CEPProp="logradouro" title="Logradouro" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col1">
                                    <div class="row">
                                        <div class="col3">
                                            <div class="campo-basico">
                                                <label>Número:</label>
                                                <input  id="txtCadastro_PFPasso2_Endereco1_Numero" runat="server" type="text" maxlength="5" class="EstiloCampoObrigatorio validate[required]" title="Número" />
                                            </div>
                                        </div>

                                        <div class="col3">
                                            <div class="campo-basico">
                                                <label>Complemento:</label>
                                                <input  id="txtCadastro_PFPasso2_Endereco1_Complemento" runat="server" type="text" maxlength="10" title="Complemento" />
                                            </div>
                                        </div>

                                        <div class="col3">
                                            <div class="campo-basico">
                                                <label>Bairro:</label>
                                                <input  id="txtCadastro_PFPasso2_Endereco1_Bairro" runat="server" type="text" data-CaixaAlta="true" maxlength="18" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="end1" data-CEPProp="bairro" title="Bairro" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Cidade:</label>
                                        <input  id="txtCadastro_PFPasso2_Endereco1_Cidade" runat="server" type="text" data-CaixaAlta="true" maxlength="28" class="EstiloCampoObrigatorio validate[required]" data-CEPGroup="end1" data-CEPProp="cidade" title="Cidade" />
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>Estado:</label>
                                        <asp:dropdownlist id="cboCadastro_PFPasso2_Endereco1_Estado" runat="server" type="select-one" cssclass="EstiloCampoObrigatorio validate[required]" data-CEPGroup="end1" data-CEPProp="uf_sigla" datatextfield="Value" datavaluefield="Id">
                                        </asp:dropdownlist>
                                    </div>
                                </div>

                                <div class="col3">
                                    <div class="campo-basico">
                                        <label>País:</label>
                                        <asp:dropdownlist id="cboCadastro_PFPasso2_Endereco1_Pais" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" data-CEPGroup="end1" data-CEPProp="pais" cssclass="EstiloCampoObrigatorio validate[required]">
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col3-2">
                                    <p>Endereço para correspondência:</p>

                                    <div class="lista-radio tipo-endereco">
                                        <label class="radio">
                                            <input  id="rdoCadastro_PFPasso2_End1" name="rdoCadastro_PFPasso2_End" runat="server" type="radio" style="width:1em !important;" onclick="return rdoCadastroEndereco_Click(this)" checked="true" />
                                            Residencial
                                        </label>
                                        <label class="radio">
                                            <input  id="rdoCadastro_PFPasso2_End2" name="rdoCadastro_PFPasso2_End" runat="server" type="radio" style="width:1em !important;" onclick="return rdoCadastroEndereco_Click(this)" />
                                            Comercial
                                        </label>
                                        <label class="radio">
                                            <input  id="rdoCadastro_PFPasso2_End3" name="rdoCadastro_PFPasso2_End" runat="server" type="radio" style="width:1em !important;" onclick="return rdoCadastroEndereco_Click(this)" />
                                            Outro
                                        </label>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col1">
                                    <p class="red" style="padding-bottom:0px; margin-top:0px;">Todos os campos são obrigatórios</p>
                                </div>
                            </div>

                            <div  id="pnlSegundoEndereco" runat="server" style="float:left; display:none" class="endereco-correspondencia">
                                <div class="row">
                                    <div class="col1">
                                        <div class="menu-exportar clear">
                                            <h5>Endereço de Correspondência</h5>
                                        </div>
                                    </div>
                                </div>

                                <input type="hidden" runat="server" id="hidCadastro_PFPasso2_IdEndereco2" />

                                <div class="row"> 
                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>CEP:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_CEP" runat="server" type="text" maxlength="8" class="Mascara_CEP ProibirLetras EstiloCampoObrigatorio " onkeyup="txtCEP_KeyUp(this, event)" data-CEPGroup="end2" data-CEPProp="cep" title="CEP" />
                                        </div>
                                    </div>

                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Logradouro:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_Logradouro" runat="server" type="text" data-CaixaAlta="true" maxlength="30" class="EstiloCampoObrigatorio" data-CEPGroup="end2" data-CEPProp="logradouro" title="Logradouro" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Número:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_Numero" runat="server" type="text" maxlength="5" class="EstiloCampoObrigatorio" title="Número" />
                                        </div>
                                    </div>

                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Complemento:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_Complemento" runat="server" type="text" data-CaixaAlta="true" maxlength="10" title="Complemento" />
                                        </div>
                                    </div>

                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Bairro:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_Bairro" runat="server" type="text" data-CaixaAlta="true" maxlength="18" class="EstiloCampoObrigatorio" data-CEPGroup="end2" data-CEPProp="bairro" title="Bairro" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Cidade:</label>
                                            <input  id="txtCadastro_PFPasso2_Endereco2_Cidade" runat="server" type="text" data-CaixaAlta="true" maxlength="28" class="EstiloCampoObrigatorio" data-CEPGroup="end2" data-CEPProp="cidade" title="Cidade" />
                                        </div>
                                    </div>

                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>Estado:</label>
                                            <asp:dropdownlist id="cboCadastro_PFPasso2_Endereco2_Estado" runat="server" datatextfield="Value" datavaluefield="Id" data-CEPGroup="end2" data-CEPProp="uf_sigla" cssclass="EstiloCampoObrigatorio">
                                            </asp:dropdownlist>
                                        </div>
                                    </div>

                                    <div class="col3">
                                        <div class="campo-basico">
                                            <label>País:</label>
                                            <asp:dropdownlist id="cboCadastro_PFPasso2_Endereco2_Pais" runat="server" type="select-one" datatextfield="Value" data-CEPGroup="end2" data-CEPProp="pais" datavaluefield="Id" cssclass="EstiloCampoObrigatorio">
                                            </asp:dropdownlist>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div id="pnlBotaoSalvarDadosPasso2" runat="server" class="row">
                                <div class="col3-2">
                                    <button class="botao btn-padrao btn-erica" type="button" onclick="return GradSite_Cadastro_PFPasso2_Verificar(this)">Salvar</button>
                                </div>
                            </div>
                            
                            <div id="pnlBotaoAlterarDadosPasso2" runat="server" visible="false" class="row">
                                <div class="col3" style="margin-left: 312px;">
                                    <button class="botao btn-padrao" onclick="return GradSite_Cadastro_AlterarDadosRedirecionando(this)" type="button">Alterar Dados</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="pnlDadosFinanceiros" data-IdConteudo="DadosFinanceiros" runat="server" class="aba" style="display:none">

                        <div class="menu-exportar clear">
                            <h3>Dados Financeiros 
                            <% if (this.ModoDeTeste) { %>
                                <a href="#" onclick="return Cadastro_PreencherTeste_P3()" style="font-size:0.4em; font-weight:normal; color:Blue">preencher</a>
                                <a href="#" onclick="return Cadastro_PreencherTeste_P3(true)" style="font-size:0.4em; font-weight:normal; color:Blue; margin-left:2em">completo</a>
                                <% } %>
                            </h3>
                        </div>

                        <div class="form-padrao">
                            <div class="row">
                                <div class="col1">
                                    <div class="row">
                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Salário / Pro-Labore:</label>
                                                <input  id="txtCadastro_PFPasso3_SalarioProlabore" runat="server" type="text" maxlength="14" placeholder="R$ 0,00" class="ValorMonetario validate[custom[numeroFormatado]]" />
                                            </div>
                                        </div>

                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Aplicações Financeiras:</label>
                                                <input  id="txtCadastro_PFPasso3_ValorTotalEmAplicacoesFinanceiras" runat="server" type="text" maxlength="14" placeholder="R$ 0,00" class="ValorMonetario validate[custom[numeroFormatado]]" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Bens Móveis:</label>
                                                <input  id="txtCadastro_PFPasso3_TotalEmBensMoveis" runat="server" type="text" maxlength="14" placeholder="R$ 0,00" class="ValorMonetario validate[custom[numeroFormatado]]" />
                                            </div>
                                        </div>

                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Bens Imóveis:</label>
                                                <input  id="txtCadastro_PFPasso3_TotalEmBensImoveis" runat="server" type="text" maxlength="14" placeholder="R$ 0,00" class="ValorMonetario validate[custom[numeroFormatado]]" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Outros Rendimentos:</label>
                                                <input  id="txtCadastro_PFPasso3_OutrosRendimentos" runat="server" type="text" maxlength="14" placeholder="R$ 0,00" class="ValorMonetario validate[custom[numeroFormatado]]" />
                                            </div>
                                        </div>

                                        <div class="col2">
                                            <div class="campo-basico">
                                                <label>Desc. dos Outros Rendimentos:</label>
                                                <input  id="txtCadastro_PFPasso3_OutrosRendimentosDesc" runat="server" type="text" class="" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col1">
                                            <div class="menu-exportar clear">
                                                <h5>Informações Bancárias</h5>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="pnlFormNovaContaBancaria" runat="server">

                                        <div class="row">
                                            <div class="col2">
                                                <div class="campo-basico">
                                                    <label>Tipo de Conta:</label>
                                                    <asp:dropdownlist id="cboCadastro_PFPasso3_ContasBancarias_TipoConta" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" cssclass="EstiloCampoObrigatorio">
                                                    </asp:dropdownlist>
                                                </div>
                                            </div>

                                            <div class="col2">
                                                <div class="campo-basico">
                                                    <label>Banco:</label>
                                                        <asp:dropdownlist id="cboCadastro_PFPasso3_ContasBancarias_Banco" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" onchange="return cboCadastro_PFPasso3_ContasBancarias_Banco_OnChange(this);" cssclass="EstiloCampoObrigatorio">
                                                        </asp:dropdownlist>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col2">
                                                <div class="campo-basico">
                                                    <label>Agência e Dígito:</label>
                                                    <div class="clear">
                                                       <input  id="txtCadastro_PFPasso3_ContasBancarias_Agencia" type="text" maxlength="5" class="EstiloCampoObrigatorio ProibirLetras input-telefone" style="float:left;" />
                                                       <input  id="txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito" type="text" maxlength="2" class="EstiloCampoObrigatorio ProibirLetrasMenosX input-ddd" style="float:right; position:absolute; right:0;" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col2">
                                                <div class="campo-basico">
                                                    <label>Conta e Dígito:</label>
                                                    <div class="clear">
                                                        <input  id="txtCadastro_PFPasso3_ContasBancarias_Conta" type="text" maxlength="13" class="EstiloCampoObrigatorio ProibirLetras input-telefone" style="float:left;" />
                                                        <input  id="txtCadastro_PFPasso3_ContasBancarias_ContaDigito" type="text" maxlength="2" class="EstiloCampoObrigatorio ProibirLetras input-ddd" style="float:right; position:absolute; right:0;" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col3-2">
                                                <!--p class="red">Todos os campos são obrigatórios</p-->

                                                <p>Títular da Conta? <button class="interrogacao" title="Caso sua conta seja conjunta, será necessário informar o nome e CPF do co-titular" type="button" tabindex="-1">?</button></p>

                                                <div class="lista-radio">
                                                    <label class="radio">
                                                        <input  id="rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_Sim" name="rdoCadastro_PFPasso3_ContasBancarias_ContaTitular" type="radio" onclick="return rdoCadastro_PFPasso3_ContasBancarias_Click(this)" checked="checked" />
                                                        Sim
                                                    </label>
                                                    <label class="radio">
                                                        <input  id="rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_Nao" name="rdoCadastro_PFPasso3_ContasBancarias_ContaTitular" type="radio" onclick="return rdoCadastro_PFPasso3_ContasBancarias_Click(this)" />
                                                         Não
                                                    </label>
                                                </div>
                                            </div>

                                            <div id="pnlTitularConta" style="display:none; float:left; width:100%">
                                                <p style="width:55%;">
                                                    <label for="txtCadastro_PFPasso3_ContasBancarias_NomeTitular">Nome do Titular:</label>
                                                    <input  id="txtCadastro_PFPasso3_ContasBancarias_NomeTitular" type="text" data-CaixaAlta="true" maxlength="120" class="EstiloCampoObrigatorio" />
                                                </p>

                                                <p style="width:20%;">
                                                    <label for="txtCadastro_PFPasso3_ContasBancarias_CPFTitular">CPF do Titular:</label>
                                                    <input  id="txtCadastro_PFPasso3_ContasBancarias_CPFTitular" type="text" data-CaixaAlta="true"  maxlength="10" class="EstiloCampoObrigatorio Mascara_CPF ProibirLetras " />
                                                </p>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <button class="botao btn-padrao btn-erica" onclick="return btnCadastro_PFPasso3_AdicionarContaBancaria_Click(this)" type="button">Adicionar</button>
                                            </div>
                                        </div>

                                    </div>

                                    <!--div class="row">
                                        <div class="col1">
                                            <table class="tabela">
                                                <tr class="tabela-titulo tabela-type-small">
                                                    <td>Banco</td>
                                                    <td>Cód. Banco</td>
                                                    <td>Tipo</td>
                                                    <td>Agência</td>
                                                    <td>Díg.</td>
                                                    <td>Conta</td>
                                                    <td>Díg.</td>
                                                    <td></td>
                                                </tr>
                                                        
                                                <tr class="tabela-type-small">
                                                    <td>Banco Bradesco SA</td>
                                                    <td>00000</td>
                                                    <td>Conta Corrente</td>
                                                    <td>00000</td>
                                                    <td>00000</td>
                                                    <td>00000</td>
                                                    <td>00000</td>
                                                    <td class="alignCenter"><button type="button" class="botao btn-invista">Excluir</button></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div-->


                                    <input type="hidden" id="hidCadastro_PFPasso3_ContasBancarias" runat="server" />

                                    <table id="tblCadastro_PFPasso3_ContaBancaria" class="tabela TabelaDeContas">
                                        <thead>
                                            <tr class="tabela-titulo tabela-type-small">
                                                <td style="width:16%">Banco</td>
                                                <td style="width:10%">Cód. Banco</td>
                                                <td style="width:14%">Tipo</td>
                                                <td style="width:12%">Agência</td>
                                                <td style="width:8%">Dígito</td>
                                                <td style="width:12%">Conta</td>
                                                <td style="width:8%">Dígito</td>
                                                <td style="width:4%">&nbsp;</td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr id="trNenhumItem" runat="server" class="NenhumItem">
                                                <td colspan="9">(nenhuma conta incluída)</td>
                                            </tr>
                                            <asp:repeater id="rptContasBancarias" runat="server" visible="false">
                                            <itemtemplate>
                                            <tr>
                                                <td class="BancoDesc">      <%#Eval("BancoDesc") %>      </td>
                                                <td class="Banco">          <%#Eval("Banco") %>          </td>
                                                <td class="TipoConta">      <%#Eval("TipoContaDesc") %>  </td>
                                                <td class="Agencia">        <%#Eval("Agencia") %>        </td>
                                                <td class="AgenciaDigito">  <%#Eval("AgenciaDigito") %>  </td>
                                                <td class="Conta">          <%#Eval("Conta") %>          </td>
                                                <td class="ContaDigito">    <%#Eval("ContaDigito") %>    </td>
                                                <td>
                                                    <button onclick="return btnCadastro_PFPasso3_ExcluirContaBancaria_Click(this);" class="BotaoIcone IconeRemoverMarrom"><span>Excluir</span></button>
                                                </td>
                                            </tr>
                                            </itemtemplate>
                                            </asp:repeater>
                                            <tr style="display: none;" class="Template">
                                                <td class="BancoDesc"></td>
                                                <td class="Banco"></td>
                                                <td class="TipoConta"></td>
                                                <td class="Agencia"></td>
                                                <td class="AgenciaDigito"></td>
                                                <td class="Conta"></td>
                                                <td class="ContaDigito"></td>
                                                <td>
                                                    <button onclick="return btnCadastro_PFPasso3_ExcluirContaBancaria_Click(this);" class="BotaoIcone IconeRemoverMarrom"><span>Excluir</span></button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <div class="row">
                                        <div class="col1">
                                            <div class="menu-exportar clear">
                                                <h5>Informações Complementares</h5>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col3-2">
                                            <ul class="lista lista-pergunta" style="margin-bottom:0px">
                                                <li class="clear" title="São consideradas pessoas vinculadas administradores, empregados, operadores, agentes autônomos, sócios, diretores e demais profissionais que prestem serviço relacionado diretamente à atividade de intermediação, bem como cônjuge ou companheiro, filhos menores e parentes de até 2º grau. Segundo a resolução º 505 da CVM, pessoas vinculadas devem observar regras especiais para negociação em Bolsa.">
                                                    <span>Pessoa Vinculada:</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="pessoa-vinculada" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="pessoa-vinculada" value="Sim, a Gradual" /> Sim, a Gradual</label>
                                                        <label class="radio"><input type="radio" name="pessoa-vinculada" value="Sim, a outra corretora" /> Sim, a outra corretora</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_PessoaVinculada_Nao"  type="radio" runat="server" name="rdoPessoaVinculada" value="0" checked="true" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_PessoaVinculada_SimG" type="radio" runat="server" name="rdoPessoaVinculada" value="2" /> Sim, à Gradual</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_PessoaVinculada_Sim"  type="radio" runat="server" name="rdoPessoaVinculada" value="1" /> Sim, a outra corretora</label>
                                                    </div>
                                                </li>

                                                <li class="clear" title="Caso opere por intermédio de terceiros ou gestores, clique em não.">
                                                    <span>Opera por conta própria:</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="opera-conta-propria" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="opera-conta-propria" value="Sim" /> Sim</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_OperaContaPropria_Nao" type="radio" runat="server" name="rdoOperaPorContaPropria" value="Nao" onclick="return rdoCadastro_PFPasso3_Opera_Click(this)" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_OperaContaPropria_Sim" type="radio" runat="server" name="rdoOperaPorContaPropria" value="Sim" onclick="return rdoCadastro_PFPasso3_Opera_Click(this)" checked="true" /> Sim</label>
                                                    </div>

                                                    <div id="pnlDadosClienteOpera" runat="server" style="display: none; float: left; width: 900px;height: 110px;margin-top:-20px;">

                                                        <p style="width:69%;float:left;margin-top:0px;margin-right:2em">
                                                            <label for="txtCadastro_PFPasso3_CliNomeCompleto">Nome Completo do Cliente:</label>
                                                            <input  id="txtCadastro_PFPasso3_CliNomeCompleto" type="text" runat="server" maxlength="60" class="EstiloCampoObrigatorio" style="width:450px" />
                                                        </p>

                                                        <p style="width:20%;float:left;margin-top:0px">
                                                            <label for="ContentPlaceHolder1_txtCadastro_PFPasso3_CliCPF">CPF/CNPJ:</label>
                                                            <input  id="txtCadastro_PFPasso3_CliCPF" type="text" runat="server" maxlength="20" class="ProibirLetras EstiloCampoObrigatorio" style="width:196px" />
                                                        </p>

                                                    </div>

                                                </li>

                                                <li class="clear" title="São considerados agentes públicos que desempenham ou tenham desempenhado, nos últimos cinco anos, cargos, empregos ou funções públicas relevantes no Brasil ou outros países, territórios e dependências estratégicas, assim como seus representantes, familiares e outras pessoas de seu relacionamento próximo.">
                                                    <span>Pessoa politicamente exposta:</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="politicamente-exposta" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="politicamente-exposta" value="Sim" /> Sim</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_PPE_Nao" type="radio" runat="server" name="rdoPPE" value="Nao" checked="true" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_PPE_Sim" type="radio" runat="server" name="rdoPPE" value="Sim" /> Sim</label>
                                                    </div>
                                                </li>

                                                <li class="clear">
                                                    <span>Emancipado:</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="emancipado" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="emancipado" value="Sim" /> Sim</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_Emancipado_Nao" type="radio" runat="server" name="rdoEmancipado" value="Nao" onclick="return rdoCadastro_PFPasso3_Emancipado_Click(this);" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_Emancipado_Sim" type="radio" runat="server" name="rdoEmancipado" value="Sim" onclick="return rdoCadastro_PFPasso3_Emancipado_Click(this);" checked="true" /> Sim</label>
                                                    </div>
                                                </li>

                                                <li class="clear" title="Conforme Lei FATCA – Foreign Account Tax Compliance Act.">
                                                    <span>Você é 'US Person'?</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="representante-legal" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="representante-legal" value="Sim" /> Sim</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_USPerson_Nao" type="radio" runat="server" name="rdoCadastro_PFPasso3_USPerson" value="Nao" checked="true" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_USPerson_Sim" type="radio" runat="server" name="rdoCadastro_PFPasso3_USPerson" value="Sim" /> Sim</label>
                                                    </div>
                                                </li>

                                                <li class="clear" title="Caso possua representante legal ou procurador, clique em sim e preencha o formulário.">
                                                    <span>Representante Legal/Procurador:</span>
                                                    <div class="lista-radio">
                                                        <!--label class="radio"><input type="radio" name="representante-legal" value="Não" /> Não</label>
                                                        <label class="radio"><input type="radio" name="representante-legal" value="Sim" /> Sim</label-->

                                                        <label> <input  id="rdoCadastro_PFPasso3_Procurador_Nao" type="radio" runat="server" name="rdoRepresentanteLegalProcurador" value="Nao" onclick="return rdoCadastro_PFPasso3_RepresentanteLegal_Click(this);" checked="true" /> Não</label>
                                                        <label> <input  id="rdoCadastro_PFPasso3_Procurador_Sim" type="radio" runat="server" name="rdoRepresentanteLegalProcurador" value="Sim" onclick="return rdoCadastro_PFPasso3_RepresentanteLegal_Click(this);" /> Sim</label>
                                                    </div>
                                                </li>
                                            </ul>

                                        </div>
                                    </div>
                                    

                                    <div id="pnlDadosRepresentanteLegal" runat="server" style="display:none">

                                        <div class="row">
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="cboCadastro_PFPasso3_RepSituacaoLegal">Situação Legal:</label>
                                                    <div class="clear">
                                                        <asp:dropdownlist id="cboCadastro_PFPasso3_RepSituacaoLegal" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" cssclass="">
                                                        </asp:dropdownlist>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col2">
                                                <div class="campo-basico">
                                                    <label for="txtCadastro_PFPasso3_RepNomeCompleto">Nome Completo:</label>
                                                    <div class="clear">
                                                        <input  id="txtCadastro_PFPasso3_RepNomeCompleto" type="text" data-CaixaAlta="true" runat="server" maxlength="60" class="EstiloCampoObrigatorio" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="txtCadastro_PFPasso3_RepCPF">CPF:</label>
                                                    <div class="clear">
                                                        <input  id="txtCadastro_PFPasso3_RepCPF" type="text" runat="server" maxlength="10" class="EstiloCampoObrigatorio Mascara_CPF ProibirLetras" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="txtCadastro_PFPasso3_RepDataNascimento">Data de Nascimento:</label>
                                                    <div class="clear">
                                                        <input  id="txtCadastro_PFPasso3_RepDataNascimento" type="text" runat="server" maxlength="10" class="Mascara_Data EstiloCampoObrigatorio" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="cboCadastro_PFPasso3_RepTipoDocumento">Tipo de Documento:</label>
                                                    <div class="clear">
                                                        <asp:dropdownlist id="cboCadastro_PFPasso3_RepTipoDocumento" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" onchange="return cboCadastro_PFPasso3_ContasBancarias_Banco_OnChange(this);" cssclass="EstiloCampoObrigatorio">
                                                        </asp:dropdownlist>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="cboCadastro_PFPasso3_RepOrgaoEmissor">Órgão Emissor:</label>
                                                    <div class="clear">
                                                        <asp:dropdownlist id="cboCadastro_PFPasso3_RepOrgaoEmissor" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" onchange="return cboCadastro_PFPasso3_ContasBancarias_Banco_OnChange(this);" cssclass="EstiloCampoObrigatorio">
                                                        </asp:dropdownlist>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="txtCadastro_PFPasso3_RepNumeroDocumento">Nº do Documento:</label>
                                                    <div class="clear">
                                                        <input  id="txtCadastro_PFPasso3_RepNumeroDocumento" type="text" runat="server" maxlength="16" class="EstiloCampoObrigatorio" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col3">
                                                <div class="campo-basico">
                                                    <label for="cboCadastro_PFPasso3_RepEstadoEmissao">Estado de Emissão:</label>
                                                    <div class="clear">
                                                        <asp:dropdownlist id="cboCadastro_PFPasso3_RepEstadoEmissao" runat="server" type="select-one" datatextfield="Value" datavaluefield="Id" onchange="return cboCadastro_PFPasso3_ContasBancarias_Banco_OnChange(this);" cssclass="EstiloCampoObrigatorio">
                                                        </asp:dropdownlist>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    

                                    <div class="row">
                                        <div class="col1">
                                            <p style="width:95%;" class="IconeAjuda" title="Conforme determine a Instrução CVM553/2014">
                                                <label for="chkCadastro_PFPasso3_Ciente_Doc_Regulamento">Declaro que recebi e estou ciente dos seguintes documentos:</label>
                                                <label><input  id="chkCadastro_PFPasso3_Ciente_Doc_Regulamento" runat="server" type="checkbox" style="margin-left:4px" /> regulamento </label>
                                                <label><input  id="chkCadastro_PFPasso3_Ciente_Doc_Prospecto"   runat="server" type="checkbox" style="margin-left:4px" /> prospecto</label>
                                                <label><input  id="chkCadastro_PFPasso3_Ciente_Doc_Lamina"      runat="server" type="checkbox" style="margin-left:4px" /> prospecto lâmina</label>
                                            </p>
                                        </div>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="col1">
                                            <p style="width:95%;background-position:left 0px" class="IconeAjuda" title="Declaração conforme a Instrução da CVM553/2014">
                                                <label class="Icone-Ajuda" for="">O propósito da relação de negócios com a Gradual se dará com o seguinte fim:</label>
                                                <input id="txtCadastro_PFPasso3_Proposito" runat="server" type="text" data-CaixaAlta="true" maxlength="40" class="input-padrao validate[required]" />
                                            </p>
                                        </div>
                                    </div>

                                    <div id="pnlFormPerfil" runat="server">

                                        <div class="menu-exportar">
                                            <h4>Perfil do Investidor</h4>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <input type="hidden" id="hidCadastro_PFPasso3_JaPreencheuSuit" runat="server" value="false" />

                                                <p><strong>1. Há quanto tempo você investe no mercado fianceiro?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio" style="display:block;">
                                                        <input  id="rdoSuit_01_A" type="radio" value="0" name="rdoSuit_01" />
                                                        Há mais de cinco anos;
                                                    </label>
                                                    <label class="radio" style="display:block;">
                                                        <input  id="rdoSuit_01_B" type="radio" value="1" name="rdoSuit_01" />
                                                         Entre dois e cinco anos;
                                                    </label>
                                                    <label class="radio" style="display:block;">
                                                        <input  id="rdoSuit_01_C" type="radio" value="2" name="rdoSuit_01" />
                                                        Há menos de dois anos;
                                                    </label>
                                                    <label class="radio" style="display:block;">
                                                        <input  id="rdoSuit_01_D" type="radio" value="3" name="rdoSuit_01" />
                                                         Ainda não invisto;
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>2. Qual o seu conhecimento sobre mercado financeiro?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio"><input  id="rdoSuit_02_A" type="radio" value="0" name="rdoSuit_02" /> Nenhum</label>
                                                    <label class="radio"><input  id="rdoSuit_02_B" type="radio" value="1" name="rdoSuit_02" /> Pouco</label>
                                                    <label class="radio"><input  id="rdoSuit_02_C" type="radio" value="2" name="rdoSuit_02" /> Bom</label>
                                                    <label class="radio"><input  id="rdoSuit_02_D" type="radio" value="3" name="rdoSuit_02" /> Ótimo</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>3. O que você pretende quando investe no mercado financeiro?</strong></p>

                                                <div class="lista-radio">
                                                    <label class="radio"><input  id="rdoSuit_03_A" type="radio" value="0" name="rdoSuit_03" /> Proteger seu dinheiro da inflação</label>
                                                    <label class="radio"><input  id="rdoSuit_03_B" type="radio" value="1" name="rdoSuit_03" /> Ganhar dos produtos dos bancos (CDB, Capitalização)</label>
                                                    <label class="radio"><input  id="rdoSuit_03_C" type="radio" value="2" name="rdoSuit_03" /> Ganhar da Poupança</label>
                                                    <label class="radio"><input  id="rdoSuit_03_D" type="radio" value="3" name="rdoSuit_03" /> Investir com isenção de imposto de renda</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>4. Em qual destes ativos você aplicaria a maior parte do seu dinheiro?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio"><input  id="rdoSuit_04_A" type="radio" value="0" name="rdoSuit_04" /> Imóveis</label>
                                                    <label class="radio"><input  id="rdoSuit_04_B" type="radio" value="1" name="rdoSuit_04" /> Fundos de Renda fixa e/ou produtos com isenção de IR (LCI ou LCA)</label>
                                                    <label class="radio"><input  id="rdoSuit_04_C" type="radio" value="2" name="rdoSuit_04" /> Fundos Multimercados;</label>
                                                    <label class="radio"><input  id="rdoSuit_04_D" type="radio" value="3" name="rdoSuit_04" /> Ações e/ou Fundos de Ações;</label>
                                                    <label class="radio"><input  id="rdoSuit_04_E" type="radio" value="4" name="rdoSuit_04" /> Derivativos</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>5. Quanto você pretende aplicar?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_A" type="radio" value="0" name="rdoSuit_05" /> Até R$ 50.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_B" type="radio" value="1" name="rdoSuit_05" /> De R$ 50.000,00 a R$ 300.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_C" type="radio" value="2" name="rdoSuit_05" /> De R$ 300.000,00 a R$ 600.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_05_D" type="radio" value="3" name="rdoSuit_05" /> Acima de R$ 600.000,00</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>6. Quanto você estaria disposto a perder por uma decisão equivocada?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_A" type="radio" value="0" name="rdoSuit_06" /> Nada, não aceitaria perdas</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_B" type="radio" value="1" name="rdoSuit_06" /> Até R$ 5.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_C" type="radio" value="2" name="rdoSuit_06" /> Entre R$ 5.000,00 e 50.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_D" type="radio" value="3" name="rdoSuit_06" /> Entre R$ 50.000,00 e R$ 200.000,00</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_06_E" type="radio" value="4" name="rdoSuit_06" /> Acima de R$ 200.000,00</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>7. Qual o tempo disponível que você tem para manter seu dinheiro aplicado?</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_A" type="radio" value="0" name="rdoSuit_07" /> Menos de 3 meses</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_B" type="radio" value="1" name="rdoSuit_07" /> Entre 3 e 6 meses</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_C" type="radio" value="2" name="rdoSuit_07" /> Entre 6 meses e 1 ano</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_07_D" type="radio" value="3" name="rdoSuit_07" /> Mais de 1 ano</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col1">
                                                <p><strong>8. Informe sobre sua formação acadêmia:</strong></p>
                            
                                                <div class="lista-radio">
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_A" type="radio" value="0" name="rdoSuit_08" /> Nível Superior em Economia ou em áreas relacionadas à Finanças</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_B" type="radio" value="1" name="rdoSuit_08" /> Formação superior em outras áreas</label>
                                                    <label class="radio" style="display:block;"><input  id="rdoSuit_08_C" type="radio" value="2" name="rdoSuit_08" /> Não possui formação superior</label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <p id="pnlBotaoSalvarDadosPasso3" runat="server" class="Continuar" style="padding-top:24px;width:90%">
                                        <button onclick="return GradSite_Cadastro_PFPasso3_Verificar(this)" class="botao btn-padrao btn-erica" style="width:auto; margin-left:16px">Salvar</button>
                                    </p>
                                    
                                    <p id="pnlBotaoAlterarDadosPasso3" runat="server" visible="false" class="row">
                                        <div class="col3" style="margin-left: 312px;">
                                            <button class="botao btn-padrao" onclick="return GradSite_Cadastro_AlterarDadosRedirecionando(this)" type="button">Alterar Dados</button>
                                        </div>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="pnlDadosContratuais" data-IdConteudo="DadosContratuais" runat="server" class="aba" style="display:none">

                        <div id="pnlEscolherAplicacao" runat="server">

                            <div class="row">
                                <div class="col8" style="">
                            
                                    <p>
                                        Você deseja aplicar em:
                                    </p>
                                    <p>
                                        <label>
                                        <input type="radio" name="TipoDeAplicacao" value="1" onclick="return rdoTipoDeAplicacao_Click(this)" checked="checked" /> Bovespa / BM&amp;F
                                        </label>
                                    </p>
                                    <p>
                                        <label>
                                        <input type="radio" name="TipoDeAplicacao" value="2" onclick="return rdoTipoDeAplicacao_Click(this)"  /> Fundos de Investimentos
                                        </label>
                                    </p>
                                    <p>
                                        <label>
                                        <input type="radio" name="TipoDeAplicacao" value="3" onclick="return rdoTipoDeAplicacao_Click(this)" /> Ambos
                                        </label>
                                    </p>

                                </div>
                            </div>

                            <div id="pnlFundoParaAplicar" class="row" style="display:none">
                                <div class="col8" style="padding-bottom:24px">
                                    <select id="cboFundoParaAplicar">
                                    <asp:repeater id="rptFundosParaAplicar" runat="server">
                                    <itemtemplate>
                                    <option value="<%#Eval("IdProduto") %>" data-termo="../..<%# Eval("FullPathTermoPF") %>"><%#Eval("Fundo") %></option>
                                    </itemtemplate>
                                    </asp:repeater>
                                    </select>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col5" style="">
                                <button class="botao btn-padrao" onclick="return btnSalvarDesejaAplicar_Click(this)" type="button">Salvar</button>
                                </div>
                            </div>

                        </div>

                        <div id="pnlContratos" runat="server" style="display:none">
                        
                            <h5>Contratos</h5>

                            <table>
                                <tfoot id="pnlRodapePasso4" runat="server">
                                    <tr>
                                        <td colspan="2">
                                            <input  id="chkCadastro_PFPasso4_ConcordoContratos" type="checkbox" style="width:1em !important; margin:2px 6px 0px 28%; padding:0px; border:none" />
                                            <label for="chkCadastro_PFPasso4_ConcordoContratos" style="font-size:12px">Li, concordo e estou ciente dos termos de contrato acima</label>
                                        </td>
                                    </tr>
                                </tfoot>
                                <tbody>
                                    <tr class="NaoFundos">
                                        <td>Contrato de Intermediação</td>
                                        <td style="text-align:center"><a href="../../Resc/PDFs/Contrato_de_Intermediacao_1501.pdf" target="_blank">Visualizar</a></td>
                                    </tr>
                                    <tr class="SomenteFundos">
                                        <td>Termo de Adesão ao Fundo</td>
                                        <td style="text-align:center"><a id="lnkTermoFundo2" href="../../Resc/PDFs/" target="_blank">Visualizar</a></td>
                                    </tr>
                                    <tr>
                                        <td>Regras de Parâmetros de Atuação</td>
                                        <td style="text-align:center"><a href="../../Resc/PDFs/Regras_e_Parametros_1501.pdf" target="_blank">Visualizar</a></td>
                                    </tr>
                                    <tr>
                                        <td>Código de Ética</td>
                                        <td style="text-align:center"><a href="../../Resc/PDFs/Codigo_de_Conduta_Etica_1501.pdf" target="_blank">Visualizar</a></td>
                                    </tr>
                                </tbody>
                            </table>

                            <p id="pnlBotaoFinalizar" runat="server" class="Continuar" style="padding-top:24px; width:100%; text-align:center height:auto; margin-bottom:0px">
                                <button onclick="return btnCadastro_PFPasso4_Click(this)" class="botao btn-padrao btn-erica" style="padding:4px 12px; width:30%">Finalizar Cadastro</button>
                            </p>

                        </div>

                        <div id="pnlCadastroRealizado" style="display:none">
                            <h3>Seja Bem-Vindo!</h3>
                            <p>
                            A partir de agora, você já pode conhecer as vantagens de ser cliente Gradual. 
                            Envie toda a documentação necessária para a abertura da sua conta e comece a realizar as suas operações!
                            </p>
                            <ul class="investir-passos">
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/printer.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 1</h4>
                                        <h5>Imprima, leia e assine os documentos abaixo.</h5>
                                        <p class="TextoEmDestaque" style="line-height:32px">
                                            <a id="lnkFichaCadastral" href="#" target="_blank">Ficha Cadastral</a><br />
                                            <a id="lnkTermo" href="#" target="_blank">Termo de adesão ao Contrato de Intermediação</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/scanner.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 2</h4>
                                        <h5>Tire uma cópia dos documentos necessários. Para consultar a lista da documentação obrigatória, <a href="../../ContratoeDocumentacao/Default">clique aqui</a>.</h5>
                                    </div>
                                </li>
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/carta.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 3</h4>
                                        <h5>Envie toda a documentação por correio para endereço abaixo ou se, preferir, digitalize-a e envie para: <a href="mailto:cadastrodigital@gradualinvestimentos.com.br">cadastrodigital@gradualinvestimentos.com.br</a>.</h5>

                                        <p>Gradual CCTVM S.A.<br>
                                        A/C Cadastro<br>
                                        Avenida Juscelino Kubitschek, 50 - 5º e 6º andares<br>
                                        Vila Nova Conceição &ndash; São Paulo - SP &ndash; CEP: 04543-011</p>
                                    </div>
                                </li>
                            </ul>
                        </div>


                        <div id="pnlCadastroRealizado_Fundos" style="display:none">
                            <h3>Seja Bem-Vindo!</h3>
                            <p>
                            A partir de agora, você já pode conhecer as vantagens de ser cliente Gradual. 
                            Envie toda a documentação necessária para a abertura da sua conta e comece a realizar as suas operações!
                            </p>
                            <ul class="investir-passos">
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/printer.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 1</h4>
                                        <h5>Imprima, leia e assine os documentos abaixo.</h5>
                                        <p class="TextoEmDestaque" style="line-height:32px">
                                            <a id="lnkFichaCadastral_Fundo" href="#" target="_blank">Ficha Cadastral</a><br />
                                            <a id="lnkTermoFundo" href="/Resc/PDFs/AdesaoFundos/" target="_blank">Termo de adesão ao Fundo</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/scanner.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 2</h4>
                                        <h5>Tire uma cópia dos documentos necessários. Para consultar a lista da documentação obrigatória, <a href="../../ContratoeDocumentacao/Default">clique aqui</a>.</h5>
                                    </div>
                                </li>
                                <li class="clear">
                                    <div class="iconer">
                                        <img src="../../Resc/Skin/Default/Img/carta.png">
                                    </div>
                                    <div class="investir-passos-texto">
                                        <h4>Passo 3</h4>
                                        <h5>Envie toda a documentação por correio para endereço abaixo ou se, preferir, digitalize-a e envie para: <a href="mailto:cadastrodigital@gradualinvestimentos.com.br">cadastrodigital@gradualinvestimentos.com.br</a>.</h5>

                                        <p>Gradual CCTVM S.A.<br>
                                        A/C Cadastro<br>
                                        Avenida Juscelino Kubitschek, 50 - 5º e 6º andares<br>
                                        Vila Nova Conceição &ndash; São Paulo - SP &ndash; CEP: 04543-011</p>
                                    </div>
                                </li>
                            </ul>
                        </div>



                    </div>

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

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Meu Cadastro" />

</asp:content>