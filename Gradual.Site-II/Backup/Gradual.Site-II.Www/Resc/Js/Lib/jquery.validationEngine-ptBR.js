
(function($) {
    $.fn.validationEngineLanguage = function() { };
    $.validationEngineLanguage =
    {
        newLang: function()
        {
            $.validationEngineLanguage.allRules = {
                    "required":{ // Add your regex rules here, you can take telephone as an example
                        "regex":"none",
                        "alertText":"Campo obrigat&oacute;rio",
                        "alertTextCheckboxMultiple":"Favor selecionar uma op&ccedil;&atilde;o",
                        "alertTextCheckboxe":"Campo obrigat&oacute;rio"},
                    "length":{
                        "regex":"none",
                        "alertText":"Deve ter de ",
                        "alertText2":" a ",
                        "alertText3": " caracteres"},
                    "maxCheckbox":{
                        "regex":"none",
                        "alertText":"N&uacute;mero m&aacute;ximo de op&ccedil;&otilde;es escolhidas"},
                    "equals": {
                        "regex": "none",
                        "alertText": "Os campos n&atilde;o correspondem"},
                    "minCheckbox":{
                        "regex":"none",
                        "alertText":"Por favor escolha ",
                        "alertText2":" op&ccedil;&otilde;es"},    
                    "confirm":{
                        "regex":"none",
                        "alertText":"O campo de confirma&ccedil;&atilde;o n&atilde;o est&aacute; igual"},
                    "telefone":{
                        "regex":/[0-9]{4}-[0-9]{4}/,
                        "alertText":"Telefone inv&aacute;lido; formato: 9999-9999"},    
                    "cep":{
                        "regex":/[0-9]{5}-[0-9]{3}/,
                        "alertText":"CEP inv&aacute;lido; formato: 99999-999"},    
                    "cpf":{
                        "regex":/\d{3}\.\d{3}\.\d{3}-\d{2}/,
                        "alertText":"CPF inv&aacute;lido; formato: 999.999.999-99"},    
                    "cnpj":{
                        "regex":/\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}/,
                        "alertText":"CNPJ inv&aacute;lido; formato: 99.999.999/9999-99"},    
                        //ATENÇÃO: Quando fizer o mod11, deixar passar cpfs que começem com 99000 ou 88000
                    
                    //CPF e CNPJ
                    "validatecpfcnpj":{
                        "func"      : function(caller) { return !validatecpfcnpj(caller); },
                        "alertText" : "CPF ou CNPJ inv&aacute;lido!"},
                        
                    //Só CPF
                    "validatecpf":{
                        "func"      : function(caller) { return !validatecpf(caller); },
                        "alertText" : "CPF inv&aacute;lido!"},
                    
                    //Campos obrigatórios que têm máscara de números, porque o "vazio" _____-_____ não pega no required
                    "requiredWithMask":{
                        "func"      : function(caller) { return !validateRequiredWithMask(caller); },
                        "alertText" : "Campo obrigat&oacute;rio"},
                    
                    //Campos com tamanho fixo
                    "fixedLength":{
                        "func"      : function(caller) { return validateFixedLength(caller); },
                        "alertText" : "Deve ter mais caracteres"},
                    
                    //VALIDA Somente CNPJ
                    "validatecnpj":{
                        "func"      : function(caller) { return validatecnpj(caller); },
                        "alertText" : "CNPJ inv&aacute;lido!"},
                        
                    //VALIDA Datas maiores
                    "CompararDatas":{
                        "func"      : function(caller) { return CompararDatas(caller); },
                        "alertText" : "Data inv&aacute;lida"},

                    //VALIDA Datas tem que estar no passado
                    "DataNoPassado":{
                        "func"      : function(caller) { return VerificarDataNoPassado(caller); },
                        "alertText" : "Data inv&aacute;lida, deve estar no passado"},

                    //VALIDA data nascimento / expedicao
                    "CompararDatas_nascimento_expedicao":{
                        "func"      : function(caller) { return CompararDatas(caller); },
                        "alertText" : "Data de expedi&ccedil;&atilde;o deve ser maior do que a data de nascimento."},
                        
                    "ValorLimiteBovespa":{
                        "func"      : function(caller) { return VerificarValorLimiteBovespa(caller); },
                        "alertText" : "Valor inv&aacute;lido"},
                        
                    "DataLimiteBovespa":{
                        "func"      : function(caller) { return VerificarDataLimiteBovespa(caller); },
                        "alertText" : "A data digitada deve ser maior que a &uacute;ltima data de vencimento cadastrada"},
                        
                    "CampoIgualAnterior":{
                        "func"      : function(caller) { return VerificarCampoIgualAnterior(caller); },
                        "alertText" : "Valor n&atilde;o confere"},
                        
                    "ObrigatorioSeNaoValor":{
                        "func"      : function(caller) { return VerificarObrigatorioSeNaoValor(caller); },
                        "alertText" : "Campo obrigat&oacute;rio"},

                    "EmailGambizento":{
                        "func"      : function(caller) { return VerificarEmailGambizendoIgnorante(caller); },
                        "alertText" : "Email inv&aacute;lido"},

                    "Email":{
                        "regex":/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.(([a-z]{2,3})|(aero|coop|info|museum|name))$/,
                        //"regex":"/^[a-zA-Z0-9_\.\-]+\@([a-zA-Z0-9\-]+\.)+[a-zA-Z0-9]{2,4}$/",
                        "alertText":"Email inv&aacute;lido"},    
                    "date":{
                         "regex":/^[0-9]{4}\-\[0-9]{1,2}\-\[0-9]{1,2}$/,
                         "alertText":"* Invalid date, must be in YYYY-MM-DD format"},
                    "data":{
                         "func": function(caller) { return VerificarDataBrasileira(caller); },
                         "alertText":"Data inv&aacute;lida"},
                    "onlyNumber":{
                        "regex": /^[0-9\ ]+$/,
                        "alertText":"Somente n&uacute;meros"},    
                    "numeroFormatado":{
                        "regex": /[\d\.,]{1,20}/,                    //TODO: Precisa melhorar muuuito
                        "alertText":"N&uacute;mero inv&aacute;lido"},    
                    "noSpecialCaracters":{
                        "regex":/^[0-9a-zA-Z]+$/,
                        "alertText":"N&atilde;o s&atilde;o permitidos caracteres especiais"},    
                    "ajaxUser":{
                        "file":"validateUser.php",
                        "extraData":"name=eric",
                        "alertTextOk":"* This user is available",    
                        "alertTextLoad":"* Loading, please wait",
                        "alertText":"* This user is already taken"},    
                    "ajaxName":{
                        "file":"validateUser.php",
                        "alertText":"* This name is already taken",
                        "alertTextOk":"* This name is available",    
                        "alertTextLoad":"* Loading, please wait"},        
                    "onlyLetter":{
                        "regex": /^[a-zA-Z\ \']+$/,
                        "alertText":"Somente letras"},
                    "validate2fields":{
                        "func" : function(caller) { return validate2fields(caller); },
                        "alertText":"* You must have a firstname and a lastname"}    
                    };
            
        }
    };

    $.validationEngineLanguage.newLang();
    
})(jQuery);

