

/*

    Javascript não-obtrusivo para validação
        2006 Luciano De Maria

    Como funciona:
        Ao incluir o js em uma página, ele automaticamente "varre" o html em
        busca de controles com as classes css específicas para validação
        (função estética opcional), e atacha os eventos de mousedown, click,
        focus e lostfocus para o controle.
        Um span de mensagem é criado no html no mesmo nível do controle, com
        o id valMsg_[classe_css_da_mensagem]_[nome_do_controle] e classe valMsg_[classe_css_da_mensagem]

    Exemplo:
        Para um controle obrigatório numerico de id "txtTelefone", colocar
        as classes: valOb e valNum na textbox.
        O script irá atachar o keyDown e o blur da textbox, e fazer a verificacao
        para somente numeros e conteudo vazio.
        Caso não valide, será criado um span com a mensagem de erro:
            <span id="valMsg_valOb_txtTelefone" cssclass="valMsg valOb">Campo Obrigatório !</span>
            e será adicionada uma classe " campo_invalido " no controle
            
        Caso valide, será adicionada uma classe " campo_valido " no controle
    
    Esse js é para ser acompanhado pela classe vb pValidation, que faz as mesmas
    validacoes server-side, caso o javascript falhe ou esteja desligado.
*/


//onload nao obtrusivo
function addLoadEvent(func) 
{
    var oldonload = window.onload;

    if (typeof window.onload != 'function') 
        window.onload = func;
    else
        window.onload = function() {oldonload();func();}
}

//no fim deste arquivo:
//addLoadEvent(pValidation_init()) 

//olha todos os elementos que têm css, e atacha os eventos 
function pValidation_init()
{
    //alert("oi");
    
    var ipts = document.getElementsByTagName("input");
    
    for(var a = 0; a < ipts.length; a++)
    {
        //alert(ipts[a].id + ipts[a].title);
        if (ipts[a].className != "")
            pValidation_checkClasses(ipts[a]);
    }
}

//funcao pra atachar eventos conforme o browser
function pValidation_handleEvent(obj,evname,func)
{
    if(navigator.userAgent.indexOf("MSIE") != -1)
    {
        obj.attachEvent("on" + evname, func);
    }
    else
    {
        obj.addEventListener(evname, func, false);
    }
}


//verifica se tem alguma coisa inválida pra não dar submit
function pValidation_checkSubmit()
{

    var ipts = document.getElementsByTagName("input");

    //primeiro, valida todo mundo:

    for(var a = 0; a < ipts.length; a++)
    {
        if (ipts[a].className != "")
        {
            //sair de foco causa validacao
            var classes = ipts[a].className.split(" ");
    
            if (classes != "")
            {
                //alert(classes.length + " classes encontradas");
                
                for(var b = 0; b < classes.length; b++)
                {
                    switch (classes[b])
                    {
                        case "valOb" : valOb_validate(ipts[a]); break;
                        
                        case "valTxt" :            valTxt_validate(ipts[a]);        break;
                        case "valNum" :            valNum_validate(ipts[a]);        break;
                        case "valNum" :            valNum_validate(ipts[a]);        break;
                        case "valF1Num" :        valF1Num_validate(ipts[a]);        break;
                        case "valF2Num" :        valF2Num_validate(ipts[a]);        break;
                        case "valF3Num" :        valF3Num_validate(ipts[a]);        break;
                        case "valDataDia" :        valDataDia_validate(ipts[a]);    break;
                        case "valDataDiaMes" :    valDataDiaMes_validate(ipts[a]);break;
                        case "valDataMesL" :    valDataMesL_validate(ipts[a]);    break;
                        case "valDataMesC" :    valDataMesC_validate(ipts[a]);    break;
                        case "valCPF" :            valCPF_validate(ipts[a]);        break;
                        case "valCEP" :            valCEP_validate(ipts[a]);        break;
                        case "valCNPJ" :        valCNPJ_validate(ipts[a]);        break; //window.status+=" cnpj";
                        case "valEmail" :        valEmail_validate(ipts[a]);        break; //window.status+=" email";
                    }
                }
            }
        }
    }
    
    var spns = document.getElementsByTagName("span");
    
    //se tiver algun SPAN cujo ID começe com "valMsg", 
    //tem alguma coisa inválida;
    for(a = 0; a < spns.length; a++)
    {
        //alert("");
        if(spns[a].id.substring(0,6) == "valMsg") 
        {
            alert("Existem campos inv\u00e1lidos. Favor verificar.");
            return false;
        }
    }
    
    return true;
}

