const uri = 'api/Adecuaciones';
const urisolic = 'api/Adecuaciones/Filtros/Solicitudes';
const uritipo = 'api/Adecuaciones/Filtros/Tipos';
const urimes = 'api/Adecuaciones/Filtros/Meses';
const urianio = 'api/Adecuaciones/Filtros/Anios';
const fecha = new Date();
/**
 * Inicializa y configura el componente DataTable para el listado principal de Adecuaciones.
 * @summary Carga la tabla con paginación desde el servidor (Server-side processing).
 * @description
 * Si la tabla ya existe (isDataTable), simplemente recarga sus datos vía AJAX.
 * Si no existe, la instancia configurando:
 * - La URL de los datos: `api/Adecuaciones/Paginado`.
 * - Envía los parámetros de los filtros desplegables de la UI (Solicitud, Tipo, Mes, Año) inyectándolos en el objeto request `d`.
 * - Configura el renderizado de columnas (Fechas formateadas, Totales monetarios con separador de miles y signo de dólar).
 * - Renderiza botones de acción por fila (Ej. Botón "Ver Oficio" enlazado a DetalleOficio.aspx).
 */
function adec() {
    if ($.fn.DataTable.isDataTable('#tablaclavepresupuestaladecuacion')) {
        $('#tablaclavepresupuestaladecuacion').DataTable().ajax.reload();
        return;
    }

    $('#tablaclavepresupuestaladecuacion').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: 'api/Adecuaciones/Paginado',
            type: 'GET',
            data: function (d) {
                var solic = $("#numsolicitud_filtro").val();
                if (solic === "(Seleccione un Número de Solicitud)") solic = "0";
                
                var tipo = $("#tipoadecuacion").val();
                if (tipo === "(Seleccione un Tipo de Adecuación)") tipo = "0";

                var mes = $("#mes").val();
                if (mes === "(Seleccione un Mes)") mes = "0";
                
                var anio = $("#anio").val();
                if (anio === "(Seleccione un Año)") anio = "0";

                d.solic = solic;
                d.tipo = tipo;
                d.mes = mes;
                d.anio = anio;
            }
        },
        deferRender: true,
        dom: '<"d-flex justify-content-end mb-3"f>' +
             '<"row"<"col-sm-12"tr>>' +
             '<"row align-items-center mt-3"<"col-sm-12 col-md-4"l><"col-sm-12 col-md-4 text-center"i><"col-sm-12 col-md-4 d-flex justify-content-end"p>>',
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/es-MX.json',
            search: "",
            searchPlaceholder: "Buscar...",
            paginate: {
                previous: '<i class="fas fa-chevron-left"></i>',
                next: '<i class="fas fa-chevron-right"></i>'
            }
        },
        columns: [
            { 
                data: null,
                className: 'ps-3',
                render: function(data, type, row) {
                    var oficionumero = row.oficionumero || '';
                    var solic = `<span class="text-sm font-weight-bold">${oficionumero}</span>`;
                    return `<div class="d-flex px-2 py-1 align-items-center">${solic}</div>`;
                }
            },
            { 
                data: 'tipoadecuacion',
                className: 'align-middle',
                render: function(data) {
                    return `<span class="text-sm font-weight-bold">${data || ''}</span>`;
                }
            },
            { 
                data: 'fechasolicitud',
                className: 'align-middle text-center',
                render: function(data) {
                    var fecha = data;
                    if (!fecha || fecha == "0001-01-01T00:00:00") {
                        fecha = "";
                    } else {
                        var fech = fecha.split('T')[0].split('-');
                        fecha = fech[2] + "-" + fech[1] + "-" + fech[0];
                    }
                    return `<span class="text-secondary text-sm">${fecha}</span>`;
                }
            },
            { 
                data: 'montoadecuacion',
                className: 'align-middle text-center',
                render: function(data) {
                    var montototal = data;
                    if (montototal == 0 || montototal == null) {
                        montototal = "";
                    } else {
                        montototal = "$" + parseFloat(montototal).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    }
                    return `<span class="text-secondary text-sm font-weight-bold">${montototal}</span>`;
                }
            },
            { 
                data: null,
                className: 'align-middle text-center',
                render: function() { return '<span class="text-secondary text-sm"></span>'; }
            },
            { 
                data: null,
                className: 'align-middle text-center',
                render: function() { return '<span class="text-secondary text-sm"></span>'; }
            },
            { 
                data: null,
                className: 'align-middle text-center',
                render: function(data, type, row) {
                    var oficionumero = row.oficionumero || '';
                    var encodedOficio = encodeURIComponent(oficionumero);
                    return `<a href="DetalleOficio.aspx?oficio=${encodedOficio}" class="btn btn-primary btn-sm mb-0">Ver Oficio</a>`;
                }
            }
        ],
        drawCallback: function() {
            $('[data-toggle="tooltip"]').tooltip({
                placement: 'top'
            });
        }
    });
}
function uploadExcelAdecuaciones() {
    var input = document.getElementById('fileAdecExcel');
    if (input.files.length === 0) {
        informar("Por favor seleccione un archivo Excel.");
        return;
    }
    var file = input.files[0];
    var formData = new FormData();
    formData.append("excelFile", file);

    // Mostrar loader/sweetalert
    Swal.fire({
        title: 'Cargando...',
        text: 'Procesando el archivo Excel, por favor espere.',
        allowOutsideClick: false,
        onBeforeOpen: () => {
            Swal.showLoading()
        }
    });

    fetch('/api/Adecuaciones/UploadExcel', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            Swal.fire('¡Éxito!', data.message, 'success').then(() => {
                $('#modal-upload-adec').modal('hide');
                adec(); // recargar la tabla
            });
        } else {
            Swal.fire('Error', data.message, 'error');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire('Error', 'Ocurrió un problema al subir el archivo.', 'error');
    });
}

    // Evento manual para abrir/cerrar el dropdown (por si falla Bootstrap) eliminado
    // porque generaba conflicto con el data-bs-toggle="dropdown" nativo de Bootstrap
    // y obligaba a dar doble clic.

    // Cerrar al dar clic afuera
    $(document).on('click', function(e) {
        if (!$(e.target).closest('.dropdown').length) {
            $('.custom-select-menu').removeClass('show');
        }
    });

    // Evento para Custom Selects
    $(document).on('click', '.custom-select-item', function(e) {
        e.preventDefault();
        let $menu = $(this).closest('.custom-select-menu');
        let $container = $menu.closest('.dropdown').parent();
        let $btnText = $container.find('button span');
        let $hiddenInput = $menu.closest('.dropdown').next('input[type="hidden"]');
        if($hiddenInput.length === 0) {
            $hiddenInput = $container.find('input[type="hidden"]');
        }

        // Quitar active a todos los items de este menu
        $menu.find('.custom-select-item').removeClass('active');
        // Poner active al seleccionado
        $(this).addClass('active');

        // Actualizar texto del botón y valor del input oculto
        $btnText.text($(this).text());
        $hiddenInput.val($(this).data('value'));

        // Cerrar el menú
        $menu.removeClass('show');

        // Disparar evento change en el input oculto
        $hiddenInput.trigger('change');
    });

    $(document).on('change', '.rev-tipo', function() {
        let $card = $(this).closest('.modern-card-item');
        let val = $(this).val();
        
        $card.removeClass('type-amp type-red');
        
        if (val == "1" || val == "3") {
            $card.addClass('type-amp');
        } else if (val == "2" || val == "4") {
            $card.addClass('type-red');
        }
        
        if (typeof recalcularTotalesRevision === 'function') {
            recalcularTotalesRevision();
        }
    });

    $(document).on('change', '#t_o', function() {
        try {
            let val = $(this).val() || "";
            val = String(val);
            
            // Reglas de visualización
            if (val === "4" || val === "") {
                $("#divMesesGrid").hide();
            } else {
                $("#divMesesGrid").show();
            }

            // Reglas de habilitación
            if (val === "3") {
                activarmeses();
                let ml = document.getElementById('montoliberar');
                if (ml) ml.disabled = false;
            } else {
                // Habilitar todos los meses para Reducción, Traspaso, etc.
                let meses = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];
                meses.forEach(m => {
                    let el = document.getElementById('txt' + m);
                    if (el) el.disabled = false;
                });
                let ml = document.getElementById('montoliberar');
                if (ml) ml.disabled = false;
            }
        } catch(e) {
            console.error("Error en change de t_o:", e);
        }
    });