///Valida CPF e CNPJ
function validatecpfcnpj(caller)
{
    //var the
    var theCPF = $(caller).val();
        theCPF = theCPF.replace('.','','gi');
        theCPF = theCPF.replace('.','','gi');
        theCPF = theCPF.replace('-','','gi');
        theCPF = theCPF.replace('/','','gi');

    var checkStr = $(caller).val();
        checkStr = checkStr.replace('.','','gi');
        checkStr = checkStr.replace('.','','gi');
        checkStr = checkStr.replace('-','','gi');
        checkStr = checkStr.replace('/','','gi');

    var checkOK  = "0123456789"; 
    var allValid = true;
    var allNum = "";

     if (theCPF == "")
        return true;

    if (((theCPF.length == 11)   && (theCPF == 11111111111)
    ||   (theCPF == 22222222222) || (theCPF == 33333333333)
    ||   (theCPF == 44444444444) || (theCPF == 55555555555)
    ||   (theCPF == 66666666666) || (theCPF == 77777777777)
    ||   (theCPF == 88888888888) || (theCPF == 99999999999)
    ||   (theCPF == 00000000000)))
        return true;
    
    ///Verifica se o cpf/cnpj começa com  88000 e 99000 para cliente não residente
    if (theCPF.substring(0, 5) == '88000' || theCPF.substring(0, 5) == '99000')
        return false;

    if (!((checkStr.length == 11) || (checkStr.length == 14)))
        return true;
    
    for (i = 0;  i < checkStr.length;  i++)
    {
        ch = checkStr.charAt(i);
        for (j = 0;  j < checkOK.length;  j++)
            if (ch == checkOK.charAt(j))
                break;
        if (j == checkOK.length)
        {
            allValid = false;
            break;
        }
        allNum += ch;
    }

    if (!allValid)
        return true;
    
    var chkVal = allNum;
    var prsVal = parseFloat(allNum);
    if (chkVal != "" && !(prsVal > "0"))
        return true;

    if (theCPF.length == 11)
    {
        var tot = 0;

        for (i = 2;  i <= 10;  i++)
            tot += i * parseInt(checkStr.charAt(10 - i));

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(9)))
            return true;

        tot = 0;
  
        for (i = 2;  i <= 11;  i++)
            tot += i * parseInt(checkStr.charAt(11 - i));

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(10)))
            return true;
    }
    else
    {
        var tot  = 0;
        var peso = 2;
  
        for (i = 0;  i <= 11;  i++)
        {
            tot += peso * parseInt(checkStr.charAt(11 - i));
            peso++;
            if (peso == 10)
                peso = 2;
        }

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(12)))
            return true;
    
        tot  = 0;
        peso = 2;
  
        for (i = 0;  i <= 12;  i++)
        {
            tot += peso * parseInt(checkStr.charAt(12 - i));
            peso++;
            if (peso == 10)
                peso = 2;
        }

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(13)))
            return true;
  }

  return false;
}