//funcao para adicionar os event handlers conforme
//as classes
function pValidation_checkClasses(obj)
{
    var classes = obj.className.split(" ");
    
    if (classes != "")
    {
        //alert(classes.length + " classes encontradas");
        
        for(var a = 0; a < classes.length; a++)
        {
            /* 2010 aliases pra Gradual */
            if(classes[a] == "GradWindow_TextBoxNumerica") classes[a] = "valNum";
            
            if(classes[a] == "GradWindow_TextBoxDecimal")  classes[a] = "valF2Num";

            if(classes[a] == "GradWindow_TextBoxDeData")   classes[a] = "valDataDia";

            switch (classes[a])
            {
                case "valOb" :
                        
                        pValidation_handleEvent(obj, "blur", valOb_OnBlur);
                        pValidation_handleEvent(obj, "keyup", valOb_OnKeyUp);
                    break;
                                            
                case "valTxt" :

                        pValidation_handleEvent(obj, "blur", valTxt_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valTxt_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valTxt_OnKeyUp);
                        
                    break;
                                
                case "valNum" :

                        pValidation_handleEvent(obj, "blur", valNum_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valNum_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valNum_OnKeyUp);
                        
                    break;
                                
                case "valF1Num" :

                        pValidation_handleEvent(obj, "blur", valF1Num_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valF1Num_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valF1Num_OnKeyUp);
                        
                    break;
                                
                case "valF2Num" :

                        pValidation_handleEvent(obj, "blur", valF2Num_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valF2Num_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valF2Num_OnKeyUp);
                        
                    break;
                                
                case "valF3Num" :

                        pValidation_handleEvent(obj, "blur", valF3Num_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valF3Num_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valF3Num_OnKeyUp);
                        
                    break;
                                
                case "valDataDia" :

                        pValidation_handleEvent(obj, "blur", valDataDia_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valDataDia_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valDataDia_OnKeyUp);
                        
                    break;
                                
                case "valDataDiaMes" :

                        pValidation_handleEvent(obj, "blur", valDataDiaMes_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valDataDiaMes_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valDataDiaMes_OnKeyUp);
                        
                    break;
                                
                case "valDataMesL" :

                        pValidation_handleEvent(obj, "blur", valDataMesL_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valDataMesL_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valDataMesL_OnKeyUp);
                        
                    break;
                                
                case "valDataMesC" :

                        pValidation_handleEvent(obj, "blur", valDataMesC_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valDataMesC_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valDataMesC_OnKeyUp);
                        
                    break;
                                
                case "valCPF" :

                        pValidation_handleEvent(obj, "blur", valCPF_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valCPF_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valCPF_OnKeyUp);
                        
                    break;
                            
                case "valCEP" :

                        pValidation_handleEvent(obj, "blur", valCEP_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valCEP_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valCEP_OnKeyUp);
                        
                    break;
                            
                case "valCNPJ" :

                        pValidation_handleEvent(obj, "blur", valCNPJ_OnBlur);
                        pValidation_handleEvent(obj, "keydown", valCNPJ_OnKeyDown);
                        pValidation_handleEvent(obj, "keyup", valCNPJ_OnKeyUp);
                        
                    break;
                                            
                case "valEmail" :

                        pValidation_handleEvent(obj, "blur", valEmail_OnBlur);
                        
                    break;
                                        
                default :
                    break;
            }
        }
    }
}


//funcao generica pra pegar o objeto do evento
function pValidation_getEvtObj(evt)
{
    var ev;

    //pega o objeto da pagina que está recebendo o evento, dependendo do browser
    if(navigator.userAgent.indexOf("MSIE") != -1)
        {ev = event.srcElement;}
    else
        {ev = evt.currentTarget;}
    
    return ev;
}


//funcao generica para controlar o estilo/mensagem em caso de valido/invalido
function pValidation_displayValidation(ev, type, msg, valido)
{
    var spn;
    spn = document.getElementById("valMsg_" + type + "_" + ev.id);
    
    if(!valido)
    {
        //window.status += " " + "false";
        //invalido
        //alert(ev.className);
        ev.className = ev.className.replace("campo_invalido","").replace("campo_valido","") + " campo_invalido";
        
        if (spn == null)
        {
            spn = document.createElement("span");
            spn.setAttribute("id", "valMsg_" + type + "_" + ev.id, 0);
            spn.className = "valMsg " + type;
            spn.innerHTML = msg;
            
            ev.parentNode.appendChild(spn);
        }
    }
    else
    {
        //window.status += " " + "true";
        
        if (spn != null) spn.parentNode.removeChild(spn);
        
        //se nao tiver mais nenhum span de validacao, pode voltar a classe.
        //isso é pra uma validacao nao interferir na outra, caso uma
        //outra validacao ainda esteja invalida no memso controle
        if (ev.parentNode.getElementsByTagName("span").length == 0)
            ev.className = ev.className.replace("campo_invalido","").replace("campo_valido","") + " campo_valido";
    }
}





