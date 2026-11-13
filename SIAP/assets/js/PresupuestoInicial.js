/* Carga de presupuesto inicial: validar -> revisar -> confirmar */
(function () {
    'use strict';
    var API = 'api/presupuestoinicial/';
    var validacion = null;   // último resultado válido (trae el token)
    var dtErrores = null, dtAdv = null, dtHist = null;

    var fmt = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
    function moneda(v) { return fmt.format(v || 0); }
    function esc(s) { return $('<div>').text(s == null ? '' : String(s)).html(); }
    function fecha(v) {
        if (!v) return '';
        var d = new Date(v);
        return isNaN(d) ? v : d.toLocaleDateString('es-MX') + ' ' + d.toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' });
    }
    function mensajeError(xhr) {
        var r = xhr.responseJSON;
        if (typeof r === 'string') return r;
        if (r && r.Message) return r.ExceptionMessage || r.Message;
        return xhr.responseText || 'Error de comunicación con el servidor.';
    }
    // SweetAlert2 del proyecto es una versión antigua (usa 'type' y devuelve {value}); estos
    // ayudantes funcionan con esa y con las nuevas (que usan 'icon' y {isConfirmed}).
    function cargando(titulo, texto) {
        Swal.fire({ title: titulo, text: texto || '', allowOutsideClick: false, allowEscapeKey: false, showConfirmButton: false });
        Swal.showLoading();
    }
    function preguntar(titulo, html) {
        return Swal.fire({ title: titulo, html: html, type: 'question', showCancelButton: true,
                           confirmButtonText: 'Sí, importar', cancelButtonText: 'Cancelar' })
            .then(function (r) { return !!(r && (r.isConfirmed || r.value === true)); });
    }
    var idioma = { url: 'https://cdn.datatables.net/plug-ins/1.13.4/i18n/es-MX.json' };

    // ---------------------------------------------------------------- años y estructura
    function cargarAnios() {
        $.getJSON(API + 'anios').done(function (anios) {
            var $c = $('#cboAnio').empty();
            if (!anios || !anios.length) {
                $c.append('<option value="">Sin estructura configurada</option>');
                return;
            }
            anios.sort(function (a, b) { return b - a; });
            $.each(anios, function (_, a) { $c.append($('<option>').val(a).text(a)); });
            var actual = new Date().getFullYear();
            if (anios.indexOf(actual + 1) >= 0 && new Date().getMonth() >= 9) $c.val(actual + 1);
            else if (anios.indexOf(actual) >= 0) $c.val(actual);
            mostrarEstructura();
        });
    }

    function mostrarEstructura() {
        var anio = $('#cboAnio').val();
        if (!anio) { $('#estructura').empty(); return; }
        $.getJSON(API + 'estructura', { anio: anio }).done(function (segs) {
            if (!segs.length) { $('#estructura').text('No hay estructura de clave para ' + anio + '.'); return; }
            var html = 'Clave de <b>' + segs[0].LongitudClave + '</b> caracteres para ' + esc(anio) + ': ';
            $.each(segs, function (_, s) {
                html += '<span title="' + esc(s.Nombre) + ' (posición ' + s.Posicion + ')"><b>' + esc(s.Abreviatura) + '</b> ' + s.Longitud + '</span>';
            });
            $('#estructura').html(html);
        });
    }

    // ---------------------------------------------------------------- validar
    function validar() {
        var anio = $('#cboAnio').val(), oficio = $.trim($('#txtOficio').val()), file = $('#fileExcel')[0].files[0];
        if (!anio) return Swal.fire('Falta el año', 'Seleccione el año del presupuesto.', 'warning');
        if (!oficio) return Swal.fire('Falta el oficio', 'Capture el número de oficio.', 'warning');
        if (!file) return Swal.fire('Falta el archivo', 'Seleccione el Excel del presupuesto inicial.', 'warning');

        var fd = new FormData();
        fd.append('anio', anio); fd.append('oficio', oficio); fd.append('archivo', file, file.name);

        cargando('Revisando el archivo…', 'Se valida cada clave e importe');
        $.ajax({ url: API + 'validar', type: 'POST', data: fd, processData: false, contentType: false })
            .done(function (r) { Swal.close(); mostrarResultado(r); })
            .fail(function (xhr) { Swal.fire('No se pudo validar', mensajeError(xhr), 'error'); });
    }

    function mostrarResultado(r) {
        validacion = r.Valido ? r : null;
        $('#panelResultado').show();
        $('#rClaves').text((r.TotalFilas || 0).toLocaleString('es-MX'));
        $('#rAprobado').text(moneda(r.TotalAprobado));
        $('#rLiberado').text(moneda(r.TotalLiberado));
        $('#rNoLiberado').text(moneda(r.TotalNoLiberado));
        $('#rErrores').text((r.TotalErrores || 0).toLocaleString('es-MX'));

        if (r.Valido) {
            $('#tituloResultado').html('<i class="fa-solid fa-circle-check text-success me-2"></i>El archivo es correcto: ' + esc(r.Archivo));
            $('#btnImportar').show();
        } else {
            $('#tituloResultado').html('<i class="fa-solid fa-circle-xmark text-danger me-2"></i>El archivo fue rechazado: ' + esc(r.Archivo));
            $('#btnImportar').hide();
        }

        var gen = '';
        $.each(r.ErroresGenerales || [], function (_, e) { gen += '<div class="alert alert-danger text-white py-2">' + esc(e) + '</div>'; });
        $('#erroresGenerales').html(gen);

        // Errores por fila
        if (dtErrores) { dtErrores.destroy(); dtErrores = null; }
        var $tb = $('#tblErrores tbody').empty();
        if (r.Errores && r.Errores.length) {
            $.each(r.Errores, function (_, e) {
                $tb.append('<tr><td>' + e.Fila + '</td><td class="clave">' + esc(e.Clave) + '</td><td>' + esc(e.Motivo) + '</td></tr>');
            });
            $('#avisoTope').text(r.TotalErrores > r.Errores.length ? 'Se muestran los primeros ' + r.Errores.length + ' de ' + r.TotalErrores + ' errores.' : '');
            $('#bloqueErrores').show();
            dtErrores = $('#tblErrores').DataTable({ pageLength: 10, order: [[0, 'asc']], language: idioma });
        } else {
            $('#bloqueErrores').hide();
        }

        // Advertencias de catálogo
        if (dtAdv) { dtAdv.destroy(); dtAdv = null; }
        var $ta = $('#tblAdvertencias tbody').empty();
        var advGen = '';
        $.each(r.AdvertenciasGenerales || [], function (_, a) { advGen += '<div class="alert alert-warning text-white py-2">' + esc(a) + '</div>'; });
        $('#advertenciasGenerales').html(advGen);
        if ((r.Advertencias && r.Advertencias.length) || advGen) {
            $.each(r.Advertencias || [], function (_, a) {
                $ta.append('<tr><td><b>' + esc(a.Abreviatura) + '</b> ' + esc(a.Nombre) + '</td><td class="clave">' + esc(a.Valor) + '</td><td class="num">' + a.Claves + '</td></tr>');
            });
            $('#bloqueAdvertencias').show();
            if (r.Advertencias && r.Advertencias.length)
                dtAdv = $('#tblAdvertencias').DataTable({ pageLength: 5, searching: false, language: idioma });
        } else {
            $('#bloqueAdvertencias').hide();
        }
        $('html, body').animate({ scrollTop: $('#panelResultado').offset().top - 20 }, 300);
    }

    // ---------------------------------------------------------------- importar
    function importar() {
        if (!validacion) return;
        preguntar('¿Importar el presupuesto inicial ' + validacion.Anio + '?',
            '<b>' + validacion.TotalFilas.toLocaleString('es-MX') + '</b> claves por <b>' + moneda(validacion.TotalAprobado) + '</b><br/>Oficio ' + esc(validacion.Oficio)
        ).then(function (confirmado) {
            if (!confirmado) return;
            cargando('Importando…', 'Guardando las claves, no cierre la página');
            $.ajax({
                url: API + 'importar', type: 'POST', contentType: 'application/json',
                data: JSON.stringify({ Token: validacion.Token, Anio: validacion.Anio, Oficio: validacion.Oficio, Archivo: validacion.Archivo })
            }).done(function (r) {
                Swal.fire('Presupuesto cargado', r.claves.toLocaleString('es-MX') + ' claves por ' + moneda(r.aprobado), 'success');
                limpiar();
                cargarHistorial();
            }).fail(function (xhr) {
                var r = xhr.responseJSON;
                if (r && r.Errores !== undefined) { Swal.close(); mostrarResultado(r); return; }
                Swal.fire('No se importó', mensajeError(xhr), 'error');
            });
        });
    }

    function limpiar() {
        validacion = null;
        $('#panelResultado').hide();
        $('#fileExcel').val('');
        $('#txtOficio').val('');
    }

    // ---------------------------------------------------------------- historial
    function cargarHistorial() {
        $.getJSON(API + 'historial').done(function (lista) {
            if (dtHist) { dtHist.destroy(); dtHist = null; }
            var $tb = $('#tblHistorial tbody').empty();
            $.each(lista, function (_, h) {
                var archivo = h.TieneArchivo
                    ? '<a href="' + API + 'archivo/' + h.Id + '" title="Descargar"><i class="fa-solid fa-file-excel icono-excel me-2"></i>' + esc(h.Archivo || 'archivo.xlsx') + '</a>'
                    : '<i class="fa-solid fa-file-excel icono-excel me-2 opacity-5"></i>' + esc(h.Archivo || '—');
                $tb.append('<tr><td>' + esc(h.Anio) + '</td><td>' + esc(h.Oficio) + '</td><td>' + archivo + '</td>' +
                    '<td class="num" data-order="' + h.MontoTotalAprobado + '">' + moneda(h.MontoTotalAprobado) + '</td>' +
                    '<td class="num">' + (h.TotalClaves == null ? '' : h.TotalClaves.toLocaleString('es-MX')) + '</td>' +
                    '<td data-order="' + (h.FechaCarga || '') + '">' + fecha(h.FechaCarga) + '</td><td>' + esc(h.Usuario) + '</td></tr>');
            });
            $('#totalRegistros').text(lista.length);
            dtHist = $('#tblHistorial').DataTable({ pageLength: 10, order: [[5, 'desc']], language: idioma });
        }).fail(function (xhr) { Swal.fire('Historial', mensajeError(xhr), 'error'); });
    }

    $(function () {
        cargarAnios();
        cargarHistorial();
        $('#cboAnio').on('change', function () { mostrarEstructura(); validacion = null; $('#panelResultado').hide(); });
        $('#fileExcel').on('change', function () { validacion = null; $('#panelResultado').hide(); });
        $('#btnCargar').on('click', validar);
        $('#btnImportar').on('click', importar);
        $('#btnCancelar').on('click', limpiar);
    });
})();