function verMovimientos(id, oficio) {
    // Preparar el modal
    $("#txtnumsolicitud").prop("disabled", true);
    $("#txtasunto").prop("disabled", true);
    $("#txtnumsolicitud").val(id);
    $("#txtnumsolicitud").prop("disabled", true);
    $("#cbotipoadecuacion").prop("disabled", true);
    $("#txtfechasolicitud").prop("disabled", true);
    $("#txtconcepto").prop("disabled", true);
    $("#txtmontoadecuacion").prop("disabled", true);
    
    // Ocultar sección de agregar
    $("#general").hide();
    
    // Cambiar cabeceras de la tabla para visualización
    $("#tablaclavepres thead").html(`
        <tr>
            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Clave Presupuestal</th>
            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Tipo Operación</th>
            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-end">Monto Total</th>
            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center">Desglose Mensual</th>
        </tr>
    `);
    
    $("#contenidoadecuacion").html('<tr><td colspan="4" class="text-center">Cargando movimientos...</td></tr>');
    $('#modal-form').modal('show');
    
    // Fetch data
    fetch('/api/Adecuaciones/Movimientos/' + id)
        .then(res => res.json())
        .then(data => {
            let html = '';
            let suma = 0;
            data.forEach(m => {
                suma += m.Total;
                let formattedTotal = "$" + parseFloat(m.Total).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                
                // Generar tabla de meses oculta para el SweetAlert
                let htmlMeses = `
                    <table class='table table-sm table-bordered mt-2' style='font-size:12px;'>
                        <tr style='background-color:#eee;'><th>Ene</th><th>Feb</th><th>Mar</th><th>Abr</th><th>May</th><th>Jun</th></tr>
                        <tr><td>${m.Enero}</td><td>${m.Febrero}</td><td>${m.Marzo}</td><td>${m.Abril}</td><td>${m.Mayo}</td><td>${m.Junio}</td></tr>
                        <tr style='background-color:#eee;'><th>Jul</th><th>Ago</th><th>Sep</th><th>Oct</th><th>Nov</th><th>Dic</th></tr>
                        <tr><td>${m.Julio}</td><td>${m.Agosto}</td><td>${m.Septiembre}</td><td>${m.Octubre}</td><td>${m.Noviembre}</td><td>${m.Diciembre}</td></tr>
                    </table>
                `;
                let btnMeses = `<button class="btn btn-sm btn-info m-0 px-3 py-1" onclick="verMesesMov('${m.ClavePresupuestal}', \`${htmlMeses}\`)"><i class="fas fa-calendar-alt"></i> Meses</button>`;

                html += `<tr>
                    <td><a href="DetalleClave.aspx?clave=${m.ClavePresupuestal}" class="text-primary font-weight-bold text-decoration-underline">${m.ClavePresupuestal}</a></td>
                    <td>${m.TipoOperacion || 'MOVIMIENTO'}</td>
                    <td class="text-end font-weight-bold">${formattedTotal}</td>
                    <td class="text-center">${btnMeses}</td>
                </tr>`;
            });
            
            if(data.length === 0) {
                html = '<tr><td colspan="4" class="text-center">No hay movimientos registrados para este oficio.</td></tr>';
            }
            
            $("#contenidoadecuacion").html(html);
            $("#txtmontoadecuacion").val(suma.toFixed(2));
            $("#lblsumaadecuacion").text("Total Afectado: $" + parseFloat(suma).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        })
        .catch(err => {
            $("#contenidoadecuacion").html('<tr><td colspan="4" class="text-center text-danger">Error al cargar datos.</td></tr>');
            console.error(err);
        });
}

function verMesesMov(clave, htmlMeses) {
    Swal.fire({
        title: 'Desglose Mensual',
        html: `<b>Clave:</b> ${clave}<br>${htmlMeses}`,
        width: '600px',
        confirmButtonText: 'Cerrar'
    });
}
//-------- Numero de Solicitud -------------------
function solicitudes() {
    fetch(urisolic)
        .then(response => response.json())
        .then(data => _displaysolicitudes(data))
        .catch(error => console.error('No fue posible completar la solicitud', error));
}

function _displaysolicitudes(data) {
    const ul = document.getElementById('menu-numsolicitud');
    ul.innerHTML = '<li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Número de Solicitud)</a></li>';
    data.forEach(item => {
        let li = document.createElement("li");
        li.innerHTML = `<a class="dropdown-item custom-select-item" data-value="${item.value}">${item.text}</a>`;
        ul.appendChild(li);
    });
}
//-------------- Tipo de Adecuacion --------------
function tipos() {
    fetch(uritipo)
        .then(response => response.json())
        .then(data => _displaytipos(data))
        .catch(error => console.error('No fue posible completar la solicitud', error));
}

function _displaytipos(data) {
    const ul = document.getElementById('menu-tipo');
    ul.innerHTML = '<li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Tipo de Adecuación)</a></li>';
    data.forEach(item => {
        let li = document.createElement("li");
        li.innerHTML = `<a class="dropdown-item custom-select-item" data-value="${item.value}">${item.text}</a>`;
        ul.appendChild(li);
    });
}
//----------- Meses ----------------------------
const monthNames = ["", "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"];
function mes() {
    fetch(urimes)
        .then(response => response.json())
        .then(data => _displaymes(data))
        .catch(error => console.error('No fue posible completar la solicitud', error));
}

function _displaymes(data) {
    const ul = document.getElementById('menu-mes');
    ul.innerHTML = '<li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Mes)</a></li>';
    data.forEach(item => {
        let li = document.createElement("li");
        let mName = item.value > 0 && item.value <= 12 ? monthNames[item.value] : item.value;
        li.innerHTML = `<a class="dropdown-item custom-select-item" data-value="${item.value}">${mName}</a>`;
        ul.appendChild(li);
    });
}
//----------- Anios ----------------------------
function anios() {
    fetch(urianio)
        .then(response => response.json())
        .then(data => _displayanios(data))
        .catch(error => console.error('No fue posible completar la solicitud', error));
}

function _displayanios(data) {
    const ul = document.getElementById('menu-anio');
    ul.innerHTML = '<li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Año)</a></li>';
    data.forEach(item => {
        let li = document.createElement("li");
        li.innerHTML = `<a class="dropdown-item custom-select-item" data-value="${item.value}">${item.value}</a>`;
        ul.appendChild(li);
    });
}
//---------- Filtros de busqueda ------------------------
function filtrar() {
    if ($.fn.DataTable.isDataTable('#tablaclavepresupuestaladecuacion')) {
        $('#tablaclavepresupuestaladecuacion').DataTable().ajax.reload();
    }
}
//--------- Editar Adecuación
function editaradec() {
    $("#modal-form").modal("show");
}
//--------- Eliminar Adecuación ----------
function eliminarAdec() {

}
//========================== Adecuación=================================
/* DESACTIVADO PORQUE LA CARGA DE OFICIO PDF YA INCLUYE MOVIMIENTOS MANUALES. SE DEJA COMENTADO POR SI SE REQUIERE REACTIVAR
function agregaradecuacion()
{
    $("#hddaccion").val("A|0");
    $("#hddposicion").val("");
    combos();
    limpiar();
    activarmeses();
    //document.getElementById("txtoficionumero").focus();
    //nextFocus("txtoficionumero", "txtoficionumero");
    // $("txtoficionumero").focus();
    //eliminar("hola mundo");
    //$("#modal-form").modal('show', { backdrop: 'static', keyboard: false }, function () { document.getElementById("txtoficionumero").focus(); });
    $('#modal-form').modal({ backdrop: 'static', keyboard: false })
    $("#modal-form").modal('show');
    $("#modal-form").on('shown.bs.modal', function () {
        document.getElementById("txtoficionumero").focus();
        // Custom selects ya no usan Select2
    });

}
*/
//$('.modal').on('shown', function () {
//    $(this).find('input').focus();
//});
function limpiar()
{
    $("#txtasunto").val("");
    $("#txtasunto").val("");
    $("#txtnumsolicitud").val("");
    $("#txtfechasolicitud").val("");
    $("#txtconcepto").val("");
    $("#txtmontoadecuacion").val("");
    var cadena = "Emisor:$0.00  Receptor:$0.00  Diferencia:$0.00";
    $("#lblsumaadecuacion").val(cadena);
    $("#txtclavebusqueda").val("");
    $("#sec").val("");
    $("#ca").val("");
    $("#ur").val("");
    $("#fi").val("");
    $("#fu").val("");
    $("#sf").val("");
    $("#ssf").val("");
    $("#ps").val("");
    $("#pp").val("");
    $("#oa").val("");
    $("#ai").val("");
    $("#pt").val("");
    $("#mpio").val("");
    $("#ga").val("");
    $("#cogc").val("");
    $("#gasto").val("");
    $("#gto").val("");
    $("#ag").val("");
    $("#tg").val("");
    $("#tr").val("");
    $("#ff").val("");
    $("#sfte").val("");
    $("#rm").val("");
    $("#pf").val("");
    $("#cp").val("");
    $("#dm").val("");
    $("#nl").val("");
    $("#li").val("");
    $("#montoliberar").val("");
    $("#12meses").val("");
    $("#txtenero").val("");
    $("#txtfebrero").val("");
    $("#txtmarzo").val("");
    $("#txtabril").val("");
    $("#txtmayo").val("");
    $("#txtjunio").val("");
    $("#txtjulio").val("");
    $("#txtagosto").val("");
    $("#txtseptiembre").val("");
    $("#txtoctubre").val("");
    $("#txtnoviembre").val("");
    $("#txtdiciembre").val("");
    var montoliberar = document.getElementById('montoliberar');
    montoliberar.disabled = true;
    
    // Reset custom dropdowns
    $("#text-cbotipoadecuacion").text("Seleccione el Tipo de Adecuación");
    $("#cbotipoadecuacion").val("");
    $("#text-cbostatusadecuacion").text("Seleccione el Estatus");
    $("#cbostatusadecuacion").val("");
    $("#text-cbotipoclave").text("EXISTENTE");
    $("#cbotipoclave").val("EX");
    $("#text-t_o").text("(Seleccione)");
    $("#t_o").val("");
}
function combos()
{
    var $menu = $("#menu-cbotipoadecuacion");
    var $menuTo = $("#menu-t_o");
    $menu.empty();
    $menuTo.empty();
    
    $menu.append('<li><a class="dropdown-item custom-select-item active" data-value="">Seleccione el Tipo de Adecuación</a></li>');
    $menuTo.append('<li><a class="dropdown-item custom-select-item active" data-value="">(Seleccione)</a></li>');
    
    const catalog = [
        { clave: "3", text: "AMPLIACION" },
        { clave: "4", text: "REDUCCION" },
        { clave: "8", text: "TRASPASO" },
        { clave: "7", text: "RECALENDARIZACION" },
        { clave: "5", text: "LIBERACION" },
        { clave: "6", text: "RETENCION" }
    ];
    
    catalog.forEach(item => {
        $menu.append(`<li><a class="dropdown-item custom-select-item" data-value="${item.clave}">${item.text}</a></li>`);
        $menuTo.append(`<li><a class="dropdown-item custom-select-item" data-value="${item.clave}">${item.text}</a></li>`);
    });

    $("#text-cbotipoadecuacion").text("Seleccione el Tipo de Adecuación");
    $("#cbotipoadecuacion").val("");
    $("#text-t_o").text("(Seleccione)");
    $("#t_o").val("");

    // Cargar estatus (mantener si tienes endpoint, o hardcodear si no)
    var $menuEstatus = $("#menu-cbostatusadecuacion");
    $menuEstatus.empty();
    $menuEstatus.append('<li><a class="dropdown-item custom-select-item active" data-value="">Seleccione el Estatus</a></li>');
    $menuEstatus.append('<li><a class="dropdown-item custom-select-item" data-value="1">Registrado</a></li>');
    $menuEstatus.append('<li><a class="dropdown-item custom-select-item" data-value="2">Autorizado</a></li>');
    $("#text-cbostatusadecuacion").text("Seleccione el Estatus");
    $("#cbostatusadecuacion").val("");
}
function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 1; i--) {
        selectElement.remove(i);
    }
}
function calendario()
{
    
     
  
}
/**
 * Maneja el evento de click para agregar una nueva clave presupuestal al detalle de la adecuación actual.
 * @summary Valida la clave y los montos antes de llamar al flujo de inserción en tabla.
 * @description
 * 1. Recupera la clave ingresada (o concatena los fragmentos de los inputs individuales si se usa el formulario avanzado).
 * 2. Realiza validaciones estrictas de longitud (debe tener 56 o 57 caracteres obligatorios).
 * 3. Valida que los campos de cabecera como 'Oficio', 'Asunto', 'Solicitud', y 'Fecha' no estén vacíos.
 * 4. Verifica si el movimiento es de 'Liberación', en cuyo caso el campo 'montoliberar' debe ser un decimal válido.
 * 5. Verifica localmente si la clave ya fue agregada a la tabla para evitar duplicidad accidental.
 * 6. Dispara un Request asíncrono (AJAX a `CheckClave`) para saber si la clave es nueva en la BD de presupuesto.
 *    - Si NO existe, muestra un SweetAlert de advertencia preguntando al usuario si realmente desea registrar esa nueva clave o corregir posibles errores de dedo.
 *    - Si EXISTE, procede directamente a agregarla visualmente ejecutando `_agregarFilaATabla`.
 */