///Valida somente CPF
function validatecpf(caller)
{
    //var the
    var theCPF   = $(caller).val().replace('.','','gi').replace('.','','gi').replace('.','','gi').replace('-','','gi').replace('_','','gi').replace('/','','gi');
    var checkStr = $(caller).val().replace('.','','gi').replace('.','','gi').replace('.','','gi').replace('-','','gi').replace('_','','gi').replace('/','','gi');
    var checkOK  = "0123456789"; 
    var allValid = true;
    var allNum = "";

     if (theCPF == "")
        return false;

     if (theCPF == "____________")
        return false;

     if (theCPF == "___________")
        return false;

     if (theCPF == "__________")
        return false;

     if (theCPF == "_________")
        return false;

    if (((theCPF.length == 11)   && (theCPF == 11111111111)
    ||   (theCPF == 22222222222) || (theCPF == 33333333333)
    ||   (theCPF == 44444444444) || (theCPF == 55555555555)
    ||   (theCPF == 66666666666) || (theCPF == 77777777777)
    ||   (theCPF == 88888888888) || (theCPF == 99999999999)
    ||   (theCPF == 00000000000)))
        return true;
    
    ///Verifica se o cpf/cnpj começa com  88000 e 99000 para cliente não residente
    if (theCPF.substring(0, 5) == '88000' || theCPF.substring(0, 5) == '99000')
        return false;

//    if (!((checkStr.length == 11) || (checkStr.length == 14)))
//        return true;
   
    for (i = 0;  i < checkStr.length;  i++)
    {
        ch = checkStr.charAt(i);
        for (j = 0;  j < checkOK.length;  j++)
            if (ch == checkOK.charAt(j))
                break;
        if (j == checkOK.length)
        {
            allValid = false;
            break;
        }
        allNum += ch;
    }

    if (!allValid)
        return true;
    
    var chkVal = allNum;
    var prsVal = parseFloat(allNum);

    if (chkVal != "" && !(prsVal > "0"))
        return true;

    
    var tot = 0;

    for (i = 2;  i <= 10;  i++)
        tot += i * parseInt(checkStr.charAt(10 - i));

    if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(9)))
        return true;

    tot = 0;
  
    for (i = 2;  i <= 11;  i++)
        tot += i * parseInt(checkStr.charAt(11 - i));

    if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(10)))
        return true;

    return false;
}    

