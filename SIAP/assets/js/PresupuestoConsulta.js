/* Consulta General del presupuesto (por ejercicio) y alta manual de claves */
(function () {
    'use strict';
    var API = 'api/presupuesto/';
    var tabla = null;
    var estructura = [];          // segmentos de la clave del ejercicio elegido
    var longitudClave = 0;

    var fmt = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
    function moneda(v) { return fmt.format(v || 0); }
    function esc(s) { return $('<div>').text(s == null ? '' : String(s)).html(); }
    function anio() { return $('#cboEjercicio').val(); }
    function mensajeError(xhr) {
        var r = xhr.responseJSON;
        if (typeof r === 'string') return r;
        if (r && r.Message) return r.ExceptionMessage || r.Message;
        return xhr.responseText || 'Error de comunicación con el servidor.';
    }
    // SweetAlert2 del proyecto es antiguo: usa 'type' y responde {value}
    function aviso(titulo, html, tipo) { return Swal.fire({ title: titulo, html: html, type: tipo }); }
    function cargando(titulo) {
        Swal.fire({ title: titulo, allowOutsideClick: false, allowEscapeKey: false, showConfirmButton: false });
        Swal.showLoading();
    }
    function preguntar(titulo, html, boton) {
        return Swal.fire({ title: titulo, html: html, type: 'question', showCancelButton: true,
                           confirmButtonText: boton, cancelButtonText: 'Cancelar' })
            .then(function (r) { return !!(r && (r.isConfirmed || r.value === true)); });
    }

    // ------------------------------------------------------------ ejercicios y filtros
    function cargarEjercicios() {
        return $.getJSON(API + 'ejercicios').done(function (lista) {
            var $c = $('#cboEjercicio').empty();
            $.each(lista, function (_, e) {
                $c.append($('<option>').val(e.Anio).text(e.Anio + (e.Claves ? '' : ' (vacío)')));
            });
            var guardado = null;
            try { guardado = sessionStorage.getItem('siap.ejercicio'); } catch (e) { }
            var conDatos = lista.filter(function (e) { return e.Claves > 0; });
            if (guardado && lista.some(function (e) { return String(e.Anio) === guardado; })) $c.val(guardado);
            else if (conDatos.length) $c.val(conDatos[0].Anio);
        });
    }

    function cargarOpcionesSegmento($sel) {
        var abrev = $sel.data('abrev');
        var primera = $sel.find('option:first').text();
        $sel.empty().append($('<option value="">').text(primera));
        if (!anio()) return;
        $.getJSON(API + 'filtros/segmento', { anio: anio(), abrev: abrev }).done(function (ops) {
            $.each(ops, function (_, o) {
                var t = o.Valor + ' · ' + (o.Descripcion || 'sin catálogo') + ' (' + o.Claves + ')';
                $sel.append($('<option>').val(o.Valor).text(t));
            });
        });
    }

    function cargarRegiones() {
        $.getJSON(API + 'filtros/regiones').done(function (lista) {
            var $r = $('#fRegion');
            $.each(lista || [], function (_, r) { $r.append($('<option>').val(r).text(r)); });
        });
    }

    function filtroSegmentos() {
        var partes = [];
        $('.filtro-seg').each(function () {
            if ($(this).val()) partes.push($(this).data('abrev') + ':' + $(this).val());
        });
        return partes.join('|');
    }

    // ------------------------------------------------------------ matriz
    function celdaSegmento(abrev) {
        return function (_, type, row) {
            var s = row.Segmentos && row.Segmentos[abrev];
            if (!s) return '';
            if (type !== 'display') return s.Valor;
            var d = s.SinCatalogo ? '<span class="seg-d sin-cat">sin catálogo</span>'
                                  : '<span class="seg-d" title="' + esc(s.Descripcion || '') + '">' + esc(s.Descripcion || '') + '</span>';
            return '<span class="seg-v">' + esc(s.Valor) + '</span>' + d;
        };
    }
    function celdaMoneda(v, type) { return type === 'display' ? moneda(v) : v; }

    function iniciarTabla() {
        tabla = $('#tablaPresupuesto').DataTable({
            serverSide: true,
            processing: true,
            deferLoading: 0,
            ajax: {
                url: API + 'matriz/paginado',
                type: 'GET',
                data: function (d) {
                    // Solo lo que el servidor usa (evita URLs enormes)
                    var o = { draw: d.draw, start: d.start, length: d.length, 'search[value]': d.search.value,
                              anio: anio(), seg: filtroSegmentos(), region: $('#fRegion').val(), estado: $('#fEstado').val() };
                    if (d.order && d.order.length) {
                        o['order[0][column]'] = d.order[0].column;
                        o['order[0][dir]'] = d.order[0].dir;
                        o['columns[' + d.order[0].column + '][name]'] = d.columns[d.order[0].column].name;
                    }
                    return o;
                },
                dataSrc: function (json) {
                    var t = json.totales || {};
                    $('#tClaves').text((t.Claves || 0).toLocaleString('es-MX'));
                    $('#tAprobado').text(moneda(t.Aprobado));
                    $('#tModificado').text(moneda(t.Modificado));
                    $('#tLiberado').text(moneda(t.Liberado));
                    $('#tPorLiberar').text(moneda(t.PorLiberar));
                    return json.data;
                },
                error: function (xhr) { aviso('Consulta', esc(mensajeError(xhr)), 'error'); }
            },
            dom: '<"d-flex justify-content-end mb-3"f>rt<"d-flex justify-content-between align-items-center mt-3 flex-wrap"lip>',
            columns: [
                { data: 'ClavePresupuestal', name: 'clave', render: function (v, type) { return type === 'display' ? '<span class="clave-txt">' + esc(v) + '</span>' : v; } },
                { data: null, name: 'seg:UR', render: celdaSegmento('UR') },
                { data: null, name: 'seg:PP', render: celdaSegmento('PP') },
                { data: null, name: 'seg:OG', render: celdaSegmento('OG') },
                { data: null, name: 'seg:MPIO', render: celdaSegmento('MPIO') },
                { data: 'Aprobado', name: 'aprobado', className: 'num', render: celdaMoneda },
                { data: 'Modificado', name: 'modificado', className: 'num', render: celdaMoneda },
                { data: 'Liberado', name: 'liberado', className: 'num', render: celdaMoneda },
                { data: 'PorLiberar', name: 'porliberar', className: 'num', render: celdaMoneda }
            ],
            order: [[0, 'asc']],
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.4/i18n/es-MX.json',
                search: '', searchPlaceholder: 'Buscar en la clave...'
            },
            pageLength: 10
        });

        $('#tablaPresupuesto tbody').on('click', 'tr', function () {
            var data = tabla.row(this).data();
            if (data) window.location.href = 'DetalleClave.aspx?clave=' + encodeURIComponent(data.ClavePresupuestal);
        });
    }

    function recargar() { if (tabla && anio()) tabla.ajax.reload(); }

    // ------------------------------------------------------------ valores sin catálogo
    var sinCatalogo = [];
    function revisarSinCatalogo() {
        if (!anio()) return;
        $.getJSON(API + 'sincatalogo', { anio: anio() }).done(function (lista) {
            sinCatalogo = lista || [];
            if (sinCatalogo.length) {
                $('#lblSinCatalogo').text(sinCatalogo.length + ' valores sin catálogo');
                $('#btnSinCatalogo').show();
            } else {
                $('#btnSinCatalogo').hide();
            }
            pintarSinCatalogo();
        });
    }

    function pintarSinCatalogo() {
        var $tb = $('#tblSinCatalogo tbody').empty();
        $('#lblAnioSinCat').text(anio());
        if (!sinCatalogo.length) {
            $tb.append('<tr><td colspan="6" class="text-center text-success">Todos los valores del ejercicio están en su catálogo.</td></tr>');
            return;
        }
        $.each(sinCatalogo, function (_, v) {
            var padre = v.SegmentoPadre ? '<b>' + esc(v.SegmentoPadre) + '</b> ' + esc(v.ValorPadre || '') : '—';
            $tb.append('<tr><td><b>' + esc(v.Abreviatura) + '</b> ' + esc(v.Nombre || '') + '</td>' +
                '<td><b class="clave-mini">' + esc(v.Valor) + '</b></td><td>' + padre + '</td>' +
                '<td><code>' + esc(v.TablaCatalogo) + '</code></td><td class="num">' + v.Claves + '</td>' +
                '<td class="clave-mini">' + esc(v.EjemploClave) + '</td></tr>');
        });
    }

    function reprocesar() {
        preguntar('¿Reprocesar catálogos ' + anio() + '?',
                  'Se vuelven a buscar en los catálogos los segmentos de todas las claves del ejercicio. No se cambian importes.',
                  'Sí, reprocesar').then(function (ok) {
            if (!ok) return;
            cargando('Reprocesando…');
            $.ajax({ url: API + 'reprocesar-catalogos?anio=' + encodeURIComponent(anio()), type: 'POST' })
                .done(function (r) {
                    aviso('Catálogos reprocesados',
                        r.Claves.toLocaleString('es-MX') + ' claves revisadas, ' + r.SegmentosActualizados + ' segmentos actualizados.<br/>' +
                        'Sin catálogo: <b>' + r.SinCatalogoAntes + '</b> → <b>' + r.SinCatalogoDespues + '</b>', 'success');
                    revisarSinCatalogo();
                    recargar();
                })
                .fail(function (xhr) { aviso('No se pudo reprocesar', esc(mensajeError(xhr)), 'error'); });
        });
    }

    // ------------------------------------------------------------ alta manual
    function cargarEstructura() {
        estructura = []; longitudClave = 0;
        $('#contenedorSegmentos').empty();
        $('#lblEjercicioAlta').text(anio() || '');
        if (!anio()) return;
        $.getJSON('api/presupuestoinicial/estructura', { anio: anio() }).done(function (segs) {
            estructura = segs || [];
            longitudClave = estructura.length ? estructura[0].LongitudClave : 0;
            $('#txtClaveManual').attr('maxlength', longitudClave || 80);
            $('#lblLongitud').text(longitudClave ? '(' + longitudClave + ' caracteres en ' + anio() + ')' : '(sin estructura para ' + anio() + ')');
            var html = '';
            $.each(estructura, function (_, s) {
                html += '<div class="seg-box"><label title="' + esc(s.Nombre) + '">' + esc(s.Abreviatura) + '</label>' +
                        '<input type="text" class="form-control input-segmento" maxlength="' + s.Longitud + '" data-length="' + s.Longitud + '"' +
                        ' title="' + esc(s.Nombre) + '" style="width:' + (s.Longitud * 11 + 18) + 'px" /></div>';
            });
            $('#contenedorSegmentos').html(html);
            sincronizarSegmentos();
        });
    }

    function contar() {
        var n = $('#txtClaveManual').val().length;
        var ok = longitudClave && n === longitudClave;
        $('#lblConteo').html(longitudClave ? '<span class="' + (ok ? 'text-success' : 'text-muted') + '">' + n + ' de ' + longitudClave + ' caracteres</span>' : '');
    }

    function sincronizarSegmentos() {
        var clave = $('#txtClaveManual').val().toUpperCase().replace(/\s+/g, '');
        $('#txtClaveManual').val(clave);
        var pos = 0;
        $('.input-segmento').each(function () {
            var len = parseInt($(this).data('length'), 10);
            $(this).val(clave.substring(pos, pos + len));
            pos += len;
        });
        contar();
    }

    function sincronizarDesdeSegmentos() {
        var clave = '';
        $('.input-segmento').each(function () {
            var v = $(this).val().toUpperCase().replace(/\s+/g, '');
            $(this).val(v);
            clave += v;
        });
        $('#txtClaveManual').val(clave);
        contar();
    }

    function decodificar() {
        var clave = $('#txtClaveManual').val();
        if (!longitudClave || clave.length !== longitudClave) {
            return aviso('Clave incompleta', 'Para ' + anio() + ' la clave debe tener ' + longitudClave + ' caracteres; tiene ' + clave.length + '.', 'warning');
        }
        $.getJSON(API + 'decodificar', { clave: clave }).done(function (res) {
            var tb = '';
            $.each(res.Segmentos || [], function (_, s) {
                var falta = s.Descripcion === 'No existe en el catálogo';
                tb += '<tr><td class="ps-3"><b>' + esc(s.Abreviatura) + '</b> <span class="text-secondary">' + esc(s.Nombre) + '</span></td>' +
                      '<td class="text-center"><b>' + esc(s.Valor) + '</b></td>' +
                      '<td' + (falta ? ' class="sin-cat"' : '') + '>' + esc(s.Descripcion) + '</td></tr>';
            });
            if (!tb) tb = '<tr><td colspan="3" class="text-center text-muted">No hay estructura configurada para esta clave.</td></tr>';
            $('#tablaDecodificacion tbody').html(tb);
            $('#resultadoDecodificacion').show();
        }).fail(function (xhr) { aviso('No se pudo decodificar', esc(mensajeError(xhr)), 'error'); });
    }

    function guardarClave() {
        var clave = $('#txtClaveManual').val();
        var aprobado = parseFloat($('#txtAprobadoManual').val()) || 0;
        if (!longitudClave || clave.length !== longitudClave)
            return aviso('Clave incompleta', 'Para ' + anio() + ' la clave debe tener ' + longitudClave + ' caracteres.', 'warning');
        if (aprobado <= 0) return aviso('Monto', 'El monto aprobado debe ser mayor a 0.', 'warning');

        preguntar('¿Agregar la clave al ejercicio ' + anio() + '?',
                  '<span class="clave-txt">' + esc(clave) + '</span><br/>Aprobado <b>' + moneda(aprobado) + '</b>, todo por liberar.',
                  'Sí, agregar').then(function (ok) {
            if (!ok) return;
            $.ajax({
                url: API + 'nueva', type: 'POST', contentType: 'application/json',
                data: JSON.stringify({ Anio: parseInt(anio(), 10), ClavePresupuestal: clave, Aprobado: aprobado, Oficio: $('#txtOficioManual').val() })
            }).done(function (r) {
                var extra = r.sinCatalogo && r.sinCatalogo.length
                    ? '<br/><span class="sin-cat">Sin catálogo: ' + esc(r.sinCatalogo.join(', ')) + '</span>' : '';
                aviso('Clave agregada', 'Quedó por liberar hasta que una adecuación de liberación la calendarice.' + extra, 'success');
                $('#txtClaveManual, #txtAprobadoManual, #txtOficioManual').val('');
                sincronizarSegmentos();
                $('#resultadoDecodificacion').hide();
                recargar();
                revisarSinCatalogo();
                $('.filtro-seg').each(function () { cargarOpcionesSegmento($(this)); });
            }).fail(function (xhr) { aviso('No se guardó', esc(mensajeError(xhr)), 'error'); });
        });
    }

    // ------------------------------------------------------------ inicio
    function cambiarEjercicio() {
        try { sessionStorage.setItem('siap.ejercicio', anio()); } catch (e) { }
        $('.filtro-seg').each(function () { cargarOpcionesSegmento($(this)); });
        cargarEstructura();
        revisarSinCatalogo();
        recargar();
    }

    $(function () {
        iniciarTabla();
        cargarRegiones();
        cargarEjercicios().always(cambiarEjercicio);

        $('#cboEjercicio').on('change', cambiarEjercicio);
        $('#fEstado, #fRegion, .filtro-seg').on('change', recargar);
        $('#btnLimpiarFiltros').on('click', function () {
            $('#fEstado, #fRegion, .filtro-seg').val('');
            tabla.search('');
            recargar();
        });
        $('#btnSinCatalogo').on('click', function () { new bootstrap.Modal(document.getElementById('modalSinCatalogo')).show(); });
        $('#btnReprocesar').on('click', reprocesar);

        $('#txtClaveManual').on('input', sincronizarSegmentos);
        $('#contenedorSegmentos').on('input', '.input-segmento', function () {
            sincronizarDesdeSegmentos();
            var len = parseInt($(this).data('length'), 10);
            if ($(this).val().length >= len) $(this).parent().next().find('input').focus();
        });
        $('#btnDecodificar').on('click', decodificar);
        $('#btnGuardarClave').on('click', guardarClave);
    });
})();