//funcao caso o valor seja obrigatório (somente para input ou textarea)
//classe: valOb
    function valOb_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        var v = false;
        v = (evObj.value != "");
        
        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valOb", "Elemento obrigat&oacute;rio.", v);
    }
        
        function valOb_OnBlur(evt)
        {
            var evObj = pValidation_getEvtObj(evt);
            valOb_validate(evObj);
        }

        function valOb_OnKeyUp(evt)
        {
            var evObj = pValidation_getEvtObj(evt);
            valOb_validate(evObj);
        }





//funcao caso o valor seja apenas texto, sem numeros
//classe: valTxt
    function valTxt_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /[a-zA-Z ]*/;
        
        if (evObj.value != "")
        {
            var s = evObj.value.match(rgx);            
            v = (s[0].length == evObj.value.length);
        }
                
        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valTxt", "Somente texto, sem acentos.", v);
    }
        
        function valTxt_OnBlur(evt){valTxt_validate(pValidation_getEvtObj(evt));}
        
        function valTxt_OnKeyUp(evt){valTxt_validate(pValidation_getEvtObj(evt));}

    function valTxt_OnKeyDown(evt)
    {
        var key = evt.keyCode;
        //window.status = key;
        //    enter      backspace     null        espaço       home 35, end, setas 40            a 65 -z 90
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 64 && key < 91))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }



function strInsert(tgt, idx, value)
{
    return tgt.substr(0, idx) + value + tgt.substr(idx, (tgt.length - idx));
    //return (tgt.length - idx);
}

//funcao caso o valor seja numerico (somente para input ou textarea)
//classe: valNum
    function valF1Num_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /\d*/;
                
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.                    
        if (evObj.value != "")
        {
            //para o match dos digitos, ignora ponto e virgula
            var nvalue = evObj.value.replace(".", "").replace(",", "");
                
            //tira zeros à esquerda
            while(nvalue.substr(0, 1) == '0')
                nvalue = nvalue.substr(1);
            
            //alert(nvalue);
            
            var s = nvalue.match(rgx);
            v = (s[0].length == nvalue.length);
                        
            //1234567890
            //12.345.678,90
            if(v == true)
            {
                //alert(nvalue);
                
                if(nvalue.length < 2) nvalue = "0" + nvalue;
                if(nvalue.length < 3) nvalue = "0" + nvalue;
                nvalue = strInsert(nvalue, nvalue.length - 2, ",");
                if(nvalue.length > 6) nvalue = strInsert(nvalue, nvalue.length - 6, ".");
                if(nvalue.length > 10) nvalue = strInsert(nvalue, nvalue.length - 10, ".");
                if(nvalue.length > 14) nvalue = strInsert(nvalue, nvalue.length - 14, ".");
            
                evObj.value = nvalue;
            }
        }

        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);
    }
        
        function valF1Num_OnBlur(evt){valF1Num_validate(pValidation_getEvtObj(evt));}
        
        function valF1Num_OnKeyUp(evt){valF1Num_validate(pValidation_getEvtObj(evt));}

    function valF1Num_OnKeyDown(evt)
    {
        var key = evt.keyCode;
        var val = pValidation_getEvtObj(evt).value;
        
        //alert(key);
        //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
        if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    
    
    
    

//funcao caso o valor seja numerico (somente para input ou textarea)
//classe: valNum
    function valF2Num_validate(value, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /\d*/;
                
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.                    
        if (value != "")
        {
            var nvalue = value.replace(/\./gi, "").replace(/,/gi, "");
            
            //alert("Validando " + nvalue);
            //para o match dos digitos, ignora ponto e virgula
            var s = nvalue.match(rgx);
            v = (s[0].length == nvalue.length);
        }

        if (forcaInvalido == true) v = false;
        
        return v;
        //chama o display da validacao conforme o resultado (true ou false):
        //pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);
    }
        
        function valF2Num_OnBlur(evt){/*valF2Num_validate(pValidation_getEvtObj(evt));*/}
        
        function valF2Num_OnKeyUp(evt){/*valF2Num_validate(pValidation_getEvtObj(evt));*/}
        
        
    function valF2Num_Format(evt, nextChar)
    {
        var obj = pValidation_getEvtObj(evt);
        var val = obj.value;
        
        var nval;
        
        if(nextChar == "<")
        {
            nval = val.substr(0, val.length - 1);
        }
        else
        {

            if(obj.selectionStart || obj.selectionStart == 0)
            {
                var lValorPreCursor, lValorPosCursor;

                //selectionStart é método do firefox pra pegar o texto selecionado (se houver)
                
                lValorPreCursor = val.substr(0, obj.selectionStart);
                lValorPosCursor = val.substr(obj.selectionEnd);

                nval = lValorPreCursor + nextChar + lValorPosCursor;
            }
            else
            {
                //no IE só pega o texto selecionado a partir dessa função:
                var lTextoSelecionado = document.selection.createRange().text;

                if(lTextoSelecionado == val)
                {
                    //o cara (provavelmente) selecionou todo o texto na textbox, então substitui:

                    nval = nextChar;

                    if(nextChar != "")
                        document.selection.empty();

                }
                else
                {
                    nval = val + nextChar;
                }
            }
        }
        
        //alert(nval);
        
        /*
        alert("curvalue = " + val);
        alert("nextvalue = " + nval);
        */
        //verifica se está tudo certinho (so numeros, ignorando , e .)
        
        var v = valF2Num_validate(nval, false);
        if(v)
        {
            //só formata se nao tiver , inserida:
            if(nval.indexOf(",") == -1)
            {
                //tira os pontos e virgulas para reformatar:
                nval = nval.replace(/\./gi, "").replace(/,/gi, "");
                //alert(nval);

                //reinsere a pontuacao:
                if(nval.length > 3) nval = strInsert(nval, nval.length - 3, ".");
                if(nval.length > 7) nval = strInsert(nval, nval.length - 7, ".");
                if(nval.length > 11) nval = strInsert(nval, nval.length - 11, ".");
                if(nval.length > 15) nval = strInsert(nval, nval.length - 15, ".");

                //sobrescreve o valor do objeto
                //alert("nval = " + nval);
            }

            obj.value = nval;

            obj.focus();
        }

        //pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);

        //retorna falso pra nao repetir
        if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();
        return false;
    }

    function valF2Num_OnKeyDown(evt)
    {
        var key = evt.keyCode;
        var val = pValidation_getEvtObj(evt).value;
        
        try
        {
            var lchar = String.fromCharCode(key);

            //window.status += " " + key + "(" + lchar + ") ";

            //window.status = val.indexOf(",");

        }catch(erro){}
        
        try
        {
            //if(window.status.length > 30)
            //    window.status = "";

            var lchar = String.fromCharCode(key);

            //console.log( key + "(" + lchar + ") " );

        }catch(erro){}

        //alert(key);
        //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
        if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //window.status += " a";

            //se for numero, formata:
            if((key > 47 && key < 58) || (key > 95 && key < 106) || (key==8) || (key == 110) || (key == 188))
            {
                //window.status += " b";

                if(key > 95 && key < 106) key = key - 48;

                var nchar = String.fromCharCode(key);
                
                if(key == 8) nchar = "<";
                
                if(key == 110 || key == 188) nchar = ",";

                //window.status += " c(" + nchar + ")";

                return valF2Num_Format(evt, nchar);
            }
            else
            {
                //window.status += " t";

                return true;
            }
        }
        else
        {
            if(navigator.userAgent.indexOf("MSIE") == -1)
            {
                //window.status += " p";
                evt.preventDefault();
            }

            return false;
        }
    }
    
    
    
    

