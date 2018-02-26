/*********** Declarando variáveis para facilitar ***************/
var api = "http://localhost:63649/api/Relatorio/";

var apiTipoRelatorio = "http://localhost:63649/api/TipoRelatorio/"
var formulario = $("#form-relatorios");
var bodyGeneric = $("#modal-generic");
var body = $("#modal-relatorio");
var bodyDespesa = $("#modal-despesa-relatorio");
var bodyDespesaFotter = $("#modal-despesa-relatorio .modal-footer");
var modalFooter = $("#modal-relatorio .modal-footer");
var titleModal = $("#modal-title");
var botaoSalvar = $("#salvar-relatorio");
var botaoSalvarDespesaRelatorio = $("#salvarDespesaRelatorio");
var tabela = $("#tabela-relatorios");

var tipoRelatorio = $("#tipoRelatorio");
var descricao = $("#descricao");
var comentario = $("#comentario");
var dataVisualizacao = $("#data-visualizacao");
var data = $("#data");

var date = new Date();

var tabelaRelatorioDespesaAdicionada = $("#tabela-relatoriosDespesaAdicionada");
var tabelaRelatorioDespesa = $("#tabela-relatoriosDespesa");
var t, tRelatorioDespesaAdicionada, tRelatorioDespesa;



var elemento = '<button type="button" class="btn btn-info left" id="btn-add-despesa" onclick="adicionarDespesaRelatorio()">Adicionar Despesa</button>';
var Desvincular = '<button type="button" class="btn btn-danger" style="position:absolute;left:10px;" id="btn-desvincular-despesa" onclick="ExcluirDespesaRelatorio()">Desvincular</button>';
/***************************************************************/
listarTabela();

GetTipoRelatorio();

/************* Utilizando componentes em campos ****************/
$(".selecpicker").selectpicker();
//data.datepicker({
//    language: 'pt-Br',
//    todayBtn: "linked",
//    clearBtn: true,
//    todayHighlight: true
//});
data.mask('00/00/0000');
/***************************************************************/

function Reload() {
    t.reload();
    tRelatorioDespesaAdicionada.reload();
    tRelatorioDespesa.reload();
   // window.location = "/Relatorios";
}
function TipoRelatorioOptions(array) {
    var option;
    $.each(array, function (index, elem) {
        option = document.createElement("option");
        option.value = elem.Id;
        option.innerText = elem.Descricao;
        tipoRelatorio.append(option);
    });
}


function GetTipoRelatorio() {
    $.ajaxSetup({ async: false });
    var retorno;
    $.ajax({
        type: "GET",
        url: apiTipoRelatorio,
        contentType: "application/json",
        success: function (data) {

            retorno = $.map(data, function (elem) {
                return elem;
            });
        },
        error: function (error) {
            console.log(error);
        }


    });
    $.ajaxSetup({ async: true });

    TipoRelatorioOptions(retorno);
}


/************* Funções disparadas nos cliques ******************/


function novoRelatorio() {
    dataVisualizacao.val(date.toLocaleDateString());
    data.val(date.toLocaleDateString());
    formValidation();
    titleModal.html("Adicionar Relatório");
    body.modal('show');
}

function fechar() {
    formulario.validate().destroy();
    $("#btn-add-despesa").remove();
    botaoSalvar.removeAttr("disabled");
}

function adicionarDespesaRelatorio() {
    body.modal('hide');
    bodyDespesa.modal('show');
    listarTabelaRelatorioDespesaAdicionada();
    listartabelaRelatorioDespesa();
}

function SalvarDespesaRelatorio() {
    var despesasSelecionadas = $('table tbody .m-checkbox input:checked').toArray().map(function (check) {
        return $(check).val();
    });
    var obj = {};
    var objChave = [];
    var id;
    $.each(despesasSelecionadas, function (key, value) {
        objChave.push({
            IdDespesa: value
        });
    });
    obj.Chave = objChave;
    id = botaoSalvarDespesaRelatorio.attr("data-id");

    VincularDespesa(JSON.stringify(obj), id);
}

