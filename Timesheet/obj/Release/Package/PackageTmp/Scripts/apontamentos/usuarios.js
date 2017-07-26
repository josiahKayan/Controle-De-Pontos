
var relativepath = "~";
var initialcitycode = "";

function validaPerfis() {
    var perfis = document.getElementsByName('checkperfil');
    for (icont = 0; icont < perfis.length; icont++) {
        if (perfis[icont].checked) {
            return true;
        }
    }
    return false;
}

//function validaLogin(login) {
//    if ( login == "" ) {
//    }/
//}

function insere() {
    //valida as datas de inicio e termino do projeto
    var compareFinPlann = (comparaDatas($("#VALIDTO").val(), $("#VALIDFROM").val()) > 0);
    var selecionouPerfil = validaPerfis();
    if (!compareFinPlann ) {
        showAlert('error', 'A data de final da validade deve ser maior que a data inicial. Revise as datas e tente novamente.');
    } else if (!selecionouPerfil) {
        showAlert('error', 'Informe um perfil para o usu&aacute;rio.');
    } else {
        document.getElementsByTagName('form')[0].submit();
    }
}

function update() {
    //valida as datas de inicio e termino do projeto
    var compareFinPlann = (comparaDatas($("#VALIDTO").val(), $("#VALIDFROM").val()) > 0);
    var selecionouPerfil = validaPerfis();
    if (!compareFinPlann) {
        showAlert('error', 'A data de final da validade deve ser maior que a data inicial. Revise as datas e tente novamente.');
    } else if (!selecionouPerfil) {
        showAlert('error', 'Informe um perfil para o usu&aacute;rio.');
    } else {
        document.getElementsByTagName('form')[0].submit();
    }
}

function initialize() {

    $("#USERNAME").attr("size", 50);
    $("#USERNAME").attr("maxlength", 30);
    $("#USERNAME").css("width", 300);
    $("#USERNAME").css("text-transform", "uppercase");

    $("#VALIDFROM").attr({ class: "calendario-format" });
    $("#VALIDFROM").css("width", "70");
    $("#VALIDFROM").mask("99/99/9999");

    $("#VALIDTO").attr({ class: "calendario-format" });
    $("#VALIDTO").css("width", "70");
    $("#VALIDTO").mask("99/99/9999");

    $(".calendario-format").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    });
    $(".calendario-format").change(function () {
        if (!validarData($(this).val())) {
            $(this).val("");
        }
    });


}

function cancelar() {
    window.open(relativepath + 'Usuarios', '_self');
}

