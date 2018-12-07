<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssinaturaEletronica.aspx.cs" Inherits="Gradual.Site.Www.Integracao.AssinaturaEletronica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
            <div class="form-padrao" style="padding-top:40px">

                <div class="row">
                    <div class="col1">
                        <div class="menu-exportar clear">
                            <h5>Alterar Assinatura Eletrônica</h5>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Assinatura Eletrônica Atual:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaAtual" type="password" class="mostrar-teclado" />
                            <button class="teclado-virtual" type="button"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>

                    <div class="col2">
                        <div class="campo-basico campo-senha" style="padding-top:22px">
                            <a href="EsqueciAssinatura.aspx">Esqueci minha assinatura</a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Nova Assinatura Eletrônica:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaNova" type="password" class="mostrar-teclado" />
                        </div>
                    </div>

                    <div class="col2">
                        <div class="campo-basico campo-senha campo-longo-teclado">
                            <label>Confirmar Nova Assinatura Eletrônica:</label>
                            <input id="txtCadastro_PFPasso4_AssinaturaNovaC" type="password" class="mostrar-teclado" />
                            <button class="teclado-virtual" type="button"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col6">
                        <a class="botao btn-padrao" href="#" onclick="return btnCadastro_PFPasso4_AlterarAssinatura_Click(this)" style="width:220px">Alterar Assinatura</a>
                    </div>
                </div>

            </div>

        </div>
    </form>
</body>
</html>
