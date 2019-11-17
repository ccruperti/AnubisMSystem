$(function() {
    var datePickerOptions = {
        format: "yyyy-mm-dd",
        weekStart: 1,
        todayBtn: "linked",
        language: "es",
        orientation: "bottom auto",
        todayHighlight: true,
        autoclose: true,
    };

    $(".fecha").datepicker(datePickerOptions);

    $("#actividades-tab-menu .item").tab();

    $(".ui.basic.modal").modal();

    $("body").on("click",
        ".eliminar-actividad",
        function () {
            if (confirm("¿Desea eliminar la actividad seleccionada?")) {
                var posActividad = $(this).data("actividad");
                actividades.eliminar(posActividad);
            }
        });

    $('.nuevo-parametro').dropdown(
        'setting',
        'onChange',
        function (value, text, $choice) {
            var posAct = $(this).attr('data-actividad');
            actividades.agregarParametro(posAct, value);
        }
    ).dropdown('setting', 'fullTextSearch', true);

    //$("#nuevaact").on("change",
    //    ".nuevo-parametro",
    //    function () {
    //        var posAct = $(this).children("select").first().attr("data-actividad");
    //        actividades.agregarParametro(posAct);
    //    });


    actividades.registrarActividadesEdicion();


    // Busqueda de Clientes

    var opcionesSearchClientes = {
        apiSettings: {
            url: '/Cotizaciones/BuscarCliente?cliente={query}',
            method: 'post'
        },
        fields: {
            results: 'items',
            title: 'Nombre',
            description: 'RUC'
        },
        searchDelay: 300,
        minCharacters: 3,
        onSelect: function (result, response) {
            $('#id-cliente').val(result.IdCliente);
            CargarContactos();
        },
        error: {
            noResults: 'No se han encontrado clientes con esa Razon Social. Intente otro nombre.'
        }
    };

    $('#search-clientes').search(opcionesSearchClientes);
});

function cargarDefault(edicion = false, idcot) {
    var selectionData = $('<option>',
        {
            text: 'Seleccione un Cliente'
        });
    var dropcliente = $("#Clientes");
  
    //dropcliente.append(selectionData);

    var selectionDataContacto = $('<option>',
        {
            value: idcot

        });
    
  
    selectionDataContacto.attr('selected', 'selected');
    selectionDataContacto.val(idcot);
        $("#Clientes").append(selectionDataContacto);
    

    var dropContacto = $("#Contactos").append(selectionDataContacto);
}

// Variables Globales
var indicadorActividad = -1;
var IndicadorTabs = -1;
var contador = 0;
var calculo = 0.00;
var menuTabsActividades = $("#actividades-tab-menu");

// Variables Memoria
var actividades = {
    lista: new Array(),
    secuencia: 0,
    descuento: 0
};

//Evento para capturar el cambio de descuento
$('#Descuento').on('change', function () {
    //Capturo el valor del nuevo descuento y lo asigno a la cotizacion
    var descuento = $('#Descuento').val();
    if (descuento >= 0 && descuento <= 100) {
        //Quiere decir que el porcentaje de descuento es valido
        actividades.descuento = parseInt(descuento);
    }
    else {
        $('#Descuento').val('0');
        actividades.descuento = 0;
    }

});

// EDICION Registrar Actividades
actividades.registrarActividadesEdicion = function() {
    $("#actividades-tab-menu .item").each(function(key, element) {
        var ordenItem = key + 1;
        var item = {
            nombre: "Actividad " + ordenItem,
            idMenu: "actividad-" + key + "-itemmenu",
            idTab: "actividad-" + key + "-tab",
            idDropParam: "actividad-" + key + "-dropparam",
            idDropPaquete: "actividad-" + key + "-droppaquete",
            idTabla: "actividad-" + key + "-tabla",
            idDropMatrx: "selector-matrices-" + key,
            idDropMatrx2: "selector-matrices2-" + key, 
            idDropNorma: "selector-normativa-" + key,
            idDropTMuestreo: "selector-tipomuestreo-" + key,
            idDropPaq: "selector-paquetes-" + key,
            idCabeceraP: "cabecera-parametro-" + key,
            parametros: new Array(),
            orden: ordenItem,
            posicion: key,
            sumatoria: 0,
            //Lista de Parametros seleccionados de la actividad
            parametrosSeleccionados: new Array()
        };
        actividades.secuencia = actividades.secuencia + 1;
        actividades.descuento = parseInt($("#Descuento").val());
        //Agrego los parametros de la actividad
        $("#actividad-" + key + "-tabla").children().each(function(paramKey, element) {
            var ordenItem = paramKey + 1;
            var itemParametro = {
                IdParametro: $("#Actividad-" + item.posicion + "-parametro-" + paramKey + "-inputid").val(),
                nombre: $("#Actividades-" + item.posicion + "-parametros-" + paramKey + "-name").data("name"),
                id: "#actividad" + item.posicion + "-parametro" + paramKey,
                orden: ordenItem,
                posicion: paramKey,
                cantidad: $("#actividad-" + item.posicion + "-parametro-" + paramKey + "-inputcantidad").val(),
                precioU: $("#Actividad-" + item.posicion + "-parametro-" + paramKey + "-inputprecio").val(),
                costo: $("#Actividad-" + item.posicion + "-parametro-" + paramKey + "-inputprecio").val(),
                subtotal: 0,
                IdTabla: item.idTabla,
                IdInputParam: "actividad-" + item.posicion + "-parametro-" + paramKey + "-inputid",
                idInputPecio: "Actividad-" + item.posicion + "-parametro-" + paramKey + "-inputprecio",
                idInputCantidad: "actividad-" + item.posicion + "-parametro-" + paramKey + "-inputcantidad",
                idRow: "actividad-" + item.posicion + "-parametro-" + paramKey + "-idrow",
                IdSubtotal: "Actividad-" + item.posicion + "-parametro-" + paramKey + "-subtotal"
            };
            itemParametro.precioU = itemParametro.costo;
            itemParametro.subtotal = (itemParametro.precioU * itemParametro.cantidad).toFixed(2);
            itemParametro.costo = String(itemParametro.costo).replace(".", ",");
            itemParametro.subtotal = String(itemParametro.costo).replace(".", ",");
            item.parametros.push(itemParametro);
        });
        actividades.lista.push(item);
    });

    actividades.reportar();
};