//funcao caso o valor seja numerico (somente para input ou textarea)
//classe: valNum
    function valF3Num_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /\d*/;
                
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.                    
        if (evObj.value != "")
        {
            var nvalue;
            
            if(evObj.value.indexOf(",") != -1)
                nvalue = evObj.value.substr(0, evObj.value.indexOf(",")).replace(".", "").replace(",", "");
            else
                nvalue = evObj.value.replace(".", "");
            
            //alert(nvalue);
            
            //tira zeros à esquerda
            while(nvalue.substr(0, 1) == '0')
                nvalue = nvalue.substr(1);
            
            //alert(nvalue);
            
            //para o match dos digitos, ignora ponto e virgula
            var s = nvalue.match(rgx);
            v = (s[0].length == nvalue.length);
                        
            //1234567890
            //12.345.678,90
            if(v == true)
            {                
                var commaidx = evObj.value.indexOf(",");
                
                //alert(diff);
                if(nvalue.length > 3) nvalue = strInsert(nvalue, nvalue.length - 3, ".");
                if(nvalue.length > 7) nvalue = strInsert(nvalue, nvalue.length - 7, ".");
                if(nvalue.length > 11) nvalue = strInsert(nvalue, nvalue.length - 11, ".");
            
                if(commaidx != -1)
                    nvalue = nvalue + evObj.value.substr(evObj.value.indexOf(","));
                    
                evObj.value = nvalue;
            }
        }

        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);
    }
        
        function valF3Num_OnBlur(evt){/*valF3Num_validate(pValidation_getEvtObj(evt));*/}
        
        function valF3Num_OnKeyUp(evt){valF3Num_validate(pValidation_getEvtObj(evt));}

    function valF3Num_OnKeyDown(evt)
    {
        var key = evt.keyCode;
        var val = pValidation_getEvtObj(evt).value;
        
        //alert(key);
        //    enter                         ,                                          ,                    backspace     null       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
        if ((key==13) || (key == 188 && val.indexOf(",") == -1) || (key == 110 && val.indexOf(",") == -1) || (key==8) || (key==0) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
        {
            return true;
        }
        else
        {
            if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;
        }
    }
    
    
    
    
    
    
    

//funcao caso o valor seja numerico (somente para input ou textarea)
//classe: valNum
    function valNum_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /\d*/;
        
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "")
        {
            var s = evObj.value.match(rgx);
            v = (s[0].length == evObj.value.length);
        }
                
        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valNum", "Somente n&uacute;meros.", v);
    }
        
        function valNum_OnBlur(evt){valNum_validate(pValidation_getEvtObj(evt));}
        
        function valNum_OnKeyUp(evt){valNum_validate(pValidation_getEvtObj(evt));}

    function valNum_OnKeyDown(evt)
    {
        var key = evt.keyCode;
        //window.status = key;
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab                         0 48 - 9 57                 numpad
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9)|| (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    
    
    
    
    
//funcao caso o valor seja data, formato dd/mm/aaaa (somente para input)
//classe: valDataDia

    function valDataDia_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{2}\/\d{2}\/\d{4}/;
        
        if (forcaInvalido == true) v = false;
        
        if (v == false)
        {
            pValidation_displayValidation(evObj, "valDataDia", "Data inv&aacute;lida, favor verificar.", v);
            return;
        }
        
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "" && v == true)
        {
            //se estiver dd/mm/aa, poe o seculo pra ver se rola:
            
            if(evObj.value.length == 8)
            {
                var pos = evObj.value.substring(6,8);
                var sec = "20";
                if(pos > 19) sec = "19";
                
                evObj.value = evObj.value.substring(0,6) + sec + pos;
            }
            
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);
                v = (s[0].length == evObj.value.length);
                
                if (v == true)
                {
                    //se passou no teste de formato, verifica a data:
                    //essa é uma regex semi-decente. ainda passam algumas coisas inválidas,
                    //mas aí tem que parar no server-side.
                    rgx = /(0[1-9]|[12][0-9]|3[01])[\/](0[1-9]|1[012])[\/](19|20)\d\d/
                    
                    s = evObj.value.match(rgx);
                    if (s != null)
                        v = (s[0].length == evObj.value.length);
                    else
                        v = false;
                    
                    //se nao passar, mostra outra mensagem:
                    if (v == false)
                    {
                        pValidation_displayValidation(evObj, "valDataDia", "Data inv&aacute;lida, favor verificar.", v);
                        return;
                    }
                }
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valDataDia", "Formato: dd/mm/aaaa", v);
    }
        
    function valDataDia_OnBlur(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        //completa com seculos
        if (evObj.value.length == 8)
            evObj.value = strInsert(evObj.value, 6, "20");
            
        valDataDia_validate(pValidation_getEvtObj(evt));
    }
        
    function valDataDia_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 10)
            valDataDia_validate(evObj);
    }

    function valDataDia_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
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
    
    

    
    

    
//funcao caso o valor seja data, formato mm/aa (somente para input)
//classe: valDataDiaMes

    function valDataDiaMes_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{2}\/\d{2}/;
        
        if (forcaInvalido == true) v = false;
        
        if (v == false)
        {
            pValidation_displayValidation(evObj, "valDataDiaMes", "Data inv&aacute;lida, favor verificar.", v);
            return;
        }    
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
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
                    rgx = /(0[1-9]|1[0-9]|2[0-9]|3[01])[\/](0[1-9]|1[0-2])/
                    
                    s = evObj.value.match(rgx);
                    if (s != null)
                        v = (s[0].length == evObj.value.length);
                    else
                        v = false;
                    
                    //se nao passar, mostra outra mensagem:
                    if (v == false)
                    {
                        pValidation_displayValidation(evObj, "valDataDiaMes", "Data inv&aacute;lida, favor verificar.", v);
                        return;
                    }
                }
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valDataDiaMes", "Formato: dd/mm", v);
    }
        
        function valDataDiaMes_OnBlur(evt){valDataDiaMes_validate(pValidation_getEvtObj(evt));}
        
    function valDataDiaMes_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 10)
            valDataDiaMes_validate(evObj);
    }

    function valDataDiaMes_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por /, poe a / automatico
            if(evObj.value.length == 2)
                evObj.value = evObj.value + "/";
            
            //se apertou numero diferente de 0 1 2 ou 3, poe o zero
            if(evObj.value.length == 0 && key != 48 && key != 49  && key != 50 && key != 51 && key != 96 && key != 97  && key != 98 && key != 99)
                evObj.value = evObj.value + "0";
            
            //se apertou numero diferente de zero ou 1, poe o zero
            if(evObj.value.length == 3 && key != 48 && key != 96  && key != 49 && key != 97)
                evObj.value = evObj.value + "0";
                
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /          /(numpad)                 0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==111) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    
    
    
    
