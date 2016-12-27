
var relativepath = "~";
var initialcitycode = "";

function insere() {
    console.log("Entrou aqui");
    if ($("#selectcountry").val() != "1058") {
        $("#selectcity").val(0);
    }

    if (document.getElementById('grupo').value == null || document.getElementById('grupo').value == "") {
        document.getElementById('grupo').value = ".";
    }


    //se for usuario, verifica se as senhas estao iguais e nao vazias e o login nao esta vazio
    var isSubmit = true;
    if ($("#checkisusuario").prop("checked")) {
        if ($("#loginusuario").val() == "") {
            showAlert("error", "Informe o login ID para acesso ao sistema.");
            isSubmit = false;
        }
    }

    if (document.getElementById("selecttipo").value == "1") {
        if (!ValidarCNPJ()) {
            isSubmit = false;
        }
    } else {
        if (!ValidarCPF()) {
            isSubmit = false;
        }
    }


    if (isSubmit) {
        //se for pessoa juridica, apaga os campos primeiro e ultimo nome
        if (document.getElementById("selecttipo").value == "1") {
            $("#FIRSTNAME").val("");
            $("#LASTNAME").val("");
        }
        document.getElementsByTagName('form')[0].submit();
    }

}

function insereGetReturn() {

    if ($("#selectcountry").val() != "1058") {
        $("#selectcity").val(0);
    }

    //se for usuario, verifica se as senhas estao iguais e nao vazias e o login nao esta vazio
    var isSubmit = true;
    if ($("#checkisusuario").prop("checked")) {
        if ($("#loginusuario").val() == "") {
            showAlert("error", "Informe o login ID para acesso ao sistema.");
            isSubmit = false;
        }
    }

    if (document.getElementById("selecttipo").value == "1") {
        if (!ValidarCNPJ()) {
            isSubmit = false;
        }
    } else {
        if (!ValidarCPF()) {
            isSubmit = false;
        }
    }


    if (isSubmit) {
        //se for pessoa juridica, apaga os campos primeiro e ultimo nome
        if (document.getElementById("selecttipo").value == "1") {
            $("#FIRSTNAME").val("");
            $("#LASTNAME").val("");
        }
        
        document.getElementsByTagName('form')[0].action = relativepath +"Cliente/CreateGetReturn";
        document.getElementsByTagName('form')[0].submit();
    }

}



function update() {

    if ($("#selectcountry").val() != "1058") {
        $("#selectcity").val(0);
    }
    //se for usuario, verifica se as senhas estao iguais e nao vazias e o login nao esta vazio
    var isSubmit = true;

    if (document.getElementById("selecttipo").value == "1") {
        if (!ValidarCPF()) {
            isSubmit = false;
        }
    } else {
        if (!ValidarCNPJ()) {
            isSubmit = false;
        }
    }

    if (isSubmit) {
        //se for pessoa juridica, apaga os campos primeiro e ultimo nome
        if (document.getElementById("selecttipo").value == "1") {
            $("#FIRSTNAME").val("");
            $("#LASTNAME").val("");
        }
        document.getElementsByTagName('form')[0].submit();
    }

}

function initialize(citycode) {
    initialcitycode = citycode;

    $("#NAME").attr("size", 50);
    $("#NAME").attr("maxlength", 60);
    $("#NAME").css("width", 360);

    $("#SHORTNAME").attr("size", 30);
    $("#SHORTNAME").attr("maxlength", 30);
    $("#SHORTNAME").css("width", 180);

    $("#FIRSTNAME").attr("size", 30);
    $("#FIRSTNAME").attr("maxlength", 30);
    $("#FIRSTNAME").css("width", 180);

    $("#LASTNAME").attr("size", 30);
    $("#LASTNAME").attr("maxlength", 30);
    $("#LASTNAME").css("width", 180);

    $("#ADDRESS").attr("size", 50);
    $("#ADDRESS").attr("maxlength", 150);
    $("#ADDRESS").css("width", 360);

    $("#NUMBER").attr("size", 10);
    $("#NUMBER").attr("maxlength", 10);
    $("#NUMBER").css("width", 100);

    $("#COMPLEMENT").attr("size", 50);
    $("#COMPLEMENT").attr("maxlength", 100);
    $("#COMPLEMENT").css("width", 360);

    $("#DISTRICT").attr("size", 50);
    $("#DISTRICT").attr("maxlength", 60);
    $("#DISTRICT").css("width", 360);

    $("#CITYID").attr("size", 10);
    $("#CITYID").attr("maxlength", 10);
    $("#CITYID").css("width", 100);

    $("#COUNTRYID").attr("size", 10);
    $("#COUNTRYID").attr("maxlength", 10);
    $("#COUNTRYID").css("width", 100);

    $("#EMAIL").attr("size", 50);
    $("#EMAIL").attr("maxlength", 100);
    $("#EMAIL").css("width", 360);

    $("#USERGROUP").attr("size", 50);
    $("#USERGROUP").attr("maxlength", 20);
    $("#USERGROUP").css("width", 180);

    $("#TELEPHONENUMBER").attr("size", 20);
    $("#TELEPHONENUMBER").attr("maxlength", 20);
    $("#TELEPHONENUMBER").attr("class", "campotelefone");
    $("#TELEPHONENUMBER").css("width", 120);


    $("#TELEPHONEEXTENSION").attr("size", 20);
    $("#TELEPHONEEXTENSION").attr("maxlength", 20);
    $("#TELEPHONEEXTENSION").attr("class", "campotelefone");
    $("#TELEPHONEEXTENSION").css("width", 120);

    $("#MOBILEPHONENUMBER").attr("size", 20);
    $("#MOBILEPHONENUMBER").attr("maxlength", 20);
    $("#MOBILEPHONENUMBER").attr("class", "campotelefone");
    $("#MOBILEPHONENUMBER").css("width", 120);

    $("#INSCRICAOESTADUAL").attr("size", 20);
    $("#INSCRICAOESTADUAL").attr("maxlength", 20);
    $("#INSCRICAOESTADUAL").css("width", 120);

    $("#INSCRICAOMUNICIPAL").attr("size", 20);
    $("#INSCRICAOMUNICIPAL").attr("maxlength", 20);
    $("#INSCRICAOMUNICIPAL").css("width", 120);

    $("#ZIP").attr("size", 10);
    $("#ZIP").attr("maxlength", 10);
    $("#ZIP").css("width", 100);

    $("#CITYID").css("display", 50);
    $("#CITYID").css("display", 50);

    $("#loginusuario").attr("size", 20);
    $("#loginusuario").attr("maxlength", 20);
    $("#loginusuario").css("width", 120);

    // $("#senhasusuario").attr("size", 20);
    // $("#senhasusuario").attr("maxlength", 20);
    // $("#senhasusuario").css("width", 100);

    $("#selectcountry").val(1058);
    if (citycode == "1302603") {
        $("#selectstate").val("AM");
        $("#selectcity").val(1302603);
    } else {
        $("#selectcity").val(citycode);
    }

    $("#selectcountry").change(function () {
        if (this.value == "1058") {
            $(".cidadesdobrasil").show();
        } else {
            $(".cidadesdobrasil").hide();
            //$("#selectcity").val("");
        }
    });
    $("#selectstate").change(function () {
        atualizaCidades();
    });


    $("#CPFCNPJ").attr("size", 20);
    $("#CPFCNPJ").attr("maxlength", 20);
    $("#CPFCNPJ").css("width", 120);
    $("#selecttipo").css("width", 100);
    $("#selecttipo").change(function () {
        atualizaCpfCnpj();
    });


    $(".campotelefone").mask("+99?(99) 999999999");
    $(".campotelefone").each(function () {
        if ($(this).val() == "") {
            $(this).val("+55");
        }
    });

    $(".linhausuario").hide();

    $("#checkisusuario").change(function () {
        atualizaUsuario();
    });
    atualizaCpfCnpj();
    atualizaUsuario();
    atualizaCidades();

}