//Calcular subtotal
actividades.calcularSubtotal = function (precioU, cantidad) {

};

// Reportar Elementos Memoria
actividades.reportar = function() {
    console.info("Reportando datos en memoria");
};

// Agregar Actividad
actividades.agregar = function() {
    var ordenItem = actividades.lista.length + 1;
    var posicionItem = actividades.lista.length;
    actividades.secuencia = actividades.lista.length;
   
    // Operaciones en Memoria
    var item = {
        nombre: "Actividad " + ordenItem,
        idMenu: "actividad-" + actividades.secuencia + "-itemmenu",
        idTab: "actividad-" + actividades.secuencia + "-tab",
        idDropParam: "actividad-" + actividades.secuencia + "-dropparam",
        idDropPaquete: "actividad-" + actividades.secuencia + "-droppaquete",
        idTabla: "actividad-" + actividades.secuencia + "-tabla",
        idDropMatrx: "selector-matrices-" + actividades.secuencia,
        idDropMatrx2: "selector-matrices2-" + actividades.secuencia,
        idDropNorma: "selector-normativa-" + actividades.secuencia,
        idDropPaq: "selector-paquetes-" + actividades.secuencia,
        idCabeceraP: "cabecera-parametro-" + actividades.secuencia,
        orden: ordenItem,  
        idCabecera: "header-act-" + actividades.secuencia,
        posicion: posicionItem, 
        parametros: new Array(),
        sumatoria: 0,
        parametrosSeleccionados: new Array(),
        idDropTMuestreo: "selector-tipomuestreo-" + actividades.secuencia
    };
    
    // Operaciones en HTML
    $("#actividades-tab-menu .item").removeClass("active");
    $("#nuevaact .tab").removeClass("active");

    // Agregar div tab
    var tabMenuActividades = $("<div>");
    tabMenuActividades.addClass("active item");
    tabMenuActividades.attr("id", item.idMenu);
    tabMenuActividades.text(item.nombre);
    tabMenuActividades.attr("data-tab", item.idTab);
    tabMenuActividades.append('<div class="ui red label eliminar-actividad" data-actividad="' +
        item.posicion +
        '"><i class="delete icon"></i></div>');
    menuTabsActividades.append(tabMenuActividades);


    var ajaxOptions = {
        url: "/Cotizaciones/RegistrarActividad?a=" + item.posicion + "&c=" + actividades.secuencia,
        beforeSend: function() {
            //$("#nueva-actividad").html('<option> Loading ...</option>');
            $("#nueva-actividad").prop("disabled", true); // disable button
        },
        success: function (partialData) {
            //var option = $('<option>',
            //    {
            //        text: "Seleccione una Clasificacion",
            //    });
            //$("#" + item.idDropMatrx).append(option);
            
            $("#nuevaact").append(partialData);

            $("#" + item.idDropParam + ".dropdown").dropdown();

            $("#" + item.idCabecera).text("Datos de la Actividad " + item.orden);
          
            actividades.lista.push(item);
          
            var dropsettings = {
                fullTextSearch: true,
                forceSelection: false
            }

            $("#" + item.idDropParam).dropdown(
                'setting',
                'onChange',
                function(value, text, $choice) {
                    var posAct = $(this).attr('data-actividad');
                  
                    actividades.agregarParametro(posAct, value);
                    //$(this).attr("placeholder", "Seleccione un parametro");

                }
            ).dropdown("setting", "fullTextSearch", true).dropdown("setting", "forceSelection", false);
                
            $("#actividades-tab-menu .item").tab();
            $("#" + item.idDropMatrx).dropdown();
            $("#" + item.idDropMatrx2).dropdown();
            $("#" + item.idDropNorma).dropdown();
            $("#" + item.idDropPaq).dropdown();
            $("#" + item.idDropTMuestreo).dropdown(); 
            actividades.secuencia = actividades.secuencia + 1;
            $("#nueva-actividad").prop("disabled", false);

        }
    };
    $.ajax(ajaxOptions);
    console.log("Actividades Lista  ");
    console.log(actividades.lista);
};


// Eliminar Actividad
actividades.eliminar = function(pos) {
    var item = actividades.lista[pos];
    // Operaciones en HTML
    $("#" + item.idTab).remove();
    $("#" + item.idMenu).remove();
    // Operaciones en memoria
    actividades.lista.splice(pos, 1);
    if (actividades.lista.length > 0) {
        $("#actividades-tab-menu").first(".item").addClass("active");
        $("#nuevaact").first(".tab").addClass("active");

    }

    $.each(actividades.lista,
        function(key, actividad) {
            if (actividad.orden > pos) {


                actividades.reordenarActividad(actividad.posicion, actividad.posicion - 1);

                actividad.orden = actividad.orden - 1;
                actividad.posicion = actividad.posicion - 1;
                actividad.secuencia = actividad.secuencia - 1;
                actividad.nombre = "Actividad " + actividad.orden;

                // Tabs
                $("#" + actividad.idMenu).html(actividad.nombre +
                    '<div class="ui red label eliminar-actividad" data-actividad="' +
                    actividad.posicion +
                    '"><i class="delete icon"></i></div>');
            };
        });

    actividades.reportar();
};