function btnagregarclavepresupuestal()
{
    var noliberado = 0;
    var clavetemp = "";
    var claveverificar = "";
    var varid = [];
    varid = $("#hddaccion").val().split("|");
    var longuitudcadena = 0;
    var auxsec = "";
    
    // Tomar la clave directamente del campo de texto
    clavetemp = $("#txtclavebusqueda").val().trim();
    
    // Fallback por si acaso
    if (clavetemp === "") {
        clavetemp = $("#ca").val().trim() + $("#ur").val().trim() + $("#fi").val().trim() + $("#fu").val().trim() + $("#sf").val().trim() + $("#ssf").val().trim()
            + $("#ps").val().trim() + $("#pp").val().trim() + $("#oa").val().trim() + $("#ai").val().trim() + $("#pt").val().trim() + $("#mpio").val().trim()
            + $("#ga").val().trim() + $("#cogc").val().trim() + $("#gasto").val().trim() + $("#gto").val().trim() + $("#ag").val().trim() + $("#tg").val().trim()
            + $("#tr").val().trim() + $("#ff").val().trim() + $("#sfte").val().trim() + $("#rm").val().trim() + $("#pf").val().trim() + $("#cp").val().trim()
            + $("#dm").val().trim();
    }

    longuitudcadena = clavetemp.length;
    // Se acepta 56 o 57 caracteres ya que algunas claves pueden no tener un dígito
    if (longuitudcadena < 56 || longuitudcadena > 57)
    {
        informar("La longitud de la clave presupuestal (" + longuitudcadena + " caracteres) no es válida, debe ser de 56 o 57 caracteres. Verifique por favor...");
        return;
    }
    if ($("#txtoficionumero").val()=="")
    {
        informar("Escriba el número de oficio por favor...");
        return;
    } 
    if ($("#txtasunto").val() == "")
    {
        informar("Escriba el número de asunto por favor...");
        return;
    }
    if ($("#txtnumsolicitud").val() == "")
    {
        informar("Escriba el número de solicitud por favor...");
        return;
    }
    // Se ha eliminado la validación de cbotipoadecuacion ya que está oculto en la vista actual.
    if ($("#txtfechasolicitud").val() == "") {
        informar("Escriba la fecha de la solicitud por favor...");
        return;
    }
    else {
        if (existeFecha($("#txtfechasolicitud").val()) == false) {
            informar("El formato de fecha es incorrecto...");
            return;
        }
    }
    if ($("#txtconcepto").val() == "") {
        informar("Escriba el concepto por favor...");
        return;
    }
    // Se han removido las validaciones estrictas de T_O porque ahora se permiten movimientos libres (AMPLIACION / REDUCCION)
    
    //****************************************************validamos si existe claves duplicadas*****************************************************
    if ($("#cbotipoadecuacion").val() != "6" && varid[0] !="c") //!!!!verificar porque no validad si es consultar(de forma general o al momento de modificar la clave)
    {
        let filas = $("#tablaclavepres").find('tbody tr').length;
        if (filas > 0)
        {
            $("#tablaclavepres tbody tr").each(function (index) {
                var numsecuencia = $(this).find("td").eq(0).html();
                var clavepres = $(this).find("td").eq(1).html();
                if (auxsec == "") {
                    auxsec = "primera";
                }
                if (auxsec != numsecuencia.trim()) {
                    if (clavetemp == clavepres) {
                        informar("La clave presupuestal que quiere agregar ya existe en la adecuación.. verifique por favor");
                        return;
                    }
                    auxsec = numsecuencia.trim();
                }

  
            })
        }
        
    }
    //****************************************************validamos si existe claves duplicadas*****************************************************

    //****************************************************PRIMER CASO SI ES LIBERACIÓN**************************************************************
    if ($("#cbotipoadecuacion").val() == "4") //Liberación
    {
        //verificamos si tiene monto el campo liberar recurso
        if ($("#montoliberar").val() == "")
        {
            informar("Ingrese el monto a liberar.....");
            return;
        }
        else
        {
            if (validateDecimal($("#montoliberar").val()) == false)
            {
                informar("El formato del monto a liberar no es correcto.....");
                return;
            }
        }
    }
    //****************************************************PRIMER CASO SI ES LIBERACIÓN******************************    //==============================================================================================================================================
    //============================================================AGREGAR A LA TABLA================================================================
    //==============================================================================================================================================

    // Check in backend if clave exists
    $.ajax({
        url: 'api/adecuaciones/CheckClave/' + clavetemp,
        method: 'GET',
        success: function(res) {
            if (!res.exists) {
                Swal.fire({
                    title: 'Clave Inexistente',
                    text: 'La clave presupuestal es 100% nueva y no existe en el catálogo. ¿Desea registrarla o prefiere rectificarla?',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Sí, registrarla',
                    cancelButtonText: 'No, rectificar'
                }).then((result) => {
                    // SweetAlert2 uses result.value or result.isConfirmed
                    if (result.value || result.isConfirmed) { 
                        _agregarFilaATabla(clavetemp);
                    }
                });
            } else {
                _agregarFilaATabla(clavetemp);
            }
        },
        error: function() {
            informar("Error al validar clave en el servidor");
        }
    });
}

/**
 * Agrega visualmente una nueva fila de movimiento (clave presupuestal) a la tabla temporal de la interfaz.
 * @summary Procesa los montos capturados en los meses, calcula los totales y añade una fila HTML al DOM de la tabla `#tablaclavepres`.
 * @description 
 * Esta función es llamada después de que el usuario captura una clave válida y sus respectivos montos mensuales.
 * Pasos que realiza:
 * 1. Extrae los valores ingresados en los inputs de los meses (Enero a Diciembre), eliminando comas y parseando a Float.
 * 2. Suma los valores de todos los meses para obtener un 'importeTotalMeses'. Si la suma de meses es 0, toma el valor de 'Monto a Liberar'.
 * 3. Calcula la secuencia o número de partida de la fila.
 * 4. Construye un template literal HTML para la fila (`<tr>`), almacenando atributos data (`data-clave`, `data-to`, `data-enero`, etc.) que posteriormente serán leídos al momento de guardar la adecuación manual en la BD.
 * 5. Agrega la fila a la tabla, y elimina cualquier fila de muestra que pudiera existir previamente.
 * 6. Invoca la función `limpiarnueva()` para reiniciar el formulario de captura.
 * @param {string} clavetemp - La cadena de texto de la clave presupuestal verificada (usualmente de 56 caracteres).
 */
