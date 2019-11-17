var globalDatePickerOptions = {
    format: "yyyy-mm-dd",
    language: "es",
    orientation: "bottom auto",
    autoclose: true,
    weekStart: 1,
    todayBtn: true,
    todayHighlight: true,
    templates: {
        rightArrow: '<i class="chevron right icon"></i>',
        leftArrow: '<i class="chevron left icon"></i>'
    }
};

var globalDropdownOptions = {

};

var urlAddress = {
    address: null,
    setAddress: function (urlAddress) {
        this.address = urlAddress;
    },
    isChanged: function (urlAddress) {
        if (this.address !== urlAddress) {
            this.setAddress(urlAddress);
            return true;
        }
        return false;
    }
};

var globalSettings = {
    urls: {
        partialLoadUrl: urlAddress,
        fullLoadUrl: urlAddress
    }
};

//Datepicker de Programacion, fechas pasadas a hoy desactivadas
var dateProgra = new Date();
dateProgra.setDate(dateProgra.getDate());
var PrograDatePickerOptions = {
    startDate: dateProgra,
    format: "yyyy-mm-dd",
    language: "es",
    orientation: "bottom auto",
    autoclose: true,
    weekStart: 1,
    todayBtn: true,
    todayHighlight: true,
    templates: {
        rightArrow: '<i class="chevron right icon"></i>',
        leftArrow: '<i class="chevron left icon"></i>'
    }
};

//Datepicker de Ingreso Manual, fechas superiores a hoy desactivadas

var IngresoDatePickerOptions = {
    endDate: '+0d',
    format: "yyyy-mm-dd",
    language: "es",
    orientation: "bottom auto",
    autoclose: true,
    weekStart: 1,
    todayBtn: true,
    todayHighlight: true,
    templates: {
        rightArrow: '<i class="chevron right icon"></i>',
        leftArrow: '<i class="chevron left icon"></i>'
    }
};

var globalClockPickerOptions = {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i"
};