// EDICION: Desactiva una actividad
actividades.desactivar = function(pos) {
    var item = actividades.lista[pos];
    // Operaciones en HTML
    $("#" + item.idTab).hide();
    $("#" + item.idMenu).hide();
    $("#actividad-" + pos + "-activo").val(false);
    // Operaciones en memoria
    if (actividades.lista.length > 0) {
        $("#actividades-tab-menu").first(".item").addClass("active");
        $("#nuevaact").first(".tab").addClass("active");
    }

    $.each(actividades.lista,
        function(key, actividad) {
            if (actividad.orden > pos) {
                actividad.orden = actividad.orden - 1;
                actividad.nombre = "Actividad " + actividad.orden;
            };
        });

};
//EDICION DESACTIVAR ROW
actividades.desactivarParam = function(posAct, posParam) {
    var actividad = actividades.lista[posAct].parametros[posParam];
    $("#" + actividad.idRow).hide();
    $("actividad-" + posAct + "-param-" + posParam + "-activo").val(false);
    $.each(actividades,
        function(key, parametro) {
            if (parametro.orden > pos) {
                parametro.orden = parametro.orden - 1;
                parametro.nombre = "Parametro " + parametro.orden;
            };
        });

};
//EDICION AGREGAR PARAMETRO
actividades.agregarParametroEdicion = function() {
    var item = actividades.lista;
    $(item).each(function(key, element) {
        var ordenItem = key + 1;
        var tablas = item[key].idTabla;
        var table = $("#" + item[key].idTabla).find("tr");
        $(table).each(function(key2, element2) {
            var ordenItemParam = key2 + 1;
            var itempParametro = {
                //Nombre: $('#' + element.idDropParam + ' option:selected').text(),
                id: "#actividad" + key + "-parametro" + key2,
                orden: ordenItemParam,
                posicion: key2,
                cantidad: 0,
                precioU: 0,
                costo: 0,
                IdTabla: item[key].idTabla,
                IdInputParam: "actividad-" + key + "-parametro-" + key2 + "-inputid",
                idInputPecio: "Actividad-" + key + "-parametro-" + key2 + "-inputprecio",
                idInputCantidad: "actividad-" + key + "-parametro-" + key2 + "-inputcantidad",
                idRow: "actividad-" + key + "-parametro-" + key2 + "-idrow"
            };
            actividades.lista[key].parametros.push(itempParametro);
            //params.push(itempParametro);
            //item.parametros[key2].posicion = item[key].parametros[key2].posicion + 1;

        });;


    });
    actividades.reportar();
};

// Reordenar indice de actividad
actividades.reordenarActividad = function(posAct, newPosAct) {
    var inputList = $('input[data-actividad="' +
        posAct +
        '"], select[data-actividad="' +
        posAct +
        '"], button[data-actividad="' +
        posAct +
        '"]');
    $.each(inputList,
        function(key, item) {
            var input = $(item);
            // Tipo de bloque del input
            var inputType = input.data("type");
            // Nombre formulario del input
            var inputName = input.data("name");
            // Si el input es actividad
            if (inputType === "act") {
                input.attr("name", "Actividad[" + newPosAct + "]." + inputName);
            }
            // Si el input es parametro
            else if (inputType === "param") {
                var inputParamPos = input.data("parametro");
                input.attr("name", "Actividad[" + newPosAct + "].Parametros[" + inputParamPos + "]." + inputName);
            }
            input.attr("data-actividad", newPosAct);
        });
};

// Reordenar indice de parametro
actividades.reordenarParametroActividad = function(posAct, posParam,newPosParam) {
    var inputList = $('input[data-actividad="' + posAct + '"][data-type="param"][data-parametro="' + posParam + '"');
    //Recupero el boton de la fila y cambio su secuencia de parametro
    var botonDelete = $('button[data-parametro="' + posParam + '"]');
    botonDelete.attr("data-parametro", newPosParam);
    //var inputList = $('.input-parametro-' + posAct);
    $.each(inputList,
        function(key, item) {
            var input = $(item);
            // Nombre formulario del input
            var inputName = input.data("name");
            //alert("esta es la posicion a reordenar"+ newPosParam);
            input.attr("data-parametro", newPosParam);
            input.attr("name", "Actividades[" + posAct + "].Parametros[" + newPosParam + "]." + inputName);
        });
};