//funcao caso o valor seja data, formato mm/aaaa (somente para input)
//classe: valDataMesL

    function valDataMesL_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{2}\/\d{4}/;
        
        if (forcaInvalido == true) v = false;
        
        if (v == false)
        {
            pValidation_displayValidation(evObj, "valDataMesL", "Data inv&aacute;lida, favor verificar.", v);
            return;
        }    
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "" && v == true)
        {
            //se estiver mm/aa, poe o seculo pra ver se rola:
            
            if(evObj.value.length == 5)
            {
                var pos = evObj.value.substring(3,8);
                var sec = "20";
                if(pos > 19) sec = "19";
                
                evObj.value = evObj.value.substring(0,3) + sec + pos;
            }
            
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);
                v = (s[0].length == evObj.value.length);
                
                if (v == true)
                {
                    //se passou no teste de formato, verifica a data:
                    //essa é uma regex semi-decente. ainda passam algumas coisas inválidas,
                    //mas aí tem que parar no server-side.
                    rgx = /(0[1-9]|1[012])[\/](19|20)\d\d/
                    
                    s = evObj.value.match(rgx);
                    if (s != null)
                        v = (s[0].length == evObj.value.length);
                    else
                        v = false;
                    
                    //se nao passar, mostra outra mensagem:
                    if (v == false)
                    {
                        pValidation_displayValidation(evObj, "valDataMesL", "Data inv&aacute;lida, favor verificar.", v);
                        return;
                    }
                }
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valDataMesL", "Formato: mm/aaaa", v);
    }
        
        function valDataMesL_OnBlur(evt){valDataMesL_validate(pValidation_getEvtObj(evt));}
        
    function valDataMesL_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 10)
            valDataMesL_validate(evObj);
    }

    function valDataMesL_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por /, poe a / automatico
            if(evObj.value.length == 2)
                evObj.value = evObj.value + "/";
            
            //se apertou numero diferente de zero ou 1, poe o zero
            if(evObj.value.length == 0 && key != 48 && key != 96  && key != 49 && key != 97)
                evObj.value = evObj.value + "0";
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /          /(numpad)                 0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==111) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    
    
    
    

    
//funcao caso o valor seja data, formato mm/aa (somente para input)
//classe: valDataMesC

    function valDataMesC_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{2}\/\d{2}/;
        
        if (forcaInvalido == true) v = false;
        
        if (v == false)
        {
            pValidation_displayValidation(evObj, "valDataMesC", "Data inv&aacute;lida, favor verificar.", v);
            return;
        }    
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
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
                    rgx = /(0[1-9]|1[012])[\/]\d\d/
                    
                    s = evObj.value.match(rgx);
                    if (s != null)
                        v = (s[0].length == evObj.value.length);
                    else
                        v = false;
                    
                    //se nao passar, mostra outra mensagem:
                    if (v == false)
                    {
                        pValidation_displayValidation(evObj, "valDataMesC", "Data inv&aacute;lida, favor verificar.", v);
                        return;
                    }
                }
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valDataMesC", "Formato: mm/aa", v);
    }
        
        function valDataMesC_OnBlur(evt){valDataMesC_validate(pValidation_getEvtObj(evt));}
        
    function valDataMesC_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 10)
            valDataMesC_validate(evObj);
    }

    function valDataMesC_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por /, poe a / automatico
            if(evObj.value.length == 2)
                evObj.value = evObj.value + "/";
            
            //se apertou numero diferente de zero ou 1, poe o zero
            if(evObj.value.length == 0 && key != 48 && key != 96  && key != 49 && key != 97)
                evObj.value = evObj.value + "0";
            
            //se apertou numero diferente de zero ou 1, poe o zero
            if(evObj.value.length == 3 && key != 48 && key != 96  && key != 49 && key != 97)
                evObj.value = evObj.value + "0";
                
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /          /(numpad)                 0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==111) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    
    
    
    
    
    
