<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaInterna.Master" AutoEventWireup="true" CodeBehind="Ouvidoria.aspx.cs" Inherits="Gradual.Site.Www.Ouvidoria" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="PaginaConteudo">

    <h2>Ouvidoria</h2>

    <div class="row">
        <div class="col1">

            <p>
                A Ouvidoria é a última instância para solicitações de clientes.
            </p>
            <p>
                Dessa forma, você só deverá utilizar esse canal caso já tenha entrado em 
                contato com a Central de Atendimento da Gradual por e-mail, chat ou 
                telefone e ainda não tenha solucionado sua reclamação.
            </p>
            <p>
                Lembre-se: tenha o número do protocolo da sua reclamação na Central de Atendimento antes de acionar a Ouvidoria.
            </p>

            <p>
                <strong>Telefone: 0800 655 1466</strong><br />
                E-mail: <a href="mailto:ouvidoria@gradualinvestimentos.com.br">ouvidoria@gradualinvestimentos.com.br</a>.<br />
                Disponível nos dias úteis, das 9h às 18h.
            </p>

            <p><strong>Já entrou em contato com a Central de Atendimento?</strong></p>
            
            &nbsp;&nbsp;&nbsp;
            <input  id="rdoContatoAtendimento_Sim" type="radio" name="rdoContatoAtendimento" onclick="return rdoContatoAtendimento_Click(this)" />
            <label for="rdoContatoAtendimento_Sim">Sim</label>

            &nbsp;&nbsp;&nbsp;
            <input  id="rdoContatoAtendimento_Nao" type="radio" name="rdoContatoAtendimento" onclick="return rdoContatoAtendimento_Click(this)" />
            <label for="rdoContatoAtendimento_Nao">Não</label>
        </div>
    </div>

    <div id="frmOuvidoria" class="form-padrao" style="display:none;padding-top:2em">
        <div class="row">
            <div class="col2">
                <div class="campo-basico form-obrigatorio">
                    <label>Nome completo:</label>
                    <input  id="txtOuvidora_Nome" type="text" runat="server" class="validate[required]" />
                </div>
            </div>

            <div class="col2">
                <div class="campo-basico form-obrigatorio">
                    <label>Código do Cliente:</label>
                    <input  id="txtOuvidora_CodigoCliente" type="text" runat="server" class="validate[required] ProibirLetras" maxlength="20" />
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col2">
                <div class="campo-basico form-obrigatorio">
                    <label>CPF:</label>
                    <input  id="txtOuvidora_CpfCnpj" type="text" runat="server" class="validate[custom[validatecpf]] Mascara_CPF ProibirLetras EstiloCampoObrigatorio" />
                </div>
            </div>
                        
            <div class="col2">
                <div class="campo-basico form-obrigatorio">
                    <label>E-mail:</label>
                    <input  id="txtOuvidora_Email" type="text" maxlength="80" class="validate[required,custom[Email]]" runat="server" />
                </div>
            </div>
        </div>
                    
        <div class="row">
            <div class="col2">
                <div class="campo-basico">
                    <label>Assessor:</label>
                    <input  id="txtOuvidora_Assessor" type="text" runat="server" class="validate[required]" />
                </div>
            </div>

            <div class="col2">
                <div class="campo-basico">
                    <label>*N° do Protocolo de Atendimento:</label>
                    <input  id="txtOuvidora_Protocolo" type="text" runat="server" class="validate[required]" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col1">
                <div class="row">
                    <div class="col4">
                        <div class="campo-basico">
                            <label>Telefone:</label>
                            <div class="clear">
                                <input  id="txtOuvidora_Telefone_DDD"     runat="server" type="text" title="DDD"  maxlength="2" class="input-ddd validate[required,custom[onlyNumber],custom[fixedLength]] ProibirLetras " style=""  />
                                <input  id="txtOuvidora_Telefone_Numero"  runat="server" type="text" title="Número de Telefone"  maxlength="10" class="input-telefone validate[custom[requiredWithMask]] MesmaLinha ProibirLetras" style="" />
                            </div>
                        </div>
                    </div>

                    <div class="col4">
                        <div class="campo-basico">
                            <label>Cidade:</label>
                            <input  id="txtOuvidora_Cidade" type="text" runat="server" class="validate[required]" />
                        </div>
                    </div>

                    <div class="col4">
                        <div class="campo-basico">
                            <label>Estado:</label>

                            <asp:dropdownlist id="cboOuvidora_Estado" runat="server" type="select-one" cssclass="validate[required]" datatextfield="Value" datavaluefield="Id">
                            </asp:dropdownlist>
                        </div>
                    </div>

                    <div class="col4">
                        <div class="campo-basico">
                            <label>País:</label>

                            <asp:dropdownlist id="cboOuvidora_Pais" runat="server" type="select-one" cssclass="EstiloCampoObrigatorio validate[required]" datatextfield="Value" datavaluefield="Id" >
                            </asp:dropdownlist>

                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="row">
            <div class="col6">
                <label>Mensagem:</label>
                <textarea id="txtOuvidora_Mensagem" type="text" runat="server" class="validate[required]" style="width:34em; height:10em"></textarea>
            </div>
        </div>

        <div class="row">
            <div class="col3-2">
                <p class="red">Todos os campos são obrigatórios</p>
            </div>
        </div>
                    
        <div class="row">
            <div class="col1">

                <asp:button id="btnOuvidoria_Enviar" onclientclick="return btnOuvidoria_Enviar_Click()" onclick="btnOuvidoria_Enviar_Click" runat="server" cssclass="botao btn-padrao btn-erica" text="Enviar >" />

            </div>
        </div>
    </div>
    
    <script language="javascript">

    function btnOuvidoria_Enviar_Click()
    {
        var lRetorno = GradSite_ValidacaoComDoubleCheck($("section.PaginaConteudo"));

        return lRetorno;
    }

    function rdoContatoAtendimento_Click(pSender)
    {
        if(  $("#rdoContatoAtendimento_Sim").is(":checked")  )
        {
            $("#frmOuvidoria").show();
        }
        else
        {
            $("#frmOuvidoria").hide();

            GradSite_ExibirMensagem("I", "Antes de entrar em contato com a Ouvidoria, registre sua reclamação na Central de Atendimento.", false, null);
        }
    }

    </script>


</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Ouvidoria" />

</asp:Content>