$(function () {
    $('.ui.menu .item[data-tab]').tab();

    // Carga de datepickers global


    $('.date-picker').datepicker(globalDatePickerOptions);
    $('.date-picker').attr('autocomplete', 'off');
    /*
    $('.date-picker-programacion').datepicker(PrograDatePickerOptions);
    $('.date-picker-programacion').attr('autocomplete', 'off');

    $('.date-picker-ingreso').datepicker(IngresoDatePickerOptions);
    $('.date-picker-ingreso').attr('autocomplete', 'off');

    // Carga de hour pickers global
    $('.hour-picker').flatpickr(globalClockPickerOptions);*/

    // Carga de contenidos de un modal dinamico
    $('body').on('click', 'a[data-modal], button[data-modal]', function () {
        var buttonTrigger = $(this);
        buttonTrigger.addClass('loading');
        buttonTrigger.prop('disabled');

        var modal = $('#' + $(this).attr('data-modal'));
        var modalContent = modal.find('div.content');
        var modalActions = modal.find('.actions button, .actions a');

        // Cambiar titulo modal
        if (buttonTrigger.hasAttribute('data-modal-title')) {
            var modalHeader = modal.find('div.header');
            var modalTitle = buttonTrigger.attr('data-modal-title');
            if (modalHeader.has('i.icon').length) {
                var icon = modalHeader.find('i.icon');
                modalHeader.text(modalTitle);
                modalHeader.prepend(icon);
            } else {
                modalHeader.text(modalTitle);
            }
        }

        var modalOptions = {
            centered: false,
            transition: 'fade down',
            observeChanges: true
        };
        var ajaxOptions = {
            url: $(this).attr('data-modal-url'),
            method: 'get',
            success: function (responseView) {

                modalContent.html(responseView);
                if (modalContent.has('.accordion').length) {
                    $('.ui.accordion').accordion({
                        selector: {
                            trigger: '.title .icon'
                        }
                    });
                }
                if (modalContent.has('.ui.search.dropdown').length) {
                    modalContent.find('.ui.search.dropdown').dropdown();
                }

                if (modalContent.has('.date-picker').length) {
                    modalContent.find('.date-picker').datepicker(globalDatePickerOptions);;
                }
                modal.trigger('form-loaded');
            }
        };
        modalActions.prop('disabled');
        modalActions.addClass('disabled loading');
        modalContent.addClass('loading');
        modal.modal(modalOptions).modal('show');
        $.ajax(ajaxOptions)
            .fail(function (error) {
                alert('Ocurrio un error obteniendo la vista');
            })
            .always(function () {
                modalContent.removeClass('loading');
                modalActions.removeProp('disabled');
                modalActions.removeClass('disabled loading');
                buttonTrigger.removeClass('loading');
                buttonTrigger.removeProp('disabled');

            });
    });

    // Botones que realizan acciones de submit a un formulario
    $('body').on('click', 'a[data-submit-form], button[data-submit-form]', function () {
        var form = $('#' + $(this).attr('data-submit-form'));
        // Si le agrega parametros
        if ($(this).attr('data-submit-form-parameter')) {
            var newFormAction = form.attr('action') + $(this).attr('data-submit-form-parameter');
            form.attr('action', newFormAction);
        }
        form.submit();
    });

    // Envio de formularios asincronos dentro de modales
    $('body').on('submit', '.modal form.modal-form', function (event) {
        event.preventDefault();
        var modal = $(this).closest('div.modal');
        var modalContent = modal.find('div.content');
        var modalActions = modal.find('.actions button, .actions a');
        var ajaxOptions = {
            url: $(this).attr('action'),
            method: $(this).attr('method'),
            data: $(this).serialize()
        };
        modalContent.addClass('loading');
        modalActions.prop('disabled');
        modalActions.addClass('disabled loading');
        $.ajax(ajaxOptions)
            .done(function (responseData, statusCode, xhr) {
                modalContent.html(responseData);
                modal.trigger('form-completed');
                if (modalContent.has('#redirect-url').length) {
                    setTimeout(function () {
                        window.location.href = modalContent.find('#redirect-url').val();
                    }, 1600);
                } else if (modalContent.has('form').length) {
                    console.log('Mantener abierto');
                } else {
                    setTimeout(function () {
                        modal.modal('hide');
                    }, 1600);
                }
            })
            .fail(function (response, statusCode, xhr) {
                modalContent.html(response.responseText);
                if (modalContent.has('.accordion')) {
                    $('.ui.accordion').accordion();
                }
                if (statusCode === 400) {
                    modal.trigger('form-invalid');
                }
                modalActions.removeProp('disabled');
                modalActions.removeClass('disabled');
                modal.trigger('form-failed');
            })
            .always(function () {
                if (modalContent.has('.accordion').length) {
                    $('.ui.accordion').accordion({
                        selector: {
                            trigger: '.title .icon'
                        }
                    });
                }
                if (modalContent.has('.date-picker').length) {
                    modalContent.find('.date-picker').datepicker(globalDatePickerOptions);
                }
                if (modalContent.has('.ui.search.dropdown').length) {
                    modalContent.find('.ui.search.dropdown').dropdown();
                }
                if (modalContent.has('.hour-picker').length) {
                    modalContent.find('.hour-picker').flatpickr(globalClockPickerOptions);
                }
                modalActions.removeClass('disabled loading');
                modalContent.removeClass('disabled loading');
            });
        return false;
    });

    // Seleccion de checkboxes en titulo acordeon
    $('body').on('change', '.ui.accordion .title input[type="checkbox"]', function () {
        var checksContenido = $(this).closest('.title')
            .next('.content')
            .find('input[type="checkbox"]')
            .prop('checked', $(this).is(':checked'));

    });

    // Envio de formularios asincronos
    $('body').on('submit', 'form.async-form', function (event) {
        event.preventDefault();
        var asyncForm = $(this);
        var actions = asyncForm.find('a[data-form-submit]');
        var ajaxOptions = {
            url: asyncForm.attr('action'),
            method: asyncForm.attr('method'),
            data: asyncForm.serialize()
        };
        actions.prop('disabled');
        actions.addClass('disabled loading');
        $.ajax(ajaxOptions)
            .done(function (responseData, statusCode, xhr) {
                asyncForm.html(responseData);
            })
            .fail(function (response, statusCode, xhr) {
                asyncForm.html(response.responseText);
                actions.removeProp('disabled');
                actions.removeClass('disabled');
            })
            .always(function () {
                if (asyncForm.has('.ui.accordion').length) {
                    $('.ui.accordion').accordion({
                        selector: {
                            trigger: '.title .icon'
                        }
                    });
                }
                if (asyncForm.has('.date-picker').length) {
                    asyncForm.find('.date-picker').datepicker(globalDatePickerOptions);
                }
                if (asyncForm.has('.ui.dropdown').length) {
                    asyncForm.find('.ui.dropdown').dropdown();
                }
                if (asyncForm.has('.hour-picker').length) {
                    asyncForm.find('.hour-picker').flatpickr(globalClockPickerOptions);
                }
                actions.removeClass('loading');
            });
        return false;
    });

    // Seleccion de checkboxes en titulo acordeon
    $('body').on('change', '.ui.accordion .title input[type="checkbox"]', function () {
        var checksContenido = $(this).closest('.title')
            .next('.content')
            .find('input[type="checkbox"]')
            .prop('checked', $(this).is(':checked'));

    });

    // Seleccion de todos los checkboxes de un grupo
    $('body').on('change', 'input[type="checkbox"][checkbox-group-checkall]', function () {
        var checkboxGroup = $('input[type="checkbox"][checkbox-group="' + $(this).attr('checkbox-group-checkall') + '"]');
        if (checkboxGroup.length) {
            checkboxGroup.prop('checked', $(this).is(':checked'));
        }
    });

    $('body').on('change', '.ui.accordion .content input[type="checkbox"]', function () {
        var checksContenido = $(this).closest('.content')
            .find('input[type="checkbox"]');
        var checksMarcados = $(this).closest('.content')
            .find('input[type="checkbox"]:checked');

        var checkTitulo = $(this).closest('.content')
            .prev('.title')
            .find('input[type="checkbox"]');
        if (checksContenido.length === checksMarcados.length) {
            checkTitulo.prop('checked', true);
        } else {
            checkTitulo.prop('checked', false);
        }
    });

    // Agregado de listas dinamicas de inputs
    $('body').on('click', 'a[data-list-add], button[data-list-add]', function () {
        addDynamicInput($(this));
    });

    // Agregado de listas dinamicas de inputs
    $('body').on('change', 'select[data-list-add]', function () {
        if ($(this).val() !== 0 && $(this).val() !== '' && $(this).val() !== undefined) {
            $(this).addClass('disabled loading');
            $(this).prop('disabled');
            var urlParameters = '&id=' + $(this).val();
            if ($(this).attr('data-list-add-quantity')) {
                var quantity = $('input[data-list-add-quantity=' + $(this).attr('data-list-add-quantity') + ']').val();
                urlParameters += '&cantidad=' + quantity;
            }
            addDynamicInput($(this), null, null, urlParameters);
        }
    });

    // Eliminado de listas dinamicas de inputs
    $('body').on('click', 'a[data-list-item-delete], button[data-list-item-delete]', function () {
        deleteDynamicInput($(this));
    });

    // Recarga de opciones de un drop down
    $('body').on('sourceUpdate', 'select[data-remote-source]', function () {
        $(this).addClass('loading');
        alert('Recargar dropdown');
        var ajaxOptions = {
            url: $(this).attr('data-remote-source'),
            method: 'POST'
        };
        $.ajax(ajaxOptions)
            .done(function (response, statusCode, xhr) {

                console.log(response);
            })
            .fail();
    });

    // Cargado de inputs por dropdown
    $('body').on('reloadData', 'select[data-input-loader-url]', function () {
        var select = $(this);

        var selectedValue = select.val();

        var defaultOption = select.find('option value=""');

        select.addClass('loading');
        select.dropdown();

        var ajaxOptions = {
            url: select.attr('data-input-loader-url')
        };

        $.ajax(ajaxOptions)
            .done(function (response, statusCode, xhr) {
                select.children().remove();

                if (defaultOption.length) {
                    select.append(defaultOption);
                }

                $.each(response, function (key, value) {
                    var item = $('option');
                    select.append();
                });
            })
            .fail(function (response, statusCode, xhr) {
                alert('Error en actualizacion de dropdowns');
            })
            .always(function () {
                select.removeClass('loading');
                select.dropdown();
            });
    });

    // Renderizar tablas con clase datatable
    $('table.datatable').each(function (index) {
        RenderDataTable($(this));
    });

    $('body').on('click', '[data-trigger-detailview]', function () {
        RenderDetailView($(this));
    });

    $('body').on('click', '[detail-view-container][detail-view-url]', function () {
        PartialViewRequestTrigger($(this));
    });

    $('body').on('keyup', 'input.upper-text', function () {
        var value = $(this).val();
        if (value !== undefined && value !== null && value !== '' && value.length > 0) {
            $(this).val(value.toUpperCase());
        }
    });
});

