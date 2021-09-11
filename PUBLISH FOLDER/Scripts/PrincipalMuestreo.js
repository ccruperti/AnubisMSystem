//Variables Globales
var tablaCot;
var tablaOM;
var tablaMI;
var tablaCM;

//Listado de las opciones de Tab en la pantalla de muestreo
var vistas = ['cotizaciones', 'OM', 'MI', 'CM'];

$(document).ready(function () {

    $('.ui.menu .item[data-tab]').tab('setting', 'onFirstLoad', function () {
        var table = $(this).find('table.datatable');
        if (table.length) {
            
        }
    });

    $('#receptarMuestra').on('click', function () {
        var checkboxesMuestras = $('input[type="checkbox"][checkbox-group="muestras-ingresadas"]:checked');
        if (checkboxesMuestras.length) {
            receptarMuestras(checkboxesMuestras, $(this).attr('data-modal-url'));
        } else {
            alert('Debe seleccionar al menos una muestra para Receptar');
        }
    });

    muestras.AgregarMuestrasMemoria();
    configuracionTablas();
    configuracionCabecera("cotizaciones", tablaCot);
    configuracionCabecera("OM", tablaOM);
    configuracionCabecera("MI", tablaMI);
    configuracionCabecera("CM", tablaCM);

    //Configuracion de Caja de Busqueda para la tablas
    generarFiltradoBusqueda("cotizaciones", tablaCot);
    generarFiltradoBusqueda("OM", tablaOM);
    generarFiltradoBusqueda("MI", tablaMI);
    generarFiltradoBusqueda("CM", tablaCM);

    //Agregar el Menu como Tab
    $("#tab-menu").tab();

    //Mostrar Cotizaciones
    $("#cotizaciones").show();
    //Esconder Muestreo
    $("#OM").hide();
    $("#MI").hide();
    $("#CM").hide();
    $("#Contenido_cotizaciones").addClass("active");

    //Verifico el Tab que actualmente se encuentra


    //Evento Cambiar a Tab de Cotizaciones
    $('#tab-cotizaciones').on('click', function () {
        //Desactivo los Tabs
        ocultarTabs();
        cambiarTab('cotizaciones');
    });

    //Cambiar a Tab de Ordenes de Muestra
    $('#tab-OM').on('click', function () {
        ocultarTabs();
        cambiarTab('OM');
    });

    //Cambiar a Tab de Muestras Ingresadas
    $("#tab-MI").on('click', function () {
        ocultarTabs();
        cambiarTab('MI');
    });
    //Cambiar a Tab de Consulta de Muestras
    $("#tab-CM").on('click', function () {
        ocultarTabs();
        cambiarTab('CM');
    });

    //Registrar eventos de detalles para cada tabla
    registrarEventosDetalle();

    //Evento de envio de email para OM
    $("#enviarCorreo_OM").on("click", function () {
        //console.log("Pilas que hay click");
        EnviarOM();
    });

    //Evento de recepcion de muestras de laboratorio
    $('#receptarMuestra').on('click', function () {
        receptarMuestras($(this).data("modal-url"));
    });


    $("#muestras-tab-menu .item").tab();


    var datePicker = {
        format: "yyyy-mm-dd",
        weekStart: 1,
        language: "es",
        orientation: "bottom auto",
        todayHighlight: true,
        autoclose: true
    };


    $(".propiedades-tabla").DataTable({
        scrollY: "100px",
        colReorder: true,
        scrollCollapse: true,
        paging: false,
        retrieve: true,
        searching: false,
        bFilter: false,
        ordering: false,
        oLanguage: { "sZeroRecords": "", "sEmptyTable": "" }
    });

    $('.ui.checkbox').checkbox();
    $(".ui.search").dropdown({
        fullTextSearch: true
    });

    $(".switch-estado").on("change",
        function () {
            var muestra = $(this).attr("data-muestra");
            var codigo = $(this).attr("data-codigo");

            var check = $("#muestra-" + muestra + "-switch");
            var idpm = check.attr("data-pmid");
            var ajaxOptions = {
                type: "POST",
                url: "/Muestreo/CambiarEstadoMuestra?IdPm=" +
                    idpm,
                dataType: "json",
                success: function (partialData) {
                    if (check.is(":checked")) {
                        alert("Se ha programado la muestra: " + codigo);
                    } else {
                        alert("La muestra: " + codigo + " queda pendiente!");
                    }


                    //$(".search").search();      
                }
            };
            $.ajax(ajaxOptions);
        });

    $(".fecha").datepicker(datePicker);

    $('.nuevo-material').dropdown('setting', 'fullTextSearch', true);
    $('.nuevo-equipo').dropdown('setting', 'fullTextSearch', true);
    $(".nuevo-material").on("change",
        function () {
            var posMuestra = $(this).children().attr("data-muestra");
            var idmat = $(this).children().val();
            muestras.AgregarMateriales(posMuestra, idmat);

        });
    $(".nuevo-envase").on("change",
        function () {
            var posMuestra = $(this).children().attr("data-muestra");
            var idmat = $(this).children().val();

            muestras.AgregarEnvases(posMuestra, idmat);

        });
    $(".agregar-tecnico").on("change",
        function () {
            var posMuestra = $(this).children().attr("data-muestra");
            var idtec = $(this).children().val();
            muestras.AgregarTecnico(posMuestra, idtec);

        });
    $(".agregar-vehiculo").on("change",
        function () {
            var idveh = $(this).children().val();
            muestras.AgregarVehiculos(idveh);
        });
    //$(".nuevo-equipo").on("change",
    //    function () {
    //        var posMuestra = $(this).children().attr("data-muestra");
    //        var idequipo = $(this).children().val();
    //        muestras.AgregarEnvases(posMuestra, idequipo);

    //    });
    $(".nuevo-equipo").on("change",
        function () {

            var idequipo = $(this).children().val();
            muestras.AgregarEquipos(idequipo);
        });

    function desactivarSeleccionTabla() {
        if (ultimaSeleccion !== 0) {
            $('tr.row').filter('[data-id = ' + ultimaSeleccion + ']').removeClass('active');
        }
    }


    var opcionesSearchSitios = {
        apiSettings: {
            url: '/Muestreo/BuscarSitio?sitio={query}',
            method: 'post'
        },
        fields: {
            results: 'items',
            title: 'Nombre',
            idlciente: 'idcliente'
        },
        searchDelay: 300,
        minCharacters: 3,
        onSelect: function (result, response) {
            result.idcliente = $('#id-sitio').attr("data-cot");
            $('#id-sitio').val(result.IdSitio);
        },
        error: {
            noResults: 'No se han encontrado Sitios con ese nombre. Intente otro nombre.'
        }
    };
    var opcionesSearchClientes = {
        apiSettings: {
            url: '/Muestreo/BuscarCliente?cliente={query}',
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

    var selectedOptions = [];   
    $("body").on("change", ".Envases",
        function () {
            selectedOptions=[];
            $(".Envases option:selected").each(function () {
                if ($(this).text() != 'Seleccione Envase')
                    selectedOptions.push($(this).text());

            });
            $.unique(selectedOptions); 
            $('#cantEtiquetas').text(selectedOptions.length);

        });

}); 

//Variables Globales
var ultimaSeleccion;

// Variables Memoria
var muestras = {
    lista: new Array(),
    secuencia: 0,
    descuento: 0
};

var itemListaEQ = new Array();
var itemListaVEH = new Array();

//Agregar muestras a Memoria
muestras.AgregarMuestrasMemoria = function () {
    $("#muestras-tab-menu .item").each(function (key, element) {
        var ordenItem = key + 1;
        var item = {
            nombre: "muestra- " + ordenItem,
            idMenu: "actividad-" + key + "-itemmenu",
            idTab: "muestra-" + key + "-tab",
            idDropEquipos: "muestra-" + key + "-dropequipos",
            idDropMateriales: "muestra-" + key + "-dropmateriales",
            idDropSitios: "muestra-" + key + "-dropsitios",
            idDropMatriz: "muestra-" + key + "-dropmatrices",
            idDropNormativas: "muestra-" + key + "-dropnormativas",
            idDropTecnicos: "muestra-" + key + "-droptecnicos",
            idTablaTecnico: "muestra-" + key + "-tablatecnico",
            idTablaDetTecnico: "muestra-" + key + "-tabladettecnicos",
            idTablaDetMateriales: "muestra-" + key + "-tabladetmateriales",
            idTablaDetEnvases: "muestra-" + key + "-tabladetenvases",
            idTablaEquipos: "muestra-" + key + "-tablaequipos",
            idTablaMateriales: "muestra-" + key + "-tablamateriales",
            idInputCantidad: "muestra-" + key,
            envases: new Array(),
            materiales: new Array(),
            tecnicos: new Array(),
            orden: ordenItem,
            posicion: key,
            sumatoria: 0
        };
        muestras.secuencia = muestras.secuencia + 1;
        //Agrego las Muestras al Array
        muestras.lista.push(item);
    });
};


//Agregar Materiales 
muestras.AgregarMateriales = function (posMuestra, IdMaterial) {
    var item = muestras.lista[posMuestra];
    var ordenItemMaterial = item.materiales.length + 1;
    var posicionItemMaterial = item.materiales.length;
    console.log("Length de array " + item.materiales.length);
    //Agrego el id del parametro que se ha agregado a la lista de parametros selccionados de la actividad
    var itemMaterial = {
        nombre: (name == null) ? $("#" + item.idDropMateriales + " option:selected").text() : name,
        IdMaterial: parseInt(IdMaterial),
        //id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemMaterial,
        posicion: posicionItemMaterial,
        cantidad: 0,
        IdTabla: item.idTablaMateriales,
        idInputCantidad: "muestra-" + item.posicion + "-material-" + posicionItemMaterial + "-inputcantidad",
        idRow: "muestra-" + item.posicion + "-material-" + posicionItemMaterial + "-idrow"
    };
    var table = $("#" + item.idTablaMateriales);
    var tabledet = $("#" + item.idTablaDetMateriales);
    var ajaxOptions = {
        url: "/Muestreo/RegistrarMateriales?m=" +
            posMuestra +
            "&IdMaterial=" +
            IdMaterial +
            "&c=" +
            posicionItemMaterial,
        success: function (partialData) {
            table.append(partialData);

            tabledet.append(partialData);
            item.materiales.push(itemMaterial);

            //$(".search").search();      
        }
    };
    $.ajax(ajaxOptions);
};
//Agregar Envases
muestras.AgregarEnvases = function (posMuestra, IdMaterial) {
    console.log("id muestra: " + posMuestra);
    var item = muestras.lista[posMuestra];
    var ordenItemEquipo = item.materiales.length + 1;
    var posicionItemEquipo = item.materiales.length;
    //Agrego el id del parametro que se ha agregado a la TAB2 de parametros selccionados de la actividad
    var itemEnvases = {
        nombre: (name == null) ? $("#" + item.idDropEquipos + " option:selected").text() : name,
        IdMaterial: parseInt(IdMaterial),
        //id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemEquipo,
        posicion: posicionItemEquipo,
        cantidad: 0,
        IdTabla: item.idTablaEquipos,
        idInputCantidad: "muestra-" + item.posicion + "-equipo-" + posicionItemEquipo + "-inputcantidad",
        idRow: "muestra-" + item.posicion + "-equipo-" + posicionItemEquipo + "-idrow"
    };
    var table = $("#" + item.idTablaEquipos);
    var ajaxOptions = {
        url: "/Muestreo/RegistrarMateriales?m=" +
            posMuestra +
            "&IdMaterial=" +
            IdMaterial +
            "&c=" +
            posicionItemEquipo,
        success: function (partialData) {
            table.append(partialData);
            item.materiales.push(itemEnvases);

            //$(".search").search();      
        }
    };
    $.ajax(ajaxOptions);
};
//Agregar Tecnico
muestras.AgregarTecnico = function (posMuestra, IdPersonal) {
    console.log("id muestra: " + posMuestra);
    var item = muestras.lista[posMuestra];
    var ordenItemTecnico = item.tecnicos.length + 1;
    var posicionItemTecnico = item.tecnicos.length;
    //Agrego el id del parametro que se ha agregado a la lista de parametros selccionados de la actividad
    var itemTecnicos = {
        nombre: (name == null) ? $("#" + item.idDropTecnicos + " option:selected").text() : name,
        idPersonal: parseInt(IdPersonal),
        //id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemTecnico,
        posicion: posicionItemTecnico,
        cantidad: 0,
        IdTabla: item.idTablaTecnico,
        IdTablaDet: "muestra-" + posicionItemTecnico + "-tabladettecnicos",
        idRow: "muestra-" + item.posicion + "-tecnico-" + posicionItemTecnico + "-idrow"
    };
    var table = $("#" + item.idTablaTecnico);
    var tabladet = $("#" + item.idTablaDetTecnico);
    var ajaxOptions = {
        url: "/Muestreo/RegistrarTecnicos?m=" +
            posMuestra +
            "&IdPersonal=" +
            IdPersonal +
            "&c=" +
            posicionItemTecnico,
        success: function (partialData) {
            table.append(partialData);
            tabladet.append(partialData);
            item.tecnicos.push(itemTecnicos);

            //$(".search").search();      
        }
    };
    $.ajax(ajaxOptions);
};
//Agregar Equipo
muestras.AgregarEquipos = function (IdEquipo) {

    var ordenItemEquipo = itemListaEQ.length + 1;
    var posicionItemEquipo = itemListaEQ.length;
    //Agrego el id del parametro que se ha agregado a la lista de parametros selccionados de la actividad
    var itemEquipos = {
        nombre: (name == null) ? $("#om-dropequipos" + " option:selected").text() : name,
        idequipo: parseInt(IdEquipo),
        //id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemEquipo,
        posicion: posicionItemEquipo,
        IdRow: "equipo-" + posicionItemEquipo + "-idrow"

    };
    var table = $("#muestra-tablaequipos");
    var ajaxOptions = {
        url: "/Muestreo/RegistrarEquipos?c=" +
            posicionItemEquipo +
            "&IdEquipo=" +
            IdEquipo,

        success: function (partialData) {
            table.append(partialData);
            itemListaEQ.push(itemEquipos);
            //$(".search").search();      
        }
    };
    $.ajax(ajaxOptions);
};
//Agregar Vehiculos
muestras.AgregarVehiculos = function (IdVehiculo) {

    var ordenItemVehiculo = itemListaVEH.length + 1;
    var posicionItemVehiculo = itemListaVEH.length;
    var itemVehiculo = {
        nombre: (name == null) ? $("#om-dropequipos" + " option:selected").text() : name,
        idvehiculo: parseInt(IdVehiculo),
        //id: "#actividad" + item.posicion + "-parametro" + posicionItemParametro,
        orden: ordenItemVehiculo,
        posicion: posicionItemVehiculo,
        IdRow: "vehiculo-" + posicionItemVehiculo + "-idrow"

    };
    var table = $("#muestra-tablavehiculos");
    var ajaxOptions = {
        url: "/Muestreo/RegistrarVehiculos?c=" +
            posicionItemVehiculo +
            "&IdVehiculo=" +
            IdVehiculo,

        success: function (partialData) {
            table.append(partialData);
            itemListaVEH.push(itemVehiculo);

            //$(".search").search();      
        }
    };
    $.ajax(ajaxOptions);
};
//Eliminar Materiales
muestras.eliminarMaterial = function (posMuestra, posMat) {

    console.log("Pos Muestra:" + posMuestra, "Pos Mat : " + posMat);
    var item = muestras.lista[posMuestra].materiales[posMat];
    //Elimino el parametro seleccionado

    var muestra = muestras.lista[posMuestra];
    $("#" + item.idRow).remove();
    muestras.lista[posMuestra].materiales.splice(posMat, 1);

    $.each(muestras.lista[posMuestra].materiales,
        function (key, material) {
            if (material.posicion > posMat) {
                //Recupero el row que esta en esa posicion y le disminuyo en 1 su posicion
                var newIdRow = 'muestra-' + posMuestra + '-material-' + (material.posicion - 1) + '-idrow';
                $('#muestra-' + posMuestra + '-material-' + material.posicion + '-idrow').attr('id', newIdRow);
                material.idRow = newIdRow;
                muestras.reordenarMaterialMuestra(posMuestra, material.posicion, material.posicion - 1);
                material.posicion = material.posicion - 1;
            }
        });

};
$("#nuevaact").on("click",
    ".eliminarmaterial",
    function () {
        var boton = $(this);
        var act = boton.attr("data-muestra");
        var material = boton.attr("data-material");

        muestras.eliminarMaterial(act, material);

    });
//Eliminar Tecnicos
muestras.eliminarTecnico = function (posMuestra, posTec) {

    console.log("Pos Muestra:" + posMuestra, "Pos Tec : " + posTec);
    var item = muestras.lista[posMuestra].tecnicos[posTec];
    //Elimino el parametro seleccionado

    var muestra = muestras.lista[posMuestra];
    $("#" + item.idRow).remove();
    muestras.lista[posMuestra].tecnicos.splice(posTec, 1);

    $.each(muestras.lista[posMuestra].tecnicos,
        function (key, tecnico) {
            if (tecnico.posicion > posTec) {
                //Recupero el row que esta en esa posicion y le disminuyo en 1 su posicion
                var newIdRow = 'muestra-' + posMuestra + '-tecnico-' + (tecnico.posicion - 1) + '-idrow';
                $('#muestra-' + posMuestra + '-tecnico-' + tecnico.posicion + '-idrow').attr('id', newIdRow);
                tecnico.idRow = newIdRow;
                muestras.reordenarTecnicoMuestra(posMuestra, tecnico.posicion, tecnico.posicion - 1);
                tecnico.posicion = tecnico.posicion - 1;
            }
        });

};
$("#nuevaact").on("click",
    ".eliminartecnico",
    function () {
        var boton = $(this);
        var tec = boton.attr("data-tecnico");
        var muestra = boton.attr("data-muestra");

        muestras.eliminarTecnico(muestra, tec);

    });
//Eliminar Equipos
muestras.eliminarEquipo = function (posEquipo) {

    //console.log("Pos Muestra:" + posMuestra, "Pos Mat : " + posEquipo);
    var item = itemListaEQ[posEquipo];
    console.log(item);
    //Elimino el equipo seleccionado
    $("#" + item.IdRow).remove();
    itemListaEQ.splice(posEquipo, 1);
    $.each(item,
        function (key, equipo) {
            if (equipo.posicion > posEquipo) {
                //Recupero el row que esta en esa posicion y le disminuyo en 1 su posicion
                var newIdRow = 'equipo-' + (equipo.posicion - 1) + '-idrow';
                $('#equipo-' + equipo.posicion + '-idrow').attr('id', newIdRow);
                equipo.IdRow = newIdRow;
                muestras.reordenarEquipoMuestra(equipo.posicion, equipo.posicion - 1);
                equipo.posicion = equipo.posicion - 1;
            }
        });

};

//Eliminar Vehiculos 
muestras.eliminarVehiculos = function (posVehiculo) {

    //console.log("Pos Muestra:" + posMuestra, "Pos Mat : " + posEquipo);
    var item = itemListaVEH[posVehiculo];
    console.log(item);
    //Elimino el equipo seleccionado
    $("#" + item.IdRow).remove();
    itemListaVEH.splice(posVehiculo, 1);
    $.each(item,
        function (key, vehiculo) {
            if (vehiculo.posicion > posVehiculo) {
                //Recupero el row que esta en esa posicion y le disminuyo en 1 su posicion
                var newIdRow = 'vehiculo-' + (vehiculo.posicion - 1) + '-idrow';
                $('#vehiculo-' + vehiculo.posicion + '-idrow').attr('id', newIdRow);
                vehiculo.IdRow = newIdRow;
                muestras.reordenarVehiculoLista(vehiculo.posicion, vehiculo.posicion - 1);
                vehiculo.posicion = vehiculo.posicion - 1;
            }
        });

};
$("body").on("click", ".eliminarequipo",
    function () {
        var boton = $(this);
        var equipo = boton.attr("data-equipo");
        muestras.eliminarEquipo(equipo);

    });
$("body").on("click", ".eliminarvehiculo",
    function () {
        var boton = $(this);
        var vehiculo = boton.attr("data-vehiculo");
        muestras.eliminarVehiculos(vehiculo);

    });
//Reordenar Posicion Tabla Materiales
muestras.reordenarMaterialMuestra = function (posMuestra, posMat, newPosMat) {
    var inputList = $('input[data-muestra="' + posMuestra + '"][data-type="material"][data-material="' + posMat + '"');
    //Recupero el boton de la fila y cambio su secuencia de parametro
    var botonDelete = $('button[data-material="' + posMat + '"]');
    botonDelete.attr("data-material", newPosMat);
    //var inputList = $('.input-parametro-' + posMuestra);
    $.each(inputList,
        function (key, item) {
            var input = $(item);
            // Nombre formulario del input
            var inputName = input.data("name");
            //alert("esta es la posicion a reordenar"+ newPosMat);
            input.attr("data-material", newPosMat);
            input.attr("name", "Muestras[" + posMuestra + "].Materiales[" + newPosMat + "]." + inputName);
        });
};
//Reordenar items tabla Tecnicos
muestras.reordenarTecnicoMuestra = function (posMuestra, posTec, newPosTec) {
    var inputList = $('input[data-muestra="' + posMuestra + '"][data-type="tecnico"][data-tecnico="' + posTec + '"');
    //Recupero el boton de la fila y cambio su secuencia de parametro
    var botonDelete = $('button[data-tecnico="' + posTec + '"]');
    botonDelete.attr("data-tecnico", newPosTec);
    //var inputList = $('.input-parametro-' + posMuestra);
    $.each(inputList,
        function (key, item) {
            var input = $(item);
            // Nombre formulario del input
            var inputName = input.data("name");
            //alert("esta es la posicion a reordenar"+ newPosMat);
            input.attr("data-tecnico", newPosTec);
            input.attr("name", "Muestras[" + posMuestra + "].Tecnicos[" + newPosTec + "]." + inputName);
        });
};
// Reordenar Posicion Tabla Equipos
muestras.reordenarEquipoMuestra = function (posEquipo, newposEquipo) {
    var inputList = $('input[data-type="equipo"][data-equipo="' + posEquipo + '"');
    //Recupero el boton de la fila y cambio su secuencia de parametro
    var botonDelete = $('button[data-equipo="' + posEquipo + '"]');
    botonDelete.attr("data-equipo", newposEquipo);
    //var inputList = $('.input-parametro-' + posMuestra);
    $.each(inputList,
        function (key, item) {
            var input = $(item);
            // Nombre formulario del input
            var inputName = input.data("name");
            //alert("esta es la posicion a reordenar"+ newposEquipo);
            input.attr("data-equipo", newposEquipo);
            input.attr("name", "Equipos[" + newposEquipo + "]." + inputName);
        });
};
muestras.reordenarVehiculoLista = function (posVehiculo, newposVehiculo) {
    var inputList = $('input[data-type="vehiculo"][data-vehiculo="' + posVehiculo + '"');
    //Recupero el boton de la fila y cambio su secuencia de parametro
    var botonDelete = $('button[data-vehiculo="' + posVehiculo + '"]');
    botonDelete.attr("data-vehiculo", newposVehiculo);
    //var inputList = $('.input-parametro-' + posMuestra);
    $.each(inputList,
        function (key, item) {
            var input = $(item);
            // Nombre formulario del input
            var inputName = input.data("name");
            //alert("esta es la posicion a reordenar"+ newposEquipo);
            input.attr("data-equipo", newposVehiculo);
            input.attr("name", "Equipos[" + newposVehiculo + "]." + inputName);
        });
};
function obtenerSeleccion() {
    //Recupero todos los checks de cotizaciones
    var checks = $(table.rows().nodes()).find('.cots');
    console.log(checks);
    var listaCot = new Array();
    //Itero las lista de cheks de cotizaciones
    $.each(checks,
        function (key, check) {
            if (check.checked) {
                //El elemento box esta seleccionado
                var element = check.closest("tr").getAttributeNode("data-id").value;
                console.log(element);
                //Si estado masivo
                listaCot.push(parseFloat(element));

            }
        });
    return listaCot;
};
function obtenerSeleccionActs() {
    //Recupero todos los checks de cotizaciones
    var tablaact = $("#tabla-acts");
    var checks = $(tablaact).children().find('.acts');
    console.log(checks);
    var listaCot = [];
    //Itero las lista de cheks de cotizaciones
    $.each(checks,
        function (key, check) {
            if ($(check).is(":checked")) {
                //El elemento box esta seleccionado
                var element = check.closest("tr").getAttributeNode("data-id").value;
                console.log(element);
                //Si estado masivo
                listaCot.push(parseFloat(element));
                console.log(listaCot);

            }
        });
    return listaCot;
};
//Funcion que permite programar muestras de diferentes actividades
$("#boton-programar").on("click",
    function () {

        var elements2 = obtenerSeleccionActs();
        //var tablacot = $("#tabla-act").DataTable();
        var id = $("#tabla-acts").attr('data-cot');

        if (elements2.length === 0) {
            alert("Debe Seleccionar una actividad primero");
        } else {
            $(this).addClass('ui primary loading button');
            $(this).unbind('click');
            var data = {
                Actividades: elements2,
                IdCot: id
            }
            JSON.stringify(data);
            var opciones = {
                url: '/Muestreo/RegistrarOrdenMuestreo',
                type: "GET",
                dataType: 'json',
                data: data,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {


                },
                complete: function (data) {
                    location.href = this.url;
                }
            };
            jQuery.ajaxSettings.traditional = true;

            $.ajax(opciones);
        }
    });
$("#boton-ingresarmuestras").on("click",
    function () {
        $(this).addClass('ui loading button');
        $(this).unbind('click');
        var elements2 = obtenerSeleccionActs();
        //var tablacot = $("#tabla-act").DataTable();
        var id = $("#tabla-acts").attr('data-cot');

        if (elements2.length === 0) {
            alert("Debe Seleccionar una actividad primero");
        } else {
            var data = {
                Actividades: elements2,
                IdCot: id
            }
            JSON.stringify(data);
            var opciones = {
                url: '/Muestreo/IngresoMuestras',
                type: "GET",
                dataType: 'json',
                data: data,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {


                },
                complete: function (data) {
                    location.href = this.url;
                }
            };
            jQuery.ajaxSettings.traditional = true;

            $.ajax(opciones);
        }
    });
$('#visualizar-actividades').on('click',
    function () {
        //Obtengo las cotizaciones seleccionadas
        var elements = obtenerSeleccion();

        //Declaro las opciones iniciales para la peticion
        if (elements.length === 0) {
            alert("Debe Seleccionar una cotizacion primero");
        } else {

            //Preparo la solicitud para listado de actividades
            var opciones = {
                url: '/Muestreo/ListadoActividades',
                type: "POST",
                data: { cotizaciones: elements },
                success: function (partialData) {
                    $.each(elements,
                        function (key, value) {
                            $("#cont-act").html("");
                            $("#cont-act").append(partialData);


                        });

                    $('#modal-actividades').modal('show');


                }
            };
            $.ajax(opciones);
        }
    });

//Modal de programacion de actividades
$(".progbutton").on("click",
    function () {
        var idcot = $(this).attr("data-IdCot");
        //Preparo la solicitud de cambio de estado
        var opciones = {
            url: '/Muestreo/ListadoActividades',
            type: "POST",
            data: { cotizaciones: idcot },
            success: function (partialData) {
                $("#cont-act").html("");
                $("#cont-act").append(partialData);
                $('#modal-actividades').modal('show');
                $('#check-act').on("change", function () {
                    var checkboxes = $("#tabla-acts").find('.acts');
                    checkboxes.prop('checked', $(this).is(':checked'));
                });
            }
        };
        $.ajax(opciones);
    });


//var table = $("#tabla-act").DataTable({
//    scrollY: "200px",
//    colReorder: true,
//    scrollCollapse: true,
//    dom: "tipr"

//}).draw();

//var table5 = $("#tabla-cotsaprobadas").DataTable({
//    scrollY: "200px",
//    colReorder: true,
//    scrollCollapse: true,
//    dom: "tipr"

//}).draw();

//var table2 = $("#myTable2").DataTable({
//    scrollY: "200px",
//    colReorder: true,
//    scrollCollapse: true

//}).draw();
//$("#myTable5").DataTable({
//    scrollY: "200px",
//    colReorder: true,
//    scrollCollapse: true

//})


//$("#clicktab3").on("click",
//    function () {
//        $("#row2").hide();
//        $("#row1").hide();
//        $("#row4").hide();

//        $("#Tab1").hide();
//        $("#Tab2").hide();
//        $("#Tab4").hide();


//        $("#row3").show();
//        $("#Tab3").show();
//        $("#Tab1").removeClass("active");
//        $("#Tab2").removeClass("active");
//        $("#Tab4").removeClass("active");

//        $("#Tab3").addClass("active");
//        $('#clicktab1').removeClass('active');
//        $('#clicktab2').removeClass('active');
//        $('#clicktab4').removeClass('active');

//        $('#clicktab3').addClass('active');
//        var DesdeOM = $('#Desde3').val();
//        var hastaOM = $('#Hasta3').val();
//        //$.get('/Muestreo/ListadoOM2' + '?Desde2=' + DesdeOM + '&Hasta2=' + hastaOM, function (data) {
//        //    $("#Tab3").html("");
//        //    $("#Tab3").html(data);
//        //});
//    });

//$("#clicktab4").on("click",
//    function () {
//        $("#row2").hide();
//        $("#row1").hide();
//        $("#row3").hide();
//        $("#Tab1").hide();
//        $("#Tab2").hide();
//        $("#Tab3").hide();

//        $("#row4").show();
//        $("#Tab4").show();
//        $("#Tab1").removeClass("active");
//        $("#Tab2").removeClass("active");
//        $("#Tab3").removeClass("active");
//        $("#Tab4").addClass("active");
//        $('#clicktab1').removeClass('active');
//        $('#clicktab2').removeClass('active');
//        $('#clicktab3').removeClass('active');
//        $('#clicktab4').addClass('active');
//        var DesdeOM = $('#Desde3').val();
//        var hastaOM = $('#Hasta3').val();
//        //$.get('/Muestreo/ListadoOM' + '?Desde2=' + DesdeOM + '&Hasta2=' + hastaOM, function (data) {
//        //    $("#tab3").html("");
//        //    $("#Tab3").html(data);
//        //    $('#Desde3').on('changeDate',
//        //        function (e) {
//        //            //var fecha = new Date(new Date(e.date)).addDays(6);
//        //            fechadesde = new Date(new Date(e.date));
//        //            $('#Desde3').val(fechadesde.formatYYYYMMDDD());
//        //            //$('#filtroFecha2').submit();

//        //        });

//        //    $('#Hasta3').on('changeDate',
//        //        function (e) {
//        //            //var fecha = new Date(new Date(e.date)).addDays(-6);
//        //            fechahasta = new Date(new Date(e.date));
//        //            $('#Hasta3').val(fechahasta.formatYYYYMMDDD());

//        //            //$('#filtroFecha2').submit();
//        //        });
//        //});


//    });

$('#Tab3').on('click',
    '#myTable5 tr.detalle',
    function (e) {
        $('.detalle').removeClass('active');
        $(this).addClass('active');
        var loading = $('#detalle2');
        var urlLoad = $(this).data('enlacemuestras');
        console.log(urlLoad);
        loading.html('');
        //loading.addClass('loading');
        loading.load(urlLoad);
    });

//$('#Tab4').on('click',
//    '#myTable2 tr.detalle',
//    function (e) {
//        $('.detalle').removeClass('active');
//        $(this).addClass('active');
//        var loading = $('#detalle2');
//        var urlLoad = $(this).data('enlacemuestras');
//        console.log(urlLoad);
//        loading.html('');
//        //loading.addClass('loading');
//        loading.load(urlLoad);

//    });
$('#Tab1').on('click',
    '#tabla-cotsaprobadas tr.detallellamada',
    function (e) {
        $('.detallellamada').removeClass('active');
        $(this).addClass('active');
        var loading = $('#detalle-llamadas');
        var urlLoad = $(this).data('enlace');
        console.log(urlLoad);
        loading.html('');
        //loading.addClass('loading');
        loading.load(urlLoad);

    });


//Evento click que muestra el detalle de la muestra dentro del detalle del plan de muestreo
$("#Contenido_OM").on("click", "#detalle_OM tr.detalle_plan_muestra", function () {
    console.log("click en detalle de muestra");
    $(".detalle_plan_muestra").removeClass('active');
    $(this).addClass('active');
    var loading = $('#detallemuestra');
    var urlLoad = $(this).data('enlace');
    loading.html('');
    loading.load(urlLoad);
});


$("body").on("click", "tr.detalle-m",
    function () {
        $('.detdetalle-m').removeClass('active');
        $(this).addClass('active');
        var loading = $('#detallemuestra');
        var urlLoad = $(this).data('enlace');
        console.log(urlLoad);
        loading.html('');
        loading.load(urlLoad);
    });

//$("#clicktab3").on("click",
//    function () {
//        $("#row1").hide();
//        $("#row2").hide();
//        $("#row3").show();
//        $("#Tab1").hide();
//        $("#Tab2").hide();
//        $("#Tab3").show();
//        $("#Tab1").removeClass("active");
//        $("#Tab2").removeClass("active");
//        $("#Tab3").addClass("active");
//        $('#clicktab1').removeClass('active');
//        $('#clicktab2').removeClass('active');
//        $('#clicktab3').addClass('active');
//        $.get('/Muestreo/ListadoOM', function (data) {
//            $("#Tab3").html(data);

//        });

//    });

function CargarMuestras(muestra) {
    var tab = $("#muestra-" + muestra + "-tab");
    var seleccionmuestra = muestras.lista[muestra];
    var idmuestra = tab.attr("data-mid");
    $("#actividades-tab-menu .item.active").removeClass("active");
    tab.addClass("active");
    var ajaxOptions = {
        url: "/Muestreo/PlanMuestreo?m=" + muestra + "&Id=" + idmuestra,

        success: function (partialData) {
            $("#nuevaact").append(partialData);
            $("#muestras-tab-menu .item").tab();

            $(".ui.search").dropdown({
                fullTextSearch: true
            });

        },
        complete: function () {

        }

    };
    $.ajax(ajaxOptions);

};

//Pantalla Registro Orden Muestreo
$("#muestras-tab-menu").on("click", ".selectores", function () {
    var muestra = $(this).attr("data-muestra");
    var cliente = $(this).attr("data-cliente");
    var idsitio = $(this).attr("data-idsitio");

    console.log(cliente);
    var check = $("#muestra-" + muestra + "-switch");
    var seleccion = muestras.lista[muestra];
    console.log(check);
    var tab = $("#muestra-" + muestra + "-tab");
    var id = $("#muestra-" + muestra + "-itemmenu").attr("data-muestraId");
    $("#muestras-tab-menu .item").removeClass("active");
    $("#muestra-" + muestra + "-itemmenu").addClass("active");
    $("#nuevaact .tab").removeClass("active");

    $('.nuevo-material').dropdown('setting', 'fullTextSearch', true);
    $('.nuevo-equipo').dropdown('setting', 'fullTextSearch', true);
    console.log(id);

    var ajaxOptions = {
        url: "/Muestreo/PlanMuestreo?m=" + muestra + "&Id=" + id,
        success: function (partialData) {
            if (check.is(':checked')) {
                tab.addClass("active");

                $("#nuevaact").append(partialData);
                tab.show();
                $("#hora-desde-" + muestra).mdtimepicker();
                $("#hora-hasta-" + muestra).mdtimepicker();
                $(".propiedades-tabla").DataTable({
                    scrollY: "100px",
                    colReorder: true,
                    scrollCollapse: true,
                    paging: false,
                    searching: false,
                    retrieve: true,
                    bFilter: false,
                    ordering: false,
                    oLanguage: { "sZeroRecords": "", "sEmptyTable": "" }
                });

                $("#muestra-" + muestra + "-droptecnicos").dropdown();

                $("#muestra-" + muestra + "-dropsitiosingreso").dropdown();
                var opcionesSearchSitios = {
                    apiSettings: {
                        url: '/Muestreo/BuscarSitio?sitio={query}' + '&IdCliente=' + cliente,
                        method: 'post'
                    },
                    fields: {
                        results: 'items',
                        title: 'Nombre'
                    },
                    searchDelay: 300,
                    minCharacters: 3,
                    onSelect: function (result, response) {

                        $("#id-" + muestra + "-sitio").val(result.IdSitio);
                        var option = $("<option>");
                        option.val(result.IdMatriz);

                        $("#muestra-" + muestra + "-dropmatrices").append(option);
                        option.attr("selected", option.val());
                        console.log("ESTE ES EL ", idsitio);
                        cargarDatosSitio(muestra, result.IdSitio);
                    },
                    error: {
                        noResults: 'No se han encontrado Sitios con ese nombre o no se lo ha agregado al cliente. Intente agregando un nuevo sitio'
                    }
                };
                $("#search-" + muestra + "-sitios").search(opcionesSearchSitios);
                $("#muestra-" + muestra + "-botonparam").on("click",
                    function () {

                        $("#modal-" + muestra + "-param").modal("show");
                        $(".props-params").DataTable({
                            scrollY: "400px",
                            colReorder: true,
                            scrollCollapse: true,
                            paging: false,
                            bFilter: false,
                            oLanguage: { "sZeroRecords": "", "sEmptyTable": "" },

                        });
                        $("#muestra-" + muestra + "-modaldetalle").on("click",
                            function () {
                                var sit = $("#muestra-" + muestra + "-dropsitios").val();
                                console.log("intentando cargar sitio: " + sit);
                                $("modal-" + sit + "-ident").modal("show");
                                //cargarDatosSitio(muestra, sit);


                            });
                        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                        $("#muestra-" + muestra + "-botoncerrar").on("click",
                            function () {

                                $("#modal-" + muestra + "-param").destroy();
                            });
                    });
                var sitio = null;
                $(".dropsitios").on("change",
                    function () {
                        sitio = $(this).children().val();
                        cargarDatosSitio(muestra, sitio);

                    });



            } else {
                tab.hide();
                console.log("Hidden");
            }

            $("#muestras-tab-menu .item").tab();
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }
    };
    $.ajax(ajaxOptions);

});

$(".boton-modal").on("click",
    function () {
        var muestra = $(this).attr("data-muestra");
        $("#modal-" + muestra + "-sitios").modal("show");
    });

$(".mostrar-modal-detalle").on("click",
    function () {
        var m = $(this).attr("data-muestra");
        console.log("ESTOY EN EL MODAL: " + m);
        var sitio = $("#muestra-" + m + "-dropsitios").val();
        $("#modal-" + m + "-ident").modal("show");
        cargarDatosSitio(m, sitio);

    });
function SitioNuevo(muestra, c) {
    console.log("Presiono boton de guardar");
    $("#matriz-" + muestra + "-sitio").dropdown();
    var clavesitio = $("#clavesitio-" + muestra + "-sitio").val();
    var norma = $("#norma-" + muestra + "-sitio").val();
    var lugardescarga = $("#lugard-" + muestra + "-sitio").val();
    var matriz = $("#matriz-" + muestra + "-sitio").val();
    var nombre = $("#nombre-" + muestra + "-sitio").val();
    var latitud = $("#Nlatitud-" + muestra + "-sitio").val();
    var longitud = $("#Nlongitud-" + muestra + "-sitio").val();
    var caudal = $("#Ncaudal-" + muestra + "-sitio").val();

    console.log("ESTA ES LA LAAAAT!:", latitud);
    $.ajax({
        type: 'POST',
        url: '/Muestreo/AgregarSitio?IdCliente=' + c + "&IdMatriz=" + matriz + "&Nombre=" + nombre
            + "&NormaTecnica=" + norma + "&LugarDescarga=" + lugardescarga + "&ClaveSitio=" + clavesitio + "&Longitud=" + longitud
            + "&Latitud=" + latitud + "&Caudal=" + caudal,
        //data: {
        //    Nombre: nombre,
        //    NormaTecnica: norma,
        //    ClaveSitio: clavesitio,
        //    LugarDescarga: lugardescarga,
        //    IdMatriz: matriz,
        //    Longitud: longitud,
        //    Latitud: latitud
        //},
        dataType: 'json',
        success: function (data) {
            $("#modal-" + muestra + "-sitios").html("");
            var option = $("<option>");
            option.val(data.IdSitio);
            option.text(data.Nombre);
            console.log("NOMBRE GUARDADO: ", data.Nombre);
            $("#muestra-" + muestra + "-dropsitios").append(option);


        },
        complete: function () {
            //clavesitio.text("");
            //norma.text("");
            //lugardescarga.text("");
            //nombre.text("");
            //latitud.text("");
            //longitud.text("");
        }
    });
}
//function filtrarDropMatrices(muestra, selectedvalue) {
//    var drop = $("muestra" + muestra + "-dropmatrices");
//    var ajaxOptions = {
//        url: "/Muestreo/PlanMuestreo?m=" + muestra + "&Id=" + id,
//        success: function(partialData) {

//        }
//    };
$(".mostrar-modal").on("click",
    function () {
        $("#modal-param").modal("show");
    });

$("#boton-agregarcontacto").on("click",
    function () {
        $("#modal-contacto").modal("show");
        $("#agregar-contacto").on("click",
            function () {

                var cliente = $(this).attr("data-cliente");
                console.log(cliente);
                var correo = $("#input-contacto-correo").val();
                var telefono = $("#input-contacto-numtelf").val();
                var nombre = $("#input-contacto-nombre").val();
                var tipocont = $("#tipo-contacto").val();
                $.ajax({
                    type: 'POST',
                    url: '/Muestreo/AgregarContacto?IdCliente=' + cliente,
                    data: {
                        Nombre: nombre,
                        NumTelf: telefono,
                        Correo: correo,
                        IdTipoContacto: tipocont


                    },
                    dataType: 'json',
                    success: function (data) {
                        var option = $("<option>");
                        option.val(data.IdContacto);
                        option.text(data.Nombre);
                        $("#drop-contactos").append(option);

                    }
                });
            });
    });
$("#actividad-itemmenu2").on("click",
    function () {
        $(".item.active").removeClass("active");
        $(this).addClass("active");
        $("#Tab2").addClass("item active");
        $("#Tab2").show();
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });
$("#actividad-itemmenu1").on("click",
    function () {
        $(".item.active").removeClass("active");
        $(this).addClass("active");
        $("#Tab1").addClass("item active");
        $("#Tab1").show();
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });

function syncInputs(posMuestra) {
    var inputcant = $("#muestra-" + posMuestra).val();
    var materiales = muestras.lista[posMuestra].materiales;
    materiales.map(function (elem, inx, ob) {
        console.log(elem);
        $("#" + elem.idInputCantidad).val(inputcant);
    });

    $.each(materiales,
        function (key, val) {
            val.cantidad = inputcant;
        });
};

$(".logbutton").on("click",
    function () {
        var id = $(this).attr("data-IdCot");
        console.log(id);
        $.ajax({
            type: 'GET',
            url: '/PrincipalMuestreo/LogLlamada?IdCotizacion=' + id, 
            success: function (data) {
                $('#modal-log .ui.log.modal').html(data);
                $(".ui.log.modal").modal("show");
            }
        });
    });

$("#Modal-Calendario").on("click",
    function () {
        $('.ui.gen.modal').modal().modal('attach events', '.Modalus', 'show');

    });

function TimeFix(durationInMinutes, minTime) {
    var hour = moment(minTime, "HH:mm");
    $(".fc-body .fc-slats table tr").each(function (index) {
        $(this).find("td.fc-widget-content").eq(0).html("<span>" + hour.format("HH:mm") + "</span>");
        hour.add(durationInMinutes, "minutes");
    });
}

$(".confirmation").on('click', function () {
    return confirm('Está seguro de que desea salir?');
});
$("#check-viajes").on("change",
    function () {
        if ($(this).is(":checked")) {
            $("#fecha-monitoreo").removeAttr("disabled");
            $("#hora-monitoreo").removeAttr("disabled");
            $("#hora-monitoreo2").removeAttr("disabled");

            $("#Modal-Calendario").attr("disabled", "disabled");

        } else {
            $("#fecha-monitoreo").attr("disabled", "disabled");
            $("#hora-monitoreo").attr("disabled", "disabled");
            $("#hora-monitoreo2").attr("disabled", "disabled");

            $("#Modal-Calendario").removeAttr("disabled");


        }
    });
function cargarDatosSitio(muestra, idsitio) {
    if (idsitio != null || idsitio !== "")
        $.ajax({
            type: "Get",
            url: "/Muestreo/ObtenerDatosSitio",
            data: { idsitio: idsitio },
            dataType: "json",
            success: function (data) {
                //Cargar datos en pantalla de registro
                $("#muestra-" + muestra + "-txtident").html("");
                $("#lugar-" + muestra + "-sitio").val(data['lugar']);
                $("#muestra-" + muestra + "-txtident").val(data['nombre']);
                $("#tiopomuestreo-" + muestra + "-sitio").val(data['unidades']);
                $("#norma-" + muestra + "-txtnorma").text(data['norma']);
                $("#latitud-" + muestra + "-sitio").val(data['latitud']);
                $("#longitud-" + muestra + "-sitio").val(data['longitud']);
                $("#caudal-" + muestra + "-sitio").val(data['caudal']);
                $("#lugar-" + muestra + "-sitio").val(data['lugar']);
                //Carga datos en pantalla pop up de detalle
                $("#muestra-" + muestra + "-identpuntodetalle").val(data['nombre']);
                $("#muestra-" + muestra + "-normatecdetalle").text(data['norma']);
                $("#muestra-" + muestra + "-latdetalle").val(data['latitud']);
                $("#muestra-" + muestra + "-longdet").val(data['longitud'])
                var option = $("<option>");
                option.val(data['matriz']);
                option.attr("selected", data['matriz']);
                option.text(data['nombrematriz']);
                console.log(option, "OPCION");
                var options = $("#muestra-" + muestra + "-dropmatrices").find("option");
                $("#muestra-" + muestra + "-dropmatrices").dropdown();
                $.each(options,
                    function (key, value) {
                        if ($(value).attr("value") === option.attr("value")) {
                            value.remove();
                        }
                    });

                $("#muestra-" + muestra + "-dropmatrices").append(option);
                //$("#muestra-" + muestra + "-matrizdetalle").append(option);
            }
        });
}
function CargarContactos() {

    var IdCliente = $("#id-cliente").val();

    $.post("/Muestreo/JsonContactosMuestreo?IdCliente=" + IdCliente, function (data) {
        if (data.length === 0) {
            $("#drop-contactos").children().remove();

            var noresults = $("<option>").text("No hay Contactos Registrados").val("");
            $("#drop-contactos").append(noresults);
            $("#drop-contactos").prop("disabled", true);

        } else {
            $("#drop-contactos").children().remove();

            $("#drop-contactos").prop("disabled", false);
            $.each(data,
                function (key, reg) {
                    console.log(reg);
                    var option = $("<option>").attr({ value: reg.Id }).text(reg.Nombre);

                    $("#drop-contactos").append(option);

                });

        }
        $("#drop-contactos").dropdown();
        cargarDatosContacto();
    }).fail(function (error) {
        console.log(error.responseText);
    });

    function cargarDatosContacto() {
        var idContacto = $("#drop-contactos").val();
        if (idContacto != null || idContacto !== "")
            $.ajax({
                type: "Get",
                url: "/Muestreo/ObtenerDatosContacto",
                data: { idcliente: idContacto },
                dataType: "json",
                success: function (data) {
                    $("#txtemail").val(data['email']);
                    $("#txttelefono").val(data['telefono']);
                }
            });
    }
}

//Este Metodo Configura todas las tablas de la vista
function configuracionTablas() {

    //Objeto opcion de datatable
    var opcionDT = {
        scrollY: "200px",
        colReorder: true,
        scrollCollapse: true,
        retrieve: true,
        dom: "tipr"
    };

    //Configuracion de DataTable Cotizaciones
    tablaCot = $("#tabla-cotizaciones").DataTable(opcionDT).draw();
    //*************************************

    //Configuracion de DataTable Ordenes de Muestra
    tablaOM = $("#tabla-OM").DataTable(opcionDT).draw();
    //*************************************

    //Configuracion de DataTable Muestras Ingresadas
    tablaMI = $("#tabla-MI").DataTable(opcionDT).draw();
    //*************************************
    //Configuracion de DataTable Muestras Ingresadas
    tablaCM = $("#tabla-CM").DataTable(opcionDT).draw();
    //*************************************
}

//Este Metodo Configura las cabeceras de las tablas
function configuracionCabecera(nombreVista, tabla) {
    //Opciones de Configuracion
    var datePickerOptions = {
        format: "yyyy-mm-dd",
        weekStart: 1,
        language: "es",
        orientation: "bottom auto",
        todayHighlight: true,
        autoclose: true
    };

    //Configuracion de Fechas Desde y Hasta
    $("#Fecha_Desde_" + nombreVista).datepicker(datePickerOptions);
    $("#Fecha_Hasta_" + nombreVista).datepicker(datePickerOptions);
    //Evento onchage de la fecha desde
    $("#Fecha_Desde_" + nombreVista).on('changeDate', function (e) { 
         $('#Filtro_Fecha_' + nombreVista).submit();
    });
    //Evento onchage de la fecha hasta
    $("#Fecha_Hasta_" + nombreVista).on('changeDate', function (e) { 
         $('#Filtro_Fecha_' + nombreVista).submit();
    });

    $('#selector_pagina_' + nombreVista).on('click', '.datapager',
        function () {
            var pager = $(this).data('pager');
            //Verifico que el tamanio de la tabla sea diferente al que se esta mostrando
            var tam = tabla.page.len();
            console.log(tam);
            console.log(pager);
            if (tam == parseInt(pager)) {
                console.log("es igual");
            }
            else {
                console.log("es diferente");
                tabla.page.len(pager).draw();
                if (pager === -1) {
                    $('#paginador_tabla_' + nombreVista).text("Todas");
                } else {
                    $('#paginador_tabla_' + nombreVista).text(pager);
                }
            }
        });


    //Paginador para la tabla de Cotizaciones
    $('#paginador_tabla_' + nombreVista).text(tablaCot.page.len());
}

//Funcion que me permite cambiar de Tab
function cambiarTab(nombreVista) {
    //Mostrar Vista
    $("#" + nombreVista).show();
    $("#Contenido_" + nombreVista).show();
    $("#Contenido_" + nombreVista).addClass("active");
    $('#tab-' + nombreVista).addClass('active');
    $("#detalle_" + nombreVista).html("");
}

//Funcion que resetea los Tabs de la vista
function ocultarTabs() {
    //Quitar Vistas
    vistas.forEach(function (nombreVista) {
        $("#" + nombreVista).hide();
        $("#Contenido_" + nombreVista).hide();
        $("#Contenido_" + nombreVista).removeClass("active");
        $('#tab-' + nombreVista).removeClass('active');
    });
}

//Funcion que registra los eventos sobre las tablas
function registrarEventosDetalle() {
    vistas.forEach(function (nombreVista) {
        var selector = '#tabla-' + nombreVista + ' tr.detalle_' + nombreVista;
        $('#Contenido_' + nombreVista).on('click',selector,
            function (e) {
                console.log("Presionando tabla de la vista :" + nombreVista);
                var detalle = $('#detalle_' + nombreVista);
                $('.detalle_' + nombreVista).removeClass('active');
                $(this).addClass('active');
                detalle.html("");
                var urlLoad = $(this).data('enlace');
                detalle.load(urlLoad);
            });
    });
}

//Funcion que envia el correo de la Orden de Muestreo
function EnviarOM() {
    var OMS = obtenerSeleccionElementos();
    var opciones = {
        type: "POST",
        url: '/PrincipalMuestreo/EnviarOMS',
        data: { OMs: OMS },
        success: function (data) {
            //Recupero la fecha desde y hasta
            console.log("Si esta entrando y enviandose");
            var desde = $("#Contenido_OM #Fecha_Desde_OM").serialize().value;
            var hasta = $("#Contenido_OM #Fecha_Hasta_OM").serialize().value;
            console.log($("#Contenido_OM #Fecha_Desde_OM"))
            console.log($("#Contenido_OM #Fecha_Hasta_OM"))
            console.log(desde + " -- " + hasta);
            if (data.exito) {
                console.log("Pilas si esta entrando");
                alert('Correo(s) enviado(s) correctamente');
                //$("#Contenido_OM").load("/PrincipalMuestreo/ListadoOM?desde=" + desde + "hasta=" + hasta);
                location.reload();
            }
            else {
                alert("Error al enviar los correos electronicos");
            }
            }
    }
    $.ajax(opciones);
};

//Funcion que recupera la lista de seleccion de elementos en una tabla dependiendo de una clase
/*Parametros 
tabla ==> nombre de la tabla a buscar
tipoElemento ==> nombre de la clase del elemento de seleccion en la tabla*/
function obtenerSeleccionElementos() {
    var checks = $('input[type="checkbox"][checkbox-group="ordenes-muestreo"]:checked');
    console.log(checks);
    var listaCot = new Array();
    //Itero las lista de cheks de cotizaciones
    $.each(checks,
        function (key, check) {
            listaCot.push(parseFloat($(check).val()));
        });
    console.log(listaCot);
    return listaCot;
};

//Funcion para receptar muestras del laboratorio
function receptarMuestras(checkboxesMuestras, enlace) {

    var listaSeleccion = new Array();
    
    var idCotizacion = $(checkboxesMuestras[0]).attr('data-cotizacion');

    $.each(checkboxesMuestras, function (key, check) {
        listaSeleccion.push($(check).val());
    });
    console.log(listaSeleccion);
    $.post(enlace, { idsMuestras: listaSeleccion, idCotizacion: idCotizacion }, function (vista_parcial) {
        $('#modal-ingreso-muestras .content').html(vista_parcial);
        $('#modal-ingreso-muestras .content .date-picker').datepicker(globalDatePickerOptions);
        $('#modal-ingreso-muestras .content .hour-picker').flatpickr(globalClockPickerOptions);
        $('#modal-ingreso-muestras .content .ui.dropdown').dropdown();
        $('#modal-ingreso-muestras').modal("show");
    });
};


//Fucnion que devuleve las muestras chekeadas del 4to tab 
    function obtenerSelecciondeMuestras() {
        //Recupero todos los checks de cotizaciones
        var tabla = $('#tabla-CM');
        var checks = $(tabla).children().find('.mols');
        console.log(checks);
        var listaM = [];
        //Itero las lista de cheks de cotizaciones
        $.each(checks,
            function (key, check) {
                if ($(check).is(":checked")) {
                    //El elemento box esta seleccionado
                    var element = check.closest("tr").getAttributeNode("data-id").value;
                    console.log(element);
                    //Si estado masivo
                    listaM.push(parseFloat(element)); 
                }
            });
        return listaM;
    };
 
//Función de reenvio de Email
function ReenviarEmail() {
    var ids = obtenerSelecciondeMuestras();
    console.log(ids);
    var email = prompt("Ingrese el correo electrónico para el envío:", "@");
    if (email == null || email == "") {
        console.log("User cancelled prompt.");
    } else { 
        $.ajax({
            method: "POST",
            data: { ids: ids, email: email },
            url: 'PrincipalMuestreo/ReenvioEmail',
            success: function(data) {
                if (data.exito) {
                    console.log('exito');
                    window.location.href = "PrincipalMuestreo/Index";
                } else {
                    alert("Error al enviar los correos electronicos");
                }
            }
        });
    }
};

//Funcion que genera el filtrado y las busquedas en las tablas de las pantallas
function generarFiltradoBusqueda(nombreVista, table) {

        //Verifico las columnas de las tablas
        $($('#tabla-' + nombreVista + " thead tr th").get().reverse()).each(function (key, obj) {
            $("#filtro_global_tabla_" + nombreVista).after('<div class="item" data-column-search="' +
                $(obj).index() +
                '">' +
                $(obj).text() +
                '</span>');
        });

        //Agrego el evento sobre la busqueda seleccionada en ese momento
        $('#input_busqueda_tabla_' + nombreVista).on('keyup',
            function () {
                var searchFilterMode = $("#filtro_tabla_" + nombreVista).find('.active');
                console.log(table);

                if (searchFilterMode.data('column-search') === -1) {
                    //table.search(this.value).draw();
                } else {
                    table.column(searchFilterMode.data('column-search')).search(this.value).draw();
                }
            });
};


////Funcion que obtiene las etiquetas seleccionadas 
//function obtenerSelecciondeEtiquetas() {
//    //Recupero todos los checks de cotizaciones
//    var tabla = $("#consultaEtiquetas");
//    var checks = $(tabla).children().find('.etiq');
//    var listaEt = [];
//    //Itero las lista de cheks de cotizaciones
//    $.each(checks,
//        function (key, check) {
//            if ($(check).is(":checked")) {
//                //El elemento box esta seleccionado
//                var element = check.closest("tr").getAttributeNode("data-id").value; 
//                //Si estado masivo
//                listaEt.push(parseFloat(element));
//            }
//        });
//    console.log(listaEt);
//    return listaEt;
//};

////Función de reenvio de Email
//$('#generar_etiquetas').on('click', function (e)
//{
//    var ids = obtenerSelecciondeEtiquetas();
//    console.log(ids);
//    var url = $('#generar_etiquetas').data('modal-url');
//    console.log(url);
//    //$.ajax({
//    //    method: "POST",
//    //    data: { ids: ids, email: email },
//    //    url: 'PrincipalMuestreo/ReenvioEmail',
//    //    success: function (data) {
//    //        if (data.exito) {
//    //            console.log('exito');
//    //            window.location.href = "PrincipalMuestreo/Index";
//    //        } else {
//    //            alert("Error al enviar los correos electronicos");
//    //        }
//    //    }
//    //});

//});



       
      
    
 