function ExportToExcel(item) {
   var itemSelecionado = document.getElementById("selectreport").value;

   var periodoInicial = document.getElementById("selectperiodoinicial").options[document.getElementById("selectperiodoinicial").selectedIndex].text;
   var periodoFinal = document.getElementById("selectperiodoFinal").options[document.getElementById("selectperiodoFinal").selectedIndex].text;
   var pjselect = document.getElementById("selectprojeto").options[document.getElementById("selectprojeto").selectedIndex].text;
   var consultorSelect = document.getElementById("selectconsultor").options[document.getElementById("selectconsultor").selectedIndex].text;

   var dataInicial = periodoInicial.split("/");
   var dataFinal = periodoFinal.split("/");
   
   if (consultorSelect == "" && pjselect == "" && dataInicial[0] != dataFinal[0]) {
      window.alert(" Selecione pelo menos um consultor ou um projeto  ");
   }
   else {

      if (dataInicial[1] <= dataFinal[1]) {
         if (dataInicial[0] <= dataFinal[0]) {
            if (!itemSelecionado == "") {
              if (itemSelecionado == "Relatório Mensal de Projetos") {
                  document.getElementById('formrelatorios').action = relativepath + "Relatorios/ExportToExcelRelatorioMensalProjetos/";
               }
               else {
                  document.getElementById('formrelatorios').action = relativepath + "Relatorios/ExportToExcelHourForProject/";
               }
               document.getElementById('formrelatorios').method = "post";
               document.getElementById('formrelatorios').submit();
            }
         }
         else {
            window.alert("O Período Inicial precisa ser menor do que o Período Final");
         }
      }
      else {
         window.alert("O Período Inicial precisa ser menor do que o Período Final");
      }
   }
}


function desabilita() {
   document.getElementById("selectperiodoinicial").disabled = true;
   document.getElementById("selectperiodoFinal").disabled = true;
   document.getElementById("selectconsultor").disabled = true;
   document.getElementById("selectreport").disabled = true;
   document.getElementById("btbaixar").disabled = true;
}

function desabilitaTudo() {
  document.getElementById("selectperiodoinicial").disabled = true;
  document.getElementById("selectperiodoFinal").disabled = true;
  document.getElementById("selectconsultor").disabled = true;
  document.getElementById("selectreport").disabled = true;
  document.getElementById("selectprojeto").disabled = true;
}

function carregaConsultores() {
   desabilita();
   document.getElementById('formrelatorios').action = relativepath + "Relatorios/Relatorios/";
   document.getElementById('formrelatorios').method = "post";
   document.getElementById('formrelatorios').submit();
}

function processAjax(action, projeto) {
   var retorno;
   var urlCrypt = "/Relatorios/" + action + "/";
   $.ajax({
      type: "POST",
      url: urlCrypt,
      async: false,
      data: { id: projeto },
      success: function (data) {
         retorno = data;
      }
   });
   return retorno;
}


