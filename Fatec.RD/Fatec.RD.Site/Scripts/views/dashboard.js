var apiDespesa = "http://localhost:63649/api/Despesa/";
var apiRelatorio = "http://localhost:63649/api/Relatorio/";

var somadespesa = $("#somaDespesa");
var somarelatorio = $("#somaRelatorio");

var qtdDespesa = $("#qtdDespesa");
var qtdRelatorio = $("#qtdRelatorio");

var progressodespesa = $("#progressoDespesa");
var progressorelatorio = $("#progressoRelatorio");



$(document).ready(function () {

    LoadSelect();

    GetDespesa();

    $("#relatorio-options").change(function () {
        GetDespesaPorRelatorio(this.value);
    });



});



function GetDespesa() {
    $.ajax({
        type: 'GET',
        url: apiDespesa,
        success: function (despesa) {

            MostrarQtd(qtdDespesa, despesa);
            MostrarSoma(somadespesa, despesa);
            MostrarProgresso(progressodespesa, despesa);
        },
        error: function (error) {
            console.log(error);
        }
    })
}


function GetDespesaPorRelatorio(id) {
    $.ajax({
        type: 'GET',
        url: apiRelatorio + "/" + id + "/Despesas",
        success: function (despesasRelatorio) {

            MostrarSoma(somarelatorio, despesasRelatorio);
            MostrarProgresso(progressorelatorio, despesasRelatorio);
        },
        error: function (error) {
            console.log(error);
            alert(error);
        }
    })
}



function LoadSelect() {
    $.ajax({
        type: 'GET',
        url: apiRelatorio,
        success: function (data) {
            
            array = GetArrayJSON(data);
            MostrarQtd(qtdRelatorio, data);
            PreencheSelect(array);
        },
        error: function (error) {
            console.log(error);
        }
    });
}


function MostrarQtd(elem, dataObj) {
    elem.text(dataObj.length);
}

function MostrarSoma(elem, DataObj) {
    var total = 0;
    var array = GetArrayJSON(DataObj);
    total = SomaValor(array);

    elem.text(numberToReal(total));
}


function MostrarProgresso(elem, dataObj) {
    var array = GetArrayJSON(dataObj);
    var soma = SomaValor(array);

    var media = (soma / dataObj.length) / 100;

    media *= 10;
    if (media > 100) media = 100;


    elem.attr('aria-valuenow', media).width(media + '%');
}

function numberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}


function GetArrayJSON(objeto) {
    return $.map(objeto, function (elem) {
        return elem;
    });
}

function SomaValor(array) {
    var soma = 0;
    for (var i = 0; i < array.length; i++) {
        soma += array[i].Valor;
    }
    return soma;
}


function PreencheSelect(array) {
    var option, select = $("#relatorio-options");
    $.each(array, function (index, elem) {
        option = document.createElement("option");
        option.value = elem.Id;
        option.innerText = elem.Descricao;
        select.append(option);
    });
}