// Llamada a elementos de agregado
function addDynamicInput(addTrigger, addTriggerData = null, addTriggerUrl = null, addTriggerUrlParameters = null) {
    var list = $('[data-list="' + addTrigger.attr('data-list-add') + '"]');
    if (!list.length) {
        alert('No se encuentra el elemento de lista o no esta especificado el atributo "data-list-add" en el boton de agregar');
        return;
    }

    if (!addTrigger.attr('data-list-add-url') && addTriggerUrl === null) {
        alert('No se encuentra el atributo "data-list-add-url" o no se indica cual es la URL de agregado');
        return;
    }

    var items = list.find('[data-list-item]');


    var defaultUrlParameters = '?i=' + items.length;
    if (addTrigger.hasAttribute('data-list-add-urlparameter')) {
        defaultUrlParameters += addTrigger.attr('data-list-add-urlparameter');
    }
    var url = addTriggerUrl !== null ? addTriggerUrl : addTrigger.attr('data-list-add-url') + defaultUrlParameters;

    console.log(url);

    var ajaxOptions = {
        url: url,
        data: addTriggerData,
        method: 'GET'
    };

    $.ajax(ajaxOptions)
        .done(function (responseData, statusCode, xhr) {
            list.append(responseData);
        })
        .fail(function (response, statusCode, xhr) {
            list.append(response.responseText);
            alert('Fallo agregado de item');
        })
        .always(function () {
            addTrigger.removeClass('disabled loading');
            addTrigger.removeProp('disabled');
        });
}

