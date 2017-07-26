

function insere() {

    if ($("#selectcountry").val() != "1058") {
        $("#selectcity").val(0);
    }

    document.getElementsByTagName('form')[0].submit();

}

function update() {

    if ($("#selectcountry").val() != "1058") {
        $("#selectcity").val(0);
    }

    document.getElementsByTagName('form')[0].submit();
    
}

function initialize() {

    $("#NAME").attr("size", 50);
    $("#NAME").attr("maxlength", 60);
    $("#NAME").css("width", 300);

    $("#SHORTNAME").attr("size", 40);
    $("#SHORTNAME").attr("maxlength", 40);
    $("#SHORTNAME").css("width", 240);

    $("#FIRSTNAME").attr("size", 30);
    $("#FIRSTNAME").attr("maxlength", 30);
    $("#FIRSTNAME").css("width", 180);

    $("#LASTNAME").attr("size", 30);
    $("#LASTNAME").attr("maxlength", 30);
    $("#LASTNAME").css("width", 180);

    $("#ADDRESS").attr("size", 50);
    $("#ADDRESS").attr("maxlength", 150);
    $("#ADDRESS").css("width", 300);

    $("#NUMBER").attr("size", 10);
    $("#NUMBER").attr("maxlength", 10);
    $("#NUMBER").css("width", 50);

    $("#COMPLEMENT").attr("size", 50);
    $("#COMPLEMENT").attr("maxlength", 100);
    $("#COMPLEMENT").css("width", 300);

    $("#DISTRICT").attr("size", 50);
    $("#DISTRICT").attr("maxlength", 60);
    $("#DISTRICT").css("width", 300);

    $("#CITYID").attr("size", 10);
    $("#CITYID").attr("maxlength", 10);
    $("#CITYID").css("width", 50);

    $("#COUNTRYID").attr("size", 10);
    $("#COUNTRYID").attr("maxlength", 10);
    $("#COUNTRYID").css("width", 50);

    $("#EMAIL").attr("size", 50);
    $("#EMAIL").attr("maxlength", 100);
    $("#EMAIL").css("width", 300);

    $("#TELEPHONENUMBER").attr("size", 20);
    $("#TELEPHONENUMBER").attr("maxlength", 20);
    $("#TELEPHONENUMBER").attr("class", "campotelefone");
    $("#TELEPHONENUMBER").css("width", 100);


    $("#TELEPHONEEXTENSION").attr("size", 20);
    $("#TELEPHONEEXTENSION").attr("maxlength", 20);
    $("#TELEPHONEEXTENSION").attr("class", "campotelefone");
    $("#TELEPHONEEXTENSION").css("width", 100);

    $("#MOBILEPHONENUMBER").attr("size", 20);
    $("#MOBILEPHONENUMBER").attr("maxlength", 20);
    $("#MOBILEPHONENUMBER").attr("class", "campotelefone");
    $("#MOBILEPHONENUMBER").css("width", 100);

    $("#CPFCNPJ").attr("size", 20);
    $("#CPFCNPJ").attr("maxlength", 20);
    $("#CPFCNPJ").css("width", 100);
    $("#CPFCNPJ").mask("99.999.999/9999-99");

    $("#INSCRICAOESTADUAL").attr("size", 20);
    $("#INSCRICAOESTADUAL").attr("maxlength", 20);
    $("#INSCRICAOESTADUAL").css("width", 100);

    $("#INSCRICAOMUNICIPAL").attr("size", 20);
    $("#INSCRICAOMUNICIPAL").attr("maxlength", 20);
    $("#INSCRICAOMUNICIPAL").css("width", 100);

    $("#ZIP").attr("size", 10);
    $("#ZIP").attr("maxlength", 10);
    $("#ZIP").css("width", 50);

    $("#CITYID").css("display", 50);
    $("#CITYID").css("display", 50);

    $("#selectcountry").val(1058);
    $("#selectcity").val(1302603);

    $("#selectcountry").change(function () {
        if (this.value == "1058") {
            $("#selectcity").css("display", "");
            $("#selectcity").attr("readonly", false);
        } else {
            $("#selectcity").css("display", "");
            $("#selectcity").attr("readonly", true);
            $("#selectcity").val("");
        }
    });


    $(".campotelefone").mask("(99) 9?9999999");

}