// Agregar Parametro Actividad
actividades.agregarParametro = function (posAct, idParametro, name = null, posParametro = null) {
  
    var item = actividades.lista[posAct];
    var ordenItemParametro = item.parametros.length + 1;
    var posicionItemParametro = (posParametro == null) ? item.parametros.length : posParametro;
   
    //Agrego el id del parametro que se ha agregado a la lista de parametros selccionados de la actividad
    item.parametrosSeleccionados.push(parseInt(idParametro));
    recargarDropParam(posAct);
    var itemParametro = {
        nombre: (name == null) ? $("#" + item.idDropParam + " option:selected").text() : name,
        IdParametro: parseInt(idParametro),
        id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemParametro,
        posicion: posicionItemParametro,
        cantidad: 0,
        precioU: 0,
        costo: 0,
        subtotal: 0,
        IdTabla: item.idTabla,
        IdInputParam: "actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-inputid",
        idInputPecio: "Actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-inputprecio",
        idInputCantidad: "actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-inputcantidad",
        idRow: "actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-idrow",
        IdSubtotal: "Actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-subtotal"
    };



    var table = $("#" + item.idTabla);
    //Agrego el costo del parametro al objeto del paramtero
    var ajaxOptions = {
        url: "/Cotizaciones/RegistrarParametro?a=" +
            posAct +
            "&parametroId=" +
            idParametro +
            "&c=" +
            posicionItemParametro,
        success: function(partialData) {
            table.append(partialData);
            //$(".search").search();
            itemParametro.costo = $("#" + itemParametro.idInputPecio).val();
            itemParametro.precioU = parseFloat(itemParametro.costo).toFixed(2);
            var cantgeneral = $("#actividad-" + posAct).val();
            $("#" + itemParametro.idInputCantidad).val(cantgeneral);
            itemParametro.cantidad = $("#" + itemParametro.idInputCantidad).val();
            itemParametro.subtotal = parseFloat(itemParametro.precioU * itemParametro.cantidad).toFixed(2);
            $("#" + itemParametro.IdSubtotal).html("");
            $("#" + itemParametro.IdSubtotal).append("$ " + itemParametro.subtotal);
            itemParametro.subtotal = String(itemParametro.subtotal).replace(".", ",");
            itemParametro.costo = String(itemParametro.costo).replace(".", ",");
            item.parametros.push(itemParametro);
        }
    };
    $.ajax(ajaxOptions);
};
actividades.agregarPaquete = function(posAct, IdPaquete) {
    var item = actividades.lista[posAct];
    var ordenItemParametro = item.parametros.length + 1;
    var posicionItemParametro = item.parametros.length;
    var inputcantgeneral = $("#actividad-" + posAct).val();
    var itemParametro = {
        nombre: "Parametro " + ordenItemParametro,
        id: "actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemParametro,
        posicion: posicionItemParametro,
        cantidad: 2,
        //IdPaquete: boton.parent().siblings(".dropcontainer2").find("select.dropparam2").val(),
        IdTabla: "actividad-" + item.posicion + "-tabla",
        IdSubtotal: "Actividad-" + item.posicion + "-parametro-" + posicionItemParametro + "-subtotal"
    };
    var tabla = $("#" + itemParametro.IdTabla);

    var ajaxOptions = {
        url: "/Cotizaciones/PaquetesParametros?a=" + posAct + "&c=" + posicionItemParametro + "&IdPaquete=" + IdPaquete,
        success: function(partialData) {
            //$(".search").dropdown();
            tabla.append(partialData);
            var tr = tabla.find("tr").toArray();
            $.each(tr,
                function(key, actividad) {
                    //posicionItemParametro++;
                    var itemParametro2 = {
                        IdParametro: $("#actividad-" + item.posicion + "-hidden-" + key).val(),
                        nombre: $("#Actividades-" + item.posicion + "-parametros-" + key + "-name").data("name"),
                        id: "actividad" + item.posicion + "-parametro" + key,
                        orden: ordenItemParametro,
                        posicion: key,
                        cantidad: 0,
                        costo: $("#Actividad-" + item.posicion + "-parametro-" + key + "-inputprecio").val(),
                        precioU: 0,
                        //IdPaquete: boton.parent().siblings(".dropcontainer2").find("select.dropparam2").val(),
                        IdTabla: "actividad-" + item.posicion + "-tabla",
                        IdInputParam: "actividad-" + item.posicion + "-parametro-" + key + "-inputid",
                        idInputPecio: "Actividad-" + item.posicion + "-parametro-" + key + "-inputprecio",
                        idInputCantidad: "actividad-" +
                            item.posicion +
                            "-parametro-" +
                            posicionItemParametro +
                            "-inputcantidad",
                        idRow: "actividad-" + item.posicion + "-parametro-" + key + "-idrow",
                        subtotal: 0,
                        IdSubtotal: "Actividad-" + item.posicion + "-parametro-" + key + "-subtotal"

                    };
                    $("#" + itemParametro2.idInputCantidad).val(inputcantgeneral);
                    itemParametro2.precioU = parseFloat(itemParametro2.costo).toFixed(2);
                    itemParametro2.cantidad = $("#" + itemParametro2.idInputCantidad).val();
                    itemParametro2.subtotal = parseFloat(itemParametro2.precioU * itemParametro2.cantidad).toFixed(2);
                    $("#" + itemParametro2.IdSubtotal).html("");
                    $("#" + itemParametro2.IdSubtotal).append("$ " + itemParametro2.subtotal);               
                    item.parametros.push(itemParametro2);
                    posicionItemParametro++;
                });
            //$(".search").dropdown();

        }
    };
    $.ajax(ajaxOptions);
};
// Eliminar Parametro Actividad
actividades.eliminarParametro = function (posAct, posParam) {


    var item = actividades.lista[posAct].parametros[posParam];
    //Elimino el parametro seleccionado

    var act = actividades.lista[posAct];
    var index = act.parametrosSeleccionados.indexOf(item.IdParametro);
    act.parametrosSeleccionados.splice(index, 1);
    recargarDropParam(posAct);
    $("#" + item.idRow).remove();

    actividades.lista[posAct].parametros.splice(posParam, 1);

    $.each(actividades.lista[posAct].parametros,
        function(key, parametro) {
            if (parametro.posicion > posParam) {
                //Recupero el row que esta en esa posicion y le disminuyo en 1 su posicion
                var newIdRow = 'actividad-' + posAct + '-parametro-' + (parametro.posicion - 1) + '-idrow';
                $('#actividad-' + posAct + '-parametro-' + parametro.posicion + '-idrow').attr('id', newIdRow);
                parametro.idRow = newIdRow;
                actividades.reordenarParametroActividad(posAct, parametro.posicion, parametro.posicion - 1);
                parametro.posicion = parametro.posicion - 1;
            }
        });
    actividades.reportar();
};


// Registrar Actividad
$("#nueva-actividad").on("click",
    function() {
        actividades.agregar();
    });

// Costos
function CargarModal() {
    $("#modalcond").modal("show");
};
//Caragar dato dummy drop clientes
function AppendCliente() {
    var selectionData = $('<option>', {
        text: 'Seleccione un parametro',
        selected: 'selected'
    });
    var dropcliente = $("#Clientes");
    dropcliente.append(selectionData);
}

$("#boton-agregar").on("click",
    function() {
        $("#modal-cliente").modal("show");
    });