// Eliminacion de elementos dinamicos de una lista
function deleteDynamicInput(deleteTrigger) {
    var list = deleteTrigger.closest('[data-list]');

    var listItem = $('[data-list-item="' + deleteTrigger.attr('data-list-item-delete') + '"]').remove();

    var listItems = list.children('[data-list-item]');


    listItems.each(function (itemPosition, itemElement) {
        var listItemInputs = $(itemElement).find('input');
        listItemInputs.each(function (key, inputElement) {
            var originalName = $(inputElement).attr('name');
            $(inputElement).attr('name', originalName.replaceBetweenLast('[', ']', itemPosition));
        });
        var deleteItem = $(itemElement).find('[data-list-item-delete]');
        deleteItem.attr('data-list-item-delete', list.attr('data-list') + '-' + itemPosition);
        $(itemElement).attr('data-list-item', list.attr('data-list') + '-' + itemPosition);
    });
}

/// Recargador de plugins en contenido
function PartialViewRendered(partialViewContainer) {
    if (partialViewContainer.has('.ui.dropdown').length) {
        partialViewContainer.find('.ui.dropdown').dropdown();
    }
    if (partialViewContainer.hasClass('segment')) {
        partialViewContainer.removeClass('loading');
    }
}

/// Solicita la vista parcial en el contenedor indicado
function PartialViewRequest(partialViewContainer, partialViewUrl) {
    var ajaxOptions = {
        url: partialViewUrl
    };
    if (partialViewContainer.hasClass('segment')) {
        partialViewContainer.addClass('loading');
    }
    $.ajax(ajaxOptions)
        .done(function (response) {
            partialViewContainer.html(response);
        })
        .fail(function () {
            alert('ERROR: Ocurrio un error solicitando la URL: "' + ajaxOptions.url + '"');
        })
        .always(function () {
            PartialViewRendered(partialViewContainer);
        });
}

