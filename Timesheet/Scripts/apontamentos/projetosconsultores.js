
var relativepath = "~";
var countselecteds = 0;


function capturaConsultoresAlocados() {
   countselecteds = 0;
   $("#to_select_list option").each(function () {
      var valueSelect = $(this).val();
      countselecteds++;
      //valida apenas os novos
      if (valueSelect != "-" &&
          valueSelect.substring(0, 1) != "+") {
         var value = document.getElementById("idsconsultoresselecionados").value;
         value = "," + value + $(this).val() + ",";
         document.getElementById("idsconsultoresselecionados").value = value;
      }
   });

   return (document.getElementById("idsconsultoresselecionados").value != "");
}

function salvar() {
   var mensagem = "";
   countselecteds = 0;
   capturaConsultoresAlocados();
   document.getElementById('formapontamentos').action = relativepath + "ProjetosConsultores/Save/";
   document.getElementById('formapontamentos').method = "post";
   document.getElementById('formapontamentos').submit();
}


function cancelar() {
   document.getElementById('formapontamentos').action = relativepath + "ProjetosConsultores/SelectionProjectConsultant/";
   document.getElementById('formapontamentos').method = "post";
   document.getElementById('formapontamentos').submit();
}


function mudarprojeto() {
   desabilitaItensPorProjeto();
   document.getElementById('formapontamentos').action = relativepath + "ProjetosConsultores/SelectionProjectConsultant/";
   document.getElementById('formapontamentos').method = "post";
   document.getElementById('formapontamentos').submit();
}

function mudarperiodo(periodoselecionado) {
   var periodid = periodoselecionado.value;
   desabilitaItens();
   document.getElementById('formapontamentos').action = relativepath + "ProjetosConsultores/SelectionProjectConsultant/";
   document.getElementById('formapontamentos').method = "post";
   document.getElementById('formapontamentos').submit();
}

//this will move selected items from source list to destination list     
function move_list_items(sourceid, destinationid) {
   $("#" + sourceid + " option:selected").appendTo("#" + destinationid);
}

//this will move all selected items from source list to destination list
function move_list_items_all(sourceid, destinationid) {
   $("#" + sourceid + " option").appendTo("#" + destinationid);
}


function desabilitaItens() {

   //document.getElementById('selectperiodo').disabled = true;
   document.getElementById('selectprojeto').disabled = true;
   document.getElementById('btsalvar').disabled = true;
   document.getElementById('btenviar').disabled = true;
   document.getElementById("from_select_list").disabled = true;
   document.getElementById("moveright").disabled = true;
   document.getElementById("moverightall").disabled = true;
   document.getElementById("moveleft").disabled = true;
   document.getElementById("moveleftall").disabled = true;
   document.getElementById("to_select_list").disabled = true;

   document.getElementById("to_select_list").disabled = true;
}

function desabilitaItensPorProjeto() {

   //document.getElementById("selectperiodo").disabled = true;
   //document.getElementById("selectprojeto").disabled = true;
   document.getElementById("btsalvar").disabled = true;
   document.getElementById("btenviar").disabled = true;

   document.getElementById("from_select_list").disabled = true;
   document.getElementById("moveright").disabled = true;
   document.getElementById("moverightall").disabled = true;
   document.getElementById("moveleft").disabled = true;

   document.getElementById("moveleftall").disabled = true;
   document.getElementById("to_select_list").disabled = true;
}

function desabilitaEnquantoProjetoForBranco() {

   document.getElementById("btsalvar").disabled = true;
   document.getElementById("btenviar").disabled = true;
   document.getElementById("from_select_list").disabled = true;
   document.getElementById("moveright").disabled = true;
   document.getElementById("moverightall").disabled = true;
   document.getElementById("moveleft").disabled = true;

   document.getElementById("moveleftall").disabled = true;
   document.getElementById("to_select_list").disabled = true;
}

function habilitaEnquantoProjetoForBranco() {

   document.getElementById("btsalvar").disabled = false;
   document.getElementById("btenviar").disabled = false;
   document.getElementById("from_select_list").disabled = false;
   document.getElementById("moveright").disabled = false;
   document.getElementById("moverightall").disabled = false;
   document.getElementById("moveleft").disabled = false;

   document.getElementById("moveleftall").disabled = false;
   document.getElementById("to_select_list").disabled = false;
}