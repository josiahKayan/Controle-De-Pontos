
var relativepath = "/";


$(document).ready(function () {
    $("#btnlogin").click(function () {
        loginfunction();
    });
});


//SCRIPT DE INICIO DA PAGINA
$(document).ready(function () {
    $(".username").focus(function () {
        $(".user-icon").css("left", "-48px");
    });
    $(".username").blur(function () {
        $(".user-icon").css("left", "0px");
    });

    $(".password").focus(function () {
        $(".pass-icon").css("left", "-48px");
    });
    $(".password").blur(function () {
        $(".pass-icon").css("left", "0px");
    });
    $("#btnloginConsultor").click(function () {
        loginfunction('consultor');
    });
    $("#btnloginGestor").click(function () {
        loginfunction('gestor');
    });

    $('#alert').click(function () {
        closeAlert();
    });

    

});


function loginfunction(tipo) {
    var mensagem = "";
    var loginid = document.getElementById('loginid').value;
    var password = document.getElementById('password').value;
    if (loginid == "Login ID" || loginid.value == "") {
        mensagem = "&nbsp;&nbsp;Informe o login do usu&aacuterio.";
    } else if (password == "Password" || password == "") {
        mensagem = "&nbsp;&nbsp;Informe a senha do usu&aacuterio.";
    }
    if (mensagem == "") {
      if (tipo == 'consultor') {
        desabilita();
            document.getElementById('login-form').action = relativepath + "Login/Acessar";
            document.getElementById('login-form').method = "post";
            document.getElementById('login-form').submit();
            //desabilita();
            //window.open('/Login/Acessar','_top');
        }
    } else {
        showAlert('error', mensagem);
    }
}


function desabilita() {
  document.getElementById("btnloginConsultor").disabled = true;
}
