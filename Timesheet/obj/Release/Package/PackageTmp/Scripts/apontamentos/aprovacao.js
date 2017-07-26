
var relativepath = "~";


function salvar() {
    var mensagem = "";
    document.getElementById('formapontamentos').action = relativepath + "Gestor/AprovacaoSalvar/";
    document.getElementById('formapontamentos').method = "post";
    document.getElementById('formapontamentos').submit();
}

function adicionarlinha() {

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


    divcopiarHTML = divcopiarHTML.replace("_selectdata_insert_id_", "_selectdata_insert_" + numeroultimalinha);
    divcopiarHTML = divcopiarHTML.replace("_selectdata_insert_name_", "_selectdata_insert_" + numeroultimalinha);


    divcopiarHTML = divcopiarHTML.replace("_selectprojeto_insert_id_", "_selectprojeto_insert_" + numeroultimalinha);
    divcopiarHTML = divcopiarHTML.replace("_selectprojeto_insert_name_", "_selectprojeto_insert_" + numeroultimalinha);


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

    divcopiarHTML = divcopiarHTML.replace("<table>", "");
    divcopiarHTML = divcopiarHTML.replace("</table>", "");
    divcopiarHTML = divcopiarHTML.replace("<tbody>", "");
    divcopiarHTML = divcopiarHTML.replace("</tbody>", "");
    $("#tabelaapontamentos tbody").append(divcopiarHTML);

    initializeMasks();

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
    desabilitaItens();
    var periodid = periodoselecionado.value;
    document.getElementById('formapontamentos').action = relativepath + "Gestor/Aprovacao/";
    document.getElementById('formapontamentos').method = "post";
    document.getElementById('formapontamentos').submit();
}

function mudargrupo(grupo) {
    var grupoid = grupo.value;
    document.getElementById('formapontamentos').action = relativepath + "Gestor/Aprovacao/";
    document.getElementById('formapontamentos').method = "post";
    document.getElementById('formapontamentos').submit();
}


function cancelar() {
    document.getElementById('formapontamentos').action = relativepath + "Gestor/Aprovacao/";
    document.getElementById('formapontamentos').method = "post";
    document.getElementById('formapontamentos').submit();
}

function apontamentosrelatorio() {
    var itemSelecionado = document.getElementById("selectrelatorio").value;
  
    if (  !itemSelecionado == "") {
        if (itemSelecionado == "Relatório Mensal de Projetos")
        {
           document.getElementById('formapontamentos').action = relativepath + "Gestor/ExportToExcel/";
        }
        else
        {
            document.getElementById('formapontamentos').action = relativepath + "Gestor/ExportToExcelHours/";
        }
        document.getElementById('formapontamentos').method = "post";
        document.getElementById('formapontamentos').submit();
    }

}

/**
String.prototype.replaceAll = function (de, para) {
    var str = this;
    var pos = str.indexOf(de);
    while (pos > -1) {
        str = str.replace(de, para);
        pos = str.indexOf(de);
    }
    return (str);
}

**/

function desabilitaItens() {
   document.getElementById("btsalvar").disabled = true;
   document.getElementById("selectrelatorio").disabled = true;
   document.getElementById("btbaixar").disabled = true;
}