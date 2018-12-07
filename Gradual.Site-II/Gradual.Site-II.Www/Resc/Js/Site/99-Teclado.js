var field = '#txtGradSite_Login_Senha'; //nome do field que receberá os caracteres do teclado. Normalmente um field input type="password"
var array = [];
var arrayRef = []
var ref = "";
var refNova = "";
var refConfirmacao = "";
var clickRef;
var controlClick = 0;
var activeControl;
var ativarTecladoValidationNovoCadastro = false;

function init() {
    Rearrange();

    var keys = $("#virtualKeys")[0].getElementsByTagName("button"); //pego todos os elementos link dentro do div keys
    var i;
}

function getValidationKeyBoard() 
{
    var validationKeyBoard = $("#ValidationKeyboard");
    return validationKeyBoard;
}

function getEntryKeyBoard() 
{
    var entryKeyBoard = $("#EntryKeyboard");
    return entryKeyBoard;
}

function getValidationKeys() {
    var validationKeys = $("#ValidationKeyboard")[0].getElementsByTagName("button");
    return validationKeys;
}

function getEntryKeys() {
    var entryKeys = $("#EntryKeyboard")[0].getElementsByTagName("button");
    return entryKeys;
}

function getPasswordContainer() {
    var passwordContainer = $("#passwordContainer");
    return passwordContainer;
}

function Rearrange() 
{
    var keys = getValidationKeys();

    var i;
    array = [];
    for (i = 0; i < keys.length; i++) {
        var num1 = Randomize();
        var num2 = Randomize();

        keys[i].firstChild.value = num1 + "|" + num2; //atribuo ao link o valor sorteado
        keys[i].firstChild.data = num1 + " ou " + num2;
    }

}

function Randomize() {

    var Continue = true;
    var sort;

    while (Continue) {
        sort = Math.random(); //sort recebe um número entre 0 e 1, sorteado randomicamente. Isso faz a cada reload os números aparecerem numa ordem

        if ((sort >= 0) && (sort < 0.1)) //de acordo com o valor de sort, atribuo um inteiro a ele
        {
            sort = 0;
        }
        else if ((sort >= 0.1) && (sort < 0.2)) {
            sort = 1;
        }
        else if ((sort >= 0.2) && (sort < 0.3)) {
            sort = 2;
        }
        else if ((sort >= 0.3) && (sort < 0.4)) {
            sort = 3;
        }
        else if ((sort >= 0.4) && (sort < 0.5)) {
            sort = 4;
        }
        else if ((sort >= 0.5) && (sort < 0.6)) {
            sort = 5;
        }
        else if ((sort >= 0.6) && (sort < 0.7)) {
            sort = 6;
        }
        else if ((sort >= 0.7) && (sort < 0.8)) {
            sort = 7;
        }
        else if ((sort >= 0.8) && (sort < 0.9)) {
            sort = 8;
        }
        else {
            sort = 9;
        }

        if (array.indexOf(sort) < 0) {
            array[array.length] = sort;
            Continue = false;
        }
    }

    return sort;
}