function _agregarFilaATabla(clavetemp) {
    // Sumar los meses
    let pEnero = parseFloat($("#txtenero").val().replace(/,/g, '')) || 0;
    let pFebrero = parseFloat($("#txtfebrero").val().replace(/,/g, '')) || 0;
    let pMarzo = parseFloat($("#txtmarzo").val().replace(/,/g, '')) || 0;
    let pAbril = parseFloat($("#txtabril").val().replace(/,/g, '')) || 0;
    let pMayo = parseFloat($("#txtmayo").val().replace(/,/g, '')) || 0;
    let pJunio = parseFloat($("#txtjunio").val().replace(/,/g, '')) || 0;
    let pJulio = parseFloat($("#txtjulio").val().replace(/,/g, '')) || 0;
    let pAgosto = parseFloat($("#txtagosto").val().replace(/,/g, '')) || 0;
    let pSeptiembre = parseFloat($("#txtseptiembre").val().replace(/,/g, '')) || 0;
    let pOctubre = parseFloat($("#txtoctubre").val().replace(/,/g, '')) || 0;
    let pNoviembre = parseFloat($("#txtnoviembre").val().replace(/,/g, '')) || 0;
    let pDiciembre = parseFloat($("#txtdiciembre").val().replace(/,/g, '')) || 0;

    let tipoOperacion = $("#t_o").val() || "";
    let importeTotalMeses = 0;

    let sumMeses = pEnero + pFebrero + pMarzo + pAbril + pMayo + pJunio + pJulio + pAgosto + pSeptiembre + pOctubre + pNoviembre + pDiciembre;

    if (sumMeses > 0) {
        importeTotalMeses = sumMeses;
    } else {
        importeTotalMeses = parseFloat($("#montoliberar").val().replace(/,/g, '')) || 0;
    }

    // Obtener la secuencia
    let filas = $("#tablaclavepres tbody tr").length;
    let nextSec = filas + 1;
    let numSecStr = nextSec < 10 ? "0" + nextSec : nextSec.toString();

    // Construir la fila
    let htmlRow = `<tr data-clave="${clavetemp}" data-to="${tipoOperacion}" data-montototal="${importeTotalMeses}" data-enero="${pEnero}" data-febrero="${pFebrero}" data-marzo="${pMarzo}" data-abril="${pAbril}" data-mayo="${pMayo}" data-junio="${pJunio}" data-julio="${pJulio}" data-agosto="${pAgosto}" data-septiembre="${pSeptiembre}" data-octubre="${pOctubre}" data-noviembre="${pNoviembre}" data-diciembre="${pDiciembre}">
        <td>${numSecStr}</td>
        <td>
            <a href="#" class="text-primary font-weight-bold text-decoration-underline" data-bs-toggle="modal" data-bs-target="#modal-decifrador">
                ${clavetemp}
            </a>
        </td>
        <td>${tipoOperacion}</td>
        <td>${importeTotalMeses.toFixed(2)}</td>
        <td>-</td>
        <td>-</td>
        <td>${importeTotalMeses.toFixed(2)}</td>
        <td>
            <button class="btn btn-danger btn-sm mb-0" onclick="$(this).closest('tr').remove();">X</button>
        </td>
    </tr>`;

    // Si es el primero, quitar el de ejemplo si existiera
    if (filas === 1 && $("#tablaclavepres tbody tr").eq(0).text().includes("010012211H04501A01A001000A1B113110026115528O001-0081120262")) {
        $("#tablaclavepres tbody").empty();
    }

    $("#tablaclavepres tbody").append(htmlRow);

    // Limpiar campos de meses para la siguiente clave
    limpiarnueva();
    informar("Clave agregada a la lista.");
}
/* DESACTIVADO PORQUE LA CARGA DE OFICIO PDF YA INCLUYE MOVIMIENTOS MANUALES. SE DEJA COMENTADO POR SI SE REQUIERE REACTIVAR
function guardarAdecuacionManual() {
    // Validar cabecera
    let asunto = $("#txtasunto").val().trim();
    let numSolicitud = $("#txtnumsolicitud").val().trim();
    let idTipoAdecuacion = "";
    let textoTipoAdecuacion = "";
    let fechaSolicitud = $("#txtfechasolicitud").val();
    let concepto = $("#txtconcepto").val().trim();
    let valMonto = $("#txtmontoadecuacion").val() || "0";
    let montoAdec = parseFloat(valMonto.replace(/,/g, '')) || 0;

    if (!numOficio || !numSolicitud || !fechaSolicitud) {
        informar("Por favor complete los datos obligatorios del oficio (Número, Solicitud y Fecha).");
        return;
    }

    let filas = $("#tablaclavepres tbody tr");
    if (filas.length === 0) {
        informar("Debe agregar al menos una clave presupuestal.");
        return;
    }

    let movimientos = [];
    filas.each(function() {
        let tr = $(this);
        let clave = tr.attr("data-clave");
        if (clave && clave !== "undefined") {
            movimientos.push({
                ClavePresupuestal: clave,
                TipoOperacion: tr.attr("data-to") || "0",
                MontoTotal: parseFloat(tr.attr("data-montototal")) || 0,
                Enero: parseFloat(tr.attr("data-enero")) || 0,
                Febrero: parseFloat(tr.attr("data-febrero")) || 0,
                Marzo: parseFloat(tr.attr("data-marzo")) || 0,
                Abril: parseFloat(tr.attr("data-abril")) || 0,
                Mayo: parseFloat(tr.attr("data-mayo")) || 0,
                Junio: parseFloat(tr.attr("data-junio")) || 0,
                Julio: parseFloat(tr.attr("data-julio")) || 0,
                Agosto: parseFloat(tr.attr("data-agosto")) || 0,
                Septiembre: parseFloat(tr.attr("data-septiembre")) || 0,
                Octubre: parseFloat(tr.attr("data-octubre")) || 0,
                Noviembre: parseFloat(tr.attr("data-noviembre")) || 0,
                Diciembre: parseFloat(tr.attr("data-diciembre")) || 0
            });
        }
    });

    if (movimientos.length === 0) {
        informar("No se encontraron claves válidas.");
        return;
    }

    let hasAmpliacion = false;
    let hasReduccion = false;
    let sumReceptor = 0;
    let sumEmisor = 0;

    movimientos.forEach(m => {
        let to = m.TipoOperacion.toUpperCase();
        let sumRow = m.MontoTotal; // Ahora usamos el MontoTotal que ya corregimos que trae la cantidad real
        if (to.includes("AMPLIACION") || m.TipoOperacion == "1" || m.TipoOperacion == "3") {
            hasAmpliacion = true;
            sumReceptor += sumRow;
        }
        if (to.includes("REDUCCION") || to.includes("DISMINUCION") || m.TipoOperacion == "2" || m.TipoOperacion == "4") {
            hasReduccion = true;
            sumEmisor += sumRow;
        }
    });

    if (hasAmpliacion && hasReduccion) {
        idTipoAdecuacion = "4"; // Traspaso Id is 4
        textoTipoAdecuacion = "TRASPASO";
    } else if (hasAmpliacion) {
        idTipoAdecuacion = "6"; // Ampliacion Id is 6
        textoTipoAdecuacion = "AMPLIACION";
    } else if (hasReduccion) {
        idTipoAdecuacion = "2"; // Reduccion Id is 2
        textoTipoAdecuacion = "REDUCCION";
    } else {
        idTipoAdecuacion = "1"; // Default to Recalendarización to prevent FK crash, though it should be prevented earlier
        textoTipoAdecuacion = "OTRO";
    }

    if (idTipoAdecuacion == "4") {
        if (Math.abs(sumReceptor - sumEmisor) > 0.01) {
            informar("En un Traspaso, la suma de las claves emisoras ($" + sumEmisor.toFixed(2) + ") debe ser igual a la suma de las receptoras ($" + sumReceptor.toFixed(2) + ").");
            return;
        }
    }

    if ($("#chkApartado").is(":checked") && !apartadoIdActual) {
        informar("Indicó que viene de un Apartado, pero no ha buscado un folio válido.");
        return;
    }

    let sumaMovimientos = 0;
    movimientos.forEach(m => {
        if (m.TipoOperacion.toUpperCase().includes("AMPLIACION") || m.TipoOperacion === "1" || m.TipoOperacion === "3") {
            sumaMovimientos += m.MontoTotal;
        } else if (m.TipoOperacion.toUpperCase().includes("REDUCCION") || m.TipoOperacion === "2" || m.TipoOperacion === "4") {
            sumaMovimientos += m.MontoTotal; // En reducciones solas, este es el monto total
        } else {
            sumaMovimientos += m.MontoTotal;
        }
    });

    if (montoAdec === 0) {
        // En un traspaso se divide entre 2 porque hay una emisora y una receptora de la misma cantidad
        if (idTipoAdecuacion == "4" || idTipoAdecuacion == "7") {
            montoAdec = sumaMovimientos / 2;
        } else {
            montoAdec = sumaMovimientos;
        }
    }

    let payload = {
        NumOficio: numOficio,
        NumSolicitud: numSolicitud,
        Asunto: asunto,
        TipoAdecuacionId: idTipoAdecuacion,
        TipoAdecuacion: textoTipoAdecuacion,
        FechaSolicitud: fechaSolicitud,
        Concepto: concepto,
        MontoTotal: montoAdec,
        Estatus: "Autorizado",
        ApartadoId: apartadoIdActual,
        Movimientos: movimientos
    };

    // Mostrar loading
    $("#btnguardaradecuacion").prop("disabled", true).text("Guardando...");

    $.ajax({
        url: '/api/adecuaciones/GuardarManual',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function(res) {
            $("#btnguardaradecuacion").prop("disabled", false).text("Guardar Adecuación");
            if(res.success) {
                guardar("Adecuación registrada correctamente con " + movimientos.length + " movimientos.");
                $("#modal-form").modal("hide");
                // Recargar grid de oficios
                $('#tablaclavepresupuestaladecuacion').DataTable().ajax.reload();
            } else {
                eliminar(res.message || "Ocurrió un error al guardar.");
            }
        },
        error: function(err) {
            $("#btnguardaradecuacion").prop("disabled", false).text("Guardar Adecuación");
            eliminar("Error de comunicación AJAX: " + err.responseText);
        }
    });
}
*/
//========================== Adecuación=================================
//==========================mensajes general=============================
function guardar(mensaje)
{
    Swal.fire({
        position: 'center',
        showConfirmButton: true,
        confirmButtonColor: '#cb0c9f',
        confirmButtonText: 'Entendido',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-primary px-4 py-2'
        },
        html: `
            <div class="text-center py-4">
                <div class="mb-4">
                    <i class="fas fa-check-circle" style="font-size: 4rem; color: #cb0c9f; filter: drop-shadow(0px 4px 6px rgba(203, 12, 159, 0.3));"></i>
                </div>
                <h4 class="font-weight-bolder text-dark mb-2">¡Completado!</h4>
                <p class="text-muted text-sm mb-0">${mensaje}</p>
            </div>
        `
    });
}
function informar(mensaje)
{
    Swal.fire({
        position: 'center',
        showConfirmButton: true,
        confirmButtonText: 'Aceptar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-warning px-4 py-2 text-white'
        },
        html: `
            <div class="text-center py-4">
                <div class="mb-4">
                    <i class="fas fa-exclamation-circle" style="font-size: 4rem; color: #f5365c; filter: drop-shadow(0px 4px 6px rgba(245, 54, 92, 0.3));"></i>
                </div>
                <h4 class="font-weight-bolder text-dark mb-2">¡Atención!</h4>
                <p class="text-muted text-sm mb-0">${mensaje}</p>
            </div>
        `
    });
}
function eliminar(mensaje)
{
    Swal.fire({
        position: 'center',
        showConfirmButton: true,
        confirmButtonText: 'Eliminado',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger px-4 py-2'
        },
        html: `
            <div class="text-center py-4">
                <div class="mb-4">
                    <i class="fas fa-trash-alt" style="font-size: 4rem; color: #ea0606; filter: drop-shadow(0px 4px 6px rgba(234, 6, 6, 0.3));"></i>
                </div>
                <h4 class="font-weight-bolder text-dark mb-2">¡Eliminado!</h4>
                <p class="text-muted text-sm mb-0">${mensaje}</p>
            </div>
        `
    });
}
//==========================mensajes general=============================