function ExcluirDespesaRelatorio() {
    var despesasSelecionadas = $('table tbody .m-checkbox input:checked').toArray().map(function (check) {
        return $(check).val();
    });
    var obj = {};
    var objChave = [];
    var id;
    $.each(despesasSelecionadas, function (key, value) {
        objChave.push({
            IdDespesa: value
        });
    });
    obj.Chave = objChave;
    id = botaoSalvarDespesaRelatorio.attr("data-id");

    DesvincularDespesa(JSON.stringify(obj), id);
}
/***************************************************************/

/************** Funções com requisições para a API *************/
function inserir(relatorio) {
    relatorio = JSON.stringify(relatorio);
    $.ajax({
        type: "POST",
        url: api,
        data: relatorio,
        contentType: "application/json",
        success: function (retorno) {
            console.log(retorno);
            modalFooter.append(elemento);
            botaoSalvar.attr("disabled", true);
            limparCampos();
            botaoSalvarDespesaRelatorio.attr("data-id", retorno.Id);
            alert("Inserido com sucesso!");
            Reload();

        },
        error: function (error) {
            console.log(error);
        }
    })
}

function alterar(id, relatorio) {
    $.ajax({
        type: 'PUT',
        url: api + '/' + id,
        data: relatorio,
        success: function () {
            alert("Alterado com sucesso!");

        },
        error: function (error) {
            console.log(error);
        }
    })
}