//CPF

function pValidation_CPF(valor)
{

    sCPF = valor.replace(".","").replace(".","").replace("-","");

    if (sCPF == "") return false;
    
    var frase = "1234567890-.";
    var fcerta = true;

    //window.status += " " + sCPF;
    for (i=0; i< sCPF.length; i++)
        fcerta &= frase.indexOf(sCPF.charAt(i)) != -1;

    if (!fcerta)
    {
        return false;
    }
    else
    {
        var num_cpf = sCPF;
    
        //window.status += " " + "1";
        //Existem casos que o CPF tem um ou menos dígitos
        //nestes casos são adicionados um zeros a esquerda
        if (num_cpf.length < 11)
            num_cpf += string$("0",11 - num_cpf.length); 
        
        //window.status += " " + "2";
        //Calcula o primeiro digito de num_cpf
        var li_conta1 = 0;
        for(i=1;i<=9;i++)
            li_conta1 += parseInt(num_cpf.charAt(i-1))*(11-i);
    
        //window.status += " " + "3";
        var li_conta2 = 11 - (li_conta1 % 11);
            if (li_conta2 > 9)
                li_conta2=0;

        //window.status += " " + "4";
        if (li_conta2 != num_cpf.charAt(num_cpf.length - 2))
            return false;
     
        //Calcula o segundo digito de ls_num_cpf
        li_conta1 = 0;
        for(i=1;i<=9;i++)
            li_conta1 += parseInt(num_cpf.charAt(i))*(11-i);
        
        //window.status += " " + "5";
        var li_conta2 = 11 - (li_conta1 % 11);
            if (li_conta2 > 9)
                li_conta2 = 0;

        //window.status += " " + "6";
        if (li_conta2 == num_cpf.charAt(num_cpf.length - 1))
            return true;
    }

    return false;
}

    function valCPF_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{3}\.\d{3}\.\d{3}-\d{2}/;
        
        if(forcaInvalido == true) v = false;
        
        if(v == false)
        {
            pValidation_displayValidation(evObj, "valCPF", "CPF inv&aacute;lido, favor verificar.", v);
            return;
        }
        
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "" && v == true)
        {
            //window.status = evObj.value + evObj.value.match(rgx);
            
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);
                v = (s[0].length == evObj.value.length);
                
                if (v == true)
                {
                    //window.status = "cpf sok";
                        
                    //se passou no teste de formato, verifica os numeros:
                    v = pValidation_CPF(evObj.value);
                    
                    //se nao passar, mostra outra mensagem:
                    if (v == false)
                    {
                        //window.status = "cpf inv";
                        pValidation_displayValidation(evObj, "valCPF", "CPF inv&aacute;lido, favor verificar.", v);
                        return;
                    }
                    else
                    {
                        //window.status = "cpf ok";
                    }
                }
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valCPF", "Formato: (000)nnn.nnn.nnn-nn", v);
    }
        
        function valCPF_OnBlur(evt){valCPF_validate(pValidation_getEvtObj(evt));}
        
    function valCPF_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 14)
            valCPF_validate(evObj);
    }

    function valCPF_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //window.status = key;
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por ., poe . automatico
            if(evObj.value.length == 3 || evObj.value.length == 7)
                evObj.value = evObj.value + ".";
                
            //hora de por -, poe - automatico
            if(evObj.value.length == 11)
                evObj.value = evObj.value + "-";
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /                         .              -      -(numpad)             0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==27) || (key==190) || key==189 || key==109 || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }





