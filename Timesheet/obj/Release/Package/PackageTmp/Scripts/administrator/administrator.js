var name = "";
var periodosresetar = "";

function Senha(item)
{
  name = item;
  chamaDialogo();
}

function Senha() {
  chamaDialogo();
}

function chamaDialogo() {
  $(function () {
    $("#mensagem").dialog();
  });

}

function reiniciarSenha() {


  var x = processAjax("Resetar", ""+periodosresetar);
  $("#mensagem").dialog("close");
  confirmaDialogo();
}

function voltar() {
  $("#mensagem").dialog("close");
}

function confirmaDialogo() {
  $(function () {
    $("#confirma").dialog();
  });
}

function processAjax(action, username) {
    var urlCrypt = "/Usuarios/" + action + "/";
    $.ajax({
    type: "POST",
    url: urlCrypt,
    data: { username: username },
    success: function (data) {
      return "ok";
    },
    fail: function (x) {
    }
  });
}








function getmarcados() {
  var stringchecados = "";
  var itemschecks = document.getElementsByClassName("checkitemclass");

  for (icont = 0; icont < itemschecks.length; icont++) {

    if (itemschecks[icont].checked) {
      stringchecados = stringchecados + itemschecks[icont].id.substr(8) + ",";
    }
  }

  if (stringchecados.length > 0) {
    stringchecados = stringchecados.substr(0, stringchecados.length - 1);
  }
  return stringchecados;
}

function resetarMarcados() {
  var periodosresetar = getmarcados();
}

function openDialogResetar() {
  periodosresetar = getmarcados();
  if (periodosresetar.length > 0) {
    $("#mensagem").dialog();
  }
}