function Key_Click(sender, Event, Param)
{
    Event.preventDefault();

    //Percorre rotinas de entrada apenas caso não seja o teclado de login
    if (!ativarTecladoValidationNovoCadastro)
    {
        if ($(sender).attr("id").indexOf("Seguranca") >= 0)
        {
            if (activeControl.attr("id").indexOf("Atual") <= 0)
            {
                if (controlClick >= 6)
                {
                    GradSite_ExibirMensagem("I", "O campo deve possuir 6 caracteres numéricos!");
                    return;
                }
            }
        }

        if ($(sender).attr("id").indexOf("Seguranca") >= 0)
        {
            if (activeControl.length > 0)
            {
                //activeControl.val(activeControl.val() + Param);
                switch (activeControl.attr("id")) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
                {
                    case "txtCadastro_PFPasso4_SenhaAtual":
                        {
                            activeControl.val(activeControl.val() + "●");
                            ref = ref + Param;
                            controlClick++;
                            break;
                        }
                    case "txtCadastro_PFPasso4_SenhaNovaC":
                        {
                            activeControl.val(activeControl.val() + "●");
                            refConfirmacao = refConfirmacao + Param;
                            controlClick++;
                            break;
                        }
                    case "txtCadastro_PFPasso4_SenhaNova":
                        {
                            activeControl.val(activeControl.val() + "●");
                            refNova = refNova + Param;
                            controlClick++;
                            break;
                        }
                    case "txtCadastro_PFPasso4_AssinaturaAtual":
                        {
                            activeControl.val(activeControl.val() + "●");
                            ref = ref + Param;
                            controlClick++;
                            break;
                        }
                    case "txtCadastro_PFPasso4_AssinaturaNova":
                        {
                            activeControl.val(activeControl.val() + "●");
                            refNova = refNova + Param;
                            controlClick++;
                            break;
                        }
                    case "txtCadastro_PFPasso4_AssinaturaNovaC":
                        {
                            activeControl.val(activeControl.val() + "●");
                            refConfirmacao = refConfirmacao + Param;
                            controlClick++;
                            break;
                        }
                    default:
                        {
                            activeControl.val(activeControl.val() + Param);
                            arrayRef = arrayRef + Param;
                            controlClick++;
                            break;
                        }
                }
            }
        }
    }

    if ($(sender).attr("id").indexOf("Seguranca") == -1) 
    {
        if ($(field).val().length == 6)
            return;

        var values = clickRef.split("|");

        arrayRef[controlClick] = [values[0], values[1]];
        controlClick++;
        $(field).val($(field).val() + Param);
        //Rearrange();
    }

    var keys = getValidationKeys();
    lastHoverId = Param;
    var key = keys[lastHoverId];
    key.onmouseover(sender, Event, Param);
    return false;
}

var lastHoverId;
function Key_MouseOver(Sender, Event, Param) {
    //Event.preventDefault();

    var keys = getValidationKeys();
    var i;
    lastHoverId = Param;
    clickRef = keys[Param].firstChild.value;

//    for (i = 0; i < keys.length; i++) {
//        keys[i].firstChild.data = "";
//    }

    return false;
}

function Key_MouseOut(Sender, event, Param) 
{
    event.preventDefault();

    var control = -1;
    var keys = getValidationKeys();
    var i;

    clickRef = keys[Param].value;

//    for (i = 0; i < keys.length; i++) 
//    {
//        control++;
//        var num1 = array[control];
//        control++;
//        var num2 = array[control];
//        keys[i].firstChild.data = num1 + ' ou ' + num2;
//    }

    return false;
}

function Key_Clear(sender, event) 
{
    event.preventDefault();
    $("#lblNovoCadastro_MsgLogin").text("");

    //if ($(sender).attr('id').indexOf('Seguranca') >= 0) 
    if (activeControl != undefined && activeControl != null)
    {
        arrayRef = [];
        controlClick = 0;
        activeControl.val("");

        switch (activeControl.attr("id")) // Tipos de teclado -> 0:QWERTY | 1:DINAMICO | 2:DINAMICO_SENHA | 3:DINAMICO_ASSINATURA
        {
            case "txtCadastro_PFPasso4_SenhaAtual":
                {
                    ref = "";
                    controlClick = 0;
                    break;
                }
            case "txtCadastro_PFPasso4_SenhaNovaC":
                {
                    refConfirmacao = "";
                    controlClick = 0;
                    break;
                }
            case "txtCadastro_PFPasso4_SenhaNova":
                {
                    refNova = "";
                    controlClick = 0;
                    break;
                }
            case "txtCadastro_PFPasso4_AssinaturaAtual":
                {
                    ref = "";
                    controlClick = 0;
                    break;
                }
            case "txtCadastro_PFPasso4_AssinaturaNova":
                {
                    refNova = "";
                    controlClick = 0;
                    break;
                }
            case "txtCadastro_PFPasso4_AssinaturaNovaC":
                {
                    refConfirmacao = "";
                    controlClick = 0;
                    break;
                }
        }
    }
    else 
    {
        arrayRef = [];
        $(field).val("");
        controlClick = 0;
    }
}

function showKeyboard(sender, event, param) 
{
    event.preventDefault();

    VerificarTeclado(sender, param);

    return false;
}

$(document).ready(function ()
{
    $('#passwordPanel').modal({ show: false })
    GradSite_AtivarInputs('section.PaginaConteudo');
    init();
});