//CEP

    function valCEP_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{5}-\d{3}/;
        
        if(forcaInvalido == true) v = false;
                
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "" && v == true)
        {
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);
                v = (s[0].length == evObj.value.length);
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valCEP", "Formato: nnnnn-nnn", v);
    }
        
        function valCEP_OnBlur(evt){valCEP_validate(pValidation_getEvtObj(evt));}
        
    function valCEP_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 9)
            valCEP_validate(evObj);
    }

    function valCEP_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por -
            if(evObj.value.length == 5)
                evObj.value = evObj.value + "-";
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /                        0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {if(navigator.userAgent.indexOf("MSIE") == -1)evt.preventDefault();    return false;}
    }
    




//CNPJ

    function valCNPJ_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx =  /\d{8}\/\d{4}-\d{2}/;
        
        if(forcaInvalido == true) v = false;
        
        //se nao tiver nada, deixa quieto. se for obrigatorio,
        //vai cair na validacao de obrigatorio. se nao for, ok.            
        if (evObj.value != "" && v == true)
        {
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);
                v = (s[0].length == evObj.value.length);
            }
            else
            {
                v = false;
            }
        }
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valCNPJ", "Formato: nnnnnnnn/nnnn-nn", v);
    }
        
        function valCNPJ_OnBlur(evt){valCNPJ_validate(pValidation_getEvtObj(evt));}
        
    function valCNPJ_OnKeyUp(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
        
        if (evObj.value.length == 16)
            valCNPJ_validate(evObj);
    }

    function valCNPJ_OnKeyDown(evt)
    {
        var evObj = pValidation_getEvtObj(evt);
            
        var key = evt.keyCode;
        
        //se apertou numero 
        if((key > 47 && key < 58) || (key > 95 && key < 106))
        {
            //hora de por /
            if(evObj.value.length == 8)
                evObj.value = evObj.value + "/";
                
            //hora de por -
            if(evObj.value.length == 13)
                evObj.value = evObj.value + "-";
        }
        
        //    enter      backspace     null        espaço       home 35, end, setas 40                 tab            /                        0 48 - 9 57                    0 96 - 9 105
        if ((key==13) || (key==8) || (key==0) || (key==32) || (key > 34 && key < 41) || (key==46) || (key==9) || (key==193) || (key==27) || (key > 47 && key < 58) || (key > 95 && key < 106))
            {return true;}
        else
            {return false;}
    }
    


