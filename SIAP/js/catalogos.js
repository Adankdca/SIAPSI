let currentTable = '';
let currentSchema = [];
let dataTable = null;
let fkData = {}; // Cache for foreign key options

$(document).ready(function() {
    loadCatalogs();

    // Event handler for custom selects (main catalog and modal dropdowns)
    $(document).on('click', '.custom-select-item', function(e) {
        e.preventDefault();
        let $menu = $(this).closest('.custom-select-menu');
        let $container = $menu.closest('.dropdown');
        let $btnText = $container.find('button span.selected-text');
        let $hiddenInput = $container.find('input[type="hidden"]');

        $menu.find('.custom-select-item').removeClass('active');
        $(this).addClass('active');

        $btnText.text($(this).text());
        $hiddenInput.val($(this).data('value'));

        $hiddenInput.trigger('change');
    });

    // When the main catalog select changes
    $('#selectCatalogo').on('change', function() {
        const tableName = $(this).val();
        if(tableName) {
            $('#dashboardCatalogos').addClass('d-none');
            $('#btnVolverDashboard').removeClass('d-none');
            $('#tableContainer').removeClass('d-none');
            loadCatalogData(tableName);
        } else {
            volverDashboard();
        }
    });
});

function volverDashboard() {
    $('#selectCatalogo').val('');
    $('#selectCatalogoText').text('-- Seleccione un Catálogo --');
    $('#btnAgregar').addClass('d-none');
    $('#dashboardCatalogos').removeClass('d-none');
    $('#btnVolverDashboard').addClass('d-none');
    $('#tableContainer').addClass('d-none');
    if(dataTable) {
        dataTable.clear().destroy();
        $('#tablaHeaders').empty();
        $('#tablaCatalogos tbody').empty();
        dataTable = null;
    }
}

function formatCatalogName(rawName) {
    const map = {
        'cat_aniogasto': 'AÑO DE GASTO',
        'aniogasto': 'AÑO DE GASTO',
        'cat_digitosministracion': 'DÍGITOS DE MINISTRACIÓN',
        'digitoministracion': 'DÍGITOS DE MINISTRACIÓN',
        'digitosministracion': 'DÍGITOS DE MINISTRACIÓN',
        'cat_actividadinstitucional': 'ACTIVIDAD INSTITUCIONAL',
        'actividadinstitucional': 'ACTIVIDAD INSTITUCIONAL',
        'cat_asignaciongasto': 'ASIGNACIÓN DE GASTO',
        'asignaciongasto': 'ASIGNACIÓN DE GASTO',
        'cat_capitulogasto': 'CAPÍTULO DE GASTO',
        'capitulogasto': 'CAPÍTULO DE GASTO',
        'cat_clasificacionadministrativa': 'CLASIFICACIÓN ADMINISTRATIVA',
        'clasificacionadministrativa': 'CLASIFICACIÓN ADMINISTRATIVA',
        'cat_finalidad': 'FINALIDAD',
        'finalidad': 'FINALIDAD',
        'cat_fuentefinanciamiento': 'FUENTE DE FINANCIAMIENTO',
        'fuentefinanciamiento': 'FUENTE DE FINANCIAMIENTO',
        'cat_objetogasto': 'OBJETO DE GASTO',
        'objetogasto': 'OBJETO DE GASTO',
        'cat_partidagenerica': 'PARTIDA GENÉRICA',
        'partidagenerica': 'PARTIDA GENÉRICA',
        'cat_programa': 'PROGRAMA',
        'programa': 'PROGRAMA',
        'cat_proyecto': 'PROYECTO',
        'proyecto': 'PROYECTO',
        'cat_region': 'REGIÓN',
        'region': 'REGIÓN',
        'cat_dependencia': 'DEPENDENCIA',
        'dependencia': 'DEPENDENCIA',
        'programasfondo': 'PROGRAMAS Y FONDOS',
        'subfuentesfinanciamiento': 'SUBFUENTES DE FINANCIAMIENTO',
        'tiposministracionfolio': 'TIPOS MINISTRACIÓN FOLIO',
        'tiporecurso': 'TIPO DE RECURSO',
        'proyectosestrategicos': 'PROYECTOS ESTRATÉGICOS',
        'tipoadecuacion': 'TIPO DE ADECUACIÓN',
        'adecuacionapartados': 'APARTADOS DE ADECUACIÓN',
        'estatus': 'ESTATUS',
        'ramos': 'RAMOS'
    };
    
    if (map[rawName.toLowerCase()]) {
        return map[rawName.toLowerCase()];
    }
    
    // Fallback si no está mapeado
    return rawName.replace('cat_', '').toUpperCase();
}