function excluir(id) {
    $.ajax({
        type: 'DELETE',
        url: api + '/' + id,
        success: function () {
            alert("Deletado com sucesso!");
            limparCampos();
            Reload();
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function selecionarPorId(id) {
    $.ajaxSetup({ async: false });
    var retorno;
    $.ajax({
        type: 'GET',
        url: api + id,
        success: function (relatorio) {
            retorno = relatorio;
        },
        error: function (error) {
            console.log(error);
        }
    });
    $.ajaxSetup({ async: true });
    return retorno;
}

function VincularDespesa(despesas, id) {
    console.log(despesas);
    $.ajax({
        type: 'POST',
        url: api + id + "/Despesas",
        data: despesas,
        contentType: "application/json",
        success: function () {
            alert("Inserido com sucesso!");
            bodyDespesa.modal('hide');
            Reload();

        },
        error: function (error) {
            console.log(error);
        }
    });
}

function DesvincularDespesa(despesas, id) {
    $.ajax({
        type: 'DELETE',
        url: api + id + "/Despesas",
        data: despesas,
        contentType: "application/json",
        success: function () {
            alert("Removido com sucesso!");
            bodyDespesa.modal('hide');
            Reload();

        },
        error: function (error) {
            console.log(error);
        }
    });
}
/***************************************************************/

/*********************** Funções internas **********************/
function salvarRelatorio() {
    var id = botaoSalvar.attr("data-id");
    var relatorio = {
        Descricao: descricao.val(),
        TipoRelatorio: tipoRelatorio.val(),
        Comentario: comentario.val()
    };
    if (id != undefined)
        alterar(id, relatorio);
    else
        inserir(relatorio);
}

function preencherCampos(Relatorio) {
    //var value= CatchIdOptions(tipoRelatorio, Relatorio.TipoRelatorio);
    //tipoRelatorio.val(value);
    tipoRelatorio.val(Relatorio.TipoRelatorio);
    descricao.val(Relatorio.Descricao);
    comentario.val(Relatorio.Comentario);
    dataVisualizacao.val($.format.date(Relatorio.DataCriacao, "dd/MM/yyyy"));
}

function CatchIdOptions(Select, string) {
    //return Select.filter(function () {
    //  return  $.trim($(this).text()) == string;

   // })​​​​​​
}
function limparCampos() {
    formulario.each(function () {
        this.reset();
    });

    botaoSalvar.removeAttr('data-id');
}

function abrirModalAlterar(id) {
    formValidation();
    botaoSalvar.attr("data-id", id);
    botaoSalvarDespesaRelatorio.attr("data-id", id);

    var Relatorio = (selecionarPorId(id));
    titleModal.html("Alterar Relatório");
    body.modal('show');
    preencherCampos(Relatorio);
    modalFooter.append(elemento);
    Reload();
}

function abrirModalExcluir(id) {
    if (confirm("Tem certeza que deseja excluir?")) {
        excluir(id);
        limparCampos();

    }
}
/***************************************************************/

/************************* Validações **************************/
function formValidation() {
    formulario.validate({
        errorClass: "errorClass",
        rules: {
            tipoDespesa: { required: true },
            data: { required: true },
            tipoPagamento: { required: true },
            valor: { required: true }
        },
        messages: {
            tipoDespesa: { required: "Campo obrigatório." },
            data: { required: "Campo obrigatório." },
            tipoPagamento: { required: "Campo obrigatório." },
            valor: { required: "Campo obrigatório." /*, minlength: "Campo deve possuir no mínimo {0} caracteres", maxlength: "Campo deve possuir no máximo {0} caracteres"*/ }
        },
        submitHandler: function (form) {
            salvarRelatorio();
        }
    });
}

$("#tabVinculadas").click(function () {
    bodyDespesaFotter.append(Desvincular);
});


$("#tabDesvinculadas").click(function () {
    $("#btn-desvincular-despesa").remove();
});


/***************** RelatórioDespesa Vinculada *****************/


function listarTabelaRelatorioDespesaAdicionada() {
    var id = botaoSalvarDespesaRelatorio.attr("data-id");
    tRelatorioDespesaAdicionada = tabelaRelatorioDespesaAdicionada.mDatatable({
        translate: {
            records: {
                noRecords: "Nenhum resultado encontrado.",
                processing: "Processando..."
            },
            toolbar: {
                pagination: {
                    items: {
                        default: {
                            first: "Primeira",
                            prev: "Anterior",
                            next: "Próxima",
                            last: "Última",
                            more: "Mais",
                            input: "Número da página",
                            select: "Selecionar tamanho da página"
                        },
                        info: 'Exibindo' + ' {{start}} - {{end}} ' + 'de' + ' {{total}} ' + 'resultados'
                    },
                }
            }
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: api + id + "/Despesas",
                    map: function (t) {
                        var e = t;
                        return void 0 !== t.data && (e = t.data), e
                    }
                }
            },
            pageSize: 10,
            serverPaging: !0,
            serverFiltering: !0,
            serverSorting: !0
        },
        layout: {
            theme: "default",
            class: "",
            scroll: !1,
            footer: !1
        },
        sortable: !0,
        pagination: !0,
        toolbar: {
            items: {
                pagination: {
                    pageSizeSelect: [10, 20, 30, 50, 100]
                }
            }
        },
        search: {
            input: $("#generalSearch")
        },
        columns: [
            {

                field: "Id",
                title: "#",
                width: 50,
                sortable: !1,
                textAlign: "center",
                selector: { class: "m-checkbox--solid m-checkbox--brand" }
            },
            {
                field: "Data",
                title: "Data",
            },
            {
                field: "Valor",
                title: "Valor",
            },
            {
                field: "Comentario",
                title: "Comentario",
            }],
        extensions: { checkbox: { vars: { selectedAllRows: 'selectedAllRows', requestIds: 'requestIds', rowIds: 'meta.Id', }, }, }
    });

    bodyDespesaFotter.append(Desvincular);
}
/***************************************************************/

/*********************** RelatórioDespesa Desvinculada **********************/


function listartabelaRelatorioDespesa() {
    tRelatorioDespesa = tabelaRelatorioDespesa.mDatatable({
        translate: {
            records: {
                noRecords: "Nenhum resultado encontrado.",
                processing: "Processando..."
            },
            toolbar: {
                pagination: {
                    items: {
                        default: {
                            first: "Primeira",
                            prev: "Anterior",
                            next: "Próxima",
                            last: "Última",
                            more: "Mais",
                            input: "Número da página",
                            select: "Selecionar tamanho da página"
                        },
                        info: 'Exibindo' + ' {{start}} - {{end}} ' + 'de' + ' {{total}} ' + 'resultados'
                    },
                }
            }
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: api + "Despesas",
                    map: function (t) {
                        var e = t;
                        return void 0 !== t.data && (e = t.data), e
                    }
                }
            },
            pageSize: 10,
            serverPaging: !0,
            serverFiltering: !0,
            serverSorting: !0
        },
        layout: {
            theme: "default",
            class: "",
            scroll: !1,
            footer: !1
        },
        sortable: !0,
        pagination: !0,
        toolbar: {
            items: {
                pagination: {
                    pageSizeSelect: [10, 20, 30, 50, 100]
                }
            }
        },
        search: {
            input: $("#generalSearch")
        },
        columns: [
            {

                field: "Id",
                title: "#",
                width: 50,
                sortable: !1,
                textAlign: "center",
                selector: { class: "m-checkbox--solid m-checkbox--brand" }
            },
            {
                field: "Data",
                title: "Data",
            },
            {
                field: "Valor",
                title: "Valor",
            },
            {
                field: "Comentario",
                title: "Comentario",
            }],
        extensions: { checkbox: { vars: { selectedAllRows: 'selectedAllRows', requestIds: 'requestIds', rowIds: 'meta.Id', }, }, }

    });

}
/***************************************************************/


