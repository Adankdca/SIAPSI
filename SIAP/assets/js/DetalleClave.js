/* Detalle de una clave presupuestal.
   Todo sale de api/presupuesto/detalle-clave, que ya entrega los importes cuadrados:
     Modificado = Aprobado + Aumentos − Disminuciones
     Modificado = Ministrado + Apartado + Disponible + Por liberar                      */
(function () {
    'use strict';
    var fmt = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
    function moneda(v) { return fmt.format(v || 0); }
    function esc(s) { return $('<div>').text(s == null ? '' : String(s)).html(); }
    function pct(v, total) { return total > 0 ? Math.round((v / total) * 1000) / 10 : 0; }
    var MESES = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    // Segmentos que identifican la clave a simple vista (en este orden)
    var CHIPS = [['UR', 'Unidad responsable'], ['PP', 'Programa'], ['PT', 'Proyecto'], ['OG', 'Partida'],
                 ['MPIO', 'Municipio'], ['FF', 'Fuente'], ['PF', 'Programa fondo']];

    function textoSegmento(s) {
        if (!s) return '';
        if (s.SinCatalogo) return s.Valor + ' · <span class="sin-cat">sin catálogo</span>';
        return esc(s.Valor) + (s.Descripcion ? ' · ' + esc(s.Descripcion) : '');
    }

    function pintarEncabezado(d) {
        $('#lblClave').text(d.ClavePresupuestal);
        $('#lblClavePrincipal').text('Clave ' + d.ClavePresupuestal);
        $('#lblEjercicio').text(d.Ejercicio || '—');
        pintarObra(d);
        var html = '';
        $.each(CHIPS, function (_, c) {
            var s = d.Segmentos && d.Segmentos[c[0]];
            if (!s) return;
            html += '<div class="chip" title="' + esc(c[1] + ' ' + s.Valor + (s.Descripcion ? ' · ' + s.Descripcion : '')) + '"><b>' + esc(c[1]) + '</b><span>' + textoSegmento(s) + '</span></div>';
        });
        $('#chips').html(html);
    }

    // Obra en BDSICOFIN: el proyecto y sus acciones (una clave puede tener varias: obra, supervisión...)
    function pintarObra(d) {
        var $b = $('#bloqueObra');
        var obras = d.Obras || [];
        var og = d.Segmentos && d.Segmentos.OG ? d.Segmentos.OG.Valor : '';
        var esObra = /^6/.test(og); // capítulo 6000: inversión pública
        if (obras.length) {
            var porProyecto = {};
            var orden = [];
            $.each(obras, function (_, o) {
                var k = o.Proyecto || '(sin nombre de proyecto)';
                if (!porProyecto[k]) { porProyecto[k] = []; orden.push(k); }
                porProyecto[k].push(o);
            });
            var h = '<div class="etq-obra">Obra · BDSICOFIN</div>';
            $.each(orden, function (_, k) {
                h += '<div class="proy">' + esc(k) + '</div><ul>';
                $.each(porProyecto[k], function (_, o) {
                    var lugar = [o.Municipio, o.Localidad].filter(function (x) { return x; }).join(', ');
                    h += '<li><b>' + esc(o.Accion || o.TipoAccion || 'Acción') + '</b>' +
                         (lugar ? ' · ' + esc(lugar) : '') +
                         (o.Estatus ? ' · <span class="badge bg-secondary badge-est">' + esc(o.Estatus) + '</span>' : '') +
                         (o.IdAccion ? ' <span class="text-xs text-muted">(' + esc(o.IdAccion) + ')</span>' : '') + '</li>';
                });
                h += '</ul>';
            });
            $b.html(h).show();
        } else if (d.AvisoSicofin && esObra) {
            $b.html('<div class="etq-obra">Obra · BDSICOFIN</div><div class="nota">' + esc(d.AvisoSicofin) + '</div>').show();
        } else if (esObra && d.ClaveCortaSicofin) {
            $b.html('<div class="etq-obra">Obra · BDSICOFIN</div><div class="nota">No se encontró la obra en BDSICOFIN con la clave ' +
                    esc(d.ClaveCortaSicofin) + '.</div>').show();
        } else {
            $b.hide();
        }
    }

    function pintarIndicadores(d) {
        var mes = d.Meses[d.MesActual - 1] || { Disponible: 0, Calendarizado: 0, Ministrado: 0 };
        $('#lblMesActual').text(MESES[d.MesActual - 1].toLowerCase());
        $('#kpiDisponibleMes').text(moneda(mes.Disponible));
        $('#subDisponibleMes').text('Calendarizado ' + moneda(mes.Calendarizado) + ' − ministrado ' + moneda(mes.Ministrado) +
            (d.Apartado > 0 ? '. Ojo: hay ' + moneda(d.Apartado) + ' apartados sin mes asignado.' : ''));
        $('#kpiDisponible').text(moneda(d.Disponible));
        $('#kpiModificado').text(moneda(d.Modificado));
        $('#subModificado').text('Aprobado ' + moneda(d.Aprobado) + (d.Aumentos ? ' + aumentos ' + moneda(d.Aumentos) : '') + (d.Disminuciones ? ' − disminuciones ' + moneda(d.Disminuciones) : ''));
        $('#kpiPorLiberar').text(moneda(d.PorLiberar));
        $('#lblDevengado').text(moneda(d.Devengado));
        $('#lblPagado').text(moneda(d.Pagado));

        var alertas = '';
        if (d.Disponible < 0)
            alertas += '<div class="alert alert-danger text-white py-2"><i class="fa-solid fa-triangle-exclamation me-2"></i>Lo ministrado y apartado (' + moneda(d.Ministrado + d.Apartado) + ') supera lo liberado (' + moneda(d.Liberado) + ').</div>';
        if (d.PorLiberar < 0)
            alertas += '<div class="alert alert-danger text-white py-2"><i class="fa-solid fa-triangle-exclamation me-2"></i>Los meses calendarizados (' + moneda(d.Liberado) + ') suman más que el modificado (' + moneda(d.Modificado) + ').</div>';
        if (d.Apartado > 0)
            alertas += '<div class="alert alert-warning text-white py-2"><i class="fa-solid fa-hourglass-half me-2"></i>Hay ' + moneda(d.Apartado) + ' apartados por adecuaciones en trámite; no están disponibles hasta que se registren o se cancelen.</div>';
        $('#alertas').html(alertas);
    }

    function pintarModificado(d) {
        // Puente: de dónde sale el modificado
        var p = '<span>Aprobado</span> <span class="v">' + moneda(d.Aprobado) + '</span>';
        p += ' <span class="op">+</span> <span>aumentos</span> <span class="v text-success">' + moneda(d.Aumentos) + '</span>';
        p += ' <span class="op">−</span> <span>disminuciones</span> <span class="v text-danger">' + moneda(d.Disminuciones) + '</span>';
        p += ' <span class="op">=</span> <span>modificado</span> <span class="v">' + moneda(d.Modificado) + '</span>';
        $('#puente').html(p);

        // Barra: cómo está repartido (los negativos no se dibujan)
        var partes = [
            ['c-min', 'Ministrado', Math.max(d.Ministrado, 0), '#lgMin', '#pcMin'],
            ['c-apa', 'Apartado', Math.max(d.Apartado, 0), '#lgApa', '#pcApa'],
            ['c-dis', 'Disponible', Math.max(d.Disponible, 0), '#lgDis', '#pcDis'],
            ['c-lib', 'Por liberar', Math.max(d.PorLiberar, 0), '#lgLib', '#pcLib']
        ];
        var total = partes.reduce(function (a, x) { return a + x[2]; }, 0);
        var html = '';
        $.each(partes, function (_, x) {
            var w = pct(x[2], total);
            if (w > 0) html += '<div class="' + x[0] + '" style="width:' + w + '%" title="' + x[1] + ': ' + moneda(x[2]) + ' (' + w + ' %)"></div>';
            $(x[3]).text(moneda(x[2]));
            $(x[4]).text(pct(x[2], d.Modificado) + ' % del modificado');
        });
        $('#barraModificado').html(html || '<div style="width:100%" title="Sin importe"></div>');
    }

    function pintarMeses(d) {
        var tb = '', tc = 0, tm = 0, td = 0;
        $.each(d.Meses, function (_, m) {
            tc += m.Calendarizado; tm += m.Ministrado; td += m.Disponible;
            var avance = m.Calendarizado > 0 ? Math.min(100, pct(m.Ministrado, m.Calendarizado)) : 0;
            var vacio = m.Calendarizado === 0 && m.Ministrado === 0;
            tb += '<tr class="' + (m.EsMesActual ? 'actual' : '') + (vacio ? ' text-muted' : '') + '">' +
                  '<td>' + esc(m.Nombre) + (m.EsMesActual ? ' <span class="badge bg-gradient-success badge-est ms-1">mes en curso</span>' : '') + '</td>' +
                  '<td class="num">' + moneda(m.Calendarizado) + '</td>' +
                  '<td class="num">' + moneda(m.Ministrado) + '</td>' +
                  '<td class="num' + (m.Disponible < 0 ? ' text-danger' : '') + '">' + moneda(m.Disponible) + '</td>' +
                  '<td><div class="mini" title="' + avance + ' % ministrado"><div style="width:' + avance + '%"></div></div></td></tr>';
        });
        $('#tblMeses tbody').html(tb);
        $('#tblMeses tfoot').html('<tr class="font-weight-bold"><td>Total liberado</td><td class="num">' + moneda(tc) + '</td><td class="num">' + moneda(tm) + '</td><td class="num">' + moneda(td) + '</td><td></td></tr>' +
            '<tr class="text-muted"><td>Por liberar (mes cero)</td><td class="num">' + moneda(d.PorLiberar) + '</td><td></td><td></td><td></td></tr>');
    }

    function pintarApartados(d) {
        if (!d.Apartados || !d.Apartados.length) { $('#cardApartados').hide(); return; }
        var h = '';
        $.each(d.Apartados, function (_, a) {
            var f = a.FechaApartado ? new Date(a.FechaApartado).toLocaleDateString('es-MX') : '';
            h += '<div class="d-flex justify-content-between border-bottom py-2"><div><div class="text-sm font-weight-bold">' + esc(a.Oficio || 'Sin oficio') + '</div>' +
                 '<div class="text-xs text-muted">' + esc(a.Motivo || '') + (f ? ' · ' + f : '') + '</div></div>' +
                 '<div class="num font-weight-bold" style="color:#E39B23;">' + moneda(a.Monto) + '</div></div>';
        });
        $('#listaApartados').html(h);
        $('#cardApartados').show();
    }

    function pintarMinistraciones(d) {
        var tb = '';
        $.each(d.Ministraciones || [], function (_, m) {
            var cancelada = (m.Estatus || '').toUpperCase() === 'CANCELADA';
            tb += '<tr' + (cancelada ? ' class="text-muted"' : '') + '><td>' + esc(m.Folio) + '</td><td>' + esc(MESES[m.Mes - 1] || m.Mes) + '</td>' +
                  '<td class="num"' + (cancelada ? ' style="text-decoration:line-through"' : '') + '>' + moneda(m.Importe) + '</td>' +
                  '<td><span class="badge badge-est ' + (cancelada ? 'bg-secondary' : 'bg-gradient-success') + '">' + esc(m.Estatus) + '</span></td></tr>';
        });
        $('#tblMinistraciones tbody').html(tb || '<tr><td colspan="4" class="text-center text-muted py-3">Aún no hay ministraciones de esta clave.</td></tr>');
    }

    function pintarProyecto(d) {
        var otras = d.ClavesDelProyecto || [];
        $('#lblNumProyecto').text('(' + otras.length + ')');
        $('#lblPrefijo').text(d.PrefijoProyecto ? 'Claves del ejercicio con los mismos segmentos de UR a Proyecto: ' + d.PrefijoProyecto : '');
        if (!otras.length) {
            $('#tbodyProyecto').html('<tr><td colspan="6" class="text-center text-muted py-3">Es la única clave de este proyecto.</td></tr>');
            return;
        }
        var tb = '';
        $.each(otras, function (_, c) {
            tb += '<tr><td>' + textoSegmento(c.Segmentos.OG) + '</td><td>' + textoSegmento(c.Segmentos.MPIO) + '</td><td>' + textoSegmento(c.Segmentos.PF) + '</td>' +
                  '<td class="num">' + moneda(c.Modificado) + '</td><td class="num">' + moneda(c.PorLiberar) + '</td>' +
                  '<td class="text-end"><a class="btn btn-outline-primary btn-sm mb-0 py-1 px-2" href="DetalleClave.aspx?clave=' + encodeURIComponent(c.ClavePresupuestal) + '" title="' + esc(c.ClavePresupuestal) + '">Ver</a></td></tr>';
        });
        $('#tbodyProyecto').html(tb);
    }

    function cargarDesglose(clave) {
        $.getJSON('api/presupuesto/decodificar', { clave: clave }).done(function (res) {
            var html = '';
            $.each((res && res.Segmentos) || [], function (_, s) {
                var falta = s.Descripcion === 'No existe en el catálogo';
                html += '<tr><td>' + esc(s.Nombre) + '</td><td class="text-secondary">' + esc(s.Abreviatura) + '</td><td class="font-weight-bold">' + esc(s.Valor) + '</td>' +
                        '<td' + (falta ? ' style="color:#b7791f"' : '') + '>' + esc(s.Descripcion) + '</td></tr>';
            });
            $('#contenidoDesglose').html(html);
            $('#lblNumSegmentos').text(res && res.Segmentos ? '(' + res.Segmentos.length + ' segmentos)' : '');
        });
    }

    function cargarAdecuaciones(clave) {
        $.getJSON('api/adecuaciones/GetPorClave/' + encodeURIComponent(clave)).done(function (res) {
            var html = '';
            $.each(res || [], function (_, it) {
                var f = it.FechaSolicitud && it.FechaSolicitud.indexOf('0001-01-01') !== 0 ? new Date(it.FechaSolicitud).toLocaleDateString('es-MX') : '';
                var link = it.NumOficio ? 'DetalleOficio.aspx?oficio=' + encodeURIComponent(it.NumOficio) : 'Adecuaciones.aspx';
                html += '<tr><td class="font-weight-bold">' + esc(it.NumOficio || 'Sin oficio') + '</td><td>' + f + '</td><td>' + esc(it.TipoOperacion || '') + '</td>' +
                        '<td class="num">' + moneda(it.Total) + '</td><td class="text-end"><a class="btn btn-outline-primary btn-sm mb-0 py-1 px-2" href="' + link + '">Ver oficio</a></td></tr>';
            });
            $('#tbodyHistorialAdec').html(html || '<tr><td colspan="5" class="text-center text-muted py-3">Aún no hay adecuaciones de esta clave.</td></tr>');
        }).fail(function () {
            $('#tbodyHistorialAdec').html('<tr><td colspan="5" class="text-center text-danger py-3">No se pudo cargar el historial.</td></tr>');
        });
    }

    $(function () {
        var clave = new URLSearchParams(window.location.search).get('clave');
        if (!clave) { $('#lblClave').text('No se indicó la clave.'); return; }
        $.getJSON('api/presupuesto/detalle-clave', { clave: clave }).done(function (d) {
            pintarEncabezado(d);
            pintarIndicadores(d);
            pintarModificado(d);
            pintarMeses(d);
            pintarApartados(d);
            pintarMinistraciones(d);
            pintarProyecto(d);
        }).fail(function (xhr) {
            $('#lblClave').text(xhr.status === 404 ? 'La clave no existe en el presupuesto.' : 'No se pudo cargar la clave.');
        });
        cargarDesglose(clave);
        cargarAdecuaciones(clave);
    });
})();