/// Analiza un elemento 
function PartialViewRequestTrigger(triggerElement) {
    var partialViewContainerAttr = triggerElement.attr('detail-view-container');
    var partialViewUrlAttr = triggerElement.attr('detail-view-url');
    if (!partialViewContainerAttr.length || partialViewContainerAttr === undefined || partialViewContainerAttr === '' ||
        !partialViewUrlAttr.length || partialViewUrlAttr === undefined || partialViewUrlAttr === '') {
        alert('ERROR: Los atributos [detail-view-container] o [detail-view-url] no estan correctos. Verificar si estan llenos.');
    }
    var partialViewsContainers = partialViewContainerAttr.split(' ');
    var partialViewsUrls = partialViewUrlAttr.split(' ');
    if (partialViewsContainers.length !== partialViewsUrls.length) {
        alert('ERROR: No se han declarado la misma cantidad de elementos en los atributos [detail-view-container] y [detail-view-url]. Verificar espacios.');
    }

    for (var i = 0; i < partialViewsUrls.length; i++) {
        var partialViewContainer = $('#' + partialViewsContainers[i]);
        if (!partialViewContainer.length) {
            alert('ERROR: El elemento ' + partialViewsContainers[i] + ' no se ha encontrado. Verificar si el nombre esta correcto.');
        } else {
            PartialViewRequest(partialViewContainer, partialViewsUrls[i]);
        }
    }
}

/// Renderizador de parciales
function RenderDetailView($viewTrigger) {
    if (globalSettings.urls.partialLoadUrl.isChanged($viewTrigger.attr('data-trigger-detailview'))) {
        if ($viewTrigger.attr('data-trigger-container')) {
            var viewContainer = $('#' + $viewTrigger.attr('data-trigger-container'));

            if (viewContainer.hasClass('segment')) {
                viewContainer.addClass('loading');
            }

            var ajaxOptions = {
                url: $viewTrigger.attr('data-trigger-detailview')
            };

            $.ajax(ajaxOptions)
                .done(function (responseData, statusCode, xhr) {
                    viewContainer.html(responseData);
                    if (viewContainer.has('.ui.dropdown').length) {
                        viewContainer.find('.ui.dropdown').dropdown();
                    }
                })
                .always(function () {
                    if (viewContainer.hasClass('segment')) {
                        viewContainer.removeClass('loading');
                    }
                });
        } else {
            alert('No se ha encontrado el contenedor donde cargar la vista parcial. Verificar si existe la propiedad "data-trigger-container" o el contenedor al que hace referencia.');
        }
    }
}