//Valida somente CNPJ
function validatecnpj(caller)
{
    //var the
    var theCPF   = $(caller).val().replace(/\./gi, "").replace(/-/gi, "").replace("/", "", "gi");
    var checkStr = $(caller).val().replace(/\./gi, "").replace(/-/gi, "").replace("/", "", "gi");

    var checkOK  = "0123456789"; 
    var allValid = true;
    var allNum   = "";

     if (theCPF == "")
        return true;

    if (((theCPF.length == 11)   && (theCPF == 11111111111)
    ||   (theCPF == 22222222222) || (theCPF == 33333333333)
    ||   (theCPF == 44444444444) || (theCPF == 55555555555)
    ||   (theCPF == 66666666666) || (theCPF == 77777777777)
    ||   (theCPF == 88888888888) || (theCPF == 99999999999)
    ||   (theCPF == 00000000000)))
        return true;
    
    ///Verifica se o cpf/cnpj começa com  88000 e 99000 para cliente não residente
    if (theCPF.substring(0, 5) == '88000' || theCPF.substring(0, 5) == '99000')
        return true;

    if (!((checkStr.length == 11) || (checkStr.length == 14)))
        return false;
   

    for (i = 0;  i < checkStr.length;  i++)
    {
        ch = checkStr.charAt(i);
        for (j = 0;  j < checkOK.length;  j++)
            if (ch == checkOK.charAt(j))
                break;
        if (j == checkOK.length)
        {
            allValid = false;
            break;
        }
        allNum += ch;
    }

    if (!allValid)
        return false;
    
    var chkVal = allNum;
    var prsVal = parseFloat(allNum);
    if (chkVal != "" && !(prsVal > "0"))
        return false;

    var tot  = 0;
    var peso = 2;
  
    for (i = 0;  i <= 11;  i++)
    {
        tot += peso * parseInt(checkStr.charAt(11 - i));
        peso++;
        if (peso == 10)
            peso = 2;
    }

    if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(12)))
        return false;
    
    tot  = 0;
    peso = 2;
  
    for (i = 0;  i <= 12;  i++)
    {
        tot += peso * parseInt(checkStr.charAt(12 - i));
        peso++;
        if (peso == 10)
            peso = 2;
    }

    if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(13)))
        return false;
 
  return true;
}

function validateRequiredWithMask(caller)
{
    var lValor = caller[0].value;

    lValor = lValor.replace("_", "", "gi").replace("-", "", "gi");

    return (lValor == "");
}

function validateFixedLength(caller)
{
    var lValor = caller[0].value;

    var lMax = caller[0].getAttribute("maxlength");

    return (lValor.length == lMax);
}

function CompararDatas(caller, rules)
{
    var lIdDoOutroCampo = "txtClientes_DadosCompletos_Documento_DataNascimento";
    var lDataInicial    = ConversaoParaData($("#" + lIdDoOutroCampo).val());
    var lDataFinal      = ConversaoParaData($(caller).val());
    
    if (lDataInicial != null && lDataFinal != null)
    {
        if (lDataInicial < lDataFinal)
        {
            return false;
        }
    }
    
    return true;
}

