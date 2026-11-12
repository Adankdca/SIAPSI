$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const oficioParam = urlParams.get('oficio');

    if (!oficioParam) {
        window.location.href = "Adecuaciones.aspx";
        return;
    }

    $('#oficioTitle').text(oficioParam);

    const encodedOficio = encodeURIComponent(oficioParam);
    const apiUrl = `api/Adecuaciones/Oficio/${encodedOficio}`;

    const pdfUrl = `Uploads/Oficios/Adecuaciones/${oficioParam.replace(/[\/\\?%*:|"<>\s]/g, '_')}.pdf`;
    
    // Verificar si el PDF existe
    $.ajax({
        url: pdfUrl,
        type: 'HEAD',
        success: function() {
            $('#btnViewPDF').show().on('click', function() {
                window.open(pdfUrl, '_blank');
            });
        }
    });

    fetch(apiUrl)
        .then(response => response.json())
        .then(data => {
            const tbody = document.querySelector('#tablaMovimientosOficio tbody');
            
            if (data.length === 0) {
                tbody.innerHTML = '<tr><td colspan="4" class="text-center">No se encontraron adecuaciones para este oficio.</td></tr>';
                return;
            }

            let html = '';
            let totalAmpliaciones = 0;
            let totalReducciones = 0;
            let count = 1;
            
            let globalEstatus = "N/A";
            let globalFecha = "N/A";
            let globalSolicita = "N/A";
            let globalAutoriza = "N/A";

            function getTipoBadge(tipo) {
                if (!tipo) return '<span class="badge bg-gradient-secondary">Desconocido</span>';
                let t = tipo.toString().trim().toUpperCase();
                
                // Mapeo según cat_tipoadecuacion
                const catalog = {
                    '1': { desc: 'INCREMENTOS', class: 'bg-gradient-success' },
                    '2': { desc: 'DECREMENTOS', class: 'bg-gradient-danger' },
                    '3': { desc: 'AMPLIACIÓN', class: 'bg-gradient-success' },
                    '4': { desc: 'REDUCCIÓN', class: 'bg-gradient-danger' },
                    '5': { desc: 'LIBERACIÓN', class: 'bg-gradient-info' },
                    '6': { desc: 'RETENCIÓN', class: 'bg-gradient-warning' },
                    '7': { desc: 'RECALENDARIZACIÓN', class: 'bg-gradient-primary' },
                    '8': { desc: 'TRASPASO', class: 'bg-gradient-secondary' }
                };

                // Si no es un número directo, intentamos deducir el ID del catálogo por el texto
                let mappedId = t;
                if (!catalog[t]) {
                    if (t.includes('AMPLIACI')) mappedId = '3';
                    else if (t.includes('REDUCCI')) mappedId = '4';
                    else if (t.includes('INC') || t.includes('TRASPASO INCREMENTO')) mappedId = '1';
                    else if (t.includes('DEC') || t.includes('TRASPASO DECREMENTO')) mappedId = '2';
                    else if (t.includes('LIBERACI')) mappedId = '5';
                    else if (t.includes('RETENCI')) mappedId = '6';
                    else if (t.includes('RECALENDARIZACI')) mappedId = '7';
                    else if (t.includes('TRASPASO')) mappedId = '8';
                }

                if (catalog[mappedId]) {
                    return `<span class="badge ${catalog[mappedId].class} text-xxs">${mappedId} - ${catalog[mappedId].desc}</span>`;
                }

                // Fallback si no logramos mapearlo a nada
                if (t.includes('AMP') || t.includes('INC')) {
                    return `<span class="badge bg-gradient-success text-xxs">${t}</span>`;
                } else if (t.includes('RED') || t.includes('DEC')) {
                    return `<span class="badge bg-gradient-danger text-xxs">${t}</span>`;
                }

                return `<span class="badge bg-gradient-secondary text-xxs">${t}</span>`;
            }

            data.forEach(item => {
                // Formatear Fecha
                let fecha = item.fechasolicitud;
                if (!fecha || fecha === "0001-01-01T00:00:00") {
                    fecha = "N/A";
                } else {
                    let parts = fecha.split('T')[0].split('-');
                    fecha = parts[2] + "-" + parts[1] + "-" + parts[0];
                }
                if (count === 1) {
                    globalEstatus = item.estatus;
                    globalFecha = fecha;
                    globalSolicita = item.solicita || item.elaboro || "N/A";
                    globalAutoriza = item.autoriza || item.autorizo || "N/A";
                }

                // Aquí "montoadecuacion" en la API puede ser el total, y tendríamos que ver si es reducción o ampliación
                // Asumimos que viene "receptor" para ampliación y "emisor" para reducción, pero la API actual trae "montoadecuacion"
                // Si trae clave, mostrar la clave, de lo contrario mostrar ID Solicitud
                let descripcionClave = item.clave ? item.clave : `ID Solicitud: ${item.numsolicitud || 'N/A'}`;
                
                let monto = item.montoadecuacion || 0;
                let numMonto = parseFloat(monto);
                
                let isAmpliacion = false;
                let isReduccion = false;
                let t = (item.tipooperacion || "").toString().toLowerCase();
                if (t === '1' || t === '3' || t.includes('amp')) isAmpliacion = true;
                if (t === '2' || t === '4' || t.includes('red')) isReduccion = true;
                
                if (isAmpliacion) totalAmpliaciones += numMonto;
                else if (isReduccion) totalReducciones += Math.abs(numMonto);
                else if (numMonto > 0) totalAmpliaciones += numMonto; // fallback
                
                let isNeg = numMonto < 0;
                let absMonto = Math.abs(numMonto);
                let strMonto = (isNeg ? "-$" : "$") + absMonto.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                
                let tipoBadge = getTipoBadge(item.tipooperacion);
                let btnClave = item.clave ? `<a href="DetalleClave.aspx?clave=${encodeURIComponent(item.clave)}" class="btn btn-sm btn-primary mb-0" style="padding: 0.5rem 1rem;">Ver Clave</a>` : '';

                // Construir fila de tabla
                html += `
                <tr>
                    <td class="align-middle text-center text-secondary text-sm">
                        <span class="text-secondary text-sm">${count}</span>
                    </td>
                    <td class="align-middle">
                        <div class="d-flex px-2 py-1 align-items-center">
                            <span class="text-sm font-weight-bold">${descripcionClave}</span>
                        </div>
                    </td>
                    <td class="align-middle text-center">
                        ${tipoBadge}
                    </td>
                    <td class="align-middle text-center text-sm">
                        <span class="text-secondary text-sm font-weight-bold">${strMonto}</span>
                    </td>
                    <td class="align-middle text-center">
                        ${btnClave}
                    </td>
                </tr>
                `;
                count++;
            });
            
            tbody.innerHTML = html;
            
            document.getElementById('docOficio').innerText = oficioParam;
            document.getElementById('docEstatus').innerText = globalEstatus;
            document.getElementById('docFecha').innerText = globalFecha;
            document.getElementById('docSolicita').innerText = globalSolicita;
            document.getElementById('docAutoriza').innerText = globalAutoriza;
            
            let finalTotal = Math.max(totalAmpliaciones, totalReducciones);
            if (finalTotal === 0 && (totalAmpliaciones > 0 || totalReducciones > 0)) {
                 finalTotal = totalAmpliaciones > 0 ? totalAmpliaciones : totalReducciones;
            }
            
            let strTotal = "$" + finalTotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
            $("#sumMonto").text(strTotal);
            $("#docTotal").text(strTotal);
            
            // Initialize DataTables for pagination
            if ($.fn.DataTable.isDataTable('#tablaMovimientosOficio')) {
                $('#tablaMovimientosOficio').DataTable().destroy();
            }
            $('#tablaMovimientosOficio').DataTable({
                "pageLength": 10,
                "lengthMenu": [5, 10, 25, 50],
                "dom": '<"d-flex justify-content-end mb-3"f>rt<"d-flex justify-content-between align-items-center mt-3"lip>',
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.13.4/i18n/es-MX.json",
                    "search": "",
                    "searchPlaceholder": "Buscar...",
                    "paginate": {
                        "previous": '<i class="fa-solid fa-chevron-left"></i>',
                        "next": '<i class="fa-solid fa-chevron-right"></i>'
                    }
                },
                "ordering": false
            });
        })
        .catch(error => {
            console.error('Error fetching adecuaciones:', error);
            document.querySelector('#tablaMovimientosOficio tbody').innerHTML = '<tr><td colspan="4" class="text-center text-danger">Error al cargar la información.</td></tr>';
        });
});
