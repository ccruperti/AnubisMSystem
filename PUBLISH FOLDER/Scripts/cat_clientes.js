 
    $('#t_Clientes').on('click','.detalle',function(e) {
            $('.detalle').removeClass('active');
            $(this).addClass('active');

            var loading = $('#p_Contactos');

            var urlLoad = $(this).data('enlace');
            loading.html('');
            loading.addClass('loading');
            loading.load(urlLoad,
                function () { 
                    loading.removeClass('loading');
                });
    });

//$('#t_Clientes').DataTable();
$(document).ready(function () {
    $('.ui.checkbox').checkbox();


    $(document).on("keyup", "#RUC", function (e) {
        var $that = $(this),
            maxlength = 13;
        if ($.isNumeric(maxlength)) {
            if ($that.val().length === maxlength) {
                e.preventDefault();
                // If keyCode is not delete key 
                if (e.keyCode !== 64) {
                    return;
                }
            }
            $that.val($that.val().substr(0, maxlength));
        }
    });
});


   

    $('#n_cliente').on('click',
            function(e) {
                modalCliente();
            });


        function modalCliente() {

            $('#m_Cliente').load('Cat_Clientes/ModalCliente',
                function() {
                    $(this).modal('show');
                });

        }

        function modalContacto(id) {

            $('#m_Cliente').load('Cat_Clientes/ModalContacto' + "?IdCliente=" + id,
                function() {
                    $(this).modal('show');
                });

        }


   