$("#boton-costos").on("click",
    function() {
        var lista = {
            descuento: actividades.descuento,
            lista: actividades.lista
        };

        var Url = "/Cotizaciones/VerResumenCostos";
        var opciones = {
            url: Url,
            data: lista,
            type: "post",
            success: function(vistaParcial) {
                $("#modal-contenido-costos").html("");
                $("#modal-contenido-costos").append(vistaParcial);
                $("#modal-costos").modal("show");
            }
        };
        $.ajax(opciones).fail(function(error) {
        });
    });
$("#guardarcont").on("click",
    function() {

        var boton = $(this).data("status");

        var object = $("#form-post").serialize();
        if (boton === "guardar") {
            var ajaxOptions = {
                url: "/Cotizaciones/RegistrarCotizacion",
                data: {
                    model: object
                },
                contentType: "application/json; charset=UTF-8",
                success: function(partialData) {
                    alert("Guardado");
                    $(this).attr("data-status", "edicion");


                }
            };
            $.ajax(ajaxOptions);
        } else if (boton === "edicion") {
            var ajaxOptionsCont = {
                url: "/Cotizaciones/EditarCotizacion?IdContizacion",
                data: {
                    model: object
                },
                success: function(partialData) {


                    alert("Guardado");

                }
            };
            $.ajax(ajaxOptionsCont);
        }
    });

function VerTotales() {
    var valores = $(".calcularvalor");
    var contenido = $("#modal-contenido");
    var div = $("<div>").addClass("field");
    var header = $("<div>").addClass("ui dividing header");
    var label2 = $("<h4>").text("Actividad " + contador + ":");
    header.appendTo(div);
    label2.appendTo(header);
    div.appendTo(contenido);
    var labeledinput = $("<div>").addClass("ui right labeled left icon input");
    var icon = $("<i>").addClass("dollar icon");
    icon.appendTo(labeledinput);
    labeledinput.appendTo(div);
    var label = $("<a>").text("Subtotal").addClass("ui tag label");

    var input = $("<input>").attr("type", "text").attr("disabled", "disabled").text(calculo);
    input.appendTo(labeledinput);
    label.appendTo(labeledinput);
};

$("#nuevaact").on("click",
    ".nuevo-paquete",
    function() {
        var act = $(this).attr("data-actividad");
        var item = actividades.lista[act].idDropPaq;
        var paquete = $("#" + item).val();
        actividades.agregarPaquete(act, paquete);

    });
// Agregar parametros
$("#nuevaact").on("click",
    ".eliminar-edicion",
    function() {
        var boton = $(this);
        var act = boton.data("actividad");
        var param = boton.data("parametro");
    });

//function agregarparam(act) {

//    actividades.agregarParametro(act);

//}

$("#nuevaact").on("change",
    ".calcularvalor",
    function () { 
        //var idinput = actividades.lista[$(this).attr("data-actividad")].parametros[$(this).attr("data-parametro")].IdInputParam;
        //console.log(idinput);
        var td = $(this);
        //console.log("Valor TD"); 
        //console.log(td); 
        var parametro = actividades.lista[$(this).attr("data-actividad")].parametros[$(this).attr("data-parametro")];
        //console.log("Data Actividad"); 
        //console.log($(this).attr("data-actividad"));
        //console.log("Data Parametro");  
        //console.log($(this).attr("data-parametro"));
        //console.log(parametro);
        parametro.cantidad = td.val(); 
        //var precio = td.data("precio");
        var idcant = td.attr("data-cantidad");
        //console.log(idcant); 
        //Recupero los inputs de precio y cantidad y subtotal
        var inputCant = $("#" + parametro.idInputCantidad);
        var inputPrecio = $("#" + parametro.idInputPecio);
        var labelSubtotal = $("#" + parametro.IdSubtotal);
        //console.log("inputCant: " + inputCant.val());
        //console.log("inputPrecio: " +inputPrecio.val());
        //console.log("inputSubt: " +labelSubtotal.val()); 
        //Forzo el cambio de los valores de precio a formato entendible por javascript
        var precioFormateado = String(inputPrecio.val()).replace(",", ".");


        //Guardo los nuevos valores del parametro
        parametro.cantidad = parseInt(inputCant.val());
        parametro.costo = parseFloat(precioFormateado);
        parametro.precioU = precioFormateado;

        //Realizo el calculo del subtotal
        parametro.subtotal = parametro.cantidad * parametro.costo;

        //Muestro los valores de calculo
 

        //Actualizo los valores de cantidad, precio y subtotal en la vista
        inputCant.val(parametro.cantidad);
        inputPrecio.val(parseFloat(parametro.costo).toFixed(2));
        labelSubtotal.text("$ " + parseFloat(parametro.subtotal).toFixed(2));

        //Actualizo los valores del parametro costo y subtotal para que sean enviados correctamente al servidor
        parametro.costo = String(parametro.costo.toFixed(2)).replace(".", ",");
        parametro.subtotal = String(parametro.subtotal.toFixed(2)).replace(".", ",");
        

        //Muestro los nuevos valores guardados en el objeto
  

        ////td.attr("data-precioact", calculo2);

        //var inputPrecio = $("#" + parametro)

        ////Buscar el parametro de la actividad que se esta cambiando y modificar la cantidad
        ////var parametro = actividades.lista.find(element => element.parametros.find(para => para.idInputCantidad == idcant)).parametros.find(param => param.idInputCantidad == idcant);

        //console.log(parametro.cantidad);
        ////Recupero el costo del parametro para calcular su total = costo * cantidad
        ////parametro.costo = $('#' + parametro.idInputPecio).val();
        
        //console.log(precio);
        ////console.log(parametro.costo);
        //var valor = parametro.precioU;
        //var preciostring = String(valor).replace(",", ".");
        //var preciodecimal = parseFloat(preciostring).toFixed(2);
        ////var cantidad = parseFloat(td.val()).toFixed(2);
        //var calculo2 = preciodecimal * parseInt(td.val());
        //parametro.subtotal = calculo2;
        ////parametro.costo = String(calculo2.toFixed(2)).replace(".", ",");
        //precio.html("");
        //precio.append("$ " + String(calculo2.toFixed(2)).replace(".", ","));
        ////console.log(parametro.costo);
        ////input.attr("data-precioact", calculo2);
    });

