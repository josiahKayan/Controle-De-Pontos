

function Procurar(item) {

    var info = document.getElementById("ProjectName").value;
    document.getElementById('formproject').action = relativepath + "InfoProjetos/Procurar/";
    document.getElementById('formproject').method = "post";
    document.getElementById('formproject').submit();

}