function procesarOficio() {
    let fileInput = document.getElementById("fileOficio");
    if (fileInput.files.length === 0) {
        informar("Por favor seleccione un archivo PDF o Excel.");
        return;
    }

    let file = fileInput.files[0];
    let formData = new FormData();
    formData.append("file", file);

    $("#uploadProgressContainer").show();
    $("#uploadProgressBar").css("width", "50%").removeClass("bg-danger bg-success").css("background", "linear-gradient(90deg, #cb0c9f, #7928ca)");
    $("#uploadProgressText").text("50%").removeClass("text-danger text-success").css("color", "#cb0c9f");
    $("#uploadProgressLabel").text("Analizando Documento...").removeClass("text-danger text-success").css("color", "#cb0c9f");

    $("#btnProcesarOficio").prop("disabled", true).text("Analizando...");
    $("#uploadResult").html("");

    $.ajax({
        url: '/api/UploadOficio',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            try {
            $("#uploadProgressBar").css("width", "100%").css("background", "#28a745");
            $("#uploadProgressText").text("100%").css("color", "#28a745");
            $("#uploadProgressLabel").text("Completado").css("color", "#28a745");
            $("#btnProcesarOficio").prop("disabled", false).text("Procesar Archivo");
            if (res.success) {
                const proceed = () => {
                    let tbody = $("#listaRevisionMovimientos");
                    tbody.empty();
                    
                    function generarClaveSegments(claveStr) {
                    claveStr = claveStr || "";
                    // Rellenar con espacios si es corta, truncar si es larga
                    if (claveStr.length < 56) claveStr = claveStr.padEnd(56, ' ');
                    else if (claveStr.length > 56) claveStr = claveStr.substring(0, 56);
                    
                    const segmentsInfo = [
                        { name: 'UR', len: 2 }, { name: 'URO', len: 3 }, { name: 'FI', len: 1 },
                        { name: 'F', len: 1 }, { name: 'SF', len: 1 }, { name: 'SSF', len: 1 },
                        { name: 'PS', len: 1 }, { name: 'PP', len: 3 }, { name: 'OA', len: 2 },
                        { name: 'AI', len: 3 }, { name: 'PT', len: 4 }, { name: 'MPIO', len: 3 },
                        { name: 'GA', len: 1 }, { name: 'COGC', len: 2 }, { name: 'GASTO', len: 5 },
                        { name: 'GTO', len: 2 }, { name: 'AG', len: 2 }, { name: 'TG', len: 1 },
                        { name: 'TR', len: 1 }, { name: 'FF', len: 1 }, { name: 'SFTE', len: 1 },
                        { name: 'RM', len: 2 }, { name: 'PF', len: 5 }, { name: 'RI', len: 3 },
                        { name: 'CP', len: 4 }, { name: 'DM', len: 1 }
                    ];
                    
                    let html = '';
                    let currentIndex = 0;
                    
                    for (let seg of segmentsInfo) {
                        let val = claveStr.substring(currentIndex, currentIndex + seg.len);
                        html += `
                        <div class="d-flex flex-column align-items-center" style="width: calc(${seg.len} * 0.75rem + 10px);">
                            <span style="font-size: 0.6rem; color: #6c757d; font-weight: 600;">${seg.name}</span>
                            <input type="text" class="form-control text-center segment-input px-0 py-1" 
                                style="font-size: 0.75rem; height: 26px; border-radius: 4px; background-color: #f8f9fa; border: 1px solid #e9ecef;"
                                value="${val.trim()}" maxlength="${seg.len}">
                        </div>`;
                        currentIndex += seg.len;
                    }
                    return html;
                }
                
                res.data.Movimientos.forEach((mov, index) => {
                    let tipoClave = mov.TipoOperacionClave || 3; // Usa T/O del backend o 3 por defecto
                    let importe = mov.MontoReduccion > 0 ? mov.MontoReduccion : mov.MontoAmpliacion;
                    
                    let mesesHtml = generarRangosCalendario([mov.Enero, mov.Febrero, mov.Marzo, mov.Abril, mov.Mayo, mov.Junio, mov.Julio, mov.Agosto, mov.Septiembre, mov.Octubre, mov.Noviembre, mov.Diciembre], mov.PorLiberar);
                    
                    let cardTypeClass = (tipoClave === 4 || tipoClave === 2) ? "type-red" : "type-amp";
                    
                    let warningHtml = "";
                    if (mov.RequiresReview) {
                        warningHtml = `
                        <div class="alert alert-warning py-2 px-3 mb-3 d-flex align-items-center" style="font-size: 0.85rem; border-radius: 6px; border: none; color: #ffffff; text-shadow: 1px 1px 2px rgba(0,0,0,0.2);">
                            <i class="fas fa-exclamation-triangle me-2" style="font-size: 1.1rem; color: #ffffff;"></i> 
                            <strong style="color: #ffffff;">¡Revisar!</strong><span style="margin-left: 5px; color: #ffffff;">${mov.ReviewMessage}</span>
                        </div>`;
                    }
                    
                    let html = `
                    <div class="modern-card-item ${cardTypeClass}" data-index="${index}" data-importe-original="${mov.ImporteOriginal || 0}">
                        ${warningHtml}
                        <div class="row align-items-center mb-2">
                            <div class="col-11">
                                <div class="modern-input-group">
                                    <div class="modern-input-label mb-1">
                                        <span class="badge bg-dark me-2" style="font-size: 0.75rem;">${(index + 1).toString().padStart(2, '0')}</span>Clave Presupuestal
                                        ${mov.EsNuevaClave ? '<span class="badge bg-gradient-success ms-2 text-xxs" title="Esta clave se registrará en el presupuesto inicial.">NUEVA</span>' : ''}
                                    </div>
                                    <div class="clave-segments-container d-flex flex-wrap gap-1">
                                        ${generarClaveSegments(mov.ClavePresupuestal)}
                                    </div>
                                </div>
                            </div>
                            <div class="col-1 text-end px-0">
                                <button type="button" class="btn-delete-modern btn-eliminar-rev" title="Eliminar fila" onclick="$(this).closest('.modern-card-item').remove(); recalcularTotalesRevision();"><i class="fas fa-trash"></i></button>
                            </div>
                        </div>
                        
                        <div class="row align-items-center">
                            <div class="col-md-6 mb-2 mb-md-0">
                                <div class="modern-input-group dropdown">
                                    <div class="modern-input-label mb-1">Tipo de Operación</div>
                                    <input type="hidden" class="rev-tipo" value="${tipoClave}">
                                    <button class="form-control d-flex justify-content-between align-items-center custom-select-btn px-0 border-0" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer; background: transparent; box-shadow: none; padding: 0; font-weight: 600; color: #32325d; font-size: 0.85rem; height: auto;">
                                        <span class="selected-text text-start">${tipoClave == 3 ? 'Ampliación Liquida' : tipoClave == 4 ? 'Reducción Liquida' : tipoClave == 1 ? 'Incrementos Destino' : 'Decrementos Origen'}</span>
                                        <i class="fas fa-chevron-down text-muted" style="font-size: 0.75rem;"></i>
                                    </button>
                                    <ul class="dropdown-menu w-100 custom-select-menu" style="z-index: 1050;">
                                        <li><a class="dropdown-item custom-select-item ${tipoClave == 3 ? 'active' : ''}" data-value="3">Ampliación Liquida</a></li>
                                        <li><a class="dropdown-item custom-select-item ${tipoClave == 4 ? 'active' : ''}" data-value="4">Reducción Liquida</a></li>
                                        <li><a class="dropdown-item custom-select-item ${tipoClave == 1 ? 'active' : ''}" data-value="1">Incrementos Destino</a></li>
                                        <li><a class="dropdown-item custom-select-item ${tipoClave == 2 ? 'active' : ''}" data-value="2">Decrementos Origen</a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="modern-input-group">
                                    <div class="modern-input-label">Importe ($)</div>
                                    <div class="d-flex align-items-center">
                                        <i class="fas fa-money-bill-wave text-success me-2"></i>
                                        <input type="text" class="form-control rev-importe w-100" value="${importe.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2})}" onblur="let val = parseFloat(this.value.replace(/,/g, '')) || 0; this.value = val.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2}); recalcularTotalesRevision();">
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        ${mesesHtml}
                    </div>
                    `;
                    tbody.append(html);
                });
                
                recalcularTotalesRevision();
                
                // Show PDF Preview
                let objectUrl = URL.createObjectURL(file);
                $("#previewFrame").attr("src", objectUrl);
                
                $("#modalRevisionOficio").attr("data-oficio", res.data.Oficio);
                $("#revNumSolicitud").val(res.data.Oficio || "");
                
                if (res.data.Fecha) {
                    $("#modalRevisionOficio").attr("data-fecha", res.data.Fecha);
                    let parts = res.data.Fecha.split('T')[0].split('-');
                    if (parts.length === 3) {
                        $("#revFechaOficio").val(parts[2] + "/" + parts[1] + "/" + parts[0]);
                    } else {
                        $("#revFechaOficio").val(res.data.Fecha);
                    }
                } else {
                    $("#revFechaOficio").val("");
                }
                
                if (res.data.Solicita) {
                    $("#modalRevisionOficio").attr("data-solicita", res.data.Solicita);
                    $("#revSolicita").val(res.data.Solicita);
                } else {
                    $("#revSolicita").val("");
                }
                
                if (res.data.Autoriza) {
                    $("#modalRevisionOficio").attr("data-autoriza", res.data.Autoriza);
                    $("#revAutoriza").val(res.data.Autoriza);
                } else {
                    $("#revAutoriza").val("");
                }

                $("#modalRevisionOficio").modal("show");
                
                // Guardar el número de oficio original en un atributo
                $("#modalRevisionOficio").attr("data-oficio", res.data.Oficio);
                };

                if (res.isDuplicate) {
                    Swal.fire({
                        title: '<span style="font-size: 1.5rem; font-weight: 700; color: #32325d;">Posible Duplicado</span>',
                        html: '<style>.btn-vino { background-color: #621132 !important; border-color: #621132 !important; color: #ffffff !important; }</style>' +
                              '<p style="color: #525f7f; font-size: 0.95rem; margin-bottom: 1rem;">El número de solicitud <strong style="color: #32325d; font-weight: 700;">' + (res.data.Oficio || '') + '</strong> ya existe en el sistema.</p>' +
                              '<p style="color: #8898aa; font-size: 0.85rem; margin-bottom: 0;">¿Es una solicitud diferente? De ser así, deberás cambiar el número en la pantalla de revisión para poder aplicarla.</p>',
                        showCancelButton: true,
                        confirmButtonText: '<i class="fas fa-arrow-right me-2"></i> Sí, continuar',
                        cancelButtonText: '<i class="fas fa-times me-1"></i> Cancelar',
                        buttonsStyling: false,
                        customClass: {
                            popup: 'border-0 shadow-lg',
                            confirmButton: 'btn bg-gradient-info text-white shadow-sm me-3',
                            cancelButton: 'btn btn-vino shadow-sm'
                        }
                    }).then((result) => {
                        if (result.isConfirmed || result.value) {
                            proceed();
                        } else {
                            $("#uploadProgressBar").css("width", "0%");
                            $("#uploadProgressText").text("0%");
                            $("#uploadProgressLabel").text("Cancelado");
                        }
                    });
                } else {
                    proceed();
                }
            } else {
                $("#uploadProgressBar").css("background", "#dc3545");
                $("#uploadProgressText").text("Error").css("color", "#dc3545");
                $("#uploadProgressLabel").text("Error").css("color", "#dc3545");
                $("#uploadResult").html(`<div class="alert alert-danger text-white mt-3"><strong>Error:</strong> ${res.message}</div>`);
            }
            } catch (ex) {
                alert("JS Error: " + ex.message + "\n" + ex.stack);
                console.error(ex);
            }
        },
        error: function (err) {
            $("#uploadProgressBar").css("width", "100%").css("background", "#dc3545");
            $("#uploadProgressText").text("Error").css("color", "#dc3545");
            $("#uploadProgressLabel").text("Error").css("color", "#dc3545");
            $("#btnProcesarOficio").prop("disabled", false).text("Procesar Archivo");
            $("#uploadResult").html(`<div class="alert alert-danger text-white mt-3"><strong>Error del Servidor:</strong> No se pudo procesar el archivo.</div>`);
        }
    });
}

