
var matchhora = new RegExp(/^([0-1][0-9]|2[0-3]):[0-5][0-9]$/gi);
var matchdata = new RegExp(/((0[1-9]|[12][0-9]|3[01])\/(0[13578]|1[02])\/[12][0-9]{3})|((0[1-9]|[12][0-9]|30)\/(0[469]|11)\/[12][0-9]{3})|((0[1-9]|1[0-9]|2[0-8])\/02\/[12][0-9]([02468][1235679]|[13579][01345789]))|((0[1-9]|[12][0-9])\/02\/[12][0-9]([02468][048]|[13579][26]))/gi);


function showAlert(type, message) {
    //if (type == 'success') {
    //    $('#alert').addClass('alert-' + type).html(message).fadeIn();
    //    setTimeout("closeAlert()", 6000); // 6 segundos
    //} else {
    //    $('#alertBox').addClass('alertBox-' + type).html(message);
    //    openAlertBox();
    //}

    showStickySuccessToast(type, message);

}

function showStickySuccessToast(typeMsg,message) {
    $().toastmessage('showToast', {
        text: message,
        sticky: false,
        position: 'middle-center',
        type: typeMsg,
        closeText: '',
        stayTime: 5000
    });

}

function closeAlert() {
  $('#alert').fadeOut();
}
function openAlertBox() {
// Define the Dialog and its properties.
    $("#alertBox").dialog({
            resizable: false,
            modal: true,
            title: "Aviso",
            height: 250,
            width: 400,
            autoOpen: false,
            buttons: {
                "Fechar": function () {
                    $(this).dialog('close');
                }
            }
    });
    $("#alertBox").dialog("open");
    setTimeout("closeAlertBox()", 6000); // 6 segundos
}

function closeAlertBox() {
    $("#alertBox").dialog("close");
}

function validaHora(hora) {
    return hora.match(matchhora);
}
function validaData(data) {
    return data.match(matchdata);
}

function validaHH(hora) {
    return (hora * 1) > 23;
}
function validaMM(min) {
    return (min * 1) > 59;
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

/**
* Soma duas horas.
* Exemplo:  12:35 + 07:20 = 19:55.
*/
function somaHora(horaInicio, horaSomada) {

    horaIni = horaInicio.split(':');
    horaSom = horaSomada.split(':');

    horasTotal = parseInt(horaIni[0], 10) + parseInt(horaSom[0], 10);
    minutosTotal = parseInt(horaIni[1], 10) + parseInt(horaSom[1], 10);

    if (minutosTotal >= 60) {
        minutosTotal -= 60;
        horasTotal += 1;
    }

    horaFinal = completaZeroEsquerda(horasTotal) + ":" + completaZeroEsquerda(minutosTotal);
    return horaFinal;
}

/**
* Retona a diferença entre duas horas.
* Exemplo: 14:35 a 17:21 = 02:46
* Adaptada de http://stackoverflow.com/questions/2053057/doing-time-subtraction-with-jquery
*/
function diferencaHoras(horaInicial, horaFinal) {

    // Tratamento se a hora inicial é menor que a final 
    if (!isHoraInicialMenorHoraFinal(horaInicial, horaFinal)) {
        aux = horaFinal;
        horaFinal = horaInicial;
        horaInicial = aux;
    }

    hIni = horaInicial.split(':');
    hFim = horaFinal.split(':');

    horasTotal = parseInt(hFim[0], 10) - parseInt(hIni[0], 10);
    minutosTotal = parseInt(hFim[1], 10) - parseInt(hIni[1], 10);

    if (minutosTotal < 0) {
        minutosTotal += 60;
        horasTotal -= 1;
    }

    horaFinal = completaZeroEsquerda(horasTotal) + ":" + completaZeroEsquerda(minutosTotal);
    return horaFinal;
}

/**
 * Verifica se a hora inicial é menor que a final.
 */
function isHoraInicialMenorHoraFinal(horaInicial, horaFinal) {
    horaIni = horaInicial.split(':');
    horaFim = horaFinal.split(':');

    // Verifica as horas. Se forem diferentes, é só ver se a inicial 
    // é menor que a final.
    hIni = parseInt(horaIni[0], 10);
    hFim = parseInt(horaFim[0], 10);
    if (hIni != hFim)
        return hIni < hFim;

    // Se as horas são iguais, verifica os minutos então.
    mIni = parseInt(horaIni[1], 10);
    mFim = parseInt(horaFim[1], 10);
    if (mIni != mFim)
        return mIni < mFim;
}


/**
 * Completa um número menor que dez com um zero à esquerda.
 * Usado aqui para formatar as horas... Exemplo: 3:10 -> 03:10 , 10:5 -> 10:05
 */
function completaZeroEsquerda(numero) {
    return (numero < 10 ? "0" + numero : numero);
}



function validarData(data) {
    var patternValidaData = /^(((0[1-9]|[12][0-9]|3[01])([-.\/])(0[13578]|10|12)([-.\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-.\/])(0[469]|11)([-.\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-.\/])(02)([-.\/])(\d{4}))|((29)(\.|-|\/)(02)([-.\/])([02468][048]00))|((29)([-.\/])(02)([-.\/])([13579][26]00))|((29)([-.\/])(02)([-.\/])([0-9][0-9][0][48]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][2468][048]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][13579][26])))$/;

    return patternValidaData.test(data);
}

