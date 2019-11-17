var table5 = $("#tabla-cotsaprobadas").DataTable();

$('#cambiarEstadoAprobadas').on('click',
    function () {
      
        var Idestado = $('#EstadoMuestroDrop').val(); 
        console.log(Idestado);
                var listaCot = obtenerSeleccion();
                console.log(listaCot);
                if (listaCot.length === 0) {
                    alert("Debe Seleccionar una cotizacion primero");
                } else {
                    if (confirm("¿Está seguro que desea cambiar el estado de la cotización?")) {
                        //Preparo la solicitud de cambio de estado
                        var requrl = '/Muestreo/CambiarEstados';
                        var opciones = {
                            url: requrl,
                            type: "POST",
                            dataType: 'json',
                            data: { cotizaciones: listaCot, estado: Idestado },
                            success: function (response) {
                                if (response.data) {
                                    location.reload();
                                } else {
                                    console.log("error");
                                }
                            }
                        };
                        $.ajax(opciones);
                       // console.log($('#IdEstadoMuestreo').val());
                    }
                }
    });
    function obtenerSeleccion() {
        //Recupero todos los checks de cotizaciones
         
        var checks = $(table5.rows().nodes()).find('.cots');
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




////CAMBIO ESTADOS OM
var table2 = $("#myTable2").DataTable();

$('#cambiarEstadoOM').on('click',
    function () {
         var Idestado = $('#EstadosOM').val();
        var listaCot = obtenerSeleccionOM();
        console.log(listaCot);
        if (listaCot.length === 0) {
            alert("Debe Seleccionar una OM primero");
        } else {
            if (confirm("¿Está seguro que desea cambiar el estado de la OM?")) {
                //Preparo la solicitud de cambio de estado
                var requrl = '/Muestreo/CambiarEstadosOM';
                var opciones = {
                    url: requrl,
                    type: "POST",
                    dataType: 'json',
                    data: { OMS: listaCot, estado: Idestado },
                    success: function (response) {
                        if (response.data) {
                            location.reload();
                        } else {
                            console.log("error");
                        }
                    }
                };
                $.ajax(opciones);
                // console.log($('#IdEstadoMuestreo').val());
            }
        }
    });

function obtenerSeleccionOM() {
    //Recupero todos los checks de cotizaciones
    var tabla = $("#myTable2");
    var checks = $(tabla).children().find('.omchecks');
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

function GenForms(id) {
    console.log(id);
    $.ajax({
        type: "POST",
        url: '/Muestreo/GenerarForms',
        data: { id : id}
        })
        .fail(function (e) {
            console.log(e.responseText);
        });
}

//$('#receptarMuestra').on('click',
//    function() { 
//        var listaPM = obtenerSeleccionMuestras(); 
//        var opciones = {
//            type: "POST",
//            url: '/Muestreo/IngresarPM',
//            data: { Ids: listaPM },
//            success: function ( ) {
                
//                    location.reload();
                
//            }
//        }
//        $.ajax(opciones);
//    });

function obtenerSeleccionMuestras() {
    //Recupero todos los checks de cotizaciones
    var tabla = $("#tabla-acts");
    var checks = $(tabla).children().find('.muestras');
    console.log(checks);
    var listaPM = [];
    //Itero las lista de cheks de cotizaciones
    $.each(checks,
        function (key, check) {
            if ($(check).is(":checked")) {
                //El elemento box esta seleccionado
                var element = check.closest("tr").getAttributeNode("data-id").value;
                console.log(element);
                //Si estado masivo
                listaPM.push(parseFloat(element));
                console.log(listaPM);
            }
        });
    return listaPM;
};


//function EnviarOM() { 
//    var OMS = obtenerSeleccionOM();
//    var opciones = {
//        type: "POST",
//        url: '/Muestreo/EnviarOMS',
//        data: { OMs: OMS },
//        success: function () { 
//            location.reload(); 
//        }
//    }
//    $.ajax(opciones);
//};

 