function recalcularTotalesRevision() {
    let totAmp = 0;
    let totRed = 0;
    let rowCount = 0;
    $("#listaRevisionMovimientos .modern-card-item").each(function () {
        let tipo = $(this).find(".rev-tipo").val();
        
        // Actualizar clase de color visual basado en el tipo
        $(this).removeClass("type-amp type-red");
        if (tipo === "3" || tipo === "1") {
            $(this).addClass("type-amp");
        } else {
            $(this).addClass("type-red");
        }

        let importeStr = $(this).find(".rev-importe").val() || "0";
        let importe = parseFloat(importeStr.replace(/,/g, '')) || 0;
        let tipoVal = parseInt(tipo); 
        if (tipoVal === 3 || tipoVal === 1) {
            totAmp += importe;
        } else {
            totRed += importe;
        }
        rowCount++;
    });

    $("#revTotalAmp").text("$" + totAmp.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
    $("#revTotalRed").text("$" + totRed.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
    $("#revTotalTrans").text(rowCount);
}

function confirmarRevisionOficio() {
    let movimientos = [];
    let valid = true;
    let obj = {
        NumSolicitud: $("#modalRevisionOficio").data("oficio"),
        Movimientos: []
    };

    $("#listaRevisionMovimientos .modern-card-item").each(function () {
        let row = $(this);
        // Reconstruir la clave a partir de los segmentos
        let clave = "";
        row.find(".segment-input").each(function () {
            let segVal = $(this).val().trim();
            let segLen = parseInt($(this).attr("maxlength"));
            clave += segVal.padEnd(segLen, ' ');
        });
        clave = clave.trim();
        let tipo = row.find(".rev-tipo").val();
        let importeStr = row.find(".rev-importe").val() || "0";
        let importe = parseFloat(importeStr.replace(/,/g, '')) || 0;
        
        let meses = [0,0,0,0,0,0,0,0,0,0,0,0];
        let porLiberar = 0;
        row.find('.rango-row').each(function() {
            let de = parseInt($(this).find('.rev-de').val()) || 0;
            let al = parseInt($(this).find('.rev-al').val()) || 0;
            let mStr = $(this).find('.rev-monto').val() || "0";
            let m = parseFloat(mStr.replace(/,/g, '')) || 0;
            if (de === 0 && al === 0) {
                porLiberar += m;
            } else {
                for (let i = de; i <= al; i++) {
                    if (i >= 1 && i <= 12) {
                        meses[i-1] += m;
                    }
                }
            }
        });
        
        movimientos.push({
            ClavePresupuestal: clave,
            TipoOperacion: tipo,
            MontoTotal: importe,
            Enero: meses[0],
            Febrero: meses[1],
            Marzo: meses[2],
            Abril: meses[3],
            Mayo: meses[4],
            Junio: meses[5],
            Julio: meses[6],
            Agosto: meses[7],
            Septiembre: meses[8],
            Octubre: meses[9],
            Noviembre: meses[10],
            Diciembre: meses[11],
            PorLiberar: porLiberar
        });
    });
    
    if (movimientos.length === 0) {
        informar("No hay movimientos para aplicar.");
        return;
    }
    
    let numOficio = $("#revNumSolicitud").val() || $("#modalRevisionOficio").attr("data-oficio");
    let fechaText = $("#revFechaOficio").val();
    let solicitaPdf = $("#revSolicita").val();
    let autorizaPdf = $("#revAutoriza").val();
    
    let fechaPdf = null;
    if (fechaText && fechaText.includes("/")) {
        let parts = fechaText.split("/");
        if (parts.length === 3) {
            fechaPdf = parts[2] + "-" + parts[1] + "-" + parts[0] + "T00:00:00";
        }
    } else {
        fechaPdf = $("#modalRevisionOficio").attr("data-fecha");
    }
    
    let payload = {
        NumSolicitud: numOficio,
        FechaSolicitud: fechaPdf,
        Solicita: solicitaPdf,
        Autoriza: autorizaPdf,
        Movimientos: movimientos
    };

    $("#btnConfirmarRevision").prop("disabled", true).text("Aplicando...");

    $.ajax({
        url: '/api/UploadOficio/AplicarPersonalizado',
        type: 'POST',
        data: JSON.stringify(payload),
        contentType: 'application/json',
        success: function (res) {
            $("#btnConfirmarRevision").prop("disabled", false).html('<i class="fas fa-check me-2"></i>Confirmar y Aplicar');
            if (res.success) {
                guardar("El oficio se aplicó correctamente al presupuesto.");
                $("#modalRevisionOficio").modal("hide");
                $('#tablaclavepresupuestaladecuacion').DataTable().ajax.reload();
            } else {
                informar("Error: " + res.message);
            }
        },
        error: function (err) {
            $("#btnConfirmarRevision").prop("disabled", false).html('<i class="fas fa-check me-2"></i>Confirmar y Aplicar');
            informar("Error de servidor al aplicar el oficio.");
        }
    });
}

function agregarMovimientoRevisionManual() {
    let index = $("#listaRevisionMovimientos .modern-card-item").length;
    let tbody = $("#listaRevisionMovimientos");
    
    function getClaveSegmentsHTML() {
        const segmentsInfo = [
            { name: 'UR', len: 2 }, { name: 'URO', len: 3 }, { name: 'FI', len: 1 },
            { name: 'F', len: 1 }, { name: 'SF', len: 1 }, { name: 'SSF', len: 1 },
            { name: 'PS', len: 1 }, { name: 'PP', len: 3 }, { name: 'OA', len: 2 },
            { name: 'AI', len: 3 }, { name: 'PT', len: 4 }, { name: 'MPIO', len: 3 },
            { name: 'GA', len: 1 }, { name: 'COGC', len: 2 }, { name: 'GASTO', len: 5 },
            { name: 'GTO', len: 2 }, { name: 'AG', len: 2 }, { name: 'TG', len: 1 },
            { name: 'TR', len: 1 }, { name: 'FF', len: 1 }, { name: 'SFTE', len: 1 },
            { name: 'RM', len: 2 }, { name: 'PF', len: 5 }, { name: 'RI', len: 3 },
            { name: 'CP', len: 4 }, { name: 'DM', len: 1 }
        ];
        let html = '';
        for (let seg of segmentsInfo) {
            html += `
            <div class="d-flex flex-column align-items-center" style="width: calc(${seg.len} * 0.75rem + 10px);">
                <span style="font-size: 0.6rem; color: #6c757d; font-weight: 600;">${seg.name}</span>
                <input type="text" class="form-control text-center segment-input px-0 py-1" 
                    style="font-size: 0.75rem; height: 26px; border-radius: 4px; background-color: #f8f9fa; border: 1px solid #e9ecef;"
                    value="" maxlength="${seg.len}">
            </div>`;
        }
        return html;
    }

    let mesesHtml = generarRangosCalendario([0,0,0,0,0,0,0,0,0,0,0,0], 0);

    let html = `
    <div class="modern-card-item type-amp" data-index="${index}" data-importe-original="0">
        <div class="row align-items-center mb-2">
            <div class="col-11">
                <div class="modern-input-group">
                    <div class="modern-input-label mb-1">
                        <span class="badge bg-dark me-2" style="font-size: 0.75rem;">${(index + 1).toString().padStart(2, '0')}</span>Clave Presupuestal
                        <span class="badge bg-gradient-info ms-2 text-xxs">MANUAL</span>
                    </div>
                    <div class="clave-segments-container d-flex flex-wrap gap-1">
                        ${getClaveSegmentsHTML()}
                    </div>
                </div>
            </div>
            <div class="col-1 text-end px-0">
                <button type="button" class="btn-delete-modern btn-eliminar-rev" title="Eliminar fila" onclick="$(this).closest('.modern-card-item').remove(); recalcularTotalesRevision();"><i class="fas fa-trash"></i></button>
            </div>
        </div>
        
        <div class="row align-items-center">
            <div class="col-md-6 mb-2 mb-md-0">
                <div class="modern-input-group dropdown">
                    <div class="modern-input-label mb-1">Tipo de Operación</div>
                    <input type="hidden" class="rev-tipo" value="3">
                    <button class="form-control d-flex justify-content-between align-items-center custom-select-btn px-0 border-0" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer; background: transparent; box-shadow: none; padding: 0; font-weight: 600; color: #32325d; font-size: 0.85rem; height: auto;">
                        <span class="selected-text text-start">Ampliación Liquida</span>
                        <i class="fas fa-chevron-down text-muted" style="font-size: 0.75rem;"></i>
                    </button>
                    <ul class="dropdown-menu w-100 custom-select-menu" style="z-index: 1050;">
                        <li><a class="dropdown-item custom-select-item active" data-value="3">Ampliación Liquida</a></li>
                        <li><a class="dropdown-item custom-select-item" data-value="4">Reducción Liquida</a></li>
                        <li><a class="dropdown-item custom-select-item" data-value="1">Incrementos Destino</a></li>
                        <li><a class="dropdown-item custom-select-item" data-value="2">Decrementos Origen</a></li>
                    </ul>
                </div>
            </div>
            <div class="col-md-6">
                <div class="modern-input-group">
                    <div class="modern-input-label">Importe ($)</div>
                    <div class="d-flex align-items-center">
                        <i class="fas fa-money-bill-wave text-success me-2"></i>
                        <input type="text" class="form-control rev-importe w-100" value="0.00" onblur="let val = parseFloat(this.value.replace(/,/g, '')) || 0; this.value = val.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2}); recalcularTotalesRevision();">
                    </div>
                </div>
            </div>
        </div>
        
        ${mesesHtml}
    </div>
    `;
    
    tbody.append(html);
    recalcularTotalesRevision();
    
    let container = document.querySelector(".modern-scroll");
    if(container) {
        container.scrollTop = container.scrollHeight;
    }
}

function ventananuevaclave() {
    limpiarnueva();
    var x = document.getElementById("nuevaclave");
    var y = document.getElementById("txtclavebusqueda");

    if ($("#cbotipoclave").val() == "NU") {
        $("#cbotipoclave option[value=EX]").attr("selected", true);
        $("#altanuevaclave").modal("show");
    }
    else
    {
        $("#cbotipoclave option[value=EX]").attr("selected", true);
    }
    //if (x.style.display === "none")
    //{
    //    x.style.display = "block";
    //    y.disabled = true;
    //}
    //else
    //{
    //    x.style.display = "none";
    //    y.disabled = false;
    //    y.focus();
    //}
}
function limpiarnueva()
{
    $("#txtclavebusqueda").val("");
    $("#sec").val("");
    $("#ca").val("");
    $("#ur").val("");
    $("#fi").val("");
    $("#fu").val("");
    $("#sf").val("");
    $("#ssf").val("");
    $("#ps").val("");
    $("#pp").val("");
    $("#oa").val("");
    $("#ai").val("");
    $("#pt").val("");
    $("#mpio").val("");
    $("#ga").val("");
    $("#cogc").val("");
    $("#gasto").val("");
    $("#gto").val("");
    $("#ag").val("");
    $("#tg").val("");
    $("#tr").val("");
    $("#ff").val("");
    $("#sfte").val("");
    $("#rm").val("");
    $("#pf").val("");
    $("#cp").val("");
    $("#dm").val("");
    // $("#t_o").val(""); // Comentado para no perder la operación seleccionada
    $("#montoliberar").val(""); // Limpiar importe total
    $("#nl").val("");
    $("#12meses").val("");
    $("#txtenero").val("");
    $("#txtfebrero").val("");
    $("#txtmarzo").val("");
    $("#txtabril").val("");
    $("#txtmayo").val("");
    $("#txtjunio").val("");
    $("#txtjulio").val("");
    $("#txtagosto").val("");
    $("#txtseptiembre").val("");
    $("#txtoctubre").val("");
    $("#txtnoviembre").val("");
    $("#txtdiciembre").val("");
}
function mascara(o, f) {
    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}
function cpf(v) {
    v = v.replace(/([^0-9\.]+)/g, '');
    v = v.replace(/^[\.]/, '');
    v = v.replace(/[\.][\.]/g, '');
    v = v.replace(/\.(\d)(\d)(\d)/g, '.$1$2');
    v = v.replace(/\.(\d{1,2})\./g, '.$1');
    v = v.toString().split('').reverse().join('').replace(/(\d{3})/g, '$1,');
    v = v.split('').reverse().join('').replace(/^[\,]/, '');
    return v;
}
function existen_registros(tabla) {
    let filas = $(tabla).find('tbody tr').length;

    if (filas > 0) {
        return 1;
    }
    else {
        return 0;
    }
}
function secuencia() {
    let filas = $(tablaclavepres).find('tbody tr').length;
    if (filas > 0) {
        //obtenemos el ultimo valor de la secuencia del registro de la tabla
        var valor = $("tablaclavepres").find('tr').eq(filas).find('td').eq(0).html();
        var sec = parseInt(valor);
        if (validarEntero(sec) == true) {
            sec = sec + 1;
            if (sec > 0 && sec < 10) {
                $("#sec").val("0" + sec.toString());
            }
            else {
                $("#sec").val(sec.toString());
            }
        }
        else {
            $("#sec").val("");
        }
        t_o.focus();
    }
    else {
        $("#sec").val("01");
        t_o.focus();
    }

}
function validarEntero(valor) {
    //intento convertir a entero.
    //si era un entero no le afecta, si no lo era lo intenta convertir
    valor = parseInt(valor)

    //Compruebo si es un valor numérico
    if (isNaN(valor)) {
        //entonces (no es numero) devuelvo el valor cadena vacia
        return false
    }
    else {
        //En caso contrario (Si era un número) devuelvo el valor
        return true
    }
}
$("#txtclavebusqueda").focus(function () {
    $(this).css("background-color", "#495057");
    $("#txtclavebusqueda").select();
})
function existeFecha(fecha) {//validar si la fecha introducida es real
    var fechaf = fecha.split("/");
    var day = fechaf[0];
    var month = fechaf[1];
    var year = fechaf[2];
    var date = new Date(year, month, '0');
    if ((day - 0) > (date.getDate() - 0)) {
        return false;
    }
    return true;
}
function llenadotipooperacion()
{
    //limpiamos el menu custom
    var $menuTo = $('#menu-t_o');
    $menuTo.empty();
    $menuTo.append('<li><a class="dropdown-item custom-select-item active" data-value="">(Seleccione)</a></li>');
    $("#text-t_o").text("(Seleccione)");
    $("#t_o").val("");

    var valor = document.getElementById("cbotipoadecuacion").value;
    var montoliberar = document.getElementById('montoliberar');
    $("#txtenero").val("0.00");
    $("#txtfebrero").val("0.00");
    $("#txtmarzo").val("0.00");
    $("#txtabril").val("0.00");
    $("#txtmayo").val("0.00");
    $("#txtjunio").val("0.00");
    $("#txtjulio").val("0.00");
    $("#txtagosto").val("0.00");
    $("#txtseptiembre").val("0.00");
    $("#txtoctubre").val("0.00");
    $("#txtnoviembre").val("0.00");
    $("#txtdiciembre").val("0.00");
    switch (valor)
    {
        case "8"://TRASPASO
        case "7"://RECALENDARIZACIÓN
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="1">1 - INCREMENTOS</a></li>');
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="2">2 - DECREMENTOS</a></li>');
            break;
        case "5"://LIBERACIÓN
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="5">5 - LIBERACIÓN</a></li>');
            montoliberar.disabled = false;
            break;
        case "6"://RETENCIÓN
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="6">6 - RETENCIÓN</a></li>');
            montoliberar.disabled = false;
            break;
        case "3"://AMPLIACIÓN 
        case "4":// REDUCCIÓN
        default:
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="3">3 - AMPLIACIÓN</a></li>');
            $menuTo.append('<li><a class="dropdown-item custom-select-item" data-value="4">4 - REDUCCIÓN</a></li>');
            break;
    }

    if (valor != "5" && valor != "6") {
        montoliberar.disabled = true;
        $("#montoliberar").val("");
    }
}

let apartadoIdActual = null;
function toggleApartado() {
    if ($("#chkApartado").is(":checked")) {
        $("#divFolioApartado").show();
        $("#txtmontoadecuacion").prop("readonly", true);
    } else {
        $("#divFolioApartado").hide();
        $("#txtmontoadecuacion").prop("readonly", false).val("");
        $("#txtFolioApartado").val("");
        apartadoIdActual = null;
    }
}

function buscarApartado() {
    let folio = $("#txtFolioApartado").val().trim();
    if (!folio) {
        informar("Ingrese un folio de apartado");
        return;
    }
    $.ajax({
        url: 'api/adecuaciones/Apartado/' + folio,
        method: 'GET',
        success: function(res) {
            $("#txtmontoadecuacion").val(res.Monto.toFixed(2));
            $("#txtoficionumero").val(res.Oficio);
            apartadoIdActual = res.Id;
            guardar("Apartado cargado correctamente. Monto reservado: $" + res.Monto.toFixed(2));
        },
        error: function() {
            informar("No se encontró el apartado con folio " + folio);
            apartadoIdActual = null;
            $("#txtmontoadecuacion").val("");
        }
    });
}
function liberado(clavepresupuestal)
{
    $("#nl").val("");
    $("#li").val("");
    $.ajax({
        type: "POST",
        url: "Webservice1.asmx/Noliberado",
        data: "{'clavepresupuestal':'" + clavepresupuestal + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg)
        {
            var datos = msg.d;
            $(datos).each(function ()
            {
                $("#nl").val(this.noliberado);
                $("#li").val(this.liberado);
            });
        },
        error: function (msg) {
            $("#msgcuenta").text("Error al llenar el combo");
        }
    });
}
function validateDecimal(valor)
{
    var RE = /^\d*\.?\d*$/;
    if (RE.test(valor)) {
        return true;
    } else {
        return false;
    }
}
function recorrergrid()
{
    let pagodetalle = [];
    document.querySelectorAll('#tabla tbody tr').forEach(function (e) {
        let fila =
        {
            num: e.querySelector('.num').innerText,
            idregistro: e.querySelector('.idregistro').innerText,
            idtipopago: e.querySelector('.idtipopago').innerText,
            tipopago: e.querySelector('.tipopago').innerText,
            numpago: e.querySelector('.numpago').innerText,
            numpoliza: e.querySelector('.numpoliza').innerText,
            fechapoliza: e.querySelector('.fechapoliza').innerText,
            porcentajepagado: e.querySelector('.porcentajepagado').innerText,
            importe: e.querySelector('.importe').innerText,
            idcombinado: e.querySelector('.idcombinado').innerText,
            combinadoop: e.querySelector('.combinadoop').innerText,
            folio: e.querySelector('.folio').innerText
        };
        pagodetalle.push(fila);
    });
                  //var customerId = $(this).find(".customerIDCell").html();
                //var campo1, campo2, campo3;
                //$(this).children("td").each(function (index2) {
                //    switch (index2)
                //    {
                //        case 0:
                //            campo1 = $(this).text();
                //            break;
                //        case 2:
                //            campo1 = $(this).text();
                //            break;
                //        case 3:
                //            campo1 = $(this).text();
                //            break;
                //        default:

                //    }
                //    $(this).css("background-color", "#ECF8E0");
                //})
}
function activarmeses()
{
    var enero = document.getElementById('txtenero');
    var febrero = document.getElementById('txtfebrero');
    var marzo = document.getElementById('txtmarzo');
    var abril = document.getElementById('txtabril');
    var mayo = document.getElementById('txtmayo');
    var junio = document.getElementById('txtjunio');
    var julio = document.getElementById('txtjulio');
    var agosto = document.getElementById('txtagosto');
    var septiembre = document.getElementById('txtseptiembre');
    var octubre = document.getElementById('txtoctubre');
    var noviembre = document.getElementById('txtnoviembre');
    var diciembre = document.getElementById('txtdiciembre');
    $("#txtenero").val("0.00");
    enero.disabled = true;
    $("#txtfebrero").val("0.00");
    febrero.disabled = true;
    $("#txtmarzo").val("0.00");
    marzo.disabled = true;
    $("#txtabril").val("0.00");
    abril.disabled = true;
    $("#txtmayo").val("0.00");
    mayo.disabled = true;
    $("#txtjunio").val("0.00");
    junio.disabled = true;
    $("#txtjulio").val("0.00");
    julio.disabled = true;
    $("#txtagosto").val("0.00");
    agosto.disabled = true;
    $("#txtseptiembre").val("0.00");
    septiembre.disabled = true;
    $("#txtoctubre").val("0.00");
    octubre.disabled = true;
    $("#txtnoviembre").val("0.00");
    noviembre.disabled = true;
    $("#txtdiciembre").val("0.00");
    diciembre.disabled = true;

    var anioactual = fecha.getFullYear();
    var mesactual = fecha.getMonth() + 1;
    for (x = mesactual; x <= 12; x++)
    {
        switch (x)
        { 
            case 1:
                enero.disabled = false;
                break;
            case 2:
                febrero.disabled = false;
                break;
            case 3:
                marzo.disabled = false;
                break;
            case 4:
                abril.disabled = false;
                break;
            case 5:
                mayo.disabled = false;
                break;
            case 6:
                junio.disabled = false;
                break;
            case 7:
                julio.disabled = false;
                break;
            case 8:
                agosto.disabled = false;
                break;
            case 9:
                septiembre.disabled = false;
                break;
            case 10:
                octubre.disabled = false;
                break;
            case 11:
                noviembre.disabled = false;
                break;
            case 12:
                diciembre.disabled = false;
                break;
        }
    }
}
function verificarmontoliberar()
{
    if ($("#nl").val() == "") {
        informar("Favor de consultar una clave presupuestal para liberar recurso");
        return;
    }
    else
    {

    }
}
function limpiarclave()
{
    if ($("#txtclavebusqueda").val()=="")
    {
        $("#sec").val("");
        $("#ca").val("");
        $("#ur").val("");
        $("#fi").val("");
        $("#fu").val("");
        $("#sf").val("");
        $("#ssf").val("");
        $("#ps").val("");
        $("#pp").val("");
        $("#oa").val("");
        $("#ai").val("");
        $("#pt").val("");
        $("#mpio").val("");
        $("#ga").val("");
        $("#cogc").val("");
        $("#gasto").val("");
        $("#gto").val("");
        $("#ag").val("");
        $("#tg").val("");
        $("#tr").val("");
        $("#ff").val("");
        $("#sfte").val("");
        $("#rm").val("");
        $("#pf").val("");
        $("#cp").val("");
        $("#dm").val("");
        $("#nl").val("");
        $("#li").val("");
        $("#montoliberar").val("");
    }
}
function validarFormatoFecha(campo) {//validar el formato de fecha
    var fechaFormateada = moment(campo).format("DD/MM/YYYY");
    var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
    if ((fechaFormateada.match(RegExPattern)) && (fechaFormateada != '')) {
        return true;
    }
    else {
        return false;
    }
}

// =====================================================================
// NUEVO: Función para Guardar toda la Adecuación en la Base de Datos
// =====================================================================
function guardarTodoEnBD() {
    // 1. Recolectar datos de la cabecera
    var adecuacion = {
        NumOficio: $("#txtnumsolicitud").val(),
        NumSolicitud: $("#txtnumsolicitud").val(),
        Asunto: $("#txtasunto").val(),
        TipoAdecuacion: $("#cbotipoadecuacion option:selected").text(),
        FechaSolicitud: $("#txtfechasolicitud").val(), // Puede requerir formato YYYY-MM-DD
        MontoTotal: parseFloat($("#txtmontoadecuacion").val() || 0),
        Estatus: $("#cbostatusadecuacion option:selected").text(),
        Movimientos: []
    };

    // 2. Recolectar movimientos de la cuadrícula (si hubiera tabla implementada)
    // Como ejemplo, recolectamos el que está actualmente en los inputs de meses
    // En una versión final, esto iteraría sobre las filas de #tablaclavepres
    
    var claveCapturada = $("#ca").val() + $("#ur").val() + $("#fi").val() + $("#fu").val() + $("#sf").val() + $("#ssf").val()
        + $("#ps").val() + $("#pp").val() + $("#oa").val() + $("#ai").val() + $("#pt").val() + $("#mpio").val()
        + $("#ga").val() + $("#cogc").val() + $("#gasto").val() + $("#gto").val() + $("#ag").val() + $("#tg").val()
        + $("#tr").val() + $("#ff").val() + $("#sfte").val() + $("#rm").val() + $("#pf").val() + $("#cp").val() + $("#dm").val();

    if (claveCapturada.length > 10) {
        var tipoOp = $("#cbotipoadecuacion").val() == "2" ? "DISMINUCION" : "AUMENTO";
        
        adecuacion.Movimientos.push({
            TipoOperacion: tipoOp,
            ClavePresupuestal: claveCapturada,
            Enero: parseFloat($("#txtenero").val() || 0),
            Febrero: parseFloat($("#txtfebrero").val() || 0),
            Marzo: parseFloat($("#txtmarzo").val() || 0),
            Abril: parseFloat($("#txtabril").val() || 0),
            Mayo: parseFloat($("#txtmayo").val() || 0),
            Junio: parseFloat($("#txtjunio").val() || 0),
            Julio: parseFloat($("#txtjulio").val() || 0),
            Agosto: parseFloat($("#txtagosto").val() || 0),
            Septiembre: parseFloat($("#txtseptiembre").val() || 0),
            Octubre: parseFloat($("#txtoctubre").val() || 0),
            Noviembre: parseFloat($("#txtnoviembre").val() || 0),
            Diciembre: parseFloat($("#txtdiciembre").val() || 0)
        });
    }

    // 3. Enviar a C# vía AJAX
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/GuardarAdecuacion",
        data: JSON.stringify({ adecuacion: adecuacion }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var resultado = response.d.split('|');
            if (resultado[0] === "OK") {
                guardar("¡Adecuación #" + resultado[1] + " guardada exitosamente en la base de datos!");
                limpiar();
            } else {
                eliminar("Ocurrió un error en el servidor: " + resultado[1]);
            }
        },
        error: function (err) {
            eliminar("Error de comunicación AJAX: " + err.responseText);
        }
    });
}

