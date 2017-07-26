


function novoItem() {
   window.open(relativepath + 'Projetos/Create', '_top');
}
function getselecionados() {
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
function apagarmarcados() {
   var periodosexcluir = getselecionados();
   $("#checados").val(periodosexcluir);
   document.getElementById('formexcluir').action = relativepath + "Projetos/Excluir";
   document.getElementById('formexcluir').method = "post";
   document.getElementById('formexcluir').submit();
}
function openDialogExclusao() {
   var periodosexcluir = getselecionados();
   if (periodosexcluir.length > 0) {
      $("#dialog-confirm").dialog("open");
   }
}



function voltar() {
   $("#mensagem").dialog("close");
}

function confirmaDialogo() {
   $(function () {
      $("#confirma").dialog();
   });
}



var name = "";
var periodosresetar = "";

function Senha(item) {
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