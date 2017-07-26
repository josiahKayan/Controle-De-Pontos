var name = "";

function Senha(item) {
  console.log(item);
  name = item;
  var r = confirm("Deseja Reiniciar a senha do usuário?");
  if (r == true) {
    chamaDialogo();
    alert("SENHA ALTERADA PARA VALOR PADRÃO!!");
    window.open(relativepath + 'Login/', '_top');
  }
}

function chamaDialogo() {
  var username = name;
  processAjax("Resetar", username);
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