function VerificarDataNoPassado(caller, rules)
{
    if(!VerificarDataBrasileira(caller))
        return false;

    var lDataInicial    = ConversaoParaData($(caller).val());
    var lDataFinal      = ConversaoParaData( GradSite_DataDeHoje() );
    
    if (lDataInicial != null && lDataFinal != null)
    {
        if (lDataInicial > lDataFinal)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    return false;
}

function VerificarDataBrasileira(caller)
{
    var lReg = /(0[1-9]|[12][0-9]|3[01])[\/](0[1-9]|1[012])[\/](19|20)\d\d/;

    return lReg.test(caller.val());
}

function ConversaoParaData(pData)
{
    if(pData && pData.indexOf("/") != -1)
    {
        var lArrayData = pData.split("/");

        return new Date(lArrayData[2], lArrayData[1] - 1, lArrayData[0]);
    }
    else
    {
        return null;
    }
}

function VerificarCampoIgualAnterior(caller)
{
    caller = $(caller);

    var lValorAtual = caller.val();
    
    var lConferirCom = caller.attr("data-ConferirCom");

    if(lConferirCom != "")
    {
        lConferirCom = $("#" + lConferirCom);
    }

    var lValorAnterior = lConferirCom.val();
    
    return (lValorAtual == lValorAnterior);
}

function VerificarObrigatorioSeNaoValor(caller, rules)
{
    // campo é obrigatorio se o valor de rules[0] não for rules[1]

    caller = $(caller);

    if(caller.val() == "")
    {
        var lOutroCampo = $("#" + rules[2]);

        return (lOutroCampo.val() != rules[3]);
    }
    
    return false;
}

function VerificarEmailGambizendoIgnorante(caller)
{
    var lRegex = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.(([a-z]{2,3})|(aero|coop|info|museum|name))$/;

    var lValue = caller[0].value.toLowerCase();

    var lArrayIgnorante = [];

    lArrayIgnorante.push("a@a.com");
    lArrayIgnorante.push("a@aa.com");
    lArrayIgnorante.push("a@a.com.br");
    lArrayIgnorante.push("a@aa.com.br");

    for(var a = 0; a < lArrayIgnorante.length; a++)
    {
        if(lArrayIgnorante[a] == lValue.toLowerCase())
        {
            return false;
        }
    }

    return lRegex.test(lValue);
}

function Validacao_HabilitarMascaraNumerica(pCampos, pMascara)
{
    pCampos
        .bind("keydown", Validacao_SomenteNumeros_OnKeyDown)
        .unmask()
        .mask(pMascara);
}

function Validacao_HabilitarSomenteNumeros(pCampos)
{
    pCampos.bind("keydown", Validacao_SomenteNumeros_OnKeyDown)
}

function Validacao_HabilitarSomenteNumerosEoX(pCampos)
{
    pCampos.bind("keydown", Validacao_SomenteNumerosEoX_OnKeyDown)
}

function Validacao_HabilitarSomenteNumerosComFormatacao(pCampos)
{
    pCampos
        .bind("keydown", Validacao_SomenteNumeros_OnKeyDown)
        .bind("keyup",   Validacao_NumerosFormatados_OnKeyUp);
}

function Validacao_HabilitarDataComFormatacao(pCampos)
{
    pCampos
        .bind("keydown", Validacao_DataFormatada_OnKeyDown)
        .bind("keyup",   Validacao_DataFormatada_OnKeyUp);
}

function Validacao_SomenteNumeros_OnKeyDown(evt)
{
    var key = evt.keyCode;
    var val = evt.target.value;

    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad               numpad -      regular -          x    
    //console.log("Validacao_SomenteNumeros_OnKeyDown(" + key + ")");
    if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106) || key == 109 || key == 173 || key == 88)
        { return true;}
    else
        { if(evt.preventDefault)evt.preventDefault();    return false;}
}

function Validacao_SomenteNumerosEoX_OnKeyDown(evt)
{
    var key = evt.keyCode;
    var val = evt.target.value;

    //alert(key);
    //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad               numpad -      regular -
    if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106) || key == 109 || key == 173)
        {return true;}
    else
        {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
}

function Validacao_NumerosFormatados_OnKeyUp(evt)
{
    var v = true;
    var rgx = /\d*/;

    var evObj = (navigator.userAgent.indexOf("MSIE") != -1) ? evt.srcElement : evt.currentTarget;

    //se nao tiver nada, deixa quieto. se for obrigatorio,
    //vai cair na validacao de obrigatorio. se nao for, ok.
    if (evObj.value != "")
    {
        //para o match dos digitos, ignora ponto e virgula
        var nvalue = evObj.value.replace(/\./gi, "").replace(/,/gi, "");

        //tira zeros à esquerda
        while(nvalue.substr(0, 1) == '0')
            nvalue = nvalue.substr(1);

        var s = nvalue.match(rgx);
        v = (s[0].length == nvalue.length);

        if(v == true)
        {
            if(nvalue.length < 2) nvalue = "0" + nvalue;
            if(nvalue.length < 3) nvalue = "0" + nvalue;

            nvalue = strInsert(nvalue, nvalue.length - 2, ",");

            if(nvalue.length >  6) nvalue = strInsert(nvalue, nvalue.length -  6, ".");
            if(nvalue.length > 10) nvalue = strInsert(nvalue, nvalue.length - 10, ".");
            if(nvalue.length > 14) nvalue = strInsert(nvalue, nvalue.length - 14, ".");
            if(nvalue.length > 18) nvalue = strInsert(nvalue, nvalue.length - 18, ".");
            if(nvalue.length > 22) nvalue = strInsert(nvalue, nvalue.length - 22, ".");
            if(nvalue.length > 26) nvalue = strInsert(nvalue, nvalue.length - 26, ".");

            evObj.value = nvalue;
        }
        else
        {
            //aqui falhou por não ser numérico de alguma forma...
        }
    }
}