function listarTabela() {
    t = tabela.mDatatable({
        translate: {
            records: {
                noRecords: "Nenhum resultado encontrado.",
                processing: "Processando..."
            },
            toolbar: {
                pagination: {
                    items: {
                        default: {
                            first: "Primeira",
                            prev: "Anterior",
                            next: "Próxima",
                            last: "Última",
                            more: "Mais",
                            input: "Número da página",
                            select: "Selecionar tamanho da página"
                        },
                        info: 'Exibindo' + ' {{start}} - {{end}} ' + 'de' + ' {{total}} ' + 'resultados'
                    },
                }
            }
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: api,
                    map: function (t) {
                        var e = t;
                        return void 0 !== t.data && (e = t.data), e
                    }
                }
            },
            pageSize: 10,
            serverPaging: !0,
            serverFiltering: !0,
            serverSorting: !0
        },
        layout: {
            theme: "default",
            class: "",
            scroll: !1,
            footer: !1
        },
        sortable: !0,
        pagination: !0,
        toolbar: {
            items: {
                pagination: {
                    pageSizeSelect: [10, 20, 30, 50, 100]
                }
            }
        },
        search: {
            input: $("#generalSearch")
        },
        columns: [

            {
                field: "TipoRelatorio",
                title: "Tipo Relatório",
                sortable: !1
            },
            {
                field: "Descricao",
                title: "Descrição",
            },
            {
                field: "Comentario",
                title: "Comentário",

            },
            //{
            //    field: "Status",
            //    title: "Status",
            //    template: function (t) {
            //        var e = {
            //            1: { title: "Pending", class: "m-badge--brand" },
            //            2: { title: "Delivered", class: " m-badge--metal" },
            //            3: { title: "Canceled", class: " m-badge--primary" },
            //            4: { title: "Success", class: " m-badge--success" },
            //            5: { title: "Info", class: " m-badge--info" },
            //            6: { title: "Danger", class: " m-badge--danger" },
            //            7: { title: "Warning", class: " m-badge--warning" }
            //        };
            //        return '<span class="m-badge ' + e[t.Status].class + ' m-badge--wide">' + e[t.Status].title + "</span>"
            //    }
            //},
            {
                field: "Acoes",
                title: "Ações",
                width: 50,
                sortable: false,
                overflow: "visible",
                template: function (t, e, a) {
                    return '\
                            <div class="dropdown">\
                                <a href="#" class="btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown">\
                                    <i class="la la-ellipsis-h"></i>\
                                </a>\
                                <div class="dropdown-menu dropdown-menu-right">\
                                    <a class="dropdown-item" href="#" onclick="abrirModalAlterar(' + t.Id + ')">\
                                        <i class="la la-edit"></i> Editar\
                                    </a>\
                                    <a class="dropdown-item" href="#" onclick="abrirModalExcluir('+ t.Id + ')">\
                                        <i class="la la-leaf"></i> Excluir\
                                    </a>\
                                </div>\
                            </div>';
                }
            }]
    }),
        e = t.getDataSourceQuery();
    $("#m_form_status").on("change", function () {
        var e = t.getDataSourceQuery();
        e.Status = $(this).val().toLowerCase(),
            t.setDataSourceQuery(e),
            t.load()
    }).val(void 0 !== e.Status ? e.Status : ""),
        $("#m_form_type").on("change", function () {
            var e = t.getDataSourceQuery();
            e.Type = $(this).val().toLowerCase(),
                t.setDataSourceQuery(e),
                t.load()
        }).val(void 0 !== e.Type ? e.Type : ""),
        $("#m_form_status, #m_form_type").selectpicker();

}


