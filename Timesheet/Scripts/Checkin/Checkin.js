


function salvar() {
   if (isNulo())
   {
      showAlert("error", "O campo Observação precisa ser preenchido");
   }
   else
   {
      var mensagem = "";
      var validaEntradaSaida = true;
      var validaTempoIntervalo = true;
      //verifica se as horas de saida estao maior que a hora de entrada, ou intervalo maior que as horas trabalhadas
      $(".campohorasimples").each(function (index, value) {
         if (value.style.display != "none") {
            if (!validaHoraEntradaSaida(value)) {
               validaEntradaSaida = false;
            } else if (!validaIntervalo(value)) {
               validaTempoIntervalo = false;
            }
         }
      });

      if (validaEntradaSaida && validaTempoIntervalo) {
         document.getElementById('formcheckin').action = relativepath + "Checkin/Salvar/";
         document.getElementById('formcheckin').method = "post";
         document.getElementById('formcheckin').submit();
      }
      else {
         showAlert("error", "Algumas horas de início, término e intervalo não foram validadas. Verifique os dados informados e tente novamente");
      }
   }
}

function esconderSalvar() {

   document.getElementById("btsalvar").disabled = true;

}

function mostrarSalvar() {

   document.getElementById("btsalvar").disabled = false;

}

function adicionarlinhaA() {

   var divcopiarHTML = $("#div_insert_").html();
   var numeroultimalinha = document.getElementById("containsert").value;
   //prepara adicionar linha
   numeroultimalinha++;
   document.getElementById("containsert").value = numeroultimalinha;
   //alterar os ids para ficar de acordo com a ultima linha


   divcopiarHTML = divcopiarHTML.replace("tradd_apontamento_", "tradd_apontamento_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("idapont_check_insert_id_", "idapont_check_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("idapont_check_insert_name_", "idapont_check_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("idapont_insert_id_", "idapont_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("idapont_insert_name_", "idapont_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("idapont_insert_value_", "idapont_insert_value_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("_selectDescricaoData_insert_id_", "selectDescricaoData_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("_selectDescricaoData_insert_name_", "selectDescricaoData_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("_selectDescricaoData_insert_value_", "selectDescricaoData_insert_value_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("_selectdata_insert_id_", "_selectdata_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("_selectdata_insert_name_", "_selectdata_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("_selecttipoentrada_insert_id_", "_selecttipoentrada_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("_selecttipoentrada_insert_name_", "_selecttipoentrada_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("entrada_insert_id_", "entrada_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("entrada_insert_name_", "entrada_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("saida_insert_id_", "saida_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("saida_insert_name_", "saida_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("intervalo_insert_id_", "intervalo_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("intervalo_insert_name_", "intervalo_insert_" + numeroultimalinha);

  


   divcopiarHTML = divcopiarHTML.replace("observacao_insert_id_", "observacao_insert_" + numeroultimalinha);
   divcopiarHTML = divcopiarHTML.replace("observacao_insert_name_", "observacao_insert_" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("idanchor_id", "idanchor_id" + numeroultimalinha);

   divcopiarHTML = divcopiarHTML.replace("<table>", "");
   divcopiarHTML = divcopiarHTML.replace("</table>", "");
   divcopiarHTML = divcopiarHTML.replace("<tbody>", "");
   divcopiarHTML = divcopiarHTML.replace("</tbody>", "");

   $("#tabelacheckin tbody").append(divcopiarHTML);

   initializeMasks();

   //seta focus na linha adicionada, campo entrada.

   window.setTimeout(function () {
      //seta a proxima data
      focusUltimaLinha(numeroultimalinha);
   }, 0);
   //$('body').scrollTo("#" + "idanchor_id" + numeroultimalinha);
   //$("#entrada_insert_" + numeroultimalinha).focus();

   $('html, body').scrollTop($(document).height());


}

function focusUltimaLinha(numeroLinha) {
   proximaData = ProximaDataDiaSemana(proximaData, limiteData);
   var proximaDataSelect = DatetimePAraYYYYMMDDbarras(proximaData);
   $("#_selectdata_insert_" + numeroLinha).val(proximaDataSelect);
   //document.getElementById("_selectdata_insert_" + numeroLinha).value = proximaDataSelect;
   document.getElementById("entrada_insert_" + numeroLinha).focus();
}


function checkar(objetochecado, position) {
   var stridschecks = document.getElementById('idscheckeds').value;
   var idcheck = document.getElementById('idapont_' + position).value;
   if (objetochecado.checked) {
      document.getElementById('idscheckeds').value = stridschecks + idcheck + ",";
   } else {
      document.getElementById('idscheckeds').value = stridschecks.replace(idcheck + ",", "");
   }

}


function checkaradicionar(objetochecado) {
   var position = objetochecado.id.substr(21);

   var stridschecks = document.getElementById('idscheckedsnovos').value;
   var idcheck = position;
   if (objetochecado.checked) {
      document.getElementById('idscheckedsnovos').value = stridschecks + idcheck + ",";
   } else {
      document.getElementById('idscheckedsnovos').value = stridschecks.replace(idcheck + ",", "");
   }

}

function apagarmarcados() {

   //marcaOcultos();
   var stridschecks = document.getElementById('idscheckeds').value;
   var stridschecksnovos = document.getElementById('idscheckedsnovos').value;
   var idsexcluir = document.getElementById('idsexcluir').value;

   if (stridschecks.length > 0) {
      var arrayids = stridschecks.split(",");
      for (cont = 0; cont < arrayids.length; cont++) {
         if (arrayids[cont] != "") {
            //alert(arrayids[cont]);

            document.getElementById("trapontamento_" + arrayids[cont]).style.display = "none";
            idsexcluir = idsexcluir + "," + arrayids[cont] + ",";
         }
      }
      document.getElementById("idsexcluir").value = idsexcluir;
   }
   if (stridschecksnovos.length > 0) {
      var arrayids = stridschecksnovos.split(",");
      for (cont = 0; cont < arrayids.length; cont++) {
         if (arrayids[cont] != "") {
            document.getElementById("tradd_apontamento_" + arrayids[cont]).style.display = "none";
         }
      }
   }

   //document.getElementById('idscheckeds').value = "";
   document.getElementById('idscheckedsnovos').value = "";

}

function mudarperiodo(periodoselecionado) {
  desabilita();
   var periodid = periodoselecionado.value;
   document.getElementById('formcheckin').action = relativepath + "Checkin/Checkin";
   document.getElementById('formcheckin').method = "post";
   document.getElementById('formcheckin').submit();
}

function cancelar() {
   document.getElementById('formcheckin').action = relativepath + "Checkin/Checkin";
   document.getElementById('formcheckin').method = "post";
   document.getElementById('formcheckin').submit();
}

function apontamentosrelatorio() {
   document.getElementById('formcheckin').action = relativepath + "Checkin/ExportToExcel/";
   document.getElementById('formcheckin').method = "post";
   document.getElementById('formcheckin').submit();
}

function CallScheduler() {
   document.getElementById('formcheckin').action = relativepath + "Checkin/CallScheduler/";
   document.getElementById('formcheckin').method = "post";
   document.getElementById('formcheckin').submit();
}

function isNulo() {
   
   var x = document.getElementsByClassName('textareagrid');
   //var x = document.getElementById("tradd_apontamento_" + arrayids[cont]).style.display = "none";
   var i = 0;
   var text;
   var display ;
   var branco = "";

   for (i = 0; i < x.length-1; i++) {
      text = x[i].value;
      if (text === branco) {
          x[i].style.borderColor = "red";
         return true;
      }
   }
   return false;
   
}


//function marcaOcultos() {
//   var x = document.getElementsByClassName('textareagrid');
//   var i = 0;
//   var text;
//   var branco = "";

//   for (i = 0; i < x.length - 1; i++) {
//      text = x[i].value;
//      if (text === branco) {
//         x[i].value = "delete";
         
//      }
//   }
//}



function desabilita() {

  document.getElementById("adicionarlinha").disabled = true;
  document.getElementById("excluirmarcados").disabled = true;
  document.getElementById("btsalvar").disabled = true;
  document.getElementById("btenviar").disabled = true;
  

}