/**
* Compara duas datas. Retorna 0 se iguais. -1 se data1 menor. 1 se data1 maior.
*/
function comparaDatas(data1, data2)
{
    var array1 = data1.toString().split("/");
    var array2 = data2.toString().split("/");

    var v1 = (array1[2] + array1[1] + array1[0]) * 1;
    var v2 = (array2[2] + array2[1] + array2[0]) * 1;

    //alert(v1 + " - " + v2);

    if (v1 == v2) {
        return 0;
    } else if (v1 < v2) {
        return -1;
    }
    return 1;
}

function DiaSemanaExtenso(data) {
    var dias_semana = new Array("Domingo", "Segunda-feira",
          "Terça-feira", "Quarta-feira", "Quinta-feira",
          "Sexta-feira", "Sábado");
    var dia = data.getDay();
    return dias_semana[dia];
}

function isDiaSemana(data) {
    var dia = data.getDay();
    return dia!= 0 && dia!=6;
}


function DataBRPAraDatetime(dataFormat) {
    dia = dataFormat.substring(0, 2);
    mes = dataFormat.substring(3, 5);
    ano = dataFormat.substring(6, 10);
    return new Date(ano,((mes*1) - 1),dia);
}

function DataBRPAraYYYYMMDDbarras(dataBR) {
    dia = dataBR.substring(0, 2);
    mes = dataBR.substring(3, 5);
    ano = dataBR.substring(6, 10);
    return ano+"/"+mes+"/"+dia;
}

function DatetimePAraYYYYMMDDbarras(data) {
    dia = data.getDate();
    mes = data.getMonth() + 1;
    ano = data.getFullYear();

    if (dia < 10) {
        dia = "0" + dia;
    }
    if (mes < 10) {
        mes = "0" + mes;
    }

    return ano+"/"+mes+"/"+dia;
}
function DatetimePAraDDMMYYYYbarras(data) {
    dia = data.getDate();
    mes = data.getMonth() + 1;
    ano = data.getFullYear();

    if (dia < 10) {
        dia = "0" + dia;
    }
    if (mes < 10) {
        mes = "0" + mes;
    }

    return dia + "/" + mes + "/" + ano;
}


function ProximaDataDiaSemana(data, limiteData) {
    var proximaData = new Date(data.getFullYear(), data.getMonth(), data.getDate());
    proximaData.setDate(data.getDate()+1);

    while (!isDiaSemana(proximaData)) {
        proximaData.setDate(proximaData.getDate() + 1);
    }
    //se a proxima data for menor que a data passada, entao fica com a data limite
    if (comparaDatas(DatetimePAraDDMMYYYYbarras(proximaData), DatetimePAraDDMMYYYYbarras(limiteData)) > 0) {
        proximaData = new Date(limiteData.getFullYear(), limiteData.getMonth(), limiteData.getDate());
    }

    return proximaData;
}