function atualizaCpfCnpj() {
    if (document.getElementById("selecttipo").value == "0") {
        $("#labelcpfcnpj").html("CPF");
        $("#CPFCNPJ").mask("999.999.999-99");
        $(".linhapessoajuridica").hide();
        $(".linhapessoafisica").show();
    } else {
        $("#labelcpfcnpj").html("CNPJ");
        $("#CPFCNPJ").mask("99.999.999/9999-99");
        $(".linhapessoajuridica").show();
        $(".linhapessoafisica").hide();
    }
}

function atualizaUsuario() {
    if (document.getElementById("checkisusuario").checked) {
        $(".linhausuario").show();
        //$("#spanusuariosistema").html("O parceiro também é usuário do sistema");

    } else {
        $(".linhausuario").hide();
        //$("#spanusuariosistema").html("O parceiro não é usuário do sistema");
    }
    $("#spanusuariosistema").html("O parceiro também é usuário do sistema");
}

//valida o CPF digitado
function ValidarCPF() {
    var cpf = $("#CPFCNPJ").val();
    if (cpf == "") {
        return true;
    }
    cpf = cpf.replace(/\D/g, ""); //Remove tudo o que não é dígito
    exp = /\.|\-/g
    cpf = cpf.toString().replace(exp, "");
    var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
    var soma1 = 0, soma2 = 0;
    var vlr = 11;

    for (i = 0; i < 9; i++) {
        soma1 += eval(cpf.charAt(i) * (vlr - 1));
        soma2 += eval(cpf.charAt(i) * vlr);
        vlr--;
    }
    soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
    soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

    var digitoGerado = (soma1 * 10) + soma2;
    if (digitoGerado != digitoDigitado) {
        showAlert("error", "CPF inválido.");
        return false;
    }
    return true;
}

//valida o CNPJ digitado
function ValidarCNPJ() {
    var cnpj = $("#CPFCNPJ").val();

    if (cnpj == "") {
        return true;
    }

    cnpj = cnpj.replace(/\D/g, ""); //Remove tudo o que não é dígito
    var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    var dig1 = new Number;
    var dig2 = new Number;

    exp = /\.|\-|\//g
    cnpj = cnpj.toString().replace(exp, "");
    var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

    for (i = 0; i < valida.length; i++) {
        dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
        dig2 += cnpj.charAt(i) * valida[i];
    }
    dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
    dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

    if (((dig1 * 10) + dig2) != digito) {
        showAlert("error", "CNPJ inválido.");
        return false;
    }
    return true;
}

function atualizaCidades() {
    //pega os dados de cidades, a partir do estado
    var UF = $("#selectstate").val();
    $.ajax({
        type: "GET",
        url: relativepath + "InfoWS/Cidades/",
        data: { id: UF},
        dataType: "json",
        success: function (json) {
            var options = "";
            $.each(json, function (key, data) {
                if (data.CITYCODE == initialcitycode) {
                    options += '<option value="' + data.CITYCODE + '" selected>' + data.CITY + '</option>';
                } else {
                    options += '<option value="' + data.CITYCODE + '">' + data.CITY + '</option>';
                }
            });
            $("#selectcity").html(options);
        }
    });


}


function cancelar() {
    window.open(relativepath + 'Cliente', '_self');
}