/// Renderizador personalizado para DataTables
function RenderDataTable($table) {

    var defaultOptions = {
        ordering: false,
        paging: true,
        searching: true,
        dom: 'tip',
        pageLength: 20,
        
    };

    var $dataTable = $table.DataTable(defaultOptions);

    var defaultPagerOptions = {
        icon: 'th list icon',
        dropdownIcon: 'dropdown icon',
        texto: 'Paginar ({page-count}) filas',
        buttons: ['copy', 'excel', 'pdf'],
        pageOptions: [
            { value: 20 },
            { value: 40 },
            { value: 60 },
            { value: 80 },
            { value: 100 },
            { value: -1, text: 'Todas las filas' }
        ]
    };

    var pagerCounter = '<strong class="datatables-pagecounter">' + $dataTable.page.len() + '</strong>';

    /// Agregara dropdown del paginador
    if ($table.attr('datatable-pager')) {
        var pagerDropdown = $('#' + $table.attr('datatable-pager'));
        pagerDropdown.html('<i class="' + defaultPagerOptions.icon + '"></i> ' + defaultPagerOptions.texto.replace('{page-count}', pagerCounter) + ' <i class="' + defaultPagerOptions.dropdownIcon + '"></i>');
        var pagerDropdownMenu = $('<div class="menu"></div>');
        defaultPagerOptions.pageOptions.forEach(function (pageOption) {
            if (pageOption.text === undefined) {
                pageOption.text = pageOption.value;
            }
            var pagerDropdownItem = $('<div class="item" datatable-pagecount="' + pageOption.value + '">' + pageOption.text + '</div>');

            pagerDropdownItem.on('click', function () {
                if ($dataTable.page.len() !== pagerDropdownItem.attr('datatable-pagecount')) {
                    var pageCount = pagerDropdownItem.attr('datatable-pagecount');
                    if (pageCount === '-1') {
                        pageCount = 'Todas';
                    }
                    pagerDropdown.find('.datatables-pagecounter').text(pageCount);
                    $dataTable.page.len(pageCount).draw();
                }
            });

            pagerDropdownMenu.append(pagerDropdownItem);
        });
        pagerDropdown.append(pagerDropdownMenu);
    }

    var defaultSearchOptions = {
        searchPlaceholder: 'Buscar',
        searchIcon: 'search icon',
        searchClass: '',
        searchColumnText: 'Todas las Columnas'
    };

    /// Agregar campo de busqueda que filtra por columna
    if ($table.attr('datatable-search')) {
        var searchContainer = $('#' + $table.attr('datatable-search'));

        var searchIcon = $('<i class="' + defaultSearchOptions.searchIcon + '"></i>');

        searchContainer.append(searchIcon);

        var searchInput = $('<input class="' + defaultSearchOptions.searchClass + '" placeholder="' + defaultSearchOptions.searchPlaceholder + '" type="text" />');

        searchContainer.append(searchInput);

        var searchSelection = $('<div class="ui basic floating dropdown button"></div>');

        searchSelection.append('<div class="text">' + defaultSearchOptions.searchColumnText + '</div>');
        searchSelection.append('<i class="dropdown icon"></i>');

        var searchColumnMenu = $('<div class="menu"></div>');

        var defaultSearchText = $('<div class="item">' + defaultSearchOptions.searchColumnText + '</div>');

        defaultSearchText.on('click', function () {
            $dataTable.searchSelectorOption = null;
            performDataTablesSearch($dataTable, searchInput);
        });

        searchColumnMenu.append(defaultSearchText);

        $dataTable.columns().every(function () {
            var columnHeader = this.header();
            var column = this;
            var excludeSearch = columnHeader.attributes.getNamedItem('datatable-search-exclude') !== null ? true : false;
            if (columnHeader.textContent !== undefined && columnHeader.textContent.trim() !== '' && !excludeSearch) {
                var searchOption = $('<div class="item">' + columnHeader.textContent + '</div>');
                searchOption.on('click', function () {
                    $dataTable.searchSelectorOption = column;
                    performDataTablesSearch($dataTable, searchInput);
                });
                searchColumnMenu.append(searchOption);
            }
        });

        searchSelection.append(searchColumnMenu);

        searchContainer.append(searchSelection);

        //searchSelection.dropdown();

        $dataTable.searchSelector = searchSelection;

        searchInput.on('keyup change', function () {
            performDataTablesSearch($dataTable, searchInput);
        });
    }

    if ($table.attr('datatable-info')) {
        var infoContainer = $('#' + $table.attr('datatable-info'));
        var dataTableInfo = $dataTable.page.info();
        infoContainer.append('<i class="info circle icon"></i> Mostrando ' + dataTableInfo.recordsDisplay + ' de un total de ' + dataTableInfo.recordsTotal + ' registros');
    }

    if ($table.attr('datatable-paging')) {
        var pagingContainer = $('#' + $table.attr('datatable-paging'));

        renderDataTablesPaging(pagingContainer, $dataTable);
    }
}