function generarRangosCalendario(meses, porLiberar) {
    let rangos = [];
    let actual = null;
    for(let i=0; i<12; i++) {
        if (meses[i] > 0) {
            if (actual && actual.monto === meses[i] && actual.al === i) {
                actual.al = i + 1;
            } else {
                if (actual) rangos.push(actual);
                actual = { de: i + 1, al: i + 1, monto: meses[i] };
            }
        } else {
            if (actual) {
                rangos.push(actual);
                actual = null;
            }
        }
    }
    if (actual) rangos.push(actual);
    
    let html = `<div class="calendario-container mt-3 pt-3 border-top">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <span class="modern-input-label" style="font-size: 0.75rem;">Calendario de Pagos</span>
                        <button type="button" class="btn btn-sm btn-outline-primary py-1 px-2 m-0" onclick="agregarRango(this)"><i class="fas fa-plus"></i></button>
                    </div>
                    <div class="rangos-list">`;
    
    if (rangos.length === 0 && porLiberar === 0) {
        html += `<div class="text-muted text-xs font-italic">Sin calendario</div>`;
    }
    
    rangos.forEach(r => {
        html += createRangoRow(r.de, r.al, r.monto);
    });
    
    if (porLiberar > 0) {
        html += createRangoRow(0, 0, porLiberar);
    }
    
    html += `</div></div>`;
    return html;
}