$("#nuevaact").on("change",
    ".filtrodrop",
    function() {

        var posAct = $(this).children("select").first().attr("data-actividad");

        //var actividad = actividades.lista[posAct];

        //var options = {
        //    filtroParametro: {
        //        IdClasificacion: $("#" + actividad.idDropMatrx).val(),
        //        seleccionados: actividad.parametrosSeleccionados
        //    }
        //};

        //var dropParametros = $("#" + actividad.idDropParam);
        //dropParametros.prop('disabled', 'disabled');
        //dropParametros.addClass('loading');
        //dropParametros.dropdown();

        recargarDropParam(posAct);
        //$.post("/Cotizaciones/JsonParametros", options, function(data) {
        //    dropParametros.children().remove();
        //    if (data.length === 0) {
        //        var noData = $('<option>',
        //            {
        //                text: 'No hay parametros',
        //                selected: 'selected'
        //            });
        //        dropParametros.append(noData);
        //    } else {
        //        var selectionData = $('<option>', {
        //            text: 'Seleccione un parametro',
        //            selected: 'selected'
        //        });
        //        dropParametros.append(selectionData);
        //        $.each(data,
        //            function (key, value) {
        //                var option = $('<option>',
        //                    {
        //                        text: value.ClaveParametro + " - " + value.Nombre + " - " + value.Unidades,
        //                        value: value.Id
        //                    });
        //                dropParametros.append(option);
        //            });
        //        dropParametros.removeProp('disabled');
        //    }
        //    dropParametros.removeClass('loading');
        //    dropParametros.dropdown();
        //});
    });

//Funcion que recarga la nueva informacion del dropdown
function recargarDropParam(posAct) {

    var actividad = actividades.lista[posAct];

    var options = {
        filtroParametro: {
            IdClasificacion: $("#" + actividad.idDropMatrx).val(),
            seleccionados: actividad.parametrosSeleccionados
        }
    };


    var dropParametros = $("#" + actividad.idDropParam);
    dropParametros.prop('disabled', 'disabled');
    dropParametros.addClass('loading');
    dropParametros.dropdown();

    $.post("/Cotizaciones/JsonParametros", options, function (data) {
        dropParametros.children().remove();
        if (data.length === 0) {
            var noData = $('<option>',
                {
                    text: 'No hay parametros',
                    selected: 'selected'
                });
            dropParametros.append(noData);
        } else {
            var selectionData = $('<option>', {
                text: 'Seleccione un parametro',
                selected: 'selected'
            });
            dropParametros.append(selectionData);
            $.each(data,
                function (key, value) {
                    var option = $('<option>',
                        {
                            text: value.ClaveParametro + " - " + value.Nombre + " - " + value.Unidades,
                            value: value.Id
                        });
                    dropParametros.append(option);
                });
            dropParametros.removeProp('disabled');
        }
        dropParametros.removeClass('loading');
        dropParametros.dropdown();
    });
}

function CalcularPrecio() {
    var row = $("");
};

$("#nuevaact").on("click",
    ".eliminarparam",
    function() {
        var boton = $(this);
        var act = boton.attr("data-actividad");
        var parametro = boton.attr("data-parametro");

        actividades.eliminarParametro(act, parametro);

    });

function CargarContactos() {
    var options = {
        IdCliente: $("#id-cliente").val()
    };
    $.post("/Cotizaciones/JsonContactos", options, function (data) {
        $("#Contactos").children().remove();
        if (data.length === 0) {
            var noresults = $("<option>").text("No hay Contactos Registrados").val("");
            $("#Contactos").append(noresults);
            $("#Contactos").prop("disabled", true);

        } else {
            $("#Contactos").prop("disabled", false);
            $.each(data,
                function (key, reg) {
                    var option = $("<option>").attr({ value: reg.Id }).text(reg.Nombre);

                    $("#Contactos").append(option);

                });

        }
        $("#Contactos").dropdown();
        cargarDatosContacto();
    });


}

function cargarDatosContacto() {
    var idContacto = $("#Contactos").val();
    if (idContacto != null || idContacto !== "")
        $.ajax({
            type: "Get",
            url: "/Cotizaciones/ObtenerDatosContacto",
            data: { idcliente: idContacto },
            dataType: "json",
            success: function (data) {
                $("#txtemail").val(data['email']);
                $("#txttelefono").val(data['telefono']);
            }
        });
}

function AgregarCliente() {
    var data = new FormData();
    data.append("nombre", $("#input-nombre").val());
    data.append("direccion", $("#input-direccion").val());
    data.append("telefono", $("#input-telefono").val());
    data.append("razonsocial", $("#input-razonsoc").val());
    data.append("ruc", $("#input-RUC").val());
    data.append("replegal", $("#input-representante").val());
    data.append("nombreContacto", $("#input-contacto-nombre").val());
    data.append("email", $("#input-contacto-correo").val());
    data.append("telContacto", $("#input-contacto-numtelf").val());
    data.append("IdTipoContacto", $("#tipo-contacto").val());
    data.append("IdCliente", $("#Clientes").val());
    $.ajax({
        type: "POST",
        url: "/Cotizaciones/RegistrarNuevoCliente",
        data: data,
        contentType: false,
        processData: false,
        cache: false,
        success: function(data) {
            var option = $("<option>").attr("value", data.IdCliente).html(data.Alias);
            $("#Clientes").append(option);
        }
    });
};