async function loadCatalogs() {
    try {
        const response = await fetch('/api/catalogos/list');
        const catalogs = await response.json();
        
        const menu = $('#selectCatalogoMenu');
        const dashboard = $('#dashboardCatalogos');
        
        menu.empty();
        dashboard.empty();
        
        $('#selectCatalogoText').text('-- Seleccione un Catálogo --');
        $('#selectCatalogo').val('');
        
        menu.append(`<li><a class="dropdown-item custom-select-item active" href="#" data-value="">-- Seleccione un Catálogo --</a></li>`);
        
        catalogs.forEach(c => {
            const cleanName = formatCatalogName(c);
            // Dropdown menu
            menu.append(`<li><a class="dropdown-item custom-select-item" href="#" data-value="${c}">${cleanName}</a></li>`);
            
            // Determinar icono
            let icon = "fa-table-list";
            if(c.includes("region") || c.includes("municipio")) icon = "fa-map-location-dot";
            else if(c.includes("dependencia") || c.includes("ur")) icon = "fa-building-columns";
            else if(c.includes("fondo") || c.includes("financiamiento") || c.includes("gasto")) icon = "fa-coins";
            else if(c.includes("programa") || c.includes("proyecto")) icon = "fa-diagram-project";
            
            // Dashboard card
            dashboard.append(`
                <div class="col-xl-3 col-md-4 col-sm-6 mb-4">
                    <div class="card glass-card h-100 cursor-pointer text-center" onclick="$('#selectCatalogo').val('${c}').trigger('change')" style="cursor: pointer; transition: transform 0.2s;">
                        <div class="card-body p-3">
                            <div class="icon icon-shape bg-gradient-primary shadow text-center border-radius-md mx-auto mb-3" style="width: 60px; height: 60px;">
                                <i class="fa-solid ${icon} text-white opacity-10" style="font-size: 1.5rem; line-height: 60px;"></i>
                            </div>
                            <h6 class="mb-0 font-weight-bolder text-dark">${cleanName}</h6>
                            <p class="text-xs text-muted mb-0">Gestionar tabla</p>
                        </div>
                    </div>
                </div>
            `);
        });
        
        // Add hover effect via JS/CSS class injection if needed, though inline style transition is added
        $('.glass-card').hover(
            function() { $(this).css('transform', 'translateY(-5px)'); },
            function() { $(this).css('transform', 'translateY(0)'); }
        );
        
    } catch(e) {
        console.error(e);
        Swal.fire('Error', 'No se pudieron cargar los catálogos.', 'error');
    }
}

async function loadCatalogData(tableName) {
    try {
        currentTable = tableName;
        
        // 1. Get Schema
        const schemaRes = await fetch(`/api/catalogos/${tableName}/schema`);
        currentSchema = await schemaRes.json();
        
        // 2. Pre-fetch all foreign key data
        fkData = {};
        for(let col of currentSchema) {
            if(col.IsForeignKey) {
                const fkRes = await fetch(`/api/catalogos/${col.ReferencedTable}`);
                fkData[col.ColumnName] = await fkRes.json();
            }
        }

        // 3. Get Data
        const dataRes = await fetch(`/api/catalogos/${tableName}`);
        const data = await dataRes.json();

        renderTable(data);
        $('#btnAgregar').removeClass('d-none');

    } catch(e) {
        console.error(e);
        Swal.fire('Error', 'No se pudo cargar el catálogo.', 'error');
    }
}

function renderTable(data) {
    if (dataTable) {
        dataTable.clear().destroy();
        $('#tablaCatalogos tbody').empty();
    }

    const thead = $('#tablaHeaders');
    thead.empty();
    
    let columns = [];

    // Create headers
    currentSchema.forEach(col => {
        thead.append(`<th>${col.ColumnName}</th>`);
        columns.push({
            data: col.ColumnName,
            render: function(data, type, row) {
                if(col.IsForeignKey && data != null) {
                    // Try to find the display name in the cached FK data
                    const fkOptions = fkData[col.ColumnName];
                    if(fkOptions) {
                        const match = fkOptions.find(o => o.Id == data || o.id == data || o[col.ReferencedColumn] == data);
                        if(match) {
                            // Try to guess the display column (Nombre, Descripcion, Clave)
                            return `${data} - ` + (match.Descripcion || match.Nombre || match.Clave || '');
                        }
                    }
                }
                return data;
            }
        });
    });

    // Action column
    thead.append('<th class="text-end">Acciones</th>');
    columns.push({
        data: null,
        orderable: false,
        className: 'text-end',
        render: function(data, type, row) {
            const pkCol = currentSchema.find(c => c.IsPrimaryKey);
            const pkValue = pkCol ? row[pkCol.ColumnName] : '';
            const rowJson = encodeURIComponent(JSON.stringify(row));
            return `<button class="btn btn-sm btn-primary mb-0" onclick="abrirModal(true, '${rowJson}')"><i class="fa-solid fa-pen"></i></button>`;
        }
    });

    dataTable = $('#tablaCatalogos').DataTable({
        data: data,
        columns: columns,
        dom: '<"d-flex justify-content-end mb-3"f>' +
             '<"row"<"col-sm-12"tr>>' +
             '<"row align-items-center mt-3"<"col-sm-12 col-md-4"l><"col-sm-12 col-md-4 text-center"i><"col-sm-12 col-md-4 d-flex justify-content-end"p>>',
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-MX.json',
            search: "",
            searchPlaceholder: "Buscar...",
            paginate: {
                previous: '<i class="fas fa-chevron-left"></i>',
                next: '<i class="fas fa-chevron-right"></i>'
            }
        },
        responsive: true
    });
}