function generarCalendarioHtml(mov, isPdf) {
    let rangos = [];
    if (isPdf) {
        rangos = mov.rangos || [];
    } else {
        let meses = [mov.Enero, mov.Febrero, mov.Marzo, mov.Abril, mov.Mayo, mov.Junio, mov.Julio, mov.Agosto, mov.Septiembre, mov.Octubre, mov.Noviembre, mov.Diciembre];
        rangos = generarRangosCalendario(meses, mov.PorLiberar);
    }
    
    let html = `
    <div class="calendario-container mt-3">
        <div class="d-flex justify-content-between align-items-center mb-2">
            <h6 class="text-secondary fw-bold mb-0" style="font-size: 0.75rem;">CALENDARIO</h6>
            <button type="button" class="btn btn-sm btn-outline-primary py-0 px-2 rounded-circle" onclick="agregarRango(this)" title="Agregar Rango"><i class="fas fa-plus"></i></button>
        </div>
        <div class="rangos-list">`;
        
    if (rangos.length === 0) {
        // html += `<div class="text-muted small fst-italic">Sin calendario</div>`;
        html += createRangoRow(0, 0, 0); // Always show at least one row, defaulting to Por Liberar
    } else {
        rangos.forEach(r => {
            html += createRangoRow(r.de, r.al, r.monto);
        });
    }
    
    html += `
        </div>
    </div>`;
    return html;
}

function renderMonthBoxes(de, al) {
    let html = '<div class="month-box-container flex-wrap">';
    // PL box
    let isPL = (de === 0 && al === 0);
    html += `<div class="month-box ${isPL ? 'selected' : ''}" data-month="0" onclick="toggleMonth(this)" style="width: auto; padding: 0 8px;" title="Por Liberar">PL</div>`;
    for(let i = 1; i <= 12; i++) {
        let cls = "month-box";
        if (!isPL && (i === de || i === al)) cls += " selected";
        else if (!isPL && de > 0 && al > 0 && i > de && i < al) cls += " in-range";
        html += `<div class="${cls}" data-month="${i}" onclick="toggleMonth(this)">${i}</div>`;
    }
    html += '</div>';
    return html;
}

function createRangoRow(de, al, monto) {
    let montoFormatted = monto.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2});
    return `
    <div class="row align-items-center mb-1 rango-row">
        <div class="col-8 pe-1">
            <input type="hidden" class="rev-de" value="${de}">
            <input type="hidden" class="rev-al" value="${al}">
            ${renderMonthBoxes(de, al)}
        </div>
        <div class="col-3 px-1">
            <input type="text" class="form-control form-control-sm text-end rev-monto" value="${montoFormatted}" onblur="formatRangoMonto(this)" title="Importe del Mes">
        </div>
        <div class="col-1 ps-1 text-center">
            <i class="fas fa-times text-danger" style="cursor:pointer;" onclick="$(this).closest('.rango-row').remove();"></i>
        </div>
    </div>`;
}

function toggleMonth(btn) {
    let box = $(btn);
    let row = box.closest('.rango-row');
    let month = parseInt(box.attr('data-month'));
    let inputDe = row.find('.rev-de');
    let inputAl = row.find('.rev-al');
    
    let currentDe = parseInt(inputDe.val()) || 0;
    let currentAl = parseInt(inputAl.val()) || 0;
    
    if (month === 0) {
        inputDe.val(0);
        inputAl.val(0);
    } else if (currentDe === 0 || (currentDe > 0 && currentAl > 0)) {
        inputDe.val(month);
        inputAl.val(0);
    } else {
        if (month < currentDe) {
            inputAl.val(currentDe);
            inputDe.val(month);
        } else {
            inputAl.val(month);
        }
    }
    updateMonthBoxes(row);
    recalcularTotalCard(btn);
}

function updateMonthBoxes(row) {
    let de = parseInt(row.find('.rev-de').val()) || 0;
    let al = parseInt(row.find('.rev-al').val()) || 0;
    let isPL = (de === 0 && al === 0);
    
    row.find('.month-box').each(function() {
        let m = parseInt($(this).attr('data-month'));
        $(this).removeClass('selected in-range');
        if (m === 0) {
            if (isPL) $(this).addClass('selected');
        } else {
            if (!isPL) {
                if (m === de || m === al) $(this).addClass('selected');
                else if (de > 0 && al > 0 && m > de && m < al) $(this).addClass('in-range');
            }
        }
    });
}

function formatRangoMonto(input) {
    let val = parseFloat(input.value.replace(/,/g, '')) || 0; 
    input.value = val.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2});
    recalcularTotalCard(input);
}

function recalcularTotalCard(element) {
    let card = $(element).closest('.modern-card-item');
    if (card.length === 0) return;
    
    let suma = 0;
    card.find('.rango-row').each(function() {
        let de = parseInt($(this).find('.rev-de').val()) || 0;
        let al = parseInt($(this).find('.rev-al').val()) || 0;
        let numMeses = (de === 0 && al === 0) ? 1 : (al > 0 ? (al - de + 1) : 1);
        if (numMeses < 1) numMeses = 1;
        
        let monto = parseFloat($(this).find('.rev-monto').val().replace(/,/g, '')) || 0;
        suma += (monto * numMeses);
    });
    
    // Actualizar el importe de la operación
    let importeInput = card.find('.rev-importe');
    if (importeInput.length > 0 && suma > 0) {
        importeInput.val(suma.toLocaleString('en-US', {minimumFractionDigits:2, maximumFractionDigits:2}));
    }
    
    // Comparar con el importe original para quitar la advertencia si ya cuadra
    let importeOriginal = parseFloat(card.attr('data-importe-original')) || 0;
    if (importeOriginal > 0 && Math.abs(suma - importeOriginal) <= 1) {
        card.find('.alert-warning').fadeOut(300, function() { $(this).remove(); });
    }
    
    // Actualizar también los totales globales de la página
    if (typeof recalcularTotalesRevision === 'function') {
        recalcularTotalesRevision();
    }
}

function agregarRango(btn) {
    $(btn).closest('.calendario-container').find('.rangos-list').append(createRangoRow(0, 0, 0));
}
$(document).ready(function() {
    function limpiarModalCargarOficio() {
        document.getElementById("fileOficio").value = "";
        let fileNameEl = document.getElementById("fileOficioName");
        if(fileNameEl) {
            fileNameEl.innerText = 'o haz clic para explorar (.pdf)';
            fileNameEl.classList.remove('text-primary', 'font-weight-bold');
            fileNameEl.closest('.modern-file-label').style.backgroundColor = '#f8f9fa';
        }
        $("#uploadProgressContainer").hide();
        $("#uploadProgressBar").css("width", "0%");
        $("#uploadProgressText").text("0%");
        $("#uploadResult").html("");
        $("#btnProcesarOficio").prop("disabled", false).text("Procesar Archivo");
        
        // Reset preview
        $("#previewFrame").attr("src", "");
    }

    $('#modalCargarOficio').on('hidden.bs.modal', function () {
        limpiarModalCargarOficio();
    });

    $('#modalRevisionOficio').on('hidden.bs.modal', function () {
        limpiarModalCargarOficio();
    });
});