function syncInputs(posAct) {
    var inputcant = $("#actividad-" + posAct).val(); 
    var parametros = actividades.lista[posAct].parametros;
    parametros.map(function(elem, inx, ob) {
        $("#" + elem.idInputCantidad).val(inputcant);
    });

    $.each(parametros,
        function(key, val) {
            val.cantidad = inputcant;
            var subtotal = parseFloat(inputcant) * parseFloat(val.precioU);
            val.subtotal = subtotal;
            $("#" + val.IdSubtotal).text("$ " + subtotal);
        });
};

$('.confirmation').on('click', function () {
    return confirm('Está seguro de que desea salir?');
});

$("#nuevaact").on('click', '.duplicar',
    function(e) {
        var actividad = $(this).data('actividad');
        actividades.duplicarActividad(actividad);
    });


actividades.duplicarActividad = function (actividad) { 
    var actividadDuplicar = actividades.lista[actividad];
    var ordenItem = actividades.lista.length + 1;
    var posicionItem = actividades.lista.length;
    actividades.secuencia = actividades.lista.length;

    // Operaciones en Memoria
    var nuevaActividad = {
        nombre: "Actividad " + ordenItem,
        idMenu: "actividad-" + actividades.secuencia + "-itemmenu",
        idTab: "actividad-" + actividades.secuencia + "-tab",
        idDropParam: "actividad-" + actividades.secuencia + "-dropparam",
        idDropPaquete: "actividad-" + actividades.secuencia + "-droppaquete",
        idTabla: "actividad-" + posicionItem + "-tabla",
        idDropMatrx: "selector-matrices-" + actividades.secuencia,
        idDropMatrx2: "selector-matrices2-" + actividades.secuencia,
        idDropNorma: "selector-normativa-" + actividades.secuencia,
        idDropTMuestreo: "selector-tipomuestreo-" + actividades.secuencia,
        idDropPaq: "selector-paquetes-" + actividades.secuencia,
        idCabeceraP: "cabecera-parametro-" + actividades.secuencia,
        orden: ordenItem,
        idCabecera: "header-act-" + actividades.secuencia,
        posicion: posicionItem, 
        parametros: new Array(),
        sumatoria: actividadDuplicar.sumatoria,
        parametrosSeleccionados: actividadDuplicar.parametrosSeleccionados,
        actividad: posicionItem,
        contador: posicionItem,
        DescripcionMuestra: actividad.DescripcionMuestra,
        Observaciones: "",
        IdClasificacion: 0,
        IdNormativa: 0,
        IdMatriz: 0,
        NumeroMuestras: 0,
        IdTipoMuestreo: 0
};
     
    var nuevosParametros = actividadDuplicar.parametros;

    $.each(nuevosParametros,
        function (key, parametro) {

            var nuevoParametro = {};
            var posicionItemParametro = parametro.posicion;

            //Agrego el id del parametro que se ha agregado a la lista de parametros selccionados de la actividad
            nuevoParametro.id = "#actividad" + posicionItem + "-parametro" + posicionItemParametro;
            nuevoParametro.IdTabla = "actividad-" + posicionItem + "-tabla";
            nuevoParametro.IdInputParam = "actividad-" + posicionItem + "-parametro-" + posicionItemParametro + "-inputid";
            nuevoParametro.idInputPecio = "Actividad-" + posicionItem + "-parametro-" + posicionItemParametro + "-inputprecio";
            nuevoParametro.idInputCantidad = "actividad-" + posicionItem + "-parametro-" + posicionItemParametro + "-inputcantidad";
            nuevoParametro.idRow = "actividad-" + posicionItem + "-parametro-" + posicionItemParametro + "-idrow";
            nuevoParametro.IdSubtotal = "Actividad-" + posicionItem + "-parametro-" + posicionItemParametro + "-subtotal";
            nuevoParametro.cantidad = parametro.cantidad;
            nuevoParametro.IdParametro = parametro.IdParametro;
            nuevoParametro.precioU = parametro.precioU;
            nuevoParametro.costo = parametro.costo;
            nuevoParametro.nombre = parametro.nombre;
            nuevoParametro.orden = parametro.orden;
            nuevoParametro.posicion = parametro.posicion;
            nuevoParametro.subtotal = parametro.subtotal;
            nuevaActividad.parametros.push(nuevoParametro);
        });
    actividades.lista.push(nuevaActividad);
    console.log("Actividades Lista (Duplicado)");
    console.log(actividades.lista);

    nuevaActividad.IdClasificacion = $("#" + actividadDuplicar.idDropMatrx).val();
    nuevaActividad.IdClasificacion = $("#" + actividadDuplicar.idDropMatrx).val();
    nuevaActividad.IdNormativa = $("#" + actividadDuplicar.idDropNorma).val();
    nuevaActividad.IdMatriz = $("#" + actividadDuplicar.idDropMatrx2).val();

    nuevaActividad.IdTipoMuestreo = $("#" + actividadDuplicar.idDropTMuestreo).val(); 

    nuevaActividad.DescripcionMuestra = $("#Descripcion-" + actividad ).val();
    nuevaActividad.Observaciones = $("#Observacion-" + actividad).val();
    nuevaActividad.NumeroMuestras = $("#actividad-" + actividad).val();
    //console.log(actividad, $("#actividad-" + actividad).val());
    // Operaciones en HTML
    $("#actividades-tab-menu .item").removeClass("active");
    $("#nuevaact .tab").removeClass("active");

    var tabMenuActividades = $("<div>");
    tabMenuActividades.addClass("active item");
    tabMenuActividades.attr("id", nuevaActividad.idMenu);
    tabMenuActividades.text(nuevaActividad.nombre);
    tabMenuActividades.attr("data-tab", nuevaActividad.idTab);
    tabMenuActividades.append('<div class="ui red label eliminar-actividad" data-actividad="' +
        nuevaActividad.posicion +
        '"><i class="delete icon"></i></div>');
    menuTabsActividades.append(tabMenuActividades);


    var ajaxOptions = {
        url: '/Cotizaciones/DuplicarActividad',
        type: 'POST',
        data: nuevaActividad,
        success: function (response) {
            // Agregar div tab
            $("#nuevaact").append(response);

            $("#" + nuevaActividad.idDropParam + ".dropdown").dropdown();

            $("#" + nuevaActividad.idCabecera).text("Datos de la Actividad " + nuevaActividad.orden);
           
            

            var dropsettings = {
                fullTextSearch: true,
                forceSelection: false
            }

            $("#" + nuevaActividad.idDropParam).dropdown(
                'setting',
                'onChange',
                function (value, text, $choice) {
                    var posAct = $(this).attr('data-actividad');

                    actividades.agregarParametro(posAct, value);
                    //$(this).attr("placeholder", "Seleccione un parametro");

                }
            ).dropdown("setting", "fullTextSearch", true).dropdown("setting", "forceSelection", false);

            $("#actividades-tab-menu .item").tab();
            $("#" +nuevaActividad.idTab).addClass("active");
            $("#" + nuevaActividad.idDropTMuestreo).dropdown();
            $("#" +nuevaActividad.idDropMatrx).dropdown();
            $("#" +nuevaActividad.idDropMatrx2).dropdown();
            $("#" +nuevaActividad.idDropNorma).dropdown();
            $("#" +nuevaActividad.idDropPaq).dropdown();
            actividades.secuencia = actividades.secuencia + 1;
            $("#nueva-actividad").prop("disabled", false);
        }
    }
    $.ajax(ajaxOptions).fail(function(error) {
        console.log(error.responseText);
    });
    
    
}