function abrirModal(isEdit, rowJsonEncoded = null) {
    $('#modalTitle').text(isEdit ? 'Editar Registro' : 'Nuevo Registro');
    const formFields = $('#formFields');
    formFields.empty();

    let rowData = {};
    if(isEdit && rowJsonEncoded) {
        rowData = JSON.parse(decodeURIComponent(rowJsonEncoded));
    }

    currentSchema.forEach(col => {
        const value = rowData[col.ColumnName] || '';
        let inputHtml = '';

        if(col.IsPrimaryKey) {
            // Hidden or readonly PK
            inputHtml = `<input type="text" class="form-control" id="f_${col.ColumnName}" name="${col.ColumnName}" value="${value}" readonly>`;
        }
        else if (col.IsForeignKey) {
            // Custom Dropdown
            let selectedText = '-- Seleccione --';
            let optionsHtml = `<li><a class="dropdown-item custom-select-item ${!value ? 'active' : ''}" href="#" data-value="">-- Seleccione --</a></li>`;
            
            const fkOptions = fkData[col.ColumnName] || [];
            fkOptions.forEach(opt => {
                const optVal = opt[col.ReferencedColumn] || opt.Id || opt.id;
                const optText = opt.Descripcion || opt.Nombre || opt.Clave || optVal;
                const isActive = (value == optVal);
                if (isActive) selectedText = optText;
                
                optionsHtml += `<li><a class="dropdown-item custom-select-item ${isActive ? 'active' : ''}" href="#" data-value="${optVal}">${optText}</a></li>`;
            });
            
            inputHtml = `
            <div class="dropdown w-100">
                <button class="form-control d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="background-color: #fff; border: 1px solid #dee2e6; color: #32325d; height: auto; padding: 0.5rem 0.75rem;">
                    <span class="selected-text text-start text-truncate" style="flex: 1;">${selectedText}</span>
                    <i class="fa-solid fa-chevron-down ms-2 text-muted" style="font-size: 0.8rem;"></i>
                </button>
                <ul class="dropdown-menu w-100 custom-select-menu shadow" style="max-height: 250px; overflow-y: auto; z-index: 1050;">
                    ${optionsHtml}
                </ul>
                <input type="hidden" id="f_${col.ColumnName}" name="${col.ColumnName}" value="${value}">
            </div>`;
        }
        else {
            // Normal text input
            inputHtml = `<input type="text" class="form-control" id="f_${col.ColumnName}" name="${col.ColumnName}" value="${value}">`;
        }

        const hiddenClass = (col.IsPrimaryKey && !isEdit) ? 'd-none' : '';

        formFields.append(`
            <div class="col-md-12 mb-3 ${hiddenClass}">
                <label class="form-label fw-bold">${col.ColumnName}</label>
                ${inputHtml}
            </div>
        `);
    });

    $('#modalForm').modal('show');
}

async function guardarRegistro() {
    const data = {};
    let hasError = false;

    currentSchema.forEach(col => {
        const val = $(`#f_${col.ColumnName}`).val();
        
        if (!col.IsPrimaryKey && (val === undefined || val === '')) {
            hasError = true;
        }

        data[col.ColumnName] = val;
    });

    if (hasError) {
        Swal.fire('Advertencia', 'Por favor completa todos los campos', 'warning');
        return;
    }

    try {
        const response = await fetch(`/api/catalogos/${currentTable}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            Swal.fire('Éxito', 'Registro guardado correctamente', 'success');
            $('#modalForm').modal('hide');
            loadCatalogData(currentTable);
        } else {
            const err = await response.text();
            Swal.fire('Error', err, 'error');
        }
    } catch(e) {
        console.error(e);
        Swal.fire('Error', 'Error de conexión al guardar.', 'error');
    }
}