function performDataTablesSearch($dataTable, searchInput) {
    if ($dataTable.searchSelectorOption !== null && $dataTable.searchSelectorOption !== undefined) {
        $dataTable.column($dataTable.searchSelectorOption.selector.cols).search(searchInput.val()).draw();
    }

    $dataTable.search(searchInput.val()).draw();
}

function renderDataTablesPaging($pagingContainer, $dataTable) {
    var dataTableInfo = $dataTable.page.info();

    var previousButton = $('<button class="ui blue labeled icon button" type="button"><i class="chevron left icon"></i>Anterior</button>');
    $pagingContainer.append(previousButton);
    previousButton.on('click', function () {
        $dataTable.page('previous').draw('page');
    });

    var nextButton = $('<button class="ui blue right labeled icon button" type="button">Siguiente<i class="chevron right icon"></i></button>');
    $pagingContainer.append(nextButton);
    nextButton.on('click', function () {
        $dataTable.page('next').draw('page');
    });

    if (dataTableInfo.pages > 5) {
        var currentPage = dataTableInfo.page + 1;
    }
}

function renderDataTablePageButton(pageNumber) {

}

// Extensiones globales
String.prototype.replaceBetweenLast = function (openingChar, closingChar, replaceValue) {
    var openingCharPosition = this.lastIndexOf(openingChar);
    var closingCharPosition = this.lastIndexOf(closingChar);
    if (openingCharPosition !== -1 && closingCharPosition !== -1) {
        return this.substring(0, openingCharPosition + 1) + replaceValue + this.substring(closingCharPosition, this.length);
    }
    return this;
};

String.prototype.replaceBetweenFirst = function (openingChar, closingChar, replaceValue) {
    var openingCharPosition = this.indexOf(openingChar);
    var closingCharPosition = this.indexOf(closingChar);
    if (openingCharPosition !== -1 && closingCharPosition !== -1) {
        return this.substring(0, openingCharPosition + 1) + replaceValue + this.substring(closingCharPosition, this.length);
    }
    return this;
};

Date.prototype.formatYYYYMMDDD = function () {
    var mm = this.getMonth() + 1;
    var dd = this.getDate();
    return [
        this.getFullYear(),
        (mm > 9 ? '' : '0') + mm,
        (dd > 9 ? '' : '0') + dd
    ].join('-');
};

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
};

// Extensiones para jQuery
$.fn.extend({
    // Devuelve bool (true/false) si un elemento tiene un determinado atributo. No valida vacios.
    hasAttribute: function (attributeName) {
        return this.attr(attributeName) !== undefined && this.attr(attributeName) !== false;
    },
    // Devuelve bool (true/false) si un elemento tiene un determinado atributo. Valida vacios
    hasRequiredAttribute: function (attributeName) {
        if (this.hasAttribute(attributeName)) {
            return this.attr(attributeName).trim() !== '';
        }
        return false;
    }
});

//Filtro de Fechas del Tab de Calendario
$("#Fecha_Desde_Calendario").datepicker(globalDatePickerOptions);
$("#Fecha_Hasta_Calendario").datepicker(globalDatePickerOptions);
//Evento onchage de la fecha desde
$("#Fecha_Desde_Calendario").on('changeDate', function (e) {
    $('#Filtro_Fecha_Calendario').submit();
});
//Evento onchage de la fecha hasta
$("#Fecha_Hasta_Calendario").on('changeDate', function (e) {
    $('#Filtro_Fecha_Calendario').submit();
});