actividades.duplicar = function (actividad) {

    var ordenItem = actividades.lista.length + 1;
    //Ultima posicion de las actividades
    var posicionItem = actividades.lista.length;
    actividades.secuencia = actividades.lista.length;
    var Act = actividades.lista[actividad];
    // Operaciones en Memoria
    var item = {
        nombre: "Actividad " + ordenItem,
        idMenu: "actividad-" + actividades.secuencia + "-itemmenu",
        idTab: "actividad-" + actividades.secuencia + "-tab",
        idDropParam: "actividad-" + actividades.secuencia + "-dropparam",
        idDropPaquete: "actividad-" + actividades.secuencia + "-droppaquete",
        idTabla: "actividad-" + actividades.secuencia + "-tabla",
        idDropMatrx: "selector-matrices-" + actividades.secuencia,
        idDropMatrx2: "selector-matrices2-" + actividades.secuencia,
        idDropNorma: "selector-normativa-" + actividades.secuencia,
        idDropPaq: "selector-paquetes-" + actividades.secuencia,
        idCabeceraP: "cabecera-parametro-" + actividades.secuencia,
        DescripcionMuestra: "Descripcion-" + actividades.secuencia,
        idDropTMuestreo: "selector-tipomuestreo-" + actividades.secuencia,
        orden: ordenItem,
        idCabecera: "header-act-" + actividades.secuencia,
        posicion: posicionItem,
        parametros: new Array(),
        sumatoria: 0,
        parametrosSeleccionados: Act.parametrosSeleccionados
    };

    // Operaciones en HTML
    $("#actividades-tab-menu .item").removeClass("active");
    $("#nuevaact .tab").removeClass("active");

    // Agregar div tab
    var tabMenuActividades = $("<div>");
    tabMenuActividades.addClass("active item");
    tabMenuActividades.attr("id", item.idMenu);
    tabMenuActividades.text(item.nombre);
    tabMenuActividades.attr("data-tab", item.idTab);
    tabMenuActividades.append('<div class="ui red label eliminar-actividad" data-actividad="' +
        item.posicion +
        '"><i class="delete icon"></i></div>');
    menuTabsActividades.append(tabMenuActividades);



    var ajaxOptions = {
        url: "/Cotizaciones/RegistrarActividad?a=" + item.posicion + "&c=" + actividades.secuencia,
        beforeSend: function () {
            //$("#nueva-actividad").html('<option> Loading ...</option>');
            $("#nueva-actividad").prop("disabled", true); // disable button
        },
        success: function (partialData) {
          
            $("#nuevaact").append(partialData);

            $("#actividades-tab-menu .item").tab();
            $("#" + item.idDropParam + ".dropdown").dropdown();

            $("#" + item.idCabecera).text("Datos de la Actividad " + item.orden);
            actividades.lista.push(item);

            var dropsettings = {
                fullTextSearch: true,
                forceSelection: false
            }

            $("#" + item.idDropParam).dropdown(
                'setting',
                'onChange',
                function (value, text, $choice) {
                    var posAct = $(this).attr('data-actividad');
                    actividades.agregarParametro(posAct, value);
                    //$(this).attr("placeholder", "Seleccione un parametro");
                }
            ).dropdown("setting", "fullTextSearch", true).dropdown("setting", "forceSelection", false)
                .dropdown("placeholder", false);

            $("#" + item.idDropMatrx).dropdown();
            $("#" + item.idDropMatrx2).dropdown();
            $("#" + item.idDropNorma).dropdown();
            $("#" + item.idDropTMuestreo).dropdown();
            $("#" + item.idDropPaq).dropdown();
            actividades.secuencia = actividades.secuencia + 1;
            $("#nueva-actividad").prop("disabled", false);
            //var table = $("#" + item.idTabla);
            $.each(Act.parametros,
                function (key, value) {

                    //var parametro = item.parametros[key];
                    actividades.agregarParametro(item.posicion, value.IdParametro, value.nombre, key);
                    //item.parametros.push(itemParametro);
                });
            actividades.reportar();
        }
    };
    $.ajax(ajaxOptions);
};