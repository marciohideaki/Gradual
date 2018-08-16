
function GradAux_DataDeHoje()
{
    var lData = new Date();

    return ("0" + lData.getDate()).substr(0, 2) + "/" + ("0" + (lData.getMonth() + 1)).substr(0, 2) + "/" + lData.getFullYear();
}

function GradAux_DataDeHojeComHora()
{
    return GradAux_DataDeHojeComHoraCompleta().substr(0, 16);
}

function GradAux_DataDeHojeComHoraCompleta()
{
    var lData = new Date();

    var lRetorno = "";
    var lVal = "";

    lVal = ("0" + lData.getDate());

    if(lVal.length == 3) lVal = lVal.substr(1, 2);

    lRetorno = lVal + "/";

    lVal = ("0" + (lData.getMonth() + 1));

    if(lVal.length == 3) lVal = lVal.substr(1, 2);

    lRetorno = lRetorno + lVal + "/" + lData.getFullYear() + " ";

    lVal = ("0" + (lData.getHours() + 1))

    if(lVal.length == 3) lVal = lVal.substr(1, 2);

    lRetorno = lRetorno + lVal + ":";

    lVal = ("0" + (lData.getMinutes() + 1));

    if(lVal.length == 3) lVal = lVal.substr(1, 2);

    lRetorno = lRetorno + lVal + ":";

    lVal = ("0" + (lData.getSeconds() + 1));

    if(lVal.length == 3) lVal = lVal.substr(1, 2);

    lRetorno = lRetorno + lVal;

    return lRetorno;
}

function GradAux_DataAPartirDeString(pStringDeDataBrasileira)
{
    var lComponentes = pStringDeDataBrasileira.split("/");

    if(lComponentes.length == 3)
    {
        var lRetorno = new Date(lComponentes[2], lComponentes[1], lComponentes[0]);

        return lRetorno;
    }

    return "<erro em DataAPartirDeString>";
}

function GradAux_DataEstaNoPassado(pStringDeDataBrasileira)
{
    var lData = GradAux_DataAPartirDeString( pStringDeDataBrasileira );

    if(lData != "<erro em DataAPartirDeString>")
    {
        return (lData < new Date());
    }

    throw "Data [" + pStringDeDataBrasileira + "] não é válida";
}
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
    , MilSep: "."
    , DecSep: ","
}