function strInsert(tgt, idx, value)
{
    return tgt.substr(0, idx) + value + tgt.substr(idx, (tgt.length - idx));
}

function Validacao_DataFormatada_OnKeyDown(evt)
{
    var evObj = (navigator.userAgent.indexOf("MSIE") != -1) ? evt.srcElement : evt.currentTarget;

    var key = evt.keyCode;

    //se apertou numero 
    if((key > 47 && key < 58) || (key > 95 && key < 106))
    {
        //hora de por /, poe a / automatico
        if(evObj.value.length == 2 || evObj.value.length == 5)
            evObj.value = evObj.value + "/";

        //se apertou numero diferente de zero ou 1, poe o zero
        if(evObj.value.length == 3 && key != 48 && key != 96  && key != 49 && key != 97)
            evObj.value = evObj.value + "0";
    }

    //apertou / com apenas 1 numero antes, completa com 0
    //4/ vira 04/
    if((key == 193 || key == 111) && evObj.value.length == 1)
        evObj.value = "0" + evObj.value;

    //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /         / numpad                   0 48 - 9 57                    0 96 - 9 105
    if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==111) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
        {return true;}
    else
        {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
}

function Validacao_DataFormatada_OnKeyUp(evt)
{
    var v = true;
    var rgx =  /\d{2}\/\d{2}\/\d{4}/;

    var evObj = (navigator.userAgent.indexOf("MSIE") != -1) ? evt.srcElement : evt.currentTarget;

    if (evObj.value != "" && v == true)
    {
        if(evObj.value.match(rgx))
        {
            var s = evObj.value.match(rgx);
            v = (s[0].length == evObj.value.length);

            if (v == true)
            {
                //se passou no teste de formato, verifica a data:
                //essa é uma regex semi-decente. ainda passam algumas coisas inválidas,
                //mas aí tem que parar no server-side.
                rgx = /(0[1-9]|[12][0-9]|3[01])[\/](0[1-9]|1[012])[\/](18|19|20)\d\d/
                    
                s = evObj.value.match(rgx);

                if (s != null)
                    v = (s[0].length == evObj.value.length);
                else
                    v = false;
                    
                //se nao passar, mostra outra mensagem:
                if (v == false)
                {
                    //pValidation_displayValidation(evObj, "valDataDia", "Data inv&aacute;lida, favor verificar.", v);

                    return;
                }
            }
        }
        else
        {
            v = false;
        }
    }
}


function VerificarValorLimiteBovespa(pSender)
{
    var lCheckBox = $(pSender).parent().prev("p").find("input[type='checkbox']");

    if(lCheckBox.length > 0 && lCheckBox.is(":checked"))
    {
        var lValor = $(pSender).val();

        if(lValor == "") return true;

        if( isNaN($.format.number(lValor)) ) return true;
    }

    return false;
}

function VerificarDataLimiteBovespa(pSender)
{
    var lCheckBox = $(pSender).parent().prev("p").prev("p").find("input[type='checkbox']");

    if(lCheckBox.length > 0 && lCheckBox.is(":checked"))
    {
        var lValor = $(pSender).val();

        if(lValor == "" || lValor == "__/__/____") return true;

        try
        {
            if( GradAux_DataEstaNoPassado(lValor) )
                return true;
        }
        catch(erro){ return true; }
    }

    return false;
}