//funcao caso o valor seja email
//classe: valEmail
    function valEmail_validate(evObj, forcaInvalido)
    {
        //a validacao ocorre aqui:
        
        var v = true;
        var rgx = /[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}/;
        
        if (evObj.value != "")
        {
            if(evObj.value.match(rgx))
            {
                var s = evObj.value.match(rgx);        
                    
                v = (s[0].length == evObj.value.length);
            }
            else
            {
                v = false;
            }
        }
                
        if (forcaInvalido == true) v = false;
        
        //chama o display da validacao conforme o resultado (true ou false):
        pValidation_displayValidation(evObj, "valEmail", "Formato: ttttt@ttttt.ttt", v);
    }
        
        function valEmail_OnBlur(evt){valEmail_validate(pValidation_getEvtObj(evt));}
        


/*

    Outras funções auxiliares:

*/



function GradAux_NumeroFromStringPtBR(pString)
{
    var lString = pString.replace(/\./gi, "");

    var lRetorno;

    if(lString.indexOf(",") != -1)
    {
        var lCasas = lString.length - lString.indexOf(",") - 1;

        lRetorno = new Number(new Number(lString.replace(",", "")) / Math.pow(10, lCasas));
    }
    else
    {
        lRetorno = new Number(lString);
    }

    return lRetorno;
}


function GradAux_Multiplicacao(pTermoDecimal, pTermoInteiro)
{
    var lTermoA = pTermoDecimal + "";
    var lTermoB = pTermoInteiro + "";

    if(lTermoA.indexOf(",") == -1)
        lTermoA = lTermoA + ",00";

    var lCasas = lTermoA.length - lTermoA.indexOf(",") - 1;

    lTermoA = lTermoA.replace(",", "");

    lTermoA = GradAux_NumeroFromStringPtBR(lTermoA);
    lTermoB = GradAux_NumeroFromStringPtBR(lTermoB + "");

    return lTermoA * lTermoB / Math.pow(10, lCasas);
}


/* Objeto para conversão de string para número e vice-versa (padrões de formato brasileiros) */

var NumConv = 
{
    NumToStr : function(pNumber, pCasasDecimais)
    {
        var lRetorno = "";
        var lStringOriginal = pNumber + "";

        var lParteNum, lParteDec;

        var lSinal = "";
            
        if(lStringOriginal.indexOf(".") == -1)
        {
            //número sem parte decimal

            lParteNum = lStringOriginal;
            lParteDec = "";
        }
        else
        {
            //número com parte decimal

            lParteNum = lStringOriginal.substr(0, lStringOriginal.indexOf("."));
            lParteDec = lStringOriginal.substr(lStringOriginal.indexOf(".") + 1);
        }
            
        if(lStringOriginal.charAt(0) == "-")
        {
            lSinal = "-";

            lParteNum = lParteNum.substr(1);
        }

        //var lQtdMil = Math.floor(lParteNum.length / 3);
        var lQtdMilIns = 0;

        for(var a = (lParteNum.length - 1); a >= 0; a--)
        {
            lRetorno = lParteNum.charAt(a) + lRetorno;

            if((lRetorno.length - lQtdMilIns) % 3 == 0 && a > 0)
            {
                lRetorno = this.MilSep + lRetorno;

                lQtdMilIns++;
            }
        }

        if(pCasasDecimais && pCasasDecimais != "" && pCasasDecimais != null && !isNaN(pCasasDecimais))
        {
            while(lParteDec.length < pCasasDecimais)
            {
                lParteDec = lParteDec + "0";
            }

            if(lParteDec > pCasasDecimais)
            {
                lParteDec = lParteDec.substr(0, pCasasDecimais);
            }
        }

        if(lParteDec != "")
        {
            lRetorno = lRetorno + this.DecSep + lParteDec;
        }

        lRetorno = lSinal + lRetorno;

        return lRetorno;
    }
    , StrToNum : function(pString)
    {
        var lStringOriginal = pString.replace(/ /gi, "");

        var lStringFinal = "";

        var lParteNum, lParteDec;

        if(lStringOriginal.indexOf(this.DecSep) == -1)
        {
            //número sem parte decimal

            lParteNum = lStringOriginal;
            lParteDec = "0";
        }
        else
        {
            //número com parte decimal
                
            lParteNum = lStringOriginal.substr(0, lStringOriginal.indexOf(this.DecSep));
            lParteDec = lStringOriginal.substr(lStringOriginal.indexOf(this.DecSep) + 1);
        }

        lParteNum = lParteNum.replace(/\./gi, "").replace(/,/gi, "");

        return new Number(lParteNum + "." + lParteDec);
    }
    , StrToPretty : function(pString, pCasasDecimais)
    {
        return NumConv.NumToStr(new Number(   pString.replace(".", "").replace(",", ".")  ), pCasasDecimais);
    }
    , MilSep: "."
    , DecSep: ","
}





