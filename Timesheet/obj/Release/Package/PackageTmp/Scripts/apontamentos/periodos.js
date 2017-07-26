
var relativepath = "~";
var initialcitycode = "";


function initializeMask() {
   $("#NAME").css("width", "400");
   $("#DESCRIPTION").css("width", "400");
   $("#DESCRIPTION").css("height", "60");
   $("#CUSTOMERWBS").css("width", "400");

   $("#PLANNEDSTARTDATE").attr({ class: "calendario-format" });
   $("#PLANNEDSTARTDATE").css("width", "70");
   $("#PLANNEDSTARTDATE").mask("99/99/9999");

   $("#ACTUALSTARTDATE").attr({ class: "calendario-format" });
   $("#ACTUALSTARTDATE").css("width", "70");
   $("#ACTUALSTARTDATE").mask("99/99/9999");

   $("#PLANNEDFINISHDATE").attr({ class: "calendario-format" });
   $("#PLANNEDFINISHDATE").css("width", "70");
   $("#PLANNEDFINISHDATE").mask("99/99/9999");

   $("#ACTUALFINISHDATE").attr({ class: "calendario-format" });
   $("#ACTUALFINISHDATE").css("width", "70");
   $("#ACTUALFINISHDATE").mask("99/99/9999");

   $("#selectempresa").css("width", "404");
   $("#selectgestor").css("width", "404");
   $("#selectstatus").css("width", "404");


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



function chamaCalendario(){
   $( "#datepicker" ).datepicker();
}













//function insereitem() {

//   //valida as datas de inicio e termino do projeto
//   var compareFinPlann = (comparaDatas($("#PLANNEDFINISHDATE").val(), $("#PLANNEDSTARTDATE").val()) > 0);
//   var compareFinStart = (comparaDatas($("#ACTUALFINISHDATE").val(), $("#ACTUALSTARTDATE").val()) > 0);

//   if (document.getElementById("NAME").value == "") {
//      showAlert('error', 'Informe o nome do projeto.');
//   }
//   else if (document.getElementById('PLANNEDFINISHDATE').value == "") {
//      showAlert('error', 'O campo: Término Planejado é obrigatório');
//      document.getElementById('PLANNEDFINISHDATE').style.borderColor = "red";
//   }

//   else if (compareFinPlann && compareFinStart) {
//      document.getElementsByTagName('form')[0].submit();
//   } else {
//      showAlert('error', 'As datas do final do projeto devem ser maiores que a data de início. Revise as datas e tente novamente.');
//   }
//}
//function voltar() {
//   window.open(relativepath + 'Projetos', '_self');
//}


//function dataValida(data) {
//    var st = data.replace("/",".");
//    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
//    var dt = new Date(st.replace(pattern, '$3-$2-$1'));
//}

